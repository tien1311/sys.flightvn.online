using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using Dapper;
using Microsoft.Extensions.Configuration;
using Manager.Model.Models.ThongKe;
using Manager.Common.Helpers.AreaHelpers;

namespace Manager_EV.Areas.DuLichArea.Controllers
{
    [Area(AreaNameConst.AREA_DuLich)]

    public class ThongKeController : Controller
    {

        public IConfiguration _configuration;

        public ThongKeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetData(DateTime fromDate, DateTime toDate)
        {
            string visaConnectionString = _configuration.GetConnectionString("SQL_EV_VISA");
            string tourBookingConnectionString = _configuration.GetConnectionString("SQL_EV_TOUR");
            string hotelBookingConnectionString = _configuration.GetConnectionString("SQL_EV_HOTEL");
            string carBookingConnectionString = _configuration.GetConnectionString("SQL_EV_CAR");


            toDate = toDate.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59); // Ngày , giờ, phút, giây cuối cùng của tháng

            List<ThongKeData> ListData = new List<ThongKeData>();

            if (fromDate == DateTime.MinValue || toDate == DateTime.MinValue)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn ngày";
                return RedirectToAction("Index");
            }

            // Lấy dữ liệu thao tháng
            string visaSqlQuery = $"SELECT " +
                                $"FORMAT(DATEADD(month, DATEDIFF(month, 0, b.CreatedDate), 0), 'yyyy-MM') AS CreatedDate, " +
                                $"N'VISA' AS Name, " +
                                $"SUM(CASE WHEN b.StatusID = 8 THEN b.Price ELSE 0 END) AS TotalPrice, " +
                                $"SUM(CASE WHEN b.StatusID = 1 THEN 1 ELSE 0 END) AS SLMoi, " +
                                $"SUM(CASE WHEN b.StatusID = 2 THEN 1 ELSE 0 END) AS SLDaNhanBooking, " +
                                $"SUM(CASE WHEN b.StatusID = 3 THEN 1 ELSE 0 END) AS SLDaTiepNhanHoSo, " +
                                $"SUM(CASE WHEN b.StatusID = 4 THEN 1 ELSE 0 END) AS SLDangXuLyHoSo, " +
                                $"SUM(CASE WHEN b.StatusID = 5 THEN 1 ELSE 0 END) AS SLYeuCauBoSungHoSo, " +
                                $"SUM(CASE WHEN b.StatusID = 6 THEN 1 ELSE 0 END) AS SLNopHoSoThanhCong, " +
                                $"SUM(CASE WHEN b.StatusID = 7 THEN 1 ELSE 0 END) AS SLDangXetDuyetHoSo, " +
                                $"SUM(CASE WHEN b.StatusID = 8 THEN 1 ELSE 0 END) AS SLHoanThanh, " +
                                $"SUM(CASE WHEN b.StatusID = 9 THEN 1 ELSE 0 END) AS SLHuy " +
                                $"FROM Booking b " +
                                $"JOIN Type t ON b.TypeID = t.ID " +
                                $"JOIN Product p ON t.ProductID = p.ID " +
                                $"WHERE b.CreatedDate BETWEEN @fromDate AND @toDate " +
                                $"GROUP BY FORMAT(DATEADD(month, DATEDIFF(month, 0, b.CreatedDate), 0), 'yyyy-MM') " +
                                $"ORDER BY CreatedDate";

            string tourBookingNoiDiaSqlQuery = $"SELECT " +
                                        $"FORMAT(DATEADD(month, DATEDIFF(month, 0, CONVERT(date, CreateDate)), 0), 'yyyy-MM') AS CreatedDate, " +
                                        $"N'Tour Nội Địa' AS Name, " +
                                        $"SUM(CASE WHEN IDStatus = 7 THEN totalPrice ELSE 0 END) AS TotalPrice, " +
                                        $"SUM(CASE WHEN IDStatus = 1 THEN 1 ELSE 0 END) AS SLMoi, " +
                                        $"SUM(CASE WHEN IDStatus = 2 THEN 1 ELSE 0 END) AS SLDaTiepNhan, " +
                                        $"SUM(CASE WHEN IDStatus = 3 THEN 1 ELSE 0 END) AS SLDaGiuCho, " +
                                        $"SUM(CASE WHEN IDStatus = 4 THEN 1 ELSE 0 END) AS SLDaDatCoc, " +
                                        $"SUM(CASE WHEN IDStatus = 5 THEN 1 ELSE 0 END) AS SLHoanTatThanhToan, " +
                                        $"SUM(CASE WHEN IDStatus = 6 THEN 1 ELSE 0 END) AS SLHuy, " +
                                        $"SUM(CASE WHEN IDStatus = 7 THEN 1 ELSE 0 END) AS SLHoanThanh " +
                                        $"FROM TourBooking " +
                                        $"WHERE CONVERT(date, CreateDate) BETWEEN @fromDate AND @toDate " +
                                        $"AND LoaiTour = 1" +
                                        $"GROUP BY FORMAT(DATEADD(month, DATEDIFF(month, 0, CONVERT(date, CreateDate)), 0), 'yyyy-MM') " +
                                        $"ORDER BY CreatedDate";

            string tourBookingQuocTeSqlQuery = $"SELECT " +
                                        $"FORMAT(DATEADD(month, DATEDIFF(month, 0, CONVERT(date, CreateDate)), 0), 'yyyy-MM') AS CreatedDate, " +
                                        $"N'Tour Quốc Tế' AS Name, " +
                                        $"SUM(CASE WHEN IDStatus = 7 THEN totalPrice ELSE 0 END) AS TotalPrice, " +
                                        $"SUM(CASE WHEN IDStatus = 1 THEN 1 ELSE 0 END) AS SLMoi, " +
                                        $"SUM(CASE WHEN IDStatus = 2 THEN 1 ELSE 0 END) AS SLDaTiepNhan, " +
                                        $"SUM(CASE WHEN IDStatus = 3 THEN 1 ELSE 0 END) AS SLDaGiuCho, " +
                                        $"SUM(CASE WHEN IDStatus = 4 THEN 1 ELSE 0 END) AS SLDaDatCoc, " +
                                        $"SUM(CASE WHEN IDStatus = 5 THEN 1 ELSE 0 END) AS SLHoanTatThanhToan, " +
                                        $"SUM(CASE WHEN IDStatus = 6 THEN 1 ELSE 0 END) AS SLHuy, " +
                                        $"SUM(CASE WHEN IDStatus = 7 THEN 1 ELSE 0 END) AS SLHoanThanh " +
                                        $"FROM TourBooking " +
                                        $"WHERE CONVERT(date, CreateDate) BETWEEN @fromDate AND @toDate " +
                                        $"AND LoaiTour = 2" +
                                        $"GROUP BY FORMAT(DATEADD(month, DATEDIFF(month, 0, CONVERT(date, CreateDate)), 0), 'yyyy-MM') " +
                                        $"ORDER BY CreatedDate";

            string hotelBookingSqlQuery = $"SELECT " +
                                          $"FORMAT(DATEADD(month, DATEDIFF(month, 0, b.CreatedDate), 0), 'yyyy-MM') AS CreatedDate, " +
                                          $"N'Khách Sạn' AS Name, " +
                                          $"SUM(CASE WHEN b.StatusID = 6 THEN b.TotalPrice ELSE 0 END) AS TotalPrice, " +
                                          $"SUM(CASE WHEN b.StatusID = 1 THEN 1 ELSE 0 END) AS SLMoi, " +
                                          $"SUM(CASE WHEN b.StatusID = 2 THEN 1 ELSE 0 END) AS SLDaNhanBooking, " +
                                          $"SUM(CASE WHEN b.StatusID = 3 THEN 1 ELSE 0 END) AS SLDaGiuCho, " +
                                          $"SUM(CASE WHEN b.StatusID = 4 THEN 1 ELSE 0 END) AS SLDaDatCoc, " +
                                          $"SUM(CASE WHEN b.StatusID = 5 THEN 1 ELSE 0 END) AS SLHoanTatThanhToan, " +
                                          $"SUM(CASE WHEN b.StatusID = 6 THEN 1 ELSE 0 END) AS SLHoanThanh, " +
                                          $"SUM(CASE WHEN b.StatusID = 7 THEN 1 ELSE 0 END) AS SLHuy " +
                                          $"FROM Booking b " +
                                          $"WHERE b.CreatedDate BETWEEN @fromDate AND @toDate " +
                                          $"GROUP BY FORMAT(DATEADD(month, DATEDIFF(month, 0, b.CreatedDate), 0), 'yyyy-MM') " +
                                          $"ORDER BY CreatedDate ASC";

            string carBookingSqlQuery = $"SELECT " +
                                        $"FORMAT(DATEADD(month, DATEDIFF(month, 0, r.departure), 0), 'yyyy-MM') AS CreatedDate, " +
                                        $"N'Xe Đưa Đón' AS Name, " +
                                        $"SUM(CASE WHEN r.status_enviet = 'COMPLETE' THEN r.price_customer ELSE 0 END) AS TotalPrice, " +
                                        $"SUM(CASE WHEN r.status_enviet = 'PENDING' THEN 1 ELSE 0 END) AS SLChoGui, " +
                                        $"SUM(CASE WHEN r.status_enviet = 'CREATE' THEN 1 ELSE 0 END) AS SLChoXuLy, " +
                                        $"SUM(CASE WHEN r.status_enviet = 'WAITING' THEN 1 ELSE 0 END) AS SLChoXacNhan, " +
                                        $"SUM(CASE WHEN r.status_enviet = 'CONFIRM' THEN 1 ELSE 0 END) AS SLXacNhanChuyen, " +
                                        $"SUM(CASE WHEN r.status_enviet = 'REJECT' THEN 1 ELSE 0 END) AS SLTuChoi, " +
                                        $"SUM(CASE WHEN r.status_enviet = 'CANCEL' THEN 1 ELSE 0 END) AS SLHuy, " +
                                        $"SUM(CASE WHEN r.status_enviet = 'COMPLETE' THEN 1 ELSE 0 END) AS SLHoanThanh " +
                                        $"FROM Requests r " +
                                        $"WHERE r.departure BETWEEN @fromDate AND @toDate " +
                                        $"GROUP BY FORMAT(DATEADD(month, DATEDIFF(month, 0, r.departure), 0), 'yyyy-MM') " +
                                        $"ORDER BY CreatedDate";

            using (IDbConnection dbConnection = new SqlConnection(visaConnectionString))
            {
                dbConnection.Open();
                ListData.AddRange(dbConnection.Query<ThongKeData>(visaSqlQuery, new { fromDate, toDate }));
                dbConnection.Close();
            }

            using (IDbConnection dbConnection = new SqlConnection(tourBookingConnectionString))
            {
                dbConnection.Open();
                ListData.AddRange(dbConnection.Query<ThongKeData>(tourBookingNoiDiaSqlQuery, new { fromDate, toDate }));
                dbConnection.Close();
            }

            using (IDbConnection dbConnection = new SqlConnection(tourBookingConnectionString))
            {
                dbConnection.Open();
                ListData.AddRange(dbConnection.Query<ThongKeData>(tourBookingQuocTeSqlQuery, new { fromDate, toDate }));
                dbConnection.Close();
            }

            using (IDbConnection dbConnection = new SqlConnection(hotelBookingConnectionString))
            {
                dbConnection.Open();
                ListData.AddRange(dbConnection.Query<ThongKeData>(hotelBookingSqlQuery, new { fromDate, toDate }));
                dbConnection.Close();
            }

            using (IDbConnection dbConnection = new SqlConnection(carBookingConnectionString))
            {
                dbConnection.Open();
                ListData.AddRange(dbConnection.Query<ThongKeData>(carBookingSqlQuery, new { fromDate, toDate }));
                dbConnection.Close();
            }


            return PartialView("_ThongKePartialView", ListData);
        }


    }
}
