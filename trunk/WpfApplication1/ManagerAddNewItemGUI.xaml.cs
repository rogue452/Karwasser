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
    /// Interaction logic for ManagerAddNewItemGUI.xaml
    /// </summary>
    public partial class ManagerAddNewItemGUI : Window
    {

        string jobID;
        public ManagerAddNewItemGUI(string  jobID)
        {
            InitializeComponent();
            this.jobID = jobID;
        }



        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            ManagerJobInfoGui MJIG = new ManagerJobInfoGui(jobID);
            MJIG.Show();
            this.Close();

        }
    }
}


