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
    /// Interaction logic for ManagerGeneralItemStagesGui.xaml
    /// </summary>
    public partial class ManagerGeneralItemStagesGui : Window
    {
        DataTable dt = new DataTable("ItemStages");
        string itemID,itemName;

        public ManagerGeneralItemStagesGui(string itemid)
        {
            this.itemID = itemid;
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = "select itemName from item where itemid='" + itemID + "'";
                MySqlCommand MSQLcrcommand1 = new MySqlCommand(Query1, MySqlConn);
                MSQLcrcommand1.ExecuteNonQuery();
                MySqlDataAdapter mysqlDAdp = new MySqlDataAdapter(MSQLcrcommand1);
                MySqlDataReader dr = MSQLcrcommand1.ExecuteReader();

                while (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                    {
                        itemName = dr.GetString(0);
                    }

                }
                MySqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            type_comboBox.Items.Add("בעבודה");
            type_comboBox.Items.Add("תיקון");
            type_comboBox.Items.Add("פסול");
            type_comboBox.Items.Add("הסתיים");
            type_comboBox.SelectedIndex = 0;
            itemidlabel.Content = itemID;
            itemnamelabel.Content = itemName;

            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("SELECT itemStageOrder as `מספר שלב`,stageName as `שם שלב` ,stage_discription as `תאור השלב`  FROM item WHERE itemid='" + itemID + "'  and itemStatus='בעבודה' ");
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

        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void type_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string status;
            status = type_comboBox.SelectedValue.ToString();
            try
            {
                MySqlConnection MySqlConn = new MySqlConnection(Login.Connectionstring);
                MySqlConn.Open();
                string Query1 = ("SELECT itemStageOrder as `מספר שלב`,stageName as `שם שלב` ,stage_discription as `תאור השלב`  FROM item WHERE itemid='" + itemID + "'  and itemStatus='" + status + "' ");
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
    }
}
