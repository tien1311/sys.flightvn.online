using System.Collections.Generic;

namespace Manager.Model.Models.CarBooking
{
    public class Location
    {
        public int status { get; set; }
        public string message { get; set; }
        public List<Address> data { get; set; }

    }
}
