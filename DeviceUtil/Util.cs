using System.Collections.Generic;
using System.Linq;
using HMSDataRepo.Model;

namespace DeviceUtil
{
    public static class Util
    {
        public static List<Camera> GetCameras(bool excludeWebCam = false)
        {
            if (excludeWebCam)
            {
                return new List<Camera>(WPFMediaKit.DirectShow.Controls.MultimediaUtil.VideoInputDevices.Select(d => new Camera() { DevicePath = d.DevicePath, Name = d.Name })) { }.Where(s => !s.IsWebCam).ToList();
            }
            return new List<Camera>(WPFMediaKit.DirectShow.Controls.MultimediaUtil.VideoInputDevices.Select(d => new Camera() { DevicePath = d.DevicePath, Name = d.Name })) { };
        }
        public static Camera GetCamera(string path)
        {
            Camera camera = null;
            DirectShowLib.DsDevice dev = WPFMediaKit.DirectShow.Controls.MultimediaUtil.VideoInputDevices.FirstOrDefault(s => s.DevicePath == path || s.Name == path);
            camera = new Camera() { DevicePath = dev?.DevicePath, Name = dev?.Name };
            return camera;
        }
    }
}