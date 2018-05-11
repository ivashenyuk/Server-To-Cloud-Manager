using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draduation_Work_Server_Upload_Files
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Draduation Work Server Upload Files To Client  |  port=2004";
            ServerUpoadFilesToClient serverLoadFiles = new ServerUpoadFilesToClient("localhost", 2004);
            serverLoadFiles.StartServer();
        }
    }
}
