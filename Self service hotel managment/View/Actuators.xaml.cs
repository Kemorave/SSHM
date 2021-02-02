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
using DeviceUtil;

namespace Self_service_hotel_managment.View
{
    /// <summary>
    /// Interaction logic for Actuators.xaml
    /// </summary>
    public partial class Actuators : UserControl
    {
        public Actuators()
        {
            InitializeComponent();
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as MenuItem)?.DataContext is Camera camera)
            {
                await WinPace.Helper.MahDialogHelper.OpenDialogAsync(new Dialog.PreviewCameraDevice(camera));
            }
        }
    }
}
