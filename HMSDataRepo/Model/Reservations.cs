using System;

namespace HMSDataRepo.Model
{
    public class Reservations : IDBModel
    {


        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate)]
        public int ID { get; set; }

        [Newtonsoft.Json.JsonProperty]
        public int Person_Id { get; set; }

        [Newtonsoft.Json.JsonProperty]
        public int Room_Id { get; set; }
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore)]
        public bool IsAvailable =>StartDate.AddDays(Days) > DateTime.Now;
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore)]
        public string ReadableTimeLeft => ToReadableString(TimeLeft);
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore)]
        public string ReadableTimeElapsed => ToReadableString(TimeElapsed);
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore)]
        public TimeSpan TimeLeft => StartDate.AddDays(Days) - (DateTime.Now);
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore)]
        public TimeSpan TimeElapsed => DateTime.Now-(StartDate);
        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate)]
        public DateTime StartDate { get; set; }

        [Newtonsoft.Json.JsonProperty]
        public int Days { get; set; }
        [Newtonsoft.Json.JsonProperty]
        public decimal TotalPrice { get; set; }

        private static string ToReadableString(TimeSpan span)
        {
            string formatted = span.Seconds < 0 ? "Expired" : string.Format("{0} {1} {2}",//"{0} {1} {2} {3}"
                span.Duration().Days > 0 ? string.Format("{0:0} day{1}, ", span.Days, span.Days == 1 ? string.Empty : "s") : string.Empty,
                span.Duration().Hours > 0 ? string.Format("{0:0} hour{1}, ", span.Hours, span.Hours == 1 ? string.Empty : "s") : string.Empty,
                span.Duration().Minutes > 0 ? string.Format("{0:0} minute{1}, ", span.Minutes, span.Minutes == 1 ? string.Empty : "s") : string.Empty);//,
                                                                                                                                                       //  span.Duration().Seconds > 0 ? string.Format("{0:0} second{1}", span.Seconds, span.Seconds == 1 ? string.Empty : "s") : string.Empty);

            if (formatted.EndsWith(", "))
            {
                formatted = formatted.Substring(0, formatted.Length - 2);
            }
            if (string.IsNullOrEmpty(formatted))
            {
                formatted = "Expired";
            }
            return formatted;
        }

    }
}