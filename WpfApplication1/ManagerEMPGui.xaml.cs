﻿// ***********************************************************************
// Assembly         : WpfApplication1
// Author           : user
// Created          : 06-10-2014
//
// Last Modified By : user
// Last Modified On : 06-10-2014
// ***********************************************************************
// <copyright file="ManagerEMPGui.xaml.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
        /// <summary>
        /// The dt
        /// </summary>
        public static DataTable dt = new DataTable("employess");
       // public static DataGrid dataGrid1 = new DataGrid();
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerEMPGui"/> class.
        /// </summary>
        public ManagerEMPGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("SELECT empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` , emp_insidenum as `מספר עובד` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון`, emp_cellphone as `טלפון נייד`, emp_start_date as `תאריך התחלת עבודה` FROM project.employees ");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt = new DataTable("employess");
                dt.Clear();
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







        /// <summary>
        /// Handles the Click event of the ExcelBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ExcelBtn_Click(object sender, RoutedEventArgs e)
        {

            ExportToExcel();
        }





        /// <summary>
        /// Exports to excel.
        /// </summary>
        private void ExportToExcel()
        {
            try
            {
                /*
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון` from project.employees ");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt = new DataTable("employess");
                dt.Clear();
                mysqlDAdp.Fill(dt);
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
                  */
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
                    bool success = CreateExcelFile.CreateExcelDocument(dt, saveto);
                    if (success)
                    {
                        MessageBox.Show(" נוצר בהצלחה Microsoft Excel -מסמך ה", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(" לא נוצר  Microsoft Excel -התרחשה שגיאה ולכן מסמך ה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        /// <summary>
        /// Reafreashandclears this instance.
        /// </summary>
        private void reafreashandclear()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("SELECT empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` , emp_insidenum as `מספר עובד` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון`, emp_cellphone as `טלפון נייד`, emp_start_date as `תאריך התחלת עבודה` from project.employees ");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                // DataTable dt = new DataTable("employess");
                dt.Clear();
                mysqlDAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            IDSearchTextBox.Clear();
            FirstNameSearchTextBox.Clear();
        }

        /// <summary>
        /// Handles the TextChanged event of the FirstNameSearchTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void FirstNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                CheckSingleQuotationMark CSQ = new CheckSingleQuotationMark();
                String searchkey = CSQ.checkForSingleQuotationMark(this.FirstNameSearchTextBox.Text);
                //String searchkey = this.FirstNameSearchTextBox.Text;
                string Query1 = "SELECT empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` , emp_insidenum as `מספר עובד` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון`, emp_cellphone as `טלפון נייד`, emp_start_date as `תאריך התחלת עבודה` FROM  employees WHERE  emp_firstname Like '%" + searchkey + "%' ";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt = new DataTable("employess");
                dt.Clear();
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





        /// <summary>
        /// Handles the TextChanged event of the IDSearchTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void IDSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                CheckSingleQuotationMark CSQ = new CheckSingleQuotationMark();
                String searchidkey = CSQ.checkForSingleQuotationMark(this.IDSearchTextBox.Text);
               // String searchidkey = this.IDSearchTextBox.Text;
                string Query1 = "SELECT empid as `תעודת זהות`,emp_firstname as `שם פרטי` ,emp_lastname as `שם משפחה` , emp_insidenum as `מספר עובד` ,emp_address as `כתובת` ,emp_phone as `מספר טלפון`, emp_cellphone as `טלפון נייד`, emp_start_date as `תאריך התחלת עבודה` FROM employees WHERE  empid Like '%" + searchidkey + "%' ";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt = new DataTable("employees");
                dt.Clear();
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

        /// <summary>
        /// Handles the Click event of the AddBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            ManagerAddEmployeeGUI MAEG = new ManagerAddEmployeeGUI();
            //MAEG.Show();
            MAEG.ShowDialog();
            //this.Close();

        }

        /// <summary>
        /// Handles the Click event of the DeleteBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

              try
                {
                    // this will give us the first colum of the selected row in the DataGrid.
                    System.Collections.IList rows = dataGrid1.SelectedItems;
                    DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                    if (MessageBox.Show("?האם אתה בטוח שברצונך למחוק עובד זה", "וידוא מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
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
                            MessageBox.Show("!העובד נמחק מהמערכת", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        reafreashandclear();
                   }//end else

                

                }//end try
              catch { MessageBox.Show("לא נבחר עובד למחיקה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); }
        }


        /// <summary>
        /// Handles the Click event of the UpdateBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                    DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                    
                    if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן עובד זה", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        //dont do stuff
                    }
                    else // if the user clicked on "Yes" so he wants to Update.
                        {
                            if (!string.IsNullOrWhiteSpace(row["מספר טלפון"].ToString()))
                            {
                                try
                                {
                                    int phoneCheck = Convert.ToInt32(row["מספר טלפון"].ToString());
                                }
                                catch
                                {
                                    MessageBox.Show("שדה מספר טלפון לא מכיל רק מספרים", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                    reafreashandclear();
                                    return;
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(row["מספר עובד"].ToString()))
                            {
                                try
                                {
                                    int empCheck = Convert.ToInt32(row["מספר עובד"].ToString());
                                }
                                catch
                                {
                                    MessageBox.Show("שדה מספר עובד לא מכיל רק מספרים", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                    reafreashandclear();
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("אנא הכנס מספר עובד", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                reafreashandclear();
                                return;
                            }

                            if (!string.IsNullOrWhiteSpace(row["טלפון נייד"].ToString()))
                            {

                                try
                                {
                                    int cellphoneCheck = Convert.ToInt32(row["טלפון נייד"].ToString());
                                }
                                catch
                                {
                                    MessageBox.Show("שדה טלפון נייד לא מכיל רק מספרים", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                    reafreashandclear();
                                    return;
                                }
                            }


                            if (string.IsNullOrWhiteSpace(row["שם פרטי"].ToString()))
                            {
                            MessageBox.Show("אנא הכנס שם פרטי", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                reafreashandclear();
                                return;
                            }
                            if (string.IsNullOrWhiteSpace(row["שם משפחה"].ToString()))
                            {
                                MessageBox.Show("אנא הכנס שם משפחה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                reafreashandclear();
                                return;
                            }
                            if (string.IsNullOrWhiteSpace(row["כתובת"].ToString()))
                            {
                                MessageBox.Show("אנא הכנס כתובת", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                reafreashandclear();
                                return;
                            }
                            

                            string selected = row["תעודת זהות"].ToString();
                            string firstname = row["שם פרטי"].ToString();
                            string lastname = row["שם משפחה"].ToString();
                            string address = row["כתובת"].ToString();
                            string phone = row["מספר טלפון"].ToString();
                            string empnum = row["מספר עובד"].ToString();
                            string cell = row["טלפון נייד"].ToString();
                            if ((firstname.Length > 45) || (lastname.Length > 45) || (address.Length > 45) || (phone.Length > 45) || (empnum.Length > 45) || (cell.Length > 45))
                            {
                                MessageBox.Show("אסור ששדה יכיל יותר מ - 45 תוים", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                reafreashandclear();
                                return;
                            }
                            if (row["תאריך התחלת עבודה"].ToString().Equals(""))
                            {
                                try
                                {

                                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                    MySqlConn.Open();
                                    string Query1 = "UPDATE employees SET emp_firstname='" + firstname + "',emp_lastname='" + lastname + "',emp_address='" + address + "',emp_phone='" + phone + "',emp_cellphone='" + cell + "',emp_insidenum='" + empnum + "' ,emp_start_date=NULL WHERE empid='" + selected + "'";
                                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                                    MSQLcrcommand1.ExecuteNonQuery();
                                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                                    MySqlConn.Close();
                                    MessageBox.Show("!פרטי העובד עודכנו", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            else
                            {
                                string start = Convert.ToDateTime(row["תאריך התחלת עבודה"].ToString()).ToString("yyyy-MM-dd");
                                try
                                {

                                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                    MySqlConn.Open();
                                    string Query1 = "UPDATE employees SET emp_firstname='" + firstname + "',emp_lastname='" + lastname + "',emp_address='" + address + "',emp_phone='" + phone + "',emp_cellphone='" + cell + "',emp_insidenum='" + empnum + "',emp_start_date='" + start + "' WHERE empid='" + selected + "'";
                                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                                    MSQLcrcommand1.ExecuteNonQuery();
                                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                                    MySqlConn.Close();
                                    MessageBox.Show("!פרטי העובד עודכנו", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }

                            reafreashandclear();
                        }//end else
                        
  
            }//end try
            catch { MessageBox.Show("לא נבחר עובד לעדכון ", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); }
        }//end function




        /// <summary>
        /// Handles the AutoGeneratingColumn event of the Grid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridAutoGeneratingColumnEventArgs"/> instance containing the event data.</param>
        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "תעודת זהות")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }

            /*
            if (e.Column.Header.ToString() == "תפקיד")
            {
                Console.WriteLine("dorrrrrrrrrrrrrr");
                string columnName = e.Column.Header.ToString();
                Dictionary<string, string> comboKey = new Dictionary<string, string>()
                    {
                        {"מנהל","מנהל"},
                        {"מזכירה","מזכירה"},
                        {"איכות","איכות"},
                        
                    };
                DataGridTemplateColumn col1 = new DataGridTemplateColumn();
                col1.Header = columnName;
                col1.SortMemberPath = columnName;
                #region Editing
                FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(ComboBox));
                Binding b1 = new Binding(columnName);
                b1.IsAsync = true;
                b1.Mode = BindingMode.TwoWay;
                factory1.SetValue(ComboBox.ItemsSourceProperty, comboKey);
                factory1.SetValue(ComboBox.SelectedValuePathProperty, "Key");
                factory1.SetValue(ComboBox.DisplayMemberPathProperty, "Value");
                factory1.SetValue(ComboBox.SelectedValueProperty, b1);
                factory1.SetValue(ComboBox.SelectedItemProperty, col1);

                DataTemplate cellTemplate1 = new DataTemplate();
                cellTemplate1.VisualTree = factory1;
                col1.CellTemplate = cellTemplate1;
                col1.CellEditingTemplate = cellTemplate1;
                col1.IsReadOnly = false;
                col1.InvalidateProperty(ComboBox.SelectedValueProperty);
                #endregion

                #region View
                FrameworkElementFactory sfactory = new FrameworkElementFactory(typeof(TextBlock));
                sfactory.SetValue(TextBlock.TextProperty, b1);
                DataTemplate cellTemplate = new DataTemplate();
                cellTemplate.VisualTree = sfactory;
                col1.CellTemplate = cellTemplate;
                #endregion

                e.Column = col1;
            }
             */

            if (e.Column.Header.ToString() == "תאריך התחלת עבודה")
            {
                string colname = e.Column.Header.ToString();
                DataGridTemplateColumn dgct = new DataGridTemplateColumn();
                dgct.Header = colname;
                dgct.SortMemberPath = colname;

                Binding b = new Binding(colname);
                b.StringFormat = "dd/MM/yyyy";

                #region Editing
                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(DatePicker));
                factory.SetValue(DatePicker.SelectedDateProperty, b);
                DataTemplate cellEditingTemplate = new DataTemplate();
                cellEditingTemplate.VisualTree = factory;
                dgct.CellEditingTemplate = cellEditingTemplate;
                #endregion

                #region View
                FrameworkElementFactory sfactory = new FrameworkElementFactory(typeof(TextBlock));
                sfactory.SetValue(TextBlock.TextProperty, b);
                DataTemplate cellTemplate = new DataTemplate();
                cellTemplate.VisualTree = sfactory;
                dgct.CellTemplate = cellTemplate;
                #endregion
                e.Column = dgct;
            }




        }



        // go to previous screen.
        /// <summary>
        /// Handles the Click event of the Back_Btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerGui MG = new ManagerGui();
            MG.Show();
            Login.close = 1;
            this.Close();
        }


        /// <summary>
        /// Handles the clicked event of the exit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void exit_clicked(object sender, CancelEventArgs e)
        {
            Console.WriteLine("" + Login.close);

            if (Login.close == 0) // then the user want to exit.
            {
                if (MessageBox.Show("?האם אתה בטוח שברצונך לצאת מהמערכת ", "וידוא יציאה", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    e.Cancel = true; ; //don't exit.
                }
                else // if the user clicked on "Yes" so he wants to Update.
                {
                    // logoff user
                    try
                    {
                        string empid1 = Login.empid;
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "update users set connected='לא מחובר' where empid='" + empid1 + "' ";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    MessageBox.Show("               נותקת בהצלחה מהמערכת\n          תודה שהשתמשת במערכת קרוסר\n                          !להתראות" , "!הצלחה" , MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
            else
            {

            }
            Login.close = 0;
        }




        /// <summary>
        /// Handles the Click event of the exit_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("" + Login.close + " כפתור התנתקות");

            if (Login.close == 0) // then the user want to exit.
            {
                if (MessageBox.Show("?האם אתה בטוח שברצונך לצאת מהמערכת ", "וידוא יציאה", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return; //don't exit.
                }
                else // if the user clicked on "Yes" so he wants to Update.
                {
                    // logoff user
                    try
                    {
                        string empid1 = Login.empid;
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "update users set connected='לא מחובר' where empid='" + empid1 + "' ";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    MessageBox.Show("               נותקת בהצלחה מהמערכת\n          תודה שהשתמשת במערכת קרוסר\n                          !להתראות", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                    Login LI = new Login();
                    LI.Show();
                    Login.close = 1;
                    this.Close();
                }
            }
            else
            {

            }
            Login.close = 0;
        }


    } //end class
} // end namespace




