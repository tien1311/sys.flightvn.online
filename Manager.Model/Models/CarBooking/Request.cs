using System;
using System.ComponentModel.DataAnnotations;

namespace Manager.Model.Models.CarBooking
{
    public partial class Request
    {
        [Key]
        public int id { get; set; }
        public string evcode { get; set; }
        public string location_from { get; set; }
        public string location_to { get; set; }
        public string type { get; set; }
        public string vat { get; set; }
        public string agent_code { get; set; }
        public string coupon { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public DateTime date_booking { get; set; }
        public DateTime departure { get; set; }
        public string type_car { get; set; }
        public double price { get; set; }
        public double discount { get; set; }
        public double commission { get; set; }
        public double vat_price { get; set; }
        public int vat_percent { get; set; }
        public double total { get; set; }
        public double? other_fee { get; set; }
        public double price_customer { get; set; }
        public string booking_notes { get; set; }
        public string vat_notes { get; set; }
        public string vat_mst { get; set; }
        public string vat_address { get; set; }
        public string payment_id { get; set; }
        public string payment_type { get; set; }
        public string payment_status { get; set; }
        public string status_enviet { get; set; }
        public string ev_cancellation_reason { get; set; }
        public string dt_cancellation_reason { get; set; }
        public bool email_cancel { get; set; }
        public bool email_send { get; set; }
        public string user_booking { get; set; }
        public string status_booking { get; set; }
        public string id_booking { get; set; }
        public string other_fee_reason { get; set; }
        public DateTime? date_xacnhan { get; set; }
        public string payment_makh { get; set; }
    }
}
