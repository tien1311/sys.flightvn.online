//using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;
using TangDuLieu;

namespace Manager.DataAccess.Repository
{
    public class ChuongTrinhKhuyenMaiRepository
    {
        private  string SQL_Agent_MAIN;
        DBase db = new DBase();
        public ChuongTrinhKhuyenMaiRepository(IConfiguration configuration)
        {
            SQL_Agent_MAIN = configuration.GetConnectionString("SQL_Agent_MAIN");
        }
        public KhuyenMaiDaiLyModel ChuongTrinhKhuyenMai()
        {
            KhuyenMaiDaiLyModel result = new KhuyenMaiDaiLyModel();
            List<DaiLyKhuyenMai> listDaiLy = new List<DaiLyKhuyenMai>();
            List<ChuongTrinhKhuyenMai> listChuongTrinh = new List<ChuongTrinhKhuyenMai>();

            string sqlChuongTrinh = @"select Title, Status, DateFrom, DateTo, IDChuongTrinh from ChuongTrinhKhuyenMai group by Title, Status, DateFrom, DateTo, IDChuongTrinh order by DateTo desc";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                listChuongTrinh = (List<ChuongTrinhKhuyenMai>)conn.Query<ChuongTrinhKhuyenMai>(sqlChuongTrinh, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            result.ListChuongTrinhKhuyenMai = listChuongTrinh;
            result.ListDaiLyKhuyenMai = listDaiLy;
            return result;
        }
        public bool InsertListDaiLy(List<DaiLyKhuyenMai> ListDaiLyKhuyenMai)
        {
            int result = 0, dem = 0, IDChuongTrinh = 0;
            string SqlInsert = "";
            string sql = @"select top 1 IDChuongTrinh from ChuongTrinhKhuyenMai order by RowID desc";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                try
                {
                    IDChuongTrinh = conn.QueryFirst<int>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                catch (Exception ex)
                {
                }
            }
            if (IDChuongTrinh >= 1)
            {
                IDChuongTrinh++;
            }
            if (IDChuongTrinh == 0)
            {
                IDChuongTrinh++;
            }
            for (int i = 0; i < ListDaiLyKhuyenMai.Count; i++)
            {
                SqlInsert += @"INSERT INTO ChuongTrinhKhuyenMai (MaKH, SoLuong, Title, Description, Content,DateFrom, DateTo, IDChuongTrinh,Status) VALUES ('" + ListDaiLyKhuyenMai[i].MaKH + "', '" + ListDaiLyKhuyenMai[i].SoLuong + "', N'" + ListDaiLyKhuyenMai[i].Title.Trim() + "', N'" + ListDaiLyKhuyenMai[i].Description + "', N'" + ListDaiLyKhuyenMai[i].Content + "','" + DateTime.Parse(ListDaiLyKhuyenMai[i].DateFrom) + "', '" + DateTime.Parse(ListDaiLyKhuyenMai[i].DateTo) + "', '" + IDChuongTrinh + "', 1)";
                dem++;
                if (dem == 10)
                {

                    result = db.ExecuteNoneQuery(SqlInsert, CommandType.Text, "server18", null);
                    SqlInsert = "";
                    dem = 0;
                }
            }
            if (dem > 0)
            {
                result = db.ExecuteNoneQuery(SqlInsert, CommandType.Text, "server18", null);
            }

            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public bool EditStatusCTKM(int IDChuongTrinh, string Status)
        {
            int i = 0;
            string sql = @"update ChuongTrinhKhuyenMai set Status = '" + Status + "' where IDChuongTrinh = N'" + IDChuongTrinh + "'";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
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
        public List<DaiLyKhuyenMai> ListDaiLy(int IDChuongTrinh)
        {
            List<DaiLyKhuyenMai> listDaiLy = new List<DaiLyKhuyenMai>();
            string sqlDaiLy = @"select RowID, MaKH, SoLuong, Title, DateFrom, DateTo from ChuongTrinhKhuyenMai where IDChuongTrinh = N'" + IDChuongTrinh + "'";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                listDaiLy = (List<DaiLyKhuyenMai>)conn.Query<DaiLyKhuyenMai>(sqlDaiLy, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return listDaiLy;
        }
        public bool SaveDaiLy(string ID, string MaKH, string SoLuong)
        {
            int i = 0;
            string sql = @"update ChuongTrinhKhuyenMai set MaKH = '" + MaKH + "', SoLuong = '" + SoLuong + "' where RowID = N'" + ID + "'";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
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
        public List<ChuongTrinhKhuyenMai> DSChuongTrinhKhuyenMai()
        {
            List<ChuongTrinhKhuyenMai> listCTKM = new List<ChuongTrinhKhuyenMai>();
            string sql = @"select * from CTKM order by DateTo desc";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                listCTKM = (List<ChuongTrinhKhuyenMai>)conn.Query<ChuongTrinhKhuyenMai>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return listCTKM;
        }
        public ChuongTrinhKhuyenMai EditCTKM(int ID)
        {
            ChuongTrinhKhuyenMai result = new ChuongTrinhKhuyenMai();
            string sql = @"select * from CTKM where RowID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                result = conn.QueryFirst<ChuongTrinhKhuyenMai>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool SaveCreateCTKM(string Title, string Images, string datefrom, string dateto, string CreateContent)
        {
            int i = 0;
            string sql = "INSERT INTO [CTKM] ([Title] ,[Images],[DateFrom],[DateTo],[Content],[Status]) VALUES ( N'" + Title + "',N'" + Images + "','" + DateTime.Parse(datefrom) + "','" + DateTime.Parse(dateto) + "',N'" + CreateContent + "',1)";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
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
        public bool SaveEditCTKM(int ID, string Title, string Images, string datefrom, string dateto, string CreateContent)
        {
            int i = 0;
            string sql = "UPDATE CTKM SET Title = N'" + Title + "', Images = N'" + Images + "', DateFrom = '" + DateTime.Parse(datefrom) + "',DateTo = '" + DateTime.Parse(dateto) + "', Content = N'" + CreateContent + "' where RowID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
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
        public bool StatusCTKM(int ID, string Status)
        {
            int i = 0;
            string sql = @"update CTKM set Status = '" + Status + "' where RowID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
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
    }
}
