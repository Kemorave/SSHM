using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace Self_service_hotel_managment
{
    public static class TrayCommands
    {
        static TrayCommands()
        {
            CloseApplicationCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(CloseApplication);
            OpenWindowCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OpenWindow);
            LogoutCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(Logout);
        }

        private static void Logout()
        {
            try
            {
                Services.LocalData.DeleteLoginInfo();
                CloseApplication();
            }
            catch (Exception)
            {

            }
        }

        private static void CloseApplication()
        {
            Environment.Exit(Environment.ExitCode);
        }

        [DllImport("user32")] public static extern int FlashWindow(IntPtr hwnd, bool bInvert);

        private static void OpenWindow()
        {
            var Window = App.Current.MainWindow;


            if (!Window.IsVisible)
            {
                Window.Show();
            }

            if (Window.WindowState == WindowState.Minimized)
            {
                Window.WindowState = WindowState.Normal;
            }
            WindowInteropHelper wih = new WindowInteropHelper(Window);
            FlashWindow(wih.Handle, true);

            //Window.Activate();
            //Window.Topmost = true;  // important
            //Window.Topmost = false; //
        }
        public static GalaSoft.MvvmLight.CommandWpf.RelayCommand LogoutCommand { get; }
        public static GalaSoft.MvvmLight.CommandWpf.RelayCommand CloseApplicationCommand { get; }
        public static GalaSoft.MvvmLight.CommandWpf.RelayCommand OpenWindowCommand { get; }
    }
}