using HelloWPF.Classes;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for InvoiceControl.xaml
    /// </summary>
    public partial class InvoiceControl : UserControl
    {
        ContentControl MainWindowControl;
        public InvoiceControl(ContentControl WindowControl)
        {
            InitializeComponent();
            this.MainWindowControl = WindowControl;
            populateInvoiceTable(DateTime.Now.ToString("dd/M/yyyy"));
        }

        private void populateInvoiceTable(string date)
        {
            List<Invoice> invoices = new List<Invoice>();
            MongoClient dbClient = new MongoClient("mongodb://Thinkershut:Dev123@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
            var database = dbClient.GetDatabase("main_db");
            List<String> names = database.ListCollectionNames().ToList<String>();
            if (!names.Contains("CurrentInvoices"))
            {
                database.CreateCollection("CurrentInvoices");
            }
            var collection = database.GetCollection<Invoice>("CurrentInvoices");
            var filter = Builders<Invoice>.Filter.Eq(i => i.Date, date);
            invoices = collection.Find<Invoice>(filter).Limit(15).ToList<Invoice>();

            invoiceTable.ItemsSource = invoices;
            invoiceTable.Items.Refresh();
        }

        private void dateChangedEvent(Object sender, SelectionChangedEventArgs e)
        {
            DateTime date = (DateTime)dateBox.SelectedDate;
            populateInvoiceTable(date.ToString("dd/M/yyyy"));
        }

        private void viewInvoiceEvent(Object Sender, RoutedEventArgs e)
        {
            viewInvoice();
        }

        private void printInvoiceEvent(Object Sender, RoutedEventArgs e)
        {
            Invoice invoice = invoiceTable.SelectedItem as Invoice;
            if (invoice != null)
            {
                App.printBill(invoice);
            }
        }

        private void deleteInvoiceEvent(Object Sender, RoutedEventArgs e)
        {
            deleteInvoice();
        }

        private void viewInvoice()
        {
            Invoice invoice = invoiceTable.SelectedItem as Invoice;
            if (invoice != null)
            {
                App.currentInvoice = invoice;
                MainWindowControl.Content = new BillingControl(invoice, MainWindowControl,true);
            }
        }

        private void deleteInvoice()
        {
            Invoice invoice = invoiceTable.SelectedItem as Invoice;
            if (invoice != null)
            {
                MongoClient dbClient = new MongoClient("mongodb://Thinkershut:Dev123@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
                var database = dbClient.GetDatabase("main_db");
                List<String> names = database.ListCollectionNames().ToList<String>();
                if (!names.Contains("CurrentInvoices"))
                {
                    database.CreateCollection("CurrentInvoices");
                }
                var collection = database.GetCollection<Invoice>("CurrentInvoices");
                try
                {
                    collection.DeleteOne(invoice.ToBsonDocument());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    List<Invoice> invoices = invoiceTable.ItemsSource as List<Invoice>;
                    invoices.Remove(invoice);
                    invoiceTable.Items.Refresh();
                }
            }
        }

        private void dataGridKeyEvent(object sender, KeyEventArgs e)
        {
            if (invoiceTable.SelectedItem != null)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        viewInvoice();
                        break;
                }
            }
        }
    }
}
