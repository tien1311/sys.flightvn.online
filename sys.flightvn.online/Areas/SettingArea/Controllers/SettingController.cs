using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace Manager_EV.Areas.SettingArea.Controllers
{
    [Area(AreaNameConst.AREA_Setting)]
    public class SettingController : Controller
    {
        AccountModel result = new AccountModel();
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;

        public SettingController(IHostingEnvironment environment, IConfiguration configuration, IUnitOfWork_Repository unitOfWork_Repository)
        {
            _hostingEnvironment = environment;
            _configuration = configuration;
            _unitOfWork_Repository = unitOfWork_Repository;
        }
        public IActionResult profile()
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }

            result = _unitOfWork_Repository.Profile_Rep.DSprofile(acc.MaNV);
            return View("profile", result);
        }

        [HttpPost]
        public IActionResult PhanViec(string khoachinh)
        {
            NhanVienModel result = new NhanVienModel();
            result = _unitOfWork_Repository.Profile_Rep.PhanViec(khoachinh);
            return PartialView("CongViec", result);
        }
        [HttpPost]
        public IActionResult QuyDinh(string khoachinh)
        {
            NhanVienModel result = new NhanVienModel();
            result = _unitOfWork_Repository.Profile_Rep.QuyDinh(khoachinh);
            return PartialView("CongViec", result);
        }
        public IActionResult CapNhatCauHinh()
        {
            string Server = _configuration.GetConnectionString("SQL_EV_SERVICE");
            CapNhatCauHinh result = new CapNhatCauHinh();
            result = _unitOfWork_Repository.Profile_Rep.CapNhat(Server);
            return PartialView("CapNhatCauHinh", result);
        }
        //
        [HttpPost]
        public IActionResult CapNhat(string HDMoi, string HDKT, string FirstCheckBox, string SecondCheckBox, string Button, string Status)
        {
            string Server = _configuration.GetConnectionString("SQL_EV_SERVICE");
            CapNhatCauHinh result = new CapNhatCauHinh();
            bool ret = false;
            if (Button == "UpdateStatus")
            {
                ret = _unitOfWork_Repository.Profile_Rep.UpdateStatusSMS(Status, Server);
            }
            else
            {
                ret = _unitOfWork_Repository.Profile_Rep.Update(HDMoi, HDKT, FirstCheckBox, SecondCheckBox);
            }

            ViewBag.Message = "Cập nhật thành công";
            result = _unitOfWork_Repository.Profile_Rep.CapNhat(Server);
            return View("CapNhatCauHinh", result);
        }
        public IActionResult EditProfile()
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            DanhSachLienHeModel result = _unitOfWork_Repository.Profile_Rep.DSLienHe(acc.MaNV);
            return View(result);
        }

        [HttpPost]
        public JsonResult EditProfile(string SaveSDTSMS, string SDT, string Email)
        {
            string thongBao = "";
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (SaveSDTSMS != null)
            {
                bool ret = _unitOfWork_Repository.Profile_Rep.SaveDSLienHe(SDT, Email, acc.MaNV);
                if (ret == true)
                {
                    thongBao = "Cập nhật dữ liệu thành công";
                }
                else
                {
                    thongBao = "Cập nhật dữ liệu thất bại";
                }
            }
            return Json(thongBao);
        }
    }
}
