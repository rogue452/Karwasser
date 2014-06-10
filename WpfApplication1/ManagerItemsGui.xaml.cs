// ***********************************************************************
// Assembly         : WpfApplication1
// Author           : user
// Created          : 06-10-2014
//
// Last Modified By : user
// Last Modified On : 06-10-2014
// ***********************************************************************
// <copyright file="ManagerItemsGui.xaml.cs" company="">
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
    /// Interaction logic for ManagerItemsGui.xaml
    /// </summary>
    public partial class ManagerItemsGui : Window
    {
        /// <summary>
        /// The dt
        /// </summary>
        public static DataTable dt = new DataTable("items");
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerItemsGui"/> class.
        /// </summary>
        public ManagerItemsGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("SELECT itemid as `מקט פריט`, itemName as `שם פריט`,item_discription as `תיאור פריט` FROM item GROUP BY itemid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt = new DataTable("jobs");
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
        /// Handles the Click event of the ADD_new_item_pattern control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ADD_new_item_pattern_Click(object sender, RoutedEventArgs e)
        {
            ManagerAddNewItemPatternGUI MANIG = new ManagerAddNewItemPatternGUI();
            MANIG.ShowDialog();
        }



        /// <summary>
        /// Handles the Click event of the ExcelBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ExcelBtn_Click(object sender, RoutedEventArgs e)
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
                dialog.FileName = "רשימת פריטים כללית נכון לתאריך -  " + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString(); ; // Default file name
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
                string Query1 = ("SELECT itemid as `מקט פריט`, itemName as `שם פריט`,item_discription as `תיאור פריט` FROM item  WHERE itemid Like '%" + searchkey + "%' GROUP BY itemid ");
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
        /// Handles the TextChanged event of the itemNameSearchTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void itemNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchkey = this.itemNameSearchTextBox.Text;
                string Query1 = ("SELECT itemid as `מקט פריט`, itemName as `שם פריט`,item_discription as `תיאור פריט` FROM item  WHERE itemName Like '%" + searchkey + "%' GROUP BY itemid  ");
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
        /// Handles the Click event of the DeleteBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                if (MessageBox.Show("?האם אתה בטוח שברצונך למחוק פריט זה", "וידוא מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else // if the user clicked on "Yes" so he wants to Delete.
                {
                    string selected = row["מקט פריט"].ToString();
                    int times = 1;
                    // we need to check if the item is in use.
                     try
                    {
                        Console.WriteLine("שורה 178");
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT COUNT(itemid) FROM project.jobs WHERE itemid='" + selected + "' "); //GROUP BY jobid LIMIT 1    // AND job_status !='" + today + "' ORDER BY startDate DESC LIMIT 1 "); //to see if the orderid already in the system.
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        times = Convert.ToInt32(MSQLcrcommand1.ExecuteScalar());
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        Console.WriteLine("שורה 186");
                        MySqlConn.Close();
                    }
                     catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return; ;
                    }

                     if (times == 0) // if the item is not in any job the delete.
                     {
                         try
                         {
                             MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                             MySqlConn.Open();
                             string Query1 = "delete from item where itemid='" + selected + "' ";
                             MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                             MSQLcrcommand1.ExecuteNonQuery();
                             MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                             MySqlConn.Close();
                             MessageBox.Show("!תבנית הפריט נמחקה מהמערכת", "!הצלחה",MessageBoxButton.OK,MessageBoxImage.Information);
                         }
                         catch (Exception ex)
                         {
                             MessageBox.Show(ex.Message);
                         }

                         try
                         {
                             MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                             MySqlConn.Open();
                             string Query1 = ("SELECT itemid as `מקט פריט`, itemName as `שם פריט`,item_discription as `תיאור פריט` FROM item GROUP BY itemid");
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
                         MessageBox.Show("לא ניתן למחוק תבנית פריט זה\n   .בגלל שנעשה בו שימוש בעבודה אחת לפחות" , "!שים לב",MessageBoxButton.OK,MessageBoxImage.Error);
                         return;
                     }

                }//end else

            }//end try
            catch 
            {
                MessageBox.Show("לא נבחרה תבנית פריט למחיקה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }//end function


        /// <summary>
        /// Handles the Click event of the UpdateBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

                if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן פריט זה", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    //dont do stuff
                }

                else // if the user clicked on "Yes" so he wants to Update.
                {
                    string selected_Item = row["מקט פריט"].ToString();
                    string itemname = row["שם פריט"].ToString();
                    string itemdesc = row["תיאור פריט"].ToString();

                    try
                    {

                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "UPDATE item SET itemName='" + itemname + "',item_discription='" + itemdesc + "' WHERE itemid='" + selected_Item + "' ";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();
                        MessageBox.Show("תבנית הפריט עודכנה", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT itemid as `מקט פריט`, itemName as `שם פריט`,item_discription as `תיאור פריט` FROM item GROUP BY itemid");
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

            }
            catch
            {
                MessageBox.Show("לא נבחרה תבנית פריט לעדכון", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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
        /// Handles the Click event of the item_stages_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void item_stages_button_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected_Item = row["מקט פריט"].ToString();
                ManagerGeneralItemStagesGui MGIG = new ManagerGeneralItemStagesGui(selected_Item);
                MGIG.Owner = this;
                MGIG.ShowDialog();
            }
            catch 
            {
                MessageBox.Show("לא נבחר פריט", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
              return;
            }
        }

        /// <summary>
        /// Handles the AutoGeneratingColumn event of the Grid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridAutoGeneratingColumnEventArgs"/> instance containing the event data.</param>
        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מקט פריט")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
        }

        /// <summary>
        /// Handles the Click event of the Back_Btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            {
                ManagerJobGui MJG = new ManagerJobGui();
                MJG.Show();
                Login.close = 1;
                this.Close();
            }
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
