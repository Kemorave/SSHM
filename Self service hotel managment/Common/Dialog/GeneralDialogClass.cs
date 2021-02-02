using GalaSoft.MvvmLight.Messaging;
using static WinPace.Helper.MahDialogHelper;

namespace Self_service_hotel_managment.Common.Dialog
{
    public class GeneralDialogClass
    {
        public const string ShowNoNetworkDialogTokken = "ShowNoNetworkDialogTokken";
        public const string ShowErrorDialogTokken = "ShowErrorDialogTokken";
        public const string GeneralDialogClassTokken = "GeneralDialogClass";
        GeneralDialogClass()
        {
            MS = GalaSoft.MvvmLight.Messaging.Messenger.Default;
            MS.Register<string>(this, GeneralDialogClassTokken, ShowDialog);
        }
        public async void ShowDialog(string obj)
        {
            switch (obj)
            {
                case ShowNoNetworkDialogTokken: { if (!View.Dialog.NetworkProblemDialog.IsOpen) await new View.Dialog.NetworkProblemDialog().OpenDialogAsync(); break; }
                //case ShowErrorDialogTokken: {await new View.Dialog.NetworkProblemDialog().OpenDialogAsync(); break; }
                default:
                    break;
            }
        }

        public static GeneralDialogClass Instance { get; } = new GeneralDialogClass();
        public IMessenger MS { get; }
    }
}