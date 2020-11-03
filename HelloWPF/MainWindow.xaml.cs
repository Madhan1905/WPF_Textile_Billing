using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace HelloWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void keyEventHandler(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F1)
            {
                MainWindowControl.Content = new BillingControl();
            } else if(e.Key == Key.F2)
            {
                MainWindowControl.Content = new ViewItemsControl();
            }
            
        }

    }
}
