using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Work_Server_Restoration_Password
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server Login Users  |  port=2008";
            ServerRestorationPassword loginUser = new ServerRestorationPassword("localhost", 2008);
            loginUser.StartServer();
        }
    }
}
