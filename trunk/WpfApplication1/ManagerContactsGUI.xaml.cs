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

namespace project
{
    /// <summary>
    /// Interaction logic for ManagerContactsGUI.xaml
    /// </summary>
    public partial class ManagerContactsGUI : Window
    {
        string hpcostid;
        string cosName;
        string cosADDs;
        string cos_insideNum;
        public static DataTable dt = new DataTable("contacts");
        public ManagerContactsGUI(string hpcostid, string cos_insideNum, string cosName, string cosADDs)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.hpcostid = hpcostid;
            this.cosName = cosName;
            this.cosADDs = cosADDs;
            this.cos_insideNum = cos_insideNum;
            conHP_label.Content = hpcostid;
            cos_insideNum_label.Content = cos_insideNum;
            con_name_label.Content = cosName;
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select contactid as `מספר איש קשר`,contactName as `שם איש קשר` ,contactEmail as `אימייל איש קשר` ,contactPhone as `טלפון איש קשר`,contactCellPhone as `טלפון נייד של איש הקשר` ,contactDepartment as `מחלקת איש קשר`, contactDesc as `הערות לגבי איש הקשר` from costumers  where costumerid='" + hpcostid + "'");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt = new DataTable("contacts");
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


        private void refreashandclear()
                {
                try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("select contactid as `מספר איש קשר`,contactName as `שם איש קשר` ,contactEmail as `אימייל איש קשר` ,contactPhone as `טלפון איש קשר`,contactCellPhone as `טלפון נייד של איש הקשר` ,contactDepartment as `מחלקת איש קשר`, contactDesc as `הערות לגבי איש הקשר` from costumers  where costumerid='" + hpcostid + "'");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        //DataTable dt = new DataTable("contacts");
                        dt.Clear();
                        mysqlDAdp.Fill(dt);
                        dataGrid1.ItemsSource = dt.DefaultView;
                        mysqlDAdp.Update(dt);
                        MySqlConn.Close();
                        IDSearchTextBox.Clear();
                        FirstNameSearchTextBox.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }



        private void ExcelBtn_Click(object sender, RoutedEventArgs e)
        {

            ExportToExcel();
        }





        private void ExportToExcel()
        {
            try
            {
                /*
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select contactid as `מספר איש קשר`,contactName as `שם איש קשר` ,contactEmail as `אימייל איש קשר` ,contactPhone as `טלפון איש קשר` ,contactDepartment as `מחלקת איש קשר` from costumers  where costumerid='" + hpcostid + "'");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
               // DataTable dt = new DataTable("contacts");
                dt.Clear();
                mysqlDAdp.Fill(dt);
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
                */
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "רשימת אנשי הקשר של " +cosName+ "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString(); ; // Default file name
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

        
            /*SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "רשימת לקוחות"; // Default file name
            dialog.DefaultExt = ".text"; // Default file extension
            dialog.Filter = "Text documents (.txt)|*.txt";  //EXcel documents (.xlsx)|*.xlsx";    // Filter files by extension 

            // Show save file dialog box
            Nullable<bool> result = dialog.ShowDialog();

            // Process save file dialog box results 
            if (result == true)
            {
                dataGrid1.SelectAllCells();
                dataGrid1.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, dataGrid1);
                String result1 = (string)Clipboard.GetData(DataFormats.Text);
                dataGrid1.UnselectAllCells();
                string saveto = dialog.FileName;
                System.IO.StreamWriter file = new System.IO.StreamWriter(@saveto, false, Encoding.Default);

                file.WriteLine(result1.Replace("‘,’", "‘ ‘"));
                file.Close();
                file.Dispose();
                // Save document 
                MessageBox.Show("                                                                             !קובץ הטקסט נשמר\n\n           :כדי לפתוח באקסל מומלץ להשתמש ב''פתיחה באמצעות'' ולבחור ב\n\n                                                 ''Microsoft Excel''");
            }*/
        }


    



        private void FirstNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchkey = this.FirstNameSearchTextBox.Text;
                string Query1 = ("select contactid as `מספר איש קשר`,contactName as `שם איש קשר` ,contactEmail as `אימייל איש קשר` ,contactPhone as `טלפון איש קשר` ,contactDepartment as `מחלקת איש קשר` from costumers where contactName Like '%" + searchkey + "%' and costumerid='" + hpcostid + "'");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
               // DataTable dt = new DataTable("contacts");
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
              //  string Connectionstring = " Server=localhost;Database=project; UId=root;Password=1234;";
              //  MySqlConnection MySqlConn = new MySqlConnection(Connectionstring);
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchidkey = this.IDSearchTextBox.Text;
                string Query1 = ("select contactid as `מספר איש קשר`,contactName as `שם איש קשר` ,contactEmail as `אימייל איש קשר` ,contactPhone as `טלפון איש קשר` ,contactDepartment as `מחלקת איש קשר` from costumers where contactid Like '%" + searchidkey + "%'  and costumerid='" + hpcostid + "'");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
               // DataTable dt = new DataTable("contacts");
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
            ManagerAddContactsGUI MACG = new ManagerAddContactsGUI(hpcostid , cos_insideNum , cosName, cosADDs);
            MACG.Owner = this;
            MACG.ShowDialog();
            //Login.close = 1;
           // this.Close();
        }




        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {   
                try
                {
                    System.Collections.IList rows = dataGrid1.SelectedItems;
                    DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                    if (MessageBox.Show("?האם אתה בטוח שברצונך למחוק איש קשר זה", "וידוא מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        //do no stuff
                    }
                    else // if the user clicked on "Yes" so he wants to Delete.
                    {
                        int con=0;
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string query1 = ("select COUNT(contactid) from costumers where costumerid='" + hpcostid + "'");
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                            while (dr.Read())
                            {

                                con = dr.GetInt32(0);

                            }
                            MySqlConn.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }
                        if (con==1)
                        {
                            if (MessageBox.Show(".זהו איש הקשר היחיד עבור לקוח זה\n  .מחיקה שלו תמחק את גם הלקוח\n  ?האם אתה רוצה למחוק בכל זאת", "וידוא מחיקת איש קשר יחיד", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                            {
                                return;
                            }
                        }
                           
                        // this will give us the first colum of the selected row in the DataGrid.
                        string selected = row["מספר איש קשר"].ToString();

                        // MessageBox.Show("" + selected + "");
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = "delete from costumers  where costumerid='" + hpcostid + "' and contactid='" + selected + "'";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();
                            MessageBox.Show("!איש הקשר נמחק מהמערכת", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                            if (con == 1)
                            {
                                ManagerCusGui MCG = new ManagerCusGui();
                                MCG.Show();
                                Login.close = 1;
                                this.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        refreashandclear();
                        
                    }//end else
                
            }//end try
                catch { MessageBox.Show("לא נבחר איש קשר למחיקה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); }
            
        }//end function







        // go to previous screen.
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerCusGui MCG = new ManagerCusGui();
            MCG.Show();
            Login.close = 1;
            this.Close();
        }






        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["מספר איש קשר"].ToString();
                // MessageBox.Show(""+selected+ "");
                    if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן איש קשר זה", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        return; //dont do stuff
                    }
                    else // if the user clicked on "Yes" so he wants to Update.
                    {
                        string contactEmail ;
                        string contactcell ;
                        string contactPhone ;
                        string contactName ;
                        string contactDepartment ;
                        string contactdesc ;

                        if ( !string.IsNullOrWhiteSpace(row["אימייל איש קשר"].ToString()) )
                        {
                            //checking if the email intered correctlly.
                            if ((Regex.IsMatch(row["אימייל איש קשר"].ToString(), @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")))
                            {
                                 contactEmail = row["אימייל איש קשר"].ToString(); 
                            }
                            else 
                            {
                                MessageBox.Show("כתובת האימייל שהזנת לא תקינה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                refreashandclear();
                                return;
                            }
                        }
                        else
                        {
                             contactEmail = "לא הוזן"; 
                        }

                        if (string.IsNullOrWhiteSpace(row["טלפון נייד של איש הקשר"].ToString()) && string.IsNullOrWhiteSpace(row["טלפון איש קשר"].ToString()))
                        {
                            MessageBox.Show("לפחות אחד מ- טלפון או טלפון נייד חייב להיות מוזן", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                            refreashandclear();
                            return;
                        }

                        if (!string.IsNullOrWhiteSpace(row["טלפון נייד של איש הקשר"].ToString()))
                        {
                            try
                            {
                                int cellphoneCheck = Convert.ToInt32(row["טלפון נייד של איש הקשר"].ToString());
                            }
                            catch 
                            {
                                MessageBox.Show("טלפון נייד חייב לכלול מספרים בלבד", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); 
                                refreashandclear(); 
                                return; 
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(row["טלפון איש קשר"].ToString()))
                        {
                            try
                            {
                                int cellphoneCheck = Convert.ToInt32(row["טלפון איש קשר"].ToString());
                            }
                            catch
                            {
                                MessageBox.Show("מספר טלפון חייב לכלול מספרים בלבד", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                refreashandclear();
                                return;
                            }
                        }
                        if (string.IsNullOrWhiteSpace(row["מחלקת איש קשר"].ToString()))
                        {
                            contactDepartment="לא הוזן";
                        }
                        else
                        {
                            contactDepartment = row["מחלקת איש קשר"].ToString();
                        }

                        contactcell = row["טלפון נייד של איש הקשר"].ToString();
                        contactPhone = row["טלפון איש קשר"].ToString();
                        contactName = row["שם איש קשר"].ToString();   
                        contactdesc = row["הערות לגבי איש הקשר"].ToString();
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = "UPDATE costumers SET contactName='" + contactName + "',contactEmail='" + contactEmail + "',contactPhone='" + contactPhone + "',contactDepartment='" + contactDepartment + "',contactCellPhone='" + contactcell + "',contactDesc='" + contactdesc + "' WHERE costumerid='" + hpcostid + "' and contactid='" + selected + "'";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();
                            MessageBox.Show("!פרטי איש הקשר עודכנו", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        refreashandclear();

                    } // end else // if the user clicked on "Yes" so he wants to Update.
   
            }
            catch { MessageBox.Show("לא נבחר איש קשר לעדכון", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); }
        }



        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מספר איש קשר")
            {
                // e.Cancel = true;   // For not to include 
                 e.Column.IsReadOnly = true; // Makes the column as read only
            }

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