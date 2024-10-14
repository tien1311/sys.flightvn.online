using Dapper;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.DataAccess.Repository
{
    public class ThongBaoDaiLyRepository
    {
        private string SQL_EV_MAIN_V2; /* = "Data Source=.;Initial Catalog=Manager_V2;User ID=sa;Password=EnViet@123;";*/
        private string SQL_EV_MAIN; /*= "Data Source=.;Initial Catalog=Manager;User ID=sa;Password=EnViet@123;";*/
        public ThongBaoDaiLyRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN_V2 = configuration.GetConnectionString("SQL_EV_MAIN_V2");
            SQL_EV_MAIN = configuration.GetConnectionString("SQL_EV_MAIN");
        }
        public List<TinhTrangKhoa> TinhTrang()
        {
            List<TinhTrangKhoa> result = new List<TinhTrangKhoa>();
            string sql = @"select * from AGENT_NOTIFICATION_STATUS";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<TinhTrangKhoa>)conn.Query<TinhTrangKhoa>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool SaveCreateTinhTrang(string Name, string PB)
        {
            int i = 0;
            string sql = @"INSERT INTO [AGENT_NOTIFICATION_STATUS] ([Name],[ID_Dept],[IsActive])VALUES (N'" + Name + "',N'" + PB + "','1')";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: System.Data.CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public TinhTrangKhoa EditTinhTrang(int ID)
        {
            TinhTrangKhoa result = new TinhTrangKhoa();
            string sql = @"select * from AGENT_NOTIFICATION_STATUS where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = conn.QueryFirst<TinhTrangKhoa>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool SaveEditTinhTrang(int ID, string Name, string PB)
        {
            int i = 0;
            string sql = @"UPDATE AGENT_NOTIFICATION_STATUS SET [Name] = N'" + Name + "',[ID_Dept] = N'" + PB + "' where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: System.Data.CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool DeleteTinhTrang(int ID)
        {
            int i = 0;
            string sql = @"UPDATE AGENT_NOTIFICATION_STATUS SET [IsActive] = '0' where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: System.Data.CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool ActiveTinhTrang(int ID)
        {
            int i = 0;
            string sql = @"UPDATE AGENT_NOTIFICATION_STATUS SET [IsActive] = '1' where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: System.Data.CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public List<NoiDungKhoa> NoiDung()
        {
            List<NoiDungKhoa> result = new List<NoiDungKhoa>();
            string sql = @"select QL.*, TT.Name as TinhTrang from QLTHONGBAO QL
                            left join [SERVER37].[Manager_V2].[dbo].[AGENT_NOTIFICATION_STATUS] TT on QL.IDTinhTrang = TT.ID
                            where PHANMEM = 'Sys.KhoaCode' order by QL.ROWID desc";
            using (var conn = new SqlConnection(SQL_EV_MAIN))
            {
                result = (List<NoiDungKhoa>)conn.Query<NoiDungKhoa>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool SaveCreateNoiDung(string TieuDe, string TT, string NoiDung, string NoiDungTimKiem)
        {
            int i = 0;
            string sql = @"INSERT INTO [QLTHONGBAO] ([TieuDe],[IDTinhTrang],[NoiDung],[NoiDungTimKiem],[PhanMem])VALUES (N'" + TieuDe + "','" + TT + "',N'" + NoiDung + "',N'" + NoiDungTimKiem + "','Sys.KhoaCode')";
            using (var conn = new SqlConnection(SQL_EV_MAIN))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: System.Data.CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public NoiDungKhoa EditNoiDung(int ID)
        {
            NoiDungKhoa result = new NoiDungKhoa();
            string sql = @"select * from QLTHONGBAO where ROWID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN))
            {
                result = conn.QueryFirst<NoiDungKhoa>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool SaveEditNoiDung(int ROWID, string TieuDe, string TT, string NoiDung, string NoiDungTimKiem)
        {
            int i = 0;
            string sql = @"UPDATE QLTHONGBAO SET [TieuDe] = N'" + TieuDe + "',[NoiDung] = N'" + NoiDung + "',[NoiDungTimKiem] = N'" + NoiDungTimKiem + "',[IDTinhTrang] = '" + TT + "' where ROWID = '" + ROWID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: System.Data.CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }

        public List<PhongBanModel> ListPhongBan()
        {
            List<PhongBanModel> result = new List<PhongBanModel>();
            string sql = @"select ID as MaPB, TEN as PB from PHONGBAN where ISACTIVE = 1";
            using (var conn = new SqlConnection(SQL_EV_MAIN))
            {
                result = (List<PhongBanModel>)conn.Query<PhongBanModel>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }

        public List<PhongBanModel> ListPhongBanV2()
        {
            List<PhongBanModel> result = new List<PhongBanModel>();
            string sql = @"select ID as MaPB, TEN as PB from PHONGBAN ";
            using (var conn = new SqlConnection(SQL_EV_MAIN))
            {
                result = (List<PhongBanModel>)conn.Query<PhongBanModel>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
    }
}
