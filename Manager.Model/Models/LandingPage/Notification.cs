using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.LandingPage
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }   
        public string Body { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
