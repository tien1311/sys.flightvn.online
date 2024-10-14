using Dapper;
using Manager.Model.Models;
using Manager.Model.Models.Hotel;
using Manager.Model.Models.Location;
//using FluentFTP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
//using RtfPipe.Model;
//using RtfPipe.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
//using System.Web.WebPages;
using TangDuLieu;


namespace Manager.DataAccess.Repository
{
    public class HotelRepository
    {
        private readonly IConfiguration _configuration;
        private string SQL_HOTEL;

        public HotelRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SQL_HOTEL = _configuration.GetConnectionString("SQL_EV_HOTEL");
        }

        public List<ProductsModel> Hotel()
        {
            List<ProductsModel> result = new List<ProductsModel>();
            string sql = @"select ID,Code,Name,IsActive from Product order by ID desc";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                result = (List<ProductsModel>)conn.Query<ProductsModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            return result;
        }
        public ProductsModel EditHotel(int ID)
        {
            ProductsModel result = new ProductsModel();
            List<Manager.Model.Models.Image> ListImg = new List<Manager.Model.Models.Image>();
            string sql = @"select * from Product where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                result = conn.QueryFirst<ProductsModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            if (result != null)
            {
                string sqlImg = @"select * from Image where ProductID = '" + result.ID + "'";
                using (var conn = new SqlConnection(SQL_HOTEL))
                {
                    ListImg = (List<Manager.Model.Models.Image>)conn.Query<Manager.Model.Models.Image>(sqlImg, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
                result.ListImages = ListImg;
            }
            return result;
        }

        public List<Service> GetHotelServices()
        {
            List<Service> lstService = new List<Service>();
            string sql = "SELECT * FROM Service";

            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                lstService = (List<Service>)conn.Query<Service>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            return lstService;
        }

        public bool UpdateProductHotelService(int ID, List<HotelService> listProductHotelService)
        {
            bool isSuccess = false;
            string deleteSql = @"DELETE FROM ProductService WHERE HotelId = @ID";
            string insertSql = @"INSERT INTO ProductService (ServiceId, HotelId, HotelCode) VALUES (@ServiceId, @HotelId, @HotelCode)";
            int result = 0;

            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Xóa các dịch vụ hiện có
                        conn.Execute(deleteSql, new { ID }, transaction: transaction, commandTimeout: 30, commandType: CommandType.Text);

                        // Chèn các dịch vụ mới
                        for (int i = 0; i < listProductHotelService.Count; i++)
                        {
                            result += conn.Execute(insertSql, new
                            {
                                listProductHotelService[i].ServiceId,
                                HotelId = ID,
                                listProductHotelService[i].HotelCode
                            }, transaction: transaction, commandTimeout: 30, commandType: CommandType.Text);
                        }

                        transaction.Commit();
                        isSuccess = result == listProductHotelService.Count;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return isSuccess;
        }

        public Service GetHotelServicesById(int Id)
        {
            Service service = new Service();
            string sql = "SELECT * FROM Service WHERE ID = @Id";

            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                service = (Service)conn.QuerySingleOrDefault<Service>(sql, new { Id }, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            return service;
        }

        public List<Service> GetListProductHotelServicesById(int Id) // dùng cho mục đích tạo selectList
        {
            List<Service> listHotelService = new List<Service>();
            string sql = @"select s.Id, s.ServiceName 
                            from ProductService ps 
                            JOIN Service s ON s.Id = ps.ServiceId 
                            WHERE HotelId = @Id";

            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                listHotelService = conn.Query<Service>(sql, new { Id }, commandType: CommandType.Text, commandTimeout: 30).ToList();
                conn.Close();
            }
            return listHotelService;
        }

        public List<HotelService> GetListProductHotelService(int Id)
        {
            List<HotelService> listHotelService = new List<HotelService>();
            string sql = @"select ps.Id, ps.ServiceId, ps.HotelId, ps.HotelCode 
                            from ProductService ps 
                            WHERE HotelId = @Id";

            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                listHotelService = conn.Query<HotelService>(sql, new { Id }, commandType: CommandType.Text, commandTimeout: 30).ToList();
                conn.Close();
            }
            return listHotelService;
        }


        public bool DeleteHotelServicesById(int Id)
        {
            bool isSuccess = false;
            ImageInfo imageInfo = null;

            string selectSql = "SELECT Image as Url FROM [Service] WHERE ID = @Id";
            string deleteSql = "DELETE FROM [Service] WHERE ID = @Id";

            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                try
                {
                    // Lấy thông tin hình ảnh từ cơ sở dữ liệu
                    imageInfo = conn.QuerySingleOrDefault<ImageInfo>(selectSql, new { Id }, transaction);

                    // Xóa bản ghi khỏi cơ sở dữ liệu
                    int result = conn.Execute(deleteSql, new { Id }, transaction, commandType: CommandType.Text, commandTimeout: 30);

                    if (result > 0)
                    {
                        // Nếu xóa bản ghi thành công, xóa hình ảnh từ FTP
                        if (imageInfo != null && !string.IsNullOrEmpty(imageInfo.Url))
                        {
                            bool imageDeleted = Manager.Common.Helpers.Common.DeleteImg(imageInfo.Url);

                            if (imageDeleted)
                            {
                                transaction.Commit();
                                isSuccess = true;
                            }
                            else
                            {
                                transaction.Rollback();
                                isSuccess = false;
                            }
                        }
                        else
                        {
                            transaction.Commit();
                            isSuccess = true;
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    conn.Close();
                }
            }
            return isSuccess;
        }

        public bool SaveHotelService(string serviceName, string imgUrl)
        {
            bool isSuccess = false;
            int result = 0;
            string sql = @"INSERT INTO [Service] ([ServiceName],[Image]) 
                                        VALUES (@serviceName , @imgUrl)";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                result = conn.Execute(sql, new { serviceName, imgUrl }, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }

            if (result > 0)
            {
                isSuccess = true;
            }
            return isSuccess;
        }

        public bool SaveEditHotelService(int id, string serviceName, string imgUrl)
        {
            bool isSuccess = false;
            int result = 0;
            string sql = @"UPDATE Service
                            SET ServiceName = @serviceName,
	                            [Image] = @imgUrl
	                            WHERE Id = @id";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                result = conn.Execute(sql, new { serviceName, imgUrl, id }, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }

            if (result > 0)
            {
                isSuccess = true;
            }
            return isSuccess;
        }

        public List<Province> GetListProvinceHasHotel()
        {
            List<Province> listProvince = new List<Province>();
            string sql = "SELECT DISTINCT " +
                        "pro.code as Code, " +
                        "pro.full_name as Full_Name " +
                        "FROM Product p " +
                        "JOIN provinces pro ON pro.code = p.Province";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                listProvince = conn.Query<Province>(sql).ToList();
                conn.Close();
            }
            return listProvince;
        }


        public (List<HotelBooking>,int) GetListHotelBookingV2(HotelBooking request, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize)
        {
            List<HotelBooking> listBooking = new List<HotelBooking>();
            int totalRecords = 0;
            var parameters = new DynamicParameters();

            using (var connection = new SqlConnection(SQL_HOTEL))
            {
                connection.Open();
                var query = new StringBuilder(@"
                        SELECT * FROM (
                            SELECT ROW_NUMBER() OVER (ORDER BY b.Id DESC) AS RowNum, 
                                b.*, 
                                SUM(bd.SoLuong) AS TotalRoom
                            FROM Booking b
                            LEFT JOIN BookingDetail bd ON bd.BookingCode = b.BookingCode
                            WHERE 1 = 1");

                var queryCount = new StringBuilder(@"
                        SELECT COUNT(*) FROM Booking WHERE 1=1");

                if (!string.IsNullOrEmpty(request.BookingCode))
                {
                    query.Append(" AND b.BookingCode LIKE @BookingCode");
                    queryCount.Append(" AND BookingCode LIKE @BookingCode");
                }

                if (!string.IsNullOrEmpty(request.FullName))
                {
                    query.Append(" AND b.FullName LIKE @FullName");
                    queryCount.Append(" AND FullName LIKE @FullName");
                }

                if (!string.IsNullOrEmpty(request.Email))
                {
                    query.Append(" AND b.Email LIKE @Email");
                    queryCount.Append(" AND Email LIKE @Email");
                }

                if (!string.IsNullOrEmpty(request.Phone))
                {
                    query.Append(" AND b.Phone LIKE @Phone");
                    queryCount.Append(" AND Phone LIKE @Phone");
                }

                if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    query.Append(" AND b.CreatedDate BETWEEN @StartDate AND @EndDate");
                    queryCount.Append(" AND CreatedDate BETWEEN @StartDate AND @EndDate");
                }
                else if (fromDate != DateTime.MinValue && toDate == DateTime.MinValue)
                {
                    query.Append(" AND b.CreatedDate >= @StartDate");
                    queryCount.Append(" AND CreatedDate >= @StartDate");

                }
                else if (fromDate == DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    query.Append(" AND b.CreatedDate <= @EndDate");
                    queryCount.Append(" AND CreatedDate <= @EndDate");
                }


                if (request.StatusID != 0)
                {
                    query.Append(" AND b.StatusID = @StatusID");
                    queryCount.Append(" AND StatusID = @StatusID");
                    parameters.Add("StatusID", request.StatusID);
                }

                query.Append(@"
                            GROUP BY b.Id, b.BookingCode, b.HotelCode, b.FullName, b.Phone, b.Email, 
                                     b.Nationality, b.OtherRequestFromCustomer, b.CheckinDate, 
                                     b.CheckoutDate, b.CreatedDate, b.Adults, b.Childs, b.Babies, 
                                     b.TotalRooms, b.SoTien, b.Commission, b.VAT, b.TotalPrice, b.AmountPaid, 
                                     b.PaymentMethod, b.PaymentId, b.PaymentStatus, b.PaymentAgentCode, 
                                     b.Canceller, b.CancelReason, b.OtherFee, b.OtherFeeReason, b.DateGiuCho, 
                                     b.IsVAT, b.Reciever, b.StatusID, b.Note, b.TypeID, b.Price, b.Address, b.Sex
                            ) AS Booking
                            WHERE RowNum BETWEEN ((@PageNumber - 1) * @PageSize + 1) AND (@PageNumber * @PageSize)
                            ORDER BY RowNum");

                // Tạo đối tượng tham số
                parameters.Add("BookingCode", !string.IsNullOrEmpty(request.BookingCode) ? $"%{request.BookingCode}%" : string.Empty);
                parameters.Add("FullName", !string.IsNullOrEmpty(request.FullName) ? $"%{request.FullName}%" : string.Empty);
                parameters.Add("Email", !string.IsNullOrEmpty(request.Email) ? $"%{request.Email}%" : string.Empty);
                parameters.Add("Phone", !string.IsNullOrEmpty(request.Phone) ? $"%{request.Phone}%" : string.Empty);
                parameters.Add("StartDate", fromDate);
                parameters.Add("EndDate", toDate);
                parameters.Add("PageNumber", pageNumber);
                parameters.Add("PageSize", pageSize);

                listBooking = connection.Query<HotelBooking>(query.ToString(), parameters).ToList();
                totalRecords = connection.QuerySingle<int>(queryCount.ToString(), parameters);
                connection.Close();
            }
            return (listBooking,totalRecords);
        }


        public List<HotelBooking> GetListHotelBooking(HotelBooking request, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize)
        {
            List<HotelBooking> listBooking = new List<HotelBooking>();
            using (var connection = new SqlConnection(SQL_HOTEL))
            {
                connection.Open();
                var query = new StringBuilder(@"
                        SELECT * FROM (
                            SELECT ROW_NUMBER() OVER (ORDER BY b.Id DESC) AS RowNum, 
                                b.*, 
                                SUM(bd.SoLuong) AS TotalRoom
                            FROM Booking b
                            LEFT JOIN BookingDetail bd ON bd.BookingCode = b.BookingCode
                            WHERE 1 = 1");

                if (!string.IsNullOrEmpty(request.BookingCode))
                {
                    query.Append(" AND b.BookingCode LIKE @BookingCode");
                }

                if (!string.IsNullOrEmpty(request.FullName))
                {
                    query.Append(" AND b.FullName LIKE @FullName");
                }

                if (!string.IsNullOrEmpty(request.Email))
                {
                    query.Append(" AND b.Email LIKE @Email");
                }

                if (!string.IsNullOrEmpty(request.Phone))
                {
                    query.Append(" AND b.Phone LIKE @Phone");
                }


                if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    query.Append(" AND b.CreatedDate BETWEEN @StartDate AND @EndDate");
                }
                else if (fromDate != DateTime.MinValue && toDate == DateTime.MinValue)
                {
                    query.Append(" AND b.CreatedDate >= @StartDate");
                }
                else if (fromDate == DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    query.Append(" AND b.CreatedDate <= @EndDate");
                }


                if (request.StatusID != 999)
                {
                    query.Append(" AND b.StatusID = @StatusID");
                }

                query.Append(@"
                            GROUP BY b.Id, b.BookingCode, b.HotelCode, b.FullName, b.Phone, b.Email, 
                                     b.Nationality, b.OtherRequestFromCustomer, b.CheckinDate, 
                                     b.CheckoutDate, b.CreatedDate, b.Adults, b.Childs, b.Babies, 
                                     b.TotalRooms, b.SoTien, b.Commission, b.VAT, b.TotalPrice, b.AmountPaid, 
                                     b.PaymentMethod, b.PaymentId, b.PaymentStatus, b.PaymentAgentCode, 
                                     b.Canceller, b.CancelReason, b.OtherFee, b.OtherFeeReason, b.DateGiuCho, 
                                     b.IsVAT, b.Reciever, b.StatusID, b.Note, b.TypeID, b.Price, b.Address, b.Sex
                            ) AS Booking
                            WHERE RowNum BETWEEN ((@PageNumber - 1) * @PageSize + 1) AND (@PageNumber * @PageSize)
                            ORDER BY RowNum");

                // Tạo đối tượng tham số
                var parameters = new DynamicParameters();
                parameters.Add("BookingCode", !string.IsNullOrEmpty(request.BookingCode) ? $"%{request.BookingCode}%" : string.Empty);
                parameters.Add("FullName", !string.IsNullOrEmpty(request.FullName) ? $"%{request.FullName}%" : string.Empty);
                parameters.Add("Email", !string.IsNullOrEmpty(request.Email) ? $"%{request.Email}%" : string.Empty);
                parameters.Add("Phone", !string.IsNullOrEmpty(request.Phone) ? $"%{request.Phone}%" : string.Empty);
                parameters.Add("StartDate", fromDate);
                parameters.Add("EndDate", toDate);
                parameters.Add("StatusID", request.StatusID);
                parameters.Add("PageNumber", pageNumber);
                parameters.Add("PageSize", pageSize);

                listBooking = connection.Query<HotelBooking>(query.ToString(), parameters).ToList();
                connection.Close();
            }
            return listBooking;
        }

        public int GetTotalHotelBooking(HotelBooking request, DateTime fromDate, DateTime toDate)
        {
            int totalRecords = 0;
            using (var connection = new SqlConnection(SQL_HOTEL))
            {
                connection.Open();
                var query = new StringBuilder(@"
                        SELECT COUNT(*) FROM Booking WHERE 1=1");

                if (!string.IsNullOrEmpty(request.BookingCode))
                {
                    query.Append(" AND BookingCode LIKE @BookingCode");
                }

                if (!string.IsNullOrEmpty(request.FullName))
                {
                    query.Append(" AND FullName LIKE @FullName");
                }


                if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate BETWEEN @StartDate AND @EndDate");
                }
                else if (fromDate != DateTime.MinValue && toDate == DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate >= @StartDate");
                }
                else if (fromDate == DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate <= @EndDate");
                }


                if (!string.IsNullOrEmpty(request.Email))
                {
                    query.Append(" AND Email LIKE @Email");
                }

                if (!string.IsNullOrEmpty(request.Phone))
                {
                    query.Append(" AND Phone LIKE @Phone");
                }

                if (request.StatusID != 999)
                {
                    query.Append(" AND StatusID = @StatusID");
                }

                // Tạo đối tượng tham số
                var parameters = new DynamicParameters();
                parameters.Add("BookingCode", !string.IsNullOrEmpty(request.BookingCode) ? $"%{request.BookingCode}%" : string.Empty);
                parameters.Add("FullName", !string.IsNullOrEmpty(request.FullName) ? $"%{request.FullName}%" : string.Empty);
                parameters.Add("Email", !string.IsNullOrEmpty(request.Email) ? $"%{request.Email}%" : string.Empty);
                parameters.Add("Phone", !string.IsNullOrEmpty(request.Phone) ? $"%{request.Phone}%" : string.Empty);
                parameters.Add("StatusID", request.StatusID);
                parameters.Add("StartDate", fromDate);
                parameters.Add("EndDate", toDate);
                totalRecords = connection.QuerySingle<int>(query.ToString(), parameters);
                connection.Close();
            }
            return totalRecords;
        }

        public (List<ProductsModel>,int) GetListHotelV2(List<string> hotelCodes, List<string> hotelNames, string province, int isActived, int pageNumber, int pageSize)
        {
            List<ProductsModel> listProduct = new List<ProductsModel>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(SQL_HOTEL))
            {
                connection.Open();
                var query = new StringBuilder(@"SELECT * FROM 
                                                    (SELECT ROW_NUMBER() OVER (ORDER BY p.CreatedDate DESC) AS RowNum, 
                                                        p.*, i.ImageURL AS MainImageURL 
                                                    FROM PRODUCT p 
                                                    LEFT JOIN Image i ON p.ID = i.ProductID AND i.MainImage = 1 
                                                    WHERE 1=1");
                var queryCount = new StringBuilder("SELECT COUNT(*) FROM PRODUCT WHERE 1=1");

                if (hotelCodes.Any())
                {
                    query.Append(" AND (");
                    for (int i = 0; i < hotelCodes.Count; i++)
                    {
                        if (i > 0)
                        {
                            query.Append(" OR ");
                        }
                        query.Append($"Code LIKE @HotelCodes{i}");
                    }
                    query.Append(")");

                    queryCount.Append(" AND (");
                    for (int i = 0; i < hotelCodes.Count; i++)
                    {
                        if (i > 0)
                        {
                            queryCount.Append(" OR ");
                        }
                        queryCount.Append($"Code LIKE @HotelCodes{i}");
                    }
                    queryCount.Append(")");

                    //query.Append(" AND Code IN @HotelCodes");
                    //queryCount.Append(" AND Code IN @HotelCodes");
                }

                if (hotelNames.Any())
                {
                    query.Append(" AND (");
                    for (int i = 0; i < hotelNames.Count; i++)
                    {
                        if (i > 0)
                        {
                            query.Append(" OR ");
                        }
                        query.Append($"Name LIKE @HotelName{i}");
                    }
                    query.Append(")");

                    queryCount.Append(" AND (");
                    for (int i = 0; i < hotelNames.Count; i++)
                    {
                        if (i > 0)
                        {
                            queryCount.Append(" OR ");
                        }
                        queryCount.Append($"Name LIKE @HotelName{i}");
                    }
                    queryCount.Append(")");

                }

                if (!string.IsNullOrEmpty(province))
                {
                    query.Append(" AND Province = @Province");
                    queryCount.Append(" AND Province = @Province");

                }

                if (isActived != 999)
                {
                    query.Append(" AND isActive = @isActived");
                    queryCount.Append(" AND isActive = @isActived");
                }

                query.Append(") AS PRODUCT WHERE RowNum BETWEEN ((@PageNumber-1)*@PageSize+1) AND (@PageNumber*@PageSize) ORDER BY CreatedDate DESC");
                // Tạo đối tượng tham số
                var parameters = new DynamicParameters();
                //parameters.Add("HotelCodes", hotelCodes);

                for (int i = 0; i < hotelCodes.Count; i++)
                {
                    parameters.Add($"HotelCodes{i}", "%" + hotelCodes[i] + "%");
                }

                for (int i = 0; i < hotelNames.Count; i++)
                {
                    parameters.Add($"HotelName{i}", "%" + hotelNames[i] + "%");
                }
                parameters.Add("Province", province);
                parameters.Add("isActived", isActived);
                parameters.Add("PageNumber", pageNumber);
                parameters.Add("PageSize", pageSize);

                listProduct = connection.Query<ProductsModel>(query.ToString(), parameters).ToList();
                totalRecords = connection.QuerySingle<int>(queryCount.ToString(), parameters);
                connection.Close();
            }

            return (listProduct,totalRecords);
        }

        public List<ProductsModel> GetListHotel(List<string> hotelCodes, List<string> hotelNames, string province, int isActived, int pageNumber, int pageSize)
        {
            List<ProductsModel> listProduct = new List<ProductsModel>();
            using (var connection = new SqlConnection(SQL_HOTEL))
            {
                connection.Open();
                var query = new StringBuilder(@"SELECT * FROM 
                                                    (SELECT ROW_NUMBER() OVER (ORDER BY p.CreatedDate DESC) AS RowNum, 
                                                        p.*, i.ImageURL AS MainImageURL 
                                                    FROM PRODUCT p 
                                                    LEFT JOIN Image i ON p.ID = i.ProductID AND i.MainImage = 1 
                                                    WHERE 1=1");

                if (hotelCodes.Any())
                {
                    query.Append(" AND Code IN @HotelCodes");
                }

                if (hotelNames.Any())
                {
                    query.Append(" AND (");
                    for (int i = 0; i < hotelNames.Count; i++)
                    {
                        if (i > 0)
                        {
                            query.Append(" OR ");
                        }
                        query.Append($"Name LIKE @HotelName{i}");
                    }
                    query.Append(")");
                }

                if (!string.IsNullOrEmpty(province))
                {
                    query.Append(" AND Province = @Province");
                }

                if (isActived != 999)
                {
                    query.Append(" AND isActive = @isActived");
                }

                query.Append(") AS PRODUCT WHERE RowNum BETWEEN ((@PageNumber-1)*@PageSize+1) AND (@PageNumber*@PageSize) ORDER BY CreatedDate DESC");
                // Tạo đối tượng tham số
                var parameters = new DynamicParameters();
                parameters.Add("HotelCodes", hotelCodes);
                for (int i = 0; i < hotelNames.Count; i++)
                {
                    parameters.Add($"HotelName{i}", "%" + hotelNames[i] + "%");
                }
                parameters.Add("Province", province);
                parameters.Add("isActived", isActived);
                parameters.Add("PageNumber", pageNumber);
                parameters.Add("PageSize", pageSize);

                listProduct = connection.Query<ProductsModel>(query.ToString(), parameters).ToList();
                connection.Close();
            }
            return listProduct;
        }

        public int GetTotalHotel(List<string> hotelCodes, List<string> hotelNames, string province, int isActived)
        {
            int totalRecords = 0;
            using (var connection = new SqlConnection(SQL_HOTEL))
            {
                connection.Open();
                var query = new StringBuilder("SELECT COUNT(*) FROM PRODUCT WHERE 1=1");

                if (hotelCodes.Any())
                {
                    query.Append(" AND Code IN @HotelCodes");
                }

                if (hotelNames.Any())
                {
                    query.Append(" AND (");
                    for (int i = 0; i < hotelNames.Count; i++)
                    {
                        if (i > 0)
                        {
                            query.Append(" OR ");
                        }
                        query.Append($"Name LIKE @HotelName{i}");
                    }
                    query.Append(")");
                }

                if (!string.IsNullOrEmpty(province))
                {
                    query.Append(" AND Province = @Province");
                }

                if (isActived != 999)
                {
                    query.Append(" AND isActive = @isActived");
                }

                // Tạo đối tượng tham số
                var parameters = new DynamicParameters();
                parameters.Add("HotelCodes", hotelCodes);
                for (int i = 0; i < hotelNames.Count; i++)
                {
                    parameters.Add($"HotelName{i}", "%" + hotelNames[i] + "%");
                }
                parameters.Add("Province", province);
                parameters.Add("isActived", isActived);

                totalRecords = connection.QuerySingle<int>(query.ToString(), parameters);
                connection.Close();
            }
            return totalRecords;
        }

        public ProductsModel GetHotelByID(int ID)
        {
            ProductsModel result;
            string sqlQuery = @"select * from Product where ID = @ID";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                result = conn.QuerySingle<ProductsModel>(sqlQuery, new { ID });
                conn.Close();
            }
            return result;
        }

        public ProductsModel GetAllContentHotel(int ID)
        {
            ProductsModel result = new ProductsModel();
            string sql = @"select * 
                            from Product 
                            where ID = @ID";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                result = conn.QueryFirst<ProductsModel>(sql, new { ID }, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            if (result != null)
            {
                result.ListImages = getListImg(result.ID);
                result.ListProductTypes = getListTypes(result.ID);
                result.ListHotelServices = GetListProductHotelService(result.ID);
            }
            return result;
        }

        public List<ProductsModel> GetAllHotel()
        {
            List<ProductsModel> result = new List<ProductsModel>();
            string sql = @"select p.*, i.ImageURL as MainImageURL 
                            from Product p
                            JOIN Image i ON i.ProductID = p.ID
                            WHERE i.MainImage = 1  
                            Order By p.CreatedDate Desc";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                result = conn.Query<ProductsModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30).ToList();
                conn.Close();
            }
            return result;
        }

        public List<Manager.Model.Models.Image> getListImg(int ProductID)
        {
            List<Manager.Model.Models.Image> listImg = new List<Manager.Model.Models.Image>();
            try
            {
                string sqlImg = @"select ImageURL from Image where ProductID = '" + ProductID + "'";
                using (var conn = new SqlConnection(SQL_HOTEL))
                {
                    listImg = (List<Manager.Model.Models.Image>)conn.Query<Manager.Model.Models.Image>(sqlImg, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
            }
            catch (Exception)
            {
                return listImg;
            }
            return listImg;
        }

        public List<ProductsType> getListTypes(int ProductID)
        {
            List<ProductsType> ListTypes = new List<ProductsType>();
            try
            {
                string sqlType = @"select ID,Name,Price,DiscountPrice,Description, MaxPerson from Type where ProductID = '" + ProductID + "' order by Price desc";
                using (var conn = new SqlConnection(SQL_HOTEL))
                {
                    ListTypes = (List<ProductsType>)conn.Query<ProductsType>(sqlType, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
            }
            catch (Exception)
            {
                return ListTypes;
            }
            return ListTypes;
        }

        public ProductsModel GetHotelByCode(string Code)
        {
            ProductsModel result;
            string sqlQuery = @"select * from Product where Code = @Code";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                result = conn.QuerySingle<ProductsModel>(sqlQuery, new { Code });
                conn.Close();
            }
            return result;
        }

        public bool DeleteHotelByID(int ID)
        {
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Delete from Image table
                        string deleteImagesQuery = "DELETE FROM Image WHERE ProductID = @ID";
                        conn.Execute(deleteImagesQuery, new { ID }, transaction);

                        // Delete from Type table
                        string deleteTypesQuery = "DELETE FROM Type WHERE ProductID = @ID";
                        conn.Execute(deleteTypesQuery, new { ID }, transaction);

                        // Delete from ProductService table
                        string deleteProductHotelServiceQuery = "DELETE FROM ProductService WHERE HotelId = @ID";
                        conn.Execute(deleteProductHotelServiceQuery, new { ID }, transaction);

                        // Delete from Product table
                        string deleteProductQuery = "DELETE FROM Product WHERE ID = @ID";
                        conn.Execute(deleteProductQuery, new { ID }, transaction);

                        // Commit transaction
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction if an error occurs
                        transaction.Rollback();
                        // Log the exception (not implemented here)
                        return false;
                    }
                    finally
                    {
                        transaction.Dispose();
                        conn.Close();
                    }
                }
            }
        }

        public List<ProductsType> GetListRoomTypeHotel(int ID)
        {
            List<ProductsType> result;
            string sqlQuery = @"
                                    select t.ProductID, p.Name as ProductName, t.ID, t.Name, t.MaxPerson, t.Price, t.DiscountPrice, t.Description
                                    from Type t
                                    JOIN Product p ON p.ID = t.ProductID
                                    Where t.ProductID = @ID
                                ";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                result = conn.Query<ProductsType>(sqlQuery, new { ID }).ToList();
                conn.Close();
            }
            return result;
        }

        public bool EditRoomTypeHotel(int[] productId, string[] roomTypeName, int[] roomTypeMaxPerson, double[] roomTypePrice, double[] roomTypeDiscountPrice, string[] roomTypeDescription)
        {
            string sqlDelete = @" DELETE FROM Type
                                 WHERE ProductID = @ProductID;";
            string sqlInsert = @"
                            INSERT INTO [Type] ([ProductID],[Name],[MaxPerson], [Price], [DiscountPrice], [Description]) 
                                        VALUES (@ProductID , @RoomTypeName , @RoomTypeMaxPerson, @RoomTypePrice, @RoomTypeDiscountPrice, @RoomTypeDescription)
                        ";

            int x = 0;
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var result = conn.Execute(sqlDelete, new { ProductID = productId[0] }, transaction, commandTimeout: 30, commandType: CommandType.Text);
                        if (result > 0)
                        {
                            for (int i = 0; i < roomTypeName.Length; i++)
                            {
                                x = conn.Execute(sqlInsert, new
                                {
                                    ProductID = productId[0],
                                    RoomTypeName = roomTypeName[i],
                                    RoomTypeMaxPerson = roomTypeMaxPerson[i],
                                    RoomTypePrice = roomTypePrice[i],
                                    RoomTypeDiscountPrice = roomTypeDiscountPrice[i],
                                    RoomTypeDescription = roomTypeDescription[i],
                                }, transaction, commandTimeout: 30, commandType: CommandType.Text);
                            }
                            if (x > 0)
                            {
                                transaction.Commit();
                                return true;
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

            }
            return false;
        }

        public Task<bool> SaveCreateHotel(ProductsModel data, string MaNV)
        {
            int x = 0, ID = 0;
            string sqlImg = "";
            string sqlRoomType = "";
            string sqlService = "";
            string Code = CodeProduct();
            string sql = @"INSERT INTO [Product] ([Code],[Name], [Email], [Phone], [Flag], [ShortDescription], [LongDescription], [CreatedBy],[CreatedDate], [Address], [Ward], [District], [Province])
                        OUTPUT INSERTED.ID
                        VALUES (@Code, @Name, @Email, @Phone, @Flag, @ShortDescription, @LongDescription, @MaNV, @CreatedDate, @Address, @Ward, @District, @Province)";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        ID = conn.QuerySingle<int>(sql, new
                        {
                            Code,
                            data.Name,
                            data.Email,
                            data.Phone,
                            data.Flag,
                            ShortDescription = data.ShortDescription.Trim(),
                            LongDescription = data.LongDescription.Trim(),
                            MaNV,
                            CreatedDate = DateTime.Now,
                            data.Address,
                            data.Ward,
                            data.District,
                            data.Province,
                        }, transaction, commandTimeout: 30, commandType: CommandType.Text);

                        if (ID > 0)
                        {
                            for (int i = 0; i < data.ListImages.Count; i++)
                            {
                                sqlImg = @"INSERT INTO [Image] ([ProductID],[ImageURL],[MainImage]) 
                                   VALUES (@ID , @ImageUrl , @MainImage)";
                                x = conn.Execute(sqlImg, new
                                {
                                    ID,
                                    ImageUrl = data.ListImages[i].ImageURL,
                                    data.ListImages[i].MainImage
                                }, transaction, commandTimeout: 30, commandType: CommandType.Text);
                            }
                            for (int i = 0; i < data.ListProductTypes.Count; i++)
                            {
                                sqlRoomType = @"INSERT INTO [Type] ([ProductID],[Name],[MaxPerson], [Price], [DiscountPrice], [Description]) 
                                                    VALUES (@ID , @RoomTypeName , @RoomTypeMaxPerson, @RoomTypePrice, @RoomTypeDiscountPrice, @RoomTypeDescription)";
                                x = conn.Execute(sqlRoomType, new
                                {
                                    ID,
                                    RoomTypeName = data.ListProductTypes[i].Name,
                                    RoomTypeMaxPerson = data.ListProductTypes[i].MaxPerson,
                                    RoomTypePrice = data.ListProductTypes[i].Price,
                                    RoomTypeDiscountPrice = data.ListProductTypes[i].DiscountPrice,
                                    RoomTypeDescription = data.ListProductTypes[i].Description,
                                }, transaction, commandTimeout: 30, commandType: CommandType.Text);
                            }
                            for (int i = 0; i < data.ListHotelServices.Count; i++)
                            {
                                sqlService = @"INSERT INTO [ProductService] ([ServiceId],[HotelId],[HotelCode]) 
                                                    VALUES ( @ServiceId , @ID, @Code)";
                                x = conn.Execute(sqlService, new
                                {
                                    data.ListHotelServices[i].ServiceId,
                                    ID,
                                    Code
                                }, transaction, commandTimeout: 30, commandType: CommandType.Text);
                            }
                            if (x > 0)
                            {
                                transaction.Commit();
                                return Task.FromResult(true);
                            }
                        }
                        transaction.Commit();
                        return Task.FromResult(true);
                    }
                    catch
                    {
                        transaction.Rollback();
                        return Task.FromResult(false);
                    }
                }
            }
        }


        public Task<bool> SaveUndoHotel(ProductsModel data, string MaNV)
        {
            int x = 0, ID = 0;
            string sqlImg = "";
            string sqlRoomType = "";
            string sqlService = "";
            //string Code = CodeProduct();
            string sql = @"INSERT INTO [Product] ([Code], [Name], [Email], [Phone], [ShortDescription], [LongDescription], [CreatedBy],[CreatedDate], [Address], [Ward], [District], [Province])
                        OUTPUT INSERTED.ID
                        VALUES (@Code, @Name, @Email, @Phone, @ShortDescription, @LongDescription, @MaNV, @CreatedDate, @Address, @Ward, @District, @Province)";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        ID = conn.QuerySingle<int>(sql, new
                        {
                            data.Code,
                            data.Name,
                            data.Email,
                            data.Phone,
                            ShortDescription = data.ShortDescription.Trim(),
                            LongDescription = data.LongDescription.Trim(),
                            MaNV,
                            data.CreatedDate,
                            data.Address,
                            data.Ward,
                            data.District,
                            data.Province,
                        }, transaction, commandTimeout: 30, commandType: CommandType.Text);

                        if (ID > 0)
                        {
                            for (int i = 0; i < data.ListImages.Count; i++)
                            {
                                sqlImg = @"INSERT INTO [Image] ([ProductID],[ImageURL],[MainImage]) 
                                   VALUES (@ID , @ImageUrl , @MainImage)";
                                x = conn.Execute(sqlImg, new
                                {
                                    ID,
                                    ImageUrl = data.ListImages[i].ImageURL,
                                    data.ListImages[i].MainImage
                                }, transaction, commandTimeout: 30, commandType: CommandType.Text);
                            }
                            for (int i = 0; i < data.ListProductTypes.Count; i++)
                            {
                                sqlRoomType = @"INSERT INTO [Type] ([ProductID],[Name],[MaxPerson], [Price], [DiscountPrice], [Description]) 
                                                    VALUES (@ID , @RoomTypeName , @RoomTypeMaxPerson, @RoomTypePrice, @RoomTypeDiscountPrice, @RoomTypeDescription)";
                                x = conn.Execute(sqlRoomType, new
                                {
                                    ID,
                                    RoomTypeName = data.ListProductTypes[i].Name,
                                    RoomTypeMaxPerson = data.ListProductTypes[i].MaxPerson,
                                    RoomTypePrice = data.ListProductTypes[i].Price,
                                    RoomTypeDiscountPrice = data.ListProductTypes[i].DiscountPrice,
                                    RoomTypeDescription = data.ListProductTypes[i].Description,
                                }, transaction, commandTimeout: 30, commandType: CommandType.Text);
                            }
                            for (int i = 0; i < data.ListHotelServices.Count; i++)
                            {
                                sqlService = @"INSERT INTO [ProductService] ([ServiceId],[HotelId],[HotelCode]) 
                                                    VALUES ( @ServiceId , @ID, @Code)";
                                x = conn.Execute(sqlService, new
                                {
                                    data.ListHotelServices[i].ServiceId,
                                    ID,
                                    data.Code
                                }, transaction, commandTimeout: 30, commandType: CommandType.Text);
                            }
                            if (x > 0)
                            {
                                transaction.Commit();
                                return Task.FromResult(true);
                            }
                        }
                        transaction.Commit();
                        return Task.FromResult(true);
                    }
                    catch
                    {
                        transaction.Rollback();
                        return Task.FromResult(false);
                    }
                }
            }
        }


        public async Task<bool> SaveEditHotel(ProductsModel data)
        {
            int x = 0, z = 0;
            string sqlDelImg = @"DELETE FROM Image WHERE ProductID = @ID";
            string sqlUpdateProduct = @"UPDATE [Product] 
                                SET [Name] = @Name, [Email] = @Email, [Phone] = @Phone, [Flag] = @Flag , [ShortDescription] = @ShortDescription, [LongDescription] = @LongDescription, 
                                    [Address] = @Address, [Ward] = @Ward, [District] = @District, [Province] = @Province 
                                WHERE ID = @ID";
            string sqlInsertImg = @"INSERT INTO [Image] ([ProductID],[ImageURL],[MainImage]) 
                            VALUES (@ID, @ImageUrl, @MainImage)";

            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                await conn.OpenAsync();

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Xóa ảnh cũ
                        await conn.ExecuteAsync(sqlDelImg, new { data.ID }, transaction, commandTimeout: 30, commandType: CommandType.Text);

                        // Cập nhật thông tin sản phẩm
                        z = await conn.ExecuteAsync(sqlUpdateProduct, new
                        {
                            data.Name,
                            data.Email,
                            data.Phone,
                            data.Flag,
                            ShortDescription = data.ShortDescription.Trim(),
                            LongDescription = data.LongDescription.Trim(),
                            data.Address,
                            data.Ward,
                            data.District,
                            data.Province,
                            data.ID
                        }, transaction, commandTimeout: 30, commandType: CommandType.Text);

                        // Thêm ảnh mới
                        if (z > 0)
                        {
                            for (int i = 0; i < data.ListImages.Count; i++)
                            {
                                x = await conn.ExecuteAsync(sqlInsertImg, new
                                {
                                    data.ID,
                                    ImageUrl = data.ListImages[i].ImageURL,
                                    data.ListImages[i].MainImage
                                }, transaction, commandTimeout: 30, commandType: CommandType.Text);

                                if (x <= 0)
                                {
                                    throw new Exception("Error inserting images.");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Error updating product.");
                        }

                        // Commit transaction
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        // Rollback transaction
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public bool ChangeActiveHotelByCode(string Code, bool isActive)
        {
            int x = 0;
            string sql = @"UPDATE [Product] SET [IsActive] = @isActive where Code = @Code ";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                x = conn.Execute(sql, new { isActive, Code }, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<ProductsType> TypeHotel()
        {
            List<ProductsType> result = new List<ProductsType>();
            string ProductName = "";
            string sql = @"select ID,Name,Price,DiscountPrice,ProductID from Type order by ID desc";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                result = (List<ProductsType>)conn.Query<ProductsType>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            if (result.Count > 0)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    string sqlProduct = @"select Name from Product where ID = '" + result[i].ProductID + "'";
                    using (var conn = new SqlConnection(SQL_HOTEL))
                    {
                        ProductName = conn.QueryFirst<string>(sqlProduct, null, commandType: CommandType.Text, commandTimeout: 30);
                        conn.Close();
                    }
                    result[i].ProductName = ProductName;
                }
            }
            return result;
        }
        public bool SaveCreateType(ProductsType data)
        {
            int x = 0;
            string sqlType = @"INSERT INTO [Type] ([ProductID],[Name],[Price],[DiscountPrice],[Description])
                                                VALUES ('" + data.ProductID + "',N'" + data.Name + "','" + data.Price + "','" + data.DiscountPrice + "',N'" + data.Description + "')";

            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                x = conn.Execute(sqlType, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ProductsType EditTypeHotel(int ID)
        {
            ProductsType result = new ProductsType();
            string sql = @"select * from Type where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                result = conn.QueryFirst<ProductsType>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            return result;
        }
        public bool SaveEditType(ProductsType data)
        {
            int x = 0;
            string sqlType = @"UPDATE [Type] SET [ProductID] = '" + data.ProductID + "',[Name] = N'" + data.Name + "',[Price] = '" + data.Price + "',[DiscountPrice] = '" + data.DiscountPrice + "',[Description] = N'" + data.Description + "' where ID = '" + data.ID + "'";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                x = conn.Execute(sqlType, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeleteImg(int ID)
        {
            string imageUrl = null;
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Lấy URL của hình ảnh từ cơ sở dữ liệu
                        string selectImageQuery = "SELECT ImageUrl FROM Image WHERE ID = @ID";
                        imageUrl = conn.QuerySingleOrDefault<string>(selectImageQuery, new { ID }, transaction);

                        // Xóa bản ghi hình ảnh khỏi cơ sở dữ liệu
                        string deleteImageQuery = "DELETE FROM Image WHERE ID = @ID";
                        int result = conn.Execute(deleteImageQuery, new { ID }, transaction);

                        // Xóa hình ảnh khỏi FTP nếu có URL
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            bool imageDeleted = Manager.Common.Helpers.Common.DeleteImg(imageUrl);

                            if (!imageDeleted)
                            {
                                // Nếu xóa hình ảnh không thành công, rollback giao dịch
                                transaction.Rollback();
                                return false;
                            }
                        }

                        if (result > 0)
                        {
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            // Nếu không xóa được bản ghi, rollback giao dịch
                            transaction.Rollback();
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction if an error occurs
                        transaction.Rollback();
                        // Log the exception (not implemented here)
                        return false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
        public string CodeProduct()
        {
            string date = DateTime.Now.ToString("ddMMyy");
            string code = "SPHT" + DateTime.Now.ToString("ddMMyy");
            string MSP = "";
            try
            {
                string sql = @"select top 1 code from Product where code like '%" + date + "%' order by ID desc";
                using (var conn = new SqlConnection(SQL_HOTEL))
                {
                    MSP = conn.QueryFirst<string>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
                if (MSP != null)
                {
                    int stt = int.Parse(MSP.Substring(10, 3));
                    if (stt < 9)
                    {
                        stt++;
                        code += "00" + stt;
                    }
                    else
                    {
                        if (stt < 99)
                        {
                            stt++;
                            code += "0" + stt;
                        }
                        else
                        {
                            stt++;
                            code += stt;
                        }
                    }
                }
                else
                {
                    code += "001";
                }
            }
            catch (Exception)
            {
                code += "001";
            }

            return code;
        }
        public List<HotelBooking> GetAllHotelBookingToday()
        {
            List<HotelBooking> list = new List<HotelBooking>();
            try
            {
                string sql = @"
                            SELECT b.*, SUM(bd.SoLuong) AS TotalRoom
                            FROM Booking b
                            JOIN BookingDetail bd ON bd.BookingCode = b.BookingCode
                            WHERE CAST(b.CreatedDate AS DATE) = CAST(GETDATE() AS DATE)
                            GROUP BY b.Id, 
                                     b.BookingCode, 
                                     b.TypeID,
                                     b.Price,
                                     b.Sex,
                                     b.Address,
                                     b.HotelCode, 
                                     b.FullName, 
                                     b.Phone, 
                                     b.Email, 
                                     b.Nationality, 
                                     b.OtherRequestFromCustomer, 
                                     b.CheckinDate, 
                                     b.CheckoutDate, 
                                     b.CreatedDate, 
                                     b.Adults, 
                                     b.Childs, 
                                     b.Babies, 
                                     b.TotalRooms, 
                                     b.SoTien, 
                                     b.Commission, 
                                     b.VAT, 
                                     b.OtherFee, 
                                     b.TotalPrice, 
                                     b.AmountPaid, 
                                     b.PaymentMethod, 
                                     b.PaymentId, 
                                     b.PaymentStatus, 
                                     b.PaymentAgentCode, 
                                     b.DateGiuCho, 
                                     b.Canceller, 
                                     b.CancelReason, 
                                     b.OtherFeeReason, 
                                     b.IsVAT, 
                                     b.Reciever, 
                                     b.StatusID, 
                                     b.Note
                            ORDER BY b.CreatedDate DESC
                                ";
                using (var conn = new SqlConnection(SQL_HOTEL))
                {
                    conn.Open();
                    list = conn.Query<HotelBooking>(sql, null, commandType: CommandType.Text, commandTimeout: 30).ToList();
                    conn.Close();
                }
            }
            catch (Exception)
            {
                return list;
            }
            return list;
        }

        public string GetStatusStringByStatusId(int statusId)
        {
            string status = string.Empty;
            string sql = @"select Name from Status WHERE ID = @statusId";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                status = conn.ExecuteScalar<string>(sql, new { statusId }, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            return status;
        }

        public List<HotelBookingDetail> GetBookingDetailByBookingCode(string bookingCode)
        {
            List<HotelBookingDetail> hotelBookingDetail = new List<HotelBookingDetail>();
            string sql = @"SELECT * FROM BookingDetail WHERE BookingCode = @bookingCode";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                hotelBookingDetail = conn.Query<HotelBookingDetail>(sql, new { bookingCode }, commandType: CommandType.Text, commandTimeout: 30).ToList();
                conn.Close();
            }
            return hotelBookingDetail;
        }
        public List<HotelBookingStatusDescription> GetBookingStatusByBookingCode(string bookingCode)
        {
            List<HotelBookingStatusDescription> hotelBookingStatusDescription = new List<HotelBookingStatusDescription>();
            string sql = @"SELECT * FROM BookingStatus WHERE BookingCode = @bookingCode";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                hotelBookingStatusDescription = conn.Query<HotelBookingStatusDescription>(sql, new { bookingCode }, commandType: CommandType.Text, commandTimeout: 30).ToList();
                conn.Close();
            }
            return hotelBookingStatusDescription;
        }

        public bool DetailHotelBooking(string bookingCode, string MaPB, string TenNV)
        {
            HotelBooking hotelBooking = new HotelBooking();
            bool isSuccess = false;
            int x = 0;
            try
            {
                string sql = @" SELECT *
                                FROM Booking 
                                WHERE BookingCode = @bookingCode";
                using (var conn = new SqlConnection(SQL_HOTEL))
                {
                    hotelBooking = conn.QueryFirst<HotelBooking>(sql, new { bookingCode }, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
                if (hotelBooking != null)
                {
                    if (string.IsNullOrEmpty(hotelBooking.Reciever))
                    {
                        if (MaPB.Trim() == "DL" || MaPB.Trim() == "IT")
                        {
                            string sqlupdate = @"UPDATE Booking 
                                                SET Reciever = @Reciever, StatusID = 2 
                                                WHERE BookingCode = @BookingCode ";

                            string sqlInsertBookingStatus = @"INSERT INTO [BookingStatus] (BookingId, BookingCode, HotelCode, StatusId, Description, CreatedDate, CreatedBy) 
                                                  VALUES (@bookingId, @bookingCode, @hotelCode, @statusId, @description, @createdDate, @createdBy)";

                            using (var conn = new SqlConnection(SQL_HOTEL))
                            {
                                conn.Open();
                                x = conn.Execute(sqlupdate, new { Reciever = TenNV, BookingCode = bookingCode }, null, commandTimeout: 30, commandType: CommandType.Text);
                                x = conn.Execute(sqlInsertBookingStatus, new
                                {
                                    bookingId = hotelBooking.Id,
                                    bookingCode = hotelBooking.BookingCode,
                                    hotelCode = hotelBooking.HotelCode,
                                    statusId = 2,
                                    description = "Đã nhận booking",
                                    createdDate = DateTime.Now,
                                    createdBy = TenNV
                                }, commandType: CommandType.Text, commandTimeout: 30);

                                conn.Dispose();
                            }
                        }
                    }
                    if (x > 0)
                    {
                        isSuccess = true;
                    }
                }
            }
            catch (Exception)
            {
                isSuccess = false;
                return isSuccess;
            }
            return isSuccess;
        }

        //public HotelBooking GetDetailHotelBookingByBookingCode(string bookingCode)
        //{
        //    HotelBooking hotelBooking = new HotelBooking();
        //    int x = 0;
        //    try
        //    {
        //        string sql = @" SELECT *
        //                        FROM Booking 
        //                        WHERE BookingCode = @bookingCode";
        //        using (var conn = new SqlConnection(SQL_HOTEL))
        //        {
        //            hotelBooking = (HotelBooking)conn.QueryFirst<HotelBooking>(sql, new { bookingCode = bookingCode }, commandType: System.Data.CommandType.Text, commandTimeout: 30);
        //            conn.Close();
        //        }
        //        if (hotelBooking != null)
        //        {
        //            hotelBooking.HotelInfo = GetHotelByCode(hotelBooking.HotelCode);
        //            hotelBooking.BookingDetails = GetBookingDetailByBookingCode(hotelBooking.BookingCode);
        //            hotelBooking.BookingStatus = GetBookingStatusByBookingCode(hotelBooking.BookingCode);
        //            if (hotelBooking.IsVAT == true)
        //            {
        //                hotelBooking.HotelVAT = GetHotelVATByBookingCodeAndHotelCode(hotelBooking.BookingCode, hotelBooking.HotelCode);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return hotelBooking;
        //    }
        //    return hotelBooking;
        //}

        public HotelBooking GetDetailHotelBookingByBookingCode(string bookingCode)
        {
            try
            {
                using (var conn = new SqlConnection(SQL_HOTEL))
                {
                    conn.Open();

                    // Retrieve hotel booking
                    string bookingSql = @"SELECT * FROM Booking WHERE BookingCode = @bookingCode";
                    var hotelBooking = conn.QueryFirstOrDefault<HotelBooking>(bookingSql, new { bookingCode });

                    if (hotelBooking != null)
                    {
                        // Retrieve hotel info
                        string hotelSql = @"SELECT * FROM Product WHERE Code = @Code";
                        hotelBooking.HotelInfo = conn.QueryFirstOrDefault<ProductsModel>(hotelSql, new { Code = hotelBooking.HotelCode });

                        // Retrieve booking details
                        string bookingDetailSql = @"SELECT * FROM BookingDetail WHERE BookingCode = @bookingCode";
                        hotelBooking.BookingDetails = conn.Query<HotelBookingDetail>(bookingDetailSql, new { bookingCode }).ToList();

                        // Retrieve booking status
                        string bookingStatusSql = @"SELECT * FROM BookingStatus WHERE BookingCode = @bookingCode";
                        hotelBooking.BookingStatus = conn.Query<HotelBookingStatusDescription>(bookingStatusSql, new { bookingCode }).ToList();

                        // Retrieve VAT info if applicable
                        if (hotelBooking.IsVAT == true)
                        {
                            string vatSql = "SELECT * FROM VAT WHERE BookingCode = @bookingCode AND HotelCode = @hotelCode";
                            hotelBooking.HotelVAT = conn.QueryFirstOrDefault<HotelVAT>(vatSql, new { bookingCode, hotelCode = hotelBooking.HotelCode });
                        }
                    }

                    conn.Close();
                    return hotelBooking;
                }
            }
            catch (Exception ex)
            {
                // Log exception (optional)
                // Return default or empty HotelBooking object if needed
                return new HotelBooking();
            }
        }


        public List<HotelBookingStatus> GetListHotelBookingStatus()
        {
            List<HotelBookingStatus> listStatus = new List<HotelBookingStatus>();
            string sql = @"SELECT * FROM Status";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                listStatus = conn.Query<HotelBookingStatus>(sql, null, commandType: CommandType.Text, commandTimeout: 30).ToList();
                conn.Dispose();
            }

            return listStatus;
        }

        public string GetHotelVATRates()
        {
            string VATConnectString = _configuration.GetConnectionString("SQL_EV_TOUR");
            string result = "";
            string sqlQuery = "SELECT TIGIA FROM CommissionRates WHERE ID = 1005";
            using (var conn = new SqlConnection(VATConnectString))
            {
                conn.Open();
                result = conn.QueryFirstOrDefault<string>(sqlQuery);
                conn.Close();
            }
            return result;
        }

        public HotelVAT GetHotelVATByBookingCodeAndHotelCode(string bookingCode, string HotelCode)
        {
            HotelVAT hotelVAT = new HotelVAT();
            string sqlQuery = "SELECT * FROM VAT WHERE BookingCode = @bookingCode AND HotelCode = @hotelCode ";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                hotelVAT = conn.QueryFirstOrDefault<HotelVAT>(sqlQuery, new { bookingCode, hotelCode = HotelCode });
                conn.Close();
            }

            return hotelVAT;
        }

        public Task<PostsAdsModel> GetDieuKienKhachSan()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("SQL_POST");
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string query = @"SELECT top 1 * FROM Posts WHERE ID = 8";
                    PostsAdsModel postsAdsModel = db.QueryFirstOrDefault<PostsAdsModel>(query);
                    return Task.FromResult(postsAdsModel);
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Lỗi trong quá trình truy vấn dữ liệu", ex);
            }
        }

        public async Task<bool> ChangeStatus(string bookingCode, int statusId, string statusDescription, string tenNV)
        {
            bool isSuccess = false;
            int rowAffected = 0;
            HotelBooking hotelBooking = GetDetailHotelBookingByBookingCode(bookingCode);

            if (hotelBooking != null)
            {
                string sqlChangeStatus = @"UPDATE Booking 
                                   SET StatusID = @statusId 
                                   WHERE BookingCode = @bookingCode";

                string sqlInsertBookingStatus = @"INSERT INTO [BookingStatus] (BookingId, BookingCode, HotelCode, StatusId, Description, CreatedDate, CreatedBy) 
                                                  VALUES (@bookingId, @bookingCode, @hotelCode, @statusId, @description, @createdDate, @createdBy)";

                using (var conn = new SqlConnection(SQL_HOTEL))
                {
                    conn.Open();
                    rowAffected = await conn.ExecuteAsync(sqlChangeStatus, new { bookingCode, statusId }, commandType: CommandType.Text, commandTimeout: 30);
                    rowAffected = await conn.ExecuteAsync(sqlInsertBookingStatus, new
                    {
                        bookingId = hotelBooking.Id,
                        bookingCode = hotelBooking.BookingCode,
                        hotelCode = hotelBooking.HotelCode,
                        statusId,
                        description = statusDescription,
                        createdDate = DateTime.Now,
                        createdBy = tenNV
                    },
                    commandType: CommandType.Text, commandTimeout: 30);

                    if (statusId == 3)
                    {
                        string sqlUpdateDateGiuCho = @"UPDATE Booking 
                                                       SET DateGiuCho = @dateGiuCho 
                                                        WHERE BookingCode = @bookingCode ";
                        rowAffected = await conn.ExecuteAsync(sqlUpdateDateGiuCho, new { bookingCode, dateGiuCho = DateTime.Now }, commandType: CommandType.Text, commandTimeout: 30);
                    }
                    if (statusId == 7)
                    {
                        string sqlCancel = @"UPDATE Booking 
                                   SET StatusID = 7, 
                                       Canceller = @canceller, 
                                       CancelReason = @cancelReason 
                                   WHERE BookingCode = @bookingCode";

                        hotelBooking.Canceller = tenNV;
                        hotelBooking.CancelReason = statusDescription;
                        rowAffected = await conn.ExecuteAsync(sqlCancel, new { bookingCode, canceller = tenNV, cancelReason = statusDescription }, commandType: CommandType.Text, commandTimeout: 30);
                    }
                    conn.Close();
                }

                if (rowAffected > 0)
                {
                    hotelBooking.StatusID = statusId;
                    isSuccess = true;
                    _ = Task.Run(() => SendMailHotel(hotelBooking));
                }
            }
            return isSuccess;
        }

        public async Task<bool> CancelBooking(string bookingCode, string cancelReason, string canceller)
        {
            bool isSuccess = false;
            int rowAffected = 0;
            HotelBooking hotelBooking = GetDetailHotelBookingByBookingCode(bookingCode);

            if (hotelBooking != null)
            {
                string sql = @"UPDATE Booking 
                                   SET StatusID = 7, 
                                       Canceller = @canceller, 
                                       CancelReason = @cancelReason 
                                   WHERE BookingCode = @bookingCode";
                using (var conn = new SqlConnection(SQL_HOTEL))
                {
                    conn.Open();
                    rowAffected = await conn.ExecuteAsync(sql, new { bookingCode, canceller, cancelReason }, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }

                if (rowAffected > 0)
                {
                    hotelBooking.StatusID = 7;
                    hotelBooking.Canceller = canceller;
                    hotelBooking.CancelReason = cancelReason;
                    isSuccess = true;
                    _ = Task.Run(() => SendMailHotel(hotelBooking));
                }
            }
            return isSuccess;
        }

        public async Task<bool> UpdateOtherFee(string bookingCode, string otherFeeReason, double otherFee)
        {
            bool isSuccess = false;
            int rowAffected = 0;
            string sql = @"UPDATE Booking 
                                   SET OtherFee = @otherFee, 
                                       OtherFeeReason = @otherFeeReason, 
                                       TotalPrice = SoTien - Commission + VAT + @otherFee 
                                   WHERE BookingCode = @bookingCode";
            using (var conn = new SqlConnection(SQL_HOTEL))
            {
                conn.Open();
                rowAffected = await conn.ExecuteAsync(sql, new { bookingCode, otherFeeReason, otherFee }, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }

            if (rowAffected > 0)
            {
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public Task<bool> SendMailHotel(HotelBooking request)
        {
            string programId = string.Empty;
            bool isSuccess = false;
            if (request.StatusID != 7)
            {
                programId = "EVM_CHANGESTATUSHOTEL";
            }
            if (request.StatusID == 7)
            {
                programId = "EVM_CANCELHOTEL";
            }

            string subject = "[HOTEL] " + GetStatusStringByStatusId(request.StatusID).ToUpper();


            EVEmail evEmail = new EVEmail();
            EVMailRepository evMailRepository = new EVMailRepository();
            evEmail = evMailRepository.GetEVEMailContentByProgram(programId);
            var postsAdsModel = GetDieuKienKhachSan().Result;
            if (evEmail != null)
            {
                string contentEmail = "";
                string strSanPham = "";
                string VATPriceContent = "";
                string OtherFeeContent = "";
                string VATContent = "";

                foreach (var roomItem in request.BookingDetails)
                {
                    strSanPham += "<tr>";
                    strSanPham += "<td>" + roomItem.LoaiPhong + "</td>";
                    strSanPham += "<td>" + roomItem.SoLuong + "</td>";
                    strSanPham += "<td>" + Manager.Common.Helpers.Common.FormatNumber(roomItem.SoTien, 0) + " VND </td>";
                    strSanPham += "</tr>";
                }

                if (request.IsVAT == true)
                {
                    VATPriceContent += $@"
                                      <div class='col-sm-6'>
                                                        <table style='width: 100%;''cellspacing='0' cellpadding='7' border='0'>
                                                            <tr>
                                                                <td style='width: 110px;'>VAT ({GetHotelVATRates()}%):</td>
                                                                <td colspan='2'><strong>{Manager.Common.Helpers.Common.FormatNumber(request.VAT, 0)} VND</strong></td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                    ";

                    VATContent += $@"
                    <table style='width: 100%;' cellspacing='0' cellpadding='7' border='0'>
                        <tr style='color:#fff; background-color: #006886; padding:5px 10px; font-size: 15px;'>
                            <td colspan='4' style='width:200px;'>Thông tin xuất hóa đơn</td>
                        </tr>
                    </table>
                    <div style='padding:5px 10px;color:#3f3d33;'>
                        <div class='row'>
                            <div class='col-sm-6'>
                                <table style='width: 100%;' cellspacing='0' cellpadding='7' border='0'>
                                    <tr>
                                        <td style='width: 110px;'>Mã số thuế:</td>
                                        <td colspan='3'><strong>{request.HotelVAT.MaSoThue}</strong></td>
                                    </tr>
                                </table>
                            </div>
                            <div class='col-sm-6'>
                                <table style='width: 100%;' cellspacing='0' cellpadding='7' border='0'>
                                    <tr>
                                        <td style='width: 110px;'>Tên công ty:</td>
                                        <td colspan='3'><strong>{request.HotelVAT.TenCongTy}</strong></td>
                                    </tr>
                                </table>
                            </div>
                            <div class='col-sm-6'>
                                <table style='width: 100%;' cellspacing='0' cellpadding='7' border='0'>
                                    <tr>
                                        <td style='width: 110px;'>Địa chỉ:</td>
                                        <td colspan='3'><strong>{request.HotelVAT.DiaChi}</strong></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>";
                }

                if (request.OtherFee > 0)
                {
                    OtherFeeContent += $@"
                                      <div class='col-sm-6'>
                                                        <table style='width: 100%;''cellspacing='0' cellpadding='7' border='0'>
                                                            <tr>
                                                                <td style='width: 110px;'>Phí khác: </td>
                                                                <td colspan='2'><strong>{Manager.Common.Helpers.Common.FormatNumber(request.OtherFee, 0)} ({request.OtherFeeReason}) VND</strong></td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                    ";
                }


                var webRequest = WebRequest.Create(evEmail.templateUrl);
                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                { contentEmail = reader.ReadToEnd(); }
                contentEmail = contentEmail.Replace("$_Date", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                contentEmail = contentEmail.Replace("$_Fullname", request.FullName);
                contentEmail = contentEmail.Replace("$_evcode", request.BookingCode);
                contentEmail = contentEmail.Replace("$_StatusEnviet", GetStatusStringByStatusId(request.StatusID));
                contentEmail = contentEmail.Replace("$_TenTour", request.HotelInfo.Name); // tên khách sạn
                contentEmail = contentEmail.Replace("$_MaTour", request.HotelInfo.Code); // mã khách sạn
                contentEmail = contentEmail.Replace("$_NgayDi", request.CheckinDate.ToString("dd/MM/yyyy"));
                contentEmail = contentEmail.Replace("$_NgayVe", request.CheckoutDate.ToString("dd/MM/yyyy"));
                contentEmail = contentEmail.Replace("$_adult", request.Adults.ToString());
                contentEmail = contentEmail.Replace("$_child", request.Childs.ToString());
                contentEmail = contentEmail.Replace("$_kid", request.Babies.ToString());
                contentEmail = contentEmail.Replace("{{SanPham}}", strSanPham);
                contentEmail = contentEmail.Replace("$_Email", request.Email);
                contentEmail = contentEmail.Replace("$_Phone", request.Phone);
                contentEmail = contentEmail.Replace("$_BookingNotes", request.OtherRequestFromCustomer);
                contentEmail = contentEmail.Replace("$_Price", Manager.Common.Helpers.Common.FormatNumber(request.SoTien, 0) + " VND");
                contentEmail = contentEmail.Replace("$_Discount", Manager.Common.Helpers.Common.FormatNumber(request.Commission, 0) + " VND");
                contentEmail = contentEmail.Replace("{{VATPriceContent}}", VATPriceContent);
                contentEmail = contentEmail.Replace("{{OtherFeeContent}}", OtherFeeContent);
                contentEmail = contentEmail.Replace("$_Total", Manager.Common.Helpers.Common.FormatNumber(request.TotalPrice, 0) + " VND");
                contentEmail = contentEmail.Replace("{{SanPham}}", strSanPham);
                contentEmail = contentEmail.Replace("{{VATContent}}", VATContent);
                contentEmail = contentEmail.Replace("{{OtherFeeContent}}", OtherFeeContent);
                contentEmail = contentEmail.Replace("$_Condition", postsAdsModel.Description);
                contentEmail = contentEmail.Replace("$_ThisYear", DateTime.Now.Year.ToString());
                if (!string.IsNullOrEmpty(request.CancelReason) && request.StatusID == 7) // Huỷ
                {
                    contentEmail = contentEmail.Replace("$_CancellationReason", request.CancelReason);
                }

                bool isCC = true;
                bool isBCC = true;
                if (!string.IsNullOrEmpty(request.OtherRequestFromCustomer) && request.OtherRequestFromCustomer.Trim() == "ENVIETTESTING")
                {
                    isCC = false;
                    isBCC = false;
                }
                isSuccess = Manager.Common.Helpers.Common.SendMail("ENVIET GROUP", subject, contentEmail, request.Email, evEmail.username, evEmail.password, evEmail.hostName, evEmail.port, evEmail.useSSL, evEmail.CC, evEmail.BCC, isCC, isBCC);

            }

            return Task.FromResult(isSuccess);
        }
    }
}
