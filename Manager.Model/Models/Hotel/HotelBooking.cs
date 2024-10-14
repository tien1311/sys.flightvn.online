using System;
using System.Collections.Generic;

namespace Manager.Model.Models.Hotel
{
    public class HotelBooking
    {
        public int Id { get; set; }
        public int RowNum { get; set; }
        public string BookingCode { get; set; }
        public string HotelCode { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string OtherRequestFromCustomer { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DateGiuCho { get; set; }
        public int Adults { get; set; }
        public int Childs { get; set; }
        public int Babies { get; set; }
        public double SoTien { get; set; }
        public double Commission { get; set; }
        public double VAT { get; set; }
        public double OtherFee { get; set; }
        public double TotalPrice { get; set; }
        public double AmountPaid { get; set; }
        public bool IsVAT { get; set; }
        public string Reciever { get; set; }
        public int StatusID { get; set; }
        public string Note { get; set; }
        public int TotalRoom { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentId { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentAgentCode { get; set; }
        public string Canceller { get; set; }
        public string CancelReason { get; set; }
        public string OtherFeeReason { get; set; }
        public List<HotelBookingDetail> BookingDetails { get; set; }
        public ProductsModel HotelInfo { get; set; }
        public HotelVAT HotelVAT { get; set; }
        public List<HotelBookingStatusDescription> BookingStatus { get; set; }
    }

    public class HotelBookingDetail
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string BookingCode { get; set; }
        public string HotelCode { get; set; }
        public string LoaiPhong { get; set; }
        public int SoLuong { get; set; }
        public double SoTien { get; set; }
    }

    public class HotelBookingStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class HotelVAT
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string BookingCode { get; set; }
        public string HotelCode { get; set; }
        public double VAT { get; set; }
        public string MaSoThue { get; set; }
        public string TenCongTy { get; set; }
        public string DiaChi { get; set; }
    }

    public class HotelBookingStatusDescription
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string BookingCode { get; set; }
        public string HotelCode { get; set; }
        public int StatusId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
