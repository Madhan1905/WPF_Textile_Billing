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
        private bool editMode;
        private bool popupComplete;
        public BillingControl(Invoice invoice, ContentControl WindowControl,bool editBill)
        {
            InitializeComponent();
            initializePopup();
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

        private void initializePopup()
        {
            var sysHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            var sysWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            var width = sysWidth * 0.5;

            productPopup.Height = sysHeight * 0.55;
            productPopup.Width = width;
            productPopup.HorizontalOffset = (sysWidth / 2) - sysWidth * 0.26;

            barcode_column.Width = width * 0.25;
            name_column.Width = width * 0.5;
            cost_column.Width = width * 0.2;
        }

        private void billTableTextChangeEvent(object sender, TextChangedEventArgs e)
        {
            var el = e.OriginalSource as TextBox;
            if (!el.Text.Equals("") && !popupComplete)
            {
                using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
                {
                    List<Product> items = dbConnection.Table<Product>().ToList()
                                            .FindAll(prod => prod.Name.ToUpper().StartsWith(el.Text.ToUpper()));
                    products_list.ItemsSource = items;
                    products_list.SelectedIndex = 0;
                    productPopup.IsOpen = true;
                }
            } else
            {
                productPopup.IsOpen = false;
            }
            popupComplete = false;
            
        }

        private void billTableBeginEditing(object sender, DataGridBeginningEditEventArgs e)
        {
            editMode = true;
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
                            handleProductCommit(product, queryProducts, el);
                        } else
                        {
                            billTable.CellEditEnding -= billTableCellEditEvent;
                            billTable.CancelEdit();
                            billTable.CancelEdit();
                            billTable.Items.Refresh();
                            billTable.CellEditEnding += billTableCellEditEvent;
                        }
                    }
                    editMode = false;
                } else if (e.Column.Header.Equals("Qty."))
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
                } else
                {
                    productPopup.IsOpen = false;
                    popupComplete = true;
                    var el = e.EditingElement as TextBox;
                    BillingProduct product = (BillingProduct)billTable.SelectedItem;
                    List<Product> queryProducts = new List<Product>();
                    if(products_list.SelectedIndex != -1)
                    {
                        Product selectedProduct = products_list.SelectedItem as Product;
                        queryProducts.Add(selectedProduct);
                        el.Text = selectedProduct.Name;
                        handleProductCommit(product, queryProducts, el);
                    }
                }
            }
        }

        private void handleProductCommit(BillingProduct product, List<Product> queryProducts, TextBox el)
        {
            if (queryProducts.Count > 0)
            {
                List<BillingProduct> availableProducts = new List<BillingProduct>();
                if (invoice.BillingProducts != null)
                {
                    availableProducts = JsonSerializer.Deserialize<List<BillingProduct>>(invoice.BillingProducts);
                }
                if (availableProducts.Find(prod => prod.Barcode == queryProducts[0].Barcode) != null)
                {
                    MessageBoxResult result = MessageBox.Show("Add Duplicate product?", "Confirmation",
                                                            MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        BillingProduct billingProduct = products.Find(prod => prod.Barcode == queryProducts[0].Barcode);
                        billingProduct.Quantity = billingProduct.Quantity + 1;
                        billingProduct.Total = int.Parse(queryProducts[0].MRP) * billingProduct.Quantity;

                        int discount = discountText.Text == "" ? 0 : int.Parse(discountText.Text);
                        int sum = products.Sum(product => product.Total);
                        grandTotalText.Text = formatTotal(sum - discount);

                        billTable.CellEditEnding -= billTableCellEditEvent;
                        billTable.CancelEdit();
                        billTable.CancelEdit();
                        billTable.Items.Refresh();
                        billTable.CellEditEnding += billTableCellEditEvent;
                    }
                    else
                    {
                        billTable.CellEditEnding -= billTableCellEditEvent;
                        billTable.CancelEdit();
                        billTable.CancelEdit();
                        billTable.Items.Refresh();
                        billTable.CellEditEnding += billTableCellEditEvent;
                    }
                }
                else
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
        }

        private void addProduct(BillingProduct product,List<Product> queryProducts, TextBox el)
        {
            product.Barcode = queryProducts[0].Barcode;
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
            if (!billTable.CurrentColumn.Header.ToString().Equals("Name"))
            {
                Regex regex = new Regex("[^0-9-]+");
                e.Handled = regex.IsMatch(e.Text);
            }
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

                        dbConnection.CreateTable<Product>();
                        List<Product> existingProducts = dbConnection.Table<Product>().ToList();
                        foreach(BillingProduct billingProduct in products)
                        {
                            Product product = existingProducts.Find(prod => prod.Barcode.Equals(billingProduct.Barcode));
                            product.stock = product.stock - billingProduct.Quantity;
                            dbConnection.Update(product);
                        }
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
                int selectedIndex;
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
                    case Key.Enter:
                        switchCells(e,Key.Enter);
                        break;
                    case Key.Back:
                        switchCells(e,Key.Back);
                        break;
                    case Key.Down:
                        selectedIndex = products_list.SelectedIndex;
                        if(selectedIndex < products_list.Items.Count)
                        {
                            products_list.SelectedIndex = selectedIndex + 1;
                            products_list.ScrollIntoView(products_list.Items[selectedIndex]);
                        }
                        break;
                    case Key.Up:
                        selectedIndex = products_list.SelectedIndex;
                        if(selectedIndex > 0)
                        {
                            products_list.SelectedIndex = selectedIndex - 1;
                            products_list.ScrollIntoView(products_list.Items[selectedIndex]);
                        }
                        break;
                }
            }
        }

        private void dataGridKeyUpEvent(object sender, KeyEventArgs e)
        {
            string columnSelected = billTable.CurrentColumn.Header as string;
            if (billTable.SelectedItem != null && editMode && (columnSelected == "Qty." || columnSelected == "Name"))
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        billTable.CurrentColumn = billTable.Columns[0];
                        e.Handled = true;
                        editMode = false;
                        break;
                }
            }
        }

        private void switchCells(KeyEventArgs e, Key key)
        {
            if (!editMode)
            {
                string columnSelected = billTable.CurrentColumn.Header as string;
                if (key == Key.Enter)
                {
                    switch (columnSelected)
                    {
                        case "Barcode":
                            billTable.CurrentColumn = billTable.Columns[1];
                            e.Handled = true;
                            break;
                        case "Name":
                            billTable.CurrentColumn = billTable.Columns[0];
                            break;
                        case "Qty.":
                            billTable.CurrentColumn = billTable.Columns[0];
                            break;
                    }
                } else
                {
                    switch (columnSelected)
                    {
                        case "Barcode":
                            int selectedIndex = billTable.SelectedIndex == 0 ? 0 : billTable.SelectedIndex - 1;
                            billTable.SelectedIndex = selectedIndex;
                            billTable.CurrentCell = new DataGridCellInfo(billTable.Items[selectedIndex], billTable.Columns[2]);
                            e.Handled = true;
                            break;
                        case "Name":
                            billTable.CurrentColumn = billTable.Columns[0];
                            e.Handled = true;
                            break;
                        case "Qty.":
                            billTable.CurrentColumn = billTable.Columns[1];
                            e.Handled = true;
                            break;
                    }
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
