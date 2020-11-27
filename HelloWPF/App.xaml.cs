using HelloWPF.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using TextileApp;

namespace HelloWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string drive = "D://Thinkershut";
        public static string productDatabasePath = drive+"//Products.db";
        public static string licensePath = drive+"//license.lic";
        public static Invoice currentInvoice = null;
        public static string expirationDate = "";

        public static bool validateLicenseFile(string filePath)
        {
            byte[] key = ASCIIEncoding.ASCII.GetBytes("KamehamehaX4".PadLeft(32));
            byte[] iv = ASCIIEncoding.ASCII.GetBytes("KamehamehaX4".PadLeft(16));
            //Create a file stream.
            var assembly = Assembly.GetExecutingAssembly();
            Directory.CreateDirectory(drive);
            if (File.Exists(filePath))
            {
                try
                {
                    FileStream myStream = new FileStream(filePath, FileMode.Open);
                    //Create a new instance of the default Aes implementation class
                    Aes aes = Aes.Create();

                    //Create a CryptoStream, pass it the file stream, and decrypt
                    //it with the Aes class using the key and IV.
                    CryptoStream cryptStream = new CryptoStream(
                       myStream,
                       aes.CreateDecryptor(key, iv),
                       CryptoStreamMode.Read);

                    //Read the stream.
                    StreamReader sReader = new StreamReader(cryptStream);
                    string macAddr = (
                        from nic in NetworkInterface.GetAllNetworkInterfaces()
                        where nic.OperationalStatus == OperationalStatus.Up
                        select nic.GetPhysicalAddress().ToString()
                    ).FirstOrDefault();

                    var licenseObject = JsonSerializer.Deserialize<Dictionary<string,string>>(sReader.ReadToEnd());
                    //Close the streams.
                    sReader.Close();

                    string licenseMac = licenseObject.GetValueOrDefault("macAddress");
                    DateTime licenseDate = DateTime.ParseExact(licenseObject.GetValueOrDefault("expirationDate"),
                                            "dd/M/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                    expirationDate = licenseObject.GetValueOrDefault("expirationDate");
                    bool b = DateTime.Now <= licenseDate;

                    return (licenseMac.Equals(macAddr) && DateTime.Now <= licenseDate);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Soething Went Wrong" + e.Exception.Message, "Application Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }

        public static void printBill(Invoice invoice)
        {
            //Window wd = new Window();
            //FlowDocumentScrollViewer fdr = new FlowDocumentScrollViewer();

            FlowDocument flowDocument = new FlowDocument();

            Paragraph titleParagraph = new Paragraph(new Run("Boys World - Men's Wear"));
            titleParagraph.FontFamily = new FontFamily("Segoe UI");
            titleParagraph.FontWeight = FontWeights.Bold;
            titleParagraph.FontSize = 11;
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.Margin = new Thickness(0.0);
            flowDocument.Blocks.Add(titleParagraph);
            titleParagraph = new Paragraph(new Run("53BI, First Floor, Ambai Road, Alangulam"));
            titleParagraph.FontFamily = new FontFamily("Segoe UI");
            titleParagraph.FontSize = 11;
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.Margin = new Thickness(0.0);
            flowDocument.Blocks.Add(titleParagraph);
            titleParagraph = new Paragraph(new Run("cell:7395831650"));
            titleParagraph.FontFamily = new FontFamily("Segoe UI");
            titleParagraph.FontSize = 11;
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.Margin = new Thickness(0.0);
            flowDocument.Blocks.Add(titleParagraph);

            titleParagraph = new Paragraph(new Run("Cash Bill"));
            titleParagraph.FontFamily = new FontFamily("Segoe UI");
            titleParagraph.FontSize = 11;
            titleParagraph.FontWeight = FontWeights.Bold;
            titleParagraph.TextAlignment = TextAlignment.Center;
            flowDocument.Blocks.Add(titleParagraph);

            Table table = new Table();
            TableRowGroup titleRowGroup = new TableRowGroup();
            table.RowGroups.Add(titleRowGroup);
            TableRow titleRow = new TableRow();
            titleRowGroup.Rows.Add(titleRow);

            titleParagraph = new Paragraph(new Run("Bill No." + invoice.Number));
            titleParagraph.FontFamily = new FontFamily("Segoe UI");
            titleParagraph.FontSize = 11;
            TableCell titleTableCell = new TableCell(titleParagraph);
            titleTableCell.Padding = new Thickness(2);
            titleTableCell.TextAlignment = TextAlignment.Left;
            titleRow.Cells.Add(titleTableCell);

            titleParagraph = new Paragraph(new Run("Date:" + invoice.Date));
            titleParagraph.FontFamily = new FontFamily("Segoe UI");
            titleParagraph.FontSize = 11;
            titleTableCell = new TableCell(titleParagraph);
            titleTableCell.Padding = new Thickness(2);
            titleTableCell.TextAlignment = TextAlignment.Right;
            titleRow.Cells.Add(titleTableCell);

            flowDocument.Blocks.Add(table);

            table = new Table();
            TableColumn column = new TableColumn();
            column.Width = new GridLength(0.4,GridUnitType.Star);
            table.Columns.Add(column);
            column = new TableColumn();
            column.Width = new GridLength(0.1, GridUnitType.Star);
            table.Columns.Add(column); column = new TableColumn();
            column.Width = new GridLength(0.2, GridUnitType.Star);
            table.Columns.Add(column); column = new TableColumn();
            column.Width = new GridLength(0.3, GridUnitType.Star);
            table.Columns.Add(column);

            TableRowGroup rowGroup = new TableRowGroup();
            table.RowGroups.Add(rowGroup);
            TableRow row = new TableRow();
            rowGroup.Rows.Add(row);

            Paragraph paragraph = new Paragraph(new Run("Product"));
            paragraph.FontFamily = new FontFamily("Segoe UI");
            paragraph.FontSize = 11;
            paragraph.FontWeight = FontWeights.Bold;
            TableCell tableCell = new TableCell(paragraph);
            tableCell.Padding = new Thickness(2);
            tableCell.TextAlignment = TextAlignment.Left;
            row.Cells.Add(tableCell);

            paragraph = new Paragraph(new Run("Qty."));
            paragraph.FontFamily = new FontFamily("Segoe UI");
            paragraph.FontSize = 11;
            paragraph.FontWeight = FontWeights.Bold;
            tableCell = new TableCell(paragraph);
            tableCell.Padding = new Thickness(2);
            tableCell.TextAlignment = TextAlignment.Right;
            row.Cells.Add(tableCell);

            paragraph = new Paragraph(new Run("Price"));
            paragraph.FontFamily = new FontFamily("Segoe UI");
            paragraph.FontSize = 11;
            paragraph.FontWeight = FontWeights.Bold;
            tableCell = new TableCell(paragraph);
            tableCell.Padding = new Thickness(2);
            tableCell.TextAlignment = TextAlignment.Right;
            row.Cells.Add(tableCell);

            paragraph = new Paragraph(new Run("Total"));
            paragraph.FontFamily = new FontFamily("Segoe UI");
            paragraph.FontSize = 11;
            paragraph.FontWeight = FontWeights.Bold;
            tableCell = new TableCell(paragraph);
            tableCell.Padding = new Thickness(2);
            tableCell.TextAlignment = TextAlignment.Right;
            row.Cells.Add(tableCell);

            List<BillingProduct> products = JsonSerializer.Deserialize<List<BillingProduct>>(invoice.BillingProducts);
            foreach (BillingProduct product in products)
            {
                row = new TableRow();
                rowGroup.Rows.Add(row);

                paragraph = new Paragraph(new Run(product.PrintName));
                paragraph.FontFamily = new FontFamily("Segoe UI");
                paragraph.FontSize = 11;
                tableCell = new TableCell(paragraph);
                tableCell.Padding = new Thickness(2);
                tableCell.TextAlignment = TextAlignment.Left;
                row.Cells.Add(tableCell);

                paragraph = new Paragraph(new Run(product.Quantity.ToString()));
                paragraph.FontFamily = new FontFamily("Segoe UI");
                paragraph.FontSize = 11;
                paragraph.FontSize = 11;
                tableCell = new TableCell(paragraph);
                tableCell.Padding = new Thickness(2);
                tableCell.TextAlignment = TextAlignment.Right;
                row.Cells.Add(tableCell);

                paragraph = new Paragraph(new Run(product.MRP.ToString()));
                paragraph.FontFamily = new FontFamily("Segoe UI");
                paragraph.FontSize = 11;
                tableCell = new TableCell(paragraph);
                tableCell.Padding = new Thickness(2);
                tableCell.TextAlignment = TextAlignment.Right;
                row.Cells.Add(tableCell);

                paragraph = new Paragraph(new Run(product.Total.ToString()));
                paragraph.FontFamily = new FontFamily("Segoe UI");
                paragraph.FontSize = 11;
                tableCell = new TableCell(paragraph);
                tableCell.Padding = new Thickness(2);
                tableCell.TextAlignment = TextAlignment.Right;
                row.Cells.Add(tableCell);
            }

            flowDocument.Blocks.Add(table);

            table = new Table();
            TableRowGroup footerRowGroup = new TableRowGroup();
            table.RowGroups.Add(footerRowGroup);
            TableRow footerRow = new TableRow();
            footerRowGroup.Rows.Add(footerRow);

            int total = products.Sum(product => product.Quantity);
            titleParagraph = new Paragraph(new Run("Total Items:"+total.ToString()));
            titleParagraph.FontFamily = new FontFamily("Segoe UI");
            titleParagraph.FontSize = 11;
            TableCell footerTableCell = new TableCell(titleParagraph);
            footerTableCell.Padding = new Thickness(2);
            footerTableCell.TextAlignment = TextAlignment.Left;
            footerRow.Cells.Add(footerTableCell);

            if(invoice.Discount != null && invoice.Discount != "0")
            {
                footerRow = new TableRow();
                footerRowGroup.Rows.Add(footerRow);

                titleParagraph = new Paragraph(new Run("Net Amt."));
                titleParagraph.FontFamily = new FontFamily("Segoe UI");
                titleParagraph.FontSize = 11;
                titleParagraph.FontWeight = FontWeights.Bold;
                footerTableCell = new TableCell(titleParagraph);
                footerTableCell.Padding = new Thickness(2);
                footerTableCell.TextAlignment = TextAlignment.Left;
                footerRow.Cells.Add(footerTableCell);

                titleParagraph = new Paragraph(new Run((int.Parse(invoice.Total) + int.Parse(invoice.Discount)).ToString()));
                titleParagraph.FontFamily = new FontFamily("Segoe UI");
                titleParagraph.FontSize = 13;
                titleParagraph.FontWeight = FontWeights.Bold;
                footerTableCell = new TableCell(titleParagraph);
                footerTableCell.Padding = new Thickness(2);
                footerTableCell.TextAlignment = TextAlignment.Right;
                footerRow.Cells.Add(footerTableCell);

                footerRow = new TableRow();
                footerRowGroup.Rows.Add(footerRow);

                titleParagraph = new Paragraph(new Run("Discount"));
                titleParagraph.FontFamily = new FontFamily("Segoe UI");
                titleParagraph.FontSize = 11;
                footerTableCell = new TableCell(titleParagraph);
                footerTableCell.Padding = new Thickness(2);
                footerTableCell.TextAlignment = TextAlignment.Left;
                footerRow.Cells.Add(footerTableCell);

                titleParagraph = new Paragraph(new Run(invoice.Discount));
                titleParagraph.FontFamily = new FontFamily("Segoe UI");
                titleParagraph.FontSize = 11;
                footerTableCell = new TableCell(titleParagraph);
                footerTableCell.Padding = new Thickness(2);
                footerTableCell.TextAlignment = TextAlignment.Right;
                footerRow.Cells.Add(footerTableCell);

                footerRow = new TableRow();
                footerRowGroup.Rows.Add(footerRow);

                titleParagraph = new Paragraph(new Run("Gross Amt."));
                titleParagraph.FontFamily = new FontFamily("Segoe UI");
                titleParagraph.FontSize = 11;
                titleParagraph.FontWeight = FontWeights.Bold;
                footerTableCell = new TableCell(titleParagraph);
                footerTableCell.Padding = new Thickness(2);
                footerTableCell.TextAlignment = TextAlignment.Left;
                footerRow.Cells.Add(footerTableCell);

                titleParagraph = new Paragraph(new Run(invoice.Total));
                titleParagraph.FontFamily = new FontFamily("Segoe UI");
                titleParagraph.FontSize = 13;
                titleParagraph.FontWeight = FontWeights.Bold;
                footerTableCell = new TableCell(titleParagraph);
                footerTableCell.Padding = new Thickness(2);
                footerTableCell.TextAlignment = TextAlignment.Right;
                footerRow.Cells.Add(footerTableCell);
            } else
            {
                footerRow = new TableRow();
                footerRowGroup.Rows.Add(footerRow);

                titleParagraph = new Paragraph(new Run("Net Amt."));
                titleParagraph.FontFamily = new FontFamily("Segoe UI");
                titleParagraph.FontSize = 11;
                titleParagraph.FontWeight = FontWeights.Bold;
                footerTableCell = new TableCell(titleParagraph);
                footerTableCell.Padding = new Thickness(2);
                footerTableCell.TextAlignment = TextAlignment.Left;
                footerRow.Cells.Add(footerTableCell);

                titleParagraph = new Paragraph(new Run(invoice.Total));
                titleParagraph.FontFamily = new FontFamily("Segoe UI");
                titleParagraph.FontSize = 13;
                titleParagraph.FontWeight = FontWeights.Bold;
                footerTableCell = new TableCell(titleParagraph);
                footerTableCell.Padding = new Thickness(2);
                footerTableCell.TextAlignment = TextAlignment.Right;
                footerRow.Cells.Add(footerTableCell);
            }

            flowDocument.Blocks.Add(table);

            titleParagraph = new Paragraph(new Run("Thank you for shopping with us!!!"));
            titleParagraph.FontFamily = new FontFamily("Segoe UI");
            titleParagraph.FontSize = 11;
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.Margin = new Thickness(0.0);
            flowDocument.Blocks.Add(titleParagraph);
            titleParagraph = new Paragraph(new Run("boysworld2016@gmail.com"));
            titleParagraph.FontFamily = new FontFamily("Segoe UI");
            titleParagraph.FontSize = 11;
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.Margin = new Thickness(0.0);
            flowDocument.Blocks.Add(titleParagraph);

            flowDocument.MaxPageWidth = 270;
            //fdr.Document = flowDocument;
            //wd.Content = fdr;
            //wd.Show();
            PrintDialog dialog = new PrintDialog();
            bool result = (bool)dialog.ShowDialog();
            if (result)
            {
                IDocumentPaginatorSource idpSource = flowDocument;
                dialog.PrintDocument(idpSource.DocumentPaginator, "Bill");
            }
        }
    }
}
