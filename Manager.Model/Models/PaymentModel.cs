using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;

namespace Manager.Model.Models
{
    public class PaymentAppota
    {
        public int ID { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public string PartnerCode { get; set; }
        public string ApiKey { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string OrderID { get; set; }
        public string BankCode { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentType { get; set; }
        public string AppotapayTransId { get; set; }
        public int TransactionTs { get; set; }
        public string ExtraData { get; set; }
        public string Signature { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FormattedDate
        {
            get
            {
                return CreatedDate.ToString("MM/dd/yyyy HH:mm:ss");
            }
        }
        public string TransactionPerson { get; set; }
        public string PaymentChannel { get; set; }
        public string PaymentPage { get; set; }
        public string Product { get; set; }
    }


}