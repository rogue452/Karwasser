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
    /// Interaction logic for ManagerGui.xaml
    /// </summary>
    public partial class ManagerGui : Window
    {
        public ManagerGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Users_Button_Click(object sender, RoutedEventArgs e)
        {
            UsersGui UG = new UsersGui();
            UG.Show();
          //  this.Close();
          //  this.Hide();
       
        }

        private void Employees_Button_Click(object sender, RoutedEventArgs e)
        {
            EMPGui EG = new EMPGui();
            EG.Show();
         //   this.Close();
        }
    }
}
