using Dapper;
using System;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using System.Web;
using System.Collections.Generic;
using System.Data;
using TangDuLieu;
//using RtfPipe.Tokens;
using Manager.Model.Models;

namespace Manager.DataAccess.Repository
{
    public class NotifyRepository
    {
        private readonly IConfiguration _configuration;
        DBase db = new DBase();
        public NotifyRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetNotifyTitle(string idTitle)
        {

            var databaseConfig = _configuration.GetSection("ConnectionStrings");
            var SQL_EV_MAIN_ConnectString = databaseConfig["SQL_EV_MAIN"];

            string Title = @"select TIEUDE from QLTHONGBAO where ROWID = '" + idTitle + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_ConnectString))
            {
                Title = conn.QueryFirst<string>(Title, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return Title;
        }

        public List<Member> Chitietmember(string member_kh)
        {
            List<Member> CV = new List<Member>();
            string sql = "SELECT * FROM member where member_kh='" + member_kh + "' ORDER BY member_ID DESC ";
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
                    CV.Add(thanhvien);
                }
            }
            return CV;
        }

        public string GetYahooID(string MaNV)
        {
            string result = "";
            string SQL_EV_MAIN_ConnectString = _configuration.GetConnectionString("SQL_EV_MAIN");
            string sql = @"select Yahoo from DM_NV where MANV = @MANV";
            using (var conn = new SqlConnection(SQL_EV_MAIN_ConnectString))
            {
                conn.Open();
                result = conn.QueryFirst<string>(sql, new { MANV = MaNV }, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            return result;
        }

        // Hàm để loại bỏ các thẻ HTML từ chuỗi HTML
        public string RemoveHtmlTags(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return HttpUtility.HtmlDecode(htmlDoc.DocumentNode.InnerText);
        }
    }
}
