using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using AsyncClientServer.Entity;

namespace AsyncClientServer.Server
{
    class ServiceProgram
    {
        static void Main(string[] args)
        {
            try
            {
                int port = 8001;
                AsyncService service = new AsyncService(port, new CalcHandler());
                service.Run();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
    /// <summary>
    /// async service class to send- and recieve data
    /// </summary>
}

