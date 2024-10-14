using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Web;

using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Dapper;
//using FluentFTP;
using Manager.Model.Models;

namespace Manager.DataAccess.Repository
{

    public class MemberRepository
    {
        DBase db = new DBase();
        public Danhsachmodel DanhSachMember()
        {
            Danhsachmodel result = new Danhsachmodel();
            result.ListTen = DSTen();
            return result;
        }

        public Danhsachmodel DanhSachNhanVienDuLich()
        {
            Danhsachmodel result = new Danhsachmodel();
            result.listNhanVienDuLich = DSDuLich();
            return result;
        }

        public List<ListNhanVienDuLich> DSDuLich()
        {
            List<ListNhanVienDuLich> result = new List<ListNhanVienDuLich>();
            string sql_NoiDung = " SELECT TENDANGNHAP,Ten as TENNV, MaNV FROM DM_NV WHERE MaPhongBan='DL' AND TINHTRANG=1 ";
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListNhanVienDuLich ten = new ListNhanVienDuLich();
                        ten.Ten = dt.Rows[i]["TenNV"].ToString();
                        ten.MaNV = dt.Rows[i]["MaNV"].ToString();
                        result.Add(ten);
                    }
                }
            }
            return result;
        }

        public List<Member> Chitietmember(string khoachinh)
        {

            List<Member> CV = new List<Member>();
            string sql = "SELECT * FROM member where member_ID='" + khoachinh + "' ORDER BY member_ID DESC ";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    Member thanhvien = new Member();
                    thanhvien.member_id = int.Parse(tb.Rows[i]["member_id"].ToString());
                    thanhvien.member_company = tb.Rows[i]["member_company"].ToString();
                    thanhvien.member_name = tb.Rows[i]["member_name"].ToString();
                    thanhvien.member_kh = tb.Rows[i]["member_kh"].ToString();
                    thanhvien.member_code = tb.Rows[i]["member_code"].ToString();
                    thanhvien.member_email = tb.Rows[i]["member_email"].ToString();
                    thanhvien.member_address = tb.Rows[i]["member_address"].ToString();
                    thanhvien.member_phone = tb.Rows[i]["member_phone"].ToString();
                    thanhvien.member_fax = tb.Rows[i]["member_fax"].ToString();
                    thanhvien.member_isactive = tb.Rows[i]["member_isactive"].ToString();
                    thanhvien.member_status = tb.Rows[i]["member_status"].ToString();
                    thanhvien.member_isshow = tb.Rows[i]["member_isshow"].ToString();
                    string sql_member_child = "select Code from MemberChild where MemIDRoot = " + tb.Rows[i]["member_id"].ToString();
                    DataTable tb_member_child = db.ExecuteDataSet(sql_member_child, CommandType.Text, "server18", null).Tables[0];
                    List<string> member_childs = new List<string>();
                    if (tb_member_child != null)
                    {
                        if (tb_member_child.Rows.Count > 0)
                        {
                            for (int y = 0; y < tb_member_child.Rows.Count; y++)
                            {
                                member_childs.Add(tb_member_child.Rows[y][0].ToString());
                            }
                        }
                        thanhvien.member_childs = member_childs;
                    }

                    if (tb.Rows[i]["last_login"].ToString() == "")
                    {
                        thanhvien.last_login = tb.Rows[i]["last_login"].ToString();
                    }
                    else
                    {
                        thanhvien.last_login = DateTime.Parse(tb.Rows[i]["last_login"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    thanhvien.member_website = tb.Rows[i]["member_website"].ToString();
                    thanhvien.lockReason = tb.Rows[i]["lockReason"].ToString();
                    thanhvien.KETOAN = tb.Rows[i]["KETOAN"].ToString();
                    thanhvien.IsTravel = tb.Rows[i]["IsTravel"].ToString();



                    List<NVkinhdoanh> KD = new List<NVkinhdoanh>();
                    string sql_NoiDung = " SELECT MaNV, TENDANGNHAP,Ten as TENNV FROM DM_NV WHERE MaPhongBan='KD' AND TINHTRANG=1 ";
                    DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            KD.Insert(0, new NVkinhdoanh() { RowID = "0", Select = "0", Ten = "---Chọn nhân viên kinh doanh--- " });
                            for (int a = 0; a < dt.Rows.Count; a++)
                            {
                                NVkinhdoanh ten = new NVkinhdoanh();

                                if (thanhvien.member_website == dt.Rows[a]["TenDangNhap"].ToString())
                                {
                                    ten.RowID = dt.Rows[a]["MaNV"].ToString();
                                    ten.Ten = dt.Rows[a]["TenNV"].ToString();
                                    ten.Select = "selected";
                                }
                                else
                                {
                                    ten.RowID = dt.Rows[a]["MaNV"].ToString();
                                    ten.Ten = dt.Rows[a]["TenNV"].ToString();
                                }

                                KD.Add(ten);
                            }
                            thanhvien.ListKD = KD;
                        }
                    }

                    List<NVketoan> KT = new List<NVketoan>();
                    string sql_KT = " SELECT MaNV, TENDANGNHAP,Ten as TENNV FROM DM_NV WHERE MaPhongBan='KT' AND TINHTRANG=1 ";
                    DataTable dtkt = db.ExecuteDataSet(sql_KT, CommandType.Text, "server37", null).Tables[0];
                    if (dtkt != null)
                    {
                        if (dtkt.Rows.Count > 0)
                        {
                            KT.Insert(0, new NVketoan() { RowID = "0", Select = "0", Ten = "---Chọn nhân viên kế toán--- " });
                            for (int b = 0; b < dtkt.Rows.Count; b++)
                            {
                                NVketoan nvkt = new NVketoan();

                                if (thanhvien.KETOAN.Trim() == dtkt.Rows[b]["TenDangNhap"].ToString().Trim())
                                {
                                    nvkt.RowID = dtkt.Rows[b]["MaNV"].ToString();
                                    nvkt.Ten = dtkt.Rows[b]["TenNV"].ToString();
                                    nvkt.Select = "selected";
                                }
                                else
                                {
                                    nvkt.RowID = dtkt.Rows[b]["MaNV"].ToString();
                                    nvkt.Ten = dtkt.Rows[b]["TenNV"].ToString();
                                }
                                KT.Add(nvkt);
                            }
                            thanhvien.ListKt = KT;
                        }
                    }
                    List<DsZalo> Zl = new List<DsZalo>();
                    string sql_Zalo = " SELECT * FROM zalo_daily WHERE MaKH='" + tb.Rows[i]["member_kh"].ToString() + "' ORDER BY RowID DESC ";
                    DataTable dtzalo = db.ExecuteDataSet(sql_Zalo, CommandType.Text, "server18", null).Tables[0];
                    if (dtzalo != null)
                    {
                        if (dtzalo.Rows.Count > 0)
                        {
                            for (int b = 0; b < dtzalo.Rows.Count; b++)
                            {
                                DsZalo dszalo = new DsZalo();
                                dszalo.Ten = dtzalo.Rows[b]["ZaloName"].ToString();
                                dszalo.Ngaylienket = DateTime.Parse(dtzalo.Rows[b]["AuthenDate"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                                if (dtzalo.Rows[b]["UnauthenDate"].ToString() == "")
                                {
                                    dszalo.Ngayhuy = dtzalo.Rows[b]["UnauthenDate"].ToString();
                                }
                                else
                                {
                                    dszalo.Ngayhuy = DateTime.Parse(dtzalo.Rows[b]["UnauthenDate"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                                }

                                dszalo.Lydo = dtzalo.Rows[b]["UnauthenReason"].ToString();
                                dszalo.Active = dtzalo.Rows[b]["isActive"].ToString();
                                Zl.Add(dszalo);
                            }
                            thanhvien.DsZalo = Zl;
                        }
                    }
                    List<DsSkype> SK = new List<DsSkype>();
                    string sql_SK = " SELECT yahoo_nick,yahoo_fullname,convert(nvarchar(10),yahoo_date,103)as yahoo_date,yahoo_isshow,Category FROM yahoo_booker WHERE member_id='" + khoachinh + "' ORDER BY yahoo_id DESC ";
                    DataTable dtsk = db.ExecuteDataSet(sql_SK, CommandType.Text, "server18", null).Tables[0];
                    if (dtsk != null)
                    {
                        if (dtsk.Rows.Count > 0)
                        {
                            for (int b = 0; b < dtsk.Rows.Count; b++)
                            {
                                DsSkype dssk = new DsSkype();
                                dssk.Ten = dtsk.Rows[b]["yahoo_fullname"].ToString();
                                dssk.Nick = dtsk.Rows[b]["yahoo_nick"].ToString();
                                dssk.Ngaytao = dtsk.Rows[b]["yahoo_date"].ToString();
                                dssk.Active = dtsk.Rows[b]["yahoo_isshow"].ToString();
                                SK.Add(dssk);
                            }
                            thanhvien.DsSkype = SK;
                        }
                    }
                    List<DsDienthoai> Sdt = new List<DsDienthoai>();
                    string sql_Sdt = " SELECT * FROM PHONE WHERE AGENT_ID='" + khoachinh + "' ORDER BY ROWID DESC ";
                    DataTable dtsdt = db.ExecuteDataSet(sql_Sdt, CommandType.Text, "server18", null).Tables[0];
                    if (dtsdt != null)
                    {
                        if (dtsdt.Rows.Count > 0)
                        {
                            for (int b = 0; b < dtsdt.Rows.Count; b++)
                            {
                                DsDienthoai dssdt = new DsDienthoai();
                                dssdt.Ten = dtsdt.Rows[b]["FULLNAME"].ToString();
                                dssdt.Bophan = dtsdt.Rows[b]["OFFICE"].ToString();
                                dssdt.Sdt = dtsdt.Rows[b]["PHONE_NUMBER"].ToString();
                                dssdt.Ngaytao = DateTime.Parse(dtsdt.Rows[b]["DATE_CREATED"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                                dssdt.Active = dtsdt.Rows[b]["TINHTRANG"].ToString();
                                Sdt.Add(dssdt);
                            }
                            thanhvien.DsDienthoai = Sdt;
                        }
                    }
                    List<DsTaikhoanphu> TKP = new List<DsTaikhoanphu>();
                    string sql_Tkp = " select MBC.*, (case when IsKhoa = 0 then N'Hoạt Động' else N'Ngừng Hoạt Động' end) as TinhTrang from MemberClient MBC left join member M on MBC.MemIDRoot = M.member_id where member_kh like N'" + tb.Rows[i]["member_kh"].ToString() + "' ";
                    DataTable dttkp = db.ExecuteDataSet(sql_Tkp, CommandType.Text, "server18", null).Tables[0];
                    if (dttkp != null)
                    {
                        if (dttkp.Rows.Count > 0)
                        {
                            for (int b = 0; b < dttkp.Rows.Count; b++)
                            {
                                DsTaikhoanphu dstkp = new DsTaikhoanphu();
                                dstkp.Ten = dttkp.Rows[b]["HoTen"].ToString();
                                dstkp.Email = dttkp.Rows[b]["Email"].ToString();
                                dstkp.Tinhtrang = dttkp.Rows[b]["TinhTrang"].ToString();

                                TKP.Add(dstkp);
                            }
                            thanhvien.DsTaikhoanphu = TKP;
                        }
                    }

                    CV.Add(thanhvien);

                }

            }


            return CV;
        }
        public Danhsachmodel SearchMember(string TenDL, string NguoiLH, string MaKH, string Email, string Phone)
        {
            string StrSearch = " 1=1 ";
            if (TenDL != null)
            {
                StrSearch += " AND [member_company] like N'%" + TenDL.Trim() + "%'";
            }
            if (NguoiLH != null)
            {
                StrSearch += "AND  [member_name] like N'%" + NguoiLH.Trim() + "%'";
            }
            if (MaKH != null)
            {
                StrSearch += "AND  [member_kh] like '%" + MaKH.Trim() + "%'";
            }
            if (Email != null)
            {
                StrSearch += "AND  [member_email]='" + Email.Trim() + "'";
            }
            if (Phone != null)
            {
                StrSearch += "AND  [member_phone]='" + Phone.Trim() + "'";
            }
            Danhsachmodel result = new Danhsachmodel();
            List<Member> CV = new List<Member>();
            string sql = "SELECT member_ID,member_company,member_name,member_code,member_email,member_address,member_website,member_isactive FROM member WHERE " + StrSearch + " ORDER BY member_ID DESC ";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null)
            {

                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    Member thanhvien = new Member();
                    thanhvien.member_id = int.Parse(tb.Rows[i]["member_id"].ToString());
                    thanhvien.member_company = tb.Rows[i]["member_company"].ToString();
                    thanhvien.member_name = tb.Rows[i]["member_name"].ToString();
                    thanhvien.member_code = tb.Rows[i]["member_code"].ToString();
                    thanhvien.member_email = tb.Rows[i]["member_email"].ToString();
                    thanhvien.member_address = tb.Rows[i]["member_address"].ToString();
                    thanhvien.member_isactive = tb.Rows[i]["member_isactive"].ToString();
                    CV.Add(thanhvien);
                }
                result.member = CV;
                result.ListTen = DSTen();
            }


            return result;
        }

        public bool UpdatePassword(string password, string memid)
        {
            string StrPassword = db.MD5Encrypt(password);
            string sqlUp = " UPDATE member SET member_status = 1, member_password='" + StrPassword + "',last_login=GETDATE() WHERE member_id='" + memid + "' ";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@member_password", password));
            int i = db.ExecuteNoneQuery(sqlUp, CommandType.Text, "server18", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool UpdateMember(string memberid, string khuvuc, string company, string name, string makh, string code, string email, string address, string phone, string fax, int isactive, bool isshow, string kinhdoanh, string ketoan, string dulich, string[] member_childs, string UpdatedBy)
        {
            string nvkd = "";
            string nvkt = "";
            if (kinhdoanh == "0")
            {
                nvkd = "";
            }
            else
            {
                string sql_dskd = " SELECT * from DM_NV WHERE  MaNV='" + kinhdoanh + "'";
                string dskd = db.ExecuteDataSet(sql_dskd, CommandType.Text, "server37", null).Tables[0].Rows[0]["TENDANGNHAP"].ToString();
                nvkd = dskd;
            }
            if (ketoan == "0")
            {
                nvkt = "";
            }
            else
            {
                string sql_dskt = " SELECT * from DM_NV WHERE  MaNV='" + ketoan + "'";
                string dskt = db.ExecuteDataSet(sql_dskt, CommandType.Text, "server37", null).Tables[0].Rows[0]["TENDANGNHAP"].ToString();
                nvkt = dskt;
            }
            if (phone == null)
            {
                phone = "";
            }
            if (fax == null)
            {
                fax = "";
            }
            string Sqlupdate = " UPDATE [member] SET member_status = 1, area_category='" + khuvuc + "',member_kh='" + makh + "',member_company=N'" + company + "',member_name=N'" + name + "',member_code='" + code + "',member_email='" + email + "',member_address=N'" + address + "',member_phone='" + phone + "',member_fax='" + fax + "',member_isshow='" + isshow + "',member_isactive='" + isactive + "',member_website='" + nvkd + "',KETOAN='" + nvkt + "',IsTravel='" + dulich + "',last_login=GETDATE(),UpdatedBy = '" + UpdatedBy + "',UpdatedDate = GETDATE()  WHERE member_id='" + memberid + "' ";
            int y = db.ExecuteNoneQuery(Sqlupdate, CommandType.Text, "server18", null);
            if (y > 0)
            {
                string SqlDelete = "Delete MemberChild where MemIDRoot = " + memberid;
                db.ExecuteNoneQuery(SqlDelete, CommandType.Text, "server18", null);
                for (int i = 0; i < member_childs.Length; i++)
                {
                    string SqlInsert = "Insert into MemberChild(MemIDRoot, Code) values('" + memberid + "', '" + member_childs[i] + "')";
                    int result_insert = db.ExecuteNoneQuery(SqlInsert, CommandType.Text, "server18", null);
                }

                return true;
            }
            else
                return false;
        }
        public bool ActiveMember(string Active, int RowID)
        {
            string SqlUpdate = "";
            if (Active == "1")
            {
                SqlUpdate = @"UPDATE member SET member_isactive = 1,last_login=GETDATE() WHERE member_id = '" + RowID + "' ";
            }
            else
            {
                SqlUpdate = @"UPDATE member SET member_isactive = 0,last_login=GETDATE() WHERE member_id = '" + RowID + "' ";
            }
            int result = db.ExecuteNoneQuery(SqlUpdate, CommandType.Text, "server18", null);

            if (Active == "1")
            {
                return false;
            }
            else
                return true;
        }
        public List<ListTen> DSZalo()
        {
            List<ListTen> result = new List<ListTen>();
            string sql_NoiDung = " SELECT TENDANGNHAP,Ten as TENNV FROM DM_NV WHERE MaPhongBan='KD' AND TINHTRANG=1 ";
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListTen ten = new ListTen();
                        ten.Ten = dt.Rows[i]["TenNV"].ToString();
                        result.Add(ten);
                    }

                }
            }

            return result;
        }
        public List<ListTen> DSTen()
        {
            List<ListTen> result = new List<ListTen>();
            string sql_NoiDung = " SELECT TENDANGNHAP,Ten as TENNV FROM DM_NV WHERE MaPhongBan='KD' AND TINHTRANG=1 ";
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListTen ten = new ListTen();
                        ten.Ten = dt.Rows[i]["TenNV"].ToString();
                        result.Add(ten);
                    }

                }
            }

            return result;
        }
        public Member Dangkithanhvien()
        {


            Member thanhvien = new Member();

            List<NVkinhdoanh> KD = new List<NVkinhdoanh>();
            string sql_NoiDung = " SELECT MaNV, TENDANGNHAP,Ten as TENNV FROM DM_NV WHERE MaPhongBan='KD' AND TINHTRANG=1 ";
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    KD.Insert(0, new NVkinhdoanh() { RowID = "0", Select = "0", Ten = "---Chọn nhân viên kinh doanh--- " });
                    for (int a = 0; a < dt.Rows.Count; a++)
                    {
                        NVkinhdoanh ten = new NVkinhdoanh();

                        ten.RowID = dt.Rows[a]["MaNV"].ToString();
                        ten.Ten = dt.Rows[a]["TenNV"].ToString();


                        KD.Add(ten);
                    }
                    thanhvien.ListKD = KD;
                }
            }

            List<NVketoan> KT = new List<NVketoan>();
            string sql_KT = " SELECT MaNV, TENDANGNHAP,Ten as TENNV FROM DM_NV WHERE MaPhongBan='KT' AND TINHTRANG=1 ";
            DataTable dtkt = db.ExecuteDataSet(sql_KT, CommandType.Text, "server37", null).Tables[0];
            if (dtkt != null)
            {
                if (dtkt.Rows.Count > 0)
                {
                    KT.Insert(0, new NVketoan() { RowID = "0", Select = "0", Ten = "---Chọn nhân viên kế toán--- " });
                    for (int b = 0; b < dtkt.Rows.Count; b++)
                    {
                        NVketoan nvkt = new NVketoan();


                        nvkt.RowID = dtkt.Rows[b]["MaNV"].ToString();
                        nvkt.Ten = dtkt.Rows[b]["TenNV"].ToString();

                        KT.Add(nvkt);
                    }
                    thanhvien.ListKt = KT;
                }
            }




            return thanhvien;
        }
        public Member searchHD(string mahd)
        {
            Member chitiet = new Member();
            string SqlHD = "SELECT distinct a.*, b.MAKETOAN ,c.TenDangNhap FROM HOPDONG_KYQUY a,KHACHHANG_HOPDONG b,DM_NV c WHERE b.ID=a.ID AND b.MANVKETOAN = c.MaNV AND  a.IDHOPDONG='" + mahd.Trim() + "'";
            DataTable dt = db.ExecuteDataSet(SqlHD, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {


                        chitiet.member_company = dt.Rows[i]["TENCONGTY"].ToString();
                        chitiet.member_name = dt.Rows[i]["NGUOIDAIDIEN"].ToString();
                        chitiet.member_kh = dt.Rows[i]["MAKETOAN"].ToString();
                        chitiet.member_email = dt.Rows[i]["EMAIL"].ToString();
                        chitiet.member_address = dt.Rows[i]["DIACHI"].ToString();
                        chitiet.member_phone = dt.Rows[i]["DIENTHOAI"].ToString();
                        chitiet.member_fax = dt.Rows[i]["FAX"].ToString();
                        chitiet.member_website = dt.Rows[i]["NGUOILAP"].ToString();
                        chitiet.KETOAN = dt.Rows[i]["TenDangNhap"].ToString();
                        chitiet.member_idManager = dt.Rows[i]["IDHOPDONG"].ToString();
                        chitiet.member_password = "enviet";
                        chitiet.member_code = RandomString(12);


                        List<NVkinhdoanh> KD = new List<NVkinhdoanh>();
                        string sql_NoiDung = " SELECT MaNV, TENDANGNHAP,Ten as TENNV FROM DM_NV WHERE MaPhongBan='KD' AND TINHTRANG=1 ";
                        DataTable dtkd = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
                        if (dtkd != null)
                        {
                            if (dtkd.Rows.Count > 0)
                            {
                                KD.Insert(0, new NVkinhdoanh() { RowID = "0", Select = "0", Ten = "---Chọn nhân viên kinh doanh--- " });
                                for (int a = 0; a < dtkd.Rows.Count; a++)
                                {
                                    NVkinhdoanh ten = new NVkinhdoanh();
                                    if (chitiet.member_website == dtkd.Rows[a]["TenDangNhap"].ToString())
                                    {
                                        ten.RowID = dtkd.Rows[a]["MaNV"].ToString();
                                        ten.Ten = dtkd.Rows[a]["TenNV"].ToString();
                                        ten.Select = "selected";
                                    }
                                    else
                                    {
                                        ten.RowID = dtkd.Rows[a]["MaNV"].ToString();
                                        ten.Ten = dtkd.Rows[a]["TenNV"].ToString();
                                    }


                                    KD.Add(ten);
                                }
                                chitiet.ListKD = KD;
                            }
                        }

                        List<NVketoan> KT = new List<NVketoan>();
                        string sql_KT = " SELECT MaNV, TENDANGNHAP,Ten as TENNV FROM DM_NV WHERE MaPhongBan='KT' AND TINHTRANG=1 ";
                        DataTable dtkt = db.ExecuteDataSet(sql_KT, CommandType.Text, "server37", null).Tables[0];
                        if (dtkt != null)
                        {
                            if (dtkt.Rows.Count > 0)
                            {
                                KT.Insert(0, new NVketoan() { RowID = "0", Select = "0", Ten = "---Chọn nhân viên kế toán--- " });
                                for (int b = 0; b < dtkt.Rows.Count; b++)
                                {
                                    NVketoan nvkt = new NVketoan();
                                    if (chitiet.KETOAN.Trim() == dtkt.Rows[b]["TenDangNhap"].ToString().Trim())
                                    {
                                        nvkt.RowID = dtkt.Rows[b]["MaNV"].ToString();
                                        nvkt.Ten = dtkt.Rows[b]["TenNV"].ToString();
                                        nvkt.Select = "selected";
                                    }
                                    else
                                    {
                                        nvkt.RowID = dtkt.Rows[b]["MaNV"].ToString();
                                        nvkt.Ten = dtkt.Rows[b]["TenNV"].ToString();
                                    }

                                    KT.Add(nvkt);
                                }
                                chitiet.ListKt = KT;
                            }
                        }

                    }
                }
            }

            return chitiet;
        }
        string RandomString(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            chars += "0123456789";
            chars += "abcdefghijklmnopqrstuvwxyz";
            Random ran = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[ran.Next(s.Length)]).ToArray());
        }
        public bool SaveHopDong(string khuvuc, string mahd, string company, string name, string makh, string code, string password, string email, string address, string phone, string fax, string kinhdoanh, string ketoan, string isactive, string isshow, string Tenthaydoi, string dulich)
        {
            string nvkd = "";
            string nvkt = "";
            if (kinhdoanh == "0")
            {
                nvkd = "";
            }
            else
            {

                string sql_dskd = " SELECT * from DM_NV WHERE  MaNV='" + kinhdoanh + "'";
                string dskd = db.ExecuteDataSet(sql_dskd, CommandType.Text, "server37", null).Tables[0].Rows[0]["TENDANGNHAP"].ToString();
                nvkd = dskd;
            }
            if (ketoan == "0")
            {
                nvkt = "";
            }
            else
            {

                string sql_dskt = " SELECT * from DM_NV WHERE  MaNV='" + ketoan + "'";
                string dskt = db.ExecuteDataSet(sql_dskt, CommandType.Text, "server37", null).Tables[0].Rows[0]["TENDANGNHAP"].ToString();
                nvkt = dskt;
            }

            string Strpass = db.Encrypt(password, "hahoainam", true);
            string sqlCheck = " SELECT member_id,member_email FROM [member] WHERE member_email='" + email.ToLower() + "' OR member_code='" + code + "' ";
            DataTable dtCheck = db.ExecuteDataSet(sqlCheck, CommandType.Text, "server18", null).Tables[0];
            if (dtCheck.Rows.Count <= 0)
            {
                string SqlChelbb = " SELECT * FROM  CT_BAN_GIAO_CODE WHERE SoBienBan='" + mahd + "' and MaHeThong='TKDL' ";
                DataTable dtChelbb = db.ExecuteDataSet(SqlChelbb, CommandType.Text, "server37", null).Tables[0];
                if (dtChelbb.Rows.Count > 0)
                {
                    string SqlInsert = "INSERT INTO [member] ([area_category],[member_kh],[member_company],[member_name],[member_code],[member_password],[member_email],[member_address],[member_phone],[member_fax],[member_website],[country_id],[state_id],[member_date],[member_time],[member_hit],[member_isshow],[member_isnew],[member_ishot],[member_isactive],[Phi_vanchuyen],[KETOAN],[IsTravel],[CreatedBy],[CreatedDate]) VALUES(" + khuvuc + ",'" + makh + "',N'" + company + "',N'" + name + "','" + code + "','" + Strpass + "','" + email + "',N'" + address + "','" + phone + "','" + fax + "','" + nvkd + "',0,'0',GETDATE(),GETDATE(),0,'" + isshow + "',0,0,'" + isactive + "',0,N'" + nvkd + "','" + dulich + "','" + Tenthaydoi + "',GETDATE())";
                    int dtInsert = db.ExecuteNoneQuery(SqlInsert, CommandType.Text, "server18", null);
                    if (dtInsert > 0)
                    {
                        string sql = @"UPDATE CT_BAN_GIAO_CODE SET TaiKhoan=N'" + code.Trim() + "',MatKhau='" + Strpass + "',NguoiSua=N'" + Tenthaydoi + "',NgaySua=convert(nvarchar(10),getdate(),101) WHERE SoBienBan='" + mahd.Trim() + "' and MaHeThong='TKDL'";
                        int sqlRk = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", null);
                        if (sqlRk > 0)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return false;
        }
    }
}
