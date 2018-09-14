using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AsyncClientServer.Entity;
using System.Threading.Tasks;

namespace AsyncClientServer.Server
{
    public class AsyncService
    {
        private IPAddress ipAddress;
        private int port;
        private readonly IHandler handler;
        /// <summary>
        /// create the async service object
        /// </summary>
        /// <param name="port"></param>
        public AsyncService(int port, IHandler handler)
        {
            this.port = port;
            //gets the possible Host
            string hostName = Dns.GetHostName();
            //gets IpAdress's on the host
            IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);
            this.ipAddress = null;
            //loops into IpHostInfo variable
            for (int i = 0; i < ipHostInfo.AddressList.Length; ++i)
            {
                //check the current IPAddress is a addressfamily type internetwork
                if (ipHostInfo.AddressList[i].AddressFamily ==
                  AddressFamily.InterNetwork)
                {
                    this.ipAddress = ipHostInfo.AddressList[i];
                    break;
                }
            }
            this.handler = handler;
            //throw exception if none found
            if (this.ipAddress == null)
                throw new Exception("No IPv4 address for server");
        }
        /// <summary>
        /// begin running the server
        /// </summary>
        public async void Run()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 44818);
            //create the listener object with given ipAdress and port
            TcpListener listener = new TcpListener(ep);
            Console.WriteLine(listener.LocalEndpoint);
            //start the object
            listener.Start();
            Console.Write("Array Min and Avg service is now running");

            Console.WriteLine(" on port " + this.port);
            Console.WriteLine("Hit <enter> to stop service\n");
            //make the server take more then one request before having to restart
            while (true)
            {
                try
                {
                    //waits for the client to connect
                    TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                    //starting the task
                    Thread t = new Thread(new ParameterizedThreadStart(Process));
                    //wait for the task to complete
                    t.Start(tcpClient);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        /// <summary>
        /// making the process and take the rquest from the client and the data send with it
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <returns></returns>
        public async void Process(object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;
            //gets the clients ipAdress
            string clientEndPoint =
              client.Client.RemoteEndPoint.ToString();
            Console.WriteLine("Received connection request from "
              + clientEndPoint);
            try
            {
                while (client.Connected)
                {
                    NetworkStream networkStream = client.GetStream();
                    StreamReader reader = new StreamReader(networkStream);
                    StreamWriter writer = new StreamWriter(networkStream);

                    //gets the request from the client
                    string request = await reader.ReadLineAsync();
                    if (request != null)
                    {
                        Console.WriteLine("Received service request: " + request);
                        //starts reponse to the client
                        string response = handler.Process(request);
                        Console.WriteLine("Computed response is: " + response + "\n");
                        //sends the reponse to the client
                        await writer.WriteLineAsync(response);
                        //flush the data to send
                        await writer.FlushAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}
