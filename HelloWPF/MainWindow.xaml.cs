using HelloWPF.Classes;
using SQLite;
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace HelloWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowControl.Content = new HomeControl(MainWindowControl);
        }

        private void keyEventHandler(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    Invoice invoice;
                    using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
                    {
                        try
                        {
                            if (App.currentInvoice != null)
                            {
                                invoice = App.currentInvoice;
                            }
                            else
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
                    break;

                case Key.F3:
                    MainWindowControl.Content = new ViewItemsControl();
                    break;
                case Key.F4:
                    MainWindowControl.Content = new InvoiceControl(MainWindowControl);
                    break;
                case Key.Escape:
                    if(MainWindowControl.Content is BillingControl)
                    {
                        BillingControl billingControl = MainWindowControl.Content as BillingControl;
                        bool result = billingControl.exitBilling();
                        if (result)
                        {
                            MainWindowControl.Content = new HomeControl(MainWindowControl);
                        }
                    } else
                    {
                        MainWindowControl.Content = new HomeControl(MainWindowControl);
                    }
                    break;
                case Key.Insert:
                    if(MainWindowControl.Content is ViewItemsControl)
                    {
                        ViewItemsControl control = MainWindowControl.Content as ViewItemsControl;
                        control.addData();
                    }
                    break;
            }

        }
    }
}
