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
using System.Collections;
namespace project
{
    
     



    public partial class ManagerAddNewItemPatternGUI : Window
    {

        int count = 1;

        public ManagerAddNewItemPatternGUI()
        {
            InitializeComponent();

            comboBox1.Items.Add("בעבודה");
            comboBox1.Items.Add("תיקון");
            
        }



        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            ManagerJobGui MJG = new ManagerJobGui();
            MJG.Show();
            this.Close();

        }

        private void Add_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {   
                int test = Convert.ToInt32(item_id_textBox.Text);
                string itemid;
                itemid = item_id_textBox.Text;
                if (count == 1)
                {
                    if (itemid != "")
                    {
                        try
                        {
                            string itemstatus = comboBox1.SelectedValue.ToString(); ;
                            try
                            {
                                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                                MySqlConn.Open();
                                string Query1 = ("SELECT COUNT(itemid) FROM project.item WHERE itemid='" + itemid + "'AND itemStatus='" + itemstatus + "' "); //to see if the itemid already in the system.
                                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                                MSQLcrcommand1.ExecuteNonQuery();
                                int times = Convert.ToInt32(MSQLcrcommand1.ExecuteScalar());
                                MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                                MySqlConn.Close();

                                if (times != 0)
                                {
                                    MessageBox.Show("כבר קיים מספר פריט  - " + itemid + " בעל סטטוס זה ");
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                return;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("לא נבחר סטטוס פריט");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("לא הוכנס מספר פריט");
                        return;
                    }

                    if (itemname_textBox.Text == "")
                    {
                        MessageBox.Show("לא הוכנס שם פריט");
                        return;
                    }

                    if (itemname_textBox.Text == "")
                    {
                        MessageBox.Show("לא הוכנס שם פריט");
                        return;
                    }

                    //       try
                    //       {
                    //           string index = comboBox1.SelectedValue.ToString(); ;
                    //       }
                    //       catch
                    //       {
                    //            MessageBox.Show("לא נבחר סטטוס פריט");
                    //            return;
                    //       }
                }


                if (stage_name_text_box.Text == "")
                {
                    MessageBox.Show("לא הוכנס שם שלב");
                    return;
                }

                try
                {
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = ("INSERT INTO project.item (itemid, itemStatus,itemStageOrder,itemName,stageName, stage_discription, item_discription ) VALUES ('" + item_id_textBox.Text + "','" + comboBox1.SelectedItem.ToString() + "','" + count + "','" + itemname_textBox.Text + "','" + stage_name_text_box.Text + "','" + stage_desc_textBox.Text + "','" + item_disc_textBox.Text + "') ");
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();
                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                   // MySqlConn.Close();
                    if (count == 1 && comboBox1.SelectedItem == "בעבודה")
                    {
                        string one = "1", stageName = "הסתיים", stage_disc = "הפריט מוכן", stageName3 = "נפסל", stage_disc3 = "הפריט נפסל";
                        string Query2 = ("INSERT INTO project.item (itemid, itemStatus,itemStageOrder,itemName,stageName, stage_discription, item_discription ) VALUES ('" + item_id_textBox.Text + "','הסתיים','" + one + "','" + itemname_textBox.Text + "','" + stageName + "','" + stage_disc + "','" + item_disc_textBox.Text + "') ");
                        MySqlCommand MSQLcrcommand2 = new MySqlCommand(Query2, MySqlConn);
                        MSQLcrcommand2.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp2 = new MySqlDataAdapter(MSQLcrcommand2);

                        string Query3 = ("INSERT INTO project.item (itemid, itemStatus,itemStageOrder,itemName,stageName, stage_discription, item_discription ) VALUES ('" + item_id_textBox.Text + "','פסול','" + one + "','" + itemname_textBox.Text + "','" + stageName3 + "','" + stage_disc3 + "','" + item_disc_textBox.Text + "') ");
                        MySqlCommand MSQLcrcommand3 = new MySqlCommand(Query3, MySqlConn);
                        MSQLcrcommand3.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp3 = new MySqlDataAdapter(MSQLcrcommand3);
                    }
                    MySqlConn.Close();
                    count++;

                    item_stage_num.Content = count;
                    stage_status_label.Content = comboBox1.SelectedItem.ToString();
                    comboBox1.Visibility = Visibility.Hidden;
                    stage_status_label.Visibility = Visibility.Visible;

                    item_num2_label.Content = item_id_textBox.Text;
                    item_id_textBox.Visibility = Visibility.Hidden;
                    item_num2_label.Visibility = Visibility.Visible;

                    item_name2_label.Content = itemname_textBox.Text;
                    itemname_textBox.Visibility = Visibility.Hidden;
                    item_name2_label.Visibility = Visibility.Visible;

                    item_disc_label.Content = item_disc_textBox.Text;
                    item_disc_textBox.Visibility = Visibility.Hidden;
                    item_disc_label.Visibility = Visibility.Visible;

                    stage_name_text_box.Clear();
                    stage_desc_textBox.Clear();



                    MessageBox.Show("שלב התווסף");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("!מספר פריט חייב להכיל רק מספרים");
                return;
            }

        }






    }
}


