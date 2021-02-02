using Newtonsoft.Json;
using System;
using System.Globalization;

namespace HMSDataRepo.Model
{
    public class Records: IDBModel
    {

        [JsonProperty(DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate)]
        public int ID { get; set; }

        [JsonProperty]
       public int Person_Id { get; set; }

        [JsonProperty]
        public string Content { get; set; }

        [JsonProperty]
        public string Type { get; set; }

        //[JsonConverter(typeof(CustomDateTimeConverter))]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public DateTime Date { get; set; } = DateTime.Now;
    }
    public class CustomDateTimeConverter : Newtonsoft.Json.Converters.DateTimeConverterBase
    {
        private const string Format = "yyyy-MM-dd HH:mm:ss";

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString(Format));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            var s = reader.Value.ToString();
            DateTime result;
            if (DateTime.TryParseExact(s, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }

            return null;
        }
    }
}