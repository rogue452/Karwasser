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
        public static int close = 0;
        public static string first_name;
        public static string last_name;
        public static string user_role;
        public static string connected;
        public static string empid;
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
            Name_save_button.Visibility = Visibility.Hidden;
           // My_IP();
            My_Host_Name();
            host_name_label.Content = my_host_name;
            
                // checks if the text file exists and if does then the text in the file will be loaded to the Host_textBox.
                if (File.Exists("C:/Users/Public//Host Name Login DO NOT DELETE.txt"))
                {
                    //if we want to load the text from a file the we will enable this line (and also the using System.IO line above) and we will give it the text Source path.
                    Host_textBox.Text = File.ReadAllText("C:/Users/Public//Host Name Login DO NOT DELETE.txt");
                }
            
                else
                    // if this is Windows XP
                {
                    if (File.Exists("C:/Documents and Settings/All Users/Host Name Login DO NOT DELETE.txt"))
                    {
                        //if we want to load the text from a file the we will enable this line (and also the using System.IO line above) and we will give it the text Source path.
                        Host_textBox.Text = File.ReadAllText("C:/Documents and Settings/All Users/Host Name Login DO NOT DELETE.txt");
                    }
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
                string Query = "select * from project.users where empid='" + this.textBox1.Text +"'and password='"+this.textBox2.Password+"'" ;
                MySqlCommand crcommand = new MySqlCommand(Query, objc);
                crcommand.ExecuteNonQuery();
                MySqlDataReader dr = crcommand.ExecuteReader();
                int count = 0;
                while (dr.Read())
                {
                    count++;
                    user_role = dr.GetString(2);
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
                    string Query1 = "SELECT  employees.emp_firstname, employees.emp_lastname, users.connected, users.email FROM project.users, project.employees WHERE users.empid=employees.empid and users.empid='" + this.textBox1.Text + "'and users.password='" + this.textBox2.Password + "'";
                    //MySqlCommand crcommand1 = new MySqlCommand(Query1, objc);
                    MySqlCommand crcommand1 = new MySqlCommand(Query1, objc1);
                    crcommand1.ExecuteNonQuery();
                    MySqlDataReader dr1 = crcommand1.ExecuteReader();
                    int count1 = 0;
                    while (dr1.Read())
                    {
                        count1++;
                        first_name = dr1.GetString(0);
                        last_name = dr1.GetString(1);
                        connected = dr1.GetString(2);                    
                        useremail = dr1.GetString(3);
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
                            empid = this.textBox1.Text;
                            string user_connected = "מחובר";
                            //string user_connected = "לא מחובר";

                            if (user_role.Equals("מנהל"))
                            {
                                DBConnection conn = new DBConnection();
                                Console.WriteLine(my_host_name);
                                string query2 = "UPDATE users SET connected='" + user_connected + "',last_log_in_date='" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "'  ,last_location='" + my_host_name + "' WHERE empid= '" + this.textBox1.Text + "' and password ='" + this.textBox2.Password + "' ";
                                conn.LogIn(query2, Connectionstring);
                                //conn.LogIn(query2);
                                ManagerGui MG = new ManagerGui();
                                MG.Show();
                            }

                            if (user_role.Equals("מזכירה"))
                            {
                                DBConnection conn = new DBConnection();
                                string query2 = "UPDATE users SET connected='" + user_connected + "',last_log_in_date='" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "' ,last_location='" + my_host_name + "' WHERE empid= '" + this.textBox1.Text + "' and password ='" + this.textBox2.Password + "'";
                                //conn.LogIn(query2);
                                conn.LogIn(query2, Connectionstring);
                                SecretaryGui SG = new SecretaryGui();
                                SG.Show();
                            }

                            if (user_role.Equals("איכות"))
                            {
                                DBConnection conn = new DBConnection();
                                string query2 = "UPDATE users SET connected='" + user_connected + "',last_log_in_date='" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "' ,last_location='" + my_host_name + "' WHERE empid= '" + this.textBox1.Text + "' and password ='" + this.textBox2.Password + "'";
                               // conn.LogIn(query2);
                                conn.LogIn(query2, Connectionstring);
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
                  //  textBox1.Clear();
                  //  textBox2.Clear();
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
            fp.Owner = this;
            fp.ShowDialog();
            this.Close();
        }

        private void Clinet_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            client = true;
            Req_Host_label.Visibility = Visibility.Visible;
            Host_textBox.Visibility = Visibility.Visible;
            Name_save_button.Visibility = Visibility.Visible;
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
            host_name_label.Visibility = Visibility.Visible;
            Req_Host_label.Visibility = Visibility.Hidden;
            Host_textBox.Visibility = Visibility.Hidden;
            Name_save_button.Visibility = Visibility.Hidden;
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
            MSQLPRG.Owner = this;
            MSQLPRG.ShowDialog();
            this.Close();
        }

        private void Name_save_button_Click(object sender, RoutedEventArgs e)
        {
            string newhost = Host_textBox.Text, to = "C:/Users/Public//Host Name Login DO NOT DELETE.txt";
            try
            {
                File.WriteAllText(to, newhost);
            }
            catch
            {
                to = "C:/Documents and Settings/All Users/Host Name Login DO NOT DELETE.txt";
                File.WriteAllText(to, newhost);
            }
            if (File.Exists(to))
            {
                //if we want to load the text from a file the we will enable this line (and also the using System.IO line above) and we will give it the text Source path.
                Host_textBox.Text = File.ReadAllText(to);
                MessageBox.Show(".השם נשמר בהצלחה");
            }
            else 
            {
                MessageBox.Show(".התרחשה בעיה בשמירת השם");
            }
        } 







    }
}
