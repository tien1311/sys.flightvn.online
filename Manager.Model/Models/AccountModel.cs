using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;

namespace Manager.Model.Models
{
    public class AccountModel
    {
        public int RowID { get; set; }
        public string MaNV { get; set; }
        public string Ten { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }

        public string ChiNhanh { get; set; }
        public string SinhNhat { get; set; }

        public string DiaChiThuongTru { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string PhongBan { get; set; }
        public string MaPhongBan { get; set; }
        public string ThongBao { get; set; }

        public string DienThoaiCN { get; set; }
        public string DienThoaiSMS { get; set; }
        public string Line { get; set; }
        public string DiaChiTamTru { get; set; }
        public string CMND { get; set; }
        public string NoiCap { get; set; }
        public DateTime NgayCap { get; set; }
        public DateTime NgayLamViec { get; set; }
        public string ChucVu { get; set; }
        public string SoTK { get; set; }
        public string Skyper { get; set; }
        public string TenHinh { get; set; }
        public string TenDangNhap { get; set; }
        public string MaSoThue { get; set; }
        public string NgayCapMST { get; set; }
        public string Per_Group { get; set; }
        public string Active { get; set; }
        public string StickNote { get; set; }
        public string TNMoi { get; set; }
        public string TBao { get; set; }
        public string BCVe { get; set; }
        public string NBo { get; set; }
        public string DLi { get; set; }
        public string KToan { get; set; }
        public string KDoanh { get; set; }
        public string PVe { get; set; }
        public string BPDoan { get; set; }
        public string HDon { get; set; }
        public string CA { get; set; }
        public string YSao { get; set; }
        public string CS { get; set; }
        public string DTa { get; set; }
        public string STing { get; set; }
        public string KThuat { get; set; }
        public string Dulich { get; set; }



        public string TNMoiTV { get; set; }
        public string TBaoTV { get; set; }
        public string BCVeTV { get; set; }
        public string NBoTV { get; set; }
        public string DLiTV { get; set; }
        public string KToanTV { get; set; }
        public string KDoanhTV { get; set; }
        public string PVeTV { get; set; }
        public string BPDoanTV { get; set; }
        public string HDonTV { get; set; }
        public string CATV { get; set; }
        public string YSaoTV { get; set; }
        public string CSTV { get; set; }
        public string DTaTV { get; set; }
        public string STingTV { get; set; }
        public string KThuatTV { get; set; }
        public string DulichTV { get; set; }
        public List<FileDinhKem> ListFile { get; set; }
    }
    public class FileDinhKem
    {
        public string Title { get; set; }
        public string TenFile { get; set; }
        public DateTime NgayUp { get; set; }
    }
    public class NhanVienModel
    {
        public string MaNV { get; set; }
        public string Ten { get; set; }
        public string PhongBan { get; set; }
        public string PhanViec { get; set; }
        public string QuyDinh { get; set; }
        public string PhanViecChung { get; set; }
        public string QuyDinhChung { get; set; }
        public string CongViec { get; set; }
        public string Noidung { get; set; }
        public string Noidungchung { get; set; }
        public string ThaoTac { get; set; }
    }
    public class PhongBanModel
    {
        public string PB { get; set; }
        public string MaPB { get; set; }

    }
    public class CongViecModel
    {
        public List<PhongBanModel> ListPB { get; set; }
        public List<NhanVienModel> ListNV { get; set; }
    }


}