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
    public partial class ManagerItemInfoGui : Window
    {
        DataTable dt = new DataTable("iteminfo");
        string itemID, jobID;
        public ManagerItemInfoGui(string itemID1, string jobID1)
        {
            this.itemID = itemID1;
            this.jobID = jobID1;
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            label1.Content = jobID;
            label4.Content = itemID;

            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("SELECT jobs.itemNum as `מספר פריט בקבוצה`, jobs.itemDescription as `תיאור פריט`,jobs.itemStatus  as `סטטוס הפריט`, jobs.itemStageOrder as `מספר השלב הנוכחי`,stageName  as `שם השלב הנוכחי` ,item.stage_discription as `תאור השלב הנוכחי`,itemToFixStageOrder as `מספר השלב שבו זוהה כתקול (אם זוהה)`  FROM jobs,item WHERE jobs.itemid=item.itemid and jobs.itemStageOrder=item.itemStageOrder and jobs.itemStatus=item.itemStatus and jobs.jobid='" + jobID + "' and jobs.itemid='" + itemID + "' ");
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
                string Query1 = ("SELECT jobs.itemNum as `מספר פריט בקבוצה`, jobs.itemDescription as `תיאור פריט`,jobs.itemStatus  as `סטטוס הפריט`, jobs.itemStageOrder as `מספר השלב הנוכחי`,stageName  as `שם השלב הנוכחי` ,item.stage_discription as `תאור השלב הנוכחי`,itemToFixStageOrder as `מספר השלב שבו זוהה כתקול (אם זוהה)`  FROM jobs,item WHERE jobs.itemid=item.itemid and jobs.itemStageOrder=item.itemStageOrder and jobs.itemStatus=item.itemStatus and jobs.jobid='" + jobID + "' and jobs.itemid='" + itemID + "' and jobs.itemNum Like '%" + searchkey + "%'");
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
                string Query1 = ("SELECT jobs.itemNum as `מספר פריט בקבוצה`, jobs.itemDescription as `תיאור פריט`,jobs.itemStatus  as `סטטוס הפריט`, jobs.itemStageOrder as `מספר השלב הנוכחי`,stageName  as `שם השלב הנוכחי` ,item.stage_discription as `תאור השלב הנוכחי`,itemToFixStageOrder as `מספר השלב שבו זוהה כתקול (אם זוהה)`  FROM jobs,item WHERE jobs.itemid=item.itemid and jobs.itemStageOrder=item.itemStageOrder and jobs.itemStatus=item.itemStatus and jobs.jobid='" + jobID + "'and jobs.itemid='" + itemID + "' and item.stageName Like '%" + searchNamekey + "%'");
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
                //System.Collections.IList rows = dataGrid1.SelectedItems;
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                if (MessageBox.Show("?האם אתה בטוח שברצונך למחוק פריט זה", "וידוא מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;
                }
                else // if the user clicked on "Yes" so he wants to Delete.
                {
                    string itemnum = row["מספר פריט בקבוצה"].ToString();
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "DELETE FROM jobs WHERE jobid='" + jobID + "' and itemid='" + itemID + "'and itemNum='" + itemnum + "'";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();
                        MessageBox.Show("!הפריט נמחק מהעבודה");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT jobs.itemNum as `מספר פריט בקבוצה`, jobs.itemDescription as `תיאור פריט`,jobs.itemStatus  as `סטטוס הפריט`, jobs.itemStageOrder as `מספר השלב הנוכחי`,stageName  as `שם השלב הנוכחי` ,item.stage_discription as `תאור השלב הנוכחי`,itemToFixStageOrder as `מספר השלב שבו זוהה כתקול (אם זוהה)`  FROM jobs,item WHERE jobs.itemid=item.itemid and jobs.itemStageOrder=item.itemStageOrder and jobs.itemStatus=item.itemStatus and jobs.jobid='" + jobID + "' and jobs.itemid='" + itemID + "' ");
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
            catch { MessageBox.Show("לא נבחר פריט למחיקה"); }

        }//end function



        // go to previous screen.
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerJobInfoGui MJIG = new ManagerJobInfoGui(jobID);
            MJIG.Show();
            Login.close = 1;
            this.Close();
        }



        //////////////////////
        //////////////////////
        // This func is not complete and will be changed 
        /////////////////////
        /////////////////////
        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string itemnum = row["מספר פריט בקבוצה"].ToString();

                if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן פריט זה", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                { }
                else // if the user clicked on "Yes" so he wants to Update.
                {
                    string status = row["סטטוס הפריט"].ToString();
                    string itemdes = row["תיאור פריט"].ToString();

                    // check if the item status pattern exist.
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT item.itemStatus FROM item WHERE item.itemid='" + itemID + " AND itemStatus='" + status + " ");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                        int exist = 0;
                        while (dr.Read())
                        {
                            if (!dr.IsDBNull(0))
                            {
                                exist=1;
                            }
                        }
                        MySqlConn.Close();
                        if (exist == 0)
                        {
                            MessageBox.Show("!לא נוצרו עדיין שלבים עבור הסטטוס שבחרת - סטטוס לא עודכן");
                            // need to do refreash here.
                            return;
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); return; }


                    //doing the update
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "update jobs set itemStatus='" + status + "', itemDescription='" + itemdes + "'  where jobid='" + jobID + "' and itemid='" + itemID + "'and itemNum='" + itemnum + "'";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();


                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); return; }


                    // refreash (need to change Query1)
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT jobs.itemNum as `מספר פריט בקבוצה`,jobs.itemStatus  as `סטטוס הפריט`, jobs.itemStageOrder as `מספר השלב הנוכחי`,stageName  as `שם השלב הנוכחי` ,item.stage_discription as `תאור השלב הנוכחי`,itemToFixStageOrder as `מספר השלב שבו זוהה כתקול (אם זוהה)`  FROM jobs,item WHERE jobs.itemid=item.itemid and jobs.itemStageOrder=item.itemStageOrder and jobs.itemStatus=item.itemStatus and jobs.jobid='" + jobID + "' and jobs.itemid='" + itemID + "' ");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        dt.Clear();
                        mysqlDAdp.Fill(dt);
                        dataGrid1.ItemsSource = dt.DefaultView;
                        mysqlDAdp.Update(dt);
                        MySqlConn.Close();
                        MessageBox.Show ("סטטוס התעדכן");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch { MessageBox.Show("לא נבחר פריט לעדכון "); }
        }
            
    

        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "שם השלב הנוכחי" || e.Column.Header.ToString() == "מספר השלב הנוכחי" || e.Column.Header.ToString() == "מספר פריט" || e.Column.Header.ToString() == "מספר פריט בקבוצה" || e.Column.Header.ToString() == "תאור השלב הנוכחי" || e.Column.Header.ToString() == "מספר השלב שבו זוהה כתקול (אם זוהה)")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }

            if (e.Column.Header.ToString() == "סטטוס הפריט")
            {
                string columnName = e.Column.Header.ToString();
                Dictionary<string, string> comboKey = new Dictionary<string, string>()
                    {
                        {"רישום","רישום"},
                        {"בעבודה","בעבודה"},
                        {"תיקון","תיקון"},
                        {"פסול","פסול"},
                        {"גמר ייצור","גמר ייצור"},
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
                ManagerGeneralItemStagesGui MGIG = new ManagerGeneralItemStagesGui(itemID);
                MGIG.Owner = this;
                MGIG.ShowDialog();
        }





        private void Add_Existing_button_Click(object sender, RoutedEventArgs e)
        {
            ManagerAddExistingItemGui MAEIG = new ManagerAddExistingItemGui(jobID);
            MAEIG.Show();
            Login.close = 1;
            this.Close();
        }



        private void NextStage_button_Click(object sender, RoutedEventArgs e)
        {
            string next_stage="";
            string status = "";
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                if (MessageBox.Show("?האם אתה בטוח שברצונך לקדם שלב פריט זה", "וידוא קידום", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else // if the user clicked on "Yes" so he wants to Delete.
                {
                    // this will give us the first colum of the selected row in the DataGrid.

                   // string status = row["סטטוס הפריט"].ToString();
                    string curr = row["מספר השלב הנוכחי"].ToString();
                    string itemnum =row["מספר פריט בקבוצה"].ToString();
                    // MessageBox.Show("" + status + "");

                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "select itemStatus from jobs where itemid='" + itemID + "' and   jobid='" + jobID + "' and itemNum= '" + itemnum + "'     ";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();

                        while (dr.Read())
                        {
                            if (!dr.IsDBNull(0))
                            {
                                status = dr.GetString(0);
                            }
                           
                        }

                        MySqlConn.Close();
                        // MessageBox.Show("!הלקוח נמחק מהמערכת");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("נפל בשורה מספר 404");
                        MessageBox.Show(ex.Message);
                    }


                   // if (status == "בעבודה" || status=="תיקון")
                   // {

                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = "select MIN(itemStageOrder) from item where itemid='" + itemID + "' and   itemStageOrder>'" + curr + "' and itemStatus= '" + status + "'     ";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);


                            MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();

                            while (dr.Read())
                            {
                                if (!dr.IsDBNull(0))
                                {
                                     next_stage = dr.GetString(0);
                                }
                                else { MessageBox.Show("כבר בוצע השלב האחרון "); return; }
                            }

                             MySqlConn.Close();
                           // MessageBox.Show("!הלקוח נמחק מהמערכת");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("נפל בשורה מספר 437");
                            MessageBox.Show(ex.Message);
                        }




                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = "update jobs set itemStageOrder='" + next_stage + "' where jobid='" + jobID + "' and itemid='" + itemID + "'and itemNum='" + itemnum + "'";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();


                        }
                        catch (Exception ex) 
                        {
                            MessageBox.Show(ex.Message);
                            Console.WriteLine("נפל בשורה מספר 458");
                            return; }



                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = ("SELECT jobs.itemNum as `מספר פריט בקבוצה`,jobs.itemStatus  as `סטטוס הפריט`, jobs.itemStageOrder as `מספר השלב הנוכחי`,stageName  as `שם השלב הנוכחי` ,item.stage_discription as `תאור השלב הנוכחי`,itemToFixStageOrder as `מספר השלב שבו זוהה כתקול (אם זוהה)`  FROM jobs,item WHERE jobs.itemid=item.itemid and jobs.itemStageOrder=item.itemStageOrder and jobs.itemStatus=item.itemStatus and jobs.jobid='" + jobID + "' and jobs.itemid='" + itemID + "' ");
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            dt.Clear();
                            mysqlDAdp.Fill(dt);
                            dataGrid1.ItemsSource = dt.DefaultView;
                            mysqlDAdp.Update(dt);
                            MySqlConn.Close();
                            MessageBox.Show("פריט עבר לשלב הבא");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("נפל בשורה מספר 476");
                            MessageBox.Show(ex.Message);
                        }


                    //}// end of if בעבודה
                }

           

            }
            catch  { MessageBox.Show("לא נבחר פריט לקדם"); return; }








        }

        private void PrevStage_button_Click(object sender, RoutedEventArgs e)
        {

            string prev_stage = "";
            string status = "";
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                if (MessageBox.Show("?האם אתה בטוח שברצונך לחזור שלב  אחורה בפריט זה", "וידוא חזרת שלב", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return; //do no stuff
                }
                else // if the user clicked on "Yes" so he wants to Delete.
                {
                    // this will give us the first colum of the selected row in the DataGrid.

                    // string status = row["סטטוס הפריט"].ToString();
                    string curr = row["מספר השלב הנוכחי"].ToString();
                    string itemnum = row["מספר פריט בקבוצה"].ToString();
                    // MessageBox.Show("" + status + "");

                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "select itemStatus from jobs where itemid='" + itemID + "' and   jobid='" + jobID + "' and itemNum= '" + itemnum + "'     ";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();

                        while (dr.Read())
                        {
                            if (!dr.IsDBNull(0))
                            {
                                status = dr.GetString(0);
                            }

                        }

                        MySqlConn.Close();
                        // MessageBox.Show("!הלקוח נמחק מהמערכת");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("נפסל בשורה מספר 542");
                        MessageBox.Show(ex.Message);
                    }




                   // if (status == "בעבודה" || status == "תיקון")
                   // {

                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = "select MAX(itemStageOrder) from item where itemid='" + itemID + "' and   itemStageOrder<'" + curr + "' and itemStatus= '" + status + "'     ";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);


                            MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();

                            while (dr.Read())
                            {
                                if (!dr.IsDBNull(0))
                                {
                                    prev_stage = dr.GetString(0);
                                }
                                else { MessageBox.Show("הפריט כבר נמצא בשלב הראשוני ביותר"); return; }
                            }

                            MySqlConn.Close();
                            // MessageBox.Show("!הלקוח נמחק מהמערכת");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("נפסל בשורה מספר 578");
                            MessageBox.Show(ex.Message);
                        }




                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = "update jobs set itemStageOrder='" + prev_stage + "' where jobid='" + jobID + "' and itemid='" + itemID + "'and itemNum='" + itemnum + "'";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();


                        }
                        catch (Exception ex) 
                        { 
                            MessageBox.Show(ex.Message);
                            Console.WriteLine("נפסל בשורה מספר 600");
                            return; 
                        }



                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = ("SELECT jobs.itemNum as `מספר פריט בקבוצה`,jobs.itemStatus  as `סטטוס הפריט`, jobs.itemStageOrder as `מספר השלב הנוכחי`,stageName  as `שם השלב הנוכחי` ,item.stage_discription as `תאור השלב הנוכחי`,itemToFixStageOrder as `מספר השלב שבו זוהה כתקול (אם זוהה)`  FROM jobs,item WHERE jobs.itemid=item.itemid and jobs.itemStageOrder=item.itemStageOrder and jobs.itemStatus=item.itemStatus and jobs.jobid='" + jobID + "' and jobs.itemid='" + itemID + "' ");
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            dt.Clear();
                            mysqlDAdp.Fill(dt);
                            dataGrid1.ItemsSource = dt.DefaultView;
                            mysqlDAdp.Update(dt);
                            MySqlConn.Close();
                            MessageBox.Show("הפריט הוחזר לשלב הקודם");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("נפסל בשורה מספר 623");
                            MessageBox.Show(ex.Message);
                        }


                  //  }// end of if בעבודה
                }



            }
            catch { MessageBox.Show("לא נבחר פריט להחזרת שלב"); return; }






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
