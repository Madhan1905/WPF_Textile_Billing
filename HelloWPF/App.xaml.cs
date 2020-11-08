using HelloWPF.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace HelloWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string productDatabasePath = Path.Combine(folderPath, "Products.db");
        public static Invoice currentInvoice = null;

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Soething Went Wrong" + e.Exception.Message, "Application Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }

        public static void printBill(Invoice invoice)
        {
            FlowDocument flowDocument = new FlowDocument();

            Paragraph titleParagraph = new Paragraph(new Run("Boys World - Men's Fashion"));
            titleParagraph.FontFamily = new FontFamily("Futura Medium");
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.Margin = new Thickness(0.0);
            flowDocument.Blocks.Add(titleParagraph);
            titleParagraph = new Paragraph(new Run("53BI, First Floor, Ambani Road, Alangulam"));
            titleParagraph.FontFamily = new FontFamily("Futura Medium");
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.Margin = new Thickness(0.0);
            flowDocument.Blocks.Add(titleParagraph);
            titleParagraph = new Paragraph(new Run("cell:7395831650-9629159447"));
            titleParagraph.FontFamily = new FontFamily("Futura Medium");
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.Margin = new Thickness(0.0);
            flowDocument.Blocks.Add(titleParagraph);

            titleParagraph = new Paragraph(new Run("Cash Bill"));
            titleParagraph.FontFamily = new FontFamily("Futura Medium");
            titleParagraph.FontWeight = FontWeights.Bold;
            titleParagraph.TextAlignment = TextAlignment.Center;
            flowDocument.Blocks.Add(titleParagraph);

            Table table = new Table();
            TableRowGroup titleRowGroup = new TableRowGroup();
            table.RowGroups.Add(titleRowGroup);
            TableRow titleRow = new TableRow();
            titleRowGroup.Rows.Add(titleRow);

            titleParagraph = new Paragraph(new Run("Bill No." + invoice.Number));
            titleParagraph.FontFamily = new FontFamily("Futura Medium");
            TableCell titleTableCell = new TableCell(titleParagraph);
            titleTableCell.Padding = new Thickness(2);
            titleTableCell.TextAlignment = TextAlignment.Left;
            titleRow.Cells.Add(titleTableCell);

            titleParagraph = new Paragraph(new Run("Date:" + invoice.Date));
            titleParagraph.FontFamily = new FontFamily("Futura Medium");
            titleTableCell = new TableCell(titleParagraph);
            titleTableCell.Padding = new Thickness(2);
            titleTableCell.TextAlignment = TextAlignment.Right;
            titleRow.Cells.Add(titleTableCell);

            flowDocument.Blocks.Add(table);

            table = new Table();
            TableRowGroup rowGroup = new TableRowGroup();
            table.RowGroups.Add(rowGroup);
            TableRow row = new TableRow();
            rowGroup.Rows.Add(row);

            Paragraph paragraph = new Paragraph(new Run("Product"));
            paragraph.FontFamily = new FontFamily("Futura Medium");
            paragraph.FontWeight = FontWeights.Bold;
            TableCell tableCell = new TableCell(paragraph);
            tableCell.Padding = new Thickness(2);
            tableCell.TextAlignment = TextAlignment.Left;
            row.Cells.Add(tableCell);

            paragraph = new Paragraph(new Run("Qty."));
            paragraph.FontFamily = new FontFamily("Futura Medium");
            paragraph.FontWeight = FontWeights.Bold;
            tableCell = new TableCell(paragraph);
            tableCell.Padding = new Thickness(2);
            tableCell.TextAlignment = TextAlignment.Center;
            row.Cells.Add(tableCell);

            paragraph = new Paragraph(new Run("Price"));
            paragraph.FontFamily = new FontFamily("Futura Medium");
            paragraph.FontWeight = FontWeights.Bold;
            tableCell = new TableCell(paragraph);
            tableCell.Padding = new Thickness(2);
            tableCell.TextAlignment = TextAlignment.Center;
            row.Cells.Add(tableCell);

            paragraph = new Paragraph(new Run("Total"));
            paragraph.FontFamily = new FontFamily("Futura Medium");
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
                paragraph.FontFamily = new FontFamily("Futura Medium");
                tableCell = new TableCell(paragraph);
                tableCell.Padding = new Thickness(2);
                tableCell.TextAlignment = TextAlignment.Left;
                row.Cells.Add(tableCell);

                paragraph = new Paragraph(new Run(product.Quantity.ToString()));
                paragraph.FontFamily = new FontFamily("Futura Medium");
                tableCell = new TableCell(paragraph);
                tableCell.Padding = new Thickness(2);
                tableCell.TextAlignment = TextAlignment.Center;
                row.Cells.Add(tableCell);

                paragraph = new Paragraph(new Run(product.MRP.ToString()));
                paragraph.FontFamily = new FontFamily("Futura Medium");
                tableCell = new TableCell(paragraph);
                tableCell.Padding = new Thickness(2);
                tableCell.TextAlignment = TextAlignment.Center;
                row.Cells.Add(tableCell);

                paragraph = new Paragraph(new Run(product.Total.ToString()));
                paragraph.FontFamily = new FontFamily("Futura Medium");
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

            titleParagraph = new Paragraph(new Run("Net Amt."));
            titleParagraph.FontFamily = new FontFamily("Futura Medium");
            titleParagraph.FontWeight = FontWeights.Bold;
            TableCell footerTableCell = new TableCell(titleParagraph);
            footerTableCell.Padding = new Thickness(2);
            footerTableCell.TextAlignment = TextAlignment.Left;
            footerRow.Cells.Add(footerTableCell);

            titleParagraph = new Paragraph(new Run(invoice.Total));
            titleParagraph.FontFamily = new FontFamily("Futura Medium");
            titleParagraph.FontWeight = FontWeights.Bold;
            footerTableCell = new TableCell(titleParagraph);
            footerTableCell.Padding = new Thickness(2);
            footerTableCell.TextAlignment = TextAlignment.Right;
            footerRow.Cells.Add(footerTableCell);

            flowDocument.Blocks.Add(table);

            titleParagraph = new Paragraph(new Run("Thank you for shopping"));
            titleParagraph.FontFamily = new FontFamily("Futura Medium");
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.Margin = new Thickness(0.0);
            flowDocument.Blocks.Add(titleParagraph);
            titleParagraph = new Paragraph(new Run("boysworld2016@gmail.com"));
            titleParagraph.FontFamily = new FontFamily("Futura Medium");
            titleParagraph.TextAlignment = TextAlignment.Center;
            titleParagraph.Margin = new Thickness(0.0);
            flowDocument.Blocks.Add(titleParagraph);

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
