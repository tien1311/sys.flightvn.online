using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models.ViewModel
{
    public class MemberListViewModel
    {
        public int STT { get; set; }
        public int ID { get; set; }
        public string TenDaiLy { get; set; }
        public string NguoiLienHe { get; set; }
        public string MaKH { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string NhanVien { get; set; }
        public string NguoiDaiDien { get; set; }
        public string TaiKhoan { get; set; }
        public List<MemberListViewModel> MemberList { get; set; }
    }



}
