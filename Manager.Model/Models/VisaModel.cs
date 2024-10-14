using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Manager.Model.Models
{
    public class VisaModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string VisaType { get; set; }
        public string ShortDescription { get; set; }
        public bool IsActive { get; set; }
        public List<Image> ListImages { get; set; }
    }
    public class TypeVisa
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double DiscountPrice { get; set; }
        public string Description { get; set; }
        public string Documents { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public List<VisaModel> ListVisa { get; set; }
    }
    public class Image
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ImageURL { get; set; }
        public bool MainImage { get; set; }
    }
    public class VisaBookingModel
    {
        public int ID { get; set; }
        //public int TypeID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string VisaName { get; set; }
        public string VisaType { get; set; }
        public string VisaCode { get; set; }
        public string DocumentsType { get; set; }
        public double Price { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public string Reciever { get; set; }
        public int StatusID { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentId { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentAgentCode { get; set; }
        public List<VisaStatus> ListVisaStatus { get; set; }
    }
    public class VisaStatus
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
