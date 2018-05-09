using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draduation_Work_Server_Load_Files_From_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Draduation Work Server Load Files From Client  |  port=2003";
            ServerLoadFilesFromClient serverLoadFiles = new ServerLoadFilesFromClient("localhost", 2003);
            serverLoadFiles.StartServer();
        }
    }
}
