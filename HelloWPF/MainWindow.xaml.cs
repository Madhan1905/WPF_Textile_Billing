using HelloWPF.Classes;
using SQLite;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using TextileApp;

namespace HelloWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (App.validateLicenseFile(App.licensePath))
            {
                DateTime expirationDate = DateTime.ParseExact(App.expirationDate, "dd/M/yyyy", 
                                            System.Globalization.CultureInfo.InvariantCulture);
                double daysLeft = Math.Ceiling((expirationDate - DateTime.Now).TotalDays);
                if (daysLeft == 10 || daysLeft == 5 || daysLeft == 3 || daysLeft == 1)
                {
                    MessageBox.Show("Your license file expires in "+ daysLeft + " days.\nPlease Contact admin @8870395228", "Reminder",
                                                            MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                this.WindowState = WindowState.Maximized;
                footer_grid.Visibility = Visibility.Visible;
                MainWindowControl.Content = new HomeControl(MainWindowControl);
            } else
            {
                var sysHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
                var sysWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                this.Height = (sysHeight * 0.3);
                this.Width = (sysWidth * 0.3);
                this.Left = sysWidth * 0.5 - this.Width * 0.5;
                this.Top = sysHeight * 0.5 - this.Height * 0.5;

                MainWindowControl.Content = new LicenseControl(this,MainWindowControl,footer_grid);
            }

            Closing += MainWindow_Closing;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_tick;
            timer.Start();
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
                            }
                            MainWindowControl.Content = new BillingControl(invoice, MainWindowControl,false);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    MainWindowControl.Content = new BillingControl(App.currentInvoice, MainWindowControl,false);
                    break;

                case Key.F3:
                    MainWindowControl.Content = new InvoiceControl(MainWindowControl);
                    break;
                case Key.F4:
                    PasswordPopup popup = new PasswordPopup(MainWindowControl);
                    popup.ShowDialog();
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

        void timer_tick(Object sender, EventArgs e)
        {
            LiveTimerText.Content = DateTime.Now.ToString("HH:mm:ss");
        }

        public void closePrompt(CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to Exit?", "Confirmation",
                                                                            MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if(e == null)
                {
                    Closing -= MainWindow_Closing;
                    Close();
                }
            } else if(e != null)
            {
                e.Cancel = true;
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            closePrompt(e);
        }
    }
}
