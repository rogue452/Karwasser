// ***********************************************************************
// Assembly         : WpfApplication1
// Author           : user
// Created          : 06-10-2014
//
// Last Modified By : user
// Last Modified On : 06-10-2014
// ***********************************************************************
// <copyright file="SecAddContactsGUI.xaml.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// Interaction logic for SecAddContactsGUI.xaml
    /// </summary>
    public partial class SecAddContactsGUI : Window
    {
        /// <summary>
        /// The contact_name
        /// </summary>
       string contact_name;
       /// <summary>
       /// The contact_email
       /// </summary>
        string contact_email;
        /// <summary>
        /// The contact_dep
        /// </summary>
        string contact_dep;
        /// <summary>
        /// The cos ad ds
        /// </summary>
        string cosADDs;
        /// <summary>
        /// The contact_phone
        /// </summary>
        string contact_phone;
        /// <summary>
        /// The contact_cellphone
        /// </summary>
        string contact_cellphone;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecAddContactsGUI"/> class.
        /// </summary>
        /// <param name="selected">The selected.</param>
        /// <param name="cos_insideNum">The cos_inside number.</param>
        /// <param name="cosName">Name of the cos.</param>
        /// <param name="cosADDs">The cos ad ds.</param>
        public SecAddContactsGUI(string selected, string cos_insideNum, string cosName, string cosADDs)
        {
            InitializeComponent();
            CostName_label.Content = cosName;
            CostNum_label.Content = selected;
            cos_num_label.Content = cos_insideNum;
            this.cosADDs=cosADDs;
            name_W_label.Visibility = Visibility.Hidden;
            mail_W_label.Visibility = Visibility.Hidden;
            phone_W_label.Visibility = Visibility.Hidden;
            both_W_label.Visibility = Visibility.Hidden;
            cell_W_label.Visibility = Visibility.Hidden;
            dep_W_label.Visibility = Visibility.Hidden;   
        }




        /// <summary>
        /// Handles the Click event of the Back_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            Login.close = 1;
            this.Close();
        }



        /// <summary>
        /// Handles the Click event of the Add_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Add_button_Click(object sender, RoutedEventArgs e)
        {
            name_W_label.Visibility = Visibility.Hidden;
            mail_W_label.Visibility = Visibility.Hidden;
            phone_W_label.Visibility = Visibility.Hidden;
            both_W_label.Visibility = Visibility.Hidden;
            cell_W_label.Visibility = Visibility.Hidden;
            dep_W_label.Visibility = Visibility.Hidden;
            bool f1 = false, f2 = false, f3 = false, f4 = false, f5 = false, f6 = false, f7 = false, f8 = false;

            // if (email_textBox1.Text != null)
            if (!string.IsNullOrWhiteSpace(email_textBox1.Text))
            {
                if ((Regex.IsMatch(this.email_textBox1.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")))
                {
                    contact_email = email_textBox1.Text;
                    f1 = true;
                }
                else
                {
                    mail_W_label.Content = "אנא בדוק תקינות כתובת האימייל";
                    mail_W_label.Visibility = Visibility.Visible;
                    //MessageBox.Show("אנא בדוק תקינות כתובת האימייל");

                }
            }
            else
            {
                contact_email = "לא הוזן";
                f1 = true;
                // mail_W_label.Content = "אנא הכנס כתובת אימייל";
                // mail_W_label.Visibility = Visibility.Visible;
                //MessageBox.Show("אנא הכנס כתובת אימייל");

            }


            if (!string.IsNullOrWhiteSpace(contact_name_textBox.Text))
            {
                contact_name = contact_name_textBox.Text;
                f2 = true;
            }
            else
            {
                name_W_label.Content = "אנא הכנס שם איש קשר";
                name_W_label.Visibility = Visibility.Visible;
                //MessageBox.Show("אנא הכנס שם איש קשר ");
                
            }

            if (!string.IsNullOrWhiteSpace(cont_phone_text.Text))
            {
                try
                {
                    int phoneCheck = Convert.ToInt32(cont_phone_text.Text);
                    contact_phone = cont_phone_text.Text;
                    f5 = true;
                }
                catch
                {
                    f7 = true;
                    phone_W_label.Content = "מספר הטלפון חייב להכיל מספרים בלבד!";
                    phone_W_label.Visibility = Visibility.Visible;
                    //MessageBox.Show("!מספר הטלפון חייב להכיל מספרים בלבד");
                    
                }
                
            }

            if (!string.IsNullOrWhiteSpace(cell_textBox.Text))
            {
                try
                {
                    int cellphoneCheck = Convert.ToInt32(cell_textBox.Text);
                    contact_cellphone = cell_textBox.Text;
                    f6 = true;
                }
                catch
                {
                    f8 = true;
                    cell_W_label.Content = "מספר הטלפון נייד חייב להכיל מספרים בלבד!";
                    cell_W_label.Visibility = Visibility.Visible;
                    //MessageBox.Show("!מספר הטלפון נייד חייב להכיל מספרים בלבד");
                    
                }
                
            }


            if (f5 || f6) //user enterd phone and/or cellphone correctly.
            {
                if (!f7 && !f8) // if non was wrong.
                {
                    f3 = true;
                }
            }

            //user did not enterd cellphone and/or phone.
            if (string.IsNullOrWhiteSpace(cont_phone_text.Text) && string.IsNullOrWhiteSpace(cell_textBox.Text))
            {
                both_W_label.Content = "אנא הכנס מספר טלפון ו/או נייד עבור איש הקשר";
                both_W_label.Visibility = Visibility.Visible;
                //MessageBox.Show("אנא הכנס מספר טלפון ו/או נייד עבור איש הקשר ");
            }



            if (!string.IsNullOrWhiteSpace(cont_dep_text.Text))
            {
                contact_dep = cont_dep_text.Text;
                f4 = true;
            }
            else
            {
                contact_dep = "לא הוזן";
                f4 = true;
              //  dep_W_label.Content = "אנא הכנס מחלקת איש קשר";
               // dep_W_label.Visibility = Visibility.Visible;
                //MessageBox.Show("אנא הכנס מחלקת איש קשר ");
                
            }




            // if all is ok then add new user to the DB.
            if (f1 && f2 && f3 && f4)
            {
                

                // string query = ("insert into project.costumers (costumerid, costumerName, contactName , contactEmail,contactPhone,costumerAddress,contactDepartment) values ('" + cusid + "','" + custname + "','" + contact_name + "','" + contact_email + "','" + contact_phone + "','" + cusaddress + "','" + contact_dep + "')");
                try
                {
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    
                    //MessageBox.Show("" + CostNum_label.Content + "");
                    string query1 = ("select MAX(contactid) from costumers where costumerid='" + CostNum_label.Content + "'");
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();

                     int max = Convert.ToInt32(MSQLcrcommand1.ExecuteScalar());
                     max++;

                     // if only phone
                     string query = ("insert into project.costumers (costumerid, contactid, costumerName, contactName , contactEmail,contactPhone,costumerAddress,contactDepartment,costumer_insideNum) values ('" + CostNum_label.Content + "','" + max + "','" + CostName_label.Content + "','" + contact_name + "','" + contact_email + "','" + contact_phone + "','" + cosADDs + "','" + contact_dep + "','" + cos_num_label.Content + "')");

                     if (!f5 && f6) // if only cell
                     {
                         query = ("insert into project.costumers (costumerid, contactid, costumerName, contactName , contactEmail,contactCellPhone,costumerAddress,contactDepartment,costumer_insideNum) values ('" + CostNum_label.Content + "','" + max + "','" + CostName_label.Content + "','" + contact_name + "','" + contact_email + "','" + contact_cellphone + "','" + cosADDs + "','" + contact_dep + "','" + cos_num_label.Content + "')");
                     }
                     if (f5 && f6) // if both
                     {
                         query = ("insert into project.costumers (costumerid, contactid, costumerName, contactName , contactEmail,contactPhone,contactCellPhone,costumerAddress,contactDepartment,costumer_insideNum) values ('" + CostNum_label.Content + "','" + max + "','" + CostName_label.Content + "','" + contact_name + "','" + contact_email + "','" + contact_phone + "','" + contact_cellphone + "','" + cosADDs + "','" + contact_dep + "','" + cos_num_label.Content + "')");
                     }
                    //string query = ("insert into project.costumers (costumerid, contactid, costumerName, contactName , contactEmail,contactPhone,costumerAddress,contactDepartment) values ('" + CostNum_label.Content + "','" + max + "','" + CostName_label.Content + "','" + contact_name + "','" + contact_email + "','" + contact_phone + "','" + cosADDs + "','" + contact_dep + "')");
                    DBConnection DBC = new DBConnection();
                    DBC.InsertDataIntoDB(Login.Connectionstring, query);
                    contact_name_textBox.Clear();
                    email_textBox1.Clear();
                    cont_phone_text.Clear();
                    cell_textBox.Clear();
                    cont_dep_text.Clear();
                    try
                    {
                        MySqlConnection MySqlConn1 = new MySqlConnection(Login.Connectionstring);
                        MySqlConn1.Open();
                        string Query1 = ("select contactid as `מספר איש קשר`,contactName as `שם איש קשר` ,contactEmail as `אימייל איש קשר` ,contactPhone as `טלפון איש קשר`,contactCellPhone as `טלפון נייד של איש הקשר` ,contactDepartment as `מחלקת איש קשר`, contactDesc as `הערות לגבי איש הקשר` from costumers  where costumerid='" + CostNum_label.Content + "'");
                        MySqlCommand MSQLcrcommand11 = new MySqlCommand(Query1, MySqlConn1);
                        MSQLcrcommand11.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand11);
                        SecContactsGUI.dt.Clear();
                        mysqlDAdp.Fill(SecContactsGUI.dt);
                        mysqlDAdp.Update(SecContactsGUI.dt);
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




        /// <summary>
        /// Handles the clicked event of the exit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void exit_clicked(object sender, CancelEventArgs e)
        {
            Console.WriteLine("" + Login.close);

            if (Login.close == 0) // then the user want to exit.
            {
                if (MessageBox.Show("?האם אתה בטוח שברצונך לסגור את החלון ", "וידוא יציאה מהוספת איש קשר חדש", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    e.Cancel = true; ; //don't exit.
                }
            }

            Login.close = 0;
           
        }





    }
}
