using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Manager.DataAccess.Repository;
using Manager.Model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager_EV.Areas.DataArea.Controllers
{
    [Area(AreaNameConst.AREA_Data)]
    public class LotusmileController : Controller
    {
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;
        public LotusmileController(IUnitOfWork_Repository unitOfWork_Repository)
        {
            _unitOfWork_Repository = unitOfWork_Repository;
        }
        public IActionResult Lotusmile()
        {
            List<LotusmileModel> result = _unitOfWork_Repository.Lotusmile_Rep.Lotusmile();
            return View(result);
        }
        public JsonResult DeleteLotusmile(int ID)
        {
            bool result = _unitOfWork_Repository.Lotusmile_Rep.DeleteLotusmile(ID);
            return Json(result);
        }
    }
}
