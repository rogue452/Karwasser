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

namespace project
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ManagerAddNewCusGUI : Window
    {

        string cusid;
        string custname;
        string cusaddress;
        string contact_name;
        string contact_email;
        string contact_phone;
        string contact_dep;


        public ManagerAddNewCusGUI()
        {
            InitializeComponent();
        }

        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
           // ManagerCusGui MCG = new ManagerCusGui();
            Login.close = 1;
            this.Close();
          //  MCG.Show();
        }



        private void Add_button_Click(object sender, RoutedEventArgs e)
        {
            bool f1 = false, f2 = false, f3 = false, f4 = false, f5 = false, f6 = false, f7 = false;
            if (id_textBox != null && !string.IsNullOrWhiteSpace(id_textBox.Text))
            {
                try
                {
                    int custideCheck = Convert.ToInt32(id_textBox.Text);
                }
                catch
                {
                    MessageBox.Show("!מספר הלקוח חייב להכיל מספרים בלבד");
                    return;
                }
                cusid = id_textBox.Text;
                f1 = true;
            }
            else
            {
                MessageBox.Show("אנא הכנס מס לקוח ");
            }

            // if (firstname_textBox.Text != null)
            if (custname_textBox != null && !string.IsNullOrWhiteSpace(custname_textBox.Text))
            {
                custname = custname_textBox.Text;
                f2 = true;
                //  MessageBox.Show("" + username + "");
            }
            else
            {
                MessageBox.Show("אנא הכנס שם לקוח");
            }

            //  if (address_textBox != null)
            if (address_textBox != null && !string.IsNullOrWhiteSpace(address_textBox.Text))
            {
                cusaddress = address_textBox.Text;
                f3 = true;
            }
            else
            {
                MessageBox.Show("אנא הכנס כתובת לקוח ");
            }

            // if (email_textBox1.Text != null)
            if (email_textBox1 != null && !string.IsNullOrWhiteSpace(email_textBox1.Text))
            {
                if ((Regex.IsMatch(this.email_textBox1.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")))
                {
                    contact_email = email_textBox1.Text;
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

            if (contact_name_textBox != null && !string.IsNullOrWhiteSpace(contact_name_textBox.Text))
            {
                contact_name = contact_name_textBox.Text;
                f5 = true;
            }
            else
            {
                MessageBox.Show("אנא הכנס שם איש קשר ");
            }

            if (cont_phone_text != null && !string.IsNullOrWhiteSpace(cont_phone_text.Text))
            {
                try
                {
                    int phoneCheck = Convert.ToInt32(cont_phone_text.Text);
                }
                catch
                {
                    MessageBox.Show("!מספר הטלפון חייב להכיל מספרים בלבד");
                    return;
                }
                contact_phone = cont_phone_text.Text;
                f6 = true;
            }
            else
            {
                MessageBox.Show("אנא הכנס טלפון איש קשר ");
            }

            if (cont_dep_text != null && !string.IsNullOrWhiteSpace(cont_dep_text.Text))
            {
                contact_dep = cont_dep_text.Text;
                f7 = true;
            }
            else
            {
                MessageBox.Show("אנא הכנס מחלקת איש קשר ");
            }




            // if all is ok then add new user to the DB.
            if (f1 && f2 && f3 && f4 && f5 && f6 && f7)
            {
                //string not = "לא מחובר";
                // string query = ("insert into project.costumers (costumerid, costumerName, contactName , contactEmail,contactPhone,costumerAddress,contactDepartment) values ('" + cusid + "','" + custname + "','" + contact_name + "','" + contact_email + "','" + contact_phone + "','" + cusaddress + "','" + contact_dep + "')");

                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string query1 = ("select costumerid from costumers where costumerid='" + cusid + "'");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                int count = 0;
                while (dr.Read())
                {

                    count++;
                
                }
                //MessageBox.Show("" + count + "");
                if (count == 0)
                {
                    string query = ("insert into project.costumers (costumerid, contactid, costumerName, contactName , contactEmail,contactPhone,costumerAddress,contactDepartment) values ('" + cusid + "','  1 ','" + custname + "','" + contact_name + "','" + contact_email + "','" + contact_phone + "','" + cusaddress + "','" + contact_dep + "')");
                    DBConnection DBC = new DBConnection();
                    DBC.InsertDataIntoDB(Login.Connectionstring, query);

                    try
                    {
                        MySqlConnection MySqlConn1 = new MySqlConnection(Login.Connectionstring);
                        MySqlConn1.Open();
                        string Query1 = ("select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
                        MySqlCommand MSQLcrcommand11 = new MySqlCommand(Query1, MySqlConn1);
                        MSQLcrcommand11.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand11);
                        // DataTable dt = new DataTable("custumers");
                        ManagerCusGui.dt.Clear();
                        mysqlDAdp.Fill(ManagerCusGui.dt);
                        //dataGrid1.ItemsSource = ManagerCusGui.dt.DefaultView;
                        mysqlDAdp.Update(ManagerCusGui.dt);
                        MySqlConn1.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }    
                }
                else
                {
                    
                    MySqlConn.Close();
                    MessageBox.Show("מספר לקוח כבר קיים במערכת ");
                }
            }

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

