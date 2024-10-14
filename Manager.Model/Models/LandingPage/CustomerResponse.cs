using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.LandingPage
{
    public class CustomerResponse
    {
        public int Id {  get; set; }
        public int CustomerRequestId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
