using AsyncClientServer.Protocol;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncClientServer.Client
{
    public class TCPClientConConfig
    {
        public async Task<TcpClient> Connect()
        {
            try
            {
                System.Net.Sockets.TcpClient client = new TcpClient();
                int port = 44818;
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
                await client.ConnectAsync(IPAddress.Parse("10.143.78.18"), port); // Connect
                return client;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task<string> SendRequest(TcpClient client, string method, string data)
        {
            try
            {
                string response = null;
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add("methods");
                dataSet.Tables.Add("data");
                dataSet.Tables["methods"].Columns.Add("method");
                dataSet.Tables["data"].Columns.Add("data");
                dataSet.Tables["methods"].Rows.Add("method").SetField<string>("method", method);
                dataSet.Tables["data"].Rows.Add("data").SetField<string>("data", data);
                string requestData = Newtonsoft.Json.JsonConvert.SerializeObject(dataSet);
                //create's the object that streams the data from client to server
                NetworkStream networkStream = client.GetStream();
                StreamWriter writer = new StreamWriter(networkStream);
                StreamReader reader = new StreamReader(networkStream);
                //sets autoflush to true so i will see on its own if we are trying to send something to the sever
                writer.AutoFlush = true;
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

        public WebRequest CustomProtocol()
        {
            WebRequest.RegisterPrefix("custom", new CustomWebRequestCreator());
            WebRequest req = WebRequest.Create("custom://customHost.contoso.com/");
            return req;
        }
    }
}
