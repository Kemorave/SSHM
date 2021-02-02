using System;

namespace HMSDataRepo.Model
{
    public class Rooms : ICloneable, IDBModel
    {

        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate)]
        public int ID { get; set; }

        [Newtonsoft.Json.JsonProperty]
        public string Description { get; set; }

        [Newtonsoft.Json.JsonProperty]
        public decimal Price { get; set; }

        public object Clone()
        {
            return new Rooms() { Description = this.Description, ID = this.ID, Price = this.Price };
        }
    }
}