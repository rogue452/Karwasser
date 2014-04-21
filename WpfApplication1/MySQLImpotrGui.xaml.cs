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
                MessageBox.Show("הסיסמה עודכנה");
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
                                            MessageBox.Show(" מאר הנתונים יעודכן מהקובץ הנבחר\n.בסיום התהליך תופיע הודעה\n.לחץ אישור להתחלת התהליך");
                                            mb.ImportFromFile(from);
                                            conn.Close();
                                            MessageBox.Show(".SQL - מאר הנתונים עודכן מקובץ ה");
                                        }
                                }
                        }

                  }// end try
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("SQL - התרחשה שגיאה בייבוא מקובץ ה");
                }
            }
        }



        // this will close the current scren and will open the Log In scren. 
        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            Login LI = new Login();
            LI.Show();
            this.Close();
        }
    }
}
