using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manager.Model.Models.CarBooking
{
    public partial class Commission
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public double discount { get; set; }
        public bool active { get; set; }
    }
}
