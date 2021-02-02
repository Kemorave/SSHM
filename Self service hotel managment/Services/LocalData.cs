using System;
using System.Diagnostics;
using System.IO;

namespace Services
{
    public class LocalData
    {
        public const string LoginInfoFile = "SSHLogin.txt";
        public const string ReservationsFile = "absfort";
        public static HMSDataRepo.Model.People GetLoginInfo()
        {
            try
            { string json = File.Exists(LoginInfoFile)? File.ReadAllText(LoginInfoFile):null;
           
                return Newtonsoft.Json.JsonConvert.DeserializeObject<HMSDataRepo.Model.People>(json);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Faild to get login info " + e.Message);
                return null;
            }
        }

        public static bool HasLoginData()
        {
            HMSDataRepo.Model.People data = GetLoginInfo();
            if (data == null)
            {
                return false;
            }
            if (data.Email != null && data.PersonName != null && data.PasswordHash != null)
            {
                return true;
            }
            return false;
        }
        public static void SaveLoginInfo(HMSDataRepo.Model.People person)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(person);
            try
            {
                File.WriteAllText(LoginInfoFile, json);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Faild to save login info " + e.Message);
            }
        }
        public static void DeleteLoginInfo()
        {
            try
            {
                File.Delete(LoginInfoFile);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Faild to delete login info " + e.Message);
            }
        }


    }
}