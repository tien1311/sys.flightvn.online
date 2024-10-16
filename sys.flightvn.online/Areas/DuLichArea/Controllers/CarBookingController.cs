using EasyInvoice.Client;
using EasyInvoice.Json.Linq;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.Repository;
using Manager.DataAccess.Services.CarBooking;
using Manager.Model.Models;
using Manager.Model.Models.CarBooking;
using Manager.Model.Models.CarBooking.Result;
using Manager.Model.Models.Other;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;
using static Manager.DataAccess.Helpers.HoaDonHelper;

namespace Manager_EV.Areas.DuLichArea.Controllers
{
    [Area(AreaNameConst.AREA_DuLich)]
    public class CarBookingController : Controller
    {
        private readonly TaxiServices taxiServices;
        public CarBookingController(TaxiServices _taxiServices)
        {
            taxiServices = _taxiServices;
        }

        // Settings và lưu bản ghi (records) 
        public Dictionary<string, int> Record
        {
            get
            {
                if (HttpContext.Session.IsAvailable)
                {
                    // Chứa active record [0] và list records [1]
                    Dictionary<string, int> record = HttpContext.Session.GetObject<Dictionary<string, int>>("record");
                    if (record != null)
                    {
                        return record;
                    }
                }
                return new Dictionary<string, int> { { "record", 10 } };
            }
        }


        //[Route("CarBooking")]
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetObject("record", Record);
            int page = 1;
            int size = Record["record"];

            // Lấy dữ liệu nếu ViewBag.data chưa được thiết lập
            ViewBag.pending = await taxiServices.GetDataRecordSQL(null);

            // Phân trang
            PagedList<Manager.Model.Models.CarBooking.Request> pagination_pending = new PagedList<Manager.Model.Models.CarBooking.Request>(ViewBag.pending, page, size);
            ViewBag.pagination_pending = pagination_pending;

            return View();
        }

        public IActionResult Discount()
        {
            ViewBag.coupon = taxiServices.GetCoupon();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LoadPartialDataListBooking(string jsonString, string tab, int page)
        {

            int size = Record["record"];
            if (tab == "pending")
            {
                ViewBag.pending = await taxiServices.GetDataRecordSQL(jsonString);
                // Phân trang
                PagedList<Manager.Model.Models.CarBooking.Request> pagination_pending = new PagedList<Manager.Model.Models.CarBooking.Request>(ViewBag.pending, page, size);
                ViewBag.pagination_pending = pagination_pending;
                return PartialView("_Partial_Data_Booking_Pending");
            }
            else
            {
                ViewBag.finish = await taxiServices.GetDataRecordAPI(jsonString);
                // Phân trang
                PagedList<Record> pagination_finish = new PagedList<Record>(ViewBag.finish, page, size);
                ViewBag.pagination_finish = pagination_finish;
                return PartialView("_Partial_Data_Booking_Finish");
            }
        }

        [HttpPost]
        public async Task<string> SendBooking([FromBody] JObject jsonData)
        {
            AccountModel account = AccountManager.GetAccountCurrent(HttpContext);
            string result = "Gửi booking thất bại!";
            if (account.MaPhongBan == "DL" || account.MaPhongBan == "IT")
            {
                int id = int.Parse(jsonData["id"].ToString());
                var request = await taxiServices.GetRequests(id);
                result = await taxiServices.SendBooking(request);
            }
            return result;
        }

        //public async Task<string> SendEmail(Manager.Model.Models.CarBooking.Request request)
        //{
        //    return await taxiServices.SendEmail(request);
        //}


        public async Task<string> AddCoupon([FromBody] JObject data)
        {
            return await taxiServices.AddCoupon(data);
        }

        [HttpPost]
        public async Task<string> ChangeStatus([FromBody] JObject data)
        {
            return await taxiServices.ChangeStatus(data);
        }

        [HttpPost]
        public async Task<string> GetLinkPayment([FromBody] JObject data)
        {
            var result = await taxiServices.GetLinkPayment_Ver2(data);
            return result;
        }


        public async Task<string> EditCoupon([FromBody] JObject data)
        {
            return await taxiServices.EditCoupon(data);
        }

        public async Task<string> ApplyCoupon([FromBody] JObject data)
        {
            return await taxiServices.ApplyCoupon(data);
        }


        public async Task<string> DeleteCoupon([FromBody] JObject data)
        {
            return await taxiServices.DeleteCoupon(data);
        }


        [HttpPost]
        public async Task<IActionResult> LoadPartialModalBookingDetail([FromBody] JObject data)
        {
            AccountModel account = AccountManager.GetAccountCurrent(HttpContext);
            string evcode = data["evcode"].ToString();
            var request = await taxiServices.GetRequests(evcode);
            if (account.MaPhongBan == "DL" && request.status_enviet == "PENDING")
            {
                await taxiServices.UpdateStatusEnviet(evcode, "ACCEPT", account.TenDangNhap);
            }
            else if (account.MaPhongBan == "IT" && request.status_enviet == "PENDING")
            {
                await taxiServices.UpdateStatusEnviet(evcode, "PENDING", null);
            }
            ViewBag.booking_detail = request;

            return PartialView("_Partial_Modal_Booking_Detail");
        }

        [HttpPost]
        public async Task<string> UpdatePayment([FromBody] JObject data)
        {
            var result = await taxiServices.UpdatePayment(data);
            if (result)
            {
                return "Cập nhật phương thức thanh toán thành công";
            }
            else
            {
                return "Cập nhật phương thức thanh toán thất bại";
            }
        }

        public async Task<IActionResult> UpdateOtherFee(string evcode, double otherFee, string reason)
        {
            var isSuccess = false;
            evcode = evcode.Trim();

            if (string.IsNullOrEmpty(otherFee.ToString()) || string.IsNullOrEmpty(reason))
            {
                return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin" });
            }

            isSuccess = await taxiServices.UpdateOtherFee(evcode, otherFee, reason);
            if (isSuccess)
            {
                var item = await taxiServices.GetBookingByevcode(evcode);
                return Json(new { success = true, message = "Cập nhật phí thành công", newTotalPrice = item.total, reason = item.other_fee_reason });
            }
            return Json(new { success = false, message = "Cập nhật phí thất bại, vui lòng kiểm tra lại thông tin hoặc liên hệ IT" });
        }

        public async Task<IActionResult> UpdateThanhToanChuyenKhoan(string evcode)
        {
            bool isSuccess_Car = false;
            bool isSuccess_Payment = false;
            var gateway_Rep = new GatewayRepository();

            var isSuccess_Car_Task = taxiServices.UpdateThanhToanChuyenKhoan(evcode);
            var isSuccess_Payment_Task = gateway_Rep.UpdateUsersPaysChuyenKhoan(evcode);

            Task.WaitAll(isSuccess_Car_Task, isSuccess_Payment_Task);
            isSuccess_Car = isSuccess_Car_Task.Result;
            isSuccess_Payment = isSuccess_Payment_Task.Result;

            if (isSuccess_Car && isSuccess_Payment)
            {
                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
            }
            return Json(new { success = false, message = "Cập nhật trạng thái thất bại, vui lòng kiểm tra lại thông tin hoặc liên hệ IT" });
        }

        [HttpPost]
        public async Task<IActionResult> CancelBooking([FromBody] JObject data)
        {
            AccountModel account = AccountManager.GetAccountCurrent(HttpContext);
            var response = "Hủy chuyến thất bại!";
            if (account.MaPhongBan == "DL" || account.MaPhongBan == "IT")
            {
                var result = await taxiServices.CancelBooking(data);
                response = result == 1 ? "Hủy chuyến thành công!" : "Hủy chuyến thất bại!";
            }
            var obj = Json(new
            {
                msg = response,
            });
            return obj;
        }


        [HttpPost]
        public async Task UpdateStatusBooking(string idbooking, string status)
        {
            await taxiServices.UpdateStatusBooking(idbooking, status);
        }

        public async Task UpdateStatusEnviet(string idbooking, string status, string user)
        {
            await taxiServices.UpdateStatusEnviet(idbooking, status, user);
        }


        public async Task<List<Manager.Model.Models.CarBooking.Request>> GetDataRecordSQL(string jsonString)
        {
            List<Manager.Model.Models.CarBooking.Request> data = await taxiServices.GetDataRecordSQL(jsonString);
            return data;
        }

        public async Task<List<Record>> GetDataRecordAPI(string jsonString)
        {
            List<Record> data = await taxiServices.GetDataRecordAPI(jsonString);
            return data;
        }

        public async Task<IActionResult> GetLocation(string keyword = "")
        {
            List<Address> list_address_start = await taxiServices.GetLocation(keyword);
            return Json(list_address_start);
        }

        public async Task<List<string>> GetLocationData()
        {
            List<string> list_pickup_address = await taxiServices.GetLocationData();
            return list_pickup_address;
        }

        [HttpGet]
        public async Task<string> SyncStatusBooking(string id)
        {
            return await taxiServices.SyncStatusBooking(id);
        }

    }
}
