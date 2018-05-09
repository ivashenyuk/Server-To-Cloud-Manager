using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using Graduation_Work_Server_Get_Info_Files.HelperClasses;

namespace Graduation_Work_Server_Get_Info_Files.Classes
{
    public class ServerGetListDirectoriesAndFiles
    {
        class Inf
        {
            private string _nameFile;
            private DateTime _lastWriteTime;
            private long _sizeFile;
            private string _typeFile;
            private byte[] _icoFile;
            private string _fullPath;
            public Inf(string nameFile, DateTime lastWriteTime, long sizeFile, string typeFile, byte[] icoFile, string fullPath)
            {
                this._nameFile = nameFile;
                this._lastWriteTime = lastWriteTime;
                this._sizeFile = sizeFile;
                this._typeFile = typeFile;
                this._icoFile = icoFile;
                this._fullPath = fullPath;
            }
            public string NameFile
            {
                get { return this._nameFile; }
                set { this._nameFile = value; }
            }
            public DateTime LastWriteTime
            {
                get { return this._lastWriteTime; }
                set { this._lastWriteTime = value; }
            }
            public long SizeFile
            {
                get { return this._sizeFile; }
                set { this._sizeFile = value; }
            }
            public string TypeFile
            {
                get { return this._typeFile; }
                set { this._typeFile = value; }
            }
            public byte[] IcoFile
            {
                get { return this._icoFile; }
                set { this._icoFile = value; }
            }
            public string FullPath
            {
                get { return this._fullPath; }
                set { this._fullPath = value; }
            }
        }

        IPHostEntry ipHost;
        IPAddress ipAddr;
        IPEndPoint ipEndPoint;

        Socket sListener;
        public ServerGetListDirectoriesAndFiles(string ipHost, int ipEndPointPort)
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
                        handler.Send(this.GetAllFiles(data));
                    }
                    else
                    {
                        // Отправляем ответ клиенту
                        handler.Send(this.GetAllFiles());
                    }
                    // this.GetAllFiles();
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
        private byte[] GetAllFiles(string getFilesInDirectory = "E:\\MyOneDrive\\Івашенюк Юрій")
        {
            List<Inf> listFileInfo = new List<Inf>();
            try
            {
                string[] listInfoDirectories = Directory.GetDirectories(getFilesInDirectory);
                //List<string> listInfoDirectories = CustomSearcher.GetDirectories(getFilesInDirectory);
                

                string[] allFilesInDirectory = Directory.GetFiles(getFilesInDirectory);

                allFilesInDirectory = listInfoDirectories.Concat<string>(allFilesInDirectory);

                foreach (string item in allFilesInDirectory)
                {
                    if (Directory.Exists(item))
                    {
                        DirectoryInfo fileInfo = new DirectoryInfo(item);
                        Icon icon = null;

                        byte[] iconBytes;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            if (icon != null)
                            {
                                icon.Save(ms);
                                iconBytes = ms.ToArray();
                            }
                            else
                            {
                                iconBytes = new byte[3];
                            }
                        }

                        listFileInfo.Add(new Inf(fileInfo.Name, fileInfo.LastWriteTime,
                            Directory.GetFiles(item, "*", SearchOption.AllDirectories).Length,
                            Path.GetExtension(item), iconBytes, fileInfo.FullName));
                    }
                    else if (File.Exists(item))
                    {
                        FileInfo fileInfo = new FileInfo(item);
                        Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(fileInfo.FullName);
                        byte[] iconBytes;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            icon.Save(ms);
                            iconBytes = ms.ToArray();
                        }

                        listFileInfo.Add(new Inf(fileInfo.Name, fileInfo.LastWriteTime, fileInfo.Length,
                            Path.GetExtension(item), iconBytes, fileInfo.FullName));
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }


            string reply = JsonConvert.SerializeObject(listFileInfo);
            byte[] msg = Encoding.UTF8.GetBytes(reply);
            return msg;
        }

    }
    static class Extantion
    {
        public static T[] Concat<T>(this T[] x, T[] y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            int oldLen = x.Length;
            Array.Resize<T>(ref x, x.Length + y.Length);
            Array.Copy(y, 0, x, oldLen, y.Length);
            return x;
        }
    }
}
