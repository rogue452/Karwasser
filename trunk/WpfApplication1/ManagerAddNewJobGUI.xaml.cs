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
using System.Collections;
using System.ComponentModel;

namespace project
{
    /// <summary>
    /// Interaction logic for ManagerAddNewJobGUI.xaml
    /// </summary>
    public partial class ManagerAddNewJobGUI : Window
    {
        DataTable dt = new DataTable("custumers");
        DataTable dt1 = new DataTable("items");
        DataTable dt2 = new DataTable("Contacts");
        public ManagerAddNewJobGUI()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
          //  Create_DataTable_Columns_Start();
       

            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
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
                string Query1 = ("select itemid as `מספר פריט`,itemName as `שם פריט`, item_discription as `תאור פריט` from project.item WHERE itemStatus='בעבודה' group by itemid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt1 = new DataTable("items");
                mysqlDAdp.Fill(dt1);
                Create_DataTable1_Columns_End();
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
                            string c = drc["מספר לקוח"].ToString();

                            Console.WriteLine("מספר הלקוח לפני האיף לא ריק - " + c + "");
                                if (c != "") // in case the user deleted a cell in the cust tableand now it have a string of-  "".
                                { 
                                  custcheck++; // if it will be more then one then the user chosen more then one cust.
                                  if (custcheck<2)
                                      {
                                        Console.WriteLine("מספר הלקוח הנבחר הוא - " + drc["מספר לקוח"].ToString() + "");
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
                                                
                                          Console.WriteLine("הכמות היא  - " + check + "ומספר הפריט הוא -  "+dri["מספר פריט"].ToString()+"");
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
                                        else { MessageBox.Show("שדה הכמות מכיל כמות שלילית או 0 בפריט מספר - " + dri["מספר פריט"].ToString() + ""); return; }

                                   }// if (q != "")

                               }// end try
                               catch
                               { MessageBox.Show("שדה הכמות לא כולל רק מספרים בפריט מספר - " + dri["מספר פריט"].ToString() + ""); return; }

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
            
        private void Reload_Cust_Table()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchidkey = this.IDSearchTextBox.Text;
                string Query1 = "select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid";
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
        private void Reload_Items_Table()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select itemid as `מספר פריט`,itemName as `שם פריט`, discription as `תאור פריט` from project.item group by itemid");
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


        private void Create_DataTable1_Columns_End()
        {
            // DataColumn q = new DataColumn();
            // q.ColumnName = "כמות";
            // q.DataType = typeof(string);
            // dt1.Columns.Add(q);
            dt1.Columns.Add(new DataColumn("כמות", typeof(string)));
        }




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



        private void ExportToExcel()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
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
                    MessageBox.Show(" נוצר בהצלחה Microsoft Excel -מסמך ה");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        private void CustumerNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchkey = this.CustumerNameSearchTextBox.Text;
                string Query1 = "select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers  where  costumerName Like '%" + searchkey + "%' group by costumerid";
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

        private void IDSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchidkey = this.IDSearchTextBox.Text;
                string Query1 = "select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers  where  costumerid Like '%" + searchidkey + "%' group by costumerid";
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







        private void ADD_Btn_Click(object sender, RoutedEventArgs e)
        {
            // if dates were intered.
            if (startdatePicker.Text != "" && finishdatePicker.Text != "")
            {
                string jobid, jobdes;
                jobid = jobid_textBox.Text;
                if (jobid != "")
                {
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT COUNT(jobid) FROM project.jobs WHERE jobid='" + jobid + "'"); //to see if the jobid already in the system.
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        int times = Convert.ToInt32(MSQLcrcommand1.ExecuteScalar());
                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();

                        if (times != 0)
                        {
                            MessageBox.Show("כבר קיים מספר עבודה - " + jobid + "");
                            return;
                        }
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
                    else { MessageBox.Show(".תאריך ההתחלה שנבחר הוא לאחר תאריך הסיום"); return; }
                }
                else { MessageBox.Show("לא הוכנס מספר עבודה"); return; }
            }// the if (startdatePicker.Text != "" && finishdatePicker.Text != "") closer.
            else { MessageBox.Show(".לא נבחרו 2 התאריכים"); return; }

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
                        string selected = row["מספר לקוח"].ToString();


                        sizeofItemsnewtable = changedRecordsItemsTable.Rows.Count; // will give an exp if the size of the new items table is zero.
                        int itemNum = 0;
                        string itemStatus = "בעבודה", itemStageOrder = "1", job_status = "נרשמה";
                        string jobid, jobdes, itemsdes = "לא נרשם תיאור עדיין";
                        jobid = jobid_textBox.Text;
                        jobdes = jobdes_textbox.Text;
                        String start = Convert.ToDateTime(startdatePicker.Text).ToString("yyyy-MM-dd"), end = Convert.ToDateTime(finishdatePicker.Text).ToString("yyyy-MM-dd");
                        // customerid1 = customerid1_label.Content.ToString();

                        int count = 0; // will count the number of rows with cell "" in the  changedRecordsItemsTable.
                        foreach (DataRow dri in changedRecordsItemsTable.Rows)
                        {
                            itemNum = 0;
                            string q = dri["כמות"].ToString();

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
                                            string itemid = dri["מספר פריט"].ToString();
                                            try
                                            {
                                                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                                MySqlConn.Open();
                                                string Query1 = ("INSERT INTO project.jobs (jobid, itemid,itemNum,itemStatus,itemStageOrder, expectedItemQuantity,costumerid, itemsDescription, job_status, jobdescription, startDate, expectedFinishDate, contact_id) VALUES ('" + jobid + "','" + itemid + "','" + itemNum + "','" + itemStatus + "','" + itemStageOrder + "','" + item_quantity + "','" + selected + "','" + itemsdes + "','" + job_status + "','" + jobdes + "','" + start + "','" + end + "','" + contactid + "')");
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
                                            Console.WriteLine("הכמות היא  - " + item_quantity + "ומספר הפריט הוא -  " + dri["מספר פריט"].ToString() + "");
                                            Console.WriteLine("מספר עבודה - " + jobid + "");
                                            Console.WriteLine("מספר איש קשר - " + contactid + "");
                                        }
                                    }//end if (item_quantity > 0)
                                    else { MessageBox.Show("שדה הכמות מכיל כמות שלילית או 0 בפריט מספר - " + dri["מספר פריט"].ToString() + ""); return; }

                                } // if (q != "")
                                else { count++; }
                               

                            }// end try
                            catch
                            { MessageBox.Show("שדה הכמות לא כולל רק מספרים בפריט מספר - " + dri["מספר פריט"].ToString() + ""); return; }

                        }// end foreach (DataRow dri in changedRecordsTable.Rows)
                        if (count == changedRecordsItemsTable.Rows.Count)
                            {
                                MessageBox.Show("  לא נבחרו פריטים מהטבלה "); return; 
                            }

                        else { MessageBox.Show("!העבודה נוספה למערכת"); }
                    }
                    catch
                    {
                        MessageBox.Show("לא נבחרו פריטים");
                        //Reload_Items_Table();

                        return;
                    }
                }
                catch { MessageBox.Show("לא נבחר איש קשר"); Console.WriteLine("לא נבחר איש קשר"); return; }

            }
            catch { MessageBox.Show("לא נבחר לקוח"); Console.WriteLine("לא נבחר לקוח"); return; }

        }



        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Collections.IList rows = dataGrid1.SelectedItems;
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                if (MessageBox.Show("?האם אתה בטוח שברצונך למחוק לקוח זה", "וידוא מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else // if the user clicked on "Yes" so he wants to Delete.
                {
                    // this will give us the first colum of the selected row in the DataGrid.

                    string selected = row["מספר לקוח"].ToString();
                    // MessageBox.Show("" + selected + "");

                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "delete from costumers where costumerid='" + selected + "'";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();
                        MessageBox.Show("!הלקוח נמחק מהמערכת");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
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
                }//end else

            }//end try
            catch { MessageBox.Show("לא נבחר לקוח למחיקה"); }

        }//end function



        // go to previous screen.
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerGui MG = new ManagerGui();
            MG.Show();
            Login.close = 1;
            this.Close();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Collections.IList rows = dataGrid1.SelectedItems;

                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["מספר לקוח"].ToString();
                // MessageBox.Show(""+selected+ "");



                if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן לקוח זה", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
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
                            string Query1 = ("select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
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


        
        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מספר לקוח" || e.Column.Header.ToString() == "שם לקוח" || e.Column.Header.ToString() == "כתובת לקוח")
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
            if (e.Column.Header.ToString() == "מספר לקוח")  
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

        private void dataGrid3_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מספר איש קשר" || e.Column.Header.ToString() == "שם איש קשר" || e.Column.Header.ToString() == "מחלקת איש קשר")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
        }



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
   //                      string itemid = dg2row["מספר פריט"].ToString();
   //                      string customerid = dg2row["כמות"].ToString();
   //                      MessageBox.Show("" + itemid + "");
                         
          //  list.Add(customerid);
        //    {
             //   MessageBox.Show("סומן");
         //   }

           // MessageBox.Show("" + dataGrid1.CurrentCell.ToString()+ "");
        }




       private void chkUNSelect_Checked(object sender, RoutedEventArgs e)
        {
            //CheckBox Check1 = (CheckBox)dataGrid1.CurrentCell.Item;
           // Check1.IsChecked = false;
            MessageBox.Show(" בוטלה בחירה");

        }




        private void Grid_AutoGeneratingColumn1(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מספר פריט" || e.Column.Header.ToString() == "שם פריט" || e.Column.Header.ToString() == "תאור פריט")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }


            if (e.Column.Header.ToString() == "בחר/י פריט/ים")
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



           /*     DataGridCheckBoxColumn dg2chbcolumn = new DataGridCheckBoxColumn();
                dg2chbcolumn.Header = "בחר פריט/ים";
                dg2chbcolumn.IsThreeState = false;
             //   dataGrid2.Columns.Add(dg2chbcolumn);
                e.Column = dg2chbcolumn;
            */
            }
          
            

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




        private void Contacts_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["מספר לקוח"].ToString();
                string CosName = row["שם לקוח"].ToString();
                string cosADDs = row["כתובת לקוח"].ToString();
                ManagerContactsGUI MCG = new ManagerContactsGUI(selected, CosName, cosADDs);
                MCG.Show();
                Login.close = 1;
                this.Close();
            }
            catch { MessageBox.Show("לא נבחר לקוח"); }
        }

        private void dataGrid1_Preview_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["מספר לקוח"].ToString();
                try
                {
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = ("select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
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


        private void dataGrid1_Focus(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["מספר לקוח"].ToString();
                try
                {
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = ("select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
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

        private void dataGrid1_Focusable(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["מספר לקוח"].ToString();
                try
                {
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = ("select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
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

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["מספר לקוח"].ToString();
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }// end try

            catch { return; }

        }









        private void exit_clicked(object sender, CancelEventArgs e)
        {
            Console.WriteLine("" + Login.close);

            if (Login.close == 0) // then the user want to exit.
            {
                if (MessageBox.Show("?האם אתה בטוח שברצונך לצאת מהמערכת ", "וידוא יציאה", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
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
                    MessageBox.Show("               נותקת בהצלחה מהמערכת\n          תודה שהשתמשת במערכת קרוסר\n                          !להתראות");
                }
            }
            else
            {

            }
            Login.close = 0;
        }












  }  
}

