using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Dapper;
using System.Data.SqlClient;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class ProfileRepository
    {
        DBase db = new DBase();
        string server_EV_MAIN; /* = "Data Source=.;Initial Catalog=Manager;User ID=sa;Password=EnViet@123;";*/
        string SQL_EV_MAIN_V2; /* = "Data Source=.;Initial Catalog=Manager_V2;User ID=sa;Password=EnViet@123;"*/
        public ProfileRepository(IConfiguration configuration)
        {
            server_EV_MAIN = configuration.GetConnectionString("SQL_EV_MAIN");
            SQL_EV_MAIN_V2 = configuration.GetConnectionString("SQL_EV_MAIN_V2");

        }
        AccountModel profile = new AccountModel();
        public AccountModel DSprofile(string MaNV)
        {
            AccountModel result = new AccountModel();
            List<FileDinhKem> ListFile = new List<FileDinhKem>();
            string sql = "select * from DM_NV where Yahoo ='" + MaNV + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb.Rows.Count > 0)
            {
                if (bool.Parse(tb.Rows[0]["TINHTRANG"].ToString()) == true)
                {
                    result.RowID = Convert.ToInt32(tb.Rows[0]["RowID"].ToString());
                    result.MaNV = tb.Rows[0]["Yahoo"].ToString();
                    result.NgaySinh = DateTime.Parse(tb.Rows[0]["SinhNhat"].ToString());
                    result.HoTen = tb.Rows[0]["Ten"].ToString();
                    result.ChucVu = tb.Rows[0]["ChucVu"].ToString();
                    result.ChiNhanh = tb.Rows[0]["ChiNhanh"].ToString();
                    result.DiaChiThuongTru = tb.Rows[0]["DiaChiThuongTru"].ToString();
                    result.DienThoai = tb.Rows[0]["DienThoai"].ToString();
                    result.Email = tb.Rows[0]["Email"].ToString();
                    result.PhongBan = tb.Rows[0]["PhongBan"].ToString();
                    if (tb.Rows[0]["TenHinh"].ToString() != "")
                    {
                        result.TenHinh = "http://daily.airline24h.com/upload/hinhnv/" + tb.Rows[0]["TenHinh"].ToString();
                    }
                    else
                    {
                        result.TenHinh = "/images/img.jpg";
                    }
                    result.DienThoaiCN = tb.Rows[0]["DienThoaiCN"].ToString();
                    result.DienThoaiSMS = tb.Rows[0]["DienThoaiSMS"].ToString();
                    result.Line = tb.Rows[0]["Line"].ToString();
                    result.DiaChiTamTru = tb.Rows[0]["DiaChiTamTru"].ToString();
                    result.CMND = tb.Rows[0]["CMND"].ToString();
                    result.NoiCap = tb.Rows[0]["NoiCap"].ToString();
                    result.NgayCap = DateTime.Parse(tb.Rows[0]["NgayCap"].ToString());
                    result.NgayLamViec = DateTime.Parse(tb.Rows[0]["NgayLamViec"].ToString());
                    result.SoTK = tb.Rows[0]["SoTK1"].ToString();
                    result.Skyper = tb.Rows[0]["Skyper"].ToString();
                    result.TenDangNhap = tb.Rows[0]["TenDangNhap"].ToString();
                    result.MaSoThue = tb.Rows[0]["MASOTHUE"].ToString();
                    result.NgayCapMST = tb.Rows[0]["NGAYCAPMST"].ToString();
                }
            }
            string sqlFile = @"select File_NV.Title,TenFile = 'https://Manager.airline24h.com/upload/fileNhanVien/'+ File_NV.TenFile,File_NV.NgayUp 
                                from FILE_NHANVIEN File_NV left join [SERVER37].[Manager].[dbo].[DM_NV] on DM_NV.MaNV = File_NV.MaNV
                                where DM_NV.Yahoo = '" + MaNV + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                ListFile = (List<FileDinhKem>)conn.Query<FileDinhKem>(sqlFile, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            result.ListFile = ListFile;
            return result;
        }
        public NhanVienModel QuyDinh(string MaNV)
        {
            NhanVienModel CV = new NhanVienModel();
            string sql = @"select * from PHANVIEC_QUYDINH_NV where MaNV = '" + MaNV + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb.Rows.Count > 0)
            {
                CV.Noidung = tb.Rows[0]["QuyDinh"].ToString();
                CV.Noidungchung = tb.Rows[0]["QuyDinhChung"].ToString();
                CV.CongViec = "Quy định";
            }
            else
            {
                CV.Noidung = "Chưa có dữ liệu";
                CV.Noidungchung = "Chưa có dữ liệu";
                CV.CongViec = "Quy định";
            }
            return CV;
        }
        public NhanVienModel PhanViec(string MaNV)
        {
            NhanVienModel CV = new NhanVienModel();
            string sql = @"select * from PHANVIEC_QUYDINH_NV where MaNV = '" + MaNV + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb.Rows.Count > 0)
            {
                CV.Noidung = tb.Rows[0]["PhanViec"].ToString();
                CV.Noidungchung = tb.Rows[0]["PhanViecChung"].ToString();
                CV.CongViec = "Phân việc";
            }
            else
            {
                CV.Noidung = "Chưa có dữ liệu";
                CV.Noidungchung = "Chưa có dữ liệu";
                CV.CongViec = "Phân việc";
            }
            return CV;
        }
        public CapNhatCauHinh CapNhat(string Server)
        {

            CapNhatCauHinh CV = new CapNhatCauHinh();
            string sql = @"select * from CAUHINH_HDDL_CU where RowID = '1'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            CV.HDMoi = DateTime.Parse(tb.Rows[0]["NgayHDMoi"].ToString()).ToString("dd/MM/yyyy");
            CV.HDKT = DateTime.Parse(tb.Rows[0]["NgayHD_KT"].ToString()).ToString("dd/MM/yyyy");
            CV.FristCheckBoxHDDT = tb.Rows[0]["FristCheckBoxHDDT"].ToString();
            CV.SecondCheckBoxHDDT = tb.Rows[0]["SecondCheckBoxHDDT"].ToString();
            string sql1 = "select Status from BankStatement_Status";
            using (var conn = new SqlConnection(Server))
            {
                string response = conn.QueryFirst<string>(sql1, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
                conn.Close();
                CV.Status = response;
            }
            return CV;
        }
        public bool UpdateStatusSMS(string Status, string Server)
        {
            bool result = false;
            int result_sql = 0;
            if (Status == "true")
            {
                Status = "1";
            }
            else
            {
                Status = "0";
            }
            string sql = @"update BankStatement_Status set Status = " + Status;
            using (var conn = new SqlConnection(Server))
            {
                result_sql = conn.Execute(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
                conn.Close();
            }
            if (result_sql > 0)
            {
                result = true;
            }
            return result;
        }
        public bool Update(string HDMoi, string HDKT, string FirstCheckBox, string SecondCheckBox)
        {
            DateTime ngayBD = DateTime.ParseExact(HDMoi, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime ngayKT = DateTime.ParseExact(HDKT, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            CapNhatCauHinh KH = new CapNhatCauHinh();
            string sql = @"update CAUHINH_HDDL_CU set NgayHDMoi='" + ngayBD + "', NgayHD_KT='" + ngayKT + "', FristCheckBoxHDDT = N'" + FirstCheckBox + "', SecondCheckBoxHDDT = N'" + SecondCheckBox + "' where RowID ='1'";
            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", null);
            if (i > 0)
            {
                return true;
            }
            else
                return false;

            //KH.HDMoi = DateTime.Parse(dt.Rows[0]["NgayHDMoi"].ToString()).ToString("MM/dd/yyyy");
            //KH.HDKT = DateTime.Parse(dt.Rows[0]["NgayHD_KT"].ToString()).ToString("MM/dd/yyyy");

        }
        public AccountModel StickNote(string MaNV)
        {
            AccountModel result = new AccountModel();
            string sql = @"select * from StickNote where MaNV = '" + MaNV + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    result.MaNV = tb.Rows[0]["MaNV"].ToString();
                    result.StickNote = tb.Rows[0]["StickNote"].ToString();
                }
            }
            return result;
        }
        public bool SaveStickNote(string MaNV, string createContent)
        {
            string sql = "";
            string sqlCheck = @"select * from StickNote where MaNV = '" + MaNV + "'";
            DataTable tb = db.ExecuteDataSet(sqlCheck, CommandType.Text, "server37", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    sql = @"UPDATE StickNote SET StickNote = @StickNote WHERE MaNV = @MaNV ";
                }
                else
                {
                    sql = "INSERT INTO [StickNote] ([MaNV] ,[StickNote]) VALUES ( @MaNV,@StickNote)";
                }
            }

            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@MaNV", MaNV));
            Param.Add(new DBase.AddParameters("@StickNote", createContent));

            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public DanhSachLienHeModel DSLienHe(string MaNV)
        {
            DanhSachLienHeModel result = new DanhSachLienHeModel();
            try
            {
                string sql = @"Select * from DanhSachLienHe where MaNV = '" + MaNV + "'";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    result = conn.QueryFirst<DanhSachLienHeModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }

        }
        public bool SaveDSLienHe(string SDT, string Email, string MaNV)
        {
            int i = 0;
            string sqlCheck = @"Select * from DanhSachLienHe where MaNV = '" + MaNV + "'";
            DataTable tb = db.ExecuteDataSet(sqlCheck, CommandType.Text, "server37", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    string sql = @"UPDATE DanhSachLienHe SET DienThoaiSMS = @DienThoaiSMS, Email = @Email WHERE MaNV = @MaNV";
                    List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                    Param.Add(new DBase.AddParameters("@MaNV", MaNV));
                    Param.Add(new DBase.AddParameters("@Email", Email));
                    Param.Add(new DBase.AddParameters("@DienThoaiSMS", SDT));
                    i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
                }
                else
                {
                    string sql = "INSERT INTO [DanhSachLienHe] ([MaNV] ,[DienThoaiSMS], [Email]) VALUES ( @MaNV,@DienThoaiSMS,@Email)";
                    List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                    Param.Add(new DBase.AddParameters("@MaNV", MaNV));
                    Param.Add(new DBase.AddParameters("@Email", Email));
                    Param.Add(new DBase.AddParameters("@DienThoaiSMS", SDT));
                    i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
                }
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
