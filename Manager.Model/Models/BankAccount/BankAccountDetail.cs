using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.BankAccount
{
    public class BankAccountDetail
    {
        public int RowNum { get; set; }
        public int Id { get; set; }
        public string AgentCode { get; set; }
        public string PhoneNumber { get; set; }
        public string SecondarySerial { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class BankAccountDetailExcelHeader
    {
        public const string AgentCode = "AgentCode";
        public const string Serial = "Serial";
        public const string PhoneNumber = "PhoneNumber";

    }
}
