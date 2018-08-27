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

        static TcpClient client;
        public MainWindow()
        {
            try
            {
                client = new TcpClient();
                int port = 65432;
                //gets the possible Host's
                string server = Dns.GetHostName();
                //gets IP Host info from variable server
                IPHostEntry ipHostInfo = Dns.GetHostEntry(server);
                IPAddress ipAddress = null;
                //loops into ipHostinfos IPAddress's
                for (int i = 0; i < ipHostInfo.AddressList.Length; ++i)
                {
                    //checks if the current ip addrees Family type is equel InterNetwork
                    if (ipHostInfo.AddressList[i].AddressFamily ==
                      AddressFamily.InterNetwork)
                    {
                        ipAddress = ipHostInfo.AddressList[i];
                        break;
                    }
                }
                //throw exception if none were to find
                if (ipAddress == null)
                {
                    throw new Exception("No IPv4 address for server");
                }
                this.Connect(ipAddress, port); // Connect
            }
            catch (Exception)
            {
                throw;
            }
            InitializeComponent();
        }
        private async void Connect(IPAddress iPAddress, int port)
        {
            await client.ConnectAsync(iPAddress, port);
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
                Task<string> tsResponse = SendRequest(method, data);
                //give client text
                listBox1.Items.Add("Sent request, waiting for response");
                //waits for response
                await tsResponse;
                //converts the response into decimal
                decimal dResponse = decimal.Parse(tsResponse.Result);
                listBox1.Items.Add("Received response: " +
                 dResponse.ToString("F2"));
            }
            catch (Exception ex)
            {
                listBox1.Items.Add(ex.Message);
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
        private static async Task<string> SendRequest(string method, string data)
        {
            try
            {
                string response = null;

                //create's the TCP client object
                //create's the object that streams the data from client to server
                NetworkStream networkStream = client.GetStream();
                StreamWriter writer = new StreamWriter(networkStream);
                StreamReader reader = new StreamReader(networkStream);

                //sets autoflush to true so i will see on its own if we are trying to send something to the sever
                writer.AutoFlush = true;
                //request data string (sends to sevrer)
                string requestData = "method=" + method + "&" + "data=" +
                  data + "&eor"; // 'End-of-request'
                                 //write's the request to server async
                await writer.WriteLineAsync(requestData);
                //wait for the response
                response = await reader.ReadLineAsync();

                return response;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            client.Dispose();
        }
    }
}
