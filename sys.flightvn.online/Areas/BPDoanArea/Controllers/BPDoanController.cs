using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Manager.DataAccess.Repository;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Manager_EV.Areas.BPDoanArea.Controllers
{
    [Area(AreaNameConst.AREA_BPDoan)]

    public class BPDoanController : Controller
    {
        CultureInfo provider;
        //FlightGroupRepository _unitOfWork_Repository.FlightGroup_Rep = new FlightGroupRepository();
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;
        public BPDoanController(IUnitOfWork_Repository unitOfWork_Repository)
        {
            _unitOfWork_Repository = unitOfWork_Repository;
        }
        public IActionResult Index_DOAN()
        {
            return View();
        }
        public IActionResult FlightGroup()
        {
            List<FlightGroupModel> result = _unitOfWork_Repository.FlightGroup_Rep.ListFlightGroup();
            return View(result);
        }
        public IActionResult SeachFlightGroup(int TinhTrang, string MaChuyenBay, string NoiDi, string NoiDen, string cal_from)
        {
            string dateFrom = null;
            if (cal_from != null)
            {
                DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
                dateFrom = dFrom.ToString("yyyy-MM-dd");
            }
            List<FlightGroupModel> result = _unitOfWork_Repository.FlightGroup_Rep.SeachFlightGroup(TinhTrang, MaChuyenBay, NoiDi, NoiDen, dateFrom);
            return View("FlightGroup", result);
        }
        public IActionResult CreateFlightGroup()
        {
            List<ListAirline> result = _unitOfWork_Repository.FlightGroup_Rep.ListAirline();
            return View("CreateFlightGroup", result);
        }
        [HttpPost]
        public JsonResult SaveCreateFlightGroup(FlightGroupModel flight)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string tenNhanVien = acc.HoTen;
            bool result = _unitOfWork_Repository.FlightGroup_Rep.SaveCreateFlightGroup(flight, tenNhanVien);
            return Json(result);
        }
        public IActionResult EditFlightGroup(int ID)
        {
            FlightGroupModel result = _unitOfWork_Repository.FlightGroup_Rep.EditFlightGroup(ID);
            return PartialView("EditFlightGroup", result);
        }
        public IActionResult PlusFlightGroup(int ID)
        {
            FlightGroupModel result = _unitOfWork_Repository.FlightGroup_Rep.EditFlightGroup(ID);
            return PartialView("PlusFlightGroup", result);
        }
        [HttpPost]
        public JsonResult SavePlusFlightGroup(FlightGroupModel flight)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string tenNhanVien = acc.HoTen;
            bool result = _unitOfWork_Repository.FlightGroup_Rep.SavePlusFlightGroup(flight, tenNhanVien);
            return Json(result);
        }
        [HttpPost]
        public JsonResult SaveEditFlightGroup(FlightGroupModel flight)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string tenNhanVien = acc.HoTen;
            bool result = _unitOfWork_Repository.FlightGroup_Rep.SaveEditFlightGroup(flight, tenNhanVien);
            return Json(result);
        }
        [HttpPost]
        public JsonResult DeleteFlightGroup(int ID)
        {
            bool result = _unitOfWork_Repository.FlightGroup_Rep.DeleteFlightGroup(ID);
            return Json(result);
        }
        [HttpPost]
        public JsonResult ActiveFlightGroup(int ID)
        {
            bool result = _unitOfWork_Repository.FlightGroup_Rep.ActiveFlightGroup(ID);
            return Json(result);
        }
        public IActionResult DanhSachBooking()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DanhSachBooking(string cal_from, string cal_to)
        {
            List<BookingFlightGroup> result = new List<BookingFlightGroup>();
            DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
            DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
            string dateFrom = dFrom.ToString("yyyy-MM-dd");
            string dateTo = dTo.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
            ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
            result = _unitOfWork_Repository.FlightGroup_Rep.DanhSachVedoan(dateFrom, dateTo);
            return View("DanhSachBooking", result);
        }
        [HttpPost]
        public IActionResult DetailBooking(int ID)
        {
            DetailBookingFlightGroup result = new DetailBookingFlightGroup();
            result = _unitOfWork_Repository.FlightGroup_Rep.DetailBooking(ID);
            return PartialView(result);
        }
    }
}
