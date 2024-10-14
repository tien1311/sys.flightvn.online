using System;
using System.Collections.Generic;

namespace Manager.Model.Models.PaymentGateway
{
    public class UserPay
    {
        public int Id { get; set; }
        public int RowNum { get; set; }
        public string MaKH { get; set; }
        public string MaKH_DL { get; set; }
        public string Name { get; set; }
        public string soDienThoai { get; set; }
        public string email { get; set; }
        public string DiaChi { get; set; }
        public string paymentType { get; set; }
        public string requestType { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string orderId { get; set; }
        public string PaymentId { get; set; }
        public string Partner_TransId { get; set; }
        public string paymentStatus { get; set; }
        public int resultCode { get; set; }
        public string FailedCode { get; set; }
        public string FailedReason { get; set; }
        public string Note { get; set; }
    }
}
