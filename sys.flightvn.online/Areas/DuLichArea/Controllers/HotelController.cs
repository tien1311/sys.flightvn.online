using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading;
using Manager.Model.Models.Hotel;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Manager.Common.Helpers;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Manager.Model.Models.PaginationBase;
using Manager.Model.Models.Interface.IDeleteModel;

namespace Manager_EV.Areas.DuLichArea.Controllers
{
    [Area(AreaNameConst.AREA_DuLich)]

    public class HotelController : Controller
    {
        public IConfiguration _configuration;
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;
        private static Stack<DeletedItemModel<ProductsModel>> deletedItemsStack = new Stack<DeletedItemModel<ProductsModel>>();
        private static Timer deletionTimer;


        public HotelController(IConfiguration configuration, IUnitOfWork_Repository unitOfWork_Repository)
        {
            _configuration = configuration;
            _unitOfWork_Repository = unitOfWork_Repository;
            InitializeDeletionTimer();
        }
        private void InitializeDeletionTimer()
        {
            deletionTimer = new Timer(CheckAndRemoveExpiredItems, null, 1000, 50000);
        }

        private static void CheckAndRemoveExpiredItems(object state)
        {
            var now = DateTime.Now;
            while (deletedItemsStack.Count > 0 && (now - deletedItemsStack.Peek().DeletionTime).TotalSeconds > 15)
            {
                var deletedItems = deletedItemsStack.Pop().DeletedItems;

                // Danh sách để lưu các URL của hình ảnh
                List<string> imageUrls = new List<string>();

                // Duyệt qua từng sản phẩm
                foreach (var product in deletedItems)
                {
                    // Kiểm tra nếu sản phẩm có danh sách hình ảnh
                    if (product.ListImages != null)
                    {
                        // Duyệt qua từng hình ảnh trong danh sách của sản phẩm
                        foreach (var image in product.ListImages)
                        {
                            // Thêm URL của hình ảnh vào danh sách
                            imageUrls.Add(image.ImageURL);
                        }
                    }
                }

                // Xóa các hình ảnh
                foreach (var imageUrl in imageUrls)
                {
                    bool isSuccess = Common.DeleteImg(imageUrl);
                }
            }
        }

        #region HOTEL
        public IActionResult Hotel(int page = 1, int pageSize = 10)
        {
            var paginationVM = GetListHotelPagination(string.Empty, string.Empty, string.Empty, 999, page, pageSize);
            return View(paginationVM);
        }

        private PaginationBase<ProductsModel> GetListHotelPagination(string hotelCodes, string hotelNames, string province, int isActived, int page, int pageSize)
        {
            var hotelCodeList = string.IsNullOrEmpty(hotelCodes)
            ? new List<string>()
                           : hotelCodes.Split(',')
                                      .Select(hotelCode => hotelCode.Trim())
                                      .Where(hotelCode => !string.IsNullOrEmpty(hotelCode))
                                      .ToList();

            var hotelNameList = string.IsNullOrEmpty(hotelNames)
            ? new List<string>()
                          : hotelNames.Split(',')
                                     .Select(hotelName => hotelName.Trim())
                                     .Where(hotelName => !string.IsNullOrEmpty(hotelName))
                                     .ToList();
            var (listOrders, totalRecord) = _unitOfWork_Repository.Hotel_Rep.GetListHotelV2(hotelCodeList, hotelNameList, province, isActived, page, pageSize);

            PaginationBase<ProductsModel> paginationVM = new PaginationBase<ProductsModel>()
            {
                ListProduct = listOrders,
                TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize),
                CurrentPage = page,
                PageSize = pageSize,
                TotalQuantityOfProduct = totalRecord
            };
            return paginationVM;
        }

        public IActionResult GetAllHotelByFilter(string hotelCodes, string hotelNames, string province, int isActived, int pageSize, int page)
        {
            var listHotelPagination = GetListHotelPagination(hotelCodes, hotelNames, province, isActived, page, pageSize);
            return PartialView("_TableHotelPaginationPatial", listHotelPagination);
        }

        [HttpPost]
        public IActionResult IsActived(string code)
        {
            var item = _unitOfWork_Repository.Hotel_Rep.GetHotelByCode(code);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                _unitOfWork_Repository.Hotel_Rep.ChangeActiveHotelByCode(code, item.IsActive);
                return Json(new { success = true, item.IsActive, message = "Đổi trạng thái thành công" });
            }
            return Json(new { success = false, message = "Đổi trạng thái thất bại, vui lòng liên hệ IT" });
        }

        public IActionResult CreateHotel()
        {
            var listProvince = _unitOfWork_Repository.Location_Rep.GetProvinces();
            return View(listProvince);
        }

        [HttpGet]
        public IActionResult GetListProvince()
        {
            var provinces = _unitOfWork_Repository.Location_Rep.GetProvinces();
            return Json(provinces);
        }

        [HttpGet]
        public IActionResult GetDistricts(string provinceCode)
        {
            var districts = _unitOfWork_Repository.Location_Rep.GetDistrictsByProvinceCode(provinceCode);
            return Json(districts);
        }

        [HttpGet]
        public IActionResult GetWards(string districtCode)
        {
            var wards = _unitOfWork_Repository.Location_Rep.GetWardsByDistrictCode(districtCode);
            return Json(wards);
        }


        public IActionResult EditHotel(int ID)
        {
            var model = _unitOfWork_Repository.Hotel_Rep.EditHotel(ID);
            var provinces = GetListProvince(); // Giả sử có phương thức này để lấy danh sách tỉnh thành
            var districts = GetDistricts(model.Province); // Lấy danh sách quận huyện theo tỉnh
            var wards = GetWards(model.District); // Lấy danh sách phường xã theo quận huyện

            ViewBag.ProvinceList = provinces;
            ViewBag.DistrictList = districts;
            ViewBag.WardList = wards;

            return View(model);
        }


        public IActionResult EditRoomTypeHotel(int ID)
        {
            List<ProductsType> result = _unitOfWork_Repository.Hotel_Rep.GetListRoomTypeHotel(ID);
            return View(result);
        }

        [HttpPost]
        public IActionResult SaveEditRoomTypeHotel(int[] productId, string[] roomTypeName, int[] roomTypeMaxPerson, double[] roomTypePrice, double[] roomTypeDiscountPrice, string[] roomTypeDescription)
        {
            var isSuccess = _unitOfWork_Repository.Hotel_Rep.EditRoomTypeHotel(productId, roomTypeName, roomTypeMaxPerson, roomTypePrice, roomTypeDiscountPrice, roomTypeDescription);
            if (isSuccess == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu sản phẩm thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu sản phẩm không thành công";

            List<ProductsModel> result = _unitOfWork_Repository.Hotel_Rep.Hotel();
            return View("Hotel", result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCreateHotel(
        string Name, string Email, string Phone, string ShortDescription, string LongDescription,
        string Address, string provinceName, string districtName, string wardName,
        List<bool> mainImages, List<IFormFile> imageFiles,
        string[] roomTypeName, int[] roomTypeMaxPerson, double[] roomTypePrice, double[] roomTypeDiscountPrice, string[] roomTypeDescription,
        int[] hotelServiceArray, int flag)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Phone) ||
                string.IsNullOrEmpty(ShortDescription) || string.IsNullOrEmpty(LongDescription) || string.IsNullOrEmpty(Address) ||
                string.IsNullOrEmpty(provinceName) || string.IsNullOrEmpty(districtName) || string.IsNullOrEmpty(wardName))
            {
                return Json(new { success = false, message = "Bạn phải điền đầy đủ thông tin." });
            }

            if (mainImages == null || !mainImages.Any(img => img))
            {
                return Json(new { success = false, message = "Bạn phải chọn hình đại diện." });
            }

            // Create and populate data models
            var data = new ProductsModel
            {
                Name = Name,
                Email = Email,
                Phone = Phone,
                ShortDescription = ShortDescription,
                LongDescription = LongDescription,
                Flag = flag,
                Address = Address,
                Province = provinceName,
                District = districtName,
                Ward = wardName,
                ListProductTypes = roomTypeName.Select((name, i) => new ProductsType
                {
                    Name = name,
                    MaxPerson = roomTypeMaxPerson[i],
                    Price = roomTypePrice[i],
                    DiscountPrice = roomTypeDiscountPrice.Length > i ? roomTypeDiscountPrice[i] : 0,
                    Description = roomTypeDescription[i]
                }).ToList(),
                ListImages = imageFiles.Select((file, i) => new Image
                {
                    MainImage = mainImages[i],
                    ImageURL = Common.UploadImg(file)
                }).ToList(),
                ListHotelServices = hotelServiceArray.Select(id => new HotelService { ServiceId = id }).ToList()
            };

            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = await _unitOfWork_Repository.Hotel_Rep.SaveCreateHotel(data, acc.MaNV);

            // Return JSON response
            return Json(new { success = ret, message = ret ? "Lưu thành công" : "Lưu không thành công" });
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _unitOfWork_Repository.Hotel_Rep.GetHotelByID(id);
            if (item != null)
            {
                var deletedItems = new List<ProductsModel>(); // Lưu trữ các mục đã xóa tạm thời
                var hotelItem = _unitOfWork_Repository.Hotel_Rep.GetAllContentHotel(Convert.ToInt32(id));

                if (hotelItem != null)
                {
                    bool success = _unitOfWork_Repository.Hotel_Rep.DeleteHotelByID(Convert.ToInt32(id));
                    if (success)
                    {
                        deletedItems.Add(hotelItem); // Lưu mục đã xóa
                    }
                }
                if (deletedItems.Any())
                {
                    deletedItemsStack.Push(new DeletedItemModel<ProductsModel>
                    {
                        DeletedItems = deletedItems,
                        DeletionTime = DateTime.Now
                    }); // Lưu các mục đã xóa vào Stack
                    return Json(new { success = true, message = "Xoá thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi xoá khách sạn" });
                }
            }
            return Json(new { success = false, message = "Không tìm thấy khách sạn này, vui lòng liên hệ IT để được hỗ trợ" });
        }

        [HttpPost]
        public IActionResult DeleteSelected(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    var deletedItems = new List<ProductsModel>(); // Lưu trữ các mục đã xóa tạm thời

                    foreach (var item in items)
                    {
                        var hotelItem = _unitOfWork_Repository.Hotel_Rep.GetAllContentHotel(Convert.ToInt32(item));
                        if (hotelItem != null)
                        {
                            var success = _unitOfWork_Repository.Hotel_Rep.DeleteHotelByID(Convert.ToInt32(item));
                            if (success)
                            {
                                deletedItems.Add(hotelItem); // Lưu mục đã xóa
                            }
                        }
                    }
                    if (deletedItems.Any())
                    {
                        deletedItemsStack.Push(new DeletedItemModel<ProductsModel>
                        {
                            DeletedItems = deletedItems,
                            DeletionTime = DateTime.Now
                        }); // Lưu các mục đã xóa vào Stack
                        return Json(new { success = true, message = "Xoá thành công" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Có lỗi xảy ra khi xoá khách sạn" });
                    }
                }
            }
            return Json(new { success = false, message = "Có lỗi xảy ra khi xoá khách sạn" });
        }

        [HttpPost]
        public async Task<IActionResult> UndoDelete()
        {
            if (deletedItemsStack.Count > 0)
            {
                var deletedItemModel = deletedItemsStack.Pop(); // Lấy các mục đã xóa cuối cùng ra khỏi Stack
                var items = deletedItemModel.DeletedItems;
                bool allSuccess = true;
                foreach (var item in items)
                {
                    var success = await _unitOfWork_Repository.Hotel_Rep.SaveUndoHotel(item, item.CreatedBy);
                    if (!success)
                    {
                        allSuccess = false;
                        break;
                    }
                }
                if (allSuccess)
                {
                    return Json(new { success = true, message = "Hoàn tác thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi hoàn tác" });
                }
            }
            return Json(new { success = false, message = "Có lỗi xảy ra khi hoàn tác" });
        }

        [HttpPost]
        public async Task<IActionResult> SaveEditHotel(int ID, string Name, string Email, int Flag, string Phone, string ShortDescription, string LongDescription, List<bool> mainImages, List<IFormFile> imageFiles, List<bool> mainImgs, List<string> imagesURL,
            string Address, string provinceName, string districtName, string wardName)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Phone) ||
                string.IsNullOrEmpty(ShortDescription) || string.IsNullOrEmpty(LongDescription) || string.IsNullOrEmpty(Address) ||
                string.IsNullOrEmpty(provinceName) || string.IsNullOrEmpty(districtName) || string.IsNullOrEmpty(wardName))
            {
                return Json(new { success = false, message = "Bạn phải điền đầy đủ thông tin." });
            }

            ProductsModel data = new ProductsModel();
            List<Image> ListImg = new List<Image>();
            data.ID = ID;
            data.Name = Name;
            data.Email = Email;
            data.Phone = Phone;
            data.Flag = Flag;
            data.ShortDescription = ShortDescription;
            data.LongDescription = LongDescription;
            data.Address = Address;
            data.Province = provinceName;
            data.District = districtName;
            data.Ward = wardName;

            for (int i = 0; i < imageFiles.Count; i++)
            {
                Image img = new Image();
                img.MainImage = mainImages[i];
                img.ImageURL = Common.UploadImg(imageFiles[i]);
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

            bool ret = await _unitOfWork_Repository.Hotel_Rep.SaveEditHotel(data);
            return Json(new { success = ret, message = ret ? "Lưu thành công" : "Lưu không thành công" });
        }

        public JsonResult DeleteImg(int id)
        {
            bool isSuccess = _unitOfWork_Repository.Hotel_Rep.DeleteImg(id);
            if (isSuccess)
            {
                return Json(new { success = true, message = "Xoá thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Xoá thất bại" });
            }
        }

        #endregion
        #region HOTEL SERVICE
        public IActionResult HotelService()
        {
            var item = _unitOfWork_Repository.Hotel_Rep.GetHotelServices();
            return View(item);
        }

        public IActionResult CreateHotelService()
        {
            return View();
        }

        public IActionResult EditProductHotelService(int id) // Còn hàm này là trang Khách sạn (dùng để chỉnh sửa dịch vụ của khách sạn đó
        {
            var item = _unitOfWork_Repository.Hotel_Rep.GetListProductHotelServicesById(id);
            var hotelItem = _unitOfWork_Repository.Hotel_Rep.GetAllContentHotel(id);
            var allServices = _unitOfWork_Repository.Hotel_Rep.GetHotelServices();

            ViewBag.HotelName = hotelItem.Name;
            ViewBag.HotelID = hotelItem.ID;
            ViewBag.HotelCode = hotelItem.Code;
            ViewBag.AllServices = allServices;
            ViewBag.SelectedServices = item.Select(s => s.Id).ToList();

            return View(item);
        }

        [HttpPost]
        public IActionResult SaveEditProductHotelService(int hotelId, string hotelCode, int[] hotelServiceArray) // Còn hàm này là trang Khách sạn (dùng để chỉnh sửa dịch vụ của khách sạn đó
        {
            List<HotelService> listHotelService = new List<HotelService>();
            for (int i = 0; i < hotelServiceArray.Length; i++)
            {
                HotelService hotelservice = new HotelService();
                hotelservice.ServiceId = hotelServiceArray[i];
                hotelservice.HotelId = hotelId;
                hotelservice.HotelCode = hotelCode;
                listHotelService.Add(hotelservice);
            }

            bool isSuccess = false;
            isSuccess = _unitOfWork_Repository.Hotel_Rep.UpdateProductHotelService(hotelId, listHotelService);
            if (isSuccess)
            {
                return Json(new { success = true, message = "Lưu thành công" });
            }
            return Json(new { success = false, message = "Không thể lưu" });
        }

        public IActionResult EditHotelService(int id) // Cái này là trang Cấu hình dịch vụ
        {
            var item = _unitOfWork_Repository.Hotel_Rep.GetHotelServicesById(id);
            return View(item);
        }

        [HttpPost]
        public IActionResult SaveCreateHotelService(string serviceName, IFormFile ImageInputValue)
        {
            if (string.IsNullOrEmpty(serviceName) || ImageInputValue == null)
            {
                return Json(new { success = false, message = "Vui lòng nhập đủ thông tin" });
            }

            bool isSuccess = false;
            string imgUrl = Common.UploadImg(ImageInputValue);
            isSuccess = _unitOfWork_Repository.Hotel_Rep.SaveHotelService(serviceName, imgUrl);
            if (isSuccess == true)
            {
                return Json(new { success = true, message = "Lưu thành công" });
            }
            return Json(new { success = false, message = "Lưu thất bại, vui lòng liên hệ IT" });
        }

        [HttpPost]
        public IActionResult SaveEditHotelService(int id, string serviceName, IFormFile ImageInputValue)
        {
            if (string.IsNullOrEmpty(serviceName) || ImageInputValue == null)
            {
                return Json(new { success = false, message = "Vui lòng nhập đủ thông tin" });
            }

            bool isSuccess = false;
            string imgUrl = Common.UploadImg(ImageInputValue);
            isSuccess = _unitOfWork_Repository.Hotel_Rep.SaveEditHotelService(id, serviceName, imgUrl);
            if (isSuccess == true)
            {
                return Json(new { success = true, message = "Lưu thành công" });
            }
            return Json(new { success = false, message = "Lưu thất bại, vui lòng liên hệ IT" });
        }

        [HttpPost]
        public IActionResult DeleteHotelService(int id)
        {
            bool isSuccess = _unitOfWork_Repository.Hotel_Rep.DeleteHotelServicesById(id);
            if (isSuccess == true)
            {
                return Json(new { success = true, message = "Xoá thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Xoá thất bại, vui lòng liên hệ IT" });
            }
        }
        #endregion
        #region HOTEL BOOKING

        public IActionResult HotelBooking(int page = 1, int pageSize = 10)
        {
            HotelBooking hotelBooking = new HotelBooking();
            var paginationVM = GetListHotelBookingPagination(hotelBooking, DateTime.MinValue, DateTime.MinValue, page, pageSize);
            return View(paginationVM);
        }
        private PaginationBase<HotelBooking> GetListHotelBookingPagination(HotelBooking request, DateTime fromDate, DateTime toDate, int page, int pageSize)
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

            var (listOrders, totalRecord) = _unitOfWork_Repository.Hotel_Rep.GetListHotelBookingV2(request, fromDate, toDate, page, pageSize);

            PaginationBase<HotelBooking> paginationVM = new PaginationBase<HotelBooking>()
            {
                ListProduct = listOrders,
                TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize),
                CurrentPage = page,
                PageSize = pageSize,
                TotalQuantityOfProduct = totalRecord
            };
            return paginationVM;
        }

        public IActionResult GetAllHotelBookingByFilter(HotelBooking request, DateTime fromDate, DateTime toDate, int page, int pageSize)
        {
            var listHotelPagination = GetListHotelBookingPagination(request, fromDate, toDate, page, pageSize);
            return PartialView("_TableHotelBookingPaginationPatial", listHotelPagination);
        }

        [HttpPost]
        public IActionResult DetailHotelBooking(string bookingCode, int statusId)
        {
            if (statusId == 1)
            {
                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                bool isSuccess = _unitOfWork_Repository.Hotel_Rep.DetailHotelBooking(bookingCode, acc.MaPhongBan, acc.HoTen);
            }

            HotelBooking result = _unitOfWork_Repository.Hotel_Rep.GetDetailHotelBookingByBookingCode(bookingCode);
            return View(result);
        }

        public async Task<IActionResult> UpdateOtherFee(string bookingCode, string otherFeeReason, double otherFee)
        {
            if (string.IsNullOrEmpty(otherFeeReason) || otherFee == 0)
            {
                return Json(new { success = false, message = "Vui lòng nhập đủ thông tin" });
            }
            bool isSuccess = await _unitOfWork_Repository.Hotel_Rep.UpdateOtherFee(bookingCode, otherFeeReason, otherFee);
            if (isSuccess)
            {
                HotelBooking hotelBooking = _unitOfWork_Repository.Hotel_Rep.GetDetailHotelBookingByBookingCode(bookingCode);
                return Json(new { success = true, message = "Cập nhật phí thành công", bookingCode, otherFee, otherFeeReason, totalPrice = hotelBooking.TotalPrice });

            }
            return Json(new { success = false, message = "Cập nhật thất bại, vui lòng liên hệ IT" });
        }

        [HttpPost]
        public async Task<IActionResult> CancelBooking(string bookingCode, string cancelReason)
        {
            if (string.IsNullOrEmpty(cancelReason))
            {
                return Json(new { success = false, message = "Vui lòng nhập lý do huỷ" });
            }
            AccountModel account = AccountManager.GetAccountCurrent(HttpContext);
            bool isSuccess = await _unitOfWork_Repository.Hotel_Rep.CancelBooking(bookingCode, cancelReason, account.Ten);
            if (isSuccess)
            {
                return Json(new { success = true, message = "Huỷ booking thành công", canceller = account.Ten, cancelReason });

            }
            return Json(new { success = false, message = "Đổi trạng thái thất bại, vui lòng liên hệ IT" });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(string bookingCode, int tinhTrang, string statusDescription)
        {
            if (string.IsNullOrEmpty(statusDescription) || tinhTrang == 0)
            {
                return Json(new { success = false, message = "Vui lòng nhập đầy đủ dữ liệu" });
            }
            var booking = _unitOfWork_Repository.Hotel_Rep.GetDetailHotelBookingByBookingCode(bookingCode);
            if (booking.StatusID == tinhTrang)
            {
                return Json(new { success = false, message = "Đơn hàng hiện tại đã ở trạng thái này rồi. Vui lòng chọn tình trạng khác để cập nhật." });
            }
            AccountModel account = AccountManager.GetAccountCurrent(HttpContext);
            bool isSuccess = await _unitOfWork_Repository.Hotel_Rep.ChangeStatus(bookingCode, tinhTrang, statusDescription, account.HoTen);
            if (isSuccess)
            {
                DateTime statusCreatedDate = DateTime.Now;
                string statusText = _unitOfWork_Repository.Hotel_Rep.GetStatusStringByStatusId(tinhTrang);
                HotelBooking hotelBooking = _unitOfWork_Repository.Hotel_Rep.GetDetailHotelBookingByBookingCode(bookingCode);

                return Json(new
                {
                    success = true,
                    message = "Đổi trạng thái thành công",
                    bookingCode = hotelBooking.BookingCode,
                    statusText,
                    statusId = tinhTrang,
                    statusDescription,
                    createdDate = statusCreatedDate.ToString("dd/MM/yyyy HH:mm:ss"),
                    createdBy = account.HoTen,
                    canceller = hotelBooking.Canceller,
                    cancelReason = hotelBooking.CancelReason,
                });
            }
            return Json(new { success = false, message = "Đổi trạng thái thất bại, vui lòng liên hệ IT" });
        }
        #endregion
    }
}
