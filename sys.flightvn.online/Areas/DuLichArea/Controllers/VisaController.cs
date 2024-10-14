using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace Manager_EV.Areas.DuLichArea.Controllers
{
    [Area(AreaNameConst.AREA_DuLich)]
    public class VisaController : Controller
    {
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;
        public VisaController(IUnitOfWork_Repository unitOfWork_Repository)
        {
            _unitOfWork_Repository = unitOfWork_Repository;
        }
        //VisaRepository _unitOfWork_Repository.Visa_Rep = new VisaRepository();
        public IActionResult Visa()
        {
            List<VisaModel> result = _unitOfWork_Repository.Visa_Rep.Visa();
            return View(result);
        }
        public IActionResult CreateVisa()
        {
            return View();
        }
        public IActionResult EditVisa(int ID)
        {
            VisaModel result = _unitOfWork_Repository.Visa_Rep.EditVisa(ID);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> SaveCreateVisa(string Name, string ShortDescription, string VisaType, List<bool> mainImages, List<IFormFile> imageFiles)
        {
            VisaModel data = new VisaModel();
            List<Image> ListImg = new List<Image>();
            data.Name = Name;
            data.VisaType = VisaType;
            data.ShortDescription = ShortDescription;
            for (int i = 0; i < mainImages.Count; i++)
            {
                Image img = new Image();
                img.MainImage = mainImages[i];
                img.ImageURL = UploadImg(imageFiles[i]);
                ListImg.Add(img);
            }
            data.ListImages = ListImg;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = await _unitOfWork_Repository.Visa_Rep.SaveCreateVisa(data, acc.MaNV);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu sản phẩm thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu sản phẩm không thành công";
            List<VisaModel> result = _unitOfWork_Repository.Visa_Rep.Visa();
            return View("Visa", result);
        }
        [HttpPost]
        public async Task<IActionResult> SaveEditVisa(int ID, string Name, string ShortDescription, string VisaType, List<bool> mainImages, List<IFormFile> imageFiles, List<bool> mainImgs, List<string> imagesURL)
        {
            VisaModel data = new VisaModel();
            List<Image> ListImg = new List<Image>();
            data.ID = ID;
            data.Name = Name;
            data.VisaType = VisaType;
            data.ShortDescription = ShortDescription;
            for (int i = 0; i < imageFiles.Count; i++)
            {
                Image img = new Image();
                img.MainImage = mainImages[i];
                img.ImageURL = UploadImg(imageFiles[i]); ;
                ListImg.Add(img);
            }

            for (int i = 0; i < imagesURL.Count; i++)
            {
                Image img = new Image();
                img.MainImage = mainImgs[i];
                img.ImageURL = imagesURL[i];
                ListImg.Add(img);
            }

            data.ListImages = ListImg;

            bool ret = await _unitOfWork_Repository.Visa_Rep.SaveEditVisa(data);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu sản phẩm thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu sản phẩm không thành công";

            List<VisaModel> result = _unitOfWork_Repository.Visa_Rep.Visa();
            return View("Visa", result);
        }
        [HttpPost]
        public JsonResult ChangeActiveVisa(int ID, int Active)
        {
            bool result = _unitOfWork_Repository.Visa_Rep.ChangeActiveVisa(ID, Active);
            return Json(result);
        }
        public IActionResult TypeVisa()
        {
            List<TypeVisa> result = _unitOfWork_Repository.Visa_Rep.TypeVisa();
            return View(result);
        }
        public IActionResult CreateType()
        {
            return View();
        }
        public IActionResult EditType(int ID)
        {
            TypeVisa result = _unitOfWork_Repository.Visa_Rep.EditTypeVisa(ID);
            return View(result);
        }
        public IActionResult SaveCreateType(TypeVisa data)
        {
            bool ret = _unitOfWork_Repository.Visa_Rep.SaveCreateType(data);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu sản phẩm thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu sản phẩm không thành công";
            List<TypeVisa> result = _unitOfWork_Repository.Visa_Rep.TypeVisa();
            return View("TypeVisa", result);
        }
        public IActionResult SaveEditType(TypeVisa data)
        {
            bool ret = _unitOfWork_Repository.Visa_Rep.SaveEditType(data);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu sản phẩm thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu sản phẩm không thành công";
            List<TypeVisa> result = _unitOfWork_Repository.Visa_Rep.TypeVisa();
            return View("TypeVisa", result);
        }
        public string UploadImg(IFormFile imageFiles)
        {
            string ftpServerUrl = "ftp://Manager.airline24h.com";
            string username = "envietDuLich";
            string password = "enviet@123";
            // Create FtpWebRequest object
            var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + imageFiles.FileName;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServerUrl + "/" + filename);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);
            // Upload the file to the FTP server
            using (Stream ftpStream = request.GetRequestStream())
            {
                imageFiles.CopyTo(ftpStream);
            }
            string http = "https://Manager.airline24h.com/upload/dulich/" + filename;
            return http;
        }
        public JsonResult DeleteImg(int id)
        {
            bool result = _unitOfWork_Repository.Visa_Rep.DeleteImg(id);
            return Json(result);
        }
        public IActionResult VisaBooking()
        {
            List<VisaBookingModel> result = _unitOfWork_Repository.Visa_Rep.VisaBooking();
            return View(result);
        }
        public IActionResult DetailVisaBooking(int ID)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            VisaBookingModel result = _unitOfWork_Repository.Visa_Rep.DetailVisaBooking(ID, acc.MaPhongBan, acc.HoTen);
            return View(result);
        }
        public async Task<JsonResult> ChangeStatus(int ID, int StatusID, string Note)
        {
            bool result = await _unitOfWork_Repository.Visa_Rep.ChangeStatus(ID, StatusID, Note);
            return Json(result);
        }
    }
}
