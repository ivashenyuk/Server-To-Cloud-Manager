using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.IO;

namespace Draduation_Work_Server_Create_Folder
{
    public class ServerCreateDirectory
    {
        IPHostEntry ipHost;
        IPAddress ipAddr;
        IPEndPoint ipEndPoint;

        Socket sListener;
        public ServerCreateDirectory(string ipHost, int ipEndPointPort)
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
                    string data = null;

                    // Мы дождались клиента, пытающегося с нами соединиться
                    if (handler.Poll(200, SelectMode.SelectRead))
                    {
                        byte[] bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);

                        data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        handler.Send(this.CreateDirectory(data));
                    }
                    else
                    {
                        // Отправляем ответ клиенту
                        handler.Send(this.CreateDirectory());
                    }

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
        private byte[] CreateDirectory(string path = "E:\\MyOneDrive\\Івашенюк Юрій")
        {
            try
            {
                path = "E:\\MyOneDrive\\" + path;
                string[] countDirectories = Directory.GetDirectories(path);
                string fullPath = path + "\\New Folder (" + (countDirectories.Length + 1) + ")";

                for (int i = 0, k = 1; i < countDirectories.Length + 1; i++, k++)
                {
                    fullPath = path + "\\New Folder (" + k + ")";
    
                    if (!Directory.Exists(fullPath))
                    {
                        DirectoryInfo info = Directory.CreateDirectory(fullPath);
                        if (info.Exists)
                        {
                            Console.WriteLine(fullPath + " | Folder was created!");
                            return BitConverter.GetBytes(true);
                        }
                        else
                        {
                            return BitConverter.GetBytes(false);
                        }
                    }
                    else
                    {
                        i--;
                    }
                }

                return BitConverter.GetBytes(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BitConverter.GetBytes(false);
            }
        }
    }
}
