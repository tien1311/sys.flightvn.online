using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;



namespace Manager.Model.Models
{



    public class SubjectModel
    {

        public int subject_id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string Date { get; set; }

        public string URL { get; set; }

        public int subject_isnew { get; set; }
        public string subject_number { get; set; }
        public int subject_isshow { get; set; }
        public int subject_ishot { get; set; }
        public int subject_com { get; set; }
        public string subject_status_daily { get; set; }
        public int stt { get; set; }
        public string subject_date { get; set; }
        public string subject_content { get; set; }
        public string SecName { get; set; }
        public string subject_updateby { get; set; }
        public string subject_picnote { get; set; }
        public string subject_name { get; set; }
        public string subject_code { get; set; }
        public string auth { get; set; }
        public string DelayMoney { get; set; }
        public string NguoiGui { get; set; }
        public string SoDienThoai { get; set; }
        public string SoVeEMD { get; set; }
        public string Skype { get; set; }
        public string Ticket { get; set; }
        public string NoiDungHuyHanhTrinh { get; set; }
        public string NgayHoan { get; set; }
        public string TenKhachEMD { get; set; }
        public string code_ticket { get; set; }
        public string YeuCau { get; set; }
        public string Remark { get; set; }
        public string NgayBay { get; set; }
        public string SoNgayHoan { get; set; }
        public bool IsCheckHoan { get; set; }
        public bool IsCheckEMD { get; set; }
        public List<NhanKyHoanVe> ListNhatKy { get; set; }
        public List<Hang> listHang { get; set; }
        public List<NhatKyXuLy> ListNhatKyXuLy { get; set; }
        public List<TinhTrangYeuCau> ListTinhTrangYeuCau { get; set; }
    }
    public class NhanKyHoanVe
    {
        public string ThuocTinh { get; set; }
        public string GiaTriCu { get; set; }
        public string GiaTriMoi { get; set; }
        public string NhanVienSua { get; set; }
        public string NgaySua { get; set; }

    }
    public class NhatKyXuLy
    {
        public string subject_update { get; set; }
        public string subject_updateby { get; set; }
        public string Skype { get; set; }
        public string DienThoaiNhanVien { get; set; }
        public string subject_picnote { get; set; }
        public string TinhTrang { get; set; }
        public string ngayxuly { get; set; }
    }
    public class TinhTrangYeuCau
    {
        public string value { get; set; }
        public string text { get; set; }
    }
}








