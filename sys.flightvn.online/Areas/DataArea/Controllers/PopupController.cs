using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using Manager.Model.Models;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;

namespace Manager_EV.Areas.DataArea.Controllers
{
    [Area(AreaNameConst.AREA_Data)]
    public class PopupController : Controller
    {
        PopupRepository Popup_Rep = new PopupRepository();
        private IHostingEnvironment _hostingEnvironment;
        CultureInfo provider;
        public PopupController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult UploadPopup()
        {
            Upload_ImgModel result = Popup_Rep.UploadPopup();
            return View(result);
        }
        [HttpPost]
        public IActionResult SaveImgPopup(IFormFile file, string Link, string status, string HinhDaUP)
        {
            string url = "", duongdan = "/UploadImg/PopupSys_Img/", FolName = "UploadImg/PopupSys_Img";
            if (Link == null)
            {
                Link = "";
            }
            if (file != null)
            {
                url = UploadImg(file, duongdan, FolName);
            }
            else
            {
                url = HinhDaUP;
            }
            bool ret = Popup_Rep.SaveImgPopup(url, Link, status);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã không lưu thành công";

            Upload_ImgModel result = Popup_Rep.UploadPopup();
            return View("UploadPopup", result);
        }
        [HttpPost]
        public IActionResult SaveImgBannerAirline24h(IFormFile file, string Link, string status, string HinhDaUP)
        {
            string url = "", duongdan = "/UploadImg/BannerAirline_Img/", FolName = "UploadImg/BannerAirline_Img";
            if (Link == null)
            {
                Link = "";
            }
            if (file != null)
            {
                url = UploadImg(file, duongdan, FolName);
            }
            else
            {
                url = HinhDaUP;
            }
            bool ret = Popup_Rep.SaveImgBannerAirline(url, Link, status);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã không lưu thành công";

            Upload_ImgModel result = Popup_Rep.UploadPopup();
            return View("UploadPopup", result);
        }
        public string UploadImg(IFormFile upload, string duongdan, string FolName)
        {
            string url = "";
            string folderName = FolName;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);

            var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
            string fullPath = Path.Combine(newPath, filename);
            var stream = new FileStream(fullPath, FileMode.Create);
            upload.CopyToAsync(stream);
            url = duongdan + filename;
            return url;
        }
        [HttpPost]
        public IActionResult SaveImgBannerEvbay(IFormFile file, string Link, string status, string HinhDaUP)
        {
            string url = "", duongdan = "/UploadImg/BannerEvbay/", FolName = "UploadImg/BannerEvbay";
            if (Link == null)
            {
                Link = "";
            }
            if (file != null)
            {
                url = UploadImg(file, duongdan, FolName);
            }
            else
            {
                url = HinhDaUP;
            }
            bool ret = Popup_Rep.SaveImgBannerEvbay(url, Link, status);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã không lưu thành công";

            Upload_ImgModel result = Popup_Rep.UploadPopup();
            return View("UploadPopup", result);
        }
        [HttpPost]
        public IActionResult SaveImgBannerADVEvbay(IFormFile file, string Link, string status, string HinhDaUP)
        {
            string url = "", duongdan = "/UploadImg/BannerEvbay/", FolName = "UploadImg/BannerEvbay";
            if (Link == null)
            {
                Link = "";
            }
            if (file != null)
            {
                url = UploadImg(file, duongdan, FolName);
            }
            else
            {
                url = HinhDaUP;
            }
            bool ret = Popup_Rep.SaveImgBannerADVEvbay(url, Link, status);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã không lưu thành công";

            Upload_ImgModel result = Popup_Rep.UploadPopup();
            return View("UploadPopup", result);
        }
        [HttpPost]
        public IActionResult SaveImgBannerADVVeDoanEvbay(IFormFile file, string Link, string status, string HinhDaUP)
        {
            string url = "", duongdan = "/UploadImg/BannerEvbay/", FolName = "UploadImg/BannerEvbay";
            if (Link == null)
            {
                Link = "";
            }
            if (file != null)
            {
                url = UploadImg(file, duongdan, FolName);
            }
            else
            {
                url = HinhDaUP;
            }
            bool ret = Popup_Rep.SaveImgBannerADVVeDoanEvbay(url, Link, status);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã không lưu thành công";

            Upload_ImgModel result = Popup_Rep.UploadPopup();
            return View("UploadPopup", result);
        }
        [HttpPost]
        public IActionResult SaveImgBannerADSEnvietGroup(IFormFile file, string Link, string status, string HinhDaUP)
        {
            string url = "", duongdan = "/UploadImg/BannerEvbay/", FolName = "UploadImg/BannerEvbay";
            if (Link == null)
            {
                Link = "";
            }
            if (file != null)
            {
                url = UploadImg(file, duongdan, FolName);
            }
            else
            {
                url = HinhDaUP;
            }
            bool ret = Popup_Rep.SaveImgBannerADSEnvietGroup(url, Link, status);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã không lưu thành công";

            Upload_ImgModel result = Popup_Rep.UploadPopup();
            return View("UploadPopup", result);
        }
    }
}
