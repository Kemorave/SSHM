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
    /// Interaction logic for PeopleDialog.xaml
    /// </summary>
    public partial class PeopleDialog : CustomDialog
    {
        public People Model { get; private set; }

        public PeopleDialog()
        {

            InitializeComponent();
            Model = new People();
            DataContext = Model;
        }
        public PeopleDialog(People people)
        {
            InitializeComponent();
            Model = people.Clone() as People;
            DataContext = Model;
        }

        private async void Confirm(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password?.Length >= 8)
            {
                Model.Password = PasswordBox.Password;
                await this.CloseDialogAsync();
            }
            else
            {
                MessageBox.Show(App.Current.MainWindow, "Password too short");
            }
        }

        private async void Cancel(object sender, RoutedEventArgs e)
        {
            Model = null;
            await this.CloseDialogAsync();
        }
        public static async Task<People> ShowEditDialog(People RoomDevices)
        {

            PeopleDialog d = new PeopleDialog(RoomDevices);
            await d.OpenDialogAsync();
            return d.Model;
        }
        public static async Task<People> ShowAddDialog()
        {
            PeopleDialog d = new PeopleDialog();
            await d.OpenDialogAsync();
            return d.Model;
        }


    }
}