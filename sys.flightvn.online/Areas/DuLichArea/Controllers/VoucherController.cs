using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Manager.Model.Models.ViewModel.VoucherVM;
using Manager.Model.Models.PaginationBase;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace Manager_EV.Areas.DuLichArea.Controllers
{
    [Area(AreaNameConst.AREA_DuLich)]
    public class VoucherController : Controller
    {
        private readonly IUnitOfWork_Repository _unitOfWork_Rep;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public VoucherController(IConfiguration configuration, IUnitOfWork_Repository unitOfWork_Rep)
        {
            _configuration = configuration;
            _unitOfWork_Rep = unitOfWork_Rep;
            //Voucher_Rep = new VoucherRepository(_configuration);
        }

        #region CẤU HÌNH VOUCHER 

        public IActionResult Voucher()
        {
            List<VoucherModel> result = _unitOfWork_Rep.Voucher_Rep.Voucher();
            return View(result);
        }

        public IActionResult CreateVoucher()
        {
            // Lấy danh sách loại voucher
            //ViewData["ListVoucherType"] = Voucher_Rep.GetAllVoucherType();
            // Lấy danh sách dịch vụ
            //ViewData["listServiceType"] = Voucher_Rep.GetAllServiceType();

            return View();
        }

        public IActionResult EditVoucher(int ID)
        {
            VoucherModel result = _unitOfWork_Rep.Voucher_Rep.GetVoucherById(ID);
            // Lấy danh sách loại voucher
            //ViewData["ListVoucherType"] = Voucher_Rep.GetAllVoucherType();
            // Lấy danh sách dịch vụ
            ViewData["listServiceType"] = _unitOfWork_Rep.Voucher_Rep.GetAllServiceType();
            // Lấy Service Id 
            ViewData["ServiceId"] = _unitOfWork_Rep.Voucher_Rep.GetVoucherServiceTypeById(ID).ServiceId;
            return View(result);
        }

        //[HttpPost]
        //public IActionResult CreateVoucher(VoucherModel voucherModel, VoucherServiceType voucherServiceType, List<IFormFile> imageFiles, List<bool> mainImages)
        //{
        //    List<ImageVoucher> ListImg = new List<ImageVoucher>();

        //    voucherModel.CreateDate = DateTime.Now;
        //    voucherModel.IsActive = false;
        //    for (int i = 0; i < mainImages.Count; i++)
        //    {
        //        ImageVoucher img = new ImageVoucher();
        //        img.isMainImage = mainImages[i];
        //        img.imageUrl = UploadImg(imageFiles[i]);
        //        ListImg.Add(img);
        //    }
        //    voucherModel.listImage = ListImg;
        //    AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
        //    string messageSuccess = Voucher_Rep.SaveCreateVoucher(voucherModel, voucherServiceType, acc.MaNV).GetAwaiter().GetResult();
        //    if (messageSuccess == StaticDetailVoucher.SUCCESS)
        //    {
        //        TempData["thongbao"] = "Bạn đã lưu voucher thành công";
        //        return Json(new { success = true });
        //    }
        //    else
        //    {
        //        TempData["thongbao"] = "Bạn đã lưu voucher không thành công";
        //        return Json(new { success = false });
        //    }
        //}

        [HttpPost]
        public IActionResult UpsertVoucher(VoucherModel voucherModel, VoucherServiceType voucherServiceType, List<IFormFile> imageFiles, string mainImageName, List<int> CurrentImagesId = null)
        {
            JsonResult jsonResultFromModelState = HandleModelStateUpsert(ModelState, voucherModel);
            if (jsonResultFromModelState != null)
            {
                return Json(jsonResultFromModelState.Value);
            }
            // Xử lý lỗi hình ảnh
            var (listImageAfterhandle, message) = HandleImagesUpsert(voucherModel, imageFiles, mainImageName, CurrentImagesId);
            if (message != StaticDetailVoucher.SUCCESS)
            {
                return Json(new { success = false, message });
            }

            string messageResult = string.Empty;
            voucherModel.IsActive = false;
            voucherModel.listImage = listImageAfterhandle;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (voucherModel.Id == 0)
            {
                voucherModel.CreateDate = DateTime.Now;
                messageResult = _unitOfWork_Rep.Voucher_Rep.SaveCreateVoucher(voucherModel, voucherServiceType, acc.HoTen).GetAwaiter().GetResult();
            }
            else
            {
                messageResult = _unitOfWork_Rep.Voucher_Rep.SaveEditVoucher(voucherModel, voucherServiceType, CurrentImagesId, mainImageName).GetAwaiter().GetResult();
            }
            if (messageResult == StaticDetailVoucher.SUCCESS)
            {
                return Json(new { success = true, message = "Bạn đã lưu voucher thành công" });
            }
            return Json(new { success = false, message = messageResult });
        }

        private JsonResult HandleModelStateUpsert(ModelStateDictionary modelState, VoucherModel voucherModel)
        {
            string error = string.Empty;
            // Kiểm tra phải lỗi null value từ ngày tháng không
            if (!voucherModel.ExpiryDateFrom.HasValue || !voucherModel.ExpiryDateTo.HasValue
                || string.IsNullOrEmpty(voucherModel.ExpiryDateFrom.ToString()) || string.IsNullOrEmpty(voucherModel.ExpiryDateTo.ToString()))
            {
                return Json(new { success = false, message = "Vui lòng không bỏ trống hạn sử dụng" });
            }
            // Kiểm tra lỗi giá giảm > giá bán
            if (voucherModel.Price < voucherModel.DiscountPrice)
            {
                return Json(new { success = false, message = "Giá giảm hiện đang lớn hơn giá bán" });
            }
            if (!modelState.IsValid)
            {
                // Lấy các thông báo lỗi từ ModelState
                error = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .Select(ms => new
                    {
                        Field = ms.Key,
                        Message = ms.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                    }).FirstOrDefault().Message;
                return Json(new { success = false, message = error });
            }
            return null;
        }

        private (List<ImageVoucher>, string) HandleImagesUpsert(VoucherModel voucherModel, List<IFormFile> imageFiles, string mainImageName, List<int> CurrentImagesId)
        {
            string message = StaticDetailVoucher.SUCCESS;
            List<ImageVoucher> imageVouchers = new List<ImageVoucher>();

            // Create
            if (voucherModel.Id == 0 && string.IsNullOrEmpty(mainImageName))
            {
                return (imageVouchers, "Bạn chưa chọn hình đại diện");
            }

            if (imageFiles != null && imageFiles.Any())
            {
                foreach (var file in imageFiles)
                {
                    ImageVoucher img = new ImageVoucher
                    {
                        isMainImage = file.FileName == mainImageName,
                    };
                    imageVouchers.Add(img);
                }
                bool isMainImageExist = imageVouchers.Any(x => x.isMainImage);

                if (!isMainImageExist)
                {
                    if (voucherModel.Id == 0)
                    {
                        message = "Không xác định được hình đại diện, vui lòng thử lại";
                    }
                    else
                    {
                        // Hình trong edit có main chưa
                        isMainImageExist = CurrentImagesId.Any(id => id.ToString() == mainImageName);
                        message = isMainImageExist ? StaticDetailVoucher.SUCCESS : "Không xác định được hình đại diện, vui lòng thử lại";
                    }
                }
            }
            // Trường hợp k có hình nào được chọn
            else
            {
                if (voucherModel.Id == 0)
                {
                    message = "Bạn chưa chọn file hình";
                }
                else
                {
                    // Check if current images have the main image
                    bool isMainImageExist = CurrentImagesId.Any(id => id.ToString() == mainImageName);
                    message = isMainImageExist ? StaticDetailVoucher.SUCCESS : "Không xác định được hình đại diện, vui lòng thử lại";
                }
            }

            return (imageVouchers, message);
        }


        //[HttpPost]
        //public IActionResult EditVoucher(VoucherModel voucherModel, VoucherServiceType voucherServiceType, List<IFormFile> imageFiles, List<bool> mainImages, List<bool> mainImgs, List<string> imagesURL)
        //{
        //    List<ImageVoucher> ListImg = new List<ImageVoucher>();

        //    if (imageFiles.Count != 0)
        //    {
        //        for (int i = 0; i < mainImages.Count; i++)
        //        {
        //            ImageVoucher img = new ImageVoucher();
        //            img.isMainImage = mainImages[i];
        //            img.imageUrl = UploadImg(imageFiles[i]);
        //            ListImg.Add(img);
        //        }
        //    }
        //    if (imagesURL.Count != 0)
        //    {
        //        for (int i = 0; i < imagesURL.Count; i++)
        //        {
        //            ImageVoucher img = new ImageVoucher();
        //            img.isMainImage = mainImgs[i];
        //            img.imageUrl = imagesURL[i];
        //            ListImg.Add(img);
        //        }
        //    }
        //    voucherModel.listImage = ListImg;
        //    // Save new edit
        //    bool ret = Voucher_Rep.SaveEditVoucher(voucherModel, voucherServiceType).GetAwaiter().GetResult();
        //    if (ret == true)
        //    {
        //        TempData["thongbao"] = "Bạn đã chỉnh sửa voucher thành công";
        //        return Json(new { success = true });
        //    }
        //    else
        //    {
        //        TempData["thongbao"] = "Bạn đã chỉnh sửa voucher không thành công";
        //        return Json(new { success = false });
        //    }
        //}

        [HttpPost]
        public IActionResult DeleteImage(int ID, int voucherId)
        {
            string isResultMessage = _unitOfWork_Rep.Voucher_Rep.DeleteImg(ID, voucherId);
            if (isResultMessage == StaticDetailVoucher.SUCCESS)
            {
                return Json(new { success = true, message = "Xóa thành công" });
            }
            else if (isResultMessage == StaticDetailVoucher.FAIL)
            {
                return Json(new { success = false, message = "Danh sách hình không thể trống" });
            }
            else
            {
                return Json(new { success = false, message = isResultMessage });
            }
        }
        #endregion

        #region BOOKING VOUCHER
        public IActionResult BookingVoucher(int page = 1, int pageSize = 50)
        {
            var paginationVM = GetOrderHeaderPagination(0, page, pageSize);
            ViewData["listStatus"] = _unitOfWork_Rep.Voucher_Rep.GetAllStatus();
            return View(paginationVM);
        }
        public IActionResult GetAllOrderByFilter(int statusId, int page = 1, int pageSize = 50, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var orderHeaderPagination = GetOrderHeaderPagination(statusId, page, pageSize, dateFrom, dateTo);
            return PartialView("_TableVoucherPaginationPartial", orderHeaderPagination);
        }

        private PaginationBase<OrderHeaderVoucher> GetOrderHeaderPagination(int statusId, int page, int pageSize, DateTime? dateForm = null, DateTime? dateTo = null)
        {
            var offset = (page - 1) * pageSize;
            var (listOrders, totalRecord) = _unitOfWork_Rep.Voucher_Rep.GetAllOrderHeaderVoucher(pageSize, offset, statusId, dateForm, dateTo);

            PaginationBase<OrderHeaderVoucher> paginationVM = new PaginationBase<OrderHeaderVoucher>()
            {
                ListProduct = listOrders,
                TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize),
                CurrentPage = page,
                PageSize = pageSize,
                TotalQuantityOfProduct = totalRecord
            };
            return paginationVM;
        }
        public IActionResult DetailVoucherBooking(int ID)
        {
            OrderVM = new OrderVM()
            {
                OrderHeaderVoucher = _unitOfWork_Rep.Voucher_Rep.GetOrderHeaderVoucherById(ID, _unitOfWork_Rep.Location_Rep),
                OrderVouchers = _unitOfWork_Rep.Voucher_Rep.GetAllOrderVoucherById(ID),
                ListStatus = _unitOfWork_Rep.Voucher_Rep.GetAllStatus()
            };
            return View(OrderVM);
        }
        [HttpPost]
        public IActionResult UpdateStatusBooking(OrderHeaderVoucher orderHeaderVoucher, int oldStatusId)
        {
            if (orderHeaderVoucher.OrderStatusId == oldStatusId)
            {
                return Json(new { success = false, message = "Trạng thái không thay đổi." });
            }
            if (oldStatusId == StaticDetailVoucher.OrderStatusId_CANCELLED)
            {
                return Json(new { success = false, message = "Booking đã bị hủy." });
            }
            if (oldStatusId == StaticDetailVoucher.OrderStatusId_COMPLETED)
            {
                return Json(new { success = false, message = "Booking đã hoàn thành." });
            }
            // Lấy ra Account hiện tại
            AccountModel currentAccount = AccountManager.GetAccountCurrent(HttpContext);
            // Cập nhật ngày chỉnh sửa
            orderHeaderVoucher.EditDate = DateTime.Now;
            string result = _unitOfWork_Rep.Voucher_Rep.UpdateStatusBooking(orderHeaderVoucher, currentAccount, _unitOfWork_Rep.Location_Rep);
            if (result == StaticDetailVoucher.SUCCESS)
            {
                return Json(new { success = true, message = "Cập nhật thành công." });
            }
            else if (result != StaticDetailVoucher.FAIL && result != StaticDetailVoucher.SUCCESS)
            {
                return Json(new { success = false, message = result });
            }
            else
            {
                return Json(new { success = false, message = "Cập nhật thất bại." });
            }
        }

        // Hủy Booking
        public IActionResult CancelBooking(OrderHeaderVoucher orderHeaderVoucher, int oldStatusId)
        {
            if (oldStatusId == StaticDetailVoucher.OrderStatusId_COMPLETED)
            {
                return Json(new { success = false, message = "Booking đã hoàn thành." });
            }
            if (oldStatusId == StaticDetailVoucher.OrderStatusId_CANCELLED)
            {
                return Json(new { success = false, message = "Booking đã bị hủy." });
            }
            // Lấy ra Account hiện tại
            AccountModel currentAccount = AccountManager.GetAccountCurrent(HttpContext);
            string isCancelSuccess = _unitOfWork_Rep.Voucher_Rep.CancelBooking(orderHeaderVoucher, currentAccount, _unitOfWork_Rep.Location_Rep);
            if (isCancelSuccess == StaticDetailVoucher.SUCCESS)
            {
                return Json(new { success = true, message = "Hủy Booking thành công." });
            }
            else if (isCancelSuccess != StaticDetailVoucher.FAIL && isCancelSuccess != StaticDetailVoucher.SUCCESS)
            {
                return Json(new { success = false, message = isCancelSuccess });
            }
            return Json(new { success = false, message = "Hủy Booking thất bại, vui lòng liện hệ tới Flight VN để xử lý." });
        }
        public IActionResult LoadOrderStatusInDetailBooking(int orderHeaderId)
        {
            return ViewComponent("DetailBookingVoucher", new { orderHeaderId });
        }
        public IActionResult LoadOrderStatusInBooking(int orderHeaderId)
        {
            return ViewComponent("BookingVoucher", new { orderHeaderId });
        }
        #endregion

       

        [HttpPost]
        public IActionResult ChangeActiveVoucher(int ID, int Active)
        {
            bool result = _unitOfWork_Rep.Voucher_Rep.ChangeActiveVoucher(ID, Active);
            return Json(result);
        }

        [HttpPost]
        public IActionResult DeleteVoucher(int id)
        {
            string isMessageSuccess = _unitOfWork_Rep.Voucher_Rep.DeleteVoucher(id);
            if (isMessageSuccess == StaticDetailVoucher.SUCCESS)
            {
                return Json(new { success = true, message = "Xóa thành công" });
            }
            else if (isMessageSuccess == StaticDetailVoucher.FAIL)
            {
                return Json(new { success = false, message = "Xóa thất bại." });

            }
            else
            {
                return Json(new { success = false, message = isMessageSuccess });

            }
        }

        [HttpGet]
        public IActionResult GetServicesByType(string type)
        {
            switch (type)
            {
                case "TOURLOCATION":
                    List<TourLocation> listTourLocation = _unitOfWork_Rep.TourLocation_Rep.GetListTourLocation();
                    if (listTourLocation.Count < 0)
                    {
                        return Json(new { success = false, message = "Hiện chưa có điểm tham quan nào." });

                    }
                    return Json(new { success = true, data = listTourLocation });
            }

            return Json(new { success = false, message = "Thao tác thất bại." });

        }
    }
}
