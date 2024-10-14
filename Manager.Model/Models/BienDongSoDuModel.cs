using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{


    public class BienDongSoDuModel
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public int STT { get; set; }
        public string MACK { get; set; }
        public string NGANHANG { get; set; }
        public string MAKH { get; set; }
        public string SOTIEN { get; set; }
        public string SOBUTTOAN { get; set; }
        public string NOIDUNG { get; set; }
        public string NGAYCK { get; set; }
        public string NGAYNHAN { get; set; }
        public string NOCO { get; set; }
        public string NGAYGUI { get; set; }
        public string NGAYSUA { get; set; }
        public string NGUOISUA { get; set; }

        public string DULIEUEFF { get; set; }

    }

    public class NganHang
    {
        public string MANH { get; set; }
        public string TENNH { get; set; }
    }
}
