using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.IP
{
    public class APIPartner
    {
        public int RowNum { get; set; }
        public int Id { get; set; }
        public string Company { get; set; }
        public string PhysicalAddress { get; set; }
        public string IPAddresses { get; set; }
        public string IPAddressIDs { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public List<IPPartner> IPPartners { get; set; }
    }
}
