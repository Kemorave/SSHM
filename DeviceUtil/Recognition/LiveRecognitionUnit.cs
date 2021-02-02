using System.Drawing;
using System.Threading;
using WPFMediaKit.DirectShow.MediaPlayers;
using ZXing;

namespace DeviceUtil.Recognition
{

    public class LiveRecognitionUnit : System.ComponentModel.ISupportInitialize
    {
        ~LiveRecognitionUnit()
        {
            LiveUnitsDispatched--; try
            {
                VideoElement.Dispatcher.Invoke(() =>
            {
                try
                {
                    VideoElement.EnableSampleGrabbing = false;
                    VideoElement.Stop();
                }
                catch
                {

                }
            });
            }
            catch
            {

            }
        }

        protected RecognitionResult _RecognitionResult = new RecognitionResult();

        private bool _IsActive;

        public static event System.EventHandler<RecognitionResult> ResultFound;

        public static int LiveUnitsDispatched { get; protected set; }

        public static LiveRecognitionUnit Create(int key, string cam, IBarcodeReader barcodeReader)
        {
            return new LiveRecognitionUnit(key, cam, barcodeReader);
        }

        private LiveRecognitionUnit(int key, string cam, IBarcodeReader barcodeReader)
        {
            LiveUnitsDispatched++;
            _RecognitionResult = new RecognitionResult
            {
                Key = key,
                CameraName = DeviceUtil.Util.GetCamera(cam).Name,
                CameraPath = cam
            };
            Key = key;
            BarcodeReader = barcodeReader;
            barcodeReader.ResultFound += BarcodeReader_ResultFound;
        }

        private void BarcodeReader_ResultFound(Result obj)
        {
            if (obj != null)
            {
                _RecognitionResult.Result = obj;
                ResultFound?.Invoke(this, _RecognitionResult);
            }
        }

        public virtual void ProcessFrame(Bitmap VideoFrame)
        {
            if (IsProcessing)
            {
                return;
            }
            if (VideoFrame != null)
            {
                IsProcessing = true;
                //lock (this)
                {
                    try
                    {
                        BarcodeReader.Decode(VideoFrame);
                    }
                    catch
                    { 
                    }
                    finally
                    {
                        System.Threading.Tasks.Task.Run(() =>
                        {
                            Thread.Sleep(Delay);
                            IsProcessing = false;
                        });
                    }
                }
            }
        }

        public void BeginInit()
        {
            VideoElement = new WPFMediaKit.DirectShow.Controls.VideoCaptureElement();

            VideoElement.BeginInit();
            VideoElement.VideoCaptureSource = this._RecognitionResult.CameraPath;
            VideoElement.Stretch = System.Windows.Media.Stretch.Fill;
            VideoElement.DesiredPixelHeight = 400;
            VideoElement.DesiredPixelWidth = 400;
             VideoElement.Loaded += VideoElement_Loaded;
            VideoElement.LoadedBehavior = WPFMediaKit.DirectShow.MediaPlayers.MediaState.Manual;
            VideoElement.UnloadedBehavior = WPFMediaKit.DirectShow.MediaPlayers.MediaState.Manual;
            // VideoElement. 
            VideoElement.EnableSampleGrabbing = true;

            VideoElement.NewVideoSample += OnNewVideoSample;
        }

        private void VideoElement_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Start();
        }

        public void EndInit()
        {
            VideoElement.EndInit();
            Start();
        }

        public void Start()
        {
            IsActive = true;
        }
        public void Stop()
        {
            IsActive = false;
        }

        protected virtual void OnNewVideoSample(object sender, VideoSampleArgs e)
        {
            if (!IsProcessing)
            {
                ProcessFrame(e.VideoFrame);
            }
        }

        public bool IsActive
        {
            set
            {
                if (value)
                {
                    VideoElement?.Play();
                }
                else
                {
                    VideoElement?.Stop();
                }
                _IsActive = value;
            }
            get => _IsActive;
        }

        public int Delay { get; set; } = 100;

        public WPFMediaKit.DirectShow.Controls.VideoCaptureElement VideoElement { get; private set; }

        public int Key { get => _RecognitionResult.Key; set => _RecognitionResult.Key = value; }

        public IBarcodeReader BarcodeReader { get; }

        public bool IsProcessing { get; protected set; }

    }
}