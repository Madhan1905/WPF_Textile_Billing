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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HelloWPF
{

    public partial class BillingControl : UserControl
    {
        private Invoice invoice;
        private ContentControl MainWindowControl;
        private bool editMode = false;
        private List<BillingProduct> products;
        public BillingControl(Invoice invoice, ContentControl WindowControl)
        {
            InitializeComponent();
            this.invoice = invoice;
            this.MainWindowControl = WindowControl;

            dateTextBox.Text = DateTime.Now.ToString("dd/M/yyyy");
            invoiceTextBox.Text = invoice.Number.ToString();

            billTable.CellEditEnding += billTableCellEditEvent;
            billTable.PreviewTextInput += NumberValidationEvent;

            if (invoice.BillingProducts != null)
            {
                products = JsonSerializer.Deserialize<List<BillingProduct>>(invoice.BillingProducts);
                editMode = true;
            } else
            {
                products = new List<BillingProduct>();
            }
            int sum = products.Sum(product => product.Total);
            grandTotalText.Text = formatTotal(sum);
            billTable.ItemsSource = products;
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
                            var sql_cmd = dbConnection.CreateCommand("SELECT * FROM 'Product' WHERE Barcode==" + el.Text);
                            List<Product> queryProducts = sql_cmd.ExecuteQuery<Product>();

                            BillingProduct product = (BillingProduct)billTable.SelectedItem;
                            if (queryProducts.Count > 0)
                            {
                                product.Barcode = el.Text;
                                product.Name = queryProducts[0].Name;
                                product.SellingPrice = int.Parse(queryProducts[0].SellingPrice);
                                product.MRP = int.Parse(queryProducts[0].MRP);
                                product.Quantity = 1;
                                product.Total = int.Parse(queryProducts[0].SellingPrice);
                                product.Serial = products.IndexOf(product)+1;
                                grandTotalText.Text = formatTotal(int.Parse(grandTotalText.Text) + int.Parse(queryProducts[0].SellingPrice));
                                invoice.BillingProducts = JsonSerializer.Serialize<List<BillingProduct>>(products);
                            }
                            else
                            {
                                MessageBox.Show("Barcode not Found", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                } else
                {
                    var el = e.EditingElement as TextBox;
                    if (!el.Text.Equals(""))
                    {
                        BillingProduct product = (BillingProduct)billTable.SelectedItem;
                        product.Quantity = int.Parse(el.Text);
                        product.Total = product.Quantity * product.MRP;

                        int sum = products.Sum(product => product.Total);
                        grandTotalText.Text = formatTotal(sum);
                    }
                }
            }
            billTable.CellEditEnding -= billTableCellEditEvent;
            billTable.CommitEdit();
            billTable.CommitEdit();
            billTable.CancelEdit();
            billTable.CancelEdit();
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

        private void saveInvoiceEvent(Object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to print?", "Confirmation", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    
                    break;
                case MessageBoxResult.No:break;
            }
            invoice.Total = grandTotalText.Text;
            invoice.BillingProducts = JsonSerializer.Serialize<List<BillingProduct>>(products);

            using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
            {
                try
                {
                    dbConnection.CreateTable<Invoice>();
                    dbConnection.Update(invoice);
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
            }
        }

        private void exitBillingEvent(Object sender, RoutedEventArgs e)
        {
            exitBilling();
        }

        public bool exitBilling()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure, you want to delete?", "Confirmation", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
                    {
                        try
                        {
                            if (!editMode)
                            {
                                dbConnection.CreateTable<Invoice>();
                                dbConnection.Delete(invoice);
                            }
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
                    }
                    return true;
                case MessageBoxResult.No: return false;
            }
            return false;
        }

        private void dataGridKeyEvent(object sender, KeyEventArgs e)
        {
            if(billTable.SelectedItem != null)
            {
                switch (e.Key)
                {
                    case Key.Delete:
                        BillingProduct product = billTable.SelectedItem as BillingProduct;
                        products.Remove(product);
                        billTable.ItemsSource = products;
                        billTable.Items.Refresh();
                        break;
                    case Key.Back:
                        int index = billTable.SelectedIndex;
                        if (index > 0)
                        {
                            billTable.SelectedIndex = index - 1;
                            billTable.CurrentCell = new DataGridCellInfo(
                                 billTable.Items[index - 1], billTable.Columns[2]);
                            billTable.BeginEdit();
                        }
                        break;
                    case Key.Enter:
                        break;
                }
            }
        }
    }
}
