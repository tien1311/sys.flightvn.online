using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class KhuyenMaiDaiLyModel
    {
        public List<DaiLyKhuyenMai> ListDaiLyKhuyenMai { get; set; }
        public List<ChuongTrinhKhuyenMai> ListChuongTrinhKhuyenMai { get; set; }
    }
    [Serializable]
    public class DaiLyKhuyenMai
    {
        public string RowID { get; set; }
        public string MaKH { get; set; }
        public int SoLuong { get; set; }
        public string Title { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int IDChuongTrinh { get; set; }
    }
    public class ChuongTrinhKhuyenMai
    {
        public string RowID { get; set; }
        public string Images { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public bool Status { get; set; }
        public int IDChuongTrinh { get; set; }
    }
}
