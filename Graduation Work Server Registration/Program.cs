using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Work_Server_Registration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server Login Users  |  port=2006";
            ServerRegistrationUser loginUser = new ServerRegistrationUser("localhost", 2006);
            loginUser.StartServer();
        }
    }
}
