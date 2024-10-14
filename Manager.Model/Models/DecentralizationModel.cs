using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;

namespace Manager.Model.Models
{
    public class DecentralizationModel
    {
        public string RowID { get; set; }
        public string Phongban { get; set; }
        public string Maphongban { get; set; }

        public string Dschucnang { get; set; }
        public List<Checkchucnang> Dschucnangphongban { get; set; }


    }
    public class Dschucnang
    {
        public string RowID { get; set; }
        public string Chucnang { get; set; }

    }
    public class Checkchucnang
    {
        public string RowID { get; set; }
        public string Tinhnangmoi { get; set; }
        public string Thongbao { get; set; }
        public string Baocaove { get; set; }
        public string Noibo { get; set; }
        public string Daili { get; set; }
        public string Ketoan { get; set; }
        public string Kinhdoanh { get; set; }
        public string Phongve { get; set; }
        public string BPdoan { get; set; }
        public string Hoadon { get; set; }
        public string Ca { get; set; }
        public string Yensao { get; set; }
        public string Cs { get; set; }
        public string Data { get; set; }
        public string Setting { get; set; }
        public string Kythuat { get; set; }
        public string Dulich { get; set; }

    }
    public class DecentralizationMemberModel
    {

        public string Tennhanvien { get; set; }
        public string MaNV { get; set; }

        public string Dschucnang { get; set; }


    }
    public class Chitietnhanvien
    {

        public string Tennhanvien { get; set; }
        public string MaNV { get; set; }

        public string Phongban { get; set; }
        public List<Dschucnangmember> Dschucnangmember { get; set; }

    }
    public class Dschucnangmember
    {


        public string MaNV { get; set; }

        public string Dschucnang { get; set; }


    }


}