using HMSDataRepo.Model;
using System;
using ZXing;

namespace DeviceUtil.Recognition
{
    public class RecognitionResult : EventArgs
    {
        public RecognitionResult(string camera, Result result,int  key)
        {
            CameraPath = camera;
            Result = result;
            Key = key;
        }
        public RecognitionResult() { }
        public string CameraName { get; internal set; }
        public string CameraPath { get; internal set; }
        public Result Result { get; internal set; }
        public int Key { get; internal set; }
    }
}