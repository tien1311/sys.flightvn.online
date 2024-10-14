using EasyInvoice.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TangDuLieu;
using Dapper;
using System.Data.SqlClient;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class ThongTinDaiLyRepository
    {
        DBase db = new DBase();
        string server_EV_MAIN; /*= "Data Source = .; Initial Catalog = Manager_V2; User ID = sa; Password=EnViet@123;";*/
        public ThongTinDaiLyRepository(IConfiguration configuration)
        {
            server_EV_MAIN = configuration.GetConnectionString("SQL_EV_MAIN_V2");
        }
        //Hiển thị thông tin cá nhân và thông tin hợp đồng
        public ThongTinDaiLyModel DSThongTinDaiLy()
        {

            ThongTinDaiLyModel acc = new ThongTinDaiLyModel();
            string SqlDetail = " SELECT *,convert(nvarchar(10),member_date,103) as member_dates FROM member";
            DataTable dt = db.ExecuteDataSet(SqlDetail, CommandType.Text, "server18", null).Tables[0];

            if (dt.Rows.Count > 0 && dt != null)
            {
                acc.member_id = int.Parse(dt.Rows[0]["member_id"].ToString());
                acc.member_name = dt.Rows[0]["member_name"].ToString();
                acc.member_kh = dt.Rows[0]["member_kh"].ToString();
                acc.member_company = dt.Rows[0]["member_company"].ToString();
                acc.member_email = dt.Rows[0]["member_email"].ToString();
                acc.member_address = dt.Rows[0]["member_address"].ToString();
                acc.member_phone = dt.Rows[0]["member_phone"].ToString();
                acc.member_code = dt.Rows[0]["member_code"].ToString();
                acc.member_fax = dt.Rows[0]["member_fax"].ToString();
                acc.member_date = DateTime.Parse(dt.Rows[0]["member_date"].ToString());
                string isshow = dt.Rows[0]["member_isshow"].ToString();
                string ishot = dt.Rows[0]["member_ishot"].ToString();
                string isnew = dt.Rows[0]["member_isnew"].ToString();
                string isActive = dt.Rows[0]["member_isactive"].ToString();
                acc.member_isshow = Convert.ToBoolean(isshow);
                acc.member_ishot = Convert.ToBoolean(Convert.ToInt16(ishot));
                acc.member_isnew = Convert.ToBoolean(Convert.ToInt16(isnew));
                acc.member_isactive = Convert.ToBoolean(Convert.ToInt16(isActive));

                acc.last_login = DateTime.Parse(dt.Rows[0]["last_login"].ToString());
                acc.lockReason = dt.Rows[0]["lockReason"].ToString();

                //Lấy thông tin kinh doanh

                string sql_kinhdoanh = @"select top 1  NV.Ten,NV.DienThoai
                                        from KHACHHANG_HOPDONG HD 
                                        left join DM_NV NV on HD.MAKINHDOANH = NV.MaNV

                                
                                where HD.MAKETOAN = '" + dt.Rows[0]["member_kh"].ToString() + "' order by NGAYLAP DESC";
                //string sql_kinhdoanh = @"select * from DM_NV where  tendangnhap = '" + dt.Rows[0]["member_website"].ToString() + "'";
                DataTable tb = db.ExecuteDataSet(sql_kinhdoanh, CommandType.Text, "serverUpdate", null).Tables[0];
                if (tb != null && tb.Rows.Count > 0)
                {
                    acc.nhanvien_kd = tb.Rows[0]["Ten"].ToString();
                    acc.sodienthoai_kd = tb.Rows[0]["DienThoai"].ToString();

                }
                else
                {
                    acc.nhanvien_kd = "Chưa cập nhật";
                    acc.sodienthoai_kd = "Chưa cập nhật";

                }


            }
            if (!string.IsNullOrEmpty(dt.Rows[0]["member_kh"].ToString()))
            {

                string SqlHD = @"SELECT KH_HD.XACNHANTT,KH_HD.MAKETOAN,KH_HD.TENCONGTY,KH_HD.MASOTHUE,KH_HD.GPKD,KH_HD.DIACHI,KH_HD.DIENTHOAI,KH_HD.FAX,KH_HD.EMAIL,KH_HD.NGUOIDAIDIEN,KH_HD.CHUCVU,KH_HD.NGAYSINH
                            ,KH_HD.DTNGUOIDAIDIEN,KH_HD.CMND,KH_HD.NGAYCAP + ' - ' + KH_HD.NOICAP as NGAYNOICAP,DM_NV.TEN,DM_NV.DIENTHOAI,DM_NV.LINE
                            FROM KHACHHANG_HOPDONG  KH_HD
                            LEFT JOIN DM_NV on KH_HD.MANVKETOAN  = DM_NV.MANV
                            WHERE MAKETOAN='" + dt.Rows[0]["member_kh"].ToString() + "'";
                DataTable dt_ketoan = db.ExecuteDataSet(SqlHD, CommandType.Text, "server37", null).Tables[0];
                if (dt_ketoan.Rows.Count > 0 && dt_ketoan != null)
                {
                    ThongTManager thongtin_hd = new ThongTManager();
                    thongtin_hd.MAKETOAN = dt_ketoan.Rows[0]["MAKETOAN"].ToString();
                    thongtin_hd.MASOTHUE = dt_ketoan.Rows[0]["MASOTHUE"].ToString();
                    thongtin_hd.NGAYNOICAP = dt_ketoan.Rows[0]["NGAYNOICAP"].ToString();
                    thongtin_hd.GPKD = dt_ketoan.Rows[0]["GPKD"].ToString();
                    thongtin_hd.CHUCVU = dt_ketoan.Rows[0]["CHUCVU"].ToString();
                    thongtin_hd.CMND = dt_ketoan.Rows[0]["CMND"].ToString();
                    thongtin_hd.DIACHI = dt_ketoan.Rows[0]["DIACHI"].ToString();
                    thongtin_hd.DIENTHOAI = dt_ketoan.Rows[0]["DIENTHOAI"].ToString();
                    thongtin_hd.DTNGUOIDAIDIEN = dt_ketoan.Rows[0]["DTNGUOIDAIDIEN"].ToString();
                    thongtin_hd.EMAIL = dt_ketoan.Rows[0]["EMAIL"].ToString();
                    thongtin_hd.FAX = dt_ketoan.Rows[0]["FAX"].ToString();
                    thongtin_hd.NGAYSINH = dt_ketoan.Rows[0]["NGAYSINH"].ToString();
                    thongtin_hd.NGUOIDAIDIEN = dt_ketoan.Rows[0]["NGUOIDAIDIEN"].ToString();
                    thongtin_hd.TENCONGTY = dt_ketoan.Rows[0]["TENCONGTY"].ToString();
                    thongtin_hd.NHANVIENKETOAN = dt_ketoan.Rows[0]["TEN"].ToString();
                    thongtin_hd.DIENTHOAIKETOAN = dt_ketoan.Rows[0]["DIENTHOAI"].ToString();
                    thongtin_hd.LINE = dt_ketoan.Rows[0]["LINE"].ToString();
                    acc.ThongTinHD = thongtin_hd;


                }

            }

            return acc;
        }

        //Lấy công nợ
        public Task<DuNoDaiLy> LayCongNoNhanVien(string maKH)
        {

            DuNoDaiLy result3 = new DuNoDaiLy();
            Agent agent = new Agent();

            try
            {
                string URL = "https://ev-agent.azurewebsites.net/api/debt/status";
                agent.AgentId = maKH;
                string DATA = JsonConvert.SerializeObject(agent);
                ServicePointManager.Expect100Continue = true;
                string strUrl = string.Format(URL);
                WebRequest requestObjPost = WebRequest.Create(strUrl);
                requestObjPost.Method = "POST";
                requestObjPost.ContentType = "application/json";
                requestObjPost.Headers.Add("x-functions-key", "JGNXlDFecS7RNLXcUalLAbSdqa28qS8tSPQUb4RvnesqSMHY/gAjmA==");
                using (var streamWrite = new StreamWriter(requestObjPost.GetRequestStream()))
                {
                    streamWrite.Write(DATA);
                    streamWrite.Flush();
                    streamWrite.Close();
                    var httpResponse = (HttpWebResponse) requestObjPost.GetResponseAsync().GetAwaiter().GetResult();
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result2 = streamReader.ReadToEnd();
                            result3 = JsonConvert.DeserializeObject<DuNoDaiLy>(result2);

                        }
                    }
                }

                return Task.FromResult(result3);

            }
            catch (Exception)
            {
                return Task.FromResult(result3);
                throw;
            }

        }

        //Xác nhận thông tin hợp đồng
        public bool XacNhan(string member_kh)
        {
            string sql = @"UPDATE KHACHHANG_HOPDONG SET XACNHANTT='1' WHERE MAKETOAN='" + member_kh + "'";
            int result = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", null);
            if (result > 0)
            {
                return true;
            }
            return false;

        }

        //Đổi mật khẩu
        public bool DoiMatKhau(string email, string matkhaucu, string matkhaumoi, string re_matkhaumoi)
        {
            if (matkhaumoi == "")
            {
                return false;
            }
            //Mã hóa password
            string StrPassword = db.MD5Encrypt(matkhaucu);

            string sql = @"select * from member where member_email = '" + email + "' and member_password = '" + StrPassword + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb.Rows.Count > 0)
            {

                if (matkhaumoi == re_matkhaumoi)
                {
                    string StrPasssword_new = db.MD5Encrypt(matkhaumoi);
                    string update = @"update member set member_password = '" + StrPasssword_new + "' where  member_email = '" + email + "' and member_password = '" + StrPassword + "'";
                    int result = db.ExecuteNoneQuery(update, CommandType.Text, "server18", null);
                    if (result > 0)
                    {
                        return true;
                    }
                    return false;

                }
                else
                {
                    return false;
                }
            }
            return false;


        }
        public Task<List<CongNoNhanVienModel>> CongNoNVAsync()
        {
            List<CongNoNhanVienModel> ListCNNV = new List<CongNoNhanVienModel>();
            try
            {
                string sql = @"select * from CONGNO_NHANVIEN where not  exists (select * from CONGNO_NHANVIEN_LOAITRU L where L.MANV = CONGNO_NHANVIEN.MANV) and NumberCode in (select top 1 MAX(NumberCode) from CONGNO_NHANVIEN) and SOTIENNO < 0 and MANV <> '' order by SOTIENNO";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    ListCNNV = (List<CongNoNhanVienModel>)conn.Query<CongNoNhanVienModel>(sql, null, commandType: CommandType.Text, commandTimeout: 90);
                    conn.Dispose();
                }
                return Task.FromResult(ListCNNV);
            }
            catch (Exception)
            {
                return Task.FromResult(ListCNNV);
            }

        }
    }
}
