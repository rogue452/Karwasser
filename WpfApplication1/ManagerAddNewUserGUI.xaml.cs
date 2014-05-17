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
using System.Text.RegularExpressions;
using System.ComponentModel;
using Microsoft.Win32;


namespace project
{
    /// <summary>
    /// Interaction logic for ManagerAddNewUserGUI.xaml
    /// </summary>
    public partial class ManagerAddNewUserGUI : Window
    {
        //string empid;
        string password;
        string email;
        string role;
        public static DataTable dt = new DataTable("employess");
        public ManagerAddNewUserGUI()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("SELECT empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` , emp_insidenum as `מספר עובד` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון`, emp_cellphone as `טלפון נייד`, emp_start_date as `תאריך התחלת עבודה` FROM project.employees WHERE employees.empid not in (SELECT users.empid FROM project.users)");
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
           
            Role_comboBox.Items.Add("מנהל");
            Role_comboBox.Items.Add("מזכירה");
            Role_comboBox.Items.Add("איכות");
           // Fill_IDcomboBox();
            
        }


      /*  private void Fill_IDcomboBox()
        {
            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
            MySqlConn.Open();
            string Query1 = ("SELECT * FROM project.employees where employees.empid not in (SELECT users.empid FROM project.users)");
            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
            MSQLcrcommand1.ExecuteNonQuery();
            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
            DataSet ds = new DataSet();
            mysqlDAdp.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string ToCB = ds.Tables[0].Rows[i][0] + "-" + ds.Tables[0].Rows[i][1] + "-" + ds.Tables[0].Rows[i][2];
                Empid_comboBox.Items.Add(ToCB);
            }
        }*/

        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            ManagerUsersGui MUG = new ManagerUsersGui();
            Login.close = 1;
            this.Close();
            MUG.Show();
        }

        private void FirstNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchkey = this.FirstNameSearchTextBox.Text;
                string Query1 = "SELECT empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` , emp_insidenum as `מספר עובד` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון`, emp_cellphone as `טלפון נייד`, emp_start_date as `תאריך התחלת עבודה` FROM  employees WHERE  emp_firstname Like '%" + searchkey + "%' AND employees.empid not in (SELECT users.empid FROM project.users) ";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt = new DataTable("employess");
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
                string Query1 = "SELECT empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` , emp_insidenum as `מספר עובד` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון`, emp_cellphone as `טלפון נייד`, emp_start_date as `תאריך התחלת עבודה` FROM employees WHERE  empid Like '%" + searchidkey + "%' AND employees.empid not in (SELECT users.empid FROM project.users)";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt = new DataTable("employees");
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



        // This func will check and add the new user to the DB if all is ok.
        private void Add_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("נכנס לטראי");
                DataRowView row1 = (DataRowView)dataGrid1.SelectedItems[0];
            }
            catch { MessageBox.Show("אנא בחר עובד"); return; }
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string empid = row["תעודת זהות"].ToString();
                Console.WriteLine(empid);
                bool f1 = false, f2 = false, f3 = false;

                //  if (Password_textBox != null)
                if (Password_textBox != null && !string.IsNullOrWhiteSpace(Password_textBox.Password))
                {
                    password = Password_textBox.Password;
                    f1 = true;
                }
                else
                {
                    MessageBox.Show("אנא הכנס סיסמא");
                }

                // if (Email_textBox.Text != null)
                if (Email_textBox != null && !string.IsNullOrWhiteSpace(Email_textBox.Text))
                {
                    if ((Regex.IsMatch(this.Email_textBox.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")))
                    {
                        email = Email_textBox.Text;
                        //   MessageBox.Show("" + email + "");

                        f2 = true;
                    }
                    else
                    {
                        MessageBox.Show("אנא בדוק תקינות כתובת האימייל");
                    }
                }
                else
                {
                    MessageBox.Show("אנא הכנס כתובת אימייל");
                }

                if (Role_comboBox.SelectedValue != null)
                {
                    role = Role_comboBox.SelectedValue.ToString();
                    f3 = true;
                }
                else
                {
                    MessageBox.Show("אנא בחר תפקיד");
                }

                // if all is ok then add new user to the DB.
                if (f1 && f2 && f3)
                {
                    DateTime yesterday = DateTime.Today.AddDays(-1);
                    string date = Convert.ToDateTime(yesterday).ToString("yyyy-MM-dd");
                    Console.WriteLine(date);
                    //string not = "לא מחובר";
                    string query = ("insert into project.users (empid, password, role , email , last_email_sent_date) values ('" + empid + "','" + password + "','" + role + "','" + email + "','" + date + "')");
                    DBConnection DBC = new DBConnection();
                    DBC.InsertDataIntoDB(Login.Connectionstring, query);
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` , emp_insidenum as `מספר עובד` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון`, emp_cellphone as `טלפון נייד`, emp_start_date as `תאריך התחלת עבודה` FROM project.employees WHERE employees.empid not in (SELECT users.empid FROM project.users)");
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




        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
           if (e.Column.Header.ToString() == "תאריך התחלת עבודה")
            {
                string colname = e.Column.Header.ToString();
                DataGridTemplateColumn dgct = new DataGridTemplateColumn();
                dgct.Header = colname;
                dgct.SortMemberPath = colname;

                Binding b = new Binding(colname);
                b.StringFormat = "dd/MM/yyyy";

                #region Editing
                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(DatePicker));
                factory.SetValue(DatePicker.SelectedDateProperty, b);
                DataTemplate cellEditingTemplate = new DataTemplate();
                cellEditingTemplate.VisualTree = factory;
                dgct.CellEditingTemplate = cellEditingTemplate;
                #endregion

                #region View
                FrameworkElementFactory sfactory = new FrameworkElementFactory(typeof(TextBlock));
                sfactory.SetValue(TextBlock.TextProperty, b);
                DataTemplate cellTemplate = new DataTemplate();
                cellTemplate.VisualTree = sfactory;
                dgct.CellTemplate = cellTemplate;
                #endregion
                e.Column = dgct;
            }
                e.Column.IsReadOnly = true; // Makes the column as read only
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
