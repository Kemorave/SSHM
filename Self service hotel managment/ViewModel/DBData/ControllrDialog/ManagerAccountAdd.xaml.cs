using HMSDataRepo.Model; 
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using static WinPace.Helper.MahDialogHelper;

namespace Self_service_hotel_managment.ViewModel.DBData.ControllrDialog
{
    /// <summary> 
    /// Interaction logic for ManagerAccountAdd.xaml 
    /// </summary> 
    public partial class ManagerAccountAdd : MahApps.Metro.Controls.Dialogs.CustomDialog
    {
        public ManagerAccountAdd()
        {
            this.DataContext = new People();
            InitializeComponent();
          //  GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<Tuple<People, System.Net.HttpStatusCode>>(this, ViewModel.DBData.ControllerVM.ManagersControllerVM.SuccessTokken, Check, true);
        }

        private void Check(Tuple<People, HttpStatusCode> obj)
        {
            if (obj.Item2 != HttpStatusCode.OK && obj.Item1 == this.DataContext)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(Common.Dialog.GeneralDialogClass.ShowNoNetworkDialogTokken, Common.Dialog.GeneralDialogClass.GeneralDialogClassTokken);

            }
        }



        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as People).Password = PasswordBox.Password;
            (DataContext as People).AccountType = "Manager";
           // GalaSoft.MvvmLight.Messaging.Messenger.Default.Send((DataContext as People), ControllerVMBase<People, DataGridClipboardCopyMode>.AddEntityTokken);
           await this.CloseDialogAsync();
        }
    }
}