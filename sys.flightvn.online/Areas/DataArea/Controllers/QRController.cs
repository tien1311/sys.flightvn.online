using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Model.Models;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace Manager_EV.Areas.DataArea.Controllers
{
    [Area(AreaNameConst.AREA_Data)]
    public class QRController : Controller
    {
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;
        public QRController(IUnitOfWork_Repository unitOfWork_Repository)
        {
            _unitOfWork_Repository = unitOfWork_Repository;
        }
        public IActionResult ListAddInfo()
        {
            List<SuggestAddInfo> result = _unitOfWork_Repository.QR_Rep.ListAddInfo();
            return View(result);
        }
        public IActionResult CreateDescription()
        {
            return View();
        }
        public IActionResult EditDescription(int ID)
        {
            SuggestAddInfo result = _unitOfWork_Repository.QR_Rep.EditDescription(ID);
            return View(result);
        }
        public IActionResult DeleteDescription(int ID)
        {
            bool result = _unitOfWork_Repository.QR_Rep.DeleteDescription(ID);
            return Json(result);
        }
        public IActionResult ActiveDescription(int ID)
        {
            bool result = _unitOfWork_Repository.QR_Rep.ActiveDescription(ID);
            return Json(result);
        }
        public IActionResult SaveCreateDescription(string Description)
        {
            bool result = _unitOfWork_Repository.QR_Rep.SaveCreateDescription(Description);
            return Json(result);
        }
        public IActionResult SaveEditDescription(string Description, int ID)
        {
            bool result = _unitOfWork_Repository.QR_Rep.SaveEditDescription(ID, Description);
            return Json(result);
        }
    }
}
