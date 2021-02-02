using System;

namespace DeviceUtil
{
    public class Camera : ICloneable
    {
        public Camera()
        {

        }
        //[ColumnName]
        public string DevicePath { get; set; }
        public string Name { get; set; }
        public bool IsWebCam { get => Name?.ToLower().Contains("webcam") == true; }
        public object Clone()
        {
            return new Camera() { DevicePath = this.DevicePath, Name = this.Name };
        }
    }
}