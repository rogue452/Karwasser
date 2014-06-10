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
    /// Interaction logic for SecAddEmployeeGUI.xaml
    /// </summary>
    public partial class SecAddEmployeeGUI : Window
    {
         string empid;
        string firstname;
        string lastname;
        string address;
        string phone="";
        string cellphone="";
        string emp_num;
        string start="";

        public SecAddEmployeeGUI()
        {
            InitializeComponent();
            phone_W_label.Visibility = Visibility.Hidden;
            id_W_label.Visibility = Visibility.Hidden;
            first_W_label.Visibility = Visibility.Hidden;
            address_W_label.Visibility = Visibility.Hidden;
            last_W_label.Visibility = Visibility.Hidden;
            cell_W_label.Visibility = Visibility.Hidden;
            empnum_W_label.Visibility = Visibility.Hidden;
          

        }

        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            Login.close = 1;
            this.Close();
        }


        // This func will check and add the new user to the DB if all is ok.
        private void Add_button_Click(object sender, RoutedEventArgs e)
        {
            bool f1 = false, f2 = false, f3 = false, f4 = false, f5 = false, f6 = false;
            phone_W_label.Visibility = Visibility.Hidden;
            id_W_label.Visibility = Visibility.Hidden;
            first_W_label.Visibility = Visibility.Hidden;
            address_W_label.Visibility = Visibility.Hidden;
            last_W_label.Visibility = Visibility.Hidden;
            cell_W_label.Visibility = Visibility.Hidden;
            empnum_W_label.Visibility = Visibility.Hidden;

            // if (id_textBox.Text != null)
            if (id_textBox != null && !string.IsNullOrWhiteSpace(id_textBox.Text))
            {
                try
                {   //to see if the empid already in the system.
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = ("SELECT COUNT(empid) FROM employees WHERE empid='" + id_textBox.Text + "' ");
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();
                    int empidtimes = Convert.ToInt32(MSQLcrcommand1.ExecuteScalar());
                    MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                    MySqlConn.Close();

                    if (empidtimes != 0)
                    {
                        MessageBox.Show("כבר קיים עובד בעל תעודת זהות זו - " + id_textBox.Text , "!שים לב" ,MessageBoxButton.OK ,MessageBoxImage.Error);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                empid = id_textBox.Text;
                f1 = true;
            }
            else
            {
                id_W_label.Visibility = Visibility.Visible;
                //MessageBox.Show("אנא הכנס תעודת זהות");

            }

            if (firstname_textBox != null && !string.IsNullOrWhiteSpace(firstname_textBox.Text))
            {
                firstname = firstname_textBox.Text;
                f2 = true;
            }
            else
            {
                first_W_label.Visibility = Visibility.Visible;
               // MessageBox.Show("אנא הכנס שם פרטי ");
            }


            if (lastname_textBox != null && !string.IsNullOrWhiteSpace(lastname_textBox.Text))
            {
             
                    lastname = lastname_textBox.Text;
                    f3 = true;
            }   
                else
                {
                    last_W_label.Visibility = Visibility.Visible;
                    //MessageBox.Show("אנא הכנס שם משפחה ");
                }



            if (emp_num_textBox != null && !string.IsNullOrWhiteSpace(emp_num_textBox.Text))
            {
                try
                {   //to see if the emp_insidenum already in the system.
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = ("SELECT COUNT(emp_insidenum) FROM employees WHERE emp_insidenum='" + emp_num_textBox.Text + "' "); 
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();
                    int times = Convert.ToInt32(MSQLcrcommand1.ExecuteScalar());
                    MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                    MySqlConn.Close();

                    if (times != 0)
                    {
                        MessageBox.Show("כבר קיים מספר עובד - " + emp_num_textBox.Text, "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                emp_num = emp_num_textBox.Text;
                f4 = true;
            }
            else
            {
                empnum_W_label.Visibility = Visibility.Visible;
               // MessageBox.Show("אנא הכנס מספר עובד");
            }


            if (phone_textBox1 != null && !string.IsNullOrWhiteSpace(phone_textBox1.Text))
            {
                try
                {
                    int phoneCheck = Convert.ToInt32(phone_textBox1.Text);
                }
                catch 
                {
                    phone_W_label.Visibility = Visibility.Visible;
                   // MessageBox.Show("!מספר הטלפון חייב להכיל מספרים בלבד");
                    return;
                }
                phone = phone_textBox1.Text;
                
            }
          
            if (startdatePicker != null && !string.IsNullOrWhiteSpace(startdatePicker.Text))
            {
                try
                {
                    start = Convert.ToDateTime(startdatePicker.Text).ToString("yyyy-MM-dd");
                    f6 = true;
                }
                catch { MessageBox.Show("תאריך התחלה אינו תקין", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); return; }
            }

            if (cell_textBox != null && !string.IsNullOrWhiteSpace(cell_textBox.Text))
            {
                try
                {
                    int cellphoneCheck = Convert.ToInt32(cell_textBox.Text);
                }
                catch
                {
                    cell_W_label.Visibility = Visibility.Visible;
                   // MessageBox.Show("!מספר הנייד חייב להכיל מספרים בלבד");
                    return;
                }
                cellphone = cell_textBox.Text;
            }

            if (address_textBox1 != null && !string.IsNullOrWhiteSpace(address_textBox1.Text))
            {
                address = address_textBox1.Text;
                f5 = true;
            }
            else
            {
                address_W_label.Visibility = Visibility.Visible;
               // MessageBox.Show("אנא הכנס כתובת");
            }
                
            // if all is ok then add new user to the DB.
            if (f1 && f2 && f3 && f4 && f5 && f6)
            {
                try
                {
                    string query = ("insert into project.employees (empid, emp_firstname, emp_lastname, emp_address , emp_phone,emp_cellphone,emp_start_date,emp_insidenum) values ('" + empid + "','" + firstname + "','" + lastname + "','" + address + "','" + phone + "','" + cellphone + "','" + start + "','" + emp_num + "')");
                    DBConnection DBC = new DBConnection();
                    DBC.InsertDataIntoDB(Login.Connectionstring, query);
                    //MessageBox.Show("העובד התווסף למערכת");
                
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = ("select empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון` from project.employees ");
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();
                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                    // DataTable dt = new DataTable("employess");
                    ManagerEMPGui.dt.Clear();
                    mysqlDAdp.Fill(ManagerEMPGui.dt);
                   // ManagerEMPGui.dataGrid1.ItemsSource = ManagerEMPGui.dt.DefaultView;
                    mysqlDAdp.Update(ManagerEMPGui.dt);
                    MySqlConn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

            if (f1 && f2 && f3 && f4 && f5 && !f6)
            {
                try
                {
                    string query = ("insert into project.employees (empid, emp_firstname, emp_lastname, emp_address , emp_phone,emp_cellphone,emp_insidenum) values ('" + empid + "','" + firstname + "','" + lastname + "','" + address + "','" + phone + "','" + cellphone + "','" + emp_num + "')");
                    DBConnection DBC = new DBConnection();
                    DBC.InsertDataIntoDB(Login.Connectionstring, query);
                    //MessageBox.Show("העובד התווסף למערכת");

                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = ("SELECT empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` , emp_insidenum as `מספר עובד` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון`, emp_cellphone as `טלפון נייד`, emp_start_date as `תאריך התחלת עבודה` FROM project.employees ");
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();
                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                    SecEMPGui.dt.Clear();
                    mysqlDAdp.Fill(SecEMPGui.dt);
                    mysqlDAdp.Update(SecEMPGui.dt);
                    MySqlConn.Close();
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
                if (MessageBox.Show("?האם אתה בטוח שברצונך לסגור את החלון ", "וידוא יציאה מהוספת עובד חדש", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    e.Cancel = true; ; //don't exit.
                }
            }

            Login.close = 0;
            
        }





    }
}
