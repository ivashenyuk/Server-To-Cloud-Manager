using Graduation_Work_Server_Get_Info_Files.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Work_Server_Get_Info_Files
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Graduation Work Server Get Info Files  |  port=1999";
            ServerGetTreeDirectories serverGetTreeDirectories = new ServerGetTreeDirectories("localhost", 1999);
            serverGetTreeDirectories.StartServer();
        }
    }
}
