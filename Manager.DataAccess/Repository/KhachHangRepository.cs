using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;
//using Manager_Manager.Models.ViewModel;
using System.Data;
using System.Globalization;
using Manager.Model.Models;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class KhachHangRepository
    {
      
        DBase db = new DBase();
        KhachHangVIPModel nhomDaiLyModel = new KhachHangVIPModel();
        private readonly string _connectionString;
        public KhachHangRepository(IConfiguration configuration)
        {

            _connectionString = configuration.GetConnectionString("SQL_EV_MAIN");
            
        }
        public List<KhachHangModel> KhachHangVip()
        {
            List<KhachHangModel> result = new List<KhachHangModel>();
            string sql = "select *, thangsinh = MONTH(NgaySinh) from KHACHHANGVIP order by MONTH(NgaySinh), DAY(NgaySinh)";
            DataTable dt = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    KhachHangModel KH = new KhachHangModel();
                    KH.ID = dt.Rows[i]["ROWID"].ToString();
                    KH.Hoten = dt.Rows[i]["HOTEN"].ToString();
                    KH.Ghichu = dt.Rows[i]["GHICHU"].ToString();
                    KH.Chuc = dt.Rows[i]["DONVI"].ToString();
                    KH.Diachi = dt.Rows[i]["DIACHI"].ToString();
                    KH.NGT = dt.Rows[i]["NguoiGioiThieu"].ToString();
                    KH.Hang = dt.Rows[i]["Hang"].ToString();
                    KH.Mien = dt.Rows[i]["Mien"].ToString();
                    KH.IsHotro = dt.Rows[i]["IsHoTro"].ToString();
                    KH.Thangsinh = dt.Rows[i]["thangsinh"].ToString();
                    if (dt.Rows[i]["NGAYLAP"].ToString() != "")
                    {
                        KH.Ngaylap = DateTime.Parse(dt.Rows[i]["NGAYLAP"].ToString()).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        KH.Ngaylap = dt.Rows[i]["NGAYLAP"].ToString();
                    }
                    if (dt.Rows[i]["NgaySinh"].ToString() != "")
                    {
                        KH.Ngaysinh = DateTime.Parse(dt.Rows[i]["NgaySinh"].ToString()).ToString("dd/MM/yyyy");

                    }
                    else
                    {
                        KH.Ngaysinh = dt.Rows[i]["NgaySinh"].ToString();
                    }
                    KH.Nguoilap = dt.Rows[i]["NGUOILAP"].ToString();
                    KH.SDT = dt.Rows[i]["SODIENTHOAI"].ToString();
                    result.Add(KH);
                }
            }

            return result;
        }
        public List<ListNhanVienKinhDoanh> DSNhanVienKinhDoanh()
        {
            List<ListNhanVienKinhDoanh> result = new List<ListNhanVienKinhDoanh>();
            string sql_NoiDung = "SELECT TENDANGNHAP,Ten as TENNV, Yahoo FROM DM_NV WHERE (MaPhongBan='KD'or MaPhongBan='KDC') and TinhTrang=1";
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListNhanVienKinhDoanh ten = new ListNhanVienKinhDoanh();
                        ten.Ten = dt.Rows[i]["TenNV"].ToString();
                        ten.MaNV = dt.Rows[i]["Yahoo"].ToString();
                        result.Add(ten);
                    }
                }
            }
            return result;
        }
        public KhachHangModel ChiTietKH(string ID)
        {
            List<QuaTangModel> ListQT = new List<QuaTangModel>();
            string sql = @"select * from KHACHHANGVIP where ROWID = '" + ID + "' ";
            DataTable dt = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];

            KhachHangModel KH = new KhachHangModel();
            if (dt != null && dt.Rows.Count > 0)
            {
                KH.ID = dt.Rows[0]["ROWID"].ToString();
                KH.Hoten = dt.Rows[0]["HOTEN"].ToString();
                KH.Ghichu = dt.Rows[0]["GHICHU"].ToString();
                KH.NGT = dt.Rows[0]["NGUOIGIOITHIEU"].ToString();
                KH.Hang = dt.Rows[0]["Hang"].ToString();
                KH.Mien = dt.Rows[0]["Mien"].ToString();
                KH.TangSN = dt.Rows[0]["IsTangSN"].ToString();
                KH.Chuc = dt.Rows[0]["DONVI"].ToString();
                KH.Diachi = dt.Rows[0]["DIACHI"].ToString();
                KH.IsHotro = dt.Rows[0]["IsHoTro"].ToString();
                KH.Nhom = dt.Rows[0]["NHOM"].ToString(); 
                KH.Nhanvienkinhdoanh = dt.Rows[0]["NHANVIENKINHDOANH"].ToString();
                if (dt.Rows[0]["NGAYLAP"].ToString() != "")
                {
                    KH.Ngaylap = DateTime.Parse(dt.Rows[0]["NGAYLAP"].ToString()).ToString("dd/MM/yyyy");
                }
                else
                {
                    KH.Ngaylap = dt.Rows[0]["NGAYLAP"].ToString();
                }
                if (dt.Rows[0]["NgaySinh"].ToString() != "")
                {
                    KH.Ngaysinh = DateTime.Parse(dt.Rows[0]["NgaySinh"].ToString()).ToString("dd/MM/yyyy");
                }
                else
                {
                    KH.Ngaysinh = dt.Rows[0]["NgaySinh"].ToString();
                }
                KH.Nguoilap = dt.Rows[0]["NGUOILAP"].ToString();
                KH.SDT = dt.Rows[0]["SODIENTHOAI"].ToString();

                string sqlQT = @"select * from QUATANG where IDKhachHangVip = '" + ID + "' ";
                DataTable dtQT = db.ExecuteDataSet(sqlQT, CommandType.Text, "server37", null).Tables[0];
                if (dtQT != null && dtQT.Rows.Count > 0)
                {
                    for (int i = 0; i < dtQT.Rows.Count; i++)
                    {
                        QuaTangModel QT = new QuaTangModel();
                        QT.Nam = dtQT.Rows[i]["Nam"].ToString();
                        QT.QuaTang = dtQT.Rows[i]["TenQuaTang"].ToString();
                        ListQT.Add(QT);
                    }
                }

                KH.Listquatang = ListQT;
            }

            return KH;
        }
        public bool EditKHV(string ID, string name, string Chucvu, string hang, string mien, string NGT, string phone, string birthday, string address, int IsHotro, string ngaytang, string gift, string note, string nhom, string nvkd, string tennv)
        {
            string sinhnhat = "";
            if (note == null)
            {
                note = "";
            }
            if (birthday != null)
            {
                sinhnhat = DateTime.ParseExact(birthday, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

            string sqlUpdate = @"
        UPDATE KHACHHANGVIP 
        SET HOTEN = @NAME, 
            DONVI = @UNIT, 
            DIACHI = @ADDRESS, 
            SODIENTHOAI = @PHONE, 
            NGUOISUA = @TENNV, 
            NGAYSUA = GETDATE(), 
            NgaySinh = @BIRTHDAY, 
            GHICHU = @NOTE, 
            NguoiGioiThieu = @NGT, 
            Hang = @HANG, 
            Mien = @MIEN, 
            IsHoTro = @IsHotro,
            NHOM = @NHOM,
            NHANVIENKINHDOANH = @NHANVIENKINHDOANH
        WHERE ROWID = @ID";

            var parametersUpdate = new DynamicParameters();
            parametersUpdate.Add("@NAME", name);
            parametersUpdate.Add("@UNIT", Chucvu);
            parametersUpdate.Add("@ADDRESS", address);
            parametersUpdate.Add("@PHONE", phone);
            parametersUpdate.Add("@TENNV", tennv);
            parametersUpdate.Add("@BIRTHDAY", sinhnhat);
            parametersUpdate.Add("@NOTE", note);
            parametersUpdate.Add("@ID", ID);
            parametersUpdate.Add("@HANG", hang);
            parametersUpdate.Add("@MIEN", mien);
            parametersUpdate.Add("@NGT", NGT);
            parametersUpdate.Add("@IsHotro", IsHotro);
            parametersUpdate.Add("@NHOM", nhom);
            parametersUpdate.Add("@NHANVIENKINHDOANH", nvkd);

            using (var connection = new SqlConnection(_connectionString))
            {
                int rowsAffected = connection.Execute(sqlUpdate, parametersUpdate, commandType: CommandType.Text);

                if (rowsAffected > 0)
                {
                    if (gift != null && ngaytang != null)
                    {
                        string sqlInsertGift = @"
                    INSERT INTO [QUATANG] ([IDKhachHangVip], [Nam], [TenQuaTang]) 
                    VALUES (@ID, @NGAYTANG, @GIFT)";

                        var parametersInsertGift = new DynamicParameters();
                        parametersInsertGift.Add("@GIFT", gift);
                        parametersInsertGift.Add("@NGAYTANG", ngaytang);
                        parametersInsertGift.Add("@ID", ID);

                        connection.Execute(sqlInsertGift, parametersInsertGift, commandType: CommandType.Text);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool SaveKHV(string name, string Chucvu, string hang, string mien, string NGT, string phone, string birthday, string address, int IsHotro, string ngaytang, string gift, string note, string nhom, string nvkd, string tennv)
        {
            string sinhnhat = "";

          
            if (note == null)
            {
                note = "";
            }

           
            if (birthday != null)
            {
                sinhnhat = DateTime.ParseExact(birthday, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

           
            string sqlInsert = @"
        INSERT INTO [dbo].[KHACHHANGVIP] 
        ([HOTEN], [DONVI], [DIACHI], [SODIENTHOAI], [KHUYENMAI], [NGUOILAP], [NGAYLAP], [NGUOISUA], [NGAYSUA], [NgaySinh], [GHICHU], [NGUOIGIOITHIEU], [HANG], [MIEN], [IsTangSN], [IsHoTro], [NHOM], [NHANVIENKINHDOANH])
        VALUES 
        (@HOTEN, @DONVI, @DIACHI, @SODIENTHOAI, @KHUYENMAI, @NGUOILAP, GETDATE(), NULL, NULL, @NgaySinh, @GHICHU, @NGUOIGIOITHIEU, @HANG, @MIEN, NULL, @IsHoTro, @NHOM, @NHANVIENKINHDOANH);
        SELECT SCOPE_IDENTITY();";

           
            var parameters = new DynamicParameters();
            parameters.Add("@HOTEN", name);
            parameters.Add("@DONVI", Chucvu);
            parameters.Add("@DIACHI", address);
            parameters.Add("@SODIENTHOAI", phone);
            parameters.Add("@KHUYENMAI", null); 
            parameters.Add("@NGUOILAP", tennv);
            parameters.Add("@NgaySinh", sinhnhat);
            parameters.Add("@GHICHU", note);
            parameters.Add("@HANG", hang);
            parameters.Add("@MIEN", mien);
            parameters.Add("@IsHoTro", IsHotro);
            parameters.Add("@NHOM", nhom);
            parameters.Add("@NHANVIENKINHDOANH", nvkd);
            parameters.Add("@NGUOIGIOITHIEU", NGT);


            int ID;
            using (var connection = new SqlConnection(_connectionString))
            {
                ID = connection.ExecuteScalar<int>(sqlInsert, parameters, commandType: CommandType.Text);
            }

           
            if (gift != null && ngaytang != null)
            {
                string sqlQT = @"
            INSERT INTO [QUATANG] ([IDKhachHangVip], [Nam], [TenQuaTang]) 
            VALUES (@ID, @NGAYTANG, @GIFT)";

                var parametersQT = new DynamicParameters();
                parametersQT.Add("@GIFT", gift);
                parametersQT.Add("@NGAYTANG", ngaytang);
                parametersQT.Add("@ID", ID);

                using (var connection = new SqlConnection("server37"))
                {
                    connection.Execute(sqlQT, parametersQT, commandType: CommandType.Text);
                }
            }

           
            return ID > 0;
        }



        public Show EditKhachHang(string ID)
        {
            Show result = new Show();
            string sql = @"select *, NHOM.ID as IDNhom from KHACHHANG_VIP KH left join NHOMKH NHOM on KH.NHOMKH = NHOM.ID  where KH.ID = '" + ID + "' ";
            DataTable dt = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];

            result.ID = int.Parse(dt.Rows[0]["ID"].ToString());
            result.MaKH = dt.Rows[0]["MAKH"].ToString();
            result.TenCty = dt.Rows[0]["TENKH"].ToString();
            result.Nhom = dt.Rows[0]["TENNHOM"].ToString();
            result.IDNhom = int.Parse(dt.Rows[0]["IDNhom"].ToString());
            result.Ghichu = dt.Rows[0]["GHICHU"].ToString();
            result.Status = dt.Rows[0]["Status"].ToString();
            result.ListNhomDL = NhomDL().ListNhomDL;
            return result;
        }
        public bool Edit(int ID, string MaKH, string TenKH, string NhomKH, string GhiChu, bool Hidden)
        {

            string sql = "UPDATE KHACHHANG_VIP SET MAKH = @MAKH, TENKH = @TENKH, NHOMKH = @NHOMKH, GHICHU = @GHICHU, Status = @Status WHERE ID = @ID ";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@MAKH", MaKH));
            Param.Add(new DBase.AddParameters("@TENKH", TenKH));
            Param.Add(new DBase.AddParameters("@NHOMKH", NhomKH));
            Param.Add(new DBase.AddParameters("@GHICHU", GhiChu));
            Param.Add(new DBase.AddParameters("@ID", ID));
            Param.Add(new DBase.AddParameters("@Status", Hidden));
            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public KhachHangVIPModel SearchSelect(string NhomDL2)
        {
            try
            {
                KhachHangVIPModel result = new KhachHangVIPModel();
                List<KhachHangVIP> CV = new List<KhachHangVIP>();
                KhachHangVIP show = new KhachHangVIP();
                string sql_NoiDung = " SELECT * FROM KHACHHANG_VIP KH left join NHOMKH NHOM on KH.NHOMKH = NHOM.ID WHERE NHOMKH ='" + NhomDL2 + "' and KH.Status = 0";
                // string sql_NoiDung = " SELECT * FROM QLTHONGBAO WHERE PHANMEM ='Sys.GuiMailKinhDoanh'";
                DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        show = new KhachHangVIP();
                        show.ID = int.Parse(dt.Rows[i]["ID"].ToString());
                        show.MaKH = dt.Rows[i]["MAKH"].ToString();
                        show.TenCty = dt.Rows[i]["TENKH"].ToString();
                        show.Nhom = dt.Rows[i]["TENNHOM"].ToString();
                        show.Ghichu = dt.Rows[i]["GHICHU"].ToString();
                        CV.Add(show);
                    }

                }
                result.ListKhachHangVIP = CV;
                result.ListNhomDL = NhomDL().ListNhomDL;
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public KhachHangVIPModel SearchKH(string MAKH)
        {
            List<NhomKhachHangVIP> result = new List<NhomKhachHangVIP>();
            string sql = "select * from KHACHHANG_HOPDONG where  MAKETOAN like '" + MAKH + "%'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                NhomKhachHangVIP info = new NhomKhachHangVIP();
                info.MaKH = tb.Rows[0]["MAKETOAN"].ToString();
                info.TenCty = tb.Rows[0]["TENCONGTY"].ToString();
                result.Add(info);
            }
            nhomDaiLyModel.ListNhomDL = NhomDL().ListNhomDL;
            nhomDaiLyModel.ListNhomKhachHangVIP = result;
            return nhomDaiLyModel;
        }

        public KhachHangVIPModel NhomDL()
        {

            try
            {
                KhachHangVIPModel result = new KhachHangVIPModel();
                List<KhachHangVIP> CV = new List<KhachHangVIP>();
                KhachHangVIP show = new KhachHangVIP();
                List<NhomDL> ListNhom = new List<NhomDL>();

                string sqlNhomDL = "select * from NHOMKH";
                DataTable tbNhomDL = db.ExecuteDataSet(sqlNhomDL, CommandType.Text, "server37", null).Tables[0];
                if (tbNhomDL != null && tbNhomDL.Rows.Count > 0)
                {
                    for (int i = 0; i < tbNhomDL.Rows.Count; i++)
                    {
                        NhomDL Nhom = new NhomDL();
                        Nhom.IDNhom = int.Parse(tbNhomDL.Rows[i]["ID"].ToString());
                        Nhom.TenNhom = tbNhomDL.Rows[i]["TENNHOM"].ToString();
                        ListNhom.Add(Nhom);
                    }
                }
                result.ListNhomDL = ListNhom;

                string sql = "select * from KHACHHANG_VIP where Status = 0";
                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                //if (tb != null && tb.Rows.Count > 0)
                //{
                //    for (int i = 0; i < tb.Rows.Count; i++)
                //    {
                //        show = new DaiLy();
                //        show.ID = int.Parse(tb.Rows[i]["ID"].ToString());
                //        show.MaKH = tb.Rows[i]["MAKH"].ToString();
                //        show.TenCty = tb.Rows[i]["TENKH"].ToString();
                //        show.Nhom = tb.Rows[i]["NHOMKH"].ToString();
                //        show.Ghichu = tb.Rows[i]["GHICHU"].ToString();

                //        CV.Add(show);
                //    }
                //    result.ListDaiLy = CV;

                //}
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public string InsertDL(string MaKHtxt, string TenCtytxt, string NhomDL1, string ghichutxt)
        {
            if (ghichutxt == null)
            {
                ghichutxt = "";
            }
            string result = "";
            string sqlKT = @"select  top 1 * from KHACHHANG_VIP KH left join NHOMKH NHOM on KH.NHOMKH = NHOM.ID where KH.MAKH = '" + MaKHtxt + "' and KH.Status = 0";
            DataTable tb = db.ExecuteDataSet(sqlKT, CommandType.Text, "server37", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                return result = "Mã " + MaKHtxt + " đã thuộc nhóm " + tb.Rows[0]["TENNHOM"].ToString();
            }
            else
            {
                string sql = "INSERT INTO [KHACHHANG_VIP] ([MAKH] ,[TENKH],[NHOMKH] ,[GHICHU], [Status]) VALUES ( @MAKH,@TENKH,@NHOMKH,@GHICHU,@Status)";
                List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                Param.Add(new DBase.AddParameters("@MAKH", MaKHtxt));
                Param.Add(new DBase.AddParameters("@TENKH", TenCtytxt));
                Param.Add(new DBase.AddParameters("@NHOMKH", NhomDL1));
                Param.Add(new DBase.AddParameters("@GHICHU", ghichutxt));
                Param.Add(new DBase.AddParameters("@Status", false));
                int abc = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
                return result = "Bạn đã lưu thành công";
            }

        }
        public IEnumerable<NhomDL> GetDepartments(int categoryId)
        {
            List<NhomDL> result = new List<NhomDL>();
            string sql = $@"SELECT * from NHOMKH where ID = '" + categoryId + "'";
            var tbl = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            foreach (DataRow row in tbl.Rows)
            {
                result.Add(new NhomDL()
                {
                    NoiDung = row["NOIDUNG"].ToString(),
                });
            }
            return result;
        }
        public List<NhomDL> ListNhomDL()
        {
            List<NhomDL> ListNhom = new List<NhomDL>();
            string sqlNhomDL = "select * from NHOMKH";
            DataTable tbNhomDL = db.ExecuteDataSet(sqlNhomDL, CommandType.Text, "server37", null).Tables[0];
            if (tbNhomDL != null && tbNhomDL.Rows.Count > 0)
            {
                for (int i = 0; i < tbNhomDL.Rows.Count; i++)
                {
                    NhomDL Nhom = new NhomDL();
                    Nhom.IDNhom = int.Parse(tbNhomDL.Rows[i]["ID"].ToString());
                    Nhom.TenNhom = tbNhomDL.Rows[i]["TENNHOM"].ToString();
                    Nhom.NoiDung = tbNhomDL.Rows[i]["NOIDUNG"].ToString();
                    ListNhom.Add(Nhom);
                }
            }

            return ListNhom;
        }
        public NhomDL EditNhomDL(int ID)
        {
            NhomDL Nhom = new NhomDL();
            string sqlNhomDL = "select * from NHOMKH where ID = '" + ID + "'";
            DataTable tbNhomDL = db.ExecuteDataSet(sqlNhomDL, CommandType.Text, "server37", null).Tables[0];
            if (tbNhomDL != null && tbNhomDL.Rows.Count > 0)
            {
                Nhom.IDNhom = int.Parse(tbNhomDL.Rows[0]["ID"].ToString());
                Nhom.TenNhom = tbNhomDL.Rows[0]["TENNHOM"].ToString();
                Nhom.NoiDung = tbNhomDL.Rows[0]["NOIDUNG"].ToString();
            }
            return Nhom;
        }
        public bool SaveCreateNhomDL(string Title, string CreateContent)
        {
            string sql = "INSERT INTO [NHOMKH] ([TENNHOM], [NOIDUNG]) VALUES ( @TENNHOM, @NOIDUNG)";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@TENNHOM", Title));
            Param.Add(new DBase.AddParameters("@NOIDUNG", CreateContent));

            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveEditNhomDL(int ID, string Title, string CreateContent)
        {
            string sql = "UPDATE NHOMKH SET TENNHOM = @TENNHOM, NOIDUNG = @NOIDUNG WHERE ID = @ID ";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@TENNHOM", Title));
            Param.Add(new DBase.AddParameters("@ID", ID));
            Param.Add(new DBase.AddParameters("@NOIDUNG", CreateContent));

            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
