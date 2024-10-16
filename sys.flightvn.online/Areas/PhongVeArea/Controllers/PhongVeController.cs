using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Manager.Model.Models;
using Manager.Model.Models.ViewModel;
using Manager.Model.Models.Other;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace sys.flightvn.online.Areas.PhongVeArea.Controllers
{
    [Area(AreaNameConst.AREA_PhongVe)]
    public class PhongVeController : Controller
    {
        private readonly IConfiguration _configuration;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;
        AccountModel acc = new AccountModel();
        CultureInfo provider;
        public PhongVeController(IHostingEnvironment environment, IConfiguration configuration, IUnitOfWork_Repository unitOfWork_Repository)
        {
            _hostingEnvironment = environment;
            _configuration = configuration;
            _unitOfWork_Repository = unitOfWork_Repository;

        }
        public IActionResult Index_BK()
        {
            return View();
        }
        public IActionResult CORPORATE()
        {
            return View();
        }
        public IActionResult News(int? page = 1, int pageSize = 7)
        {
            try
            {

                ViewBag.tieude = "Bản tin Phòng Vé";
                //BangTinRepository _unitOfWork_Repository.BangTin_Rep = new BangTinRepository();


                List<SubjectModel> list = _unitOfWork_Repository.BangTin_Rep.BangTin();

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "News";
                model.RouteValue = new RouteValueDictionary {
                    { "i", 3}
                };
                return View("News", model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult QuyDinhPV(int? page = 1, int pageSize = 7)
        {
            try
            {

                ViewBag.tieude = "QUY ĐỊNH PHÒNG VÉ";
                //BangTinRepository _unitOfWork_Repository.BangTin_Rep = new BangTinRepository();


                List<SubjectModel> list = _unitOfWork_Repository.BangTin_Rep.BangTin();

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "QuyDinhPV";
                model.RouteValue = new RouteValueDictionary {
                    { "i", 3}
                };
                return View("News", model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //public IActionResult QuyDinhHang(int? page = 1, int pageSize = 7)
        //{
        //    try
        //    {

        //        ViewBag.tieude = "Quy định Hãng";
        //        BangTinRepository _unitOfWork_Repository.BangTin_Rep = new BangTinRepository();


        //        List<SubjectModel> list = _unitOfWork_Repository.BangTin_Rep.QuyDinhHang();

        //        int pageNumber = (page ?? 1);
        //        //Phân trang 
        //        var model = PagingList.Create(list, pageSize, pageNumber);
        //        model.Action = "QuyDinhHang";
        //        model.RouteValue = new RouteValueDictionary {
        //            { "i", 3}
        //        };
        //        return View("News", model);
        //    }
        //    catch (System.Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        public IActionResult DanhSachTongGuiMailDaiLy(int? page = 1, int pageSize = 50)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            int pageNumber = page ?? 1;
            //GuiMailDaiLyRepository guimail_Rep = new GuiMailDaiLyRepository();
            string dateFrom = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTo = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.DateTo = DateTime.Now.ToString("dd/MM/yyyy");
            TempData["thongbaoInfo"] = "Báo cáo từ ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " đến ngày " + DateTime.Now.ToString("dd/MM/yyyy");
            DanhSachTongNhanVienXuatDoiVe result = new DanhSachTongNhanVienXuatDoiVe();
            result = _unitOfWork_Repository.GuiMail_DaiLy_Rep.SearchTongXuatDoiNhanVien(dateFrom, dateTo, "All", "HN", false, "Search");
            ViewBag.NGAY = "HN";
            ViewBag.MORONG = false;
            return View(result);
        }

        [HttpPost]
        public IActionResult DanhSachTongGuiMailDaiLy(string cal_from, string cal_to, string NHANVIEN, string NGAYTIMKIEM, string MORONG, string search)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            bool moRong = false;
            if (MORONG == "on")
            {
                moRong = true;
            }
            provider = CultureInfo.InvariantCulture;
            //GuiMailDaiLyRepository guimail_Rep = new GuiMailDaiLyRepository();
            //format lại ngày 
            DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
            DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
            //Chuyển lại thành string để truyền vào
            string dateFrom = dFrom.ToString("yyyy-MM-dd");
            string dateTo = dTo.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
            ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
            string tungay = dFrom.ToString("dd/MM/yyyy");
            string denngay = dTo.ToString("dd/MM/yyyy");
            DanhSachTongNhanVienXuatDoiVe result = new DanhSachTongNhanVienXuatDoiVe();
            if (search == "Search")
            {
                if (NGAYTIMKIEM == "HN")
                {
                    dateFrom = DateTime.Now.ToString("MM/dd/yyyy");
                    dateTo = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
                }
                else
                {
                    if (NGAYTIMKIEM == "HT")
                    {
                        dateFrom = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy");
                        dateTo = DateTime.Now.AddDays(0).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        if (NGAYTIMKIEM == "TuanT")
                        {
                            DateTime dt = DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek);
                            dateFrom = dt.AddDays(-7).ToString("MM/dd/yyyy");
                            dateTo = dt.AddDays(0).ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            if (NGAYTIMKIEM == "ThangT")
                            {
                                dateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1).ToString("MM/dd/yyyy");
                                dateTo = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1).ToString("MM/dd/yyyy");

                            }
                            else
                            {
                                dateFrom = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1).ToString("MM/dd/yyyy");
                                dateTo = new DateTime(DateTime.Now.Year, 1, 1).ToString("MM/dd/yyyy");

                            }
                        }
                    }
                }
                tungay = DateTime.Parse(dateFrom).ToString("dd/MM/yyyy");
                denngay = DateTime.Parse(dateTo).AddDays(-1).ToString("dd/MM/yyyy");
            }
            result = _unitOfWork_Repository.GuiMail_DaiLy_Rep.SearchTongXuatDoiNhanVien(dateFrom, dateTo, NHANVIEN, NGAYTIMKIEM, moRong, search);
            TempData["thongbaoInfo"] = "Báo cáo từ ngày " + tungay + " đến ngày " + denngay;
            ViewBag.NGAY = NGAYTIMKIEM;
            ViewBag.MORONG = moRong;
            return View(result);
        }


        [HttpPost]
        public IActionResult GuiMailDaiLy(string buttonclick, GuiMailDaiLytModel info, IFormFile[] files, string MAKH_2, string cal_from, string cal_to, string SoVeSearch)
        {
            acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            //GuiMailDaiLyRepository guimail_Rep = new GuiMailDaiLyRepository();
            TongQuatMail result = new TongQuatMail();
            ViewBag.NOIDUNG = _unitOfWork_Repository.GuiMail_DaiLy_Rep.NoiDungLuuY();
            string dateFrom = "";
            string dateTo = "";
            //format lại ngày 
            if (cal_from != null)
            {
                DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
                dateFrom = dFrom.ToString("yyyy-MM-dd");
                ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
            }

            if (cal_to != null)
            {
                DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
                dateTo = dTo.ToString("yyyy-MM-dd");
                ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
            }
            ViewBag.SoVe = SoVeSearch;
            if (buttonclick == "searchBtn")
            {
                result = _unitOfWork_Repository.GuiMail_DaiLy_Rep.SearchMaKH(info.MAKH);
                ViewBag.MAKH = "";
                ViewBag.DAILY = "";
                ViewBag.PNR = "";
                ViewBag.HANG = "BSP";
                ViewBag.TINHTRANG = "XUATVE";
            }
            else
            {
                if (info.MAKH == null || info.MAKH == "")
                {
                    TempData["thongbaoError"] = "Mã KH không được để trống";
                    return View(result);
                }
                if (info.DAILY == null || info.DAILY == "")
                {
                    TempData["thongbaoError"] = "Tên đại lý không được để trống";
                    return View(result);
                }
                if (info.PNR == null || info.PNR == "")
                {
                    TempData["thongbaoError"] = "PNR không được để trống";
                    return View(result);
                }
                ViewBag.MAKH = info.MAKH;
                ViewBag.DAILY = info.DAILY;
                ViewBag.PNR = info.PNR;
                ViewBag.HANG = info.HANG;
                ViewBag.TINHTRANG = info.TINHTRANG;
                TempData["thongbaoSuccess"] = "Mail của bạn đã được gửi thành công";

                decimal PhiDichVu = decimal.Parse(info.PHIDICHVU.Replace(",", ""));
                bool result_sendmail = _unitOfWork_Repository.GuiMail_DaiLy_Rep.SendMailAgent(acc.Ten, acc.DienThoai, acc.TenDangNhap, info.MAKH, info.DAILY, info.MAIL, info.MAILCC, info.PNR, info.HANG, info.TINHTRANG, info.NOIDUNG, info.DIEUKIEN, files, _hostingEnvironment.WebRootPath, PhiDichVu, acc.TenDangNhap);
                if (result_sendmail == true)
                {
                    TempData["thongbaoSuccess"] = "Mail của bạn đã được gửi thành công";
                }
                else
                {
                    TempData["thongbaoError"] = "Gửi mail thất bại";
                }
            }

            return View(result);
        }

        public IActionResult GuiMailDaiLy()
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            //GuiMailDaiLyRepository guimail_Rep = new GuiMailDaiLyRepository();
            ViewBag.MAKH = "";
            ViewBag.DAILY = "";
            ViewBag.PNR = "";
            ViewBag.HANG = "BSP";
            ViewBag.TINHTRANG = "XUATVE";
            ViewBag.NOIDUNG = _unitOfWork_Repository.GuiMail_DaiLy_Rep.NoiDungLuuY();
            //TongQuatMail tongQuat = new TongQuatMail();
            //return View(tongQuat);
            return View();
        }
        public IActionResult DanhSachGuiMailDaiLy(int? page = 1, int pageSize = 50)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            int pageNumber = page ?? 1;
            //GuiMailDaiLyRepository guimail_Rep = new GuiMailDaiLyRepository();
            string dateFrom = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTo = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.DateTo = DateTime.Now.ToString("dd/MM/yyyy");
            DanhSachXuatDoiVe result = new DanhSachXuatDoiVe();
            result = _unitOfWork_Repository.GuiMail_DaiLy_Rep.SearchGuiXuatDoi(dateFrom, dateTo, "", "", "All", "All");
            return View(result);
        }
        [HttpPost]
        public IActionResult DanhSachGuiMailDaiLy(string cal_from, string cal_to, string PNR, string MAKH, string TINHTRANG, string NHANVIEN, int? page = 1, int pageSize = 50)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            provider = CultureInfo.InvariantCulture;
            //GuiMailDaiLyRepository guimail_Rep = new GuiMailDaiLyRepository();
            //format lại ngày 
            DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
            DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
            //Chuyển lại thành string để truyền vào
            string dateFrom = dFrom.ToString("yyyy-MM-dd");
            string dateTo = dTo.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
            ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
            DanhSachXuatDoiVe result = new DanhSachXuatDoiVe();
            int pageNumber = page ?? 1;
            result = _unitOfWork_Repository.GuiMail_DaiLy_Rep.SearchGuiXuatDoi(dateFrom, dateTo, PNR, MAKH, TINHTRANG, NHANVIEN);
            return View(result);
        }
        public IActionResult ChiTietNoiDungXuatDoi(string khoachinh)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            //GuiMailDaiLyRepository guimail_Rep = new GuiMailDaiLyRepository();
            GuiMailDaiLytModel result = new GuiMailDaiLytModel();
            result = _unitOfWork_Repository.GuiMail_DaiLy_Rep.ChiTietXuatDoiVe(khoachinh);
            return View(result);
        }

        public IActionResult DetailNews(int subject_id)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            //BangTinRepository _unitOfWork_Repository.BangTin_Rep = new BangTinRepository();
            SubjectModel content = _unitOfWork_Repository.BangTin_Rep.ContentPV(subject_id);
            return View("DetailNews", content);
        }
        public IActionResult NewDetailNews()
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            //BangTinRepository _unitOfWork_Repository.BangTin_Rep = new BangTinRepository();
            SubjectModel content = _unitOfWork_Repository.BangTin_Rep.ContentPVNew();
            return View("DetailNews", content);
        }
        public IActionResult VeSot()
        {
            string server_EV_MAIN = _configuration.GetConnectionString("SQL_EV_MAIN");
            //Hết token
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ListVeSotModel result = _unitOfWork_Repository.VeSot_Rep.DSVeSot(server_EV_MAIN);
            return View("VeSot", result);
        }
        public IActionResult SearchVeSot(string ma, string getBCVS, string Search)
        {
            string server_EV_MAIN = _configuration.GetConnectionString("SQL_EV_MAIN");
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ListVeSotModel content = new ListVeSotModel();
            VeSotRepository vesot_Repository = _unitOfWork_Repository.VeSot_Rep;
            if (getBCVS != null)
            {
                content = vesot_Repository.VeSot(server_KH_KT, acc.Ten);
            }
            if (Search != null)
            {
                content = vesot_Repository.SearchVeSot(ma, server_EV_MAIN);
            }
            if (content.ThongBao != "" && content.ThongBao != null)
            {
                //ViewBag.message = content.ThongBao;
                TempData["thongbaoError"] = content.ThongBao;
            }
            return View("VeSot", content);
        }
        public JsonResult SendSMSVeSot(List<ListTenNVModel> listNV)
        {
            bool result = false;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string NguoiGui = acc.Ten + " (" + acc.MaNV + ")";
            result = _unitOfWork_Repository.VeSot_Rep.SendSMS_VeSot(NguoiGui);
            return Json(result);
        }
        public IActionResult BaoCaoVeSot(string PNR, string HANG, string SOVE, string Index)
        {
            string Server = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            string Server_Main = _configuration.GetConnectionString("SQL_EV_MAIN");
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (PNR != null)
            {
                PNR = PNR.Trim();
                ViewBag.PNR = PNR.Trim();
            }
            else
            {
                PNR = "";
            }
            if (HANG != null)
            {
                HANG = HANG.Trim();
                ViewBag.HANG = HANG.Trim();
            }
            else
            {
                HANG = "";
            }
            if (SOVE != null)
            {
                SOVE = SOVE.Trim();
                ViewBag.SOVE = SOVE.Trim();
            }
            else
            {
                SOVE = "";
            }



            //GuiMailDaiLyRepository rep = new GuiMailDaiLyRepository();
            List<LoaiPhiXuatModel> listLoaiPhi = _unitOfWork_Repository.GuiMail_DaiLy_Rep.ListPhiXuat(Server);
            ChiTietVe chiTietVe = _unitOfWork_Repository.GuiMail_DaiLy_Rep.VeDetail(Server_Main, PNR, SOVE);
            if (chiTietVe != null)
            {
                ViewBag.SOLUONG = chiTietVe.SoLuong;
            }
            else
            {
                ViewBag.SOLUONG = 1;
            }

            ViewBag.ChiTietVe = chiTietVe;
            ViewBag.ListLoaiPhi = listLoaiPhi;
            ViewBag.CurrentIndex = Index;
            return View();
        }
        [HttpPost]
        public JsonResult SaveBaoCaoVeSot(string PNR, string Hang, string SoVe, string MaKH, string GiaMua, string PhiMua, string PhiBan, string PhiHoan, string ChietKhau, string MaGioiThieu, string NguoiGioiThieu, string LoaiPhi, string PhiXuatVe, string SoLuong)
        {
            string server_KT = _configuration.GetConnectionString("SQL_KT_MAIN");
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            int result = _unitOfWork_Repository.VeSot_Rep.SaveBaoCaoVeSot(server_KT, server_KH_KT, PNR, Hang, SoVe, MaKH.ToUpper(), GiaMua, PhiMua, PhiBan, PhiHoan, ChietKhau, acc.MaNV, MaGioiThieu, NguoiGioiThieu, LoaiPhi, PhiXuatVe, SoLuong);
            return Json(result);
        }
        [HttpPost]
        public JsonResult UpdateBaoCaoVeSot(string PNR, string Hang, string SoVe, string MaKH, string GiaMua, string PhiMua, string PhiBan, string PhiHoan, string ChietKhau, string MaGioiThieu, string NguoiGioiThieu, int ID, string LoaiPhi, string PhiXuatVe, string SoLuong)
        {
            string server_KT = _configuration.GetConnectionString("SQL_KT_MAIN");
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            VeSotRepository vesot_Rep = _unitOfWork_Repository.VeSot_Rep;
            int result = -3;
            result = vesot_Rep.UpdateBaoCaoVeSot(server_KT, server_KH_KT, PNR, Hang, SoVe, MaKH.ToUpper(), GiaMua, PhiMua, PhiBan, PhiHoan, ChietKhau, acc.MaNV, MaGioiThieu, NguoiGioiThieu, ID, LoaiPhi, PhiXuatVe, SoLuong);
            //if (!vesot_Rep.CheckDataMainEFF(SoVe, PNR, server_KH_KT))
            //{

            //}
            return Json(result);
        }
        public JsonResult SearchMaKHBaoCaoVeSot(string MaKH)
        {
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string result = _unitOfWork_Repository.VeSot_Rep.SearchMaKHBaoCaoVeSot(MaKH, server_KH_KT);
            return Json(result);
        }

        public IActionResult HoanVe(int? page = 1, int pageSize = 50)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            VeHoanRepository VeHoan_Rep = _unitOfWork_Repository.VeHoan_Rep;
            int pageNumber = page ?? 1;

            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            string SRefurnd = VeHoan_Rep.Refurnd(acc.TenDangNhap);
            if (string.IsNullOrEmpty(SRefurnd))
            {
                return Json(new { success = false, error = 1 });
            }
            List<Hang> listHang = VeHoan_Rep.ListHang(SRefurnd);
            List<NguoiXuLy> listNguoiXuLy = VeHoan_Rep.ListNguoiXuLy();
            ViewBag.listHang = listHang;
            ViewBag.listNguoiXuLy = listNguoiXuLy;
            List<VeHoanModel> ListVeHoan = VeHoan_Rep.DSVeHoan(acc.TenDangNhap);
            var model = PagingList.Create(ListVeHoan, pageSize, pageNumber);
            model.Action = "HoanVe";
            model.RouteValue = new RouteValueDictionary {
                        { "i", 3}
                    };
            return View(model);
        }
        public IActionResult VeDangHoan(int? page = 1, int pageSize = 50)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            VeHoanRepository VeHoan_Rep = _unitOfWork_Repository.VeHoan_Rep;
            int pageNumber = page ?? 1;

            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            string SRefurnd = VeHoan_Rep.Refurnd(acc.TenDangNhap);
            if (string.IsNullOrEmpty(SRefurnd))
            {
                return Json(new { success = false, error = 1 });
            }
            List<Hang> listHang = VeHoan_Rep.ListHang(SRefurnd);
            List<NguoiXuLy> listNguoiXuLy = VeHoan_Rep.ListNguoiXuLy();
            ViewBag.listHang = listHang;
            ViewBag.listNguoiXuLy = listNguoiXuLy;
            List<VeHoanModel> ListVeHoan = VeHoan_Rep.DSVeDangHoan(acc.TenDangNhap);
            var model = PagingList.Create(ListVeHoan, pageSize, pageNumber);

            model.Action = "HoanVe";
            model.RouteValue = new RouteValueDictionary {
                        { "i", 3}
                    };
            return View("HoanVe", model);
        }
        public IActionResult ChiTietVeHoan(int khoachinh)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            SubjectModel ChiTietVeHoan = _unitOfWork_Repository.VeHoan_Rep.ChiTietVeHoan(khoachinh, acc.TenDangNhap);
            HttpContext.Session.SetInt32("IDVe", khoachinh);
            HttpContext.Session.SetInt32("isshow", ChiTietVeHoan.subject_isshow);
            HttpContext.Session.SetInt32("ishot", ChiTietVeHoan.subject_ishot);
            HttpContext.Session.SetInt32("com", ChiTietVeHoan.subject_com);
            HttpContext.Session.SetString("picnote", ChiTietVeHoan.subject_picnote);
            return View(ChiTietVeHoan);
        }
        public IActionResult SearchVeHoan(string loaive, string Dieukien, string Giatri, string tinhtrang, string cal_from, string cal_to, string nguoixuly, string vedenhan, int? page = 1)
        {
            HttpContext.Session.SetString("loaive", loaive);
            if (Dieukien != null)
            {
                HttpContext.Session.SetString("Dieukien", Dieukien);
            }
            if (Giatri != null)
            {
                HttpContext.Session.SetString("Giatri", Giatri);
            }
            HttpContext.Session.SetString("tinhtrang", tinhtrang);
            if (cal_from != null)
            {
                HttpContext.Session.SetString("cal_from", cal_from);
            }
            if (cal_to != null)
            {
                HttpContext.Session.SetString("cal_to", cal_to);
            }
            HttpContext.Session.SetString("nguoixuly", nguoixuly);
            if (vedenhan != null)
            {
                HttpContext.Session.SetString("vedenhan", vedenhan);
            }

            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);

            int pageSize = 50;
            int pageNumber = page ?? 1;
            VeHoanRepository VeHoan_Rep = _unitOfWork_Repository.VeHoan_Rep;
            string SRefurnd = VeHoan_Rep.Refurnd(acc.TenDangNhap);
            List<Hang> listHang = VeHoan_Rep.ListHang(SRefurnd);
            List<NguoiXuLy> listNguoiXuLy = VeHoan_Rep.ListNguoiXuLy();
            ViewBag.listHang = listHang;
            ViewBag.listNguoiXuLy = listNguoiXuLy;
            List<VeHoanModel> ListVeHoan = VeHoan_Rep.SearchVeHoan(loaive, Dieukien, Giatri, tinhtrang, cal_from, cal_to, nguoixuly, vedenhan, acc.TenDangNhap);
            var model = PagingList.Create(ListVeHoan, pageSize, pageNumber);
            model.Action = "PhanTrangVeHoan";
            model.RouteValue = new RouteValueDictionary {
                        { "i", 3}
                    };
            return View("HoanVe", model);
        }
        public IActionResult PhanTrangVeHoan(int? page = 1, int pageSize = 50)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            string loaive = HttpContext.Session.GetString("loaive");
            string Dieukien = HttpContext.Session.GetString("Dieukien");
            string Giatri = HttpContext.Session.GetString("Giatri");
            string tinhtrang = HttpContext.Session.GetString("tinhtrang");
            string cal_from = HttpContext.Session.GetString("cal_from");
            string cal_to = HttpContext.Session.GetString("cal_to");
            string nguoixuly = HttpContext.Session.GetString("nguoixuly");
            string vedenhan = HttpContext.Session.GetString("vedenhan");


            int pageNumber = page ?? 1;
            VeHoanRepository VeHoan_Rep = _unitOfWork_Repository.VeHoan_Rep;
            string SRefurnd = VeHoan_Rep.Refurnd(acc.TenDangNhap);
            List<Hang> listHang = VeHoan_Rep.ListHang(SRefurnd);
            List<NguoiXuLy> listNguoiXuLy = VeHoan_Rep.ListNguoiXuLy();
            ViewBag.listHang = listHang;
            ViewBag.listNguoiXuLy = listNguoiXuLy;
            List<VeHoanModel> ListVeHoan = VeHoan_Rep.SearchVeHoan(loaive, Dieukien, Giatri, tinhtrang, cal_from, cal_to, nguoixuly, vedenhan, acc.TenDangNhap);
            var model = PagingList.Create(ListVeHoan, pageSize, pageNumber);
            model.Action = "PhanTrangVeHoan";
            model.RouteValue = new RouteValueDictionary {
                        { "i", 3}
                    };
            return View("HoanVe", model);
        }
        public IActionResult XuLyVe(string loaive, string BtnChuyen, string BtnSave, string tinhtrang, string ghichu, string ngaybay, string songay, string checkHoan, string checkEMD, string SoVeEMD, string TenKhach, string YeuCau)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            VeHoanRepository VeHoan_Rep = _unitOfWork_Repository.VeHoan_Rep;
            int? page = 1;
            int pageSize = 50;
            int pageNumber = page ?? 1;
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            int? ID = HttpContext.Session.GetInt32("IDVe");
            int? isshow = HttpContext.Session.GetInt32("isshow");
            int? ishot = HttpContext.Session.GetInt32("ishot");
            int? com = HttpContext.Session.GetInt32("com");
            string picnote = HttpContext.Session.GetString("picnote");
            string kq = "";
            if (BtnChuyen == "chuyen")
            {
                kq = VeHoan_Rep.BtnChuyen(ID, isshow, loaive);
                //if (kq != "" && kq != null)
                //{
                //    ViewBag.message = kq;
                //}
                if (kq == StaticDetailVoucher.SUCCESS)
                {
                    TempData["thongbaoSuccess"] = "Chuyển danh mục thành công !";
                }
                else
                {
                    if (kq == StaticDetailVoucher.FAIL + "1")
                    {
                        TempData["thongbaoSuccess"] = "Đại lý đã hủy số vé này !";
                    }
                    else
                    {
                        TempData["thongbaoError"] = "Chuyển danh mục không thành công !";
                    }
                }
            }
            if (BtnSave == "save")
            {
                kq = VeHoan_Rep.BtnSave(acc.TenDangNhap, ID, isshow, ishot, com, picnote, loaive, tinhtrang, ghichu, ngaybay, songay, checkHoan, checkEMD, SoVeEMD, TenKhach, YeuCau);
                //if (kq != "" && kq != null)
                //{
                //    ViewBag.message = kq;
                //}
                if (kq == StaticDetailVoucher.SUCCESS)
                {
                    TempData["thongbaoSuccess"] = "Lưu vé hoàn thành công";
                }
                else
                {
                    if (kq == StaticDetailVoucher.FAIL)
                    {
                        TempData["thongbaoError"] = "Đại lý đã hủy số vé này !";
                    }
                    else
                    {
                        TempData["thongbaoError"] = "Lưu vé hoàn không thành công ";
                    }
                }
            }
            string SRefurnd = VeHoan_Rep.Refurnd(acc.TenDangNhap);
            List<Hang> listHang = VeHoan_Rep.ListHang(SRefurnd);
            List<NguoiXuLy> listNguoiXuLy = VeHoan_Rep.ListNguoiXuLy();
            ViewBag.listHang = listHang;
            ViewBag.listNguoiXuLy = listNguoiXuLy;

            List<VeHoanModel> ListVeHoan = VeHoan_Rep.DSVeHoan(acc.TenDangNhap);
            var model = PagingList.Create(ListVeHoan, pageSize, pageNumber);
            model.Action = "HoanVe";
            model.RouteValue = new RouteValueDictionary {
                        { "i", 3}
                    };
            return View("HoanVe", model);
        }
        public IActionResult GuiMailHang(string mail, string CCC, string CCkhac, string noidungtxt, string NoiDung, IFormFile[] files, string saveBtn)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }
            GuiMailDaiLyRepository guimail_Rep = _unitOfWork_Repository.GuiMail_DaiLy_Rep;
            GuiMailHang result = new GuiMailHang();
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (saveBtn != null)
            {
                if (mail == null || mail == "")
                {
                    TempData["thongbaoError"] = "Bạn chưa chọn mail To";
                    result = guimail_Rep.Guimailhang();
                    return View("GuiMailHang", result);
                }
                if (CCC == null || CCC == "")
                {
                    TempData["thongbaoError"] = "Bạn chưa chọn Mail CC";
                    result = guimail_Rep.Guimailhang();
                    return View("GuiMailHang", result);
                }
                if (CCkhac == null)
                {
                    CCkhac = "";
                }
                if (noidungtxt == null)
                {
                    noidungtxt = "";
                }
                if (NoiDung == null)
                {
                    NoiDung = "";
                }

                bool result_sendmail = guimail_Rep.SendMailHang(acc.HoTen, acc.Email, acc.DienThoai, acc.TenDangNhap, mail, CCC, CCkhac, noidungtxt, NoiDung, files, _hostingEnvironment.WebRootPath);
                if (result_sendmail == true)
                {
                    TempData["thongbaoSuccess"] = "Mail của bạn đã được gửi thành công";
                }
                else
                {
                    TempData["thongbaoError"] = "Gửi mail thất bại";
                }

            }

            result = guimail_Rep.Guimailhang();
            return View("GuiMailHang", result);
        }
        [HttpGet]
        public JsonResult GetTo(int khoachinh)
        {


            string result = _unitOfWork_Repository.GuiMail_DaiLy_Rep.GetTo(khoachinh);


            return Json(result);
        }
        public JsonResult GetCC(int khoachinh)
        {


            string result = _unitOfWork_Repository.GuiMail_DaiLy_Rep.GetCC(khoachinh);


            return Json(result);
        }
        public JsonResult GetTieuDe(int khoachinh)
        {


            string result = _unitOfWork_Repository.GuiMail_DaiLy_Rep.GetTieuDe(khoachinh);


            return Json(result);
        }
        public IActionResult DanhSachGuiMailHang()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DanhSachGuiMailHang(string cal_from, string cal_to)
        {
            provider = CultureInfo.InvariantCulture;
            GuiMailHang result = new GuiMailHang();
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
            result = _unitOfWork_Repository.ImportDoanhSo_Rep.DanhSachMailHang(dateFrom, dateTo);
            return View("DanhSachGuiMailHang", result);
        }
        [HttpPost]
        public IActionResult NoiDungMailHang(string khoachinh)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            GuiMailHangModel result = new GuiMailHangModel();

            result = _unitOfWork_Repository.GuiMail_DaiLy_Rep.MailHang(khoachinh);

            return View(result);
        }

        [HttpPost]
        public JsonResult CheckMaKH(string MaKH)
        {
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            string result = _unitOfWork_Repository.GuiMail_DaiLy_Rep.ExistsMaKH(MaKH, server_KH_KT);
            if (result == "")
            {
                result = "Mã KH này không có trong danh sách";
            }
            return Json(result);
        }
        public IActionResult Booking()
        {
            string server_AIRLINE_BOOKING = _configuration.GetConnectionString("SQL_AIRLINE_BOOKING");
            string server_EV_MAIN = _configuration.GetConnectionString("SQL_EV_MAIN");

            BookingViewModel result = _unitOfWork_Repository.Booking_Rep.Booking(server_AIRLINE_BOOKING, server_EV_MAIN);
            return View(result);
        }
        public IActionResult SearchBooking(BookingSearch bookingSearch)
        {
            string server_AIRLINE_BOOKING = _configuration.GetConnectionString("SQL_AIRLINE_BOOKING");
            string server_EV_MAIN = _configuration.GetConnectionString("SQL_EV_MAIN");
            BookingViewModel result = _unitOfWork_Repository.Booking_Rep.SearchBooking(server_AIRLINE_BOOKING, server_EV_MAIN, bookingSearch);
            return View("Booking", result);
        }
        public IActionResult DetailBooking(string ID)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            string server_AIRLINE_BOOKING = _configuration.GetConnectionString("SQL_AIRLINE_BOOKING");
            string server_EV_MAIN = _configuration.GetConnectionString("SQL_EV_MAIN");
            DetailBooking result = _unitOfWork_Repository.Booking_Rep.DetailBooking(server_AIRLINE_BOOKING, server_EV_MAIN, ID, acc.TenDangNhap);
            return View(result);
        }

        [HttpPost]
        public JsonResult UpdateThongTinKhach(string ID, string OrderID, string CodeDi, string CodeVe, string LgsProtiValues, string LgsOldValues, string LgsNewValues)
        {
            bool result = false;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string server_AIRLINE_BOOKING = _configuration.GetConnectionString("SQL_AIRLINE_BOOKING");
            result = _unitOfWork_Repository.Booking_Rep.UpdateThongTinKhach(ID, OrderID, CodeDi, CodeVe, server_AIRLINE_BOOKING, LgsProtiValues, LgsOldValues, LgsNewValues, acc.TenDangNhap);

            return Json(result);
        }
        [HttpPost]
        public JsonResult UpdateHoaDon(string ID, string CongTy, string DiaChiCTY, string MST, string DiaChiHD, string LgsProtiValues, string LgsOldValues, string LgsNewValues)
        {
            bool result = false;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string server_AIRLINE_BOOKING = _configuration.GetConnectionString("SQL_AIRLINE_BOOKING");
            result = _unitOfWork_Repository.Booking_Rep.UpdateHoaDon(ID, CongTy, DiaChiCTY, MST, DiaChiHD, server_AIRLINE_BOOKING, LgsProtiValues, LgsOldValues, LgsNewValues, acc.TenDangNhap);

            return Json(result);
        }
        [HttpPost]
        public JsonResult UpdateLienHe(string ID, string NguoiLienHe, string Email, string ThanhPho, string DienThoai, string DiaChi, string QuocGia, string LgsProtiValues, string LgsOldValues, string LgsNewValues)
        {
            bool result = false;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string server_AIRLINE_BOOKING = _configuration.GetConnectionString("SQL_AIRLINE_BOOKING");
            result = _unitOfWork_Repository.Booking_Rep.UpdateLienHe(ID, NguoiLienHe, Email, ThanhPho, DienThoai, DiaChi, QuocGia, server_AIRLINE_BOOKING, LgsProtiValues, LgsOldValues, LgsNewValues, acc.TenDangNhap);

            return Json(result);
        }
        [HttpPost]
        public JsonResult UpdateTinhTrang(string ID, string HTThanhToan, string NganHangCK, string LuotDi, string TimeLimit, string DateLimit, string MaGiaoDich, string NganHangTTTT, string TinhTrang, string TinhTrangTT, string SoTienCK, string MaDatChoLD, string MaDatChoLV, string LuotVe, string Remark, string MaThamChieu, string TienTrucTuyen, string LgsProtiValues, string LgsOldValues, string LgsNewValues)
        {
            bool result = false;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string server_AIRLINE_BOOKING = _configuration.GetConnectionString("SQL_AIRLINE_BOOKING");
            result = _unitOfWork_Repository.Booking_Rep.UpdateTinhTrang(ID, HTThanhToan, NganHangCK, LuotDi, TimeLimit, DateLimit, MaGiaoDich, NganHangTTTT, TinhTrang, TinhTrangTT, SoTienCK, MaDatChoLD, MaDatChoLV, LuotVe, Remark, MaThamChieu, TienTrucTuyen, server_AIRLINE_BOOKING, LgsProtiValues, LgsOldValues, LgsNewValues, acc.TenDangNhap);

            return Json(result);
        }
        [HttpPost]
        public JsonResult UpdateThongTinCBLD(string ID, string SoHieu_LD, string Hang_LD, string Code_LD, string DiemDi_LD, string DiemDen_LD, string NgayDi_LD, string GioDi_LD, string GioDen_LD, string GiaNet_LD, string ThuePhi_LD, string PhiDV_LD, string Giam_LD, string TongTien_LD, string LgsProtiValues, string LgsOldValues, string LgsNewValues)
        {
            bool result = false;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string server_AIRLINE_BOOKING = _configuration.GetConnectionString("SQL_AIRLINE_BOOKING");
            result = _unitOfWork_Repository.Booking_Rep.UpdateThongTinCBLD(ID, SoHieu_LD, Hang_LD, Code_LD, DiemDi_LD, DiemDen_LD, NgayDi_LD, GioDi_LD, GioDen_LD, GiaNet_LD, ThuePhi_LD, PhiDV_LD, Giam_LD, TongTien_LD, server_AIRLINE_BOOKING, LgsProtiValues, LgsOldValues, LgsNewValues, acc.TenDangNhap);

            return Json(result);
        }
        [HttpPost]
        public JsonResult UpdateThongTinCBLV(string ID, string SoHieu_LV, string Hang_LV, string Code_LV, string DiemDi_LV, string DiemDen_LV, string NgayDi_LV, string GioDi_LV, string GioDen_LV, string GiaNet_LV, string ThuePhi_LV, string PhiDV_LV, string Giam_LV, string TongTien_LV, string LgsProtiValues, string LgsOldValues, string LgsNewValues)
        {
            bool result = false;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string server_AIRLINE_BOOKING = _configuration.GetConnectionString("SQL_AIRLINE_BOOKING");
            result = _unitOfWork_Repository.Booking_Rep.UpdateThongTinCBLV(ID, SoHieu_LV, Hang_LV, Code_LV, DiemDi_LV, DiemDen_LV, NgayDi_LV, GioDi_LV, GioDen_LV, GiaNet_LV, ThuePhi_LV, PhiDV_LV, Giam_LV, TongTien_LV, server_AIRLINE_BOOKING, LgsProtiValues, LgsOldValues, LgsNewValues, acc.TenDangNhap);

            return Json(result);
        }
        [HttpPost]
        public JsonResult UpdateTinhTrangBooking(string ID, string TinhTrang, string LgsProtiValues, string LgsOldValues, string LgsNewValues)
        {
            bool result = false;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string server_AIRLINE_BOOKING = _configuration.GetConnectionString("SQL_AIRLINE_BOOKING");
            result = _unitOfWork_Repository.Booking_Rep.UpdateTinhTrangBooking(ID, TinhTrang, server_AIRLINE_BOOKING, LgsProtiValues, LgsOldValues, LgsNewValues, acc.TenDangNhap);

            return Json(result);
        }
        [HttpPost]
        public JsonResult UpdateGiaoCho(string ID, string per, string LgsProtiValues, string LgsOldValues, string LgsNewValues)
        {
            bool result = false;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string server_AIRLINE_BOOKING = _configuration.GetConnectionString("SQL_AIRLINE_BOOKING");
            result = _unitOfWork_Repository.Booking_Rep.UpdateGiaoCho(ID, per, server_AIRLINE_BOOKING, LgsProtiValues, LgsOldValues, LgsNewValues, acc.TenDangNhap);

            return Json(result);
        }
        [HttpPost]
        public JsonResult SendSMSBookingDetail(string ID, string hanhTrinh_SMS, string nguoiLienHe_SMS, string dienThoai_SMS, string gio_SMS, string ngayThanhToan_SMS, string trangThai_SMS, string giaVeMoi_SMS)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool result = false;
            string server_AIRLINE_BOOKING = _configuration.GetConnectionString("SQL_AIRLINE_BOOKING");
            result = _unitOfWork_Repository.Booking_Rep.SendSMSBooking(ID, hanhTrinh_SMS, nguoiLienHe_SMS, dienThoai_SMS, gio_SMS, ngayThanhToan_SMS, giaVeMoi_SMS, trangThai_SMS, server_AIRLINE_BOOKING, acc.TenDangNhap);
            return Json(result);
        }
        [HttpPost]
        public JsonResult SendSMSBookingDetailXuatVe(string ID)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool result = false;
            result = _unitOfWork_Repository.Booking_Rep.SendSMSBookingXuatVe(ID);
            return Json(result);
        }
        [HttpPost]
        public JsonResult SendMail(DetailBooking DetailBooking)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool result = false;
            result = _unitOfWork_Repository.Booking_Rep.SendMailBooking(DetailBooking);
            return Json(result);
        }
    }
}
