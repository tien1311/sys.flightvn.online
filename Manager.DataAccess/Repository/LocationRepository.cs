using Dapper;
using Manager.Model.Models.Location;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.DataAccess.Repository
{
    public class LocationRepository
    {
        private readonly string _connectionString;
        public IConfiguration _configuration;

        public LocationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("SQL_EV_HOTEL");
        }

        public List<Province> GetProvinces()
        {
            List<Province> result = new List<Province>();
            string sql = @"select * from Provinces";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                result = (List<Province>)conn.Query<Province>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }

        public List<District> GetDistrictsByProvinceCode(string provinceCode)
        {
            List<District> result = new List<District>();
            var sql = "SELECT * FROM Districts WHERE Province_Code = @ProvinceCode";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                result = (List<District>)conn.Query<District>(sql, new { ProvinceCode = provinceCode }, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }

        public List<Ward> GetWardsByDistrictCode(string districtCode)
        {
            List<Ward> result = new List<Ward>();
            var sql = "SELECT * FROM Wards WHERE District_Code = @DistrictCode";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                result = (List<Ward>)conn.Query<Ward>(sql, new { DistrictCode = districtCode }, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }

        public string GetProvinceNameByCode(string code)
        {
            string name = string.Empty;
            var sql = "SELECT full_name FROM Provinces WHERE code = @code";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                name = conn.QuerySingleOrDefault<string>(sql, new { code });
                conn.Dispose();
            }
            return name;
        }
        public string GetDistrictNameByCode(string code)
        {
            string name = string.Empty;
            var sql = "SELECT full_name FROM Districts WHERE code = @code";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                name = conn.QuerySingleOrDefault<string>(sql, new { code });
                conn.Dispose();
            }
            return name;
        }

        public string GetWardNameByCode(string code)
        {
            string name = string.Empty;
            var sql = "SELECT full_name FROM Wards WHERE code = @code";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                name = conn.QuerySingleOrDefault<string>(sql, new { code });
                conn.Dispose();
            }
            return name;
        }

        public string GetDistrictByCode(string districtCode)
        {
            List<string> result = new List<string>();
            var sql = "SELECT Full_Name FROM Districts WHERE Code = @DistrictCode";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                result = (List<string>)conn.Query<string>(sql, new { DistrictCode = districtCode }, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result.FirstOrDefault();
        }

        public string GetProvinceByCode(string drovinceCode)
        {
            List<string> result = new List<string>();
            var sql = "SELECT Full_Name FROM Provinces WHERE Code = @ProvinceCode";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                result = (List<string>)conn.Query<string>(sql, new { ProvinceCode = drovinceCode }, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result.FirstOrDefault();
        }
    }
}
