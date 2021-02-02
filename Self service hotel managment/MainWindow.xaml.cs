using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Self_service_hotel_managment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            App.Current.Exit += Current_Exit;
            Task.Run(  () => {
                Thread.Sleep(2000);
                Connect();
            });
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {

                TaskbarIcon?.Dispose();
            }
            catch 
            {
                 
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            WindowState = WindowState.Minimized;
            Hide();
        }
        private void Connect()
        {
            Task.Run(async () => {
                var res = await VM.AppData.Instance.AutoLoginAsync();
                Dispatcher.Invoke(() => {
                    if (res == true)
                    {
                        RetryButton.Visibility = Visibility.Collapsed;
                        this.Content = new View.MainView();
                    }
                    else if (res == false)
                    {
                        RetryButton.Visibility = Visibility.Visible;
                    }
                    else if (res == null)
                    {
                        RetryButton.Visibility = Visibility.Collapsed;
                        this.Content = new View.Login();
                    }
                });
            });
        }

        private void Retry(object sender, RoutedEventArgs e)
        {

            RetryButton.Visibility = Visibility.Collapsed;
            Connect();
        }
    }
}
