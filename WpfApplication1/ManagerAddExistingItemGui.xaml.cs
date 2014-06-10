// ***********************************************************************
// Assembly         : WpfApplication1
// Author           : user
// Created          : 06-10-2014
//
// Last Modified By : user
// Last Modified On : 06-10-2014
// ***********************************************************************
// <copyright file="ManagerAddExistingItemGui.xaml.cs" company="">
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
    /// Interaction logic for ManagerAddExistingItemGui.xaml
    /// </summary>
    public partial class ManagerAddExistingItemGui : Window
    {
        /// <summary>
        /// The dt
        /// </summary>
        DataTable dt = new DataTable("existingItems");
        /// <summary>
        /// The job identifier
        /// </summary>
        string jobID;
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerAddExistingItemGui"/> class.
        /// </summary>
        /// <param name="jobID1">The job i d1.</param>
        public ManagerAddExistingItemGui(string jobID1)
        {
            this.jobID = jobID1;
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            jobid_label.Content = jobID;


            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select itemid as `מקט פריט`,itemName as `שם פריט`, item_discription as `תאור פריט` from project.item WHERE itemStatus='בעבודה' group by itemid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt1 = new DataTable("items");
                mysqlDAdp.Fill(dt);
                Create_DataTable1_Columns_End();
                //   Make_CheckBox1_Columns_False();
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
        /// Create_s the data table1_ columns_ end.
        /// </summary>
        private void Create_DataTable1_Columns_End()
        {
            dt.Columns.Add(new DataColumn("כמות", typeof(string)));
           // dt.Columns.Add(new DataColumn("מקט לקוח", typeof(string)));
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
                dialog.FileName = " רשימת פריטים לעבודה מספר " + jobID + " נכון לתאריך - " + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString(); ; // Default file name
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
        /// Handles the TextChanged event of the ItemIDSearch_TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void ItemIDSearch_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            String searchkey = this.ItemIDSearch_TextBox.Text;
            dt.DefaultView.RowFilter = string.Format("`מקט פריט` LIKE '%{0}%'", searchkey);
         
        }


        /// <summary>
        /// Handles the TextChanged event of the StageNameSearchTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void StageNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            String searchNamekey = this.StageNameSearchTextBox.Text;
            dt.DefaultView.RowFilter = string.Format("`שם פריט` LIKE '%{0}%'", searchNamekey);
            /*
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchNamekey = this.StageNameSearchTextBox.Text;
                string Query1 = ("select itemid as `מקט פריט`,itemName as `שם פריט`, item_discription as `תאור פריט`   FROM jobs,item WHERE jobs.itemid=item.itemid and jobs.itemStageOrder=item.itemStageOrder and jobs.itemStatus=item.itemStatus and jobs.jobid='" + jobID + "' and item.stageName Like '%" + searchNamekey + "%'");
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
            */
        }
/*
        private void ADD_Btn_Click(object sender, RoutedEventArgs e)
        {
          //  ManagerAddNewItemGUI MANIG = new ManagerAddNewItemGUI(jobID);
          //  MANIG.Show();
          //  this.Close();
        }

*/

        // go to previous screen.
        /// <summary>
        /// Handles the Click event of the Back_Btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerJobInfoGui MJIG = new ManagerJobInfoGui(jobID);
            MJIG.Show();
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
            if (e.Column.Header.ToString() == "מקט פריט" || e.Column.Header.ToString() == "שם פריט" || e.Column.Header.ToString() == "תאור פריט")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }


          /*  if (e.Column.Header.ToString() == "סטטוס הפריט")
            {
                string columnName = e.Column.Header.ToString();
                Dictionary<string, string> comboKey = new Dictionary<string, string>()
                    {
                        {"בעבודה","בעבודה"},
                        {"בתיקון","בתיקון"},
                        {"תקול","תקול"},
                        {"הסתיים","הסתיים"},
                    };
                DataGridTemplateColumn col1 = new DataGridTemplateColumn();
                col1.Header = columnName;
                col1.SortMemberPath = columnName;
                #region Editing
                FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(ComboBox));
                Binding b1 = new Binding(columnName);
                b1.IsAsync = true;
                b1.Mode = BindingMode.TwoWay;
                factory1.SetValue(ComboBox.ItemsSourceProperty, comboKey);
                factory1.SetValue(ComboBox.SelectedValuePathProperty, "Key");
                factory1.SetValue(ComboBox.DisplayMemberPathProperty, "Value");
                factory1.SetValue(ComboBox.SelectedValueProperty, b1);
                factory1.SetValue(ComboBox.SelectedItemProperty, col1);

                DataTemplate cellTemplate1 = new DataTemplate();
                cellTemplate1.VisualTree = factory1;
                col1.CellTemplate = cellTemplate1;
                col1.CellEditingTemplate = cellTemplate1;
                col1.IsReadOnly = false;
                col1.InvalidateProperty(ComboBox.SelectedValueProperty);
                #endregion

                #region View
                FrameworkElementFactory sfactory = new FrameworkElementFactory(typeof(TextBlock));
                sfactory.SetValue(TextBlock.TextProperty, b1);
                DataTemplate cellTemplate = new DataTemplate();
                cellTemplate.VisualTree = sfactory;
                col1.CellTemplate = cellTemplate;
                #endregion

                e.Column = col1;
            }*/
        }




        /// <summary>
        /// Handles the Click event of the Item_Stages_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Item_Stages_button_Click(object sender, RoutedEventArgs e)
        {
             try
            {
             DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
             string selected = row["מקט פריט"].ToString();
             ManagerGeneralItemStagesGui MGISG = new ManagerGeneralItemStagesGui(selected);
             MGISG.ShowDialog();
             //this.Close();
            }
             catch { MessageBox.Show("לא נבחר פריט", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); }
        }



        /// <summary>
        /// Handles the Click event of the Add_Existing_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Add_Existing_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable changedRecordsItemsTable = dt.GetChanges();
                int sizeofItemsnewtable = changedRecordsItemsTable.Rows.Count;
                int count = 0; // will count the number of rows with cell "" in the  changedRecordsItemsTable.
                Console.WriteLine("sizeofItemsnewtable = " + sizeofItemsnewtable);

                // test the input
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


                // if the unpute was OK then we can add the items.
                count = 0;
                foreach (DataRow dri in changedRecordsItemsTable.Rows)// for every row in the updateds table.
                {
                    string q = dri["כמות"].ToString();
                    Console.WriteLine("q = " + q);
                    string itemid = dri["מקט פריט"].ToString();
                    Console.WriteLine("itemid = " + itemid);
                    //string cost_itemid = dri["מקט לקוח"].ToString();
                    try
                    {

                        if (q != "") // in case the user deleted a cell in the item and now it have a string of-  "" .
                        {
                            int new_item_quantity = Convert.ToInt32(q) , maxItemNum=0;
                            if (new_item_quantity > 0)
                            {
                                string  itemStatus = "רישום", itemStageOrder = "1";
                                try
                                {
                                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                    MySqlConn.Open();
                                    string Query1 = "(SELECT *, MAX(itemNum) FROM project.jobs WHERE jobid='" + jobID + "' AND itemid='" + itemid + "')";
                                    Console.WriteLine("שורה 267 Query1 = " + Query1);
                                    MySqlCommand crcommand1 = new MySqlCommand(Query1, MySqlConn);
                                    crcommand1.ExecuteNonQuery();
                                    MySqlDataReader dr1 = crcommand1.ExecuteReader();
                                    int count1 = 0, expectedItemQuantity=0;
                                    string costumerid = "", itemsDescription = "", job_status = "", jobdescription = "", startDate = "", expectedFinishDate = "", contact_id = "", orderid = "", group_Status = "", group_StageOrder = "", deliveryid = "", invoiceNumber = "", group_costomer_itemid = "", group_itemToFixStageOrder = "", reg_date="";
                                    
                                    while (dr1.Read())
                                    {
                                        if (!dr1.IsDBNull(0))  
                                        {
                                            count1++; // inc count only if there are already items with this itemid for this jobid.
                                            Console.WriteLine("שורה 279 count1 = " + count1);
                                            maxItemNum = dr1.GetInt32(25); // get the added MAX(itemNum) column
                                            Console.WriteLine("שורה 281 maxItemNum = " + maxItemNum);
                                            expectedItemQuantity = dr1.GetInt32(5);
                                            Console.WriteLine("שורה 283 expectedItemQuantity = " + expectedItemQuantity);
                                            costumerid = dr1.GetString(6);
                                            Console.WriteLine("שורה 285 costumerid = " + costumerid);
                                            itemsDescription = dr1.GetString(8);
                                            Console.WriteLine("שורה 287 itemsDescription = " + itemsDescription);
                                            job_status = dr1.GetString(10);
                                            Console.WriteLine("שורה 289 job_status = " + job_status);
                                            jobdescription = dr1.GetString(11);
                                            Console.WriteLine("שורה 291 jobdescription = " + jobdescription);
                                            startDate = dr1.GetString(12);
                                            Console.WriteLine("שורה 293 startDate = " + startDate);
                                            startDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
                                            Console.WriteLine("שורה 295 startDate = " + startDate);
                                            expectedFinishDate = dr1.GetString(13);
                                            Console.WriteLine("שורה 297 expectedFinishDate = " + expectedFinishDate);
                                            expectedFinishDate = Convert.ToDateTime(expectedFinishDate).ToString("yyyy-MM-dd");
                                            Console.WriteLine("שורה 299 expectedFinishDate = " + expectedFinishDate);
                                            contact_id = dr1.GetString(15);
                                            Console.WriteLine("שורה 301 contact_id = " + contact_id);
                                            orderid = dr1.GetString(16);
                                            Console.WriteLine("שורה 303 orderid = " + orderid);
                                            group_Status = dr1.GetString(17);
                                            Console.WriteLine("שורה 305 group_Status = " + group_Status);
                                            group_StageOrder = dr1.GetString(18);
                                            Console.WriteLine("שורה 307 group_StageOrder = " + group_StageOrder);
                                            deliveryid = dr1.GetString(19);
                                            Console.WriteLine("שורה 309 deliveryid = " + deliveryid);
                                            invoiceNumber = dr1.GetString(20);
                                            Console.WriteLine("שורה 311 invoiceNumber = " + invoiceNumber);
                                            group_costomer_itemid = dr1.GetString(21);
                                            Console.WriteLine("שורה 312 group_costomer_itemid = " + group_costomer_itemid);
                                            group_itemToFixStageOrder= dr1.GetString(22);
                                            Console.WriteLine("שורה 315 group_itemToFixStageOrder = " + group_itemToFixStageOrder);
                                            reg_date = dr1.GetString(24);
                                            Console.WriteLine("שורה 317 reg_date = " + reg_date);
                                            reg_date = Convert.ToDateTime(reg_date).ToString("yyyy-MM-dd");
                                            Console.WriteLine("שורה 319 reg_date = " + reg_date);
                                        }
                                    }
                                    MySqlConn.Close();
                                    Console.WriteLine("שורה 323 לפני בדיקה האם המקט כבר קיים");
                                    if (count1 == 1)// if an itemid already exist in this job.
                                    {
                                      //  int new_expected = new_item_quantity + expectedItemQuantity;
                                        for (int i = 1; i <= new_item_quantity; i++)
                                        {
                                            Console.WriteLine("שורה 329 if (count1 == 1)// if an itemid already exist in this job.");
                                            maxItemNum++; 
                                          //  string itemid = dri["מקט פריט"].ToString();
                                            try
                                            {
                                                MySqlConnection MySqlConn1 = new MySqlConnection(Login.Connectionstring);
                                                MySqlConn1.Open();
                                                string Query2 = ("INSERT INTO project.jobs (jobid, itemid,itemNum,itemStatus,itemStageOrder, expectedItemQuantity,costumerid, itemsDescription, job_status, jobdescription, startDate, expectedFinishDate, contact_id , orderid , group_Status , group_StageOrder , deliveryid , invoiceNumber , group_costomer_itemid, group_itemToFixStageOrder, reg_date ) VALUES ('" + jobID + "','" + itemid + "','" + maxItemNum + "','" + itemStatus + "','" + itemStageOrder + "','" + expectedItemQuantity + "','" + costumerid + "','" + itemsDescription + "','" + job_status + "','" + jobdescription + "','" + startDate + "','" + expectedFinishDate + "','" + contact_id + "','" + orderid + "','" + group_Status + "','" + group_StageOrder + "','" + deliveryid + "','" + invoiceNumber + "','" + group_costomer_itemid + "','" + group_itemToFixStageOrder + "','" + reg_date + "')");
                                                Console.WriteLine("שורה 377 Query2 = " + Query2);
                                                MySqlCommand MSQLcrcommand2 = new MySqlCommand(Query2, MySqlConn1);
                                                MSQLcrcommand2.ExecuteNonQuery();
                                                MySqlDataAdapter mysqlDAdp1 = new MySqlDataAdapter(MSQLcrcommand2);
                                                MySqlConn1.Close();
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message);
                                                Console.WriteLine("if (for (int i = 1; i <= new_item_quantity; i++))");
                                                return;
                                            }

                                        }//end for (int i = 1; i <= new_item_quantity; i++)

                                    } //  end of if (count1 == 1)// if an itemid already exist in this job.

                                    else // cound1==0 so no such itemid for this jobid.
                                    {
                                        try
                                        {
                                            Console.WriteLine("שורה 373 cound1==0 so no such itemid for this jobid. ");
                                            MySqlConnection MySqlConn5 = new MySqlConnection(Login.Connectionstring);
                                            MySqlConn5.Open();
                                            string Query5 = "(SELECT *, MAX(itemNum) FROM project.jobs WHERE jobid='" + jobID + "')";
                                            Console.WriteLine("שורה 377 Query5 = " + Query5);
                                            MySqlCommand crcommand5 = new MySqlCommand(Query5, MySqlConn5);
                                            crcommand5.ExecuteNonQuery();
                                            MySqlDataReader dr5 = crcommand5.ExecuteReader();
                                            while (dr5.Read())
                                            {
                                                expectedItemQuantity = dr5.GetInt32(5);
                                                Console.WriteLine("שורה 384 expectedItemQuantity = " + expectedItemQuantity);
                                                costumerid = dr5.GetString(6);
                                                Console.WriteLine("שורה 386 costumerid = " + costumerid);
                                                itemsDescription = dr5.GetString(8);
                                                Console.WriteLine("שורה 388 itemsDescription = " + itemsDescription);
                                                job_status = dr5.GetString(10);
                                                Console.WriteLine("שורה 390 job_status = " + job_status);
                                                jobdescription = dr5.GetString(11);
                                                Console.WriteLine("שורה 392 jobdescription = " + jobdescription);
                                                startDate = dr5.GetString(12);
                                                Console.WriteLine("שורה 394 startDate = " + startDate);
                                                startDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
                                                Console.WriteLine("שורה 396 startDate = " + startDate);
                                                expectedFinishDate = dr5.GetString(13);
                                                Console.WriteLine("שורה 398 expectedFinishDate = " + expectedFinishDate);
                                                expectedFinishDate = Convert.ToDateTime(expectedFinishDate).ToString("yyyy-MM-dd");
                                                Console.WriteLine("שורה 400 expectedFinishDate = " + expectedFinishDate);
                                                contact_id = dr5.GetString(15);
                                                Console.WriteLine("שורה 402 contact_id = " + contact_id);

                                                orderid = dr5.GetString(16);
                                                Console.WriteLine("שורה 405 orderid = " + orderid);
                                                deliveryid = dr5.GetString(19);
                                                Console.WriteLine("שורה 407 deliveryid = " + deliveryid);
                                                invoiceNumber = dr5.GetString(20);
                                                Console.WriteLine("שורה 409 invoiceNumber = " + invoiceNumber);
                                                reg_date = dr5.GetString(24);
                                                Console.WriteLine("שורה 411 reg_date = " + reg_date);
                                                reg_date = Convert.ToDateTime(reg_date).ToString("yyyy-MM-dd");
                                                Console.WriteLine("שורה 413 reg_date = " + reg_date);
                                            }
                                            MySqlConn5.Close();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                            Console.WriteLine("שורה 423");
                                            return;
                                        }

                                        int itemNum = 0;
                                        for (int i = 1; i <= new_item_quantity; i++)
                                        {
                                            itemNum++;
                                            Console.WriteLine("שורה 430 itemNum = " + itemNum);
                                            try
                                            {
                                                MySqlConnection MySqlConn4 = new MySqlConnection(Login.Connectionstring);
                                                MySqlConn4.Open();
                                                string Query4 = ("INSERT INTO project.jobs (jobid, itemid,itemNum,itemStatus,itemStageOrder, expectedItemQuantity,costumerid, itemsDescription, job_status, jobdescription, startDate, expectedFinishDate, contact_id , orderid  , deliveryid , invoiceNumber , reg_date) VALUES ('" + jobID + "','" + itemid + "','" + itemNum + "','" + itemStatus + "','" + itemStageOrder + "','" + new_item_quantity + "','" + costumerid + "','" + itemsDescription + "','" + job_status + "','" + jobdescription + "','" + startDate + "','" + expectedFinishDate + "','" + contact_id + "','" + orderid + "','" + deliveryid + "','" + invoiceNumber + "','" + reg_date + "')");
                                                Console.WriteLine("שורה 436 Query4 = " + Query4);
                                                MySqlCommand MSQLcrcommand4 = new MySqlCommand(Query4, MySqlConn4);
                                                MSQLcrcommand4.ExecuteNonQuery();
                                                MySqlDataAdapter mysqlDAdp4 = new MySqlDataAdapter(MSQLcrcommand4);
                                                MySqlConn4.Close();

                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message);
                                                Console.WriteLine("שורה 445");
                                                return;
                                            }

                                        } // end for (int i = 1; i <= new_item_quantity; i++)
                                    }// end else // cound1==0 so no such itemid for this jobid.

                                }// end of first try.
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                    Console.WriteLine("Query1");
                                    return;
                                }

                               
                            }//end if (item_quantity > 0)
                            else 
                            { 
                                MessageBox.Show("שדה הכמות מכיל כמות שלילית או 0 בפריט מספר - " + dri["מקט פריט"].ToString() + "" , "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                return; 
                            }

                        } // if (q != "")
                        else { count++; }


                    }// end try
                    catch
                    { 
                        MessageBox.Show("שדה הכמות לא כולל רק מספרים בפריט מספר - " + dri["מקט פריט"].ToString() + "", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; 
                    }

                }// end foreach (DataRow dri in changedRecordsTable.Rows)}

                if (count == changedRecordsItemsTable.Rows.Count)
                {
                    MessageBox.Show("  לא נבחרו פריטים מהטבלה ", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); return;
                }
                else 
                { 
                    MessageBox.Show(".!הפריט/ים נוסף/ו לעבודה", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                    ManagerJobInfoGui MJIG = new ManagerJobInfoGui(jobID);
                    MJIG.Show();
                    Login.close = 1;
                    this.Close();
                }
            }
            catch 
            {
                MessageBox.Show("לא נבחרו פריטים", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
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