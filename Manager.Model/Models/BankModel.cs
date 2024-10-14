using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class BankModel
    {
        public int ID { get; set; }
        public string ShortName { get; set; }

    }
    public class BANK_ACCOUNT
    {
        public int ID { get; set; }
        public int IDBank { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public int Position { get; set; }
        public int IsActive { get; set; }
    }
}
