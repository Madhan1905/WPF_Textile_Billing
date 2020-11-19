using HelloWPF.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using TextileApp;

namespace HelloWPF
{
    /// <summary>
    /// Interaction logic for ViewItemsControl.xaml
    /// </summary>
    public partial class ViewItemsControl : UserControl
    {
        public ViewItemsControl(PasswordPopup popup)
        {
            InitializeComponent();
            AdminControl.Content = new DashboardControl();
            popup.Close();
        }

        private void dashboardButtonEvent(Object Sender, RoutedEventArgs e)
        {
            AdminControl.Content = new DashboardControl();
        }

        private void itemsButtonEvent(Object Sender, RoutedEventArgs e)
        {
            AdminControl.Content = new TextileApp.ItemsControl();
        }

        public void addData()
        {
            if (AdminControl.Content is TextileApp.ItemsControl)
            {
                TextileApp.ItemsControl control = AdminControl.Content as TextileApp.ItemsControl;
                control.addData();
            }
        }
    }
}
