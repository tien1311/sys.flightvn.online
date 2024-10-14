using Dapper;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.DataAccess.Repository
{
    public class PermissionRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string departmentsConnectionString;
        private readonly string airlineConnectionString;
        private readonly string userPermissionConnectionString;

        public PermissionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            departmentsConnectionString = _configuration.GetConnectionString("SQL_EV_MAIN");
            airlineConnectionString = _configuration.GetConnectionString("SQL_Agent_MAIN");
            userPermissionConnectionString = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
        }

        public Task<List<AccountModel>> GetAccountFromDepartment()
        {
            List<AccountModel> account = new List<AccountModel>();
            string sql = @"
                            SELECT 
                                TenDangNhap,
                                Ten as HoTen,
                                MaPhongBan 
                                FROM DM_NV 
                                WHERE DM_NV.TINHTRANG=1
                        ";
            using (var conn = new SqlConnection(departmentsConnectionString))
            {
                conn.Open();
                account = conn.Query<AccountModel>(sql, null).ToList();
                conn.Close();
            }
            return Task.FromResult(account);
        }

        public Task<List<AccountModel>> GetAccountFromDepartment(string departmentCode)
        {
            List<AccountModel> account = new List<AccountModel>();
            string sql = @"
                            SELECT 
                                TenDangNhap,
                                Ten as HoTen,
                                MaPhongBan 
                                FROM DM_NV 
                                WHERE DM_NV.TINHTRANG=1
                                AND MAPHONGBAN = @departmentCode 
                        ";
            using (var conn = new SqlConnection(departmentsConnectionString))
            {
                conn.Open();
                account = conn.Query<AccountModel>(sql, new { departmentCode }).ToList();
                conn.Close();
            }
            return Task.FromResult(account);
        }

        public Task<List<AccountModel>> GetListDepartment()
        {
            List<AccountModel> department = new List<AccountModel>();
            string sql = @"SELECT 
                            ID as MaPhongBan,
                            Ten as PhongBan 
                            FROM PHONGBAN 
                            WHERE PHONGBAN.TINHTRANG=1 
                            order by POSITIONID";
            using (var conn = new SqlConnection(departmentsConnectionString))
            {
                conn.Open();
                department = conn.Query<AccountModel>(sql, null).ToList();
                conn.Close();
            }
            return Task.FromResult(department);
        }

        public Task<List<FlightModel>> GetListAirline()
        {
            List<FlightModel> listAirline = new List<FlightModel>();
            string sql = @"SELECT 
                            section_id as Id, 
                            section_name as Airline 
                            FROM subject_section 
                            WHERE parent_id='76' AND Position_id='3'";
            using (var conn = new SqlConnection(airlineConnectionString))
            {
                conn.Open();
                listAirline = conn.Query<FlightModel>(sql, null).ToList();
                conn.Close();
            }
            return Task.FromResult(listAirline);
        }

        public Task<List<Permission>> GetUserPermission(string userId)
        {
            List<Permission> userPermissions = new List<Permission>();
            string sql = @"SELECT * FROM Permission WHERE UserID = @userId";
            using (var conn = new SqlConnection(userPermissionConnectionString))
            {
                conn.Open();
                userPermissions = conn.Query<Permission>(sql, new { userId }).ToList();
                conn.Close();
            }
            return Task.FromResult(userPermissions);
        }


        public async Task<bool> InsertOrUpdateUserPermissions(List<Permission> permissions)
        {
            bool isSuccess = false;

            string sqlCheckExist = @"SELECT COUNT(1) FROM [Permission] WHERE UserId = @userId AND PageId = @pageId";

            string sqlInsert = @"INSERT INTO [Permission] (UserId, PageId, CanRead, CanWrite, CanDelete, CanExportExcel, CreatedDate, CreatedBy, IsModify)
                VALUES (@userId, @pageId, @canRead, @canWrite, @canDelete, @canExportExcel, @createdDate, @createdBy, @isModify)";

            string sqlUpdatePermission = @"UPDATE [Permission] 
                          SET CanRead = @canRead, 
                              CanWrite = @canWrite, 
                              CanDelete = @canDelete, 
                              CanExportExcel = @canExportExcel, 
                              IsModify = @isModify 
                          WHERE UserId = @userId AND PageId = @pageId";

            string sqlInsertPermissionLogs = @"INSERT INTO [PermissionModifyLogs] (PermissionId, UserId, ModifiedDate, ModifiedBy)
                              VALUES (@permissionId, @userId, @modifiedDate, @modifiedBy)";

            using (var conn = new SqlConnection(userPermissionConnectionString))
            {
                conn.Open();
                foreach (var permission in permissions)
                {
                    // Check if the permission already exists
                    int count = await conn.ExecuteScalarAsync<int>(sqlCheckExist, new { userId = permission.UserId, pageId = permission.PageId });

                    if (count > 0)
                    {
                        // Update existing permission
                        int rowsAffected = await conn.ExecuteAsync(sqlUpdatePermission, new
                        {
                            pageId = permission.PageId,
                            canRead = permission.CanRead,
                            canWrite = permission.CanWrite,
                            canDelete = permission.CanDelete,
                            canExportExcel = permission.CanExportExcel,
                            isModify = true,
                            userId = permission.UserId
                        });

                        if (rowsAffected > 0)
                        {
                            // Insert into PermissionModifyLogs after successful update
                            var permissionId = await conn.ExecuteScalarAsync<int>(
                                "SELECT Id FROM [Permission] WHERE UserId = @userId AND PageId = @pageId",
                                new { userId = permission.UserId, pageId = permission.PageId });

                            await conn.ExecuteAsync(sqlInsertPermissionLogs, new
                            {
                                permissionId,
                                userId = permission.UserId,
                                modifiedDate = DateTime.UtcNow,
                                modifiedBy = permission.CreatedBy
                            });

                            isSuccess = true;
                        }
                    }
                    else
                    {
                        // Insert new permission
                        int rowsAffected = await conn.ExecuteAsync(sqlInsert, new
                        {
                            userId = permission.UserId,
                            pageId = permission.PageId,
                            canRead = permission.CanRead,
                            canWrite = permission.CanWrite,
                            canDelete = permission.CanDelete,
                            canExportExcel = permission.CanExportExcel,
                            createdDate = permission.CreatedDate,
                            createdBy = permission.CreatedBy,
                            isModify = false,
                        });
                        isSuccess = true;
                    }
                }

                conn.Close();
            }

            return isSuccess;
        }



    }
}
