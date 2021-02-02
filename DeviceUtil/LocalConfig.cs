using System.Collections.Generic;
using System.Linq;

namespace DeviceUtil
{
    public class LocalConfig
    {
        public const string RoomDevicesTableName = "RoomDevices";
        private const string DBName = "Actuators.DB";
        private static string DBPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, DBName);
        LocalConfig()
        {
            Initialize();
        }
        public static LocalConfig Instance { get; } = new LocalConfig();
        public List<RoomDevices> GetRoomDevicess()
        {
            try
            {
                var json = System.IO.File.ReadAllText(DBPath);
                if (!string.IsNullOrEmpty(json))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<RoomDevices>>(json);
                }
            }
            catch (System.Exception)
            {

            }
            return new List<RoomDevices>() ;
        }
        public void AddRoomDevicess(RoomDevices info)
        {
            try
            {
                var list = GetRoomDevicess();
                info.ID = list.Count;
                list.Add(info);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                if (!string.IsNullOrEmpty(json))
                {
                    System.IO.File.WriteAllText(DBPath, json);
                    return;
                }
            }
            catch (System.Exception)
            {

            }
        }
        public void RemoveRoomDevicess(RoomDevices info)
        {
            try
            {
                var list = GetRoomDevicess();
                list.Remove(list.FirstOrDefault(s=>s.ID==info.ID));
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                if (!string.IsNullOrEmpty(json))
                {
                    System.IO.File.WriteAllText(DBPath, json);
                    return;
                }
            }
            catch (System.Exception)
            {

            }
        }
        public void Initialize()
        {
            if (!System.IO.File.Exists(DBPath))
            {
                System.IO.File.CreateText(DBPath);
            }
        }
    }
}