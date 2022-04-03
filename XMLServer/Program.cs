using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace XMLServer
{
    internal class Program
    {
        private static Thread _thread1;
        private static XMLServer _xmlserver = new XMLServer();
        static void Main(string[] args)
        {
            Program.startThread();
        }
        private static void startThread()
        {
            Program._thread1 = new Thread(Program._xmlserver.listen);
            Program._thread1.Start();
        }
    }

    internal class XMLServer
    {
        private IPAddress _adress;
        private int _port;
        private TcpListener _listner;
        private TcpClient _client;
        private NetworkStream _stream;
        private byte[] _buffer;
        private XmlDocument defaultXML = new XmlDocument();

        public void listen()
        {
            this._adress = IPAddress.Parse("0.0.0.0");
            this._port = 35000;
            new FileLogger(FileLogger._INFORMATION, "XML-Serer started");
            Console.WriteLine(FileLogger._INFORMATION, "Started");
            this._listner = new TcpListener(this._adress, this._port);

            while (true)
            {
                try
                {
                    this._listner.Start();
                    new FileLogger(FileLogger._INFORMATION, "Server Listen started");
                    #region Load XML

                    defaultXML.Load(@"C:\\Users\\dsteuer\\Documents\\GitHub\\20080-BKS\\XMLS BKS\\ManufactirubgData.xml");
                    #endregion
                    this._client = this._listner.AcceptTcpClient();
                    this._stream = this._client.GetStream();

                    while (true)
                        {
                        Console.WriteLine("connection established");

                        _stream.ReadTimeout = 15000;
                            byte[] _lengthBuffer = new byte[4];
                            _stream.Read(_lengthBuffer, 0, 4);
                            int datalength = BitConverter.ToInt32(_lengthBuffer, 0);
                            _buffer = new byte[datalength];
                            int totalReadBytes = 0;
                            
                            while (true)
                            {
                                int readBytes = _stream.Read(_buffer, totalReadBytes, _buffer.Length - totalReadBytes);
                            break;
                                /*if (readBytes == 0)
                                {
                                  throw new Exception("No bytes read from network stream");
                                }
                                totalReadBytes += readBytes;
                                if (totalReadBytes >= _buffer.Length)
                                {
                                  break;
                                } */
                            } 

                            byte[] data = Encoding.Default.GetBytes(defaultXML.OuterXml);
                            var sizeBuffer = BitConverter.GetBytes((int) data.Length);
                            var buffer = new byte[sizeBuffer.Length + data.Length];
                            sizeBuffer.CopyTo(buffer, 0);
                            data.CopyTo(buffer, 4);
                            _stream.Write(buffer, 0,buffer.Length);
                            break;
                        }



                }
                catch (Exception ex)
                {
                    new FileLogger(FileLogger._EXCEPTION, ex.ToString());
                }
                finally
                {
                   // _stream.Close();
                   // _listner.Stop();
                   // _client.Close();
                   // _stream = null;
                   // _client = null;
                   // _listner = null;
                }
            }
        }
    }
}

