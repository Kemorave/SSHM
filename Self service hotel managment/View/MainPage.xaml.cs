using System;
using System.Collections.Generic;
using System.Drawing;
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
using MahApps.Metro.Controls;

namespace Self_service_hotel_managment.View
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        WPFMediaKit.DirectShow.Controls.VideoCaptureElement videoElement = new WPFMediaKit.DirectShow.Controls.VideoCaptureElement();

        public MainPage()
        {
            InitializeComponent();
            videoElement.BeginInit();

            // videoElement.DesiredPixelWidth = 400;
            //videoElement.DesiredPixelHeight = 400;
            //videoElement.LoadedBehavior = WPFMediaKit.DirectShow.MediaPlayers.MediaState.Manual;
            //videoElement.UnloadedBehavior = WPFMediaKit.DirectShow.MediaPlayers.MediaState.Manual;
            //videoElement.VideoCaptureSource = DevicesUtil.GetCameras().First().Name;
            //videoElement.EnableSampleGrabbing = true;

            //videoElement.NewVideoSample += VideoElement_NewVideoSample;
            //videoElement.EndInit();
            //videoElement.Play();
            ZXing.BarcodeWriter barcodeWriter = new ZXing.BarcodeWriter();
            barcodeWriter.Format = ZXing.BarcodeFormat.QR_CODE;
            ImageBox.Source = KemoTools.IO.Shell.ThumnailHelper.UnManagedBitmapToBitmapImage(barcodeWriter.Write("Hi there " + Environment.UserName), false);
            var br = new ZXing.BarcodeReader() { AutoRotate = true };
            br.Options.TryHarder = true;
            br.Options.PureBarcode = false;
            br.Options.PossibleFormats = new List<ZXing.BarcodeFormat>() { ZXing.BarcodeFormat.QR_CODE };

            DeviceUtil.Recognition.LiveRecognitionUnit liveRecognitionUnit = DeviceUtil.Recognition.LiveRecognitionUnit.Create(1,DeviceUtil.Util.GetCameras().First().DevicePath, br);
            DeviceUtil.Recognition.LiveRecognitionUnit.ResultFound += LiveRecognitionUnit_ResultFound;
            liveRecognitionUnit.BeginInit();
            liveRecognitionUnit.VideoElement.HorizontalAlignment = HorizontalAlignment.Stretch;
            liveRecognitionUnit.VideoElement.VerticalAlignment = VerticalAlignment.Stretch;
            liveRecognitionUnit.VideoElement.DesiredPixelHeight = 1400;
            liveRecognitionUnit.VideoElement.DesiredPixelWidth = 1400;
            liveRecognitionUnit.VideoElement.Margin = new Thickness(10);
            liveRecognitionUnit.EndInit();

            liveRecognitionUnit.Start();

            VidCont.Content = liveRecognitionUnit.VideoElement;
        }
        private void LiveRecognitionUnit_ResultFound(object sender, DeviceUtil.Recognition.RecognitionResult e)
        {
            Dispatcher.Invoke(() =>
            {
                Badge.Badge = null;
                Badge.Badge = e.Result?.Text;
            });
        }

        private void VideoElement_NewVideoSample(object sender, WPFMediaKit.DirectShow.MediaPlayers.VideoSampleArgs e)
        {
            Task.Run(() =>
            {
                if (Caplock == true)
                {
                    return;
                }
                // bitmap?.Dispose();
                bitmap = e.VideoFrame;
            });
        }

        private void VideoCaptureElement_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as WPFMediaKit.DirectShow.Controls.VideoCaptureElement).BeginInit();
            (sender as WPFMediaKit.DirectShow.Controls.VideoCaptureElement).EndInit();
        }
        Bitmap bitmap;

        public bool Caplock { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Caplock = true;
            if (bitmap != null)
            {
                Microsoft.Win32.SaveFileDialog fileDialog = new Microsoft.Win32.SaveFileDialog();
                if (fileDialog.ShowDialog() == true)
                {
                    //using (var ms = new System.IO.MemoryStream())
                    {
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                        bitmap.Save(fileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        //System.Drawing.Image.FromStream(ms).Save(fileDialog.FileName) ;
                    }
                }
            }
        }

        private void QRButton(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TargetBox.Text))
            {
                return;
            }
            ZXing.BarcodeWriter barcodeWriter = new ZXing.BarcodeWriter();
            barcodeWriter.Format = ZXing.BarcodeFormat.QR_CODE;
            ImageBox.Source = KemoTools.IO.Shell.ThumnailHelper.UnManagedBitmapToBitmapImage(barcodeWriter.Write(TargetBox.Text), false);

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await MahApps.Metro.Controls.Dialogs.DialogManager.ShowMetroDialogAsync(App.Current.MainWindow as MetroWindow, new ViewModel.DBData.ControllrDialog.ManagerAccountAdd());

        }

        private   void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // await MahApps.Metro.Controls.Dialogs.DialogManager.ShowMetroDialogAsync(this, new ViewModel.DBData.ControllrDialog.ManagerAccountEdit());
           // NavigationService.Navigate(new View.ManagersListView());
        }
    }
}
