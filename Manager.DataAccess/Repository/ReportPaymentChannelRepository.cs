using Dapper;
using EasyInvoice.Client;
using Manager.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;


namespace Manager.DataAccess.Repository
{
    public class ReportPaymentChannelRepository
    {

        public async Task<List<ReportPaymentChannelModel>> ReportPaymentChannel(string FromDate, string ToDate, string Channel, string Server)
        {
            List<ReportPaymentChannelModel> response = new List<ReportPaymentChannelModel>();
            try
            {
                string wheres = "";
                if (Channel != "All")
                {
                    wheres = " and Channel = '" + Channel + "'";
                }
                string sql = @"select * 
                            from REPORT_PAYMENT_CHANNEL 
                            where CreatedDate >= '" + FromDate + "' and CreatedDate <= '" + ToDate + " 23:59:59'" + wheres;
                using (var conn = new SqlConnection(Server))
                {
                    response = (List<ReportPaymentChannelModel>)await conn.QueryAsync<ReportPaymentChannelModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                    conn.Close();
                }

                return response;
            }
            catch (Exception)
            {
                return response;
            }

        }
        public async Task<List<string>> Channels(string Server)
        {
            List<string> response = new List<string>();
            string sql = @"select UserName from ApiCompanyName where isnull(IsHidden,0) = 0";
            using (var conn = new SqlConnection(Server))
            {
                response = (List<string>)await conn.QueryAsync<string>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
                conn.Close();
            }
            return response;
        }
    }

}
