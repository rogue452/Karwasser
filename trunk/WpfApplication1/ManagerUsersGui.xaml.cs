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
    /// Interaction logic for UsersGui.xaml
    /// </summary>
    public partial class ManagerUsersGui : Window
    {
                public ManagerUsersGui()
                {
                    InitializeComponent();
                    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                 //   ListCollectionView CollectionViewList;
                  //  DataTable DataTableFiltered;
                 //   DataSet ds = new DataSet();

                    try
                    {   
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("select users.empid as `תעודת זהות`,employees.emp_firstname as `שם פרטי` ,employees.emp_lastname as `שם משפחה` ,users.user_name as `שם משתמש` ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as `כתובת אימייל` from project.users , project.employees where users.empid=employees.empid");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        DataTable dt = new DataTable("users");
                        mysqlDAdp.Fill(dt);
                        dataGrid1.ItemsSource = dt.DefaultView; 
                        mysqlDAdp.Update(dt);
                        MySqlConn.Close();








                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }



                /*
                    DBConnection conn = new DBConnection();
                    string query = ("select userid as מספר__משתמש ,first_name as שם__פרטי ,last_name as שם__משפחה ,user_name as שם__משתמש ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as כתובת__אימייל from users");
                    dataGrid1.ItemsSource = conn.GetDataTableFromDB(query).Tables[0].DefaultView;
                    DataTable dt = new DataTable();
            
                */
                }







            private void TXTBtn_Click(object sender, RoutedEventArgs e)
                {
           
                    ExportToTXT();
                }





            private void ExportToTXT()
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.FileName = "רשימת משתמשים"; // Default file name
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
                        System.IO.StreamWriter file = new System.IO.StreamWriter(@saveto,false,Encoding.Default);
                
                        file.WriteLine(result1.Replace("‘,’", "‘ ‘"));
                        file.Close();
                        file.Dispose();
                        // Save document 
                        MessageBox.Show("                                                                             !קובץ הטקסט נשמר\n\n           :כדי לפתוח באקסל מומלץ להשתמש ב''פתיחה באמצעות'' ולבחור ב\n\n                                                 ''Microsoft Excel''");
                    }
                }


    



        private void FirstNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
               // string Connectionstring = " Server=localhost;Database=project; UId=root;Password=1234;";
               // MySqlConnection MySqlConn = new MySqlConnection(Connectionstring);
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchkey = this.FirstNameSearchTextBox.Text;
                string Query1 = "select users.empid as `תעודת זהות` ,employees.emp_firstname as `שם פרטי` ,employees.emp_lastname as `שם משפחה` ,users.user_name as `שם משתמש` ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as `כתובת אימייל` from project.users , project.employees where users.empid=employees.empid and employees.emp_firstname Like '%" + searchkey + "%' ";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("users");
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
                string Query1 = "select users.empid as `תעודת זהות` ,employees.emp_firstname as `שם פרטי` ,employees.emp_lastname as `שם משפחה` ,users.user_name as `שם משתמש` ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as `כתובת אימייל` from project.users , project.employees where users.empid=employees.empid and users.empid Like '%" + searchidkey + "%' ";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("users");
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
            ManagerAddNewUserGUI MAUG = new ManagerAddNewUserGUI();
            MAUG.Show();
            this.Close();
        }




        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
       
                try
                {
                    System.Collections.IList rows = dataGrid1.SelectedItems;
                    DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                    if (MessageBox.Show("?האם אתה בטוח שברצונך למחוק משתמש זה", "וידוא מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        //do no stuff
                    }
                    else // if the user clicked on "Yes" so he wants to Delete.
                    {
                        // this will give us the first colum of the selected row in the DataGrid.
                        
                        string selected = row["תעודת זהות"].ToString();
                        // MessageBox.Show("" + selected + "");

                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = "delete from users where empid='" + selected + "'";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();
                            MessageBox.Show("!המשתמש נמחק מהמערכת");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = ("select users.empid as `תעודת זהות`,employees.emp_firstname as `שם פרטי` ,employees.emp_lastname as `שם משפחה` ,users.user_name as `שם משתמש` ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as `כתובת אימייל` from project.users , project.employees where users.empid=employees.empid");
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            DataTable dt = new DataTable("users");
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
            catch { MessageBox.Show("לא נבחר משתמש למחיקה"); }
            
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
                    string selected = row["תעודת זהות"].ToString();
                    // MessageBox.Show(""+selected+ "");



                        if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן משתמש זה", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        {
                            //dont do stuff
                        }
                        else // if the user clicked on "Yes" so he wants to Update.
                        {
                            //checking if the email intered correctlly.
                            if ((Regex.IsMatch(row["כתובת אימייל"].ToString(), @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")))
                            {
                                string firstname = row["שם פרטי"].ToString();
                                string lastname = row["שם משפחה"].ToString();
                                string username = row["שם משתמש"].ToString();
                                string pass = row["סיסמה"].ToString();
                                string role = row["תפקיד"].ToString();
                                string connected = row["מחובר"].ToString();
                                string email = row["כתובת אימייל"].ToString();

                                try
                                {

                                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                    MySqlConn.Open();
                                    string Query1 = "update users set empid='" + selected + "',user_name='" + username + "',password='" + pass + "',role='" + role + "',connected='" + connected + "',email='" + email + "'where empid='" + selected + "'";
                                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                                    MSQLcrcommand1.ExecuteNonQuery();
                                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                                    MySqlConn.Close();
                                    MessageBox.Show("!פרטי המשתמש עודכנו");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                try
                                {
                                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                    MySqlConn.Open();
                                    string Query1 = ("select users.empid as `תעודת זהות`,employees.emp_firstname as `שם פרטי` ,employees.emp_lastname as `שם משפחה` ,users.user_name as `שם משתמש` ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as `כתובת אימייל` from project.users , project.employees where users.empid=employees.empid");
                                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                                    MSQLcrcommand1.ExecuteNonQuery();
                                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                                    DataTable dt = new DataTable("users");
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
                            else MessageBox.Show("כתובת האימייל שהזנת לא תקינה");

                        }


                    
                
            }
            catch { MessageBox.Show("לא נבחר משתמש לעדכון "); }
        }



        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "תעודת זהות" || e.Column.Header.ToString() == "שם פרטי" || e.Column.Header.ToString() == "שם משפחה")
            {
                // e.Cancel = true;   // For not to include 
                 e.Column.IsReadOnly = true; // Makes the column as read only
            }

        }





    }

}