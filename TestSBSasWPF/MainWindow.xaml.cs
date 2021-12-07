using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace TestSBSasWPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread __client = null;
        private SocketClient _client;
        private ObservableCollection<TCPPackeItem> list = new ObservableCollection<TCPPackeItem>();

        public SocketClient Client { get => _client; set => _client = value; }
        private System.Timers.Timer timer = new System.Timers.Timer(1000);
        
        public MainWindow()
        {
            InitializeComponent();
            refreshList();
            _client = new SocketClient();
            getDataFromClient();
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

        }
        private void getDataFromClient()
        {
            _serverAddr.Text = _client._serverAddr;
            _serverPort.Text = _client._port;
            _uid.Text = _client._uid;
            _count.Text = _client._count.ToString();
            _personnumber.Text = _client._personalNummer;
            _personalPassword.Text = _client._personalPassword;
        }
        private void setDataToClient()
        {
            
            _client._serverAddr = _serverAddr.Text;
            _client._port = _serverPort.Text;
            _client._uid = _uid.Text;
            _client._personalNummer = _personnumber.Text;
            _client._personalPassword = _personalPassword.Text; 
        }
        private bool checkSockentClientIsRunning()
        {
            return true;
        }
        private void _btnServerStart_Click(object sender, RoutedEventArgs e)
        {
            _btnServerStart.Visibility = Visibility.Hidden;
            __client = new Thread(_client.start);
            __client.Start();
            _btnServerStop.Visibility = Visibility.Visible;
            timer.Start();
        }
        private void _btnServerStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            _btnServerStop.Visibility=Visibility.Hidden;
            _client.dispose();
            __client.Abort();
            _btnServerStart.Visibility=Visibility.Visible;
        }

        private void _serverAddr_TextChanged(object sender, TextChangedEventArgs e)
        {
            _client._serverAddr = _serverAddr.Text;
        }

        private void _serverPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            _client._port = _serverPort.Text;
        }

        private void _uid_TextChanged(object sender, TextChangedEventArgs e)
        {
            _client._uid = _uid.Text;
        }

        private void _count_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void _personnumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            _personnumber.Text = _client._personalNummer;
        }

        private void _personalPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            _client._personalPassword = _personalPassword.Text;
        }

        private void refreshList() {
            list.Add(new TCPPackeItem());
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            this.list = new ObservableCollection<TCPPackeItem>( new List<TCPPackeItem>(_client.respondList));
            //            _DataGrid.ItemsSource = this.list;
            //_DataGrid.ItemsSource = null;
            refreshList();
            Console.WriteLine("Timer over");
            //getDataFromClient();
        }
    }
}
