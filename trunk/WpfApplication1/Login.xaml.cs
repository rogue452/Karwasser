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
using System.IO;

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
        public static string my_host_name;

        public Login()
        {
            InitializeComponent();
            client = false;
            Req_Host_label.Visibility = Visibility.Hidden;
            Host_textBox.Visibility = Visibility.Hidden;
           // My_IP();
            My_Host_Name();
            host_name_label.Content = my_host_name;
      
            // checks if the text file exists and if does then the text in the file will be loaded to the Host_textBox.
            if (File.Exists("C:/Users/Public//Host Name Login.txt"))
            {
                //if we want to load the text from a file the we will enable this line (and also the using System.IO line above) and we will give it the text Source path.
                Host_textBox.Text = File.ReadAllText("C:/Users/Public//Host Name Login.txt");
            }
            this_cpu_HostName_label.Visibility = Visibility.Visible;
            host_name_label.Visibility = Visibility.Visible;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }



        private void button1_Click(object sender, RoutedEventArgs e)//כפתור כניסה 
        {
            try
            {
                if (client.Equals(true)) // if this is a remote computer. 
                {
                    serverip = this.Host_textBox.Text;
                    Connectionstring = "Server=" + serverip + "; Database=project; UId=root;Password=1234;";
                   // MessageBox.Show("" + Connectionstring +"");
                }
                else if (client.Equals(false)) // // if this is the host computer (the one with the SQL DataBase on it).
                {
                    Connectionstring = "Server=localhost;Database=project; UId=root;Password=1234;";
                }
               // string Connectionstring = " Server=localhost;Database=project; UId=root;Password=1234;";
                MySqlConnection objc = new MySqlConnection(Connectionstring);
                try
                {
                   // MessageBox.Show("ניסיון התחברות");
                    objc.Open();
                   // MessageBox.Show("התחברות הצליחה");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
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
                  //  string Connectionstring1 = " Server=localhost;Database=project; UId=root;Password=1234;";
                 //   MySqlConnection objc1 = new MySqlConnection(Connectionstring1);
                    MySqlConnection objc1 = new MySqlConnection(Connectionstring);
                    try
                    {
                        //MessageBox.Show(" 1 ניסיון התחברות");
                        objc1.Open();
                       // MessageBox.Show("התחברות הצליחה 1");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    string Query1 = "select users.userid, employees.emp_firstname, employees.emp_lastname, users.user_name,  users.connected, users.email from project.users, project.employees where users.empid=employees.empid and users.user_name='" + this.textBox1.Text + "'and users.password='" + this.textBox2.Password + "'";
                    //MySqlCommand crcommand1 = new MySqlCommand(Query1, objc);
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
                    //MessageBox.Show(""+connected+"");
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



        //  This button will open on click a new Forgot_pass window
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            Forgot_pass fp = new Forgot_pass();
            fp.Show();
            this.Close();
        }

        private void Clinet_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            client = true;
            Req_Host_label.Visibility = Visibility.Visible;
            Host_textBox.Visibility = Visibility.Visible;
            this_cpu_HostName_label.Visibility = Visibility.Hidden;
            //myip_label.Visibility = Visibility.Hidden;
            host_name_label.Visibility = Visibility.Hidden;
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
            Req_Host_label.Visibility = Visibility.Hidden;
            Host_textBox.Visibility = Visibility.Hidden;
            this_cpu_HostName_label.Visibility = Visibility.Visible;
            host_name_label.Visibility = Visibility.Visible;
           // myip_label.Visibility = Visibility.Visible;
        }


        public static void My_Host_Name()
        {
            my_host_name = System.Environment.MachineName;
        }



        public static void My_IP()
         // public static bool IsLocalIpAddress(string host)
          {
           try
            {
              try
              {
                  IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                  IPAddress[] address = hostEntry.AddressList;
                  myip = address.GetValue(2).ToString();

              }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message);
              }

               // maybe we will change it from IP to Hostname, i will see what i can do.

               // i think this will give the IP on Windows XP
            try
            {
                  IPAddress[] ip = Dns.GetHostAddresses(Dns.GetHostName());
                  foreach (IPAddress theaddress in ip)
                  {
                      myip = theaddress.ToString();
                  }

                  }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message);
              }

                  //!!!!הפונקציה הזאת מציגה לי את האי פי השני- לא למחוק אותה עד שאדע איזה איפי מתאים
               /*   var defaultGateway =
                  from nics in NetworkInterface.GetAllNetworkInterfaces()
                  from props in nics.GetIPProperties().GatewayAddresses
                  where nics.OperationalStatus == OperationalStatus.Up
                  select props.Address.ToString();
                  myip = defaultGateway.First();*/








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

        private void MySQL_button_Click(object sender, RoutedEventArgs e)
        {
            MySQLPasswordREQuestGui MSQLPRG = new MySQLPasswordREQuestGui();
            MSQLPRG.Show();
            this.Close();
        } 
    }
}
