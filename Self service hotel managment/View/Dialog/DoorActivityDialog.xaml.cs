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

namespace Self_service_hotel_managment.View.Dialog
{
    /// <summary>
    /// Interaction logic for DoorActivityDialog.xaml
    /// </summary>
    public partial class DoorActivityDialog : MahApps.Metro.Controls.Dialogs.CustomDialog
    {
        public DoorActivityDialog(People person,Reservations reservations,string camera)
        {
            Person = person;
            Reservation = reservations;
            Camera = camera;
            InitializeComponent();
        }

        public People Person { get; }
        public Reservations Reservation { get; }
        public string Camera { get; }
    }
}
