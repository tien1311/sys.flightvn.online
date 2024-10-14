using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class Danhsachmodel
    {
        public List<ListTen> ListTen { get; set; }

        public List<ListNhanVienDuLich> listNhanVienDuLich { get; set; }
        public List<Member> member { get; set; }
    }
    public class ListTen
    {
        public int RowID { get; set; }

        public string Ten { get; set; }
    }

    public class ListNhanVienDuLich
    {
        public int RowID { get; set; }
        public string Ten { get; set; }
        public string MaNV { get; set; }
    }
    public class ListNhanVienKinhDoanh
    {
        public int RowID { get; set; }
        public string Ten { get; set; }
        public string MaNV { get; set; }
        public string Email { get; set; }
    }
    public class ListNhanVienMK
    {
        public int RowID { get; set; }
        public string Ten { get; set; }
        public string Email { get; set; }
        public string Yahoo { get; set; }
    }

    public class Member
    {
        public int member_id { get; set; }

        public string member_idManager { get; set; }

        public int area_category { get; set; }

        public string member_kh { get; set; }

        public string member_company { get; set; }

        public string member_name { get; set; }

        public string member_code { get; set; }

        public string member_password { get; set; }

        public string member_email { get; set; }

        public string member_address { get; set; }

        public string member_address2 { get; set; }

        public string member_phone { get; set; }

        public string member_fax { get; set; }

        public string member_website { get; set; }

        public int country_id { get; set; }

        public int state_id { get; set; }

        public DateTime member_date { get; set; }

        public DateTime member_time { get; set; }

        public string member_hit { get; set; }

        public string member_isshow { get; set; }

        public string member_isnew { get; set; }

        public string member_ishot { get; set; }

        public string member_isactive { get; set; }

        public string Phi_vanchuyen { get; set; }

        public string KETOAN { get; set; }

        public string last_login { get; set; }

        public string lockReason { get; set; }

        public string member_status { get; set; }
        public string IsTravel { get; set; }
        public List<string> member_childs { get; set; }
        public List<NVkinhdoanh> ListKD { get; set; }
        public List<NVketoan> ListKt { get; set; }
        public List<DsZalo> DsZalo { get; set; }
        public List<DsSkype> DsSkype { get; set; }
        public List<DsDienthoai> DsDienthoai { get; set; }
        public List<DsTaikhoanphu> DsTaikhoanphu { get; set; }
    }
    public class NVkinhdoanh
    {
        public string RowID { get; set; }

        public string Ten { get; set; }

        public string Select { get; set; } = "";


    }
    public class NVketoan
    {
        public string RowID { get; set; }

        public string Ten { get; set; }

        public string Select { get; set; } = "";


    }
    public class DsZalo
    {
        public int ID { get; set; }

        public string Ten { get; set; }

        public string Ngaylienket { get; set; }

        public string Ngayhuy { get; set; }

        public string Lydo { get; set; }

        public string Active { get; set; }


    }
    public class DsSkype
    {
        public int ID { get; set; }

        public string Ten { get; set; }

        public string Nick { get; set; }

        public string Ngaytao { get; set; }

        public string Active { get; set; }
    }
    public class DsDienthoai
    {
        public int ID { get; set; }

        public string Ten { get; set; }

        public string Bophan { get; set; }

        public string Sdt { get; set; }

        public string Ngaytao { get; set; }

        public string Active { get; set; }
    }
    public class DsTaikhoanphu
    {
        public int ID { get; set; }

        public string Ten { get; set; }

        public string Email { get; set; }

        public string Tinhtrang { get; set; }


    }
}
