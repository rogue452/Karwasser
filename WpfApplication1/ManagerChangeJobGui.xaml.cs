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

namespace project
{
    /// <summary>
    /// Interaction logic for ManagerChangeJobGui.xaml
    /// </summary>
    public partial class ManagerChangeJobGui : Window
    {
        bool keep=false;
        bool only = false;
        DataTable dt = new DataTable("custumers");
        DataTable dt1 = new DataTable("items");
        DataTable dt2 = new DataTable("Contacts");
        string jobid;
        public ManagerChangeJobGui(string jobid1, string oID, string hpcost, string contactNumber)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.jobid = jobid1;
            jobid_label1.Content = jobid;
            order_label.Content = oID;
            cost_label.Content = hpcost;
            contact_label.Content = contactNumber;
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select costumerid as `חפ לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                mysqlDAdp.Fill(dt);
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
                Create_DataTable1_Columns_Start();
                mysqlDAdp.Fill(dt1);
               // Create_DataTable1_Columns_End();
                dataGrid2.ItemsSource = dt1.DefaultView;
                mysqlDAdp.Update(dt1);
                MySqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }




        private void Create_DataTable1_Columns_Start()
        {
            dt1.Columns.Add(new DataColumn("כמות", typeof(string)));
            dt1.Columns.Add(new DataColumn("מקט לקוח", typeof(string)));
        }



        private void Item_Search_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            String searchkey = this.Item_Search_textBox.Text;
            dt1.DefaultView.RowFilter = string.Format("`מקט פריט` LIKE '%{0}%'", searchkey);

        }



        private void keep_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            keep = true;
        }



        private void keep_checkBox_UnChecked(object sender, RoutedEventArgs e)
        { 
            keep = false;
        }



        private void only_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            only = true;
            label2.Visibility = Visibility.Hidden;
            label5.Visibility = Visibility.Hidden;
            label3.Visibility = Visibility.Hidden;
            Item_Search_textBox.Visibility = Visibility.Hidden;
            label6.Visibility = Visibility.Hidden;
            dataGrid2.Visibility = Visibility.Hidden;
            label4.Visibility = Visibility.Hidden;
            orderid_label.Visibility = Visibility.Hidden;
            orderID_textBox.Visibility = Visibility.Hidden;
            startDate_label.Visibility = Visibility.Hidden;
            startdatePicker.Visibility = Visibility.Hidden;
            expectedFinishDate_label1.Visibility = Visibility.Hidden;
            finishdatePicker.Visibility = Visibility.Hidden;
            jobdescription_label.Visibility = Visibility.Hidden;
            jobdes_textbox.Visibility = Visibility.Hidden;
            keep_checkBox.Visibility = Visibility.Hidden;

        }



        private void only_checkBox_UnChecked(object sender, RoutedEventArgs e)
        {
            only = false;
            label2.Visibility = Visibility.Visible;
            label5.Visibility = Visibility.Visible;
            label3.Visibility = Visibility.Visible;
            Item_Search_textBox.Visibility = Visibility.Visible;
            label6.Visibility = Visibility.Visible;
            dataGrid2.Visibility = Visibility.Visible;
            label4.Visibility = Visibility.Visible;
            orderid_label.Visibility = Visibility.Visible;
            orderID_textBox.Visibility = Visibility.Visible;
            startDate_label.Visibility = Visibility.Visible;
            startdatePicker.Visibility = Visibility.Visible;
            expectedFinishDate_label1.Visibility = Visibility.Visible;
            finishdatePicker.Visibility = Visibility.Visible;
            jobdescription_label.Visibility = Visibility.Visible;
            jobdes_textbox.Visibility = Visibility.Visible;
            keep_checkBox.Visibility = Visibility.Visible;
        }

        private void CustumerNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchkey = this.CustumerNameSearchTextBox.Text;
                string Query1 = "select costumerid as `חפ לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers  where  costumerName Like '%" + searchkey + "%' group by costumerid";
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

        private void IDSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchidkey = this.IDSearchTextBox.Text;
                string Query1 = "select costumerid as `חפ לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers  where  costumerid Like '%" + searchidkey + "%' group by costumerid";
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
            if (only == false)
            {
                string orderid, jobdes;
                int del = 0;
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
                            string Query1 = ("SELECT jobid FROM project.jobs WHERE orderid='" + orderid + "' AND startDate<='" + today + "' AND jobid != '" + jobid + "'ORDER BY startDate DESC LIMIT 1 "); //to see if the orderid already in the system.
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            Console.WriteLine("שורה 476");
                            while (dr.Read())
                            {
                                if (!dr.IsDBNull(0))
                                {
                                    Console.WriteLine("שורה 480");
                                    if (MessageBox.Show("מספר הזמנה זהה כבר קיים האחרון ביותר היה עבור מספר עבודה " + dr.GetString(0) + "\n?האם ברצונך להוסיף בכל זאת", "מספר הזמנה קיים", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
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
                        //    string fullyear = DateTime.Now.ToString("yyyy");
                        //   string twodigyear = DateTime.Now.ToString("yy");
                        //   int jobid1 = 0;
                        //   string[] arry;
                        //   string tosplit, first;
                        /*
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
                        */

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
                                            MessageBox.Show("שדה הכמות מכיל כמות שלילית או 0 מקט פריט - " + testrow["מקט פריט"].ToString() + "", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
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


                            string reg_date = DateTime.Now.ToString("yyyy-MM-dd");
                            count = 0;
                            foreach (DataRow dri in changedRecordsItemsTable.Rows)
                            {
                                itemNum = 0;
                                // if the user wants to keep the current items for this job then we need to get the itemNum.
                                if (keep == true)
                                {
                                    try
                                    {
                                        Console.WriteLine("שורה 325");
                                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                        MySqlConn.Open();
                                        string Query1 = ("SELECT MAX(itemNum) FROM project.jobs WHERE jobid='" + jobid + "' AND itemid='" + dri["מקט פריט"].ToString() + "' ");
                                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                                        MSQLcrcommand1.ExecuteNonQuery();
                                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                                        Console.WriteLine("שורה 333");
                                        while (dr.Read())
                                        {
                                            if (!dr.IsDBNull(0))
                                            {
                                                itemNum = dr.GetInt32(0);
                                            }
                                        }

                                        MySqlConn.Close();

                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                        return; ;
                                    }

                                }
                                string q = dri["כמות"].ToString();
                                string cosItemID = dri["מקט לקוח"].ToString();
                                if (cosItemID == "")
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
                                            // if the user wanted to delete the current items then we need to do it just one time BEFOUR we add the new items but after we know that all the inputs are OK. 
                                            if (keep == false)
                                            {
                                                if (del == 0)
                                                {
                                                    try
                                                    {
                                                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                                        MySqlConn.Open();
                                                        string Query1 = "DELETE FROM jobs WHERE jobid ='" + jobid + "'";
                                                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                                                        MSQLcrcommand1.ExecuteNonQuery();
                                                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                                                        MySqlConn.Close();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        MessageBox.Show(ex.Message);
                                                    }
                                                    del++;

                                                }// end if (del == 0)
                                            } //end if (keep == false)

                                            int expected = itemNum + item_quantity;
                                            string itemid1 = dri["מקט פריט"].ToString();
                                            //string reg_date = DateTime.Now.ToString("yyyy-MM-dd");
                                            for (int i = 1; i <= item_quantity; i++)
                                            {
                                                Console.WriteLine("לפני שאילתא");
                                                itemNum++;
                                                string itemid = dri["מקט פריט"].ToString();
                                                
                                                try
                                                {
                                                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                                    MySqlConn.Open();
                                                    string Query1 = ("INSERT INTO project.jobs (jobid, itemid,itemNum, expectedItemQuantity,costumerid, jobdescription, startDate, expectedFinishDate, contact_id,orderid,group_costomer_itemid ,reg_date) VALUES ('" + jobid + "','" + itemid + "','" + itemNum + "','" + expected + "','" + selected + "','" + jobdes + "','" + start + "','" + end + "','" + contactid + "','" + orderid + "','" + cosItemID + "' , '" + reg_date + "')");
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

                                            //now we need to uptade all of the old items in this current itemid with our new description of the job.
                                            if (keep == true)
                                            {
                                                try
                                                {
                                                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                                    MySqlConn.Open();
                                                    string Query1 = "UPDATE jobs SET expectedItemQuantity='" + expected + "',itemsDescription='לא נרשם תיאור',group_Status='רישום' ,group_costomer_itemid='" + cosItemID + "', group_StageOrder='1' ,reg_date='" + reg_date + "'  where jobid='" + jobid + "' AND itemid='" + itemid1 + "'";
                                                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                                                    MSQLcrcommand1.ExecuteNonQuery();
                                                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                                                    MySqlConn.Close();
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.Message);
                                                }
                                            }

                                        }//end if (item_quantity > 0)
                                        else { MessageBox.Show("שדה הכמות מכיל כמות שלילית או 0 בפריט מספר - " + dri["מקט פריט"].ToString() + ""); return; }

                                    } // if (q != "")
                                    else { count++; }


                                }// end try
                                catch
                                { MessageBox.Show("שדה הכמות לא כולל רק מספרים בפריט מספר - " + dri["מקט פריט"].ToString() + ""); return; }

                            }// end foreach (DataRow dri in changedRecordsTable.Rows)
                            if (count == changedRecordsItemsTable.Rows.Count)
                            {
                                MessageBox.Show("  לא נבחרו פריטים מהטבלה "); return;
                            }

                            else
                            {
                                //now we need to uptade all of the old items whith itemid that we did not add to our new description of the job.
                                if (keep == true)
                                {
                                    try
                                    {
                                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                        MySqlConn.Open();
                                        string Query1 = "UPDATE jobs SET job_status='נרשמה',jobdescription='" + jobdes + "',startDate='" + start + "',expectedFinishDate='" + end + "',actualFinishDate=NULL ,costumerid='" + selected + "',contact_id='" + contactid + "',orderid='" + orderid + "',deliveryid='לא עודכן',invoiceNumber='לא עודכן' ,reg_date='" + reg_date + "' WHERE jobid='" + jobid + "'";
                                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                                        MSQLcrcommand1.ExecuteNonQuery();
                                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                                        MySqlConn.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                        MessageBox.Show("!שינוי העבודה נכשל בגלל בעיה בחיבור\n  .אנא בדוק את תקינות העבודה ופרטיה", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }
                                }
                                MessageBox.Show("!שינוי העבודה הצליח", "הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                                ManagerJobGui MJG = new ManagerJobGui();
                                MJG.Show();
                                Login.close = 1;
                                this.Close();
                            }
                        }
                        catch
                        {
                            MessageBox.Show("לא נבחרו פריטים");


                            return;
                        }
                    }
                    catch { MessageBox.Show("לא נבחר איש קשר"); Console.WriteLine("לא נבחר איש קשר"); return; }

                }
                catch { MessageBox.Show("לא נבחר לקוח"); Console.WriteLine("לא נבחר לקוח"); return; }
            } //end if(only==false)

            else // the user only wants to update the costumer and\or contact.
            {
                try
                {
                    // if not cost was selected.
                    DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

                    try
                    {   // if no contact to the cost was selected.
                        DataRowView row1 = (DataRowView)dataGrid3.SelectedItems[0];
                    }
                    catch
                    {
                        MessageBox.Show("!לא נבחר איש קשר", "שיםב לב", MessageBoxButton.OK, MessageBoxImage.Warning);
                        //MessageBox.Show("לא נבחר איש קשר"); Console.WriteLine("לא נבחר איש קשר");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("!לא נבחר לקוח", "שים לב", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //MessageBox.Show("לא נבחר לקוח"); Console.WriteLine("לא נבחר לקוח"); 
                    return; 
                }
                DataRowView rowcost = (DataRowView)dataGrid1.SelectedItems[0];
                string costid = rowcost["חפ לקוח"].ToString();
                DataRowView rowcontact = (DataRowView)dataGrid3.SelectedItems[0];
                string contactid = rowcontact["מספר איש קשר"].ToString();
                // now we need to update the cost and contact for this jobid
                try
                {
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = "UPDATE jobs SET costumerid='" + costid + "',contact_id='" + contactid + "' WHERE jobid='" + jobid + "'";
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();
                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                    MySqlConn.Close();
                    MessageBox.Show("!עדכון הלקוח ו/או איש הקשר הצליח", "הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("!עדכון הלקוח ו/או איש הקשר נכשל בגלל בעיה בחיבור\n  .אנא נסה שוב", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "חפ לקוח" || e.Column.Header.ToString() == "שם לקוח" || e.Column.Header.ToString() == "כתובת לקוח")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
        }

        private void dataGrid3_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מספר איש קשר" || e.Column.Header.ToString() == "שם איש קשר" || e.Column.Header.ToString() == "מחלקת איש קשר")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
        }

        private void Grid_AutoGeneratingColumn1(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מקט פריט" || e.Column.Header.ToString() == "שם פריט" || e.Column.Header.ToString() == "תאור פריט")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
        }


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


        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerJobGui MJG = new ManagerJobGui();
            MJG.Show();
            Login.close = 1;
            this.Close();

        }


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
