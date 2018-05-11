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

namespace Graduation_Work_Server_Login
{

    public class ServerLoginUser
    {
        public const int sizeBuffer = 20971520;

        IPHostEntry ipHost;
        IPAddress ipAddr;
        IPEndPoint ipEndPoint;

        Socket sListener;

        public ServerLoginUser(string ipHost, int ipEndPointPort)
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
                    {
                        byte[] receiveBytes = new byte[5000];

                        int bytesRec = handler.Receive(receiveBytes);

                        UserInformation infoUser = receiveBytes.UnpackFromXml<UserInformation>();

                        UserInformation infoUserResponse = this.GetInformationAboutuser(infoUser);

                        handler.Send(infoUserResponse.PackToXml());
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

        private UserInformation GetInformationAboutuser(UserInformation user)
        {
            user.Name = "Івашенюк Юрій";
            return user;
            try
            {
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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
