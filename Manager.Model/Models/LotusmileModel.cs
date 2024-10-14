using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class LotusmileModel
    {
        public int ID { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthdayDate { get; set; }
        public string Nationality { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string NOTE { get; set; }
        public string Createby { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
