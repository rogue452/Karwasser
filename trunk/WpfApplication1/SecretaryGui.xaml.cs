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
    /// Interaction logic for SecretaryGui.xaml
    /// </summary>
    public partial class SecretaryGui : Window
    {
        public SecretaryGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            NameLabel.Content = "                                    שלום " + Login.user_name + "!\n               אנא בחר/י מה ברצונך/ה לעשות.";
        }


        private void Employees_Button_Click(object sender, RoutedEventArgs e)
        {
            SecEMPGui SEG = new SecEMPGui();
            SEG.Show();
            //   this.Close();
        }
    }
}
