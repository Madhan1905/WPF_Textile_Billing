using HelloWPF.Classes;
using SQLite;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextileApp;

namespace HelloWPF
{
    /// <summary>
    /// Interaction logic for HomeControl.xaml
    /// </summary>
    public partial class HomeControl : UserControl
    {
        ContentControl MainWindowControl;
        public HomeControl(ContentControl WindowControl)
        {
            InitializeComponent();
            MainWindowControl = WindowControl;
        }

        private void billScreenButtonEvent(Object Sender, RoutedEventArgs e)
        {
            Invoice invoice;
            using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
            {
                try
                {
                    if(App.currentInvoice != null)
                    {
                        invoice = App.currentInvoice;
                    } else
                    {
                        invoice = new Invoice() { Date = DateTime.Now.ToString("dd/M/yyyy") };
                        App.currentInvoice = invoice;
                        invoice.Number = dbConnection.Table<Product>().Count() + 1;
                    }
                    MainWindowControl.Content = new BillingControl(invoice, MainWindowControl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            MainWindowControl.Content = new BillingControl(App.currentInvoice, MainWindowControl);
        }

        private void adminScreenButtonEvent(Object Sender, RoutedEventArgs e)
        {
            PasswordPopup popup = new PasswordPopup(MainWindowControl);
            popup.ShowDialog();
        }

        private void invoiceScreenButtonEvent(Object Sender, RoutedEventArgs e)
        {
            MainWindowControl.Content = new InvoiceControl(MainWindowControl);
        }

        private void quitEvent(Object Sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            MainWindow mainWindow = (MainWindow)window;
            mainWindow.closePrompt(null);
        }
    }
}
