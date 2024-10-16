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
using EasyInvoice.Client;
using System.Linq;
using Manager.Model.Models.ViewModel;
using Manager.Model.Models;
using Manager.Model.Services.Abstraction;
using Manager.Model.Models.Other;
using Manager.DataAccess.Repository;
using Manager.Model.Services.Notification.Request;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace Manager_EV.Areas.KinhDoanhArea.Controllers
{
    [Area(AreaNameConst.AREA_KinhDoanh)]
    public class KinhDoanhController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private IConfiguration _configuration;
        private INotifyService _notifyService;
        private IUnitOfWork_Repository _unitOfWork_Repository;
        //ThongTinHDRepository _unitOfWork_Repository.ThongTinHD_Rep = new ThongTinHDRepository();

        CultureInfo provider;
        public KinhDoanhController(IHostingEnvironment environment, IConfiguration configuration, INotifyService notifyService, IUnitOfWork_Repository unitOfWork_Repository)
        {
            _hostingEnvironment = environment;
            _configuration = configuration;
            _notifyService = notifyService;
            _unitOfWork_Repository = unitOfWork_Repository;          
        }
        public IActionResult Index_KD()
        {
            return View();
        }

        public IActionResult TraCuuHopDong(string MaKH, string tungay, string denngay)
        {
            DSHopDongDaiLy result = new DSHopDongDaiLy();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            string dateFrom = "", dateTo = "";
            if (tungay != null && tungay != "")
            {
                provider = CultureInfo.InvariantCulture;
                //format lại ngày 
                DateTime dFrom = DateTime.ParseExact(tungay, "dd/MM/yyyy", provider, DateTimeStyles.None);
                DateTime dTo = DateTime.ParseExact(denngay, "dd/MM/yyyy", provider, DateTimeStyles.None);
                //Chuyển lại thành string để truyền vào
                dateFrom = dFrom.ToString("yyyy-MM-dd");
                dateTo = dTo.ToString("yyyy-MM-dd");
            }


            result = _unitOfWork_Repository.ThongTinHD_Rep.SearchThongTinHD(acc.MaNV, MaKH, dateFrom, dateTo);
            return View("TraCuuHopDong", result);
        }

        public IActionResult News(int? page = 1, int pageSize = 7)
        {
            try
            {
                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                //Hết token

                ViewBag.tieude = "Bản tin kinh doanh";
                List<SubjectModel> list = _unitOfWork_Repository.BangTin_Rep.BangTin();

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "News";
                model.RouteValue = new RouteValueDictionary {
                        { "i", 10}
                    };
                return View("News", model);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IActionResult GuiMailDaiLy_KD(string saveBtn, string searchBtn, string MAKH, string DAILY, string MAIL, string MAILCC, IFormFile[] files, string SOPHIEU, string noiDungTong)
        {

            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            GuiMailDaiLyRepository guimail_Rep = _unitOfWork_Repository.GuiMail_DaiLy_Rep;
            GuiMailDaiLyModel result = new GuiMailDaiLyModel();
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (searchBtn != null)
            {
                result = guimail_Rep.SearchMaKH_KD(MAKH);
                return View("GuiMailDaiLy_KD", result);
            }
            if (saveBtn != null)
            {
                if (MAKH == null || MAKH == "")
                {
                    TempData["thongbaoError"] = "Mã KH không được để trống";
                    return View("GuiMailDaiLy_KD", result);
                }
                if (DAILY == null || DAILY == "")
                {
                    TempData["thongbaoError"] = "Tên đại lý không được để trống";
                    return View("GuiMailDaiLy_KD", result);
                }

                bool result_sendmail = guimail_Rep.SendMailKinhdoanhEV(acc.Ten, acc.Email, acc.DienThoai, acc.TenDangNhap, MAKH, DAILY, MAIL, MAILCC, SOPHIEU, files, _hostingEnvironment.WebRootPath, acc.TenDangNhap, noiDungTong);
                if (result_sendmail == true)
                {
                    TempData["thongbaoSuccess"] = "Mail của bạn đã được gửi thành công";
                }
                else
                {
                    TempData["thongbaoError"] = "Gửi mail thất bại";
                }

            }
            result = guimail_Rep.Guimaildaili();
            return View("GuiMailDaiLy_KD", result);
        }


        /// <summary>
        /// Render tieu de
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Tieude()
        {

            return PartialView("Tieude", new ListTieuDe());
        }

        public IActionResult QuyDinh(int? page = 1, int pageSize = 7)
        {
            try
            {

                ViewBag.tieude = "QUY ĐỊNH PHÒNG KINH DOANH";
                List<SubjectModel> list = _unitOfWork_Repository.BangTin_Rep.BangTin();

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "QuyDinh";
                model.RouteValue = new RouteValueDictionary {
                    { "i", 10}
                };
                return View("News", model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult DetailNews(int subject_id)
        {
            SubjectModel content = _unitOfWork_Repository.BangTin_Rep.ContentKD(subject_id);

            return View("DetailNews", content);
        }
        public IActionResult NewDetailNews()
        {
            SubjectModel content = _unitOfWork_Repository.BangTin_Rep.ContentKDNew();

            return View("DetailNews", content);
        }
        public IActionResult DaiLy()
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.ACC = acc.MaNV;
            DanhSachDaiLy content = _unitOfWork_Repository.DaiLy_Rep.ListKinhDoanh();
            return View("DaiLy", content);
        }
        public IActionResult SearchDaiLy(string MaNV, string DieuKien, string GiaTri, string search_KH, string search_All)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.ACC = acc.MaNV;
            DanhSachDaiLy content = new DanhSachDaiLy();
            if (MaNV == null)
            {
                content = _unitOfWork_Repository.DaiLy_Rep.ListDaiLy(acc.MaNV, DieuKien, GiaTri, search_KH, search_All);
            }
            else
            {
                content = _unitOfWork_Repository.DaiLy_Rep.ListDaiLy(MaNV, DieuKien, GiaTri, search_KH, search_All);
            }
            return View("DaiLy", content);
        }
        [HttpPost]
        public IActionResult ThongTinCodeSignIn(string khoachinh)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.ACC = acc.MaNV;
            List<CodeSignIn> content = _unitOfWork_Repository.DaiLy_Rep.ListCodeSignin(khoachinh);
            return PartialView("ThongTinCodeSignIn", content);
        }
        public IActionResult ThongTinDoanhSo(string MaKH, string Nam)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string Thang = null;
            if (Nam == null)
            {
                Nam = DateTime.Now.Year.ToString();
            }
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            List<ImportDoanhSoViewModel> content = _unitOfWork_Repository.ImportDoanhSo_Rep.TraCuuDoanhSo(Thang, Nam, MaKH);
            //if(content.Count <= 0)
            //{
            //    content = ImportDoanhSo_Rep.TraCuuDoanhSo(Thang,(int.Parse(Nam) - 1).ToString(), MaKH);
            //}
            ViewBag.MAKH = MaKH;
            return PartialView("ThongTinDoanhSo", content);
        }


        public JsonResult SearchDoanhSo(string MaKH, string Nam)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string Thang = null;
            if (Nam == null)
            {
                Nam = DateTime.Now.Year.ToString();
            }

            List<ImportDoanhSoViewModel> content = _unitOfWork_Repository.ImportDoanhSo_Rep.TraCuuDoanhSo(Thang, Nam, MaKH);
            return Json(content);
        }
        public IActionResult Chart()
        {
            return View();
        }
        public IActionResult SearchChart(string MaKH, string Nam)
        {
            if (MaKH != null && Nam != null)
            {
                List<ImportDoanhSoViewModel> ListDoanhSo = _unitOfWork_Repository.Chart_Rep.GetDoanhSo(MaKH, Nam);
                return View("Chart", ListDoanhSo);
            }
            else
            {
                TempData["thongbaoError"] = "Bạn phải nhập mã KH và năm !";
                return View("Chart");
            }
            return View("Chart");
        }

        public IActionResult DoanhSoDaiLy(string thang, string nam, string MaNV)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.ACC = acc.MaNV;
            ImportDoanhSoRepository DoanhSo_Rep = _unitOfWork_Repository.ImportDoanhSo_Rep;
            DoanhSoViewModel content = new DoanhSoViewModel();
            if (MaNV == null)
            {
                content = DoanhSo_Rep.DoanhSo(acc.MaNV, thang, nam);
            }
            else
            {
                content = DoanhSo_Rep.DoanhSo(MaNV, thang, nam);
            }
            return View(content);
        }
        [HttpGet]
        public JsonResult GetNoiDung(int khoachinh)
        {
            ListTieuDe result = new ListTieuDe();
            result = _unitOfWork_Repository.ImportDoanhSo_Rep.GetNoiDung(khoachinh);
            return Json(result);
        }
        public IActionResult DanhSachGuiMailKinhDoanh()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DanhSachGuiMailKinhDoanh(string cal_from, string cal_to)
        {
            provider = CultureInfo.InvariantCulture;
            GuiMailDaiLyModel result = new GuiMailDaiLyModel();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
            DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
            string dateFrom = dFrom.ToString("yyyy-MM-dd");
            string dateTo = dTo.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
            ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
            if (acc.MaNV == "NV00016" || acc.MaNV == "NV00352")
            {
                result = _unitOfWork_Repository.ImportDoanhSo_Rep.DanhSachMailAll(dateFrom, dateTo);
            }
            else
            {
                result = _unitOfWork_Repository.ImportDoanhSo_Rep.DanhSachMail(acc.TenDangNhap, dateFrom, dateTo);
            }

            //
            return View("DanhSachGuiMailKinhDoanh", result);
        }
        [HttpPost]
        public IActionResult NoiDungMailKinhDoanh(string khoachinh)
        {
            GuiMailDaiLytModel result = new GuiMailDaiLytModel();

            result = _unitOfWork_Repository.GuiMail_DaiLy_Rep.MaiKinhDoanh(khoachinh);

            return View(result);
        }
        public IActionResult DanhSachIDBooker()
        {
            List<INFO_IDBookerModel> list = _unitOfWork_Repository.BookerClub_Rep.ListIDBooker();
            ViewBag.select = "MaKH";
            ViewBag.value = "";
            return View(list);
        }
        public IActionResult SearchIDBooker(string select, string value, string Search)
        {
            if (Search == "Search")
            {
                List<INFO_IDBookerModel> list = _unitOfWork_Repository.BookerClub_Rep.SearchIDBooker(select, value);
                ViewBag.select = select;
                ViewBag.value = value;
                return View("DanhSachIDBooker", list);
            }
            else
            {
                return ExportExcelIDBooker(select, value);
            }

        }
        [HttpPost]
        public IActionResult ExportExcelIDBooker(string select, string value)
        {
            BookerClubRepository BookerClub_Rep = _unitOfWork_Repository.BookerClub_Rep;
            List<INFO_IDBookerModel> result = new List<INFO_IDBookerModel>();
            if (value != null)
            {
                result = BookerClub_Rep.SearchIDBooker(select, value);
            }
            else
            {
                result = BookerClub_Rep.ListIDBooker();
            }

            byte[] fileContents;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("IDBooker");

            ws.Cells.Style.Font.Name = "Times New Roman";
            ws.Cells.Style.Font.Size = 12;
            int rowHeader = 1;
            ws.Cells["A" + rowHeader].Value = "STT";
            ws.Cells["B" + rowHeader].Value = "Ngay up";
            ws.Cells["C" + rowHeader].Value = "MaKH";
            ws.Cells["D" + rowHeader].Value = "Ten dai ly";
            ws.Cells["E" + rowHeader].Value = "NVKD";
            ws.Cells["F" + rowHeader].Value = "ID Booker";
            ws.Cells["G" + rowHeader].Value = "Ho tên";
            ws.Cells["H" + rowHeader].Value = "SDT";
            ws.Cells["I" + rowHeader].Value = "STK";
            ws.Cells["J" + rowHeader].Value = "Ngan hang";
            ws.Cells["K" + rowHeader].Value = "Chu tai khoan";
            ws.Cells["A" + rowHeader + ":K" + rowHeader].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells["A" + rowHeader + ":K" + rowHeader].Style.Fill.BackgroundColor.SetColor(Color.Green);
            ws.Cells["A" + rowHeader + ":K" + rowHeader].Style.Font.Bold = true;
            ws.Cells["A" + rowHeader + ":K" + rowHeader].Style.Font.Color.SetColor(Color.White);
            ws.Column(1).Width = 10;
            ws.Column(2).Width = 20;
            ws.Column(3).Width = 10;
            ws.Column(4).Width = 40;
            ws.Column(5).Width = 20;
            ws.Column(6).Width = 20;
            ws.Column(7).Width = 30;
            ws.Column(8).Width = 20;
            ws.Column(9).Width = 20;
            ws.Column(10).Width = 20;
            ws.Column(11).Width = 40;
            int rowStart = rowHeader + 1;
            int STT = 1;
            foreach (var item in result)
            {
                ws.Cells["A" + rowStart].Value = STT;
                ws.Cells["A" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["B" + rowStart].Value = item.UpdateDate;
                ws.Cells["B" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["C" + rowStart].Value = item.CreateUp;
                ws.Cells["C" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["D" + rowStart].Value = item.CompanyName;
                ws.Cells["D" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["E" + rowStart].Value = item.Sales;
                ws.Cells["E" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["F" + rowStart].Value = item.ID_Booker;
                ws.Cells["F" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["G" + rowStart].Value = item.Name;
                ws.Cells["G" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["H" + rowStart].Value = item.Tel;
                ws.Cells["H" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["I" + rowStart].Value = item.BankNumber;
                ws.Cells["I" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["J" + rowStart].Value = item.BankName;
                ws.Cells["J" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["K" + rowStart].Value = item.BankAccount;
                ws.Cells["K" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                STT++;
                rowStart++;
            }

            fileContents = pck.GetAsByteArray();
            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "DanhSachIDBooker.xlsx"
            );

        }
        public IActionResult ThongBaoDaiLy()
        {
            string MaPB = "KD";
            KhoaCodeDaiLyModel result = new KhoaCodeDaiLyModel();

            result = _unitOfWork_Repository.KhoaCodeDaiLy_Rep.DSThongBaoDaiLy(MaPB);
            return View("ThongBaoDaiLy", result);
        }
        [HttpPost]
        public async Task<IActionResult> ThongBaoDaiLy(string IDtxt, string MaKHtxt, string tenDLtxt, string noiDungKhoatxt, string IDNoiDungKhoa, string TinhTrangKhoa, string Email, string SoDT, string MailCC, string searchBtn, string saveBtn)
        {
            string MaPB = "KD";
            KhoaCodeDaiLyRepository khoacodedaily_Rep = _unitOfWork_Repository.KhoaCodeDaiLy_Rep;
            NotifyRepository notify_Rep = _unitOfWork_Repository.Notify_Rep;
            KhoaCodeDaiLyModel result = new KhoaCodeDaiLyModel();
            List<DSDaiLyModel> listDSDaiLy = new List<DSDaiLyModel>();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (searchBtn != null)
            {
                listDSDaiLy = khoacodedaily_Rep.DSDaiLy(MaKHtxt);
                result.DSDaiLy = listDSDaiLy;
                result.DSKhoaCodeDaiLy = khoacodedaily_Rep.DSThongBaoDaiLy(MaPB).DSKhoaCodeDaiLy;
                result.DSTinhTrangKhoa = khoacodedaily_Rep.DSTinhTrangKhoa(MaPB);
                return View("ThongBaoDaiLy", result);
            }
            if (saveBtn != null)
            {
                string tenNV = acc.HoTen;
                string MaNVLap = acc.MaNV;
                bool ret = khoacodedaily_Rep.SaveTBDL(MaKHtxt, tenDLtxt, noiDungKhoatxt, MaNVLap, tenNV, IDNoiDungKhoa, TinhTrangKhoa, MailCC, Email, SoDT, MaPB);
                result = khoacodedaily_Rep.DSThongBaoDaiLy(MaPB);
                if (ret == true)
                {
                  
                    string Title = notify_Rep.GetNotifyTitle(IDNoiDungKhoa);
                    // Đoạn này dùng để bỏ hết thẻ HTML
                    var Content = notify_Rep.RemoveHtmlTags(noiDungKhoatxt);
                    List<Member> memberResult = notify_Rep.Chitietmember(MaKHtxt);
                    //var kinhDoanhMember = memberResult.FirstOrDefault(member => member.ListKD.Any(kd => kd.Select == "selected"));
                    string MaNVKinhDoanh = "";
               
                   TempData["thongbaoSuccess"] = "Đã gửi thông báo đến đại lý";
                    
                }
                else TempData["thongbaoError"] = "Gửi thông báo thất bại";
                return View("ThongBaoDaiLy", result);
            }
            result = khoacodedaily_Rep.DSThongBaoDaiLy(MaPB);
            return View("ThongBaoDaiLy", result);
        }
        [HttpPost]
        public JsonResult GetTieuDe(int ID)
        {
            List<TieuDeModel> result = _unitOfWork_Repository.KhoaCodeDaiLy_Rep.GetTieuDe(ID);
            return Json(result);
        }
        [HttpGet]
        public JsonResult GetNoiDungThongBaoDL(int ID)
        {
            string result = _unitOfWork_Repository.KhoaCodeDaiLy_Rep.GetNoiDung(ID);
            return Json(result);
        }
    }
}
