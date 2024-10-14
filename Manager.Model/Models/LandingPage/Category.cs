using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.LandingPage
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public int Position { get; set; }
        public int ParentId { get; set; }
        public bool IsHeaderMenu { get; set; }
        public bool IsActived { get; set; }
        public string CreatedBy {  get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get;set; }
    }
}
