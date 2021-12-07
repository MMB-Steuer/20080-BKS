using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Security.Permissions;
using System.Threading;
using System.Windows;
using System.Xml;

namespace MMBServer
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Thread for SocketServer
        SocketServer _socketServer;
        ODBCConnector _connector;
        Thread __socketServer = null;
        public MainWindow()
        {
            InitializeComponent();
            // init DB Connection
            _connector = ODBCConnector.getInstance();
            _socketServer = SocketServer.getInstance();
            this.startSocketServer();
            MMBConfig _config = MMBConfig.getInstance();
            

        }

        private void startSocketServer()
        {
            if (this.__socketServer == null)
                __socketServer = new Thread(this._socketServer.listen);
            this.__socketServer.Start();
            this.checkServerStatus();
        }
        private void checkServerStatus()
        {
            if(socketServerStateIsActive())
            {
                this._btnServerStart.Visibility = Visibility.Hidden;
                this._btnServerStop.Visibility = Visibility.Visible;
            }
            else
            {
                this._btnServerStart.Visibility = Visibility.Visible;
                this._btnServerStop.Visibility = Visibility.Hidden;
            }
        }

        private bool socketServerStateIsActive() {
            if (__socketServer != null && __socketServer.IsAlive)
            {
                return true;
            }
            else
            {
            return false;
            }
        }


        private void _btnOption_Click(object sender, RoutedEventArgs e)
        {
            if (socketServerStateIsActive())
            {
                MessageBox.Show("Server muss beendet werden");
            }else
            {
                PasswordWindow pw = new PasswordWindow("SETTINGS");
                pw.Show();
            }
        }

        private void _btnMMB_Click(object sender, RoutedEventArgs e)
        {
            if (socketServerStateIsActive())
            {
                MessageBox.Show("Server muss beendet werden");
            }
            else
            {
                PasswordWindow pw = new PasswordWindow("MMBSETTINGS");
                pw.Show();
            }
        }

        private void _btnServerStart_Click(object sender, RoutedEventArgs e)
        {
            LogService.getInstance().create(LogService._INFORMATION, "TcpIpSocketServer start by user", "");
            this.startSocketServer();
        }
        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        private void _btnServerStop_Click(object sender, RoutedEventArgs e)
        {
            LogService.getInstance().create(LogService._INFORMATION, "TcpIpSocketServer closed by user", "");
            this._socketServer.dispose();
            this.__socketServer.Abort();
            this.__socketServer = null;
            this.checkServerStatus();
        }
    }
}
