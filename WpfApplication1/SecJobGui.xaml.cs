// ***********************************************************************
// Assembly         : WpfApplication1
// Author           : user
// Created          : 06-10-2014
//
// Last Modified By : user
// Last Modified On : 06-10-2014
// ***********************************************************************
// <copyright file="SecJobGui.xaml.cs" company="">
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
using MySql.Data.MySqlClient;
using System.Data;
using System.ComponentModel;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace project
{
    /// <summary>
    /// Interaction logic for SecJobGui.xaml
    /// </summary>
    public partial class SecJobGui : Window
    {
        /// <summary>
        /// The dt
        /// </summary>
        DataTable dt = new DataTable("jobs");
        /// <summary>
        /// The mistake
        /// </summary>
        bool mistake = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="SecJobGui"/> class.
        /// </summary>
        public SecJobGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            refreashandclear();
        }




        /// <summary>
        /// Handles the Click event of the TXTBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TXTBtn_Click(object sender, RoutedEventArgs e)
        {

            ExportToExcel();
        }





        /// <summary>
        /// Exports to excel.
        /// </summary>
        private void ExportToExcel()
        {
            try
            {
               
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "רשימת עבודות" + " נכון לתאריך - " + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString(); ; // Default file name
                dialog.DefaultExt = ".xlsx"; // Default file extension
                dialog.Filter = "Microsoft Excel 2003 and above Documents (.xlsx)|*.xlsx";  // |Text documents (.txt)|*.txt| Filter files by extension 

                // Show save file dialog box
                Nullable<bool> result = dialog.ShowDialog();

                // Process save file dialog box results 
                if (result == true)
                {
                    string saveto = dialog.FileName;
                    bool success = CreateExcelFile.CreateExcelDocument(dt, saveto);
                    if (success)
                    {
                        MessageBox.Show(" נוצר בהצלחה Microsoft Excel -מסמך ה", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else { 
                            MessageBox.Show(" לא נוצר  Microsoft Excel -התרחשה שגיאה ולכן מסמך ה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); 
                         }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Refreashandclears this instance.
        /// </summary>
        private void refreashandclear()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("SELECT jobid as `מספר עבודה`, reg_date as `תאריך רישום`,orderid as`מספר הזמנה` ,jobdescription  as `תאור עבודה` ,jobs.costumerid as `חפ לקוח` ,costumers.costumerName as `שם לקוח` ,contact_id as `מספר איש קשר` , costumers.contactName as `שם איש קשר` ,job_status as `סטטוס עבודה`,startDate  as `תאריך התחלה`,expectedFinishDate as `תאריך סיום משוער` ,actualFinishDate as `תאריך סיום בפועל` ,deliveryid  as `תעודת משלוח` ,invoiceNumber  as `מספר חשבונית`  FROM project.jobs,project.costumers WHERE jobs.costumerid=costumers.costumerid AND jobs.contact_id=costumers.contactid GROUP BY jobid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                dt.Clear();
                mysqlDAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
                Name_Search_TextBox.Clear();
                JobIDSearchTextBox.Clear();
                Start_datePicker.SelectedDate = null;
                End_datePicker.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Handles the TextChanged event of the JobIDSearchTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void JobIDSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchkey = this.JobIDSearchTextBox.Text;
                string Query1 = ("SELECT jobid as `מספר עבודה`, reg_date as `תאריך רישום`,orderid as`מספר הזמנה` ,jobdescription  as `תאור עבודה` ,jobs.costumerid as `חפ לקוח` ,costumers.costumerName as `שם לקוח` ,contact_id as `מספר איש קשר` , costumers.contactName as `שם איש קשר` ,job_status as `סטטוס עבודה`,startDate  as `תאריך התחלה`,expectedFinishDate as `תאריך סיום משוער` ,actualFinishDate as `תאריך סיום בפועל` ,deliveryid  as `תעודת משלוח` ,invoiceNumber  as `מספר חשבונית`  from project.jobs, project.costumers  where  jobid Like '" + searchkey + "%' AND jobs.costumerid=costumers.costumerid AND jobs.contact_id=costumers.contactid GROUP BY jobid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                dt.Clear();
                mysqlDAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the Name_Search_TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void Name_Search_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // if both of the datepickers are with dates
            if (!string.IsNullOrWhiteSpace(Start_datePicker.Text) || !string.IsNullOrWhiteSpace(End_datePicker.Text))
            {
                Fillterdates();
            }
            if (mistake == false)
            {
                string searchidkey = this.Name_Search_TextBox.Text;
                dt.DefaultView.RowFilter = string.Format("`שם לקוח` LIKE '%{0}%'", searchidkey);
            }

        }

        /// <summary>
        /// Handles the Click event of the ADD_Btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ADD_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerAddNewJobGUI MACG = new ManagerAddNewJobGUI();
            MACG.Show();
            Login.close = 1;
            this.Close();
        }



        



        // go to previous screen.
        /// <summary>
        /// Handles the Click event of the Back_Btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            SecretaryGui SG = new SecretaryGui();
            SG.Show();
            Login.close = 1;
            this.Close();
        }



        /// <summary>
        /// Handles the AutoGeneratingColumn event of the Grid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridAutoGeneratingColumnEventArgs"/> instance containing the event data.</param>
        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //if the Column is a date Column then show me only the date
            if (e.Column.Header.ToString() == "תאריך התחלה")
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";

            }

            if (e.Column.Header.ToString() == "תאריך רישום")
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
            }

            if (e.Column.Header.ToString() == "תאריך סיום משוער" )
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
            }

            if (e.Column.Header.ToString() == "תאריך סיום בפועל")
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
            }

            e.Column.IsReadOnly = true; // Makes the column as read only 
        }



        //This function will set the 2 DatePickers to null and will reaload to the default datagrid.
        /// <summary>
        /// Handles the Click event of the Refresh_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Refresh_button_Click(object sender, RoutedEventArgs e)
        {
            Start_datePicker.SelectedDate = null;
            End_datePicker.SelectedDate = null;
            refreashandclear();
        }



        //This function will filter the startDate by date selected from 2 DatePickers.
        // if the dates were not right a message will be shown. 
        /// <summary>
        /// Handles the Click event of the Filter_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Filter_button_Click(object sender, RoutedEventArgs e)
        {
            Fillterdates();
            if (!string.IsNullOrWhiteSpace(Name_Search_TextBox.Text))
            {
                string searchidkey = this.Name_Search_TextBox.Text;
                dt.DefaultView.RowFilter = string.Format("`שם לקוח` LIKE '%{0}%'", searchidkey);
            }
            
        }


        /// <summary>
        /// Fillterdateses this instance.
        /// </summary>
        private void Fillterdates()
        {
            mistake =false;
            //MessageBox.Show("" + Start_datePicker.Text + "");
            if (Start_datePicker.Text != "" && End_datePicker.Text != "")
            {
                String start, end;
                DateTime s = (DateTime)Convert.ToDateTime(Start_datePicker.Text);
                DateTime f = (DateTime)Convert.ToDateTime(End_datePicker.Text);
                TimeSpan ts = f - s;

                if (ts.Days >= 0)
                {
                    start = Convert.ToDateTime(Start_datePicker.Text).ToString("yyyy-MM-dd");
                    end = Convert.ToDateTime(End_datePicker.Text).ToString("yyyy-MM-dd");

                    string radio = "startDate";
                    if (ExpectedFinishDate_radioButton.IsChecked == true)
                    {
                        radio = "expectedFinishDate";
                    }
                    if (ActualFinishDate_radioButton.IsChecked == true)
                    {
                        radio = "actualFinishDate";
                    }
                    //MessageBox.Show("" + radio + "");
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT jobid as `מספר עבודה`, reg_date as `תאריך רישום`,orderid as`מספר הזמנה` ,jobdescription  as `תאור עבודה` ,jobs.costumerid as `חפ לקוח` ,costumers.costumerName as `שם לקוח` ,contact_id as `מספר איש קשר` , costumers.contactName as `שם איש קשר` ,job_status as `סטטוס עבודה`,startDate  as `תאריך התחלה`,expectedFinishDate as `תאריך סיום משוער` ,actualFinishDate as `תאריך סיום בפועל` ,deliveryid  as `תעודת משלוח` ,invoiceNumber  as `מספר חשבונית`  FROM jobs,costumers WHERE " + radio + " BETWEEN '" + start + "' AND '" + end + "' AND jobs.costumerid=costumers.costumerid AND jobs.contact_id=costumers.contactid GROUP BY jobid");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        dt.Clear();
                        mysqlDAdp.Fill(dt);
                        dataGrid1.ItemsSource = dt.DefaultView;
                        mysqlDAdp.Update(dt);
                        MySqlConn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("אסור שתאריך הסוף הנבחר יהיה לפני תאריך ההתחלה הנבחר", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                    mistake = true;
                    return;
                }
            }
            else
            {
                if (Start_datePicker.Text == "" && End_datePicker.Text != "")
                { 
                    MessageBox.Show("לא נבחר תאריך התחלה לסינון", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                    mistake = true;
                    return;
                }
                if (Start_datePicker.Text != "" && End_datePicker.Text == "")
                {
                    MessageBox.Show("לא נבחר תאריך סוף לסינון", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                    mistake = true;
                    return;
                }
                if (Start_datePicker.Text == "" && End_datePicker.Text == "")
                {
                    MessageBox.Show("לא נבחרו תאריכי התחלה וסוף לסינון ", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                    mistake = true;
                    return;
                }
            }
        }



        /// <summary>
        /// Handles the Click event of the ViewJobIInfo_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ViewJobIInfo_button_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {                
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["מספר עבודה"].ToString();
                SecJobInfoGui SJIG = new SecJobInfoGui(selected);
                SJIG.Show();
                Login.close = 1;
                this.Close();
            }//end try
            catch { MessageBox.Show("לא נבחרה עבודה לצפיה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); }
        }



        /// <summary>
        /// Handles the clicked event of the exit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
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
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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






    }
}
