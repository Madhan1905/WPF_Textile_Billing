using System;
using System.Windows;
using System.IO;
using SQLite;
using HelloWPF.Classes;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace HelloWPF
{
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow(Product product)
        {
            InitializeComponent();
            barcode.Focus();

            var sysHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            var sysWidth = System.Windows.SystemParameters.PrimaryScreenWidth;

            this.Height = (sysHeight * 0.55);
            this.Width = (sysWidth * 0.40);
            this.Left = sysWidth * 0.5 - this.Width * 0.5;
            this.Top = sysHeight * 0.5 - this.Height * 0.5;

            barcode.Text = product.Barcode;
            name.Text = product.Name;
            printName.Text = product.PrintName;
            cost.Text = product.Cost;
            mrp.Text = product.MRP;
            stock.Text = product.stock.ToString();
            if (!product.Barcode.Equals(""))
            {
                submitButton.Content = "Update";
            }
        }

        private void texboxFocusedEvent(object sender, RoutedEventArgs e)
        {
            if (barcode.IsFocused)
            {
                barcode.SelectAll();
            }
            if (name.IsFocused)
            {
                name.SelectAll();
            }
            if (printName.IsFocused)
            {
                printName.SelectAll();
            }
            if (cost.IsFocused)
            {
                cost.SelectAll();
            }
            if (mrp.IsFocused)
            {
                mrp.SelectAll();
            }
            if (stock.IsFocused)
            {
                stock.SelectAll();
            }
        }

        private void submitButtonEvent(Object sender, RoutedEventArgs e)
        {
            submitData();
        }

        private void submitFocusEvent(Object sender, RoutedEventArgs e)
        {
            Console.WriteLine("");
        }

        private void submitData()
        {
            if (barcode.Text.Trim().Equals("") || name.Text.Trim().Equals("") ||
                printName.Text.Trim().Equals("") || cost.Text.Trim().Equals("") || mrp.Text.Trim().Equals(""))
            {
                error_text.Visibility = Visibility.Visible;
            }
            else
            {
                error_text.Visibility = Visibility.Collapsed;
                submitButton.Visibility = Visibility.Collapsed;
                createProgress.Visibility = Visibility.Visible;

                Product product = new Product()
                {
                    Barcode = barcode.Text,
                    Name = name.Text,
                    PrintName = printName.Text,
                    Cost = cost.Text,
                    MRP = mrp.Text,
                    stock = long.Parse(stock.Text)
                };

                using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
                {
                    try
                    {
                        dbConnection.CreateTable<Product>();
                        if (submitButton.Content.Equals("Update"))
                        {
                            dbConnection.Update(product);
                        }
                        else
                        {
                            dbConnection.Insert(product);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("The Barcode already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                Close();
            }
        }

        private void NumberValidationEvent(Object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void keyEventHandler(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    KeyEventArgs args = new KeyEventArgs(Keyboard.PrimaryDevice,Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                    args.RoutedEvent = Keyboard.KeyDownEvent;
                    InputManager.Current.ProcessInput(args);
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }
    }
}
