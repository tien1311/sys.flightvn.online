using System.Collections.Generic;

namespace Manager.Model.Models.PaymentGateway
{
    public class Payment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActived { get; set; }
        public string Image { get; set; }
        public List<PaymentFee> PaymentFees { get; set; }
    }
}
