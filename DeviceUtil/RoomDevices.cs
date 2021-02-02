
using System;
using System.Collections.Generic;
using DeviceUtil.Recognition;
using HMSDataRepo.Model;
using Newtonsoft.Json;

namespace DeviceUtil
{
    public class RoomDevices : ICloneable, IDBModel
    {
        public RoomDevices()
        { }
        public int Room_Id { get; set; }
        public string Camera_Path { get; set; }

        [JsonIgnore]
        public string DoorHandel_Path { get; set; }
        [JsonIgnore]
        public Camera Camera { get { if (_Cam == null) { _Cam = Util.GetCamera(Camera_Path); } return _Cam; } }
        [JsonIgnore]
        public LiveRecognitionUnit LiveRecognitionUnit => GetLRU();


        [System.Configuration.SettingsDescription("None data store")]
        public int ID { get; set; }
        private LiveRecognitionUnit live;
        private Camera _Cam;

        private LiveRecognitionUnit GetLRU()
        {
            if (live == null && Camera_Path != null)
            {
                ZXing.BarcodeReader br = new ZXing.BarcodeReader() { AutoRotate = true };
                br.Options.TryHarder = true;
                br.Options.PureBarcode = false;
                br.Options.PossibleFormats = new List<ZXing.BarcodeFormat>() { ZXing.BarcodeFormat.QR_CODE };
                live = LiveRecognitionUnit.Create(Room_Id, Camera.Name, br);
                live.BeginInit();
                live.EndInit();
                live.Start();
                //live.IsActive = true;
            }
            return live;
        }

        public object Clone()
        {
            return new RoomDevices() { Camera_Path = this.Camera_Path, Room_Id = this.Room_Id };
        }
    }
}