﻿namespace Manager.Model.Models.PaymentGateway
{
    public class PaymentFee
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public string RequestType { get; set; }
        public string Name { get; set; }
        public double Percent { get; set; }
        public bool IsActived { get; set; }
        public decimal? FixedCosts { get; set; }
        public string PaymentName { get; set; }
        public string Image { get; set; }
    }
}
