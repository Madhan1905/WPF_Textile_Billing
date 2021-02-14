using HelloWPF.Classes;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
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

namespace TextileApp
{
    /// <summary>
    /// Interaction logic for AddItemsControl.xaml
    /// </summary>
    public partial class AddItemsControl : UserControl
    {
        public AddItemsControl(Product product)
        {
            InitializeComponent();
            barcode.Focus();
            Loaded += controlLoaded;

            barcode.Text = product.Barcode;
            name.Text = product.Name;
            printName.Text = product.PrintName;
            //cost.Text = product.Cost;
            mrp.Text = product.MRP;
            stock.Text = product.stock.ToString();
            if (!product.Barcode.Equals(""))
            {
                submitButton.Content = "Update";
            }
        }

        private void controlLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(barcode);
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
            //if (cost.IsFocused)
            //{
            //    cost.SelectAll();
            //}
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
                printName.Text.Trim().Equals("") /*|| cost.Text.Trim().Equals("")*/ || mrp.Text.Trim().Equals(""))
            {
                error_text.Visibility = Visibility.Visible;
            }
            else
            {
                error_text.Visibility = Visibility.Collapsed;
                submitButton.Visibility = Visibility.Collapsed;
                createProgress.Visibility = Visibility.Visible;

                var product = new Product
                {
                    Barcode = barcode.Text,
                    Name = name.Text,
                    PrintName = printName.Text,
                    //Cost = cost.Text,
                    MRP = mrp.Text,
                    stock = long.Parse(stock.Text)
                };

                MongoClient dbClient = new MongoClient("mongodb://Thinkershut:Dev123@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
                var database = dbClient.GetDatabase("main_db");
                try
                {
                    List<String> names = database.ListCollectionNames().ToList<String>();
                    if (!names.Contains("Products"))
                    {
                        database.CreateCollection("Products");
                    }
                    var collection = database.GetCollection<Product>("Products");

                    if (submitButton.Content.Equals("Update"))
                    {
                        var filter = Builders<Product>.Filter.Eq(p => p.Barcode, barcode.Text);
                        collection.ReplaceOne(filter, product);
                    }
                    else
                    {
                        collection.InsertOne(product);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("The Barcode already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                finally
                {
                    submitButton.Visibility = Visibility.Visible;
                    createProgress.Visibility = Visibility.Collapsed;
                    barcode.Text = "";
                    name.Text = "";
                    printName.Text = "";
                    //cost.Text = "";
                    mrp.Text = "";
                    stock.Text = "";
                }
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
                    KeyEventArgs args = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
                    args.RoutedEvent = Keyboard.KeyDownEvent;
                    InputManager.Current.ProcessInput(args);
                    break;
                case Key.Escape:
                    break;
            }
        }
    }
}
