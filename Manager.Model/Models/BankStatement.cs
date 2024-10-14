using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class BankStatementModel
    {
        public int ID { get; set; }
        public string ClientID { get; set; }
        public string ClientRequestID { get; set; }
        public string CreatedUp { get; set; }
        public string CreatedDate { get; set; }
        public string Code { get; set; }
    }
    public class BankStatement_Request_Model
    {
        public int ID { get; set; }
        public int ID_Bank_Statement { get; set; }
        public string RequestType { get; set; }
        public string RequestCode { get; set; }
        public Pagination Pagination { get; set; }
    }
    public class BankStatement_Request_Params_Model
    {
        public int ID { get; set; }
        public string CreatedDate { get; set; }
        public int ID_BankStatement_Request { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionChannel { get; set; }
        public string TransactionCode { get; set; }
        public string AccountNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime TransactionDateVN { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string DebitOrCredit { get; set; }
        public string VaPrefixCd { get; set; }
        public string VaNbr { get; set; }
        public decimal Amount { get; set; }
        public string TraceNumber { get; set; }
        public string BeneficiaryName { get; set; }
        public string BeneficiaryAccountName { get; set; }
        public string ReceiverBankName { get; set; }
        public string RemmitterName { get; set; }
        public string RemmitterAccountNumber { get; set; }
        public string IssuerBankName { get; set; }
        public string VirtualAccount { get; set; }
        public string ReferenceNumber { get; set; }
        public string PartnerCustomerCode { get; set; }
        public string Customer1 { get; set; } = "";
        public string Customer2 { get; set; } = "";
        public string Customer3 { get; set; } = "";
        public string Customer4 { get; set; } = "";
        public string Customer5 { get; set; } = "";
        public string Customer6 { get; set; } = "";
        public string Customer7 { get; set; } = "";
        public string Customer8 { get; set; } = "";
        public string Customer9 { get; set; } = "";
        public string Customer10 { get; set; } = "";
        public string TransactionContent { get; set; } = "";

    }
    public class Pagination
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
    }

}
