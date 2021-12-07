using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MMBServer
{
    class MMBConfig
    {
        // Singelton Pattern
        public static MMBConfig _instance;

        #region getter & setter
        public string XMLServerAdress {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/xmlClientSettings/add[@key='ServerAdress']").Attributes["value"].InnerText;
            }
            set {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/xmlClientSettings/add[@key='ServerAdress']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/xmlClientSettings");
            } 
        }
        public int XMLSeverPort {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return int.Parse( xmlDoc.SelectSingleNode("//MySettings/xmlClientSettings/add[@key='ServerPort']").Attributes["value"].InnerText );
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/xmlClientSettings/add[@key='ServerPort']").Attributes["value"].Value = value.ToString();
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/xmlClientSettings");
            }
        }
        public int XMLVersion {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return int.Parse(xmlDoc.SelectSingleNode("//MySettings/xmlClientSettings/add[@key='XMLVersion']").Attributes["value"].InnerText);
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/xmlClientSettings/add[@key='XMLVersion']").Attributes["value"].Value = value.ToString();
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/xmlClientSettings");
            }
        }
        public string UserID {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/mashineSettings/add[@key='userID']").Attributes["value"].InnerText;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/mashineSettings/add[@key='userID']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/mashineSettings");
            }
        }
        public string ProductionID {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/mashineSettings/add[@key='ProductionID']").Attributes["value"].InnerText;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/mashineSettings/add[@key='ProductionID']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/mashineSettings");
            }
        }
        public string XValue {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/mashineSettings/add[@key='XValue']").Attributes["value"].InnerText;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/mashineSettings/add[@key='XValue']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/mashineSettings");
            }
        }
        public string KommunikationUser {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/mashineSettings/add[@key='KommunikationUser']").Attributes["value"].InnerText;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/mashineSettings/add[@key='KommunikationUser']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/mashineSettings");
            }
        }
        public string PersonalNumber {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/mashineSettings/add[@key='PersonalNumber']").Attributes["value"].InnerText;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/mashineSettings/add[@key='PersonalNumber']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/mashineSettings");
            }
        }
        public string PersonalPassword {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/mashineSettings/add[@key='PersonalPassword']").Attributes["value"].InnerText;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/mashineSettings/add[@key='PersonalPassword']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/mashineSettings");
            }
        }
        public string TcpSeverAdress {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='ServerAdress']").Attributes["value"].InnerText;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='ServerAdress']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/tcpServerSettings");
            }
        }
        public string TcpPort {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='ServerPort']").Attributes["value"].InnerText;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='ServerPort']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/tcpServerSettings");
            }
        }
        public int MMBtcpPackageSize {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return int.Parse(xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='PackageSize']").Attributes["value"].InnerText);
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='PackageSize']").Attributes["value"].Value = value.ToString();
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/tcpServerSettings");
            }
        }
        public int MmbtcpPackageOffSetCMD {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return int.Parse(xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='CMD_offset']").Attributes["value"].InnerText);
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='CMD_offset']").Attributes["value"].Value = value.ToString() ;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/tcpServerSettings");
            }
        }
        public int MmbtcpPackageOffSetCount {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return int.Parse( xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='Count_offset']").Attributes["value"].InnerText );
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='Count_offset']").Attributes["value"].Value = value.ToString();
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/tcpServerSettings");
            }
        }
        public int MmbtcpPackageOffSetUID {

            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return int.Parse( xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='UID_offset']").Attributes["value"].InnerText) ;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='UID_offset']").Attributes["value"].Value = value.ToString();
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/tcpServerSettings");
            }
        }
        public int MmbtcpPackageLengthUID {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return int.Parse( xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='UID_length']").Attributes["value"].InnerText );
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='UID_length']").Attributes["value"].Value = value.ToString();
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/tcpServerSettings");
            }
        }
        public int MmbtcpPackageOffSetPersonalNumber {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return int.Parse( xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='PersonalNumber_offset']").Attributes["value"].InnerText );
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='PersonalNumber_offset']").Attributes["value"].Value = value.ToString();
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/tcpServerSettings");
            }
        }
        public int MmbtcpPackageLengthPersonalNumber {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return int.Parse( xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='PersonalNumber_length']").Attributes["value"].InnerText );
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='PersonalNumber_length']").Attributes["value"].Value = value.ToString();
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/tcpServerSettings");
            }
        }
        public int MmbtcpPackageOffSetPersonalPassword {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return int.Parse( xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='PersonalPassword_offset']").Attributes["value"].InnerText );
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='PersonalPassword_offset']").Attributes["value"].Value = value.ToString();
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/tcpServerSettings");
            }
        }
        public int MmbtcpPackageLengthPersonalPassword {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return int.Parse( xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='PersonalPassword_length']").Attributes["value"].InnerText );
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/tcpServerSettings/add[@key='PersonalPassword_length']").Attributes["value"].Value = value.ToString();
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/tcpServerSettings");
            }
        }
        public string OdbcDNS {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/odbcSettings/add[@key='DSN']").Attributes["value"].InnerText;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/odbcSettings/add[@key='DSN']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/odbcSettings");
            }
        }
        public bool OdbcUserRequired {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return bool.Parse( xmlDoc.SelectSingleNode("//MySettings/odbcSettings/add[@key='UserRequired']").Attributes["value"].InnerText );
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/odbcSettings/add[@key='UserRequired']").Attributes["value"].Value = value.ToString();
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/UserRequired");
            }
        }
        public string OdcbUser {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/odbcSettings/add[@key='User']").Attributes["value"].InnerText;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/odbcSettings/add[@key='User']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/odbcSettings");
            }
        }
        public string OdbcPwd {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/odbcSettings/add[@key='PWD']").Attributes["value"].InnerText;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/odbcSettings/add[@key='PWD']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/odbcSettings");
            }
        }
        public string MMBPwd
        {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/systemSettings/add[@key='MMBPassword']").Attributes["value"].InnerText;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/systemSettings/add[@key='MMBPassword']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/systemSettings");
            }
        }
        public string SettingsPwd
        {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/systemSettings/add[@key='SettingsPassword']").Attributes["value"].InnerText;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/systemSettings/add[@key='SettingsPassword']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/systemSettings");
            }
        }
        public string KommunikationPassword
        {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return xmlDoc.SelectSingleNode("//MySettings/mashineSettings/add[@key='KommunikationPassword']").Attributes["value"].InnerText;
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/mashineSettings/add[@key='KommunikationPassword']").Attributes["value"].Value = value;
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/mashineSettings");
            }
        }

        public bool IsDevelopeMode
        {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return bool.Parse( xmlDoc.SelectSingleNode("//MySettings/systemSettings/add[@key='IsDevelopeMode']").Attributes["value"].InnerText);
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/systemSettings/add[@key='IsDevelopeMode']").Attributes["value"].Value = value.ToString();
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/systemSettings");
            }
        }

        public int NotificationAge
        {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                return int.Parse( xmlDoc.SelectSingleNode("//MySettings/systemSettings/add[@key='NotificationAge']").Attributes["value"].InnerText );
            }
            set
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                xmlDoc.SelectSingleNode("//MySettings/systemSettings/add[@key='NotificationAge']").Attributes["value"].Value = value.ToString();
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("MySettings/systemSettings");
            }
        }

        #endregion
        private MMBConfig() {

        }
        public static MMBConfig getInstance()
        {
            if (_instance == null) { _instance = new MMBConfig(); }
            return _instance;
        }

    }
}
