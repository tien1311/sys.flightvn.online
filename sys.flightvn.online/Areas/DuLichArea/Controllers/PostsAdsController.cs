using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Security.Policy;
using System.IO;
using System.Net;
using System;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace Manager_EV.Areas.DuLichArea.Controllers
{
    [Area(AreaNameConst.AREA_DuLich)]

    public class PostsAdsController : Controller
    {
        private IUnitOfWork_Repository _unitOfWork_Repository;
        public PostsAdsController(IUnitOfWork_Repository unitOfWork_Repository)
        {
            _unitOfWork_Repository = unitOfWork_Repository;
        }
        public IActionResult PostsAds()
        {
            List<PostsAdsModel> result = _unitOfWork_Repository.PostsAds_Rep.PostsAds();
            return View(result);
        }
        public IActionResult CreatePostsAds()
        {
            PostsAdsModel result = _unitOfWork_Repository.PostsAds_Rep.CreatePostsAds();
            return PartialView("CreatePostsAds", result);
        }
        public IActionResult EditPostsAds(int ID)
        {
            PostsAdsModel result = _unitOfWork_Repository.PostsAds_Rep.EditPostsAds(ID);
            return PartialView("EditPostsAds", result);
        }
        public IActionResult SaveCreatePostsAds(int Category, string Title, string CreateContent)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Repository.PostsAds_Rep.SaveCreatePostsAds(Category, Title, CreateContent, acc.MaNV);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu bài viết thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu bài viết không thành công";

            List<PostsAdsModel> result = _unitOfWork_Repository.PostsAds_Rep.PostsAds();
            return View("PostsAds", result);
        }
        public IActionResult SaveEditPostsAds(string ID, string Title, string CreateContent, int Category)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Repository.PostsAds_Rep.SaveEditPostsAds(ID, Title, CreateContent, Category);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã chỉnh sửa bài viết thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã chỉnh sửa bài viết không thành công";

            List<PostsAdsModel> result = _unitOfWork_Repository.PostsAds_Rep.PostsAds();
            return View("PostsAds", result);
        }
        public IActionResult BannerAds()
        {
            Upload_ImgModel result = _unitOfWork_Repository.PostsAds_Rep.BannerAdsDuLich();
            return View(result);
        }
        [HttpPost]
        public IActionResult SaveImgBannerAds(IFormFile file, string status, string Link, string HinhDaUP)
        {
            string url = "";
            if (Link == null)
            {
                Link = "";
            }
            if (file != null)
            {
                url = UploadImg(file);
            }
            else
            {
                url = HinhDaUP;
            }
            bool ret = _unitOfWork_Repository.PostsAds_Rep.SaveImgBannerAdsDuLich(url, status, Link);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã không lưu thành công";
            Upload_ImgModel result = _unitOfWork_Repository.PostsAds_Rep.BannerAdsDuLich();
            return View("BannerAds", result);
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
    }
}
