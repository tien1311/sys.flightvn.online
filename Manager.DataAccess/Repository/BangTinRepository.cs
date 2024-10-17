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
using Microsoft.Extensions.Configuration;
using System.Net;
using Dapper;
using Manager.Model.Models;

namespace Manager.DataAccess.Repository
{
    public class BangTinRepository
    {
        DBase db = new DBase();
        private  string server_Airline24h;
        public BangTinRepository(IConfiguration configuation)
        {
            server_Airline24h = configuation.GetConnectionString("SQL_AIRLINE24h_MAIN");
        }
        public List<SubjectModel> BangTin()
        {
            List<SubjectModel> list = new List<SubjectModel>();
            try
            {
                string SqlView = "SELECT top 1000 TIEUDE,NGAYDANG,ROWID FROM DANGTIN WHERE BANTIN = 1 and BANTINCHAMCONG = 0 and HIENTHI = 1 ORDER BY ROWID DESC  ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server37", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel home = new SubjectModel();
                    home.Image = "/images/logoev12.png";
                    home.Title = dt.Rows[i]["TIEUDE"].ToString();
                    home.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["NGAYDANG"].ToString();
                    home.subject_id = int.Parse(dt.Rows[i]["ROWID"].ToString());
                    list.Add(home);
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<SubjectModel> KhenThuongKyLuatNoiBo()
        {
            List<SubjectModel> list = new List<SubjectModel>();
            try
            {
                string SqlView = "SELECT top 1000 TIEUDE,NGAYDANG,ROWID FROM DANGTIN WHERE KHENTHUONGKYLUAT = 1 and HIENTHI = 1 ORDER BY ROWID DESC  ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server37", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel home = new SubjectModel();
                    home.Image = "/images/logoev12.png";
                    home.Title = dt.Rows[i]["TIEUDE"].ToString();
                    home.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["NGAYDANG"].ToString();
                    home.subject_id = int.Parse(dt.Rows[i]["ROWID"].ToString());
                    list.Add(home);
                }
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<SubjectModel> QuyDinh()
        {
            List<SubjectModel> list = new List<SubjectModel>();
            try
            {
                string SqlView = "SELECT top 1000 TIEUDE,NGAYDANG=Convert(nvarchar(10),NGAYLAP,103),ROWID FROM BAIVIET WHERE ID = 51  ORDER BY ROWID DESC ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "serverAirline24h", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel home = new SubjectModel();
                    home.Image = "/images/logoev12.png";
                    home.Title = dt.Rows[i]["TIEUDE"].ToString();
                    home.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["NGAYDANG"].ToString();
                    home.subject_id = int.Parse(dt.Rows[i]["ROWID"].ToString());
                    list.Add(home);
                }
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<SubjectModel> QuyDinhOld()
        {
            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = "SELECT top 1000 TIEUDE,NGAYDANG,ROWID FROM DANGTIN WHERE TINTRONGNGAY = 1 and BANTINCHAMCONG = 0 and HIENTHI = 1 ORDER BY ROWID DESC  ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server37", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel home = new SubjectModel();
                    home.Image = "/images/logoev12.png";
                    home.Title = dt.Rows[i]["TIEUDE"].ToString();
                    home.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["NGAYDANG"].ToString();
                    home.subject_id = int.Parse(dt.Rows[i]["ROWID"].ToString());
                    list.Add(home);
                }


                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<SubjectModel> QuyDinhDL()
        {
            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = "SELECT top 1000 TIEUDE,NGAYDANG=Convert(nvarchar(10),NGAYLAP,103),ROWID FROM BAIVIET WHERE ID = 54  ORDER BY ROWID DESC ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "serverAirline24h", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel home = new SubjectModel();
                    home.Image = "/images/logoev12.png";
                    home.Title = dt.Rows[i]["TIEUDE"].ToString();
                    home.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["NGAYDANG"].ToString();
                    home.subject_id = int.Parse(dt.Rows[i]["ROWID"].ToString());
                    list.Add(home);
                }
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<SubjectModel> QuyDinhDoan()
        {
            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = "SELECT top 1000 TIEUDE,NGAYDANG=Convert(nvarchar(10),NGAYLAP,103),ROWID FROM BAIVIET WHERE ID = 53  ORDER BY ROWID DESC ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "serverAirline24h", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel home = new SubjectModel();
                    home.Image = "/images/logoev12.png";
                    home.Title = dt.Rows[i]["TIEUDE"].ToString();
                    home.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["NGAYDANG"].ToString();
                    home.subject_id = int.Parse(dt.Rows[i]["ROWID"].ToString());
                    list.Add(home);
                }
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<SubjectModel> QuyDinhKT()
        {
            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = "SELECT top 1000 TIEUDE,NGAYDANG=Convert(nvarchar(10),NGAYLAP,103),ROWID FROM BAIVIET WHERE ID = 50  ORDER BY ROWID DESC ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "serverAirline24h", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel home = new SubjectModel();
                    home.Image = "/images/logoev12.png";
                    home.Title = dt.Rows[i]["TIEUDE"].ToString();
                    home.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["NGAYDANG"].ToString();
                    home.subject_id = int.Parse(dt.Rows[i]["ROWID"].ToString());
                    list.Add(home);
                }
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<SubjectModel> BangTinChamCong()
        {
            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = "SELECT top 1000 TIEUDE,NGAYDANG,ROWID FROM DANGTIN WHERE BANTINCHAMCONG = 1 and HIENTHI = 1 ORDER BY ROWID DESC  ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server37", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel home = new SubjectModel();
                    home.Image = "/images/logoev12.png";
                    home.Title = dt.Rows[i]["TIEUDE"].ToString();
                    home.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["NGAYDANG"].ToString();
                    home.subject_id = int.Parse(dt.Rows[i]["ROWID"].ToString());
                    list.Add(home);
                }


                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<SubjectModel> SearchListAll(int? section_id, string tieude)
        {

            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = "SELECT top 1000 ngay=Convert(nvarchar(10),subject_date,103),subject_id,subject_name,subject_header,section_id,subject_picture FROM subject WHERE section_id in ( SELECT section_id FROM subject_section WHERE parent_id='" + section_id + "') OR section_id ='" + section_id + "' AND subject_isshow='1' and subject_name like N'%" + tieude + "%' ORDER BY subject_id DESC  ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server18", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel QuyDinhNghiepVu = new SubjectModel();
                    QuyDinhNghiepVu.Image = "https://Manager.airline24h.com/upload/subject/" + dt.Rows[i]["subject_picture"].ToString();
                    QuyDinhNghiepVu.Title = dt.Rows[i]["subject_header"].ToString();
                    QuyDinhNghiepVu.Description = dt.Rows[i]["subject_name"].ToString();
                    QuyDinhNghiepVu.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["ngay"].ToString();
                    QuyDinhNghiepVu.subject_id = int.Parse(dt.Rows[i]["subject_id"].ToString());
                    list.Add(QuyDinhNghiepVu);
                }


                return list;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<SubjectModel> SearchAllCongVan(string tieude)
        {

            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = @"SELECT top 1000 ngay = Convert(nvarchar(10), subject_date, 103),subject_id,subject_name,subject_header,section_id,subject_picture 
                                    FROM subject
                                    WHERE(section_id in (SELECT section_id FROM subject_section WHERE parent_id in (75, 22, 124, 125, 126, 127))
                                            OR section_id in (75, 22, 124, 125, 126, 127, 132)
                                            AND subject_isshow = '1' ) 
		                                    and subject_name like N'%" + tieude + "%' ORDER BY subject_id DESC";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server18", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel QuyDinhNghiepVu = new SubjectModel();
                    QuyDinhNghiepVu.Image = "https://Manager.airline24h.com/upload/subject/" + dt.Rows[i]["subject_picture"].ToString();
                    QuyDinhNghiepVu.Title = dt.Rows[i]["subject_header"].ToString();
                    QuyDinhNghiepVu.Description = dt.Rows[i]["subject_name"].ToString();
                    QuyDinhNghiepVu.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["ngay"].ToString();
                    QuyDinhNghiepVu.subject_id = int.Parse(dt.Rows[i]["subject_id"].ToString());
                    list.Add(QuyDinhNghiepVu);
                }


                return list;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<SubjectModel> AllCongVan()
        {

            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = "SELECT top 1000 ngay = Convert(nvarchar(10), subject_date, 103),subject_id,subject_name,subject_header,section_id,subject_picture FROM subject WHERE section_id in (SELECT section_id FROM subject_section WHERE parent_id in (75, 22, 124, 125, 126, 127)) OR section_id in (75, 22, 124, 125, 126, 127, 132) AND subject_isshow = '1'  ORDER BY subject_id DESC  ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server18", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel QuyDinhNghiepVu = new SubjectModel();
                    QuyDinhNghiepVu.Image = "https://Manager.airline24h.com/upload/subject/" + dt.Rows[i]["subject_picture"].ToString();
                    QuyDinhNghiepVu.Title = dt.Rows[i]["subject_header"].ToString();
                    QuyDinhNghiepVu.Description = dt.Rows[i]["subject_name"].ToString();
                    QuyDinhNghiepVu.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["ngay"].ToString();
                    QuyDinhNghiepVu.subject_id = int.Parse(dt.Rows[i]["subject_id"].ToString());
                    list.Add(QuyDinhNghiepVu);
                }


                return list;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<SubjectModel> ListAll(int section_id)
        {

            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = "SELECT top 1000 ngay=Convert(nvarchar(10),subject_date,103),subject_id,subject_name,subject_header,section_id,subject_picture FROM subject WHERE section_id in ( SELECT section_id FROM subject_section WHERE parent_id='" + section_id + "') OR section_id ='" + section_id + "' AND subject_isshow='1'  ORDER BY subject_id DESC  ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server18", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel QuyDinhNghiepVu = new SubjectModel();
                    QuyDinhNghiepVu.Image = "https://Manager.airline24h.com/upload/subject/" + dt.Rows[i]["subject_picture"].ToString();
                    QuyDinhNghiepVu.Title = dt.Rows[i]["subject_header"].ToString();
                    QuyDinhNghiepVu.Description = dt.Rows[i]["subject_name"].ToString();
                    QuyDinhNghiepVu.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["ngay"].ToString();
                    QuyDinhNghiepVu.subject_id = int.Parse(dt.Rows[i]["subject_id"].ToString());
                    list.Add(QuyDinhNghiepVu);
                }


                return list;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<SubjectModel> KhuyenMai()
        {

            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = "select HINHANH,TIEUDE,MOTA,NGAYLAP,ROWID,NOIDUNG from BAIVIET where ID=12";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "serverAirline24h", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel home = new SubjectModel();
                    home.Image = "https://Manager.airline24h.com/upload/images/" + dt.Rows[i]["HINHANH"].ToString();
                    home.Title = dt.Rows[i]["TIEUDE"].ToString();
                    home.Description = dt.Rows[i]["MOTA"].ToString();
                    home.Date = dt.Rows[i]["NGAYLAP"].ToString();
                    home.subject_id = int.Parse(dt.Rows[i]["ROWID"].ToString());
                    home.subject_content = dt.Rows[i]["NOIDUNG"].ToString();
                    list.Add(home);
                }
                return list;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<SubjectModel> BangTinPV()
        {
            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = "SELECT top 1000 TIEUDE,NGAYDANG,ROWID FROM BANTINPHONGVE WHERE BANTINPHONGVE = 1 and HIENTHI = 1 ORDER BY ROWID DESC  ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server37", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel home = new SubjectModel();
                    home.Image = "/images/logoev12.png";
                    home.Title = dt.Rows[i]["TIEUDE"].ToString();
                    home.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["NGAYDANG"].ToString();
                    home.subject_id = int.Parse(dt.Rows[i]["ROWID"].ToString());
                    list.Add(home);
                }


                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<SubjectModel> QuyDinhPV()
        {
            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = "SELECT top 1000 TIEUDE,NGAYDANG=Convert(nvarchar(10),NGAYLAP,103),ROWID FROM BAIVIET WHERE ID = 49  ORDER BY ROWID DESC  ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "serverAirline24h", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel home = new SubjectModel();
                    home.Image = "/images/logoev12.png";
                    home.Title = dt.Rows[i]["TIEUDE"].ToString();
                    home.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["NGAYDANG"].ToString();
                    home.subject_id = int.Parse(dt.Rows[i]["ROWID"].ToString());
                    list.Add(home);
                }


                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<SubjectModel> QuyDinhHang()
        {
            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = "SELECT top 1000 TIEUDE,NGAYDANG,ROWID FROM BANTINPHONGVE WHERE THONGBAOHANG = 1 and HIENTHI = 1 ORDER BY ROWID DESC  ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server37", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel home = new SubjectModel();
                    home.Image = "/images/logoev12.png";
                    home.Title = dt.Rows[i]["TIEUDE"].ToString();
                    home.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["NGAYDANG"].ToString();
                    home.subject_id = int.Parse(dt.Rows[i]["ROWID"].ToString());
                    list.Add(home);
                }


                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<SubjectModel> BanTinKinhDoanh()
        {
            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = "SELECT top 1000 TIEUDE,NGAYDANG,ROWID FROM DANGTINKINHDOANH WHERE BANTIN = 1 and HIENTHI = 1 ORDER BY TINQUANTRONG DESC,ROWID DESC  ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server37", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel home = new SubjectModel();
                    home.Image = "/images/logoev12.png";
                    home.Title = dt.Rows[i]["TIEUDE"].ToString();
                    home.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["NGAYDANG"].ToString();
                    home.subject_id = int.Parse(dt.Rows[i]["ROWID"].ToString());
                    list.Add(home);
                }


                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<SubjectModel> QuyDinhKinhDoanh()
        {
            List<SubjectModel> list = new List<SubjectModel>();
            try
            {

                string SqlView = "SELECT top 1000 TIEUDE,NGAYDANG=Convert(nvarchar(10),NGAYLAP,103),ROWID FROM BAIVIET WHERE ID = 52  ORDER BY ROWID DESC  ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "serverAirline24h", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SubjectModel home = new SubjectModel();
                    home.Image = "/images/logoev12.png";
                    home.Title = dt.Rows[i]["TIEUDE"].ToString();
                    home.Date = " Ngày cập nhật cuối: " + dt.Rows[i]["NGAYDANG"].ToString();
                    home.subject_id = int.Parse(dt.Rows[i]["ROWID"].ToString());
                    list.Add(home);
                }


                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //chi tiết tin yến sào, khen thưởng , kỷ luật
        public SubjectModel ContentYS(int subject_id)
        {
            try
            {
                string SqlView = "SELECT NOIDUNG,TIEUDE  FROM BAIVIET WHERE ROWID = " + subject_id + " ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "serverAirline24h", null).Tables[0];

                SubjectModel home = new SubjectModel();
                home.Title = dt.Rows[0]["TIEUDE"].ToString();
                home.subject_content = dt.Rows[0]["NOIDUNG"].ToString();
                return home;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //chi tiết tin nội bộ

        public MemoryStream GetStream(string url)
        {
            WebClient wc = new WebClient();
            byte[] data = wc.DownloadData(new Uri(url));

            return new MemoryStream(data);
        }

        public SubjectModel Content(int subject_id)
        {
            try
            {
                int result_update = 0;
                SubjectModel home = new SubjectModel();
              
               
                string SqlView = "SELECT TIEUDE,NOIDUNG FROM DANGTIN WHERE ROWID = " + subject_id + " ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server37", null).Tables[0];

                home.Title = dt.Rows[0]["TIEUDE"].ToString();
                home.subject_content = dt.Rows[0]["NOIDUNG"].ToString();
                return home;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public SubjectModel ContentOld(int subject_id)
        {
            try
            {
                string SqlView = "SELECT TIEUDE,TENFILE  FROM DANGTIN WHERE ROWID = " + subject_id + " ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server37", null).Tables[0];

                SubjectModel home = new SubjectModel();
                home.Title = dt.Rows[0]["TIEUDE"].ToString();
                home.subject_name = dt.Rows[0]["TENFILE"].ToString();

                SqlDataReader reader = null;
                string sql = "select NOIDUNGRTF  from DANGTIN where ROWID = " + subject_id + "";
                reader = db.ExecuteReader(sql, CommandType.Text, "server37", null);
                reader.Read();
                if (reader.HasRows)
                {
                    if (!reader.IsDBNull(0))
                    {
                        //Convert RTF to HTML
                        byte[] rtf = new byte[Convert.ToInt32(reader.GetBytes(0, 0,
                                                               null, 0, int.MaxValue))];
                        long bytesReceived = reader.GetBytes(0, 0, rtf, 0, rtf.Length);
                        Encoding end = Encoding.ASCII;
                        ASCIIEncoding encoding = new ASCIIEncoding();
                        string content = encoding.GetString(rtf, 0, Convert.ToInt32(bytesReceived));
                        string html = RtfPipe.Rtf.ToHtml(content);
                        home.subject_content = html;
                    }
                }
                return home;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //quy định các hãng
        public SubjectModel QuyDinhHang(int subject_id)
        {

            try
            {


                string SqlView = "SELECT subject_content FROM subject WHERE subject_id = " + subject_id + " ORDER BY subject_id DESC ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server18", null).Tables[0];
                SubjectModel home = new SubjectModel();

                home.subject_content = dt.Rows[0][0].ToString();

                return home;
            }
            catch (Exception)
            {

                throw;
            }


        }
        //chi tiết tin phòng vé
        public SubjectModel ContentPV(int subject_id)
        {
            try
            {
                int result_update = 0;
                SubjectModel home = new SubjectModel();
                string sql_update_tien = "SP_REPLACE_BAIVIET_CONTENT";

                using (var conn = new SqlConnection(server_Airline24h))
                {
                    var param = new
                    {
                        ID = subject_id
                    };
                    result_update = conn.Execute(sql_update_tien, param, null, commandType: CommandType.StoredProcedure, commandTimeout: 60);
                    conn.Dispose();
                }
                if (result_update > 0)
                {
                    string SqlView = "SELECT TIEUDE,NOIDUNG FROM BAIVIET WHERE ROWID = " + subject_id + " ";
                    DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "serverAirline24h", null).Tables[0];

                    home.Title = dt.Rows[0]["TIEUDE"].ToString();
                    home.subject_content = dt.Rows[0]["NOIDUNG"].ToString();

                }
                return home;
            }
            catch (Exception)
            {
                throw;
            }

        }
        //chi tiết tin phòng vé mới nhất
        public SubjectModel ContentPVNew()
        {
            try
            {
                string SqlView = "SELECT top 1 TIEUDE,TENFILE  FROM BANTINPHONGVE WHERE HIENTHI = 1 ORDER BY NGAYDANG DESC ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server37", null).Tables[0];
                SubjectModel home = new SubjectModel();
                home.Title = dt.Rows[0]["TIEUDE"].ToString();
                home.subject_name = dt.Rows[0]["TENFILE"].ToString();

                SqlDataReader reader = null;
                string sql = "select NOIDUNGRTF  from BANTINPHONGVE where HIENTHI = 1 ORDER BY NGAYDANG DESC";
                reader = db.ExecuteReader(sql, CommandType.Text, "server37", null);
                reader.Read();
                if (reader.HasRows)
                {
                    if (!reader.IsDBNull(0))
                    {
                        //Convert RTF to HTML
                        byte[] rtf = new byte[Convert.ToInt32(reader.GetBytes(0, 0,
                                                               null, 0, int.MaxValue))];
                        long bytesReceived = reader.GetBytes(0, 0, rtf, 0, rtf.Length);
                        Encoding end = Encoding.ASCII;
                        ASCIIEncoding encoding = new ASCIIEncoding();
                        string content = encoding.GetString(rtf, 0, Convert.ToInt32(bytesReceived));
                        string html = RtfPipe.Rtf.ToHtml(content);
                        home.subject_content = html;
                    }
                }
                return home;
            }
            catch (Exception)
            {
                throw;
            }

        }
        //chi tiết tin kinh doanh mới nhất
        public SubjectModel ContentKDNew()
        {
            try
            {
                string SqlView = "SELECT TIEUDE,TENFILE  FROM DANGTINKINHDOANH WHERE HIENTHI = 1 ORDER BY NGAYDANG DESC ";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server37", null).Tables[0];

                SubjectModel home = new SubjectModel();
                home.Title = dt.Rows[0]["TIEUDE"].ToString();
                home.subject_name = dt.Rows[0]["TENFILE"].ToString();

                SqlDataReader reader = null;
                string sql = "select NOIDUNGRTF  from DANGTINKINHDOANH where HIENTHI = 1 ORDER BY NGAYDANG DESC";
                reader = db.ExecuteReader(sql, CommandType.Text, "server37", null);
                reader.Read();
                if (reader.HasRows)
                {
                    if (!reader.IsDBNull(0))
                    {
                        //Convert RTF to HTML
                        byte[] rtf = new byte[Convert.ToInt32(reader.GetBytes(0, 0,
                                                               null, 0, int.MaxValue))];
                        long bytesReceived = reader.GetBytes(0, 0, rtf, 0, rtf.Length);
                        Encoding end = Encoding.ASCII;
                        ASCIIEncoding encoding = new ASCIIEncoding();
                        string content = encoding.GetString(rtf, 0, Convert.ToInt32(bytesReceived));
                        string html = RtfPipe.Rtf.ToHtml(content);
                        home.subject_content = html;
                    }
                }
                return home;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //chi tiết tin kinh doanh
        public SubjectModel ContentKD(int subject_id)
        {
            try
            {
                int result_update = 0;
                SubjectModel home = new SubjectModel();
                string sql_update_tien = "SP_REPLACE_BAIVIET_CONTENT";

                using (var conn = new SqlConnection(server_Airline24h))
                {
                    var param = new
                    {
                        ID = subject_id
                    };
                    result_update = conn.Execute(sql_update_tien, param, null, commandType: CommandType.StoredProcedure, commandTimeout: 60);
                    conn.Dispose();
                }
                if (result_update > 0)
                {
                    string SqlView = "SELECT TIEUDE,NOIDUNG FROM BAIVIET WHERE ROWID = " + subject_id + " ";
                    DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "serverAirline24h", null).Tables[0];

                    home.Title = dt.Rows[0]["TIEUDE"].ToString();
                    home.subject_content = dt.Rows[0]["NOIDUNG"].ToString();

                }
                return home;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //chi tiết tin noi bo
        public SubjectModel ContentNoiBo()
        {
            try
            {
                string SqlView = "SELECT top 1 TIEUDE,TENFILE  FROM DANGTIN where HIENTHI = '1' order by NGAYDANG desc";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server37", null).Tables[0];

                SubjectModel home = new SubjectModel();
                home.Title = dt.Rows[0]["TIEUDE"].ToString();
                home.subject_name = dt.Rows[0]["TENFILE"].ToString();

                SqlDataReader reader = null;
                string sql = "select top 1 NOIDUNGRTF from DANGTIN order by NGAYDANG desc";
                reader = db.ExecuteReader(sql, CommandType.Text, "server37", null);
                reader.Read();
                if (reader.HasRows)
                {
                    if (!reader.IsDBNull(0))
                    {
                        //Convert RTF to HTML
                        byte[] rtf = new byte[Convert.ToInt32(reader.GetBytes(0, 0,
                                                               null, 0, int.MaxValue))];
                        long bytesReceived = reader.GetBytes(0, 0, rtf, 0, rtf.Length);
                        Encoding end = Encoding.ASCII;
                        ASCIIEncoding encoding = new ASCIIEncoding();
                        string content = encoding.GetString(rtf, 0, Convert.ToInt32(bytesReceived));
                        string html = RtfPipe.Rtf.ToHtml(content);
                        home.subject_content = html;
                    }
                }
                return home;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
