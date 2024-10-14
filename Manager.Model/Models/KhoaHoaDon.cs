using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;

namespace Manager.Model.Models
{
    public class KhoaHoaDon
    {


        public List<KieuKhoa> KieuKhoa { get; set; }
        public List<PhanMem> PhanMem { get; set; }
        public string txt_PVL1 { get; set; }
        public string txt_PVL2 { get; set; }
        public string txt_PVL3 { get; set; }
        public string txt_PVL4 { get; set; }
        public string txt_GHTN { get; set; }
        public string txt_ChuaL1 { get; set; }
        public string txt_ChuaL2 { get; set; }
        public string txt_ChuaL3 { get; set; }
        public string txt_ChuaL4 { get; set; }
        public string txt_KHHD_1 { get; set; }
        public string txt_KHHD_2 { get; set; }
        public string txt_KHHD_3 { get; set; }
        public string txt_KHHD_4 { get; set; }
        public string txt_Serial_L1 { get; set; }
        public string txt_Serial_L2 { get; set; }
        public string txt_Serial_L3 { get; set; }
        public string txt_Serial_L4 { get; set; }
        public string txt_NgayKhoa { get; set; }

    }
    public class KieuKhoa
    {

        public string ItemKey { get; set; }
        public string ItemValue { get; set; }


    }
    public class PhanMem
    {
        public string RowID { get; set; }
        public string Phanmem { get; set; }
        public string Thongbao { get; set; }
        public string Select { get; set; }
    }



}