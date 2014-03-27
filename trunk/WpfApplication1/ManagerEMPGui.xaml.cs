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
using System.ComponentModel;
using Microsoft.Win32;

namespace project
{
    /// <summary>
    /// Interaction logic for ManagerEMPGui.xaml
    /// </summary>
    public partial class ManagerEMPGui : Window
    {
        public ManagerEMPGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מס טלפון` from project.employees ");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("employess");
                mysqlDAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                mysqlDAdp.Update(dt);
                MySqlConn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }







        private void TXTBtn_Click(object sender, RoutedEventArgs e)
        {

            ExportToTXT();
        }





        private void ExportToTXT()
        {

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "רשימת עובדים"; // Default file name
            dialog.DefaultExt = ".text"; // Default file extension
            dialog.Filter = "Text documents (.txt)|*.txt";  //EXcel documents (.xlsx)|*.xlsx";    // Filter files by extension 

            // Show save file dialog box
            Nullable<bool> result = dialog.ShowDialog();

            // Process save file dialog box results 
            if (result == true)
            {
                dataGrid1.SelectAllCells();
                dataGrid1.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, dataGrid1);
                String result1 = (string)Clipboard.GetData(DataFormats.Text);
                dataGrid1.UnselectAllCells();
                string saveto = dialog.FileName;
                System.IO.StreamWriter file = new System.IO.StreamWriter(@saveto, false, Encoding.Default);

                file.WriteLine(result1.Replace("‘,’", "‘ ‘"));
                file.Close();
                file.Dispose();
                // Save document 
                MessageBox.Show("                                                                             !קובץ הטקסט נשמר\n\n           :כדי לפתוח באקסל מומלץ להשתמש ב''פתיחה באמצעות'' ולבחור ב\n\n                                                 ''Microsoft Excel''");
            }
        
        
        
        
        
        
        
        
        }

        private void FirstNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                // string Connectionstring = " Server=localhost;Database=project; UId=root;Password=1234;";
                // MySqlConnection MySqlConn = new MySqlConnection(Connectionstring);
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchkey = this.FirstNameSearchTextBox.Text;
                string Query1 = "select empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מס טלפון` from  employees where  emp_firstname Like '%" + searchkey + "%' ";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("employess");
                mysqlDAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }





        private void IDSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           try
                {
             
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchidkey = this.IDSearchTextBox.Text;
                string Query1 = "select empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מס טלפון` from employees where  empid Like '%" + searchidkey + "%' ";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("employees");
                mysqlDAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
                }
           catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }    
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
             ManagerAddEmployeeGUI MAEG = new ManagerAddEmployeeGUI();
            MAEG.Show();
            this.Close();

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("?האם אתה בטוח שברצונך למחוק עובד זה", "וידוא מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
            }
            else // if the user clicked on "Yes" so he wants to Delete.
            {
                // this will give us the first colum of the selected row in the DataGrid.
                System.Collections.IList rows = dataGrid1.SelectedItems;
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["תעודת זהות"].ToString();
                // MessageBox.Show("" + selected + "");

                try
                {
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = "delete from employees where empid ='" + selected + "'";
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();
                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                    MySqlConn.Close();
                    MessageBox.Show("!המשתמש נמחק מהמערכת");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                 try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מס טלפון` from project.employees ");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("employess");
                mysqlDAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                mysqlDAdp.Update(dt);
                MySqlConn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
            }
           
        }
    }


            /*
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DBConnection conn = new DBConnection();
            string query = ("select * from users");
            dataGrid1.ItemsSource = conn.GetDataTableFromDB(query).Tables[0].DefaultView;
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
                printDlg.PrintVisual(dataGrid1, "DataGrid Printing.");
            }
        }*/


