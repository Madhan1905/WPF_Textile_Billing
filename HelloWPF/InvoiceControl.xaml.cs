using HelloWPF.Classes;
using SQLite;
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
            using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
            {
                dbConnection.CreateTable<Invoice>();
                invoices = dbConnection.Table<Invoice>().ToList();
                var sql_cmd = dbConnection.CreateCommand("SELECT * FROM 'Invoice' WHERE Date=='"+date+"'");
                invoices = sql_cmd.ExecuteQuery<Invoice>();
            }
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
                MainWindowControl.Content = new BillingControl(invoice, MainWindowControl);
            }
        }

        private void deleteInvoice()
        {
            Invoice invoice = invoiceTable.SelectedItem as Invoice;
            if (invoice != null)
            {
                using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
                {
                    try
                    {
                        dbConnection.CreateTable<Invoice>();
                        dbConnection.Delete(invoice);
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
        }

        private void dataGridKeyEvent(object sender, KeyEventArgs e)
        {
            if (invoiceTable.SelectedItem != null)
            {
                switch (e.Key)
                {
                    case Key.Delete:
                        deleteInvoice();
                        break;
                    case Key.Enter:
                        viewInvoice();
                        break;
                }
            }
        }
    }
}
