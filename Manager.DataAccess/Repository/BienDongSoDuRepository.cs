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
    public class BienDongSoDuRepository
    {


        DBase db = new DBase();

        public List<BienDongSoDuModel> DanhSachBienDongSoDu(string TuNgay, string DenNgay, string TinhTrang, string NganHang, string SoTien)
        {
            List<BienDongSoDuModel> list = new List<BienDongSoDuModel>();
            string sWhere = "";
            if (TinhTrang != "" && TinhTrang != "All")
            {
                sWhere += " and NOCO = N'" + TinhTrang + "'";
            }
            if (NganHang != "" && NganHang != "All")
            {
                sWhere += " and UPPER(NGANHANG) like N'" + NganHang.ToUpper() + "%'";
            }
            if (SoTien != "0" && SoTien != "" && SoTien != null)
            {
                sWhere += " and SoTien = " + SoTien;
            }
            string sql = @"select (Case when MACK like N'CK%' then 'SMS' else NganHang end) as Type,STT=Row_Number() over (order by NGAYNHAN desc), *, DULIEUEFF = 'NV00006'  from BIENDONGSODU where NGAYCK > '" + TuNgay + "' and NGAYCK <= '" + DenNgay + " 23:59:59'" + sWhere;
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    BienDongSoDuModel info = new BienDongSoDuModel();
                    info.STT = int.Parse(tb.Rows[i]["STT"].ToString());
                    info.Type = tb.Rows[i]["Type"].ToString();
                    info.MACK = tb.Rows[i]["MACK"].ToString();
                    info.NGANHANG = tb.Rows[i]["NGANHANG"].ToString();
                    info.MAKH = tb.Rows[i]["MAKH"].ToString();
                    info.SOTIEN = string.Format("{0:0,0}", decimal.Parse(tb.Rows[i]["SOTIEN"].ToString()));
                    info.NOCO = tb.Rows[i]["NOCO"].ToString();
                    info.SOBUTTOAN = tb.Rows[i]["SOBUTTOAN"].ToString();
                    info.NOIDUNG = tb.Rows[i]["NOIDUNG"].ToString();
                    if (tb.Rows[i]["NGAYCK"].ToString() != "")
                    {
                        info.NGAYCK = DateTime.Parse(tb.Rows[i]["NGAYCK"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    if (tb.Rows[i]["NGAYNHAN"].ToString() != "")
                    {
                        info.NGAYNHAN = DateTime.Parse(tb.Rows[i]["NGAYNHAN"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    if (tb.Rows[i]["NGAYGUI"].ToString() != "")
                    {
                        info.NGAYGUI = DateTime.Parse(tb.Rows[i]["NGAYGUI"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    if (tb.Rows[i]["NGAYSUA"].ToString() != "")
                    {
                        info.NGAYSUA = DateTime.Parse(tb.Rows[i]["NGAYSUA"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    info.NGUOISUA = tb.Rows[i]["NGUOISUA"].ToString();
                    info.DULIEUEFF = tb.Rows[i]["DULIEUEFF"].ToString();
                    list.Add(info);
                }

            }
            return list;
        }

        public List<NganHang> ListNganHang()
        {
            List<NganHang> list = new List<NganHang>();
            string sql = "select * from NGANHANG";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        NganHang info = new NganHang();
                        info.MANH = tb.Rows[i][1].ToString();
                        info.TENNH = tb.Rows[i][1].ToString();
                        list.Add(info);
                    }

                }
            }
            return list;
        }


        public List<BienDongSoDuModel> TraCuuChuyenKhoanDaiLy(string MAKH, string NGANHANG, string SOTIEN, string SEARCH)
        {
            DataTable tb = null;
            List<BienDongSoDuModel> list = new List<BienDongSoDuModel>();
            string sWhere = " and isnull(IsHidden,0) = 0 ";


            string sql = "";
            if (SEARCH == "Search")
            {
                if (MAKH != null)
                {
                    //MAKH = "";
                    sWhere += " and (MAKH = N'" + MAKH.ToUpper() + "' and NGUOISUA is not null)";
                }
                sql = @"select STT=Row_Number() over (order by NGAYNHAN desc), * from BIENDONGSODU  where  NOCO = N'Có' and NGAYCK > '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' and NGAYCK <= '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59'" + sWhere;
                tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            }
            else
            {
                if (NGANHANG != "All")
                {
                    sWhere += " and UPPER(NGANHANG) like N'" + NGANHANG.ToUpper() + "%'";
                }
                if (SOTIEN == "" || SOTIEN == null)
                {
                    SOTIEN = "0";
                }
                sWhere += " and isnull(IsHidden,0) = 0 and SOTIEN = " + SOTIEN.Replace(",", "").Replace(".", "");
                sql = @"select STT=Row_Number() over (order by NGAYNHAN desc), * from BIENDONGSODU where NOCO = N'Có' and NGAYCK > '" + DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") + "' and NGAYCK <= '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59'" + sWhere;
                tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            }


            if (tb != null && tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    BienDongSoDuModel info = new BienDongSoDuModel();
                    info.ID = int.Parse(tb.Rows[i]["ID"].ToString());
                    info.STT = int.Parse(tb.Rows[i]["STT"].ToString());
                    info.MACK = tb.Rows[i]["MACK"].ToString();
                    info.NGANHANG = tb.Rows[i]["NGANHANG"].ToString();
                    info.MAKH = tb.Rows[i]["MAKH"].ToString();
                    info.SOTIEN = string.Format("{0:0,0}", decimal.Parse(tb.Rows[i]["SOTIEN"].ToString()));
                    info.NOCO = tb.Rows[i]["NOCO"].ToString();
                    info.SOBUTTOAN = tb.Rows[i]["SOBUTTOAN"].ToString();
                    info.NOIDUNG = tb.Rows[i]["NOIDUNG"].ToString();
                    if (tb.Rows[i]["NGAYCK"].ToString() != "")
                    {
                        info.NGAYCK = DateTime.Parse(tb.Rows[i]["NGAYCK"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    if (tb.Rows[i]["NGAYNHAN"].ToString() != "")
                    {
                        info.NGAYNHAN = DateTime.Parse(tb.Rows[i]["NGAYNHAN"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    if (tb.Rows[i]["NGAYGUI"].ToString() != "")
                    {
                        info.NGAYGUI = DateTime.Parse(tb.Rows[i]["NGAYGUI"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    if (tb.Rows[i]["NGAYSUA"].ToString() != "")
                    {
                        info.NGAYSUA = DateTime.Parse(tb.Rows[i]["NGAYSUA"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    info.NGUOISUA = tb.Rows[i]["NGUOISUA"].ToString();

                    list.Add(info);
                }

            }
            return list;
        }

        public bool KiemTraMaKH(string MaCK, string MaKH)
        {

            string sql1 = @"select top 1 MAKH from BIENDONGSODU where MACK = '" + MaCK + "' order by ID DESC";
            DataTable tb = db.ExecuteDataSet(sql1, CommandType.Text, "server37", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    if (tb.Rows[0]["MAKH"].ToString() != "")
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        public bool KiemTraCKDataEFF(string MaCK)
        {

            string sql1 = @"select top 1 * from [KETOAN].[ELV_KETOAN].[dbo].[LIFE_OBJ3528] where field47 = '" + MaCK + "'";
            DataTable tb = db.ExecuteDataSet(sql1, CommandType.Text, "server37", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    return false;

                }
            }
            return true;
        }

        public async Task<bool> CapNhatMaKH(string MaCK, string MaKH, string Server, string NguoiSua, string Server_KT_NH, string MaNguoiSua)
        {
            var result = false;
            string server = Server;
            string sql = "SP_UPDATE_BIENDONGSODU";
            int result_sql = 0;
            using (var conn = new SqlConnection(server))
            {
                var param = new
                {
                    MACK = MaCK,
                    MAKH = MaKH.ToUpper(),
                    NGUOISUA = NguoiSua,
                    MANGUOISUA = MaNguoiSua
                };
                result_sql = await conn.ExecuteAsync(sql, param, null, commandTimeout: 30, commandType: CommandType.StoredProcedure);

            }
            if (result_sql == 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            string sql_EFF = "UpdateThongTinNH";
            using (var conn = new SqlConnection(Server_KT_NH))
            {
                await conn.ExecuteAsync(sql_EFF, null, null, commandTimeout: 30, commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        public async Task<bool> CapNhatDataEFF(string MaCK, string Server, string NguoiSua, string Server_KT_NH)
        {
            string server = Server;
            string sql = "SP_UPDATE_BIENDONGSODU_EFF";
            int result_sql = 0;
            var result = false;



            using (var conn = new SqlConnection(server))
            {
                var param = new
                {
                    MACK = MaCK,
                    NGUOISUA = NguoiSua

                };
                result_sql = await conn.ExecuteAsync(sql, param, null, commandTimeout: 30, commandType: CommandType.StoredProcedure);

            }
            if (result_sql == 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }

            string sql_EFF = "UpdateThongTinNH";
            using (var conn = new SqlConnection(Server_KT_NH))
            {
                await conn.ExecuteAsync(sql_EFF, null, null, commandTimeout: 30, commandType: CommandType.StoredProcedure);
            }

            return result;
        }
    }
}
