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
using System.Text.RegularExpressions;

namespace project
{
    /// <summary>
    /// Interaction logic for ManagerAddNewJobGUI.xaml
    /// </summary>
    public partial class ManagerAddNewJobGUI : Window
    {
        public ManagerAddNewJobGUI()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
       

            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("custumers");
                mysqlDAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
  
             try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select itemid as `מספר פריט`,itemName as `שם פריט`, discription as `תאור פריט` from project.item group by itemid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt1 = new DataTable("items");
                mysqlDAdp.Fill(dt1);
                dataGrid2.ItemsSource = dt1.DefaultView;
                mysqlDAdp.Update(dt1);

/*
                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.Header = "כמות";
                textColumn.Binding = new Binding("כמות");
                //dataGrid2.Columns.Add(textColumn);
                dataGrid2.Columns.Insert(0, textColumn);

*/

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
                string Query1 = ("select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("custumers");
                mysqlDAdp.Fill(dt);
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "רשימת לקוחות" + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString(); ; // Default file name
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



        private void CustumerNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchkey = this.CustumerNameSearchTextBox.Text;
                string Query1 = "select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers  where  costumerName Like '%" + searchkey + "%' group by costumerid";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("custumers");
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
                string Query1 = "select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers  where  costumerid Like '%" + searchidkey + "%' group by costumerid";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                DataTable dt = new DataTable("Custumers");
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

        private void ADD_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerAddNewCusGUI MACG = new ManagerAddNewCusGUI();
            MACG.Show();
            this.Close();
        }



        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Collections.IList rows = dataGrid1.SelectedItems;
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                if (MessageBox.Show("?האם אתה בטוח שברצונך למחוק לקוח זה", "וידוא מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else // if the user clicked on "Yes" so he wants to Delete.
                {
                    // this will give us the first colum of the selected row in the DataGrid.

                    string selected = row["מספר לקוח"].ToString();
                    // MessageBox.Show("" + selected + "");

                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "delete from costumers where costumerid='" + selected + "'";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();
                        MessageBox.Show("!הלקוח נמחק מהמערכת");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        DataTable dt = new DataTable("custumers");
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
            catch { MessageBox.Show("לא נבחר לקוח למחיקה"); }

        }//end function



        // go to previous screen.
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerGui MG = new ManagerGui();
            MG.Show();
            this.Close();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Collections.IList rows = dataGrid1.SelectedItems;

                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["מספר לקוח"].ToString();
                // MessageBox.Show(""+selected+ "");



                if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן לקוח זה", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //dont do stuff
                }
                else // if the user clicked on "Yes" so he wants to Update.
                {
                        string custumername = row["שם לקוח"].ToString();
                        string custumeraddress = row["כתובת לקוח"].ToString();
                      
                        try
                        {

                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = "update costumers set costumerName='" + custumername + "',costumerAddress='" + custumeraddress + "' where costumerid='" + selected + "'";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();
                            MessageBox.Show("!פרטי הלקוח עודכנו");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = ("select costumerid as `מספר לקוח`,costumerName as `שם לקוח` ,costumerAddress as `כתובת לקוח`  from project.costumers group by costumerid");
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            DataTable dt = new DataTable("custumers");
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
            catch { MessageBox.Show("לא נבחר לקוח לעדכון "); }
        }



        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מספר לקוח")  
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }

        }


        private void Grid_AutoGeneratingColumn1(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מספר פריט")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }


            if (e.Column.Header.ToString() == "תאור פריט")
            {
/*
                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.Header = "כמות";
                textColumn.Binding = new Binding("כמות");
                dataGrid2.Columns.Add(textColumn);
                Binding tempmb2 = new Binding();
                tempmb2.Mode = BindingMode.TwoWay;
*/
                DataGridTemplateColumn dgct = new DataGridTemplateColumn();
                dgct.Header = "כמות";
                dgct.SortMemberPath = "כמות";

                Binding b = new Binding("כמות");
                b.Mode = BindingMode.TwoWay;
                #region Editing
                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBox));
                factory.SetValue(TextBox.TextProperty, b);
                DataTemplate cellEditingTemplate = new DataTemplate();
                cellEditingTemplate.VisualTree = factory;
                dgct.CellEditingTemplate = cellEditingTemplate;
                #endregion

                #region View
                FrameworkElementFactory sfactory = new FrameworkElementFactory(typeof(TextBox));
                sfactory.SetValue(TextBox.TextProperty, b);
                DataTemplate cellTemplate = new DataTemplate();
                cellTemplate.VisualTree = sfactory;
                dgct.CellTemplate = cellTemplate;
                #endregion

                dataGrid2.Columns.Add(dgct);





/*
                DataGridTemplateColumn dgtc = new DataGridTemplateColumn
                    {
                        Header = "כמות",
                        CanUserSort = false,
                        CanUserReorder = false,
                    };
                DataTemplate t = new DataTemplate();

                DataTemplate ced = new DataTemplate();
                FrameworkElementFactory f2 = new FrameworkElementFactory(typeof(TextBox));
                Binding tempmb2 = new Binding();

                tempmb2.Mode = BindingMode.TwoWay;

                f2.SetBinding(TextBox.TextProperty, tempmb2);
                ced.VisualTree = f2;
                dgtc.CellEditingTemplate = ced;

                dataGrid2.Columns.Add(dgtc);
*/
            }

        }




        private void Contacts_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["מספר לקוח"].ToString();
                string CosName = row["שם לקוח"].ToString();
                string cosADDs = row["כתובת לקוח"].ToString();
                ManagerContactsGUI MCG = new ManagerContactsGUI(selected, CosName, cosADDs);
                MCG.Show();
                this.Close();
            }
            catch { MessageBox.Show("לא נבחר לקוח"); }
        }
        }  
    }

