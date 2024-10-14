using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;

namespace Manager.Model.Models
{
    public class BookingInfoModel
    {
        public string adultQuantity { get; set; }
        public string childQuantity { get; set; }
        public string kidQuantity { get; set; }
        public decimal totalPrice { get; set; }
        public decimal commission { get; set; }
        public decimal vat { get; set; }
        public decimal price { get; set; }
        public string NameTour { get; set; }
        public string TourCode { get; set; }
        public string customerName { get; set; }
        public string Namecompany { get; set; }
        public string MaKH { get; set; }
        public string customerPhone { get; set; }
        public string customerEmail { get; set; }
        public string hotelTour { get; set; }
        public string customerNote { get; set; }
        public string tourID { get; set; }
        public string CreateDate { get; set; }
        public string codetrienkhai { get; set; }
        public string TinhTrang { get; set; }
        public string tourSP { get; set; }
        public string TourName { get; set; }


    }

    public class DCHH
    {
        public string TiGia { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string NgayLap { get; set; }

    }
    public class Gia3s
    {
        public string gia { get; set; }
        public string hh { get; set; }
        public string km { get; set; }
    }

    public class Gia4s
    {
        public string gia { get; set; }
        public string hh { get; set; }
        public string km { get; set; }
    }

    public class Gia5s
    {
        public string gia { get; set; }
        public string hh { get; set; }
        public string km { get; set; }
    }

    public class Sale
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class Label
    {
        public string color { get; set; }
        public string dept { get; set; }
    }

    public class File
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Tourle
    {
        public int id { get; set; }
        public string name { get; set; }
        public string hanh_trinh { get; set; }
        public string diem_don { get; set; }
        public object note { get; set; }
        public object chuyen_bay { get; set; }
        public string diem_den { get; set; }
        public int so_ngay { get; set; }
        public int so_dem { get; set; }
        public string code_tour { get; set; }
        public List<File> file { get; set; }
    }

    public class Data
    {
        public int id { get; set; }
        public int so_khach { get; set; }
        public string ghi_chu { get; set; }
        public object xe_so { get; set; }
        public Gia3s gia_3s { get; set; }
        public Gia4s gia_4s { get; set; }
        public Gia5s gia_5s { get; set; }
        public string code_trienkhai { get; set; }
        public string ngay_di { get; set; }
        public string ngay_ve { get; set; }
        public string ngay_dong { get; set; }
        public int booked_seat { get; set; }
        public int reservation_seat { get; set; }
        public Sale sale { get; set; }
        public Label label { get; set; }
        public Tourle tourle { get; set; }
        public string total { get; set; }
    }

    public class Root
    {
        public int current_page { get; set; }
        public List<Data> data { get; set; }
        public double total { get; set; }
    }

    public class CombinedData
    {
        public Root TourData { get; set; }
        public ProvinceData ProvincesResult { get; set; }
    }

    public class Gia
    {
        public string gia_nguoi_lon { get; set; }
        public string gia_tre_em { get; set; }
        public string gia_em_be { get; set; }
        public string phu_thu_don { get; set; }
        public string phu_thu_quoctich { get; set; }
        public string hh_gia_nguoi_lon { get; set; }
        public string hh_gia_tre_em { get; set; }
        public string hh_gia_em_be { get; set; }
        public string km_gia_nguoi_lon { get; set; }
        public string km_gia_tre_em { get; set; }
        public string km_gia_em_be { get; set; }
    }

    public class TourInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public int so_ngay { get; set; }
        public int so_dem { get; set; }
        public int loai_tour { get; set; }


    }



    public class Seats
    {
        public List<string> booked { get; set; }
        public List<string> rev { get; set; }
    }

    public class DetailTourModel
    {
        public int id { get; set; }
        public int tour_id { get; set; }
        public int so_khach { get; set; }
        public string ghi_chu { get; set; }
        public object xe_so { get; set; }
        public Gia gia_rs3s { get; set; }
        public Gia gia_rs4s { get; set; }
        public Gia gia_rs5s { get; set; }
        public Gia gia_1s { get; set; }
        public Gia gia_2s { get; set; }
        public Gia gia_3s { get; set; }
        public Gia gia_4s { get; set; }
        public Gia gia_5s { get; set; }
        public string code_trienkhai { get; set; }
        public object hdv { get; set; }
        public TourInfo tour_info { get; set; }
        public List<string> tieuchuan { get; set; }
        public Sale sale { get; set; }
        public int booked_seat { get; set; }
        public int reservation_seat { get; set; }
        public Seats seats { get; set; }
        public string ngay_di { get; set; }
        public string ngay_ve { get; set; }
        public string adultQuantity { get; set; }
        public string childQuantity { get; set; }
        public string kidQuantity { get; set; }
        public decimal totalPrice { get; set; }
        public decimal price { get; set; }
        public decimal commission { get; set; }
        public string NameTour { get; set; }
        public string TourCode { get; set; }
        public string customerName { get; set; }
        public string Namecompany { get; set; }
        public string MaKH { get; set; }
        public string customerPhone { get; set; }
        public string customerEmail { get; set; }
        public string hotelTour { get; set; }
        public string customerNote { get; set; }
        public string tourID { get; set; }
        public string CreateDate { get; set; }
        public string codetrienkhai { get; set; }
        public string TinhTrang { get; set; }
        public string IDStatus { get; set; }
        public string NguoiNhan { get; set; }
        public string NguoiGui { get; set; }
        public string TenCaNhanToChuc { get; set; }
        public string MaSoThue { get; set; }
        public string DiaChi { get; set; }
        public string ngaydi { get; set; }
        public string ngayve { get; set; }
        public string LoaiTour { get; set; }
        public string lydohuy { get; set; }
        public string tourSP { get; set; }
        public string TourName { get; set; }

        public decimal Vat { get; set; }



    }



    public class ProvinceData
    {
        public Dictionary<int, string> Departure { get; set; }
        public Dictionary<int, string> Destination { get; set; }
    }


    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class TourlesEV
    {
        public string tour_Id { get; set; }
        public string name { get; set; }
        public string hanh_trinh { get; set; }
        public string diem_don { get; set; }
        public string note { get; set; }
        public string chuyen_bay { get; set; }
        public string diem_den { get; set; }
        public int? so_ngay { get; set; }
        public int? so_dem { get; set; }
        public string code_tour { get; set; }

    }
    public class Tourall
    {
        public List<Destination> Destination { get; set; }
        public List<TourEV> TourlesEV { get; set; }
    }
    public class Destination
    {
        public int IDTinh { get; set; }
        public string TenTinh { get; set; }
        public int Trangthai { get; set; }
        public int TrangThaiTinh { get; set; }
    }
    // tour hot
    public class TourEV
    {
        public int Id { get; set; }
        public string Tour_Id { get; set; }
        public string Ghi_Chu { get; set; }
        public string Loai_Xe { get; set; }
        public string Hdv { get; set; }
        public int? Tong { get; set; }
        public int? Da_Dat { get; set; }
        public int? Giu_Cho { get; set; }
        public DateTime? Ngay_Di { get; set; }
        public DateTime? Ngay_Ve { get; set; }
        public DateTime? Ngay_Dong_Tour { get; set; }
        public string CreateDate { get; set; }
        public string Short_Notes { get; set; }
        public string Long_Notes { get; set; }
        public int? Active { get; set; }
        public string Diem_Di { get; set; }
        public string Diem_Den { get; set; }
        public string Diem_Don { get; set; }
        public string Name_Tour { get; set; }
        public int? So_Ngay { get; set; }
        public int? So_Dem { get; set; }
        public string Note { get; set; }
        public int FlagID { get; set; }



        public SaleEV Sale { get; set; }
        public List<ImgEV> Imgs { get; set; }
        public List<GiaEV> Gias { get; set; }
        public List<FileEV> Files { get; set; }
        public DetailTourHotBooking DetailTourBooking { get; set; }
    }

    public class FlagModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }
    public class SaleEV
    {
        public int SaleId { get; set; }
        public string Tour_Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public TourEV Tour { get; set; }
    }
    public class ImgEV
    {
        public int Img_Id { get; set; }
        public string Tour_Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool? MainImg { get; set; }

        public TourEV Tour { get; set; }
    }
    public class GiaEV
    {
        public int gia_id { get; set; }
        public string Tour_Id { get; set; }
        public double gia_nguoi_lon { get; set; }
        public double gia_tre_em { get; set; }
        public double gia_em_be { get; set; }
        public double phu_thu_don { get; set; }
        public double phu_thu_quoctich { get; set; }
        public double hh_gia_nguoi_lon { get; set; }
        public double hh_gia_tre_em { get; set; }
        public double hh_gia_em_be { get; set; }
        public double km_gia_nguoi_lon { get; set; }
        public double km_gia_tre_em { get; set; }
        public double km_gia_em_be { get; set; }
        public string loai_gia { get; set; }

        public TourEV Tour { get; set; }
    }
    public class FileEV
    {
        public int FileId { get; set; }
        public string Tour_Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public TourEV Tour { get; set; }
    }
    public class DetailTourHotBooking
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerNote { get; set; }
        public string HotelTour { get; set; }
        public int? AdultQuantity { get; set; }
        public int? ChildQuantity { get; set; }
        public int? KidQuantity { get; set; }
        public string TourSP { get; set; }
        public string TourID { get; set; }
        public double? TotalPrice { get; set; }
        public double? Price { get; set; }
        public double? Commission { get; set; }
        public string CreateDate { get; set; }
        public string NameCompany { get; set; }
        public string MaKH { get; set; }
        public string NguoiNhan { get; set; }
        public string TinhTrang { get; set; }
        public string NguoiChuyen { get; set; }
        public string TenCaNhanToChuc { get; set; }
        public string MaSoThue { get; set; }
        public string DiaChi { get; set; }
        public int? LoaiTour { get; set; }
        public string LyDoHuy { get; set; }
        public string IDStatus { get; set; }
        public double? Vat { get; set; }
        public string TourName { get; set; }
        public string NgayDi { get; set; }
        public string NgayVe { get; set; }
    }
}