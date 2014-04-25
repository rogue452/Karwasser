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

using MySql.Data.MySqlClient;
using Microsoft.Win32;
using System.IO;
using System.Net.Mail;

namespace project
{
   
    /// Interaction logic for ManagerGui.xaml
    
    public partial class ManagerGui : Window
    {
        public ManagerGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            NameLabel.Content = "                                    שלום "+ Login.user_name +"!\n               אנא בחר/י מה ברצונך/ה לעשות.";
            CPUName_label.Content = Login.my_host_name;
            Check_Jobs_Time();
        }




        private void Users_Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerUsersGui MUG = new ManagerUsersGui();
            MUG.Show();
            this.Close();      
        }




        private void Employees_Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerEMPGui MEG = new ManagerEMPGui();
            MEG.Show();
            this.Close();
        }

        private void Custumer_Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerCusGui MCG = new ManagerCusGui();
            MCG.Show();
            this.Close();

        }

        private void job_btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerJobGui MJG = new ManagerJobGui();
            MJG.Show();
            this.Close();

        }

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

                            //create the sche.sql with the sche string.
                            File.WriteAllText(schepath , sche);
                            MSQLcrcommand1.Connection = MySqlConn;
                            MySqlConn.Open();
                            MessageBox.Show("פעולה זו עלולה להמשך כמה דקות, הודעה תופיע בסוף התהליך");

                            // create the sql dump file to the tempPlace path.
                            mb.ExportToFile(tempPlace);
                            MySqlConn.Close();

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



        private void Check_Jobs_Time()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                string Query1 = ("SELECT jobs.jobid FROM project.jobs ,project.users WHERE (DATEDIFF('" + today + "',jobs.expectedFinishDate)<=3) AND users.userid='" + Login.user_id + "' AND (DATEDIFF(users.last_email_sent_date, '" + today + "')<0) GROUP BY jobid");
                Console.WriteLine(Query1);
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                
                int count=0;
                string jobids = "";
                string jobid = "";
                while (dr.Read())   // run on all the jobs
                    {
                    count++;
                    jobid = dr.GetString(0);
                    jobids = jobids +"מספר עבודה - "+ jobid + ".\n";   // setting up the email body.
                }
                MySqlConn.Close();
                Console.WriteLine(" כמות העבודות שתאריך הסיום שלהן מתקרב (יירשם 0 גם במידה ונשלח כבר אי מייל היום) "+count);
                if (count>0)    // if there are jobs.
                {
                    //string body = ("תאריך הסיום של העבודות הבאות הוא בעוד עד 3 ימים מהיום:");

                    string body = ("תאריך הסיום של העבודות הבאות הוא בעוד פחות מארבע ימים  מהיום - ") + DateTime.Now.ToString("dd/MM/yyyy") + " :\n" + jobids;
                    string subject = "הודעה אוטומטית ממערכת Karwasser 2003";
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
                        Console.WriteLine("נשלח האימייל");
                        try
                        {

                            //MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query2 = "update users set last_email_sent_date='" + today + "' WHERE userid='" + Login.user_id + "'";
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






    }
}