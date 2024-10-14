using Dapper;
using Manager.Model.Models;
using Manager.Model.Models.IP;
using Manager.Model.Models.Location;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using TangDuLieu;

namespace Manager.DataAccess.Repository
{
    public class IPRepository
    {
        private string SQL_EV_SERVICE;
        public IPRepository(IConfiguration configuration)
        {
            SQL_EV_SERVICE = configuration.GetConnectionString("SQL_EV_SERVICE");
        }

        public async Task<(List<APIPartner>, int)> GetAPIPartners(int page, int pageSize, string companyName)
        {
            List<APIPartner> listPartner = new List<APIPartner>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(SQL_EV_SERVICE))
            {
                await connection.OpenAsync();
                var query = new StringBuilder(@"SELECT *
                                                FROM (
                                                    SELECT 
                                                        ROW_NUMBER() OVER (ORDER BY api.CreatedDate DESC) AS RowNum,
                                                    api.Id,
                                                    api.Company,
                                                    api.PhysicalAddress,
                                                    api.CreatedDate,
                                                    api.CreatedBy,
                                                    STUFF((
                                                        SELECT ', ' + ip.IPAddress
                                                        FROM IP_Partner ip
                                                        WHERE ip.PartnerId = api.Id
                                                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS IPAddresses,
                                                    STUFF((
                                                        SELECT ', ' + CAST(ip.Id AS NVARCHAR)
                                                        FROM IP_Partner ip
                                                        WHERE ip.PartnerId = api.Id
                                                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS IPAddressIDs
                                                    FROM 
                                                        API_Partner api
                                                    WHERE 1 = 1");
                var queryCount = new StringBuilder("SELECT COUNT(*) FROM API_Partner WHERE 1=1");
                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(companyName))
                {
                    string searchPattern = "%" + companyName + "%";

                    query.Append(" AND Company LIKE @companyName");
                    queryCount.Append(" AND Company LIKE @companyName");
                    parameters.Add("companyName", searchPattern);

                }
                query.Append(") AS SubQuery WHERE RowNum BETWEEN ((@PageNumber-1)*@PageSize+1) AND (@PageNumber*@PageSize) ORDER BY RowNum");

                parameters.Add("PageNumber", page);
                parameters.Add("PageSize", pageSize);

                listPartner = connection.QueryAsync<APIPartner>(query.ToString(), parameters).GetAwaiter().GetResult().ToList();
                totalRecords = connection.QuerySingleAsync<int>(queryCount.ToString(), parameters).GetAwaiter().GetResult();
                connection.Close();
            }
            return (listPartner, totalRecords);
        }


        public async Task<List<IPPartner>> GetListIP(int partnerId)
        {
            List<IPPartner> listPartner = new List<IPPartner>();
            string sql = @"select * from IP_Partner
                            where PartnerId = @partnerId";
            using (var connection = new SqlConnection(SQL_EV_SERVICE))
            {
                await connection.OpenAsync();
                listPartner = connection.QueryAsync<IPPartner>(sql,new {partnerId =  partnerId}).GetAwaiter().GetResult().ToList();
                connection.Close();
            }

            return listPartner;
        }
        public async Task<APIPartner> GetAPIPartnerById(int id)
        {
            APIPartner apiPartner = new APIPartner();
            var sql = @"
                         SELECT * FROM API_Partner WHERE Id = @id          
                            ";

            using (var connection = new SqlConnection(SQL_EV_SERVICE))
            {
                await connection.OpenAsync();
                apiPartner = connection.QueryAsync<APIPartner>(sql,new {id = id}).GetAwaiter().GetResult().FirstOrDefault();
                connection.Close();
            }

            return apiPartner;
        }
        //public APIPartner GetAPIPartnerById(int id)
        //{
        //    using (var connection = new SqlConnection(SQL_EV_SERVICE))
        //    {
        //        // Câu truy vấn SQL
        //        var sql = @"
        //        SELECT api.Id, api.Company, api.PhysicalAddress, api.CreatedDate, api.CreatedBy,
        //               ip.Id AS IPPartnerId, ip.IPAddress
        //        FROM API_Partner api
        //        JOIN IP_Partner ip ON ip.PartnerId = api.Id
        //        WHERE api.Id = @Id";

        //        // Thực hiện truy vấn
        //        var apiPartnersDictionary = new Dictionary<int, APIPartner>();

        //        var result = connection.Query<APIPartner, IPPartner, APIPartner>(
        //            sql,
        //            (api, ip) =>
        //            {
        //                if (!apiPartnersDictionary.TryGetValue(api.Id, out var apiPartner))
        //                {
        //                    apiPartner = api;
        //                    apiPartner.IPPartners = new List<IPPartner>();
        //                    apiPartnersDictionary.Add(apiPartner.Id, apiPartner);
        //                }

        //                if (ip != null)
        //                {
        //                    apiPartner.IPPartners.Add(ip);
        //                }

        //                return apiPartner;
        //            },
        //            new { Id = id },
        //            splitOn: "IPPartnerId" // Phân tách các trường để Dapper biết bắt đầu IPPartner
        //        );

        //        return apiPartnersDictionary.Values.FirstOrDefault();
        //    }
        //}

        public async Task<List<EndpointPartner>> GetListEndPointIP(int IPPartnerId)
        {
            List<EndpointPartner> listEndpoint = new List<EndpointPartner>();
            string sql = @"SELECT ep.* 
                        FROM EndPoint_Partner ep
                        JOIN IP_Partner ip ON ip.Id = ep.IPPartnerId
                        WHERE IPPartnerId = @IPPartnerId";
            using (var connection = new SqlConnection(SQL_EV_SERVICE))
            {
                await connection.OpenAsync();
                listEndpoint = connection.QueryAsync<EndpointPartner>(sql, new { IPPartnerId = IPPartnerId }).GetAwaiter().GetResult().ToList();
                connection.Close();
            }
            return listEndpoint;
        }

        public async Task<IPPartner> GetIPPartner(int IPPartnerId)
        {
            IPPartner IPPartner = new IPPartner();
            string sql = @"SELECT * FROM IP_Partner WHERE ID = @IPPartnerID";
            using (var connection = new SqlConnection(SQL_EV_SERVICE))
            {
                await connection.OpenAsync();
                IPPartner = connection.QueryAsync<IPPartner>(sql, new { IPPartnerID = IPPartnerId }).GetAwaiter().GetResult().FirstOrDefault();
                connection.Close();
            }
            return IPPartner;
        }

        public async Task<EndpointPartner> GetEndPoint(int id)
        {
            EndpointPartner endpointPartner = new EndpointPartner();
            string sql = @"SELECT * FROM EndPoint_Partner WHERE ID = @ID";
            using (var connection = new SqlConnection(SQL_EV_SERVICE))
            {
                await connection.OpenAsync();
                endpointPartner = connection.QueryAsync<EndpointPartner>(sql, new { id = id }).GetAwaiter().GetResult().FirstOrDefault();
                connection.Close();
            }

            return endpointPartner;
        }

        public async Task<bool> SaveEditAPIPartner(APIPartner request)
        {
            bool isSuccess = false;
            string updateApiPartnerSql = @"
                                        UPDATE API_Partner
                                        SET Company = @company, 
                                            PhysicalAddress = @physicalAddress
                                        WHERE Id = @Id
                                        ";
            int rowAfflect = 0;
            using (var connection = new SqlConnection(SQL_EV_SERVICE))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert into API_Partner
                        rowAfflect = await connection.ExecuteAsync(updateApiPartnerSql, new
                        {
                            Company = request.Company.Trim(),
                            PhysicalAddress = request.PhysicalAddress.Trim(),
                            Id = request.Id,
                        }, transaction, commandTimeout: 30, commandType: CommandType.Text);

                        if (rowAfflect >0)
                        {
                            transaction.Commit();
                            isSuccess = true;
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveCreateIPWhiteList(APIPartner request, string MaNV)
        {
            bool isSuccess = false;
            int ID = 0;
            string insertApiPartnerSql = @"
                INSERT INTO [dbo].[API_Partner] (Company, PhysicalAddress, CreatedDate, CreatedBy)
                OUTPUT INSERTED.ID
                VALUES (@Company, @PhysicalAddress, @CreatedDate, @CreatedBy);";

            string insertIpPartnerSql = @"
                INSERT INTO [dbo].[IP_Partner] (PartnerId, IPAddress)
                VALUES (@PartnerId, @IPAddress);";
              

            ////string insertEndpointPartnerSql = @"
            ////    INSERT INTO [dbo].[EndPoint_Partner] (IPPartnerId, EndPoint, IsActived)
            ////    VALUES (@IPPartnerId, @EndPoint, 1);";

            using (var connection = new SqlConnection(SQL_EV_SERVICE))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert into API_Partner
                        ID = connection.QuerySingle<int>(insertApiPartnerSql, new
                        {
                            Company = request.Company.Trim(),
                            PhysicalAddress = request.PhysicalAddress.Trim(),
                            CreatedDate = DateTime.Now,
                            CreatedBy = MaNV,
                        }, transaction, commandTimeout: 30, commandType: CommandType.Text);


                        // Insert into IP_Partner
                        foreach (var ipPartner in request.IPPartners)
                        {
                            var ipPartnerId = (int)connection.ExecuteAsync(insertIpPartnerSql, new
                            {
                                PartnerId = ID,
                                IPAddress = ipPartner.IPAddress.Trim()
                            }, transaction).GetAwaiter().GetResult();
                        }
                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveCreateEndPoint(int ipAddressId, string[] endpoints)
        {
            bool isSuccess = false;
            string sql = @"INSERT INTO EndPoint_Partner ([IPPartnerId], [Endpoint], [IsActived])
                   VALUES (@ipAddressId, @endpoint, 1)";

            using (var conn = new SqlConnection(SQL_EV_SERVICE))
            {
                await conn.OpenAsync();
                int totalInserted = 0;

                foreach (var endpoint in endpoints)
                {
                    var rowAffected = await conn.ExecuteAsync(sql, new { ipAddressId = ipAddressId, endpoint = endpoint.Trim() });
                    totalInserted += rowAffected;
                }

                conn.Close();

                if (totalInserted > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> SaveEditIPAddress(int PartnerId, string[] ipAddress)
        {
            bool isSuccess = false;
            string sql = @"INSERT INTO IP_Partner ([PartnerId], [IPAddress]) 
                            VALUES (@partnerId, @ipAddress)";
            using (var conn = new SqlConnection(SQL_EV_SERVICE))
            {
                await conn.OpenAsync();
                int totalInserted = 0;

                foreach (var ip in ipAddress)
                {
                    var rowAffected = await conn.ExecuteAsync(sql, new { partnerId = PartnerId, ipAddress = ip });
                    totalInserted += rowAffected;
                }

                conn.Close();

                if (totalInserted > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> DeleteIPAddress(int Id)
        {
            bool isSuccess = false;
            string DeleteEndpoint_Sql = @"DELETE FROM EndPoint_Partner WHERE IPPartnerId = @Id";
            string DeleteIP_SQL = @"DELETE FROM IP_Partner WHERE Id = @Id";
            int rowAfflect = 0;
            using (var connection = new SqlConnection(SQL_EV_SERVICE))
            {
                await connection.OpenAsync();
                rowAfflect = connection.ExecuteAsync(DeleteEndpoint_Sql, new { Id = Id }).GetAwaiter().GetResult();
                rowAfflect = connection.ExecuteAsync(DeleteIP_SQL, new { Id = Id }).GetAwaiter().GetResult();
                connection.Close();
                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public async Task<bool> ChangeIPWhiteListActive(bool isActived, int id)
        {
            bool isSuccess = false;
            string sql = @"UPDATE Endpoint_Partner 
                            SET IsActived = @isActived 
                            WHERE Id = @id";
            int rowAfflect = 0;
            using (var connection = new SqlConnection(SQL_EV_SERVICE))
            {
                await connection.OpenAsync();
                rowAfflect = connection.ExecuteAsync(sql, new { isActived = isActived, id = id }).GetAwaiter().GetResult();
                connection.Close();

                if (rowAfflect > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

    }
}
