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
        DataTable dt = new DataTable("existingItems");
        string jobID;
        public ManagerAddExistingItemGui(string jobID1)
        {
            this.jobID = jobID1;
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;



            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select itemid as `מספר פריט`,itemName as `שם פריט`, item_discription as `תאור פריט` from project.item WHERE itemStatus='בעבודה' group by itemid");
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


        private void Create_DataTable1_Columns_End()
        {
            dt.Columns.Add(new DataColumn("כמות", typeof(string)));
        }



        private void TXTBtn_Click(object sender, RoutedEventArgs e)
        {

            ExportToExcel();
        }





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
                        MessageBox.Show(" נוצר בהצלחה Microsoft Excel -מסמך ה");
                    }
                    else { 
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
                string Query1 = ("SELECT jobs.itemid as `מספר פריט`,quantity as `כמות` ,jobs.itemStageOrder as `מספר השלב הנוכחי`,stageName  as `שם השלב הנוכחי` ,jobs.itemStatus  as `סטטוס הפריט`,itemToFixStageOrder as `מספר השלב שבו זוהה כתקול (אם זוהה)`  FROM jobs,item WHERE jobs.itemid=item.itemid and jobs.itemStageOrder=item.itemStageOrder and jobs.itemStatus=item.itemStatus and jobs.jobid='" + jobID + "' and jobs.itemid Like '%" + searchkey + "%'");
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

        private void StageNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchNamekey = this.StageNameSearchTextBox.Text;
                string Query1 = ("SELECT jobs.itemid as `מספר פריט`,quantity as `כמות` ,jobs.itemStageOrder as `מספר השלב הנוכחי`,stageName  as `שם השלב הנוכחי` ,jobs.itemStatus  as `סטטוס הפריט`,itemToFixStageOrder as `מספר השלב שבו זוהה כתקול (אם זוהה)`  FROM jobs,item WHERE jobs.itemid=item.itemid and jobs.itemStageOrder=item.itemStageOrder and jobs.itemStatus=item.itemStatus and jobs.jobid='" + jobID + "' and item.stageName Like '%" + searchNamekey + "%'");
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

        private void ADD_Btn_Click(object sender, RoutedEventArgs e)
        {
          //  ManagerAddNewItemGUI MANIG = new ManagerAddNewItemGUI(jobID);
          //  MANIG.Show();
          //  this.Close();
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
                }//end else

            }//end try
            catch { MessageBox.Show("לא נבחר לקוח למחיקה"); }

        }//end function



        // go to previous screen.
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerGui MG = new ManagerGui();
            MG.Show();
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
            catch { MessageBox.Show("לא נבחר לקוח לעדכון "); }
        }



        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "שם השלב הנוכחי" || e.Column.Header.ToString() == "מספר השלב הנוכחי" || e.Column.Header.ToString() == "מספר פריט")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }


            if (e.Column.Header.ToString() == "סטטוס הפריט")
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
            }
        }

        


        private void Item_Stages_button_Click(object sender, RoutedEventArgs e)
        {
             try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["מספר פריט"].ToString();
                ManagerItemStagesGui MISG = new ManagerItemStagesGui(selected);
                MISG.Show();
                this.Close();
            }
             catch { MessageBox.Show("לא נבחר פריט"); }
        }



        private void Add_Existing_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable changedRecordsItemsTable = dt.GetChanges();
                int sizeofItemsnewtable = changedRecordsItemsTable.Rows.Count;
                int count = 0; // will count the number of rows with cell "" in the  changedRecordsItemsTable.
                foreach (DataRow dri in changedRecordsItemsTable.Rows)// for every row in the updateds table.
                {
                    string q = dri["כמות"].ToString();
                    string itemid = dri["מספר פריט"].ToString();
                    try
                    {

                        if (q != "") // in case the user deleted a cell in the item and now it have a string of-  "" .
                        {
                            int new_item_quantity = Convert.ToInt32(q) , maxItemNum=0;
                            if (new_item_quantity > 0)
                            {
                                string  itemStatus = "בעבודה", itemStageOrder = "1";
                                try
                                {
                                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                    MySqlConn.Open();
                                    string Query1 = "(SELECT *, MAX(itemNum) FROM project.jobs WHERE jobid='" + jobID + "' AND itemid='" + itemid + "')";
                                    MySqlCommand crcommand1 = new MySqlCommand(Query1, MySqlConn);
                                    crcommand1.ExecuteNonQuery();
                                    MySqlDataReader dr1 = crcommand1.ExecuteReader();
                                    int count1 = 0, expectedItemQuantity=0;
                                    string costumerid = "", itemsDescription = "", job_status = "", jobdescription = "", startDate = "", expectedFinishDate = "", contact_id = "";
                                    
                                    while (dr1.Read())
                                    {
                                        if (!dr1.IsDBNull(0))
                                        {
                                            count1++;
                                            Console.WriteLine(+count1);
                                            maxItemNum = dr1.GetInt32(16);
                                            expectedItemQuantity = dr1.GetInt32(5);
                                            costumerid = dr1.GetString(6);
                                            itemsDescription = dr1.GetString(8);
                                            job_status = dr1.GetString(10);
                                            jobdescription = dr1.GetString(11);
                                            startDate = dr1.GetString(12);
                                            startDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
                                            expectedFinishDate = dr1.GetString(13);
                                            expectedFinishDate = Convert.ToDateTime(expectedFinishDate).ToString("yyyy-MM-dd");
                                            contact_id = dr1.GetString(15);
                                        }
                                    }
                                    MySqlConn.Close();

                                    if (count1 == 1)// if an itemid already exist in this job.
                                    {
                                      //  int new_expected = new_item_quantity + expectedItemQuantity;
                                        for (int i = 1; i <= new_item_quantity; i++)
                                        {
                                            Console.WriteLine("לפני שאילתא");
                                            maxItemNum++; 
                                          //  string itemid = dri["מספר פריט"].ToString();
                                            try
                                            {
                                                MySqlConnection MySqlConn1 = new MySqlConnection(Login.Connectionstring);
                                                MySqlConn1.Open();
                                                string Query2 = ("INSERT INTO project.jobs (jobid, itemid,itemNum,itemStatus,itemStageOrder, expectedItemQuantity,costumerid, itemsDescription, job_status, jobdescription, startDate, expectedFinishDate, contact_id) VALUES ('" + jobID + "','" + itemid + "','" + maxItemNum + "','" + itemStatus + "','" + itemStageOrder + "','" + expectedItemQuantity + "','" + costumerid + "','" + itemsDescription + "','" + job_status + "','" + jobdescription + "','" + startDate + "','" + expectedFinishDate + "','" + contact_id + "')");
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

                                       /* try
                                        {
                                            MySqlConnection MySqlConn3 = new MySqlConnection(Login.Connectionstring);
                                            MySqlConn3.Open();
                                            string Query3 = "UPDATE project.jobs SET expectedItemQuantity='" + new_expected + "' WHERE jobid='"+jobID+"' AND itemid='"+itemid+"' ";
                                            MySqlCommand MSQLcrcommand3 = new MySqlCommand(Query3, MySqlConn3);
                                            MSQLcrcommand3.ExecuteNonQuery();
                                            MySqlDataAdapter mysqlDAdp2 = new MySqlDataAdapter(MSQLcrcommand3);
                                            MySqlConn.Close();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                            Console.WriteLine("Query3");
                                            return;
                                        }
                                         */

                                    }
                                    else // cound1==0 so no such itemid for this jobid.
                                    {
                                        try
                                        {
                                            MySqlConnection MySqlConn5 = new MySqlConnection(Login.Connectionstring);
                                            MySqlConn5.Open();
                                            string Query5 = "(SELECT *, MAX(itemNum) FROM project.jobs WHERE jobid='" + jobID + "')";
                                            MySqlCommand crcommand5 = new MySqlCommand(Query5, MySqlConn5);
                                            crcommand5.ExecuteNonQuery();
                                            MySqlDataReader dr5 = crcommand5.ExecuteReader();

                                            while (dr5.Read())
                                            {
                                                expectedItemQuantity = dr5.GetInt32(5);
                                                costumerid = dr5.GetString(6);
                                                itemsDescription = dr5.GetString(8);
                                                job_status = dr5.GetString(10);
                                                jobdescription = dr5.GetString(11);
                                                startDate = dr5.GetString(12);
                                                startDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
                                                expectedFinishDate = dr5.GetString(13);
                                                expectedFinishDate = Convert.ToDateTime(expectedFinishDate).ToString("yyyy-MM-dd");
                                                contact_id = dr5.GetString(15);
                                            }
                                            MySqlConn5.Close();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                            Console.WriteLine("Query5");
                                            return;
                                        }

                                        int itemNum = 0;
                                        for (int i = 1; i <= new_item_quantity; i++)
                                        {
                                            itemNum++;
                                            
                                            try
                                            {
                                                MySqlConnection MySqlConn4 = new MySqlConnection(Login.Connectionstring);
                                                MySqlConn4.Open();
                                                string Query4 = ("INSERT INTO project.jobs (jobid, itemid,itemNum,itemStatus,itemStageOrder, expectedItemQuantity,costumerid, itemsDescription, job_status, jobdescription, startDate, expectedFinishDate, contact_id) VALUES ('" + jobID + "','" + itemid + "','" + itemNum + "','" + itemStatus + "','" + itemStageOrder + "','" + new_item_quantity + "','" + costumerid + "','" + itemsDescription + "','" + job_status + "','" + jobdescription + "','" + startDate + "','" + expectedFinishDate + "','" + contact_id + "')");
                                                MySqlCommand MSQLcrcommand4 = new MySqlCommand(Query4, MySqlConn4);
                                                MSQLcrcommand4.ExecuteNonQuery();
                                                MySqlDataAdapter mysqlDAdp4 = new MySqlDataAdapter(MSQLcrcommand4);
                                                MySqlConn4.Close();

                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message);
                                                Console.WriteLine("Query4");
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
                            else { MessageBox.Show("שדה הכמות מכיל כמות שלילית או 0 בפריט מספר - " + dri["מספר פריט"].ToString() + ""); return; }

                        } // if (q != "")
                        else { count++; }


                    }// end try
                    catch
                    { MessageBox.Show("שדה הכמות לא כולל רק מספרים בפריט מספר - " + dri["מספר פריט"].ToString() + ""); return; }

                }// end foreach (DataRow dri in changedRecordsTable.Rows)}

                if (count == changedRecordsItemsTable.Rows.Count)
                {
                    MessageBox.Show("  לא נבחרו פריטים מהטבלה "); return;
                }

                else { MessageBox.Show("!הפריט/ים נוסף/ו למערכת"); }
            }
            catch { MessageBox.Show("לא נבחר פריט"); }
        }








    }

}