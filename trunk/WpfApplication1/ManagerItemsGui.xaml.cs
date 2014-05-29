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
        public static DataTable dt = new DataTable("items");
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

        private void ADD_new_item_pattern_Click(object sender, RoutedEventArgs e)
        {
            ManagerAddNewItemPatternGUI MANIG = new ManagerAddNewItemPatternGUI();
            MANIG.ShowDialog();
        }



        private void ExcelBtn_Click(object sender, RoutedEventArgs e)
        {

            ExportToExcel();
        }





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
                        MessageBox.Show(" נוצר בהצלחה Microsoft Excel -מסמך ה");
                    }
                    else
                    {
                        MessageBox.Show(" לא נוצר  Microsoft Excel -התרחשה שגיאה ולכן מסמך ה");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }


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
                             MessageBox.Show("!תבנית הפריט נמחקה מהמערכת", "הצלחה",MessageBoxButton.OK,MessageBoxImage.Information);
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
                     else
                     {
                         MessageBox.Show("לא ניתן למחוק תבנית פריט זה\n   .בגלל שנעשה בו שימוש בעבודות" , "!שים לב",MessageBoxButton.OK,MessageBoxImage.Error);
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


        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {/*
            try
            {

                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

                if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן קבוצת פריט זו", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    //dont do stuff
                }

                else // if the user clicked on "Yes" so he wants to Update.
                {
                    string selected_Item = row["מקט פריט"].ToString();
                    string itemdesc = row["תיאור לגבי קבוצת הפריטים"].ToString();
                    string exqun = row["כמות נדרשת מהפריט"].ToString();
                    try
                    {
                        int expectedq = Convert.ToInt32(exqun);
                    }
                    catch
                    {
                        MessageBox.Show("שדה הכמות נדרשת לא מכיל רק מספרים");
                        return;
                    }

                    try
                    {

                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();
                        MessageBox.Show("! קבוצת הפריט עודכנה");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT jobs.itemid as `מקט פריט`,item.itemName as `שם פריט`,group_costomer_itemid as `מקט לקוח`, expectedItemQuantity as `כמות נדרשת מהפריט`, COUNT(jobs.itemid) as `כמות בפועל מהפריט` ,itemsDescription as `תיאור לגבי קבוצת הפריטים`, group_Status as `סטטוס קבוצת הפריט` , group_StageOrder as `מספר השלב הנוכחי של קבוצת הפריט` ,stageName as `שם השלב הנוכחי של קבוצת הפריט`, stage_discription as `תאור השלב הנוכחי של קבוצת הפריט`  FROM jobs,item  WHERE jobs.jobid='" + jobID + "' AND jobs.itemid=item.itemid AND jobs.itemStageOrder=item.itemStageOrder AND jobs.itemStatus=item.itemStatus group by jobs.itemid ");
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
            catch { MessageBox.Show("לא נבחרה קבוצת פריט לעדכון "); }
       */ }


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
              MessageBox.Show("לא נבחר פריט");
              return;
            }
        }

        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מקט פריט")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
        }

        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            {
                ManagerJobGui MJG = new ManagerJobGui();
                MJG.Show();
                Login.close = 1;
                this.Close();
            }
        }




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
