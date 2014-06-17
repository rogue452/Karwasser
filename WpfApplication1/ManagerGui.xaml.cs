// ***********************************************************************
// Assembly         : WpfApplication1
// Author           : user
// Created          : 06-10-2014
//
// Last Modified By : user
// Last Modified On : 06-10-2014
// ***********************************************************************
// <copyright file="ManagerGui.xaml.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
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
using System.ComponentModel;
using MySql.Data.MySqlClient;
using Microsoft.Win32;
using System.IO;
using System.Net.Mail;

namespace project
{

    /// <summary>
    /// Class ManagerGui.
    /// </summary>
    /// Interaction logic for ManagerGui.xaml
    
    public partial class ManagerGui : Window
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerGui" /> class.
        /// </summary>
        public ManagerGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            NameLabel.Content = "                    שלום " + Login.first_name1 + " " + Login.last_name1 + "!\n               אנא בחר/י מה ברצונך/ה לעשות.";
            CPUName_label.Content = Login.my_host_name;
            Email_label.Visibility = Visibility.Hidden;
            email_sent_label.Visibility = Visibility.Hidden;
            Check_Jobs_Time();
            
        }




        /// <summary>
        /// Handles the Click event of the Users_Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Users_Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerUsersGui MUG = new ManagerUsersGui();
            MUG.Show();
           // this.Hide();
            Login.close = 1;
            this.Close();      
        }




        /// <summary>
        /// Handles the Click event of the Employees_Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Employees_Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerEMPGui MEG = new ManagerEMPGui();
            MEG.Show();
            Login.close = 1;
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the Custumer_Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Custumer_Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerCusGui MCG = new ManagerCusGui();
            MCG.Show();
            Login.close = 1;
            this.Close();

        }

        /// <summary>
        /// Handles the Click event of the job_btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void job_btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerJobGui MJG = new ManagerJobGui();
            MJG.Show();
            Login.close = 1;
            this.Close();

        }

        /// <summary>
        /// Handles the Click event of the MySQL_Backup_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void MySQL_Backup_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlCommand MSQLcrcommand1 = new MySqlCommand();

                using (MySqlBackup mb = new MySqlBackup(MSQLcrcommand1))
                    {
                        SaveFileDialog dialog = new SaveFileDialog();
                        dialog.FileName = "MySQL" + " Backup - from date " + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " and time " + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString(); ; // Default file name
                        dialog.DefaultExt = ".sql"; // Default file extension
                        dialog.Filter = "MySQL Backup File (.SQL)|*.SQL";  // |Text documents (.txt)|*.txt| Filter files by extension 

                        // Show save file dialog box
                        Nullable<bool> result = dialog.ShowDialog();

                        // Process save file dialog box results 
                        if (result == true)
                        {
                            string saveto = dialog.FileName;

                            //the missing schema string.
                            string sche = "--\n-- Create schema project by Shuki Porat\n-- \n\nCREATE DATABASE IF NOT EXISTS project; \nUSE project;\n\n";
                            // the schepath is the path for the sche string and the tempPlace  is the path for the temp sql dump file.
                            string schepath = "C://Users/Public//sche.sql";
                            string tempPlace = "C://Users/Public//temp.sql";

                            try
                            {
                                //create the sche.sql with the sche string.
                                File.WriteAllText(schepath, sche);
                            }
                            catch // if this is Windows XP
                            {
                                schepath = "C:/Documents and Settings/All Users/sche.sql";
                                tempPlace = "C:/Documents and Settings/All Users/temp.sql";
                                //create the sche.sql with the sche string.
                                File.WriteAllText(schepath, sche);
                            }
                            try
                            {
                                MSQLcrcommand1.Connection = MySqlConn;
                                MySqlConn.Open();
                                MessageBox.Show("פעולה זו עלולה להמשך כמה דקות, הודעה תופיע בסוף התהליך");

                                // create the sql dump file to the tempPlace path.
                                mb.ExportToFile(tempPlace);
                                MySqlConn.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                            // Append the sql dump to the string file.
                            File.AppendAllText(schepath, File.ReadAllText(tempPlace));

                            // copy the file to the user selected place.
                            File.WriteAllText(saveto, File.ReadAllText(schepath));

                            // Delete a file by using File class static method... 
                            if (System.IO.File.Exists(@tempPlace))
                            {
                                // Use a try block to catch IOExceptions, to 
                                // handle the case of the file already being 
                                // opened by another process. 
                                try
                                {
                                    System.IO.File.Delete(@tempPlace);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }

                            if (System.IO.File.Exists(schepath))
                            {
                                // Use a try block to catch IOExceptions, to 
                                // handle the case of the file already being 
                                // opened by another process. 
                                try
                                {
                                    System.IO.File.Delete(schepath);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            MessageBox.Show("נשמר בהצלחה למיקום שצויין SQL -קובץ גיבוי ה");
                        }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }



        /// <summary>
        /// Check_s the jobs_ time.
        /// </summary>
        private void Check_Jobs_Time()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                string Query1 = ("SELECT jobs.jobid ,jobs.expectedFinishDate FROM project.jobs ,project.users WHERE (DATEDIFF(jobs.expectedFinishDate,'" + today + "')<4) AND (DATEDIFF(jobs.expectedFinishDate,'" + today + "')>=0) AND users.empid='" + Login.empid + "' AND (DATEDIFF(users.last_email_sent_date, '" + today + "')<0) AND jobs.job_status!='הסתיימה' AND jobs.job_status!='בוטלה' GROUP BY jobid");
                Console.WriteLine(Query1);
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                
                int count=0;
                DateTime date_to=new DateTime();
                string jobids = "";
                string jobid = "", date_to_send="";
                while (dr.Read())   // run on all the jobs
                    {
                    count++;
                    jobid = dr.GetString(0);
                    date_to = dr.GetDateTime(1);
                    date_to_send = date_to.ToString("dd/MM/yyyy");
                    jobids = jobids + "מספר עבודה - " + jobid + " עד תאריך: " + date_to_send + ".\n";   // setting up the email body.
                }
                MySqlConn.Close();
                Console.WriteLine(" כמות העבודות שתאריך הסיום שלהן מתקרב (יירשם 0 גם במידה ונשלח כבר אי מייל היום) "+count);
                if (count>0)    // if there are jobs.
                {
                    //string body = ("תאריך הסיום של העבודות הבאות הוא בעוד עד 3 ימים מהיום:");

                    string body = ("תאריך הסיום של העבודות הבאות הוא בעוד פחות מארבע ימים  מהיום - ") + DateTime.Now.ToString("dd/MM/yyyy") + " :\n" + jobids;
                    string subject = "הודעה אוטומטית ממערכת קרוסר";
                    string toAddress = Login.useremail;
                    string senderID = "karwasser2003@gmail.com";// sender's email id
                    const string senderPassword = "SP271984"; // sender password
                    Console.WriteLine(body);
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
                        email_sent_label.Visibility = Visibility.Visible;
                        Console.WriteLine("נשלח האימייל");
                        try
                        {

                            //MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query2 = "update users set last_email_sent_date='" + today + "' WHERE empid='" + Login.empid + "'";
                            MySqlCommand MSQLcrcommand2 = new MySqlCommand(Query2, MySqlConn);
                            MSQLcrcommand2.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand2);
                            MySqlConn.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    catch (Exception)
                    {
                        Email_label.Visibility = Visibility.Visible;
                        Console.WriteLine("(אולי אין חיבור לאינטרנט) !!!.בעיה בשליחת אימייל");
                        //MessageBox.Show("!!!.בעיה בשליחת אימייל");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        /// <summary>
        /// Handles the clicked event of the exit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
        private void exit_clicked(object sender, CancelEventArgs e)
        {
            Console.WriteLine("" + Login.close);

            if (Login.close == 0) // then the user want to exit.
            {
                if (MessageBox.Show("?האם אתה בטוח שברצונך לצאת מהמערכת ", "וידוא יציאה", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    e.Cancel = true; ; //don't exit.
                }
                else // if the user clicked on "Yes" so he wants to Update.
                {
                    // logoff user
                    try
                    {
                        string empid1 = Login.empid;
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "update users set connected='לא מחובר' where empid='" + empid1 + "' ";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    MessageBox.Show("               נותקת בהצלחה מהמערכת\n          תודה שהשתמשת במערכת קרוסר\n                          !להתראות" , "!הצלחה" , MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
            else
            {

            }
            Login.close = 0;
        }




        /// <summary>
        /// Handles the Click event of the exit_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("" + Login.close + " כפתור התנתקות");

            if (Login.close == 0) // then the user want to exit.
            {
                if (MessageBox.Show("?האם אתה בטוח שברצונך לצאת מהמערכת ", "וידוא יציאה", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return; //don't exit.
                }
                else // if the user clicked on "Yes" so he wants to Update.
                {
                    // logoff user
                    try
                    {
                        string empid1 = Login.empid;
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "update users set connected='לא מחובר' where empid='" + empid1 + "' ";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    MessageBox.Show("               נותקת בהצלחה מהמערכת\n          תודה שהשתמשת במערכת קרוסר\n                          !להתראות", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                    Login LI = new Login();
                    LI.Show();
                    Login.close = 1;
                    this.Close();
                }
            }
            else
            {

            }
            Login.close = 0;
        }

        /// <summary>
        /// Handles the Click event of the stat_btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void stat_btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerStat MSG = new ManagerStat();
            MSG.Show();
            Login.close = 1;
            this.Close();
        }




    }
}
