using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models.ViewModel
{
    public class BookingViewModel
    {
        public List<Booking> ListBooking { get; set; }
        public List<SelectOptionValue> ListTinhTrang { get; set; }
        public List<SelectOptionValue> ListHTThanhToan { get; set; }
        public List<SelectOptionValue> ListNguoiThayDoi { get; set; }
    }
    public class Booking
    {
        public string RowID { get; set; }
        public string Order_ID { get; set; }
        public string BkStatus { get; set; }
        public string Contact_Name { get; set; }
        public string Contact_Phone { get; set; }
        public double BagGage_OneWay { get; set; }
        public double BagGage_RoundTrip { get; set; }
        public string Remark { get; set; }
        public double Total { get; set; }
        public DateTime DateBook { get; set; }
        public string Assigned { get; set; }
        public string Transaction_Note { get; set; }
        public string User_Confirm { get; set; }
    }
    public class BookingSearch
    {
        public string Booking { get; set; }
        public string NguoiLH { get; set; }
        public string cal_from { get; set; }
        public string cal_to { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Tinhtrang { get; set; }
        public string Thanhtoan { get; set; }
        public string NguoiThayDoi { get; set; }
        public string Xacnhan { get; set; }
        public string Thietbi { get; set; }
    }

    public class DetailBooking
    {
        public string OrderID_SMS { get; set; }
        public string HanhTrinh_SMS { get; set; }
        public string NguoiLienHe_SMS { get; set; }
        public string DienThoai_SMS { get; set; }
        public string Gio_SMS { get; set; }
        public string NgayThanhToan_SMS { get; set; }
        public string GiaVeMoi_SMS { get; set; }

        public string LoaiVe { get; set; }
        public string HinhThucThanhToan { get; set; }
        public string NganHangChuyenKhoan { get; set; }
        public string HanhTrinh { get; set; }
        public string DiemDi { get; set; }
        public string DiemDen { get; set; }
        public string LuotDi { get; set; }
        public string GhiChu { get; set; }
        public string ThietBi { get; set; }
        public string NguoiThayDoi { get; set; }
        public string TimeLimit { get; set; }
        public string DateLimit { get; set; }
        public string MaGiaoDich { get; set; }
        public string NganHangTTTT { get; set; }
        public string RemarkKeToan { get; set; }
        public string TinhTrang { get; set; }
        public string TinhTrangStatus { get; set; }
        public string TinhTrangThanhToan { get; set; }
        public string CheckTinhTrangTT { get; set; }
        public string SoTienChuyenKhoan { get; set; }
        public string MaDatCho_LD { get; set; }
        public string MaDatCho_LV { get; set; }
        public string LuotVe { get; set; }
        public string Remark { get; set; }
        public string NgonNgu { get; set; }
        public string NgayBook { get; set; }
        public string NgayThayDoi { get; set; }
        public string MaThamChieu { get; set; }
        public string SoTienGDTrucTuyen { get; set; }
        public string TongTienMoi { get; set; }

        public string MaVoucher { get; set; }
        public string MaKeToan { get; set; }
        public string NguoiLienHe { get; set; }
        public string Email { get; set; }
        public string ThanhPho { get; set; }
        public string DonVi { get; set; }
        public string GiamGia { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public string QuocGia { get; set; }

        public string Airline_LD { get; set; }
        public string SoHieu_LD { get; set; }
        public string Hang_LD { get; set; }
        public string Code_LD { get; set; }
        public string DiemDi_LD { get; set; }
        public string DiemDen_LD { get; set; }
        public string NgayDi_LD { get; set; }
        public string GioDi_LD { get; set; }
        public string GioDen_LD { get; set; }
        public double GiaNet_LD { get; set; }
        public double Thue_LD { get; set; }
        public double PhiDV_LD { get; set; }
        public double Giam_LD { get; set; }
        public double TongTien_LD { get; set; }

        public string Airline_LV { get; set; }
        public string SoHieu_LV { get; set; }
        public string Hang_LV { get; set; }
        public string Code_LV { get; set; }
        public string DiemDi_LV { get; set; }
        public string DiemDen_LV { get; set; }
        public string NgayDi_LV { get; set; }
        public string GioDi_LV { get; set; }
        public string GioDen_LV { get; set; }
        public double GiaNet_LV { get; set; }
        public double Thue_LV { get; set; }
        public double PhiDV_LV { get; set; }
        public double Giam_LV { get; set; }
        public double TongTien_LV { get; set; }

        public string CongTy_HD { get; set; }
        public string MST_HD { get; set; }
        public string DiaChiCT_HD { get; set; }
        public string DiaChiHD_HD { get; set; }

        public string NguoiNhan_GV { get; set; }
        public string DiaChiGiaoVe_GV { get; set; }
        public string DienThoai_GV { get; set; }
        public string ThanhPho_GV { get; set; }
        public string GiaoCho { get; set; }
        public string Adt { get; set; }
        public string Chd { get; set; }
        public string Inf { get; set; }

        public List<SelectOptionValue> ListTinhTrang { get; set; }
        public List<SelectOptionValue> ListHTThanhToan { get; set; }
        public List<SelectOptionValue> ListNguoiThayDoi { get; set; }
        public List<DaXuatVe> ListDaXuatVe { get; set; }
        public List<ThongTinKhach> ListThongTinKhach { get; set; }
        public List<Log> ListLog { get; set; }
    }
    public class DaXuatVe
    {
        public string Order_ID { get; set; }
    }
    public class ThongTinKhach
    {
        public string RowID { get; set; }
        public string DanhXung { get; set; }
        public string FullName { get; set; }
        public string Ho { get; set; }
        public string Ten { get; set; }
        public string NgaySinh { get; set; }
        public string Code_LD { get; set; }
        public string Code_LV { get; set; }
        public string HanhLy_LD { get; set; }
        public string HanhLy_LV { get; set; }
    }
    public class SelectOptionValue
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Select { get; set; }
    }
    public class Log
    {
        public string Order_ID { get; set; }
        public string Properties { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string UserUpdate { get; set; }
        public string DateUpdate { get; set; }
    }

}
