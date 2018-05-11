using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Work_Server_Login
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server Login Users  |  port=2005";
            ServerLoginUser loginUser = new ServerLoginUser("localhost", 2005);
            loginUser.StartServer();
        }
    }
}
