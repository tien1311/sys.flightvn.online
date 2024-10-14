using System;

namespace Manager.Model.Models.CarBooking.Result
{
    public class Record
    {
        public int id { get; set; }
        public string admin_id { get; set; }
        public string evcode { get; set; }
        public string agency_id { get; set; }
        public string driver_id_created { get; set; }
        public string status { get; set; }
        public string customer_name { get; set; }
        public string customer_phone { get; set; }
        public string pickup_address { get; set; }
        public string destination_address { get; set; }
        public string area { get; set; }
        public string round_trip { get; set; }
        public string is_have_bill { get; set; }
        public string price_customer { get; set; }
        public string price_bid { get; set; }
        public string is_collect_money { get; set; }
        public string pickup_time { get; set; }
        public string type_of_car { get; set; }
        public string count { get; set; }
        public string note { get; set; }
        public string created_on { get; set; }
        public string modified_on { get; set; }
        public string type_reject { get; set; }
        public string type { get; set; }
        public string utm_source { get; set; }
        public string utm_campaign { get; set; }
        public string utm_medium { get; set; }
        public string remote_ip { get; set; }
        public string url { get; set; }
        public string voucher { get; set; }
        public string stop_point { get; set; }
        public string tracking_info { get; set; }
        public string website { get; set; }
        public string google_excel { get; set; }
        public string call_back_id { get; set; }
        public string customer_property { get; set; }
        public string price_booking { get; set; }
        public string trip_status { get; set; }
        public string type_of_car_value { get; set; }
    }
}
