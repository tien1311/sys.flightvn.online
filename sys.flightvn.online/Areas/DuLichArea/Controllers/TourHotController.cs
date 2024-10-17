using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.Repository;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Manager_EV.Areas.DuLichArea.Controllers
{
    [Area(AreaNameConst.AREA_DuLich)]

    public class TourHotController : Controller
    {
        private readonly IConfiguration _configuration;
        CultureInfo provider;
        public TourHotController(IConfiguration configuration)
        {

            _configuration = configuration;
        }
        public IActionResult AddTourHot()
        {
            TourHotRepository TourHot_Rep = new TourHotRepository(_configuration);
            List<Destination> listTinhThanh = TourHot_Rep.ListDestination();
            List<Destination> listTinhThanhQT = TourHot_Rep.ListDestinationQT();
            List<FlagModel> listFlag = TourHot_Rep.ListFlag();
            ViewBag.listTinhThanh = listTinhThanh;
            ViewBag.listFlag = listFlag;
            ViewBag.listTinhThanhQT = listTinhThanhQT;
            return View();
        }
        public IActionResult ListTourHot(string tentour, string selectedDate)
        {
            TourHotRepository TourHot_Rep = new TourHotRepository(_configuration);
            List<Destination> listTinhThanh = TourHot_Rep.ListDestination();
            ViewBag.listTinhThanh = listTinhThanh;

            if (tentour == null)
            {
                List<TourEV> listour = TourHot_Rep.GetAllListTourHot();
                return View(listour);
            }

            if (selectedDate == null)
            {
                List<TourEV> listour = TourHot_Rep.SearchIDTourHot(tentour);
                return View(listour);
            }
            else
            {
                // Convert selectedDate from yyyy-MM to MM/yyyy
                string formattedDate = null;
                DateTime parsedDate;
                if (DateTime.TryParseExact(selectedDate, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    formattedDate = parsedDate.ToString("MM/yyyy");
                }

                List<TourEV> listour = TourHot_Rep.SearchDayAndIDTourHot(tentour, formattedDate);
                return View(listour);
            }
        }


        [HttpPost]
        public IActionResult EditTourHot(string Tour_Id)
        {
            TourHotRepository TourHot_Rep = new TourHotRepository(_configuration);
            try
            {
                TourEV DetailSPTourHot = TourHot_Rep.DetailTourEV(Tour_Id);
                List<Destination> listTinhThanh = TourHot_Rep.ListDestination();
                List<FlagModel> listFlag = TourHot_Rep.ListFlag();
                ViewBag.listTinhThanh = listTinhThanh;
                ViewBag.listFlag = listFlag;
                return View(DetailSPTourHot);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
        [HttpPost]
        public JsonResult DeleteImg(int id)
        {
            TourHotRepository TourHot_Rep = new TourHotRepository(_configuration);
            try
            {
                bool result = TourHot_Rep.DeleteImg(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CreateTour(
     string tour_name, string[] diem_den, string chuyen_bay, string loai_xe, string so_ngay, string so_dem, string loai_tour,
     IFormFile[] files, string ngay_di, string ngay_ve, string ngay_dong_tour, string hdv, string sales_name, string sales_email, string sales_phoneNumber, string[] gia_loai,
     string[] gia_nguoi_lon, string[] gia_tre_em, string[] gia_em_be, string[] phu_thu_don, string[] phu_thu_quoctich, string[] hh_gia_nguoi_lon, string[] hh_gia_tre_em,
     string[] hh_gia_em_be, string[] km_gia_nguoi_lon, string[] km_gia_tre_em, string[] km_gia_em_be, string note, string short_notes, string long_notes,
     List<bool> mainImages, List<IFormFile> imageFiles, string diem_di, string tong, string da_dat, string giu_cho, string diem_don, string gan_co)
        {
            if (string.IsNullOrEmpty(tour_name))
            {
                return Json(new { error = true, message = "Bạn chưa nhập tên tour!" });
            }

            if (diem_den == null || diem_den.Length == 0)
            {
                return Json(new { error = true, message = "Bạn chưa nhập điểm đến!" });
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

            if (files == null || files.Length == 0)
            {
                return Json(new { error = true, message = "Bạn chưa tải lên file chương trình!" });
            }

            if (gia_loai == null || gia_loai.Length == 0)
            {
                return Json(new { error = true, message = "Bạn chưa nhập loại giá!" });
            }

            if (!DateTime.TryParse(ngay_di, out DateTime parsedNgayDi) ||
                !DateTime.TryParse(ngay_ve, out DateTime parsedNgayVe))
            {
                return Json(new { error = true, message = "Ngày đi hoặc ngày về không hợp lệ!" });
            }

            TourHotRepository TourHot_Rep = new TourHotRepository(_configuration);
            bool results = TourHot_Rep.AddTourHot(tour_name, diem_den, chuyen_bay, loai_xe, so_ngay, so_dem, loai_tour, files, ngay_di, ngay_ve, ngay_dong_tour, hdv, sales_name, sales_email,
                sales_phoneNumber, gia_loai, gia_nguoi_lon, gia_tre_em, gia_em_be, phu_thu_don, phu_thu_quoctich, hh_gia_nguoi_lon, hh_gia_tre_em, hh_gia_em_be, km_gia_nguoi_lon,
                km_gia_tre_em, km_gia_em_be, note, short_notes, long_notes, mainImages, imageFiles, diem_di, tong, da_dat, giu_cho, diem_don, gan_co);

            if (results)
            {
                return Json(new { success = true, message = "Tour được tạo mới thành công!" });
            }
            else
            {
                return Json(new { error = true, message = "Tạo mới thất bại, vui lòng thử lại" });
            }
        }

        [HttpPost]
        public JsonResult EditTour(string tour_name, string[] diem_den, string chuyen_bay, string loai_xe, string so_ngay, string so_dem, string loai_tour,
    IFormFile[] files, string ngay_di, string ngay_ve, string ngay_dong_tour, string hdv, string sales_name, string sales_email, string sales_phoneNumber, string[] gia_loai,
    string[] gia_nguoi_lon, string[] gia_tre_em, string[] gia_em_be, string[] phu_thu_don, string[] phu_thu_quoctich, string[] hh_gia_nguoi_lon, string[] hh_gia_tre_em,
    string[] hh_gia_em_be, string[] km_gia_nguoi_lon, string[] km_gia_tre_em, string[] km_gia_em_be, string note, string short_notes, string long_notes_edit,
    List<bool> mainImages, List<IFormFile> imageFiles, List<bool> mainImgs, List<string> imagesURL, string diem_di, string tong, string da_dat, string giu_cho, string diem_don, string Tour_id, string gan_co)
        {
            if (string.IsNullOrEmpty(tour_name))
            {
                return Json(new { error = true, message = "Bạn chưa nhập tên tour!" });
            }

            if (diem_den == null || diem_den.Length == 0)
            {
                return Json(new { error = true, message = "Bạn chưa nhập điểm đến!" });
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

            if (string.IsNullOrEmpty(long_notes_edit))
            {
                return Json(new { error = true, message = "Bạn chưa nhập ghi chú dài!" });
            }

            if (string.IsNullOrEmpty(diem_don))
            {
                return Json(new { error = true, message = "Bạn chưa nhập điểm đón!" });
            }

            if (gia_loai == null || gia_loai.Length == 0)
            {
                return Json(new { error = true, message = "Bạn chưa nhập loại giá!" });
            }

            if (!DateTime.TryParse(ngay_di, out DateTime parsedNgayDi) ||
                !DateTime.TryParse(ngay_ve, out DateTime parsedNgayVe))
            {
                return Json(new { error = true, message = "Ngày đi hoặc ngày về không hợp lệ!" });
            }

            TourHotRepository TourHot_Rep = new TourHotRepository(_configuration);
            bool results = TourHot_Rep.EditTourHot(tour_name, diem_den, chuyen_bay, loai_xe, so_ngay, so_dem, loai_tour, files, ngay_di, ngay_ve, ngay_dong_tour, hdv, sales_name, sales_email,
                sales_phoneNumber, gia_loai, gia_nguoi_lon, gia_tre_em, gia_em_be, phu_thu_don, phu_thu_quoctich, hh_gia_nguoi_lon, hh_gia_tre_em, hh_gia_em_be, km_gia_nguoi_lon,
                km_gia_tre_em, km_gia_em_be, note, short_notes, long_notes_edit, mainImages, imageFiles, mainImgs, imagesURL, diem_di, tong, da_dat, giu_cho, diem_don, Tour_id, gan_co);

            if (results)
            {
                return Json(new { success = true, message = "Cập nhật tour thành công !" });
            }
            else
            {
                return Json(new { error = true, message = "Cập nhật tour thất bại !" });
            }
        }


        public IActionResult BookingTourHot(string FromDate, string ToDate, string searchBtn, int page = 1, int pageSize = 20)
        {
            TourHotRepository TourHot_Rep = new TourHotRepository(_configuration);

            if (searchBtn != null)
            {
                DateTime dFrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider, DateTimeStyles.None);
                DateTime dTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider, DateTimeStyles.None);
                string dateFrom = dFrom.ToString("yyyy-MM-dd");
                string dateTo = dTo.ToString("yyyy-MM-dd");
                ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
                ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
                List<BookingInfoModel> resuft = TourHot_Rep.SearchBookingTourHot(dFrom.ToString("MM/dd/yyyy"), dTo.ToString("MM/dd/yyyy"), page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalItems = TourHot_Rep.GetTotalSearchBookingsCountTourHot(dFrom.ToString("MM/dd/yyyy"), dTo.ToString("MM/dd/yyyy"));
                ViewBag.IsSearch = true;
                return View(resuft);
            }
            else
            {
                List<BookingInfoModel> resuft = TourHot_Rep.ListBookingTourHot(page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalItems = TourHot_Rep.GetTotalBookingsCountTourHot();
                ViewBag.IsSearch = false;
                return View(resuft);
            }
        }
        public async Task<IActionResult> Chitietbookingtourhot(string tourcode, string tourcodetrienkhai)
        {
            try
            {
                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                TourHotRepository TourHot_Rep = new TourHotRepository(_configuration);

                // Retrieve booking details
                var itemBooking = TourHot_Rep.NewDetailBookingTourHot(tourcodetrienkhai);
                if (itemBooking == null)
                {
                    return BadRequest("Booking details not found.");
                }

                // Check if current user matches and update status
                List<ListNhanVienDuLich> DSdulich = TourHot_Rep.DSDuLich();
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
                            TourHot_Rep.ChangeBookingStatusTourHot(itemBooking.IDStatus, tourcodetrienkhai);
                            var isAddSuccess = TourHot_Rep.AddNguoiNhanTourHot(tourcodetrienkhai, acc.HoTen, "");
                            itemBooking.NguoiNhan = acc.HoTen;
                        }
                    }
                }

                // Retrieve tour details
                TourEV DetailSPTourHot = TourHot_Rep.DetailTourEV(tourcode);
                if (DetailSPTourHot == null)
                {
                    return BadRequest("Tour details not found.");
                }

                // Retrieve booking details
                DetailTourHotBooking DetailBookingTourHot = TourHot_Rep.GetBookingDetail(tourcodetrienkhai);
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
                TourHotRepository TourHot_Rep = new TourHotRepository(_configuration);
                DetailTourModel itemBooking = TourHot_Rep.NewDetailBookingTourHot(TourCode);

                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                string tenNhanVienHienTai = acc.HoTen;


                if (itemBooking != null)
                {
                    itemBooking.NguoiNhan = NguoiNhan;

                    bool success = false;

                    var isChangeSuccess = TourHot_Rep.ChangeBookingStatusTourHot(itemBooking.IDStatus, TourCode);
                    var isAddSuccess = TourHot_Rep.AddNguoiNhanTourHot(TourCode, itemBooking.NguoiNhan, tenNhanVienHienTai);

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

                TourHotRepository TourHot_Rep = new TourHotRepository(_configuration);
                DetailTourModel dulichData = TourHot_Rep.NewDetailBookingTourHot(TourCode);
                if (dulichData.tourID != "" && dulichData.tourID != null)
                {
                    dulichData.IDStatus = "3";
                    TourHot_Rep.ChangeBookingStatusTourHot(dulichData.IDStatus, TourCode);
                    return Json(new { success = true, message = "Gửi booking thành công", idStatus = dulichData.IDStatus });
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
                TourHotRepository TourHot_Rep = new TourHotRepository(_configuration);
                DetailTourModel itemBooking = TourHot_Rep.NewDetailBookingTourHot(TourCode);
                if (itemBooking != null)
                {
                    bool success = false;
                    bool SendMailSuccess = false;

                    itemBooking.IDStatus = IDStatus;
                    success = TourHot_Rep.ChangeBookingStatusTourHot(itemBooking.IDStatus, TourCode);
                    if (success)
                    {
                        if (IDStatus == "7")
                        {
                            //SendMailSuccess = TourHot_Rep.SendMailSuccessBookingTourHot(itemBooking.customerEmail, itemBooking.TourCode, itemBooking.LoaiTour, itemBooking.customerNote);
                            //if (SendMailSuccess)
                            //{
                                return Json(new { success = true, message = "Bạn đã đổi trạng thái thành công!" });
                            //}
                            //else
                            //{
                            //    return Json(new { success = true, message = "Gửi mail thất bại!" });
                            //}
                        }
                        if (IDStatus == "3")
                        {


                            DetailTourModel dulichData = TourHot_Rep.NewDetailBookingTourHot(TourCode);
                            //bool sendmail = TourHot_Rep.SendMailBookingTourHot(dulichData.tourSP, dulichData.tourID, dulichData.customerName, dulichData.customerPhone, dulichData.customerEmail, dulichData.customerNote, dulichData.hotelTour, dulichData.adultQuantity, dulichData.childQuantity, dulichData.kidQuantity, dulichData.TourName, dulichData.ngaydi, dulichData.ngayve, dulichData.tourID, dulichData.commission, dulichData.totalPrice, dulichData.ghi_chu, dulichData.LoaiTour, dulichData.price, dulichData.Vat);
                            //if (sendmail)
                            //{
                                TourHot_Rep.ChangeBookingStatusTourHot(dulichData.IDStatus, TourCode);
                                return Json(new { success = true, message = "Gửi booking thành công", idStatus = dulichData.IDStatus });
                            //}
                            //else
                            //{
                            //    return Json(new { success = false, message = "Gửi booking thất bại" });
                            //}
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
                TourHotRepository TourHot_Rep = new TourHotRepository(_configuration);
                DetailTourModel dulichData = TourHot_Rep.NewDetailBookingTourHot(TourCode);

                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                string tenNhanVienHienTai = acc.HoTen;
                var itemBooking = dulichData;
             
                bool saveLationReason = TourHot_Rep.SaveLationReasonTourHot(TourCode, cancellationReason);
                
                if (itemBooking != null)
                {
                    bool success = false;


                    itemBooking.IDStatus = "6"; // Trạng thái huỷ booking có status = 6

                    var isChangeSuccess = TourHot_Rep.ChangeBookingStatusTourHot(itemBooking.IDStatus, TourCode);
                    var isAddSuccess = TourHot_Rep.AddNguoiNhanTourHot(TourCode, itemBooking.NguoiNhan, tenNhanVienHienTai);

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
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }



        [HttpPost]
        public JsonResult ChangeActiveTourHot(int Tour_Id, int Active)
        {
            TourHotRepository TourHot_Rep = new TourHotRepository(_configuration);
            bool result = TourHot_Rep.ChangeActiveTourHot(Tour_Id, Active);
            return Json(result);
        }

    }
}
