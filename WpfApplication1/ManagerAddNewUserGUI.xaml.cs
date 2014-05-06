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
                string Query1 = ("SELECT empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון` FROM project.employees WHERE employees.empid not in (SELECT users.empid FROM project.users) ");
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
                string Query1 = "select empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון` from  employees WHERE employees.empid not in (SELECT users.empid FROM project.users)  AND emp_firstname Like '%" + searchkey + "%' ";
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
                string Query1 = "SELECT empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון` from employees WHERE employees.empid not in (SELECT users.empid FROM project.users) AND empid Like '%" + searchidkey + "%' ";
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



        // This func will check and add the new user to the DB if all is ok.
        private void Add_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string empid = row["תעודת זהות"].ToString();
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

                }
            }
            catch { MessageBox.Show("אנא בחר עובד"); }
        }




        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "תעודת זהות" || e.Column.Header.ToString() == "שם פרטי" || e.Column.Header.ToString() == "שם משפחה" || e.Column.Header.ToString() == "כתובת"||e.Column.Header.ToString() == "מספר טלפון")
            {
               
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
        }





    }
}
