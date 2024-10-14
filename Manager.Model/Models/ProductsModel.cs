using Manager.Model.Models.Hotel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace Manager.Model.Models
{
    public class ProductsModel
    {
        public int RowNum { get; set; }
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Flag { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string MainImageURL { get; set; }
        public List<Image> ListImages { get; set; }
        public List<ProductsType> ListProductTypes { get; set; }
        public List<HotelService> ListHotelServices { get; set; }
    }
    public class ProductsType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double DiscountPrice { get; set; }
        public string Description { get; set; }
        public int ProductID { get; set; }
        public int MaxPerson { get; set; }
        public string ProductName { get; set; }
        public List<VisaModel> ListVisa { get; set; }
    }
    //public class Image
    //{
    //    public int ID { get; set; }
    //    public int ProductID { get; set; }
    //    public string ImageURL { get; set; }
    //    public bool MainImage { get; set; }
    //}
    public class ProductsBookingModel
    {
        public int ID { get; set; }
        //public int TypeID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public double Price { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public string Reciever { get; set; }
        public int StatusID { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ProductsStatus> ListProductsStatus { get; set; }
    }
    public class ProductsStatus
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
