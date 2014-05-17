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
    /// Interaction logic for ManagerAddContactsGUI.xaml
    /// </summary>
    public partial class ManagerAddContactsGUI : Window
    {

        string contact_name;
        string contact_email;
        string contact_phone;
        string contact_dep;
        string cosADDs;

        public ManagerAddContactsGUI(string selected, string cosName,string cosADDs)
        {
            InitializeComponent();
            CostName_label.Content = cosName;
            CostNum_label.Content = selected;
            this.cosADDs=cosADDs;
        }




        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            //ManagerCusGui MCG = new ManagerCusGui();
            Login.close = 1;
            this.Close();
         //   MCG.Show();
        }



        private void Add_button_Click(object sender, RoutedEventArgs e)
        {
            bool f1 = false, f2 = false, f3 = false, f4 = false;

            // if (email_textBox1.Text != null)
            if (email_textBox1 != null && !string.IsNullOrWhiteSpace(email_textBox1.Text))
            {
                if ((Regex.IsMatch(this.email_textBox1.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")))
                {
                    contact_email = email_textBox1.Text;
                    //   MessageBox.Show("" + email + "");

                    f1 = true;
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
                f2 = true;
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
                f3 = true;
            }
            else
            {
                MessageBox.Show("אנא הכנס טלפון איש קשר ");
            }

            if (cont_dep_text != null && !string.IsNullOrWhiteSpace(cont_dep_text.Text))
            {
                contact_dep = cont_dep_text.Text;
                f4 = true;
            }
            else
            {
                MessageBox.Show("אנא הכנס מחלקת איש קשר ");
            }




            // if all is ok then add new user to the DB.
            if (f1 && f2 && f3 && f4)
            {
          
                // string query = ("insert into project.costumers (costumerid, costumerName, contactName , contactEmail,contactPhone,costumerAddress,contactDepartment) values ('" + cusid + "','" + custname + "','" + contact_name + "','" + contact_email + "','" + contact_phone + "','" + cusaddress + "','" + contact_dep + "')");
                try
                {
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    
                    MessageBox.Show("" + CostNum_label.Content + "");
                    string query1 = ("select MAX(contactid) from costumers where costumerid='" + CostNum_label.Content + "'");
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();
                    //MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                  //  int max;
                   // Object max;
                  //  while (dr.Read())
                   // {
                    // max = MSQLcrcommand1.ExecuteScalar();
                     int max = Convert.ToInt32(MSQLcrcommand1.ExecuteScalar());
                     max++;
                    
                      //  max = dr["MAX(contactid)"];
                     

                  //  }
                    //max = (int) dr.GetValue(0);
                     MessageBox.Show("" + max + "");
                    //MessageBox.Show("" + max + "");
                    //MessageBox.Show("" + count + "");

                    string query = ("insert into project.costumers (costumerid, contactid, costumerName, contactName , contactEmail,contactPhone,costumerAddress,contactDepartment) values ('" + CostNum_label.Content + "','" + max + "','" + CostName_label.Content + "','" + contact_name + "','" + contact_email + "','" + contact_phone + "','" + cosADDs + "','" + contact_dep + "')");
                    DBConnection DBC = new DBConnection();
                    DBC.InsertDataIntoDB(Login.Connectionstring, query);
                    try
                    {
                        MySqlConnection MySqlConn1 = new MySqlConnection(Login.Connectionstring);
                        MySqlConn1.Open();
                        string Query1 = ("select contactid as `מספר איש קשר`,contactName as `שם איש קשר` ,contactEmail as `אימייל איש קשר` ,contactPhone as `טלפון איש קשר` ,contactDepartment as `מחלקת איש קשר` from costumers  where costumerid='" + CostNum_label.Content + "'");
                        MySqlCommand MSQLcrcommand11 = new MySqlCommand(Query1, MySqlConn1);
                        MSQLcrcommand11.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand11);
                        // DataTable dt = new DataTable("employess");
                        ManagerContactsGUI.dt.Clear();
                        mysqlDAdp.Fill(ManagerContactsGUI.dt);
                        //ManagerEMPGui.dataGrid1.ItemsSource = ManagerEMPGui.dt.DefaultView;
                        mysqlDAdp.Update(ManagerContactsGUI.dt);
                        MySqlConn1.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
