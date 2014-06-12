// ***********************************************************************
// Assembly         : WpfApplication1
// Author           : user
// Created          : 06-10-2014
//
// Last Modified By : user
// Last Modified On : 06-10-2014
// ***********************************************************************
// <copyright file="MySQLPasswordREQuestGui.xaml.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
using System.ComponentModel;

namespace project
{
    /// <summary>
    /// Interaction logic for MySQLPasswordREQuestGui.xaml
    /// </summary>
    public partial class MySQLPasswordREQuestGui : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySQLPasswordREQuestGui"/> class.
        /// </summary>
        public MySQLPasswordREQuestGui()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Login.close = 1;
        }

        /// <summary>
        /// Handles the Click event of the Back_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            //Login LI = new Login();
           // LI.Show();
            Login.close = 1;
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the Enter_button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Enter_button_Click(object sender, RoutedEventArgs e)
        {
            if (password.Password == "דב777בלום")
            {
                MySQLImpotrGui MSQLI = new MySQLImpotrGui();
                MSQLI.Show();
                Login.close = 0;
                this.Close();
            }
            else {
                MessageBox.Show("הסיסמה שהזנת לא נכונה", "!שים לב", MessageBoxButton.OK, MessageBoxImage.Error);
                     password.Clear();
                 }
        }




        /// <summary>
        /// Handles the clicked event of the exit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void exit_clicked(object sender, CancelEventArgs e)
        {
            Console.WriteLine("" + Login.close);

            if (Login.close == 0) // then the user want to exit.
            {
            }
            else
            {
                Login window = new Login();
                window.Show();
            }
            Login.close = 0;
        }




    }
}
