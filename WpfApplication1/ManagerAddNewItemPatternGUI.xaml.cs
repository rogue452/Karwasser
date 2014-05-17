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
using System.ComponentModel;

namespace project
{
    
     



    public partial class ManagerAddNewItemPatternGUI : Window
    {

        int count = 1;

        public ManagerAddNewItemPatternGUI()
        {
            InitializeComponent();
            Login.close = 1;
            stages_comboBox.Items.Add("חריטה");
            stages_comboBox.Items.Add("ביקורת ביניים - חריטה");
            stages_comboBox.Items.Add("כרסום");
            stages_comboBox.Items.Add("ביקורת ביניים - כרסום");
            stages_comboBox.Items.Add("ריתוך");
            stages_comboBox.Items.Add("ביקורת ביניים - ריתוך");
            stages_comboBox.Items.Add("צביעה");
            stages_comboBox.Items.Add("ביקורת ביניים - צביעה");
            stages_comboBox.Items.Add("פירוק");
            stages_comboBox.Items.Add("ביקורת ביניים - פירוק");
            stages_comboBox.Items.Add("הרכבה");
            stages_comboBox.Items.Add("ביקורת ביניים - הרכבה");
            stages_comboBox.Items.Add("ביקורת סופית");
            
        }



        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
           // ManagerJobGui MJG = new ManagerJobGui();
          //  MJG.Show();
            Login.close = 1;
            this.Close();

        }


        private void Add_button_Click(object sender, RoutedEventArgs e)
        {
            
            string itemid, stagename="";
            itemid = item_id_textBox.Text;
            try
            {
                stagename = stages_comboBox.SelectedValue.ToString();
            }
            catch
            {
                MessageBox.Show("!לא נבחר שלב");
                return;
            }
            if (count == 1)
            {
                if (itemid != "")
                { 
                        try
                        {
                            MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                            MySqlConn.Open();
                            string Query1 = ("SELECT COUNT(itemid) FROM project.item WHERE itemid='" + itemid + "' "); //to see if the itemid already in the system.
                            MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                            MSQLcrcommand1.ExecuteNonQuery();
                            int times = Convert.ToInt32(MSQLcrcommand1.ExecuteScalar());
                            MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();
                            MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                            MySqlConn.Close();

                            if (times != 0)
                            {
                                MessageBox.Show("כבר קיים מק``ט פריט  - " + itemid);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }
                    } // end if (itemid != "")
                else
                {
                    MessageBox.Show("לא הוכנס מק``ט פריט");
                    return;
                }


                if (itemname_textBox.Text == "")
                {
                    MessageBox.Show("לא הוכנס שם פריט");
                    return;
                }
            } // end if (count == 1)

            try // if all is OK then create the stage.
            {
                    MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                    MySqlConn.Open();
                    string Query1 = ("INSERT INTO project.item (itemid, itemStatus,itemStageOrder,itemName,stageName, stage_discription, item_discription ) VALUES ('" + item_id_textBox.Text + "', 'בעבודה' ,'" + count + "','" + itemname_textBox.Text + "','" + stages_comboBox.SelectedItem.ToString() + "','" + stage_desc_textBox.Text + "','" + item_disc_textBox.Text + "') ");
                    MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                    MSQLcrcommand1.ExecuteNonQuery();
                    MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);

                    if (count == 1)
                    {
                        // creating the default stages.
                        string one = "1", two = "2", itemStatus = "נרשמה", stageName = "שרטוט", stage_disc = "שרטוט הפריט";
                        
                        string Query2 = ("INSERT INTO project.item (itemid, itemStatus,itemStageOrder,itemName,stageName, stage_discription, item_discription ) VALUES ('" + item_id_textBox.Text + "','"+itemStatus+"','" + one + "','" + itemname_textBox.Text + "','" + stageName + "','" + stage_disc + "','" + item_disc_textBox.Text + "') ");
                        MySqlCommand MSQLcrcommand2 = new MySqlCommand(Query2, MySqlConn);
                        MSQLcrcommand2.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp2 = new MySqlDataAdapter(MSQLcrcommand2);

                        stageName = "רכישת חומר גלם";
                        stage_disc = "רכישת חומר הגלם לעבודה";
                        string Query3 = ("INSERT INTO project.item (itemid, itemStatus,itemStageOrder,itemName,stageName, stage_discription, item_discription ) VALUES ('" + item_id_textBox.Text + "','"+itemStatus+"','" + two + "','" + itemname_textBox.Text + "','" + stageName + "','" + stage_disc + "','" + item_disc_textBox.Text + "') ");
                        MySqlCommand MSQLcrcommand3 = new MySqlCommand(Query3, MySqlConn);
                        MSQLcrcommand3.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp3 = new MySqlDataAdapter(MSQLcrcommand3);

                        itemStatus="תיקון";
                        stageName = "תיקון הפריט";
                        stage_disc = "תיקון לפי מהות התקלה והחלטת הבוחן במקום";
                        string Query4 = ("INSERT INTO project.item (itemid, itemStatus,itemStageOrder,itemName,stageName, stage_discription, item_discription ) VALUES ('" + item_id_textBox.Text + "','"+itemStatus+"','" + one + "','" + itemname_textBox.Text + "','" + stageName + "','" + stage_disc + "','" + item_disc_textBox.Text + "') ");
                        MySqlCommand MSQLcrcommand4 = new MySqlCommand(Query4, MySqlConn);
                        MSQLcrcommand4.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp4 = new MySqlDataAdapter(MSQLcrcommand4);

                        itemStatus="פסול";
                        stageName = "פסילה";
                        stage_disc = "נמצא כלא ניתן לתיקון ולכן נפסל";
                        string Query5 = ("INSERT INTO project.item (itemid, itemStatus,itemStageOrder,itemName,stageName, stage_discription, item_discription ) VALUES ('" + item_id_textBox.Text + "','"+itemStatus+"','" + one + "','" + itemname_textBox.Text + "','" + stageName + "','" + stage_disc + "','" + item_disc_textBox.Text + "') ");
                        MySqlCommand MSQLcrcommand5 = new MySqlCommand(Query5, MySqlConn);
                        MSQLcrcommand5.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp5 = new MySqlDataAdapter(MSQLcrcommand5);

                        itemStatus="גמר ייצור";
                        stageName = "ביקורת לקוח";
                        stage_disc = "מבוקר על ידי הלקוח";
                        string Query6 = ("INSERT INTO project.item (itemid, itemStatus,itemStageOrder,itemName,stageName, stage_discription, item_discription ) VALUES ('" + item_id_textBox.Text + "','"+itemStatus+"','" + one + "','" + itemname_textBox.Text + "','" + stageName + "','" + stage_disc + "','" + item_disc_textBox.Text + "') ");
                        MySqlCommand MSQLcrcommand6 = new MySqlCommand(Query6, MySqlConn);
                        MSQLcrcommand6.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp6 = new MySqlDataAdapter(MSQLcrcommand6);

                        stageName = "אריזה";
                        stage_disc = "נארז למשלוח";
                        string Query7 = ("INSERT INTO project.item (itemid, itemStatus,itemStageOrder,itemName,stageName, stage_discription, item_discription ) VALUES ('" + item_id_textBox.Text + "','"+itemStatus+"','" + two + "','" + itemname_textBox.Text + "','" + stageName + "','" + stage_disc + "','" + item_disc_textBox.Text + "') ");
                        MySqlCommand MSQLcrcommand7 = new MySqlCommand(Query7, MySqlConn);
                        MSQLcrcommand7.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp7 = new MySqlDataAdapter(MSQLcrcommand7);

                        itemStatus="הסתיים";
                        stageName = "המתנה למשלוח";
                        stage_disc = "ממתין לשילוח";
                        string Query8 = ("INSERT INTO project.item (itemid, itemStatus,itemStageOrder,itemName,stageName, stage_discription, item_discription ) VALUES ('" + item_id_textBox.Text + "','"+itemStatus+"','" + one + "','" + itemname_textBox.Text + "','" + stageName + "','" + stage_disc + "','" + item_disc_textBox.Text + "') ");
                        MySqlCommand MSQLcrcommand8 = new MySqlCommand(Query8, MySqlConn);
                        MSQLcrcommand8.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp8 = new MySqlDataAdapter(MSQLcrcommand8);

                        stageName = "נשלח";
                        stage_disc = "נשלח ללקוח";
                        string Query9 = ("INSERT INTO project.item (itemid, itemStatus,itemStageOrder,itemName,stageName, stage_discription, item_discription ) VALUES ('" + item_id_textBox.Text + "','"+itemStatus+"','" + two + "','" + itemname_textBox.Text + "','" + stageName + "','" + stage_disc + "','" + item_disc_textBox.Text + "') ");
                        MySqlCommand MSQLcrcommand9 = new MySqlCommand(Query9, MySqlConn);
                        MSQLcrcommand9.ExecuteNonQuery();
                        MySqlDataAdapter mysqlDAdp9 = new MySqlDataAdapter(MSQLcrcommand9);
                    }
                    MySqlConn.Close();
                    count++;

                    item_stage_num.Content = count;

                    l_label.Visibility = Visibility.Visible;
                    last_stageName_label.Content = stagename;
                    last_stageName_label.Visibility = Visibility.Visible;

                    item_num2_label.Content = item_id_textBox.Text;
                    item_id_textBox.Visibility = Visibility.Hidden;
                    item_num2_label.Visibility = Visibility.Visible;

                    item_name2_label.Content = itemname_textBox.Text;
                    itemname_textBox.Visibility = Visibility.Hidden;
                    item_name2_label.Visibility = Visibility.Visible;

                    item_disc_label.Content = item_disc_textBox.Text;
                    item_disc_textBox.Visibility = Visibility.Hidden;
                    item_disc_label.Visibility = Visibility.Visible;

                    stage_desc_textBox.Clear();

                    MessageBox.Show("שלב התווסף");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

        }


    
        private void exit_clicked(object sender, CancelEventArgs e)
        {
            Console.WriteLine("" + Login.close);

            if (Login.close == 0) // then the user want to exit.
            {
                if (MessageBox.Show("?האם אתה בטוח שברצונך לצאת מהמערכת ", "וידוא יציאה", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
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
                    MessageBox.Show("               נותקת בהצלחה מהמערכת\n          תודה שהשתמשת במערכת קרוסר\n                          !להתראות");
                }
            }
            else
            {

            }
            Login.close = 0;
        }
        


    }
}


