using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class Rules_Airlines
    {
        public string AirlineID { get; set; }
        public string AirlineIATACode { get; set; }
        public string AirlineICAOCode { get; set; }
        public string AirlineOATCode { get; set; }
        public string AirlineName { get; set; }
        public string Country { get; set; }
    }
    public class Rules_Partners
    {
        public string PartnerID { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string PartnerPhones { get; set; }
        public string PartnerEmails { get; set; }
    }
    public class Rules_RuleDetails
    {
        public string PartnerDetailID { get; set; }
        public string RuleDetailID { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string AirlineID { get; set; }
        public string AirlineName { get; set; }
        public string SeatClasses { get; set; }
        public string CabinClassCode { get; set; }
        public string CabinClassName_vi { get; set; }
        public string CabinClassName_en { get; set; }
        public string DomesticRules_vi { get; set; }
        public string DomesticRules_en { get; set; }
        public string IntlRules_vi { get; set; }
        public string IntlRules_en { get; set; }
    }
    public class Rule_Categories
    {
        public string CategoryID { get; set; }
        public string AirlineID { get; set; }
        public string AirlineName { get; set; }
        public string CategoryName { get; set; }
    }
    public class ImportPartnerDetails
    {
        public int PartnerID { get; set; }
        public List<Rules_RuleDetails> ListRuleDetails { get; set; }
    }
}
