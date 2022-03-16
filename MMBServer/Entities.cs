using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MMBServer
{
    internal class Entities
    {
    }
    public class ManufactoringDataRequest
    {
        private int? iD = null;
        private int? uID = null;
        private string boardNo = null;
        private string rodNo = null;
        private string dsc = null; // _salesOrdeNumber
        private string fertAufNr = null;
        private string persID = null;
        private string password = string.Empty;
        private string passwordMD5 = null;
        private string productionKey = null;
        private int? startPos = null;
        private int? endPos = null;
        private string xValue = string.Empty;
        #region Getter / Setter
        public int? ID { get => iD; set => iD = value; }
        public string BoardNo { get => boardNo; set => boardNo = value; }
        public string RodNo { get => rodNo; set => rodNo = value; }
        public string Dsc { get => dsc; set => dsc = value; }
        public string FertAufNr { get => fertAufNr; set => fertAufNr = value; }
        public string PasswordMD5 { get => passwordMD5; set => passwordMD5 = value; }
        public string ProductionKey { get => productionKey; set => productionKey = value; }
        public int? StartPos { get => startPos; set => startPos = value; }
        public int? EndPos { get => endPos; set => endPos = value; }
        public string PersID { get => persID; set => persID = value; }
        public int? UID { get => uID; set => uID = value; }
        #endregion

        public XmlDocument toXML()
        {

            MMBConfig _config = MMBConfig.getInstance();
            // TODO Version abfrage
            var _version = _config.XMLVersion;
            var _communicationmUser = _config.KommunikationUser;
            var _communicationUserPassword = _config.KommunikationPassword;
            var _mashineUserID = _config.UserID;
            var _personelNumber = _config.PersonalNumber;
            var _productionKey = _config.ProductionID;
            var _xValue = _config.XValue;
            string cwPassword = ManufactoringDataRequest.getMD5Hash(_communicationmUser, _communicationUserPassword, this.UID.ToString(), _xValue);
            Console.WriteLine("cwPassword " +cwPassword);
            var telegram = new XDocument(
            new XElement("Root",
                new XElement("Version", _version),
                new XElement("Telegram", new XAttribute("Name", "ManufactoringDataRequest"),
                new XElement("Version_TechnicalData", "000"),
                new XElement("CommunicationUSR", _communicationmUser),
                new XElement("CommunicationPWD", cwPassword),
                new XElement("UserID", _mashineUserID),
                new XElement("SAP_Workplace", "XXX"),
                new XElement("PersonnelNumber", _personelNumber),
                (this.UID.ToString().Length < 4 ?  new XElement("dsc", this.UID) : new XElement("BoardNr", this.UID)),
                new XElement("ProductionKey", _productionKey)
                    )));
            var xmlDocument = new XmlDocument();
            using (var xmlReader = telegram.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            LogService.getInstance().create(LogService._INFORMATION, "Create Manufactoring Request", xmlDocument.OuterXml.ToString());

            return xmlDocument;
        }
        public static  string getMD5Hash(string user, string password, string text, string xValue)
        {
            if (text == null)
                text = string.Empty;
            text = text.PadLeft(10, '0');

            var combination = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                combination += Convert.ToChar(Convert.ToInt32(text[i]) + Convert.ToInt32(xValue.ToCharArray()[i]));
            }

            var complete = user; 
            complete += password;
            complete += combination;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(complete);
            byte[] result = md5.ComputeHash(textToHash);
            Console.WriteLine("byte " + result.Length);
            var resultString = string.Empty;
            //int j = 0;
            for(int j = 0; j < result.Length; j++)
//            while (j > result.Length)
            {
                resultString += result[j];
              //  j++;
            }

            return resultString;
        }
        #region DBConnections
        public static List<ManufactoringDataRequest> getAll()
        {
            List<ManufactoringDataRequest> list = new List<ManufactoringDataRequest>();
            ODBCConnector con = ODBCConnector.getInstance();
            if (con.IsInitialized != true)
            {
                throw new Exception("ODBC not initialized");
            }
            OdbcDataReader dbreader = con.executeCMD("SELECT * FROM Jobs");
            while (dbreader.Read())
            {
                ManufactoringDataRequest i = new ManufactoringDataRequest();
                try
                {
                    i.ID = dbreader.GetInt32(dbreader.GetOrdinal("ID"));
                }
                catch (Exception Ex) { }
                try
                {
                    i.UID = dbreader.GetInt32(dbreader.GetOrdinal("UID"));
                }
                catch (Exception Ex) { }
                try
                {
                    i.BoardNo = dbreader.GetString(dbreader.GetOrdinal("BoarNo"));
                }
                catch (Exception Ex) { }
                try
                {
                    i.RodNo = dbreader.GetString(dbreader.GetOrdinal("RodNo"));
                }
                catch (Exception Ex) { }
                try
                {
                    i.Dsc = dbreader.GetString(dbreader.GetOrdinal("dsc"));
                }
                catch (Exception Ex) { }
                try
                {
                    i.FertAufNr = dbreader.GetString(dbreader.GetOrdinal("FertAufNr"));
                }
                catch (Exception Ex) { }
                try
                {
                    i.PersID = dbreader.GetString(dbreader.GetOrdinal("PersID"));
                }
                catch (Exception Ex) { }
                try
                {
                    i.PasswordMD5 = dbreader.GetString(dbreader.GetOrdinal("PasswordMD5"));
                }
                catch (Exception Ex) { }
                try
                {
                    i.ProductionKey = dbreader.GetString(dbreader.GetOrdinal("ProductionKey"));
                }
                catch (Exception Ex) { }
                try
                {
                    i.startPos = dbreader.GetInt32(dbreader.GetOrdinal("startPos"));
                }
                catch (Exception Ex) { }
                try
                {
                    i.endPos = dbreader.GetInt32(dbreader.GetOrdinal("endPos"));
                }
                catch (Exception Ex) { }
                list.Add(i);
            }
            return list;
        }
        public void save()
        {
            if (this.ID != null)
            {
                this._update();
            }
            else
            {
                this._create();
            }
        }
        public ManufactoringDataRequest _create()
        {
            ODBCConnector con = ODBCConnector.getInstance();
            if (con.IsInitialized != true)
            {
                throw new Exception("ODBC not initialized");
            }
            string cmd = "Insert Jobs (BoardNo, RodNo, dsc, FertAufNr, PersID, PasswordMD5, ProductionKey," +
                " startPos, endPos, UID) VALUES (";
            // BoardNo
            if (String.IsNullOrEmpty(boardNo) == false)
            {
                cmd += this.boardNo + " , ";
            }
            else
            {
                cmd += "null, ";
            }
            //RodNo string
            if (String.IsNullOrEmpty(RodNo) == false)
            {
                cmd += this.RodNo + " , ";
            }
            else
            {
                cmd += "null, ";
            }
            //dsc string
            if (String.IsNullOrEmpty(dsc) == false)
            {
                cmd += this.dsc + " , ";
            }
            else
            {
                cmd += "null, ";
            }
            //FertAufNr string
            if (String.IsNullOrEmpty(fertAufNr) == false)
            {
                cmd += this.fertAufNr + " , ";
            }
            else
            {
                cmd += "null, ";
            }
            //PersID int
            if (this.PersID != null)
            {
                cmd += this.PersID + " , ";
            }
            else
            {
                cmd += "null, ";
            }
            //PasswordMD5 string
            if (String.IsNullOrEmpty(PasswordMD5) == false)
            {
                cmd += this.PasswordMD5 + " , ";
            }
            else
            {
                cmd += "null, ";
            }
            //ProductionKey string
            if (String.IsNullOrEmpty(ProductionKey) == false)
            {
                cmd += this.productionKey + " , ";
            }
            else
            {
                cmd += "null, ";
            }
            //startPos int
            if (this.startPos != null)
            {
                cmd += this.startPos + " , ";
            }
            else
            {
                cmd += "null, ";
            }
            //endPos int
            if (this.endPos != null)
            {
                cmd += this.endPos + " , ";
            }
            else
            {
                cmd += "null, ";
            }
            cmd += ");";
            con.ExecuteNonQuery(cmd);
            con.dispose();
            return this;
        }
        public ManufactoringDataRequest _update()
        {
            ODBCConnector con = ODBCConnector.getInstance();
            if (con.IsInitialized != true)
            {
                throw new Exception("ODBC not initialized");
            }
            string cmd = "UPDATE Jobs Set ";
            // Update UID
            if (this.UID != null)
            {
                cmd += "UID = " + this.UID + " , ";
            }
            else
            {
                cmd += "UID = null, ";
            }
            // Update BoardNo
            if (String.IsNullOrEmpty(this.BoardNo) == false)
            {
                cmd += "BoardNo = " + this.BoardNo + " , ";
            }
            else
            {
                cmd += "BoardNo = null, ";
            }
            // Update RodNo
            if (String.IsNullOrEmpty(this.RodNo) == false)
            {
                cmd += "RodNo = " + this.RodNo + " , ";
            }
            else
            {
                cmd += "RodNo = null, ";
            }
            // Update dsc
            if (String.IsNullOrEmpty(this.dsc) == false)
            {
                cmd += "dsc = " + this.dsc + " , ";
            }
            else
            {
                cmd += "dsc = null, ";
            }
            // Update FertAufNr
            if (String.IsNullOrEmpty(this.FertAufNr) == false)
            {
                cmd += "FertAufNr = " + this.FertAufNr + " , ";
            }
            else
            {
                cmd += "FertAufNr = null, ";
            }
            // Update PersID
            if (this.PersID != null)
            {
                cmd += "PersID = " + this.PersID + " , ";
            }
            else
            {
                cmd += "PersID = null, ";
            }
            // Update PasswordMD5
            if (String.IsNullOrEmpty(this.PasswordMD5) == false)
            {
                cmd += "PasswordMD5 = " + this.PasswordMD5 + " , ";
            }
            else
            {
                cmd += "PasswordMD5 = null, ";
            }
            // Update ProductionKey
            if (String.IsNullOrEmpty(this.ProductionKey) == false)
            {
                cmd += "ProductionKey = " + this.ProductionKey + " , ";
            }
            else
            {
                cmd += "ProductionKey = null, ";
            }
            // Update startPos
            if (this.startPos != null)
            {
                cmd += "startPos = " + this.startPos + " , ";
            }
            else
            {
                cmd += "startPos = null, ";
            }
            // Update endPos
            if (this.endPos != null)
            {
                cmd += "endPos = " + this.endPos + " , ";
            }
            else
            {
                cmd += "endPos = null, ";
            }
            cmd += " WHERE ID = " + this.ID;
            con.ExecuteNonQuery(cmd);
            con.dispose();
            return this;
        }
        #endregion
    }
    public class ManufactoringJob
    {
        // interne ID
        private int? iD = null;
        private int? uID = null;
        private string reference = null;
        private string systemNum = null;
        private string cyclNumber = null;
        private string dsc = null;
        private int? qty = null;
        private string keyNumber = null;
        private string keyDescription = null;
        private string keyBlank = null;
        private string error = null;
        private int? running = null;
        private string customerInfo = null;
        private DateTime? jobCreationDate = null;
        private DateTime? jobCompletedDate = null;
        private DateTime? jobStartDate = null;
        private DateTime? jobFinishDate = null;
        private int? quantity = null;
        private string statusCode = null;
        private int? qtyWork = null;
        private int? bLA = null;
        private int? bLB = null;

        public int? ID { get => iD; set => iD = value; }
        public int? UID { get => uID; set => uID = value; }
        public string Reference { get => reference; set => reference = value; }
        public string SystemNum { get => systemNum; set => systemNum = value; }
        public string CyclNumber { get => cyclNumber; set => cyclNumber = value; }
        public string Dsc { get => dsc; set => dsc = value; }
        public int? Qty { get => qty; set => qty = value; }
        public string Error { get => error; set => error = value; }
        public int? Running { get => running; set => running = value; }
        public string CustomerInfo { get => customerInfo; set => customerInfo = value; }
        public DateTime? JobCreationDate { get => jobCreationDate; set => jobCreationDate = value; }
        public DateTime? JobCompletedDate { get => jobCompletedDate; set => jobCompletedDate = value; }
        public DateTime? JobStartDate { get => jobStartDate; set => jobStartDate = value; }
        public DateTime? JobFinishDate { get => jobFinishDate; set => jobFinishDate = value; }
        public int? Quantity { get => quantity; set => quantity = value; }
        public string StatusCode { get => statusCode; set => statusCode = value; }
        public int? BLA { get => bLA; set => bLA = value; }
        public int? BLB { get => bLB; set => bLB = value; }

        #region XML Generation
        public XmlDocument DeleteTelegram()
        {

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
            UIDNode.InnerText = this.UID + ""; ;
            TelegrammNode.AppendChild(UIDNode);
            LogService.getInstance().create(LogService._INFORMATION, "Create DeleteTelegram", xml.OuterXml.ToString());
            Console.WriteLine("Delete Send " + xml.OuterXml);
            return xml;
        }
        public XmlDocument Receipt(string User, string Password, string UserId)
        {
            MMBConfig _config = MMBConfig.getInstance();
            var _version = _config.XMLVersion;
            var _communicationmUser = _config.KommunikationUser;
            var _communicationUserPassword = _config.KommunikationPassword;
            var _mashineUserID = _config.UserID;
            var _personelNumber = _config.PersonalNumber;
            var _productionKey = _config.ProductionID;
            var _xValue = _config.XValue;
            string cwPassword = ManufactoringDataRequest.getMD5Hash(_communicationmUser, _communicationUserPassword, this.UID.ToString(), _xValue);

            MMBConfig config = MMBConfig.getInstance();

            XmlDocument xml = new XmlDocument();
            XmlNode rootNode = xml.CreateElement("Root");
            xml.AppendChild(rootNode);
            XmlNode telegrammNode = xml.CreateElement("Telegram");
            rootNode.AppendChild(telegrammNode);
            XmlAttribute TelegrammName = xml.CreateAttribute("Name");
            TelegrammName.Value = "Receipt";
            telegrammNode.Attributes.Append(TelegrammName);

            // Version Declaration
            XmlNode Version = xml.CreateElement("Version");
            telegrammNode.AppendChild(Version);
            Version.InnerText = config.XMLVersion.ToString() ;

            //Communication User, machines' identification for commmunication, fixed for machine
            XmlNode CommunicationUserNode = xml.CreateElement("CommunicationUSR");
            CommunicationUserNode.InnerText = config.KommunikationUser;
            telegrammNode.AppendChild(CommunicationUserNode);

            /* Communication Password. The Password is hashed with the MD5 Key The hashed key must be send as ASCII digits */
            XmlNode CommunicationPWDNode = xml.CreateElement("CommunicationPWD");
            CommunicationPWDNode.InnerText = cwPassword;
            telegrammNode.AppendChild(CommunicationPWDNode);

            // Identification number for the machine, fixed at installation by BKS
            XmlNode UserIDNode = xml.CreateElement("UserId");
            UserIDNode.InnerText = config.UserID;
            telegrammNode.AppendChild(UserIDNode);

            // Date element
            XmlNode DateNode = xml.CreateElement("Date");
            DateNode.InnerText = DateTime.Now.ToString();
            telegrammNode.AppendChild(DateNode);

            // Status Node
            XmlNode StatusNode = xml.CreateElement("Status");
            telegrammNode.AppendChild(StatusNode);

            // Unique Identifier, of the Job
            XmlNode UIDNode = xml.CreateElement("UID");
            UIDNode.InnerText = this.dsc.ToString();
            StatusNode.AppendChild(UIDNode);

            // Transaction Identifier, ca be Omitted
            XmlNode TIDNode = xml.CreateElement("TID");
            StatusNode.AppendChild(TIDNode);

            // In Case of an master-key-system key, insert Name here 
            XmlNode SystemNumberNode = xml.CreateElement("SystemNumber");
            SystemNumberNode.InnerText = this.SystemNum;
            StatusNode.AppendChild(SystemNumberNode);

            // Key Number
            XmlNode KeyNumberNode = xml.CreateElement("KeyNumber");
            StatusNode.AppendChild(KeyNumberNode);

            // Key Description of the Key (e.g. order number)
            XmlNode KeyDescriptionNode = xml.CreateElement("KeyDescription");
            StatusNode.AppendChild(KeyDescriptionNode);

            // Key Reference (Profile of the Key)
            XmlNode KeyBlankNode = xml.CreateElement("KeyBlank");
            StatusNode.AppendChild(KeyBlankNode);

            /* Error detected in the job -Message, please fill verbosely Whatever the machine 
             * wants to tell us about the error: Type: string.Proposal: Number 6 digits, 
             * Main problem, special problem, freestyle text explaining the problem(solution to solve the problem)
             */
            XmlNode ErrorNode = xml.CreateElement("Error");
            if (this.Error != null)
            {
                ErrorNode.InnerText = this.Error;
            }
            StatusNode.AppendChild(ErrorNode);

            /* Running number of the key in the job Means: first key of the job, 
             * second key of the job, and so on paired with the uid, it will be the unique - 
             * searchkey to identify the key */
            XmlNode RunningNode = xml.CreateElement("Running");
            if (this.Running != null)
            {
                RunningNode.InnerText = this.Running.ToString();
            }
            StatusNode.AppendChild(RunningNode);

            // Customer Information
            XmlNode CustomerInfoNode = xml.CreateElement("CustomerInfo");
            StatusNode.AppendChild(CustomerInfoNode);

            /* Job Creation date(Date when the specific Job was imported 
             * in the program) which means the upload and therefore activation 
             * in machine Time-Format suggestion: like in Oracle - DB: dd.mm.yyyy hh:mm: ss */
            XmlNode JobCreationDateNode = xml.CreateElement("JobCreationDate");
            JobCreationDateNode.InnerText = this.JobCreationDate.ToString();
            StatusNode.AppendChild(JobCreationDateNode);

            /* Job complete date(Date when the specific Job was completed ) which means, leaving the 
             * machine towards key-caroussel Time - Format suggestion: like in Oracle - DB: dd.mm.yyyy hh:mm: ss */
            XmlNode JobCompletedDateNode = xml.CreateElement("JobCompletedDate");
            if (this.JobCompletedDate != null)
            {
                JobCompletedDateNode.InnerText = this.JobCompletedDate.ToString();
            }
            StatusNode.AppendChild(JobCompletedDateNode);

            /* Job Starting date(Date when the specific Job was begun ) when entering machine 
             * Time-Format suggestion: like in Oracle - DB: dd.mm.yyyy hh:mm: ss */
            XmlNode JobStartDateNode = xml.CreateElement("JobStartDate");
            if (this.JobStartDate != null)
            {
                JobStartDateNode.InnerText = this.JobStartDate.ToString();
            }
            StatusNode.AppendChild(JobStartDateNode);

            /* Job finish date(Date when the specific Job was ended) Timestamp when leaving Finish 
             * station, id est: state of process when key is usable, but may not ready for customer, 
             * other stations still to come.Time - Format suggestion: like in Oracle - DB: dd.mm.yyyy hh:mm:ss */
            XmlNode JobFinishDateNode = xml.CreateElement("JobFinishDate");
            if (this.JobFinishDate != null)
            {
                JobFinishDateNode.InnerText = this.JobFinishDate.ToString();
            }
            StatusNode.AppendChild(JobFinishDateNode);

            // Quantity of Key in the Job
            XmlNode QuantityNode = xml.CreateElement("Quantity");
            if (this.Quantity != null)
            {
                QuantityNode.InnerText = this.Quantity.ToString();
            }
            StatusNode.AppendChild(QuantityNode);

            /* <!--Status Code of the JOB (W = Completed , T = Aborted ) 
             * W= (Worked) Completed means all keys passed their finish stations 
             * (Will be regarded as produced. Reproduction needs special user pwd) 
             * T= (Terminated) at least one key did not pass his finish station before user terminated or error. */
            XmlNode StatusCodeNode = xml.CreateElement("StatusCode");
            if (this.StatusCode != null)
            {
                StatusCodeNode.InnerText = this.StatusCode.ToString();
            }
            StatusNode.AppendChild(StatusCodeNode);


            // Quantity of key in the job wich were produced
            //NOTE: Removed
            /*XmlNode QtyWorkNode = xml.CreateElement("QtyWork");
            if (this.QtyWork != null)
            {
                QtyWorkNode.InnerText = this.QtyWork.ToString();
            }
            StatusNode.AppendChild(QtyWorkNode);
            LogService.getInstance().create(LogService._INFORMATION, "Create Receipt", xml.OuterXml.ToString());
            */ 
            return xml;
        }
        public static List<ManufactoringJob> convertFromManufactoringData(XmlDocument xml)
        {
            List<ManufactoringJob> list = new List<ManufactoringJob>();
            XmlNodeList nodeList = xml.DocumentElement.SelectNodes("/Root/Telegram/Job");
            string UID = "", Reference = "", SystemNum = "", CylNumber = "", Dsc = "";
            int CylNumer = 0, Qty = 0, Quantity = 0, BLA = 0, BLB = 0;
            DateTime JobCreationDate = DateTime.Now;
            string res = "";
            foreach (XmlNode node in nodeList)
            {
                // Attribute auslesen
                UID = node.SelectSingleNode("UID").InnerText;
                Reference = node.SelectSingleNode("Reference").InnerText;
                SystemNum = node.SelectSingleNode("SystemNum").InnerText;
                CylNumber = node.SelectSingleNode("CylNumber").InnerText;
                Dsc = node.SelectSingleNode("Dsc").InnerText;
                Qty = Int32.Parse(node.SelectSingleNode("Qty").InnerText);
                Quantity = Int32.Parse(node.SelectSingleNode("Qty").InnerText);

                // BLA BLB Nodes
                // Length Values auslesen
                XmlNode specs = node.SelectSingleNode("CylinderSpec");
                foreach (XmlNode node2 in specs)
                {
                    if (node2.Name == "Length" && node2.Attributes["Length"] != null) {
                        if (node2.Attributes["Length"].Value == "1") BLA = int.Parse(node2.InnerText);
                        if (node2.Attributes["Length"].Value == "2") BLB = int.Parse(node2.InnerText);
                    }
                }

                ManufactoringJob item = new ManufactoringJob();
                // Attribute in item schreiben
                item.UID = Int32.Parse(UID);
                item.Reference = Reference;
                item.SystemNum = SystemNum;
                item.CyclNumber = CylNumber;
                item.Dsc = Dsc;
                item.Qty = Qty;
                item.Quantity = Quantity;
                item.JobCreationDate = JobCreationDate;
                list.Add(item);
            }
            return list;
        }
        #endregion
        #region DB Activities
        public static List<ManufactoringJob> getAllJobs()
        {
            List<ManufactoringJob> list = new List<ManufactoringJob>();
            ODBCConnector con = ODBCConnector.getInstance();
            if (con.IsInitialized != true)
            {
                throw new Exception("ODBC not initialized");
            }
            OdbcDataReader dbreader = con.executeCMD("SELECT * FROM Jobs");

            while (dbreader.Read())
            {
                ManufactoringJob j = new ManufactoringJob();
                try
                {
                    j.ID = dbreader.GetInt32(dbreader.GetOrdinal("ID"));
                }
                catch (Exception Ex) { }
                try
                {
                    j.UID = dbreader.GetInt32(dbreader.GetOrdinal("UID"));
                }
                catch (Exception Ex) { }
                try
                {
                    j.Reference = dbreader.GetString(dbreader.GetOrdinal("Reference"));
                }
                catch (Exception Ex) { }
                try
                {
                    j.SystemNum = dbreader.GetString(dbreader.GetOrdinal("SystemNum"));
                }
                catch (Exception Ex) { }
                try
                {
                    j.CyclNumber = dbreader.GetString(dbreader.GetOrdinal("CyclNumber"));
                }
                catch (Exception Ex) { }
                try
                {
                    j.Dsc = dbreader.GetString(dbreader.GetOrdinal("Dsc"));
                }
                catch (Exception Ex) { }
                try
                {
                    j.Qty = dbreader.GetInt32(dbreader.GetOrdinal("Qty"));
                }
                catch (Exception Ex) { }

                try
                {
                    j.Error = dbreader.GetString(dbreader.GetOrdinal("Error"));
                }
                catch (Exception Ex) { }
                try
                {
                    j.Running = dbreader.GetInt32(dbreader.GetOrdinal("Running"));
                }
                catch (Exception Ex) { }
                try
                {
                    j.CustomerInfo = dbreader.GetString(dbreader.GetOrdinal("CustomerInfo"));
                }
                catch (Exception Ex) { }
                try
                {
                    j.JobCreationDate = dbreader.GetDateTime(dbreader.GetOrdinal("JobCreationDate"));
                }
                catch (Exception Ex) { }
                try
                {
                    j.JobCompletedDate = dbreader.GetDateTime(dbreader.GetOrdinal("JobCompletedDate"));
                }
                catch (Exception Ex) { }
                try
                {
                    j.JobStartDate = dbreader.GetDateTime(dbreader.GetOrdinal("JobStartDate"));
                }
                catch (Exception Ex) { }
                try
                {
                    j.JobFinishDate = dbreader.GetDateTime(dbreader.GetOrdinal("JobFinishDate"));
                }
                catch (Exception Ex) { }
                try
                {
                    j.Quantity = dbreader.GetInt32(dbreader.GetOrdinal("Quantity"));
                }
                catch (Exception Ex) { }
                try
                {
                    j.StatusCode = dbreader.GetString(dbreader.GetOrdinal("StatusCode"));
                }
                catch (Exception Ex) { }

                list.Add(j);
            }


            con.dispose();

            return list;
        }
        // Create or Update in DB
        public void save()
        {

            if (this.ID != null)
            {
                this._update();
            }
            else
            {
                this._create();
            }
        }
        // Dataset Update
        private ManufactoringJob _update()
        {
            ODBCConnector con = ODBCConnector.getInstance();
            if (con.IsInitialized != true)
            {
                throw new Exception("ODBC not initialized");
            }
            string cmd = "UPDATE Jobs Set ";
            // Update UID
            if (this.UID != null)
            {
                cmd += "UID = " + this.UID + " , ";
            }
            else
            {
                cmd += "UID = null, ";
            }
            //Update Reference
            if (String.IsNullOrEmpty(this.Reference) == false)
            {
                cmd += "Reference = '" + this.Reference.Trim() + "' , ";
            }
            else
            {
                cmd += "Reference = null, ";
            }
            //Update SystemNum
            if (String.IsNullOrEmpty(this.SystemNum) == false)
            {
                cmd += "SystemNum = '" + this.SystemNum.Trim() + "' , ";
            }
            else
            {
                cmd += "SystemNum = null, ";
            }
            //Update CyclNumber
            if (String.IsNullOrEmpty(this.CyclNumber) == false)
            {
                cmd += "CyclNumber = '" + this.CyclNumber.Trim() + "' , ";
            }
            else
            {
                cmd += "CyclNumber = null, ";
            }
            //Update Dsc
            if (String.IsNullOrEmpty(this.Dsc) == false)
            {
                cmd += "Dsc = '" + this.Dsc.Trim() + "' , ";
            }
            else
            {
                cmd += "Dsc = null, ";
            }
            //Update Qty
            if (this.Qty != null)
            {
                cmd += "Qty = " + this.Qty + " , ";
            }
            else
            {
                cmd += "QtyWork = null, ";
            }

            //Update Error
            if (this.Error != null)
            {
                cmd += "Error = '" + this.Error.Trim() + "' , ";
            }
            else
            {
                cmd += "Error = null, ";
            }
            //Update Running
            if (this.Running != null)
            {
                cmd += "Running = " + this.Running + " , ";
            }
            else
            {
                cmd += "Running = null, ";
            }
            //Update CustomerInfo
            if (this.CustomerInfo != null)
            {
                cmd += "CustomerInfo = '" + this.CustomerInfo.Trim() + "' , ";
            }
            else
            {
                cmd += "CustomerInfo = null, ";
            }
            //Update JobCreationDate
            if (this.JobCreationDate != null)
            {
                cmd += "JobCreationDate = '" + this.JobCreationDate + "' , ";
            }
            else
            {
                cmd += "JobCreationDate = null, ";
            }
            //Update JobCompletedDate
            if (this.JobCompletedDate != null)
            {
                cmd += "JobCompletedDate = '" + this.JobCompletedDate + "' , ";
            }
            else
            {
                cmd += "JobCompletedDate = null, ";
            }
            //Update JobStartDate
            if (this.JobStartDate != null)
            {
                cmd += "JobStartDate = '" + this.JobStartDate + "' , ";
            }
            else
            {
                cmd += "JobStartDate = null, ";
            }
            //Update JobFinishDate
            if (this.JobFinishDate != null)
            {
                cmd += "JobFinishDate = '" + this.JobFinishDate + "' , ";
            }
            else
            {
                cmd += "JobFinishDate = null, ";
            }
            //Update Quantity
            if (this.Quantity != null)
            {
                cmd += "Quantity = " + this.Quantity + " , ";
            }
            else
            {
                cmd += "Quantity = null, ";
            }
            //Update StatusCode
            if (this.StatusCode != null)
            {
                cmd += "StatusCode = '" + this.StatusCode.Trim() + "' , ";
            }
            else
            {
                cmd += "StatusCode = null, ";
            }
            //Update QtyWork
            
            cmd += " WHERE ID = " + this.ID;
            con.ExecuteNonQuery(cmd);
            con.dispose();
            return this;
        }
        // DataSet Create
        private ManufactoringJob _create()
        {
            ODBCConnector con = ODBCConnector.getInstance();
            if (con.IsInitialized != true)
            {
                throw new Exception("ODBC not initialized");
            }
            string cmd = "Insert Jobs (UID, Reference,SystemNum," +
                "CyclNumber,Dsc,Qty,TID,KeyNumber,KeyDescription,KeyBlank," +
                "Error,Running,CustomerInfo,JobCreationDate,JobCompletedDate," +
                "JobStartDate,JobFinishDate,Quantity,StatusCode,QtyWork, BLA, BLB) VALUES (";

            if (this.UID != null)
            {
                cmd += this.UID + " , ";
            }
            else
            {
                cmd += "null, ";
            }
            //Update Reference
            if (String.IsNullOrEmpty(this.Reference) == false)
            {
                cmd += "'" + this.Reference.Trim() + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            //Update SystemNum
            if (String.IsNullOrEmpty(this.SystemNum) == false)
            {
                cmd += "'" + this.SystemNum.Trim() + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            //Update CyclNumber
            if (String.IsNullOrEmpty(this.CyclNumber) == false)
            {
                cmd += "'" + this.CyclNumber.Trim() + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            //Update Dsc
            if (String.IsNullOrEmpty(this.Dsc) == false)
            {
                cmd += "'" + this.Dsc.Trim() + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            //Update Qty
            if (this.Qty != null)
            {
                cmd += "" + this.Qty + " , ";
            }
            else
            {
                cmd += "null, ";
            }

            //Update Error
            if (this.Error != null)
            {
                cmd += "'" + this.Error.Trim() + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            //Update Running
            if (this.Running != null)
            {
                cmd += "" + this.Running + " , ";
            }
            else
            {
                cmd += "null, ";
            }
            //Update CustomerInfo
            if (this.CustomerInfo != null)
            {
                cmd += "'" + this.CustomerInfo.Trim() + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            //Update JobCreationDate
            if (this.JobCreationDate != null)
            {
                cmd += "'" + this.JobCreationDate + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            //Update JobCompletedDate
            if (this.JobCompletedDate != null)
            {
                cmd += "'" + this.JobCompletedDate + "' , ";
            }
            else
            {
                cmd += " null, ";
            }
            //Update JobStartDate
            if (this.JobStartDate != null)
            {
                cmd += "'" + this.JobStartDate + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            //Update JobFinishDate
            if (this.JobFinishDate != null)
            {
                cmd += "'" + this.JobFinishDate + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            //Update Quantity
            if (this.Quantity != null)
            {
                cmd += "" + this.Quantity + " , ";
            }
            else
            {
                cmd += "null, ";
            }
            //Update StatusCode
            if (this.StatusCode != null)
            {
                cmd += "'" + this.StatusCode.Trim() + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            // BLA
            if (this.BLA != null)
            {
                cmd += "'" + this.BLA + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            //BLB
            if (this.BLB != null)
            {
                cmd += "'" + this.BLB + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            cmd += ");";
            con.ExecuteNonQuery(cmd);
            con.dispose();
            return this;

        }
        public void delete()
        {
            List<ManufactoringJob> list = new List<ManufactoringJob>();
            ODBCConnector con = ODBCConnector.getInstance();
            if (con.IsInitialized != true)
            {
                throw new Exception("ODBC not initialized");
            }
            con.ExecuteNonQuery("DELETE from Jobs WHERE ID = " + this.ID);
        }
        #endregion
    }
    public class TestData
    {
        private int iD = -1;
        private int uID;
        private string xmlContent = "";

        public int ID { get => iD; set => iD = value; }
        public int UID { get => uID; set => uID = value; }
        public string XmlContent { get => xmlContent; set => xmlContent = value; }

        public static List<TestData> findAll()
        {
            List<TestData> list = new List<TestData>();
            ODBCConnector con = ODBCConnector.getInstance();
            if (con.IsInitialized != true)
            {
                //throw new Exception("ODBC not initialized");
            }
            OdbcDataReader dbreader = con.executeCMD("SELECT * FROM testData");

            while (dbreader.Read())
            {
                TestData i = new TestData();
                try
                {
                    i.ID = dbreader.GetInt32(dbreader.GetOrdinal("ID"));
                }
                catch (Exception Ex) { }
                try
                {
                    i.UID = dbreader.GetInt32(dbreader.GetOrdinal("UID"));
                }
                catch (Exception Ex) { }
                try
                {
                    i.XmlContent = dbreader.GetValue(dbreader.GetOrdinal("XMLContent")) as String;
                }
                catch (Exception Ex) { }
                list.Add(i);
            }
            return list;
        }

        public static void truncate()
        {
            List<TestData> list = new List<TestData>();
            ODBCConnector con = ODBCConnector.getInstance();
            con.ExecuteNonQuery("TRUNCATE Table testData");
        }

        public void save()
        {
            if (this.iD > 0)
            {
                this._update();
            }
            else
            {
                this._create();
            }
        }
        private TestData _create()
        {
            ODBCConnector con = ODBCConnector.getInstance();
            if (con.IsInitialized != true)
            {
                throw new Exception("ODBC not initialized");
            }
            string cmd = "Insert testData (UID, XMLContent) VALUES (";
            if (this.UID != null)
            {
                cmd += this.UID + " , ";
            }
            else
            {
                cmd += "null, ";
            }
            if (String.IsNullOrEmpty(this.xmlContent) == false)
            {
                cmd += "'" + this.xmlContent.Trim() + "' ";
            }
            else
            {
                cmd += "null ";
            }
            cmd += ");";
            con.ExecuteNonQuery(cmd);
            con.dispose();
            return this;
        }
        private TestData _update()
        {
            throw new NotImplementedException();
        }
    }
}
