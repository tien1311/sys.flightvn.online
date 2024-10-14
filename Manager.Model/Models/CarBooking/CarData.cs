namespace Manager.Model.Models.CarBooking
{
    public class CarData
    {
        public string id { get; set; }
        public string area_id { get; set; }
        public string districtid { get; set; }
        public string provinceid { get; set; }
        public string type_of_car { get; set; }
        public string street { get; set; }
        public string time { get; set; }
        public string schedule { get; set; }
        public string price { get; set; }
        public string roundtrip_price { get; set; }
        public string description { get; set; }
        public string created_on { get; set; }
        public string updated_on { get; set; }
        public string address { get; set; }
        public AreaConfigurationByTime areaConfigurationByTime { get; set; }
        public AreaConfigurationByAddress areaConfigurationByAddress { get; set; }
        public Area area { get; set; }
    }
}
