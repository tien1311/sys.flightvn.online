using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class VeHoanModel
    {
        public int STT { get; set; }
        public string SoVe { get; set; }
        public string NgayGui { get; set; }
        public string TinhTrang { get; set; }
        public string DanhMuc { get; set; }
        public string ThongTin { get; set; }
        public string subject_id { get; set; }
        public string subject_isnew { get; set; }
        public string subject_ishot { get; set; }
        public string member_kh { get; set; }
        public string NgayHoan { get; set; }
        public string YeuCau { get; set; }
        public string Hang { get; set; }
        public string GiaTri { get; set; }
        public string ID_Hoan { get; set; }
        public string TenDL { get; set; }
        public string GhiChu { get; set; }
        public string NguoiXuLy { get; set; }
        public string NgayXuLy { get; set; }
        public string EMD { get; set; }
        public string SoLuongVeHoan { get; set; }
        public string SoLuongVeDangHoan { get; set; }
    }
    public class Hang
    {
        public string RefundID { get; set; }
        public string RefundName { get; set; }
    }
    public class NguoiXuLy
    {
        public string Name { get; set; }
    }
    public class DanhSachVeHoan
    {
        public List<Hang> listHang { get; set; }
        public List<VeHoanModel> listVeHoan { get; set; }
        public List<NguoiXuLy> listNguoiXuLy { get; set; }
    }
}
