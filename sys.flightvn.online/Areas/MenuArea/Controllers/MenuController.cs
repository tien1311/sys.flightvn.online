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
using Manager.Model.Models;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Manager_EV.Areas.MenuArea.Controllers
{
    [Area(AreaNameConst.AREA_Menu)]
    [Authorize]
    public class MenuController : Controller
    {
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;

        public MenuController(IUnitOfWork_Repository unitOfWork_Repository)
        {
            _unitOfWork_Repository = unitOfWork_Repository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ThongBao()
        {
            return PartialView("ThongBao");
        }
        public IActionResult NghiepVu()
        {
            return View();
        }
        public IActionResult CSKH()
        {
            return View();
        }
        public IActionResult ChinhSachQN()
        {
            return View();
        }
        public IActionResult ChinhSachQT()
        {
            return View();
        }
        public IActionResult ChinhSachDB()
        {
            return View();
        }
        public IActionResult TinhNangMoi()
        {
            return View();
        }
        public IActionResult KhenThuong_KyLuat()
        {
            return View();
        }
        public IActionResult iframe()
        {
            return View();
        }
        public IActionResult DS_CNNV()
        {
            List<CongNoNhanVienModel> ListCNNV = Task.Run(() => _unitOfWork_Repository.ThongTinDaiLy_Rep.CongNoNVAsync()).Result;
            return View(ListCNNV);
        }

        #region Xử lý Task ASYNC
        public IActionResult GetClaimValue(string key)
        {
            string claimValue = User.Claims.FirstOrDefault(claim => claim.Type == key)?.Value;
            return Json(claimValue);
        }

        public IActionResult GetActiveUserAsync(string maNV)
        {
            var result = _unitOfWork_Repository.Login_Rep.GetActiveUserAsync(maNV);
            return Json(result.Result);
        }
        public IActionResult GetSoLuongVeSotAsync(string maNV)
        {
            var result = _unitOfWork_Repository.VeSot_Rep.SoLuongVeSotAsync(maNV);
            return Json(result.Result);
        }
        public IActionResult GetSLVeSotBCCTCAsync(string maNV)
        {
            var result = _unitOfWork_Repository.GuiMail_DaiLy_Rep.SLVeSotBCCTCAsync(maNV);
            return Json(result.Result);
        }
        public IActionResult GetLayCongNoNhanVien(string maNV)
        {
             CongNoRepository congno_Rep = _unitOfWork_Repository.CongNo_Rep;
             decimal result = congno_Rep.SoDuCuoiNgayDapper(maNV).Result * -1;
             //var result = _unitOfWork_Repository.ThongTinDaiLy_Rep.LayCongNoNhanVien(maNV);
             return Json(result);
        }
        public IActionResult GetDemTBChuaXem(string maNV)
        {
            var result = _unitOfWork_Repository.ThongBao_Rep.DemTBChuaXem(maNV);
            return Json(result.Result);
        }

        public IActionResult GetSoLuongVeHoanAsync()
        {
            var result = _unitOfWork_Repository.VeHoan_Rep.SoLuongVeHoanAsync();
            return Json(result.Result);
        }
        public IActionResult GetSoLuongVeDangHoanAsync()
        {
            var result = _unitOfWork_Repository.VeHoan_Rep.SoLuongVeDangHoanAsync();
            return Json(result.Result);
        }
        public IActionResult GetSD_HangIndexAsync()
        {
            var result = _unitOfWork_Repository.DoanhSoHang_Rep.SD_HangIndexAsync();
            return Json(result.Result);
        }
        public IActionResult GetSD_HangVNIndexAsync()
        {
            var result = _unitOfWork_Repository.DoanhSoHang_Rep.SD_HangVNIndexAsync();
            return Json(result.Result);
        }

        public IActionResult GetSD_HangAirInternationIndexAsync()
        {
            var result = _unitOfWork_Repository.DoanhSoHang_Rep.SD_HangAirInternationIndexAsync();
            return Json(result.Result);
        }
        public IActionResult GetCongNoNVAsync()
        {
            var result = _unitOfWork_Repository.ThongTinDaiLy_Rep.CongNoNVAsync();
            return Json(result.Result);
        }
        public IActionResult GetDSCongNoNVQuaHanAsync()
        {
            var result = _unitOfWork_Repository.CongNoQuaHan_Rep.DSCongNoNVQuaHanAsync();
            return Json(result.Result);
        }
        public IActionResult GetSoLuongBaiVietAsync()
        {
            var result = _unitOfWork_Repository.Forum_Rep.SoLuongBaiVietAsync();
            return Json(result.Result);
        }

        public IActionResult GetLoadPopup()
        {
            var result = _unitOfWork_Repository.Popup_Rep.LoadPopup();
            return Json(result.Result);
        }
        public IActionResult GetPopupPartial(string key)
        {
            return PartialView("Popup", key);
        }
        #endregion
    }
}
