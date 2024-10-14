using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models.ViewModel
{
    public class DoanhSoHangViewModel
    {
        public int ID { get; set; }
        public int STT { get; set; }
        public string Ngay { get; set; }
        public double VN { get; set; }
        public double QH { get; set; }
        public double VJ { get; set; }
        public double VU { get; set; }
        public double IATA { get; set; }
        public double KHAC { get; set; }
        public double TONG { get; set; }
    }
    public class DoanhSoHang_VN : DoanhSoHang_IATA
    {
        public double FHQ { get; set; }
        public double UJU { get; set; }
        public double JPQ { get; set; }
        public double LXQ { get; set; }
    }
    public class DoanhSoHang_IATA : DoanhSoHangViewModel
    {
        public double DS_1A { get; set; }
        public double DS_1B { get; set; }
        public double DS_1G { get; set; }
    }
    public class DoanhSoHang_BAMBOO : DoanhSoHangViewModel
    {
        public double MienTrung { get; set; }
        public double MienNam { get; set; }
    }
    public class ChiTietDoanhSoHang
    {
        public List<DoanhSoHang_VN> ChiTietDoanhSoHang_VN { get; set; }
        public List<DoanhSoHang_IATA> ChiTietDoanhSoHang_IATA { get; set; }
    }
}
