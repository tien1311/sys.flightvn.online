using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{

    public class GuiMailDaiLytModel
    {
        public int ID { get; set; }
        public int STT { get; set; }
        public string MAKH { get; set; }
        public string DAILY { get; set; }
        public string PNR { get; set; }
        public string HANG { get; set; }
        public string MAIL { get; set; }
        public string MAILCC { get; set; }
        public string TINHTRANG { get; set; }
        [DataType(DataType.MultilineText)]
        public string NOIDUNG { get; set; }
        [DataType(DataType.MultilineText)]
        public string DIEUKIEN { get; set; }
        public string URL { get; set; }
        public string NGUOIGUI { get; set; }
        public string NGAYGUI { get; set; }
        public string SOPHIEU { get; set; }

        public string PHIDICHVU { get; set; }

    }

    public class TongQuatMail
    {
        public int TimeOutEdit { get; set; } = 0;
        public List<GuiMailDaiLytModel> ListGuiMail { get; set; } = null;
        public List<ChiTietVe> ListChiTietVe { get; set; } = null;
        public List<LoaiPhiXuatModel> ListPhiXuat { get; set; } = null;
    }
    public class LoaiPhiXuatModel
    {
        public string Name { get; set; }
        public double ExchangeRate { get; set; }
        public double Amount { get; set; }
        public string Selected { get; set; } = "";
    }

    public class GuiMailDaiLyKDModel
    {
        public int ID { get; set; }
        public string STT { get; set; }
        public string MAKH { get; set; }
        public string DAILY { get; set; }
        public string PNR { get; set; }
        public string HANG { get; set; }
        public string MAIL { get; set; }
        public string MAILCC { get; set; }
        public string TINHTRANG { get; set; }
        [DataType(DataType.MultilineText)]
        public string NOIDUNG { get; set; }
        [DataType(DataType.MultilineText)]
        public string DIEUKIEN { get; set; }
        public string URL { get; set; }
        public string NGUOIGUI { get; set; }
        public string NGAYGUI { get; set; }
        public string SOPHIEU { get; set; }

        public string PHIDICHVU { get; set; }
        public string ACCOUNT { get; set; }

    }

    public class ThongTinChung
    {
        public string MAKH { get; set; }
        public string DAILY { get; set; }
        public string MANV { get; set; }
        public string PNR { get; set; }
        public string HANG { get; set; }
        public string TINHTRANG { get; set; }
        public DateTime NGAYNHAPLIEU { get; set; }
        public string GHICHU { get; set; }

    }

    public class ChiTietVe
    {
        public int ID { get; set; }
        public string MAHHK { get; set; }
        public string SoVe { get; set; }
        public string GiaMua { get; set; }
        public string PhiDVMua { get; set; }
        public string PhiXuatVe { get; set; }
        public string LoaiPhi { get; set; } = "";
        public string PhiDVBan { get; set; }
        public string PhiHoan { get; set; }
        public string ChietKhau { get; set; }
        public string PNR { get; set; }
        public string MAKH { get; set; }
        public string MAKH_EFF { get; set; }
        public string GHICHU { get; set; }
        public string TenNhanVien { get; set; } = "";
        public string NGAYSUA { get; set; } = "";

        public string MAGIOITHIEU { get; set; }
        public string NGUOIGIOITHIEU { get; set; }
        public string NGUOISUA { get; set; }
        public string CODE { get; set; }
        public bool TinhTrangEFF { get; set; } = true;
        public int SoLanSua { get; set; } = 0;
        public DateTime? NgaySua_New { get; set; }
        public int SoLuong { get; set; } = 0;
    }
    public class GuiMailDaiLyModel
    {

        public List<ListTieuDe> ListNoiDung { get; set; }
        public List<GuiMailDaiLyKDModel> Guimailkinhdoanh { get; set; }

    }


    public class DanhSachXuatDoiVe
    {
        public List<GuiMailDaiLytModel> ListXuatDoiVe { get; set; }
        public List<NhanVien> ListNhanVien { get; set; }

    }
    public class DanhSachTongNhanVienXuatDoiVe
    {
        public List<NhanVienXuatDoiVe> ListNhanVienXuatDoiVe { get; set; }
        public List<NhanVien> ListNhanVien { get; set; }
    }

    public class NhanVienXuatDoiVe
    {
        public int STT { get; set; }
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string SLXuatVe { get; set; }
        public string SLDoiVe { get; set; }
    }

    public class NhanVien
    {
        public string TenNhanVien { get; set; }
        public string TenDangNhap { get; set; }
    }
    public class ListTieuDe
    {
        public int RowID { get; set; }

        public string TieuDe { get; set; }

        public string NoiDungTimKiem { get; set; }
    }

}
