using System;

namespace Manager.Model.Models.CarBooking
{
    public class Area
    {
        public string id { get; set; }
        public string provinceid { get; set; }
        public string districtid { get; set; }
        public string street { get; set; }
        public string area_name { get; set; }
        public string description { get; set; }
        public DateTime? created_on { get; set; }
        public DateTime? updated_on { get; set; }
    }
}
