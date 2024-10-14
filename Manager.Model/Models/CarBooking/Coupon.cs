using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manager.Model.Models.CarBooking
{
    public partial class Coupon
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public double discount { get; set; }
        public string type { get; set; }
        public bool active { get; set; }
    }
}
