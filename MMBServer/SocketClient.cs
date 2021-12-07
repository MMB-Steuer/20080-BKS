using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MMBServer
{
    public class SocketClient
    {
        private static SocketClient instance = null;

        private string serverpath = "";
        private string serverport = "";
        private TcpClient con = null;
        private bool isinitialized = false;

        public static SocketClient getInstance()
        {
            if (instance == null) { instance = new SocketClient(); }
            return instance;
        }

        public void initCon(string ServerAddr, string ServerPort)
        {
            this.serverpath = ServerAddr;
            this.serverport = ServerPort;
            this.con = new TcpClient(ServerAddr, Int32.Parse(ServerPort));
            this.con.Close();
            this.con = null;
            this.isinitialized = true;
        }

        public string sendMessage(byte[] data)
        {
            this.con = new TcpClient(this.serverpath, Int32.Parse(this.serverport));
            if (this.isinitialized == false)
            {
                throw new Exception("Not Initialized");
            }
            Console.WriteLine("data to Server: " + data.Length);
            NetworkStream stream = this.con.GetStream();
            var sizeBuffer = BitConverter.GetBytes((int)data.Length);
            var buffer = new byte[sizeBuffer.Length + data.Length];
            sizeBuffer.CopyTo(buffer, 0);
            data.CopyTo(buffer, sizeBuffer.Length);
            stream.Write(buffer, 0, buffer.Length);
            Console.WriteLine("buffer to Server: " + buffer.Length);
            Console.WriteLine("data to Server: " + data.Length);
            var lengthBuffer = new byte[4];
            stream.Read(lengthBuffer, 0, 4);
            var length = BitConverter.ToInt32(lengthBuffer, 0);
            buffer = new byte[length];
            int totalReadyBytes = 0;
            while (true)
            {
                var readBytes = stream.Read(buffer, totalReadyBytes, buffer.Length - totalReadyBytes);
                if (readBytes == 0)
                {
                    throw new Exception("No bytes read from network stream");
                }
                totalReadyBytes += readBytes;
                if (totalReadyBytes >= buffer.Length)
                {
                    break;
                }
            }

            string text = System.Text.UTF8Encoding.UTF8.GetString(buffer);
            stream.Close();
            this.con.Close();
            this.con = null;
            Console.WriteLine(text);
            
            LogService.getInstance().create(LogService._INFORMATION, text);
            return text;
        }



        public void dispose()
        {
            this.con.Close();
            SocketClient.instance = null;
        }

    }
}
