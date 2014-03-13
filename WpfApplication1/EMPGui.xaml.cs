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

namespace project
{
    /// <summary>
    /// Interaction logic for EMPGui.xaml
    /// </summary>
    public partial class EMPGui : Window
    {
        public EMPGui()
        {
            InitializeComponent();
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
        }





    }
}
