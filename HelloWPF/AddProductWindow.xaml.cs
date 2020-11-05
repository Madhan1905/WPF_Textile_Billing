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

            var sysHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            var sysWidth = System.Windows.SystemParameters.PrimaryScreenWidth;

            this.Height = (sysHeight * 0.50);
            this.Width = (sysWidth * 0.40);
            this.Left = sysWidth*0.5 - this.Width*0.5;
            this.Top = sysHeight*0.5 - this.Height *0.5;

            barcode.Text = product.Barcode;
            name.Text = product.Name;
            description.Text = product.Description;
            cost.Text = product.Cost;
            mrp.Text = product.MRP;
            if (!product.Barcode.Equals(""))
            {
                submitButton.Content = "Update";
            }
        }

        private void submitButtonEvent(Object sender, RoutedEventArgs e)
        {
            submitButton.Visibility = Visibility.Collapsed;
            createProgress.Visibility = Visibility.Visible;

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
                try{
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

        private void NumberValidationEvent(Object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
