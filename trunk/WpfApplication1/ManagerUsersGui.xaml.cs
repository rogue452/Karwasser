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
//SelectionChanged="dataGrid1_SelectionChanged" הורדתי את זה מהXML
using MySql.Data.MySqlClient;
using System.Data;
using System.ComponentModel;
using Microsoft.Win32;


namespace project
{
    /// <summary>
    /// Interaction logic for UsersGui.xaml
    /// </summary>
    public partial class ManagerUsersGui : Window
    {
        public ManagerUsersGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
         //   ListCollectionView CollectionViewList;
          //  DataTable DataTableFiltered;
         //   DataSet ds = new DataSet();

            try
            {
                string Connectionstring = " Server=localhost;Database=project; UId=root;Password=1234;";
                MySqlConnection MySqlConn = new MySqlConnection(Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select users.empid as `תעודת זהות`,employees.emp_firstname as `שם פרטי` ,employees.emp_lastname as `שם משפחה` ,users.user_name as `שם משתמש` ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as `כתובת אימייל` from project.users , project.employees where users.empid=employees.empid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("users");
                mysqlDAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView; 

            //    mysqlDAdp.Fill(ds);
            //    ICollectionView UsersView = CollectionViewSource.GetDefaultView(dt);
            //    UsersView.Filter
           //     CollectionViewList = new ListCollectionView(ds);
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        /*
            DBConnection conn = new DBConnection();
            string query = ("select userid as מספר__משתמש ,first_name as שם__פרטי ,last_name as שם__משפחה ,user_name as שם__משתמש ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as כתובת__אימייל from users");
            dataGrid1.ItemsSource = conn.GetDataTableFromDB(query).Tables[0].DefaultView;
            DataTable dt = new DataTable();
            
        */
        }


        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
                printDlg.PrintVisual(dataGrid1, "DataGrid Printing.");
            }
        }






        private void TXTBtn_Click(object sender, RoutedEventArgs e)
        {
           
            ExportToTXT();
        }





        private void ExportToTXT()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "רשימת משתמשים"; // Default file name
            dialog.DefaultExt = ".text"; // Default file extension
            dialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension 

            // Show save file dialog box
            Nullable<bool> result = dialog.ShowDialog();

            // Process save file dialog box results 
            if (result == true)
            {
                dataGrid1.SelectAllCells();
                dataGrid1.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, dataGrid1);
                String resultat1 = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                String result1 = (string)Clipboard.GetData(DataFormats.Text);
                dataGrid1.UnselectAllCells();
                string saveto = dialog.FileName;
                System.IO.StreamWriter file = new System.IO.StreamWriter(@saveto);
                //   System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Shuki\Dropbox\Braude\לימודים\Final Pro\Scoend Stage\p\פרוייקט\Excel\test.txt");
                //   file.WriteLine(result.Replace("s","d");

                file.WriteLine(result1.Replace("‘,’", "‘ ‘"));
                file.Close();

                // Save document 
                string filename = dialog.FileName;
                MessageBox.Show("Unicode קובץ הטקסט נוצר - להצגה באקסל שמור מחדש בתבנית ");
            }

     /*   dataGrid1.SelectAllCells();
        dataGrid1.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
        ApplicationCommands.Copy.Execute(null, dataGrid1);
        String resultat = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
        String result = (string)Clipboard.GetData(DataFormats.Text);
        dataGrid1.UnselectAllCells();
        System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Shuki\Dropbox\Braude\לימודים\Final Pro\Scoend Stage\p\פרוייקט\Excel\test.txt");
     //   file.WriteLine(result.Replace("s","d");
            
            file.WriteLine(result.Replace("‘,’", "‘ ‘"));
        file.Close();

        MessageBox.Show("Unicode קובץ הטקסט נוצר - להצגה באקסל שמור מחדש בתבנית ");
      */
        }



        private void FirstNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string Connectionstring = " Server=localhost;Database=project; UId=root;Password=1234;";
                MySqlConnection MySqlConn = new MySqlConnection(Connectionstring);
                MySqlConn.Open();
                String searchkey = this.FirstNameSearchTextBox.Text;
                string Query1 = "select users.empid as `תעודת זהות` ,employees.emp_firstname as `שם פרטי` ,employees.emp_lastname as `שם משפחה` ,users.user_name as `שם משתמש` ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as `כתובת אימייל` from project.users , project.employees where users.empid=employees.empid and employees.emp_firstname Like '%" + searchkey + "%' ";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("users");
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
                string Connectionstring = " Server=localhost;Database=project; UId=root;Password=1234;";
                MySqlConnection MySqlConn = new MySqlConnection(Connectionstring);
                MySqlConn.Open();
                String searchidkey = this.IDSearchTextBox.Text;
                string Query1 = "select users.empid as `תעודת זהות` ,employees.emp_firstname as `שם פרטי` ,employees.emp_lastname as `שם משפחה` ,users.user_name as `שם משתמש` ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as `כתובת אימייל` from project.users , project.employees where users.empid=employees.empid and users.empid Like '%" + searchidkey + "%' ";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("users");
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
