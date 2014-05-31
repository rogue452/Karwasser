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
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;

namespace project
{
    /// <summary>
    /// Interaction logic for MySQLImpotrGui.xaml
    /// </summary>
    public partial class MySQLImpotrGui : Window
    {
        public MySQLImpotrGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
          //  Login.close = 1;
        }


        // this finction will reset the root password to our password giving the current password from the user.
        private void Reset_button_Click(object sender, RoutedEventArgs e)
        {
            string pass = Root_password.Password;
            try
            {
                string Connectionstring = "Server=localhost;Database=project; UId=root;Password='" + pass + "' ";
                MySqlConnection MySqlConn = new MySqlConnection(Connectionstring);
                MySqlConn.Open();
                string Query1 = ("UPDATE mysql.user SET Password=PASSWORD('1234') WHERE User='root';\nFLUSH PRIVILEGES;");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                MySqlConn.Close();
                MessageBox.Show("הסיסמה עודכנה", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // this function will allow remote host to reach this program DataBase.
        private void remote_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Connectionstring = "Server=localhost;Database=project; UId=root;Password=1234 ";
                MySqlConnection MySqlConn = new MySqlConnection(Connectionstring);
                MySqlConn.Open();
                string Query1 = ("GRANT ALL PRIVILEGES ON project.* TO 'root'@'%' IDENTIFIED BY '1234';\nflush PRIVILEGES ;");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                MySqlConn.Close();
                MessageBox.Show("האיפשור עודכן", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }




        //this function will import from the user chosen file to the MySQL DB. 
        private void Import_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".sql"; // Default file extension
            dialog.Filter = "SQL File (.sql)|*.sql";   // |Text documents (.txt)|*.txt| Filter files by extension
             
            // Show save file dialog box
            Nullable<bool> result = dialog.ShowDialog();

            // Process save file dialog box results 
            if (result == true)
            {
                string Connectionstring = "Server=localhost; UId=root;Password=1234 ";
                string from = dialog.FileName;
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(Connectionstring))
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                                {
                                        using (MySqlBackup mb = new MySqlBackup(cmd))
                                        {
                                            cmd.Connection = conn;
                                            conn.Open();
                                            MessageBox.Show(" מאגר הנתונים יעודכן מהקובץ הנבחר\n.בסיום התהליך תופיע הודעה\n.לחץ אישור להתחלת התהליך", "אנא לחץ אישור", MessageBoxButton.OK, MessageBoxImage.Information);
                                            mb.ImportFromFile(from);
                                            conn.Close();
                                            MessageBox.Show(".SQL - מאגר הנתונים עודכן מקובץ ה", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                                        }
                                }
                        }

                  }// end try
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("SQL - התרחשה שגיאה בייבוא מקובץ ה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        // this will close the current scren and will open the Log In scren. 
        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
         //   Login LI = new Login();
          //  LI.Show();
            this.Close();
        }





        private void exit_clicked(object sender, CancelEventArgs e)
        {
           /* Console.WriteLine("" + Login.close);

            if (Login.close == 0) // then the user want to exit.
            {
            }
            else
            {*/
                Login window = new Login();
                window.Show();
           // }
            //Login.close = 0;
        }




    }
}
