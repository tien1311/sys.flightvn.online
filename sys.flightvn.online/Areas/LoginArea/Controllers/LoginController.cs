using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using Microsoft.AspNetCore.Identity;
using Manager.Common.Abstractions;
using Manager.Model.Models.ViewModel;
using Manager.Model.Models;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;
using Manager_EV.Areas.MenuArea.Controllers;
using Microsoft.Extensions.Configuration;

namespace Manager_EV.Areas.LoginArea.Controllers
{
    [Area(AreaNameConst.AREA_Login)]
    public class LoginController : Controller
    {
        // GET: Login
        private IConfiguration _configuration;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult login1()
        {
            return View();
        }

        private readonly IJwtTokenGenerator tokenGenerator;

        public LoginController(IJwtTokenGenerator tokenGenerator, IConfiguration configuration)
        {
            this.tokenGenerator = tokenGenerator;
            this._configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(LoginViewModel model, string returnUrl = null)
        {
            try
            {
                string SQL_EV_MAIN = _configuration.GetConnectionString("SQL_EV_MAIN");
                AccountModel acc = LoginRepository.Login(model.UserName, model.Password, SQL_EV_MAIN);
                if (acc != null && acc.MaNV != null)
                {
                    if (acc.ThongBao != "" && acc.ThongBao != null)
                    {
                        TempData["msg"] = acc.ThongBao;
                        return View("Index");
                    }
                    var accessTokenResult = tokenGenerator.GenerateAccessTokenWithClaimsPrincipal(
                        acc.Ten,
                        GetUserClaims(acc));
                    await HttpContext.SignInAsync(accessTokenResult.ClaimsPrincipal,
                        accessTokenResult.AuthProperties);

                    return RedirectToAction("Index", "Menu", new { area = AreaNameConst.AREA_Menu });
                }
                else
                {
                    TempData["msg"] = "* Tên đăng nhập hoặc mật khẩu không đúng";

                }
            }
            catch (Exception ex)
            {

            }
            return View("Index");
        }
        private static IEnumerable<Claim> GetUserClaims(AccountModel acc)
        {

            IEnumerable<Claim> claims = new Claim[]
                    {
                    new Claim(ClaimTypes.Name, acc.Ten),
                    new Claim("RowID", acc.RowID.ToString()),
                    new Claim("MaNV", acc.MaNV),
                    new Claim("HoTen",acc.HoTen),
                    new Claim("Ten", acc.Ten),
                    new Claim("DiaChi", acc.DiaChiThuongTru),
                    new Claim("NgaySinh", acc.NgaySinh.ToString()),
                    new Claim("DienThoai",  acc.DienThoai),
                    new Claim("Email", acc.Email),
                    new Claim("PhongBan", acc.PhongBan),
                    new Claim("MaPhongBan", acc.MaPhongBan),
                    new Claim("ChiNhanh", acc.ChiNhanh),
                    new Claim("TenHinh", acc.TenHinh),
                    new Claim("Per_Group", acc.Per_Group),
                    new Claim("TenDangNhap", acc.TenDangNhap),
                    new Claim("Active", acc.Active),
                    //phan quyen phong ban
                    new Claim("TNMoi", acc.TNMoi),
                    new Claim("TBao", acc.TBao),
                    new Claim("BCVe", acc.BCVe),
                    new Claim("NBo", acc.NBo),
                    new Claim("DLi", acc.DLi),
                    new Claim("KToan", acc.KToan),
                    new Claim("KDoanh", acc.KDoanh),
                    new Claim("PVe", acc.PVe),
                    new Claim("BPDoan", acc.BPDoan),
                    new Claim("HDon", acc.HDon),
                    new Claim("CA", acc.CA),
                    new Claim("YSao", acc.YSao),
                    new Claim("CS", acc.CS),
                    new Claim("DTa", acc.DTa),
                    new Claim("STing", acc.STing),
                    new Claim("KThuat", acc.KThuat),
                    new Claim("Dulich", acc.Dulich),
                    //phan quyen member
                    new Claim("TNMoiTV", acc.TNMoiTV),
                    new Claim("TBaoTV", acc.TBaoTV),
                    new Claim("BCVeTV", acc.BCVeTV),
                    new Claim("NBoTV", acc.NBoTV),
                    new Claim("DLiTV", acc.DLiTV),
                    new Claim("KToanTV", acc.KToanTV),
                    new Claim("KDoanhTV", acc.KDoanhTV),
                    new Claim("PVeTV", acc.PVeTV),
                    new Claim("BPDoanTV", acc.BPDoanTV),
                    new Claim("HDonTV", acc.HDonTV),
                    new Claim("CATV", acc.CATV),
                    new Claim("YSaoTV", acc.YSaoTV),
                    new Claim("CSTV", acc.CSTV),
                    new Claim("DTaTV", acc.DTaTV),
                    new Claim("STingTV", acc.STingTV),
                    new Claim("KThuatTV", acc.KThuatTV),
                    new Claim("DulichTV", acc.DulichTV),

                };
            return claims;


        }


        [HttpPost]

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");

        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(MenuController.Index), "Menu");
            }
        }

        [HttpPost]
        public JsonResult ForgetPassword(string email)
        {

            try
            {
                string result = LoginRepository.ForgetPassword(email);
                ViewBag.msg = result;
                return Json(result);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}














































