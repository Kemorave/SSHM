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
using MahApps.Metro.Controls.Dialogs;
using WinPace.Helper;

namespace Self_service_hotel_managment.View.Dialog
{
    /// <summary>
    /// Interaction logic for RoomDialog.xaml
    /// </summary>
    public partial class RoomDialog : CustomDialog
    {
        private bool _add;
        /// <summary>
        /// Add room
        /// </summary>
        public RoomDialog()
        {
            _add = true;
            Room = new Rooms();
            DataContext = Room;
            InitializeComponent();
        }
        /// <summary>
         /// Edit room
         /// </summary>
         /// <param name="room"></param>
        public RoomDialog(Rooms room)
        {
            Room = room;
            DataContext = Room;
            InitializeComponent();
        }
        public Rooms Room { get; private set; }

        private async void StartTask(object sender, RoutedEventArgs e)
        {
            try
            {
                 
                    await this.CloseDialogAsync(); 
            }
            catch (Exception)
            {

            }

        }
        public static async Task<Rooms> ShowEditDialog(Rooms RoomDevices)
        {

            RoomDialog d = new RoomDialog(RoomDevices);
            await d.OpenDialogAsync();
            return d.Room;
        }
        public static async Task<Rooms> ShowAddDialog()
        {
            RoomDialog d = new RoomDialog();
            await d.OpenDialogAsync();
            return d.Room;
        }

        private async void Cancel(object sender, RoutedEventArgs e)
        {
            Room = null;
            await this.CloseDialogAsync();
        }
    }
}