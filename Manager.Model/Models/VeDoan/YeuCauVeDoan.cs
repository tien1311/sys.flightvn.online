using System;

namespace Manager.Model.Models.VeDoan
{
    public class YeuCauVeDoan
    {
        public int RowNum { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public string AgentCode { get; set; }
        public string GroupName { get; set; }
        public int GroupSize { get; set; }
        public string BriefDescription { get; set; }
        public string TripType { get; set; }
        public string ContactFullName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactOtherPhone { get; set; }
        public string ContactCompanyName { get; set; }
        public string Remark { get; set; }
        public string StreetLine1 { get; set; }
        public string StreetLine2 { get; set; }
        public string PostalCode { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string GroupType { get; set; }
        public string DepartureCode1 { get; set; }
        public string ArrivalCode1 { get; set; }
        public DateTime DepartureDateTime1 { get; set; }
        public string DepartureCode2 { get; set; }
        public string ArrivalCode2 { get; set; }
        public DateTime DepartureDateTime2 { get; set; }
        public string PreferredAirlineCode { get; set; }
        public string PreferredFlightNumber { get; set; }
        public string TravelClass { get; set; }
    }

}
