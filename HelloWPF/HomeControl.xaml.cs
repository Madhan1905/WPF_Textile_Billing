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

            List<Invoice> invoices = new List<Invoice>();
            using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
            {
                dbConnection.CreateTable<Invoice>();
                invoices = dbConnection.Table<Invoice>().ToList();
                var sql_cmd = dbConnection.CreateCommand("SELECT * FROM 'Invoice' WHERE Date=='" + DateTime.Now.ToString("dd/M/yyyy") + "'");
                invoices = sql_cmd.ExecuteQuery<Invoice>();
            }
            sales_text.Text = invoices.Count.ToString();
            invoices = invoices.FindAll(invoice => invoice.Total != null);
            int sum = invoices.Sum(invoice => int.Parse(invoice.Total));
            amount_text.Text = sum.ToString();
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
                        dbConnection.CreateTable<Invoice>();
                        dbConnection.Insert(invoice);
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

        private void itemScreenButtonEvent(Object Sender, RoutedEventArgs e)
        {
            MainWindowControl.Content = new ViewItemsControl();
        }

        private void invoiceScreenButtonEvent(Object Sender, RoutedEventArgs e)
        {
            MainWindowControl.Content = new InvoiceControl(MainWindowControl);
        }

        private void quitEvent(Object Sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Close();
        }
    }
}
