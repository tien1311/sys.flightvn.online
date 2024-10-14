using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Services.Model.Request
{


    public class YeuCauXuatHDDTRequest
    {


    }



    public class SaveYCRequest
    {
        public string username { get; set; }

        public YeuCauXuat_EInvoiceRequest requestInvoice { get; set; }

    }


    public class DeleteCancelYCXHDDTRequest : BaseRequest
    {

        public string AgentID { get; set; } = "";

        public string RequestID { get; set; } = "";
    }

    public class DanhSachYCXHDDTRequest : BaseRequest
    {
        public string AgentId { get; set; } = "";
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int Status { get; set; }
        public string RequestID { get; set; }
    }


    [Serializable]
    public class StatusDSYC
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }



    [Serializable]
    public class YeuCauXuat_EInvoiceRequest
    {

        public RequestInvoice_TQ tongQuat { get; set; } = null;
        public List<TicketInfoDto> danhSachVe { get; set; } = null;


    }

    [Serializable]
    public class RequestInvoice_TQ
    {
        public string MaYeuCau { get; set; }
        public string NguoiXuLy { get; set; }
        public DateTime? NgayXuLy { get; set; }
        public string KyKeKhai { get; set; }
        public string AgentCode { get; set; } = "0";
        public string TenKH { get; set; }
        public string MaKH { get; set; }
        public string TenKH_KT { get; set; }
        public string TenDV { get; set; }
        public string TenNguoiLap { get; set; }
        public string MaST { get; set; }
        public string DiaChi { get; set; }
        public string GhiChu { get; set; }
        public int TinhTrang { get; set; }
        public string TenTinhTrang { get; set; }
        public string SoCT { get; set; }
        public string KHHD { get; set; }
        public string IKey { get; set; }
        public string MauSo { get; set; }
        public DateTime NgayLap { get; set; }
        public string LyDoHuy { get; set; }
        public string DiaChiNhan { get; set; }
        public string NguoiNhan { get; set; }
        public string SDTNhan { get; set; }
        public string TinhThanh { get; set; }
        public DateTime NgayLapHD { get; set; }
        public int LoaiHoaDon { get; set; } = 1; // VNa = 1 ?
        public decimal TongPhiDichVu { get; set; }
        public int DieuChinhPhiDV { get; set; } = 1;
        public decimal PhiDVEV { get; set; }
        public string HDDV { get; set; }
        public DateTime? NgayHDDV { get; set; }
        public bool HDDT { get; set; } = false;

        public string Email { get; set; }
    }


    [Serializable]
    public class TicketInfoDto
    {
        public string SoVe { get; set; }
        public int SoLuong { get; set; } = 1;
        public decimal DonGia { get; set; } = 0;
        public decimal Thue { get; set; } = 0;
        public decimal ThueKhac { get; set; } = 0;
        public decimal PhiDoi { get; set; } = 0;
        public decimal TongCong { get; set; } = 0;
        public decimal PhiEV { get; set; } = 0;

        public decimal PhiHoan { get; set; } = 0;
        public string GhiChu { get; set; }
        public string HangHangKhong { get; set; }
        public string NgayXuat { get; set; }

        public string HanhTrinh { get; set; }
    }


}
