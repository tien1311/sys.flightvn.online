using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyInvoice.Json;
using EasyInvoice.Json.Linq;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Manager.DataAccess.Repository;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Manager_EV.Areas.DuLichArea.Controllers
{
    [Area(AreaNameConst.AREA_DuLich)]
    public class DulichController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;
        CultureInfo provider;
        public DulichController(IConfiguration configuration, IUnitOfWork_Repository unitOfWork_Repository)
        {
            _configuration = configuration;
            _unitOfWork_Repository = unitOfWork_Repository;
        }

        public IActionResult IndexDuLich()
        {
            return View();
        }



        public IActionResult BookingTour(string FromDate, string ToDate, string searchBtn, int page = 1, int pageSize = 20)
        {
            DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;

            if (searchBtn != null)
            {
                DateTime dFrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider, DateTimeStyles.None);
                DateTime dTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider, DateTimeStyles.None);
                string dateFrom = dFrom.ToString("yyyy-MM-dd");
                string dateTo = dTo.ToString("yyyy-MM-dd");
                ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
                ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
                List<BookingInfoModel> resuft = Dulich_Rep.SearchBooking(dFrom.ToString("MM/dd/yyyy"), dTo.ToString("MM/dd/yyyy"), page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalItems = Dulich_Rep.GetTotalSearchBookingsCount(dFrom.ToString("MM/dd/yyyy"), dTo.ToString("MM/dd/yyyy"));
                ViewBag.IsSearch = true;
                return View(resuft);
            }
            else
            {
                List<BookingInfoModel> resuft = Dulich_Rep.ListBooking(page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalItems = Dulich_Rep.GetTotalBookingsCount();
                ViewBag.IsSearch = false;
                return View(resuft);
            }
        }



        public IActionResult BookingTourQT(string FromDate, string ToDate, string searchBtn, int page = 1, int pageSize = 20)
        {
            DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;

            if (searchBtn != null)
            {
                DateTime dFrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider, DateTimeStyles.None);
                DateTime dTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider, DateTimeStyles.None);
                string dateFrom = dFrom.ToString("yyyy-MM-dd");
                string dateTo = dTo.ToString("yyyy-MM-dd");
                ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
                ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
                List<BookingInfoModel> resuft = Dulich_Rep.SearchBookingQT(dFrom.ToString("MM/dd/yyyy"), dTo.ToString("MM/dd/yyyy"), page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalItems = Dulich_Rep.GetTotalSearchBookingsQTCount(dFrom.ToString("MM/dd/yyyy"), dTo.ToString("MM/dd/yyyy"));
                ViewBag.IsSearch = true;
                return View(resuft);
            }
            else
            {
                List<BookingInfoModel> resuft = Dulich_Rep.ListBookingQT(page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalItems = Dulich_Rep.GetTotalBookingsQTCount();
                ViewBag.IsSearch = false;
                return View(resuft);
            }
        }

        public static async Task<string> GetEnVietToken()
        {

            string apiUrl = "https://dev-api.envietgroup.com/daily/Account/Authenticate";
            string userName = "enviet";
            string passWord = "EnViet@456";
            var token = "";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-master-key", "1370542bcb7c6b71e34e975d4697f89bab164a520934ff47a5aceb780fdc92e6");
                var content = new StringContent($"{{\"userName\":\"{userName}\",\"passWord\":\"{passWord}\"}}", Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    JObject jmessage = JObject.Parse(responseContent);
                    token = jmessage.GetValue("result")["token"].ToString();
                }
            }
            return token;
        }

        [HttpPost]
        public async Task<IActionResult> Chitietbookingtour(string tourcode, string tourcodetrienkhai)
        {
            try
            {
                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                DulichRepository dulichRepository = _unitOfWork_Repository.Dulich_Rep;
                var itemBooking = dulichRepository.NewDetailBooking(tourcodetrienkhai);
                string token = await GetEnVietToken();
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://api.envietgroup.com/api/v1/DatViet/Tour/GetDetail";

                    string api_token = "OWMxams3NXVkaWFoenJuNGY4eDN0cHd5c2wyZW1v";
                    string urlWithParams = $"{apiUrl}?trienkhai_id={tourcode}&api_token={api_token}";
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = await client.GetAsync(urlWithParams);

                    if (response.IsSuccessStatusCode)
                    {
                        List<ListNhanVienDuLich> DSdulich = dulichRepository.DSDuLich();
                        foreach (var danhSach in DSdulich)
                        {
                            if (danhSach.Ten == acc.HoTen)
                            {
                                if (response.IsSuccessStatusCode == true)
                                {

                                    if (itemBooking.IDStatus == "1")
                                    {
                                        TempData["thongbaoInfo"] = "Mới";
                                        ViewBag.code = tourcodetrienkhai;
                                        itemBooking.IDStatus = "2"; // Status = 2 là Đã tiếp nhận (Dựa vào bảng Status của Database TOUR) 
                                        dulichRepository.ChangeBookingStatus(itemBooking.IDStatus, tourcodetrienkhai);
                                        var isAddSuccess = dulichRepository.AddNguoiNhan(itemBooking.TourCode, acc.HoTen, "");
                                        itemBooking.NguoiNhan = acc.HoTen;
                                    }
                                }
                            }
                        }

                        string responseBody = await response.Content.ReadAsStringAsync();
                        DetailTourModel apiData = JsonConvert.DeserializeObject<DetailTourModel>(responseBody);

                        // Dùng reflection để cập nhật thuộc tính của itemBooking từ apiData
                        foreach (PropertyInfo property in apiData.GetType().GetProperties())
                        {
                            if (property.CanRead)
                            {
                                var value = property.GetValue(apiData, null);
                                if (value != null) // chỉ cập nhật các giá trị từ apiData
                                {
                                    if (property.Name == "code_trienkhai" || property.Name == "ghi_chu" || property.Name == "gia_1s"
                                        || property.Name == "gia_2s" || property.Name == "gia_3s" || property.Name == "gia_4s" || property.Name == "gia_5s"
                                        || property.Name == "sale" || property.Name == "tour_info" || property.Name == "so_khach" || property.Name == "booked_seat"
                                        || property.Name == "reservation_seat" || property.Name == "gia_rs3s" || property.Name == "gia_rs4s" || property.Name == "gia_rs5s"
                                        )
                                    {
                                        itemBooking.GetType().GetProperty(property.Name).SetValue(itemBooking, value, null);
                                    }
                                }
                            }
                        }

                        return View(itemBooking);
                    }
                    else
                    {
                        return BadRequest($"Lỗi: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }



        [HttpPost]
        public async Task<JsonResult> ChuyenBooking(string TourCode, string NguoiNhan)
        {
            try
            {
                DulichRepository dulichRepository = _unitOfWork_Repository.Dulich_Rep;
                DetailTourModel itemBooking = dulichRepository.NewDetailBooking(TourCode);

                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                string tenNhanVienHienTai = acc.HoTen;


                if (itemBooking != null)
                {
                    itemBooking.NguoiNhan = NguoiNhan;

                    bool success = false;

                    var isChangeSuccess = dulichRepository.ChangeBookingStatus(itemBooking.IDStatus, TourCode);
                    var isAddSuccess = dulichRepository.AddNguoiNhan(TourCode, itemBooking.NguoiNhan, tenNhanVienHienTai);

                    if (isChangeSuccess && isAddSuccess)
                    {
                        success = true;
                    }

                    if (success)
                    {
                        return Json(new { success = true, message = "Bạn đã chuyển thành công!", itemBooking.IDStatus });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Bạn đã chuyển thất bại." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy dữ liệu đặt tour." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
        [HttpPost]
        public async Task<JsonResult> CancelBooking(string cancellationReason, string TourCode)
        {
            try
            {
                DulichRepository dulichRepository = _unitOfWork_Repository.Dulich_Rep;
                DetailTourModel dulichData = dulichRepository.NewDetailBooking(TourCode);

                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                string tenNhanVienHienTai = acc.HoTen;
                var itemBooking = dulichData;
                bool resuft = dulichRepository.MailCancelBoooking(
                    dulichData.LoaiTour,
                    dulichData.customerName,
                    dulichData.TourCode,
                    dulichData.NameTour,
                    dulichData.tourID,
                    dulichData.ngaydi,
                    dulichData.ngayve,
                    dulichData.hotelTour,
                    dulichData.adultQuantity,
                    dulichData.childQuantity,
                    dulichData.kidQuantity,
                    dulichData.Namecompany,
                    dulichData.MaKH,
                    dulichData.customerPhone,
                    dulichData.customerEmail,
                    dulichData.customerNote,
                    dulichData.commission,
                    dulichData.totalPrice,
                    dulichData.MaSoThue,
                    dulichData.TenCaNhanToChuc,
                    dulichData.DiaChi,
                    dulichData.price,
                    dulichData.Vat,
                    cancellationReason
                );
                bool saveLationReason = dulichRepository.SaveLationReason(TourCode, cancellationReason);
                if (resuft)
                {
                    if (itemBooking != null)
                    {
                        bool success = false;


                        itemBooking.IDStatus = "6"; // Trạng thái huỷ booking có status = 6

                        var isChangeSuccess = dulichRepository.ChangeBookingStatus(itemBooking.IDStatus, TourCode);
                        var isAddSuccess = dulichRepository.AddNguoiNhan(TourCode, itemBooking.NguoiNhan, tenNhanVienHienTai);

                        if (isChangeSuccess && isAddSuccess)
                        {
                            success = true;
                        }

                        if (success)
                        {
                            return Json(new { success = true, message = "Bạn đã hủy tour booking thành công!", itemBooking.IDStatus });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Bạn đã hủy tour booking thất bại." });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Không tìm thấy dữ liệu đặt tour." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Gửi mail bị lỗi vui lòng liên hệ IT." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
        [HttpPost]
        public async Task<JsonResult> ChangeBookingStatus(string IDStatus, string TourCode)
        {
            try
            {
                DulichRepository dulichRepository = _unitOfWork_Repository.Dulich_Rep;
                DetailTourModel itemBooking = dulichRepository.NewDetailBooking(TourCode);
                if (itemBooking != null)
                {
                    bool success = false;
                    bool SendMailSuccess = false;

                    itemBooking.IDStatus = IDStatus;
                    success = dulichRepository.ChangeBookingStatus(itemBooking.IDStatus, TourCode);
                    if (success)
                    {
                        if (IDStatus == "7")
                        {
                            SendMailSuccess = dulichRepository.SendMailSuccessBooking(itemBooking.customerEmail, itemBooking.TourCode, itemBooking.LoaiTour);
                            if (SendMailSuccess)
                            {
                                return Json(new { success = true, message = "Bạn đã đổi trạng thái thành công!" });
                            }
                            else
                            {
                                return Json(new { success = true, message = "Gửi mail thất bại!" });
                            }
                        }
                        else
                        {
                            return Json(new { success = true, message = "Bạn đã đổi trạng thái thành công!" });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Bạn đã đổi trạng thái thất bại." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy dữ liệu đặt tour." });
                }


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }


        public IActionResult Dieuchinhhoahong()
        {
            DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;

            List<DCHH> resuft = Dulich_Rep.EditHH();
            return View(resuft);
        }
        [HttpPost]
        public IActionResult CreateConfiguration(string Name, string TiLe, string create)
        {
            DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;

            if (create != null)
            {
                bool result = Dulich_Rep.CreateConfiguration(Name, TiLe);
                if (result)
                {
                    TempData["thongbaoSuccess"] = "Bạn đã lưu cấu hình thành công";
                    List<DCHH> resuft = Dulich_Rep.EditHH();

                    return View("Dieuchinhhoahong", resuft);
                }
                else
                {
                    TempData["thongbaoError"] = "Bạn đã lưu cấu hình thất bại";
                    List<DCHH> resuft = Dulich_Rep.EditHH();

                    return View("Dieuchinhhoahong", resuft);

                }
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public JsonResult XoaCauHinh(int id)
        {
            DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;

            bool result = Dulich_Rep.XoaCauHinh(id);
            return Json(result);
        }
        [HttpPost]
        public IActionResult EditConfiguration(string ID, string Name, string TiLe, string create)
        {
            if (create != null)
            {
                DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;

                bool resuft1 = Dulich_Rep.SaveConfiguration(ID, Name, TiLe);
                if (resuft1)
                {
                    TempData["thongbaoSuccess"] = "Bạn đã lưu cấu hình thành công";
                    List<DCHH> resuft = Dulich_Rep.EditHH();

                    return View("Dieuchinhhoahong", resuft);
                }
                else
                {
                    TempData["thongbaoError"] = "Bạn đã lưu cấu hình thất bại";
                    List<DCHH> resuft = Dulich_Rep.EditHH();

                    return View("Dieuchinhhoahong", resuft);

                }
            }
            else
            {
                DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;

                List<DCHH> resuft = Dulich_Rep.EditConfiguration(ID);
                return View(resuft);
            }
        }


        [HttpPost]
        public async Task<JsonResult> Guimailbooking(string TourCode, string codetrienkhai)
        {
            try
            {
                string token = await GetEnVietToken();
                DulichRepository dulichRepository = _unitOfWork_Repository.Dulich_Rep;
                DetailTourModel dulichData = dulichRepository.NewDetailBooking(TourCode);

                if (dulichData != null)
                {
                    var (success, errorMessage) = await dulichRepository.ApiSendOrder(
                        dulichData.codetrienkhai = codetrienkhai,
                        dulichData.tourID,
                        dulichData.customerName,
                        dulichData.customerPhone,
                        dulichData.customerEmail,
                        dulichData.customerNote,
                        dulichData.hotelTour,
                        dulichData.adultQuantity,
                        dulichData.childQuantity,
                        dulichData.kidQuantity,
                        dulichData.NameTour,
                        dulichData.ngaydi,
                        dulichData.ngayve,
                        dulichData.TourCode,
                        dulichData.commission,
                        dulichData.totalPrice,
                        dulichData.customerNote,
                        dulichData.Vat,
                        dulichData.LoaiTour,
                        dulichData.price,
                        token);

                    if (success)
                    {
                        dulichData.IDStatus = "3";
                        dulichRepository.ChangeBookingStatus(dulichData.IDStatus, TourCode);
                        return Json(new { success = true, message = "Bạn đã giữ chỗ thành công!", dulichData.IDStatus });
                    }
                    else
                    {
                        return Json(new { success = false, message = $"{errorMessage}" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy dữ liệu đặt tour." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }




        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://datanet.datviettour.com.vn/api/tour/get?get_outbound=1";
                    string api_token = "OWMxams3NXVkaWFoenJuNGY4eDN0cHd5c2wyZW1v";

                    string urlWithParams = $"{apiUrl}&api_token={api_token}";

                    var response = await client.GetAsync(urlWithParams);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        ProvinceData apiData = JsonConvert.DeserializeObject<ProvinceData>(responseBody);

                        return View(apiData);
                    }
                    else
                    {
                        return BadRequest($"Lỗi: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        public async Task<IActionResult> TinhThanh()
        {
            DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;

            List<Destination> result = Dulich_Rep.ListDestination();
            return View(result);
        }


        [HttpPost]
        public async Task<IActionResult> GetTinhThanh(List<string> selectedOptions)
        {
            DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;

            List<Destination> allDestinations = Dulich_Rep.ListDestination();
            Dulich_Rep.UpdateAllTrangThaiDestination(0);

            if (selectedOptions != null && selectedOptions.Count > 0)
            {
                foreach (string option in selectedOptions)
                {
                    var item = allDestinations.FirstOrDefault(d => d.TenTinh == option);
                    if (item != null)
                    {
                        item.Trangthai = 1;
                        Dulich_Rep.UpdateTrangThaiDestination(item.IDTinh, item.Trangthai);
                    }
                    else
                    {
                        var trangThai = 0;
                        Dulich_Rep.UpdateAllTrangThaiDestination(trangThai);
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetTinhThanhOther(List<string> selectedOptions)
        {
            DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;

            List<Destination> allDestinations = Dulich_Rep.ListDestination();
            Dulich_Rep.UpdateAllTrangThaiTinhDestination(1);

            if (selectedOptions != null && selectedOptions.Count > 0)
            {
                foreach (string option in selectedOptions)
                {
                    var item = allDestinations.FirstOrDefault(d => d.TenTinh == option);
                    if (item != null)
                    {
                        item.Trangthai = 0;
                        Dulich_Rep.UpdateTrangThaiTinhDestination(item.IDTinh, item.Trangthai);
                    }
                    else
                    {
                        var trangThai = 1;
                        Dulich_Rep.UpdateAllTrangThaiTinhDestination(trangThai);
                    }
                }
            }
            return View();
        }
        public async Task<IActionResult> TinhThanhQT()
        {
            DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;

            List<Destination> result = Dulich_Rep.ListDestinationQT();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetTinhThanhQT(List<string> selectedOptions)
        {
            DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;

            List<Destination> allDestinations = Dulich_Rep.ListDestinationQT();
            Dulich_Rep.UpdateAllTrangThaiDestinationQT(0);

            if (selectedOptions != null && selectedOptions.Count > 0)
            {
                foreach (string option in selectedOptions)
                {
                    var item = allDestinations.FirstOrDefault(d => d.TenTinh == option);
                    if (item != null)
                    {
                        item.Trangthai = 1;
                        Dulich_Rep.UpdateTrangThaiDestinationQT(item.IDTinh, item.Trangthai);
                    }
                    else
                    {
                        var trangThai = 0;
                        Dulich_Rep.UpdateAllTrangThaiDestinationQT(trangThai);
                    }
                }
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetTinhThanhOtherQT(List<string> selectedOptions)
        {
            DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;

            List<Destination> allDestinations = Dulich_Rep.ListDestinationQT();
            Dulich_Rep.UpdateAllTrangThaiTinhDestinationQT(1);

            if (selectedOptions != null && selectedOptions.Count > 0)
            {
                foreach (string option in selectedOptions)
                {
                    var item = allDestinations.FirstOrDefault(d => d.TenTinh == option);
                    if (item != null)
                    {
                        item.Trangthai = 0;
                        Dulich_Rep.UpdateTrangThaiTinhDestinationQT(item.IDTinh, item.Trangthai);
                    }
                    else
                    {
                        var trangThai = 1;
                        Dulich_Rep.UpdateAllTrangThaiTinhDestinationQT(trangThai);
                    }
                }
            }
            return View();
        }
        public IActionResult ListTourHot()
        {
            DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;


            Tourall tourall = new Tourall
            {
                TourlesEV = Dulich_Rep.ListTourEV(),
                Destination = Dulich_Rep.ListDestination()
            };
            List<Destination> listTinhThanh = Dulich_Rep.ListDestination();
            ViewBag.listTinhThanh = listTinhThanh;

            return View(tourall);
        }
        [HttpPost]
        public JsonResult CreateTour(string tour_name, string[] diem_den, string chuyen_bay, string loai_xe, string so_ngay, string so_dem, string loai_tour,
    IFormFile[] files, string ngay_di, string ngay_ve, string ngay_dong_tour, string hdv, string sales_name, string sales_email, string sales_phoneNumber, string[] gia_loai,
    string[] gia_nguoi_lon, string[] gia_tre_em, string[] gia_em_be, string[] phu_thu_don, string[] phu_thu_quoctich, string[] hh_gia_nguoi_lon, string[] hh_gia_tre_em,
    string[] hh_gia_em_be, string[] km_gia_nguoi_lon, string[] km_gia_tre_em, string[] km_gia_em_be, string note, string short_notes, string long_notes,
    List<bool> mainImages, List<IFormFile> imageFiles, string diem_di, string tong, string da_dat, string giu_cho, string diem_don)
        {
            if (string.IsNullOrEmpty(tour_name))
            {
                return Json(new { error = true, message = "Bạn chưa nhập tên tour!" });
            }

            if (string.IsNullOrEmpty(so_ngay))
            {
                return Json(new { error = true, message = "Bạn chưa nhập số ngày!" });
            }
            if (string.IsNullOrEmpty(so_dem))
            {
                return Json(new { error = true, message = "Bạn chưa nhập số đêm!" });
            }
            if (string.IsNullOrEmpty(ngay_di))
            {
                return Json(new { error = true, message = "Bạn chưa nhập ngày đi!" });
            }
            if (string.IsNullOrEmpty(ngay_ve))
            {
                return Json(new { error = true, message = "Bạn chưa nhập ngày về!" });
            }
            if (string.IsNullOrEmpty(short_notes))
            {
                return Json(new { error = true, message = "Bạn chưa nhập ghi chú ngắn!" });
            }
            if (string.IsNullOrEmpty(long_notes))
            {
                return Json(new { error = true, message = "Bạn chưa nhập ghi chú dài!" });
            }
            if (mainImages == null || !mainImages.Contains(true))
            {
                return Json(new { error = true, message = "Bạn phải chọn ảnh đại diện!" });
            }
            if (string.IsNullOrEmpty(diem_don))
            {
                return Json(new { error = true, message = "Bạn chưa nhập điểm đón!" });
            }

            DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;

            bool results = Dulich_Rep.AddTourNoiDia(tour_name, diem_den, chuyen_bay, loai_xe, so_ngay, so_dem, loai_tour, files, ngay_di, ngay_ve, ngay_dong_tour, hdv, sales_name, sales_email,
                sales_phoneNumber, gia_loai, gia_nguoi_lon, gia_tre_em, gia_em_be, phu_thu_don, phu_thu_quoctich, hh_gia_nguoi_lon, hh_gia_tre_em, hh_gia_em_be, km_gia_nguoi_lon,
                km_gia_tre_em, km_gia_em_be, note, short_notes, long_notes, mainImages, imageFiles, diem_di, tong, da_dat, giu_cho, diem_don);

            if (results)
            {
                return Json(new { success = true, message = "Tour được tạo mới thành công!" });
            }
            else
            {
                return Json(new { error = false, message = "Tạo mới thất bại, vui lòng thử lại" });
            }
        }
        public IActionResult BookingTourHot(string FromDate, string ToDate, string searchBtn, int page = 1, int pageSize = 20)
        {
            DulichRepository Dulich_Rep = _unitOfWork_Repository.Dulich_Rep;


            if (searchBtn != null)
            {
                DateTime dFrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider, DateTimeStyles.None);
                DateTime dTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider, DateTimeStyles.None);
                string dateFrom = dFrom.ToString("yyyy-MM-dd");
                string dateTo = dTo.ToString("yyyy-MM-dd");
                ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
                ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
                List<BookingInfoModel> resuft = Dulich_Rep.SearchBookingTourHot(dFrom.ToString("MM/dd/yyyy"), dTo.ToString("MM/dd/yyyy"), page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalItems = Dulich_Rep.GetTotalSearchBookingsCountTourHot(dFrom.ToString("MM/dd/yyyy"), dTo.ToString("MM/dd/yyyy"));
                ViewBag.IsSearch = true;
                return View(resuft);
            }
            else
            {
                List<BookingInfoModel> resuft = Dulich_Rep.ListBookingTourHot(page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalItems = Dulich_Rep.GetTotalBookingsCountTourHot();
                ViewBag.IsSearch = false;
                return View(resuft);
            }
        }
        public async Task<IActionResult> Chitietbookingtourhot(string tourcode, string tourcodetrienkhai)
        {
            try
            {
                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                DulichRepository dulichRepository = _unitOfWork_Repository.Dulich_Rep;

                // Retrieve booking details
                var itemBooking = dulichRepository.NewDetailBookingTourHot(tourcodetrienkhai);
                if (itemBooking == null)
                {
                    return BadRequest("Booking details not found.");
                }

                // Check if current user matches and update status
                List<ListNhanVienDuLich> DSdulich = dulichRepository.DSDuLich();
                bool userFound = false;
                foreach (var danhSach in DSdulich)
                {
                    if (danhSach.Ten == acc.HoTen)
                    {
                        userFound = true;
                        if (itemBooking.IDStatus == "1")
                        {
                            TempData["thongbaoInfo"] = "Mới";
                            ViewBag.code = tourcodetrienkhai;
                            itemBooking.IDStatus = "2";
                            dulichRepository.ChangeBookingStatusTourHot(itemBooking.IDStatus, tourcodetrienkhai);
                            var isAddSuccess = dulichRepository.AddNguoiNhanTourHot(tourcodetrienkhai, acc.HoTen, "");
                            itemBooking.NguoiNhan = acc.HoTen;
                        }
                    }
                }

                // Retrieve tour details
                TourEV DetailSPTourHot = dulichRepository.DetailTourEV(tourcode);
                if (DetailSPTourHot == null)
                {
                    return BadRequest("Tour details not found.");
                }

                // Retrieve booking details
                DetailTourHotBooking DetailBookingTourHot = dulichRepository.GetBookingDetail(tourcodetrienkhai);
                if (DetailBookingTourHot == null)
                {
                    return BadRequest("Booking details not found.");
                }

                // Assign booking details to tour details
                DetailSPTourHot.DetailTourBooking = DetailBookingTourHot;

                // Filter the prices based on the HotelTour value
                if (DetailSPTourHot.Gias != null)
                {
                    var filteredGias = DetailSPTourHot.Gias
                        .Where(g => g.loai_gia == DetailBookingTourHot.HotelTour)
                        .ToList();

                    DetailSPTourHot.Gias = filteredGias;
                }

                return View(DetailSPTourHot);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<JsonResult> ChuyenBookingTourHot(string TourCode, string NguoiNhan)
        {
            try
            {
                DulichRepository dulichRepository = _unitOfWork_Repository.Dulich_Rep;
                DetailTourModel itemBooking = dulichRepository.NewDetailBookingTourHot(TourCode);

                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                string tenNhanVienHienTai = acc.HoTen;


                if (itemBooking != null)
                {
                    itemBooking.NguoiNhan = NguoiNhan;

                    bool success = false;

                    var isChangeSuccess = dulichRepository.ChangeBookingStatusTourHot(itemBooking.IDStatus, TourCode);
                    var isAddSuccess = dulichRepository.AddNguoiNhanTourHot(TourCode, itemBooking.NguoiNhan, tenNhanVienHienTai);

                    if (isChangeSuccess && isAddSuccess)
                    {
                        success = true;
                    }

                    if (success)
                    {
                        return Json(new { success = true, message = "Bạn đã chuyển thành công!", itemBooking.IDStatus });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Bạn đã chuyển thất bại." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy dữ liệu đặt tour." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
        [HttpPost]
        public async Task<JsonResult> GuimailbookingTourHot(string TourCode)
        {
            try
            {
                string token = await GetEnVietToken();
                DulichRepository dulichRepository = _unitOfWork_Repository.Dulich_Rep;
                DetailTourModel dulichData = dulichRepository.NewDetailBookingTourHot(TourCode);
                if (dulichData.tourID != "" && dulichData.tourID != null)
                {
                    bool sendmail = dulichRepository.SendMailBookingTourHot(dulichData.tourSP, dulichData.tourID, dulichData.customerName, dulichData.customerPhone, dulichData.customerEmail, dulichData.customerNote, dulichData.hotelTour, dulichData.adultQuantity, dulichData.childQuantity, dulichData.kidQuantity, dulichData.TourName, dulichData.ngaydi, dulichData.ngayve, dulichData.tourID, dulichData.commission, dulichData.totalPrice, dulichData.ghi_chu, dulichData.LoaiTour, dulichData.price, dulichData.Vat);
                    if (sendmail)
                    {
                        dulichData.IDStatus = "3";
                        dulichRepository.ChangeBookingStatusTourHot(dulichData.IDStatus, TourCode);
                        return Json(new { success = true, message = "Gửi booking thành công", idStatus = dulichData.IDStatus });

                    }
                    else
                    {
                        return Json(new { success = false, message = "Gửi booking thất bại" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Gửi booking thất bại" });
                }


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
        public async Task<JsonResult> ChangeBookingStatusTourHot(string IDStatus, string TourCode)
        {
            try
            {
                DulichRepository dulichRepository = _unitOfWork_Repository.Dulich_Rep;
                DetailTourModel itemBooking = dulichRepository.NewDetailBookingTourHot(TourCode);
                if (itemBooking != null)
                {
                    bool success = false;
                    bool SendMailSuccess = false;

                    itemBooking.IDStatus = IDStatus;
                    success = dulichRepository.ChangeBookingStatusTourHot(itemBooking.IDStatus, TourCode);
                    if (success)
                    {
                        if (IDStatus == "7")
                        {
                            SendMailSuccess = dulichRepository.SendMailSuccessBookingTourHot(itemBooking.customerEmail, itemBooking.TourCode, itemBooking.LoaiTour);
                            if (SendMailSuccess)
                            {
                                return Json(new { success = true, message = "Bạn đã đổi trạng thái thành công!" });
                            }
                            else
                            {
                                return Json(new { success = true, message = "Gửi mail thất bại!" });
                            }
                        }
                        else
                        {
                            return Json(new { success = true, message = "Bạn đã đổi trạng thái thành công!" });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Bạn đã đổi trạng thái thất bại." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy dữ liệu đặt tour." });
                }


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
        public async Task<JsonResult> CancelBookingTourHot(string cancellationReason, string TourCode)
        {
            try
            {
                DulichRepository dulichRepository = _unitOfWork_Repository.Dulich_Rep;
                DetailTourModel dulichData = dulichRepository.NewDetailBookingTourHot(TourCode);

                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                string tenNhanVienHienTai = acc.HoTen;
                var itemBooking = dulichData;
                bool resuft = dulichRepository.MailCancelBoookingTourHot(
                    dulichData.LoaiTour,
                    dulichData.customerName,
                    dulichData.TourCode,
                    dulichData.TourName,
                    dulichData.tourID,
                    dulichData.ngaydi,
                    dulichData.ngayve,
                    dulichData.hotelTour,
                    dulichData.adultQuantity,
                    dulichData.childQuantity,
                    dulichData.kidQuantity,
                    dulichData.Namecompany,
                    dulichData.MaKH,
                    dulichData.customerPhone,
                    dulichData.customerEmail,
                    dulichData.customerNote,
                    dulichData.commission,
                    dulichData.totalPrice,
                    dulichData.MaSoThue,
                    dulichData.TenCaNhanToChuc,
                    dulichData.DiaChi,
                    dulichData.price,
                    dulichData.Vat,
                    cancellationReason
                );
                bool saveLationReason = dulichRepository.SaveLationReasonTourHot(TourCode, cancellationReason);
                if (resuft)
                {
                    if (itemBooking != null)
                    {
                        bool success = false;


                        itemBooking.IDStatus = "6"; // Trạng thái huỷ booking có status = 6

                        var isChangeSuccess = dulichRepository.ChangeBookingStatusTourHot(itemBooking.IDStatus, TourCode);
                        var isAddSuccess = dulichRepository.AddNguoiNhanTourHot(TourCode, itemBooking.NguoiNhan, tenNhanVienHienTai);

                        if (isChangeSuccess && isAddSuccess)
                        {
                            success = true;
                        }

                        if (success)
                        {
                            return Json(new { success = true, message = "Bạn đã hủy tour booking thành công!", itemBooking.IDStatus });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Bạn đã hủy tour booking thất bại." });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Không tìm thấy dữ liệu đặt tour." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Gửi mail bị lỗi vui lòng liên hệ IT." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        public IActionResult EditTourHot(string Tour_Id)
        {
            DulichRepository dulichRepository = _unitOfWork_Repository.Dulich_Rep;
            try
            {
                Tourall tourall = new Tourall
                {
                    TourlesEV = new List<TourEV> { dulichRepository.DetailTourEV(Tour_Id) },
                    Destination = dulichRepository.ListDestination()
                };
                List<Destination> listTinhThanh = dulichRepository.ListDestination();
                ViewBag.listTinhThanh = listTinhThanh;

                return PartialView("Partial_EditTour", tourall);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }



    }
}
