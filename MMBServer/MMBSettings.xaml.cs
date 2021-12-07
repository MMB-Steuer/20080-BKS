using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MMBServer
{
    /// <summary>
    /// Interaktionslogik für MMBSettings.xaml
    /// </summary>
    public partial class MMBSettings : Window
    {
        MMBConfig _config;
        public MMBSettings()
        {
            InitializeComponent();
            _config = MMBConfig.getInstance();
            this.getConfigData();
        }

        private void getConfigData()
        {
            this._XMLServerAdress.Text = _config.XMLServerAdress;
            this._XMLServerPort.Text = _config.XMLSeverPort.ToString();
            this._XMLVersion.Text = _config.XMLVersion.ToString();
            this._UserID.Text = _config.UserID.ToString();
            this._ProductionID.Text = _config.ProductionID;
            this._XValue.Text = _config.XValue.ToString();
            this._KommunikationsUser.Text = _config.KommunikationUser;
            this._KommunikationsUserPassword.Text = _config.KommunikationPassword;

            this._DevelopeMode.IsChecked = _config.IsDevelopeMode;
            this._tcpServerAdress.Text = _config.TcpSeverAdress;
            this._tcpServerPort.Text    = _config.TcpPort.ToString();
            this._odbcDSN.Text  = _config.OdbcDNS.ToString();

            this._ODBCCredentialsRequired.IsChecked = _config.OdbcUserRequired;
            this._odbcUser.Text = _config.OdcbUser.ToString();
            this._odbcPwd.Text = _config.OdbcPwd.ToString();

        }
        private void showODBCUser()
        {
            if (_ODBCCredentialsRequired.IsChecked == true)
            {
                this._odbcUser.Visibility = Visibility.Visible;
                this._odbcPwd.Visibility= Visibility.Visible;
                this._label_odbcPwd.Visibility = Visibility.Visible;
                this._label_odbcUser.Visibility = Visibility.Visible;  
            } else
            {
                this._odbcUser.Visibility = Visibility.Hidden;
                this._odbcPwd.Visibility = Visibility.Hidden;
                this._label_odbcPwd.Visibility = Visibility.Hidden;
                this._label_odbcUser.Visibility = Visibility.Hidden;
            }

        }

        private void saveConfigData()
        {
            _config.XMLServerAdress = this._XMLServerAdress.Text;
            _config.XMLSeverPort = int.Parse(this._XMLServerPort.Text);
            _config.XMLVersion = int.Parse(this._XMLVersion.Text);
            _config.UserID = this._UserID.Text;
            _config.ProductionID = this._ProductionID.Text;
            _config.XValue = this._XValue.Text;
            _config.KommunikationUser = this._KommunikationsUser.Text;
            _config.KommunikationPassword = this._KommunikationsUserPassword.Text;
            _config.IsDevelopeMode = (this._DevelopeMode.IsChecked == true);
            _config.TcpSeverAdress = this._tcpServerAdress.Text;
            _config.TcpPort = this._tcpServerPort.Text;
            _config.OdbcDNS = this._odbcDSN.Text;
            _config.OdbcPwd = this._odbcPwd.Text;
            _config.OdcbUser = this._odbcUser.Text;
            
            this.getConfigData();
        }
        // enable / disable DevelopeMode
        private void _DevelopeMode_Click(object sender, RoutedEventArgs e)
        {

        }
        // open Log Window 
        private void _BTNLogs_Click(object sender, RoutedEventArgs e)
        {

        }
        // Enable Disable ODBC User / Password requirement
        private void _ODBCCredentialsRequired_Click(object sender, RoutedEventArgs e)
        {
            this.showODBCUser();
        }



        // close Window
        private void _BTN_abort_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // open Customer Password Change Window
        private void _BTN_SettingsPwdChange_Click(object sender, RoutedEventArgs e)
        {
            ResetPasswordWindow RPW = new ResetPasswordWindow(ResetPasswordWindow._BKSPWD);
            RPW.Show();
        }
        // open MMB Password reset Window
        private void _BTN_changeMMBPwd_Click(object sender, RoutedEventArgs e)
        {
            ResetPasswordWindow RPW = new ResetPasswordWindow(ResetPasswordWindow._MMBPWD);
            RPW.Show();
        }
        // Submit Changes
        private void _BTN_submit_Click(object sender, RoutedEventArgs e)
        {
            this.saveConfigData();
            LogService.getInstance().create(LogService._INFORMATION, "Einstellungen gespeichert", "MMB Einstellungen");
            this.Close();
        }


    }
}
