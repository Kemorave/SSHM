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
using MahApps.Metro.Controls.Dialogs;

namespace Self_service_hotel_managment.View.Dialog
{
    /// <summary>
    /// Interaction logic for PreviewCameraDevice.xaml
    /// </summary>
    public partial class PreviewCameraDevice : CustomDialog
    {
        public PreviewCameraDevice(Camera camera)
        {
            InitializeComponent();
            CamNameTB.Text = camera.Name;
            VCE.VideoCaptureSource = camera.Name;
        }

        private async void End(object sender, RoutedEventArgs e)
        {
            await WinPace.Helper.MahDialogHelper.CloseDialogAsync(this);
        }
    }
}