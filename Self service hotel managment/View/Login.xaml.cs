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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HMSDataRepo.Model;

namespace Self_service_hotel_managment.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }

      
        private async void TryLogin(object sender, RoutedEventArgs e)
        {
            if (await VM.AppData.Instance.Login(EmailBox.Text,People.HashString(PassBox.Password))==true)
            {
                App.Current.MainWindow.Content = new MainView();
            }
        }
    }
}
