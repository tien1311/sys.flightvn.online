using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Manager.Model.Models;
using Manager.Model.Models.BankAccount;
using Manager.Model.Models.Location;
using Manager.Model.Models.Other;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class BankRepository
    {
        private  string SQL_EV_MAIN_V2;
        private string SQL_BANK;
        private IConfiguration _configuration;
        public BankRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SQL_EV_MAIN_V2 = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            SQL_BANK = _configuration.GetConnectionString("SQL_BANK");
        }

        public (List<BankAccount>,int) GetBankAccounts(List<string> bankCodes, List<string> bankNames, List<string> firstSerials, int page, int pageSize)
        {
            List<BankAccount> bankAccounts = new List<BankAccount>();
            int totalRecords = 0;
            using (var connection = new SqlConnection(SQL_BANK))
            {
                connection.Open();
                var query = new StringBuilder(@"SELECT * FROM 
                                                    (SELECT ROW_NUMBER() OVER (ORDER BY CreatedDate DESC) AS RowNum, 
                                                    b.*
                                                    FROM Bank_Account b
                                                    WHERE 1=1");
                var queryCount = new StringBuilder("SELECT COUNT(*) FROM Bank_Account WHERE 1=1");

                if (bankCodes.Any())
                {
                    query.Append(" AND (");
                    for (int i = 0; i < bankCodes.Count; i++)
                    {
                        if (i > 0)
                        {
                            query.Append(" OR ");
                        }
                        query.Append($"BankCode LIKE @bankCodes{i}");
                    }
                    query.Append(")");

                    queryCount.Append(" AND (");
                    for (int i = 0; i < bankCodes.Count; i++)
                    {
                        if (i > 0)
                        {
                            queryCount.Append(" OR ");
                        }
                        queryCount.Append($"BankCode LIKE @bankCodes{i}");
                    }
                    queryCount.Append(")");

                    //query.Append(" AND Code IN @HotelCodes");
                    //queryCount.Append(" AND Code IN @HotelCodes");
                }

                if (bankNames.Any())
                {
                    query.Append(" AND (");
                    for (int i = 0; i < bankNames.Count; i++)
                    {
                        if (i > 0)
                        {
                            query.Append(" OR ");
                        }
                        query.Append($"BankName LIKE @bankNames{i}");
                    }
                    query.Append(")");

                    queryCount.Append(" AND (");
                    for (int i = 0; i < bankNames.Count; i++)
                    {
                        if (i > 0)
                        {
                            queryCount.Append(" OR ");
                        }
                        queryCount.Append($"BankName LIKE @bankNames{i}");
                    }
                    queryCount.Append(")");
                }
                if (firstSerials.Any())
                {
                    query.Append(" AND (");
                    for (int i = 0; i < firstSerials.Count; i++)
                    {
                        if (i > 0)
                        {
                            query.Append(" OR ");
                        }
                        query.Append($"FirstSerial LIKE @firstSerials{i}");
                    }
                    query.Append(")");

                    queryCount.Append(" AND (");
                    for (int i = 0; i < firstSerials.Count; i++)
                    {
                        if (i > 0)
                        {
                            queryCount.Append(" OR ");
                        }
                        queryCount.Append($"FirstSerial LIKE @firstSerials{i}");
                    }
                    queryCount.Append(")");

                }

                query.Append(") AS PRODUCT WHERE RowNum BETWEEN ((@PageNumber-1)*@PageSize+1) AND (@PageNumber*@PageSize) ORDER BY CreatedDate DESC");
                // Tạo đối tượng tham số
                var parameters = new DynamicParameters();
                //parameters.Add("HotelCodes", hotelCodes);

                for (int i = 0; i < bankCodes.Count; i++)
                {
                    parameters.Add($"bankCodes{i}", "%" + bankCodes[i] + "%");
                }

                for (int i = 0; i < bankNames.Count; i++)
                {
                    parameters.Add($"bankNames{i}", "%" + bankNames[i] + "%");
                }
                for (int i = 0; i < firstSerials.Count; i++)
                {
                    parameters.Add($"firstSerials{i}", "%" + firstSerials[i] + "%");
                }
                parameters.Add("PageNumber", page);
                parameters.Add("PageSize", pageSize);

                bankAccounts = connection.Query<BankAccount>(query.ToString(), parameters).ToList();
                totalRecords = connection.QuerySingle<int>(queryCount.ToString(), parameters);
                connection.Close();
            }

            return (bankAccounts, totalRecords);
        }

        public BankAccount GetBankAccount(int Id)
        {
            BankAccount bankAccount = new BankAccount();
            string sql = "SELECT * FROM Bank_Account WHERE Id = @Id"; 
            using (var conn = new SqlConnection(SQL_BANK))
            {
                conn.Open();
                bankAccount = conn.Query<BankAccount>(sql,new {Id  = Id}).FirstOrDefault();
                conn.Close();
            } 

            return bankAccount;
        }

        public BankAccount GetBankAccount(string bankCode)
        {
            BankAccount bankAccount = new BankAccount();
            string sql = "SELECT * FROM Bank_Account WHERE BankCode = @bankCode";
            using (var conn = new SqlConnection(SQL_BANK))
            {
                conn.Open();
                bankAccount = conn.Query<BankAccount>(sql, new { bankCode = bankCode }).FirstOrDefault();
                conn.Close();
            }
            return bankAccount;
        }

        public Task<bool> SaveCreateBankAccount(BankAccount request, string maNV)
        {
            bool isSuccess = false;
            string sql = @"INSERT INTO Bank_Account ([BankCode], [BankName], [FirstSerial], [CreatedDate], [CreatedBy]) 
                            VALUES (@bankCode, @bankName, @firstSerial, @createdDate, @createdBy)";
            int rowAfflect = 0;
            using (var conn = new SqlConnection(SQL_BANK))
            {
                conn.Open();
                rowAfflect = conn.Execute(sql, new 
                {
                    bankCode = request.BankCode, 
                    bankName = request.BankName, 
                    firstSerial = request.FirstSerial,
                    createdDate = DateTime.Now,
                    createdBy = maNV
                });
                conn.Close();

                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }

            return Task.FromResult(isSuccess);
        }

        public Task<bool> SaveEditBankAccount(BankAccount request, string maNV)
        {
            bool isSuccess = false;
            string sql = @"UPDATE Bank_Account 
                            SET BankCode = @bankCode,
                                BankName = @bankName,
                                FirstSerial = @firstSerial,
                                ModifiedDate = @modifiedDate,
                                ModifiedBy = @modifiedBy
                            WHERE Id = @Id";
            int rowAfflect = 0;
            using (var conn = new SqlConnection(SQL_BANK))
            {
                conn.Open();
                rowAfflect = conn.Execute(sql, new
                {
                    bankCode = request.BankCode,
                    bankName = request.BankName,
                    firstSerial = request.FirstSerial,
                    modifiedDate = DateTime.Now,
                    modifiedBy = maNV,
                    Id = request.Id
                });
                conn.Close();

                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }

            return Task.FromResult(isSuccess);
        }

        public Task<bool> SaveUndoBankAccount(BankAccount request, string maNV)
        {
            bool isSuccess = false;
            string sql = @"INSERT INTO Bank_Account ([BankCode], [BankName], [FirstSerial], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) 
                            VALUES (@bankCode, @bankName, @firstSerial, @createdDate, @createdBy, @modifiedDate, @modifiedBy)";
            int rowAfflect = 0;

            if (request.ModifiedDate == DateTime.MinValue)
            {
                request.ModifiedDate = DateTime.Now;
            }

            using (var conn = new SqlConnection(SQL_BANK))
            {
                conn.Open();
                rowAfflect = conn.Execute(sql, new
                {
                    bankCode = request.BankCode,
                    bankName = request.BankName,
                    firstSerial = request.FirstSerial,
                    createdDate = request.CreatedDate,
                    createdBy = request.CreatedBy,
                    modifiedDate = request.ModifiedDate,
                    modifiedBy = maNV
                });
                conn.Close();

                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return Task.FromResult(isSuccess);
        }

        public Task<bool> DeleteBankAccount(int Id)
        {
            bool isSuccess = false;
            string sql = @"DELETE FROM Bank_Account WHERE ID = @Id";
            int rowAfflect = 0;

            using ( var conn = new SqlConnection(SQL_BANK))
            {
                conn.Open();
                rowAfflect = conn.Execute(sql,new {Id = Id});
                conn.Close();
                if (rowAfflect > 0 )
                {
                    isSuccess = true;
                }
            }

            return Task.FromResult(isSuccess);

        }

        public List<BankAccountDetail> GetBankAccountDetails(List<string> agentCodes, List<string> listPhoneNumber, List<string> secondarySerials)
        {
            List<BankAccountDetail> bankAccountDetails = new List<BankAccountDetail>();
            using (var connection = new SqlConnection(SQL_BANK))
            {
                connection.Open();
                var query = new StringBuilder(@"SELECT * FROM 
                                                    (SELECT ROW_NUMBER() OVER (ORDER BY CreatedDate DESC) AS RowNum, 
                                                    b.*
                                                    FROM Bank_Account_Detail b
                                                    WHERE 1=1");
                var queryCount = new StringBuilder("SELECT COUNT(*) FROM Bank_Account_Detail WHERE 1=1");

                if (agentCodes.Any())
                {
                    query.Append(" AND (");
                    for (int i = 0; i < agentCodes.Count; i++)
                    {
                        if (i > 0)
                        {
                            query.Append(" OR ");
                        }
                        query.Append($"AgentCode LIKE @agentCodes{i}");
                    }
                    query.Append(")");
                }

                if (listPhoneNumber.Any())
                {
                    query.Append(" AND (");
                    for (int i = 0; i < listPhoneNumber.Count; i++)
                    {
                        if (i > 0)
                        {
                            query.Append(" OR ");
                        }
                        query.Append($"PhoneNumber LIKE @phoneNumber{i}");
                    }
                    query.Append(")");
                }
                if (secondarySerials.Any())
                {
                    query.Append(" AND (");
                    for (int i = 0; i < secondarySerials.Count; i++)
                    {
                        if (i > 0)
                        {
                            query.Append(" OR ");
                        }
                        query.Append($"SecondarySerial LIKE @secondarySerials{i}");
                    }
                    query.Append(")");
                }

                query.Append(") AS PRODUCT ORDER BY CreatedDate DESC");
                // Tạo đối tượng tham số
                var parameters = new DynamicParameters();
                //parameters.Add("HotelCodes", hotelCodes);

                for (int i = 0; i < agentCodes.Count; i++)
                {
                    parameters.Add($"agentCodes{i}", "%" + agentCodes[i] + "%");
                }

                for (int i = 0; i < listPhoneNumber.Count; i++)
                {
                    parameters.Add($"phoneNumber{i}", "%" + listPhoneNumber[i] + "%");
                }
                for (int i = 0; i < secondarySerials.Count; i++)
                {
                    parameters.Add($"secondarySerials{i}", "%" + secondarySerials[i] + "%");
                }

                bankAccountDetails = connection.Query<BankAccountDetail>(query.ToString(), parameters).ToList();
                connection.Close();
            }

            return bankAccountDetails;
        }

        public (List<BankAccountDetail>, int) GetBankAccountDetails(List<string> agentCodes, List<string> listPhoneNumber, List<string> secondarySerials, int page, int pageSize)
        {
            List<BankAccountDetail> bankAccounts = new List<BankAccountDetail>();
            int totalRecords = 0;
            using (var connection = new SqlConnection(SQL_BANK))
            {
                connection.Open();
                var query = new StringBuilder(@"SELECT * FROM 
                                                    (SELECT ROW_NUMBER() OVER (ORDER BY CreatedDate DESC) AS RowNum, 
                                                    b.*
                                                    FROM Bank_Account_Detail b
                                                    WHERE 1=1");
                var queryCount = new StringBuilder("SELECT COUNT(*) FROM Bank_Account_Detail WHERE 1=1");

                if (agentCodes.Any())
                {
                    query.Append(" AND (");
                    for (int i = 0; i < agentCodes.Count; i++)
                    {
                        if (i > 0)
                        {
                            query.Append(" OR ");
                        }
                        query.Append($"AgentCode LIKE @agentCodes{i}");
                    }
                    query.Append(")");

                    queryCount.Append(" AND (");
                    for (int i = 0; i < agentCodes.Count; i++)
                    {
                        if (i > 0)
                        {
                            queryCount.Append(" OR ");
                        }
                        queryCount.Append($"AgentCode LIKE @agentCodes{i}");
                    }
                    queryCount.Append(")");

                    //query.Append(" AND Code IN @HotelCodes");
                    //queryCount.Append(" AND Code IN @HotelCodes");
                }

                if (listPhoneNumber.Any())
                {
                    query.Append(" AND (");
                    for (int i = 0; i < listPhoneNumber.Count; i++)
                    {
                        if (i > 0)
                        {
                            query.Append(" OR ");
                        }
                        query.Append($"PhoneNumber LIKE @phoneNumber{i}");
                    }
                    query.Append(")");

                    queryCount.Append(" AND (");
                    for (int i = 0; i < listPhoneNumber.Count; i++)
                    {
                        if (i > 0)
                        {
                            queryCount.Append(" OR ");
                        }
                        queryCount.Append($"PhoneNumber LIKE @phoneNumber{i}");
                    }
                    queryCount.Append(")");
                }
                if (secondarySerials.Any())
                {
                    query.Append(" AND (");
                    for (int i = 0; i < secondarySerials.Count; i++)
                    {
                        if (i > 0)
                        {
                            query.Append(" OR ");
                        }
                        query.Append($"SecondarySerial LIKE @secondarySerials{i}");
                    }
                    query.Append(")");

                    queryCount.Append(" AND (");
                    for (int i = 0; i < secondarySerials.Count; i++)
                    {
                        if (i > 0)
                        {
                            queryCount.Append(" OR ");
                        }
                        queryCount.Append($"SecondarySerial LIKE @secondarySerials{i}");
                    }
                    queryCount.Append(")");

                }

                query.Append(") AS PRODUCT WHERE RowNum BETWEEN ((@PageNumber-1)*@PageSize+1) AND (@PageNumber*@PageSize) ORDER BY CreatedDate DESC");
                // Tạo đối tượng tham số
                var parameters = new DynamicParameters();
                //parameters.Add("HotelCodes", hotelCodes);

                for (int i = 0; i < agentCodes.Count; i++)
                {
                    parameters.Add($"agentCodes{i}", "%" + agentCodes[i] + "%");
                }

                for (int i = 0; i < listPhoneNumber.Count; i++)
                {
                    parameters.Add($"phoneNumber{i}", "%" + listPhoneNumber[i] + "%");
                }
                for (int i = 0; i < secondarySerials.Count; i++)
                {
                    parameters.Add($"secondarySerials{i}", "%" + secondarySerials[i] + "%");
                }
                parameters.Add("PageNumber", page);
                parameters.Add("PageSize", pageSize);

                bankAccounts = connection.Query<BankAccountDetail>(query.ToString(), parameters).ToList();
                totalRecords = connection.QuerySingle<int>(queryCount.ToString(), parameters);
                connection.Close();
            }

            return (bankAccounts, totalRecords);
        }

        public BankAccountDetail GetBankAccountDetail(int Id)
        {
            BankAccountDetail bankAccount = new BankAccountDetail();
            string sql = "SELECT * FROM Bank_Account_Detail WHERE Id = @Id";
            using (var conn = new SqlConnection(SQL_BANK))
            {
                conn.Open();
                bankAccount = conn.Query<BankAccountDetail>(sql, new { Id = Id }).FirstOrDefault();
                conn.Close();
            }

            return bankAccount;
        }

        public BankAccountDetail GetBankAccountDetail(string agentCode)
        {
            BankAccountDetail bankAccount = new BankAccountDetail();
            string sql = "SELECT * FROM Bank_Account_Detail WHERE AgentCode = @agentCode";
            using (var conn = new SqlConnection(SQL_BANK))
            {
                conn.Open();
                bankAccount = conn.Query<BankAccountDetail>(sql, new { agentCode = agentCode }).FirstOrDefault();
                conn.Close();
            }

            return bankAccount;
        }

        public Task<bool> SaveCreateBankAccountDetail(BankAccountDetail request, string maNV)
        {
            bool isSuccess = false;
            string sql = @"INSERT INTO Bank_Account_Detail ([AgentCode], [PhoneNumber], [SecondarySerial], [CreatedDate], [CreatedBy]) 
                            VALUES (@agentCode, @phoneNumber, @secondarySerial, @createdDate, @createdBy)";
            int rowAfflect = 0;
            using (var conn = new SqlConnection(SQL_BANK))
            {
                conn.Open();
                rowAfflect = conn.Execute(sql, new
                {
                    agentCode = request.AgentCode,
                    phoneNumber = request.PhoneNumber,
                    secondarySerial = request.SecondarySerial,
                    createdDate = DateTime.Now,
                    createdBy = maNV
                });
                conn.Close();

                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }

            return Task.FromResult(isSuccess);
        }

        public Task<bool> SaveEditBankAccountDetail(BankAccountDetail request, string maNV)
        {
            bool isSuccess = false;
            string sql = @"UPDATE Bank_Account_Detail 
                            SET AgentCode = @agentCode,
                                PhoneNumber = @phoneNumber,
                                SecondarySerial = @secondarySerial,
                                ModifiedDate = @modifiedDate,
                                ModifiedBy = @modifiedBy
                            WHERE Id = @Id";
            int rowAfflect = 0;
            using (var conn = new SqlConnection(SQL_BANK))
            {
                conn.Open();
                rowAfflect = conn.Execute(sql, new
                {
                    agentCode = request.AgentCode,
                    phoneNumber = request.PhoneNumber,
                    secondarySerial = request.SecondarySerial,
                    modifiedDate = DateTime.Now,
                    modifiedBy = maNV,
                    Id = request.Id
                });
                conn.Close();

                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }

            return Task.FromResult(isSuccess);
        }

        public Task<bool> SaveUndoBankAccountDetail(BankAccountDetail request, string maNV)
        {
            bool isSuccess = false;
            string sql = @"INSERT INTO Bank_Account_Detail ([AgentCode], [PhoneNumber], [SecondarySerial], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) 
                            VALUES (@agentCode, @phoneNumber, @secondarySerial, @createdDate, @createdBy, @modifiedDate, @modifiedBy)";
            int rowAfflect = 0;

            if (request.ModifiedDate == DateTime.MinValue)
            {
                request.ModifiedDate = DateTime.Now;
            }

            using (var conn = new SqlConnection(SQL_BANK))
            {
                conn.Open();
                rowAfflect = conn.Execute(sql, new
                {
                    agentCode = request.AgentCode,
                    phoneNumber = request.PhoneNumber,
                    secondarySerial = request.SecondarySerial,
                    createdDate = request.CreatedDate,
                    createdBy = request.CreatedBy,
                    modifiedDate = request.ModifiedDate,
                    modifiedBy = maNV
                });
                conn.Close();

                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return Task.FromResult(isSuccess);
        }

        public Task<bool> DeleteBankAccountDetail(int Id)
        {
            bool isSuccess = false;
            string sql = @"DELETE FROM Bank_Account_Detail WHERE ID = @Id";
            int rowAfflect = 0;

            using (var conn = new SqlConnection(SQL_BANK))
            {
                conn.Open();
                rowAfflect = conn.Execute(sql, new { Id = Id });
                conn.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }

            return Task.FromResult(isSuccess);

        }

        public List<BankModel> ListBank()
        {
            List<BankModel> result = new List<BankModel>();
            string sql = @"select * from BANK order by ShortName";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<BankModel>)conn.Query<BankModel>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public List<BANK_ACCOUNT> Bank()
        {
            List<BANK_ACCOUNT> result = new List<BANK_ACCOUNT>();
            string sql = @"select BANK_ACCOUNT_EV.*, BANK.ShortName as BankName from BANK_ACCOUNT_EV left join BANK on BANK_ACCOUNT_EV.IDBank = BANK.ID ";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<BANK_ACCOUNT>)conn.Query<BANK_ACCOUNT>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public BANK_ACCOUNT EditBank(int ID)
        {
            BANK_ACCOUNT result = new BANK_ACCOUNT();
            string sql = @"select * from BANK_ACCOUNT_EV where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = conn.QueryFirst<BANK_ACCOUNT>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool SaveCreateBank(BANK_ACCOUNT model)
        {
            int i = 0;
            string sql = @"INSERT INTO [BANK_ACCOUNT_EV] ([IDBank],[AccountName],[AccountNumber],[Position],[IsActive])VALUES ('" + model.IDBank + "',N'" + model.AccountName + "','" + model.AccountNumber + "','" + model.Position + "','1')";
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
        public bool SaveEditBank(BANK_ACCOUNT model)
        {
            int i = 0;
            string sql = @"UPDATE BANK_ACCOUNT_EV SET [IDBank] = '" + model.IDBank + "', [AccountName] = N'" + model.AccountName + "',[AccountNumber] = '" + model.AccountNumber + "',[Position] = '" + model.Position + "' where ID = '" + model.ID + "'";
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
        public bool DeleteBank(int ID)
        {
            int i = 0;
            string sql = @"UPDATE BANK_ACCOUNT_EV SET [IsActive] = '0' where ID = '" + ID + "'";
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
        public bool ActiveBank(int ID)
        {
            int i = 0;
            string sql = @"UPDATE BANK_ACCOUNT_EV SET [IsActive] = '1' where ID = '" + ID + "'";
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
