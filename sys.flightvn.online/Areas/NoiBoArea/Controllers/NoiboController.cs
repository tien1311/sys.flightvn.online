using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Routing;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace Manager_EV.Areas.NoiBoArea.Controllers
{
    [Area(AreaNameConst.AREA_NoiBo)]
    public class NoiboController : Controller
    {
        //KyNiemRepository kyNiem_Rep = new KyNiemRepository();
        KyNiemModel result = new KyNiemModel();
        private IUnitOfWork_Repository _unitOfWork_Repository;
        public NoiboController(IUnitOfWork_Repository unitOfWork_Repository)
        {
            _unitOfWork_Repository = unitOfWork_Repository;
        }
        public IActionResult DSkyniem()
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token

            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            result = _unitOfWork_Repository.KyNiem_Rep.DSkyniem();
            return View("kyniem", result);
        }

        public IActionResult kyniem(string event_id, string nam)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            result = _unitOfWork_Repository.KyNiem_Rep.kyniem(event_id, nam);
            return View("kyniem", result);
        }
        public IActionResult News(int? page = 1, int pageSize = 7)
        {
            try
            {
                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                //Hết token


                ViewBag.tieude = "Bản Tin Flight VN";
                List<SubjectModel> list = _unitOfWork_Repository.BangTin_Rep.BangTin();

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "News";
                model.RouteValue = new RouteValueDictionary {
                        { "i", 7}
                    };
                return View("News", model);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult KhenThuongKyLuatNoiBo(int? page = 1, int pageSize = 7)
        {
            try
            {
                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                //Hết token


                ViewBag.tieude = "Khen thưởng và kỷ luật";
                List<SubjectModel> list = _unitOfWork_Repository.BangTin_Rep.KhenThuongKyLuatNoiBo();

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "KhenThuongKyLuatNoiBo";
                model.RouteValue = new RouteValueDictionary {
                        { "i", 7}
                    };
                return View("KhenThuongKyLuatNoiBo", model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult QuyDinh(int? page = 1, int pageSize = 7)
        {
            try
            {


                ViewBag.tieude = "QUY ĐỊNH Flight VN";

                List<SubjectModel> list = _unitOfWork_Repository.BangTin_Rep.QuyDinh();

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "QuyDinh";
                model.RouteValue = new RouteValueDictionary {
                    { "i", 7}
                };
                return View("News", model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult QuyDinhOld(int? page = 1, int pageSize = 7)
        {
            try
            {


                ViewBag.tieude = "QUY ĐỊNH Flight VN";

                List<SubjectModel> list = _unitOfWork_Repository.BangTin_Rep.QuyDinhOld();

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "QuyDinhOld";
                model.RouteValue = new RouteValueDictionary {
                    { "i", 7}
                };
                return View("NewsOld", model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult QuyDinhDL(int? page = 1, int pageSize = 7)
        {
            try
            {


                ViewBag.tieude = "QUY ĐỊNH PHÒNG DU LỊCH";

                List<SubjectModel> list = _unitOfWork_Repository.BangTin_Rep.QuyDinhDL();

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "QuyDinhDL";
                model.RouteValue = new RouteValueDictionary {
                    { "i", 7}
                };
                return View("News", model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult QuyDinhDoan(int? page = 1, int pageSize = 7)
        {
            try
            {


                ViewBag.tieude = "QUY ĐỊNH BỘ PHẬN ĐOÀN";

                List<SubjectModel> list = _unitOfWork_Repository.BangTin_Rep.QuyDinhDoan();

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "QuyDinhDoan";
                model.RouteValue = new RouteValueDictionary {
                    { "i", 7}
                };
                return View("News", model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult QuyDinhKT(int? page = 1, int pageSize = 7)
        {
            try
            {
                ViewBag.tieude = "QUY ĐỊNH PHÒNG KẾ TOÁN";

                List<SubjectModel> list = _unitOfWork_Repository.BangTin_Rep.QuyDinhKT();

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "QuyDinhKT";
                model.RouteValue = new RouteValueDictionary {
                    { "i", 7}
                };
                return View("News", model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult BangTinChamCong(int? page = 1, int pageSize = 7)
        {
            try
            {


                ViewBag.tieude = "Bản tin chấm công";

                List<SubjectModel> list = _unitOfWork_Repository.BangTin_Rep.BangTinChamCong();

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "BangTinChamCong";
                model.RouteValue = new RouteValueDictionary {
                    { "i", 7}
                };
                return View("News", model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult DetailNews(int subject_id)
        {
            SubjectModel content = _unitOfWork_Repository.BangTin_Rep.Content(subject_id);

            return View("DetailNews", content);
        }
        public IActionResult DetailNewsOld(int subject_id)
        {
            SubjectModel content = _unitOfWork_Repository.BangTin_Rep.ContentOld(subject_id);

            return View("DetailNews", content);
        }
        public IActionResult LichLamViec()
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            List<LichLamViecModel> lamviec = new List<LichLamViecModel>();
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            lamviec = _unitOfWork_Repository.LichLamViec_Rep.LichLamViec();
            return View("LichLamViec", lamviec);
        }
        public IActionResult DetailNewsNoiBo()
        {
            SubjectModel content = _unitOfWork_Repository.BangTin_Rep.ContentNoiBo();

            return View("DetailNews", content);
        }

    }
}
