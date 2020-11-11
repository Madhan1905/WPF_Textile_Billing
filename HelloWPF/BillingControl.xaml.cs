using HelloWPF.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace HelloWPF
{

    public partial class BillingControl : UserControl
    {
        private Invoice invoice;
        private ContentControl MainWindowControl;
        private List<BillingProduct> products;
        private bool editing;
        public BillingControl(Invoice invoice, ContentControl WindowControl,bool editBill)
        {
            InitializeComponent();
            editing = editBill;

            this.invoice = invoice;
            this.MainWindowControl = WindowControl;

            dateTextBox.Text = DateTime.Now.ToString("dd/M/yyyy");
            using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
            {
                invoiceTextBox.Text = (dbConnection.Table<Invoice>().Count()+1).ToString();
            }

            billTable.CellEditEnding += billTableCellEditEvent;
            billTable.PreviewTextInput += NumberValidationEvent;

            if (invoice.BillingProducts != null)
            {
                products = JsonSerializer.Deserialize<List<BillingProduct>>(invoice.BillingProducts);
            } else
            {
                products = new List<BillingProduct>();
            }

            if (invoice.Discount != null)
            {
                discountText.Text = invoice.Discount;
            }

            int sum = products.Sum(product => product.Total);
            totalItemsText.Text = "Total Items:" + products.Count.ToString();
            int discount = discountText.Text == "" ? 0 : int.Parse(discountText.Text);
            grandTotalText.Text = formatTotal(sum-discount);
            billTable.ItemsSource = products;
        }

        private void dataGridLoaded(Object sender, RoutedEventArgs e)
        {
            billTable.SelectedIndex = 0;
            billTable.CurrentCell = new DataGridCellInfo(billTable.Items[0], billTable.Columns[0]);
            billTable.BeginEdit();
        }

        private void billTableCellEditEvent(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (e.Column.Header.Equals("Barcode"))
                {
                    using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
                    {
                        var el = e.EditingElement as TextBox;
                        if (!el.Text.Equals(""))
                        {
                            var sql_cmd = dbConnection.CreateCommand("SELECT * FROM 'Product' WHERE Barcode=='" + el.Text +"'");
                            List<Product> queryProducts = sql_cmd.ExecuteQuery<Product>();

                            BillingProduct product = (BillingProduct)billTable.SelectedItem;
                            if (queryProducts.Count > 0)
                            {
                                List<BillingProduct> availableProducts = new List<BillingProduct>();
                                if (invoice.BillingProducts != null)
                                {
                                    availableProducts = JsonSerializer.Deserialize<List<BillingProduct>>(invoice.BillingProducts);
                                }
                                if (availableProducts.Find(prod => prod.Barcode == el.Text) != null)
                                {
                                    MessageBoxResult result = MessageBox.Show("Add Duplicate product?", "Confirmation", 
                                                                            MessageBoxButton.YesNo, MessageBoxImage.Question);
                                    if(result == MessageBoxResult.Yes)
                                    {
                                        BillingProduct billingProduct = products.Find(prod => prod.Barcode == el.Text);
                                        billingProduct.Quantity = billingProduct.Quantity + 1;
                                        billingProduct.Total = int.Parse(queryProducts[0].MRP) * billingProduct.Quantity;

                                        int discount = discountText.Text == "" ? 0 : int.Parse(discountText.Text);
                                        int sum = products.Sum(product => product.Total);
                                        grandTotalText.Text = formatTotal(sum-discount);

                                        billTable.CellEditEnding -= billTableCellEditEvent;
                                        billTable.CancelEdit();
                                        billTable.CancelEdit();
                                        billTable.Items.Refresh();
                                        billTable.CellEditEnding += billTableCellEditEvent;
                                    } else
                                    {
                                        billTable.CellEditEnding -= billTableCellEditEvent;
                                        billTable.CancelEdit();
                                        billTable.CancelEdit();
                                        billTable.Items.Refresh();
                                        billTable.CellEditEnding += billTableCellEditEvent;
                                    }
                                } else
                                {
                                    addProduct(product, queryProducts, el);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Barcode not Found", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                billTable.CellEditEnding -= billTableCellEditEvent;
                                billTable.CancelEdit();
                                billTable.CancelEdit();
                                billTable.Items.Refresh();
                                billTable.CellEditEnding += billTableCellEditEvent;
                            }
                        } else
                        {
                            billTable.CellEditEnding -= billTableCellEditEvent;
                            billTable.CancelEdit();
                            billTable.CancelEdit();
                            billTable.Items.Refresh();
                            billTable.CellEditEnding += billTableCellEditEvent;
                        }
                    }
                } else
                {
                    var el = e.EditingElement as TextBox;
                    if (!el.Text.Equals("") && int.TryParse(el.Text,out _))
                    {
                        BillingProduct product = (BillingProduct)billTable.SelectedItem;
                        product.Quantity = int.Parse(el.Text);
                        product.Total = product.Quantity * product.MRP;

                        int sum = products.Sum(product => product.Total);
                        int discount = discountText.Text == "" ? 0 : int.Parse(discountText.Text);
                        grandTotalText.Text = formatTotal(sum-discount);

                        int total = products.Sum(product => product.Quantity);
                        totalItemsText.Text = "Total Items:" + total.ToString();

                        billTable.CellEditEnding -= billTableCellEditEvent;
                        billTable.CommitEdit();
                        billTable.CommitEdit();
                        billTable.Items.Refresh();
                        billTable.CellEditEnding += billTableCellEditEvent;
                    } else
                    {
                        billTable.CellEditEnding -= billTableCellEditEvent;
                        billTable.CancelEdit();
                        billTable.CancelEdit();
                        billTable.Items.Refresh();
                        billTable.CellEditEnding += billTableCellEditEvent;
                    }
                }
            }
        }

        private void addProduct(BillingProduct product,List<Product> queryProducts, TextBox el)
        {
            product.Barcode = el.Text;
            product.Name = queryProducts[0].Name;
            product.PrintName = queryProducts[0].PrintName;
            product.MRP = int.Parse(queryProducts[0].MRP);
            product.Quantity = 1;
            product.Total = int.Parse(queryProducts[0].MRP);
            product.Serial = products.IndexOf(product) + 1;

            int sum = products.Sum(product => product.Total);
            int discount = discountText.Text == "" ? 0 : int.Parse(discountText.Text);
            grandTotalText.Text = formatTotal(sum-discount);
            invoice.BillingProducts = JsonSerializer.Serialize<List<BillingProduct>>(products);
            int total = products.Sum(product => product.Quantity);
            totalItemsText.Text = "Total Items:" + total.ToString();

            billTable.CellEditEnding -= billTableCellEditEvent;
            billTable.CommitEdit();
            billTable.CommitEdit();
            billTable.Items.Refresh();
            billTable.CellEditEnding += billTableCellEditEvent;
        }

        private string formatTotal(int value)
        {
            decimal parsed = decimal.Parse(value.ToString(), CultureInfo.InvariantCulture);
            CultureInfo hindi = new CultureInfo("hi-IN");
            string text = string.Format(hindi, "{0:#,#}", parsed);
            return text.Equals("") ? "0" : text;
        }

        private void NumberValidationEvent(Object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NonNegativeNumberValidationEvent(Object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void saveInvoiceEvent(Object sender, RoutedEventArgs e)
        {
            saveInvoice();
        }

        private void exitBillingEvent(Object sender, RoutedEventArgs e)
        {
            exitBilling();
        }

        public void saveInvoice()
        {
            if(products.Count() > 0 && products[0].Barcode != null)
            {
                invoice.Time = DateTime.Now.ToString("hh:mm");
                invoice.Discount = discountText.Text == "" ? "0" : discountText.Text;
                int sum = products.Sum(product => product.Total);
                invoice.Total = (sum - int.Parse(invoice.Discount)).ToString();
                invoice.BillingProducts = JsonSerializer.Serialize<List<BillingProduct>>(products);

                using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
                {
                    try
                    {
                        dbConnection.CreateTable<Invoice>();
                        if (editing)
                        {
                            dbConnection.Update(invoice);
                        } else
                        {
                            dbConnection.Insert(invoice);
                        }
                        App.currentInvoice = null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                MessageBoxResult result = MessageBox.Show("Do you want to print?", "Confirmation", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        App.printBill(invoice);
                        break;
                    case MessageBoxResult.No: break;
                }

                using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
                {
                    try
                    {
                        invoice = new Invoice() { Date = DateTime.Now.ToString("dd/M/yyyy") };
                        App.currentInvoice = invoice;
                        invoice.Number = dbConnection.Table<Product>().Count() + 1;
                        MainWindowControl.Content = new BillingControl(invoice, MainWindowControl,false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                MainWindowControl.Content = new BillingControl(App.currentInvoice, MainWindowControl,false);
            }        
        }

        public bool exitBilling()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure, you want to exit?", "Confirmation", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        App.currentInvoice = null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        MainWindowControl.Content = new HomeControl(MainWindowControl);
                    }
                    return true;
                case MessageBoxResult.No: return false;
            }
            return false;
        }

        private void dataGridLoadingRow(Object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void dataGridKeyEvent(object sender, KeyEventArgs e)
        {
            if(billTable.SelectedItem != null)
            {
                switch (e.Key)
                {
                    case Key.Delete:
                        BillingProduct product = billTable.SelectedItem as BillingProduct;
                        int sum = products.Sum(product => product.Total);
                        int discount = discountText.Text == "" ? 0 : int.Parse(discountText.Text);
                        grandTotalText.Text = formatTotal(sum - product.Total - discount);
                        products.Remove(product);
                        billTable.ItemsSource = products;

                        int total = products.Sum(product => product.Quantity);
                        totalItemsText.Text = "Total Items:" + total.ToString();
                        invoice.BillingProducts = JsonSerializer.Serialize<List<BillingProduct>>(products);
                        billTable.Items.Refresh();
                        break;
                }
            }
        }

        private void controlKeyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                saveInvoice();
            }
        }

        private void dicountChangedEvent(Object sender, TextChangedEventArgs e)
        {
            if (!discountText.Text.Equals(""))
            {
                int sum = products.Sum(product => product.Total);
                grandTotalText.Text = formatTotal(sum - int.Parse(discountText.Text));
            } else
            {
                int sum = products.Sum(product => product.Total);
                grandTotalText.Text = formatTotal(sum);
            }
        }
    }
}
