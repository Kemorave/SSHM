using System.Collections.Generic;
using System.Threading.Tasks;
using DeviceUtil;
using HMSDataRepo.Model;
using MahApps.Metro.Controls.Dialogs;
using static WinPace.Helper.MahDialogHelper;

namespace Self_service_hotel_managment.View.Dialog
{
    /// <summary>
    /// Interaction logic for RoomCamerInfoDialog.xaml
    /// </summary>
    public partial class RoomCamerInfoDialog : CustomDialog
    {

        public RoomCamerInfoDialog(RoomDevices RoomDevices, IList<int> rooms)
        {
            RoomsList = rooms;
            this.RoomDevices = RoomDevices;
           // RoomDevices.LiveRecognitionUnit.Stop();

            InitializeComponent();
        }


        public RoomCamerInfoDialog(Camera RoomDevices, IList<int> rooms)
        {
            this.RoomDevices = new RoomDevices() { Camera_Path = RoomDevices.DevicePath };
            this.RoomsList = rooms;
            InitializeComponent();
        }

        public static async Task<RoomDevices> ShowEditDialog(RoomDevices RoomDevices, IList<int> rooms)
        {
            RoomCamerInfoDialog d = new RoomCamerInfoDialog(RoomDevices, rooms);
            await d.OpenDialogAsync();
            return d.RoomDevices;
        }
        public static async Task<RoomDevices> ShowAddDialog(Camera RoomDevices, IList<int> rooms)
        {
            RoomCamerInfoDialog d = new RoomCamerInfoDialog(RoomDevices, rooms);
            await d.OpenDialogAsync();
            return d.RoomDevices;
        }
        public IList<int> RoomsList { get; private set; }
        public RoomDevices RoomDevices { get; private set; }

        private async void Cancel(object sender, System.Windows.RoutedEventArgs e)
        {
            RoomDevices = null;
            RoomsList = null;
            await this.CloseDialogAsync();
        }

        private async void Confirm(object sender, System.Windows.RoutedEventArgs e)
        {
            if (RoomsSelector.SelectedIndex < 0)
            {
                   RoomDevices = null;
                RoomsList = null;
             await WinPace.Helper.MahDialogHelper.ShowMessage("No room selected", "A room must be selected to continue");
            }
            await this.CloseDialogAsync();
        }
    }
}