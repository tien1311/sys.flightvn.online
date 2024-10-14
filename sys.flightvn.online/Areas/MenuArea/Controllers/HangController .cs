using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Manager.Model.Services.Abstraction;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;

namespace Manager_EV.Areas.MenuArea.Controllers
{

    [Area(AreaNameConst.AREA_Menu)]
    public class HangController : Controller
    {
        private readonly INotifyService _notifyService;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAuthenticateService _authenticateService;
        public HangRepository hangRepository;
        public HangController(IConfiguration configuration, IHttpClientFactory httpClientFactory, IAuthenticateService authenticateService, INotifyService notifyService)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _authenticateService = authenticateService;
            _notifyService = notifyService;
            hangRepository = new HangRepository(_configuration, _httpClientFactory, _authenticateService, _notifyService);
        }

        public async Task<IActionResult> SoDuHang(string searchVeBtn, string searchVeVNA)
        {

            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (searchVeBtn != "" && searchVeBtn != null)
            {
                bool result_data = await hangRepository.LuuSoDuHang(acc.Ten);
                if (result_data == false)
                {
                    TempData["thongbaoError"] = "Chưa lấy được dữ liệu, xin vui lòng liên hệ IT";
                }
            }
            else
            {
                if (searchVeVNA != "" && searchVeVNA != null)
                {
                    bool result_data = hangRepository.LuuSoDuHangVNA(acc.Ten);
                    if (result_data == false)
                    {
                        TempData["thongbaoError"] = "Chưa lấy được dữ liệu, xin vui lòng liên hệ IT";
                    }
                }
            }
            List<SoDuHangViewModel> result = hangRepository.ListSoDuHang(0);
            return View(result);
        }
    }
}