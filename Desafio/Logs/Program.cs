using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs
{
    class Program
    {
        static void Main(string[] args)
        {
            GuardaLogs gl = new GuardaLogs(true,true,true,true,true,true);
            //GuardaLogs.LogMessage("Advertencia", true, true, false);
            //GuardaLogs.LogMessage("Error", true, true, true);
            GuardaLogs.LogMessage("mensaje", true, false, false);
        }
    }
}
