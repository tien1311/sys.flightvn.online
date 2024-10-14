using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.BankAccount
{
    public class BankAccount
    {
        public int RowNum { get; set; }
        public int Id { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string FirstSerial { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
