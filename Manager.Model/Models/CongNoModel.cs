using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class CongNoModel
    {
        public double SoDuDauNgay { get; set; }
        public double SoDuCuoiNgay { get; set; }
        public List<ChiTietCongNoModel> ChiTiet { get; set; }
        public double TongGiaBan { get; set; }
        public double TongNo { get; set; }
        public double TongCo { get; set; }
        public string TenDL { get; set; }
        public string MaKH { get; set; }
    }
    public class ChiTietCongNoModel
    {
        public string ChungTu { get; set; }
        public string NgayChungTu { get; set; }
        public string NgayXuat { get; set; }
        public string Code_signin { get; set; }
        public string PNR { get; set; }
        public string DienGiai { get; set; }
        public double GiaCoBan { get; set; }
        public double ChietKhau { get; set; }

        public double LuyKe { get; set; }
        public double PhatSinhNo { get; set; }
        public double PhatSinhCo { get; set; }
        public string NgayChungTuEV { get; set; }
        //public double No { get; set; }
        //public double Co { get; set; }
        public double No
        {
            get
            {
                return PhatSinhNo;
            }
        }
        public double Co
        {
            get
            {
                return PhatSinhCo;
            }
        }


    }
}

