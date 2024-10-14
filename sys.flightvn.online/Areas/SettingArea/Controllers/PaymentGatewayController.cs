using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Manager.Model.Models.PaymentGateway;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;


namespace Manager_EV.Areas.SettingArea.Controllers
{
    [Area(AreaNameConst.AREA_Setting)]
    public class PaymentGatewayController : Controller
    {
        private readonly ILogger<PaymentGatewayController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment webHostEnvironment;
        public GatewayRepository gatewayRepository;

        public PaymentGatewayController(ILogger<PaymentGatewayController> logger, IConfiguration configuration, IHostingEnvironment webHostEnvironment, GatewayRepository gatewayRepository)
        {
            this.gatewayRepository = gatewayRepository;
            _logger = logger;
            _configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> GetLinkPayment(string orderId)
        {
            GatewayRepository gatewayRepository = new GatewayRepository();
            string linkGateway = await gatewayRepository.GetLinkToPayment(orderId);
            return Json(new { success = true, link = linkGateway });
        }

        public IActionResult ListPaymentType()
        {
            var items = gatewayRepository.GetPaymentsWithFees();
            return View(items);
        }

        public IActionResult ListPaymentMethod()
        {
            var items = gatewayRepository.GetListPaymentMethod();
            return View(items);
        }

        public IActionResult AddPaymentMethod()
        {
            return View();
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPaymentMethod(PaymentMethod model, IFormFile imageFile)
        {
            try
            {
                model.IsActived = true;
                string defaultImage = "/images/PaymentGateway/enviet-logo.png";
                if (imageFile != null && imageFile.Length > 0)
                {
                    model.Image = UploadImg(imageFile);
                }
                else
                {
                    return Json(new { success = false, message = "Vui lòng chọn ảnh" });
                }
                gatewayRepository.AddPaymentMethod(model);
                return Json(new { success = true, message = "Thêm mới thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Vui lòng điền đủ thông tin" });
            }
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Payment model, string[] FeeName, double[] Percent, string[] RequestType, decimal[] FixedCosts, IFormFile imageFile, List<IFormFile> requestTypeImage)
        {
            try
            {
                model.IsActived = true;

                model.Image = UploadImg(imageFile);

                // Thêm Payment và lấy ID
                int paymentId = gatewayRepository.AddPayment(model);
                model.Id = paymentId;

                if (FeeName.Length > requestTypeImage.Count)
                {
                    return Json(new { success = false, message = "Vui lòng điền đủ thông tin và ảnh" });
                }

                // Xử lý danh sách Phí
                if (FeeName.Length > 0 && FeeName.Any(x => x != null) && FixedCosts.Length > 0 && FixedCosts.Any(x => x >= 0) && Percent.Length > 0 && Percent.Any(x => x != 0) && RequestType.Length > 0 && RequestType.Any(x => x != null) && requestTypeImage.Count > 0)
                {
                    for (int i = 0; i < FeeName.Length; i++)
                    {
                        string linkRequestTypeImage = "";
                        // Sử dụng đường dẫn đã được chọn
                        linkRequestTypeImage = UploadImg(requestTypeImage[i]);
                        PaymentFee paymentFee = new PaymentFee
                        {
                            PaymentId = model.Id,
                            RequestType = RequestType[i],
                            Name = FeeName[i],
                            Percent = Percent[i],
                            IsActived = true,
                            FixedCosts = FixedCosts[i],
                            PaymentName = model.Name,
                            Image = linkRequestTypeImage
                        };

                        gatewayRepository.AddPaymentFee(paymentFee);
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Vui lòng điền đủ thông tin" });
                }

                return Json(new { success = true, message = "Thêm mới phương thức thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Vui lòng điền đủ thông tin" });
            }
        }


        [HttpPost]
        public ActionResult ChangePaymentImage(IFormFile PaymentImage, int PaymentId)
        {
            try
            {
                string imageLink = "";
                if (PaymentImage != null && PaymentImage.Length > 0)
                {
                    imageLink = UploadImg(PaymentImage);
                    gatewayRepository.SavePaymentImage(PaymentId, imageLink);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public ActionResult Edit(string[] FeeName, double[] Percent, string[] RequestType, decimal[] FixedCosts, int PaymentID)
        {
            string connectionString = _configuration.GetConnectionString("SQL_PAYMENT_GATEWAY");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();

                    // Lấy thông tin thanh toán và phí thanh toán
                    var payment = dbConnection.QueryFirstOrDefault<Payment>("SELECT * FROM Payments WHERE Id = @Id", new { Id = PaymentID });
                    var paymentFees = dbConnection.Query<PaymentFee>("SELECT * FROM PaymentsFee WHERE PaymentId = @Id", new { Id = PaymentID }).ToList();

                    // Xử lý danh sách Phí
                    if (payment != null)
                    {
                        if (FeeName != null && Percent != null && RequestType != null)
                        {
                            for (int i = 0; i < FeeName.Length; i++)
                            {
                                PaymentFee fee;
                                if (i >= paymentFees.Count)
                                {
                                    fee = new PaymentFee
                                    {
                                        Name = FeeName[i],
                                        Percent = Percent[i],
                                        RequestType = RequestType[i],
                                        FixedCosts = FixedCosts[i],
                                        IsActived = true,
                                        PaymentName = payment.Name,
                                        PaymentId = PaymentID
                                    };

                                    dbConnection.Execute("INSERT INTO PaymentsFee (Name, [Percent], RequestType, FixedCosts, IsActived, PaymentName, PaymentId) VALUES (@Name, @Percent, @RequestType, @FixedCosts, @IsActived, @PaymentName, @PaymentId)", fee);
                                }
                                else
                                {
                                    fee = paymentFees[i];
                                    fee.Name = FeeName[i];
                                    fee.Percent = Percent[i];
                                    fee.RequestType = RequestType[i];
                                    fee.FixedCosts = FixedCosts[i];
                                    fee.IsActived = true;
                                    fee.PaymentName = payment.Name;
                                    dbConnection.Execute("UPDATE PaymentsFee SET Name = @Name, [Percent] = @Percent, RequestType = @RequestType, FixedCosts = @FixedCosts, IsActived = @IsActived, PaymentName = @PaymentName WHERE Id = @Id", fee);
                                }
                            }
                        }
                    }

                    TempData["SuccessMessage"] = "Thêm thành công";
                    return RedirectToAction("ListPaymentType", new { i = 12 });
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Vui lòng điền đủ thông tin";
                return RedirectToAction("ListPaymentType", new { i = 12 });
            }
        }


        [HttpPost]
        public ActionResult ChangePaymentFeeImage(IFormFile paymentFeeImage, int PaymentId)
        {
            try
            {
                var payment = gatewayRepository.GetPaymentFeeByID(PaymentId);

                if (paymentFeeImage != null && paymentFeeImage.Length > 0)
                {
                    string linkImage = "";
                    linkImage = UploadImg(paymentFeeImage);
                    payment.Image = linkImage;
                    gatewayRepository.UpdatePaymentFeeImage(linkImage, payment.Id);
                    return Json(new { success = true, message = "Cập nhật ảnh thành công" });
                }
                return Json(new { success = false, message = "Thao tác thất bại" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = gatewayRepository.GetPaymentByID(id);
            if (item != null)
            {
                item.IsActived = !item.IsActived;
                gatewayRepository.ChangeActivePayment(item.IsActived, id);
                return Json(new { success = true, item.IsActived });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsPaymentMethodActive(int id)
        {
            var item = gatewayRepository.GetPaymentMethodByID(id);
            if (item != null)
            {
                item.IsActived = !item.IsActived;
                gatewayRepository.ChangeActivePaymentMethod(item.IsActived, id);
                return Json(new { success = true, item.IsActived });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult ChangePaymentMethodImage(IFormFile paymentMethodImage, int PaymentId)
        {
            try
            {
                var payment = gatewayRepository.GetPaymentMethodByID(PaymentId);

                if (paymentMethodImage != null && paymentMethodImage.Length > 0)
                {
                    string linkImage = "";
                    linkImage = UploadImg(paymentMethodImage);
                    payment.Image = linkImage;
                    gatewayRepository.UpdatePaymentMethodImage(linkImage, payment.Id);
                    return Json(new { success = true, message = "Cập nhật ảnh thành công" });
                }
                return Json(new { success = false, message = "Thao tác thất bại" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }

        [HttpPost]
        public ActionResult EditPrice(int id, double fixedCost, double percent, string name)
        {
            var item = gatewayRepository.GetPaymentMethodByID(id);
            if (item != null)
            {
                item.FixedCosts = fixedCost;
                item.Percent = percent;
                item.Name = name;
                gatewayRepository.UpdatePaymentMethod(item);
                return Json(new { success = true, message = "Cập nhật giá thành công" });
            }
            return Json(new { success = false, message = "Cập nhật giá thất bại" });
        }

        [HttpPost]
        public ActionResult FeeIsActive(int id)
        {
            var item = gatewayRepository.GetPaymentFeeByID(id);
            if (item != null)
            {
                item.IsActived = !item.IsActived;
                gatewayRepository.ChangeActivePaymentFee(item.IsActived, id);
                return Json(new { success = true, item.IsActived });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult DeleteFee(int id)
        {
            var item = gatewayRepository.GetPaymentFeeByID(id);
            if (item != null)
            {
                gatewayRepository.DeletePaymentFee(item.Id);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
