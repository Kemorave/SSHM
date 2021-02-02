using HMSDataRepo.Model; 
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Self_service_hotel_managment.ViewModel.DBData.ControllrDialog
{
    /// <summary>
    /// Interaction logic for ManagerAccountEdit.xaml
    /// </summary>
    public partial class ManagerAccountEdit : MahApps.Metro.Controls.Dialogs.CustomDialog
    {
        public ManagerAccountEdit()
        {
            this.DataContext = new People() { PersonName = "Hema", Email = "Homadirar@hotmail.com" };
            InitializeComponent();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<HttpStatusCode>(this, Check);
        }

        private void Check(HttpStatusCode obj)
        {
            if (obj != HttpStatusCode.OK)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(Common.Dialog.GeneralDialogClass.ShowNoNetworkDialogTokken, Common.Dialog.GeneralDialogClass.GeneralDialogClassTokken);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          //  GalaSoft.MvvmLight.Messaging.Messenger.Default.Send((DataContext as People), ControllerVMBase<People, DataGridClipboardCopyMode>.EditEntityTokken);
        }
    }
}