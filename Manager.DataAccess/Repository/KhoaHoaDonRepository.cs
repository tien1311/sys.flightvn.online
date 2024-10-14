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
using System.Globalization;
using Dapper;
using Manager.Model.Models;

namespace Manager.DataAccess.Repository
{
    public class KhoaHoaDonRepository
    {
        DBase db = new DBase();
        public KhoaHoaDon Dulieukhoahoadon()
        {
            KhoaHoaDon result = new KhoaHoaDon();
            string sql = @"SELECT * FROM KHOA_XUATHOADON where ROwID = 2";
            DataTable dulieu = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (dulieu != null)
            {
                if (dulieu.Rows.Count > 0)
                {
                    result.txt_PVL1 = dulieu.Rows[0]["KHOA_01"].ToString();
                    result.txt_PVL2 = dulieu.Rows[0]["KHOA_02"].ToString();
                    result.txt_PVL3 = dulieu.Rows[0]["KHOA_03"].ToString();
                    result.txt_PVL4 = dulieu.Rows[0]["KHOA_04"].ToString();
                    result.txt_GHTN = dulieu.Rows[0]["CHUANGAY"].ToString();
                    result.txt_ChuaL1 = dulieu.Rows[0]["CHUA_L1"].ToString();
                    result.txt_ChuaL2 = dulieu.Rows[0]["CHUA_L2"].ToString();
                    result.txt_ChuaL3 = dulieu.Rows[0]["CHUA_L3"].ToString();
                    result.txt_ChuaL4 = dulieu.Rows[0]["CHUA_L4"].ToString();
                    string sql2 = @"SELECT NGAYGUIQUATHANG FROM LAYNGUON";
                    DataTable tbl2 = db.ExecuteDataSet(sql2, CommandType.Text, "server37", null).Tables[0];
                    result.txt_NgayKhoa = tbl2.Rows[0]["NGAYGUIQUATHANG"].ToString();
                    result.txt_KHHD_1 = dulieu.Rows[0]["KHHD_CHUA_L1"].ToString().Trim();
                    result.txt_KHHD_2 = dulieu.Rows[0]["KHHD_CHUA_L2"].ToString().Trim();
                    result.txt_KHHD_3 = dulieu.Rows[0]["KHHD_CHUA_L3"].ToString().Trim();
                    result.txt_KHHD_4 = dulieu.Rows[0]["KHHD_CHUA_L4"].ToString().Trim();

                    result.txt_Serial_L1 = dulieu.Rows[0]["Serial_L1"].ToString().Trim();
                    result.txt_Serial_L2 = dulieu.Rows[0]["Serial_L2"].ToString().Trim();
                    result.txt_Serial_L3 = dulieu.Rows[0]["Serial_L3"].ToString().Trim();
                    result.txt_Serial_L4 = dulieu.Rows[0]["Serial_L4"].ToString().Trim();

                }
            }


            result.KieuKhoa = DSTen();
            result.PhanMem = DSPhanmem();

            return result;
        }
        public List<KieuKhoa> DSTen()
        {
            List<KieuKhoa> result = new List<KieuKhoa>();
            string sql = @"SELECT ItemKey,ItemValue from List_Khoa_Xuathoadon Order by RowID asc";
            DataTable dt = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    result.Insert(0, new KieuKhoa() { ItemKey = "0", ItemValue = "---Chọn--- " });
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        KieuKhoa KK = new KieuKhoa();
                        KK.ItemKey = dt.Rows[i]["ItemKey"].ToString();
                        KK.ItemValue = dt.Rows[i]["ItemValue"].ToString();

                        result.Add(KK);
                    }

                }
            }

            return result;
        }
        public List<PhanMem> DSPhanmem()
        {
            string txt_ThongBao = "";
            List<PhanMem> result = new List<PhanMem>();
            string sql2 = @"SELECT PHANMEM FROM THONGBAO_DL ORDER BY ROWID ASC";
            DataTable dt1 = db.ExecuteDataSet(sql2, CommandType.Text, "server37", null).Tables[0];
            if (dt1 != null)
            {
                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        PhanMem KK = new PhanMem();
                        KK.Phanmem = dt1.Rows[i]["PhanMem"].ToString();
                        if (dt1.Rows[i]["PhanMem"].ToString() == "VJJS")
                        {
                            KK.Select = "selected";
                        }
                        string sql = "SELECT NOIDUNG FROM THONGBAO_DL WHERE PHANMEM='VJJS'";
                        DataTable tbl = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                        if (tbl.Rows.Count > 0)
                        {
                            txt_ThongBao = tbl.Rows[0][0].ToString();
                        }
                        else
                        {
                            txt_ThongBao = "";
                        }
                        KK.Thongbao = txt_ThongBao;
                        result.Add(KK);
                    }

                }
            }

            return result;
        }
        public int GetKey(string id)
        {
            int Key = 0;
            if (id == "0")
            {
                Key = 0;
            }
            else
            {
                string sql = @"SELECT * FROM KHOA_XUATHOADON where ROwID =2 ";
                DataTable dt = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];

                if (dt != null)
                {
                    if (int.Parse(dt.Rows[0][id].ToString()) == 1)
                    {
                        Key = 1;

                    }
                    else
                    {
                        Key = 0;
                    }
                }
            }

            return Key;
        }
        public bool Khoahoadon(string nhapmk, string id, string Tendangnhap, string Hoten)
        {
            if (CheckPass(nhapmk, Tendangnhap))
            {
                string sql2 = @"INSERT INTO Log_KhoaHD (username,thaotac,ngaythuchien) VALUES (N'" + Hoten + "','";

                string sql1 = @"UPDATE KHOA_XUATHOADON SET " + id.ToString() + "=";

                string sql = @"SELECT * FROM KHOA_XUATHOADON where ROwID =2 ";
                DataTable dt = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];

                if (int.Parse(dt.Rows[0][id].ToString()) == 0)
                {
                    sql2 += "KHOA-" + id.ToString();
                    sql1 += "1";
                    sql1 += "where RowID = 2";

                }
                else
                {
                    sql2 += "MO-" + id.ToString();
                    sql1 += "0";
                    sql1 += "where RowID = 2";

                }
                if (db.ExecuteNoneQuery(sql1, CommandType.Text, "server37", null) == 1)
                {
                    sql2 += "',GETDATE())";
                    db.ExecuteNoneQuery(sql2, CommandType.Text, "server37", null);
                    return true;
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

        }
        bool CheckPass(string nhapmk, string Tendangnhap)
        {
            string sql = @"select top 1 * from DM_NV where TenDangNhap=N'" + Tendangnhap + "' and matkhau=N'" + db.Encrypt(nhapmk, "tranquocquan", true) + "'";
            DataTable nah = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (nah.Rows.Count == 1)
                return true;
            else
                return false;
        }
        public bool KhoaPVL(string txt_PVL1, string txt_PVL2, string txt_PVL3, string txt_PVL4, string tendangnhap, string hoten, string nhapmk)
        {
            if (CheckPass(nhapmk, tendangnhap))
            {

                string sql2 = @"INSERT INTO Log_KhoaHD (username,thaotac,ngaythuchien) VALUES (N'" + hoten + "','";
                string sql = @"UPDATE KHOA_XUATHOADON SET KHOA_01=N'" + txt_PVL1.Trim() + "', ";
                sql += "KHOA_02=N'" + txt_PVL2.Trim() + "', ";
                sql += "KHOA_03=N'" + txt_PVL3.Trim() + "', ";
                sql += "KHOA_04=N'" + txt_PVL4.Trim() + "' ";
                sql += "where RowID = 2";
                if (db.ExecuteNoneQuery(sql, CommandType.Text, "server37", null) == 1)
                {
                    sql2 += "KHOAPV-KHOA_01=" + txt_PVL1.Trim() + "-KHOA_02=" + txt_PVL2.Trim() + "-KHOA_03=" + txt_PVL3.Trim()
                        + "-KHOA_04=" + txt_PVL4.Trim();
                    sql2 += "',GETDATE())";
                    db.ExecuteNoneQuery(sql2, CommandType.Text, "server37", null);
                    return true;
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

        }
        public bool UpdateKHHD(string txt_KHHD_1, string txt_KHHD_2, string txt_KHHD_3, string txt_KHHD_4, string txt_Serial_L1, string txt_Serial_L2, string txt_Serial_L3, string txt_Serial_L4, string tendangnhap, string hoten, string nhapmk)
        {
            if (CheckPass(nhapmk, tendangnhap))
            {


                string sql2 = @"INSERT INTO Log_KhoaHD (username,thaotac,ngaythuchien) VALUES (N'" + hoten + "','";
                string sql = @"UPDATE KHOA_XUATHOADON SET KHHD_CHUA_L1=N'" + txt_KHHD_1 + "',KHHD_CHUA_L2=N'" + txt_KHHD_2.Trim() + "',KHHD_CHUA_L3=N'" + txt_KHHD_3.Trim() + "',KHHD_CHUA_L4=N'" + txt_KHHD_4.Trim() + "', Serial_L1 = N'" + txt_Serial_L1.Trim() + "', Serial_L2 = N'" + txt_Serial_L2.Trim() + "' , Serial_L3 = N'" + txt_Serial_L3.Trim() + "', Serial_L4 = N'" + txt_Serial_L4.Trim() + "' ";
                sql += "where RowID = 2";
                if (db.ExecuteNoneQuery(sql, CommandType.Text, "server37", null) == 1)
                {
                    sql2 += "KHHD=" + txt_KHHD_1;
                    sql2 += "',GETDATE())";
                    db.ExecuteNoneQuery(sql2, CommandType.Text, "server37", null);
                    return true;
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

        }
        public bool UpdatePhanmem(string khoaphanmem, string noidung, string tendangnhap, string nhapmk)
        {
            if (CheckPass(nhapmk, tendangnhap))
            {
                string sql = @"UPDATE THONGBAO_DL SET NOIDUNG=N'" + noidung + "' WHERE PHANMEM='" + khoaphanmem + "'";
                db.ExecuteNoneQuery(sql, CommandType.Text, "server37", null);
                return true;

            }
            else
            {
                return false;
            }

        }
        public string GetPhanMem(string khoaphanmem)
        {
            string thongbao = "";
            string sql = "SELECT NOIDUNG FROM THONGBAO_DL WHERE PHANMEM='" + khoaphanmem + "'";
            DataTable tbl = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tbl.Rows.Count > 0)
            {

                thongbao = tbl.Rows[0][0].ToString();

            }
            return thongbao;
        }

        public bool UpdateNgayKhoa(string Ngaykhoa, string tendangnhap, string hoten, string nhapmk)
        {
            if (CheckPass(nhapmk, tendangnhap))
            {

                string sql2 = @"INSERT INTO Log_KhoaHD (username,thaotac,ngaythuchien) VALUES (N'" + hoten + "','";
                string sql = @"UPDATE LAYNGUON SET NGAYGUIQUATHANG=N'" + Ngaykhoa.Trim() + "' ";

                if (db.ExecuteNoneQuery(sql, CommandType.Text, "server37", null) == 1)
                {
                    sql2 += "NgayLayNguon=" + Ngaykhoa.Trim();
                    sql2 += "',GETDATE())";
                    db.ExecuteNoneQuery(sql2, CommandType.Text, "server37", null);
                    return true;
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

        }
        public bool UpdateGHN(string txt_GHTN, string txt_ChuaL1, string txt_ChuaL2, string txt_ChuaL3, string txt_ChuaL4, string tendangnhap, string hoten, string nhapmk)
        {
            if (CheckPass(nhapmk, tendangnhap))
            {
                string sql2 = @"INSERT INTO Log_KhoaHD (username,thaotac,ngaythuchien) VALUES (N'" + Session.HoTen + "','";
                string sql = @"UPDATE KHOA_XUATHOADON SET CHUANGAY=N'" + txt_GHTN.Trim() + "', ";
                sql += "CHUA_L1=N'" + txt_ChuaL1.Trim() + "', ";
                sql += "CHUA_L2=N'" + txt_ChuaL2.Trim() + "', ";
                sql += "CHUA_L3=N'" + txt_ChuaL3.Trim() + "', ";
                sql += "CHUA_L4=N'" + txt_ChuaL4.Trim() + "' ";
                sql += "where RowID = 2";
                if (db.ExecuteNoneQuery(sql, CommandType.Text, "server37", null) == 1)
                {
                    sql2 += "ChuaHD-CHUA_L1=" + txt_ChuaL1.Trim() + "-CHUA_L2=" + txt_ChuaL2.Trim() + "-CHUA_L3=" + txt_ChuaL3.Trim()
                        + "-CHUA_L4=" + txt_ChuaL4.Trim() + "-ChuaNgay=" + txt_GHTN.Trim();
                    sql2 += "',GETDATE())";
                    db.ExecuteNoneQuery(sql2, CommandType.Text, "server37", null);
                    return true;
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

        }
    }
}


