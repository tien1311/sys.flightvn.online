using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.IP
{
    public class IPPartner
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public string IPAddress { get; set; }
        public List<EndpointPartner> EndpointPartners { get; set; }
    }
}
