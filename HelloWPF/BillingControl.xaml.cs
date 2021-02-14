using HelloWPF.Classes;
using MongoDB.Bson;
using MongoDB.Driver;
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
        private bool endReached;
        public BillingControl(Invoice invoice, ContentControl WindowControl,bool editBill)
        {
            InitializeComponent();
            initializePopup();
            editing = editBill;

            this.invoice = invoice;
            this.MainWindowControl = WindowControl;

            dateTextBox.Text = DateTime.Now.ToString("dd/M/yyyy");
            MongoClient dbClient = new MongoClient("mongodb://Thinkershut:Dev123@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
            var database = dbClient.GetDatabase("main_db");
            List<String> names = database.ListCollectionNames().ToList<String>();
            if (!names.Contains("CurrentInvoices"))
            {
                database.CreateCollection("CurrentInvoices");
            }
            var collection = database.GetCollection<Invoice>("CurrentInvoices");
            invoiceTextBox.Text = (collection.CountDocuments(new BsonDocument()) + 1).ToString();

            billTable.CellEditEnding += billTableCellEditEvent;
            billTable.PreviewTextInput += NumberValidationEvent;
            //billTable.CurrentCellChanged += billTableBeginInit;

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
            string columnSelected = billTable.CurrentColumn.Header as string;

            if (columnSelected.Equals("Name"))
            {
                if (!el.Text.Equals("") && !popupComplete)
                {
                    MongoClient dbClient = new MongoClient("mongodb://Thinkershut:Dev123@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
                    var database = dbClient.GetDatabase("main_db");
                    List<String> names = database.ListCollectionNames().ToList<String>();
                    if (!names.Contains("Products"))
                    {
                        database.CreateCollection("Products");
                    }
                    var collection = database.GetCollection<Product>("Products");
                    List<Product> items = collection.Find<Product>(new BsonDocument()).ToList<Product>()
                                                .FindAll(prod => prod.Name.ToUpper().StartsWith(el.Text.ToUpper()));
                    products_list.ItemsSource = items;
                    products_list.SelectedIndex = 0;
                    productPopup.IsOpen = true;
                }
                else
                {
                    products_list.SelectedIndex = -1;
                    productPopup.IsOpen = false;
                }
            }

            popupComplete = false;

            if (el.Text.Equals(""))
            {
                endReached = true;
            }

        }

        private void billTableGenericTextChangeEvent(object sender, TextChangedEventArgs e)
        {
            var el = e.OriginalSource as TextBox;
            if (el.Text.Equals(""))
            {
                endReached = true;
            }
            else
            {
                endReached = false;
            }
        }

        //private void billTableBeginInit(object sender, EventArgs e)
        //{
        //    endReached = true;
        //    billTable.BeginEdit();
        //}

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
                    MongoClient dbClient = new MongoClient("mongodb://Thinkershut:Dev123@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
                    var database = dbClient.GetDatabase("main_db");
                    List<String> names = database.ListCollectionNames().ToList<String>();
                    if (!names.Contains("Products"))
                    {
                        database.CreateCollection("Products");
                    }
                    var collection = database.GetCollection<Product>("Products");
                    var el = e.EditingElement as TextBox;
                    if (!el.Text.Equals(""))
                    {
                        Product queryProduct = collection.Find(Builders<Product>.Filter.Eq("_id", el.Text)).FirstOrDefault();

                        BillingProduct product = (BillingProduct)billTable.SelectedItem;
                        handleProductCommit(product, queryProduct, el);
                    }
                    else
                    {
                        billTable.CellEditEnding -= billTableCellEditEvent;
                        billTable.CancelEdit();
                        billTable.CancelEdit();
                        billTable.Items.Refresh();
                        billTable.CellEditEnding += billTableCellEditEvent;
                    }
                    editMode = false;
                } else if (e.Column.Header.Equals("Qty."))
                {
                    var el = e.EditingElement as TextBox;
                    BillingProduct product = (BillingProduct)billTable.SelectedItem;
                    if (!el.Text.Equals("") && int.TryParse(el.Text,out _) && product.Barcode != null)
                    {
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
                    if(products_list.SelectedIndex != -1)
                    {
                        Product selectedProduct = products_list.SelectedItem as Product;
                        el.Text = selectedProduct.Name;
                        handleProductCommit(product, selectedProduct, el);
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
                popupComplete = true;
            }
        }

        private void handleProductCommit(BillingProduct product, Product queryProduct, TextBox el)
        {
            if (queryProduct != null )
            {
                List<BillingProduct> availableProducts = new List<BillingProduct>();
                if (invoice.BillingProducts != null)
                {
                    availableProducts = JsonSerializer.Deserialize<List<BillingProduct>>(invoice.BillingProducts);
                }
                if (availableProducts.Find(prod => prod.Barcode == queryProduct.Barcode) != null)
                {
                    MessageBoxResult result = MessageBox.Show("Add Duplicate product?", "Confirmation",
                                                            MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        BillingProduct billingProduct = products.Find(prod => prod.Barcode == queryProduct.Barcode);
                        billingProduct.Quantity = billingProduct.Quantity + 1;
                        billingProduct.Total = int.Parse(queryProduct.MRP) * billingProduct.Quantity;

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
                    addProduct(product, queryProduct, el);
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

        private void addProduct(BillingProduct product,Product queryProduct, TextBox el)
        {
            product.Barcode = queryProduct.Barcode;
            product.Name = queryProduct.Name;
            product.PrintName = queryProduct.PrintName;
            product.MRP = int.Parse(queryProduct.MRP);
            product.Quantity = 1;
            product.Total = int.Parse(queryProduct.MRP);
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
                    if (editing)
                    {
                        var filter = Builders<Invoice>.Filter.Eq(i => i.Number, invoice.Number);
                        collection.ReplaceOne(filter,invoice);
                    }
                    else
                    {
                        invoice.Number = collection.CountDocuments(new BsonDocument()) + 1;
                        collection.InsertOne(invoice);
                    }
                    App.currentInvoice = null;

                    if (!names.Contains("Products"))
                    {
                        database.CreateCollection("Products");
                    }
                    var productCollection = database.GetCollection<Product>("Products");
                    List<Product> existingProducts = productCollection.Find<Product>(new BsonDocument()).ToList<Product>();
                    foreach (BillingProduct billingProduct in products)
                    {
                        Product product = existingProducts.Find(prod => prod.Barcode.Equals(billingProduct.Barcode));
                        product.stock = product.stock - billingProduct.Quantity;
                        var filter = Builders<Product>.Filter.Eq(p => p.Barcode, billingProduct.Barcode);
                        productCollection.ReplaceOne(filter,product);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                MessageBoxResult result = MessageBox.Show("Do you want to print?", "Confirmation", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        App.printBill(invoice);
                        break;
                    case MessageBoxResult.No: break;
                }

                try
                {
                    invoice = new Invoice() { Date = DateTime.Now.ToString("dd/M/yyyy") };
                    App.currentInvoice = invoice;
                    invoice.Number = collection.CountDocuments(new BsonDocument()) + 1;
                    MainWindowControl.Content = new BillingControl(invoice, MainWindowControl, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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
                        if(selectedIndex != -1 && selectedIndex < products_list.Items.Count)
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
            string columnSelected = billTable.CurrentColumn.Header as string;
            if (!editMode)
            {
                if (key == Key.Enter)
                {
                    switch (columnSelected)
                    {
                        case "Barcode":
                            billTable.CurrentColumn = billTable.Columns[1];
                            e.Handled = true;
                            break;
                        case "Name":
                            int selectedIndex = billTable.SelectedIndex;
                            if (selectedIndex == billTable.Items.Count - 1)
                            {
                                discountText.Focus();
                            }
                            billTable.CurrentColumn = billTable.Columns[0];
                            //e.Handled = true;
                            break;
                        case "Qty.":
                            billTable.CurrentColumn = billTable.Columns[0];
                            break;
                    }
                }
            }
            if(key == Key.Back && (!editMode || (editMode && endReached)))
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
                        popupComplete = true;
                        e.Handled = true;
                        break;
                    case "Qty.":
                        billTable.CurrentColumn = billTable.Columns[1];
                        popupComplete = true;
                        endReached = false;
                        e.Handled = true;
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

        private void discountKeyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && discountText.Text.Equals(""))
            {
                int lastIndex = billTable.Items.Count - 1;
                billTable.SelectedIndex = lastIndex;
                billTable.CurrentCell = new DataGridCellInfo(billTable.Items[lastIndex], billTable.Columns[0]);
                endReached = true;
                billTable.BeginEdit();
            }
        }

        private void discountFocusedEvent(object sender, EventArgs e)
        {
            discountText.Background = Brushes.Yellow;
        }

        private void discountLostFocusEvent(object sender, EventArgs e)
        {
            discountText.Background = Brushes.White;
        }
    }
}
