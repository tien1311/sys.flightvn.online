using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class KhachHangVIPModel
    {
        public List<KhachHangVIP> ListKhachHangVIP { get; set; }
        public List<NhomKhachHangVIP> ListNhomKhachHangVIP { get; set; }
        public List<NhomDL> ListNhomDL { get; set; }

    }
    public class NhomDL
    {
        public int IDNhom { get; set; }
        public string TenNhom { get; set; }
        public string NoiDung { get; set; }
    }

    public class KhachHangVIP
    {
        public int ID { get; set; }
        public string MaKH { get; set; }
        public string TenCty { get; set; }
        public string Nhom { get; set; }
        public string Ghichu { get; set; }
    }
    public class NhomKhachHangVIP
    {
        public int ID { get; set; }
        public string MaKH { get; set; }
        public string TenCty { get; set; }
        public string Nhom { get; set; }
        public string Ghichu { get; set; }

    }
    public class Show
    {
        public int ID { get; set; }
        public int IDNhom { get; set; }
        public string MaKH { get; set; }
        public string TenCty { get; set; }
        public string Nhom { get; set; }
        public string Ghichu { get; set; }
        public string Status { get; set; }
        public List<NhomDL> ListNhomDL { get; set; }
    }
}
