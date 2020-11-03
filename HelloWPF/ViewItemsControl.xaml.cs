using HelloWPF.Classes;
using SQLite;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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
            string databaseName = "Products.db";
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string databasePath = Path.Combine(folderPath, databaseName);

            using (SQLiteConnection dbConnection = new SQLiteConnection(databasePath))
            {
                dbConnection.CreateTable<Product>();
                var products = dbConnection.Table<Product>().ToList();
                int rowIndex = 1;
                foreach (Product product in products)
                {
                    itemsTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow newRow = itemsTable.RowGroups[0].Rows[rowIndex];
                    newRow.Cells.Add(new TableCell(new Paragraph(new Run(rowIndex.ToString()))));
                    newRow.Cells.Add(new TableCell(new Paragraph(new Run(product.Barcode))));
                    newRow.Cells.Add(new TableCell(new Paragraph(new Run(product.Name))));
                    newRow.Cells.Add(new TableCell(new Paragraph(new Run(product.Description))));
                    newRow.Cells.Add(new TableCell(new Paragraph(new Run(product.Cost))));
                    newRow.Cells.Add(new TableCell(new Paragraph(new Run(product.MRP))));
                    rowIndex++;
                }
            }
        }

        private void Add_Item(Object Sender, RoutedEventArgs e)
        {
            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.ShowDialog();
        }
    }
}
