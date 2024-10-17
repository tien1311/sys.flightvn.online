using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.Serialization.Formatters.Binary;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Microsoft.Extensions.Configuration;

namespace Manager_EV.Areas.HomeArea.Controllers
{
    [Area(AreaNameConst.AREA_Home)]
    public class KhachHangController : Controller
    {
        private IUnitOfWork_Repository _unitOfWork_Repository;
        public KhachHangController(IUnitOfWork_Repository unitOfWork_Repository)
        {
            _unitOfWork_Repository = unitOfWork_Repository;
        }
        public IActionResult KhachHangVip()
        {
            List<KhachHangModel> result = new List<KhachHangModel>();
            result = _unitOfWork_Repository.KhachHang_Rep.KhachHangVip();
            return View(result);
        }
        public JsonResult AddKhachHangVip( string name, string Chucvu, string hang, bool block, string mien, string NGT, string phone, string birthday, string address, string ngaytang, string gift, string note, string nhom, string nvkd)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string tennv = acc.HoTen;        
            int IsHotro = block ? 1 : 0;           
            bool ret = _unitOfWork_Repository.KhachHang_Rep.SaveKHV( name, Chucvu, hang, mien, NGT, phone, birthday, address, IsHotro, ngaytang, gift, note, nhom, nvkd, tennv);         
            return Json(ret);
        }
        public JsonResult EditKhachHangVip(string ID, string name, string Chucvu, string hang, bool block, string mien, string NGT, string phone, string birthday, string address, string ngaytang, string gift, string note, string nhom, string nvkd)
        {
            
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string tennv = acc.HoTen;

          
            int IsHotro = block ? 1 : 0;

           
            bool ret = _unitOfWork_Repository.KhachHang_Rep.EditKHV(ID, name, Chucvu, hang, mien, NGT, phone, birthday, address, IsHotro, ngaytang, gift, note, nhom, nvkd, tennv);

            // Trả về kết quả dưới dạng JSON
            return Json(ret);
        }
        public IActionResult ChiTietKhachHang(string khoachinh)
        {
            KhachHangModel result = _unitOfWork_Repository.KhachHang_Rep.ChiTietKH(khoachinh);
            List<ListNhanVienKinhDoanh> DSnhanvienkinhdoanh = _unitOfWork_Repository.KhachHang_Rep.DSNhanVienKinhDoanh();
            ViewBag.DSnhanvienkinhdoanh = DSnhanvienkinhdoanh;
            return View(result);
        }
        public IActionResult TaoMoiKhachHang()
        {
            List<ListNhanVienKinhDoanh> DSnhanvienkinhdoanh = _unitOfWork_Repository.KhachHang_Rep.DSNhanVienKinhDoanh();
            ViewBag.DSnhanvienkinhdoanh = DSnhanvienkinhdoanh;
            return View();
        }
        public IActionResult NhomDL(string MaKH, string searchKH, string MaKHtxt, string NhomDL1, string TenCtytxt, string ghichutxt, string save, string searchNhom, string NhomDL2)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //GuiMailDaiLyRepository guimail_Rep = new GuiMailDaiLyRepository();
            KhachHangVIPModel result = new KhachHangVIPModel();
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (searchKH != null)
            {
                result = _unitOfWork_Repository.KhachHang_Rep.SearchKH(MaKH);
                return View(result);
            }
            if (save != null)
            {
                if (NhomDL1 == "0")
                {
                    ViewBag.thongbaokhachhang = "Bạn đã phải chọn nhóm khách hàng VIP";
                }
                else
                {
                    string result_Insert = _unitOfWork_Repository.KhachHang_Rep.InsertDL(MaKHtxt, TenCtytxt, NhomDL1, ghichutxt);
                    ViewBag.thongbaokhachhang = result_Insert;
                }
            }
            if (searchNhom != null)
            {
                result = _unitOfWork_Repository.KhachHang_Rep.SearchSelect(NhomDL2);
                return View("NhomDL", result);
            }

            result = _unitOfWork_Repository.KhachHang_Rep.NhomDL();
            return View("NhomDL", result);
        }
        public IActionResult EditKhachHang(string khoachinh)
        {
            Show result = _unitOfWork_Repository.KhachHang_Rep.EditKhachHang(khoachinh);
            return View(result);

        }
        public IActionResult Edit(int ID, string MaKH, string TenKH, string NhomKH, string GhiChu, string Edit, string Hidden)
        {
            bool hid = true;
            KhachHangVIPModel result = new KhachHangVIPModel();
            //GuiMailDaiLyRepository guimail_Rep = new GuiMailDaiLyRepository();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string tennv = acc.HoTen;
            if (Hidden != "on")
            {
                hid = false;
            }
            if (Edit != null)
            {
                bool ret = _unitOfWork_Repository.KhachHang_Rep.Edit(ID, MaKH, TenKH, NhomKH, GhiChu, hid);
                if (ret == true)
                {
                    TempData["thongbaoSuccess"] = "Bạn đã lưu khách hàng thành công";
                }
                else TempData["thongbaoError"] = "Bạn đã lưu không thành công";
            }
            result = _unitOfWork_Repository.KhachHang_Rep.NhomDL();
            return View("NhomDL", result);
        }
        [HttpGet]
        public IEnumerable<NhomDL> GetDepartments(int id)
        {
            return _unitOfWork_Repository.KhachHang_Rep.GetDepartments(id);
        }

    }
}
