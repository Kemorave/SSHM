namespace Self_service_hotel_managment.View.Dialog
{
    /// <summary>
    /// Interaction logic for NetworkProblemDialog.xaml
    /// </summary>
    public partial class NetworkProblemDialog : MahApps.Metro.Controls.Dialogs.CustomDialog
    {
        public static bool IsOpen { get; private set; }
        public NetworkProblemDialog()
        {
            IsOpen = true;
            Unloaded += NetworkProblemDialog_Unloaded;
            InitializeComponent();
        }

        private void NetworkProblemDialog_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            IsOpen = false;

        }
    }
}
