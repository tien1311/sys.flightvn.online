using Manager.Common.Helpers;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Manager.Model.Models;
using Manager.Model.Models.LandingPage;
using Manager.Model.Models.Other;
using Manager.Model.Models.PaginationBase;
using Manager.Model.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;
using Microsoft.Extensions.Configuration;
using RtfPipe.Model;
using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Manager_EV.Areas.LandingPageArea.Controllers
{
    [Area(AreaNameConst.AREA_LandingPage)]
    public class LandingPageController : Controller
    {
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;

        public LandingPageController(IUnitOfWork_Repository unitOfWork_Repository)
        {
            _unitOfWork_Repository = unitOfWork_Repository;
        }

        private Task<bool> IsFileOverSiveMB(IFormFile file, int MB)
        {
            if (file.Length > MB * 1024 * 1024)
            {
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Logo
        public async Task<IActionResult> Logo()
        {
            var item = await _unitOfWork_Repository.LandingPage_Rep.GetLogo();
            return View(item);
        }

        public async Task<IActionResult> UpdateLogo(Logo request, IFormFile ImageValue)
        {
            if (ImageValue != null)
            {
                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }

                // Xử lý cập nhật hình ảnh
                string oldImage = request.Image;
                request.Image = Common.UploadFileToSFTP(ImageValue);

                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.UpdateLogo(request);
                if (isSuccess)
                {
                    _ = Task.Run(() => Common.DeleteFileFromSFTP(oldImage));
                    return Json(new { success = true, message = "Cập nhật logo thành công" });
                }
                return Json(new { success = false, message = "Cập nhật logo thất bại, vui lòng liên hệ IT" });
            }

            return Json(new { success = false, message = "Vui lòng chọn tệp hình ảnh" });
        }

        #endregion


        #region CompanyInfo

        public async Task<IActionResult> CompanyInfo()
        {
            var listCompanyInfo = await _unitOfWork_Repository.LandingPage_Rep.GetCompanyInfo();
            return View(listCompanyInfo);
        }

        public IActionResult CreateCompanyInfo()
        {
            return View();
        }

        public async Task<IActionResult> SaveCreateCompanyInfo(CompanyInfo request, IFormFile ImageValue)
        {
            if (ImageValue != null)
            {
                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }

                // Xử lý cập nhật hình ảnh
                string oldImage = request.Image;
                request.Image = Common.UploadFileToSFTP(ImageValue);

                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveCreateCompanyInfo(request);
                if (isSuccess)
                {
                    return Json(new { success = true, message = "Thêm mới thông tin thành công" });
                }
                return Json(new { success = false, message = "Thêm mới thông tin thất bại, vui lòng liên hệ IT" });
            }

            return Json(new { success = false, message = "Vui lòng chọn tệp hình ảnh" });
        }

        public IActionResult EditCompanyInfo(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetCompanyInfo(id).Result;
            return View(item);
        }

        public async Task<IActionResult> SaveEditCompanyInfo(CompanyInfo request, IFormFile ImageValue)
        {
            if (ImageValue != null)
            {
                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }

                // Xử lý cập nhật hình ảnh
                string oldImage = request.Image;
                request.Image = Common.UploadFileToSFTP(ImageValue);

                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveEditCompanyInfo(request);
                if (isSuccess)
                {
                    _ = Task.Run(() => Common.DeleteFileFromSFTP(oldImage));
                    return Json(new { success = true, message = "Cập nhật thông tin thành công" });
                }
                return Json(new { success = false, message = "Cập nhật thông tin thất bại, vui lòng liên hệ IT" });
            }

            return Json(new { success = false, message = "Vui lòng chọn tệp hình ảnh" });
        }

        #endregion


        #region PartnerBanner
        public IActionResult PartnerBanner()
        {
            var items = _unitOfWork_Repository.LandingPage_Rep.GetPartnerBanner().GetAwaiter().GetResult();
            return View(items);
        }


        public IActionResult CreatePartnerBanner()
        {
            return View();
        }

        public async Task<IActionResult> SaveCreatePartnerBanner(PartnerBanner request, IFormFile ImageValue)
        {
            if (ImageValue != null)
            {
                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }

                // Xử lý cập nhật hình ảnh
                string oldImage = request.Image;
                request.Image = Common.UploadFileToSFTP(ImageValue);

                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveCreatePartnerBanner(request);
                if (isSuccess)
                {
                    return Json(new { success = true, message = "Thêm mới thông tin thành công" });
                }
                return Json(new { success = false, message = "Thêm mới thông tin thất bại, vui lòng liên hệ IT" });
            }

            return Json(new { success = false, message = "Vui lòng chọn tệp hình ảnh" });
        }

        public IActionResult EditPartnerBanner(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetPartnerBanner(id).Result;
            return View(item);
        }

        public async Task<IActionResult> SaveEditPartnerBanner(PartnerBanner request, IFormFile ImageValue)
        {
            if (ImageValue != null)
            {
                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }

                // Xử lý cập nhật hình ảnh
                string oldImage = request.Image;
                request.Image = Common.UploadFileToSFTP(ImageValue);

                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveEditPartnerBanner(request);
                if (isSuccess)
                {
                    _ = Task.Run(() => Common.DeleteFileFromSFTP(oldImage));
                    return Json(new { success = true, message = "Cập nhật thông tin thành công" });
                }
                return Json(new { success = false, message = "Cập nhật thông tin thất bại, vui lòng liên hệ IT" });
            }

            return Json(new { success = false, message = "Vui lòng chọn tệp hình ảnh" });
        }

        public async Task<IActionResult> IsActivedPartnerBanner(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetPartnerBanner(id).GetAwaiter().GetResult();
            if (item != null)
            {
                item.isActived = !item.isActived;
                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.ChangeActivePartnerBanner(id, item.isActived);
                if (isSuccess)
                {
                    return Json(new { success = true, item.isActived, message = "Đổi trạng thái thành công" });
                }
            }
            return Json(new { success = false, message = "Đổi trạng thái thất bại, vui lòng liên hệ IT" });
        }


        #endregion

        #region Service

        public IActionResult Service()
        {
            var items = _unitOfWork_Repository.LandingPage_Rep.GetService().GetAwaiter().GetResult();
            return View(items);
        }


        public IActionResult CreateService()
        {
            return View();
        }

        public async Task<IActionResult> SaveCreateService(Service request, IFormFile ImageValue)
        {
            if (ImageValue != null)
            {
                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }

                // Xử lý cập nhật hình ảnh
                string oldImage = request.Image;
                request.Image = Common.UploadFileToSFTP(ImageValue);

                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveCreateService(request);
                if (isSuccess)
                {
                    return Json(new { success = true, message = "Thêm mới thông tin thành công" });
                }
                return Json(new { success = false, message = "Thêm mới thông tin thất bại, vui lòng liên hệ IT" });
            }

            return Json(new { success = false, message = "Vui lòng chọn tệp hình ảnh" });
        }

        public IActionResult EditService(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetService(id).Result;
            return View(item);
        }

        public async Task<IActionResult> SaveEditService(Service request, IFormFile ImageValue)
        {
            if (ImageValue != null)
            {
                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }

                // Xử lý cập nhật hình ảnh
                string oldImage = request.Image;
                request.Image = Common.UploadFileToSFTP(ImageValue);

                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveEditService(request);
                if (isSuccess)
                {
                    _ = Task.Run(() => Common.DeleteFileFromSFTP(oldImage));
                    return Json(new { success = true, message = "Cập nhật thông tin thành công" });
                }
                return Json(new { success = false, message = "Cập nhật thông tin thất bại, vui lòng liên hệ IT" });
            }

            return Json(new { success = false, message = "Vui lòng chọn tệp hình ảnh" });
        }

        public async Task<IActionResult> DeleteService(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetService(id).GetAwaiter().GetResult();
            if (item != null)
            {
                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.DeleteService(id);
                if (isSuccess)
                {
                    _ = Task.Run(() => Common.DeleteFileFromSFTP(item.Image));
                    return Json(new { success = true, message = "Xoá thành công" });
                }
            }
            return Json(new { success = false, message = "Xoá thất bại, vui lòng liên hệ IT" });
        }

        #endregion

        #region SocialMedia
        public IActionResult SocialMedia()
        {
            var items = _unitOfWork_Repository.LandingPage_Rep.GetSocialMedia().GetAwaiter().GetResult();
            return View(items);
        }


        public IActionResult CreateSocialMedia()
        {
            return View();
        }

        public async Task<IActionResult> SaveCreateSocialMedia(SocialMedia request, IFormFile ImageValue)
        {
            if (ImageValue != null)
            {
                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }

                // Xử lý cập nhật hình ảnh
                string oldImage = request.Image;
                request.Image = Common.UploadFileToSFTP(ImageValue);

                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveCreateSocialMedia(request);
                if (isSuccess)
                {
                    return Json(new { success = true, message = "Thêm mới thông tin thành công" });
                }
                return Json(new { success = false, message = "Thêm mới thông tin thất bại, vui lòng liên hệ IT" });
            }

            return Json(new { success = false, message = "Vui lòng chọn tệp hình ảnh" });
        }

        public IActionResult EditSocialMedia(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetSocialMedia(id).Result;
            return View(item);
        }

        public async Task<IActionResult> SaveEditSocialMedia(SocialMedia request, IFormFile ImageValue)
        {
            if (ImageValue != null)
            {
                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }

                // Xử lý cập nhật hình ảnh
                string oldImage = request.Image;
                request.Image = Common.UploadFileToSFTP(ImageValue);

                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveEditSocialMedia(request);
                if (isSuccess)
                {
                    _ = Task.Run(() => Common.DeleteFileFromSFTP(oldImage));
                    return Json(new { success = true, message = "Cập nhật thông tin thành công" });
                }
                return Json(new { success = false, message = "Cập nhật thông tin thất bại, vui lòng liên hệ IT" });
            }

            return Json(new { success = false, message = "Vui lòng chọn tệp hình ảnh" });
        }

        public async Task<IActionResult> DeleteSocialMedia(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetSocialMedia(id).GetAwaiter().GetResult();
            if (item != null)
            {
                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.DeleteSocialMedia(id);
                if (isSuccess)
                {
                    _ = Task.Run(() => Common.DeleteFileFromSFTP(item.Image));
                    return Json(new { success = true, message = "Xoá thành công" });
                }
            }
            return Json(new { success = false, message = "Xoá thất bại, vui lòng liên hệ IT" });
        }

        #endregion


        #region Slider

        public IActionResult Slider()
        {
            var items = _unitOfWork_Repository.LandingPage_Rep.GetSlider().GetAwaiter().GetResult();
            return View(items);
        }


        public IActionResult CreateSlider()
        {
            return View();
        }

        public async Task<IActionResult> SaveCreateSlider(Slider request, IFormFile ImageValue)
        {
            if (ImageValue != null)
            {
                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }

                // Xử lý cập nhật hình ảnh
                string oldImage = request.Image;
                request.Image = Common.UploadFileToSFTP(ImageValue);

                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveCreateSlider(request);
                if (isSuccess)
                {
                    return Json(new { success = true, message = "Thêm mới thông tin thành công" });
                }
                return Json(new { success = false, message = "Thêm mới thông tin thất bại, vui lòng liên hệ IT" });
            }

            return Json(new { success = false, message = "Vui lòng chọn tệp hình ảnh" });
        }

        public IActionResult EditSlider(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetSlider(id).Result;
            return View(item);
        }

        public async Task<IActionResult> SaveEditSlider(Slider request, IFormFile ImageValue)
        {
            if (ImageValue != null)
            {
                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }

                // Xử lý cập nhật hình ảnh
                string oldImage = request.Image;
                request.Image = Common.UploadFileToSFTP(ImageValue);

                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveEditSlider(request);
                if (isSuccess)
                {
                    _ = Task.Run(() => Common.DeleteFileFromSFTP(oldImage));
                    return Json(new { success = true, message = "Cập nhật thông tin thành công" });
                }
                return Json(new { success = false, message = "Cập nhật thông tin thất bại, vui lòng liên hệ IT" });
            }

            return Json(new { success = false, message = "Vui lòng chọn tệp hình ảnh" });
        }

        public async Task<IActionResult> IsActivedSlider(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetSlider(id).GetAwaiter().GetResult();
            if (item != null)
            {
                item.IsActived = !item.IsActived;
                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.ChangeActiveSlider(id, item.IsActived);
                if (isSuccess)
                {
                    return Json(new { success = true, item.IsActived, message = "Đổi trạng thái thành công" });
                }
            }
            return Json(new { success = false, message = "Đổi trạng thái thất bại, vui lòng liên hệ IT" });
        }

        public async Task<IActionResult> DeleteSlider(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetSlider(id).GetAwaiter().GetResult();
            if (item != null)
            {
                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.DeleteSlider(id);
                if (isSuccess)
                {
                    _ = Task.Run(() => Common.DeleteFileFromSFTP(item.Image));
                    return Json(new { success = true, message = "Xoá thành công" });
                }
            }
            return Json(new { success = false, message = "Xoá thất bại, vui lòng liên hệ IT" });
        }
        #endregion

        #region Category

        public IActionResult Category()
        {
            var items = _unitOfWork_Repository.LandingPage_Rep.GetCategory().GetAwaiter().GetResult();
            return View(items);
        }


        public IActionResult CreateCategory()
        {
            return View();
        }

        public async Task<IActionResult> SaveCreateCategory(Manager.Model.Models.LandingPage.Category request, int HeaderValue)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (HeaderValue == 1)
            {
                request.IsHeaderMenu = true;
            }
            request.Alias = Filter.FilterChar(request.Name);
            request.CreatedBy = acc.HoTen;
            request.CreatedDate = DateTime.Now;

            bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveCreateCategory(request);
            if (isSuccess)
            {
                return Json(new { success = true, message = "Thêm mới thông tin thành công" });
            }
            return Json(new { success = false, message = "Thêm mới thông tin thất bại, vui lòng liên hệ IT" });
        }

        public IActionResult EditCategory(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetCategory(id).GetAwaiter().GetResult();
            return View(item);
        }

        public async Task<IActionResult> SaveEditCategory(Manager.Model.Models.LandingPage.Category request, int HeaderValue)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (HeaderValue == 1)
            {
                request.IsHeaderMenu = true;
            }
            //request.Alias = Filter.FilterChar(request.Name);
            request.ModifiedBy = acc.MaNV;
            request.ModifiedDate = DateTime.Now;

            bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveEditCategory(request);
            if (isSuccess)
            {
                return Json(new { success = true, message = "Cập nhật thông tin thành công" });
            }
            return Json(new { success = false, message = "Cập nhật thông tin thất bại, vui lòng liên hệ IT" });
        }

        public async Task<IActionResult> IsActivedCategory(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetCategory(id).GetAwaiter().GetResult();
            if (item != null)
            {
                item.IsActived = !item.IsActived;
                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.ChangeActiveCategory(id, item.IsActived);
                if (isSuccess)
                {
                    return Json(new { success = true, item.IsActived, message = "Đổi trạng thái thành công" });
                }
            }
            return Json(new { success = false, message = "Đổi trạng thái thất bại, vui lòng liên hệ IT" });
        }

        public async Task<IActionResult> IsActivedHeaderMenu(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetCategory(id).GetAwaiter().GetResult();
            if (item != null)
            {
                item.IsHeaderMenu = !item.IsHeaderMenu;
                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.ChangeActiveCategoryHeaderMenu(id, item.IsHeaderMenu);
                if (isSuccess)
                {
                    return Json(new { success = true, item.IsHeaderMenu, message = "Đổi trạng thái thành công" });
                }
            }
            return Json(new { success = false, message = "Đổi trạng thái thất bại, vui lòng liên hệ IT" });
        }

        #endregion


        #region Post Bài viết

        public IActionResult Post()
        {
            var items = _unitOfWork_Repository.LandingPage_Rep.GetPost().GetAwaiter().GetResult();
            return View(items);
        }


        public IActionResult CreatePost()
        {
            return View();
        }

        public async Task<IActionResult> SaveCreatePost(Post request)
        {
            if (request.CategoryId == 0 || request.CategoryId == null)
            {
                return Json(new { success = true, message = "Không thể thêm do không thuộc danh mục nào" });
            }
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            request.Alias = Filter.FilterChar(request.Name);
            request.CreatedBy = acc.HoTen;
            request.CreatedDate = DateTime.Now;

            bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveCreatePost(request);
            if (isSuccess)
            {
                return Json(new { success = true, message = "Thêm mới thông tin thành công" });
            }
            return Json(new { success = false, message = "Thêm mới thông tin thất bại, vui lòng liên hệ IT" });
        }

        public IActionResult EditPost(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetPost(id).GetAwaiter().GetResult();
            return View(item);
        }

        public async Task<IActionResult> SaveEditPost(Post request)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            request.Alias = Filter.FilterChar(request.Name);
            request.ModifiedBy = acc.MaNV;
            request.ModifiedDate = DateTime.Now;

            bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveEditPost(request);
            if (isSuccess)
            {
                return Json(new { success = true, message = "Cập nhật thông tin thành công" });
            }
            return Json(new { success = false, message = "Cập nhật thông tin thất bại, vui lòng liên hệ IT" });
        }

        public async Task<IActionResult> IsActivedPost(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetPost(id).GetAwaiter().GetResult();
            if (item != null)
            {
                item.IsActived = !item.IsActived;
                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.ChangeActivePost(id, item.IsActived);
                if (isSuccess)
                {
                    return Json(new { success = true, item.IsActived, message = "Đổi trạng thái thành công" });
                }
            }
            return Json(new { success = false, message = "Đổi trạng thái thất bại, vui lòng liên hệ IT" });
        }

        #endregion


        #region Tin tức

        public IActionResult News(int page = 1, int pageSize = 10)
        {
            News news = new News();
            var paginationVM = GetListNewsPagination(news, DateTime.MinValue, DateTime.MinValue, page, pageSize);
            return View(paginationVM);
        }

        private PaginationBase<News> GetListNewsPagination(News request, DateTime fromDate, DateTime toDate, int page, int pageSize)
        {
            if (toDate != DateTime.MinValue)
            {
                toDate = toDate.AddHours(23).AddMinutes(59).AddSeconds(59); // cuối ngày
            }
            // Xác định ngày hợp lệ của SQL Server
            DateTime sqlMinDate = new DateTime(1753, 1, 1);

            // Chuyển đổi từDate và toDate nếu chúng là DateTime.MinValue
            fromDate = fromDate == DateTime.MinValue ? sqlMinDate : fromDate;
            toDate = toDate == DateTime.MinValue ? DateTime.MaxValue : toDate;

            var (listOrders, totalRecord) = _unitOfWork_Repository.LandingPage_Rep.GetListNews(request, fromDate, toDate, page, pageSize);

            PaginationBase<News> paginationVM = new PaginationBase<News>()
            {
                ListProduct = listOrders,
                TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize),
                CurrentPage = page,
                PageSize = pageSize,
                TotalQuantityOfProduct = totalRecord
            };
            return paginationVM;
        }

        public IActionResult GetAllNewsByFilter(News request, DateTime fromDate, DateTime toDate, int page, int pageSize)
        {
            var listlPagination = GetListNewsPagination(request, fromDate, toDate, page, pageSize);
            return PartialView("_TableNewsPaginationPatial", listlPagination);
        }


        public IActionResult CreateNews()
        {
            return View();
        }

        public async Task<IActionResult> SaveCreateNews(News request, IFormFile ImageValue)
        {
            if (ImageValue != null)
            {
                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }
                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                request.Alias = Filter.FilterChar(request.Name);
                request.CreatedBy = acc.HoTen;
                request.CreatedDate = DateTime.Now;
                request.Image = Common.UploadFileToSFTP(ImageValue);

                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveCreateNews(request);
                if (isSuccess)
                {
                    return Json(new { success = true, message = "Thêm mới thông tin thành công" });
                }
                return Json(new { success = false, message = "Thêm mới thông tin thất bại, vui lòng liên hệ IT" });
            }
            else
            {
                return Json(new { success = false, message = "Vui lòng chọn hình ảnh" });
            }
        }

        public IActionResult EditNews(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetNews(id).GetAwaiter().GetResult();
            return View(item);
        }

        public async Task<IActionResult> SaveEditNews(News request, IFormFile ImageValue)
        {
            string oldImage = string.Empty;
            if (ImageValue != null)
            {
                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }
                // Xử lý cập nhật hình ảnh
                oldImage = request.Image;
                request.Image = Common.UploadFileToSFTP(ImageValue);
            }
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            request.Alias = Filter.FilterChar(request.Name);
            request.ModifiedBy = acc.MaNV;
            request.ModifiedDate = DateTime.Now;
          
            bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveEditNews(request);
            if (isSuccess)
            {
                if (ImageValue != null)
                {
                    _ = Task.Run(() => Common.DeleteFileFromSFTP(oldImage));
                }
                return Json(new { success = true, message = "Cập nhật thông tin thành công" });
            }
            return Json(new { success = false, message = "Cập nhật thông tin thất bại, vui lòng liên hệ IT" });
        }

        public async Task<IActionResult> IsActivedNews(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetNews(id).GetAwaiter().GetResult();
            if (item != null)
            {
                item.IsActived = !item.IsActived;
                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.ChangeActiveNews(id, item.IsActived);
                if (isSuccess)
                {
                    return Json(new { success = true, item.IsActived, message = "Đổi trạng thái thành công" });
                }
            }
            return Json(new { success = false, message = "Đổi trạng thái thất bại, vui lòng liên hệ IT" });
        }

        #endregion

        #region Subscribe
        public IActionResult Subscribe(int page = 1, int pageSize = 10)
        {
            Subscribe subscribe = new Subscribe();
            var paginationVM = GetListSubscribePagination(subscribe, page, pageSize);
            return View(paginationVM);
        }

        private PaginationBase<Subscribe> GetListSubscribePagination(Subscribe request, int page, int pageSize)
        {

            var (listItem, totalRecord) = _unitOfWork_Repository.LandingPage_Rep.GetListSubscribe(request, page, pageSize);

            PaginationBase<Subscribe> paginationVM = new PaginationBase<Subscribe>()
            {
                ListProduct = listItem,
                TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize),
                CurrentPage = page,
                PageSize = pageSize,
                TotalQuantityOfProduct = totalRecord
            };
            return paginationVM;
        }

        public IActionResult GetAllSubscribeByFilter(Subscribe request, int page, int pageSize)
        {
            var listPagination = GetListSubscribePagination(request, page, pageSize);
            return PartialView("_TableSubscribePaginationPatial", listPagination);
        }

        public IActionResult CreateNotification()
        {
            return View();
        }

        public IActionResult SendEmailToSubcribers(Notification request)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            request.CreatedBy = acc.HoTen;
            request.CreatedDate = DateTime.Now;
            _ = Task.Run(() => _unitOfWork_Repository.LandingPage_Rep.SaveNotificationContent(request));
            _ = Task.Run(async () => await _unitOfWork_Repository.LandingPage_Rep.SendMailToSubscribers(request));

            return Json(new { success = true, message = "Gửi thành công" });
        }


        #endregion

        #region Customer Request

        public IActionResult CustomerRequest(int page = 1, int pageSize = 10)
        {
            CustomerRequest model = new CustomerRequest();
            var paginationVM = GetListCustomerRequestPagination(model, DateTime.MinValue, DateTime.MinValue, page, pageSize, 999);
            return View(paginationVM);
        }

        private PaginationBase<CustomerRequest> GetListCustomerRequestPagination(CustomerRequest request, DateTime fromDate, DateTime toDate, int page, int pageSize, int IsResolvedValue)
        {

            if (toDate != DateTime.MinValue)
            {
                toDate = toDate.AddHours(23).AddMinutes(59).AddSeconds(59); // cuối ngày
            }
            // Xác định ngày hợp lệ của SQL Server
            DateTime sqlMinDate = new DateTime(1753, 1, 1);

            // Chuyển đổi từDate và toDate nếu chúng là DateTime.MinValue
            fromDate = fromDate == DateTime.MinValue ? sqlMinDate : fromDate;
            toDate = toDate == DateTime.MinValue ? DateTime.MaxValue : toDate;

            var (listItem, totalRecord) = _unitOfWork_Repository.LandingPage_Rep.GetListCustomerRequest(request, fromDate, toDate, page, pageSize, IsResolvedValue);

            PaginationBase<CustomerRequest> paginationVM = new PaginationBase<CustomerRequest>()
            {
                ListProduct = listItem,
                TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize),
                CurrentPage = page,
                PageSize = pageSize,
                TotalQuantityOfProduct = totalRecord
            };
            return paginationVM;
        }

        public IActionResult GetAllCustomerRequestByFilter(CustomerRequest request, DateTime fromDate, DateTime toDate, int page, int pageSize, int IsResolvedValue)
        {
            var listPagination = GetListCustomerRequestPagination(request, fromDate, toDate, page, pageSize, IsResolvedValue);
            return PartialView("_TableCustomerRequestPaginationPatial", listPagination);
        }

        public IActionResult DetailCustomerRequest(int Id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetCustomerRequest(Id).GetAwaiter().GetResult();
            return View(item);
        }

        public IActionResult SendEmailCustomerResponse(CustomerResponse request, string customerEmail)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            request.CreatedBy = acc.HoTen;
            request.CreatedDate = DateTime.Now;

            _ = Task.Run(() => _unitOfWork_Repository.LandingPage_Rep.SaveCustomerResponse(request));
            _ = Task.Run(() => _unitOfWork_Repository.LandingPage_Rep.SendMailCustomerResponse(request, customerEmail));

            return Json(new { success = true, message = "Gửi thành công" });
        }
        #endregion


        #region CompanyHistory
        public IActionResult CompanyHistory()
        {
            var items = _unitOfWork_Repository.LandingPage_Rep.GetCompanyHistory().GetAwaiter().GetResult();
            return View(items);
        }



        public IActionResult CreateCompanyHistory()
        {
            return View();
        }

        public async Task<IActionResult> SaveCreateCompanyHistory(CompanyHistory request, IFormFile ImageValue)
        {
            if (ImageValue != null)
            {
                var listCompanyHistory = _unitOfWork_Repository.LandingPage_Rep.GetCompanyHistory().GetAwaiter().GetResult();
                foreach (var item in listCompanyHistory)
                {
                    if (item.Year == request.Year)
                    {
                        return Json(new { success = false, message = $"Năm {request.Year} đã có nội dung, bạn có thực hiện chỉnh sửa" });
                    }
                }

                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }

                // Xử lý cập nhật hình ảnh
                string oldImage = request.Image;
                request.Image = Common.UploadFileToSFTP(ImageValue);

                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveCreateCompanyHistory(request);
                if (isSuccess)
                {
                    return Json(new { success = true, message = "Thêm mới thông tin thành công" });
                }
                return Json(new { success = false, message = "Thêm mới thông tin thất bại, vui lòng liên hệ IT" });
            }

            return Json(new { success = false, message = "Vui lòng chọn tệp hình ảnh" });
        }


        public IActionResult EditCompanyHistory(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetCompanyHistory(id).Result;
            return View(item);
        }

        public async Task<IActionResult> SaveEditCompanyHistory(CompanyHistory request, IFormFile ImageValue)
        {
            if (ImageValue != null)
            {
                bool isFileOverSize = IsFileOverSiveMB(ImageValue, 5).GetAwaiter().GetResult();
                if (isFileOverSize)
                {
                    return Json(new { success = false, message = "Kích thước tệp không được vượt quá 5MB" });
                }

                // Xử lý cập nhật hình ảnh
                string oldImage = request.Image;
                request.Image = Common.UploadFileToSFTP(ImageValue);

                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveEditCompanyHistory(request);
                if (isSuccess)
                {
                    _ = Task.Run(() => Common.DeleteFileFromSFTP(oldImage));
                    return Json(new { success = true, message = "Cập nhật thông tin thành công" });
                }
                return Json(new { success = false, message = "Cập nhật thông tin thất bại, vui lòng liên hệ IT" });
            }

            return Json(new { success = false, message = "Vui lòng chọn tệp hình ảnh" });
        }

        #endregion

        #region Reward

        public IActionResult Reward()
        {
            var items = _unitOfWork_Repository.LandingPage_Rep.GetReward().GetAwaiter().GetResult();
            return View(items);
        }



        public IActionResult CreateReward()
        {
            return View();
        }

        public async Task<IActionResult> SaveCreateReward(string Year, string[] RewardName, string[] RewardFrom)
        {
            var list = _unitOfWork_Repository.LandingPage_Rep.GetReward().GetAwaiter().GetResult();
            foreach (var item in list)
            {
                if (item.Year.Trim() == Year.Trim())
                {
                    return Json(new { success = false, message = $"Năm {Year.Trim()} đã có nội dung, bạn có thực hiện chỉnh sửa" });
                }
            }

            var request = new RewardYear
            {
                Year = Year.Trim(),
                Rewards = new List<Reward>()
            };

            if (RewardFrom.Length != RewardName.Length)
            {
                return Json(new { success = false, message = $"Vui lòng nhập đủ trường dữ liệu" });
            }

            for (int i = 0; i < RewardName.Length; i++)
            {
                var reward = new Reward
                {
                    RewardName = RewardName[i].Trim().ToUpper(),
                    RewardFrom = RewardFrom[i].Trim().ToUpper()
                };
                request.Rewards.Add(reward);
            }

            bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveCreateReward(request);
            if (isSuccess)
            {
                return Json(new { success = true, message = "Thêm mới thông tin thành công" });
            }
            return Json(new { success = false, message = "Thêm mới thông tin thất bại, vui lòng liên hệ IT" });
        }


        public IActionResult EditReward(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetReward(id).Result;
            return View(item);
        }

        public async Task<IActionResult> SaveEditReward(int yearId, string[] RewardName, string[] RewardFrom)
        {
            var request = new RewardYear
            {
                Rewards = new List<Reward>()
            };

            if (RewardFrom.Length != RewardName.Length)
            {
                return Json(new { success = false, message = $"Vui lòng nhập đủ trường dữ liệu" });
            }

            for (int i = 0; i < RewardName.Length; i++)
            {
                var reward = new Reward
                {
                    RewardName = RewardName[i].Trim().ToUpper(),
                    RewardFrom = RewardFrom[i].Trim().ToUpper()
                };
                request.Rewards.Add(reward);
            }

            bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveEditReward(request.Rewards, yearId);
            if (isSuccess)
            {
                return Json(new { success = true, message = "Cập nhật thông tin thành công" });
            }
            return Json(new { success = false, message = "Cập nhật thông tin thất bại, vui lòng liên hệ IT" });

        }
        #endregion


        #region Job 



        public IActionResult Job(int page = 1, int pageSize = 10)
        {
            Job item = new Job();
            var paginationVM = GetListJobPagination(item, DateTime.MinValue, DateTime.MinValue, page, pageSize);
            return View(paginationVM);
        }

        private PaginationBase<Job> GetListJobPagination(Job request, DateTime fromDate, DateTime toDate, int page, int pageSize)
        {
            if (toDate != DateTime.MinValue)
            {
                toDate = toDate.AddHours(23).AddMinutes(59).AddSeconds(59); // cuối ngày
            }
            // Xác định ngày hợp lệ của SQL Server
            DateTime sqlMinDate = new DateTime(1753, 1, 1);

            // Chuyển đổi từDate và toDate nếu chúng là DateTime.MinValue
            fromDate = fromDate == DateTime.MinValue ? sqlMinDate : fromDate;
            toDate = toDate == DateTime.MinValue ? DateTime.MaxValue : toDate;

            var (list, totalRecord) = _unitOfWork_Repository.LandingPage_Rep.GetListJob(request, fromDate, toDate, page, pageSize);

            PaginationBase<Job> paginationVM = new PaginationBase<Job>()
            {
                ListProduct = list,
                TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize),
                CurrentPage = page,
                PageSize = pageSize,
                TotalQuantityOfProduct = totalRecord
            };
            return paginationVM;
        }

        public IActionResult GetAllJobByFilter(Job request, DateTime fromDate, DateTime toDate, int page, int pageSize)
        {
            var listlPagination = GetListJobPagination(request, fromDate, toDate, page, pageSize);
            return PartialView("_TableJobPaginationPatial", listlPagination);
        }

        public IActionResult CreateJob()
        {
            return View();
        }

        public async Task<IActionResult> SaveCreateJob(Job request)
        {
            request.CreatedDate = DateTime.Now;
            if (request.OpenDate > request.CloseDate)
            {
                return Json(new { success = false, message = "Ngày mở và ngày đóng job không hợp lệ" });
            }

            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Department) || string.IsNullOrEmpty(request.Benefit) || string.IsNullOrEmpty(request.Description) || string.IsNullOrEmpty(request.Requirement))
            {
                return Json(new { success = false, message = "Vui lòng nhập đủ dữ liệu" });
            }

            request.CloseDate = request.CloseDate.AddHours(23).AddMinutes(59).AddSeconds(59);
            bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveCreateJob(request);
            if (isSuccess)
            {
                return Json(new { success = true, message = "Thêm mới thông tin thành công" });
            }
            return Json(new { success = false, message = "Thêm mới thông tin thất bại, vui lòng liên hệ IT" });
        }


        public IActionResult EditJob(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetJob(id).Result;
            return View(item);
        }

        public async Task<IActionResult> SaveEditJob(Job request)
        {
            if (request.OpenDate > request.CloseDate)
            {
                return Json(new { success = false, message = "Ngày mở và ngày đóng job không hợp lệ" });
            }

            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Department) || string.IsNullOrEmpty(request.Benefit) || string.IsNullOrEmpty(request.Description) || string.IsNullOrEmpty(request.Requirement))
            {
                return Json(new { success = false, message = "Vui lòng nhập đủ dữ liệu" });
            }

            request.CloseDate = request.CloseDate.AddHours(23).AddMinutes(59).AddSeconds(59);
            bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.SaveEditJob(request);
            if (isSuccess)
            {
                return Json(new { success = true, message = "Cập nhật thông tin thành công" });
            }
            return Json(new { success = false, message = "Cập nhật thông tin thất bại, vui lòng liên hệ IT" });
        }

        public async Task<IActionResult> IsActivedJob(int id)
        {
            var item = _unitOfWork_Repository.LandingPage_Rep.GetJob(id).GetAwaiter().GetResult();
            if (item != null)
            {
                item.IsActived = !item.IsActived;
                bool isSuccess = await _unitOfWork_Repository.LandingPage_Rep.ChangeActiveJob(id, item.IsActived);
                if (isSuccess)
                {
                    return Json(new { success = true, item.IsActived, message = "Đổi trạng thái thành công" });
                }
            }
            return Json(new { success = false, message = "Đổi trạng thái thất bại, vui lòng liên hệ IT" });
        }

        public IActionResult ListApplication(int Id)
        {
            var items = _unitOfWork_Repository.LandingPage_Rep.GetJob(Id).GetAwaiter().GetResult();
            return View(items);
        }

        #endregion




    }
}
