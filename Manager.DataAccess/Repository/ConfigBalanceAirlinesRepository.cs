using Dapper;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace Manager.DataAccess.Repository
{
    public class ConfigBalanceAirlinesRepository
    {
        private readonly IConfiguration _configuration;
        private string Server = "";
        public ConfigBalanceAirlinesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            Server = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
        }
        //Get All
        public async Task<List<ConfigAirlineBalanceModel>> ConfigAirlineBalance()
        {
            List<ConfigAirlineBalanceModel> result = new List<ConfigAirlineBalanceModel>();
            try
            {
                string sql = "select * from CONFIG_BALANCE_AIRLINE";
                using (var conn = new SqlConnection(Server))
                {
                    result = (List<ConfigAirlineBalanceModel>)await conn.QueryAsync<ConfigAirlineBalanceModel>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        //Get ID
        public async Task<ConfigAirlineBalanceModel> ConfigAirlineBalance_ID(int ID)
        {
            ConfigAirlineBalanceModel result = new ConfigAirlineBalanceModel();
            try
            {
                string sql = "select * from CONFIG_BALANCE_AIRLINE where ID = @ID";
                using (var conn = new SqlConnection(Server))
                {
                    var param = new
                    {
                        ID
                    };
                    result = await conn.QueryFirstAsync<ConfigAirlineBalanceModel>(sql, param, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        //Update ID
        public async Task<bool> Update_ConfigAirlineBalance_ID(int ID, decimal Amount)
        {
            bool result = false;
            int result_sql = 0;
            try
            {
                string sql = "update CONFIG_BALANCE_AIRLINE set Amount = @Amount  where ID = @ID";
                using (var conn = new SqlConnection(Server))
                {
                    var param = new
                    {
                        ID,
                        Amount
                    };
                    result_sql = await conn.ExecuteAsync(sql, param, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
                if (result_sql > 0)
                {
                    result = true;
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
