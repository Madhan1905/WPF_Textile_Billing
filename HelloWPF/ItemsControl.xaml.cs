using HelloWPF;
using HelloWPF.Classes;
using MongoDB.Bson;
using MongoDB.Driver;
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

namespace TextileApp
{
    /// <summary>
    /// Interaction logic for ItemsControl.xaml
    /// </summary>
    public partial class ItemsControl : UserControl
    {
        List<Product> products;
        int entryCount = 0;
        ContentControl adminControl;
        public ItemsControl(ContentControl control)
        {
            InitializeComponent();
            adminControl = control;

            MongoClient dbClient = new MongoClient("mongodb://Thinkershut:Dev123@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
            var database = dbClient.GetDatabase("main_db");
            List<String> names = database.ListCollectionNames().ToList<String>();
            if (!names.Contains("Products"))
            {
                database.CreateCollection("Products");
            }
            var collection = database.GetCollection<Product>("Products");
            List<Product> products = collection.Find<Product>(new BsonDocument()).Limit(15).ToList<Product>();
            populateTable(products);
        }

        private void populateTable(List<Product> products)
        {
            if (itemsTable.RowGroups.Count > 1)
            {
                itemsTable.RowGroups.Remove(itemsTable.RowGroups[1]);
            }
            itemsTable.RowGroups.Add(new TableRowGroup());
            int rowIndex = 0;
            foreach (Product product in products)
            {
                itemsTable.RowGroups[1].Rows.Add(new TableRow());
                TableRow newRow = itemsTable.RowGroups[1].Rows[rowIndex];
                if (rowIndex % 2 == 0)
                {
                    newRow.Background = Brushes.LightSteelBlue;
                }
                createTableIconCell(newRow, rowIndex, "edit", product);
                createTableIconCell(newRow, rowIndex, "delete", product);
                creatTableCell(((15 * entryCount) + (rowIndex + 1)).ToString(), newRow, true, true);
                creatTableCell(product.Barcode, newRow, true, true);
                creatTableCell(product.Name, newRow, true, false);
                creatTableCell(product.PrintName, newRow, true, true);
                creatTableCell(product.Cost, newRow, true, true);
                creatTableCell(product.MRP, newRow, true, true);
                creatTableCell(product.stock.ToString(), newRow, false, true);
                rowIndex++;
            }
        }

        private void createTableIconCell(TableRow row, int rowIndex, string type, Product product)
        {
            string uriPath = "pack://application:,,,/TextileApp;component/Images/" + (type.Equals("edit") ? "edit.png" : "delete.png");
            Image image = new Image { Source = new BitmapImage(new Uri(uriPath)) };
            image.Height = 25;
            image.Width = 20;

            Button button = new Button();
            if (type.Equals("edit"))
            {
                button.Click += editButtonEvent;
            }
            else
            {
                button.Click += deleteButtonEvent;
            }
            button.Tag = product;
            button.Content = image;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.Background = rowIndex % 2 == 0 ? Brushes.LightSteelBlue : Brushes.White;
            button.BorderThickness = new Thickness(0, 0, 0, 0);

            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(button);
            TableCell tableCell = new TableCell(paragraph);
            tableCell.BorderThickness = new Thickness(0, 0, 1, 0);
            tableCell.BorderBrush = Brushes.Black;
            tableCell.TextAlignment = TextAlignment.Center;
            row.Cells.Add(tableCell);
        }

        private void deleteButtonEvent(Object Sender, RoutedEventArgs e)
        {
            Button button = (Button)Sender;
            Product product = (Product)button.Tag;
            MessageBoxResult result = MessageBox.Show("Are you sure, you want to delete?", "Confirmation", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    MongoClient dbClient = new MongoClient("mongodb://Thinkershut:Dev123@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
                    var database = dbClient.GetDatabase("main_db");
                    List<String> names = database.ListCollectionNames().ToList<String>();
                    if (!names.Contains("Products"))
                    {
                        database.CreateCollection("Products");
                    }
                    var collection = database.GetCollection<Product>("Products");
                    collection.DeleteOne(product.ToBsonDocument());
                    products = collection.Find<Product>(new BsonDocument()).Limit(15).ToList<Product>();
                    populateTable(products);
                    break;
                case MessageBoxResult.No: break;
            }
        }

        private void editButtonEvent(Object Sender, RoutedEventArgs e)
        {
            Button button = (Button)Sender;
            Product product = (Product)button.Tag;
            adminControl.Content = new AddItemsControl(product);
        }

        private void creatTableCell(string value, TableRow row, Boolean border, Boolean isCenter)
        {
            TextBlock textBlock = new TextBlock(new Run(value));
            textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(textBlock);
            paragraph.FontFamily = new FontFamily("Futura Medium");
            TableCell tableCell = new TableCell(paragraph);
            tableCell.Padding = new Thickness(2);
            if (isCenter)
            {
                tableCell.TextAlignment = TextAlignment.Center;
            }
            if (border)
            {
                tableCell.BorderThickness = new Thickness(0, 0, 1, 0);
            }
            tableCell.BorderBrush = Brushes.Black;
            row.Cells.Add(tableCell);
        }

        private void search_event(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (search_text.Text != "")
                {
                    MongoClient dbClient = new MongoClient("mongodb://Thinkershut:Dev123@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
                    var database = dbClient.GetDatabase("main_db");
                    List<String> names = database.ListCollectionNames().ToList<String>();
                    if (!names.Contains("Products"))
                    {
                        database.CreateCollection("Products");
                    }
                    var collection = database.GetCollection<Product>("Products");
                    products = collection.Find<Product>(new BsonDocument()).Limit(15).ToList<Product>();

                    switch (filter_combo.SelectedIndex)
                    {
                        case 0: products = products.FindAll(prod => prod.Barcode == search_text.Text);
                                break;
                        case 1: products = products.FindAll(prod => prod.Name.ToUpper().Contains(search_text.Text.ToUpper())); 
                                break;
                        case 2: products = products.FindAll(prod => prod.PrintName.ToUpper().Contains(search_text.Text.ToUpper())); 
                                break;
                        case 3: if (int.TryParse(search_text.Text, out _)) 
                                {
                                    products = products.FindAll(prod => prod.stock < int.Parse(search_text.Text));
                                } else
                                {
                                    products = new List<Product>();
                                }
                                break;
                    }
                    navigation_grid.Visibility = Visibility.Hidden;
                    populateTable(products);
                }
                else
                {
                    MongoClient dbClient = new MongoClient("mongodb://Thinkershut:Dev123@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
                    var database = dbClient.GetDatabase("main_db");
                    List<String> names = database.ListCollectionNames().ToList<String>();
                    if (!names.Contains("Products"))
                    {
                        database.CreateCollection("Products");
                    }
                    var collection = database.GetCollection<Product>("Products");
                    List<Product> products = collection.Find<Product>(new BsonDocument()).Skip(15*entryCount).Limit(15).ToList<Product>();

                    navigation_grid.Visibility = Visibility.Visible;
                    populateTable(products);
                }

            }
        }

        private void next_event(Object Sender, RoutedEventArgs e)
        {
            entryCount++;
            MongoClient dbClient = new MongoClient("mongodb://Thinkershut:Dev123@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
            var database = dbClient.GetDatabase("main_db");
            List<String> names = database.ListCollectionNames().ToList<String>();
            if (!names.Contains("Products"))
            {
                database.CreateCollection("Products");
            }
            var collection = database.GetCollection<Product>("Products");
            List<Product> products = collection.Find<Product>(new BsonDocument()).Skip(15 * entryCount).Limit(15).ToList<Product>();
            if (products.Count > 0)
            {
                populateTable(products);
            }
            else
            {
                entryCount--;
            }
        }

        private void previous_event(Object Sender, RoutedEventArgs e)
        {
            entryCount--;
            if(entryCount >= 0)
            {
                MongoClient dbClient = new MongoClient("mongodb://Thinkershut:Dev123@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
                var database = dbClient.GetDatabase("main_db");
                List<String> names = database.ListCollectionNames().ToList<String>();
                if (!names.Contains("Products"))
                {
                    database.CreateCollection("Products");
                }
                var collection = database.GetCollection<Product>("Products");
                List<Product> products = collection.Find<Product>(new BsonDocument()).Skip(15 * entryCount).Limit(15).ToList<Product>();

                populateTable(products);
            }
        }
    }
}
