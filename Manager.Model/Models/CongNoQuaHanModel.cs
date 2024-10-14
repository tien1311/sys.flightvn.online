using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class CongNoQuaHanModel
    {
        public string ID { get; set; }
        public string TieuDe { get; set; }
        public string Thang { get; set; }
        public string UpdateBy { get; set; }
        public List<DSCongNoNVQuaHan> ListCongNoNVQuaHan { get; set; }
    }
    public class DSCongNoNVQuaHan
    {
        public string ID { get; set; }
        public string ID_DEBT { get; set; }
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string SoTienNo { get; set; }
        public string ThoiGianXuatVe { get; set; }
        public string DuNo { get; set; }
        public string GhiChu { get; set; }
        public string TinhTrang { get; set; }
    }
}
