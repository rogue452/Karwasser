﻿using System;
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


