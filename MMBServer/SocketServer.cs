using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace MMBServer
{
    public class SocketServer
    {
        private static SocketServer instance;
        private IPAddress IpAddress = null;
        private int Port = 0;

        private TcpListener listner = null;
        private TcpClient tcpClient = null;
        private NetworkStream stream = null;

        private byte[] buffer = null;

        private MMBConfig _config;

        public static SocketServer getInstance()
        {
            if (instance == null) { instance = new SocketServer(); }
            return instance;
        }
        private SocketServer()
        {
        }
        public void listen()
        {
            try
            {
                MMBConfig _config = MMBConfig.getInstance();
                this.IpAddress = IPAddress.Parse(_config.TcpSeverAdress);
                this.Port = int.Parse(_config.TcpPort);
    
                while (true)
                {
                    //TCP Listener Configurieren und Starten
                    this.listner = new TcpListener(this.IpAddress, this.Port);
                    this.listner.Start();

                    LogService.getInstance().create(LogService._INFORMATION, "TcpIpServer", "Warte auf Verbindung");
                    
                    // TCP Lister öffnen für eingehende TCP Connections
                    this.tcpClient = this.listner.AcceptTcpClient();
                    this.stream = tcpClient.GetStream();
                    this.buffer = exampleByteArray();
                    
                    LogService.getInstance().create(LogService._INFORMATION, "TcpIpServer", "Verbindung hergestellt");
                    while (true)
                    {
                        try
                        {
                            if (_config.IsDevelopeMode && this.buffer[_config.MmbtcpPackageOffSetCount] == 0 )
                            {
                                //this.stream.Write(this.exampleByteArray(), 0, _config.MMBtcpPackageSize);
                            }
                            this.stream.ReadTimeout = 15000;
                            this.stream.Read(buffer, 0, _config.MMBtcpPackageSize);
                            string UID = System.Text.ASCIIEncoding.ASCII.GetString(buffer, _config.MmbtcpPackageOffSetUID, _config.MmbtcpPackageLengthUID);
                            _config.PersonalNumber = System.Text.ASCIIEncoding.ASCII.GetString(buffer, 12, 10).Trim();
                            _config.PersonalPassword = System.Text.ASCIIEncoding.ASCII.GetString(buffer, 22, 10).Trim();
                            Console.WriteLine("CMD: " + buffer[0] + " | Count: " + buffer[_config.MmbtcpPackageOffSetCount] +  " | pw: " + _config.PersonalPassword  + " | user: " + _config.PersonalNumber + " | uid: " + UID);
                            //LogService.getInstance().create(LogService._INFORMATION, "CMD: " + buffer[0] + " | Count: " + buffer[_config.MmbtcpPackageOffSetCount] + " | pw: " + _config.PersonalPassword + " | user: " + _config.PersonalNumber + " | uid: " + UID);
                            //buffer[0] = (byte)1;
                            switch (buffer[0])
                            {
                                case 0: // Reset Data
                                    buffer = this.resetData(buffer);
                                    break;
                                case 1:  // Request Manufactoring Data
                                    buffer = this.requestManufactoringData(buffer);
                                    break;
                                case 2:
                                    // Job Finished
                                    buffer = this.jobFinished(buffer);
                                    break;
                                default: // Reset Data
                                    buffer = this.resetData(buffer);
                                    break;
                            }
                            // Counter erhöhen
                            this.buffer[_config.MmbtcpPackageOffSetCount] = (byte)(this.buffer[_config.MmbtcpPackageOffSetCount] + 1);
                            this.stream.Write(buffer, 0, _config.MMBtcpPackageSize);
                            Console.WriteLine("Data received");
                        }
                        
                        catch(ThreadAbortException ex)
                        {
                            LogService.getInstance().create(ex);
                            stream.Close();
                            tcpClient.Close();
                            listner.Stop();
                            throw ex;
                            break;
                        }
                        catch (Exception ex)
                        {
                            LogService.getInstance().create(ex);
                            listner.Stop();
                            listner = null;
                            break;
                        }
                    }
                }
            }
            catch (ThreadAbortException ex)
            {
                LogService.getInstance().create(ex);
            }
            catch (Exception ex)
            {
                LogService.getInstance().create(ex);
            }
        }

        private byte[] exampleByteArray()
        {
            byte[] buffer = new byte[MMBConfig.getInstance().MMBtcpPackageSize];
            buffer[0] = (byte)0;
            buffer[1] = (byte)0;
            for (int i = 2; i< buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte((char)' ');
            }

            return buffer;
        }
        

        private string byteToString(byte[] buffer)
        {
            string result = "";
            string uid = this.receiveUID(buffer);
            string cmd = buffer[0].ToString();
            string counter = buffer[1].ToString();
            result = "counter: " + counter + " | cmd: " + cmd + " | uid " + uid;
            return result;
        }

        private void printData(byte[] data)
        {
            byte cmd = data[0];
            byte counter = data[1];
            string UID = System.Text.ASCIIEncoding.ASCII.GetString(data, 2, 10);
            Console.WriteLine("Counter :" + counter + " | cmd: " + cmd + " | uid: " + UID);
        }

        private byte[] resetData(byte[] buffer)
        {
            buffer[0] = (byte)0;
            for (int i = 2; i < MMBConfig.getInstance().MMBtcpPackageSize; i++)
            {
                buffer[i] = Convert.ToByte((char)' ');
            }
            return buffer;
        }
        public byte[] requestManufactoringData(byte[] buffer)
        {
            LogService.getInstance().create(LogService._INFORMATION, "SocketServer.requestManufactoringData", "");
            SocketClient socket = null;
            bool isOk = false;
            string UID = this.receiveUID(buffer);
            _config = MMBConfig.getInstance();
            try
            {
                // Socket Instance öffnen
                socket = SocketClient.getInstance();
                socket.initCon(_config.XMLServerAdress, _config.XMLSeverPort.ToString());
                
                //Manufactoring Data Request
                ManufactoringDataRequest x = new ManufactoringDataRequest();
                x.UID = int.Parse(UID);
                
                // Manufactoring Request in XML Wandeln
                XmlDocument request = x.toXML();
                byte[] requestdata = Encoding.Default.GetBytes(request.OuterXml);
                    
                //Request an BKS Server senden
                string responseString = socket.sendMessage(requestdata);
                XmlDocument resXml = new XmlDocument();

                // Response XML Generieren und laden
                resXml.LoadXml(responseString);

                // JObListe
                List<ManufactoringJob> list = ManufactoringJob.convertFromManufactoringData(resXml);

                // Wenn die Antwort ein Fehler ist
                if (resXml.SelectSingleNode("Root/Telegram").Attributes["Name"].Value == "Error")
                {
                    isOk = false;
                }

                // Jeden Job im XML durch itterieren
                foreach (ManufactoringJob j in list)
                {
                    j.save();       
                     // Job in Datanbank schreiben
                    socket.sendMessage(Encoding.Default.GetBytes(j.DeleteTelegram().OuterXml)); // Delete Telegramm versenden
                }
                isOk = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            if (isOk == true)
            {
                buffer[0] = (byte)(buffer[0] + 10);
            }
            else
            {
                buffer[0] = (byte)(buffer[0] + 20);
                Console.WriteLine("cmd :" + buffer[0]);
            }
            Console.WriteLine("buffer: " + buffer);
            return buffer;
        }
        public byte[] jobFinished(byte[] buffer)
        {
            MMBConfig _config = MMBConfig.getInstance();
            LogService.getInstance().create(LogService._INFORMATION, "SocketServer.JobFinished", "");
            SocketClient socket;

            bool isOk = true;
            try
            {
                List<ManufactoringJob> list = ManufactoringJob.getAllJobs();
                int UID = int.Parse(this.receiveUID(buffer));
                ManufactoringJob j = list.FirstOrDefault(x => x.UID == UID);
                if (j == null)
                {
                    throw new Exception("not found");
                    isOk = false;
                }
                string CommunicationUSR = _config.KommunikationUser;
                string CommunicationPWD = _config.KommunikationPassword;
                string UserID = _config.UserID;
                XmlDocument receipt = j.Receipt(CommunicationUSR, CommunicationPWD, UserID);
                byte[] requestdata = Encoding.Default.GetBytes(receipt.OuterXml);
                socket = SocketClient.getInstance();
                socket.initCon(_config.XMLServerAdress, _config.XMLSeverPort.ToString());
                string responseString = socket.sendMessage(requestdata);
                j.delete();
            }
            catch(InvalidCastException ex)
            {

            }
            catch (Exception ex)
            {
                LogService.getInstance().create(ex);
            }
            if (isOk == true)
            {
                buffer[0] = (byte)(buffer[0] + 10);
            }
            else
            {
                buffer[0] = (byte)(buffer[0] + 20);
            }
            return buffer;
        }

        private string receiveUID(byte[] buffer)
        {
            string UID = System.Text.ASCIIEncoding.ASCII.GetString(buffer, 2, 10);
            return UID;
        }

        public static XmlDocument DeleteTelegram(ManufactoringJob job)
        {
            LogService.getInstance().create(LogService._INFORMATION, "SocketServer.DeleteTelegram", "");
            XmlDocument xml = new XmlDocument();
            XmlNode rootNode = xml.CreateElement("Root");
            XmlNode TelegrammNode = xml.CreateElement("Telegram");
            XmlAttribute TelegrammName = xml.CreateAttribute("Name");
            TelegrammName.Value = "Delete";
            TelegrammNode.Attributes.Append(TelegrammName);
            xml.AppendChild(rootNode);
            rootNode.AppendChild(TelegrammNode);
            XmlNode VersionNode = xml.CreateElement("Version");
            VersionNode.InnerText = "5";
            TelegrammNode.AppendChild(VersionNode);
            XmlNode UIDNode = xml.CreateElement("UID");
            UIDNode.InnerText = job.UID + ""; ;
            TelegrammNode.AppendChild(UIDNode);
            return xml;
        }

        public void dispose()
        {
            if (this.stream != null)
                this.stream.Close();
            
            if (this.tcpClient != null)
                this.tcpClient.Close();
    
            if (this.listner != null)
            this.listner.Stop();
        }

    }
}
