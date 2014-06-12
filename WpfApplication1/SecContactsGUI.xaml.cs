// ***********************************************************************
// Assembly         : WpfApplication1
// Author           : user
// Created          : 06-10-2014
//
// Last Modified By : user
// Last Modified On : 06-10-2014
// ***********************************************************************
// <copyright file="SecContactsGUI.xaml.cs" company="">
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
    /// Interaction logic for SecContactsGUI.xaml
    /// </summary>
    public partial class SecContactsGUI : Window
    {
        /// <summary>
        /// The hpcostid
        /// </summary>
        string hpcostid;
        /// <summary>
        /// The cos name
        /// </summary>
        string cosName;
        /// <summary>
        /// The cos ad ds
        /// </summary>
        string cosADDs;
        /// <summary>
        /// The cos_inside number
        /// </summary>
        string cos_insideNum;
        /// <summary>
        /// The dt
        /// </summary>
        public static DataTable dt = new DataTable("contacts");
        /// <summary>
        /// Initializes a new instance of the <see cref="SecContactsGUI"/> class.
        /// </summary>
        /// <param name="hpcostid">The hpcostid.</param>
        /// <param name="cos_insideNum">The cos_inside number.</param>
        /// <param name="cosName">Name of the cos.</param>
        /// <param name="cosADDs">The cos ad ds.</param>
        public SecContactsGUI(string hpcostid, string cos_insideNum, string cosName, string cosADDs)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.hpcostid = hpcostid;
            this.cosName = cosName;
            this.cosADDs = cosADDs;
            this.cos_insideNum = cos_insideNum;
            conHP_label.Content = hpcostid;
            cos_insideNum_label.Content = cos_insideNum;
            con_name_label.Content = cosName;
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("select contactid as `מספר איש קשר`,contactName as `שם איש קשר` ,contactEmail as `אימייל איש קשר` ,contactPhone as `טלפון איש קשר`,contactCellPhone as `טלפון נייד של איש הקשר` ,contactDepartment as `מחלקת איש קשר`, contactDesc as `הערות לגבי איש הקשר` from costumers  where costumerid='" + hpcostid + "'");
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


        /// <summary>
        /// Refreashandclears this instance.
        /// </summary>
        private void refreashandclear()
                {
                try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = ("select contactid as `מספר איש קשר`,contactName as `שם איש קשר` ,contactEmail as `אימייל איש קשר` ,contactPhone as `טלפון איש קשר`,contactCellPhone as `טלפון נייד של איש הקשר` ,contactDepartment as `מחלקת איש קשר`, contactDesc as `הערות לגבי איש הקשר` from costumers  where costumerid='" + hpcostid + "'");
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        dt.Clear();
                        mysqlDAdp.Fill(dt);
                        dataGrid1.ItemsSource = dt.DefaultView;
                        mysqlDAdp.Update(dt);
                        MySqlConn.Close();
                        IDSearchTextBox.Clear();
                        FirstNameSearchTextBox.Clear();
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
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "רשימת אנשי הקשר של " +cosName+ "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString(); ; // Default file name
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
                String searchkey = this.FirstNameSearchTextBox.Text;
                string Query1 = ("select contactid as `מספר איש קשר`,contactName as `שם איש קשר` ,contactEmail as `אימייל איש קשר` ,contactPhone as `טלפון איש קשר`,contactCellPhone as `טלפון נייד של איש הקשר` ,contactDepartment as `מחלקת איש קשר`, contactDesc as `הערות לגבי איש הקשר`  from costumers where contactName Like '%" + searchkey + "%' and costumerid='" + hpcostid + "'");
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
                String searchidkey = this.IDSearchTextBox.Text;
                string Query1 = ("select contactid as `מספר איש קשר`,contactName as `שם איש קשר` ,contactEmail as `אימייל איש קשר` ,contactPhone as `טלפון איש קשר`,contactCellPhone as `טלפון נייד של איש הקשר` ,contactDepartment as `מחלקת איש קשר`, contactDesc as `הערות לגבי איש הקשר`  from costumers where contactid Like '%" + searchidkey + "%'  and costumerid='" + hpcostid + "'");
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

        /// <summary>
        /// Handles the Click event of the ADD_Btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ADD_Btn_Click(object sender, RoutedEventArgs e)
        {
            SecAddContactsGUI SACG = new SecAddContactsGUI(hpcostid, cos_insideNum, cosName, cosADDs);
            SACG.Owner = this;
            SACG.ShowDialog();
        }




        // go to previous screen.
        /// <summary>
        /// Handles the Click event of the Back_Btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            SecCusGui SCG = new SecCusGui();
            SCG.Show();
            Login.close = 1;
            this.Close();
        }




        /// <summary>
        /// Handles the AutoGeneratingColumn event of the Grid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridAutoGeneratingColumnEventArgs"/> instance containing the event data.</param>
        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "מספר איש קשר")
            {
                // e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
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
                string selected = row["מספר איש קשר"].ToString();
                // MessageBox.Show(""+selected+ "");
                if (MessageBox.Show("?האם אתה בטוח שברצונך לעדכן איש קשר זה", "וידוא עדכון", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return; //dont do stuff
                }
                else // if the user clicked on "Yes" so he wants to Update.
                {
                    string contactEmail;
                    string contactcell;
                    string contactPhone;
                    string contactName;
                    string contactDepartment;
                    string contactdesc;

                    if (!string.IsNullOrWhiteSpace(row["אימייל איש קשר"].ToString()))
                    {
                        if (row["אימייל איש קשר"].ToString() != "לא הוזנה")
                        {
                            //checking if the email interred correctly
                            if ((Regex.IsMatch(row["אימייל איש קשר"].ToString(), @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")))
                            {
                                contactEmail = row["אימייל איש קשר"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("כתובת האימייל שהזנת לא תקינה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                                refreashandclear();
                                return;
                            }
                        }
                        else
                        {
                            contactEmail = "לא הוזנה";
                        }
                    }
                    else
                    {
                        contactEmail = "לא הוזנה";
                    }

                    if (string.IsNullOrWhiteSpace(row["טלפון נייד של איש הקשר"].ToString()) && string.IsNullOrWhiteSpace(row["טלפון איש קשר"].ToString()))
                    {
                        MessageBox.Show("לפחות אחד מ- טלפון או טלפון נייד חייב להיות מוזן", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                        refreashandclear();
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(row["טלפון נייד של איש הקשר"].ToString()))
                    {
                        try
                        {
                            int cellphoneCheck = Convert.ToInt32(row["טלפון נייד של איש הקשר"].ToString());
                        }
                        catch
                        {
                            MessageBox.Show("טלפון נייד חייב לכלול מספרים בלבד", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                            refreashandclear();
                            return;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(row["טלפון איש קשר"].ToString()))
                    {
                        try
                        {
                            int cellphoneCheck = Convert.ToInt32(row["טלפון איש קשר"].ToString());
                        }
                        catch
                        {
                            MessageBox.Show("מספר טלפון חייב לכלול מספרים בלבד", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                            refreashandclear();
                            return;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(row["מחלקת איש קשר"].ToString()))
                    {
                        contactDepartment = "לא הוזן";
                    }
                    else
                    {
                        contactDepartment = row["מחלקת איש קשר"].ToString();
                    }

                    contactcell = row["טלפון נייד של איש הקשר"].ToString();
                    contactPhone = row["טלפון איש קשר"].ToString();
                    contactName = row["שם איש קשר"].ToString();
                    contactdesc = row["הערות לגבי איש הקשר"].ToString();
                    try
                    {
                        MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                        MySqlConn.Open();
                        string Query1 = "UPDATE costumers SET contactName='" + contactName + "',contactEmail='" + contactEmail + "',contactPhone='" + contactPhone + "',contactDepartment='" + contactDepartment + "',contactCellPhone='" + contactcell + "',contactDesc='" + contactdesc + "' WHERE costumerid='" + hpcostid + "' and contactid='" + selected + "'";
                        MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                        MSQLcrcommand1.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                        MySqlConn.Close();
                        MessageBox.Show("!פרטי איש הקשר עודכנו", "!הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    refreashandclear();

                } // end else // if the user clicked on "Yes" so he wants to Update.

            }
            catch { MessageBox.Show("לא נבחר איש קשר לעדכון", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error); }
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
