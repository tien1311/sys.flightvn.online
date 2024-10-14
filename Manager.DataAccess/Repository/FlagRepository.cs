using Dapper;
using Manager.Model.Models;
//using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Manager.DataAccess.Repository
{
    public class FlagRepository
    {
        public IConfiguration _configuration;
        public string _connectionString;
        public FlagRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SQL_EV_TOURHOT");
        }

        public List<Flag> GetListFlag()
        {
            List<Flag> listFlag = new List<Flag>();
            string sql = @"SELECT * FROM Flag Order By Priority";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                listFlag = conn.Query<Flag>(sql).ToList();
                conn.Close();
            }
            return listFlag;
        }

        public Flag GetFlagById(int id)
        {
            Flag flag = new Flag();
            string sql = @"SELECT * FROM Flag WHERE ID = @ID ";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                flag = conn.QueryFirstOrDefault(sql, new { ID = id });
                conn.Close();
            }
            return flag;
        }
    }
}
