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
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Timers;
using System.Windows.Threading;
using MySql.Data.MySqlClient;

namespace project
{
  
    public partial class Login : Window
    {

        public static string first_name;
        public static string last_name;
        public static string user_role;
        public static string connected;
        public static string user_id;
        public static string user_name;
        public static string useremail;

        public Login()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void button1_Click(object sender, RoutedEventArgs e)//כפתור כניסה 
        {
            try
            {
                string Connectionstring = " Server=localhost;Database=project; UId=root;Password=1234;";
                MySqlConnection objc = new MySqlConnection(Connectionstring);
                objc.Open();
                
                string Query = "select * from project.users where user_name='" + this.textBox1.Text +"'and password='"+this.textBox2.Password+"'" ;
                MySqlCommand crcommand = new MySqlCommand(Query, objc);
                crcommand.ExecuteNonQuery();
                MySqlDataReader dr = crcommand.ExecuteReader();
                int count = 0;
                while (dr.Read())
                {
                    count++;
                    user_role = dr.GetString(5);
                }
                if (count == 1)
                {
                    string Connectionstring1 = " Server=localhost;Database=project; UId=root;Password=1234;";
                    MySqlConnection objc1 = new MySqlConnection(Connectionstring1);
                    objc1.Open();
                    string Query1 = "select users.userid, employees.emp_firstname, employees.emp_lastname, users.user_name,  users.connected, users.email from project.users, project.employees where users.empid=employees.empid and users.user_name='" + this.textBox1.Text + "'and users.password='" + this.textBox2.Password + "'";
                    MySqlCommand crcommand1 = new MySqlCommand(Query1, objc1);
                    crcommand1.ExecuteNonQuery();
                    MySqlDataReader dr1 = crcommand1.ExecuteReader();
                    int count1 = 0;
                    while (dr1.Read())
                    {
                        count1++;
                        user_id = dr1.GetString(0);
                        first_name = dr1.GetString(1);
                        last_name = dr1.GetString(2);
                        user_name = dr1.GetString(3);
                        connected = dr1.GetString(4);                    
                        useremail = dr1.GetString(5);
                    }
                    if (count1 == 1)
                    {
                        if (connected.Equals("false"))
                        {
                            MessageBox.Show("      ברוכ/ה הבא/ה " + Login.last_name + " " + Login.first_name + "", "!ההתחברות למערכת בוצעה בהצלחה", MessageBoxButton.OK);
                 //           DBConnection conn = new DBConnection();
                 //         string query2 = "update users set connected='true' where user_name= '" + this.textBox1.Text + "' and password ='" + this.textBox2.Password + "'";
                 //          conn.LogIn(query2);
                              ManagerGui MG = new ManagerGui();
                     //       UsersViewGui Test = new UsersViewGui();
                      //      MangerMainGui MGui = new MangerMainGui();
                        //      EMPGui EG = new EMPGui();
                       //       UsersGui UG = new UsersGui();
                            
                              MG.Show();
                        //    Test.Show();
                     //         EG.Show();
                     //         UG.Show();
                        //    MGui.Show();
                          //  MGui.Activate();   
                              this.Close();
                          //    MGui.ShowDialog();
                           // this.Close();
                        }
                        else {
                            MessageBox.Show( "אתה כבר מחובר למערכת  ", " שגיאה", MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        MessageBox.Show("!אינך משתמש פעיל במערכת, אנא פנה למנהל", "!ההתחברות למערכת נכשלה", MessageBoxButton.OK);
                        textBox1.Clear();
                        textBox2.Clear();
                    }
                    objc1.Close();
                }
                if (count < 1)
                {
                    MessageBox.Show("שם משתמש ו/או סיסמא שגויים! אנא נסה שנית", "!ההתחברות למערכת נכשלה", MessageBoxButton.OK);
                    textBox1.Clear();
                    textBox2.Clear();
                }
                objc.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        private void button2_Click(object sender, RoutedEventArgs e)// ניקוי שדות 
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button3_Click(object sender, RoutedEventArgs e)// יציאה 
        {
            this.Close();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            Forgot_pass fp = new Forgot_pass();
            fp.Show();
            this.Close();
        }
    }
}
