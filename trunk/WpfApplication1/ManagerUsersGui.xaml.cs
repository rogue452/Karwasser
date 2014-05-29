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
        public static DataTable dt = new DataTable("users");
                public ManagerUsersGui()
                {
                    InitializeComponent();
                    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                 //   ListCollectionView CollectionViewList;
                  //  DataTable DataTableFiltered;
                 //   DataSet ds = new DataSet();
                    reafreshandclear();
                   
                }







            private void ExcelBtn_Click(object sender, RoutedEventArgs e)
        {

            ExportToExcel();
        }





        private void ExportToExcel()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("SELECT users.empid as `תעודת זהות`,employees.emp_firstname as `שם פרטי` ,employees.emp_lastname as `שם משפחה` ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as `כתובת אימייל`, users.last_log_in_date as `התחברות אחרונה` ,last_location as `התחברות אחרונה ממחשב` , users.user_description as `הערות לגבי המשתמש` from project.users , project.employees where users.empid=employees.empid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("users");
                mysqlDAdp.Fill(dt);
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "רשימת משתמשים" + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString(); ; // Default file name
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



        public void reafreshandclear()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("SELECT users.empid as `תעודת זהות`,employees.emp_firstname as `שם פרטי` ,employees.emp_lastname as `שם משפחה` ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as `כתובת אימייל`, rec_answer as `בית ספר יסודי`,users.last_log_in_date as `התחברות אחרונה` ,last_location as `התחברות אחרונה ממחשב` , users.user_description as `הערות לגבי המשתמש` FROM project.users , project.employees WHERE users.empid=employees.empid");
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
            IDSearchTextBox.Clear();
            FirstNameSearchTextBox.Clear();
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
                string Query1 = ("SELECT users.empid as `תעודת זהות`,employees.emp_firstname as `שם פרטי` ,employees.emp_lastname as `שם משפחה` ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as `כתובת אימייל`, rec_answer as `בית ספר יסודי`,users.last_log_in_date as `התחברות אחרונה` ,last_location as `התחברות אחרונה ממחשב` , users.user_description as `הערות לגבי המשתמש` from project.users , project.employees where users.empid=employees.empid and employees.emp_firstname Like '%" + searchkey + "%' ");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt = new DataTable("users");
                dt.Clear();
                mysqlDAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                reafreshandclear();
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
             //   string Query1 = "select users.empid as `תעודת זהות` ,employees.emp_firstname as `שם פרטי` ,employees.emp_lastname as `שם משפחה` ,users.user_name as `שם משתמש` ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as `כתובת אימייל` from project.users , project.employees where users.empid=employees.empid and users.empid Like '%" + searchidkey + "%' ";
                string Query1 = ("SELECT users.empid as `תעודת זהות`,employees.emp_firstname as `שם פרטי` ,employees.emp_lastname as `שם משפחה` ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as `כתובת אימייל`, rec_answer as `בית ספר יסודי`,users.last_log_in_date as `התחברות אחרונה` ,last_location as `התחברות אחרונה ממחשב` , users.user_description as `הערות לגבי המשתמש` from project.users , project.employees where users.empid=employees.empid and users.empid Like '%" + searchidkey + "%' ");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
               // DataTable dt = new DataTable("users");
                dt.Clear();
                mysqlDAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
                }
           catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                reafreshandclear();
            }    
        }

        private void ADD_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerAddNewUserGUI MAUG = new ManagerAddNewUserGUI();
            MAUG.Show();
            Login.close = 1;
            this.Close();
        }




        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
       
                try
                {
                    System.Collections.IList rows = dataGrid1.SelectedItems;
                    DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                    if (MessageBox.Show("?האם אתה בטוח שברצונך למחוק משתמש זה", "וידוא מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
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
                        reafreshandclear();
                    }//end else
                
            }//end try
            catch { MessageBox.Show("לא נבחר משתמש למחיקה"); }
            
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
                string selected = row["תעודת זהות"].ToString();
                    // MessageBox.Show(""+selected+ "");

                if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן משתמש זה", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
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
                        if (string.IsNullOrWhiteSpace(row["סיסמה"].ToString()))
                        {
                            MessageBox.Show("אנא הכנס סיסמא", "!שים לב", MessageBoxButton.YesNo, MessageBoxImage.Error);
                            reafreshandclear();
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(row["בית ספר יסודי"].ToString()))
                        {
                            MessageBox.Show("אנא הכנס בית ספר יסודי", "!שים לב", MessageBoxButton.YesNo, MessageBoxImage.Error);
                            reafreshandclear();
                            return;
                        }
                        string pass = row["סיסמה"].ToString();
                        string role = row["תפקיד"].ToString();
                        string connected = row["מחובר"].ToString();
                        string email = row["כתובת אימייל"].ToString();
                        string userdesc = row["הערות לגבי המשתמש"].ToString();
                        string school = row["בית ספר יסודי"].ToString();
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = "update users set empid='" + selected + "',password='" + pass + "',role='" + role + "',connected='" + connected + "',email='" + email + "' ,user_description='" + userdesc + "',rec_answer='" + school + "' where empid='" + selected + "'";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();
                            MessageBox.Show("!פרטי המשתמש עודכנו", "!הצלחה", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        reafreshandclear();
                    }
                    else
                    {
                        MessageBox.Show("כתובת האימייל שהזנת לא תקינה", "!שים לב", MessageBoxButton.YesNo, MessageBoxImage.Error);
                        reafreshandclear();
                    }
                }      
                
            }
            catch
            { MessageBox.Show("לא נבחר משתמש לעדכון", "!שים לב", MessageBoxButton.YesNo, MessageBoxImage.Error); }
        }



        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "תעודת זהות" || e.Column.Header.ToString() == "שם פרטי" || e.Column.Header.ToString() == "שם משפחה" || e.Column.Header.ToString() == "התחברות אחרונה ממחשב")
            {
                // e.Cancel = true;   // For not to include 
                 e.Column.IsReadOnly = true; // Makes the column as read only
            }

            if (e.Column.Header.ToString() == "התחברות אחרונה")
             {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "HH:mm:ss - dd/MM/yyyy";
                e.Column.IsReadOnly = true; // Makes the column as read only
             }

            if (e.Column.Header.ToString() == "תפקיד")
            {
              
                string columnName = e.Column.Header.ToString();
                Dictionary<string, string> comboKey = new Dictionary<string, string>()
                    {
                        {"מנהל","מנהל"},
                        {"מזכירה","מזכירה"},
                        {"איכות","איכות"},
                        
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

            if (e.Column.Header.ToString() == "מחובר")
            {
                string columnName1 = e.Column.Header.ToString();
                Dictionary<string, string> comboKey1 = new Dictionary<string, string>()
                    {
                        {"מחובר","מחובר"},
                        {"לא מחובר","לא מחובר"},
                        
                    };
                DataGridTemplateColumn col11 = new DataGridTemplateColumn();
                col11.Header = columnName1;
                col11.SortMemberPath = columnName1;
                #region Editing
                FrameworkElementFactory factory11 = new FrameworkElementFactory(typeof(ComboBox));
                Binding b11 = new Binding(columnName1);
                b11.IsAsync = true;
                b11.Mode = BindingMode.TwoWay;
                factory11.SetValue(ComboBox.ItemsSourceProperty, comboKey1);
                factory11.SetValue(ComboBox.SelectedValuePathProperty, "Key");
                factory11.SetValue(ComboBox.DisplayMemberPathProperty, "Value");
                factory11.SetValue(ComboBox.SelectedValueProperty, b11);
                factory11.SetValue(ComboBox.SelectedItemProperty, col11);

                DataTemplate cellTemplate1 = new DataTemplate();
                cellTemplate1.VisualTree = factory11;
                col11.CellTemplate = cellTemplate1;
                col11.CellEditingTemplate = cellTemplate1;
                col11.IsReadOnly = false;
                col11.InvalidateProperty(ComboBox.SelectedValueProperty);
                #endregion

                #region View
                FrameworkElementFactory sfactory = new FrameworkElementFactory(typeof(TextBlock));
                sfactory.SetValue(TextBlock.TextProperty, b11);
                DataTemplate cellTemplate = new DataTemplate();
                cellTemplate.VisualTree = sfactory;
                col11.CellTemplate = cellTemplate;
                #endregion

                e.Column = col11;
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