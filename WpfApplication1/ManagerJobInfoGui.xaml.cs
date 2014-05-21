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
    /// Interaction logic for ManagerJobInfoGui.xaml
    /// </summary>
    public partial class ManagerJobInfoGui : Window
    {
        DataTable dt = new DataTable("jobinfo");
        string jobID;
        public ManagerJobInfoGui(string jobID1)
        {
            this.jobID = jobID1;
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            refreashandClear();
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


        private void refreashandClear()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("SELECT jobs.itemid as `מקט פריט`,item.itemName as `שם פריט`,group_costomer_itemid as `מקט לקוח`, expectedItemQuantity as `כמות נדרשת`, COUNT(jobs.itemid) as `כמות בפועל` ,itemsDescription as `תיאור סט`, group_Status as `סטטוס קבוצה` , group_StageOrder as `מספר שלב הקבוצה`  FROM jobs,item  WHERE jobs.jobid='" + jobID + "' AND jobs.itemid=item.itemid AND jobs.itemStageOrder=item.itemStageOrder AND jobs.itemStatus=item.itemStatus group by jobs.itemid ");
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



        private void ItemIDSearch_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchkey = this.ItemIDSearch_TextBox.Text;
                string Query1 = ("SELECT jobs.itemid as `מקט פריט`,item.itemName as `שם פריט`,group_costomer_itemid as `מקט לקוח`, expectedItemQuantity as `כמות נדרשת`, COUNT(jobs.itemid) as `כמות בפועל` ,itemsDescription as `תיאור סט`, group_Status as `סטטוס קבוצה` , group_StageOrder as `מספר שלב הקבוצה` ,stageName as `שם שלב הקבוצה`, stage_discription as `תאור שלב הקבוצה`  FROM jobs,item  WHERE jobs.jobid='" + jobID + "' AND jobs.itemid=item.itemid AND jobs.itemStageOrder=item.itemStageOrder AND jobs.itemStatus=item.itemStatus AND jobs.itemid Like '%" + searchkey + "%'  group by jobs.itemid ");
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
                if (MessageBox.Show("?האם אתה בטוח שברצונך למחוק פריט זה", "וידוא מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else // if the user clicked on "Yes" so he wants to Delete.
                {
                    string selected = row["מקט פריט"].ToString();

                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "delete from jobs where itemid='" + selected + "'and jobs.jobid='" + jobID + "'";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();
                        MessageBox.Show("!קבוצת הפריט נמחקה מהמערכת");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    refreashandClear();

                }//end else

            }//end try
            catch { MessageBox.Show("לא נבחרה קבוצת פריט למחיקה"); }

        }//end function



        // go to previous screen.
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerJobGui MJG = new ManagerJobGui();
            MJG.Show();
            Login.close = 1;
            this.Close();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

                if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן סט פריט זה", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;  //dont do stuff
                }

                else // if the user clicked on "Yes" so he wants to Update.
                {
                    string selected_Item = row["מקט פריט"].ToString();
                    string itemdesc = row["תיאור סט"].ToString();
                    string exqun = row["כמות נדרשת"].ToString();
                    string status = row["סטטוס קבוצה"].ToString();
                    string cosMAKAT = row["מקט לקוח"].ToString();
                    string currStageOrder = row["מספר שלב הקבוצה"].ToString();
                    try
                    {
                        int expectedq = Convert.ToInt32(exqun);
                    }
                    catch { 
                            MessageBox.Show("שדה הכמות נדרשת לא מכיל רק מספרים" , "שים לב" , MessageBoxButton.OK, MessageBoxImage.Error);
                            // need to do refreash
                            refreashandClear();
                            return; 
                          }

                    // now we will check if the status of the group was chenged.
                    bool gruopStatusChanged = false;
                    string oldstatus = "", oldStageOrder="";
                    try
                    {
                        Console.WriteLine("שורה 219");
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT group_Status ,group_StageOrder FROM project.jobs WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'LIMIT 1  ");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        Console.WriteLine("שורה 227");
                        while (dr.Read())
                        {
                            if (!dr.IsDBNull(0))
                            {
                                Console.WriteLine("שורה 232");
                                if (status != dr.GetString(0))
                                {// if group status changed.
                                    oldstatus = dr.GetString(0); // get the old group_Status.
                                    oldStageOrder = dr.GetString(1); // get the old group_StageOrder.
                                    gruopStatusChanged = true;
                                    Console.WriteLine(gruopStatusChanged);
                                }
                                else 
                                {
                                    oldstatus = status;
                                    oldStageOrder = currStageOrder;
                                } 
                                
                            }
                        }
                        dr.Close();
                        MySqlConn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return; ;
                    }
                    Console.WriteLine(status);
                    Console.WriteLine(oldstatus);
                    Console.WriteLine(oldStageOrder);


                    // if the user did not changed the group status.
                    if (gruopStatusChanged == false)
                    {
                        Console.WriteLine("שורה 252");
                        try
                        {

                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='"+cosMAKAT+"' WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();
                            //MessageBox.Show("! סט הפריט עודכן");
                            MessageBox.Show("!סט הפריט עודכן", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        refreashandClear();
                    } // end if (gruopStatusChanged == false).



                    else // user changed the group status.
                    {//        if the changed is not allwoed.
                        if ((oldstatus == "גמר ייצור") && (oldStageOrder != "0") && (status == "תיקון"))
                        {
                            MessageBox.Show("!מעבר אל שלב התיקון אפשרי בשלב גמר ייצור רק ממספר שלב 0 - עדכון לא אושר", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                            refreashandClear();
                            return;
                        }

                        if ( (oldstatus == "תיקון") && (status != "בעבודה" ) )
                        {
                            if  (status != "גמר ייצור")
                           {
                               if (status != "פסול")
                               {
                                   MessageBox.Show("!מעבר מסטטוס התיקון אפשרי רק אל הסטטוסים הבאים:\n גמר ייצור, פסול או  בעבודה.\n העדכון לא אושר", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                   refreashandClear();
                                   return;
                               }
                            }
                        }

                        Console.WriteLine(status);
                        Console.WriteLine(oldstatus);
                        //Console.WriteLine(oldStageOrder);
                        if ((status == "תיקון") && (oldstatus != "בעבודה" ))
                        {
                           if  (oldstatus != "גמר ייצור")
                           {
                            MessageBox.Show("!מעבר אל שלב התיקון אפשרי רק אל משלב גמר ייצור או שלב בעבודה - עדכון לא אושר", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                            refreashandClear();
                            return;
                           }
                        } 

                        
                        bool fromFixToWork = false, fromWorkToFix = false , fromFixToFinish=false , fromFinishToFix=false ;

                        if (oldstatus == "תיקון" && status == "גמר ייצור")
                        {
                            fromFixToFinish = true;
                        } Console.WriteLine("fromFixToFinish= " + fromFixToFinish);

                        if (oldstatus == "גמר ייצור" && status == "תיקון")
                        {
                            if (oldStageOrder == "0")
                            {
                                fromFinishToFix = true;
                            }
                        } Console.WriteLine("fromFinishToFix= " + fromFinishToFix);


                        //      oldStageOrder = currStageOrder;
                       
                        // if the new status is work and the old is fix then we need to return to origanel itemStageOrder = itemToFixStageOrder and group_StageOrder = itemStageOrder.
                        if (oldstatus == "תיקון" && status=="בעבודה")
                        {   
                            fromFixToWork = true;   
                        }
                        Console.WriteLine("fromFixToWork= " + fromFixToWork);

                        // if the new status is fix and the old is work  then we need to set the itemToFixStageOrder = group_StageOrder itemStageOrder='1'.
                        if (oldstatus == "בעבודה" && status == "תיקון")
                        {
                            fromWorkToFix = true;
                        }
                        Console.WriteLine("fromWorkToFix= " + fromWorkToFix);


                        Console.WriteLine("שורה 356");
                        // to get itemToFixStageOrder .
                        string itemToFixStageOrder = "";
                        try
                        { // we need to get the itemToFixStageOrder of the group.
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            Console.WriteLine("שורה 363");
                            string Query12 = ("SELECT itemToFixStageOrder FROM project.jobs WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' LIMIT 1  ");
                            MySqlCommand MSQLcrcommand11 = new MySqlCommand(Query12, MySqlConn);
                            MSQLcrcommand11.ExecuteNonQuery();
                            MySqlDataReader dr = MSQLcrcommand11.ExecuteReader();
                            MySqlDataAdapter mysqlDAdp1 = new MySqlDataAdapter(MSQLcrcommand11);
                            Console.WriteLine("שורה 369");
                            while (dr.Read())
                            {

                                itemToFixStageOrder = dr.GetString(0);
                                Console.WriteLine("שורה 374");


                            }
                            MySqlConn.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            refreashandClear();
                            return; ;
                        }
                        Console.WriteLine("שורה 386");


                        // now we need to update only the group to the the user's selected status.
                        try
                        {
                            string Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='1' WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' ";
                            Console.WriteLine(Query1);
                            Console.WriteLine("שורה 394");

                            
                            if (fromFixToWork == true)
                            {
                               
                                if (itemToFixStageOrder == "0")
                                {
                                    MessageBox.Show("לפני שלב התיקון היית בשלב גמר ייצור ולא בעבודה \n     !עידכון לא אושר", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                    refreashandClear();
                                    return;
                                }
                                // now we need to update the group.
                                Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='" + itemToFixStageOrder + "', itemToFixStageOrder='תוקן בעבר מבעבודה'  WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' ";
                            }


                            if (fromFixToFinish == true)
                            {
                               if (itemToFixStageOrder != "0")
                                {
                                    MessageBox.Show("לפני שלב התיקון היית בשלב העבודה ולא גמר ייצור\n     !עידכון לא אושר", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                    refreashandClear();
                                    return;
                                }
                               Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='" + itemToFixStageOrder + "', itemToFixStageOrder='תוקן בעבר מגמר ייצור'   WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' ";
                            }


                            if (fromFinishToFix == true)
                            {
                                Console.WriteLine("שורה 425 מייצור לתיקון - שלב נוכחי שיכנס למספר שלב לתיקון:");
                                Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='1' , itemToFixStageOrder='0' , group_StageOrder='1'  WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' ";
                                Console.WriteLine("currStageOrder= " + currStageOrder);
                            }
                            if (fromWorkToFix == true)
                            {
                                Console.WriteLine("שורה 431 מעבודה לתיקון - שלב נוכחי שיכנס למספר שלב לתיקון:");
                                Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='1' , itemToFixStageOrder='" + oldStageOrder + "'  WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' ";
                                Console.WriteLine("oldStageOrder= " + oldStageOrder);
                            }

                            if (fromFixToFinish == false && status=="גמר ייצור")
                            {
                                Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='0' ,  group_StageOrder='0' , itemToFixStageOrder='" + itemToFixStageOrder + "' WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' ";
                            }


                            Console.WriteLine("שורה 442");
                            Console.WriteLine(Query1);
                            MySqlConnection MySqlConn1 = new MySqlConnection(Login.Connectionstring);
                            MySqlConn1.Open();
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn1);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn1.Close();
                            //MessageBox.Show("! סט הפריט עודכן");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            refreashandClear();
                            return;

                        }



                        Console.WriteLine("שורה 461");
                        // now do the normal update with the new status.
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='1'   WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                            //if (fromFixToFinish == false && status == "גמר ייצור")
                            if (status == "גמר ייצור")
                            {Console.WriteLine("שורה 470");
                                   Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='0'   WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                            }

                            if (fromFixToWork)
                            {
                                Console.WriteLine("שורה 470");
                                Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='" + itemToFixStageOrder + "'   WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                            }
                         /*   if (fromFixToFinish == true && status == "גמר ייצור")
                            {
                                Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='0'   WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                            }
                         */ Console.WriteLine("שורה 484");
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            Console.WriteLine("שורה 493");
                            refreashandClear();
                            return;
                        }
                        refreashandClear();
                        MessageBox.Show("!סט הפריט עודכן", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                        Console.WriteLine("שורה 499");

                         

                    } // end else - user changed the group status.

                } // end else - if the user clicked on "Yes" so he wants to Update.

            }// end try.
            catch 
            {
                MessageBox.Show("!לא נבחר סט פריט לעדכון", "שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                //MessageBox.Show("לא נבחר סט פריט לעדכון ");
            }
        }






        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מקט פריט" || e.Column.Header.ToString() == "שם פריט" || e.Column.Header.ToString() == "כמות בפועל" || e.Column.Header.ToString() == "מספר שלב הקבוצה" || e.Column.Header.ToString() == "שם שלב הקבוצה" || e.Column.Header.ToString() == "תאור שלב הקבוצה")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }

            if (e.Column.Header.ToString() == "סטטוס קבוצה")
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






        private void Show_Item_button_Click(object sender, RoutedEventArgs e)
        {
             try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["מקט פריט"].ToString();
                ManagerItemInfoGui MIIG = new ManagerItemInfoGui(selected, jobID);
                MIIG.Show();
                Login.close = 1;
                this.Close();
            }
             catch { MessageBox.Show("לא נבחר פריט"); }
        }

        private void Add_Existing_button_Click(object sender, RoutedEventArgs e)
        {
            ManagerAddExistingItemGui MAEIG = new ManagerAddExistingItemGui(jobID);
            MAEIG.Show();
            Login.close = 1;
            this.Close();
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

        private void Item_Stages_button_Click(object sender, RoutedEventArgs e)
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


    }

}