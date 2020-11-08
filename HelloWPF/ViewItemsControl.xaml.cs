using HelloWPF.Classes;
using SQLite;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HelloWPF
{
    /// <summary>
    /// Interaction logic for ViewItemsControl.xaml
    /// </summary>
    public partial class ViewItemsControl : UserControl
    {
        public ViewItemsControl()
        {
            InitializeComponent();
            populateTable();
        }

        private void populateTable()
        {
            using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
            {
                dbConnection.CreateTable<Product>();
                var products = dbConnection.Table<Product>().ToList();
                if(itemsTable.RowGroups.Count > 1)
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
                    createTableIconCell(newRow,rowIndex,"edit", product);
                    createTableIconCell(newRow, rowIndex, "delete", product);
                    creatTableCell((rowIndex+1).ToString(), newRow, true, true);
                    creatTableCell(product.Barcode, newRow, true, true);
                    creatTableCell(product.Name, newRow, true, false);
                    creatTableCell(product.PrintName, newRow, true, true);
                    creatTableCell(product.Cost, newRow, true, true);
                    creatTableCell(product.MRP, newRow, false, true);
                    rowIndex++;
                }
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
            } else
            {
                button.Click += deleteButtonEvent;
            }
            button.Tag = product;
            button.Content = image;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.Background = rowIndex % 2 == 0 ? Brushes.LightSteelBlue : Brushes.White ;
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
                    using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
                    {
                        dbConnection.CreateTable<Product>();
                        dbConnection.Delete(product);
                        populateTable();
                    }
                    break;
                case MessageBoxResult.No: break;
            }
        }

        private void editButtonEvent(Object Sender, RoutedEventArgs e)
        {
            Button button = (Button)Sender;
            Product product = (Product)button.Tag;
            AddProductWindow addProductWindow = new AddProductWindow(product);
            addProductWindow.ShowDialog();

            populateTable();
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
            if(border)
            {
                tableCell.BorderThickness = new Thickness(0, 0, 1, 0);
            }
            tableCell.BorderBrush = Brushes.Black;
            row.Cells.Add(tableCell);
        }

        private void Add_Item(Object Sender, RoutedEventArgs e)
        {
            addData();
        }

        public void addData()
        {
            Product product = new Product()
            {
                Barcode = "",
                Name = "",
                SellingPrice = "",
                Cost = "",
                MRP = ""
            };
            AddProductWindow addProductWindow = new AddProductWindow(product);
            addProductWindow.ShowDialog();

            populateTable();
        }
    }
}
