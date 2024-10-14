using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class KhoaCodeDaiLyModel
    {
        public List<DSDaiLyModel> DSDaiLy { get; set; }
        public List<DSKhoaCodeDaiLyModel> DSKhoaCodeDaiLy { get; set; }
        public List<TinhTrangKhoa> DSTinhTrangKhoa { get; set; }
    }
    public class TinhTrangKhoa
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ID_Dept { get; set; }
        public int IsActive { get; set; }
    }
    public class NoiDungKhoa
    {
        public int ROWID { get; set; }
        public string NoiDung { get; set; }
        public string NoiDungTimKiem { get; set; }
        public string TinhTrang { get; set; }
        public string TieuDe { get; set; }
        public int IDTinhTrang { get; set; }
    }
    public class TieuDeModel
    {
        public int ROWID { get; set; }
        public string TieuDe { get; set; }
    }
    public class DSKhoaCodeDaiLyModel
    {
        public int ID { get; set; }
        public string MaKH { get; set; }
        public string TenDaiLy { get; set; }
        public string NoiDungKhoa { get; set; }
        public string IDNoiDungKhoa { get; set; }
        public string NgayLap { get; set; }
        public string NguoiLap { get; set; }
        public string NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public string NgayXoa { get; set; }
        public string NguoiXoa { get; set; }
        public bool Status { get; set; }
        public string TinhTrangKhoa { get; set; }
        public string MailCC { get; set; }
    }
}
