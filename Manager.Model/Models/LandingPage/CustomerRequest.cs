using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.LandingPage
{
    public class CustomerRequest
    {
        public int RowNum { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsResolved { get; set; }

    }
}
