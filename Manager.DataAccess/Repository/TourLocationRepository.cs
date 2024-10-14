using Dapper;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Manager.DataAccess.Repository
{
    public class TourLocationRepository
    {
        private IConfiguration _configuration;
        string SQL_TOURLOCATION;

        public TourLocationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SQL_TOURLOCATION = _configuration.GetConnectionString("SQL_TOURLOCATION");
        }
        public List<TourLocation> GetListTourLocation()
        {
            List<TourLocation> result = new List<TourLocation>();
            string sqlGetListTourLocation =
                        @"  Select *
                            From TourLocation
                            Order by id desc";
            using (var conn = new SqlConnection(SQL_TOURLOCATION))
            {
                conn.Open();
                result = (List<TourLocation>)conn.Query<TourLocation>(sqlGetListTourLocation, commandTimeout: 30, commandType: System.Data.CommandType.Text);
                conn.Close();
            }
            return result;
        }

        public bool SaveUpsertTourLocation(TourLocation tourLocation)
        {
            string sqlUpsertTourLocation = string.Empty;
            var parameters = new object();
            if (tourLocation.Id == 0)
            {
                // Create
                sqlUpsertTourLocation = @"Insert [TourLocation] ([Name], [Province], [District], [Phone], [Email]) VALUES (@Name, @Province, @District, @Phone, @Email)";
                parameters = new { tourLocation.Name, tourLocation.Province, tourLocation.District, tourLocation.Phone, tourLocation.Email };
            }
            else
            {
                // Edit
                sqlUpsertTourLocation = @"Update [TourLocation] set [Name] = @Name, [Province] = @Province, [District] = @District, [Phone] = @Phone, [Email] = @Email Where Id = @Id";
                parameters = new { tourLocation.Name, tourLocation.Province, tourLocation.District, tourLocation.Id, tourLocation.Phone, tourLocation.Email };
            }

            try
            {
                using (var conn = new SqlConnection(SQL_TOURLOCATION))
                {
                    conn.Open();
                    int rowAffected = conn.Execute(sqlUpsertTourLocation, parameters);
                    conn.Close();
                    if (rowAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public TourLocation GetTourLocationById(int id)
        {
            using (var con = new SqlConnection(SQL_TOURLOCATION))
            {
                List<TourLocation> result = new List<TourLocation>();
                string sql = @"select *
                               from TourLocation
                               where Id = @Id";
                con.Open();
                result = (List<TourLocation>)con.Query<TourLocation>(sql, new
                {
                    Id = id
                }, commandTimeout: 30, commandType: System.Data.CommandType.Text);
                con.Close();

                return result.FirstOrDefault();
            }
        }

        public bool DeleteTourLocationById(int id)
        {
            string sqlDeleteTourLocation = @"Delete TourLocation Where ID = @Id ";
            bool isValid = false;
            try
            {
                using (var con = new SqlConnection(SQL_TOURLOCATION))
                {
                    con.Open();
                    int x = con.Execute(sqlDeleteTourLocation, new
                    {
                        Id = id
                    }, commandTimeout: 30);

                    if (x > 0)
                    {
                        return true;
                    }
                    else return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
