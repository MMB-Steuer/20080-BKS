using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TestSBSasWPF
{
    public class SocketClient
    {
        public byte _command = (byte) 0;
        public byte _count = (byte) 0;
        public string _serverAddr = "127.0.0.1";
        public string _port = "8000";
        public string _uid = "34531185";
        public string _personalNummer = "0700";
        public string _personalPassword = "marcus";
        public bool _isRunning = false;
        public string _STATUS = "OFFLINE";

        public List<TCPPackeItem> respondList = new List<TCPPackeItem> ();

        private TcpClient _Client;
        private NetworkStream _Stream;
        private byte[] _Buffer = new byte[32];


        public SocketClient()
        {
        }
        public void start()
        {
                try
                {
                    _Client = new TcpClient(_serverAddr, Int32.Parse(_port));
                    this._isRunning = true;
                    _Stream = _Client.GetStream();
                    _STATUS = "try to connect";
                    while (true)
                    {
                        getBuffer();
                        _Stream.Write(_Buffer, 0, _Buffer.Length);
                    _STATUS = "DATA SEND";
                        _Stream.Read(_Buffer, 0, _Buffer.Length);
                    _STATUS = "DATA RECEIVED";
                        respondList.Add(TCPPackeItem.fromByteArray(_Buffer));
                    _STATUS = "WAITING for next round";
                        Thread.Sleep(new Random().Next(500, 2500));
                }

            }
                catch (Exception ex)
                {
                    _isRunning = false;
                    _STATUS = "EXCEPTION";
                    MessageBox.Show(ex.Message);
                }
           
        }
        public void dispose()
        {
            if (_Client != null)
            {
                _Client.Dispose();
            }
        }
        private void getBuffer()
        {
            try
            {
                byte[] buffer = new byte[32];
                buffer[0] = _command;
                buffer[1] = _count;
                byte [] uid_buffer = new byte[10];
                byte [] pid_buffer = new byte[10];
                byte [] ppwd_buffer = new byte[10];
                uid_buffer = Encoding.ASCII.GetBytes(_uid);
                pid_buffer = Encoding.ASCII.GetBytes(_personalNummer);
                ppwd_buffer = Encoding.ASCII.GetBytes(_personalPassword);

                uid_buffer.CopyTo(_Buffer, 2);
                pid_buffer.CopyTo(_Buffer, 12);
                ppwd_buffer.CopyTo(_Buffer, 22);

                _Buffer = buffer;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
    public class TCPPackeItem
    {
        private DateTime stamp;
        private int command;
        private int count;
        private string uid;
        private string personalnummer;
        private string personalpassword;

        public DateTime Stamp { get => stamp; set => stamp = value; }
        public int Command { get => command; set => command = value; }
        public int Count { get => count; set => count = value; }
        public string Uid { get => uid; set => uid = value; }
        public string Personalnummer { get => personalnummer; set => personalnummer = value; }
        public string Personalpassword { get => personalpassword; set => personalpassword = value; }

        public static TCPPackeItem fromByteArray(byte[] buffer)
        {
            TCPPackeItem p = new TCPPackeItem();
            p.Stamp = DateTime.Now;
            p.Command = (int)buffer[0];
            p.Count = (int)buffer[1];
            p.Uid = System.Text.ASCIIEncoding.ASCII.GetString(buffer, 2, 10);
            p.Personalnummer = System.Text.ASCIIEncoding.ASCII.GetString(buffer, 12, 10);
            p.Personalpassword = System.Text.ASCIIEncoding.ASCII.GetString(buffer, 22, 10);
            return p;
        }
    }
}
