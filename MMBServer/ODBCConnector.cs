using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMBServer
{
    public class ODBCConnector
    {
        private static ODBCConnector instance = null;
        public static ODBCConnector getInstance()
        {
            if (instance == null)
            {
                instance = new ODBCConnector();
            }
            return instance;
        }

        public OdbcConnection DbConnection = null;
        string DSN = "";
        private OdbcDataReader DbReader = null;
        private OdbcCommand DbCommand = null;
        private bool isInitialized = false;
        private MMBConfig _config;

        public bool IsInitialized { get => isInitialized; set => isInitialized = value; }

        private ODBCConnector()
        {
            _config = MMBConfig.getInstance();
            if (_config.OdbcUserRequired)
            {
                this.init(
                    _config.OdbcDNS,
                    _config.OdcbUser,
                    _config.OdbcPwd
                    );
            } else
            {
                this.init(_config.OdbcDNS);
            }
        }
        public void init(string DSN)
        {
            try
            {
                this.DSN = DSN;
                this.DbConnection = new OdbcConnection("DSN=" + DSN);
                this.IsInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void init(string DSN, string UID, string PWD)
        {
            try
            {
                this.DSN = DSN;
                this.DbConnection = new OdbcConnection("DSN=" + DSN + ";Uid=" + UID + ";Pwd=" + PWD);
                this.IsInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public OdbcDataReader executeCMD(string cmd)
        {
            this.DbConnection.Open();
            this.DbCommand = DbConnection.CreateCommand();
            try
            {


                DbCommand.CommandText = cmd;
                this.DbReader = DbCommand.ExecuteReader();

            }catch (Exception ex)
            {
                new FileLogger(FileLogger._EXCEPTION, cmd);
                new FileLogger(FileLogger._EXCEPTION, ex.StackTrace);
            }
            return DbReader;

        }
        public void ExecuteNonQuery(string cmd)
        {
            try
            {
                this.DbCommand = DbConnection.CreateCommand();
                this.DbCommand.CommandText = cmd + ";";
                this.DbConnection.Open();
                this.DbCommand.ExecuteNonQuery();
                this.DbConnection.Close();
            }
            catch (Exception ex)
            {
                new FileLogger(FileLogger._EXCEPTION, cmd);
                new FileLogger(FileLogger._EXCEPTION, ex.StackTrace);
            }
        }
        public void dispose()
        {
            DbConnection.Close();
        }

        public void init(bool v)
        {
            throw new NotImplementedException();
        }
    }
}
