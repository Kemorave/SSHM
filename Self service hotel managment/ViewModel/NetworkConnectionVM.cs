using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace Self_service_hotel_managment.ViewModel
{
    [Notify]
    public class NetworkConnectionVM : INotifyPropertyChanged
    {
        public const string AvailableNetworkTokken = "On";
        public const string UnavailableNetworkTokken = "Off";
        public const string CheckNetworkTokken = "Off";
        private readonly DispatcherTimer Timer = new DispatcherTimer();
        private int PastSeconds, ReconnectSeconds, RetryCount;
        [NonNotify]
        public static NetworkConnectionVM Instance { get; } = new NetworkConnectionVM();
        public NetworkConnectionVM()
        {
            IsEnabled = true;
            HMSDataRepo.WebApiConfig.WebActivity += this.CloudControllerBase_WebActivity; ;
            ReconnectCommand = new KemoTools.Commands.Command(ForceReconnect);
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            Timer.Tick += Timer_Tick;
            Timer.Start();
            MessengerInstance = GalaSoft.MvvmLight.Messaging.Messenger.Default;
            MessengerInstance.Register<string>(this, CheckNetworkTokken, CheckNetwork);
            CheckNetwork(CheckNetworkTokken);
        }

        private void CloudControllerBase_WebActivity(object sender, HMSDataRepo.Controllers.WebActivityResult e)
        {
            if (IsConnectionAvailable&&e.IsSuccesfull)
            {
                return;
            }
            CheckNetwork(CheckNetworkTokken);
        }

     
 

        private void ForceReconnect()
        {

            IsReconnecting = false;
            PastSeconds = 0;
            RetryCount = 0;
            ReconnectSeconds = 0;
            CheckNetwork(CheckNetworkTokken);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!IsEnabled)
            {
                return;
            }
            if (RetryCount >= 2)
            {
                IsReconnecting = false;
                Timer.Stop();
                Message = "Faild to connect";
                if (!IsConnectionAvailable)

                    Common.Dialog.GeneralDialogClass.Instance.ShowDialog(Common.Dialog.GeneralDialogClass.ShowNoNetworkDialogTokken);
            }
            if (IsConnectionAvailable)
            {
                CheckNetwork(CheckNetworkTokken);
                return;
            }

            if (ReconnectSeconds > 0 && IsReconnecting)
            {
                Message = "Reconnecting to API in " + ReconnectSeconds;
            }

            ReconnectSeconds--;

            if (ReconnectSeconds <= 0)
            {
                RetryCount++;
                CheckNetwork(CheckNetworkTokken);
            }
        }

        private void CheckNetwork(string obj)
        {
            if (!IsEnabled)
            {
                return;
            }
            if (CheckNetworkTokken.Equals(obj, StringComparison.Ordinal))
            {
                if (IsReconnecting)
                {
                    return;
                }
                IsReconnecting = true;
                System.Threading.Tasks.Task.Run(() =>
                {
                    Message = "Reconnecting ... ";

                    IsConnectionAvailable = HMSDataRepo.WebApiConfig.CheckConnection();
                    if (IsConnectionAvailable)
                    {
                        Message = "Connected to API";

                        Timer.Stop();
                        Timer.Interval = TimeSpan.FromSeconds(20);
                        Timer.Start();
                        PastSeconds = 0;
                        RetryCount = 0;
                        ReconnectSeconds = 0;
                    }
                    else
                    {
                        Timer.Stop();
                        Timer.Interval = TimeSpan.FromSeconds(1);
                        Timer.Start(); if (PastSeconds < 60)
                        {
                            PastSeconds += 5;
                        }

                        ReconnectSeconds = PastSeconds;
                    }
                    IsReconnecting = false;


                });
            }
        }

        public void Cleanup()
        {
        }

        private void ForceStop()
        {
            IsReconnecting = false;
           // Message = "Stopped";
            Timer.Stop();
        }

        public bool IsEnabled { get => isEnabled; set { if (!value) { ForceStop(); } else { CheckNetwork(CheckNetworkTokken); } SetProperty(ref isEnabled, value, isEnabledPropertyChangedEventArgs); } }


        public KemoTools.Commands.Command ReconnectCommand { get => reconnectCommand; private set => SetProperty(ref reconnectCommand, value, reconnectCommandPropertyChangedEventArgs); }
        public string Message { get => message; private set { if (IsEnabled) { SetProperty(ref message, value, messagePropertyChangedEventArgs); } } }
        public bool IsConnectionAvailable { get => isConnectionAvailable; private set => SetProperty(ref isConnectionAvailable, value, isConnectionAvailablePropertyChangedEventArgs); }
        public bool IsReconnecting { get => isReconnecting; private set => SetProperty(ref isReconnecting, value, isReconnectingPropertyChangedEventArgs); }
        [NonNotify]
        public IMessenger MessengerInstance { get; }

        #region NotifyPropertyChangedGenerator

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isEnabled;
        private static readonly PropertyChangedEventArgs isEnabledPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(IsEnabled));
        private KemoTools.Commands.Command reconnectCommand;
        private static readonly PropertyChangedEventArgs reconnectCommandPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(ReconnectCommand));
        private string message;
        private static readonly PropertyChangedEventArgs messagePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Message));
        private bool isConnectionAvailable;
        private static readonly PropertyChangedEventArgs isConnectionAvailablePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(IsConnectionAvailable));
        private bool isReconnecting;
        private static readonly PropertyChangedEventArgs isReconnectingPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(IsReconnecting));

        private void SetProperty<T>(ref T field, T value, PropertyChangedEventArgs ev)
        {
            if (!System.Collections.Generic.EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, ev);
            }
        }

        #endregion
    }
}