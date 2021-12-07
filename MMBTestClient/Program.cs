using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MMBTestClient
{
    internal class Program
    {
        private static Thread thread1;
        private static Thread thread2;
        private static TcpTestClient tcpClient = new TcpTestClient();
        private static bool runtime = true;
        static void Main(string[] args)
        {
            Console.WriteLine("start");
            //TcpTestClient.getInstance();
            Program.startTasks();
            while (runtime)
            {
                Console.Clear();
                Console.WriteLine("1 = Neustart");
                Console.WriteLine("2 = beenden");
                Console.WriteLine("3 = start");
                Console.WriteLine("exit = client beenden");

                Console.WriteLine("4 = send Cmd");
                Console.WriteLine("5 = change UID / dsc");

                string cmd = Console.ReadLine();
                switch(cmd)
                {
                    case "1": 
                        Program.stopTasks();
                        Program.startTasks();
                        break;
                    case "2":
                        Program.stopTasks();
                        break ;
                    case "3":
                        Program.startTasks();
                        break;
                    case "4":
                        Console.Write("CMD ?");
                        var cmdA = Console.ReadLine();
                        switch (cmdA)
                        {
                            case "1":   tcpClient.cmd = (byte)1; break;
                            case "2":   tcpClient.cmd = (byte)1; break;
                            default: tcpClient.cmd = (byte)0; break;
                        }
                        
                        break;
                    case "exit":
                        Program.stopTasks();
                        runtime = false;
                        break;
                    default: break; 
                }
            }
            
        }
        private static async void startTasks() {
            Program.thread1 = new Thread(tcpClient.listen);
            thread1.Start();
        }
        private static async void stopTasks()
        {
            tcpClient.dispose();
            thread1.Abort();
            thread1 = null;
        }
    }

    public class TcpTestClient
    {

        private string _host = "127.0.0.1";
        private string _port = "8000";
        private TcpClient con = null;
        private NetworkStream _stream = null;
        private byte[] data = new byte[32];

        public byte cmd = 0;
        public byte lastCmd = 0;

        public void listen()
        {

            while (true)
            {
                try
                {
                    this.con = new TcpClient(this._host, Int32.Parse(this._port));
                    this._stream = con.GetStream();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    continue;
                }
                while (true)
                {
                    try
                    {
                        if (lastCmd != 0 && lastCmd != cmd)
                        {
                            cmd = 0;
                        }
                        Thread.Sleep(new Random().Next(500, 2500));
                        data[1] = (byte)(data[1] + 1);
                        data[0] = cmd;
                        this.printData(data);
                        _stream.Write(data, 0, data.Length);
                        lastCmd = cmd;
                        Console.WriteLine("Data send");
                        _stream.Read(data, 0, data.Length);
                        Console.WriteLine("Data received");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        break;
                    }
                }
            }
        }
        private void printData(byte[] data)
        {
            byte cmd = data[0];
            byte counter = data[1];
            string UID = System.Text.ASCIIEncoding.ASCII.GetString(data, 2, 10);
            Console.WriteLine("Counter :" + counter + " | cmd: " + cmd + " | uid: " + UID);
        }

        public void dispose() { 
            try
            {
                try { _stream.Dispose(); }catch (Exception ex) { Console.WriteLine(ex); }
                try { _stream.Close(); }catch (Exception ex) { Console.WriteLine(ex); }
                try { this._stream = null; } catch (Exception ex) { Console.WriteLine(ex); }
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            try
            {
                try { con.Dispose(); } catch (Exception ex) { Console.WriteLine(ex); }
                try { con.Close(); } catch (Exception ex) { Console.WriteLine(ex); }
                try { this.con = null; } catch (Exception ex) { Console.WriteLine(ex); }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }
    }

}
