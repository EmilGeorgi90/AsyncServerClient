using Squirrel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncClientServer.Client.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpClient client = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //gets the selected method from the combobox
                string method = (string)comboBox1.SelectedItem;
                //gets the data from the text box
                string data = textBox1.Text;
                //calls the request method
                Task<string> tsResponse = TCPClientConConfig.SendRequest(client,method, data);
                //give client text
                listBox1.Items.Add("Sent request, waiting for response");
                //waits for response
                await tsResponse;
                //converts the response into decimal
                decimal dResponse = decimal.Parse(tsResponse.Result);
                listBox1.Items.Add("Received response: " +
                 dResponse.ToString("F2"));
            }
            catch (FormatException ex)
            {
                listBox1.Items.Add(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
                App.Current.Shutdown();
            }
        }
        /// <summary>
        /// Sends Request to TCP Server
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <returns>string, reuslt of request or exception log</returns>
 

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            client.Dispose();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TCPClientConConfig clientConConfig = new TCPClientConConfig();
            client = await clientConConfig.Connect();
        }
    }
}
