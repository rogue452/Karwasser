// ***********************************************************************
// Assembly         : WpfApplication1
// Author           : user
// Created          : 06-10-2014
//
// Last Modified By : user
// Last Modified On : 06-10-2014
// ***********************************************************************
// <copyright file="SecItemInfoGui.xaml.cs" company="">
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
    /// Interaction logic for SecItemInfoGui.xaml
    /// </summary>
    public partial class SecItemInfoGui : Window
    {
        /// <summary>
        /// The dt
        /// </summary>
        DataTable dt = new DataTable("iteminfo");
        /// <summary>
        /// The item identifier
        /// </summary>
        string itemID, jobID;
        /// <summary>
        /// Initializes a new instance of the <see cref="SecItemInfoGui"/> class.
        /// </summary>
        /// <param name="itemID1">The item i d1.</param>
        /// <param name="jobID1">The job i d1.</param>
        public SecItemInfoGui(string itemID1, string jobID1)
        {
            this.itemID = itemID1;
            this.jobID = jobID1;
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            label1.Content = jobID;
            label4.Content = itemID;
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
                string j1, j2;
                string[] jID = jobID.Split(new char[] { '/' });
                j1 = jID[0];
                j2 = jID[1];
                dialog.FileName = " רשימת פריטים לעבודה מספר " + j1 + "-" + j2 + " ומקט " + itemID + " נכון לתאריך - " + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString(); ; // Default file name
                //dialog.FileName = " רשימת פריטים לעבודה מספר " + j1 + "-" + j2 + " נכון לתאריך - " + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString(); ; // Default file name
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
                    else
                    {
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
                string Query1 = ("SELECT jobs.itemNum as `מספר פריט בסט`, inTheGroup as `ייחשב בקבוצה`, jobs.itemDescription as `תיאור פריט`,jobs.itemStatus  as `סטטוס הפריט`, jobs.itemStageOrder as `מספר השלב הנוכחי`,stageName  as `שם השלב הנוכחי` ,item.stage_discription as `תאור השלב הנוכחי`,itemToFixStageOrder as `מספר השלב שבו זוהה כתקול (אם זוהה)`  FROM jobs,item WHERE jobs.itemid=item.itemid and jobs.itemStageOrder=item.itemStageOrder and jobs.itemStatus=item.itemStatus and jobs.jobid='" + jobID + "' and jobs.itemid='" + itemID + "' ");
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
            ItemIDSearch_TextBox.Clear();
            StageNameSearchTextBox.Clear();
        }

        /// <summary>
        /// Handles the TextChanged event of the ItemIDSearch_TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void ItemIDSearch_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchkey = this.ItemIDSearch_TextBox.Text;
                string Query1 = ("SELECT jobs.itemNum as `מספר פריט בסט`, inTheGroup as `ייחשב בקבוצה`, jobs.itemDescription as `תיאור פריט`,jobs.itemStatus  as `סטטוס הפריט`, jobs.itemStageOrder as `מספר השלב הנוכחי`,stageName  as `שם השלב הנוכחי` ,item.stage_discription as `תאור השלב הנוכחי`,itemToFixStageOrder as `מספר השלב שבו זוהה כתקול (אם זוהה)`  FROM jobs,item WHERE jobs.itemid=item.itemid and jobs.itemStageOrder=item.itemStageOrder and jobs.itemStatus=item.itemStatus and jobs.jobid='" + jobID + "' and jobs.itemid='" + itemID + "' and jobs.itemNum Like '%" + searchkey + "%'");
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
            StageNameSearchTextBox.Clear();

        }

        /// <summary>
        /// Handles the TextChanged event of the StageNameSearchTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void StageNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchNamekey = this.StageNameSearchTextBox.Text;
                string Query1 = ("SELECT jobs.itemNum as `מספר פריט בסט`, inTheGroup as `ייחשב בקבוצה`, jobs.itemDescription as `תיאור פריט`,jobs.itemStatus  as `סטטוס הפריט`, jobs.itemStageOrder as `מספר השלב הנוכחי`,stageName  as `שם השלב הנוכחי` ,item.stage_discription as `תאור השלב הנוכחי`,itemToFixStageOrder as `מספר השלב שבו זוהה כתקול (אם זוהה)`   FROM jobs,item WHERE jobs.itemid=item.itemid and jobs.itemStageOrder=item.itemStageOrder and jobs.itemStatus=item.itemStatus and jobs.jobid='" + jobID + "'and jobs.itemid='" + itemID + "' and item.stageName Like '%" + searchNamekey + "%'");
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
            ItemIDSearch_TextBox.Clear();
        }





        // go to previous screen.
        /// <summary>
        /// Handles the Click event of the Back_Btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            SecJobInfoGui SJIG = new SecJobInfoGui(jobID);
            SJIG.Show();
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
            e.Column.IsReadOnly = true; // Makes the column as read only   
        }



        /// <summary>
        /// Handles the Click event of the Item_Stages_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Item_Stages_button_Click(object sender, RoutedEventArgs e)
        {
                ManagerGeneralItemStagesGui MGIG = new ManagerGeneralItemStagesGui(itemID);
                MGIG.Owner = this;
                MGIG.ShowDialog();
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
