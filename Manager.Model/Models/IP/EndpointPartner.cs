using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.IP
{
    public class EndpointPartner
    {
        public int Id { get; set; }
        public int IPPartnerId { get; set; }
        public string EndPoint { get; set; }
        public bool IsActived { get; set; }
    }
}
