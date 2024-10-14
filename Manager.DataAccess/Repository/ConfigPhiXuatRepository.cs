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
    public class ConfigPhiXuatRepository
    {
        private  string SQL_EV_MAIN_V2;
        public ConfigPhiXuatRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN_V2 = configuration.GetConnectionString("SQL_EV_MAIN_V2");
        }
        public List<ConfigPhiXuatModel> ListPhiXuat()
        {
            List<ConfigPhiXuatModel> result = new List<ConfigPhiXuatModel>();
            string sql = @"select * from BAOCAOVESOT_CONFIG_PHIDICHVU";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<ConfigPhiXuatModel>)conn.Query<ConfigPhiXuatModel>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public async Task<bool> UpdatePhiXuat(int ID, string Price, string ExchangeRate, string Amount)
        {
            int result_sql = 0;
            bool result = false;
            string sql = @"update BAOCAOVESOT_CONFIG_PHIDICHVU  set Price = " + Price.Replace(",", "") + " , ExchangeRate = " + ExchangeRate.Replace(",", "") + " , Amount = " + Amount.Replace(",", "") + " where ID = " + ID;
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result_sql = await conn.ExecuteAsync(sql, null, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (result_sql > 0)
            {
                result = true;
            }
            return result;
        }

    }
}
