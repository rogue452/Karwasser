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

        private void TXTBtn_Click(object sender, RoutedEventArgs e)
        {
           
            ExportToTXT();
        }





        private void ExportToTXT()
        {
        dataGrid1.SelectAllCells();
        dataGrid1.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
        ApplicationCommands.Copy.Execute(null, dataGrid1);
        String resultat = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
        String result = (string)Clipboard.GetData(DataFormats.Text);
        dataGrid1.UnselectAllCells();
        System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Shuki\Dropbox\Braude\לימודים\Final Pro\Scoend Stage\p\פרוייקט\Excel\test.txt");
     //   file.WriteLine(result.Replace("s","d");
            file.WriteLine(result.Replace("‘,’", "‘ ‘"));
        file.Close();

        MessageBox.Show("Unicode קובץ הטקסט נוצר - להצגה באקסל שמור מחדש בתבנית ");
        }





    }
}
