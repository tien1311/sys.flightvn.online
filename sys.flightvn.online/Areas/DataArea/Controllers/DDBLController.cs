using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;

namespace Manager_EV.Areas.DataArea.Controllers
{
    [Area(AreaNameConst.AREA_Data)]
    public class DDBLController : Controller
    {
        CultureInfo provider;
        DinhDanhBaoLuuRepository DDBL_Rep = new DinhDanhBaoLuuRepository();

        //public IActionResult DanhsachDDBL()
        //{
        //    return View();
        //}
        public IActionResult QuanlyDDBL()
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            DDBL result = DDBL_Rep.QuanlyDDBL(acc.MaNV);
            return View(result);
        }
        public IActionResult CreateDDBL()
        {
            ViewBag.DateEndSale = DateTime.Now.AddDays(+3).ToString("dd/MM/yyyy");
            return PartialView();
        }
        public IActionResult SaveCreateDDBL(DinhDanhBaoLuuModel DDBL, List<TenHanhKhach> ListTenHK)
        {
            string dateFrom = "";
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //format lại ngày 
            //DateTime dFrom = DateTime.ParseExact(DDBL.HanDangBan, "dd/MM/yyyy", provider, DateTimeStyles.None);
            //Chuyển lại thành string để truyền vào
            //string dateFrom = dFrom.ToString("yyyy-MM-dd");
            if (DDBL.HanDangBan == "10")
            {
                dateFrom = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");
            }
            if (DDBL.HanDangBan == "20")
            {
                dateFrom = DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd");
            }
            if (DDBL.HanDangBan == "30")
            {
                dateFrom = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            }
            DDBL.HanDangBan = dateFrom;
            bool ret = DDBL_Rep.SaveCreateDDBL(DDBL, ListTenHK, acc.MaNV, acc.Ten);
            string result = "";
            if (ret == true)
            {
                result = "Bạn đã lưu danh mục thành công";
            }
            else result = "Bạn đã lưu danh mục không thành công";

            return Json(result);
        }
        [HttpPost]
        public JsonResult SaveEditDDBL(DinhDanhBaoLuuModel DDBL, List<TenHanhKhach> ListTenHK)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //format lại ngày 
            DateTime dFrom = DateTime.ParseExact(DDBL.HanDangBan, "dd/MM/yyyy", provider, DateTimeStyles.None);
            //Chuyển lại thành string để truyền vào
            string dateFrom = dFrom.ToString("yyyy-MM-dd");
            DDBL.HanDangBan = dateFrom;
            bool ret = DDBL_Rep.SaveEditDDBL(DDBL, ListTenHK);
            string result = "";
            if (ret == true)
            {
                result = "Bạn đã lưu danh mục thành công";
            }
            else result = "Bạn đã lưu danh mục không thành công";

            return Json(result);
        }
        public IActionResult EditDDBL(string ID)
        {
            DinhDanhBaoLuuModel result = DDBL_Rep.EditDDBL(ID);
            return PartialView(result);
        }
        public IActionResult ViewDetailDDBL(string ID)
        {
            List<TenHanhKhach> result = DDBL_Rep.ViewDetailDDBL(ID);
            return PartialView(result);
        }
        public IActionResult SearchDDBL(string from_date, string to_date, string Hang, string Loai, string tenHK)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //format lại ngày 
            DateTime dFrom = DateTime.ParseExact(from_date, "dd/MM/yyyy", provider, DateTimeStyles.None);
            DateTime dTo = DateTime.ParseExact(to_date, "dd/MM/yyyy", provider, DateTimeStyles.None);
            //Chuyển lại thành string để truyền vào
            string dateFrom = dFrom.ToString("yyyy-MM-dd");
            string dateTo = dTo.ToString("yyyy-MM-dd");
            DDBL result = DDBL_Rep.SearchDDBL(dateFrom, dateTo, Hang, Loai, tenHK, acc.MaNV);
            return View("QuanlyDDBL", result);
        }
    }
}
