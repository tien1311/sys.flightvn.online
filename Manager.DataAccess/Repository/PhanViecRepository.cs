using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Net;
using System.Globalization;
using System.Data;
using System.Collections.Generic;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class PhanViecRepository
    {
        DBase db = new DBase();
        public CongViecModel ListNhanVien()
        {
            string sqlNV = @"select Yahoo,Ten,PhongBan from DM_NV where TinhTrang = '1' order by PhongBan";
            DataTable dtNV = db.ExecuteDataSet(sqlNV, CommandType.Text, "server37", null).Tables[0];

            string sqlPB = @"select PhongBan, MaPhongBan from DM_NV where TinhTrang = '1' group by PhongBan,MaPhongBan order by PhongBan";
            DataTable dtPB = db.ExecuteDataSet(sqlPB, CommandType.Text, "server37", null).Tables[0];

            CongViecModel CongViec = new CongViecModel();
            List<PhongBanModel> listPB = new List<PhongBanModel>();
            List<NhanVienModel> danhsach_NV = new List<NhanVienModel>();

            for (int i = 0; i < dtPB.Rows.Count; i++)
            {

                PhongBanModel PB = new PhongBanModel();
                PB.PB = dtPB.Rows[i]["PhongBan"].ToString();
                PB.MaPB = dtPB.Rows[i]["MaPhongBan"].ToString();
                listPB.Add(PB);
            }
            for (int i = 0; i < dtNV.Rows.Count; i++)
            {

                NhanVienModel NV = new NhanVienModel();
                NV.MaNV = dtNV.Rows[i]["Yahoo"].ToString();
                NV.Ten = dtNV.Rows[i]["Ten"].ToString();
                NV.PhongBan = dtNV.Rows[i]["PhongBan"].ToString();

                string sqlCV = @"select top 1 * from PHANVIEC_QUYDINH_NV where MaNV = '" + dtNV.Rows[i]["Yahoo"].ToString() + "'";
                DataTable dtCV = db.ExecuteDataSet(sqlCV, CommandType.Text, "server37", null).Tables[0];
                if (dtCV.Rows.Count == 0)
                {
                    NV.PhanViec = "Tạo mới";
                    NV.QuyDinh = "Tạo mới";
                    NV.PhanViecChung = "Tạo mới";
                    NV.QuyDinhChung = "Tạo mới";
                }
                else
                {
                    if (dtCV.Rows[0]["PhanViec"].ToString() == "")
                    {
                        NV.PhanViec = "Tạo mới";
                    }
                    else
                    {
                        NV.PhanViec = "Chỉnh sửa";
                    }
                    if (dtCV.Rows[0]["QuyDinh"].ToString() == "")
                    {
                        NV.QuyDinh = "Tạo mới";
                    }
                    else
                    {
                        NV.QuyDinh = "Chỉnh sửa";
                    }
                    if (dtCV.Rows[0]["PhanViecChung"].ToString() == "")
                    {
                        NV.PhanViecChung = "Tạo mới";
                    }
                    else
                    {
                        NV.PhanViecChung = "Chỉnh sửa";
                    }
                    if (dtCV.Rows[0]["QuyDinhChung"].ToString() == "")
                    {
                        NV.QuyDinhChung = "Tạo mới";
                    }
                    else
                    {
                        NV.QuyDinhChung = "Chỉnh sửa";
                    }

                }
                danhsach_NV.Add(NV);
            }
            CongViec.ListNV = danhsach_NV;
            CongViec.ListPB = listPB;

            return CongViec;
        }
        public IEnumerable<AccountModel> GetListNV(string id)
        {
            List<AccountModel> listNV = new List<AccountModel>();
            string sqlNV = @"select * from DM_NV where TinhTrang = '1' and MaPhongBan = '" + id + "'";
            DataTable dtNV = db.ExecuteDataSet(sqlNV, CommandType.Text, "server37", null).Tables[0];

            for (int i = 0; i < dtNV.Rows.Count; i++)
            {
                AccountModel NV = new AccountModel();
                NV.MaNV = dtNV.Rows[i]["Yahoo"].ToString();
                NV.HoTen = dtNV.Rows[i]["Ten"].ToString();
                NV.PhongBan = dtNV.Rows[i]["MaPhongBan"].ToString();
                listNV.Add(NV);
            }
            return listNV;
        }
        public NhanVienModel QuyDinh(string MaNV, string CV)
        {
            NhanVienModel result = new NhanVienModel();

            string sql = @"select DM_NV.Yahoo, DM_NV.Ten, DM_NV.PhongBan, PHANVIEC_QUYDINH_NV.QuyDinh from DM_NV left join PHANVIEC_QUYDINH_NV on PHANVIEC_QUYDINH_NV.MaNV = DM_NV.Yahoo where DM_NV.Yahoo = '" + MaNV + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];

            result.MaNV = tb.Rows[0]["Yahoo"].ToString();
            result.Ten = tb.Rows[0]["Ten"].ToString();
            result.PhongBan = tb.Rows[0]["PhongBan"].ToString();
            result.Noidung = tb.Rows[0]["QuyDinh"].ToString();
            result.ThaoTac = CV;
            result.CongViec = "Quy định";
            return result;
        }
        public NhanVienModel PhanViec(string MaNV, string CV)
        {
            NhanVienModel result = new NhanVienModel();

            string sql = @"select DM_NV.Yahoo, DM_NV.Ten, DM_NV.PhongBan, PHANVIEC_QUYDINH_NV.PhanViec from DM_NV left join PHANVIEC_QUYDINH_NV on PHANVIEC_QUYDINH_NV.MaNV = DM_NV.Yahoo where DM_NV.Yahoo = '" + MaNV + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];

            result.MaNV = tb.Rows[0]["Yahoo"].ToString();
            result.Ten = tb.Rows[0]["Ten"].ToString();
            result.PhongBan = tb.Rows[0]["PhongBan"].ToString();
            result.Noidung = tb.Rows[0]["PhanViec"].ToString();
            result.ThaoTac = CV;
            result.CongViec = "Phân việc";
            return result;
        }
        public NhanVienModel QuyDinhChung(string MaNV, string CV)
        {
            NhanVienModel result = new NhanVienModel();

            string sql = @"select DM_NV.Yahoo, DM_NV.Ten, DM_NV.PhongBan, PHANVIEC_QUYDINH_NV.QuyDinhChung from DM_NV left join PHANVIEC_QUYDINH_NV on PHANVIEC_QUYDINH_NV.MaNV = DM_NV.Yahoo where DM_NV.Yahoo = '" + MaNV + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];

            result.MaNV = tb.Rows[0]["Yahoo"].ToString();
            result.Ten = tb.Rows[0]["Ten"].ToString();
            result.PhongBan = tb.Rows[0]["PhongBan"].ToString();
            result.Noidung = tb.Rows[0]["QuyDinhChung"].ToString();
            result.ThaoTac = CV;
            result.CongViec = "Quy định chung";
            return result;
        }
        public NhanVienModel PhanViecChung(string MaNV, string CV)
        {
            NhanVienModel result = new NhanVienModel();

            string sql = @"select DM_NV.Yahoo, DM_NV.Ten, DM_NV.PhongBan, PHANVIEC_QUYDINH_NV.PhanViecChung from DM_NV left join PHANVIEC_QUYDINH_NV on PHANVIEC_QUYDINH_NV.MaNV = DM_NV.Yahoo where DM_NV.Yahoo = '" + MaNV + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];

            result.MaNV = tb.Rows[0]["Yahoo"].ToString();
            result.Ten = tb.Rows[0]["Ten"].ToString();
            result.PhongBan = tb.Rows[0]["PhongBan"].ToString();
            result.Noidung = tb.Rows[0]["PhanViecChung"].ToString();
            result.ThaoTac = CV;
            result.CongViec = "Phân việc chung";
            return result;
        }
        public bool LuuCongViec(string MaNV, string CongViec, string ThaoTac, string NoiDung)
        {
            string sql = "", sqlListNV = "";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@MaNV", MaNV));
            Param.Add(new DBase.AddParameters("@NoiDung", NoiDung));
            if (CongViec == "Phân việc")
            {
                if (ThaoTac == "Tạo mới")
                {
                    sqlListNV = @"SELECT * FROM PHANVIEC_QUYDINH_NV WHERE MaNV = '" + MaNV + "'";
                    DataTable tb = db.ExecuteDataSet(sqlListNV, CommandType.Text, "server37", null).Tables[0];
                    if (tb.Rows.Count != 0)
                    {
                        sql = @"UPDATE PHANVIEC_QUYDINH_NV SET PhanViec = @NoiDung WHERE MaNV = @MaNV";

                    }
                    else
                    {
                        sql = @"INSERT INTO PHANVIEC_QUYDINH_NV (MaNV, PhanViec) VALUES (@MaNV, @NoiDung)";
                    }
                }
                else
                {
                    sql = @"UPDATE PHANVIEC_QUYDINH_NV SET PhanViec = @NoiDung WHERE MaNV = @MaNV";
                }
            }
            if (CongViec == "Quy định")
            {
                if (ThaoTac == "Tạo mới")
                {
                    sqlListNV = @"SELECT * FROM PHANVIEC_QUYDINH_NV WHERE MaNV = '" + MaNV + "'";
                    DataTable tb = db.ExecuteDataSet(sqlListNV, CommandType.Text, "server37", null).Tables[0];
                    if (tb.Rows.Count != 0)
                    {
                        sql = @"UPDATE PHANVIEC_QUYDINH_NV SET QuyDinh = @NoiDung WHERE MaNV = @MaNV";
                    }
                    else
                    {
                        sql = @"INSERT INTO PHANVIEC_QUYDINH_NV (MaNV, QuyDinh) VALUES (@MaNV, @NoiDung)";
                    }
                }
                else
                {
                    sql = @"UPDATE PHANVIEC_QUYDINH_NV SET QuyDinh = @NoiDung WHERE MaNV = @MaNV";
                }
            }
            if (CongViec == "Phân việc chung")
            {
                if (ThaoTac == "Tạo mới")
                {
                    sqlListNV = @"SELECT * FROM PHANVIEC_QUYDINH_NV WHERE MaNV = '" + MaNV + "'";
                    DataTable tb = db.ExecuteDataSet(sqlListNV, CommandType.Text, "server37", null).Tables[0];
                    if (tb.Rows.Count != 0)
                    {
                        sql = @"UPDATE PHANVIEC_QUYDINH_NV SET PhanViecChung = @NoiDung WHERE MaNV = @MaNV";
                    }
                    else
                    {
                        sql = @"INSERT INTO PHANVIEC_QUYDINH_NV (MaNV, PhanViecChung) VALUES (@MaNV, @NoiDung)";
                    }
                }
                else
                {
                    sql = @"UPDATE PHANVIEC_QUYDINH_NV SET PhanViecChung = @NoiDung WHERE MaNV = @MaNV";
                }
            }
            if (CongViec == "Quy định chung")
            {
                if (ThaoTac == "Tạo mới")
                {
                    sqlListNV = @"SELECT * FROM PHANVIEC_QUYDINH_NV WHERE MaNV = '" + MaNV + "'";
                    DataTable tb = db.ExecuteDataSet(sqlListNV, CommandType.Text, "server37", null).Tables[0];
                    if (tb.Rows.Count != 0)
                    {
                        sql = @"UPDATE PHANVIEC_QUYDINH_NV SET QuyDinhChung = @NoiDung WHERE MaNV = @MaNV";
                    }
                    else
                    {
                        sql = @"INSERT INTO PHANVIEC_QUYDINH_NV (MaNV, QuyDinhChung) VALUES (@MaNV, @NoiDung)";
                    }
                }
                else
                {
                    sql = @"UPDATE PHANVIEC_QUYDINH_NV SET QuyDinhChung = @NoiDung WHERE MaNV = @MaNV";
                }
            }

            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveCongViecPB(string MaPB, string CongViec, string NoiDung)
        {
            string sql = "", sqlCV = "", sqlListNV = "";
            int x = 0;
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@NoiDung", NoiDung));
            sqlListNV = @"select Yahoo from DM_NV where TinhTrang = '1' and MaPhongBan = '" + MaPB.Trim() + "'";
            DataTable tbNV = db.ExecuteDataSet(sqlListNV, CommandType.Text, "server37", null).Tables[0];

            if (CongViec == "1")
            {
                for (int i = 0; i < tbNV.Rows.Count; i++)
                {
                    sqlCV = @"SELECT * FROM PHANVIEC_QUYDINH_NV WHERE MaNV = '" + tbNV.Rows[i]["Yahoo"].ToString() + "'";
                    DataTable tbCV = db.ExecuteDataSet(sqlCV, CommandType.Text, "server37", null).Tables[0];
                    if (tbCV.Rows.Count != 0)
                    {
                        sql = @"UPDATE PHANVIEC_QUYDINH_NV SET PhanViecChung = @NoiDung WHERE MaNV = '" + tbNV.Rows[i]["Yahoo"].ToString() + "'";
                        x = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
                    }
                    else
                    {
                        sql = @"INSERT INTO PHANVIEC_QUYDINH_NV (MaNV, PhanViecChung) VALUES ('" + tbNV.Rows[i]["Yahoo"].ToString() + "', @NoiDung)";
                        x = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
                    }
                }
            }
            else
            {
                for (int i = 0; i < tbNV.Rows.Count; i++)
                {
                    sqlCV = @"SELECT * FROM PHANVIEC_QUYDINH_NV WHERE MaNV = '" + tbNV.Rows[i]["Yahoo"].ToString() + "'";
                    DataTable tbCV = db.ExecuteDataSet(sqlCV, CommandType.Text, "server37", null).Tables[0];
                    if (tbCV.Rows.Count != 0)
                    {
                        sql = @"UPDATE PHANVIEC_QUYDINH_NV SET QuyDinhChung = @NoiDung WHERE MaNV = '" + tbNV.Rows[i]["Yahoo"].ToString() + "'";
                        x = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
                    }
                    else
                    {
                        sql = @"INSERT INTO PHANVIEC_QUYDINH_NV (MaNV, QuyDinhChung) VALUES ('" + tbNV.Rows[i]["Yahoo"].ToString() + "', @NoiDung)";
                        x = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
                    }
                }
            }

            if (x > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
