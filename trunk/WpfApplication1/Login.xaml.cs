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
using System.Net;
using System.Net.NetworkInformation;

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
        public static string Connectionstring;
        public static Boolean client;
        public static string serverip;
        public static string myip;

        public Login()
        {
            InitializeComponent();
            client = false;
            IP_label.Visibility = Visibility.Hidden;
            IP_textBox.Visibility = Visibility.Hidden;
            My_IP();
            myip_label.Content = myip;
            //IsLocalIpAddress(Dns.GetHostName());
         //   if (IsLocalIpAddress(Dns.GetHostName()) == true)
         //   {
                
         //   }
         //   else
         //   {
        //        myip_label.Content = "IP - בעיה במציאת כתובת ה";
        //    }
            labelIP.Visibility = Visibility.Visible;
            myip_label.Visibility = Visibility.Visible;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;



        //    string localComputerName = Dns.GetHostName();
        //    if (IsLocalIpAddress(localComputerName)==true)
        //    {
                
        //    }
        //    IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
        }



        private void button1_Click(object sender, RoutedEventArgs e)//כפתור כניסה 
        {
            try
            {
                if (client.Equals(true)) // if this is a remote computer.
                {
                    serverip = this.IP_textBox.Text;
                    Connectionstring = " Server=" + serverip + ";Database=project; UId=root;Password=1234;";
                }
                else if (client.Equals(false)) // // if this is the host computer (the one with the SQL DataBase on it).
                {
                    Connectionstring = " Server=localhost;Database=project; UId=root;Password=1234;";
                }
               // string Connectionstring = " Server=localhost;Database=project; UId=root;Password=1234;";
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
                    user_role = dr.GetString(4);
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
                        if (connected != "מחובר" && connected != "לא מחובר")
                        {
                            MessageBox.Show("קיימת בעיה במצב החיבור שלך, יש לפנות למנהל המערכת  ", " שגיאה", MessageBoxButton.OK);
                        }

                        if (connected.Equals("מחובר"))
                        {
                            MessageBox.Show("אתה כבר מחובר למערכת  ", " שגיאה", MessageBoxButton.OK);
                        }


                        if (connected.Equals("לא מחובר"))
                        {
                            MessageBox.Show("      ברוכ/ה הבא/ה " + Login.last_name + " " + Login.first_name + "", "!ההתחברות למערכת בוצעה בהצלחה", MessageBoxButton.OK);
                 //           DBConnection conn = new DBConnection();
                 //         string query2 = "update users set connected='true' where user_name= '" + this.textBox1.Text + "' and password ='" + this.textBox2.Password + "'";
                 //          conn.LogIn(query2);
 
                            if (user_role.Equals("מנהל"))
                            {
                                ManagerGui MG = new ManagerGui();
                                MG.Show();
                            }

                            if (user_role.Equals("מזכירה"))
                            {
                                SecretaryGui SG = new SecretaryGui();
                                SG.Show();
                            }

                            if (user_role.Equals("איכות"))
                            {
                                QualityGui QG = new QualityGui();
                                QG.Show();
                            }

                              this.Close();
                          //    MGui.ShowDialog();
                           // this.Close();
                        }

                   //     else
                   //     {
                  //          MessageBox.Show("קיימת בעיה במצב החיבור שלך, יש לפנות למנהל המערכת  ", " שגיאה", MessageBoxButton.OK);
                   //     }
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

        private void Clinet_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            client = true;
            IP_label.Visibility = Visibility.Visible;
            IP_textBox.Visibility = Visibility.Visible;
            labelIP.Visibility = Visibility.Hidden;
            myip_label.Visibility = Visibility.Hidden;
        }

        private void Clinet_checkBox_UnChecked(object sender, RoutedEventArgs e)
        {
          //  IPAddress[] a = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
           // string ip = a[0].ToString();
          //  myip_label.Content = ip;
     //       if (IsLocalIpAddress(Dns.GetHostName()) == true)
     //       {
    //            myip_label.Content = myip;
     //       }
     //       else 
     //       {
     //           myip_label.Content = "IP - בעיה במציאת כתובת ה";
     //       }
         //   IPHostEntry ipEntry = DNS.GetHostByName(strHostName);
            client = false;
            IP_label.Visibility = Visibility.Hidden;
            IP_textBox.Visibility = Visibility.Hidden;
            labelIP.Visibility = Visibility.Visible;
            myip_label.Visibility = Visibility.Visible;
        }


        public static void My_IP()
         // public static bool IsLocalIpAddress(string host)
          {
              try
              {
               //   IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
              //    IPAddress[] address = hostEntry.AddressList;
              //    myip = address.GetValue(1).ToString();
                  var defaultGateway =
                  from nics in NetworkInterface.GetAllNetworkInterfaces()
                  from props in nics.GetIPProperties().GatewayAddresses
                  where nics.OperationalStatus == OperationalStatus.Up
                  select props.Address.ToString();
                  myip = defaultGateway.First();

                 /* string localHostName = Dns.GetHostName();
                  IPAddress[] ipAddresses = Dns.GetHostAddresses(localHostName);
                  foreach (IPAddress ipAddress in ipAddresses.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                  {
                      myip=ipAddress.ToString();
                  }*/

                  /*// get host IP addresses
                  IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                  // get local IP addresses
                  IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                  // test if any host IP equals to any local IP or to localhost
                  foreach (IPAddress hostIP in hostIPs)
                  {
                      // is localhost
                      if (IPAddress.IsLoopback(hostIP)) return true;
                      // is local address
                      foreach (IPAddress localIP in localIPs)
                      {
                          if (hostIP.Equals(localIP))
                          {
                              myip = localIP.ToString();
                              return true;
                          }
                      }
                  }*/
              }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message);
              }
             // return false;
          } 
    }
}
