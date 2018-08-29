using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncClientServer.Entity
{
    public interface IHandler
    {
        string Process(string jsonData);
    }
}
