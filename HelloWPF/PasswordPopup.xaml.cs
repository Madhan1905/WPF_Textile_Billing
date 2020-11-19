using HelloWPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TextileApp
{
    /// <summary>
    /// Interaction logic for PasswordPopup.xaml
    /// </summary>
    public partial class PasswordPopup : Window
    {
        ContentControl MainWindowControl;
        public PasswordPopup(ContentControl WindowControl)
        {
            InitializeComponent();

            var sysHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            var sysWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = (sysHeight * 0.2);
            this.Width = (sysWidth * 0.3);
            this.Left = sysWidth * 0.5 - this.Width * 0.5;
            this.Top = sysHeight * 0.5 - this.Height * 0.5; 

            login_button.Margin = new Thickness(0, sysWidth * 0.01,0, 0);
            login_button.Height = sysHeight * 0.035;

            validatePasswordProgress.Margin = new Thickness(0, sysWidth * 0.01, 0, 0);
            validatePasswordProgress.Height = sysHeight * 0.035;

            MainWindowControl = WindowControl;
            password_text.Focus();
        }

        private void popupKeyEvent(Object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                validatePassword();
            }
        }

        private void submitButtonEvent(Object sender, RoutedEventArgs e)
        {
            validatePassword();
        }

        private void validatePassword()
        {
            if(password_text.Password != "")
            {
                login_button.Visibility = Visibility.Collapsed;
                validatePasswordProgress.Visibility = Visibility.Visible;

                byte[] key = ASCIIEncoding.ASCII.GetBytes("KamehamehaX4".PadLeft(32));
                byte[] iv = ASCIIEncoding.ASCII.GetBytes("KamehamehaX4".PadLeft(16));
                //Create a file stream.
                var assembly = Assembly.GetExecutingAssembly();
                Stream myStream = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames()
            .First(str => str.EndsWith("secure.enc")));

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

                string s = sReader.ReadToEnd().Replace("\r\n","");
                if (s.Equals(password_text.Password))
                {
                    MainWindowControl.Content = new ViewItemsControl(this);
                }
                //Close the streams.
                sReader.Close();
            }
        }
    }
}
