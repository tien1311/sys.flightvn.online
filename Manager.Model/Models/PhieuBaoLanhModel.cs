using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class PhieuBaoLanhModel
    {
        public List<DSDaiLyModel> DSDaiLy { get; set; }
        public List<DSPhieuBaoLanhModel> DSPhieuBaoLanh { get; set; }
    }
    public class DSDaiLyModel
    {
        public string member_kh { get; set; }
        public string member_company { get; set; }

        public string member_email { get; set; }

        public string member_tel { get; set; }
    }
    public class DSPhieuBaoLanhModel
    {
        public int ID { get; set; }
        public string ID_KhachHang { get; set; }
        public double BaoLanh { get; set; }
        public string GhiChu { get; set; }
        public string NgayLap { get; set; }
        public string NhanVienLap { get; set; }
        public string NgaySua { get; set; }
        public string NhanVienSua { get; set; }
        public string NgayXoa { get; set; }
        public string NhanVienXoa { get; set; }
        public bool TinhTrang { get; set; }
        public string TenDaiLy { get; set; }
        public string MaPhieu { get; set; }
        public string SoPhut { get; set; }
    }
}
