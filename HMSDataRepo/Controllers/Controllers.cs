using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HMSDataRepo.Controllers
{
    public class RoomsController : CloudControllerBase<Model.Rooms>
    {
        public RoomsController(WebApiConfig webApiConfig) : base(webApiConfig)
        {
            ApiRoute = "/api/v1/rooms/";
        }
    }
    public class RecordsController : CloudControllerBase<Model.Records>
    {
        public RecordsController(WebApiConfig webApiConfig) : base(webApiConfig)
        {
            ApiRoute = "/api/v1/records/";
        }
    }
    public class ReservationsController : CloudControllerBase<Model.Reservations>
    {
        public ReservationsController(WebApiConfig webApiConfig) : base(webApiConfig)
        {
            ApiRoute = "/api/v1/reservations/";
        }
    }
    public class RespondeMessage
    {
        public bool Error { get; set; }
        public int ID { get; set; }

        public string Message { get; set; }
    }
    public class PeopleController : CloudControllerBase<Model.People>
    {
        public static Regex VaildEmailRegex = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$", RegexOptions.IgnoreCase);

        public PeopleController(WebApiConfig webApiConfig) : base(webApiConfig)
        {
            ApiRoute = "/api/v1/people/";
        }
        public async Task<RespondeMessage> Authorize(string email, string passHash)
        {
            string JSON = "{ \"email\":\"" + email + "\", \"password\":\"" + passHash + "\"}";
            System.Net.Http.HttpResponseMessage res = await this.ApiConfig.Client.PostAsync($"{ApiRoute}auth", new System.Net.Http.StringContent(JSON, System.Text.Encoding.UTF8, "application/json"));
            string ress = await res.Content.ReadAsStringAsync();
            res.EnsureSuccessStatusCode();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<RespondeMessage>(ress); 
        }
        public Verfication GetVerfications(Model.People person)
        {
            if (person == null)
            {
                return null;
            }
            Verfication verfication = null;
            if (string.IsNullOrEmpty(person.PersonName))
            {
                verfication = new Verfication(nameof(person.PersonName), "Name can't be empty");
            }
            if (string.IsNullOrEmpty(person.Email))
            {
                verfication = new Verfication(nameof(person.Email), "Email can't be empty");
            }
            if (string.IsNullOrEmpty(person.PasswordHash))
            {
                verfication = new Verfication(nameof(person.PasswordHash), "Password can't be empty");
            }
            if (person.Password?.Length < 8)
            {
                verfication = new Verfication(nameof(person.PasswordHash), "Password too short");
            }
            if (!VaildEmailRegex.IsMatch(person.Email ?? "invaild"))
            {
                verfication = new Verfication(nameof(person.Email), "Invaild Email");
            }
            return verfication;
        }
    }
    public class Verfication
    {
        public Verfication(string propertyName, string vaildationMessage)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            VaildationMessage = vaildationMessage ?? throw new ArgumentNullException(nameof(vaildationMessage));
        }

        public string PropertyName { get; }
        public string VaildationMessage { get; }
    }
}