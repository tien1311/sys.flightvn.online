using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class CongNoQuaHanRepository
    {
        private string server_EV_V2;
        public CongNoQuaHanRepository(IConfiguration configuration)
        {
            server_EV_V2 = configuration.GetConnectionString("SQL_EV_MAIN_V2");
        }
        public List<CongNoQuaHanModel> DSCongNoQuaHan()
        {
            List<CongNoQuaHanModel> ListCNQH = new List<CongNoQuaHanModel>();
            return ListCNQH;
        }
        public CongNoQuaHanModel GetCongNoNVQuaHan(string id)
        {
            CongNoQuaHanModel CNQH = new CongNoQuaHanModel();
            string sql_debt = "select ID, TITLE as TieuDe, MONTH as Thang from DEBT_EMPLOYEE where ID = '" + id + "'";
            using (var conn = new SqlConnection(server_EV_V2))
            {
                CNQH = conn.QueryFirst<CongNoQuaHanModel>(sql_debt, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return CNQH;
        }
        public Task<CongNoQuaHanModel> DSCongNoNVQuaHanAsync()
        {
            CongNoQuaHanModel CNQH = new CongNoQuaHanModel();
            try
            {
                List<DSCongNoNVQuaHan> ListCongNoNVQuaHan = new List<DSCongNoNVQuaHan>();
                string sql_debt = "select top 1 ID, TITLE as TieuDe, MONTH as Thang from DEBT_EMPLOYEE order by ID desc";
                using (var conn = new SqlConnection(server_EV_V2))
                {
                    CNQH = conn.QueryFirst<CongNoQuaHanModel>(sql_debt, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                string sql_debt_detail = "select top 4 EMPLCODE as MaNV,EMPLNAME as TenNV,AMOUNT as SoTienNo from DEBT_EMPLOYEE_DETAIL where ID_DEBT = '" + CNQH.ID + "' order by AMOUNT desc";
                using (var conn = new SqlConnection(server_EV_V2))
                {
                    ListCongNoNVQuaHan = (List<DSCongNoNVQuaHan>)conn.Query<DSCongNoNVQuaHan>(sql_debt_detail, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                CNQH.ListCongNoNVQuaHan = ListCongNoNVQuaHan;
                return Task.FromResult(CNQH);
            }
            catch (Exception)
            {
                return Task.FromResult(CNQH);
            }

        }
        public List<CongNoQuaHanModel> ListCongNoQuaHan()
        {
            List<CongNoQuaHanModel> ListCongNoQuaHan = new List<CongNoQuaHanModel>();
            List<DSCongNoNVQuaHan> ListCongNoNVQuaHan = new List<DSCongNoNVQuaHan>();
            string sql_debt = "select ID, TITLE as TieuDe, MONTH as Thang, CREATENAME as UpdateBy from DEBT_EMPLOYEE order by ID desc";
            using (var conn = new SqlConnection(server_EV_V2))
            {
                ListCongNoQuaHan = (List<CongNoQuaHanModel>)conn.Query<CongNoQuaHanModel>(sql_debt, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            for (int i = 0; i < ListCongNoQuaHan.Count; i++)
            {
                string sql_debt_detail = "select EMPLCODE as MaNV,EMPLNAME as TenNV,AMOUNT as SoTienNo,OUTPUTDATE as ThoiGianXuatVe,DEBT as DuNo,STATUS as TinhTrang, NOTE as GhiChu from DEBT_EMPLOYEE_DETAIL where ID_DEBT = '" + ListCongNoQuaHan[i].ID + "' order by AMOUNT desc";
                using (var conn = new SqlConnection(server_EV_V2))
                {
                    ListCongNoNVQuaHan = (List<DSCongNoNVQuaHan>)conn.Query<DSCongNoNVQuaHan>(sql_debt_detail, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                ListCongNoQuaHan[i].ListCongNoNVQuaHan = ListCongNoNVQuaHan;
            }
            return ListCongNoQuaHan;
        }
        public List<CongNoQuaHanModel> ListCongNoQuaHanDetail(string id)
        {
            List<CongNoQuaHanModel> ListCongNoQuaHan = new List<CongNoQuaHanModel>();
            List<DSCongNoNVQuaHan> ListCongNoNVQuaHan = new List<DSCongNoNVQuaHan>();
            string sql_debt = "select ID, TITLE as TieuDe, MONTH as Thang from DEBT_EMPLOYEE where ID = '" + id + "'";
            using (var conn = new SqlConnection(server_EV_V2))
            {
                ListCongNoQuaHan = (List<CongNoQuaHanModel>)conn.Query<CongNoQuaHanModel>(sql_debt, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            for (int i = 0; i < ListCongNoQuaHan.Count; i++)
            {
                string sql_debt_detail = "select ID,EMPLCODE as MaNV,EMPLNAME as TenNV,AMOUNT as SoTienNo,OUTPUTDATE as ThoiGianXuatVe,DEBT as DuNo,STATUS as TinhTrang, NOTE as GhiChu from DEBT_EMPLOYEE_DETAIL where ID_DEBT = '" + ListCongNoQuaHan[i].ID + "' order by AMOUNT desc";
                using (var conn = new SqlConnection(server_EV_V2))
                {
                    ListCongNoNVQuaHan = (List<DSCongNoNVQuaHan>)conn.Query<DSCongNoNVQuaHan>(sql_debt_detail, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                ListCongNoQuaHan[i].ListCongNoNVQuaHan = ListCongNoNVQuaHan;
            }
            return ListCongNoQuaHan;
        }
        public string SaveCongNoQuaHan(string MANV, List<DSCongNoNVQuaHan> ListCongNoNVQuaHan, string TieuDe, string Thang, string server_EV_V2)
        {
            string result = "", sql = "";
            int result_sql = 0;
            if (MANV == null || MANV == "")
            {
                result = "Hết thời gian đăng nhập, xin vui lòng thoát ra đăng nhập lại";
                return result;
            }
            int ID_Debt = 0;
            string sql_debt = "Insert into DEBT_EMPLOYEE values(N'" + TieuDe + "', '" + Thang + "', GETDATE(),'" + MANV + "') SELECT SCOPE_IDENTITY();";
            using (var conn = new SqlConnection(server_EV_V2))
            {
                ID_Debt = conn.QueryFirst<int>(sql_debt, null, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (ID_Debt > 0)
            {
                for (int i = 0; i < ListCongNoNVQuaHan.Count; i++)
                {
                    sql += @"Insert into DEBT_EMPLOYEE_DETAIL(ID_DEBT, EMPLCODE,EMPLNAME,AMOUNT,OUTPUTDATE,DEBT,NOTE,STATUS,CREATEDATE,CREATENAME) 
                                                    Values (" + ID_Debt + ",N'" + ListCongNoNVQuaHan[i].MaNV + "',N'" + ListCongNoNVQuaHan[i].TenNV + "','" + ListCongNoNVQuaHan[i].SoTienNo + "',N'" + ListCongNoNVQuaHan[i].ThoiGianXuatVe + "','" + ListCongNoNVQuaHan[i].DuNo + "',N'" + ListCongNoNVQuaHan[i].GhiChu + "','" + ListCongNoNVQuaHan[i].TinhTrang + "', GETDATE(),'" + MANV + "')";
                }

                using (var conn = new SqlConnection(server_EV_V2))
                {
                    result_sql = conn.Execute(sql, null, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                if (result_sql > 0)
                {
                    result = "Lưu thành công";
                }
                else
                {
                    result = "Lưu không thành công";
                }
            }
            else
            {
                result = "Lưu không thành công";
            }

            return result;
        }
        public string UpdateCongNoQuaHan(string MANV, string ID, string TieuDe, string Thang)
        {
            string result = "";
            int result_sql = 0;

            string sql_debt = "UPDATE DEBT_EMPLOYEE set TITLE = N'" + TieuDe + "', MONTH = '" + Thang + "', CREATEDATE = GETDATE(), CREATENAME = '" + MANV + "' where ID = '" + ID + "'";
            using (var conn = new SqlConnection(server_EV_V2))
            {
                result_sql = conn.Execute(sql_debt, null, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (result_sql > 0)
            {
                result = "Lưu thành công";
            }
            else
            {
                result = "Lưu không thành công";
            }
            return result;
        }
        public string SaveCongNoNVQuaHan(string MANV, DSCongNoNVQuaHan CongNoNVQuaHan)
        {
            string result = "", sql = "";
            int result_sql = 0;
            if (MANV == null || MANV == "")
            {
                result = "Hết thời gian đăng nhập, xin vui lòng thoát ra đăng nhập lại";
                return result;
            }
            sql = @"Insert into DEBT_EMPLOYEE_DETAIL(ID_DEBT, EMPLCODE,EMPLNAME,AMOUNT,OUTPUTDATE,DEBT,NOTE,STATUS,CREATEDATE,CREATENAME) 
                                Values (" + CongNoNVQuaHan.ID_DEBT + ",N'" + CongNoNVQuaHan.MaNV + "',N'" + CongNoNVQuaHan.TenNV + "','" + CongNoNVQuaHan.SoTienNo + "',N'" + CongNoNVQuaHan.ThoiGianXuatVe + "','" + CongNoNVQuaHan.DuNo + "',N'" + CongNoNVQuaHan.GhiChu + "','" + CongNoNVQuaHan.TinhTrang + "', GETDATE(),'" + MANV + "')";

            using (var conn = new SqlConnection(server_EV_V2))
            {
                result_sql = conn.Execute(sql, null, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (result_sql > 0)
            {
                result = "Lưu thành công";
            }
            else
            {
                result = "Lưu không thành công";
            }
            return result;
        }
        public string DelCongNoNVQuaHan(string ID)
        {
            string result = "", sql = "";
            int result_sql = 0;
            sql = @"DELETE FROM DEBT_EMPLOYEE_DETAIL WHERE ID = '" + ID + "'";

            using (var conn = new SqlConnection(server_EV_V2))
            {
                result_sql = conn.Execute(sql, null, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (result_sql > 0)
            {
                result = "Lưu thành công";
            }
            else
            {
                result = "Lưu không thành công";
            }
            return result;
        }
    }
}
