using HelloWPF;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace TextileApp
{
    /// <summary>
    /// Interaction logic for LicenseControl.xaml
    /// </summary>
    public partial class LicenseControl : UserControl
    {
        Window MainWindow;
        ContentControl MainWindowControl;
        Grid footer_grid;
        public LicenseControl(Window window, ContentControl control,Grid grid)
        {
            InitializeComponent();

            var sysHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            var sysWidth = System.Windows.SystemParameters.PrimaryScreenWidth;

            upload_button.Margin = new Thickness(0, sysWidth * 0.02, 0, 0);
            upload_button.Height = sysHeight * 0.035;
            submit_button.Margin = new Thickness(0, sysWidth * 0.02, 0, 0);
            submit_button.Height = sysHeight * 0.035;

            MainWindow = window;
            MainWindowControl = control;
            footer_grid = grid;

            string macAddr = (
                        from nic in NetworkInterface.GetAllNetworkInterfaces()
                        where nic.OperationalStatus == OperationalStatus.Up
                        select nic.GetPhysicalAddress().ToString()
                    ).FirstOrDefault();
            ErrorLabel.Content = "Your license file is invalid or expired for the mac-id:" + macAddr + ".";
        }

        private void uploadEvent(Object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "License File (.lic)|*.lic";
            if (dialog.ShowDialog() == true)
            {
                filePath_text.Text = dialog.FileName;
                upload_button.Visibility = Visibility.Collapsed;
                submit_button.Visibility = Visibility.Visible;
            }
        }

        private void submitEvent(Object sender, RoutedEventArgs e)
        {
            if (App.validateLicenseFile(filePath_text.Text))
            {
                try
                {
                    //Create a file stream
                    FileStream myStream = new FileStream(App.licensePath, FileMode.OpenOrCreate);

                    //Create a new instance of the default Aes implementation class  
                    // and encrypt the stream.  
                    Aes aes = Aes.Create();

                    byte[] key = ASCIIEncoding.ASCII.GetBytes("KamehamehaX4".PadLeft(32));
                    byte[] iv = ASCIIEncoding.ASCII.GetBytes("KamehamehaX4".PadLeft(16));

                    //Create a CryptoStream, pass it the FileStream, and encrypt
                    //it with the Aes class.  
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Key = key;
                    aes.IV = iv;
                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);


                    CryptoStream cryptStream = new CryptoStream(
                        myStream,
                        aes.CreateEncryptor(key, iv),
                        CryptoStreamMode.Write);

                    //Create a StreamWriter for easy writing to the
                    //file stream.  
                    StreamWriter sWriter = new StreamWriter(cryptStream);

                    //Write to the stream.  
                    string macAddr = (
                        from nic in NetworkInterface.GetAllNetworkInterfaces()
                        where nic.OperationalStatus == OperationalStatus.Up
                        select nic.GetPhysicalAddress().ToString()
                    ).FirstOrDefault();

                    var licenseDetails = new Dictionary<string, string>();
                    licenseDetails.Add("macAddress", macAddr);
                    licenseDetails.Add("expirationDate", App.expirationDate);
                    string jsonString = JsonSerializer.Serialize(licenseDetails);
                    sWriter.WriteLine(jsonString);

                    //Close all the connections.  
                    sWriter.Close();
                    cryptStream.Close();
                    myStream.Close();

                    MainWindow.WindowState = WindowState.Maximized;
                    footer_grid.Visibility = Visibility.Visible;
                    MainWindowControl.Content = new HomeControl(MainWindowControl);
                }
                catch
                {
                    //Inform the user that an exception was raised.  
                    Console.WriteLine("The encryption failed.");
                    Console.ReadKey();
                    throw;
                }

            }
            else
            {
                error_text.Visibility = Visibility.Visible;
                filePath_text.Text = "";
                submit_button.Visibility = Visibility.Collapsed;
                upload_button.Visibility = Visibility.Visible;
            }
        }
    }
}
