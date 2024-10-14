//using InMemoryCacheDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
//using RedisConfig;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Manager.Common.Abstractions;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace Manager_EV.Areas.HomeArea.Controllers
{

    [Area(AreaNameConst.AREA_Home)]
    public class HomeController : Controller
    {
        private readonly IJwtTokenGenerator tokenGenerator;
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;
        public HomeController(IJwtTokenGenerator tokenGenerator, IUnitOfWork_Repository unitOfWork_Repository)
        {
            this.tokenGenerator = tokenGenerator;
            _unitOfWork_Repository = unitOfWork_Repository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public JsonResult ActiveUser(string Active, string RowID)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool result = _unitOfWork_Repository.Login_Rep.ActiveUser(Active, RowID);
            acc.Active = Active;
            return Json(result);
        }

        public IActionResult StickNote(string maNV)
        {

            AccountModel result = _unitOfWork_Repository.Profile_Rep.StickNote(maNV);
            return View(result);
        }
        public JsonResult SaveStickNote(string maNV, string createContent)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (maNV == null)
            {
                maNV = acc.MaNV;
            }
            bool res = _unitOfWork_Repository.Profile_Rep.SaveStickNote(maNV, createContent);
            AccountModel result = _unitOfWork_Repository.Profile_Rep.StickNote(maNV);
            return Json(result.StickNote);
        }
        [HttpPost]
        public JsonResult SoDuHeader()
        {
            try
            {
                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                CongNoRepository congno_Rep = _unitOfWork_Repository.CongNo_Rep;
                decimal SoDu = congno_Rep.SoDuCuoiNgayDapper(acc.MaNV).Result * -1;
                return Json(SoDu);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult Khoaxuathoadondientu()
        {
            KhoaHoaDon KHD = _unitOfWork_Repository.KhoaHoaDon_Rep.Dulieukhoahoadon();
            return View(KHD);
        }
        public JsonResult GetKey(string khoachinh)
        {
            int result = _unitOfWork_Repository.KhoaHoaDon_Rep.GetKey(khoachinh);
            return Json(result);
        }
        [HttpPost]
        public JsonResult Khoahoadon(string nhapmk, string id)
        {

            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool result = _unitOfWork_Repository.KhoaHoaDon_Rep.Khoahoadon(nhapmk, id, acc.TenDangNhap, acc.HoTen);
            return Json(result);
        }
        public JsonResult KhoaPVL(string txt_PVL1, string txt_PVL2, string txt_PVL3, string txt_PVL4, string nhapmk)
        {

            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool result = _unitOfWork_Repository.KhoaHoaDon_Rep.KhoaPVL(txt_PVL1, txt_PVL2, txt_PVL3, txt_PVL4, acc.TenDangNhap, acc.HoTen, nhapmk);
            return Json(result);
        }
        public JsonResult UpdateKHHD(string txt_KHHD_1, string txt_KHHD_2, string txt_KHHD_3, string txt_KHHD_4, string txt_Serial_L1, string txt_Serial_L2, string txt_Serial_L3, string txt_Serial_L4, string nhapmk)
        {

            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (txt_Serial_L1 == null)
            {
                txt_Serial_L1 = "";
            }
            if (txt_Serial_L2 == null)
            {
                txt_Serial_L2 = "";
            }
            if (txt_Serial_L3 == null)
            {
                txt_Serial_L3 = "";
            }
            if (txt_Serial_L4 == null)
            {
                txt_Serial_L4 = "";
            }
            bool result = _unitOfWork_Repository.KhoaHoaDon_Rep.UpdateKHHD(txt_KHHD_1, txt_KHHD_2, txt_KHHD_3, txt_KHHD_4, txt_Serial_L1, txt_Serial_L2, txt_Serial_L3, txt_Serial_L4, acc.TenDangNhap, acc.HoTen, nhapmk);
            return Json(result);
        }

        public JsonResult UpdatePhanmem(string khoaphanmem, string noidung, string nhapmk)
        {

            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool result = _unitOfWork_Repository.KhoaHoaDon_Rep.UpdatePhanmem(khoaphanmem, noidung, acc.TenDangNhap, nhapmk);
            return Json(result);
        }
        [HttpGet]
        public JsonResult GetPhanMem(string khoaphanmem)
        {

            string result = _unitOfWork_Repository.KhoaHoaDon_Rep.GetPhanMem(khoaphanmem);
            return Json(result);
        }
        public JsonResult UpdateNgayKhoa(string txt_NgayKhoa, string nhapmk)
        {

            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool result = _unitOfWork_Repository.KhoaHoaDon_Rep.UpdateNgayKhoa(txt_NgayKhoa, acc.TenDangNhap, acc.HoTen, nhapmk);
            return Json(result);
        }
        public JsonResult UpdateGHN(string txt_GHTN, string txt_ChuaL1, string txt_ChuaL2, string txt_ChuaL3, string txt_ChuaL4, string nhapmk)
        {

            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool result = _unitOfWork_Repository.KhoaHoaDon_Rep.UpdateGHN(txt_GHTN, txt_ChuaL1, txt_ChuaL2, txt_ChuaL3, txt_ChuaL4, acc.TenDangNhap, acc.HoTen, nhapmk);
            return Json(result);
        }
    }
}

