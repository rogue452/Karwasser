// ***********************************************************************
// Assembly         : WpfApplication1
// Author           : user
// Created          : 06-10-2014
//
// Last Modified By : user
// Last Modified On : 06-10-2014
// ***********************************************************************
// <copyright file="ManagerJobGui.xaml.cs" company="">
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
using System.Text.RegularExpressions;
namespace project
{
    /// <summary>
    /// Interaction logic for ManagerJobGui.xaml
    /// </summary>
    public partial class ManagerJobGui : Window
    {
        /// <summary>
        /// The dt
        /// </summary>
        DataTable dt = new DataTable("jobs");
        /// <summary>
        /// The mistake
        /// </summary>
        bool mistake = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerJobGui"/> class.
        /// </summary>
        public ManagerJobGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            refreashandclear();
           /* try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("SELECT jobid as `מספר עבודה`,jobs.costumerid as `חפ לקוח` ,costumers.costumerName as `שם לקוח` ,contact_id as `מספר איש קשר` , costumers.contactName as `שם איש קשר` ,job_status as `סטטוס עבודה`,jobdescription  as `תאור עבודה` ,startDate  as `תאריך התחלה`,expectedFinishDate as `תאריך סיום משוער` ,actualFinishDate as `תאריך סיום בפועל`  FROM project.jobs,project.costumers WHERE jobs.costumerid=costumers.costumerid AND jobs.contact_id=costumers.contactid GROUP BY jobid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt = new DataTable("jobs");
                dt.Clear();
                mysqlDAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
        }




        /// <summary>
        /// Handles the Click event of the TXTBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TXTBtn_Click(object sender, RoutedEventArgs e)
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
               
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "רשימת עבודות" + " נכון לתאריך - " + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString(); ; // Default file name
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
                    else { 
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
        /// Refreashandclears this instance.
        /// </summary>
        private void refreashandclear()
        {
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
               // string Query1 = ("SELECT jobid as `מספר עבודה`,orderid as`מספר הזמנה` ,jobs.costumerid as `חפ לקוח` ,costumers.costumerName as `שם לקוח` ,contact_id as `מספר איש קשר` , costumers.contactName as `שם איש קשר` ,job_status as `סטטוס עבודה`,jobdescription  as `תאור עבודה` ,startDate  as `תאריך התחלה`,expectedFinishDate as `תאריך סיום משוער` ,actualFinishDate as `תאריך סיום בפועל`  FROM project.jobs,project.costumers WHERE jobs.costumerid=costumers.costumerid AND jobs.contact_id=costumers.contactid GROUP BY jobid");
                string Query1 = ("SELECT jobid as `מספר עבודה`, reg_date as `תאריך רישום`,orderid as`מספר הזמנה` ,jobdescription  as `תאור עבודה` ,jobs.costumerid as `חפ לקוח` ,costumers.costumerName as `שם לקוח` ,contact_id as `מספר איש קשר` , costumers.contactName as `שם איש קשר` ,job_status as `סטטוס עבודה`,startDate  as `תאריך התחלה`,expectedFinishDate as `תאריך סיום משוער` ,actualFinishDate as `תאריך סיום בפועל` ,deliveryid  as `תעודת משלוח` ,invoiceNumber  as `מספר חשבונית`  FROM project.jobs,project.costumers WHERE jobs.costumerid=costumers.costumerid AND jobs.contact_id=costumers.contactid GROUP BY jobid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                dt.Clear();
                mysqlDAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                mysqlDAdp.Update(dt);
                MySqlConn.Close();
                Name_Search_TextBox.Clear();
                JobIDSearchTextBox.Clear();
                Start_datePicker.SelectedDate = null;
                End_datePicker.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Handles the TextChanged event of the JobIDSearchTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void JobIDSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchkey = this.JobIDSearchTextBox.Text;
                string Query1 = ("SELECT jobid as `מספר עבודה`, reg_date as `תאריך רישום`,orderid as`מספר הזמנה` ,jobdescription  as `תאור עבודה` ,jobs.costumerid as `חפ לקוח` ,costumers.costumerName as `שם לקוח` ,contact_id as `מספר איש קשר` , costumers.contactName as `שם איש קשר` ,job_status as `סטטוס עבודה`,startDate  as `תאריך התחלה`,expectedFinishDate as `תאריך סיום משוער` ,actualFinishDate as `תאריך סיום בפועל` ,deliveryid  as `תעודת משלוח` ,invoiceNumber  as `מספר חשבונית`  from project.jobs, project.costumers  where  jobid Like '" + searchkey + "%' AND jobs.costumerid=costumers.costumerid AND jobs.contact_id=costumers.contactid GROUP BY jobid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt = new DataTable("jobs");
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
        /// Handles the TextChanged event of the Name_Search_TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void Name_Search_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // if both of the datepickers are with dates
            if (!string.IsNullOrWhiteSpace(Start_datePicker.Text) || !string.IsNullOrWhiteSpace(End_datePicker.Text))
            {
                Fillterdates();
            }
            if (mistake == false)
            {
                string searchidkey = this.Name_Search_TextBox.Text;
                dt.DefaultView.RowFilter = string.Format("`שם לקוח` LIKE '%{0}%'", searchidkey);
            }

            // if not both of the datepicker are filled
       /*     if (string.IsNullOrWhiteSpace(Start_datePicker.Text) || string.IsNullOrWhiteSpace(End_datePicker.Text))
            {
                string searchidkey = this.Name_Search_TextBox.Text;
                dt.DefaultView.RowFilter = string.Format("`שם לקוח` LIKE '%{0}%'", searchidkey);
            }

            // if both of the datepickers are with dates
            if (string.IsNullOrWhiteSpace(Start_datePicker.Text) || string.IsNullOrWhiteSpace(End_datePicker.Text))
            {
                string searchidkey = this.Name_Search_TextBox.Text;
                string startdate = this.Start_datePicker.Text;
                string enddate = this.End_datePicker.Text;
                dt.DefaultView.RowFilter = string.Format("`שם לקוח` LIKE '%{0}%' AND `תאריך התחלה` LIKE '#{0}#' AND `תאריך סיום משוער` LIKE '#{0}#'", searchidkey, startdate, enddate);
            }
            */
         /*   try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                String searchidkey = this.Name_Search_TextBox.Text;
                string Query1 = ("SELECT jobid as `מספר עבודה`, reg_date as `תאריך רישום`,orderid as`מספר הזמנה` ,jobdescription  as `תאור עבודה` ,jobs.costumerid as `חפ לקוח` ,costumers.costumerName as `שם לקוח` ,contact_id as `מספר איש קשר` , costumers.contactName as `שם איש קשר` ,job_status as `סטטוס עבודה`,startDate  as `תאריך התחלה`,expectedFinishDate as `תאריך סיום משוער` ,actualFinishDate as `תאריך סיום בפועל` ,deliveryid  as `תעודת משלוח` ,invoiceNumber  as `מספר חשבונית`  from project.jobs  where  costumerid Like '%" + searchidkey + "%' group by jobid");
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                //DataTable dt = new DataTable("jobs");
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
          */
        }

        /// <summary>
        /// Handles the Click event of the ADD_Btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ADD_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerAddNewJobGUI MACG = new ManagerAddNewJobGUI();
            MACG.Show();
            Login.close = 1;
            this.Close();
        }



        /// <summary>
        /// Handles the Click event of the ChangeBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row1 = (DataRowView)dataGrid1.SelectedItems[0];
            }
            catch 
            {
                MessageBox.Show("!לא נבחרה עבודה לשינוי", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
            string selected = row["מספר עבודה"].ToString();
            string oID = row["מספר הזמנה"].ToString();
            string hpcost = row["חפ לקוח"].ToString();
            string contactNumber = row["מספר איש קשר"].ToString();
            ManagerChangeJobGui MCJG = new ManagerChangeJobGui(selected, oID, hpcost, contactNumber);
            MCJG.Show();
            Login.close = 1;
            this.Close();

           /*
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                if (MessageBox.Show("האם אתה בטוח שברצונך למחוק עבודה זו", "וידוא מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else // if the user clicked on "Yes" so he wants to Delete.
                {
                    // this will give us the first colum of the selected row in the DataGrid.

                    string selected = row["מספר עבודה"].ToString();
                    

                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "delete from jobs where jobid='" + selected + "'";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();
                        MessageBox.Show("! עבודה נמחקה מהמערכת");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("select jobid as `מספר עבודה`,costumerid as `חפ לקוח` ,job_status as `סטטוס עבודה`,jobdescription  as `תאור עבודה` ,startDate  as `תאריך התחלה`,expectedFinishDate as `תאריך סיום משוער` ,actualFinishDate as `תאריך סיום בפועל`  from jobs group by jobid");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        //DataTable dt = new DataTable("jobs");
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
                }//end else

            }//end try
            catch { MessageBox.Show("לא נבחרה עבודה למחיקה"); }
            */

        }//end function



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
        /// Handles the Click event of the UpdateBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string jobid = row["מספר עבודה"].ToString();
                // MessageBox.Show("" + jobid + "");

                    if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן עבודה זו", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return; //dont do stuff
                }
                else // if the user clicked on "Yes" so he wants to Update.
                {
                    string status = row["סטטוס עבודה"].ToString();
                    string description = row["תאור עבודה"].ToString();
                    if (row["תאריך התחלה"].ToString().Equals(""))
                    {
                        MessageBox.Show("!לא ניתן למחוק את תאריך ההתחלה" , "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        refreashandclear();
                        return;
                    }
                    if (row["תאריך סיום משוער"].ToString().Equals(""))
                    {
                        MessageBox.Show("!לא ניתן למחוק את תאריך סיום משוער", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        refreashandclear();
                        return;
                    }

                    if (row["תאריך רישום"].ToString().Equals(""))
                    {
                        MessageBox.Show("!לא ניתן למחוק את תאריך רישום אלא רק לשנות", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        refreashandclear();
                        return;
                    }

                    DateTime s = (DateTime)Convert.ToDateTime(row["תאריך התחלה"].ToString());
                    DateTime f = (DateTime)Convert.ToDateTime(row["תאריך סיום משוער"].ToString());
                    TimeSpan ts = f - s;

                    // if the days are not ok.
                    if (ts.Days < 0)
                    {
                        MessageBox.Show(".תאריך ההתחלה שנבחר הוא לאחר תאריך סיום משוער", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        refreashandclear();
                        return;
                    }

                    string orderid = row["מספר הזמנה"].ToString();
                    if (orderid == "")
                    {
                        MessageBox.Show("!לא ניתן לעדכן עם מספר הזמנה ריק", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        refreashandclear();
                        return;
                    }

                    if (row["תעודת משלוח"].ToString() != "לא עודכן")
                    {
                        if (!string.IsNullOrWhiteSpace(row["תעודת משלוח"].ToString()))
                        {
                            try
                            {
                                int testdv = Convert.ToInt32(row["תעודת משלוח"].ToString());
                            }
                            catch
                            {
                                MessageBox.Show("!תעודת המשלוח לא מכילה רק מספרים", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                refreashandclear();
                                return;
                            }
                        }
                    }
                    string deliveryid = row["תעודת משלוח"].ToString();

                    if (row["מספר חשבונית"].ToString() != "לא עודכן")
                    {
                        if (!string.IsNullOrWhiteSpace(row["מספר חשבונית"].ToString()))
                        {
                            try
                            {
                                int testinv = Convert.ToInt32(row["מספר חשבונית"].ToString());
                            }
                            catch
                            {
                                MessageBox.Show("!מספר חשבונית לא מכיל רק מספרים", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                refreashandclear();
                                return;
                            }
                        }
                    }
                    string invoiceNumber = row["מספר חשבונית"].ToString();
                  
                    string start_date =  Convert.ToDateTime(row["תאריך התחלה"].ToString()).ToString("yyyy-MM-dd");
                    string finish_date = Convert.ToDateTime(row["תאריך סיום משוער"].ToString()).ToString("yyyy-MM-dd");
                    string reg_date = Convert.ToDateTime(row["תאריך רישום"].ToString()).ToString("yyyy-MM-dd");;
                    string actual_finish_date;
                    if (row["תאריך סיום בפועל"].ToString().Equals(""))
                    {
                        try
                        {
                            string Query1 = "UPDATE jobs SET job_status='" + status + "',jobdescription='" + description + "',startDate='" + start_date + "',expectedFinishDate='" + finish_date + "',actualFinishDate=NULL ,orderid='" + orderid + "' ,deliveryid='" + deliveryid + "' ,invoiceNumber='" + invoiceNumber + "',reg_date='" + reg_date + "' WHERE jobid='" + jobid + "'";
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                           // string Query1 = "UPDATE jobs SET job_status='" + status + "',jobdescription='" + description + "',startDate='" + start_date + "',expectedFinishDate='" + finish_date + "',actualFinishDate=NULL ,orderid='" + orderid + "' WHERE jobid='" + jobid + "'";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();
                            MessageBox.Show("!פרטי העבודה עודכנו" , "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        DateTime a = (DateTime)Convert.ToDateTime(row["תאריך סיום בפועל"].ToString());
                        ts = a - s;

                        // if the days are not ok.
                        if (ts.Days < 0)
                        {
                            MessageBox.Show(".תאריך ההתחלה הוא לאחר תאריך הסיום בפועל", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                            refreashandclear();
                            return;
                        }

                        actual_finish_date = Convert.ToDateTime(row["תאריך סיום בפועל"].ToString()).ToString("yyyy-MM-dd");
                        try
                        {
                            string Query1 = "UPDATE jobs SET job_status='" + status + "',jobdescription='" + description + "',startDate='" + start_date + "',expectedFinishDate='" + finish_date + "',actualFinishDate='" + actual_finish_date + "' ,orderid='" + orderid + "' ,deliveryid='" + deliveryid + "' ,invoiceNumber='" + invoiceNumber + "' ,reg_date='" + reg_date + "' WHERE jobid='" + jobid + "'";   
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                           // string Query1 = "UPDATE jobs SET job_status='" + status + "',jobdescription='" + description + "',startDate='" + start_date + "',expectedFinishDate='" + finish_date + "',actualFinishDate='" + actual_finish_date + "' ,orderid='" + orderid + "' WHERE jobid='" + jobid + "'";
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();
                            MessageBox.Show("!פרטי העבודה עודכנו", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                    refreashandclear();
                }

            }
            catch { MessageBox.Show(".לא נבחרה עבודה לעדכון", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); }
        }



        /// <summary>
        /// Handles the AutoGeneratingColumn event of the Grid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridAutoGeneratingColumnEventArgs"/> instance containing the event data.</param>
        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "חפ לקוח" || e.Column.Header.ToString() == "מספר עבודה" || e.Column.Header.ToString() == "מספר איש קשר" || e.Column.Header.ToString() == "שם איש קשר" || e.Column.Header.ToString() == "שם לקוח")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read onl
            }



            //if the Column is a date Column then show me only the date
            if (e.Column.Header.ToString() == "תאריך התחלה")
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";

                /*
                // Create a new template column.
                DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
                templateColumn.CellTemplate = (DataTemplate)Resources["dueDateCellTemplate"];
                templateColumn.CellEditingTemplate = (DataTemplate)Resources["dueDateCellEditingTemplate"];
                e.Column = templateColumn;
                */



                DataGridTemplateColumn dgct = new DataGridTemplateColumn();
                dgct.Header = "תאריך התחלה";
                dgct.SortMemberPath = "תאריך התחלה";

                Binding b = new Binding("תאריך התחלה");
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

            if (e.Column.Header.ToString() == "תאריך רישום")
            {
                DataGridTemplateColumn dgct = new DataGridTemplateColumn();
                string colname = e.Column.Header.ToString();
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

            if (e.Column.Header.ToString() == "תאריך סיום משוער" )
            {
                DataGridTemplateColumn dgct = new DataGridTemplateColumn();
                dgct.Header = "תאריך סיום משוער";
                dgct.SortMemberPath = "תאריך סיום משוער";

                Binding b = new Binding("תאריך סיום משוער");
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

            if (e.Column.Header.ToString() == "תאריך סיום בפועל")
            {
                DataGridTemplateColumn dgct = new DataGridTemplateColumn();
                dgct.Header = "תאריך סיום בפועל";
                dgct.SortMemberPath = "תאריך סיום בפועל";

                Binding b = new Binding("תאריך סיום בפועל");
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


            if (e.Column.Header.ToString() == "סטטוס עבודה")
            {

                string columnName1 = e.Column.Header.ToString();
                Dictionary<string, string> comboKey1 = new Dictionary<string, string>()
                    {
                        {"נרשמה","נרשמה"},
                        {"בעבודה","בעבודה"},
                        {"מושהת","מושהת"},
                        {"בוטלה","בוטלה"},
                        {"הסתיימה","הסתיימה"},
                    };
                DataGridTemplateColumn col11 = new DataGridTemplateColumn();
                col11.Header = columnName1;
                col11.SortMemberPath = columnName1;

                #region Editing
                FrameworkElementFactory factory11 = new FrameworkElementFactory(typeof(ComboBox));
                Binding b11 = new Binding(columnName1);
                b11.IsAsync = true;
                b11.Mode = BindingMode.TwoWay;
                factory11.SetValue(ComboBox.ItemsSourceProperty, comboKey1);
                factory11.SetValue(ComboBox.SelectedValuePathProperty, "Key");
                factory11.SetValue(ComboBox.DisplayMemberPathProperty, "Value");
                factory11.SetValue(ComboBox.SelectedValueProperty, b11);
                factory11.SetValue(ComboBox.SelectedItemProperty, col11);

                DataTemplate cellTemplate1 = new DataTemplate();
                cellTemplate1.VisualTree = factory11;
                col11.CellTemplate = cellTemplate1;
                col11.CellEditingTemplate = cellTemplate1;
                col11.IsReadOnly = false;
                col11.InvalidateProperty(ComboBox.SelectedValueProperty);
                #endregion

                #region View
                FrameworkElementFactory sfactory = new FrameworkElementFactory(typeof(TextBlock));
                sfactory.SetValue(TextBlock.TextProperty, b11);
                DataTemplate cellTemplate = new DataTemplate();
                cellTemplate.VisualTree = sfactory;
                col11.CellTemplate = cellTemplate;
                #endregion

                e.Column = col11;


            }
        }

        /// <summary>
        /// Handles the Click event of the Contacts_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Contacts_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["חפ לקוח"].ToString();
                string CosName = row["שם לקוח"].ToString();
                string cosADDs = row["כתובת לקוח"].ToString();
                string cos_insideNum = row["מספר לקוח"].ToString();
                ManagerContactsGUI MCG = new ManagerContactsGUI(selected, cos_insideNum, CosName, cosADDs);
                MCG.Show();
                Login.close = 1;
                this.Close();
            }
            catch { MessageBox.Show("לא נבחר לקוח"); }
        }


        //This function will set the 2 DatePickers to today and will reaload to the default datagrid.
        /// <summary>
        /// Handles the Click event of the Refresh_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Refresh_button_Click(object sender, RoutedEventArgs e)
        {
            Start_datePicker.SelectedDate = null;
            End_datePicker.SelectedDate = null;
            refreashandclear();
        }



        //This function will filter the startDate by date selected from 2 DatePickers.
        // if the dates were not right a message will be shown. 
        /// <summary>
        /// Handles the Click event of the Filter_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Filter_button_Click(object sender, RoutedEventArgs e)
        {
            Fillterdates();
            if (!string.IsNullOrWhiteSpace(Name_Search_TextBox.Text))
            {
                string searchidkey = this.Name_Search_TextBox.Text;
                dt.DefaultView.RowFilter = string.Format("`שם לקוח` LIKE '%{0}%'", searchidkey);
            }
            /*
            //MessageBox.Show("" + Start_datePicker.Text + "");
            if (Start_datePicker.Text != "" && End_datePicker.Text != "")
            {
                String start, end;
                DateTime s = (DateTime)Convert.ToDateTime(Start_datePicker.Text);
                DateTime f = (DateTime)Convert.ToDateTime(End_datePicker.Text);
                TimeSpan ts = f - s;
                //MessageBox.Show("" + s + "");
                //MessageBox.Show("" + f + "");
                //MessageBox.Show("" + ts.Days + "");
                if (ts.Days >= 0)
                {
                    start = Convert.ToDateTime(Start_datePicker.Text).ToString("yyyy-MM-dd");
                    end = Convert.ToDateTime(End_datePicker.Text).ToString("yyyy-MM-dd");

                    string radio = "startDate";
                    if (ExpectedFinishDate_radioButton.IsChecked == true)
                    {
                        radio = "expectedFinishDate";
                    }
                    if (ActualFinishDate_radioButton.IsChecked == true)
                    {
                        radio = "actualFinishDate";
                    }
                    //MessageBox.Show("" + radio + "");
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT jobid as `מספר עבודה`, reg_date as `תאריך רישום`,orderid as`מספר הזמנה` ,jobdescription  as `תאור עבודה` ,jobs.costumerid as `חפ לקוח` ,costumers.costumerName as `שם לקוח` ,contact_id as `מספר איש קשר` , costumers.contactName as `שם איש קשר` ,job_status as `סטטוס עבודה`,startDate  as `תאריך התחלה`,expectedFinishDate as `תאריך סיום משוער` ,actualFinishDate as `תאריך סיום בפועל` ,deliveryid  as `תעודת משלוח` ,invoiceNumber  as `מספר חשבונית`  FROM jobs WHERE " + radio + " BETWEEN '" + start + "' AND '" + end + "'  group by jobid ");
                        //MessageBox.Show("" + Query1 + "");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
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
                else
                {
                    MessageBox.Show("אסור שתאריך הסוף הנבחר יהיה לפני תאריך ההתחלה הנבחר");
                }
            }
            else 
            {
                if (Start_datePicker.Text == "" && End_datePicker.Text != "")
                {MessageBox.Show("לא נבחר תאריך התחלה לסינון");}
                if (Start_datePicker.Text != "" && End_datePicker.Text == "")
                { MessageBox.Show("לא נבחר תאריך סוף לסינון"); }
                if (Start_datePicker.Text == "" && End_datePicker.Text == "")
                { MessageBox.Show("לא נבחרו תאריכי התחלה וסוף לסינון "); }
            }
            */
        }


        /// <summary>
        /// Fillterdateses this instance.
        /// </summary>
        private void Fillterdates()
        {
            mistake =false;
            //MessageBox.Show("" + Start_datePicker.Text + "");
            if (Start_datePicker.Text != "" && End_datePicker.Text != "")
            {
                String start, end;
                DateTime s = (DateTime)Convert.ToDateTime(Start_datePicker.Text);
                DateTime f = (DateTime)Convert.ToDateTime(End_datePicker.Text);
                TimeSpan ts = f - s;
                //MessageBox.Show("" + s + "");
                //MessageBox.Show("" + f + "");
                //MessageBox.Show("" + ts.Days + "");
                if (ts.Days >= 0)
                {
                    start = Convert.ToDateTime(Start_datePicker.Text).ToString("yyyy-MM-dd");
                    end = Convert.ToDateTime(End_datePicker.Text).ToString("yyyy-MM-dd");

                    string radio = "startDate";
                    if (ExpectedFinishDate_radioButton.IsChecked == true)
                    {
                        radio = "expectedFinishDate";
                    }
                    if (ActualFinishDate_radioButton.IsChecked == true)
                    {
                        radio = "actualFinishDate";
                    }
                    //MessageBox.Show("" + radio + "");
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT jobid as `מספר עבודה`, reg_date as `תאריך רישום`,orderid as`מספר הזמנה` ,jobdescription  as `תאור עבודה` ,jobs.costumerid as `חפ לקוח` ,costumers.costumerName as `שם לקוח` ,contact_id as `מספר איש קשר` , costumers.contactName as `שם איש קשר` ,job_status as `סטטוס עבודה`,startDate  as `תאריך התחלה`,expectedFinishDate as `תאריך סיום משוער` ,actualFinishDate as `תאריך סיום בפועל` ,deliveryid  as `תעודת משלוח` ,invoiceNumber  as `מספר חשבונית`  FROM jobs,costumers WHERE " + radio + " BETWEEN '" + start + "' AND '" + end + "' AND jobs.costumerid=costumers.costumerid AND jobs.contact_id=costumers.contactid GROUP BY jobid");
                        //MessageBox.Show("" + Query1 + "");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
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
                else
                {
                    MessageBox.Show("אסור שתאריך הסוף הנבחר יהיה לפני תאריך ההתחלה הנבחר", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                    mistake = true;
                    return;
                }
            }
            else
            {
                if (Start_datePicker.Text == "" && End_datePicker.Text != "")
                { 
                    MessageBox.Show("לא נבחר תאריך התחלה לסינון", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                    mistake = true;
                    return;
                }
                if (Start_datePicker.Text != "" && End_datePicker.Text == "")
                {
                    MessageBox.Show("לא נבחר תאריך סוף לסינון", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                    mistake = true;
                    return;
                }
                if (Start_datePicker.Text == "" && End_datePicker.Text == "")
                {
                    MessageBox.Show("לא נבחרו תאריכי התחלה וסוף לסינון ", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                    mistake = true;
                    return;
                }
            }
        }



        /// <summary>
        /// Handles the Click event of the ViewJobIInfo_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ViewJobIInfo_button_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {                
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                string selected = row["מספר עבודה"].ToString();
                ManagerJobInfoGui MJIG = new ManagerJobInfoGui(selected);
                MJIG.Show();
                Login.close = 1;
                this.Close();
            }//end try
            catch { MessageBox.Show("לא נבחרה עבודה לצפיה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); }
        }



        /// <summary>
        /// Handles the click event of the ADD_new_item_pattern control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ADD_new_item_pattern_click(object sender, RoutedEventArgs e)
        {
            ManagerItemsGui MIG = new ManagerItemsGui();
            MIG.Show();
            Login.close = 1;
            this.Close();
            /*
            ManagerAddNewItemPatternGUI MANIG = new ManagerAddNewItemPatternGUI();
            MANIG.ShowDialog();
           // this.Close();
            */

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











    }

}







      
