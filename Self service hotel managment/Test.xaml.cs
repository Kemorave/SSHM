using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Self_service_hotel_managment
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : MahApps.Metro.Controls.MetroWindow, INotifyPropertyChanged
    {
        public Test()
        {
                StartCommand = new KemoTools.Commands.Command(Start);
            CancelCommand = new KemoTools.Commands.Command(Cancel);
        InitializeComponent();
        }

        private void Cancel()
        {
            _isCanceled = true;
        }

        private void Start()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            _isCanceled = false;
            Task.Run(() =>{
                for (int i = 0; i != 100; i++)
                {
                    Thread.Sleep(100);
                    if (_isCanceled)
                    {
                        break;
                    }
                    Progress = i;
                }
                Progress = 0;
                IsBusy = false;
            });
        }

        private int progress;
        private bool isBusy;
        private bool _isCanceled;

        public event PropertyChangedEventHandler PropertyChanged;
        public KemoTools.Commands.Command StartCommand { get; private set; }
        public KemoTools.Commands.Command CancelCommand { get; private set; }
        public bool IsBusy
        {
            get => isBusy;
            set { isBusy = value; ReportChange(); }
        }

        public int Progress
        {
            get => progress;
            set { progress = value; ReportChange(); }
        }

        private void ReportChange([System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Viewport3D_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}