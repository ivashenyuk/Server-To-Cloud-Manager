using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Work_Server_Send_Email
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server Login Users  |  port=2007";
            ServerSendEmail loginUser = new ServerSendEmail("localhost", 2007);
            loginUser.StartServer();
        }
    }
}
