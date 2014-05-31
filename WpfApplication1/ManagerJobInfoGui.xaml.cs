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
            label1.Content = jobID;
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


        private void refreashandClear()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("SELECT jobs.itemid as `מקט פריט`,item.itemName as `שם פריט`,group_costomer_itemid as `מקט לקוח`,  expectedItemQuantity as `כמות נדרשת`, COUNT(jobs.itemid) as `כמות כוללת בפועל` ,itemsDescription as `תיאור סט ספציפי`, group_Status as `סטטוס קבוצה` , group_StageOrder as `מספר שלב הקבוצה` , item.item_discription as `תיאור פריט` FROM jobs,item  WHERE jobs.jobid='" + jobID + "' AND jobs.itemid=item.itemid AND jobs.itemStageOrder=item.itemStageOrder AND jobs.itemStatus=item.itemStatus group by jobs.itemid ");
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
                string Query1 = ("SELECT jobs.itemid as `מקט פריט`,item.itemName as `שם פריט`,group_costomer_itemid as `מקט לקוח`, expectedItemQuantity as `כמות נדרשת`, COUNT(jobs.itemid) as `כמות כוללת בפועל` ,itemsDescription as `תיאור סט ספציפי`, group_Status as `סטטוס קבוצה` , group_StageOrder as `מספר שלב הקבוצה` ,stageName as `שם שלב הקבוצה`, stage_discription as `תאור שלב הקבוצה`  FROM jobs,item  WHERE jobs.jobid='" + jobID + "' AND jobs.itemid=item.itemid AND jobs.itemStageOrder=item.itemStageOrder AND jobs.itemStatus=item.itemStatus AND jobs.itemid Like '%" + searchkey + "%'  group by jobs.itemid ");
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
                if (MessageBox.Show("?האם אתה בטוח שברצונך למחוק סט פריט זה", "וידוא מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else // if the user clicked on "Yes" so he wants to Delete.
                {
                    string selected = row["מקט פריט"].ToString();
                    int thecountinitemid = 0;

                    // see if this is the last set of items in this jobid
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "select COUNT(itemid) from jobs where jobid='" + jobID + "' GROUP BY itemid";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                        while (dr.Read())
                        {
                            if (!dr.IsDBNull(0))
                            {
                                thecountinitemid = dr.GetInt32(0);
                            }

                        }
                        MySqlConn.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("נפל בשורה מספר 174");
                        MessageBox.Show(ex.Message);
                    }
                    Console.WriteLine("שורה  177 thecountinitemid=" + thecountinitemid);
                    if (thecountinitemid > 1)
                    { // if this is not the last set then delete.
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = "DELETE FROM jobs WHERE jobid='" + jobID + "' and itemid='" + selected + "' ";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();
                            refreashandClear();
                            MessageBox.Show("!סט הפריט נמחק מהעבודה", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                    //  if this is the last set of items in this job then just update the job status to בוטלה
                    else
                    {
                        try
                        {
                            string Query1 = "UPDATE jobs SET job_status='בוטלה' WHERE jobid='" + jobID + "'";
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();
                            MessageBox.Show("בגלל שזהו סט הפריט האחרון בעבודה\n .סט הפריט לא נמחק אך סטטוס העבודה שונה לבוטלה", "!שים לב - שינוי סטטוס עבודה", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                    refreashandClear();

                }//end else if the user clicked on "Yes" so he wants to Delete.

            }//end try
            catch 
            {
                MessageBox.Show("לא נבחר סט פריט למחיקה" , "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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

                if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן סט פריט זה", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;  //dont do stuff
                }

                else // if the user clicked on "Yes" so he wants to Update.
                {
                    string selected_Item = row["מקט פריט"].ToString();
                    string itemdesc = row["תיאור סט ספציפי"].ToString();
                    string exqun = row["כמות נדרשת"].ToString();
                    string status = row["סטטוס קבוצה"].ToString();
                    string cosMAKAT = row["מקט לקוח"].ToString();
                    string currStageOrder = row["מספר שלב הקבוצה"].ToString();
                   // int notzero = 0;
                    try
                    {
                        int expectedq = Convert.ToInt32(exqun);
                    }
                    catch { 
                            MessageBox.Show("שדה הכמות נדרשת לא מכיל רק מספרים" , "!שים לב" , MessageBoxButton.OK, MessageBoxImage.Error);
                            // need to do refreash
                            refreashandClear();
                            return; 
                          }

                    // now we will check if the status of the group was chenged.
                    bool gruopStatusChanged = false;
                    string oldstatus = "", oldStageOrder="";
                    try
                    {
                        Console.WriteLine("שורה 223");
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT group_Status ,group_StageOrder FROM project.jobs WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'LIMIT 1  ");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        Console.WriteLine("שורה 231");
                        while (dr.Read())
                        {
                            if (!dr.IsDBNull(0))
                            {
                                Console.WriteLine("שורה 236");
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
                        Console.WriteLine("שורה 268");
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
//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////
                    else // user changed the group status.
                    {//        if the changed is not allwoed.
                        bool fromFixToWork = false, fromWorkToFix = false , fromFixToFinish=false , fromFinishToFix=false , fromFixToBad=false , badToFix=false;  // badToWork=false ,badToFinish=false ;
                        string group_itemToFixStageOrder="";
                        int no_advence = 0; // if there are items in the group that do not match the new status rules.
                        // now we are going to save the group from the DB to a DataSet.
                        DataSet group = new DataSet();
                        try
                        {
                            Console.WriteLine("שורה 301");
                            MySqlConnection MySqlConn22 = new MySqlConnection(Login.Connectionstring);
                            MySqlConn22.Open();
                            string Query22 = (" SELECT * FROM project.jobs WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' AND inTheGroup='כן' ");
                            MySqlCommand MSQLcrcommand22 = new MySqlCommand(Query22, MySqlConn22);
                            MSQLcrcommand22.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp22 = new MySqlDataAdapter(MSQLcrcommand22);
                            Console.WriteLine("שורה 308");
                            //string itemNumToDB = "";
                            mysqlDAdp22.Fill(group);
                            Console.WriteLine("שורה 311");
                            Console.WriteLine(group);
                            MySqlConn22.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            refreashandClear();
                            return;
                        }
                        
                        int groupsize;
                        groupsize = group.Tables[0].Rows.Count;
                        if (groupsize !=0 )
                        {
                            group_itemToFixStageOrder = group.Tables[0].Rows[0]["group_itemToFixStageOrder"].ToString();
                        }
                       


                        if ( (oldstatus == "פסול") && (groupsize!=0) && (status != "תיקון"))
                        {// if the user wants to change from bad status , the group isn't empty and the new status is NOT fix 
                            MessageBox.Show("!מעבר מסטטוס פסול אפשרי רק אל סטטוס תיקון - עדכון לא אושר", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                            refreashandClear();
                            return;
                        }



                        if ((oldstatus == "גמר ייצור") && (oldStageOrder != "0") && (status == "תיקון"))
                        {
                            if (groupsize!=0)
                            {
                                MessageBox.Show("!מעבר אל שלב התיקון אפשרי בשלב גמר ייצור רק ממספר שלב 0 - עדכון לא אושר", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                refreashandClear();
                                return;
                            }
                        }




                        if ( (oldstatus == "תיקון") && (status != "בעבודה" ) )
                        {
                            if  (status != "גמר ייצור")
                           {
                               if (status != "פסול")
                               {
                                    if  (status != "גמר ייצור")
                                    {
                                       if (groupsize!=0)
                                       {
                                           MessageBox.Show(":מעבר מסטטוס התיקון אפשרי רק אל הסטטוסים הבאים \n גמר ייצור, פסול או  בעבודה.\n העדכון לא אושר", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                           refreashandClear();
                                           return;
                                       }
                                    }
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
                               if (oldstatus != "פסול")
                               {
                                   if (groupsize!=0)
                                   {
                                       MessageBox.Show("עדכון לא אושר \n .מעבר אל סטטוס התיקון אפשרי רק מהסטטוסים פסול, בעבודה או גמר ייצור בשלב מספר 0", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                       refreashandClear();
                                       return;
                                   }
                               }
                           }
                        } 


                        // from all other status except  fix to status bad
                        if (oldstatus != "תיקון" && status == "פסול")
                        {
                            if (groupsize!=0)
                            {
                                MessageBox.Show("!מעבר אל סטטוס פסול אפשרי רק משלב תיקון - עדכון לא אושר", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                refreashandClear();
                                return;
                            }
                        }
                        
                        
                        
                        if (groupsize != 0)
                        { // if the group is NOT empty

    /////////////////////////////
    ////////////////////////////
                            if (oldstatus == "תיקון" && status == "גמר ייצור")
                            {
                                fromFixToFinish = true;
                            
                            } Console.WriteLine("fromFixToFinish= " + fromFixToFinish);




                            // if the new status is work and the old is fix then we need to return to origanel itemStageOrder = itemToFixStageOrder and group_StageOrder = itemStageOrder.
                            if (oldstatus == "תיקון" && status=="בעבודה")
                            {   
                                fromFixToWork = true;
                            
                            }
                            Console.WriteLine("fromFixToWork= " + fromFixToWork);
    //////////////////////////
    //////////////////////////


                            if (oldstatus == "גמר ייצור" && status == "תיקון")
                            {
                                if (oldStageOrder == "0")
                                {
                                
                                    fromFinishToFix = true;
                                }
                            } Console.WriteLine("fromFinishToFix= " + fromFinishToFix);



                            // if the new status is fix and the old is work  then we need to set the itemToFixStageOrder = group_StageOrder itemStageOrder='1'.
                            if (oldstatus == "בעבודה" && status == "תיקון")
                            {
                            
                                fromWorkToFix = true;
                            }
                            Console.WriteLine("fromWorkToFix= " + fromWorkToFix);



                            Console.WriteLine("שורה 454");
                            // to get itemToFixStageOrder .
                            string itemToFixStageOrder = "";
                            try
                            { // we need to get the itemToFixStageOrder of the group.
                                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                MySqlConn.Open();
                                Console.WriteLine("שורה 461");
                                string Query12 = ("SELECT itemToFixStageOrder FROM project.jobs WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' LIMIT 1  ");
                                MySqlCommand MSQLcrcommand11 = new MySqlCommand(Query12, MySqlConn);
                                MSQLcrcommand11.ExecuteNonQuery();
                                MySqlDataReader dr = MSQLcrcommand11.ExecuteReader();
                                MySqlDataAdapter mysqlDAdp1 = new MySqlDataAdapter(MSQLcrcommand11);
                                Console.WriteLine("שורה 467");
                                while (dr.Read())
                                {

                                    itemToFixStageOrder = dr.GetString(0);
                                    Console.WriteLine("שורה 472");


                                }
                                MySqlConn.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                refreashandClear();
                                return; ;
                            }
                            Console.WriteLine("שורה 484");




                            // from status fix to status bad we also need to check if there are items in the group
                            if (oldstatus == "תיקון" && status == "פסול")
                            {
                           
                                fromFixToBad = true;
                            } Console.WriteLine("fromFixToBad= " + fromFixToBad);




                            // from status bad to status fix
                            if (oldstatus == "פסול" && status == "תיקון")
                            {
                                badToFix = true;
                            
                            
                            } Console.WriteLine("badToFix= " + badToFix);


                            //  if (notzero != 0)
                            //  {

                        // now before we update our group of items, if status is "תיקון" or "פסול" (and we did not came from פסול - need to ask about that) then we need to update the "fix" table in the DB with: jobid - itemid - itemnum - itemStageOrder - stageName - fromFixOrBad - itemToFixStageOrder - dateAddedToFixTable.
                            if ( (status == "תיקון" || status == "פסול") && (oldstatus != "פסול") )
                            {
                                //string itemNumToDB = "";
                                try
                                {
                                    Console.WriteLine("שורה 517");
                                    MySqlConnection MySqlConn22 = new MySqlConnection(Login.Connectionstring);
                                    MySqlConn22.Open();
                                    string Query22 = ("SELECT itemNum FROM project.jobs WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' AND inTheGroup='כן'  ");
                                    MySqlCommand MSQLcrcommand22 = new MySqlCommand(Query22, MySqlConn22);
                                    MSQLcrcommand22.ExecuteNonQuery();
                                    //MySqlDataReader dr22 = MSQLcrcommand22.ExecuteReader();
                                    MySqlDataAdapter mysqlDAdp22 = new MySqlDataAdapter(MSQLcrcommand22);
                                    Console.WriteLine("שורה 525");
                                    string itemNumToDB = "";
                                    DataSet itemNumsToDB = new DataSet();
                                    mysqlDAdp22.Fill(itemNumsToDB);
                                    //int size =itemNumsToDB.Tables[0].Rows.Count;
                                    Console.WriteLine("שורה 530");
                                    Console.WriteLine(itemNumsToDB);
                                    MySqlConn22.Close();
                                    foreach (DataRow itemnumrow in itemNumsToDB.Tables[0].Rows)
                                    {
                                        Console.WriteLine("שורה 535");
                                        try
                                        {
                                            MySqlConnection MySqlConn33 = new MySqlConnection(Login.Connectionstring);
                                            MySqlConn33.Open();
                                            itemNumToDB = itemnumrow["itemNum"].ToString();
                                            Console.WriteLine(itemNumToDB);
                                            string Query33 = ("INSERT INTO project.fix (jobid, itemid,itemNum, itemStageOrder,stageStatusName, fromFixOrBad , itemToFixStageOrder , dateAddedToFixTable) VALUES ('" + jobID + "','" + selected_Item + "','" + itemNumToDB + "','" + oldStageOrder + "','" + oldstatus + "','" + status + "','" + itemToFixStageOrder + "', '" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "')");
                                            Console.WriteLine("השאילתא הנשלחת לפיקס  - " + Query33 + "");
                                            MySqlCommand MSQLcrcommand33 = new MySqlCommand(Query33, MySqlConn33);
                                            MSQLcrcommand33.ExecuteNonQuery();
                                            MySqlDataAdapter mysqlDAdp33 = new MySqlDataAdapter(MSQLcrcommand33);
                                            MySqlConn33.Close();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                            refreashandClear();
                                            return;
                                        }
                                    } // end of foreach (DataRow itemnumrow in itemNumsToDB.Tables[0].Rows)

                                } // end of try
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                    return; ;
                                }

                            }// end of if (status == "תיקון" || status == "פסול")
                            // finish updateing the fix table in the DB



                            // now we need to update only the group to the the user's selected status.
                                
                            string Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='1' WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' AND inTheGroup='כן' ";
                            Console.WriteLine("השאילתא הכללית: שורה 572");
                            Console.WriteLine(Query1);
                            
                            bool group_Updated_from_fix = false; // if the update was already done.
                            string itemToFixStageOrderFromDB , itemNumToDB1="";
                            // group_itemToFixStageOrder = group.Tables[0].Rows[0]["group_itemToFixStageOrder"].ToString();
                            if (fromFixToWork == true)
                            {
                                foreach (DataRow group_row in group.Tables[0].Rows)
                                {
                                    //group_itemToFixStageOrder = group_row["group_itemToFixStageOrder"].ToString();
                                    itemToFixStageOrderFromDB = group_row["itemToFixStageOrder"].ToString();
                                    itemNumToDB1 = group_row["itemNum"].ToString();
                                    if (itemToFixStageOrderFromDB != "0")
                                    {
                                        Console.WriteLine("שורה 587");
                                        try
                                        {
                                            Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='" + itemToFixStageOrderFromDB + "', itemToFixStageOrder='תוקן בעבר מבעבודה' WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemNum='" + itemNumToDB1 + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' AND inTheGroup='כן' ";
                                            Console.WriteLine("שורה 591");
                                            Console.WriteLine(Query1);
                                            MySqlConnection MySqlConn1 = new MySqlConnection(Login.Connectionstring);
                                            MySqlConn1.Open();
                                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn1);
                                            MSQLcrcommand1.ExecuteNonQuery();
                                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                                            MySqlConn1.Close();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                            refreashandClear();
                                            return;
                                        }
                                    } // end if (itemToFixStageOrderFromDB != "0")

                                    else
                                    {
                                        no_advence++;
                                    }
                                    
                                } // end of foreach (DataRow itemnumrow in itemNumsToDB.Tables[0].Rows)
                                if (no_advence == group.Tables[0].Rows.Count)
                                {
                                    MessageBox.Show("כל הקבוצה נכנסה לתיקון מסטטוס גמר ייצור שלב 0\n !עדכון לא אושר", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                    refreashandClear();
                                    return;
                                }
                                group_Updated_from_fix = true;   
                            }



                            if (fromFixToFinish == true)
                            {
                                foreach (DataRow group_row in group.Tables[0].Rows)
                                {
                                    itemToFixStageOrderFromDB = group_row["itemToFixStageOrder"].ToString();
                                    itemNumToDB1 = group_row["itemNum"].ToString();
                                    if (itemToFixStageOrderFromDB == "0")
                                    {
                                        Console.WriteLine("שורה 633");
                                        try
                                        {
                                            Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='" + itemToFixStageOrderFromDB + "', itemToFixStageOrder='תוקן בעבר מגמר ייצור' , group_itemToFixStageOrder='הקבוצה תוקנה בעבר מגמר ייצור'  WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemNum='" + itemNumToDB1 + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' AND inTheGroup='כן' ";
                                            Console.WriteLine("שורה 637");
                                            Console.WriteLine(Query1);
                                            MySqlConnection MySqlConn1 = new MySqlConnection(Login.Connectionstring);
                                            MySqlConn1.Open();
                                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn1);
                                            MSQLcrcommand1.ExecuteNonQuery();
                                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                                            MySqlConn1.Close();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                            refreashandClear();
                                            return;
                                        }
                                    } // end if (itemToFixStageOrderFromDB != "0")

                                    else
                                    {
                                        no_advence++;
                                    }

                                } // end of foreach (DataRow itemnumrow in itemNumsToDB.Tables[0].Rows)
                                if (no_advence == group.Tables[0].Rows.Count)
                                {
                                    MessageBox.Show("כל הקבוצה נכנסה לתיקון מסטטוס בעבודה\n !עדכון לא אושר", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                    refreashandClear();
                                    return;
                                }
                                group_Updated_from_fix = true; 
                            }


                            if (fromFinishToFix == true)
                            {
                                Console.WriteLine("שורה 672 מייצור לתיקון - שלב נוכחי שיכנס למספר שלב לתיקון:");
                                Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='1' , itemToFixStageOrder='0' , group_StageOrder='1' , group_itemToFixStageOrder='0' WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' AND inTheGroup='כן' ";
                                Console.WriteLine("currStageOrder= " + currStageOrder);
                            }
                            if (fromWorkToFix == true)
                            {
                                Console.WriteLine("שורה 678 מעבודה לתיקון - שלב נוכחי שיכנס למספר שלב לתיקון:");
                                Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='1' , itemToFixStageOrder='" + oldStageOrder + "' , group_itemToFixStageOrder='" + oldStageOrder + "' WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' AND inTheGroup='כן' ";
                                Console.WriteLine("oldStageOrder= " + oldStageOrder);
                            }

                            if (fromFixToFinish == false && status == "גמר ייצור")
                            {
                                Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='0' ,  group_StageOrder='0' , itemToFixStageOrder='" + itemToFixStageOrder + "' WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' AND inTheGroup='כן' ";
                            }


                            if (fromFixToBad == true)
                            {
                                Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='1' , itemToFixStageOrder='" + itemToFixStageOrder + "' WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' AND inTheGroup='כן' ";
                            }
/*
                            if (badToWork == true)
                            {
                                Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='" + itemToFixStageOrder + "' , itemToFixStageOrder='חזר מפסילה בשלב מספר - " + itemToFixStageOrder + "' WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' AND inTheGroup='כן' ";
                            }
*/
                 /*           if (badToFix == true)
                            {
                                // Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='1' , itemToFixStageOrder='" + itemToFixStageOrder + "' WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' AND inTheGroup='כן' ";
                                Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='1'  WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' AND inTheGroup='כן' ";
                            }*/
/*
                            if (badToFinish == true)
                            {
                                Query1 = "UPDATE jobs SET itemStatus='" + status + "' , itemStageOrder='0' , itemToFixStageOrder='חזר מפסילה בסטטוס גמר ייצור בשלב 0'  WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' AND itemStatus='" + oldstatus + "' AND itemStageOrder='" + oldStageOrder + "' AND inTheGroup='כן' ";
                            }
*/

                        // now we will update our group if we did not update it before.
                            if(group_Updated_from_fix==false)
                            {
                                try
                                {
                                    Console.WriteLine("שורה 716");
                                    Console.WriteLine(Query1);
                                    MySqlConnection MySqlConn1 = new MySqlConnection(Login.Connectionstring);
                                    MySqlConn1.Open();
                                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn1);
                                    MSQLcrcommand1.ExecuteNonQuery();
                                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                                    MySqlConn1.Close();
                                    // we updated only the group.
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                    refreashandClear();
                                    return;

                                }
                                //}
                                Console.WriteLine("שורה 734");
                            }
                        } // end of if the group is NOT empty
                            
                        else // else of if(groupsize != 0)
                        {
                            try
                            { // if the group was empty we need to get the group_itemToFixStageOrder of the group from one of the items.
                                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                MySqlConn.Open();
                                Console.WriteLine("שורה 744");
                                string Query12 = ("SELECT group_itemToFixStageOrder FROM project.jobs WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "' LIMIT 1  ");
                                MySqlCommand MSQLcrcommand11 = new MySqlCommand(Query12, MySqlConn);
                                MSQLcrcommand11.ExecuteNonQuery();
                                MySqlDataReader dr = MSQLcrcommand11.ExecuteReader();
                                MySqlDataAdapter mysqlDAdp1 = new MySqlDataAdapter(MSQLcrcommand11);
                                Console.WriteLine("שורה 750");
                                while (dr.Read())
                                {

                                    group_itemToFixStageOrder = dr.GetString(0);
                                    Console.WriteLine("שורה 755");


                                }
                                MySqlConn.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                refreashandClear();
                                return; ;
                            }
                        }

/////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////


                        // we finished to update the group (if there was a group) now we need to do the update to all of the itemid for this jobid with the new status.
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='1'   WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                            //if (fromFixToFinish == false && status == "גמר ייצור")

                            if (status == "גמר ייצור")
                            {Console.WriteLine("שורה 782");
                                   Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='0'   WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                            }

                            if (fromFixToWork)
                            {
                                Console.WriteLine("שורה 788");
                                   Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='" + group_itemToFixStageOrder + "'  , group_itemToFixStageOrder='הקבוצה תוקנה בעבר מבעבודה'  WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                                   if (group_itemToFixStageOrder == "0")
                                   {
                                       Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='1'  , group_itemToFixStageOrder='הקבוצה תוקנה בעבר מבעבודה'  WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                                   }
                            }
                            if (fromFixToFinish == true && status == "גמר ייצור")
                            {
                                   Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='0' , group_itemToFixStageOrder='הקבוצה תוקנה בעבר מגמר ייצור'   WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                            }

                            if (fromFinishToFix == true)
                            {
                                      Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='1' , group_itemToFixStageOrder='0'   WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                            }

                            if (fromWorkToFix == true)
                            {
                              
                                      Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='1' , group_itemToFixStageOrder='" + oldStageOrder + "'   WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                            }
                            Console.WriteLine("שורה 810");
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            Console.WriteLine("שורה 819");
                            refreashandClear();
                            return;
                        }
                        refreashandClear();
                        if (no_advence != 0)
                        {
                            MessageBox.Show("!סט הפריט עודכן\nאך היו " + no_advence + " פריטים שלא תאמו לשינוי הסטטוס ולא השתנו", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information); 
                        }

                        if ( (groupsize == 0) && (gruopStatusChanged == true) )
                        {
                            MessageBox.Show("!סט הפריט עודכן\nאך לא היו פריטים בקבוצה ", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        else
                        {
                            MessageBox.Show("!סט הפריט עודכן", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        Console.WriteLine("838 583"); 

                    } // end else - user changed the group status.

                } // end else - if the user clicked on "Yes" so he wants to Update.

            }// end try.
            catch 
            {
                MessageBox.Show("!לא נבחר סט פריט לעדכון", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                //MessageBox.Show("לא נבחר סט פריט לעדכון ");
            }
        }



        private void NextStage_button_Click(object sender, RoutedEventArgs e)
        {

            string next_stage = "";
            string group_Status = "";
            string next_status = "";
            string group_itemToFixStageOrder = "";
            bool last = false;
            string Query1 = "";
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                if (MessageBox.Show("?האם אתה בטוח שברצונך לקדם קבוצת פריט זה", "וידוא קידום", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return; //do no stuff
                }
                else // if the user clicked on "Yes" 
                {

                    string curr = row["מספר שלב הקבוצה"].ToString();
                    string allitemid = row["מקט פריט"].ToString();


                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        Query1 = "select group_Status , group_itemToFixStageOrder from jobs where itemid='" + allitemid + "' and   jobid='" + jobID + "' LIMIT 1";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();

                        while (dr.Read())
                        {
                            if (!dr.IsDBNull(0))
                            {
                                group_Status = dr.GetString(0);
                                group_itemToFixStageOrder = dr.GetString(1);
                            }

                        }

                        MySqlConn.Close();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("נפל בשורה מספר 1049");
                        MessageBox.Show(ex.Message);
                    }


                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        Query1 = "select MIN(itemStageOrder) from item where itemid='" + allitemid + "' and   itemStageOrder>'" + curr + "' and itemStatus= '" + group_Status + "'     ";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                        while (dr.Read())
                        {
                            if (!dr.IsDBNull(0))
                            {
                                next_stage = dr.GetString(0);
                                next_status = group_Status;
                            }
                            else
                            {
                                last = true;
                                if (group_Status == "רישום")
                                {
                                    next_status = "בעבודה";
                                    next_stage = "1";
                                }
                                if (group_Status == "בעבודה")
                                {
                                    next_status = "גמר ייצור";
                                    next_stage = "0";

                                }
                                if (group_Status == "גמר ייצור")
                                {
                                    next_status = "הסתיים";
                                    next_stage = "1";

                                }
                                if (group_Status == "תיקון")
                                {
                                    MessageBox.Show(".עבור קבוצת פריט בסטטוס תיקון עליך לשנות סטטוס ידנית", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                    MySqlConn.Close();
                                    refreashandClear();
                                    return;
                                    /*
                                    if (group_itemToFixStageOrder != "0")
                                    {
                                        next_status = "בעבודה";
                                        next_stage = group_itemToFixStageOrder;
                                    }
                                    if (group_itemToFixStageOrder == "0")
                                    {
                                        next_status = "גמר ייצור";
                                        next_stage = group_itemToFixStageOrder;
                                    }
                                     * */
                                }

                                if (group_Status == "הסתיים")
                                {
                                    MessageBox.Show(".כבר בוצעו כל השלבים הקיימים עבור קבוצת פריט זה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    MySqlConn.Close();
                                    refreashandClear();
                                    return;
                                }

                                if (group_Status == "פסול")
                                {
                                    MessageBox.Show(".עבור קבוצת פריט בסטטוס פסול עליך לשנות סטטוס ידנית", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                    MySqlConn.Close();
                                    refreashandClear();
                                    return;

                                }

                            } // end else of if (!dr.IsDBNull(0))
                        } // end while (dr.Read())

                        MySqlConn.Close();

                    } // end try
                    catch (Exception ex)
                    {
                        Console.WriteLine("נפל בשורה מספר 643");
                        MessageBox.Show(ex.Message);
                    }

                    // now we will see if there is a group.
                    DataSet group = new DataSet();
                    try
                    {
                        Console.WriteLine("שורה 301");
                        MySqlConnection MySqlConn22 = new MySqlConnection(Login.Connectionstring);
                        MySqlConn22.Open();
                        string Query22 = (" SELECT * FROM project.jobs WHERE jobid='" + jobID + "' AND itemid='" + allitemid + "' AND itemStatus='" + group_Status + "' AND itemStageOrder='" + curr + "' AND inTheGroup='כן' ");
                        MySqlCommand MSQLcrcommand22 = new MySqlCommand(Query22, MySqlConn22);
                        MSQLcrcommand22.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp22 = new MySqlDataAdapter(MSQLcrcommand22);
                        Console.WriteLine("שורה 308");
                        //string itemNumToDB = "";
                        mysqlDAdp22.Fill(group);
                        Console.WriteLine("שורה 311");
                        Console.WriteLine(group);
                        MySqlConn22.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        refreashandClear();
                        return;
                    }

                    int groupsize;
                    groupsize = group.Tables[0].Rows.Count;


                    // now update the group and only the ones who said YES.
               //     if (last == true)
                //    {
                    if (groupsize > 0)
                    {
                        Query1 = "update jobs set itemStatus='" + next_status + "' , itemStageOrder='" + next_stage + "' where jobid='" + jobID + "' and itemid='" + allitemid + "' AND itemStatus='" + group_Status + "' AND inTheGroup='כן' ";
                        //    }
                        //    else
                        //    {
                        //        Query1 = "update jobs set itemStageOrder='" + next_stage + "' where jobid='" + jobID + "'  and itemid='" + allitemid + "' AND itemStatus='" + group_Status + "' AND inTheGroup='כן' ";
                        //    }
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            //string Query1 = "update jobs set itemStageOrder='" + next_stage + "' where jobid='" + jobID + "' and itemid='" + itemID + "'and itemNum='" + itemnum + "'";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            Console.WriteLine("נפל בשורה מספר 670");
                            return;
                        }
                    }


                    // now update all of this itemid to the new group status.
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();

                        Query1 = "UPDATE jobs SET group_Status='" + next_status + "' , group_StageOrder='" + next_stage + "'   WHERE jobid='" + jobID + "' AND itemid='" + allitemid + "'";
                        //if (fromFixToFinish == false && status == "גמר ייצור")
/*
                        if (status == "גמר ייצור")
                        {
                            Console.WriteLine("שורה 782");
                            Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='0'   WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                        }

                        if (fromFixToWork)
                        {
                            Console.WriteLine("שורה 788");
                            Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='" + group_itemToFixStageOrder + "'  , group_itemToFixStageOrder='הקבוצה תוקנה בעבר מבעבודה'  WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                            if (group_itemToFixStageOrder == "0")
                            {
                                Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='1'  , group_itemToFixStageOrder='הקבוצה תוקנה בעבר מבעבודה'  WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                            }
                        }
                        if (fromFixToFinish == true && status == "גמר ייצור")
                        {
                            Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='0' , group_itemToFixStageOrder='הקבוצה תוקנה בעבר מגמר ייצור'   WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                        }

                        if (fromFinishToFix == true)
                        {
                            Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='1' , group_itemToFixStageOrder='0'   WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                        }

                        if (fromWorkToFix == true)
                        {

                            Query1 = "UPDATE jobs SET itemsDescription='" + itemdesc + "',expectedItemQuantity='" + exqun + "' ,group_costomer_itemid='" + cosMAKAT + "',group_Status='" + status + "' , group_StageOrder='1' , group_itemToFixStageOrder='" + oldStageOrder + "'   WHERE jobid='" + jobID + "' AND itemid='" + selected_Item + "'";
                        }
 */
                        Console.WriteLine("שורה 810");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Console.WriteLine("שורה 819");
                        refreashandClear();
                        return;
                    }
                    refreashandClear();



                    if (last == true)
                    {
                        if (groupsize > 0)
                        {
                            MessageBox.Show(".הקבוצה קודמה לסטטוס הבא", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                            refreashandClear();
                            return;
                        }
                        else
                        {
                            MessageBox.Show(".הקבוצה קודמה לסטטוס הבא\n       .אך !שים לב לכך שהקבוצה הייתה ריקה", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                            refreashandClear();
                            return;
                        }
                    }
                    else
                    {
                        if (groupsize > 0)
                        {
                            MessageBox.Show(".הקבוצה קודמה לשלב הבא", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                            refreashandClear();
                        }
                        else
                        {
                            MessageBox.Show(".הקבוצה קודמה לשלב הבא\n       .אך !שים לב לכך שהקבוצה הייתה ריקה", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                            refreashandClear();
                        }
                    }

                } // end of else // if the user clicked on "Yes" so he wants to Delete.

            }
            catch
            {
                MessageBox.Show(".לא נבחרה קבוצה לקדם", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                refreashandClear();
                return;
            }
        }

        private void PrevStage_button_Click(object sender, RoutedEventArgs e)
        {
            string prev_stage = "";
            string group_Status = "";
            string prev_status = "";
            string group_itemToFixStageOrder = "";
            string workLast = "";
            bool first = false;
            string Query1 = "";
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                if (MessageBox.Show("?האם אתה בטוח שברצונך להחזיר קבוצת פריט זה", "וידוא קידום", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return; //do no stuff
                }
                else // if the user clicked on "Yes" so he wants to Delete.
                {

                    string curr = row["מספר שלב הקבוצה"].ToString();
                    string allitemid = row["מקט פריט"].ToString();


                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        Query1 = "select group_Status , group_itemToFixStageOrder from jobs where itemid='" + allitemid + "' and   jobid='" + jobID + "' LIMIT 1";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();

                        while (dr.Read())
                        {
                            if (!dr.IsDBNull(0))
                            {
                                group_Status = dr.GetString(0);
                                group_itemToFixStageOrder = dr.GetString(1);
                            }

                        }

                        MySqlConn.Close();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("נפל בשורה מספר 1049");
                        MessageBox.Show(ex.Message);
                    }


                    if (group_Status == "גמר ייצור")
                    {// get the last stage of Work status.
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            Query1 = "select MAX(itemStageOrder) from item where itemid='" + allitemid + "' and itemStatus= 'בעבודה' ";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();

                            while (dr.Read())
                            {
                                if (!dr.IsDBNull(0))
                                {
                                    workLast = dr.GetString(0);
                                }

                            }

                            MySqlConn.Close();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("נפל בשורה מספר 669");
                            MessageBox.Show(ex.Message);
                        }
                    }

                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        Query1 = "select MAX(itemStageOrder) from item where itemid='" + allitemid + "' and   itemStageOrder<'" + curr + "' and itemStatus= '" + group_Status + "'     ";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                        while (dr.Read())
                        {
                            if (!dr.IsDBNull(0))
                            {
                                prev_stage = dr.GetString(0);
                                prev_status = group_Status;
                            }
                            else
                            {
                                first = true;
                                if (group_Status == "רישום")
                                {
                                    MessageBox.Show(".הפריט כבר נמצא בשלב הראשון הקיים", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    MySqlConn.Close();
                                    refreashandClear();
                                    return;
                                }

                                if (group_Status == "תיקון")
                                {
                                    MessageBox.Show(".עבור קבוצת פריט בסטטוס תיקון עליך לשנות סטטוס ידנית", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                    MySqlConn.Close();
                                    refreashandClear();
                                    return;
                                }


                                if (group_Status == "פסול")
                                {
                                    MessageBox.Show(".עבור פריט בסטטוס פסול עליך לשנות סטטוס ידנית", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                    MySqlConn.Close();
                                    refreashandClear();
                                    return;
                                }

                                if (group_Status == "בעבודה")
                                {
                                    prev_status = "רישום";
                                    prev_stage = "2";

                                }

                                if (group_Status == "גמר ייצור")
                                {
                                    prev_status = "בעבודה";
                                    prev_stage = workLast;

                                }


                                if (group_Status == "הסתיים")
                                {
                                    prev_status = "גמר ייצור";
                                    prev_stage = "1";

                                }   

                            } // end else of if (!dr.IsDBNull(0))
                        } // end while (dr.Read())

                        MySqlConn.Close();

                    } // end try
                    catch (Exception ex)
                    {
                        Console.WriteLine("נפל בשורה מספר 744");
                        MessageBox.Show(ex.Message);
                    }


                    // now we will see if there is a group.
                    DataSet group = new DataSet();
                    try
                    {
                        Console.WriteLine("שורה 301");
                        MySqlConnection MySqlConn22 = new MySqlConnection(Login.Connectionstring);
                        MySqlConn22.Open();
                        string Query22 = (" SELECT * FROM project.jobs WHERE jobid='" + jobID + "' AND itemid='" + allitemid + "' AND itemStatus='" + group_Status + "' AND itemStageOrder='" + curr + "' AND inTheGroup='כן' ");
                        MySqlCommand MSQLcrcommand22 = new MySqlCommand(Query22, MySqlConn22);
                        MSQLcrcommand22.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp22 = new MySqlDataAdapter(MSQLcrcommand22);
                        Console.WriteLine("שורה 308");
                        //string itemNumToDB = "";
                        mysqlDAdp22.Fill(group);
                        Console.WriteLine("שורה 311");
                        Console.WriteLine(group);
                        MySqlConn22.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        refreashandClear();
                        return;
                    }

                    int groupsize;
                    groupsize = group.Tables[0].Rows.Count;


                    // update the group if there is a group.
                    if (groupsize > 0)
                    {
                        Query1 = "update jobs set itemStatus='" + prev_status + "' , itemStageOrder='" + prev_stage + "' where jobid='" + jobID + "' and itemid='" + allitemid + "' AND itemStatus='" + group_Status + "' AND inTheGroup='כן' ";

                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            Console.WriteLine("נפל בשורה מספר 458");
                            return;
                        }
                    }

                    // update all the items in the set to hold the new  group_Status and group_StageOrder.
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        Query1 = "UPDATE jobs SET group_Status='" + prev_status + "' , group_StageOrder='" + prev_stage + "'   WHERE jobid='" + jobID + "' AND itemid='" + allitemid + "'";
                        Console.WriteLine("שורה 810");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Console.WriteLine("שורה 819");
                        refreashandClear();
                        return;
                    }

                    if (first == true)
                    {
                        if (groupsize > 0)
                        {
                            MessageBox.Show(".הקבוצה הוחזרה לסטטוס הקודם", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                            refreashandClear();
                            return;
                        }
                        else
                        {
                            MessageBox.Show(".הקבוצה הוחזרה לסטטוס הקודם\n       .אך !שים לב לכך שהקבוצה הייתה ריקה", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                            refreashandClear();
                            return;
                        }
                    }
                    else
                    {
                        if (groupsize > 0)
                        {
                            MessageBox.Show(".הקבוצה הוחזרה לשלב הקודם", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                            refreashandClear();
                        }
                        else
                        {
                            MessageBox.Show(".הקבוצה הוחזרה לשלב הקודם\n       .אך !שים לב לכך שהקבוצה הייתה ריקה", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                            refreashandClear();
                        }
                    }
                   
                } // end of else // if the user clicked on "Yes" so he wants to Delete.

            }
            catch
            {
                MessageBox.Show(".לא נבחרה קבוצה להחזרה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                refreashandClear();
                return;
            }
        
        }





        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מקט פריט" || e.Column.Header.ToString() == "שם פריט" || e.Column.Header.ToString() == "כמות כוללת בפועל" || e.Column.Header.ToString() == "מספר שלב הקבוצה" || e.Column.Header.ToString() == "שם שלב הקבוצה" || e.Column.Header.ToString() == "תאור שלב הקבוצה" || e.Column.Header.ToString() == "תיאור פריט")
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
             catch { MessageBox.Show("לא נבחר סט פריט", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); }
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
                MessageBox.Show("לא נבחר סט פריט", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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