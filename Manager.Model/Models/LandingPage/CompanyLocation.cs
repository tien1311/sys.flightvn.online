using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.LandingPage
{
    public class CompanyLocation
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Hotline { get; set; }
        public string City { get; set; }
        public bool IsHeadQuarter { get; set; }
        public string AreaCode { get; set; }
    }
}
