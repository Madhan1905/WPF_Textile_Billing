using System;
using System.Windows;
using System.Windows.Documents;

namespace HelloWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            for(int rowIndex = 1; rowIndex < 10; rowIndex++)
            {
                itemsTable.RowGroups[0].Rows.Add(new TableRow());
                TableRow newRow = itemsTable.RowGroups[0].Rows[rowIndex];
                newRow.Cells.Add(new TableCell(new Paragraph(new Run("first cell"))));
                newRow.Cells.Add(new TableCell(new Paragraph(new Run("first cell"))));
                newRow.Cells.Add(new TableCell(new Paragraph(new Run("first cell"))));
            }
        }

        private void Add_Item(Object Sender, RoutedEventArgs e)
        {
            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.ShowDialog();
        }

    }
}
