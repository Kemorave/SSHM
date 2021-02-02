using HMSDataRepo.Controllers;
using HMSDataRepo.Model;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceUtil
{
    public class RoomDevicesController : IController<RoomDevices, SQLiteErrorCode>
    {
        public RoomDevicesController()
        {
            LocalConfig = LocalConfig.Instance;
        }
        public LocalConfig LocalConfig;
        public async Task<SQLiteErrorCode> AddEntry(RoomDevices data)
        {
            return await Task.Run(() =>
            {

                try
                {
                    LocalConfig.AddRoomDevicess(data);
                    return SQLiteErrorCode.Ok;
                }
                catch (SQLiteException e)
                {
                    return e.ErrorCode;
                }
            });
        }

        public async Task<SQLiteErrorCode> DeleteByID(int ID)
        {
            return await Task.Run(() =>
            {
                try
                {
                    LocalConfig.RemoveRoomDevicess(new RoomDevices() { ID=ID});
                    return SQLiteErrorCode.Ok;
                }
                catch (SQLiteException e)
                {
                    return e.ErrorCode;
                }
            });
        }

        public async Task<IEnumerable<RoomDevices>> GetAllEntries()
        {
            List<RoomDevices> list = new List<RoomDevices>();
            return await Task.Run(() =>
            {
                
                return LocalConfig.GetRoomDevicess();
            });
        }

        public async Task<RoomDevices> GetByID(int ID) => (await GetAllEntries()).FirstOrDefault(s => s.ID == ID);


        public async Task<SQLiteErrorCode> SetByID(int ID, RoomDevices data)
        {
            return await Task.Run(() =>
            {
                try
                {
                    LocalConfig.RemoveRoomDevicess(data);
                    LocalConfig.AddRoomDevicess(data);
                    return SQLiteErrorCode.Ok;
                }
                catch (SQLiteException e)
                {
                    return e.ErrorCode;
                }
            });
        }
    }
}