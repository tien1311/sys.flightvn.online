using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.LandingPage
{
    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public int CategoryId { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int ViewCount { get; set; }
        public bool IsActived { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
