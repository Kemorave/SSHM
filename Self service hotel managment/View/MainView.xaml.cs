using System.Windows.Controls;

namespace Self_service_hotel_managment.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Page
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void HMenu_ItemInvoked(object sender, MahApps.Metro.Controls.HamburgerMenuItemInvokedEventArgs e)
        {
           // HMenu.Content = (e.InvokedItem as MahApps.Metro.Controls.HamburgerMenuItem)?.Tag;
        }
    }
}
