using System;

namespace HMSDataRepo
{
    public class WebApiConfig
    {

        private const string APIUrl = "https://sshdb.glitch.me";
        public static WebApiConfig Default { get; } = new WebApiConfig();
        public static event EventHandler<Controllers.WebActivityResult> WebActivity;

        public static void ReportWebActivity(bool isSucc, System.Net.HttpStatusCode? code, object sender = null)
        {
            WebActivity?.Invoke(sender, new Controllers.WebActivityResult(isSucc, code));
        }
        private WebApiConfig()
        {
            Client = GetDefaultHttpClient();
        }
        public static bool CheckConnection()
        {
            try
            {
                using (var client = new System.Net.WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
        public System.Net.Http.HttpClient Client { get; protected set; }
        protected virtual System.Net.Http.HttpClient GetDefaultHttpClient()
        {
            var HttpClient = new System.Net.Http.HttpClient();
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Unity3D");
            HttpClient.BaseAddress = (new Uri(APIUrl));
            return HttpClient;
        }
    }
}