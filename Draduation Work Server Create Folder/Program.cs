using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draduation_Work_Server_Create_Folder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Draduation Work Server Create Folder  |  port=2001";
            ServerCreateDirectory serverCreate = new ServerCreateDirectory("localhost", 2001);
            serverCreate.StartServer();
        }
    }
}
