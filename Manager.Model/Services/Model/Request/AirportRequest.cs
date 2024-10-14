using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Services.Model.Request
{
    public class AirportCodeRequest
    {
        public string ID { get; set; }
        public string AirportCode { get; set; }
        public decimal Latitude { get; set; } = 0;
        public decimal Longitude { get; set; } = 0;
        public string TimeZoneOffset { get; set; }
        public string IataCode { get; set; }
        public string CityCode { get; set; }
        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public List<Profile> Profiles { get; set; }
    }
    public class AirportCodeRequest_Search
    {
        public string Key { get; set; }
    }
    public class Profile
    {
        public string LocaleId { get; set; }
        public string AirportName { get; set; }
        public string Description { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
    }
}
