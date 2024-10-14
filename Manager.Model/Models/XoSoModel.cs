using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class ThongTinXoSoModel
    {
        public List<XoSoDetail> ListXoSoDetail { get; set; }
        public List<XoSo> ListXoSo { get; set; }
    }
    [Serializable]
    public class XoSoDetail
    {
        public int ID { get; set; }
        public int IDXoSo { get; set; }
        public string MaKH { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongDaChon { get; set; }
        public string CreateEmployee { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class XoSo
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Description { get; set; }
        public int SoLuong { get; set; }

    }
    public class Number_XoSoDetail
    {
        public int ID { get; set; }
        public string MaKH { get; set; }
        public string Number { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
