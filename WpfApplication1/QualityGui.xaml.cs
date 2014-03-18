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
    /// Interaction logic for QualityGui.xaml
    /// </summary>
    public partial class QualityGui : Window
    {
        public QualityGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            NameLabel.Content = "                                    שלום " + Login.user_name + "!\n               אנא בחר/י מה ברצונך/ה לעשות.";
        }


    }
}
