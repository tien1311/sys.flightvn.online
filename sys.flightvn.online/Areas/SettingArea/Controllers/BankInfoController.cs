using EasyInvoice.Json;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Model.Models;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace Manager_EV.Areas.SettingArea.Controllers
{
    [Area(AreaNameConst.AREA_Setting)]
    public class BankInfoController : Controller
    {
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;
        public BankInfoController(IUnitOfWork_Repository unitOfWork_Repository)
        {
            _unitOfWork_Repository = unitOfWork_Repository;
        }
        public IActionResult Bank()
        {
            List<BANK_ACCOUNT> result = _unitOfWork_Repository.Bank_Rep.Bank();
            return View(result);
        }
        public IActionResult CreateBank()
        {
            var ListBank = _unitOfWork_Repository.Bank_Rep.ListBank();
            return View(ListBank);
        }
        public IActionResult EditBank(int ID)
        {
            BANK_ACCOUNT result = _unitOfWork_Repository.Bank_Rep.EditBank(ID);
            ViewData["ListBank"] = _unitOfWork_Repository.Bank_Rep.ListBank();
            return View(result);
        }
        public IActionResult DeleteBank(int ID)
        {
            bool result = _unitOfWork_Repository.Bank_Rep.DeleteBank(ID);
            return Json(result);
        }
        public IActionResult ActiveBank(int ID)
        {
            bool result = _unitOfWork_Repository.Bank_Rep.ActiveBank(ID);
            return Json(result);
        }
        public IActionResult SaveCreateBank(string data)
        {
            BANK_ACCOUNT model = JsonConvert.DeserializeObject<BANK_ACCOUNT>(data);
            bool result = _unitOfWork_Repository.Bank_Rep.SaveCreateBank(model);
            return Json(result);
        }
        public IActionResult SaveEditBank(string data)
        {
            BANK_ACCOUNT model = JsonConvert.DeserializeObject<BANK_ACCOUNT>(data);
            bool result = _unitOfWork_Repository.Bank_Rep.SaveEditBank(model);
            return Json(result);
        }
    }
}
