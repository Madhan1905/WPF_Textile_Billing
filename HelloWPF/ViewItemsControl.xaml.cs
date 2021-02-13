using HelloWPF.Classes;
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
            AdminControl.Content = new TextileApp.ItemsControl(AdminControl);
        }

        private void addButtonEvent(Object Sender, RoutedEventArgs e)
        {
            Product product = new Product()
            {
                Barcode = "",
                Name = "",
                SellingPrice = "",
                Cost = "",
                MRP = "",
                stock = 0,
            };
            AdminControl.Content = new TextileApp.AddItemsControl(product);
        }
    }
}
