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
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        MMBConfig _config;
        public SettingsWindow()
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
        }

        private void saveConfigData() {
            _config.XMLServerAdress = this._XMLServerAdress.Text;
            _config.XMLSeverPort = int.Parse(this._XMLServerPort.Text);
            _config.XMLVersion = int.Parse(this._XMLVersion.Text);
            _config.UserID = this._UserID.Text;
            _config.ProductionID = this._ProductionID.Text;
            _config.XValue = this._XValue.Text;
            _config.KommunikationUser = this._KommunikationsUser.Text;
            _config.KommunikationPassword = this._KommunikationsUserPassword.Text;
        }

        private void _btn_abort_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void _btn_submit_Click(object sender, RoutedEventArgs e)
        {
            this.saveConfigData();
            LogService.getInstance().create(LogService._INFORMATION, "Einstellungen gespeichert", "Einstellungen");
            this.Close();
        }

        private void _resetPwd_Click(object sender, RoutedEventArgs e)
        {
            ResetPasswordWindow resetPasswordWindow = new ResetPasswordWindow(ResetPasswordWindow._BKSPWD);
            resetPasswordWindow.Show();
        }
    }
}
