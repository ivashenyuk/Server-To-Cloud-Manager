using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draduation_Work_Server_Delete_Folder_Or_File
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Draduation Work Server Delete Folder Or File  |  port=2002";
            ServerDeleteDirectoryorFile serverDelete = new ServerDeleteDirectoryorFile("localhost", 2002);
            serverDelete.StartServer();
        }
    }
}
