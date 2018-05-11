using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Draduation_Work_Server_Upload_Files
{

    public class ServerUpoadFilesToClient
    {
        public const int sizeBuffer = 20971520;

        IPHostEntry ipHost;
        IPAddress ipAddr;
        IPEndPoint ipEndPoint;

        Socket sListener;

        public ServerUpoadFilesToClient(string ipHost, int ipEndPointPort)
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
            for (int i = 0; i < 1; i++)
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
                        {
                            byte[] receiveBytes = new byte[5000];
                            Console.WriteLine("-----------*******Получиние информации о файле*******-----------");

                            int bytesRec = handler.Receive(receiveBytes);

                            InfoFileNew infoPath = receiveBytes.UnpackFromXml<InfoFileNew>();

                            InfoFileNew infoFile = this.GetInformationAboutFile(infoPath._path);
                            handler.Send(infoFile.PackToXml());
                            Console.WriteLine("----Информация о файле получена и отправлена!");

                            //InfoFileNew infoFile = receiveBytes.UnpackFromXml<InfoFileNew>();
                            Console.WriteLine("-----------*******Ожидание файла*******-----------");
                            byte[] file = GetFile(infoPath._path);



                            //byte[] receiveBytes1 = new byte[infoFile._sizeFile];

                            //int bytesRec1 = handler.Receive(receiveBytes1);
                            Console.WriteLine("----Файл получен... Отправление...");
                            handler.Send(file);
                            // CreateFile(receiveBytes1, infoFile._path, infoFile._fileName);
                            Console.WriteLine("----Файл отправлен...");
                        }

                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    i--;
                }
                finally
                {
                    Console.ReadLine();

                }
            }
        }

        private InfoFileNew GetInformationAboutFile(string pathToFile)
        {
            try
            {
                pathToFile = string.Format(@"E:\MyOneDrive\" + pathToFile);

                bool isFile = false;
                isFile = File.Exists(pathToFile);
                if (isFile)
                {
                    {
                        InfoFileNew fileNew = new InfoFileNew();
                        fileNew._fileName = Path.GetFileName(pathToFile);
                        fileNew._path = pathToFile;
                        fileNew._sizeFile = File.ReadAllBytes(pathToFile).Length;

                        return fileNew;
                    }
                }
                else
                {
                    throw new NullReferenceException("Файл не знайдено!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private byte[] GetFile(string path)
        {
            try
            {
                path = @"E:\MyOneDrive\" + path;
                string fullPath = path;
                Console.WriteLine(fullPath);

                bool isFile = false;
                isFile = File.Exists(fullPath);

                if (isFile)
                {
                    return File.ReadAllBytes(path);
                }
                else
                {
                    throw new NullReferenceException("Файл не знайдено!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new NullReferenceException("Файл не знайдено!");
            }
        }
    }

    [Serializable]
    public class InfoFileNew
    {
        public string _fileName = "";
        public string _path = "";
        public long _sizeFile = 0;
    }
    public static class HelperExtention
    {
        public static byte[] PackToXml<T>(this T @object)
        {
            XmlSerializer fileSerializer = new XmlSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();

            // Сериализуем объект
            fileSerializer.Serialize(stream, @object);

            // Считываем поток в байты
            stream.Position = 0;
            Byte[] pack = new Byte[stream.Length];
            stream.Read(pack, 0, Convert.ToInt32(stream.Length));

            return pack;
        }

        public static T UnpackFromXml<T>(this byte[] pack)
        {
            XmlSerializer fileSerializer = new XmlSerializer(typeof(T));
            MemoryStream stream1 = new MemoryStream();

            // Считываем информацию о файле
            stream1.Write(pack, 0, pack.Length);
            stream1.Position = 0;

            // Вызываем метод Deserialize
            T infoFile = (T)fileSerializer.Deserialize(stream1);


            return infoFile;
        }
    }
}
