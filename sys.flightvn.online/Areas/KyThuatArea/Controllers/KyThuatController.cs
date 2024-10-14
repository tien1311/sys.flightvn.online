using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System.Drawing;
using Microsoft.Extensions.Configuration;
using static Manager.Model.Services.Model.Response.AirportResponse;
using System.IO;
using System.Linq;
using Manager.Model.Services.Model.Request;
using Manager.Model.Models.ViewModel;
using Manager.Model.Models;
using Manager.Model.Services.Abstraction;
using Manager.Model.Models.Other;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Manager.Model.Models.PaginationBase;
using Manager.Model.Models.VeDoan;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Drawing.Printing;
using Manager.Model.Models.IP;
using RtfPipe.Tokens;
using System.Net;


namespace Manager_EV.Areas.KyThuatArea.Controllers
{
    [Area(AreaNameConst.AREA_KyThuat)]
    public class KyThuatController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        //ThongTinHDRepository _unitOfWork_Repository.ThongTinHD_Rep = new ThongTinHDRepository();
        private IAirportService _airportService;
        CultureInfo provider;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;

        public KyThuatController(IHostingEnvironment environment, IConfiguration configuration, IAirportService airportService, IUnitOfWork_Repository unitOfWork_Repository)
        {
            _hostingEnvironment = environment;
            _configuration = configuration;
            _airportService = airportService;
            _unitOfWork_Repository = unitOfWork_Repository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IPWhiteList(int page = 1, int pageSize = 10, string companyName = null)
        {
            var paginationVM = GetListIPWhiteListPagination(page, pageSize, companyName);
            return View(paginationVM);
        }

        private PaginationBase<APIPartner> GetListIPWhiteListPagination(int page, int pageSize, string companyName)
        {
            var (listOrders, totalRecord) = _unitOfWork_Repository.IP_Rep.GetAPIPartners(page, pageSize, companyName).GetAwaiter().GetResult();
            PaginationBase<APIPartner> paginationVM = new PaginationBase<APIPartner>()
            {
                ListProduct = listOrders,
                TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize),
                CurrentPage = page,
                PageSize = pageSize,
                TotalQuantityOfProduct = totalRecord
            };
            return paginationVM;
        }


        public IActionResult CreateIPWhiteList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveCreateIPWhiteList(APIPartner request)
        {

            if (string.IsNullOrEmpty(request.Company) || (string.IsNullOrEmpty(request.PhysicalAddress) || (request.IPPartners.Count == 1 && request.IPPartners.Any(x => string.IsNullOrEmpty(x.IPAddress)))))
            {
                return Json(new { success = false, message = "Vui lòng nhập đủ thông tin" });
            }

            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool isSuccess = _unitOfWork_Repository.IP_Rep.SaveCreateIPWhiteList(request, acc.MaNV).GetAwaiter().GetResult();

            if (isSuccess)
            {
                return Json(new { success = true, message = "Lưu IP white list thành công" });
            }

            return Json(new { success = false, message = "Lưu IP white list thất bại, vui lòng liên hệ IT" });
        }

        public IActionResult EditIPAddress(int Id)
        {
            var apiPartnerTask = _unitOfWork_Repository.IP_Rep.GetListIP(Id);
            var PartnerTask = _unitOfWork_Repository.IP_Rep.GetAPIPartnerById(Id);
            Task.WaitAll(apiPartnerTask,PartnerTask);

            ViewBag.PartnerId = Id;
            ViewBag.PartnerName = PartnerTask.Result.Company;
            return View(apiPartnerTask.Result);
        }

        [HttpPost]
        public IActionResult SaveEditIPAddress(int PartnerId, string[] ipAddress)
        {
            bool isSuccess = _unitOfWork_Repository.IP_Rep.SaveEditIPAddress(PartnerId, ipAddress).GetAwaiter().GetResult();

            if (isSuccess)
            {
                return Json(new { success = true, message = "Lưu địa chỉ IP thành công" });
            }

            return Json(new { success = false, message = "Lưu địa chỉ IP thất bại, vui lòng liên hệ IT" });
        }

        [HttpPost]
        public IActionResult DeleteIPAddress(int Id)
        {
            bool isSuccess = _unitOfWork_Repository.IP_Rep.DeleteIPAddress(Id).GetAwaiter().GetResult();
            if (isSuccess)
            {
                return Json(new { success = true, message = "Xoá IP thành công" });
            }
            return Json(new { success = false, message = "Xoá IP thất bại, vui lòng liên hệ IT" });
        }

        public IActionResult EditAPIPartner(int Id)
        {
            var apiPartner = _unitOfWork_Repository.IP_Rep.GetAPIPartnerById(Id).GetAwaiter().GetResult();
            return View(apiPartner);
        }

        [HttpPost]
        public IActionResult SaveEditAPIPartner(APIPartner request)
        {
            if (string.IsNullOrEmpty(request.Company) || (string.IsNullOrEmpty(request.PhysicalAddress)))
            {
                return Json(new { success = false, message = "Vui lòng nhập đủ thông tin" });
            }

            bool isSuccess = _unitOfWork_Repository.IP_Rep.SaveEditAPIPartner(request).GetAwaiter().GetResult();

            if (isSuccess)
            {
                return Json(new { success = true, message = "Lưu thông tin Partner thành công" });
            }

            return Json(new { success = false, message = "Lưu thông tin Partner thất bại, vui lòng liên hệ IT" });
        }

        [HttpPost]
        public IActionResult DetailEndPointIPWhiteList(int Id)
        {
            var listEndPointIPWhiteListTask = _unitOfWork_Repository.IP_Rep.GetListEndPointIP(Id);
            var IPPartnerTask = _unitOfWork_Repository.IP_Rep.GetIPPartner(Id);
            Task.WaitAll(listEndPointIPWhiteListTask, IPPartnerTask);

            ViewBag.IPPartnerId = IPPartnerTask.Result.Id;
            ViewBag.IPPartnerAddress = IPPartnerTask.Result.IPAddress;
            return View(listEndPointIPWhiteListTask.Result);
        }

        [HttpPost]
        public IActionResult SaveCreateEndpoint(int IPAddressId, string[] endpoint)
        {
            bool isSuccess = _unitOfWork_Repository.IP_Rep.SaveCreateEndPoint(IPAddressId, endpoint).GetAwaiter().GetResult();
            if (isSuccess)
            {
                return Json(new { success = true, message = "Lưu Endpoint thành công" });
            }
            return Json(new { success = false, message = "Lưu Endpoint thất bại, vui lòng liên hệ IT" });
        }

        [HttpPost]
        public IActionResult IsIPWhiteListActive(int Id)
        {
            var item = _unitOfWork_Repository.IP_Rep.GetEndPoint(Id).GetAwaiter().GetResult();
            if (item != null)
            {
                item.IsActived = !item.IsActived;
                bool isSuccess = _unitOfWork_Repository.IP_Rep.ChangeIPWhiteListActive(item.IsActived, Id).GetAwaiter().GetResult();
                if (isSuccess)
                {
                    return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
                }
            }
            return Json(new { success = false, message = "Cập nhật trạng thái thất bại, vui lòng liên hệ IT" });
        }

        [HttpPost]
        public IActionResult GetAllIPWhiteList(int page = 1, int pageSize = 10, string companyName = null)
        {
            var orderHeaderPagination = GetListIPWhiteListPagination(page, pageSize, companyName);
            return PartialView("_TableIPWhiteListPaginationPatial", orderHeaderPagination);
        }


        public async Task<IActionResult> PhanQuyenHangBay()
        {
            List<AccountModel> listDepartment = new List<AccountModel>();
            listDepartment = await _unitOfWork_Repository.Permission_Rep.GetListDepartment();
            return View(listDepartment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountFromDepartment(string departmentCode)
        {
            var listAccount = await _unitOfWork_Repository.Permission_Rep.GetAccountFromDepartment(departmentCode);
            return Json(listAccount);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPermission(string userId)
        {
            var userPermission = await _unitOfWork_Repository.Permission_Rep.GetUserPermission(userId);
            return PartialView("Partial_UserPermission", userPermission);
        }

        [HttpPost]
        public async Task<IActionResult> InsertOrUpdatePermission(List<Permission> permissions, string departmentCode, string userId)
        {
            PermissionRepository permissionRepository = _unitOfWork_Repository.Permission_Rep;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool isSuccess = false;

            if (string.IsNullOrEmpty(departmentCode))
            {
                return Json(new { success = false, message = "Vui lòng chọn ít nhất Phòng ban" });
            }

            // Xử lý khi không có giá trị userId và có departmentCode
            if (string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(departmentCode))
            {
                var listAccount = await permissionRepository.GetAccountFromDepartment(departmentCode);
                List<string> listUserId = new List<string>();

                foreach (var userInDepartment in listAccount)
                {
                    listUserId.Add(userInDepartment.TenDangNhap);
                }

                // Cập nhật hoặc chèn quyền cho tất cả người dùng trong phòng ban
                foreach (var user in listUserId)
                {
                    foreach (var permission in permissions)
                    {
                        var permissionClone = new Permission
                        {
                            UserId = user,
                            PageId = permission.PageId,
                            CanRead = permission.CanRead,
                            CanWrite = permission.CanWrite,
                            CanDelete = permission.CanDelete,
                            CanExportExcel = permission.CanExportExcel,
                            CreatedDate = DateTime.Now,
                            CreatedBy = acc.MaNV
                        };

                        // Chèn hoặc cập nhật quyền cho từng người dùng
                        isSuccess = await permissionRepository.InsertOrUpdateUserPermissions(new List<Permission> { permissionClone });

                    }
                }
                if (isSuccess)
                {
                    return Json(new { success = true, message = "Lưu thành công" });
                }
            }
            else
            {
                // Xử lý trường hợp có giá trị userId
                foreach (var permission in permissions)
                {
                    permission.UserId = userId;
                    permission.CreatedBy = acc.MaNV;
                    permission.CreatedDate = DateTime.Now;
                }

                isSuccess = await permissionRepository.InsertOrUpdateUserPermissions(permissions);

                if (isSuccess)
                {
                    return Json(new { success = true, message = "Lưu thành công" });
                }
            }

            return Json(new { success = false, message = "Có lỗi xảy ra, vui lòng liên hệ IT" });
        }
        public IActionResult DoiChieuThanhToan()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchThanhToanGateway(DateTime fromDate, DateTime toDate, string orderIds, string paymentType, int resultCode, int pageNumber, int pageSize, string selectedColumns)
        {
            var gateway_Rep = _unitOfWork_Repository.Gateway_Rep;
            if (toDate != DateTime.MinValue)
            {
                toDate = toDate.AddHours(23).AddMinutes(59).AddSeconds(59); // cuối ngày
            }

            var orderIdList = string.IsNullOrEmpty(orderIds)
                              ? new List<string>()
                              : orderIds.Split(',')
                                         .Select(orderId => orderId.Trim())
                                         .Where(orderId => !string.IsNullOrEmpty(orderId))
                                         .ToList();
            var listUser = gateway_Rep.GetUsersPays(orderIdList, fromDate, toDate, paymentType, resultCode, pageNumber, pageSize);
            var totalRecords = gateway_Rep.GetTotalUsersPays(orderIdList, fromDate, toDate, paymentType, resultCode);
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var totalAmount = gateway_Rep.GetTotalAmount(orderIdList, fromDate, toDate, paymentType, resultCode);
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalAmount = totalAmount;

            // Ensure selectedColumns is not null
            if (string.IsNullOrEmpty(selectedColumns))
            {
                ViewBag.SelectedColumns = new List<string>();
            }
            else
            {
                ViewBag.SelectedColumns = selectedColumns.Split(',').ToList();
            }
            return PartialView("Partial_UserPayData", listUser);
        }



        [HttpPost]
        public IActionResult ExportThanhToanGateway(DateTime fromDate, DateTime toDate, string orderIds, string paymentType, int resultCode)
        {
            var gateway_Rep = _unitOfWork_Repository.Gateway_Rep;
            if (toDate != DateTime.MinValue)
            {
                toDate = toDate.AddHours(23).AddMinutes(59).AddSeconds(59); // cuối ngày
            }
            var orderIdList = string.IsNullOrEmpty(orderIds)
                                        ? new List<string>()
                                        : orderIds.Split(',')
                                                   .Select(orderId => orderId.Trim())
                                                   .Where(orderId => !string.IsNullOrEmpty(orderId))
                                                   .ToList();
            var listUser = gateway_Rep.GetUsersPays(orderIdList, fromDate, toDate, paymentType, resultCode, 1, int.MaxValue); // Get all records
                                                                                                                              // Enhance the list to replace requestType with the exact name
            foreach (var user in listUser)
            {
                user.requestType = gateway_Rep.GetRequestType(user.paymentType, user.requestType);
            }

            // Generate Excel file from listUser
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("ThanhToanOnline");

                // Load data into the worksheet
                worksheet.Cells["A1"].LoadFromCollection(listUser, true);

                // Find the index of the CreatedDate and Amount columns
                int createdDateColumnIndex = 0;
                int amountColumnIndex = 0;
                for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                {
                    if (worksheet.Cells[1, col].Text == "CreatedDate")
                    {
                        createdDateColumnIndex = col;
                    }
                    if (worksheet.Cells[1, col].Text == "Amount")
                    {
                        amountColumnIndex = col;
                    }
                }

                if (createdDateColumnIndex > 0)
                {
                    worksheet.Column(createdDateColumnIndex).Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                }

                if (amountColumnIndex > 0)
                {
                    worksheet.Column(amountColumnIndex).Style.Numberformat.Format = "#,##0";
                }

                // Rename columns
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "STT";
                worksheet.Cells[1, 3].Value = "Mã KH";
                worksheet.Cells[1, 4].Value = "Mã KH Đại lý";
                worksheet.Cells[1, 5].Value = "Tên";
                worksheet.Cells[1, 6].Value = "Số điện thoại";
                worksheet.Cells[1, 7].Value = "Email";
                worksheet.Cells[1, 8].Value = "Địa chỉ";
                worksheet.Cells[1, 9].Value = "Phương thức thanh toán";
                worksheet.Cells[1, 10].Value = "Loại thanh toán";
                worksheet.Cells[1, 11].Value = "Tổng tiền";
                worksheet.Cells[1, 12].Value = "Ngày lập";
                worksheet.Cells[1, 13].Value = "Mã đơn hàng";
                worksheet.Cells[1, 14].Value = "Mã thanh toán";
                worksheet.Cells[1, 15].Value = "Mã tham chiếu";
                worksheet.Cells[1, 16].Value = "Trạng thái";
                worksheet.Cells[1, 17].Value = "Mã trạng thái";
                worksheet.Cells[1, 18].Value = "Mã lỗi";
                worksheet.Cells[1, 19].Value = "Nội dung lỗi";
                worksheet.Cells[1, 20].Value = "Ghi chú";


                // Apply styles to the header row
                using (var range = worksheet.Cells[1, 1, 1, 20])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Black);
                }

                // AutoFit columns to fit the content
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                worksheet.Column(1).Hidden = true; // Ẩn cột ID


                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string excelName = $"ThanhToanOnline-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }


        public IActionResult Flight()
        {
            List<FlightModel> result = _unitOfWork_Repository.Flight_Rep.ListFlight();
            return View(result);
        }

        public IActionResult CreateFlight()
        {
            List<Airline> result = _unitOfWork_Repository.Flight_Rep.ListAirline();
            return View("CreateFlight", result);
        }
        public JsonResult SaveFlightData(string hang, string hanhTrinh, string soLuong, string donGia, string donGiaGiam, string loai, string maChuyenBayDi, string gioBayDi, string ngayBayDi, string maChuyenBayVe, string gioBayVe, string ngayBayVe, string hoatDong, string dieuKien, string Specification, string Donvi)
        {
            if (donGiaGiam == null)
            {
                donGiaGiam = "0";
            }
            FlightModel flight = new FlightModel();
            flight.Airline = hang;
            flight.Itinerary = hanhTrinh;
            flight.NumberOfGuests = int.Parse(soLuong);
            flight.Price = decimal.Parse(donGia.Replace(",", ""));
            flight.PriceAgent = decimal.Parse(donGiaGiam.Replace(",", ""));
            flight.KindTrip = loai;
            flight.active = hoatDong;
            flight.Condition = dieuKien;
            flight.Specification = Specification;
            flight.Donvi = Donvi;
            List<FlightDetailModel> listDetail = new List<FlightDetailModel>();

            FlightDetailModel flightDetail = new FlightDetailModel();
            flightDetail.FlightNumber = maChuyenBayDi;
            flightDetail.FlightDate = DateTime.Parse(ngayBayDi, new CultureInfo("es-ES"));
            flightDetail.FlightHour = gioBayDi;
            listDetail.Add(flightDetail);
            //Chuyến về
            FlightDetailModel flightDetail_return = new FlightDetailModel();
            flightDetail_return.FlightNumber = maChuyenBayVe;
            flightDetail_return.FlightDate = DateTime.Parse(ngayBayVe, new CultureInfo("es-ES"));
            flightDetail_return.FlightHour = gioBayVe;
            listDetail.Add(flightDetail_return);
            flight.ListFlightDetail = listDetail;

            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string tenNhanVien = acc.HoTen;

            bool result = _unitOfWork_Repository.Flight_Rep.SaveFlight(flight, tenNhanVien);


            return Json(result);
        }
        [HttpPost]
        public JsonResult DeleteFlightData(int khoachinh)
        {
            bool result = _unitOfWork_Repository.Flight_Rep.DeleteFlight(khoachinh);
            return Json(result);
        }
        public IActionResult UpdateFlightData(string hang, string hanhTrinh, string soLuong, string donGia, string donGiaGiam, string loai, string maChuyenBayDi, string gioBayDi, string ngayBayDi, string maChuyenBayVe, string gioBayVe, string ngayBayVe, int idKhoaChinh, string hoatDong, string dieuKien, string Specification, string Donvi)
        {
            FlightModel flight = new FlightModel();
            flight.ID = idKhoaChinh;
            flight.Airline = hang;
            flight.Itinerary = hanhTrinh;
            flight.NumberOfGuests = int.Parse(soLuong);
            flight.Price = decimal.Parse(donGia.Replace(",", ""));
            flight.PriceAgent = decimal.Parse(donGiaGiam.Replace(",", ""));
            flight.KindTrip = loai;
            flight.active = hoatDong;
            flight.Condition = dieuKien;
            flight.Specification = Specification;
            flight.Donvi = Donvi;
            List<FlightDetailModel> listDetail = new List<FlightDetailModel>();

            FlightDetailModel flightDetail = new FlightDetailModel();
            flightDetail.FlightNumber = maChuyenBayDi;
            flightDetail.FlightDate = DateTime.Parse(ngayBayDi, new CultureInfo("es-ES"));
            flightDetail.FlightHour = gioBayDi;
            listDetail.Add(flightDetail);
            //Chuyến về
            FlightDetailModel flightDetail_return = new FlightDetailModel();
            flightDetail_return.FlightNumber = maChuyenBayVe;
            flightDetail_return.FlightDate = DateTime.Parse(ngayBayVe, new CultureInfo("es-ES"));
            flightDetail_return.FlightHour = gioBayVe;
            listDetail.Add(flightDetail_return);
            flight.ListFlightDetail = listDetail;

            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string tenNhanVien = acc.HoTen;

            bool result = _unitOfWork_Repository.Flight_Rep.UpdateFlight(flight, tenNhanVien);


            return Json(result);
        }


        [HttpPost]
        public IActionResult UpdateFlight(int khoachinh)
        {
            FlightModel flight = _unitOfWork_Repository.Flight_Rep.SetDateFlight(khoachinh);
            return PartialView("UpdateFlight", flight);
        }
        public IActionResult DanhSachBooking()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DanhSachBooking(string cal_from, string cal_to)
        {
            VeDoanAll result = new VeDoanAll();
            DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
            DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
            string dateFrom = dFrom.ToString("yyyy-MM-dd");
            string dateTo = dTo.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
            ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
            result = _unitOfWork_Repository.Flight_Rep.DanhSachVedoan(dateFrom, dateTo);
            return View("DanhSachBooking", result);
        }
        public IActionResult DanhSachYeuCauBooking()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult DanhSachYeuCauBooking(string cal_from, string cal_to, int page = 1, int pageSize = 10)
        //{
        //    FlightRepository flight_Rep = new FlightRepository();
        //    List<YeuCauVeDoan> listYeuCauVeDoan = null;

        //    DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
        //    DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None).AddDays(1);
        //    string dateFrom = dFrom.ToString("yyyy-MM-dd");
        //    string dateTo = dTo.ToString("yyyy-MM-dd");
        //    ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
        //    ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");

        //    listYeuCauVeDoan = flight_Rep.DanhSachYeuCau(dateFrom, dateTo, page, pageSize);
        //    return View("DanhSachYeuCauBooking", listYeuCauVeDoan);
        //}

        public IActionResult YeuCauDoan(int page = 1, int pageSize = 10, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var paginationVM = GetListVeDoanPagination(page, pageSize, dateFrom, dateTo);
            return View(paginationVM);
        }

        public IActionResult GetAllRequestGroupBooking(int page = 1, int pageSize = 10, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var orderHeaderPagination = GetListVeDoanPagination(page, pageSize, dateFrom, dateTo);
            return PartialView("_TablePaginationPatial", orderHeaderPagination);
        }

        private PaginationBase<YeuCauVeDoan> GetListVeDoanPagination(int page, int pageSize, DateTime? dateFrom, DateTime? dateTo)
        {

            var (listOrders, totalRecord) = _unitOfWork_Repository.Flight_Rep.DanhSachYeuCau(page, pageSize, dateFrom, dateTo);

            PaginationBase<YeuCauVeDoan> paginationVM = new PaginationBase<YeuCauVeDoan>()
            {
                ListProduct = listOrders,
                TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize),
                CurrentPage = page,
                PageSize = pageSize,
                TotalQuantityOfProduct = totalRecord
            };
            return paginationVM;
        }


        public IActionResult ChiTietYeuCauDoan(string code)
        {
            YeuCauVeDoan requestGroupBooking = _unitOfWork_Repository.Flight_Rep.ChiTietYeuCauDoan(code);

            ViewBag.DepartureAirport = _unitOfWork_Repository.Flight_Rep.GetAirportName(requestGroupBooking.DepartureCode1);
            ViewBag.ArrivalAirport = _unitOfWork_Repository.Flight_Rep.GetAirportName(requestGroupBooking.ArrivalCode1);
            return View(requestGroupBooking);
        }
        public IActionResult Chitietbooking(int khoachinh)
        {

            List<ChitietBooking> flight = _unitOfWork_Repository.Flight_Rep.ChiTietBooking(khoachinh);
            return View(flight);
        }
        public IActionResult Chitietyeucaubooking(int khoachinh)
        {

            List<Luuthongtin> flight = _unitOfWork_Repository.Flight_Rep.ChiTietYeuCauBooking(khoachinh);
            return View(flight);
        }
        public IActionResult DoanhSoHang()
        {
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            DateTime dtDauThang = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime dtCuoiThang = dtDauThang.AddMonths(1).AddDays(-1);
            string dateFrom = dtDauThang.ToString("yyyy-MM-dd");
            string dateTo = dtCuoiThang.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = dtDauThang.ToString("dd/MM/yyyy");
            ViewBag.DateTo = dtCuoiThang.ToString("dd/MM/yyyy");
            ViewBag.SelectOptions = "CheckMonth";
            ViewBag.Month = DateTime.Now.Month;
            ViewBag.Year = DateTime.Now.Year;
            List<DoanhSoHangViewModel> result = _unitOfWork_Repository.DoanhSoHang_Rep.ListDoanhSoHang(dateFrom, dateTo, server_KH_KT);
            return View(result);
        }
        [HttpPost]
        public IActionResult DoanhSoHang(string cal_from, string cal_to, string SearchBtn, string SelectOptions = "", string SearchBtnMonth = "", string Thang = "", string Nam = "")
        {
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            string dateFrom = "";
            string dateTo = "";
            List<DoanhSoHangViewModel> result = new List<DoanhSoHangViewModel>();
            if (SearchBtnMonth != null && SearchBtnMonth != "")
            {

                DateTime dtDauThang = new DateTime(int.Parse(Nam), int.Parse(Thang), 1);
                DateTime dtCuoiThang = dtDauThang.AddMonths(1).AddDays(-1);
                DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
                DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
                dateFrom = dtDauThang.ToString("yyyy-MM-dd");
                dateTo = dtCuoiThang.ToString("yyyy-MM-dd");
                ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
                ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
                ViewBag.SelectOptions = SelectOptions;
                ViewBag.Month = Thang;
                ViewBag.Year = Nam;
                result = _unitOfWork_Repository.DoanhSoHang_Rep.ListDoanhSoHang(dateFrom, dateTo, server_KH_KT);
            }
            else
            {

                DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
                DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
                dateFrom = dFrom.ToString("yyyy-MM-dd");
                dateTo = dTo.ToString("yyyy-MM-dd");
                ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
                ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
                ViewBag.SelectOptions = SelectOptions;
                ViewBag.Month = DateTime.Now.Month;
                ViewBag.Year = DateTime.Now.Year;
                result = _unitOfWork_Repository.DoanhSoHang_Rep.ListDoanhSoHang(dateFrom, dateTo, server_KH_KT);
            }
            return View(result);




        }
        [HttpPost]
        public IActionResult PopupDoanhSoHang(string Ngay, string Hang)
        {
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            ChiTietDoanhSoHang chiTiet = new ChiTietDoanhSoHang();
            DateTime ngay = DateTime.ParseExact(Ngay, "dd/MM/yyyy", provider, DateTimeStyles.None);
            chiTiet = _unitOfWork_Repository.DoanhSoHang_Rep.ChiTietDoanhSoHang(ngay, Hang, Ngay, server_KH_KT);
            return PartialView(chiTiet);
        }

        public IActionResult DangKyThanhVien()
        {
            Member result = _unitOfWork_Repository.Member_Rep.Dangkithanhvien();
            return View("DangKyThanhVien", result);
        }
        [HttpPost]
        public IActionResult DangKyThanhVien(string saveBtn, string seachMahd, string khuvuc, string mahd, string company, string name, string makh, string code, string password, string email, string address, string phone, string fax, string kinhdoanh, string ketoan, string isactive, string isshow, string dulich)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            MemberRepository member_Rep = _unitOfWork_Repository.Member_Rep;
            if (seachMahd != null)
            {
                if (string.IsNullOrEmpty(mahd))
                {
                    TempData["thongbaoError"] = "Mã hợp đồng không được bỏ trống";
                    Member result3 = member_Rep.Dangkithanhvien();
                    return View("DangKyThanhVien", result3);
                }
                Member searchhopdong = member_Rep.searchHD(mahd);
                if (searchhopdong.member_email == "" || searchhopdong.member_email == null)
                {
                    TempData["thongbaoError"] = "Mã hợp đồng không tồn tại";
                    Member result3 = member_Rep.Dangkithanhvien();
                    return View("DangKyThanhVien", result3);
                }
                return View("DangKyThanhVien", searchhopdong);
            }
            if (saveBtn != null)
            {
                if (dulich == null)
                {
                    dulich = "";
                }
                try
                {
                    bool saveHopdong = member_Rep.SaveHopDong(khuvuc, mahd, company, name, makh, code, password, email, address, phone, fax, kinhdoanh, ketoan, isactive, isshow, acc.TenDangNhap, dulich);
                    if (saveHopdong == true)
                    {
                        TempData["thongbaoSuccess"] = "Đăng kí tài khoản thành công";
                    }
                    else
                    {
                        TempData["thongbaoError"] = "Đăng kí tài khoản thất bại";

                    }
                }
                catch (Exception ex)
                {
                    TempData["thongbaoError"] = "Đăng kí tài khoản thất bại";
                }
                Member result1 = member_Rep.Dangkithanhvien();
                return View("DangKyThanhVien", result1);
            }
            Member result = member_Rep.Dangkithanhvien();
            return View("DangKyThanhVien", result);
        }
        public IActionResult PhanQuyen()
        {
            List<DecentralizationModel> result = _unitOfWork_Repository.Decentralization_Rep.Phanquyen();
            return View("PhanQuyen", result);
        }
        public IActionResult ThemChucNang(string khoachinh)
        {
            List<DecentralizationModel> result = _unitOfWork_Repository.Decentralization_Rep.Chitietphanquyen(khoachinh);

            return View(result);
        }
        [HttpPost]
        public JsonResult Savephanquyen(string maphongban, string tinhnangmoi, string thongbao, string baocaove, string noibo, string daili, string ketoan, string kinhdoanh, string phongve, string bpdoan, string hoadon, string ca, string yensao, string cs, string data, string setting, string kythuat, string dulich)
        {
            bool result = _unitOfWork_Repository.Decentralization_Rep.Savephanquyen(maphongban, tinhnangmoi, thongbao, baocaove, noibo, daili, ketoan, kinhdoanh, phongve, bpdoan, hoadon, ca, yensao, cs, data, setting, kythuat, dulich);

            return Json(result);
        }
        public IActionResult PhanQuyenMember()
        {
            List<DecentralizationMemberModel> result = _unitOfWork_Repository.Decentralization_Rep.Phanquyenmember();

            return View("PhanQuyenMember", result);
        }
        public IActionResult ThemChucNangMember(string khoachinh)
        {
            List<Checkchucnang> result = _unitOfWork_Repository.Decentralization_Rep.Chitietphanquyenmember(khoachinh);
            return View(result);
        }
        [HttpPost]
        public JsonResult Savephanquyenmember(string manv, string tinhnangmoi, string thongbao, string baocaove, string noibo, string daili, string ketoan, string kinhdoanh, string phongve, string bpdoan, string hoadon, string ca, string yensao, string cs, string data, string setting, string kythuat, string dulich)
        {
            bool result = _unitOfWork_Repository.Decentralization_Rep.Savephanquyenmember(manv, tinhnangmoi, thongbao, baocaove, noibo, daili, ketoan, kinhdoanh, phongve, bpdoan, hoadon, ca, yensao, cs, data, setting, kythuat, dulich);

            return Json(result);
        }
        public IActionResult ThemChucNangNVMoi()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Savephanquyenmembermoi(string manv, string tinhnangmoi, string thongbao, string baocaove, string noibo, string daili, string ketoan, string kinhdoanh, string phongve, string bpdoan, string hoadon, string ca, string yensao, string cs, string data, string setting, string kythuat, string dulich)
        {
            DecentralizationRepository phanquyen_Rep = _unitOfWork_Repository.Decentralization_Rep;
            bool checknv = phanquyen_Rep.CheckNV(manv);
            bool checkCn = phanquyen_Rep.CheckCN(manv);
            if (checknv == true && checkCn == true)
            {
                bool result = phanquyen_Rep.Savephanquyenmember(manv, tinhnangmoi, thongbao, baocaove, noibo, daili, ketoan, kinhdoanh, phongve, bpdoan, hoadon, ca, yensao, cs, data, setting, kythuat, dulich);
                return Json(true);
            }
            else
            {
                return Json(false);
            }


        }
        [HttpPost]
        public JsonResult SearchMember(string manv)
        {
            Chitietnhanvien result = _unitOfWork_Repository.Decentralization_Rep.Chitietnhanvien(manv);

            return Json(result);
        }
        public IActionResult Log_SendSMSBCV(int? page = 1, int pageSize = 50)
        {
            List<LOG_SendSMSBaoCaoVe> result = _unitOfWork_Repository.Log_SendSMS_Rep.Log_SendSMS();
            int pageNumber = page ?? 1;
            var model = PagingList.Create(result, pageSize, pageNumber);
            //Chuyển actionname về trang thông báo khi phân trang
            model.Action = "Log_SendSMSBCV";
            model.RouteValue = new RouteValueDictionary {
                        { "i", 12}
                    };
            return View(model);
        }
        public async Task<IActionResult> ListAirportCode()
        {
            string Server = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            List<AirportCodeRequest> response = new List<AirportCodeRequest>();
            AirportCodeRequest_Search request = new AirportCodeRequest_Search();
            request.Key = "All";
            response = await _airportService.Search("", Server);
            return View(response);
        }
        public IActionResult CreateAirportCode()
        {
            return PartialView();
        }
        //public async Task<IActionResult> SaveCreateAirportCode(AirportCodeRequest model)
        //{
        //    string Server = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
        //    List<AirportCodeRequest> response = new List<AirportCodeRequest>();
        //    AirportCodeRequest_Search request = new AirportCodeRequest_Search();
        //   // request.AirportCode = model.AirportCode;
        //    response = await _airportService.Search(Server);
        //    if (response.Count > 0)
        //    {
        //        TempData["thongbao"] = "Airpost Code đã tồn tại";
        //    }
        //    else
        //    {
        //        AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
        //        model.CreatedBy = acc.MaNV;
        //        model.CreatedDate = DateTime.Now;
        //        AirportCodeResponse_Insert response_insert = new AirportCodeResponse_Insert();
        //        response_insert = await _airportService.Insert(model);
        //        if (response_insert.Message == "Success")
        //        {
        //            TempData["thongbao"] = "Tạo mới thành công";
        //        }
        //        else
        //        {
        //            TempData["thongbao"] = "Tạo mới không thành công";
        //        }
        //    }



        //    //request.AirportCode = "All";
        //    response = await _airportService.Search(Server);
        //    return View("ListAirportCode", response);

        //}
        [HttpPost]
        public async Task<JsonResult> SaveCreateAirportCode(AirportCodeRequest model)
        {
            string Server = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            List<AirportCodeRequest> response = new List<AirportCodeRequest>();
            AirportCodeRequest_Search request = new AirportCodeRequest_Search();

            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            model.CreatedBy = acc.MaNV;
            model.CreatedDate = DateTime.Now;
            AirportCodeResponse_Insert response_insert = new AirportCodeResponse_Insert();
            response_insert = await _airportService.Insert(model);
            return Json(response_insert);
        }
        public async Task<IActionResult> EditAirportCode(AirportCodeRequest model)
        {
            string Server = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            List<Profile> profiles = new List<Profile>();
            profiles = await _airportService.SearchID(model.ID, Server);
            model.Profiles = profiles;
            return PartialView(model);
        }
        [HttpPost]
        public async Task<JsonResult> SaveEditAirportCode(AirportCodeRequest model)
        {
            string Server = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            model.UpdatedBy = acc.MaNV;
            AirportCodeResponse_Update response = new AirportCodeResponse_Update();
            response = await _airportService.Update(model);
            return Json(response);
        }
        [HttpDelete]
        public async Task<JsonResult> DeleteAirportCode(string airportCode)
        {
            AirportCodeResponse_Delete response = new AirportCodeResponse_Delete();
            response = await _airportService.Delete(airportCode);
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> ListBankStatement()
        {
            string FromDate = DateTime.Now.ToString("yyyy-MM-dd");
            string ToDate = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            ViewBag.DateFrom = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.DateTo = DateTime.Now.ToString("dd/MM/yyyy");
            List<BankStatement_Request_Params_Model> result = new List<BankStatement_Request_Params_Model>();
            result = await _unitOfWork_Repository.BankStatement_Rep.Search(FromDate, ToDate);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> ListBankStatement(string cal_from, string cal_to)
        {
            DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
            DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
            cal_from = dFrom.ToString("yyyy-MM-dd");
            cal_to = dTo.ToString("yyyy-MM-dd 23:59:59");
            ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
            ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
            List<BankStatement_Request_Params_Model> result = new List<BankStatement_Request_Params_Model>();
            result = await _unitOfWork_Repository.BankStatement_Rep.Search(cal_from, cal_to);
            return View(result);
        }
        public IActionResult DanhSachGiaoDich(string cal_from, string cal_to, string ma_kh, string kenh_tt, string trang_tt, string don_hang, string searchBtn)
        {
            if (searchBtn == "Search")
            {
                DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
                DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
                string dateFrom = dFrom.ToString("yyyy-MM-dd");
                string dateTo = dTo.ToString("yyyy-MM-dd");
                ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
                ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
                List<PaymentAppota> payment = _unitOfWork_Repository.Flight_Rep.SearchPayment(dateFrom, dateTo, ma_kh, kenh_tt, trang_tt, don_hang);
                return View(payment);
            }
            else
            {
                List<PaymentAppota> payment = _unitOfWork_Repository.Flight_Rep.ListPayment();
                return View(payment);
            }
        }
        public async Task<IActionResult> ConfigBalanceAirlines()
        {
            var result = await _unitOfWork_Repository.ConfigBalanceAirlines_Rep.ConfigAirlineBalance();
            return View(result);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateAirlineBalance(ConfigAirlineBalanceModel model)
        {
            bool result = false;
            result = await _unitOfWork_Repository.ConfigBalanceAirlines_Rep.Update_ConfigAirlineBalance_ID(model.ID, model.Amount);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditConfigAirlineBalance(ConfigAirlineBalanceModel model)
        {
            var result = await _unitOfWork_Repository.ConfigBalanceAirlines_Rep.ConfigAirlineBalance_ID(model.ID);
            return PartialView(result);
        }
    }
}