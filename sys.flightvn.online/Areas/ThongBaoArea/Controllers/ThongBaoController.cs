using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Routing;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;

namespace Manager_EV.Areas.ThongBaoArea.Controllers
{
    [Area(AreaNameConst.AREA_ThongBao)]
    public class ThongBaoController : Controller
    {
        ThongBaoRepository ThongBao_Rep = new ThongBaoRepository();
        public IActionResult ThongBao()
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ThongBaoModel listThongBao = ThongBao_Rep.ThongBao(acc.MaNV);
            List<ThongBaoSinhNhatDaiLi> listThongbaosinhnhatdaili = ThongBao_Rep.GetThongBaoSinhNhatDaiLi(acc.MaNV);
            ViewBag.Thongbaosinhnhatdaili = listThongbaosinhnhatdaili;
            return View(listThongBao);
        }
        public IActionResult ChiTietThongBao(int khoachinh)
        {
            ChiTietTB Thongbao = ThongBao_Rep.ChiTietThongBao(khoachinh);
            return PartialView("ChiTietThongBao", Thongbao);
        }
        [HttpPost]
        public IActionResult DanhDauDaXem(int khoachinh)
        {
            bool result = ThongBao_Rep.DanhDauDaXem(khoachinh);
            return Json(result);
        }
        [HttpPost]
        public IActionResult ChangeActiveKHV(int ID)
        {
            bool result = ThongBao_Rep.ActiveKHV(ID);
            return Json(result);
        }
    }
}
