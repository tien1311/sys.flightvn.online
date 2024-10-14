using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models.ViewModel
{
    public class DanhSachHoaDonModel
    {


        public List<HoaDonModel> ds_HoaDon { get; set; }
        public List<SoVeHoaDonModel> ds_SoVeHoaDon { get; set; }
    }
    public class HoaDonModel
    {

        public int STT { get; set; }

        public string KyHieu { get; set; }
        public string SoChungTu { get; set; }
        public string NgayChungTu { get; set; }

        public string NguoiThanhToan { get; set; }

        public string MaSoThue { get; set; }
        public string DiaChi { get; set; }
        public string HinhThucThanhToan { get; set; }
        public double TongTienHang { get; set; }

        public double ThueSuat { get; set; }
        public double TienGTGT { get; set; }
        public double ThuePhiKhac { get; set; }

        public double TongPhiDoi { get; set; }
        public double TongPhiHoan { get; set; }
        public double TongPhiDV { get; set; }
        public double TongCongTien { get; set; }

        public string TTHuy { get; set; }

        public string MaKH { get; set; }

        public int SoLanIn { get; set; }

        public string HoaDonDieuChinh { get; set; }

    }
    public class SoVeHoaDonModel
    {

        public int STT { get; set; }

        public string SoVe { get; set; }
        public string HanhTrinh { get; set; }

        public int SoLuong { get; set; }

        public double DonGia { get; set; }
        public double ThanhTien { get; set; }
        public double Vat { get; set; }
        public double ThueKhac { get; set; }
        public double PhiDoi { get; set; }
        public double PhiHoan { get; set; }
        public double TongCong { get; set; }
    }

    public class ReturnObject
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Result { get; set; }


    }

    public class ReturnObject1
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }


    }


}
