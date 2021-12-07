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
    /// Interaktionslogik für PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        private string _area = String.Empty;
        private string _passwordHash = String.Empty;
        public PasswordWindow(string area)
        {
            InitializeComponent();
            this._area = area;
            
        }

        private void _BTNabort_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void _BTNok_Click(object sender, RoutedEventArgs e)
        {
            _passwordHash = Helper.encryptPwd(_password.Password);
            switch(_area)
            {
                case "SETTINGS":
                    if (MMBConfig.getInstance().SettingsPwd.Equals(_passwordHash))
                    {
                        SettingsWindow sw = new SettingsWindow();
                        sw.Show();
                        this.Close();
                    } else
                    {
                        LogService.getInstance().create(LogService._ERROR, "Anmeldung fehlgeschlagen", "Einstellungen");
                    }
                    break;
                case "MMBSETTINGS":
                    if (MMBConfig.getInstance().MMBPwd.Equals(_passwordHash))
                    {
                        MMBSettings msw = new MMBSettings();
                        msw.Show();
                        this.Close();
                    }
                    else
                    {
                        LogService.getInstance().create(LogService._ERROR, "Anmeldung fehlgeschlagen", "MMBSettings");
                    }
                    break;
                default:
                    break;
            } 
        }



    }
}
