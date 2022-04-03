using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MMBTestClient
{

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
                string FileName = "TestClient-" + DateTime.Now.Day +   "-" +
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
