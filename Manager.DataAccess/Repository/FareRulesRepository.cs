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
    public class FareRulesRepository
    {
        private string SQL_EV_MAIN_V2;
        public FareRulesRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN_V2 = configuration.GetConnectionString("SQL_EV_MAIN_V2");
        }
        public List<Rules_Airlines> Rules_Airlines()
        {
            List<Rules_Airlines> result = new List<Rules_Airlines>();
            string sql = @"select * from FareRules_Airlines";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<Rules_Airlines>)conn.Query<Rules_Airlines>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public List<Rule_Categories> Rules_Categories()
        {
            List<Rule_Categories> result = new List<Rule_Categories>();
            string sql = @"select Cate.*,Air.AirlineName from FareRules_Categories Cate left join FareRules_Airlines Air on Air.AirlineID = Cate.AirlineID";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<Rule_Categories>)conn.Query<Rule_Categories>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public List<Rules_Partners> Rules_Partners()
        {
            List<Rules_Partners> result = new List<Rules_Partners>();
            string sql = @"select * from FareRules_Partners ";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<Rules_Partners>)conn.Query<Rules_Partners>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public List<Rules_RuleDetails> Rules_RuleDetails()
        {
            List<Rules_RuleDetails> result = new List<Rules_RuleDetails>();
            string sql = @"select Rules.*, Air.AirlineName, Cate.CategoryName from FareRules_AirlineRules Rules
                         left join FareRules_Airlines Air on Air.AirlineID = Rules.AirlineID
                         left join FareRules_Categories Cate on Cate.CategoryId = Rules.CategoryId";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<Rules_RuleDetails>)conn.Query<Rules_RuleDetails>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool SaveCreateRules_Airlines(Rules_Airlines model)
        {
            int i = 0;
            string sql = @"INSERT INTO [FareRules_Airlines] ([AirlineName] ,[AirlineIATACode] ,[AirlineICAOCode] ,[AirlineOATCode] ,[Country]) VALUES ( N'" + model.AirlineName + "','" + model.AirlineIATACode + "','" + model.AirlineICAOCode + "','" + model.AirlineOATCode + "',N'" + model.Country + "')";
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
        public bool SaveCreateRules_Categories(Rule_Categories model)
        {
            int i = 0;
            string sql = @"INSERT INTO [FareRules_Categories] ([AirlineID] ,[CategoryName]) VALUES ( N'" + model.AirlineID + "','" + model.CategoryName + "')";
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
        public bool SaveCreateRules_Partners(Rules_Partners model)
        {
            int i = 0;
            string sql = @"INSERT INTO [FareRules_Partners] ([PartnerCode],[PartnerName],[PartnerPhones],[PartnerEmails])VALUES ('" + model.PartnerCode + "',N'" + model.PartnerName + "','" + model.PartnerPhones + "','" + model.PartnerEmails + "')";
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
        public bool SaveCreateRules_RuleDetails(Rules_RuleDetails model)
        {
            int i = 0;
            string sql = @"INSERT INTO [FareRules_AirlineRules] ([AirlineID],[CategoryId],[SeatClasses],[DomesticRules_vi],[DomesticRules_en],[IntlRules_vi],[IntlRules_en],[CabinClassCode],[CabinClassName_vi],[CabinClassName_en])VALUES ('" + model.AirlineID + "','" + model.CategoryId + "','" + model.SeatClasses + "',N'" + model.DomesticRules_vi + "',N'" + model.DomesticRules_en + "',N'" + model.IntlRules_vi + "',N'" + model.IntlRules_en + "','" + model.CabinClassCode + "',N'" + model.CabinClassName_vi + "',N'" + model.CabinClassName_en + "')";
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
        public Rules_Airlines EditRules_Airlines(int ID)
        {
            Rules_Airlines result = new Rules_Airlines();
            string sql = @"select * from FareRules_Airlines where AirlineID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = conn.QueryFirst<Rules_Airlines>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public Rule_Categories EditRules_Categories(int ID)
        {
            Rule_Categories result = new Rule_Categories();
            string sql = @"select * from FareRules_Categories where CategoryID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = conn.QueryFirst<Rule_Categories>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public Rules_Partners EditRules_Partners(int ID)
        {
            Rules_Partners result = new Rules_Partners();
            string sql = @"select * from FareRules_Partners where PartnerID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = conn.QueryFirst<Rules_Partners>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public Rules_RuleDetails EditRules_RuleDetails(int ID)
        {
            Rules_RuleDetails result = new Rules_RuleDetails();
            string sql = @"select Rules.*, Air.AirlineName, Cate.CategoryName from FareRules_AirlineRules Rules
                         left join FareRules_Airlines Air on Air.AirlineID = Rules.AirlineID
                         left join FareRules_Categories Cate on Cate.CategoryId = Rules.CategoryId where RuleDetailID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = conn.QueryFirst<Rules_RuleDetails>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool SaveEditRules_Airlines(Rules_Airlines model)
        {
            int i = 0;
            string sql = @"UPDATE FareRules_Airlines SET [AirlineName] = N'" + model.AirlineName + "',[AirlineIATACode] = '" + model.AirlineIATACode + "',[AirlineICAOCode] = '" + model.AirlineICAOCode + "',[AirlineOATCode] = '" + model.AirlineOATCode + "',[Country] = N'" + model.Country + "' where AirlineID = '" + model.AirlineID + "'";
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
        public bool SaveEditRules_Categories(Rule_Categories model)
        {
            int i = 0;
            string sql = @"UPDATE FareRules_Categories SET [AirlineID] = N'" + model.AirlineID + "',[CategoryName] = '" + model.CategoryName + "' where CategoryID = '" + model.CategoryID + "'";
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
        public bool SaveEditRules_Partners(Rules_Partners model)
        {
            int i = 0;
            string sql = @"UPDATE FareRules_Partners SET [PartnerCode] = '" + model.PartnerCode + "',[PartnerName] = N'" + model.PartnerName + "',[PartnerPhones] = '" + model.PartnerPhones + "',[PartnerEmails] = '" + model.PartnerEmails + "' where PartnerID = '" + model.PartnerID + "'";
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
        public bool SaveEditRules_RuleDetails(Rules_RuleDetails model)
        {
            int i = 0;
            string sql = @"UPDATE FareRules_AirlineRules SET [AirlineID] = '" + model.AirlineID + "',[CategoryId] = '" + model.CategoryId + "',[SeatClasses] = '" + model.SeatClasses + "',[DomesticRules_vi] = N'" + model.DomesticRules_vi + "',[DomesticRules_en] = N'" + model.DomesticRules_en + "',[IntlRules_vi] = N'" + model.IntlRules_vi + "',[IntlRules_en] = N'" + model.IntlRules_en + "',[CabinClassCode] = '" + model.CabinClassCode + "',[CabinClassName_vi] = N'" + model.CabinClassName_vi + "',[CabinClassName_en] = N'" + model.CabinClassName_en + "' where RuleDetailID = '" + model.RuleDetailID + "'";
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
        public bool DeleteRules_Airlines(int ID)
        {
            int i = 0;
            string sql = @"DELETE FROM FareRules_Airlines where AirlineID = '" + ID + "'";
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
        public bool DeleteRules_Categories(int ID)
        {
            int i = 0;
            string sql = @"DELETE FROM FareRules_Categories where CategoryID = '" + ID + "'";
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
        public bool DeleteRules_Partners(int ID)
        {
            int i = 0;
            string sql = @"DELETE FROM FareRules_Partners where PartnerID = '" + ID + "'";
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
        public bool DeleteRules_PartnerDetails(int ID)
        {
            int i = 0;
            string sql = @"DELETE FROM FareRules_PartnerDetails where PartnerDetailID = '" + ID + "'";
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
        public bool DeleteRules_RuleDetails(int ID)
        {
            int i = 0;
            string sql = @"";
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
        public List<Rule_Categories> ListCategories(int AirlineID)
        {
            List<Rule_Categories> result = new List<Rule_Categories>();
            string sql = @"select * from FareRules_Categories where AirlineID = '" + AirlineID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<Rule_Categories>)conn.Query<Rule_Categories>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public List<Rules_RuleDetails> RuleDetail(int PartnerID)
        {
            List<Rules_RuleDetails> result = new List<Rules_RuleDetails>();
            string sql = @"select AirRule.*,Air.AirlineName,Cate.CategoryName,Part_Det.PartnerDetailID from FareRules_AirlineRules AirRule
							left join FareRules_Categories Cate on Cate.CategoryID = AirRule.CategoryId
							left join FareRules_Airlines Air on Air.AirlineID = AirRule.AirlineID
							left join FareRules_PartnerDetails Part_Det on AirRule.RuleDetailID = Part_Det.RuleDetailID
                            left join FareRules_Partners Part on Part.PartnerID = Part_Det.PartnerID
                            where Part.PartnerID = '" + PartnerID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<Rules_RuleDetails>)conn.Query<Rules_RuleDetails>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool SaveImportRule(ImportPartnerDetails model)
        {
            int z = 0;
            string sql = "";
            for (int i = 0; i < model.ListRuleDetails.Count; i++)
            {
                sql += "INSERT INTO [FareRules_PartnerDetails] ([PartnerID],[RuleDetailID])VALUES ('" + model.PartnerID + "','" + model.ListRuleDetails[i].RuleDetailID + "')";
            }
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                z = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (z > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
