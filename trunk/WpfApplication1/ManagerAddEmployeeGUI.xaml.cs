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
    /// Interaction logic for ManagerAddEmployeeGUI.xaml
    /// </summary>
    public partial class ManagerAddEmployeeGUI : Window
    {
        string empid;
        string firstname;
        string lastname;
        string address;
        string phone;

        public ManagerAddEmployeeGUI()
        {
            InitializeComponent();
          

        }

        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            ManagerEMPGui MEG = new ManagerEMPGui();
            this.Close();
            MEG.Show();
        }


        // This func will check and add the new user to the DB if all is ok.
        private void Add_button_Click(object sender, RoutedEventArgs e)
        {
            bool f1 = false, f2 = false, f3 = false, f4 = false, f5 = false;


            // if (id_textBox.Text != null)
            if (id_textBox != null && !string.IsNullOrWhiteSpace(id_textBox.Text))
            {
                empid = id_textBox.Text;
                f1 = true;
                //  MessageBox.Show("" + username + "");
            }
            else
            {
                MessageBox.Show("אנא הכנס תעודת זהות");

            }

            //  if (firstname_textBox != null)
            if (firstname_textBox != null && !string.IsNullOrWhiteSpace(firstname_textBox.Text))
            {
                firstname = firstname_textBox.Text;
                f2 = true;
            }
            else
            {
                MessageBox.Show("אנא הכנס שם פרטי ");
            }

            // if (lastname_textBox.Text != null)
            if (lastname_textBox != null && !string.IsNullOrWhiteSpace(lastname_textBox.Text))
            {
             
                    lastname = lastname_textBox.Text;
                    //   MessageBox.Show("" + lastname + "");
                    f3 = true;
            }   
                else
                {
                    MessageBox.Show("אנא הכנס שם משפחה ");
                }



            if (address_textBox1 != null&& !string.IsNullOrWhiteSpace(address_textBox1.Text))
            {
                address = address_textBox1.Text;
                f4 = true;
            }
            else
            {
                MessageBox.Show("אנא הכנס כתובת ");
            }


            if (phone_textBox1 != null && !string.IsNullOrWhiteSpace(phone_textBox1.Text))
            {
                phone = phone_textBox1.Text;
                f5 = true;
            }
            else
            {
                MessageBox.Show("אנא הכנס מספר טלפון  ");
            }


            // if all is ok then add new user to the DB.
            if (f1 && f2 && f3 && f4 && f5)
            {
                
                string query = ("insert into project.employees (empid, emp_firstname, emp_lastname, emp_address , emp_phone) values ('" + empid + "','" + firstname + "','" + lastname + "','" + address + "','" + phone + "')");
                DBConnection DBC = new DBConnection();
                DBC.InsertDataIntoDB(Login.Connectionstring, query);

            }
        }

    }
}
