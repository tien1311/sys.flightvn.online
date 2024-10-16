using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using EasyInvoice.Json;
using System.Data.SqlClient;
using Dapper;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class QRRepository
    {
        string SQL_EV_MAIN_V2; /*= "Data Source=27.71.232.40,1453;Initial Catalog=Manager_V2;User ID=sa;Password=EnViet@123;";*/
        public QRRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN_V2 = configuration.GetConnectionString("SQL_EV_MAIN_V2");
        }
        public List<SuggestAddInfo> ListAddInfo()
        {
            List<SuggestAddInfo> result = new List<SuggestAddInfo>();
            string sql = @"select * from DESCRIPTION_TRANSFER";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<SuggestAddInfo>)conn.Query<SuggestAddInfo>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool SaveCreateDescription(string Description)
        {
            int i = 0;
            string sql = @"INSERT INTO [DESCRIPTION_TRANSFER] ([Description],[IsActive])VALUES (N'" + Description + "','1')";
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
        public SuggestAddInfo EditDescription(int ID)
        {
            SuggestAddInfo result = new SuggestAddInfo();
            string sql = @"select * from DESCRIPTION_TRANSFER where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = conn.QueryFirst<SuggestAddInfo>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool SaveEditDescription(int ID, string Description)
        {
            int i = 0;
            string sql = @"UPDATE DESCRIPTION_TRANSFER SET [Description] = N'" + Description + "' where ID = '" + ID + "'";
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
        public bool DeleteDescription(int ID)
        {
            int i = 0;
            string sql = @"UPDATE DESCRIPTION_TRANSFER SET [IsActive] = '0' where ID = '" + ID + "'";
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
        public bool ActiveDescription(int ID)
        {
            int i = 0;
            string sql = @"UPDATE DESCRIPTION_TRANSFER SET [IsActive] = '1' where ID = '" + ID + "'";
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
    }
}
