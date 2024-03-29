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
using System.Data;
using System.ComponentModel;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Collections;

namespace project
{
    /// <summary>
    /// Interaction logic for SecAddNewJobGUI.xaml
    /// </summary>
    public partial class SecAddNewJobGUI : Window
    {
        
        /// <summary>
        /// The dt
        /// </summary>
        DataTable dt = new DataTable("custumers");
        /// <summary>
        /// The DT1
        /// </summary>
        DataTable dt1 = new DataTable("items");
        /// <summary>
        /// The DT2
        /// </summary>
        DataTable dt2 = new DataTable("Contacts");
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerAddNewJobGUI"/> class.
        /// </summary>
        public SecAddNewJobGUI()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
          //  Create_DataTable_Columns_Start();
       

            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select costumerid as `חפ לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
               // DataTable dt = new DataTable("custumers");
               // Create_DataTable_Columns_Start();
                mysqlDAdp.Fill(dt);
                //MessageBox.Show("" + dt.Rows.Count + "");
               // Create_DataTable_Columns_Start();
              //  Make_CheckBox_Columns_False();
                dataGrid1.ItemsSource = dt.DefaultView;
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
  
             try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select itemid as `מקט פריט`,itemName as `שם פריט`, item_discription as `תאור פריט` from project.item WHERE itemStatus='בעבודה' group by itemid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt1 = new DataTable("items");
                Create_DataTable1_Columns_Start();
                mysqlDAdp.Fill(dt1);
                //Create_DataTable1_Columns_End();
           //   Make_CheckBox1_Columns_False();
                dataGrid2.ItemsSource = dt1.DefaultView;
                mysqlDAdp.Update(dt1);

/*
                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.Header = "כמות";
                textColumn.Binding = new Binding("כמות");
                //dataGrid2.Columns.Add(textColumn);
                dataGrid2.Columns.Insert(0, textColumn);

*/

                MySqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }




        /// <summary>
        /// Handles the Click event of the ExcelBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ExcelBtn_Click(object sender, RoutedEventArgs e)
        {
           // IList dg2rows = dataGrid2.SelectedItems;
            //DataRowView row = (DataRowView)dataGrid2.SelectedItems[0];

         //   ListView list = new ListView();

         //   list =(DataRowView)dataGrid2.SelectedItems[0];

            DataTable changedRecordsItemsTable = dt1.GetChanges();
            DataTable changedRecordsTableCust = dt.GetChanges();
            int sizeofItemsnewtable, sizeofCustnewtable, custcheck=0;
            try
            {
                sizeofCustnewtable = changedRecordsTableCust.Rows.Count; // will give an exp if the size of the new cust table is zero.
                try
                {
                   
                    foreach (DataRow drc in changedRecordsTableCust.Rows)
                        {
                            string c = drc["חפ לקוח"].ToString();

                            Console.WriteLine("מספר הלקוח לפני האיף לא ריק - " + c + "");
                                if (c != "") // in case the user deleted a cell in the cust tableand now it have a string of-  "".
                                { 
                                  custcheck++; // if it will be more then one then the user chosen more then one cust.
                                  if (custcheck<2)
                                      {
                                        Console.WriteLine("מספר הלקוח הנבחר הוא - " + drc["חפ לקוח"].ToString() + "");
                                      }// if (custcheck<2)
                                    else 
                                      { 
                                        MessageBox.Show(".אנא בחר רק לקוח אחד\n הטבלאות יאופסו כעת");
                                        Reload_Cust_Table();
                                        Reload_Items_Table();
                                        return; 
                                      }
                                }// if (c != "") 
                       
                        }// foreach (DataRow drc in changedRecordsTableCust.Rows)
                    if (custcheck == 0) { MessageBox.Show("לא נבחר לקוח"); return; }

                    sizeofItemsnewtable = changedRecordsItemsTable.Rows.Count; // will give an exp if the size of the new items table is zero.
                    foreach (DataRow dri in changedRecordsItemsTable.Rows)
                           {
                              string q = dri["כמות"].ToString();

                             try
                               {
                                 if (q != "") // in case the user deleted a cell in the item and now it have a string of-  "" .
                                   {
                                      int check = Convert.ToInt32(q);

                                      if (check > 0)
                                        {
                                                            //  foreach (DataColumn col in changedRecordsTable.Columns)
                                                            // {
                                                            //  Console.WriteLine(q);
                                                
                                          Console.WriteLine("הכמות היא  - " + check + "ומספר הפריט הוא -  "+dri["מקט פריט"].ToString()+"");
                                                            // Console.WriteLine(dr[col.ColumnName]);
                                                            //  }

                                                            /* foreach (DataRow dr in changedRecordsTable.Rows)
                                                             {
                                                                 foreach (DataColumn col in changedRecordsTable.Columns)
                                                                 {
                                                                     Console.WriteLine(dr[col.ColumnName]);
                                                                 }
                                                             }
                                                             */
                                        }//end if (check > 0)
                                        else { MessageBox.Show("שדה הכמות מכיל כמות שלילית או 0 בפריט מספר - " + dri["מקט פריט"].ToString() + ""); return; }

                                   }// if (q != "")

                               }// end try
                               catch
                               { MessageBox.Show("שדה הכמות לא כולל רק מספרים בפריט מספר - " + dri["מקט פריט"].ToString() + ""); return; }

                           }// end foreach (DataRow dri in changedRecordsTable.Rows)
  
                }
                catch { MessageBox.Show("לא נבחרו פריטים"); return; }
                

            }
            catch { MessageBox.Show("לא נבחר לקוח"); Console.WriteLine("לא נבחר לקוח"); return; }


        //    MessageBox.Show("" + dataGrid1.CurrentCell + "");
        //    Console.WriteLine("" + dataGrid1.SelectedCells.ToString() + "");
           
/*
            foreach (DataGridRow dr in dt.Rows)
            {
                foreach (DataGridColumn col in dataGrid1.Columns)
                {
                    Console.WriteLine(dr[col[1]);
                }
            }
 */

           // dt1.Columns[1]
         //   list = dataGrid2.ColumnFromDisplayIndex(0);
        //    DataGridBoundColumn = dataGrid2.ColumnFromDisplayIndex(0);

         //   MessageBox.Show("" + dataGrid2.CheckedItems + "");
         //   MessageBox.Show("" + dataGrid2.ColumnFromDisplayIndex(4) + "");
       //     MessageBox.Show("" + dg2rows[0].ToString() + "");
       //     MessageBox.Show("" + row["בחר/י פריט/ים"].GetType().ToString() + "");
        //    MessageBox.Show("" + row["בחר פריט/ים"].ToString() + "");
         //   MessageBox.Show("" + row["כמות"].GetType().ToString() + "");
         //   MessageBox.Show("" + row["כמות"].ToString() + "");

            //MessageBox.Show("" + row.DataView + "");
          //  MessageBox.Show("" + dg2rows.Count.ToString() + "");


            //ExportToExcel();
        }

        /// <summary>
        /// Reload_s the cust_ table.
        /// </summary>
        private void Reload_Cust_Table()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchidkey = this.IDSearchTextBox.Text;
                string Query1 = "select costumerid as `חפ לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid";
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
        /// Reload_s the items_ table.
        /// </summary>
        private void Reload_Items_Table()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select itemid as `מקט פריט`,itemName as `שם פריט`, discription as `תאור פריט` from project.item group by itemid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                dt1.Clear();
                mysqlDAdp.Fill(dt1);
                dataGrid2.ItemsSource = dt1.DefaultView;
                mysqlDAdp.Update(dt1);
                MySqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        /// <summary>
        /// Create_s the data table_ columns_ start.
        /// </summary>
        private void Create_DataTable_Columns_Start()
        {

            dt.Columns.Add(new DataColumn("בחר לקוח", typeof(string)));

          /*  DataColumn i = new DataColumn();
            i.ColumnName = "בחר/י פריט/ים"; // adding the / to the name will make it 2 states checkbox, took me hours to find it out......
            i.DataType = typeof(string);
            i.ReadOnly = false;
            dt1.Columns.Add(i);
           */ 

  //          DataColumn i = new DataColumn();
  //          i.ColumnName = "בחר/י פריט/ים"; // adding the / to the name will make it 2 states checkbox, took me hours to find it out......
  //          i.DataType = typeof(bool);
  //          i.ReadOnly = false;
  //          dt1.Columns.Add(i);
          
/*
            DataColumn i = new DataColumn();
            i.ColumnName = "בחר/י פריט/ים"; // adding the / to the name will make it 2 states checkbox, took me hours to find it out......
            i.DataType = typeof(bool);
            dt1.Columns.Add(i);
*/
   //         DataColumn c = new DataColumn();
   //         c.ColumnName = "בחר/י לקוח";
   //         c.DataType = typeof(bool);
           // c.ReadOnly = false;
   //         dt.Columns.Add(c);
           // dt1.Columns.Add(new DataColumn("בחר פריט/ים", typeof(bool))); //this will show checkboxes
           // dt.Columns.Add(new DataColumn("בחר לקוח", typeof(bool))); //this will show checkboxes
        }


        /// <summary>
        /// Create_s the data table1_ columns_ start.
        /// </summary>
        private void Create_DataTable1_Columns_Start()
        {
            // DataColumn q = new DataColumn();
            // q.ColumnName = "כמות";
            // q.DataType = typeof(string);
            // dt1.Columns.Add(q);
            dt1.Columns.Add(new DataColumn("כמות", typeof(string)));
            dt1.Columns.Add(new DataColumn("מקט לקוח", typeof(string)));
        }




        /// <summary>
        /// Make_s the check box_ columns_ false.
        /// </summary>
        private void Make_CheckBox_Columns_False()
        {
            /////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////
            //this function will put false in each row of the CheckBox column.

            int sizeoftable = dt.Rows.Count;
            for (int i = 1; i <= sizeoftable; i++)
            {
                DataRow row = dt.Rows[sizeoftable-1]; //copy the last row.
                dt.Rows[sizeoftable - 1].Delete();     // delete the current last row.
                row["בחר/י לקוח"] = false;          //put false in the CheckBox column.
                dt.Rows.Add(row);                    // add the modified row as the first row.
            }
            ///////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////

        }

        /// <summary>
        /// Make_s the check box1_ columns_ false.
        /// </summary>
        private void Make_CheckBox1_Columns_False()
        {
            /////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////
            //this function will put false in each row of the CheckBox column.

            int sizeoftable = dt1.Rows.Count;
            for (int i = 1; i <= sizeoftable; i++)
            {
                DataRow row = dt1.Rows[sizeoftable-1]; //copy the last row.
                row[0] = false;                     //put false in the CheckBox column.
                dt1.Rows[sizeoftable-1].Delete();     // delete the current last row.
                dt1.Rows.Add(row);                  // add the modified row as the first row.
            }
            ///////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////

        }



        /// <summary>
        /// Exports to excel.
        /// </summary>
        private void ExportToExcel()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select costumerid as `חפ לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("custumers");
                mysqlDAdp.Fill(dt);
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "רשימת לקוחות" + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString(); ; // Default file name
                dialog.DefaultExt = ".xlsx"; // Default file extension
                dialog.Filter = "Microsoft Excel 2003 and above Documents (.xlsx)|*.xlsx";  // |Text documents (.txt)|*.txt| Filter files by extension 

                // Show save file dialog box
                Nullable<bool> result = dialog.ShowDialog();

                // Process save file dialog box results 
                if (result == true)
                {
                    string saveto = dialog.FileName;
                    CreateExcelFile.CreateExcelDocument(dt, saveto);
                    MessageBox.Show(" נוצר בהצלחה Microsoft Excel -מסמך ה", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        /// <summary>
        /// Handles the TextChanged event of the CustumerNameSearchTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void CustumerNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchkey = this.CustumerNameSearchTextBox.Text;
                CheckSingleQuotationMark CSQ = new CheckSingleQuotationMark();
                searchkey = CSQ.checkForSingleQuotationMark(searchkey);
                string Query1 = "select costumerid as `חפ לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers  where  costumerName Like '%" + searchkey + "%' group by costumerid";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
           //     DataTable dt = new DataTable("custumers");
          //      DataColumn d = dt.Columns["בחר/י לקוח"];
                dt.Clear();
         //       dt.Columns.Remove("בחר/י לקוח");
          //      dt.Columns.Add(d);
               // dt.Columns["בחר לקוח"] = d;

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
        /// Handles the TextChanged event of the IDSearchTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void IDSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchidkey = this.IDSearchTextBox.Text;
                CheckSingleQuotationMark CSQ = new CheckSingleQuotationMark();
                searchidkey = CSQ.checkForSingleQuotationMark(searchidkey);
                string Query1 = "select costumerid as `חפ לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers  where  costumerid Like '%" + searchidkey + "%' group by costumerid";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
           //     DataTable dt = new DataTable("Custumers");
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
        /// Handles the TextChanged event of the Item_Search_textBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void Item_Search_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckSingleQuotationMark CSQ = new CheckSingleQuotationMark();
            String searchkey = CSQ.checkForSingleQuotationMark(this.Item_Search_textBox.Text);
            //String searchkey = this.Item_Search_textBox.Text;
            dt1.DefaultView.RowFilter = string.Format("`מקט פריט` LIKE '%{0}%'", searchkey);

        }



        /// <summary>
        /// Handles the Click event of the ADD_Btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ADD_Btn_Click(object sender, RoutedEventArgs e)
        {
            string jobid, orderid, jobdes;
            // if dates were intered.
            if (startdatePicker.Text != "" && finishdatePicker.Text != "")
            {
                
                orderid = orderID_textBox.Text;
                if (!string.IsNullOrWhiteSpace(orderID_textBox.Text))
                {
                    string today = DateTime.Now.ToString("yyyy-MM-dd");
                    try
                    {
                        Console.WriteLine("שורה 467");
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT jobid FROM project.jobs WHERE orderid='" + orderid + "' AND startDate<='" + today + "' ORDER BY startDate DESC LIMIT 1 "); //to see if the orderid already in the system.
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                       // int times = Convert.ToInt32(MSQLcrcommand1.ExecuteScalar());
                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        Console.WriteLine("שורה 476");
                        while (dr.Read())
                          {
                              if (!dr.IsDBNull(0))
                              {
                                  Console.WriteLine("שורה 480");
                                  if (MessageBox.Show("מספר הזמנה זהה כבר קיים האחרון ביותר היה עבור מספר עבודה "+dr.GetString(0)+"\n?האם ברצונך להוסיף בכל זאת", "מספר הזמנה קיים", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                                  {
                                      MySqlConn.Close();
                                      return; //if user answerd NO
                                  }
                              }
                          }

                        MySqlConn.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return; ;
                    }


                    // get the latest jobid this year.
                    string fullyear = DateTime.Now.ToString("yyyy");
                    string twodigyear = DateTime.Now.ToString("yy");
                    int jobid1=0;
                    string[] arry;
                    string tosplit,first;
                    try
                    {
                        Console.WriteLine("שורה 508");
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT MAX(jobid) FROM project.jobs WHERE startDate BETWEEN '" + fullyear + "-01-01' AND '" + fullyear + "-12-31' ");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                        Console.WriteLine("שורה 515");
                        while (dr.Read())
                        {
                            if (dr.IsDBNull(0))
                            {
                                Console.WriteLine("שורה 520");
                                jobid1 = 0;
                            }
                            else
                            {
                                Console.WriteLine("שורה 525");
                                tosplit = dr.GetString(0);
                                Console.WriteLine(tosplit);
                                arry = tosplit.Split('/');
                                first = arry[0].ToString();
                                Console.WriteLine(first);
                                jobid1 = Convert.ToInt32(first);
                                Console.WriteLine(first);
                            }
                        } 
                        Console.WriteLine("שורה 535");
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();
                        jobid1++;
                        jobid = jobid1.ToString() + "/" + twodigyear;
                        Console.WriteLine("שורה 540");
                        Console.WriteLine(jobid);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return; ;
                    }


                    jobdes = jobdes_textbox.Text;
                    String start, end;
                    DateTime s = (DateTime)Convert.ToDateTime(startdatePicker.Text);
                    DateTime f = (DateTime)Convert.ToDateTime(finishdatePicker.Text);
                    TimeSpan ts = f - s;

                    // if the days are ok.
                    if (ts.Days >= 0)
                    {
                        start = Convert.ToDateTime(startdatePicker.Text).ToString("yyyy-MM-dd");
                        end = Convert.ToDateTime(finishdatePicker.Text).ToString("yyyy-MM-dd");
                    }
                    else { MessageBox.Show(".תאריך ההתחלה שנבחר הוא לאחר תאריך הסיום", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                }
                else { MessageBox.Show("לא הוכנסה מספר הזמנה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); return; }
            }// the if (startdatePicker.Text != "" && finishdatePicker.Text != "") closer.
            else { MessageBox.Show(".לא נבחרו 2 התאריכים", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); return; }

            DataTable changedRecordsItemsTable = dt1.GetChanges();
            //DataTable changedRecordsTableCust = dt.GetChanges();
            int sizeofItemsnewtable;
            //sizeofCustnewtable, custcheck=0;
            //string customerid1;
            try
            {
                // sizeofCustnewtable = changedRecordsTableCust.Rows.Count; // will give an exp if the size of the new cust table is zero.
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

                try
                {
                    DataRowView row1 = (DataRowView)dataGrid3.SelectedItems[0];


                    try
                    {
                        string contactid = row1["מספר איש קשר"].ToString();
                        string selected = row["חפ לקוח"].ToString();


                        sizeofItemsnewtable = changedRecordsItemsTable.Rows.Count; // will give an exp if the size of the new items table is zero.
                        int itemNum = 0;
                        string itemStatus = "רישום", itemStageOrder = "1", job_status = "נרשמה";
                        
                        jobdes = jobdes_textbox.Text;
                        String start = Convert.ToDateTime(startdatePicker.Text).ToString("yyyy-MM-dd"), end = Convert.ToDateTime(finishdatePicker.Text).ToString("yyyy-MM-dd");
                        // customerid1 = customerid1_label.Content.ToString();

                        int count = 0; // will count the number of rows with cell "" in the  changedRecordsItemsTable.

                        // input checks for כמות.
                        foreach (DataRow testrow in changedRecordsItemsTable.Rows)// for every row in the updateds table.
                        {
                            string q = testrow["כמות"].ToString();
                            Console.WriteLine("q = " + q);
                            try
                            {

                                if (q != "") // in case the user deleted a cell in the item and now it have a string of-  "" .
                                {
                                    int new_item_quantity = Convert.ToInt32(q);
                                    if (new_item_quantity > 0)
                                    {
                                        Console.WriteLine("הקלט היה טוב במקט מספר =" + testrow["מקט פריט"].ToString());
                                    }//end if (item_quantity > 0)
                                    else
                                    {
                                        MessageBox.Show("שדה הכמות מכיל כמות שלילית או 0 במקט פריט - " + testrow["מקט פריט"].ToString() + "", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }

                                } // if (q != "")
                                else { count++; }


                            }// end try
                            catch
                            {
                                MessageBox.Show("שדה הכמות לא כולל רק מספרים במקט פריט - " + testrow["מקט פריט"].ToString() + "", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                        } // end of foreach to test the input
                        if (count == changedRecordsItemsTable.Rows.Count)
                        {
                            MessageBox.Show("  לא נבחרו פריטים מהטבלה ", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        /*
                        foreach (DataRow quantity_row in changedRecordsItemsTable.Rows)
                        {
                            try
                            {
                                if(quantity_row["כמות"].ToString()!="")
                                {
                                int item_quantity = Convert.ToInt32(quantity_row["כמות"].ToString());
                                }
                            }// end try
                            catch
                            { MessageBox.Show(" אחד שדה הכמות לא כולל רק מספרים בפריט מספר - " + quantity_row["מקט פריט"].ToString() + ""); return; }
                        }
                        */

                        count = 0;
                        foreach (DataRow dri in changedRecordsItemsTable.Rows)
                        {
                            itemNum = 0;
                            string q = dri["כמות"].ToString();
                            string cosItemID = dri["מקט לקוח"].ToString();
                            if (cosItemID=="")
                             {
                                 cosItemID = "לא הוזן מקט לקוח";
                             }
                            try
                            {
                              
                                if (q != "") // in case the user deleted a cell in the item and now it have a string of-  "" .
                                {
                                    int item_quantity = Convert.ToInt32(q);

                                    if (item_quantity > 0)
                                    {
                                        for (int i = 1; i <= item_quantity; i++)
                                        {
                                            Console.WriteLine("לפני שאילתא");
                                            itemNum++;
                                            string itemid = dri["מקט פריט"].ToString();
                                            try
                                            {
                                                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                                MySqlConn.Open();
                                                string Query1 = ("INSERT INTO project.jobs (jobid, itemid,itemNum, expectedItemQuantity,costumerid, jobdescription, startDate, expectedFinishDate, contact_id,orderid,group_costomer_itemid ,reg_date) VALUES ('" + jobid + "','" + itemid + "','" + itemNum + "','" + item_quantity + "','" + selected + "','" + jobdes + "','" + start + "','" + end + "','" + contactid + "','" + orderid + "','" + cosItemID + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')");
                                                Console.WriteLine("השאילתא הנשלחת  - " + Query1 + "");
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


                                            Console.WriteLine("הלקוח הוא  - " + selected + "");
                                            Console.WriteLine("הכמות היא  - " + item_quantity + " ומספר הפריט הוא -  " + dri["מקט פריט"].ToString() + "");
                                            Console.WriteLine("מספר עבודה - " + jobid + "");
                                            Console.WriteLine("מספר הזמנה - " + orderid + "");
                                            Console.WriteLine("מספר איש קשר - " + contactid + "");
                                        }
                                    }//end if (item_quantity > 0)
                                    else { MessageBox.Show("שדה הכמות מכיל כמות שלילית או 0 בפריט מספר - " + dri["מקט פריט"].ToString() + "", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); return; }

                                } // if (q != "")
                                else { count++; }
                               

                            }// end try
                            catch
                            { MessageBox.Show("שדה הכמות לא כולל רק מספרים בפריט מספר - " + dri["מקט פריט"].ToString() + "", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); return; }

                        }// end foreach (DataRow dri in changedRecordsTable.Rows)
                        if (count == changedRecordsItemsTable.Rows.Count)
                            {
                                MessageBox.Show("  לא נבחרו פריטים מהטבלה ", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); return; 
                            }

                        else
                        {
                            MessageBox.Show("!העבודה נוספה למערכת", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                            SecJobGui SJG = new SecJobGui();
                            SJG.Show();
                            Login.close = 1;
                            this.Close();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("לא נבחרו פריטים", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        //Reload_Items_Table();

                        return;
                    }
                }
                catch { MessageBox.Show("לא נבחר איש קשר", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); Console.WriteLine("לא נבחר איש קשר"); return; }

            }
            catch { MessageBox.Show("לא נבחר לקוח", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); Console.WriteLine("לא נבחר לקוח"); return; }

        }



        // go to previous screen.
        /// <summary>
        /// Handles the Click event of the Back_Btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            SecJobGui SJG = new SecJobGui();
            SJG.Show();
            Login.close = 1;
            this.Close();
            
        }
/*
        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Collections.IList rows = dataGrid1.SelectedItems;

                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["חפ לקוח"].ToString();
                // MessageBox.Show(""+selected+ "");



                if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן לקוח זה", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    //dont do stuff
                }
                else // if the user clicked on "Yes" so he wants to Update.
                {
                        string custumername = row["שם לקוח"].ToString();
                        string custumeraddress = row["כתובת לקוח"].ToString();
                      
                        try
                        {

                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = "update costumers set costumerName='" + custumername + "',costumerAddress='" + custumeraddress + "' where costumerid='" + selected + "'";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();
                            MessageBox.Show("!פרטי הלקוח עודכנו");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = ("select costumerid as `חפ לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            DataTable dt = new DataTable("custumers");
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
            catch { MessageBox.Show("לא נבחר לקוח לעדכון "); }
        }

*/

        /// <summary>
        /// Handles the AutoGeneratingColumn event of the Grid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridAutoGeneratingColumnEventArgs"/> instance containing the event data.</param>
        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "חפ לקוח" || e.Column.Header.ToString() == "שם לקוח" || e.Column.Header.ToString() == "כתובת לקוח")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
             //   if (e.Column.Header.ToString() == "תאור פריט")
           // if (e.Column.Header.ToString() == "בחר לקוח")
           // {
                /*
                   DataGridTextColumn textColumn = new DataGridTextColumn();
                 * string colname = e.Column.Header.ToString();
                   textColumn.Header = "colname";
                   textColumn.SortMemberPath = "colname";
                   textColumn.Binding = new Binding("colname");
                  // dataGrid2.Columns.Add(textColumn);
                   Binding tempmb2 = new Binding("colname");
                   tempmb2.Mode = BindingMode.TwoWay;
                   dataGrid2.Columns.Add(textColumn);
                  


                DataGridTemplateColumn dgct = new DataGridTemplateColumn();
                string colname = e.Column.Header.ToString();
                dgct.Header = colname;
                dgct.SortMemberPath = colname;
                //    dgct.DisplayIndex = 4;
                Binding b = new Binding(colname);
                b.Mode = BindingMode.TwoWay;

                #region Editing
                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBox));
                factory.SetValue(TextBox.TextProperty, b);
                DataTemplate cellEditingTemplate = new DataTemplate();
                cellEditingTemplate.VisualTree = factory;
                dgct.CellEditingTemplate = cellEditingTemplate;
                #endregion

                #region View
                FrameworkElementFactory sfactory = new FrameworkElementFactory(typeof(TextBox));
                sfactory.SetValue(TextBox.TextProperty, b);
                DataTemplate cellTemplate = new DataTemplate();
                cellTemplate.VisualTree = sfactory;
                dgct.CellTemplate = cellTemplate;
                #endregion
           */
         //   }

            /*
            if (e.Column.Header.ToString() == "בחר לקוח")
            {
               DataGridCheckBoxColumn dg1Coscolumn = new DataGridCheckBoxColumn();
               dg1Coscolumn.Header = "בחר לקוח";
               dg1Coscolumn.IsThreeState = false;
               e.Column = dg1Coscolumn;
            }
             * /
            /*
            if (e.Column.Header.ToString() == "חפ לקוח")  
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
               // DataGridCheckBoxColumn dg1chbcolumn = new DataGridCheckBoxColumn();
              //  dg1chbcolumn.Header="בחר לקוח";
              //  dataGrid1.Columns.Add(dg1chbcolumn);

                DataGridTemplateColumn col1 = new DataGridTemplateColumn();
                col1.Header = "בחר לקוח";
                FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(CheckBox));
                Binding b1 = new Binding("נבחר");
                b1.Mode = BindingMode.TwoWay;
                factory1.SetValue(CheckBox.IsCheckedProperty, b1);
                factory1.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(chkSelect_Checked));
                Binding b2 = new Binding("בוטל");
                b1.Mode = BindingMode.TwoWay;
                factory1.SetValue(CheckBox.IsCheckedProperty, b1);
                factory1.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(chkUNSelect_Checked));
                DataTemplate cellTemplate1 = new DataTemplate();
                cellTemplate1.VisualTree = factory1;
                col1.CellTemplate = cellTemplate1;
                dataGrid1.Columns.Add(col1);
            }
             */

        }

        /// <summary>
        /// Handles the AutoGeneratingColumn event of the dataGrid3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridAutoGeneratingColumnEventArgs"/> instance containing the event data.</param>
        private void dataGrid3_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מספר איש קשר" || e.Column.Header.ToString() == "שם איש קשר" || e.Column.Header.ToString() == "מחלקת איש קשר")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
        }



        /// <summary>
        /// Handles the Checked event of the chkSelect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
       private void chkSelect_Checked(object sender, RoutedEventArgs e)
        {
           // MessageBox.Show("נבחרה שורה");
          //  System.Collections.IList rows = dataGrid1.SelectedItems;
          //  rows.Add(e);
       //     DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

     //       foreach (DataRowView row in dataGrid1.Items) //traversing the each row of gridview
      //      {
     //           row["בחר לקוח"].ToString();
      //          DataRowView row1 = (DataRowView)dataGrid1.SelectedItems[0];
      //          CheckBox checkbox = (CheckBox)row.FindControl("chkRows");
      //          if (checkbox.Checked == true) //Checking the checkbox is checked or not!!
      //          { }
       //     }

            //row["בחר לקוח"].ToString();
            //MessageBox.Show("" + row["בחר לקוח"].ToString() + "");
           // CheckBox Check = this.dataGrid1.Columns[0].GetCellContent(dataGrid1.SelectedItem) as CheckBox;
          //  MessageBox.Show("" + dataGrid1.CurrentCell.GetType() + "");
       //    MessageBox.Show("" + dataGrid1.CurrentCell.Column + "");

           /*
            DataRowView _data = dataGrid1.CurrentCell.Item as DataRowView;
            if (_data != null)
            {
                MessageBox.Show(_data.Row[0].ToString());
            }
            if (dataGrid1.CurrentCell.Item is CheckBox)
            {
                MessageBox.Show("CheckBox");
            }
            */ 

         //  MessageBox.Show("" + dataGrid1.CurrentCell + "");
          // dataGrid1.CurrentCell.Column
           //dataGrid1.SelectedCells
    //           MessageBox.Show("" + dataGrid1.SelectedCells + "");
         //  CheckBox Check1 = (CheckBox)dataGrid1.CurrentCell.Item;
               
         //   Check1.IsChecked = true;
           // (dataGrid1.GetCell(i, 0).Presenter.Content as CheckBox).IsChecked = false;
          //  Check.IsChecked = true;
         //   if (Check1.IsChecked == true)

    //        List<Object> temp = new List<Object>();
    //        temp.Add(false);sds
           int sizeoftable = dt1.Rows.Count;
            MessageBox.Show("" + sizeoftable.ToString() + "");
            foreach (DataRow dr in dt1.Rows)
            {
                foreach (DataColumn col in dt1.Columns)
                {
                    Console.WriteLine(dr[col.ColumnName]);
                }
            }


   //         DataRowView dg2row = (DataRowView)dataGrid2.SelectedItems[0];
   //                      string itemid = dg2row["מקט פריט"].ToString();
   //                      string customerid = dg2row["כמות"].ToString();
   //                      MessageBox.Show("" + itemid + "");
                         
          //  list.Add(customerid);
        //    {
             //   MessageBox.Show("סומן");
         //   }

           // MessageBox.Show("" + dataGrid1.CurrentCell.ToString()+ "");
        }




       /// <summary>
       /// Handles the Checked event of the chkUNSelect control.
       /// </summary>
       /// <param name="sender">The source of the event.</param>
       /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
       private void chkUNSelect_Checked(object sender, RoutedEventArgs e)
        {
            //CheckBox Check1 = (CheckBox)dataGrid1.CurrentCell.Item;
           // Check1.IsChecked = false;
            MessageBox.Show(" בוטלה בחירה");

        }




       /// <summary>
       /// Handles the AutoGeneratingColumn1 event of the Grid control.
       /// </summary>
       /// <param name="sender">The source of the event.</param>
       /// <param name="e">The <see cref="DataGridAutoGeneratingColumnEventArgs"/> instance containing the event data.</param>
        private void Grid_AutoGeneratingColumn1(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מקט פריט" || e.Column.Header.ToString() == "שם פריט" || e.Column.Header.ToString() == "תאור פריט")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }

           
           /* if (e.Column.Header.ToString() == "בחר/י פריט/ים")
            {
                // e.Cancel = true;   // For not to include 
             //   e.Column.IsReadOnly = true; // Makes the column as read only
                DataGridTemplateColumn col1 = new DataGridTemplateColumn();
                col1.Header = "בחר/י פריט/ים";
                FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(CheckBox));
                Binding b1 = new Binding("נבחר");
                b1.Mode = BindingMode.TwoWay;
                factory1.SetValue(CheckBox.IsCheckedProperty, b1);
                factory1.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(chkSelect_Checked));
                Binding b2 = new Binding("בוטל");
                b1.Mode = BindingMode.TwoWay;
                factory1.SetValue(CheckBox.IsCheckedProperty, b1);
                factory1.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(chkUNSelect_Checked));
                DataTemplate cellTemplate1 = new DataTemplate();
                cellTemplate1.VisualTree = factory1;
                col1.CellTemplate = cellTemplate1;
                col1.IsReadOnly = false;
                e.Column = col1;

            */

           /*     DataGridCheckBoxColumn dg2chbcolumn = new DataGridCheckBoxColumn();
                dg2chbcolumn.Header = "בחר פריט/ים";
                dg2chbcolumn.IsThreeState = false;
             //   dataGrid2.Columns.Add(dg2chbcolumn);
                e.Column = dg2chbcolumn;
            */
          //  }

          
            

        /*    if (e.Column.Header.ToString() == "תאור פריט")
         //   if (e.Column.Header.ToString() == "כמות")
            {

                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.Header = "כמות";
                textColumn.SortMemberPath = "כמות";
                textColumn.Binding = new Binding("כמות");
               // dataGrid2.Columns.Add(textColumn);
                Binding tempmb2 = new Binding("כמות");
                tempmb2.Mode = BindingMode.TwoWay;
                dataGrid2.Columns.Add(textColumn);
                */

                /*
                DataGridTemplateColumn dgct = new DataGridTemplateColumn();
                dgct.Header = "כמות";
                dgct.SortMemberPath = "כמות";
            //    dgct.DisplayIndex = 4;
                Binding b = new Binding("כמות");
                b.Mode = BindingMode.TwoWay;
                
                #region Editing
                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBox));
                factory.SetValue(TextBox.TextProperty, b);
                DataTemplate cellEditingTemplate = new DataTemplate();
                cellEditingTemplate.VisualTree = factory;
                dgct.CellEditingTemplate = cellEditingTemplate;
                #endregion

                #region View
                FrameworkElementFactory sfactory = new FrameworkElementFactory(typeof(TextBox));
                sfactory.SetValue(TextBox.TextProperty, b);
                DataTemplate cellTemplate = new DataTemplate();
                cellTemplate.VisualTree = sfactory;
                dgct.CellTemplate = cellTemplate;
                #endregion
                */
                //  dataGrid2.Columns.Add(dgct);
                
           //     NotifyPropertyChanged("dt1");





/*
                DataGridTemplateColumn dgtc = new DataGridTemplateColumn
                    {
                        Header = "כמות",
                        CanUserSort = false,
                        CanUserReorder = false,
                    };
                DataTemplate t = new DataTemplate();

                DataTemplate ced = new DataTemplate();
                FrameworkElementFactory f2 = new FrameworkElementFactory(typeof(TextBox));
                Binding tempmb2 = new Binding();

                tempmb2.Mode = BindingMode.TwoWay;

                f2.SetBinding(TextBox.TextProperty, tempmb2);
                ced.VisualTree = f2;
                dgtc.CellEditingTemplate = ced;

                dataGrid2.Columns.Add(dgtc);
*/
           // }

        }



/*
        private void Contacts_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["חפ לקוח"].ToString();
                string CosName = row["שם לקוח"].ToString();
                string cosADDs = row["כתובת לקוח"].ToString();
                string cos_insideNum = row["מספר לקוח"].ToString();
                ManagerContactsGUI MCG = new ManagerContactsGUI(selected, cos_insideNum, CosName, cosADDs);
                MCG.Show();
                Login.close = 1;
                this.Close();
            }
            catch { MessageBox.Show("לא נבחר לקוח"); }
        }
*/
        /// <summary>
        /// Handles the MouseLeftButtonDown event of the dataGrid1_Preview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void dataGrid1_Preview_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["חפ לקוח"].ToString();
                try
                {
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = ("select costumerid as `חפ לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();
                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                    dt2.Clear();
                    mysqlDAdp.Fill(dt2);
                    dataGrid3.ItemsSource = dt2.DefaultView;
                    mysqlDAdp.Update(dt2);
                    MySqlConn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }   
            }// end try

            catch { return; }
        }


        /// <summary>
        /// Handles the Focus event of the dataGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void dataGrid1_Focus(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["חפ לקוח"].ToString();
                try
                {
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = ("select costumerid as `חפ לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();
                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                    dt2.Clear();
                    mysqlDAdp.Fill(dt2);
                    dataGrid3.ItemsSource = dt2.DefaultView;
                    mysqlDAdp.Update(dt2);
                    MySqlConn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }   
            }// end try

            catch { return; }
        }

        /// <summary>
        /// Handles the Focusable event of the dataGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void dataGrid1_Focusable(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["חפ לקוח"].ToString();
                try
                {
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = ("select costumerid as `חפ לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();
                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                    dt2.Clear();
                    mysqlDAdp.Fill(dt2);
                    dataGrid3.ItemsSource = dt2.DefaultView;
                    mysqlDAdp.Update(dt2);
                    MySqlConn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }// end try

            catch { return; }

            }

        /// <summary>
        /// Handles the SelectionChanged event of the dataGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["חפ לקוח"].ToString();
                try
                {
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = ("select contactid as `מספר איש קשר`,contactName as `שם איש קשר` ,contactDepartment as `מחלקת איש קשר` from costumers  where costumerid='" + selected + "'");
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();
                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                    dt2.Clear();
                    mysqlDAdp.Fill(dt2);
                    dataGrid3.ItemsSource = dt2.DefaultView;
                    mysqlDAdp.Update(dt2);
                    MySqlConn.Close();
                    cont_label.Visibility = Visibility.Hidden;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }// end try

            catch { return; }

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
            Console.WriteLine("" + Login.close+" כפתור התנתקות");

            if (Login.close == 0) // then the user want to exit.
            {
                if (MessageBox.Show("?האם אתה בטוח שברצונך לצאת מהמערכת ", "וידוא יציאה", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                   return ; //don't exit.
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
