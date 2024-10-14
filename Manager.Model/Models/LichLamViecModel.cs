using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;

namespace Manager.Model.Models
{
    public class LichLamViecModel
    {
        public string Ten { get; set; }
        public string DT { get; set; }
        public string Line { get; set; }
        public string Gio { get; set; }
        public string ThuHwa { get; set; }
        public string ThuNgay1 { get; set; }
        public string ThuNgay2 { get; set; }

        public string ThuNgay3 { get; set; }
        public string ThuNgay4 { get; set; }
        public string ThuNgay5 { get; set; }
        public string GioHwa_1 { get; set; }
        public string GioHwa_2 { get; set; }
        public string GioNgay1_1 { get; set; }
        public string GioNgay1_2 { get; set; }
        public string GioNgay2_1 { get; set; }
        public string GioNgay2_2 { get; set; }
        public string GioNgay3_1 { get; set; }
        public string GioNgay3_2 { get; set; }
        public string GioNgay4_1 { get; set; }
        public string GioNgay4_2 { get; set; }
        public string GioNgay5_1 { get; set; }
        public string GioNgay5_2 { get; set; }
        public string FileHwa { get; set; }
        public string FileNgay1 { get; set; }
        public string FileNgay2 { get; set; }
        public string FileNgay3 { get; set; }
        public string FileNgay4 { get; set; }
        public string FileNgay5 { get; set; }
        public string PhongBan { get; set; }
        public string HWA { get; set; }
        public string Ngay1 { get; set; }
        public string Ngay2 { get; set; }
        public string Ngay3 { get; set; }
        public string Ngay4 { get; set; }
        public string Ngay5 { get; set; }
        public string DemFileHW { get; set; }
        public string DemFileNgay1 { get; set; }
        public string DemFileNgay2 { get; set; }
        public string DemFileNgay3 { get; set; }
        public string DemFileNgay4 { get; set; }
        public string DemFileNgay5 { get; set; }

    }
}
