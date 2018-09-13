using AsyncClientServer.Entity;
using AsyncClientServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TCPService.cs
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            int port = 8001;
            AsyncService service = new AsyncService(port, new CalcHandler());
            service.Run();
        }

        protected override void OnStop()
        {
        }
    }
}
