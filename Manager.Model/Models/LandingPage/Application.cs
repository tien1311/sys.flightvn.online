using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.LandingPage
{
    public class Application
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Resume { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
