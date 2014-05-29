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

        string hpcusid;
        string internalcusid;
        string custname;
        string cusaddress;
        string contact_name;
        string contact_email;
        string contact_phone;
        string contact_cellphone;
        string contact_dep;

        public ManagerAddNewCusGUI()
        {
            InitializeComponent();
            hpcusid_W_label.Visibility = Visibility.Hidden;
            internalcusid_W_label.Visibility = Visibility.Hidden;
            cusname_W_label.Visibility = Visibility.Hidden;
            cusaddress_W_label.Visibility = Visibility.Hidden;
            name_W_label.Visibility = Visibility.Hidden;
            mail_W_label.Visibility = Visibility.Hidden;
            phone_W_label.Visibility = Visibility.Hidden;
            both_W_label.Visibility = Visibility.Hidden;
            cell_W_label.Visibility = Visibility.Hidden;
            dep_W_label.Visibility = Visibility.Hidden;
            //Login.close = 1;
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
            hpcusid_W_label.Visibility = Visibility.Hidden;
            internalcusid_W_label.Visibility = Visibility.Hidden;
            cusname_W_label.Visibility = Visibility.Hidden;
            cusaddress_W_label.Visibility = Visibility.Hidden;

            name_W_label.Visibility = Visibility.Hidden;
            mail_W_label.Visibility = Visibility.Hidden;
            phone_W_label.Visibility = Visibility.Hidden;
            both_W_label.Visibility = Visibility.Hidden;
            cell_W_label.Visibility = Visibility.Hidden;
            dep_W_label.Visibility = Visibility.Hidden;

            bool f1 = false, f2 = false, f3 = false, f4 = false, f5 = false, f6 = false, f7 = false, f8 = false, f9 = false, f10 = false, f11 = false, f12 = false;
            if (!string.IsNullOrWhiteSpace(hpcusid_textBox.Text))
            {
                try
                {
                    int hpcustideCheck = Convert.ToInt32(hpcusid_textBox.Text);
                    hpcusid = hpcusid_textBox.Text;
                    f1 = true;
                }
                catch
                {
                    hpcusid_W_label.Content = "ח.פ. הלקוח חייב להכיל מספרים בלבד!";
                    hpcusid_W_label.Visibility = Visibility.Visible;
                    //MessageBox.Show("!ח.פ. הלקוח חייב להכיל מספרים בלבד");
                    
                }
            }
            else
            {
                hpcusid_W_label.Content = "אנא הכנס חפ לקוח";
                hpcusid_W_label.Visibility = Visibility.Visible;
                //MessageBox.Show("אנא הכנס חפ לקוח "); 
            }


            if (!string.IsNullOrWhiteSpace(internalcusid_textBox.Text))
            {
                try
                {
                    int hpcustideCheck = Convert.ToInt32(internalcusid_textBox.Text);
                    internalcusid = internalcusid_textBox.Text;
                    f8 = true;
                }
                catch
                {
                    internalcusid_W_label.Content = "מספר הלקוח חייב להכיל מספרים בלבד!";
                    internalcusid_W_label.Visibility = Visibility.Visible;
                    //MessageBox.Show("!מספר הלקוח חייב להכיל מספרים בלבד");
                }   
            }
            else
            {
                internalcusid = "לא הוזן";
                f8 = true;
              //  internalcusid_W_label.Content = "אנא הכנס מס לקוח";
              //  internalcusid_W_label.Visibility = Visibility.Visible;
                //MessageBox.Show("אנא הכנס מס לקוח ");
            }



            // if (firstname_textBox.Text != null)
            if (!string.IsNullOrWhiteSpace(custname_textBox.Text))
            {
                custname = custname_textBox.Text;
                f2 = true;
                //  MessageBox.Show("" + username + "");
            }
            else
            {
                cusname_W_label.Content = "אנא הכנס שם לקוח";
                cusname_W_label.Visibility = Visibility.Visible;
                //MessageBox.Show("אנא הכנס שם לקוח");
            }



            //  if (address_textBox != null)
            if (!string.IsNullOrWhiteSpace(address_textBox.Text))
            {
                cusaddress = address_textBox.Text;
                f3 = true;
            }
            else
            {
                cusaddress_W_label.Content = "אנא הכנס כתובת לקוח";
                cusaddress_W_label.Visibility = Visibility.Visible;
                //MessageBox.Show("אנא הכנס כתובת לקוח ");
            }

            // if (email_textBox1.Text != null)
            if (!string.IsNullOrWhiteSpace(email_textBox1.Text))
            {
                if ((Regex.IsMatch(this.email_textBox1.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")))
                {
                    contact_email = email_textBox1.Text;
                    //   MessageBox.Show("" + email + "");
                    f4 = true;
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
                f4 = true;
               // mail_W_label.Content = "אנא הכנס כתובת אימייל";
              //  mail_W_label.Visibility = Visibility.Visible;
                //MessageBox.Show("אנא הכנס כתובת אימייל");
            }



            if (!string.IsNullOrWhiteSpace(contact_name_textBox.Text))
            {
                contact_name = contact_name_textBox.Text;
                f5 = true;
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
                    f10 = true; //phone
                }
                catch
                {
                    f11 = true;
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
                    f9 = true; //cell
                }
                catch
                {
                    f12 = true;
                    cell_W_label.Content = "מספר נייד חייב להכיל מספרים בלבד!";
                    cell_W_label.Visibility = Visibility.Visible;
                    //MessageBox.Show("!מספר הטלפון נייד חייב להכיל מספרים בלבד");
                }
                
            }
            if (f9 || f10) //user enterd phone and/or cellphone correctly.
            {
                if (!f11 && !f12) // if non was wrong.
                {
                    f6 = true;
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
                f7 = true;
            }
            else
            {
                contact_dep = "לא הוזן";
                f7 = true;
               // dep_W_label.Content = "אנא הכנס מחלקת איש קשר";
              //  dep_W_label.Visibility = Visibility.Visible;
                //MessageBox.Show("אנא הכנס מחלקת איש קשר "); 
            }




            // if all is ok then add new user to the DB.
            if (f1 && f2 && f3 && f4 && f5 && f6 && f7 && f8)
            {

                int hp = 0;
                int count = 0;
                //string not = "לא מחובר";
                // string query = ("insert into project.costumers (costumerid, costumerName, contactName , contactEmail,contactPhone,costumerAddress,contactDepartment) values ('" + hpcusid + "','" + custname + "','" + contact_name + "','" + contact_email + "','" + contact_phone + "','" + cusaddress + "','" + contact_dep + "')");
                try
                {
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string query1 = ("select costumerid from costumers where costumerid='" + hpcusid + "'");
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();
                    MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                    while (dr.Read())
                    {

                        hp++;
                
                    }
                    MySqlConn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if (internalcusid != "לא הוזן")
                {
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string query2 = ("select costumer_insideNum from costumers where costumer_insideNum='" + internalcusid + "' AND costumer_insideNum !='לא הוזן` ");
                        MySqlCommand MSQLcrcommand2 = new MySqlCommand(query2, MySqlConn);
                        MSQLcrcommand2.ExecuteNonQuery();
                        MySqlDataReader dr2 = MSQLcrcommand2.ExecuteReader();
                        while (dr2.Read())
                        {

                            count++;

                        }
                        MySqlConn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
                else
                {
                    count=0;
                }

                //MessageBox.Show("" + count + "");
                if (hp == 0 && count == 0)
                {
                    // if only phone
                    string query = ("insert into project.costumers (costumerid, contactid, costumerName, contactName , contactEmail,contactPhone,costumerAddress,contactDepartment,costumer_insideNum) values ('" + hpcusid + "','1','" + custname + "','" + contact_name + "','" + contact_email + "','" + contact_phone + "','" + cusaddress + "','" + contact_dep + "','" + internalcusid + "')");

                    if (!f10 && f9) // if only cell
                    {
                        query = ("insert into project.costumers (costumerid, contactid, costumerName, contactName , contactEmail,contactCellPhone,costumerAddress,contactDepartment,costumer_insideNum) values ('" + hpcusid + "','1','" + custname + "','" + contact_name + "','" + contact_email + "','" + contact_cellphone + "','" + cusaddress + "','" + contact_dep + "','" + internalcusid + "')");
                    }
                    if (f9 && f10) // if both
                    {
                        query = ("insert into project.costumers (costumerid, contactid, costumerName, contactName , contactEmail,contactPhone,contactCellPhone,costumerAddress,contactDepartment,costumer_insideNum) values ('" + hpcusid + "','1','" + custname + "','" + contact_name + "','" + contact_email + "','" + contact_phone + "','" + contact_cellphone + "','" + cusaddress + "','" + contact_dep + "','" + internalcusid + "')");
                    }

                    //string query = ("insert into project.costumers (costumerid, contactid, costumerName, contactName , contactEmail,contactPhone,costumerAddress,contactDepartment) values ('" + hpcusid + "','  1 ','" + custname + "','" + contact_name + "','" + contact_email + "','" + contact_phone + "','" + cusaddress + "','" + contact_dep + "')");
                    DBConnection DBC = new DBConnection();
                    DBC.InsertDataIntoDB(Login.Connectionstring, query);
                    hpcusid_textBox.Clear();
                    internalcusid_textBox.Clear();
                    custname_textBox.Clear();
                    address_textBox.Clear();
                    contact_name_textBox.Clear();
                    email_textBox1.Clear();
                    cont_phone_text.Clear();
                    cell_textBox.Clear();
                    cont_dep_text.Clear();
                    try
                    {
                        MySqlConnection MySqlConn1 = new MySqlConnection(Login.Connectionstring);
                        MySqlConn1.Open();
                        string Query1 = ("SELECT costumerid as `חפ לקוח`,costumerName as `שם לקוח` ,costumer_insideNum as `מספר לקוח`,costumerAddress as `כתובת לקוח`,costumerDesc as `הערות בקשר ללקוח` from project.costumers group by costumerid");
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
                    if (hp > 0 && count > 0)
                    {
                        MessageBox.Show("מספר ח.פ. ומספר לקוח שהוזנו כבר קיימים במערכת", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (hp > 0)
                    {
                        MessageBox.Show("מספר ח.פ. שהוזן כבר קיים במערכת", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("מספר לקוח כבר קיים במערכת ", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }

        }




        private void exit_clicked(object sender, CancelEventArgs e)
        {
            Console.WriteLine("" + Login.close);

            if (Login.close == 0) // then the user want to exit.
            {
                if (MessageBox.Show("?האם אתה בטוח שברצונך לסגור את החלון ", "וידוא יציאה מהוספת לקוח חדש", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    e.Cancel = true; ; //don't exit.
                }
            }

            Login.close = 0;
            /*
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
            */
        }
    }
}

