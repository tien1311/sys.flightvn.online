using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class KhachHangModel
    {
        public string ID { get; set; }
        public string Hoten { get; set; }
        public string Chuc { get; set; }
        public string NGT { get; set; }
        public string Hang { get; set; }
        public string Mien { get; set; }
        public string SDT { get; set; }
        public string Diachi { get; set; }
        public List<QuaTangModel> Listquatang { get; set; }
        public string Nguoilap { get; set; }
        public string Ngaylap { get; set; }
        public string Ngaysinh { get; set; }
        public string Thangsinh { get; set; }
        public string Ghichu { get; set; }
        public string TangSN { get; set; }
        public string IsHotro { get; set; }
        public string Nhom { get; set; }
        public string Nhanvienkinhdoanh { get; set; }

    }
    public class QuaTangModel
    {
        public string ID { get; set; }
        public string Nam { get; set; }
        public string QuaTang { get; set; }
    }
}
