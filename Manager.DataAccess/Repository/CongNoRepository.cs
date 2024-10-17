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
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class CongNoRepository
    {
        DBase db = new DBase();
        private string SQL_EV_MAIN;
        private string SQL_KH_KT;
        public CongNoRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN = configuration.GetConnectionString("SQL_EV_MAIN");
            SQL_KH_KT = configuration.GetConnectionString("SQL_KH_KT");
        }
        public CongNoModel SearchCongNo(string makh, string dafr, string dato)
        {

            CongNoModel CongNo = new CongNoModel();
            List<ChiTietCongNoModel> ListChiTietCongNo = new List<ChiTietCongNoModel>();

            //string server_EV = "Data Source=27.71.232.40,1453;Initial Catalog=Manager;User ID=sa;Password=EnViet@123";
            string sql = $@"EXEC SP_CHITIET_CONGNO @MaKH='{makh}',@DateFrom='{dafr}',@DateTo='{dato}'";
            using (var conn = new SqlConnection(SQL_EV_MAIN))
            {

                ListChiTietCongNo = (List<ChiTietCongNoModel>)conn.Query<ChiTietCongNoModel>(sql, null, null, true, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            string sqltenDL = @"select TENCONGTY from KHACHHANG_HOPDONG where MAKETOAN = '" + makh + "'";
            DataTable dttenDL = db.ExecuteDataSet(sqltenDL, CommandType.Text, "server37", null).Tables[0];
            ///Lấy chi tiết trước
            CongNo.MaKH = makh;
            if (dttenDL != null && dttenDL.Rows.Count > 0)
            {
                CongNo.TenDL = dttenDL.Rows[0]["TENCONGTY"].ToString();
            }
            else
            {
                string sqltenNV = @"select ten from DM_NV where Yahoo = '" + makh + "'";
                DataTable dttenNV = db.ExecuteDataSet(sqltenNV, CommandType.Text, "server37", null).Tables[0];
                if (dttenNV != null && dttenNV.Rows.Count > 0)
                {
                    CongNo.TenDL = dttenNV.Rows[0]["ten"].ToString();
                }
            }
            CongNo.ChiTiet = ListChiTietCongNo;
            CongNo.SoDuDauNgay = decimal.ToDouble(SoDuDauNgay(makh, dafr));
            double TongNo = 0;
            double TongCo = 0;
            foreach (ChiTietCongNoModel item in CongNo.ChiTiet)
            {
                TongNo += item.No;
                TongCo += item.Co;
                item.LuyKe = CongNo.SoDuDauNgay + TongNo - TongCo;
            }
            CongNo.SoDuCuoiNgay = CongNo.SoDuDauNgay + TongNo - TongCo;
            return CongNo;
        }
        private decimal SoDuDauNgay(string maKH, string dafr)
        {
            decimal result = 0;
            try
            {
                List<ChiTietCongNoModel> ListChiTietCongNo = new List<ChiTietCongNoModel>();
                //string server_KH_KT = "Data Source=27.71.232.40,1453;Initial Catalog=ManagerAccountant;User ID=ELV_TEMP;Password=tkt@123$456;";
                string sql = $@"EXEC SP_CHITIET_CONGNO @MaKH='{maKH}',@DateFrom='2020-01-01',@DateTo='{dafr}'";
                using (var conn = new SqlConnection(SQL_EV_MAIN))
                {

                    ListChiTietCongNo = (List<ChiTietCongNoModel>)conn.Query<ChiTietCongNoModel>(sql, null, null, true, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }

                double TongNo = 0;
                double TongCo = 0;
                foreach (ChiTietCongNoModel item in ListChiTietCongNo)
                {
                    TongNo += item.No;
                    TongCo += item.Co;
                }
                decimal.TryParse((TongNo - TongCo).ToString(), out result);

                //string res = "0";
                //string ngay = DateTime.ParseExact(dafr, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(-1).ToString("MM/dd/yyyy");
                //string sql = $@"SELECT isnull(Sum(SoDu),0) as sodu FROM [fTinhSodu]('{ngay}','{maKH}') group by id_khachhang";
                ////DataTable tbl = db.ExecuteDataSet(sql, CommandType.Text, "serverKT", null).Tables[0];
                //using (var conn = new SqlConnection(SQL_KH_KT))
                //{
                //    result = conn.QueryFirst<decimal>(sql, null, null, commandType: CommandType.Text, commandTimeout: 30);
                //    conn.Dispose();
                //}
                return result;
            }
            catch (Exception)
            {
                return result;
                throw;
            }

        }
        public decimal SoDuCuoiNgay(string maKH)
        {
            decimal result = 0;
            string sql = "";
            sql = $@"SELECT top 1 isnull(SoDu,0) as sodu  FROM [ManagerAccountant].[dbo].[_DUCUOI_NEW] WITH (NOLOCK) where  ID_Khachhang = '" + maKH + "'";
            DataTable tbl = db.ExecuteDataSet(sql, CommandType.Text, "serverKT", null).Tables[0];
            if (tbl == null)
            {
                return 0;
            }
            if (tbl.Rows.Count < 1)
            {
                return 0;
            }
            if (tbl.Rows.Count > 0)
            {
                string res = tbl.Rows[0][0].ToString();
                result = decimal.Parse(res);
            }
            return result;
        }
        public async Task<decimal> SoDuCuoiNgayDapper(string maKH)
        {
            decimal result = 0;
            string sql = "";
            try
            {
                //string server_KT = "Data Source=27.71.232.40,1453; Initial Catalog = ManagerAccountant; User ID = ELV_TEMP; Password = tkt@123$456;";
                sql = $@"SELECT top 1 isnull(SoDu,0) as sodu  FROM [ManagerAccountant].[dbo].[_DUCUOI_NEW] WITH (NOLOCK) where  ID_Khachhang = '" + maKH + "'";
                using (var conn = new SqlConnection(SQL_KH_KT))
                {
                    result = await conn.QueryFirstAsync<decimal>(sql, null, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

    }
}
