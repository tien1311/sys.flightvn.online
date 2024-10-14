using System.Collections.Generic;

namespace Manager.Model.Models.CarBooking
{
    public class TypeReject
    {
        public int status { get; set; }
        public string sessage { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
}
