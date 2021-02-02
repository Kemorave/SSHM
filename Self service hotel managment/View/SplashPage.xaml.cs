using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Self_service_hotel_managment.View
{
    /// <summary>
    /// Interaction logic for SplashPage.xaml
    /// </summary>
    public partial class SplashPage : Page
    {
        public SplashPage()
        {
            KeepAlive = true;
            InitializeComponent();
            var lp = new View.LoginPage();
            lp.Return += Lp_Return;

            Task.Run(() => {
                Thread.Sleep(3000);
                Dispatcher.Invoke(() => {
                    NavigationService?.RemoveBackEntry();
                    NavigationService?.Navigate(lp);
                });
            });
        }

        private void Lp_Return(object sender, System.Windows.Navigation.ReturnEventArgs<HMSDataRepo.Model.People> e)
        {
            NavigationService.RemoveBackEntry();
            NavigationService.Navigate(new View.MainView());
        }
    }
}
