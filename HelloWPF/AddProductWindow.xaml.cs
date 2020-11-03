using System;
using System.Windows;
using System.IO;
using SQLite;
using HelloWPF.Classes;

namespace HelloWPF
{
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();

            var sysHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            var sysWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = (sysHeight * 0.50);
            this.Width = (sysWidth * 0.40);
            this.Left = sysWidth*0.5 - this.Width*0.5;
            this.Top = sysHeight*0.5 - this.Height *0.5;
        }

        private void submitButton(Object sender, RoutedEventArgs e)
        {

            Product product = new Product()
            {
                Barcode = barcode.Text,
                Name = name.Text,
                Description = description.Text,
                Cost = cost.Text,
                MRP = mrp.Text
            };

            string databaseName = "Products.db";
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string databasePath = Path.Combine(folderPath, databaseName);

            using (SQLiteConnection dbConnection = new SQLiteConnection(databasePath))
            {
                dbConnection.CreateTable<Product>();
                dbConnection.Insert(product);
            }

            Close();
        }
    }
}
