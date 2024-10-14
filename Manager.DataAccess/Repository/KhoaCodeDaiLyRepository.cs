using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Data.SqlClient;
using Dapper;
using static Manager.DataAccess.Helpers.HoaDonHelper;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class KhoaCodeDaiLyRepository
    {
        private string SQL_EV_MAIN_V2; 
        private string SQL_EV_MAIN; 
        private string SQL_Agent_MAIN;
        DBase db = new DBase();
        Mail mailDb = new Mail("EVM_KHOACODEDAILY");
        KhoaCodeDaiLyModel khoaCodeDaiLy = new KhoaCodeDaiLyModel();

        public KhoaCodeDaiLyRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN_V2 = configuration.GetConnectionString("SQL_EV_MAIN_V2");
            SQL_EV_MAIN = configuration.GetConnectionString("SQL_EV_MAIN");
            SQL_Agent_MAIN = configuration.GetConnectionString("SQL_Agent_MAIN");
        }
        public KhoaCodeDaiLyModel DSThongBaoDaiLy(string MaPB)
        {
            List<DSKhoaCodeDaiLyModel> listDSKhoaCodeDaiLy = new List<DSKhoaCodeDaiLyModel>();
            string sql = @"select K.*, TT.Name as TinhTrangKhoa from KhoaCodeDaiLy K
                            Left join [SERVER37].[Manager_V2].[dbo].[AGENT_NOTIFICATION_STATUS] TT on TT.ID =  K.TinhTrangKhoa
                            where Status = 1 and MaPhongBan = '" + MaPB + "' order by NgayLap Desc";
            using (var conn = new SqlConnection(SQL_EV_MAIN))
            {
                listDSKhoaCodeDaiLy = (List<DSKhoaCodeDaiLyModel>)conn.Query<DSKhoaCodeDaiLyModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            if (listDSKhoaCodeDaiLy.Count > 0)
            {
                for (int i = 0; i < listDSKhoaCodeDaiLy.Count; i++)
                {
                    listDSKhoaCodeDaiLy[i].NgayLap = DateTime.Parse(listDSKhoaCodeDaiLy[i].NgayLap.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    if (listDSKhoaCodeDaiLy[i].NgaySua != null)
                    {
                        listDSKhoaCodeDaiLy[i].NgaySua = DateTime.Parse(listDSKhoaCodeDaiLy[i].NgaySua.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        listDSKhoaCodeDaiLy[i].NgaySua = "";
                    }
                }
            }
            khoaCodeDaiLy.DSKhoaCodeDaiLy = listDSKhoaCodeDaiLy;
            khoaCodeDaiLy.DSTinhTrangKhoa = DSTinhTrangKhoa(MaPB);
            return khoaCodeDaiLy;
        }
        public List<DSDaiLyModel> DSDaiLy(string MaKH)
        {
            List<DSDaiLyModel> listDSDaiLy = new List<DSDaiLyModel>();
            string SqlDaiLy = " SELECT *,convert(nvarchar(10),member_date,103) as member_dates FROM member WHERE member_kh='" + MaKH + "'";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                listDSDaiLy = (List<DSDaiLyModel>)conn.Query<DSDaiLyModel>(SqlDaiLy, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            return listDSDaiLy;
        }
        public List<TieuDeModel> GetTieuDe(int ID)
        {
            List<TieuDeModel> result = new List<TieuDeModel>();
            string sql = " SELECT ROWID, TieuDe FROM QLTHONGBAO WHERE IDTinhTrang = " + ID;
            using (var conn = new SqlConnection(SQL_EV_MAIN))
            {
                result = (List<TieuDeModel>)conn.Query<TieuDeModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public string GetNoiDung(int id)
        {
            string result = "";
            string sql_NoiDung = " SELECT * FROM QLTHONGBAO WHERE RowID = " + id;
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["NoiDung"].ToString();
                }
            }
            return result;
        }
        public List<TinhTrangKhoa> DSTinhTrangKhoa(string MaPhongBan)
        {
            List<TinhTrangKhoa> result = new List<TinhTrangKhoa>();
            string sql = @"select * from AGENT_NOTIFICATION_STATUS where ID_Dept = '" + MaPhongBan + "' and IsActive = 1";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<TinhTrangKhoa>)conn.Query<TinhTrangKhoa>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        //public KhoaCodeDaiLyModel SearchKCDL(string MaKH)
        //{

        //    List<DSKhoaCodeDaiLyModel> listDSKhoaCodeDaiLy = new List<DSKhoaCodeDaiLyModel>();
        //    string sql = @"select * from KhoaCodeDaiLy where TinhTrang = 1 and ID_KhachHang='" + MaKH + "' order by NgayLap Desc";
        //    DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
        //    if (tb != null)
        //    {
        //        for (int i = 0; i < tb.Rows.Count; i++)
        //        {
        //            DSKhoaCodeDaiLyModel khoaCodeDaiLy = new DSKhoaCodeDaiLyModel();

        //            khoaCodeDaiLy.ID = int.Parse(tb.Rows[i]["ID"].ToString());
        //            khoaCodeDaiLy.MaKH = tb.Rows[i]["MaKH"].ToString();
        //            khoaCodeDaiLy.TenDaiLy = tb.Rows[i]["TenDaiLy"].ToString();
        //            khoaCodeDaiLy.NoiDungKhoa = tb.Rows[i]["NoiDungKhoa"].ToString();
        //            khoaCodeDaiLy.NguoiLap = tb.Rows[i]["NguoiLap"].ToString();
        //            khoaCodeDaiLy.NgayLap = DateTime.Parse(tb.Rows[i]["NgayLap"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
        //            khoaCodeDaiLy.NguoiSua = tb.Rows[i]["NguoiSua"].ToString();
        //            khoaCodeDaiLy.IDNoiDungKhoa = tb.Rows[i]["IDNoiDungKhoa"].ToString();
        //            khoaCodeDaiLy.MailCC = tb.Rows[i]["MailCC"].ToString();
        //            if (tb.Rows[i]["TinhTrangKhoa"].ToString() == "1")
        //            {
        //                khoaCodeDaiLy.TinhTrangKhoa = "Khóa code";
        //            }
        //            if (tb.Rows[i]["TinhTrangKhoa"].ToString() == "2")
        //            {
        //                khoaCodeDaiLy.TinhTrangKhoa = "Mở code";
        //            }
        //            if (tb.Rows[i]["TinhTrangKhoa"].ToString() == "3")
        //            {
        //                khoaCodeDaiLy.TinhTrangKhoa = "Thông báo";
        //            }

        //            if (tb.Rows[i]["NgaySua"].ToString() != "")
        //            {
        //                khoaCodeDaiLy.NgaySua = DateTime.Parse(tb.Rows[i]["NgaySua"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
        //            }
        //            else
        //            {
        //                khoaCodeDaiLy.NgaySua = "";
        //            }
        //            listDSKhoaCodeDaiLy.Add(khoaCodeDaiLy);
        //        }
        //        khoaCodeDaiLy.DSTinhTrangKhoa = DSTinhTrangKhoa();
        //        khoaCodeDaiLy.DSKhoaCodeDaiLy = listDSKhoaCodeDaiLy;
        //    }
        //    return khoaCodeDaiLy;
        //}

        public bool SaveTBDL(string MaKH, string tenDL, string noiDungKhoa, string maNVLap, string nguoiLap, string IDNoiDungKhoa, string TinhTrangKhoa, string MailCC, string Email, string SoDT, string MaPB)
        {
            int i = 0;
            string NoiDungTimKiem = "";
            string sqlTieuDe = @"select NoiDungTimKiem from QLTHONGBAO where ROWID = '" + IDNoiDungKhoa + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN))
            {
                NoiDungTimKiem = conn.QueryFirst<string>(sqlTieuDe, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            try
            {
                if (noiDungKhoa == null)
                {
                    noiDungKhoa = "";
                }
                if (MailCC == null)
                {
                    MailCC = "";
                }
                string sql = @"INSERT INTO [KhoaCodeDaiLy] ([MaKH] ,[TenDaiLy],[NoiDungKhoa],[NoiDung],[MaNVLap] ,[NguoiLap] ,[NgayLap] ,[Status] ,[TinhTrangKhoa],[MailCC],[Email],[SoDT],[IDNoiDungKhoa],[MaPhongBan]) 
                                                    VALUES ( '" + MaKH + "',N'" + tenDL + "',N'" + NoiDungTimKiem + "',N'" + noiDungKhoa + "','" + maNVLap + "',N'" + nguoiLap + "',GetDATE(),1,'" + TinhTrangKhoa + "', '" + MailCC + "', '" + Email + "', '" + SoDT + "', '" + IDNoiDungKhoa + "','" + MaPB + "')";
                using (var conn = new SqlConnection(SQL_EV_MAIN))
                {
                    i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                    conn.Dispose();
                }
                if (i > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool SendMailKC(string MaKH, string TenDaiLy, string NoiDungKhoa, string ccReceiver, string Email, string IDNoiDungKhoa)
        {
            string TieuDeKhoa = "";
            string sqlTieuDe = @"select TIEUDE from QLTHONGBAO where ROWID = '" + IDNoiDungKhoa + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN))
            {
                TieuDeKhoa = conn.QueryFirst<string>(sqlTieuDe, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            //NoiDungKhoa
            string sql_noidung = @"select top 1 * from QLTHONGBAO 
                            where RowID = " + IDNoiDungKhoa;
            DataTable tb_noidung = db.ExecuteDataSet(sql_noidung, CommandType.Text, "server37", null).Tables[0];
            //Mail Kinh Doanh
            string sql = @"select * from DM_NV DM
                            left join KHACHHANG_HOPDONG KH on DM.MaNV = KH.MAKINHDOANH
                            where MAKETOAN = '" + MaKH + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];

            //Mail Đại Lý
            string sql_mail_agent = "select * from member where member_kh = '" + MaKH + "'";
            DataTable tb_mail_agent = db.ExecuteDataSet(sql_mail_agent, CommandType.Text, "server18", null).Tables[0];
            if (Email != "")
            {
                MailMessage mail = new MailMessage(mailDb.username, Email);
                mail.From = new MailAddress(mailDb.username, "");
                mail.Subject = "Thông Báo Đại Lý";
                SmtpClient client = new SmtpClient();
                client.EnableSsl = Convert.ToBoolean(mailDb.useSSL);
                client.Port = mailDb.port;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(mailDb.username, new DBase().Decrypt(mailDb.password, "vodacthe", true));
                client.Host = mailDb.host;
                string subject = $"[Thông Báo] {MaKH}";
                try
                {
                    string mailCC = "";
                    if (tb != null)
                    {
                        if (tb.Rows.Count > 0)
                        {
                            mailCC = tb.Rows[0]["Email"].ToString();
                        }
                    }
                    if (ccReceiver != "")
                    {
                        if (mailCC != "")
                        {
                            mailCC = mailCC + "," + ccReceiver;
                        }
                        else
                        {
                            mailCC = ccReceiver;
                        }
                    }
                    mailCC = mailCC + ",notification@enviet-group.com";
                    if (mailCC != "")
                    {
                        mail.CC.Add(mailCC);
                    }
                }
                catch { }
                try { mail.Bcc.Add(mailDb.BCC); }
                catch { }

                ///-------- Start of mail body ------------
                string mailBody;
                var webRequest = System.Net.WebRequest.Create(mailDb.templateUrl);
                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new System.IO.StreamReader(content))
                { mailBody = reader.ReadToEnd(); }
                mailBody = mailBody.Replace("$_MaKH", MaKH);
                mailBody = mailBody.Replace("$_DaiLy", TenDaiLy);
                mailBody = mailBody.Replace("$_TinhTrangKhoa", TieuDeKhoa);
                mailBody = mailBody.Replace("$_NoiDung", NoiDungKhoa);
                mailBody = mailBody.Replace("$_Ngaygui", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                mail.Body = mailBody;
                mail.IsBodyHtml = true; // Format mail dạng HTML
                ///-------- End of mail body --------------
                client.Send(mail);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditKCDL(string MaKH, string tenDL, string noiDungKhoa, string tenNV, string ID, string IDNoiDungKhoa, string TinhTrangKhoa, string MailCC)
        {


            if (ID == null)
            {
                try
                {

                    if (noiDungKhoa == null)
                    {
                        noiDungKhoa = "";
                    }
                    string sql = "INSERT INTO [KhoaCodeDaiLy] ([MaKH] ,[TenDaiLy],[NoiDungKhoa] ,[NguoiLap] ,[NgayLap] ,[Status] ,[TinhTrangKhoa],[MailCC]) VALUES ( @MaKH,@TenDaiLy,@NoiDungKhoa,@NguoiLap,GetDATE(),1,@TinhTrangKhoa,@MailCC)";
                    List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                    Param.Add(new DBase.AddParameters("@MaKH", MaKH));
                    Param.Add(new DBase.AddParameters("@TenDaiLy", tenDL));
                    Param.Add(new DBase.AddParameters("@NoiDungKhoa", noiDungKhoa));
                    Param.Add(new DBase.AddParameters("@NguoiLap", tenNV));

                    Param.Add(new DBase.AddParameters("@TinhTrangKhoa", TinhTrangKhoa));
                    Param.Add(new DBase.AddParameters("@MailCC", MailCC));

                    int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);

                    if (i > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                try
                {

                    string sql = @"UPDATE KhoaCodeDaiLy SET MaKH = '" + MaKH + "', TinhTrangKhoa = " + TinhTrangKhoa + ", NoiDungKhoa = N'" + noiDungKhoa + "',NgaySua = GetDate(), NguoiSua = N'" + tenNV + "',TenDaiLy = N'" + tenDL + "' where ID = " + ID;
                    int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", null);
                    if (i > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception)
                {

                    throw;
                }
            }


        }

        public bool DelKCDL(string ID, string tenNV)
        {
            try
            {
                string sql = @"UPDATE KhoaCodeDaiLy SET NgayXoa = GETDATE(),NguoiXoa = N'" + tenNV + "', Status = 0 where ID = " + ID;
                int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", null);
                if (i > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
        }




    }
}
