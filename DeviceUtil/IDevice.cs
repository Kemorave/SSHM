using System;

namespace DeviceUtil
{
    public interface IDevice
    {
        Int32 ID { get; }
        Guid Guid { get; }
        string DevicePath { get; }
        bool IsActive { get; }

    }
}
