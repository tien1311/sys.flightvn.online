using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Services.Model.Request
{


    public class HDDTRequest
    {


    }



    public class SaveRequest
    {
        public string username { get; set; }

        public EInvoiceRequest invoice { get; set; }

    }


    public class DeleteCancelHDDTRequest : BaseRequest
    {
        public string Ikey { get; set; } = "";
        public string Serial { get; set; } = "";

        public string Pattern { get; set; } = "";
    }

    public class DanhSachHDDTRequest : BaseRequest
    {
        public string AgentId { get; set; } = "";
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int Status { get; set; }

        public string TicketNumber { get; set; }

        public string IKey { get; set; }

        public string RequestCode { get; set; }
        public string MaKH { get; set; } = "";

    }




    public class InHDDTRequest : BaseRequest
    {
        public string Ikey { get; set; } = "";
        public string Pattern { get; set; } = "";

        public int Type { get; set; } = 0;
    }


    [Serializable]
    public class EInvoiceRequest
    {

        public TongQuat tongQuat { get; set; } = null;
        public List<DetailVe> danhSachVe { get; set; } = null;

        public Invoice_CT ChiTiet { get; set; } = null;
        public List<EInvoice_CT> ChiTietHDDT { get; set; } = null;

        public List<EInvoice_Fee> DanhSachLePhi { get; set; }


    }

    [Serializable]
    public class EInvoice_CT
    {
        public string SoVe { get; set; }
        public string HanhTrinh { get; set; }
        public string HangHK { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; } = 0;
        public decimal Thue { get; set; } = 0;
        public decimal PhiKhac { get; set; } = 0;
        public decimal HoaHong { get; set; } = 0;
        public decimal PhiDoi { get; set; } = 0;
        public decimal PhiHoan { get; set; } = 0;
        public decimal PhiEV { get; set; } = 0;
        public decimal TongCong { get; set; } = 0;
        public string HDDauVao { get; set; } = "";
        public DateTime NgayHDDauVao { get; set; } = DateTime.Parse("1900-01-01");
    }

    [Serializable]
    public class EInvoice_Fee
    {
        public string TenLePhi { get; set; }
        public string TienLePhi { get; set; }
        public string TiLeLePhi { get; set; }
        public string ThueSuatPhi { get; set; }
        public string TienThuePhi { get; set; }
        public string TienSauThuePhi { get; set; }
    }

    [Serializable]
    public class Invoice_CT
    {
        public int RowID { get; set; }
        public string SOCT { get; set; }
        public string KHHOADON { get; set; }
        public int STT1 { get; set; }
        public int STT2 { get; set; }
        public int STT3 { get; set; }
        public int STT4 { get; set; }
        public int STT5 { get; set; }
        public int STT6 { get; set; }
        public int STT7 { get; set; }
        public int STT8 { get; set; }
        public int STT9 { get; set; }
        public int STT10 { get; set; }
        public int STT11 { get; set; }
        public int STT12 { get; set; }
        public int STT13 { get; set; }
        public int STT14 { get; set; }
        public int STT15 { get; set; }
        public string TENHH1 { get; set; }
        public string TENHH2 { get; set; }
        public string TENHH3 { get; set; }
        public string TENHH4 { get; set; }
        public string TENHH5 { get; set; }
        public string TENHH6 { get; set; }
        public string TENHH7 { get; set; }
        public string TENHH8 { get; set; }
        public string TENHH9 { get; set; }
        public string TENHH10 { get; set; }
        public string TENHH11 { get; set; }
        public string TENHH12 { get; set; }
        public string TENHH13 { get; set; }
        public string TENHH14 { get; set; }
        public string TENHH15 { get; set; }
        public string DVT1 { get; set; }
        public string DVT2 { get; set; }
        public string DVT3 { get; set; }
        public string DVT4 { get; set; }
        public string DVT5 { get; set; }
        public string DVT6 { get; set; }
        public string DVT7 { get; set; }
        public string DVT8 { get; set; }
        public string DVT9 { get; set; }
        public string DVT10 { get; set; }
        public string DVT11 { get; set; }
        public string DVT12 { get; set; }
        public string DVT13 { get; set; }
        public string DVT14 { get; set; }
        public string DVT15 { get; set; }
        public int SOLUONG1 { get; set; }
        public int SOLUONG2 { get; set; }
        public int SOLUONG3 { get; set; }
        public int SOLUONG4 { get; set; }
        public int SOLUONG5 { get; set; }
        public int SOLUONG6 { get; set; }
        public int SOLUONG7 { get; set; }
        public int SOLUONG8 { get; set; }
        public int SOLUONG9 { get; set; }
        public int SOLUONG10 { get; set; }
        public int SOLUONG11 { get; set; }
        public int SOLUONG12 { get; set; }
        public int SOLUONG13 { get; set; }
        public int SOLUONG14 { get; set; }
        public int SOLUONG15 { get; set; }
        public decimal DONGIA1 { get; set; }
        public decimal DONGIA2 { get; set; }
        public decimal DONGIA3 { get; set; }
        public decimal DONGIA4 { get; set; }
        public decimal DONGIA5 { get; set; }
        public decimal DONGIA6 { get; set; }
        public decimal DONGIA7 { get; set; }
        public decimal DONGIA8 { get; set; }
        public decimal DONGIA9 { get; set; }
        public decimal DONGIA10 { get; set; }
        public decimal DONGIA11 { get; set; }
        public decimal DONGIA12 { get; set; }
        public decimal DONGIA13 { get; set; }
        public decimal DONGIA14 { get; set; }
        public decimal DONGIA15 { get; set; }
        public decimal THANHTIEN1 { get; set; }
        public decimal THANHTIEN2 { get; set; }
        public decimal THANHTIEN3 { get; set; }
        public decimal THANHTIEN4 { get; set; }
        public decimal THANHTIEN5 { get; set; }
        public decimal THANHTIEN6 { get; set; }
        public decimal THANHTIEN7 { get; set; }
        public decimal THANHTIEN8 { get; set; }
        public decimal THANHTIEN9 { get; set; }
        public decimal THANHTIEN10 { get; set; }
        public decimal THANHTIEN11 { get; set; }
        public decimal THANHTIEN12 { get; set; }
        public decimal THANHTIEN13 { get; set; }
        public decimal THANHTIEN14 { get; set; }
        public decimal THANHTIEN15 { get; set; }
        public string LOAITHUE1 { get; set; }
        public string LOAITHUE2 { get; set; }
        public string LOAITHUE3 { get; set; }
        public string LOAITHUE4 { get; set; }
        public string LOAITHUE5 { get; set; }
        public string LOAITHUE6 { get; set; }
        public string LOAITHUE7 { get; set; }
        public string LOAITHUE8 { get; set; }
        public string LOAITHUE9 { get; set; }
        public string LOAITHUE10 { get; set; }
        public string LOAITHUE11 { get; set; }
        public string LOAITHUE12 { get; set; }
        public string LOAITHUE13 { get; set; }
        public string LOAITHUE14 { get; set; }
        public string LOAITHUE15 { get; set; }
    }

    [Serializable]
    public class TongQuat
    {
        public string iKey { get; set; }
        public string MaYeuCau { get; set; }
        public int RowID { get; set; }
        public bool An { get; set; }
        public string NoiDungAn { get; set; }

        public string KHHD { get; set; }
        public string MAUSO { get; set; }
        public DateTime NGAYCT { get; set; }
        public string TENKH { get; set; }
        public string TENDV { get; set; }
        public string MAST { get; set; }
        public string DIACHI { get; set; }
        public string EMAIL { get; set; }
        public string EMAILCC { get; set; }
        public string TENNGANHANG { get; set; }
        public string DIENTHOAI { get; set; }
        public string HTTHANHTOAN { get; set; }
        public string SOTK { get; set; }
        public string LOAIHOADON { get; set; }
        public string GHICHU { get; set; }
        public string TENNGUOILAP { get; set; }
        public decimal TONGTIENHANG { get; set; }
        public int THUESUAT { get; set; }
        public decimal THUEGTGT { get; set; }


        public decimal TONGCONG { get; set; }
        public string TIENBANGCHU { get; set; }
        public int TRANGTHAIHUY { get; set; }
        public int TRANGTHAIIN { get; set; }
        public decimal TONGGIAGOC { get; set; }
        public decimal TONGCHENHLECH { get; set; }
        public decimal TIENTHUKHACH { get; set; }
        public string MAKH { get; set; }
        public decimal TONGTHUEPHI { get; set; }
        public string HOACHTOAN { get; set; }
        public decimal TONGCHIETKHAU { get; set; }
        public decimal TONGKHONGTHUE { get; set; }
        public string GHICHU_CT { get; set; }
        public string PLDOANHTHU { get; set; }
        public string MAPLDOANHTHU { get; set; }
        public string HDPHATSINH { get; set; }
        public string SOBITHU { get; set; }
        public string MAYEUCAU { get; set; }
        public decimal TONGPHIDV { get; set; }
        public int PHIDV { get; set; }
        public decimal TIENGTGT_PHIDV { get; set; }
        public decimal TONGPHIDOI { get; set; }
        public int THUESUAT_PHIDOI { get; set; }
        public decimal TIENGTGT_PHIDOI { get; set; }
        public decimal TONGPHIHOAN { get; set; }
        public int THUESUAT_PHIHOAN { get; set; }
        public decimal TIENGTGT_PHIHOAN { get; set; }
        public decimal TONGTIENVESAUTHUEPHI { get; set; }
        public decimal TONGTIENVESAUTHUEPHIKHAC { get; set; }
        public decimal TONGPHIDVSAUTHUE { get; set; }
        public int LOAIHD_YEUCAU { get; set; }
        public bool WEB { get; set; }
        public string NGUOILAP { get; set; }
        public DateTime NGAYLAP { get; set; }
        public decimal TONGPHIDVGOC { get; set; }
        public bool GIUSO { get; set; }
        public decimal TONGPHIEV { get; set; }
        public bool HDDIEUCHINH { get; set; }
        public int TONGSOVE { get; set; }
        public int SOLANIN_LIEN1 { get; set; }
        public int SOLANIN_LIEN2 { get; set; }
        public int SOLANIN_LIEN3 { get; set; }
    }

    [Serializable]
    public class DetailVe
    {
        public string SoVe { get; set; }
        public int SoLuong { get; set; } = 1;
        public decimal DonGia { get; set; } = 0;
        public decimal Thue { get; set; } = 0;
        public decimal ThueKhac { get; set; } = 0;
        public decimal PhiDoi { get; set; } = 0;

        public decimal PhiHoan { get; set; } = 0;
        public decimal TongCong { get; set; } = 0;
        public string GhiChu { get; set; }
        public string HangHangKhong { get; set; }
        public string NgayXuat { get; set; }

        public string HanhTrinh { get; set; }
    }
}
