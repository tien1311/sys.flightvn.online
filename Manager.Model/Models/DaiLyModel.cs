using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;

namespace Manager.Model.Models
{
    public class DaiLyModel
    {
        public List<DaiLySearch> DSDaiLy { get; set; }
        public List<ThongTinLienHe> DSLienHe { get; set; }
        public DaiLyEV ThongTinDaiLy { get; set; }
        public List<DaiLyEV> ListDaiLy { get; set; }
        public List<CodeSignIn> ListCodeSignIn { get; set; }
        public List<PhoneModel> ListPhone { get; set; }
        public string Message { get; set; }
    }


    public class DaiLySearch
    {
        public int member_id { get; set; }
        public string member_kh { get; set; }
        public string member_company { get; set; }
        public string tinhtrang { get; set; }
        public string checkMaKH { get; set; }
    }
    public class DSHopDongDaiLy
    {
        public List<DaiLyEV> DSHopDong { get; set; }
        public string ThongBao { get; set; }
    }
    public class DaiLyEV
    {
        public int member_id { get; set; }
        public string member_kh { get; set; }
        public string member_hd { get; set; }
        public string member_file { get; set; }
        public List<string> file { get; set; }
        public string member_company { get; set; }
        public string tinhtrang { get; set; }
        public string tinhtrangdaily { get; set; }
        public string trangthai { get; set; }
        public string lydo { get; set; }
        public string ngaylap { get; set; }
        public string member_address { get; set; }
        public string HD { get; set; }
        public string member_tel { get; set; }
        public string member_email { get; set; }
        public string SoPhut { get; set; }
        public string NgayLapPBL { get; set; }
        public string CodeSignIn { get; set; }
        public string NhanVienKD { get; set; }
        public string SDTKinhDoanh { get; set; }
        public string KeToanEV { get; set; }
        public string SDTKeToan { get; set; }
        public string NoteKeToan { get; set; }
        public string SoDu { get; set; }
        public string Hang { get; set; }
        public string AmQuyChoPhep { get; set; }
        public string TienBaoLanh { get; set; }
        public string HangBay { get; set; }
        public string Mien { get; set; }
        public string Code { get; set; }
        public string Signin { get; set; }
        public string ChuKy { get; set; }
        public string HuyKyLaiHD { get; set; }
        public string HuyHD { get; set; }
        public string NhomVIP { get; set; }
        public string GhiChuVIP { get; set; }
        public string NoiDungVIP { get; set; }
        public string LoaiHopDong { get; set; }
        public string MaHopDong { get; set; }
        public string NgayLapHopDong { get; set; }
        public string GhiChuHopDong { get; set; }
    }
    public class DanhSachDaiLy
    {
        public List<DaiLyEV> ListDaiLy { get; set; }
        public List<KinhDoanh> ListKinhDoanh { get; set; }
        public string ThongBao { get; set; }
    }
    public class KinhDoanh
    {
        public string TenNV { get; set; }
        public string MaNV { get; set; }
    }
    public class ThongTinLienHe
    {

        public string Nick { get; set; }
        public string HoTen { get; set; }
        public string NgayCapNhat { get; set; }
        public string TinhTrang { get; set; }
    }
    public class PhoneModel
    {
        public int ROWID { get; set; }
        public int? AGENT_ID { get; set; }
        public string AGENT_KH { get; set; }
        public string OFFICE { get; set; }
        public string FULLNAME { get; set; }
        public string PHONE { get; set; }
        public string NICK { get; set; }
        public string TINHTRANG { get; set; }
        public string DATE { get; set; }
    }

    public class CodeSignIn
    {
        public string Hang { get; set; }
        public string Mien { get; set; }
        public string Code { get; set; }
        public string Signin { get; set; }
        public string MaKH { get; set; }
    }


}