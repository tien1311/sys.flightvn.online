using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class DinhDanhBaoLuuModel
    {
        public string ID { get; set; }
        public string HanDangBan { get; set; }
        public string Loai { get; set; }
        public string TrangThai { get; set; }
        public string Hidden { get; set; }
        public string TenNB { get; set; }
        public string DienThoaiNB { get; set; }
        public string GhiChu { get; set; }
        public string PNR { get; set; }
        public string SoVe { get; set; }
        public string Hang { get; set; }
        public string NgayDang { get; set; }
        public string MaKH { get; set; }
        public List<TenHanhKhach> ListTenHK { get; set; }
    }
    public class TenHanhKhach
    {
        public string TenHK { get; set; }
        public double GiaBan { get; set; }
        public double GiaGiam { get; set; }
    }
    public class DDBL
    {
        public string DieuKien { get; set; }
        public string NoiDungDK { get; set; }
        public List<DinhDanhBaoLuuModel> ListDDBL { get; set; }
    }
}
