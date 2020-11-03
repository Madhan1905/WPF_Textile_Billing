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
using System.Windows.Shapes;

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
    }
}
