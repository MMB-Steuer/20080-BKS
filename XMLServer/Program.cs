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

        public void listen()
        {
            this._adress = IPAddress.Parse("0.0.0.0");
            this._port = 35000;

            while (true)
            {
                try
                {
                    this._listner = new TcpListener(this._adress, this._port);
                    this._listner.Start();
                    Console.WriteLine("Listen started");
                    this._client = this._listner.AcceptTcpClient();
                    this._stream = this._client.GetStream();
                    while (true)
                    {
                        _stream.ReadTimeout = 1000000;
                        byte[] _lengthBuffer = new byte[4];
                        _stream.Read(_lengthBuffer, 0, 4);
                        int datalength = BitConverter.ToInt32(_lengthBuffer, 0);
                        _buffer = new byte[datalength];
                        int totalReadBytes = 0;
                        while (true)
                        {
                            int readBytes = _stream.Read(_buffer, totalReadBytes, _buffer.Length - totalReadBytes);
                            if (readBytes == 0)
                            {
                                throw new Exception("No bytes read from network stream");
                            }
                            totalReadBytes += readBytes;
                            if (totalReadBytes >= _buffer.Length)
                            {
                                break;
                            }
                        }
                        string txt = System.Text.UTF8Encoding.UTF8.GetString(_buffer);
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(txt);
                        Console.Write(xmlDoc.OuterXml.ToString());
                        _stream.Write(_buffer, 0, _buffer.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    try
                    {
                        _stream.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        _client.Close();
                    }
                    catch (Exception ex) { }
                    _stream = null;
                    _client = null;
                    _listner = null;
                }
            }
        }
    }
}

