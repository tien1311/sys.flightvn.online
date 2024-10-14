using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class ThongBaoModel
    {
        public List<ThongBao_DL> list_TBDL { get; set; }
        public List<ThongBao_NB> list_TBNB { get; set; }
        public List<ThongBao_ALL> list_TBALL { get; set; }
        public List<ThongBao_KT> list_TBKT { get; set; }
        public List<ThongBao_PV> list_TBPV { get; set; }
    }
    public class ThongBao_DL
    {
        public int ID { get; set; }
        public string MaKH { get; set; }
        public string TenDaiLy { get; set; }
        public string NguoiKhoa { get; set; }
        public string NgayKhoa { get; set; }
        public string TinhTrang { get; set; }
        public bool DaXem { get; set; }
        public string FileDinhKem { get; set; }
        public string FileName { get; set; }
        public string TieuDe { get; set; }
        public string NVKD { get; set; }
        public string MaKD { get; set; }
        public string MaNV { get; set; }
        public string MaNVLap { get; set; }
    }
    public class ThongBao_NB
    {
        public int ID { get; set; }
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string NguoiKhoa { get; set; }
        public string NgayKhoa { get; set; }
        public string TinhTrang { get; set; }
        public bool DaXem { get; set; }
        public string FileDinhKem { get; set; }
        public string FileName { get; set; }
        public string TieuDe { get; set; }
    }
    public class KHACHHANGVIP
    {
        public long ROWID { get; set; } 
        public string HOTEN { get; set; }
        public string DONVI { get; set; } 
        public string DIACHI { get; set; } 
        public string SODIENTHOAI { get; set; }
        public string KHUYENMAI { get; set; } 
        public string NGUOILAP { get; set; } 
        public DateTime? NGAYLAP { get; set; } 
        public string NGUOISUA { get; set; } 
        public DateTime? NGAYSUA { get; set; } 
        public DateTime? NgaySinh { get; set; }
        public string GHICHU { get; set; } 
        public string NguoiGioiThieu { get; set; } 
        public string Hang { get; set; } 
        public string Mien { get; set; } 
        public bool? IsTangSN { get; set; }
        public bool? IsHoTro { get; set; }
        public string NHOM { get; set; }
        public string NHANVIENKINHDOANH { get; set; }
    }

    public class ThongBaoSinhNhatDaiLi
    {
        public int ID { get; set; }
        public string KhachHang { get; set; }
        public DateTime NgaySinh { get; set; }
        public string NguoiTiepQuan { get; set; }
        public DateTime NgayTao { get; set; }
        public string ChucVu { get; set; }
        public string Hang { get; set; }
        public string Mien { get; set; }
        public bool? DaXem { get; set; }
    }

    public class ChiTietTB
    {
        [DataType(DataType.MultilineText)]
        public string NoiDung { get; set; }
        public string TieuDe { get; set; }
        public string ID { get; set; }
    }
    public class ThongBao_ALL
    {
        public int ID { get; set; }
        public string NguoiLap { get; set; }
        public string NgayLap { get; set; }
        public string NguoiSua { get; set; }
        public string NgaySua { get; set; }
        public string TinhTrang { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public string PhongBan { get; set; }
    }
    public class ThongBao_KT
    {
        public int ID { get; set; }
        public string NguoiLap { get; set; }
        public string NgayLap { get; set; }
        public string NguoiSua { get; set; }
        public string NgaySua { get; set; }
        public string TinhTrang { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
    }
    public class ThongBao_PV
    {
        public int ID { get; set; }
        public string NguoiLap { get; set; }
        public string NgayLap { get; set; }
        public string NguoiSua { get; set; }
        public string NgaySua { get; set; }
        public string TinhTrang { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
    }
}
