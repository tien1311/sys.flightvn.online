using System;

namespace Manager.Model.Models.CarBooking
{
    public class AreaConfigurationByAddress
    {
        public string id { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public DateTime? created_on { get; set; }
        public DateTime? modified_on { get; set; }
    }
}
