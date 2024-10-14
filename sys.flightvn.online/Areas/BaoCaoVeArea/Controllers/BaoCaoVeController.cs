using EasyInvoice.Json;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Manager.DataAccess.Repository;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Manager_EV.Areas.BaoCaoVeArea.Controllers
{
    [Area(AreaNameConst.AREA_BaoCaoVe)]
    public class BaoCaoVeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;

        private AccountModel acc = new AccountModel();
        private CultureInfo provider;
        public BaoCaoVeController(IHostingEnvironment environment, IConfiguration configuration, IUnitOfWork_Repository unitOfWork_Repository)
        {
            _configuration = configuration;
            _hostingEnvironment = environment;
            _unitOfWork_Repository = unitOfWork_Repository;
        }
        public IActionResult IndexBaoCaoVe()
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public IActionResult BaoCaoVeSot()
        {
            string timeOut = _configuration.GetSection("DataService").GetSection("TimeoutEditVeSot").Value;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.NOIDUNG = _unitOfWork_Repository.GuiMail_DaiLy_Rep.NoiDungLuuY();
            string dateFrom = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTo = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy");
            ViewBag.DateTo = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.SoVe = "";
            ViewBag.TimeOut = timeOut;
            TongQuatMail tongQuat = new TongQuatMail();
            //tongQuat.ListChiTietVe = guimail_Rep.ListChiTietVe(acc.MaNV, dateFrom, dateTo, "");
            return View(tongQuat);
        }
        public IActionResult DSVeSot()
        {
            string timeOut = _configuration.GetSection("DataService").GetSection("TimeoutEditVeSot").Value;
            string dateFrom = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTo = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy");
            ViewBag.DateTo = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.SoVe = "";
            ViewBag.TimeOut = timeOut;
            return View();
        }
        [HttpPost]
        public IActionResult DSVeSot(string cal_from, string cal_to, string SoVeSearch, string Status)
        {
            string timeOut = _configuration.GetSection("DataService").GetSection("TimeoutEditVeSot").Value;
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            string server_EV = _configuration.GetConnectionString("SQL_EV_MAIN");
            string server_EV_V2 = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            acc = AccountManager.GetAccountCurrent(HttpContext);
            GuiMailDaiLyRepository guimail_Rep = _unitOfWork_Repository.GuiMail_DaiLy_Rep;
            TongQuatMail result = new TongQuatMail();
            ViewBag.NOIDUNG = guimail_Rep.NoiDungLuuY();
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
            ViewBag.TimeOut = timeOut;
            ViewBag.SoVe = SoVeSearch;
            string TienMat = "TM";
            result.TimeOutEdit = int.Parse(timeOut);
            //if (CheckTM == false)
            //{
            //    TienMat = "";

            //}
            //else
            //{
            //    result.TimeOutEdit = -1;
            //}
            result.ListChiTietVe = guimail_Rep.ListChiTietVe(acc.MaNV, dateFrom, dateTo, SoVeSearch, server_EV, server_KH_KT, Status, server_EV_V2);

            return View(result);
        }
        public IActionResult BaoCaoVeSotWeb()
        {
            string timeOut = _configuration.GetSection("DataService").GetSection("TimeoutEditVeSot").Value;
            string dateFrom = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTo = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy");
            ViewBag.DateTo = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.SoVe = "";
            ViewBag.TimeOut = timeOut;
            return View();
        }
        [HttpPost]
        public IActionResult BaoCaoVeSotWeb(string cal_from, string cal_to, string SoVeSearch, string Status)
        {
            string timeOut = _configuration.GetSection("DataService").GetSection("TimeoutEditVeSot").Value;
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            string server_EV = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            acc = AccountManager.GetAccountCurrent(HttpContext);
            GuiMailDaiLyRepository guimail_Rep = _unitOfWork_Repository.GuiMail_DaiLy_Rep;
            TongQuatMail result = new TongQuatMail();
            ViewBag.NOIDUNG = guimail_Rep.NoiDungLuuY();
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
            ViewBag.TimeOut = timeOut;
            ViewBag.SoVe = SoVeSearch;
            string TienMat = "TM";
            result.TimeOutEdit = int.Parse(timeOut);
            result.ListChiTietVe = guimail_Rep.ListChiTietVeWeb(acc.MaNV, dateFrom, dateTo, SoVeSearch, server_EV, server_KH_KT, Status);

            return View(result);
        }
        public IActionResult VeSotBCCTC()
        {
            string timeOut = _configuration.GetSection("DataService").GetSection("TimeoutEditVeSot").Value;
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            string server_EV = _configuration.GetConnectionString("SQL_EV_MAIN");
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            GuiMailDaiLyRepository guimail_Rep = _unitOfWork_Repository.GuiMail_DaiLy_Rep;
            TongQuatMail result = new TongQuatMail();
            ViewBag.NOIDUNG = guimail_Rep.NoiDungLuuYCongNo();

            result.ListChiTietVe = guimail_Rep.ListVeSotBCCTC(acc.MaNV, server_EV, server_KH_KT);
            result.TimeOutEdit = int.Parse(timeOut);
            return View("BaoCaoVeSot", result);
        }

        public IActionResult BaoCaoVeSotVoQuy()
        {
            string server_EV_V2 = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            string timeOut = _configuration.GetSection("DataService").GetSection("TimeoutEditVeSot").Value;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            GuiMailDaiLyRepository guimail_Rep = _unitOfWork_Repository.GuiMail_DaiLy_Rep;
            ViewBag.NOIDUNG = guimail_Rep.NoiDungLuuYCongNo();
            ViewBag.TyGiaText = guimail_Rep.TextTyGia(server_EV_V2);
            string dateFrom = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTo = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy");
            ViewBag.DateTo = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.SoVe = "";
            ViewBag.TimeOut = timeOut;
            TongQuatMail tongQuat = new TongQuatMail();
            List<LoaiPhiXuatModel> ListPhiXuat = new List<LoaiPhiXuatModel>();
            ListPhiXuat = guimail_Rep.ListPhiXuat(server_EV_V2);
            tongQuat.ListPhiXuat = ListPhiXuat;
            //tongQuat.ListChiTietVe = guimail_Rep.ListChiTietVe(acc.MaNV, dateFrom, dateTo, "");
            return View(tongQuat);
        }
        //[HttpPost]
        //public IActionResult BaoCaoVeSotVoQuy(string buttonclick, GuiMailDaiLytModel info, IFormFile[] files, string MAKH_2, string cal_from, string cal_to, string SoVeSearch, bool CheckTM)
        //{
        //    string timeOut = _configuration.GetSection("DataService").GetSection("TimeoutEditVeSot").Value;
        //    string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
        //    string server_EV = _configuration.GetConnectionString("SQL_EV_MAIN");
        //    AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
        //    if (acc.Ten == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    acc = AccountManager.GetAccountCurrent(HttpContext);
        //    GuiMailDaiLyRepository guimail_Rep = new GuiMailDaiLyRepository();
        //    TongQuatMail result = new TongQuatMail();
        //    ViewBag.NOIDUNG = guimail_Rep.NoiDungLuuYCongNo();
        //    string dateFrom = "";
        //    string dateTo = "";
        //    //format lại ngày 
        //    if (cal_from != null)
        //    {
        //        DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
        //        dateFrom = dFrom.ToString("yyyy-MM-dd");
        //        ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
        //    }
        //    if (cal_to != null)
        //    {
        //        DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
        //        dateTo = dTo.ToString("yyyy-MM-dd");
        //        ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
        //    }
        //    ViewBag.TimeOut = timeOut;
        //    ViewBag.SoVe = SoVeSearch;
        //    string TienMat = "TM";
        //    if (CheckTM == false)
        //    {
        //        TienMat = "";
        //    }
        //    result.ListChiTietVe = guimail_Rep.ListChiTietVe(acc.MaNV, dateFrom, dateTo, SoVeSearch, server_EV, server_KH_KT, TienMat);
        //    result.TimeOutEdit = int.Parse(timeOut);
        //    return View(result);
        //}

        [HttpPost]
        public JsonResult SaveBaoCao(List<ChiTietVe> ListChiTietVe)
        {
            string server_KT = _configuration.GetConnectionString("SQL_KT_MAIN");
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            GuiMailDaiLyRepository guimail_Rep = _unitOfWork_Repository.GuiMail_DaiLy_Rep;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string result = guimail_Rep.LuuDetailTicket(acc.MaNV, ListChiTietVe, server_KT, server_KH_KT);
            return Json(result);
        }
        [HttpPost]
        public JsonResult SaveBaoCaoVoQuy(string data)
        {
            string server_KT = _configuration.GetConnectionString("SQL_KT_MAIN");
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            string server_EV = _configuration.GetConnectionString("SQL_EV_MAIN");
            List<ChiTietVe> ListChiTietVe = JsonConvert.DeserializeObject<List<ChiTietVe>>(data);
            GuiMailDaiLyRepository guimail_Rep = _unitOfWork_Repository.GuiMail_DaiLy_Rep;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string result = guimail_Rep.LuuDetailTicketVoQuy(acc.MaNV, ListChiTietVe, server_KT, server_KH_KT, server_EV);
            return Json(result);
        }

    }
}
