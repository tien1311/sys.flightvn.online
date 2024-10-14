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
    public class ThongBaoDaiLyController : Controller
    {
        //ThongBaoDaiLyRepository _unitOfWork_Repository.ThongBaoDaiLy_Rep = new ThongBaoDaiLyRepository();
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;
        public ThongBaoDaiLyController(IUnitOfWork_Repository unitOfWork_Repository)
        {
            _unitOfWork_Repository = unitOfWork_Repository;
        }
        public IActionResult TinhTrang()
        {
            List<TinhTrangKhoa> result = _unitOfWork_Repository.ThongBaoDaiLy_Rep.TinhTrang();
            return View(result);
        }
        public IActionResult CreateTinhTrang()
        {
            return View();
        }
        public IActionResult SaveCreateTinhTrang(string Name, string PB)
        {
            bool result = _unitOfWork_Repository.ThongBaoDaiLy_Rep.SaveCreateTinhTrang(Name, PB);
            return Json(result);
        }
        public IActionResult EditTinhTrang(int ID)
        {
            TinhTrangKhoa result = _unitOfWork_Repository.ThongBaoDaiLy_Rep.EditTinhTrang(ID);
            return View(result);
        }
        public IActionResult SaveEditTinhTrang(int ID, string Name, string PB)
        {
            bool result = _unitOfWork_Repository.ThongBaoDaiLy_Rep.SaveEditTinhTrang(ID, Name, PB);
            return Json(result);
        }
        public IActionResult DeleteTinhTrang(int ID)
        {
            bool result = _unitOfWork_Repository.ThongBaoDaiLy_Rep.DeleteTinhTrang(ID);
            return Json(result);
        }
        public IActionResult ActiveTinhTrang(int ID)
        {
            bool result = _unitOfWork_Repository.ThongBaoDaiLy_Rep.ActiveTinhTrang(ID);
            return Json(result);
        }





        public IActionResult NoiDung()
        {
            List<NoiDungKhoa> result = _unitOfWork_Repository.ThongBaoDaiLy_Rep.NoiDung();
            return View(result);
        }
        public IActionResult CreateNoiDung()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult SaveCreateNoiDung(string TieuDe, string TT, string NoiDung, string NoiDungTimKiem)
        {
            bool result = _unitOfWork_Repository.ThongBaoDaiLy_Rep.SaveCreateNoiDung(TieuDe, TT, NoiDung, NoiDungTimKiem);
            return Json(result);
        }
        public IActionResult EditNoiDung(int ID)
        {
            NoiDungKhoa result = _unitOfWork_Repository.ThongBaoDaiLy_Rep.EditNoiDung(ID);
            return PartialView(result);
        }
        public IActionResult SaveEditNoiDung(int ROWID, string TieuDe, string TT, string NoiDung, string NoiDungTimKiem)
        {
            bool result = _unitOfWork_Repository.ThongBaoDaiLy_Rep.SaveEditNoiDung(ROWID, TieuDe, TT, NoiDung, NoiDungTimKiem);
            return Json(result);
        }

    }
}
