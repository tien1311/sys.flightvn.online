using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.LandingPage
{
    public class Job
    {
        public int RowNum { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Location { get; set; }
        public string Department { get; set; }
        public double FromSalary { get; set; }
        public double ToSalary { get; set; }
        public bool IsDeal {  get; set; }
        public string Description { get; set; }
        public string Requirement { get; set; }
        public string Benefit { get; set; }
        public bool IsActived { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public List<Application> ApplicationList { get; set; }
    }
}
