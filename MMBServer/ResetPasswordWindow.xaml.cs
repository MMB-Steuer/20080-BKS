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
    /// Interaktionslogik für ResetPasswordWindow.xaml
    /// </summary>
    public partial class ResetPasswordWindow : Window
    {
        public static string _MMBPWD = "MMBPW";
        public static string _BKSPWD = "BKSPW";
        private string type = String.Empty;
        public ResetPasswordWindow(string typ)
        {
            InitializeComponent();
            type = typ;
        }

        private void _BTNabort_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void _BTNok_Click(object sender, RoutedEventArgs e)
        {
            string _passwordHash = Helper.encryptPwd(_password.Password);
            if (this._newpassword.Password.Equals(this._newpasswordA.Password) == false)
            {
                LogService.getInstance().create(LogService._ERROR, "Wiederholung nicht identisch", "Change Password: " + type );
            }else if(this._newpassword.Password.Trim().Length <= 0) {
                LogService.getInstance().create("Error", "nicht genug Zeichen");
            }
            else
            {
                MMBConfig _config = MMBConfig.getInstance();
                if (type == _MMBPWD) { 
                    if (MMBConfig.getInstance().MMBPwd.Equals(_passwordHash))
                    {
                        _config.MMBPwd = Helper.encryptPwd(_newpassword.Password);
                        LogService.getInstance().create(LogService._INFORMATION, "Password geändernt", type);
                        this.Close();
                    }
                    else
                    {
                        LogService.getInstance().create(LogService._ERROR, "Password falsch", "Passwort ändnern - falsches Passwort: " + type + " " + _password.Password);
                    }
                }
                else if(type == _BKSPWD) {
                    if (Helper.encryptPwd(_password.Password).Equals(_config.SettingsPwd) == false)
                    {
                        LogService.getInstance().create(LogService._ERROR, "Password falsch", "Passwort ändnern - falsches Passwort: " + type);
                    }
                    else
                    {
                        _config.SettingsPwd = Helper.encryptPwd(_newpassword.Password);
                        LogService.getInstance().create(LogService._INFORMATION, "Password geändernt", type);
                        this.Close();
                    }
                }
                else
                {
                    LogService.getInstance().create(LogService._ERROR, "Es ist ein interner Fehler aufgetretten", "Password ändern - Kein Typ angegeben ");
                } 
            } 
        } 
    }
}
