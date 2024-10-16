using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class LotusmileRepository
    {
        string SQL_EV_MAIN_V2; /*= "Data Source=.;Initial Catalog=Manager_V2;User ID=sa;Password=EnViet@123;";*/
        string SQL_AIRLINE24h_MAIN; /* = "Data Source=.;Initial Catalog=Airline24h;User ID=sa;Password=Ngominhtien@13;";*/

        public LotusmileRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN_V2 = configuration.GetConnectionString("SQL_EV_MAIN_V2");
            SQL_AIRLINE24h_MAIN = configuration.GetConnectionString("SQL_AIRLINE24h_MAIN");
        }
        public string Content_Right()
        {
            string Content_Right = "";
            string sql = @"select NOIDUNG FROM BAIVIET where ROWID = '189'";
            using (var conn = new SqlConnection(SQL_AIRLINE24h_MAIN))
            {
                Content_Right = conn.QueryFirst<string>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return Content_Right;
        }
        public List<LotusmileModel> Lotusmile()
        {
            List<LotusmileModel> result = new List<LotusmileModel>();
            string sql = @"select * from REGISTER_AIRLINE order by CreateDate desc";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<LotusmileModel>)conn.Query<LotusmileModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool DeleteLotusmile(int ID)
        {
            int i = 0;
            string sql = @"DELETE FROM REGISTER_AIRLINE WHERE ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
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
