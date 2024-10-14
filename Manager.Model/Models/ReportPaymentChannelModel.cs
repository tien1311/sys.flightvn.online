using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class ReportPaymentChannelModel
    {
        public int ID { get; set; }
        public string Channel { get; set; }
        public string Code { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentGateway { get; set; }
    }

}
