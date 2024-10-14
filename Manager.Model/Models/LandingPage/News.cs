using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.LandingPage
{
    public class News
    {
        public int RowNum { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Image { get; set; }
        public int ViewCount { get; set; }
        public bool IsActived { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
