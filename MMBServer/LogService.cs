using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MMBServer
{
    internal class LogService
    {
        private static LogService _instance;
        private MMBConfig _config = MMBConfig.getInstance();
        public static string _ERROR = "ERROR";
        public static string _WARNING = "WARNING";
        public static string _SUCCESS = "SUCCESS";
        public static string _DEBUG = "DEBUG";
        public static string _INFORMATION = "INFORMATION";
        public static string _EXCEPTION = "EXCEPTION";

        // Singelton Pattern
        public static LogService getInstance()
        {
            if (_instance == null) { _instance = new LogService(); }
            return _instance;
        }
        public void create(string type, string message, string hint = "")
        {
            LogEntry log = new LogEntry();
            log.Type = type;
            log.Message = message;
            log.Hint = hint;
            //show_MessageBox(log);
            if (log.Type.Equals(LogService._DEBUG) == false || (log.Type.Equals(LogService._DEBUG) && _config.IsDevelopeMode)) {
                log.save();
            }
        }
        public void create(Exception ex)
        {
            LogEntry log = new LogEntry();
            log.Type = LogService._EXCEPTION;
            log.Stacktrace = ex.StackTrace;
            log.Message = ex.Message;
            //show_MessageBox(log);
            log.save();
        }

        private void show_MessageBox(LogEntry l) {
            if (_config.IsDevelopeMode || l.Type.Equals(LogService._ERROR)) {
                MessageBox.Show(l.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void writeToDB()
        {

        }

    }
    internal class LogEntry
    {
        private DateTime _created;
        private string _type;
        private string _stacktrace;
        private string _message;
        private string _hint;
        private int _iD = int.MinValue;
        public DateTime Created { get => _created; set => _created = value; }
        public string Type { get => _type; set => _type = value; }
        public string Stacktrace { get => _stacktrace; set => _stacktrace = value; }
        public string Message { get => _message; set => _message = value; }
        public string Hint { get => _hint; set => _hint = value; }
        public int ID { get => _iD; set => _iD = value; }

        public override string ToString()
        {
            return "{ _created: " + _created + "; type: " + _type + "; _message: " + _message + "; hint: " + Hint + "}";
        }

        public LogEntry()
        {
            _created = DateTime.Now;
        }

        public void save()
        {
            Console.WriteLine(this.ToString());
            if (this.ID <= 0)
            {
                this._create();
            }
            else
            {
                this._update();
            }
        }
        private void _update() { throw new NotImplementedException(); }
        private LogEntry _create()
        {
            ODBCConnector con = ODBCConnector.getInstance();
            if (con.IsInitialized != true)
            {
                throw new Exception("ODBC not initialized");
            }
            string cmd = "Insert Log (createDate, type, stacktrace, messageText, hint) VALUES (";

            //CreateDate
            if (this._created != null)
            {
                cmd += "'" + this._created + "', ";
            }
            else
            {
                cmd += "null, ";
            }
            //type string
            if (String.IsNullOrEmpty(_type) == false)
            {
                cmd += "'" + this._type + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            //stacktrace
            if (String.IsNullOrEmpty(_stacktrace) == false)
            {
                cmd += "'" + this._stacktrace + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            //messageText
            if (String.IsNullOrEmpty(_message) == false)
            {
                cmd += "'" + this._message + "' , ";
            }
            else
            {
                cmd += "null, ";
            }
            //hint
            if (String.IsNullOrEmpty(_hint) == false)
            {
                cmd += "'" + this._hint + "' ";
            }
            else
            {
                cmd += "null";
            }
            cmd += ");";
            con.ExecuteNonQuery(cmd);
            con.dispose();
            return this;
        }
        public static void cleanNotifications()
        {
            ODBCConnector con = ODBCConnector.getInstance();
            MMBConfig _config = MMBConfig.getInstance();
            if (con.IsInitialized != true)
            {
                return;
            }

            DateTime oldesDate = DateTime.Now;
            oldesDate = oldesDate.AddDays((_config.NotificationAge * -1));
            string cmd = "DELETE FROM Log WHERE createDate < '" + oldesDate + "';";
            con.ExecuteNonQuery(cmd);
            con.dispose();
        }
    }

    internal class FileLogger
    {
        private string path = "C://Log//";
        public static string _INFORMATION = "INFORMATION";
        public static string _DEBUG = "INFORMATION";
        public static string _EXCEPTION = "INFORMATION";
            
        public FileLogger(string type, string msg)
        {
            try
            {
                string FileName = DateTime.Now.Day +   "-" +
                                  DateTime.Now.Month + "-" +
                                  DateTime.Now.Year + ".txt";
                
                DirectoryInfo dir = new DirectoryInfo(path);
                if (!dir.Exists)
                {
                    dir.Create();
                }

                if (!File.Exists(path + FileName))
                {
                    StreamWriter file = new StreamWriter(path + FileName);
                    file.WriteLine(DateTime.Now.ToString() + "\t\t\tFileCreated");
                    file.Close();
                }

                using (StreamWriter sw = File.AppendText(path+FileName))
                {
                    sw.WriteLine("");
                    sw.WriteLine(DateTime.Now.ToString() + "\t" + type + "\t" + msg);
                }	
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
