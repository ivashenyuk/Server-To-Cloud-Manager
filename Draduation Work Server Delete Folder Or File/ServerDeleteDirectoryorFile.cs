using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.IO;

namespace Draduation_Work_Server_Delete_Folder_Or_File
{
    public class ServerDeleteDirectoryorFile
    {
        IPHostEntry ipHost;
        IPAddress ipAddr;
        IPEndPoint ipEndPoint;

        Socket sListener;
        public ServerDeleteDirectoryorFile(string ipHost, int ipEndPointPort)
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
                        handler.Send(this.DeleteDirectoryOrFile(data));
                    }
                    else
                    {
                        // Отправляем ответ клиенту
                        handler.Send(this.DeleteDirectoryOrFile());
                    }
                    // Отправляем ответ клиенту\
                    //handler.Send(this.CreateDirectory());

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
        private byte[] DeleteDirectoryOrFile(string path = "E:\\MyOneDrive\\Івашенюк Юрій")
        {
            try
            {
                path = "E:\\MyOneDrive\\" + path;
                string fullPath = path;
                bool isDirectory = false;
                bool isFile = false;

                isDirectory = Directory.Exists(fullPath);
                isFile = File.Exists(fullPath);

                if (isFile)
                {
                    File.Delete(fullPath);
                    Console.WriteLine(fullPath + " | File was deleted!");
                    return BitConverter.GetBytes(true);
                }
                else if (isDirectory)
                {
                    DirectoryInfo info = new DirectoryInfo(fullPath);
                    //Directory.Delete(fullPath);
                    RecursiveDelete(info);
                    Console.WriteLine(fullPath + " | Folder was created!");
                    return BitConverter.GetBytes(true);
                }
                else
                {
                    return BitConverter.GetBytes(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BitConverter.GetBytes(false);
            }
        }
        public static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.EnumerateDirectories())
            {
                RecursiveDelete(dir);
            }
            baseDir.Delete(true);
        }
    }
}
