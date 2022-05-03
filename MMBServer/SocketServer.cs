using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

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
        
        private string originUID = null;
        private byte lastCMD = (byte)0;
        private string lastUID = "";

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
                    new FileLogger(FileLogger._INFORMATION, "TCPIPServer Warte auf Verbindung");
                    
                    // TCP Lister öffnen für eingehende TCP Connections
                    this.tcpClient = this.listner.AcceptTcpClient();
                    this.stream = tcpClient.GetStream();
                    this.buffer = exampleByteArray();
                    
                    LogService.getInstance().create(LogService._INFORMATION, "TcpIpServer", "Verbindung hergestellt");
                    while (true)
                    {
                        try
                        {
                            this.stream.ReadTimeout = 15000;
                            this.stream.Read(buffer, 0, _config.MMBtcpPackageSize);
                            string UID = System.Text.ASCIIEncoding.ASCII.GetString(buffer, _config.MmbtcpPackageOffSetUID, _config.MmbtcpPackageLengthUID);
                            //new FileLogger(FileLogger._DEBUG, "received CMD: " + buffer[0] + " | Count: " + buffer[_config.MmbtcpPackageOffSetCount] + " | pw: " + _config.PersonalPassword + " | user: " + _config.PersonalNumber + " | uid: " + UID);

                            _config.PersonalNumber = System.Text.ASCIIEncoding.ASCII.GetString(buffer, 12, 10).Trim();
                            _config.PersonalPassword = System.Text.ASCIIEncoding.ASCII.GetString(buffer, 22, 10).Trim();
                            //LogService.getInstance().create(LogService._INFORMATION, "CMD: " + buffer[0] + " | Count: " + buffer[_config.MmbtcpPackageOffSetCount] + " | pw: " + _config.PersonalPassword + " | user: " + _config.PersonalNumber + " | uid: " + UID);
                            //buffer[0] = (byte)1;
                            if (lastCMD != buffer[0] || lastUID != UID)
                            {
                                lastCMD = buffer[0];
                                lastUID = UID;
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
                            }

                            //new FileLogger(FileLogger._DEBUG, "lastcmd " + lastCMD.ToString());
                            // Counter erhöhen
                            this.buffer[_config.MmbtcpPackageOffSetCount] = (byte)(this.buffer[_config.MmbtcpPackageOffSetCount] + 1);
                            this.stream.Write(buffer, 0, _config.MMBtcpPackageSize);
                            //new FileLogger(FileLogger._DEBUG, "send CMD: " + buffer[0] + " | Count: " + buffer[_config.MmbtcpPackageOffSetCount] + " | pw: " + _config.PersonalPassword + " | user: " + _config.PersonalNumber + " | uid: " + UID);

                            buffer = this.resetData(buffer);
                        }
                        
                        catch(ThreadAbortException ex)
                        {
                            new FileLogger(FileLogger._EXCEPTION, ex.ToString());
                            LogService.getInstance().create(ex);
                            stream.Close();
                            tcpClient.Close();
                            listner.Stop();
                            
                        }
                        catch (Exception ex)
                        {
                            new FileLogger(FileLogger._EXCEPTION, ex.ToString());
                            new FileLogger(FileLogger._EXCEPTION, ex.StackTrace);
                            new FileLogger(FileLogger._EXCEPTION, ex.InnerException.ToString());

                            LogService.getInstance().create(ex);
                            listner.Stop();
                            listner = null;
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
            new FileLogger(FileLogger._INFORMATION, "request ManufactoringData");
            LogService.getInstance().create(LogService._INFORMATION, "SocketServer.requestManufactoringData", "");
            SocketClient socket = null;
            bool isOk = true;
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
                new FileLogger(FileLogger._EXCEPTION, "response \n" + responseString);

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
                    try
                    {
                        new FileLogger(FileLogger._DEBUG, j.Dsc + " saved in progress");
                        if (x.UID.ToString().Trim().Length <= 4) j.BoardNr = x.UID;
                        j.save();
                        //new FileLogger(FileLogger._DEBUG, j.Dsc + " saved");
                        // Job in Datanbank schreiben
                        socket.sendMessage(
                            Encoding.Default.GetBytes(j.DeleteTelegram().OuterXml)); // Delete Telegramm versenden
                    }
                    catch (Exception ex)
                    {
                        new FileLogger(FileLogger._EXCEPTION, ex.ToString());
                    }
                }

                isOk = true;
            }
            catch (FormatException ex)
            {
                new FileLogger(FileLogger._EXCEPTION, ex.ToString());
                new FileLogger(FileLogger._EXCEPTION, ex.ToString());
                new FileLogger(FileLogger._EXCEPTION, ex.StackTrace);
                new FileLogger(FileLogger._EXCEPTION, ex.InnerException.ToString());
            }
            catch (Exception ex)
            {
                new FileLogger(FileLogger._EXCEPTION, ex.ToString());
                new FileLogger(FileLogger._EXCEPTION, ex.ToString());
                new FileLogger(FileLogger._EXCEPTION, ex.StackTrace);
                new FileLogger(FileLogger._EXCEPTION, ex.InnerException.ToString());
                Console.WriteLine(ex);
            }
            if (isOk == true)
            {
                buffer[0] = (byte)(buffer[0] + 10);
                new FileLogger(FileLogger._INFORMATION + " 10", buffer.ToString());
            }
            else
            {
                buffer[0] = (byte)(buffer[0] + 20);
                new FileLogger(FileLogger._INFORMATION + " 20", buffer.ToString());
            }
            return buffer;
        }
        public byte[] jobFinished(byte[] buffer)
        {
            MMBConfig _config = MMBConfig.getInstance();
            LogService.getInstance().create(LogService._INFORMATION, "SocketServer.JobFinished", "");
            new FileLogger(FileLogger._INFORMATION, "JobFinished");
            bool isOk = true;
            try
            {
                List<ManufactoringJob> list = ManufactoringJob.getAllJobs();
                int UID = int.Parse(this.receiveUID(buffer));

                var _version = _config.XMLVersion;
                var _communicationUser = _config.KommunikationUser;
                var _communicationPassword = _config.KommunikationPassword;
                var _mashineUserID = _config.UserID;
                var _personelNumber = _config.PersonalNumber;
                var _productionKey = _config.ProductionID;
                var _xValue = _config.XValue;
                string cwPassword = ManufactoringDataRequest.getMD5Hash(_communicationUser, _communicationPassword, UID.ToString(), _xValue);

                XmlDocument xml = new XmlDocument();
                // Root Node
                XmlNode rootNode = xml.CreateElement("Root");
                xml.AppendChild(rootNode);

                // TELEGRAM NODE
                XmlElement telegrammNode = xml.CreateElement("Telegram");
                telegrammNode.SetAttribute("Name", "Receipt");
                rootNode.AppendChild(telegrammNode);

                XmlNode Version = xml.CreateElement("Version");
                telegrammNode.AppendChild(Version);
                Version.InnerText = _config.XMLVersion.ToString();

                //Communication User, machines' identification for commmunication, fixed for machine
                XmlNode CommunicationUserNode = xml.CreateElement("CommunicationUSR");
                CommunicationUserNode.InnerText = _config.KommunikationUser;
                telegrammNode.AppendChild(CommunicationUserNode);

                /* Communication Password. The Password is hashed with the MD5 Key The hashed key must be send as ASCII digits */
                XmlNode CommunicationPWDNode = xml.CreateElement("CommunicationPWD");
                CommunicationPWDNode.InnerText = cwPassword;
                telegrammNode.AppendChild(CommunicationPWDNode);

                // Identification number for the machine, fixed at installation by BKS
                XmlNode UserIDNode = xml.CreateElement("UserId");
                UserIDNode.InnerText = _config.UserID;
                telegrammNode.AppendChild(UserIDNode);

                //Date element
                XmlNode DateNode = xml.CreateElement("Date");
                DateNode.InnerText = DateTime.Now.ToString();
                telegrammNode.AppendChild(DateNode);

                // Status Node
                XmlNode StatusNode = xml.CreateElement("Status");
                telegrammNode.AppendChild(StatusNode);

                int r = 0;
                if (UID.ToString().Length <= 4)
                {
                    list.FindAll(job => job.Dsc.ToString().Trim() == UID.ToString());
                }
                else
                {
                    list.FindAll(job => job.BoardNr.ToString().Trim() == UID.ToString());
                }
                list.ForEach(job => {
                    r++;
                    int verbleibend = 0;
                    int Quantity = job.Quantity != null ? (int)job.Quantity : 0;
                    int QtyWork = job.QtyWork != null ? (int)job.QtyWork : 0;
                    verbleibend = Quantity - QtyWork;

                    // UID Node
                    XmlNode UIDNode = xml.CreateElement("UID");
                    UIDNode.InnerText = job.UID.ToString();
                    StatusNode.AppendChild(UIDNode);

                    // Systemnumber
                    XmlNode SystemNumNode = xml.CreateElement("SystemNumber");
                    SystemNumNode.InnerText = job.SystemNum.ToString();
                    StatusNode.AppendChild(SystemNumNode);

                    // KeyNumberNode
                    XmlNode KeyNumberNode = xml.CreateElement("KeyNumber");
                    StatusNode.AppendChild(KeyNumberNode);

                    // ERROR Node
                    XmlNode ErrorNode = xml.CreateElement("Error");
                    if (verbleibend != 0)
                    {
                        ErrorNode.InnerText = "Job Terminated";
                    }
                    StatusNode.AppendChild(ErrorNode);

                    // Running Node
                    XmlNode RunningNode = xml.CreateElement("Running");
                    RunningNode.InnerText = r.ToString();
                    StatusNode.AppendChild(RunningNode);

                    // Customer Info
                    XmlNode CustomerInfoNode = xml.CreateElement("CustomerInfo");
                    StatusNode.AppendChild(CustomerInfoNode);

                    // Job Creation Date
                    XmlNode JobCreationNode = xml.CreateElement("JobCreationDate");
                    JobCreationNode.InnerText = job.JobCreationDate.ToString();
                    StatusNode.AppendChild(JobCreationNode);

                    // Job Completed Date
                    XmlNode JobCompletedDateNode = xml.CreateElement("JobCompletedDate");
                    JobCompletedDateNode.InnerText = job.JobCompletedDate.ToString();
                    StatusNode.AppendChild(JobCompletedDateNode);

                    // JobStartDate
                    XmlNode StartDateNode = xml.CreateElement("JobStartDate");
                    StartDateNode.InnerText = job.JobStartDate.ToString();
                    StatusNode.AppendChild(StartDateNode);

                    // JobFinished Date
                    XmlNode JobFinishDateNode = xml.CreateElement("JobFinishDate");
                    JobFinishDateNode.InnerText = job.JobFinishDate.ToString();
                    StatusNode.AppendChild(JobFinishDateNode);

                    // Quantity
                    XmlNode QuantityNode = xml.CreateElement("Quantity");
                    QuantityNode.InnerText = job.Quantity.ToString();
                    StatusNode.AppendChild(QuantityNode);

                    // Status Code
                    XmlNode StatusCodeNode = xml.CreateElement("StatusCode");
                    if (verbleibend == 0)
                    {
                        StatusCodeNode.InnerText = "W";
                    }else
                    {
                        StatusCodeNode.InnerText = "T";
                    }

                    StatusNode.AppendChild(StatusCodeNode);

                    // QtyWork
                    XmlNode QtyWorkNode = xml.CreateElement("QtyWork");
                    QtyWorkNode.InnerText = job.Qty == null ? "0" : job.Qty.ToString();
                    StatusNode.AppendChild(QtyWorkNode);
                    job.delete();

                });
                new FileLogger(FileLogger._DEBUG, xml.OuterXml);
                new FileLogger(FileLogger._DEBUG, UID.ToString());
                /* byte[] data = Encoding.Default.GetBytes(xml.OuterXml);
                 SocketClient socket = SocketClient.getInstance();
                 socket.initCon(_config.XMLServerAdress, _config.XMLSeverPort.ToString());
                 string responseString = socket.sendMessage(data);
                 */
                /*
                list.FindAll(job => job.Dsc.Trim() == UID.ToString() ).ForEach(job =>
                   {
                       //new FileLogger(FileLogger._INFORMATION, "jobFinished " + UID.ToString());
                       try
                       {
                           new FileLogger(FileLogger._DEBUG, "jobID " + job.ID.ToString());

                           //string CommunicationUSR = _config.KommunikationUser;
                           //string CommunicationPWD = _config.KommunikationPassword;
                           //string UserID = _config.UserID;
                           //XmlDocument receipt = job.Receipt(CommunicationUSR, CommunicationPWD, UserID);
                           //byte[] requestdata = Encoding.Default.GetBytes(receipt.OuterXml);
                           socket = SocketClient.getInstance();
                           socket.initCon(_config.XMLServerAdress, _config.XMLSeverPort.ToString());
                           //string responseString = socket.sendMessage(requestdata);
                           //job.delete();
                       }catch (Exception ex)
                       {
                           new FileLogger(FileLogger._EXCEPTION, ex.ToString());
                       }
                   }); */

                //ManufactoringJob j = list.FirstOrDefault(x => x.Dsc == UID.ToString().Trim());
                //if (j == null)
                //{
                //  throw new Exception("not found");
                // isOk = false;
                //}
            }
            catch (InvalidCastException ex)
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
            new FileLogger(FileLogger._INFORMATION, "Delete Telegram");
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
