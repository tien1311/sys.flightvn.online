using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Html;
using Manager.Model.Models;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;
using Microsoft.Extensions.Configuration;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace Manager_EV.Areas.ContentArea.Controllers
{
    [Area(AreaNameConst.AREA_Content)]
    public class ContentController : Controller
    {
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;
        public ContentController(IUnitOfWork_Repository unitOfWork_Repository)
        {
            _unitOfWork_Repository = unitOfWork_Repository;
        }
        public IActionResult ViewContent(int subject_id)
        {
            SubjectModel content = _unitOfWork_Repository.BangTin_Rep.ContentYS(subject_id);
            if (subject_id == 141)
            {
                ViewBag.Title = "Yến Sào";
            }
            if (subject_id == 143)
            {
                ViewBag.Title = "Khen Thưởng";
            }
            if (subject_id == 145)
            {
                ViewBag.Title = "Kỷ Luật";
            }
            return View(content);
        }
    }
}
