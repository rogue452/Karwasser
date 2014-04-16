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
                string Query1 = ("select empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון` from project.employees ");
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







        private void ExcelBtn_Click(object sender, RoutedEventArgs e)
        {

            ExportToExcel();
        }





        private void ExportToExcel()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון` from project.employees ");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("employess");
                mysqlDAdp.Fill(dt);
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "רשימת עובדים" + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString(); ; // Default file name
                dialog.DefaultExt = ".xlsx"; // Default file extension
                dialog.Filter = "Microsoft Excel 2003 and above Documents (.xlsx)|*.xlsx";  // |Text documents (.txt)|*.txt| Filter files by extension 

                // Show save file dialog box
                Nullable<bool> result = dialog.ShowDialog();

                // Process save file dialog box results 
                if (result == true)
                {
                    string saveto = dialog.FileName;
                    CreateExcelFile.CreateExcelDocument(dt, saveto);
                    MessageBox.Show(" נוצר בהצלחה Microsoft Excel -מסמך ה");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                string Query1 = "select empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון` from  employees where  emp_firstname Like '%" + searchkey + "%' ";
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
                string Query1 = "select empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון` from employees where  empid Like '%" + searchidkey + "%' ";
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

              try
                {
                    // this will give us the first colum of the selected row in the DataGrid.
                    System.Collections.IList rows = dataGrid1.SelectedItems;
                    DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                    if (MessageBox.Show("?האם אתה בטוח שברצונך למחוק עובד זה", "וידוא מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        //do no stuff
                    }
                    else // if the user clicked on "Yes" so he wants to Delete.
                    {
              
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
                            MessageBox.Show("!העובד נמחק מהמערכת");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = ("select empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון` from project.employees ");
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
                   }//end else

                

                }//end try
              catch { MessageBox.Show("לא נבחר עובד למחיקה"); }
        }


        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                    DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                    
                    if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן עובד זה", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        //dont do stuff
                    }
                    else // if the user clicked on "Yes" so he wants to Update.
                        {
                            string selected = row["תעודת זהות"].ToString();
                            string firstname = row["שם פרטי"].ToString();
                            string lastname = row["שם משפחה"].ToString();
                            string address = row["כתובת"].ToString();
                            string phone = row["מספר טלפון"].ToString();

                            try
                            {

                                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                MySqlConn.Open();
                                string Query1 = "update employees set empid='" + selected + "',emp_firstname='" + firstname + "',emp_lastname='" + lastname + "',emp_address='" + address + "',emp_phone='" + phone + "'where empid='" + selected + "'";
                                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                                MSQLcrcommand1.ExecuteNonQuery();
                                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                                MySqlConn.Close();
                                MessageBox.Show("!פרטי עובד עודכנו");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            try
                            {
                                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                MySqlConn.Open();
                                string Query1 = ("select empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון` from project.employees ");
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
                        }//end else
                        
  
            }//end try
            catch { MessageBox.Show("לא נבחר עובד לעדכון "); }
        }//end function




        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "תעודת זהות")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }

        }






        // go to previous screen.
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerGui MG = new ManagerGui();
            MG.Show();
            this.Close();
        }




    } //end class
} // end namespace




