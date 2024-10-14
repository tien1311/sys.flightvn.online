using System.Collections.Generic;

namespace Manager.Model.Models.CarBooking
{
    public class CarType
    {
        public string name { get; set; }
        public string id { get; set; }
        public List<CarData> data { get; set; }
    }
}
