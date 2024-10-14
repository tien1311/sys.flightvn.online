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
using Manager.Model.Models;

namespace Manager.DataAccess.Repository
{

    public class DecentralizationRepository
    {
        DBase db = new DBase();
        public List<DecentralizationModel> Phanquyen()
        {
            List<DecentralizationModel> result = new List<DecentralizationModel>();
            string sqlNV = @"select DISTINCT ID,Name from POSITION ";
            DataTable dtNV = db.ExecuteDataSet(sqlNV, CommandType.Text, "server37", null).Tables[0];
            if (dtNV != null)
            {
                if (dtNV.Rows.Count > 0)
                {
                    for (int a = 0; a < dtNV.Rows.Count; a++)
                    {
                        string danhSachTinhNang = "";
                        DecentralizationModel ds = new DecentralizationModel();
                        ds.RowID = dtNV.Rows[a]["ID"].ToString();
                        ds.Phongban = dtNV.Rows[a]["Name"].ToString();

                        List<Dschucnang> CN = new List<Dschucnang>();
                        string sql_CN = "select DISTINCT b.FeatureName from  PERMISSION_SYS a inner join FEATURE_SYS b on a.FeatureID = b.ID  where a.PositionID = '" + dtNV.Rows[a]["ID"].ToString() + "'";
                        DataTable dt = db.ExecuteDataSet(sql_CN, CommandType.Text, "server37", null).Tables[0];
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {

                                for (int b = 0; b < dt.Rows.Count; b++)
                                {
                                    if (b == 0)
                                    {
                                        danhSachTinhNang = dt.Rows[b]["FeatureName"].ToString();
                                    }
                                    else
                                    {
                                        danhSachTinhNang += ", " + dt.Rows[b]["FeatureName"].ToString();
                                    }



                                }
                                ds.Dschucnang = danhSachTinhNang;
                            }
                        }

                        result.Add(ds);
                    }
                }
            }

            return result;
        }
        public List<DecentralizationModel> Chitietphanquyen(string khoachinh)
        {
            List<DecentralizationModel> result = new List<DecentralizationModel>();
            string sqlNV = @"select DISTINCT ID,Name from POSITION where ID='" + khoachinh + "' ";
            DataTable dtNV = db.ExecuteDataSet(sqlNV, CommandType.Text, "server37", null).Tables[0];
            if (dtNV != null)
            {
                if (dtNV.Rows.Count > 0)
                {
                    for (int b = 0; b < dtNV.Rows.Count; b++)
                    {

                        DecentralizationModel dsphongban = new DecentralizationModel();
                        dsphongban.RowID = dtNV.Rows[b]["ID"].ToString();
                        dsphongban.Phongban = dtNV.Rows[b]["Name"].ToString();

                        List<Checkchucnang> result1 = new List<Checkchucnang>();
                        string sqlPhongban = @"select DISTINCT b.FeatureName from  PERMISSION_SYS a inner join FEATURE_SYS b on a.FeatureID = b.ID where a.PositionID = '" + dtNV.Rows[b]["ID"].ToString() + "'";
                        DataTable dtPhongban = db.ExecuteDataSet(sqlPhongban, CommandType.Text, "server37", null).Tables[0];
                        if (dtPhongban != null)
                        {
                            if (dtPhongban.Rows.Count >= 0)
                            {
                                Checkchucnang ds = new Checkchucnang();
                                for (int a = 0; a < dtPhongban.Rows.Count; a++)
                                {

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "Tính năng mới")
                                    {
                                        ds.Tinhnangmoi = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "Thông báo")
                                    {
                                        ds.Thongbao = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "Báo cáo vé")
                                    {
                                        ds.Baocaove = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "Nội bộ")
                                    {
                                        ds.Noibo = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "Đại lí")
                                    {
                                        ds.Daili = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "Kế toán")
                                    {
                                        ds.Ketoan = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "Kinh doanh")
                                    {
                                        ds.Kinhdoanh = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "Phòng vé")
                                    {
                                        ds.Phongve = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "BP đoàn")
                                    {
                                        ds.BPdoan = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "Hóa đơn")
                                    {
                                        ds.Hoadon = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "CA")
                                    {
                                        ds.Ca = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "Yến sào")
                                    {
                                        ds.Yensao = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "CS")
                                    {
                                        ds.Cs = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "Data")
                                    {
                                        ds.Data = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "Setting")
                                    {
                                        ds.Setting = "true";
                                    }

                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "Kỹ thuật")
                                    {
                                        ds.Kythuat = "true";
                                    }
                                    if (dtPhongban.Rows[a]["FeatureName"].ToString().Trim() == "Du lịch")
                                    {
                                        ds.Dulich = "true";
                                    }

                                }
                                result1.Add(ds);

                            }
                            dsphongban.Dschucnangphongban = result1;
                        }
                        result.Add(dsphongban);
                    }

                }
            }

            return result;
        }
        public bool Savephanquyen(string maphongban, string tinhnangmoi, string thongbao, string baocaove, string noibo, string daili, string ketoan, string kinhdoanh, string phongve, string bpdoan, string hoadon, string ca, string yensao, string cs, string data, string setting, string kythuat, string dulich)
        {
            string sqlNV = @"Delete from  PERMISSION_SYS  where PositionID = '" + maphongban + "'";
            int dtDelete = db.ExecuteNoneQuery(sqlNV, CommandType.Text, "server37", null);
            if (dtDelete >= 0)
            {
                if (tinhnangmoi == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','1')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (thongbao == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','2')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (baocaove == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','3')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (noibo == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','4')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (daili == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','5')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (ketoan == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','6')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (kinhdoanh == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','7')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (phongve == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','8')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (bpdoan == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','9')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (hoadon == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','10')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (ca == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','11')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (yensao == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','12')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (cs == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','13')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (data == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','14')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (setting == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','15')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (kythuat == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','16')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (dulich == "true")
                {
                    string sqlInsert = @"Insert into PERMISSION_SYS (PositionID,FeatureID) VALUES  ('" + maphongban + "','98')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<DecentralizationMemberModel> Phanquyenmember()
        {
            List<DecentralizationMemberModel> result = new List<DecentralizationMemberModel>();
            string sqlNV = @"select DISTINCT a.Yahoo,a.TEN from  PERMISION_SYS_NV b inner join DM_NV a on b.MANV=a.Yahoo ";
            DataTable dtNV = db.ExecuteDataSet(sqlNV, CommandType.Text, "server37", null).Tables[0];
            if (dtNV != null)
            {
                if (dtNV.Rows.Count > 0)
                {
                    for (int a = 0; a < dtNV.Rows.Count; a++)
                    {
                        string danhSachTinhNang = "";
                        DecentralizationMemberModel ds = new DecentralizationMemberModel();

                        ds.MaNV = dtNV.Rows[a]["Yahoo"].ToString();
                        ds.Tennhanvien = dtNV.Rows[a]["TEN"].ToString();

                        List<Dschucnang> CN = new List<Dschucnang>();
                        string sql_CN = "select DISTINCT b.FeatureName from  PERMISION_SYS_NV a inner join FEATURE_SYS b on a.FeatureID = b.ID  where a.MaNV = '" + dtNV.Rows[a]["Yahoo"].ToString() + "'";
                        DataTable dt = db.ExecuteDataSet(sql_CN, CommandType.Text, "server37", null).Tables[0];
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {

                                for (int b = 0; b < dt.Rows.Count; b++)
                                {
                                    if (b == 0)
                                    {
                                        danhSachTinhNang = dt.Rows[b]["FeatureName"].ToString();
                                    }
                                    else
                                    {
                                        danhSachTinhNang += ", " + dt.Rows[b]["FeatureName"].ToString();
                                    }



                                }
                                ds.Dschucnang = danhSachTinhNang;
                            }
                        }

                        result.Add(ds);
                    }
                }
            }

            return result;
        }
        public List<Checkchucnang> Chitietphanquyenmember(string khoachinh)
        {
            List<Checkchucnang> result = new List<Checkchucnang>();
            string sqlNV = @"select DISTINCT a.MaNV, b.FeatureName from  PERMISION_SYS_NV a inner join FEATURE_SYS b on a.FeatureID = b.ID  where a.MaNV = '" + khoachinh + "'";
            DataTable dtNV = db.ExecuteDataSet(sqlNV, CommandType.Text, "server37", null).Tables[0];
            if (dtNV != null)
            {
                if (dtNV.Rows.Count > 0)
                {
                    Checkchucnang ds = new Checkchucnang();
                    for (int a = 0; a < dtNV.Rows.Count; a++)
                    {

                        ds.RowID = dtNV.Rows[a]["MaNV"].ToString();
                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "Tính năng mới")
                        {
                            ds.Tinhnangmoi = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "Thông báo")
                        {
                            ds.Thongbao = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "Báo cáo vé")
                        {
                            ds.Baocaove = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "Nội bộ")
                        {
                            ds.Noibo = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "Đại lí")
                        {
                            ds.Daili = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "Kế toán")
                        {
                            ds.Ketoan = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "Kinh doanh")
                        {
                            ds.Kinhdoanh = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "Phòng vé")
                        {
                            ds.Phongve = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "BP đoàn")
                        {
                            ds.BPdoan = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "Hóa đơn")
                        {
                            ds.Hoadon = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "CA")
                        {
                            ds.Ca = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "Yến sào")
                        {
                            ds.Yensao = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "CS")
                        {
                            ds.Cs = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "Data")
                        {
                            ds.Data = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "Setting")
                        {
                            ds.Setting = "true";
                        }

                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "Kỹ thuật")
                        {
                            ds.Kythuat = "true";
                        }
                        if (dtNV.Rows[a]["FeatureName"].ToString().Trim() == "Du lịch")
                        {
                            ds.Dulich = "true";
                        }

                    }
                    result.Add(ds);

                }
            }

            return result;
        }
        public bool CheckNV(string manv)
        {
            string sqlChecknv = @"select * from DM_NV where Yahoo='" + manv.Trim() + "'";
            DataTable dtNV = db.ExecuteDataSet(sqlChecknv, CommandType.Text, "server37", null).Tables[0];
            if (dtNV.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckCN(string manv)
        {
            string sqlChecknv = @"select * from PERMISION_SYS_NV where MaNV='" + manv.Trim() + "'";
            DataTable dtNV = db.ExecuteDataSet(sqlChecknv, CommandType.Text, "server37", null).Tables[0];
            if (dtNV.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool Savephanquyenmember(string manv, string tinhnangmoi, string thongbao, string baocaove, string noibo, string daili, string ketoan, string kinhdoanh, string phongve, string bpdoan, string hoadon, string ca, string yensao, string cs, string data, string setting, string kythuat, string dulich)
        {
            string sqlNV = @"Delete from  PERMISION_SYS_NV  where MANV = '" + manv + "'";
            int dtDelete = db.ExecuteNoneQuery(sqlNV, CommandType.Text, "server37", null);
            if (dtDelete >= 0)
            {
                if (tinhnangmoi == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','1')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (thongbao == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','2')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (baocaove == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','3')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (noibo == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','4')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (daili == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','5')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (ketoan == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','6')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (kinhdoanh == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','7')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (phongve == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','8')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (bpdoan == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','9')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (hoadon == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','10')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (ca == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','11')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (yensao == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','12')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (cs == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','13')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (data == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','14')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (setting == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','15')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (kythuat == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','16')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                if (dulich == "true")
                {
                    string sqlInsert = @"Insert into PERMISION_SYS_NV (MANV,FeatureID) VALUES  ('" + manv + "','98')";
                    int dtInsert = db.ExecuteNoneQuery(sqlInsert, CommandType.Text, "server37", null);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public Chitietnhanvien Chitietnhanvien(string manv)
        {
            Chitietnhanvien nhanvien = new Chitietnhanvien();
            string sqlChecknv = @"select * from DM_NV where Yahoo='" + manv.Trim() + "'";
            DataTable dtNV = db.ExecuteDataSet(sqlChecknv, CommandType.Text, "server37", null).Tables[0];
            if (dtNV != null)
            {
                if (dtNV.Rows.Count > 0)
                {
                    for (int i = 0; i < dtNV.Rows.Count; i++)
                    {

                        nhanvien.Tennhanvien = dtNV.Rows[i]["Ten"].ToString();
                        nhanvien.MaNV = dtNV.Rows[i]["Yahoo"].ToString();
                        nhanvien.Phongban = dtNV.Rows[i]["PhongBan"].ToString();

                        List<Dschucnangmember> Dschucnangmember = new List<Dschucnangmember>();
                        string sqlCheck = @"select * from PERMISION_SYS_NV where MaNV='" + manv.Trim() + "'";
                        DataTable dt = db.ExecuteDataSet(sqlCheck, CommandType.Text, "server37", null).Tables[0];
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                for (int a = 0; a < dt.Rows.Count; a++)
                                {
                                    Dschucnangmember Ds = new Dschucnangmember();
                                    Ds.Dschucnang = dt.Rows[a]["MaNV"].ToString();
                                    Dschucnangmember.Add(Ds);
                                }
                                nhanvien.Dschucnangmember = Dschucnangmember;
                            }
                        }
                    }

                }
            }
            return nhanvien;
        }
    }
}
