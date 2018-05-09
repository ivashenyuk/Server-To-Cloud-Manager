using Graduation_Work_Server_Get_Info_Files.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Work_Server_Get_ListInfo_View
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Graduation Work Server Get ListInfo View  |  port=2000";
            ServerGetListDirectoriesAndFiles serverGetListDirectoriesAndFiles = new ServerGetListDirectoriesAndFiles("localhost", 2000);
            serverGetListDirectoriesAndFiles.StartServer();
        }
    }
}
