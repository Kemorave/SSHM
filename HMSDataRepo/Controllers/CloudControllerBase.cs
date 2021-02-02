using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static HMSDataRepo.WebApiConfig;

namespace HMSDataRepo.Controllers
{
    public abstract class CloudControllerBase<DBModel> : IController<DBModel, HttpStatusCode> where DBModel : Model.IDBModel, new()
    {
        public WebApiConfig ApiConfig { get; }
        public string ApiRoute { get; protected set; }

        public CloudControllerBase(WebApiConfig webApiConfig)
        {
            ApiConfig = webApiConfig;
        }
        public virtual async Task<HttpStatusCode> DeleteByID(int ID)
        {
            try
            {
                System.Net.Http.HttpResponseMessage res = await ApiConfig.Client.DeleteAsync(ApiRoute + ID);
                Debugger(res, "Delete");
                res.EnsureSuccessStatusCode();
                ReportWebActivity(true, res.StatusCode, this);
                return res.StatusCode;
            }
            catch
            {
                ReportWebActivity(false, null, this);
                return HttpStatusCode.ServiceUnavailable;
            }
        }
        public virtual async Task<IEnumerable<DBModel>> GetAllEntries()
        {
            try
            {
                List<DBModel> rooms = new List<DBModel>();
                System.Net.Http.HttpResponseMessage res = await ApiConfig.Client.GetAsync(ApiRoute);
                res.EnsureSuccessStatusCode();
                string con = await res.Content.ReadAsStringAsync();
                ReportWebActivity(true, res.StatusCode, this);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<DBModel>>(con);
            }
            catch (Exception)
            {
                ReportWebActivity(false, null, this);

                throw;
            }
        }
        public virtual async Task<DBModel> GetByID(int ID)
        {
            try
            {
                System.Net.Http.HttpResponseMessage res = await ApiConfig.Client.GetAsync(ApiRoute + ID);
                res.EnsureSuccessStatusCode();
                ReportWebActivity(true, res.StatusCode, this);
                string st = await res.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<DBModel>>(st).FirstOrDefault();
            }
            catch (Exception)
            {
                ReportWebActivity(false, null, this);

                throw;
            }
        }
        public virtual async Task<HttpStatusCode> SetByID(int ID, DBModel data)
        {
            try
            {
                string JSON = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                System.Net.Http.HttpResponseMessage res = await ApiConfig.Client.PutAsync(ApiRoute + ID, new System.Net.Http.StringContent(JSON, System.Text.Encoding.UTF8, "application/json"));
                res.EnsureSuccessStatusCode();
                return res.StatusCode;
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }
        }
        public virtual async Task<HttpStatusCode> AddEntry(DBModel data)
        {
            try
            {
                string JSON = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                System.Net.Http.HttpResponseMessage res = await ApiConfig.Client.PostAsync(ApiRoute, new System.Net.Http.StringContent(JSON, System.Text.Encoding.UTF8, "application/json"));
                Debugger(res, "Add");
                res.EnsureSuccessStatusCode();
                ReportWebActivity(true, res.StatusCode, this);
                return res.StatusCode;
            }
            catch (Exception)
            {
                ReportWebActivity(false, null, this);

                return HttpStatusCode.BadRequest;
            }
        }
        public static void Debugger(System.Net.Http.HttpResponseMessage res, string type)
        {
            System.Diagnostics.Debug.WriteLine("\n\n================= " + type + " ===============\n" + res.Content.ReadAsStringAsync().Result);
            LogsConfig.Instance.Log(type, res.Content.ReadAsStringAsync().Result);
        }
        public static void Debugger(string res, string type)
        {
            System.Diagnostics.Debug.WriteLine("\n\n================= " + type + " ===============\n" + res);
            LogsConfig.Instance.Log(type, res);
        }
        public static void Debugger(Exception res)
        {
            System.Diagnostics.Debug.WriteLine("\n\n================= Controller Error ===============\n" + res.GetType().Name + "\n" + res.Message);
            LogsConfig.Instance.Log(res);
        }
    }
}