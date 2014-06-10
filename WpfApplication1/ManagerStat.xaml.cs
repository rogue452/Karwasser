// ***********************************************************************
// Assembly         : WpfApplication1
// Author           : user
// Created          : 06-10-2014
//
// Last Modified By : user
// Last Modified On : 06-10-2014
// ***********************************************************************
// <copyright file="ManagerStat.xaml.cs" company="">
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
    /// Interaction logic for ManagerStat.xaml
    /// </summary>
    public partial class ManagerStat : Window
    {
        /// <summary>
        /// The dt
        /// </summary>
        DataTable dt = new DataTable("stat");
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerStat"/> class.
        /// </summary>
        public ManagerStat()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Login.close = 1;
            all_checkBox.IsChecked = false;
            Start_datePicker.IsEnabled = false;
            End_datePicker.IsEnabled = false;
            StartDate_radioButton.IsEnabled = false;
            ExpectedFinishDate_radioButton.IsEnabled = false;
            ActualFinishDate_radioButton.IsEnabled = false;
            JobIDTextBox.IsEnabled = true;
            name_label.Content = "";
            theNUM_label.Content = "";
        }





        /// <summary>
        /// Handles the AutoGeneratingColumn event of the Grid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridAutoGeneratingColumnEventArgs"/> instance containing the event data.</param>
        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "תאריך שליחה")
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "HH:mm:ss - dd/MM/yyyy";
            }
                e.Column.IsReadOnly = true; // Makes the column as read onl
        
        }



        /// <summary>
        /// Handles the Checked event of the all_checkBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void all_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            Start_datePicker.IsEnabled = true;
            End_datePicker.IsEnabled = true;
            StartDate_radioButton.IsEnabled = true;
            ExpectedFinishDate_radioButton.IsEnabled = true;
            ActualFinishDate_radioButton.IsEnabled = true;
            JobIDTextBox.Clear();
            JobIDTextBox.IsEnabled = false;
        }

        /// <summary>
        /// Handles the UnChecked event of the all_checkBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void all_checkBox_UnChecked(object sender, RoutedEventArgs e)
        {
            Start_datePicker.IsEnabled = false;
            End_datePicker.IsEnabled = false;
            StartDate_radioButton.IsEnabled = false;
            ExpectedFinishDate_radioButton.IsEnabled = false;
            ActualFinishDate_radioButton.IsEnabled = false;
            JobIDTextBox.IsEnabled = true;
        }





        /// <summary>
        /// Handles the 1 event of the Back_Btn_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Back_Btn_Click_1(object sender, RoutedEventArgs e)
        {
            ManagerGui MG = new ManagerGui();
            MG.Show();
            Login.close = 1;
            this.Close();
        }

        /// <summary>
        /// Handles the LoadingRow event of the dataGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridRowEventArgs"/> instance containing the event data.</param>
        void dataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            int rownum =e.Row.GetIndex();
            rownum++;
            e.Row.Header = rownum.ToString();
            //e.Row.Header = (e.Row.GetIndex()).ToString();
        }

        /// <summary>
        /// Handles the Click event of the show_info_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void show_info_button_Click(object sender, RoutedEventArgs e)
        {
            if (all_checkBox.IsChecked == true)
                // with dates
            {
                if (Start_datePicker.Text != "" && End_datePicker.Text != "")
                {
                    String start, end;
                    DateTime s = (DateTime)Convert.ToDateTime(Start_datePicker.Text);
                    DateTime f = (DateTime)Convert.ToDateTime(End_datePicker.Text);
                    TimeSpan ts = f - s;

                    if (ts.Days >= 0)
                    {
                        start = Convert.ToDateTime(Start_datePicker.Text).ToString("yyyy-MM-dd");
                        end = Convert.ToDateTime(End_datePicker.Text).ToString("yyyy-MM-dd");
                        name_label.Content = "לא קיים";
                        theNUM_label.Content = "לא קיים";
                        string radio = "startDate";
                        if (ExpectedFinishDate_radioButton.IsChecked == true)
                        {
                            radio = "expectedFinishDate";
                        }
                        if (ActualFinishDate_radioButton.IsChecked == true)
                        {
                            radio = "actualFinishDate";
                        }
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = ("SELECT fix.jobid as `מספר עבודה` , fix.itemid as `מקט פריט` , fix.itemNum as `מספר הפריט בסט`,  fix.fromFixOrBad as `נשלח לסטטוס` , fix.stageStatusName as `נשלח מסטטוס` , fix.itemStageOrder as `נשלח משלב מספר` ,item.stageName as `שם השלב` , stage_discription as `תאור השלב`, fix.dateAddedToFixTable as `תאריך שליחה` FROM project.jobs, project.fix ,project.item WHERE jobs.jobid=fix.jobid AND jobs.itemid = fix.itemid AND jobs.itemNum = fix.itemNum AND " + radio + " BETWEEN '" + start + "' AND '" + end + "' AND fix.itemid = item.itemid AND  item.itemStatus = fix.stageStatusName AND item.itemStageOrder = fix.itemStageOrder ");
                            //MessageBox.Show("" + Query1 + "");
                            Console.WriteLine(Query1);
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

                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = ("SELECT item.stageName , COUNT(item.stageName)FROM project.jobs, project.fix ,project.item WHERE jobs.jobid=fix.jobid AND jobs.itemid = fix.itemid AND jobs.itemNum = fix.itemNum AND " + radio + " BETWEEN '" + start + "' AND '" + end + "' AND fix.itemid = item.itemid AND  item.itemStatus = fix.stageStatusName AND fix.stageStatusName = 'בעבודה' AND fix.stageStatusName !='תיקון' AND item.itemStageOrder = fix.itemStageOrder GROUP BY item.stageName order by item.stageName desc limit 1");
                            Console.WriteLine(Query1);
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            while (dr.Read())
                            {
                                if (!dr.IsDBNull(0))
                                {
                                    name_label.Content = dr.GetString(0);
                                    theNUM_label.Content = dr.GetInt32(1);
                                }
                                else
                                {
                                    name_label.Content = "";
                                    theNUM_label.Content = "";
                                }
                            }

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

                        return;
                    }
                }
                else
                {
                    if (Start_datePicker.Text == "" && End_datePicker.Text != "")
                    {
                        MessageBox.Show("לא נבחר תאריך התחלה לסינון", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (Start_datePicker.Text != "" && End_datePicker.Text == "")
                    {
                        MessageBox.Show("לא נבחר תאריך סוף לסינון", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (Start_datePicker.Text == "" && End_datePicker.Text == "")
                    {
                        MessageBox.Show("לא נבחרו תאריכי התחלה וסוף לסינון ", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }

            else
                // with jobid
            {
                if (!string.IsNullOrWhiteSpace(JobIDTextBox.Text))
                {
                    name_label.Content = "לא קיים";
                    theNUM_label.Content = "לא קיים";
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT fix.jobid as `מספר עבודה` , fix.itemid as `מקט פריט` , fix.itemNum as `מספר הפריט בסט`,  fix.fromFixOrBad as `נשלח לסטטוס` , fix.stageStatusName as `נשלח מסטטוס` , fix.itemStageOrder as `נשלח משלב מספר` ,item.stageName as `שם השלב` , stage_discription as `תאור השלב`, fix.dateAddedToFixTable as `תאריך שליחה` FROM project.fix ,project.item WHERE fix.jobid='"+ JobIDTextBox.Text +"' AND fix.itemid = item.itemid AND  item.itemStatus = fix.stageStatusName AND item.itemStageOrder = fix.itemStageOrder ");
                        //MessageBox.Show("" + Query1 + "");
                        Console.WriteLine(Query1);
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

                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("SELECT item.stageName , COUNT(item.stageName)FROM project.fix ,project.item WHERE fix.jobid='" + JobIDTextBox.Text + "' AND fix.itemid = item.itemid AND  item.itemStatus = fix.stageStatusName AND fix.stageStatusName = 'בעבודה' AND  fix.stageStatusName !='תיקון' AND item.itemStageOrder = fix.itemStageOrder GROUP BY item.stageName order by item.stageName desc limit 1");
                        Console.WriteLine(Query1);
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        while (dr.Read())
                        {
                            if (!dr.IsDBNull(0))
                            {
                                name_label.Content = dr.GetString(0);
                                theNUM_label.Content = dr.GetInt32(1);
                            }
                            else
                            {
                                name_label.Content = "";
                                theNUM_label.Content = "";
                            }
                        }

                        MySqlConn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                   MessageBox.Show("לא הוזן מספר עבודה ", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                   return;
                }
            
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
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "רשימת תיקונים ופסילות נכון לתאריך -  " + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString(); ; // Default file name
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
                    MessageBox.Show("               נותקת בהצלחה מהמערכת\n          תודה שהשתמשת במערכת קרוסר\n                          !להתראות", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
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
