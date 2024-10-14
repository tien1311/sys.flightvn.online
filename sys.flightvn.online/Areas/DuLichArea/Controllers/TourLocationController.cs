using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Manager.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Manager_EV.Areas.DuLichArea.Controllers
{
    [Area(AreaNameConst.AREA_DuLich)]
    public class TourLocationController : Controller
    {
        //TourLocationRepository TourLocation_Rep = new TourLocationRepository();
        //private readonly LocationRepository location_Rep = new LocationRepository();
        private readonly IUnitOfWork_Repository _repositoryManager;
        private readonly IConfiguration _configuration;

        public TourLocationController(IConfiguration configuration, IUnitOfWork_Repository ManagerAllRepository)
        {
            _configuration = configuration;
            _repositoryManager = ManagerAllRepository;
        }

        public IActionResult TourLocation()
        {
            List<TourLocation> listTourLocation = _repositoryManager.TourLocation_Rep.GetListTourLocation();
            foreach (TourLocation item in listTourLocation)
            {
                item.Province = GetProvinceByCode(item.Province);
                item.District = GetDistrictByCode(item.District);
            };
            return View(listTourLocation);
        }

        public IActionResult CreateTourLocation()
        {
            var listProvince = _repositoryManager.Location_Rep.GetProvinces();
            ViewData["ListProvince"] = listProvince;
            return View();
        }

        public IActionResult EditTourLocation(int id)
        {
            var listProvince = _repositoryManager.Location_Rep.GetProvinces();
            ViewData["ListProvince"] = listProvince;
            TourLocation tourLocation = _repositoryManager.TourLocation_Rep.GetTourLocationById(id);
            return View(tourLocation);
        }

        [HttpPost]
        public IActionResult UpsertTourLocation(TourLocation tourLocation)
        {
            // Handle Error
            string message = HandleError(tourLocation);
            if (string.IsNullOrEmpty(message))
            {
                bool isValid = true;
                if (tourLocation.Id == 0) // Id == 0 => Create
                {
                    message = "Thêm Thành công.";
                }
                else
                {
                    message = "Chỉnh sửa thành công.";
                }
                isValid = _repositoryManager.TourLocation_Rep.SaveUpsertTourLocation(tourLocation);

                if (isValid)
                {
                    return Json(new { success = true, message });
                }
                else
                {
                    // error from db
                    message = "Thao tác thất bại.";
                    return Json(new { success = false, message });
                }
            }
            else
            {
                // error from field
                return Json(new { success = false, message });
            }
        }

        public IActionResult DeleteTourLocation(int id)
        {
            bool isValid = _repositoryManager.TourLocation_Rep.DeleteTourLocationById(id);
            if (isValid)
            {
                return Json(new { success = true, message = "Xóa thành công." });
            }
            else
                return Json(new { success = false, message = "Xóa thất bại." });
        }

        public string HandleError(TourLocation tourLocation)
        {
            if (string.IsNullOrEmpty(tourLocation.Name))
            {
                return "Vui lòng nhập điểm tham quan.";
            }
            if (string.IsNullOrEmpty(tourLocation.Phone))
            {
                return "Vui lòng nhập số điện thoại";
            }
            else
            {
                // Regex để kiểm tra số điện thoại có 10 chữ số và bắt đầu bằng số 0
                string pattern = @"^0\d{9}$";
                if (!Regex.IsMatch(tourLocation.Phone, pattern))
                {
                    return "Số điện thoại không hợp lệ.";
                }
            }
            if (string.IsNullOrEmpty(tourLocation.Email))
            {
                return "Vui lòng nhập Email.";
            }
            else
            {
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(tourLocation.Email, pattern))
                {
                    return "Địa chỉ Email không hợp lệ. Vui lòng nhập đúng định dạng Email.";
                }
            }
            if (string.IsNullOrEmpty(tourLocation.Province))
            {
                return "Vui lòng chọn tỉnh/thành.";

            }
            if (string.IsNullOrEmpty(tourLocation.District))
            {
                return "Vui lòng chọn quận.";
            }
            return "";
        }
        public string GetDistrictByCode(string districtCode)
        {
            var districts = _repositoryManager.Location_Rep.GetDistrictByCode(districtCode);
            return districts;
        }
        public string GetProvinceByCode(string provinceCode)
        {
            var province = _repositoryManager.Location_Rep.GetProvinceByCode(provinceCode);
            return province;
        }
        #region API_METHOD
        [HttpGet]
        public IActionResult GetDistricts(string provinceCode)
        {
            var districts = _repositoryManager.Location_Rep.GetDistrictsByProvinceCode(provinceCode);
            return Json(districts);
        }
        #endregion
    }
}
