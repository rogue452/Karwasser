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
    /// Interaction logic for UsersGui.xaml
    /// </summary>
    public partial class UsersGui : Window
    {
        public UsersGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DBConnection conn = new DBConnection();
            string query = ("select userid as מספר__משתמש ,first_name as שם__פרטי ,last_name as שם__משפחה ,user_name as שם__משתמש ,password as סיסמה ,role as תפקיד ,connected as מחובר ,email as כתובת__אימייל from users");
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
