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
    public class ForumController : Controller
    {
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;

        public ForumController(IUnitOfWork_Repository unitOfWork_Repository)
        {
            _unitOfWork_Repository = unitOfWork_Repository;
        }

        public IActionResult DuyetBaiViet()
        {
            List<PostForumModel> result = _unitOfWork_Repository.Forum_Rep.DuyetBaiViet();
            return View(result);
        }
        public IActionResult ViewPosts(int ID)
        {
            PostForumModel result = _unitOfWork_Repository.Forum_Rep.ViewPosts(ID);
            return PartialView("ViewPosts", result);
        }
        public IActionResult ConfirmPost(int ID, string Status, string Note)
        {
            bool ret = _unitOfWork_Repository.Forum_Rep.ConfirmPost(ID, Status, Note);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn cập nhật tình trạng thành công";
            }
            else TempData["thongbaoError"] = "Bạn cập nhật tình trạng không thành công";
            List<PostForumModel> result = _unitOfWork_Repository.Forum_Rep.DuyetBaiViet();
            return View("DuyetBaiViet", result);
        }
    }
}
