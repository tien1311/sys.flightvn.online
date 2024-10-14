using Manager.Model.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Web.Mvc;

namespace Manager.Model.Models
{
    public class VoucherModel
    {
        public int Id { get; set; }
        public string VoucherCode { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập tên Voucher")]
        public string VoucherName { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập mô tả ngắn")]
        [MaxLength(255, ErrorMessage = "Mô tả ngắn không được dài hơn 255 ký tự")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập mô tả chi tiết")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập giá sản phẩm")]
        [Range(1, double.MaxValue, ErrorMessage = "Giá bán không phù hợp")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập giá giảm")]
        [Range(1, double.MaxValue, ErrorMessage = "Giá giảm không phù hợp")]
        public double DiscountPrice { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập ngày bắt đầu")]
        public DateTime? ExpiryDateFrom { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập ngày kết thúc")]
        [DateComparison(nameof(ExpiryDateFrom))]
        public DateTime? ExpiryDateTo { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public bool IsActive { get; set; }
        public string ServiceName { get; set; }
        public List<ImageVoucher> listImage { get; set; }
    }
    public class VoucherServiceType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ServiceId { get; set; }
        public int VoucherId { get; set; }
    }

    public class ServiceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class ImageVoucher
    {
        public int Id { get; set; }
        public string imageUrl { get; set; }
        public bool isMainImage { get; set; }
        public int voucherId { get; set; }
    }

    public class OrderHeaderVoucher
    {
        public int Id { get; set; }
        public string BookingCode { get; set; }
        public string Sex { get; set; }
        public double Total { get; set; }
        public double PriceVAT { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public int OrderStatusId { get; set; }
        [MaxLength(300, ErrorMessage = "Vui lòng nhập dưới {1} ký tự.")]
        public string Note { get; set; }
        [MaxLength(300, ErrorMessage = "Vui lòng nhập dưới {1} ký tự.")]
        public string NoteStatus { get; set; }
        public int PercentVAT { get; set; }

        public string CompanyName { get; set; }
        public string Reciever { get; set; }
        public double CurrentPrice { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mã số thuế.")]
        public string MaSoThue { get; set; }

        [Range(1, 200, ErrorMessage = "Số lượng người lớn tối đa từ 1 đến 200.")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng người lớn.")]
        public int NumberAdult { get; set; }
        [Range(0, 200, ErrorMessage = "Số lượng trẻ em tối đa từ 0 đến 200.")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng trẻ em hoặc đặt giá trị là 0 nếu không có.")]

        public int NumberChild { get; set; }
        [Range(0, 200, ErrorMessage = "Số lượng em bé tối đa từ 0 đến 200.")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng em bé hoặc đặt giá trị là 0 nếu không có.")]
        public int NumberBaby { get; set; }

        [Required(ErrorMessage = "Vui lòng không bỏ trống.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng không bỏ trống.")]
        [Phone(ErrorMessage = "Vui lòng nhập đúng số điện thoại.")]
        [RegularExpression("[0-9]{10}", ErrorMessage = "Sai định dạng số điện thoại.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Vui lòng không bỏ trống.")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng Email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng không bỏ trống.")]
        public string Nationality { get; set; }
        [Required(ErrorMessage = "Vui lòng không bỏ trống.")]
        public DateTime DateCustomerUse { get; set; }
        [MaxLength(250)]
        public string CancelReason { get; set; }

        [NotMapped]
        public string VoucherName { get; set; }
        [NotMapped]
        public string TourLocationName { get; set; }
        [NotMapped]
        public string TourLocationPhone { get; set; }
        [NotMapped]
        public string TourLocationEmail { get; set; }
        [NotMapped]
        public string TourLocationProvince { get; set; }
        public string TourLocationDistrict { get; set; }
        [NotMapped]
        public string Status { get; set; }

    }

    public class OrderDetailVoucher
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public int VoucherId { get; set; }
    }

    public class Status
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
    }

    public class VoucherCodeHistory
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string FormatDate { get; set; }
        public int CurrentValue { get; set; }
    }
    public static class StaticDetailVoucher
    {
        public const int OrderStatusId_INFORMATION_RECEIVED = 1; // Đã tiếp nhận thông tin
        public const int OrderStatusId_PROCESSED = 2; // Đã xử lý 
        public const int OrderStatusId_PAYMENT_SUCCESS = 3; // Hoàn tất thanh toán
        public const int OrderStatusId_CANCELLED = 4; // Hủy đơn hàng
        public const int OrderStatusId_COMPLETED = 5; // Khách sử dụng dịch vụ thành công
        public const int OrderStatusId_NEW = 6; // Mới

        public const string SUCCESS = "SUCCESS";
        public const string FAIL = "FAIL";

        public const string MaPhongBan_DuLich = "DL";
        public const string MaPhongBan_IT = "IT";

        public const string Email_ProgramId_CHANGESTATUS = "EVM_CHANGESTATUSVOUCHER";
        public const string Email_ProgramId_CANCELVOUCHER = "EVM_CANCELVOUCHER";
        public const string Email_ProgramId_COMPLETED = "EVM_SUCCESSBOOKING";

        public const int Pecent_Of_VAT8 = 8;   //8%
    }
}
