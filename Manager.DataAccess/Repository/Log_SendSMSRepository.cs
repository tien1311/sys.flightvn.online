using Dapper;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;

namespace Manager.DataAccess.Repository
{
    public class Log_SendSMSRepository
    {
        string SQL_EV_MAIN; /*= "Data Source=.;Initial Catalog=Manager;User ID=sa;Password=EnViet@123;";*/
        public Log_SendSMSRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN = configuration.GetConnectionString("SQL_EV_MAIN");
        }
        public List<LOG_SendSMSBaoCaoVe> Log_SendSMS()
        {
            List<LOG_SendSMSBaoCaoVe> listLogSMS = new List<LOG_SendSMSBaoCaoVe>();
            string sql = @"select * from LOG_BAOCAOVE order by NGAYGUI desc";
            using (var conn = new SqlConnection(SQL_EV_MAIN))
            {
                listLogSMS = (List<LOG_SendSMSBaoCaoVe>)conn.Query<LOG_SendSMSBaoCaoVe>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return listLogSMS;
        }
    }
}
