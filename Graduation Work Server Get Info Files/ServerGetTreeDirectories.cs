using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Graduation_Work_Server_Get_Info_Files.HelperClasses;

namespace Graduation_Work_Server_Get_Info_Files.Classes
{
    public class ServerGetTreeDirectories
    {
        IPHostEntry ipHost;
        IPAddress ipAddr;
        IPEndPoint ipEndPoint;

        Socket sListener;
        public ServerGetTreeDirectories(string ipHost, int ipEndPointPort)
        {
            // Устанавливаем для сокета локальную конечную точку
            this.ipHost = Dns.GetHostEntry(ipHost);
            this.ipAddr = this.ipHost.AddressList[0];
            this.ipEndPoint = new IPEndPoint(ipAddr, ipEndPointPort);

            // Создаем сокет Tcp/Ip
            sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
        public void StartServer()
        {
            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                // Начинаем слушать соединения
                while (true)
                {
                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                
                    // Программа приостанавливается, ожидая входящее соединение
                    Socket handler = sListener.Accept();

                    // Отправляем ответ клиенту\
                    handler.Send(this.GetDirectories());

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
        private byte[] GetDirectories()
        {
            List<string> listInfoDirectories = CustomSearcher.GetDirectories("E:\\MyOneDrive");

            for (int i = 0; i < listInfoDirectories.Count; i++)
            {
                listInfoDirectories[i] = listInfoDirectories[i].Replace("E:\\MyOneDrive", "Івашенюк Юрій");
                listInfoDirectories[i] = listInfoDirectories[i].Remove("Івашенюк Юрій".Length, "Івашенюк Юрій".Length + 1);
            }

            string reply = JsonConvert.SerializeObject(listInfoDirectories);
            byte[] msg = Encoding.UTF8.GetBytes(reply);

            return msg;
        }
    }
}
