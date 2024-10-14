using System.Collections.Generic;

namespace Manager.Model.Models.CarBooking
{
    public class Price
    {
        public int status { get; set; }
        public string message { get; set; }
        public Dictionary<string, CarType> data { get; set; }
    }
}
