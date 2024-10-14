using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models.ViewModel
{
    [Serializable]
    public class ImportDoanhSoViewModel
    {
        public string STT { get; set; }
        public string MaKH { get; set; }
        public double Tong { get; set; }
        public double VU { get; set; }
        public double VN { get; set; }
        public double VJ { get; set; }
        public double QH { get; set; }
        public double IATA { get; set; }
        public double Khac { get; set; }
        public string Thang { get; set; }
        public string Nam { get; set; }
        public string TenCongTy { get; set; }
    }
    public class DoanhSoViewModel
    {
        public List<KinhDoanh> ListKinhDoanh { get; set; }
        public List<ImportDoanhSoViewModel> ListDoanhSo { get; set; }
        public double TongCong { get; set; }
        public double TongVU { get; set; }
        public double TongVN { get; set; }
        public double TongVJ { get; set; }
        public double TongQH { get; set; }
        public double TongIATA { get; set; }
        public double TongKhac { get; set; }
        public string TenKD { get; set; }
    }
}
