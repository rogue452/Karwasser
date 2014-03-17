﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;


namespace project
{
    /// <summary>
    /// Interaction logic for Forgot_pass.xaml

    /// Class Forgot_pass.
    /// </summary>
    public partial class Forgot_pass : Window
    {
        /// <summary>
        /// The userpass
        /// </summary>
        public static string userpass;

        /// <summary>
        /// Initializes a new instance of the Forgot_pass class.

        /// Initializes a new instance of the <see cref="Forgot_pass"/> class.
        /// </summary>
        public Forgot_pass()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("!לפני בדיקת מייל", "b4 if", MessageBoxButton.OK);
            if ((Regex.IsMatch(this.to_txt.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")))
            {
                MessageBox.Show("!לאחר בדיקת מייל", "b4 try", MessageBoxButton.OK);
                MessageBox.Show(this.to_txt.Text,"the mail");
                try
                {
                    MessageBox.Show("!ניסיון התחברות למסד", "נתונים", MessageBoxButton.OK);
                    string Connectionstring = " Server=localhost;Database=project; UId=root;Password=1234;";
                    MySqlConnection objc = new MySqlConnection(Connectionstring);
                    objc.Open();
                    MessageBox.Show("!לאחר התחברות למסד", "נתונים", MessageBoxButton.OK);
                    string Query = "select * from users where email='" + this.to_txt.Text + "'";
                    MySqlCommand crcommand = new MySqlCommand(Query, objc);
                    MessageBox.Show("!לפני ביצוע שאילתה ", "נתונים", MessageBoxButton.OK);
                    crcommand.ExecuteNonQuery();
                    MessageBox.Show("!לאחר ביצוע שאילתה ", "נתונים", MessageBoxButton.OK);
                    MySqlDataReader dr = crcommand.ExecuteReader();
                    MessageBox.Show("הכנסת הסיסמה (במידה וקיימת) למשתנה");
                    int count = 0;
                    while (dr.Read())
                    {
                        count++;
                        userpass = dr.GetString(4);
                      
                    }
                    MessageBox.Show(this.to_txt.Text, " "+userpass);
                    MessageBox.Show("בדיקה האם המייל קיים במערכת");
                    if (count == 1)
                    {
                     // this func will send an EMail in the form of (all must be Strings): (Email Address to send,EMail Title,Email Body)   /Shuki Porat  
                        SendEmail(this.to_txt.Text, "שיחזור סיסמא למערכת קרוסאר", userpass);

                        MessageBox.Show("!הסיסמא נשלחה לכתובת המייל", "!הפעולה הושלמה", MessageBoxButton.OK);
                        Login window = new Login();
                        window.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("!המייל שהוזן אינו קיים במאגר! אנא פנה למנהל המערכת", "!הפעולה נכשלה", MessageBoxButton.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("!כתובת המייל לא הוזנה כהלכה! אנא נסה שנית", "!הפעולה נכשלה", MessageBoxButton.OK);
        }

        /// <summary>
        /// Handles the Click event of the button3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        /// 
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Login window = new Login();
            window.Show();
            this.Close();
        }



        // this func will send an EMail in the form of (all must be Strings): (Email Address to send,EMail Title,Email Body)   /Shuki Porat  
        protected string SendEmail(string toAddress, string subject, string body)
        {
            string result = "Message Sent Successfully..!!";
            string senderID = "karwasser2003@gmail.com";// sender's email id
            const string senderPassword = "SP271984"; // sender password
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", // smtp server address
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };
                MailMessage message = new MailMessage(senderID, toAddress, subject, body);
                smtp.Send(message);
            }
            catch (Exception )
            {
                result = "Error sending email.!!!";
            }
            return result;
        }

        /// <summary>
        /// Handles the Click event of the button2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            to_txt.Clear();
        }
    }
}