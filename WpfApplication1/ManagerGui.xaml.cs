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
   
    /// Interaction logic for ManagerGui.xaml
    
    public partial class ManagerGui : Window
    {
        public ManagerGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            NameLabel.Content = "                                    שלום "+ Login.user_name +"!\n               אנא בחר/י מה ברצונך/ה לעשות.";
            CPUName_label.Content = Login.my_host_name;
        }




        private void Users_Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerUsersGui MUG = new ManagerUsersGui();
            MUG.Show();
            this.Close();      
        }




        private void Employees_Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerEMPGui MEG = new ManagerEMPGui();
            MEG.Show();
            this.Close();
        }

        private void Custumer_Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerCusGui MCG = new ManagerCusGui();
            MCG.Show();
            this.Close();

        }

        private void job_btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerJobGui MJG = new ManagerJobGui();
            MJG.Show();
            this.Close();

        }

        private void MySQL_Backup_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlCommand MSQLcrcommand1 = new MySqlCommand();

                using (MySqlBackup mb = new MySqlBackup(MSQLcrcommand1))
                    {
                        SaveFileDialog dialog = new SaveFileDialog();
                        dialog.FileName = "MySQL" + " Backup - from date " + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " and time " + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString(); ; // Default file name
                        dialog.DefaultExt = ".sql"; // Default file extension
                        dialog.Filter = "MySQL Backup File (.SQL)|*.SQL";  // |Text documents (.txt)|*.txt| Filter files by extension 

                        // Show save file dialog box
                        Nullable<bool> result = dialog.ShowDialog();

                        // Process save file dialog box results 
                        if (result == true)
                        {
                            string saveto = dialog.FileName;

                            //the missing schema string.
                            string sche = "--\n-- Create schema project by Shuki Porat\n-- \n\nCREATE DATABASE IF NOT EXISTS project; \nUSE project;\n\n";
                            // the schepath is the path for the sche string and the tempPlace  is the path for the temp sql dump file.
                            string schepath = "C://Users/Public//sche.sql";
                            string tempPlace = "C://Users/Public//temp.sql";

                            //create the sche.sql with the sche string.
                            File.WriteAllText(schepath , sche);
                            MSQLcrcommand1.Connection = MySqlConn;
                            MySqlConn.Open();
                            MessageBox.Show("פעולה זו עלולה להמשך כמה דקות, הודעה תופיע בסוף התהליך");

                            // create the sql dump file to the tempPlace path.
                            mb.ExportToFile(tempPlace);
                            MySqlConn.Close();

                            // Append the sql dump to the string file.
                            File.AppendAllText(schepath, File.ReadAllText(tempPlace));

                            // copy the file to the user selected place.
                            File.WriteAllText(saveto, File.ReadAllText(schepath));
                            MessageBox.Show("נשמר בהצלחה למיקום שצויין SQL -קובץ גיבוי ה");
                        }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }
    }
}
