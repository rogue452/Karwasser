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

namespace project
{
    /// <summary>
    /// Interaction logic for ManagerAddNewUserGUI.xaml
    /// </summary>
    public partial class ManagerAddNewUserGUI : Window
    {
        string empid;
        string username;
        string password;
        string email;
        string role;

        public ManagerAddNewUserGUI()
        {
            InitializeComponent();
            Role_comboBox.Items.Add("מנהל");
            Role_comboBox.Items.Add("מזכירה");
            Role_comboBox.Items.Add("איכות");
            Fill_IDcomboBox();

        }


        private void Fill_IDcomboBox()
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
        }

        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            ManagerUsersGui MUG = new ManagerUsersGui();
            this.Close();
            MUG.Show();
        }





        // This func will check and add the new user to the DB if all is ok.
        private void Add_button_Click(object sender, RoutedEventArgs e)
        {
            bool f1=false , f2=false, f3=false, f4=false , f5=false;
            if (Empid_comboBox.SelectedValue != null)
            {
                string[] id = Empid_comboBox.SelectedValue.ToString().Split(new char[] { '-' });
                empid = id[0];
                f1 = true;

                //MessageBox.Show("" + empid + "");
            }
            else 
            {
                MessageBox.Show("אנא בחר עובד");
            }

            // if (User_name_textBox.Text != null)
            if (User_name_textBox != null && !string.IsNullOrWhiteSpace(User_name_textBox.Text))
            {
                username = User_name_textBox.Text;
                f2 = true;
              //  MessageBox.Show("" + username + "");
            }
            else
            {
                MessageBox.Show("אנא הכנס שם משתמש");
            }

          //  if (Password_textBox != null)
            if (Password_textBox != null && !string.IsNullOrWhiteSpace(Password_textBox.Text))
            {
                password = Password_textBox.Text;
                f3 = true;
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
                  
                    f4 = true;
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
                f5 = true;
            }
            else
            {
                MessageBox.Show("אנא בחר תפקיד");
            }

            // if all is ok then add new user to the DB.
            if(f1&&f2&&f3&&f4&&f5)
            {
                //string not = "לא מחובר";
                string query = ("insert into project.users (empid, user_name, password, role , email) values ('" + empid + "','" + username + "','" + password + "','" + role + "','" + email + "')");
                DBConnection DBC = new DBConnection();
                DBC.InsertDataIntoDB(Login.Connectionstring, query);

            }
        }

    }
}
