using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class ListVeSotModel
    {
        public List<VeSotModel> VeSot { get; set; }
        public List<ListTenNVModel> ListTen { get; set; }
        public string ThongBao { get; set; }
        public string NguoiGuiSMS { get; set; }
        public string NgayGuiSMS { get; set; }
    }
    public class VeSotModel
    {
        public string Expr1 { get; set; } = "";
        public string Ma { get; set; } = "";
        public string NgayMua { get; set; } = "";
        public string ID_HanhT { get; set; } = "";
        public string SoLuongVeSot { get; set; }
        public string DienThoai { get; set; }
        public string TKNockt { get; set; } = "";
        public string PNR { get; set; } = "";
        public string MaHK { get; set; } = "";
        public string TinhTrang { get; set; } = "";
        public string NgayUp { get; set; } = "";
        public string ID { get; set; } = "";
        public string NgayCapNhat { get; set; } = "";
        public string NhanVienTao { get; set; } = "";
        public string Nghiepvu { get; set; }
    }
    public class ListTenNVModel
    {
        public string Expr1 { get; set; }
        public string Ma { get; set; }
    }
    public class ChiTietVeSot
    {
        public int RowIndex { get; set; }
        public int ID { get; set; }
        public string MAHHK { get; set; }
        public string MAKH { get; set; }
        public string PNR { get; set; }
        public string SoVe { get; set; }
        public string GiaMua { get; set; }
        public string PhiDVMua { get; set; }
        public string LoaiPhi { get; set; }
        public string PhiXuatVe { get; set; }
        public string PhiDVBan { get; set; }
        public string PhiHoan { get; set; }
        public string ChietKhau { get; set; }
        public string MaGioiThieu { get; set; }
        public string NguoiGioiThieu { get; set; }
        public List<LoaiPhiXuatModel> ListLoaiPhi { get; set; } = new List<LoaiPhiXuatModel>();
    }
}
