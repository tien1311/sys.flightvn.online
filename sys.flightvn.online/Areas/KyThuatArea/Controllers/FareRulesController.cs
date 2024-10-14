using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyInvoice.Json;
using Manager.Model.Models;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace Manager_EV.Areas.KyThuatArea.Controllers
{
    [Area(AreaNameConst.AREA_KyThuat)]
    public class FareRulesController : Controller
    {
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;
        public FareRulesController(IUnitOfWork_Repository unitOfWork_Repository)
        {
            _unitOfWork_Repository = unitOfWork_Repository;
        }
        //FareRulesRepository _unitOfWork_Repository.FareRules_Rep = new FareRulesRepository();
        public IActionResult Rules_Airlines()
        {
            List<Rules_Airlines> result = _unitOfWork_Repository.FareRules_Rep.Rules_Airlines();
            return View(result);
        }
        public IActionResult Rules_Categories()
        {
            List<Rule_Categories> result = _unitOfWork_Repository.FareRules_Rep.Rules_Categories();
            return View(result);
        }
        public IActionResult Rules_Partners()
        {
            List<Rules_Partners> result = _unitOfWork_Repository.FareRules_Rep.Rules_Partners();
            return View(result);
        }
        public IActionResult Rules_RuleDetails()
        {
            List<Rules_RuleDetails> result = _unitOfWork_Repository.FareRules_Rep.Rules_RuleDetails();
            return View(result);
        }
        public IActionResult CreateRules_Airlines()
        {
            return PartialView();
        }
        public IActionResult SaveCreateRules_Airlines(Rules_Airlines model)
        {
            bool ret = _unitOfWork_Repository.FareRules_Rep.SaveCreateRules_Airlines(model);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã tạo mới thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã tạo mới không thành công";

            List<Rules_Airlines> result = _unitOfWork_Repository.FareRules_Rep.Rules_Airlines();
            return View("Rules_Airlines", result);
        }
        public IActionResult CreateRules_Categories()
        {
            var ListAirlines = _unitOfWork_Repository.FareRules_Rep.Rules_Airlines();

            return PartialView(ListAirlines);
        }
        public IActionResult SaveCreateRules_Categories(Rule_Categories model)
        {
            bool ret = _unitOfWork_Repository.FareRules_Rep.SaveCreateRules_Categories(model);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã tạo mới thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã tạo mới không thành công";

            List<Rule_Categories> result = _unitOfWork_Repository.FareRules_Rep.Rules_Categories();
            return View("Rules_Categories", result);
        }
        public IActionResult CreateRules_Partners()
        {
            return PartialView();
        }
        public IActionResult SaveCreateRules_Partners(Rules_Partners model)
        {
            bool ret = _unitOfWork_Repository.FareRules_Rep.SaveCreateRules_Partners(model);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã tạo mới thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã tạo mới không thành công";

            List<Rules_Partners> result = _unitOfWork_Repository.FareRules_Rep.Rules_Partners();
            return View("Rules_Partners", result);
        }
        public IActionResult CreateRules_RuleDetails()
        {
            var ListAirlines = _unitOfWork_Repository.FareRules_Rep.Rules_Airlines();
            return PartialView(ListAirlines);
        }
        public IActionResult SaveCreateRules_RuleDetails(string data)
        {
            Rules_RuleDetails model = JsonConvert.DeserializeObject<Rules_RuleDetails>(data);
            bool result = _unitOfWork_Repository.FareRules_Rep.SaveCreateRules_RuleDetails(model);
            return Json(result);
        }
        public IActionResult EditRules_Airlines(int ID)
        {
            Rules_Airlines result = new Rules_Airlines();
            result = _unitOfWork_Repository.FareRules_Rep.EditRules_Airlines(ID);
            return PartialView(result);
        }
        public IActionResult SaveEditRules_Airlines(Rules_Airlines model)
        {
            bool ret = _unitOfWork_Repository.FareRules_Rep.SaveEditRules_Airlines(model);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã chỉnh sửa thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã chỉnh sửa không thành công";

            List<Rules_Airlines> result = _unitOfWork_Repository.FareRules_Rep.Rules_Airlines();
            return View("Rules_Airlines", result);
        }
        public IActionResult EditRules_Categories(int ID)
        {
            Rule_Categories result = new Rule_Categories();
            result = _unitOfWork_Repository.FareRules_Rep.EditRules_Categories(ID);
            ViewData["ListAirlines"] = _unitOfWork_Repository.FareRules_Rep.Rules_Airlines();
            return PartialView(result);
        }
        public IActionResult SaveEditRules_Categories(Rule_Categories model)
        {
            bool ret = _unitOfWork_Repository.FareRules_Rep.SaveEditRules_Categories(model);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã chỉnh sửa thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã chỉnh sửa không thành công";

            List<Rule_Categories> result = _unitOfWork_Repository.FareRules_Rep.Rules_Categories();
            return View("Rules_Categories", result);
        }
        public IActionResult EditRules_Partners(int ID)
        {
            Rules_Partners result = new Rules_Partners();
            result = _unitOfWork_Repository.FareRules_Rep.EditRules_Partners(ID);
            return PartialView(result);
        }
        public IActionResult SaveEditRules_Partners(Rules_Partners model)
        {
            bool ret = _unitOfWork_Repository.FareRules_Rep.SaveEditRules_Partners(model);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã chỉnh sửa thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã chỉnh sửa không thành công";

            List<Rules_Partners> result = _unitOfWork_Repository.FareRules_Rep.Rules_Partners();
            return View("Rules_Partners", result);
        }
        public IActionResult EditRules_RuleDetails(int ID)
        {
            Rules_RuleDetails result = new Rules_RuleDetails();
            result = _unitOfWork_Repository.FareRules_Rep.EditRules_RuleDetails(ID);
            ViewData["ListAirlines"] = _unitOfWork_Repository.FareRules_Rep.Rules_Airlines();
            ViewData["ListPartners"] = _unitOfWork_Repository.FareRules_Rep.Rules_Partners();
            return PartialView(result);
        }
        public IActionResult SaveEditRules_RuleDetails(string data)
        {
            Rules_RuleDetails model = JsonConvert.DeserializeObject<Rules_RuleDetails>(data);
            bool result = _unitOfWork_Repository.FareRules_Rep.SaveEditRules_RuleDetails(model);
            return Json(result);
        }
        //public IActionResult DeleteRules_Airlines(int ID)
        //{
        //    bool ret = _unitOfWork_Repository.FareRules_Rep.DeleteRules_Airlines(ID);
        //    if (ret == true)
        //    {
        //        TempData["thongbao"] = "Bạn đã lưu bài viết thành công";
        //    }
        //    else TempData["thongbao"] = "Bạn đã lưu bài viết không thành công";

        //    List<Rules_Airlines> result = _unitOfWork_Repository.FareRules_Rep.Rules_Airlines();
        //    return View("Rules_Airlines", result);
        //}
        //public IActionResult DeleteRules_Categories(int ID)
        //{
        //    bool ret = _unitOfWork_Repository.FareRules_Rep.DeleteRules_Categories(ID);
        //    if (ret == true)
        //    {
        //        TempData["thongbao"] = "Bạn đã lưu bài viết thành công";
        //    }
        //    else TempData["thongbao"] = "Bạn đã lưu bài viết không thành công";

        //    List<Rule_Categories> result = _unitOfWork_Repository.FareRules_Rep.Rules_Categories();
        //    return View("Rules_Categories", result);
        //}
        //public IActionResult DeleteRules_Partners(int ID)
        //{
        //    bool ret = _unitOfWork_Repository.FareRules_Rep.DeleteRules_Partners(ID);
        //    if (ret == true)
        //    {
        //        TempData["thongbao"] = "Bạn đã lưu bài viết thành công";
        //    }
        //    else TempData["thongbao"] = "Bạn đã lưu bài viết không thành công";

        //    List<Rules_Partners> result = _unitOfWork_Repository.FareRules_Rep.Rules_Partners();
        //    return View("Rules_Partners", result);
        //}
        public IActionResult DeleteRules_PartnerDetails(int ID)
        {
            bool ret = _unitOfWork_Repository.FareRules_Rep.DeleteRules_PartnerDetails(ID);
            return Json(ret);
        }
        //public IActionResult DeleteRules_RuleDetails(int ID)
        //{
        //    return View();
        //}
        public IActionResult ListCategories(int AirlineID)
        {
            List<Rule_Categories> result = _unitOfWork_Repository.FareRules_Rep.ListCategories(AirlineID);
            return Json(result);
        }
        public IActionResult RuleDetail(int PartnerID)
        {
            List<Rules_RuleDetails> result = _unitOfWork_Repository.FareRules_Rep.RuleDetail(PartnerID);
            return PartialView(result);
        }
        public IActionResult ImportRule(int PartnerID)
        {
            ImportPartnerDetails result = new ImportPartnerDetails();
            List<Rules_RuleDetails> rel = _unitOfWork_Repository.FareRules_Rep.Rules_RuleDetails();
            result.PartnerID = PartnerID;
            result.ListRuleDetails = rel;
            return PartialView(result);
        }
        public IActionResult SaveImportRule(string data)
        {
            ImportPartnerDetails model = JsonConvert.DeserializeObject<ImportPartnerDetails>(data);
            bool result = _unitOfWork_Repository.FareRules_Rep.SaveImportRule(model);
            return Json(result);
        }
    }
}
