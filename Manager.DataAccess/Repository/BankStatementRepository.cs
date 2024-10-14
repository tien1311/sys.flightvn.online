using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
//using Manager_EV.Service.Abstraction;
using Microsoft.Extensions.Configuration;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Manager.Model.Models;

namespace Manager.DataAccess.Repository
{
    public class BankStatementRepository
    {
        private readonly IConfiguration _configuration;
        public BankStatementRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<BankStatement_Request_Params_Model>> Search(string FromDate, string ToDate)
        {
            string Server = _configuration.GetConnectionString("SQL_EV_SERVICE");
            List<BankStatement_Request_Params_Model> result = new List<BankStatement_Request_Params_Model>();
            string sql = @"select B.CreatedDate,P.TransactionDate, P.EffectiveDate, P.Amount, P.DebitOrCredit, P.TransactionContent, DATEADD(HOUR, 7, P.TransactionDate) as TransactionDateVN from BankStatement_Request_Params P 
                            left join BankStatement_Request R on R.ID = P.ID_BankStatement_Request
                            left join BankStatement B on B.ID = R.ID_BankStatement
                            where B.CreatedDate >= '" + FromDate + "' and B.CreatedDate <= '" + ToDate + "' order by B.CreatedDate desc";
            using (var conn = new SqlConnection(Server))
            {
                result = (List<BankStatement_Request_Params_Model>)await conn.QueryAsync<BankStatement_Request_Params_Model>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
    }
}
