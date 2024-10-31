using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.Serialization.Formatters.Binary;
using ReflectionIT.Mvc.Paging;
using OfficeOpenXml;
using System.Drawing;
using Microsoft.AspNetCore.Routing;
using System.Data;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using Microsoft.Extensions.Configuration;
using System.Linq;
using EasyInvoice.Json;
//using Manager_EV.Service;
//using Manager_EV.Services.Notification.Request;
//using Manager_Manager.Models.CarBooking;
//using Manager_Manager.Models.PaymentGateway;
using X.PagedList;
using Manager.Model.Models.ViewModel;
using Manager.Model.Models.Other;
using Manager.Model.Models;
using Manager.Model.Services.Abstraction;
using Manager.Model.Services.Model.Request;
using Manager.Model.Models.HoaDonModels.HDDT;
using Manager.DataAccess.Repository;
using Manager.Model.Services.Notification.Request;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Manager.Model.Models.PaginationBase;
using Manager.Model.Models.BankAccount;
using Manager.Common.Helpers;
using System.Threading;
using Manager.Model.Models.Interface.IDeleteModel;

namespace Manager_EV.Areas.KeToanArea.Controllers
{
    //[Route("[Controller]/[action]")]
    [Area(AreaNameConst.AREA_KeToan)]
    public class KetoanController : Controller
    {
        ImportDoanhSoRepository ImportDoanhSo_Rep = new ImportDoanhSoRepository();
        CultureInfo provider;
       
        ConvertObjectToByte convert = new ConvertObjectToByte();
        private IHostingEnvironment _hostingEnvironment;
        private IEInvoiceService _invoiceService;
        private readonly IConfiguration _configuration;
        private INotifyService _notifyService;
        private IUnitOfWork_Repository _unitOfWork_Repository;
        Uilti uilti = new Uilti();
        private static Stack<DeletedItemModel<BankAccount>> deletedItemsStack = new Stack<DeletedItemModel<BankAccount>>();
        private static Stack<DeletedItemModel<BankAccountDetail>> deletedItemsStack_BankAccountDetail = new Stack<DeletedItemModel<BankAccountDetail>>();
        private static Timer deletionTimer;

        public KetoanController(IEInvoiceService invoiceService, IHostingEnvironment hostingEnvironment, IConfiguration configuration, INotifyService notifyService, IUnitOfWork_Repository unitOfWork_Repository)
        {
            _invoiceService = invoiceService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _notifyService = notifyService;
            _unitOfWork_Repository = unitOfWork_Repository;
            InitializeDeletionTimer();
        }
        private void InitializeDeletionTimer()
        {
            deletionTimer = new Timer(CheckAndRemoveExpiredItems, null, 1000, 1000);
        }
        private static void CheckAndRemoveExpiredItems(object state)
        {
            var now = DateTime.Now;
            while (deletedItemsStack.Count > 0 && (now - deletedItemsStack.Peek().DeletionTime).TotalSeconds > 5)
            {
                var deletedItems = deletedItemsStack.Pop().DeletedItems;
            }
            while (deletedItemsStack_BankAccountDetail.Count > 0 && (now - deletedItemsStack_BankAccountDetail.Peek().DeletionTime).TotalSeconds > 5)
            {
                var deletedItems = deletedItemsStack_BankAccountDetail.Pop().DeletedItems;
            }
        }

        public IActionResult Index_KT()
        {
            return View();
        }
        public IActionResult ChinhSachChietKhau()
        {
            return View();
        }

        public IActionResult BankAccount(int page = 1, int pageSize = 10)
        {
            var paginationVM = GetListBankAccountPagination(string.Empty, string.Empty, string.Empty, page, pageSize);
            return View(paginationVM);
        }

        private PaginationBase<BankAccount> GetListBankAccountPagination(string bankCodes, string bankNames, string firstSerials, int page, int pageSize)
        {
            var bankCodeList = Common.ParseStringList(bankCodes);
            var bankNameList = Common.ParseStringList(bankNames);
            var firstSerialsList = Common.ParseStringList(firstSerials);
            var (listBank, totalRecord) = _unitOfWork_Repository.Bank_Rep.GetBankAccounts(bankCodeList, bankNameList, firstSerialsList, page, pageSize);

            PaginationBase<BankAccount> paginationVM = new PaginationBase<BankAccount>()
            {
                ListProduct = listBank,
                TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize),
                CurrentPage = page,
                PageSize = pageSize,
                TotalQuantityOfProduct = totalRecord
            };
            return paginationVM;
        }

        public IActionResult GetAllBankAccountByFilter(string bankCodes, string bankNames, string firstSerials, int pageSize, int page)
        {
            var listBankAccountPagination = GetListBankAccountPagination(bankCodes, bankNames, firstSerials, page, pageSize);
            return PartialView("_TableBankAccountPaginationPatial", listBankAccountPagination);
        }

        public IActionResult CreateBankAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveCreateBankAccount(BankAccount request)
        {
            if (string.IsNullOrEmpty(request.BankCode)  || string.IsNullOrEmpty(request.BankName) || string.IsNullOrEmpty(request.FirstSerial))
            {
                return Json(new { success = false, message = "Vui lòng nhập đủ dữ liệu" });
            }

            bool IsStringDiacritic = Common.IsStringDiacritic(request.BankCode);
            if (IsStringDiacritic == true)
            {
                return Json(new { success = false, message = "Mã ngân hàng phải là không dấu" });
            }

            BankAccount bankAccount = _unitOfWork_Repository.Bank_Rep.GetBankAccount(request.BankCode);
            if (bankAccount != null)
            {
                return Json(new { success = false, message = $"Mã ngân hàng {request.BankCode} đã tồn tại " });
            }

            var acc = AccountManager.GetAccountCurrent(HttpContext);
            bool isSuccess = _unitOfWork_Repository.Bank_Rep.SaveCreateBankAccount(request, acc.MaNV).GetAwaiter().GetResult();
            if (isSuccess == true)
            {
                return Json(new { success = true, message = "Lưu thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Lưu thất bại" });
            }
        }


        public IActionResult EditBankAccount(int Id)
        {
            BankAccount bankAccount = new BankAccount();
            bankAccount = _unitOfWork_Repository.Bank_Rep.GetBankAccount(Id);
            return View(bankAccount);
        }

        [HttpPost]
        public IActionResult SaveEditBankAccount(BankAccount request, string currentBankCode)
        {
            if (string.IsNullOrEmpty(request.BankCode) || string.IsNullOrEmpty(request.BankName) || string.IsNullOrEmpty(request.FirstSerial))
            {
                return Json(new { success = false, message = "Vui lòng nhập đủ dữ liệu" });
            }

            bool IsStringDiacritic = Common.IsStringDiacritic(request.BankCode);
            if (IsStringDiacritic == true)
            {
                return Json(new { success = false, message = "Mã ngân hàng phải là không dấu" });
            }

            if (request.BankCode != currentBankCode)
            {
                BankAccount bankAccount = _unitOfWork_Repository.Bank_Rep.GetBankAccount(request.BankCode);
                if (bankAccount != null)
                {
                    return Json(new { success = false, message = $"Mã ngân hàng {request.BankCode} đã tồn tại " });
                }
            }

            var acc = AccountManager.GetAccountCurrent(HttpContext);
            bool isSuccess = _unitOfWork_Repository.Bank_Rep.SaveEditBankAccount(request, acc.MaNV).GetAwaiter().GetResult();
            if (isSuccess == true)
            {
                return Json(new { success = true, message = "Lưu thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Lưu thất bại" });
            }
        }

        //public IActionResult DeleteBankAccount(int Id)
        //{
        //    bool isSuccess = _unitOfWork_Repository.Bank_Rep.DeleteBankAccount(Id).GetAwaiter().GetResult();
        //    if (isSuccess == true)
        //    {
        //        return Json(new { success = true, message = "Xoá thành công" });
        //    }
        //    else
        //    {
        //        return Json(new { success = false, message = "Xoá thất bại, vui lòng liên hệ IT" });
        //    }
        //}

        [HttpPost]
        public IActionResult DeleteBankAccount(int id)
        {
            var deletedItems = new List<BankAccount>(); // Lưu trữ các mục đã xóa tạm thời
            var bankAccountItem = _unitOfWork_Repository.Bank_Rep.GetBankAccount(Convert.ToInt32(id));
            if (bankAccountItem != null)
            {
                bool success = _unitOfWork_Repository.Bank_Rep.DeleteBankAccount(Convert.ToInt32(id)).GetAwaiter().GetResult();
                if (success)
                {
                    deletedItems.Add(bankAccountItem); // Lưu mục đã xóa
                }
            }
            if (deletedItems.Any())
            {
                deletedItemsStack.Push(new DeletedItemModel<BankAccount>
                {
                    DeletedItems = deletedItems,
                    DeletionTime = DateTime.Now
                }); // Lưu các mục đã xóa vào Stack
                return Json(new { success = true, message = "Xoá thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi xoá ngân hàng" });
            }
        }

        [HttpPost]
        public IActionResult DeleteBankAccountSelected(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    var deletedItems = new List<BankAccount>(); // Lưu trữ các mục đã xóa tạm thời

                    foreach (var item in items)
                    {
                        var bankAccountItem = _unitOfWork_Repository.Bank_Rep.GetBankAccount(Convert.ToInt32(item));
                        if (bankAccountItem != null)
                        {
                            var success = _unitOfWork_Repository.Bank_Rep.DeleteBankAccount(Convert.ToInt32(item)).GetAwaiter().GetResult();
                            if (success)
                            {
                                deletedItems.Add(bankAccountItem); // Lưu mục đã xóa
                            }
                        }
                    }
                    if (deletedItems.Any())
                    {
                        deletedItemsStack.Push(new DeletedItemModel<BankAccount>
                        {
                            DeletedItems = deletedItems,
                            DeletionTime = DateTime.Now
                        }); // Lưu các mục đã xóa vào Stack
                        return Json(new { success = true, message = "Xoá thành công" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Có lỗi xảy ra khi xoá tài khoản ngân hàng" });
                    }
                }
            }
            return Json(new { success = false, message = "Có lỗi xảy ra khi xoá tài khoản ngân hàng" });
        }


        [HttpPost]
        public async Task<IActionResult> UndoDeleteBankAccount()
        {
            if (deletedItemsStack.Count > 0)
            {
                var deletedItemModel = deletedItemsStack.Pop(); // Lấy các mục đã xóa cuối cùng ra khỏi Stack
                var items = deletedItemModel.DeletedItems;
                bool allSuccess = true;
                var acc = AccountManager.GetAccountCurrent(HttpContext);

                foreach (var item in items)
                {
                    var success = await _unitOfWork_Repository.Bank_Rep.SaveUndoBankAccount(item, acc.MaNV);
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



        public IActionResult BankAccountDetail(int page = 1, int pageSize = 10)
        {
            var paginationVM = GetListBankAccountDetailPagination(string.Empty, string.Empty, string.Empty, page, pageSize);
            return View(paginationVM);
        }

        private PaginationBase<BankAccountDetail> GetListBankAccountDetailPagination(string agentCodes, string phoneNumbers, string secondarySerials, int page, int pageSize)
        {
            var agentCodeList = Common.ParseStringList(agentCodes);
            var phoneNumberList = Common.ParseStringList(phoneNumbers);
            var secondarySerialsList = Common.ParseStringList(secondarySerials);
            var (listBank, totalRecord) = _unitOfWork_Repository.Bank_Rep.GetBankAccountDetails(agentCodeList, phoneNumberList, secondarySerialsList, page, pageSize);

            PaginationBase<BankAccountDetail> paginationVM = new PaginationBase<BankAccountDetail>()
            {
                ListProduct = listBank,
                TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize),
                CurrentPage = page,
                PageSize = pageSize,
                TotalQuantityOfProduct = totalRecord
            };
            return paginationVM;
        }

        public IActionResult GetAllBankAccountDetailByFilter(string agentCodes, string phoneNumbers, string secondarySerials, int pageSize, int page)
        {
            var listBankAccountDetailPagination = GetListBankAccountDetailPagination(agentCodes, phoneNumbers, secondarySerials, page, pageSize);
            return PartialView("_TableBankAccountDetailPaginationPatial", listBankAccountDetailPagination);
        }

        public IActionResult CreateBankAccountDetail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveCreateBankAccountDetail(BankAccountDetail request)
        {
            if (string.IsNullOrEmpty(request.AgentCode) || string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.SecondarySerial))
            {
                return Json(new { success = false, message = "Vui lòng nhập đủ dữ liệu" });
            }

            BankAccountDetail bankAccountDetail = _unitOfWork_Repository.Bank_Rep.GetBankAccountDetail(request.AgentCode);
            if (bankAccountDetail != null)
            {
                return Json(new { success = false, message = $"Mã KH {request.AgentCode} đã tồn tại " });
            }

            var acc = AccountManager.GetAccountCurrent(HttpContext);
            bool isSuccess = _unitOfWork_Repository.Bank_Rep.SaveCreateBankAccountDetail(request, acc.MaNV).GetAwaiter().GetResult();
            if (isSuccess == true)
            {
                return Json(new { success = true, message = "Lưu thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Lưu thất bại" });
            }
        }


        public IActionResult EditBankAccountDetail(int Id)
        {
            BankAccountDetail bankAccountDetail = new BankAccountDetail();
            bankAccountDetail = _unitOfWork_Repository.Bank_Rep.GetBankAccountDetail(Id);
            return View(bankAccountDetail);
        }

        [HttpPost]
        public IActionResult SaveEditBankAccountDetail(BankAccountDetail request, string currentAgentCode)
        {
            if (string.IsNullOrEmpty(request.AgentCode) || string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.SecondarySerial))
            {
                return Json(new { success = false, message = "Vui lòng nhập đủ dữ liệu" });
            }

            if (request.AgentCode != currentAgentCode)
            {
                BankAccountDetail bankAccountDetail = _unitOfWork_Repository.Bank_Rep.GetBankAccountDetail(request.AgentCode);
                if (bankAccountDetail != null)
                {
                    return Json(new { success = false, message = $"Mã KH {request.AgentCode} đã tồn tại " });
                }
            }

            var acc = AccountManager.GetAccountCurrent(HttpContext);
            bool isSuccess = _unitOfWork_Repository.Bank_Rep.SaveEditBankAccountDetail(request, acc.MaNV).GetAwaiter().GetResult();
            if (isSuccess == true)
            {
                return Json(new { success = true, message = "Lưu thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Lưu thất bại" });
            }
        }

        [HttpPost]
        public IActionResult DeleteBankAccountDetail(int id)
        {
            var deletedItems = new List<BankAccountDetail>(); // Lưu trữ các mục đã xóa tạm thời
            var bankAccountItem = _unitOfWork_Repository.Bank_Rep.GetBankAccountDetail(Convert.ToInt32(id));
            if (bankAccountItem != null)
            {
                bool success = _unitOfWork_Repository.Bank_Rep.DeleteBankAccountDetail(Convert.ToInt32(id)).GetAwaiter().GetResult();
                if (success)
                {
                    deletedItems.Add(bankAccountItem); // Lưu mục đã xóa
                }
            }
            if (deletedItems.Any())
            {
                deletedItemsStack_BankAccountDetail.Push(new DeletedItemModel<BankAccountDetail>
                {
                    DeletedItems = deletedItems,
                    DeletionTime = DateTime.Now
                }); // Lưu các mục đã xóa vào Stack
                return Json(new { success = true, message = "Xoá thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi xoá ngân hàng" });
            }
        }

        [HttpPost]
        public IActionResult DeleteBankAccountDetailSelected(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    var deletedItems = new List<BankAccountDetail>(); // Lưu trữ các mục đã xóa tạm thời

                    foreach (var item in items)
                    {
                        var bankAccountDetailItem = _unitOfWork_Repository.Bank_Rep.GetBankAccountDetail(Convert.ToInt32(item));
                        if (bankAccountDetailItem != null)
                        {
                            var success = _unitOfWork_Repository.Bank_Rep.DeleteBankAccountDetail(Convert.ToInt32(item)).GetAwaiter().GetResult();
                            if (success)
                            {
                                deletedItems.Add(bankAccountDetailItem); // Lưu mục đã xóa
                            }
                        }
                    }
                    if (deletedItems.Any())
                    {
                        deletedItemsStack_BankAccountDetail.Push(new DeletedItemModel<BankAccountDetail>
                        {
                            DeletedItems = deletedItems,
                            DeletionTime = DateTime.Now
                        }); // Lưu các mục đã xóa vào Stack
                        return Json(new { success = true, message = "Xoá thành công" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Có lỗi xảy ra khi xoá tài khoản ngân hàng" });
                    }
                }
            }
            return Json(new { success = false, message = "Có lỗi xảy ra khi xoá tài khoản ngân hàng" });
        }


        [HttpPost]
        public async Task<IActionResult> UndoDeleteBankAccountDetail()
        {
            if (deletedItemsStack_BankAccountDetail.Count > 0)
            {
                var deletedItemModel = deletedItemsStack_BankAccountDetail.Pop(); // Lấy các mục đã xóa cuối cùng ra khỏi Stack
                var items = deletedItemModel.DeletedItems;
                bool allSuccess = true;
                var acc = AccountManager.GetAccountCurrent(HttpContext);

                foreach (var item in items)
                {
                    var success = await _unitOfWork_Repository.Bank_Rep.SaveUndoBankAccountDetail(item, acc.MaNV);
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

        public IActionResult ExportAfterImportExcel(string agentCodes, string phoneNumbers, string secondarySerials, IFormFile excelFile)
        {

            var agentCodeList = Common.ParseStringList(agentCodes);
            var phoneNumberList = Common.ParseStringList(phoneNumbers);
            var secondarySerialsList = Common.ParseStringList(secondarySerials);
            List<BankAccountDetail> listBankAccountDetails = _unitOfWork_Repository.Bank_Rep.GetBankAccountDetails(agentCodeList, phoneNumberList, secondarySerialsList);
            try
            {
                using (var package = new ExcelPackage(excelFile.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Get the first worksheet

                    // Find column indices for AgentCode and PhoneNumber
                    int agentCodeColIndex = ExcelHelper.FindColumnIndex(worksheet, BankAccountDetailExcelHeader.AgentCode);
                    int phoneNumberColIndex = ExcelHelper.FindColumnIndex(worksheet, BankAccountDetailExcelHeader.PhoneNumber);
                    int serialColIndex = ExcelHelper.FindColumnIndex(worksheet, BankAccountDetailExcelHeader.Serial);

                    if (agentCodeColIndex == -1 || serialColIndex == -1)
                    {
                        return Json(new { success = false, message = "Lỗi, không tìm thấy cột AgentCode hoặc Số tài khoản." });
                    }

                    // Find row indices for AgentCode, PhoneNumber and Serial
                    int rowAgentCodeIndex = ExcelHelper.FindRowIndex(worksheet, BankAccountDetailExcelHeader.AgentCode) + 1; // Start from the row below the header
                    int rowSerialIndex = ExcelHelper.FindRowIndex(worksheet, BankAccountDetailExcelHeader.Serial) + 1; // Start from the row below the header
                    int rowPhoneNumberIndex = phoneNumberColIndex == -1 ? -1 : ExcelHelper.FindRowIndex(worksheet, BankAccountDetailExcelHeader.PhoneNumber) + 1; // Start from the row below the header

                    foreach (var detail in listBankAccountDetails)
                    {
                        worksheet.Cells[rowAgentCodeIndex, agentCodeColIndex].Value = detail.AgentCode;
                        worksheet.Cells[rowSerialIndex, serialColIndex].Value = detail.SecondarySerial;
                        if (phoneNumberColIndex != -1)
                        {
                            worksheet.Cells[rowPhoneNumberIndex, phoneNumberColIndex].Value = detail.PhoneNumber;
                            rowPhoneNumberIndex++;
                        }
                        rowAgentCodeIndex++;
                        rowSerialIndex++;
                    }

                    // Prepare the file for download
                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        //private int FindColumnIndex(ExcelWorksheet worksheet, string columnName)
        //{
        //    // Tìm tất cả các dòng có thể chứa tiêu đề cột
        //    for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
        //    {
        //        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //        {
        //            if (worksheet.Cells[row, col].Text.Equals(columnName, StringComparison.OrdinalIgnoreCase))
        //            {
        //                return col;
        //            }
        //        }
        //    }
        //    return -1; // Trả về -1 nếu không tìm thấy cột
        //}

        //private int FindRowIndex(ExcelWorksheet worksheet, string columnName)
        //{
        //    // Tìm tất cả các cột có thể chứa tiêu đề cột
        //    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //    {
        //        for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
        //        {
        //            if (worksheet.Cells[row, col].Text.Equals(columnName, StringComparison.OrdinalIgnoreCase))
        //            {
        //                return row;
        //            }
        //        }
        //    }
        //    return -1; // Trả về -1 nếu không tìm thấy dòng
        //}

        public IActionResult Phieubaolanh(string IDtxt, string MaKHtxt, string tenDLtxt, string ghichutxt, string tientxt, string searchBtn, string newBtn, string saveBtn, string editBtn, string searchPBLBtn, string delBtn, string thoigiantxt)
        {
            PhieuBaoLanhRepository phieuBaoLanh_Rep = _unitOfWork_Repository.PhieuBaoLanh_Rep;
            PhieuBaoLanhModel result = new PhieuBaoLanhModel();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (editBtn != null)
            {
                string tenNV = acc.HoTen;
                bool ret = phieuBaoLanh_Rep.EditPBL(MaKHtxt, tenDLtxt, ghichutxt, tientxt, tenNV, IDtxt, thoigiantxt);
                result = phieuBaoLanh_Rep.DSPhieuBaoLanh();
                if (ret == true)
                {
                    TempData["thongbaoSuccess"] = "Bạn đã lưu phiếu bảo lãnh thành công";
                }
                else TempData["thongbaoError"] = "Bạn đã lưu không thành công";
                return View("PhieuBaoLanh", result);
            }
            if (delBtn != null)
            {
                string tenNV = acc.HoTen;
                bool ret = phieuBaoLanh_Rep.DelPBL(IDtxt, tenNV);
                result = phieuBaoLanh_Rep.DSPhieuBaoLanh();
                if (ret == true)
                {
                    TempData["thongbaoSuccess"] = "Bạn đã xóa phiếu bảo lãnh thành công";
                }
                else TempData["thongbaoError"] = "Bạn đã xóa không thành công";
                return View("PhieuBaoLanh", result);
            }
            if (searchBtn != null)
            {
                result = phieuBaoLanh_Rep.DSDaiLy(MaKHtxt);
                return View("PhieuBaoLanh", result);
            }
            if (newBtn != null)
            {

            }
            if (saveBtn != null)
            {
                string tenNV = acc.HoTen;
                bool ret = phieuBaoLanh_Rep.SavePBL(MaKHtxt, tenDLtxt, ghichutxt, tientxt, tenNV, thoigiantxt);
                result = phieuBaoLanh_Rep.DSPhieuBaoLanh();
                if (ret == true)
                {
                    TempData["thongbaoSuccess"] = "Bạn đã lưu phiếu bảo lãnh thành công";
                }
                else TempData["thongbaoError"] = "Bạn đã lưu không thành công";
                return View("PhieuBaoLanh", result);
            }
            if (searchPBLBtn != null)
            {
                result = phieuBaoLanh_Rep.SearchPBL(MaKHtxt);
                return View("PhieuBaoLanh", result);
            }
            result = phieuBaoLanh_Rep.DSPhieuBaoLanh();
            return View("PhieuBaoLanh", result);
        }
        public IActionResult CongNo()
        {
            return View();
        }

        public IActionResult DanhSachThanhToan()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchThanhToan(DateTime fromDate, DateTime toDate, string orderIds, string paymentType, int pageNumber, int pageSize)
        {
            var gateway_Rep = _unitOfWork_Repository.Gateway_Rep;
            if (toDate != DateTime.MinValue)
            {
                toDate = toDate.AddHours(23).AddMinutes(59).AddSeconds(59); // cuối ngày
            }

            var orderIdList = string.IsNullOrEmpty(orderIds)
                              ? new List<string>()
                              : orderIds.Split(',')
                                         .Select(orderId => orderId.Trim())
                                         .Where(orderId => !string.IsNullOrEmpty(orderId))
                                         .ToList();
            var listUser = gateway_Rep.GetUsersPays_KeToan(orderIdList, fromDate, toDate, paymentType, pageNumber, pageSize);
            var totalRecords = gateway_Rep.GetTotalUsersPays_KeToan(orderIdList, fromDate, toDate, paymentType);
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var totalAmount = gateway_Rep.GetTotalAmount_KeToan(orderIdList, fromDate, toDate, paymentType);
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalAmount = totalAmount;

            return PartialView("Partial_UserPayDataKeToan", listUser);
        }


        [HttpPost]
        public IActionResult ExportThanhToan(DateTime fromDate, DateTime toDate, string orderIds, string paymentType)
        {
            var gateway_Rep = _unitOfWork_Repository.Gateway_Rep;
            if (toDate != DateTime.MinValue)
            {
                toDate = toDate.AddHours(23).AddMinutes(59).AddSeconds(59); // cuối ngày
            }
            var orderIdList = string.IsNullOrEmpty(orderIds)
                                        ? new List<string>()
                                        : orderIds.Split(',')
                                                   .Select(orderId => orderId.Trim())
                                                   .Where(orderId => !string.IsNullOrEmpty(orderId))
                                                   .ToList();
            var listUser = gateway_Rep.GetUsersPays_KeToan(orderIdList, fromDate, toDate, paymentType, 1, int.MaxValue); // Get all records
                                                                                                                         // Enhance the list to replace requestType with the exact name
            foreach (var user in listUser)
            {
                user.requestType = gateway_Rep.GetRequestType(user.paymentType, user.requestType);
            }

            // Create a list of anonymous objects with only the required fields
            var filteredListUser = listUser.Select(user => new
            {
                user.Id,
                user.RowNum,           // STT
                user.MaKH,             // Mã KH
                user.MaKH_DL,          // Mã KH Đại lý
                user.orderId,          // Mã đơn hàng
                user.Partner_TransId,        // Mã tham chiếu
                user.paymentType,      // Phương thức thanh toán
                user.paymentStatus,    // Trạng thái
                user.CreatedDate,      // Ngày lập
                user.Amount,           // Tổng tiền
                user.Note              // Ghi chú
            }).ToList();

            // Generate Excel file from listUser
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("ThanhToanOnline");

                // Load data into the worksheet
                worksheet.Cells["A1"].LoadFromCollection(filteredListUser, true);

                // Find the index of the CreatedDate and Amount columns
                int createdDateColumnIndex = 0;
                int amountColumnIndex = 0;
                for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                {
                    if (worksheet.Cells[1, col].Text == "CreatedDate")
                    {
                        createdDateColumnIndex = col;
                    }
                    if (worksheet.Cells[1, col].Text == "Amount")
                    {
                        amountColumnIndex = col;
                    }
                }

                if (createdDateColumnIndex > 0)
                {
                    worksheet.Column(createdDateColumnIndex).Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                }

                if (amountColumnIndex > 0)
                {
                    worksheet.Column(amountColumnIndex).Style.Numberformat.Format = "#,##0";
                }

                // Rename columns
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "STT";
                worksheet.Cells[1, 3].Value = "Mã KH";
                worksheet.Cells[1, 4].Value = "Mã KH Đại lý";
                worksheet.Cells[1, 5].Value = "Mã đơn hàng";
                worksheet.Cells[1, 6].Value = "Mã tham chiếu";
                worksheet.Cells[1, 7].Value = "Phương thức thanh toán";
                worksheet.Cells[1, 8].Value = "Trạng thái";
                worksheet.Cells[1, 9].Value = "Ngày lập";
                worksheet.Cells[1, 10].Value = "Tổng tiền";
                worksheet.Cells[1, 11].Value = "Ghi chú";


                // Apply styles to the header row
                using (var range = worksheet.Cells[1, 1, 1, 11])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Black);
                }

                // AutoFit columns to fit the content
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                worksheet.Column(1).Hidden = true; // Ẩn cột ID


                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string excelName = $"ThanhToanOnline-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        public IActionResult SearchCongNo(string cal_from, string cal_to, string MaKH, string searchBtn, string excelBtn)
        {

            provider = CultureInfo.InvariantCulture;
            CongNoModel result = new CongNoModel();
            CongNoRepository CongNo_Rep = _unitOfWork_Repository.CongNo_Rep;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
            DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
            //Chuyển lại thành string để truyền vào
            string dateFrom = dFrom.ToString("yyyy-MM-dd");
            string dateTo = dTo.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
            ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
            ViewBag.MaKH = MaKH;
            if (MaKH == null)
            {
                MaKH = "";
            }
            if (MaKH.Trim().Length == 7 || MaKH.Trim().Length == 8)
            {
                result = CongNo_Rep.SearchCongNo(MaKH, dateFrom, dateTo);
            }
            else
            {
                ViewBag.error = "Bạn đã nhập sai mã KH, vui lòng nhập lại";
                return View("CongNo");
            }
            if (searchBtn != null)
            {
                return View("CongNo", result);
            }
            if (excelBtn != null)
            {
                return ExportExcel(result);
            }
            else
            {
                return View("CongNo", new CongNoModel());
            }

        }
        public IActionResult ExportExcel(CongNoModel result)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);

            byte[] fileContents;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ThongKe");

            ws.Cells.Style.Font.Name = "Times New Roman";
            ws.Cells.Style.Font.Size = 12;


            ws.Cells["B1"].Value = "CHI TIẾT CÔNG NỢ TỪ NGÀY " + ViewBag.DateFrom + " - ĐẾN NGÀY " + ViewBag.DateTo;
            ws.Cells["B1"].Style.Font.Bold = true;

            ws.Cells["B2"].Value = result.MaKH + " - " + result.TenDL;
            if (result.SoDuDauNgay <= 0)
            {
                ws.Cells["B3"].Value = "Số dư đầu kì:" + "(Dương quỹ)" + result.SoDuDauNgay.ToString("#,##0").Replace(".", ",") + "VND";
            }
            else
            {
                ws.Cells["B3"].Value = "Số dư đầu kì:" + "(Âm quỹ)" + result.SoDuDauNgay.ToString("#,##0").Replace(".", ",") + "VND";
            }
            if (result.SoDuCuoiNgay <= 0)
            {
                ws.Cells["B4"].Value = "Số dư cuối kì:" + "(Dương quỹ)" + result.SoDuCuoiNgay.ToString("#,##0").Replace(".", ",") + "VND";
            }
            else
            {
                ws.Cells["B4"].Value = "Số dư cuối kì:" + "(Âm quỹ)" + result.SoDuCuoiNgay.ToString("#,##0").Replace(".", ",") + "VND";
            }

            ws.Cells["B2"].Style.Font.Bold = true;


            ws.Cells["K2"].Value = "Đơn vị tiền tệ:";
            ws.Cells["L2"].Value = "VNĐ";
            ws.Cells["L2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            ws.Cells["B1:M1"].Merge = true;
            ws.Cells["B1:M1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            int rowHeader = 6;
            ws.Cells["B" + rowHeader].Value = "STT";
            ws.Cells["C" + rowHeader].Value = "Chứng từ";
            ws.Cells["D" + rowHeader].Value = "Ngày CT";
            ws.Cells["E" + rowHeader].Value = "Ngày xuất";
            ws.Cells["F" + rowHeader].Value = "Code xuất";
            ws.Cells["G" + rowHeader].Value = "PNR";
            ws.Cells["H" + rowHeader].Value = "Diễn giải";
            ws.Cells["I" + rowHeader].Value = "Giá bán";
            ws.Cells["J" + rowHeader].Value = "CK";
            ws.Cells["K" + rowHeader].Value = "Nợ";
            ws.Cells["L" + rowHeader].Value = "Có";
            ws.Cells["M" + rowHeader].Value = "Lũy kế";
            ws.Cells["B" + rowHeader + ":M" + rowHeader].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells["B" + rowHeader + ":M" + rowHeader].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            ws.Cells["B" + rowHeader + ":M" + rowHeader].Style.Font.Bold = true;
            ws.Cells["B" + rowHeader + ":M" + rowHeader].Style.Font.Color.SetColor(Color.White);



            int rowStart = rowHeader + 2;
            ws.Cells["B" + (rowHeader + 1) + ":K" + (rowHeader + 1)].Merge = true;
            ws.Cells["B" + (rowHeader + 1)].Value = "Số dư đầu: " + result.SoDuDauNgay.ToString("#,##0").Replace(".", ",");


            int stt = 1;
            foreach (var item in result.ChiTiet)
            {
                DateTime ngayXuatDateTime = DateTime.ParseExact(item.NgayXuat, "dd/MM/yyyy", null);
                if (ngayXuatDateTime.Year.ToString() == "1900")
                {
                    item.NgayXuat = null;
                }
                ws.Cells["B" + rowStart].Value = stt;
                ws.Cells["B" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["C" + rowStart].Value = item.ChungTu;
                ws.Cells["C" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["D" + rowStart].Value = item.NgayChungTuEV;
                ws.Cells["D" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["E" + rowStart].Value = item.NgayXuat;
                ws.Cells["E" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["F" + rowStart].Value = item.Code_signin;
                ws.Cells["F" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["G" + rowStart].Value = item.PNR;
                ws.Cells["G" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["H" + rowStart].Value = item.DienGiai;
                ws.Cells["H" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["I" + rowStart].Value = item.GiaCoBan;
                ws.Cells["I" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["I" + rowStart].Style.Numberformat.Format = "#,##0";
                ws.Cells["J" + rowStart].Value = item.ChietKhau;
                ws.Cells["J" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["J" + rowStart].Style.Numberformat.Format = "#,##0";
                ws.Cells["K" + rowStart].Value = item.No;
                ws.Cells["K" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["K" + rowStart].Style.Numberformat.Format = "#,##0";
                ws.Cells["L" + rowStart].Value = item.Co;
                ws.Cells["L" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["L" + rowStart].Style.Numberformat.Format = "#,##0";
                ws.Cells["M" + rowStart].Value = item.LuyKe;
                ws.Cells["M" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["M" + rowStart].Style.Numberformat.Format = "#,##0";
                rowStart++;
                stt++;
            }



            ws.Cells["B" + rowStart + ":H" + rowStart].Merge = true;
            ws.Cells["B" + rowStart].Value = "Tổng:";
            ws.Cells["B" + rowStart].Style.Font.Color.SetColor(Color.Red);
            ws.Cells["B" + rowStart + ":H" + (rowStart + 1)].Style.Font.Bold = true;
            ws.Cells["I" + rowStart].Value = result.ChiTiet.Sum(x => x.GiaCoBan);
            ws.Cells["I" + rowStart].Style.Numberformat.Format = "#,##0";
            ws.Cells["J" + rowStart].Value = result.ChiTiet.Sum(x => x.ChietKhau);
            ws.Cells["J" + rowStart].Style.Numberformat.Format = "#,##0";
            ws.Cells["K" + rowStart].Value = result.ChiTiet.Sum(x => x.No);
            ws.Cells["K" + rowStart].Style.Numberformat.Format = "#,##0";
            ws.Cells["L" + rowStart].Value = result.ChiTiet.Sum(x => x.Co);
            ws.Cells["L" + rowStart].Style.Numberformat.Format = "#,##0";

            ws.Cells["B" + (rowStart + 1) + ":K" + (rowStart + 1)].Merge = true;
            ws.Cells["B" + (rowStart + 1)].Value = "Số dư cuối: " + result.SoDuCuoiNgay.ToString("#,##0").Replace(".", ",");
            ws.Cells["B" + rowHeader + ":M" + (rowStart + 1)].AutoFitColumns();
            ws.Column(8).Width = 100;
            ws.Cells["B" + rowHeader + ":M" + (rowStart + 1)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            ws.Cells["B" + rowHeader + ":B" + (rowStart + 1)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            ws.Cells["O" + rowHeader + ":O" + (rowStart + 1)].Style.WrapText = true;
            ws.Cells["B" + (rowStart + 1) + ":K" + (rowStart + 1)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            ws.Cells["B" + (rowHeader + 1) + ":K" + (rowHeader + 1)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;


            fileContents = pck.GetAsByteArray();
            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "ThongKeCongNo_" + ViewBag.DateFrom.Replace("/", "-") + "_" + ViewBag.DateFrom.Replace("/", "-") + ".xls"
            );
        }
        public IActionResult BaoCaoSMS(string cal_from, string cal_to, int? page = 1, int pageSize = 50, string TINHTRANG = "", string NGANHANG = "", string SOTIEN = "0")
        {
            int pageNumber = page ?? 1;
            BienDongSoDuRepository rep = _unitOfWork_Repository.BienDongSoDu_Rep;
            string dateFrom = "";
            string dateTo = "";
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (cal_from != null && cal_from != "")
            {
                //format lại ngày 
                DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
                DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
                //Chuyển lại thành string để truyền vào
                dateFrom = dFrom.ToString("yyyy-MM-dd");
                dateTo = dTo.ToString("yyyy-MM-dd");
                ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
                ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
                ViewBag.NganHang = NGANHANG;
                ViewBag.TinhTrang = TINHTRANG;
                ViewBag.SoTien = SOTIEN;
                ViewBag.i = "5";
                List<BienDongSoDuModel> List = rep.DanhSachBienDongSoDu(dateFrom, dateTo, TINHTRANG, NGANHANG, SOTIEN);
                var model = PagingList.Create(List, pageSize, pageNumber);
                model.Action = "BaoCaoSMS";
                model.RouteValue = new RouteValueDictionary {
                        {
                            "i", 5
                        },

                          {
                             "cal_from",cal_from
                          },
                          {
                                 "cal_to",cal_to
                          },
                          {
                              "TINHTRANG",TINHTRANG
                          },
                            {
                                 "NGANHANG",NGANHANG
                            }
               };
                return View(model);
            }
            else
            {
                dateFrom = DateTime.Now.ToString("yyyy-MM-dd");
                dateTo = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.DateFrom = DateTime.Now.ToString("dd/MM/yyyy");
                ViewBag.DateTo = DateTime.Now.ToString("dd/MM/yyyy");
                ViewBag.NganHang = "";
                ViewBag.TinhTrang = "";
                return View();
            }
        }
        //[HttpPost]
        //public IActionResult BaoCaoSMS(string cal_from, string cal_to, int? page = 1, int pageSize = 50, string TINHTRANG = "", string NGANHANG = "")
        //{
        //    provider = CultureInfo.InvariantCulture;
        //    BienDongSoDuRepository rep = new BienDongSoDuRepository();
        //    //format lại ngày 
        //    DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
        //    DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
        //    //Chuyển lại thành string để truyền vào
        //    string dateFrom = dFrom.ToString("yyyy-MM-dd");
        //    string dateTo = dTo.ToString("yyyy-MM-dd");
        //    ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
        //    ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
        //    List<BienDongSoDuModel> List = rep.DanhSachBienDongSoDu(dateFrom, dateTo,TINHTRANG,NGANHANG);
        //    int pageNumber = (page ?? 1);
        //    var model = PagingList.Create(List, pageSize, pageNumber);
        //    model.Action = "BaoCaoSMS";
        //    model.RouteValue = new RouteValueDictionary {
        //                { "i", 5}
        //            };
        //    return View(model);
        //}
        public async Task<IActionResult> KhoaCodeDaiLy(string IDtxt, string MaKHtxt, string tenDLtxt, string noiDungKhoatxt, string IDNoiDungKhoa, string TinhTrangKhoa, string Email, string SoDT, string MailCC, string searchBtn, string newBtn, string saveBtn, string editBtn, string searchPBLBtn, string delBtn)
        {
            string MaPB = "KT";
            KhoaCodeDaiLyRepository khoacodedaily_Rep = _unitOfWork_Repository.KhoaCodeDaiLy_Rep;
            NotifyRepository notify_Rep = _unitOfWork_Repository.Notify_Rep;
            KhoaCodeDaiLyModel result = new KhoaCodeDaiLyModel();
            List<DSDaiLyModel> listDSDaiLy = new List<DSDaiLyModel>();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            //if (editBtn != null)
            //{
            //    string tenNV = acc.HoTen;
            //    bool ret = khoacodedaily_Rep.EditKCDL(MaKHtxt, tenDLtxt, noiDungKhoatxt, tenNV, IDtxt, IDNoiDungKhoa, TinhTrangKhoa,MailCC);
            //    result = khoacodedaily_Rep.DSKhoaCodeDaiLy();
            //    if (ret == true)
            //    {
            //        TempData["thongbao"] = "Khóa code thành công";
            //    }
            //    else TempData["thongbao"] = "Khóa code không thành công";
            //    return View("KhoaCodeDaiLy", result);
            //}
            //if (delBtn != null)
            //{
            //    string tenNV = acc.HoTen;
            //    bool ret = khoacodedaily_Rep.DelKCDL(IDtxt, tenNV);
            //    result = khoacodedaily_Rep.DSKhoaCodeDaiLy();
            //    if (ret == true)
            //    {
            //        TempData["thongbao"] = "Xóa thành công";
            //    }
            //    else TempData["thongbao"] = "Xóa không thành công";
            //    return View("KhoaCodeDaiLy", result);
            //}
            if (searchBtn != null)
            {
                listDSDaiLy = khoacodedaily_Rep.DSDaiLy(MaKHtxt);
                result.DSDaiLy = listDSDaiLy;
                result.DSKhoaCodeDaiLy = khoacodedaily_Rep.DSThongBaoDaiLy(MaPB).DSKhoaCodeDaiLy;
                result.DSTinhTrangKhoa = khoacodedaily_Rep.DSTinhTrangKhoa(MaPB);
                return View("KhoaCodeDaiLy", result);
            }
            if (saveBtn != null)
            {
                string tenNV = acc.HoTen;
                string MaNVLap = acc.MaNV;
                bool ret = khoacodedaily_Rep.SaveTBDL(MaKHtxt, tenDLtxt, noiDungKhoatxt, MaNVLap, tenNV, IDNoiDungKhoa, TinhTrangKhoa, MailCC, Email, SoDT, MaPB);
                result = khoacodedaily_Rep.DSThongBaoDaiLy(MaPB);
                if (ret == true)
                {
                    //bool result_SendMail = khoacodedaily_Rep.SendMailKC(MaKHtxt, tenDLtxt, noiDungKhoatxt, MailCC, Email, IDNoiDungKhoa);

                    string Title = notify_Rep.GetNotifyTitle(IDNoiDungKhoa);
                    // Đoạn này dùng để bỏ hết thẻ HTML
                    var Content = notify_Rep.RemoveHtmlTags(noiDungKhoatxt);

                    List<Member> memberResult = notify_Rep.Chitietmember(MaKHtxt);
                    //var kinhDoanhMember = memberResult.FirstOrDefault(member => member.ListKD.Any(kd => kd.Select == "selected"));
                    //var keToanMember = memberResult.FirstOrDefault(member => member.ListKt.Any(kd => kd.Select == "selected"));

                    string MaNVKinhDoanh = "";
                    string MaNVKeToan = "";
                    //if (kinhDoanhMember != null)
                    //{
                    //    var selectedKinhDoanhRowIDs = kinhDoanhMember.ListKD
                    //        .FirstOrDefault(kd => kd.Select == "selected");
                    //    MaNVKinhDoanh = selectedKinhDoanhRowIDs.RowID;
                    //}
                    //if (keToanMember != null)
                    //{
                    //    var selectedKeToanRowIDs = keToanMember.ListKt
                    //       .FirstOrDefault(kd => kd.Select == "selected");
                    //    MaNVKeToan = selectedKeToanRowIDs.RowID;
                    //}

                    if (MaNVKinhDoanh != null && MaNVKinhDoanh != "")
                    {
                        MaNVKinhDoanh = notify_Rep.GetYahooID(MaNVKinhDoanh);
                        
                    }
                    if (MaNVKeToan != null && MaNVKeToan != "")
                    {
                        MaNVKeToan = notify_Rep.GetYahooID(MaNVKeToan);
                       
                    }
                    //var requestDaiLy = new NotifyLisaAgentCodeRequest("[KẾ TOÁN] " + Title, Content, "NOTIFICATION", MaKHtxt, "");

                    //bool result_SendNotifyDaiLy = await _notifyService.SendNotify(requestDaiLy);
                   
                    TempData["thongbaoSuccess"] = "Đã gửi thông báo đến đại lý";
                    
                }
                else TempData["thongbaoError"] = "Gửi thông báo thất bại";
                return View("KhoaCodeDaiLy", result);
            }
            //if (searchPBLBtn != null)
            //{
            //    result = khoacodedaily_Rep.SearchKCDL(MaKHtxt);
            //    return View("KhoaCodeDaiLy", result);
            //}
            result = khoacodedaily_Rep.DSThongBaoDaiLy(MaPB);
            return View("KhoaCodeDaiLy", result);
        }
        [HttpPost]
        public JsonResult GetTieuDe(int ID)
        {
            List<TieuDeModel> result = _unitOfWork_Repository.KhoaCodeDaiLy_Rep.GetTieuDe(ID);
            return Json(result);
        }
        [HttpGet]
        public JsonResult GetNoiDung(int ID)
        {
            string result = _unitOfWork_Repository.KhoaCodeDaiLy_Rep.GetNoiDung(ID);
            return Json(result);
        }
        public IActionResult ImportDoanhSo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ImportDoanhSo(IFormFile file, string thang, string nam, string Import, string Save)
        {
            List<ImportDoanhSoViewModel> listDoanhSo = new List<ImportDoanhSoViewModel>();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);

            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            try
            {
                if (file != null)
                {
                    if (Import != null)
                    {
                        int Check = 1;
                        if (!file.FileName.Contains("DoanhSoDaiLy"))
                        {
                            TempData["thongbaoError"] = "File định dạng không đúng !";
                            return View();
                        }
                        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                        string folderName = "UploadFile";
                        string webRootPath = _hostingEnvironment.WebRootPath;
                        string newPath = Path.Combine(webRootPath, folderName);
                        if (file.Length > 0)
                        {
                            //string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                            string fullPath = Path.Combine(newPath, file.FileName);
                            ViewBag.filename = file.FileName;
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                                stream.Flush();
                            }
                            listDoanhSo = ImportDoanhSo_Rep.GetListDoanhSo(fullPath, thang, nam);
                            bool result = ImportDoanhSo_Rep.CheckUploadDoanhSo(listDoanhSo);

                            if (result == false)
                            {
                                TempData["thongbaoError"] = "Tháng và năm bạn vừa cập nhật đã tồn tại? Nếu bạn cập nhập sai tháng và muốn xóa dữ liệu cũ cập nhật dữ liệu mới thì bấm Cập Nhật, nếu bạn đã chọn tháng hoặc năm sai thì chọn lại file, tháng và năm bấm Xem Trước";
                                Check = 0;
                            }
                            HttpContext.Session.SetInt32("Check", Check);
                            byte[] bytes = ObjectToByteArray(listDoanhSo);
                            HttpContext.Session.Set("listDoanhSo", bytes);
                            return View(listDoanhSo);
                        }
                    }
                }
                if (Save != null)
                {
                    int? Check = HttpContext.Session.GetInt32("Check");
                    byte[] bytes = HttpContext.Session.Get("listDoanhSo");
                    if (bytes != null)
                    {
                        object obj = ByteArrayToObject(bytes);
                        listDoanhSo = (List<ImportDoanhSoViewModel>)obj;
                        if (listDoanhSo != null)
                        {
                            if (listDoanhSo.Count > 0)
                            {
                                if (Check == 1)
                                {
                                    bool ret = InsertDoanhSo(listDoanhSo);
                                    if (ret == true)
                                    {
                                        TempData["thongbaoSuccess"] = "Lưu File thành công !";
                                    }
                                    else
                                    {
                                        TempData["thongbaoError"] = "Lưu File không thành công !";
                                    }
                                }
                                else
                                {
                                    bool ret = UpdateDoanhSo(listDoanhSo);
                                    if (ret == true)
                                    {
                                        TempData["thongbaoSuccess"] = "Lưu File thành công !";
                                    }
                                    else
                                    {
                                        TempData["thongbaoError"] = "Lưu File không thành công !";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        TempData["thongbaoError"] = "Bạn phải Import File trước !";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        public bool InsertDoanhSo(List<ImportDoanhSoViewModel> listDoanhSo)
        {
            bool ret = false;
            if (listDoanhSo != null)
            {
                if (listDoanhSo.Count > 0)
                {
                    ret = ImportDoanhSo_Rep.InsertDoanhSo(listDoanhSo);
                }
            }
            return ret;
        }
        public bool UpdateDoanhSo(List<ImportDoanhSoViewModel> listDoanhSo)
        {
            bool ret = false;
            if (listDoanhSo != null)
            {
                if (listDoanhSo.Count > 0)
                {
                    ret = ImportDoanhSo_Rep.UpdateDoanhSo(listDoanhSo);
                }
            }
            return ret;
        }
        // Convert a Object to an byte array
        private byte[] ObjectToByteArray(List<ImportDoanhSoViewModel> subjects)
        {
            byte[] bytes = null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, subjects);
                bytes = ms.ToArray();
            }
            return bytes;
        }
        // Convert a byte array to an Object
        private object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = binForm.Deserialize(memStream);

            return obj;
        }
        public IActionResult TraCuuDoanhSo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult TraCuuDoanhSo(string Thang, string Nam, string MaKH, string Search, string Export, int? page = 1, int pageSize = 50)
        {
            List<ImportDoanhSoViewModel> listDoanhSo = new List<ImportDoanhSoViewModel>();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (Search != null)
            {
                HttpContext.Session.Clear();
                int pageNumber = page ?? 1;
                if (Thang != null)
                {
                    HttpContext.Session.SetString("Thang", Thang);
                }
                if (MaKH != null)
                {
                    HttpContext.Session.SetString("MaKH", MaKH);
                }
                HttpContext.Session.SetString("Nam", Nam);
                if (Thang == null && MaKH == null)
                {
                    TempData["thongbaoError"] = "Bạn phải nhập Tháng hoặc Mã KH !";
                    return View();
                }
                listDoanhSo = ImportDoanhSo_Rep.TraCuuDoanhSo(Thang, Nam, MaKH);
                byte[] bytes = ObjectToByteArray(listDoanhSo);
                HttpContext.Session.Set("listDoanhSo", bytes);
                var model = PagingList.Create(listDoanhSo, pageSize, pageNumber);
                model.Action = "PhanTrangDoanhSo";
                model.RouteValue = new RouteValueDictionary {
                        { "i", 5}
                    };
                return View("TraCuuDoanhSo", model);
            }
            if (Export != null)
            {
                byte[] bytes = HttpContext.Session.Get("listDoanhSo");
                if (bytes != null)
                {
                    object obj = ByteArrayToObject(bytes);
                    listDoanhSo = (List<ImportDoanhSoViewModel>)obj;
                    if (listDoanhSo != null)
                    {
                        if (listDoanhSo.Count > 0)
                        {
                            return ExportExcel(listDoanhSo);
                        }
                    }
                }
                else
                {
                    TempData["thongbaoError"] = "Bạn phải tra cứu doanh số trước !";
                }
            }
            return View();
        }
        public IActionResult PhanTrangDoanhSo(int? page = 1, int pageSize = 50)
        {
            string Thang = HttpContext.Session.GetString("Thang");
            string Nam = HttpContext.Session.GetString("Nam");
            string MaKH = HttpContext.Session.GetString("MaKH");
            int pageNumber = page ?? 1;

            List<ImportDoanhSoViewModel> listDoanhSo = new List<ImportDoanhSoViewModel>();
            listDoanhSo = ImportDoanhSo_Rep.TraCuuDoanhSo(Thang, Nam, MaKH);
            var model = PagingList.Create(listDoanhSo, pageSize, pageNumber);
            model.Action = "PhanTrangDoanhSo";
            model.RouteValue = new RouteValueDictionary {
                        { "i", 5}
                    };
            return View("TraCuuDoanhSo", model);
        }
        public IActionResult ExportExcel(List<ImportDoanhSoViewModel> model)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);

            byte[] fileContents;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ThongKe");

            ws.Cells.Style.Font.Name = "Times New Roman";
            ws.Cells.Style.Font.Size = 12;


            ws.Cells["B1"].Value = "DOANH SỐ ĐẠI LÝ";
            ws.Cells["B1"].Style.Font.Bold = true;

            ws.Cells["B1:K1"].Merge = true;
            ws.Cells["B1:K1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            int rowHeader = 3;
            ws.Cells["B" + rowHeader].Value = "STT";
            ws.Cells["C" + rowHeader].Value = "Tháng";
            ws.Cells["D" + rowHeader].Value = "Mã KH";
            ws.Cells["E" + rowHeader].Value = "Tổng";
            ws.Cells["F" + rowHeader].Value = "VN";
            ws.Cells["G" + rowHeader].Value = "VJ";
            ws.Cells["H" + rowHeader].Value = "QH";
            ws.Cells["I" + rowHeader].Value = "VU";
            ws.Cells["J" + rowHeader].Value = "IATA";
            ws.Cells["K" + rowHeader].Value = "Khác";

            ws.Cells["B" + rowHeader + ":K" + rowHeader].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells["B" + rowHeader + ":K" + rowHeader].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            ws.Cells["B" + rowHeader + ":K" + rowHeader].Style.Font.Bold = true;
            ws.Cells["B" + rowHeader + ":K" + rowHeader].Style.Font.Color.SetColor(Color.White);

            int rowStart = rowHeader + 1;
            int stt = 1;
            foreach (var item in model)
            {
                ws.Cells["B" + rowStart].Value = stt;
                ws.Cells["B" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                ws.Cells["C" + rowStart].Value = item.Thang + "/" + item.Nam;
                ws.Cells["C" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                ws.Cells["D" + rowStart].Value = item.MaKH;
                ws.Cells["D" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                ws.Cells["E" + rowStart].Value = item.Tong;
                ws.Cells["E" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["E" + rowStart].Style.Numberformat.Format = "#,##0";

                ws.Cells["F" + rowStart].Value = item.VN;
                ws.Cells["F" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["F" + rowStart].Style.Numberformat.Format = "#,##0";

                ws.Cells["G" + rowStart].Value = item.VJ;
                ws.Cells["G" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["G" + rowStart].Style.Numberformat.Format = "#,##0";

                ws.Cells["H" + rowStart].Value = item.QH;
                ws.Cells["H" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["H" + rowStart].Style.Numberformat.Format = "#,##0";

                ws.Cells["I" + rowStart].Value = item.VU;
                ws.Cells["I" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["I" + rowStart].Style.Numberformat.Format = "#,##0";

                ws.Cells["J" + rowStart].Value = item.IATA;
                ws.Cells["J" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["J" + rowStart].Style.Numberformat.Format = "#,##0";

                ws.Cells["K" + rowStart].Value = item.Khac;
                ws.Cells["K" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["K" + rowStart].Style.Numberformat.Format = "#,##0";

                rowStart++;
                stt++;
            }

            ws.Cells["B" + rowHeader + ":K" + rowStart].AutoFitColumns();
            //ws.Column(9).Width = 100;
            ws.Cells["B" + rowHeader + ":K" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
            ws.Cells["B" + rowHeader + ":B" + rowStart].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            ws.Cells["O" + rowHeader + ":O" + rowStart].Style.WrapText = true;
            //ws.Cells["B" + (rowStart + 1) + ":J" + (rowStart + 1)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            //ws.Cells["B" + (rowHeader + 1) + ":J" + (rowHeader + 1)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

            fileContents = pck.GetAsByteArray();
            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                //fileDownloadName: "ThongKeCongNo_" + ViewBag.DateFrom.Replace("/", "-") + "_" + ViewBag.DateFrom.Replace("/", "-") + ".xls"
                fileDownloadName: "DoanhSoDaiLy.xlsx"
            );
        }
        public IActionResult CongNoDaiLy()
        {

            //List<CongNoViewModel> result = new List<CongNoViewModel>();
            //AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            ////Hết token
            //if (acc.Email == null)
            //{
            //    return RedirectToAction("Index", "Login");
            //}
            //string thang = DateTime.Now.Year.ToString();
            //string nam = DateTime.Now.Year.ToString();
            //ViewBag.Thang = thang;
            //ViewBag.Nam = nam;

            ////result = congno_Rep.LayCongNoDaiLy("KH56779", nam);
            //result = ImportDoanhSo_Rep.LayCongNoDaiLy(acc.MaNV, thang, nam);

            return View();
        }
        [HttpPost]

        public IActionResult CongNoDaiLy(string thang, string nam)
        {

            List<CongNoViewModel> result = new List<CongNoViewModel>();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Nam = nam;
            ViewBag.Thang = thang;

            // Lay danh sach cong no
            result = ImportDoanhSo_Rep.LayCongNoDaiLy(acc.MaNV, thang, nam);
            if (result != null)
            {
                //foreach (var congNo in result)
                //{
                //    UpdateFile(congNo.MaKH, congNo.RowID);
                //}
                TempData["thongbaoSuccess"] = "Cập nhật thành công";
            }
            else
            {
                TempData["thongbaoError"] = "Không có dữ liệu cập nhật";

            }
            //TempData["thongbao"] = "Cập nhật thành công";
            return View("CongNoDaiLy", result);
        }
        //[HttpPost]
        //public IActionResult UpdateFile(string maKH, int rowID)
        //{

        //    HtmlToPdfConverter converter = new HtmlToPdfConverter();
        //    WebKitConverterSettings settings = new WebKitConverterSettings();
        //    settings.WebKitPath = Path.Combine(_hostingEnvironment.ContentRootPath, "QtBinariesWindows");
        //    converter.ConverterSettings = settings;
        //    string url = Request.Scheme + "://" + Request.Host.Value.ToString() + Request.PathBase.Value.ToString();

        //    PdfDocument document = converter.Convert(url + "/KeToan/ChiTietCongNoInPdf?ConfirmID=" + rowID + "&MaKH=" + maKH);

        //    MemoryStream ms = new MemoryStream();
        //    document.Save(ms);
        //    document.Close(true);
        //    ms.Position = 0;
        //    FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");

        //    var dir = _hostingEnvironment.WebRootPath;
        //    var path = Path.Combine(dir, "UploadFile");

        //    var tenFile = ImportDoanhSo_Rep.GetTenFile(rowID);
        //    var fileName = path + "\\" + tenFile;
        //    ////add time to avoid the duplicate

        //    using (var fs = new FileStream(fileName, FileMode.Create))
        //    {
        //        fileStreamResult.FileStream.CopyTo(fs);
        //    }
        //    _unitOfWork_Repository.ImportDoanhSo_Rep.UpdateTinhTrang(rowID, tenFile);
        //    return View();
        //}
        public IActionResult ChiTietCongNoInPdf(int ConfirmID, string MaKH)
        {

            if (MaKH == null)
            {
                MaKH = "";
            }
            CongNoViewModel result = new CongNoViewModel();
            result = ImportDoanhSo_Rep.LayChiTietCongNo(ConfirmID);

            return View("ChiTietCongNoInPdf", result);
        }
        public IActionResult DanhSach()
        {

            //Lấy thông tin Account
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Email == null)
            {
                return RedirectToAction("Index", "Login", new { area = AreaNameConst.AREA_Login });
            }
            provider = CultureInfo.InvariantCulture;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DanhSach(string submitBtn, string from_date, string to_date, int Status, string SoHD, string Pattern, string Serial, string TicketNumber, string RequestCode, string IkeySearch, string MaKH)
        {

            //Lấy thông tin Account
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            try
            {
                if (acc.Email == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                provider = CultureInfo.InvariantCulture;

                DanhSachHDDTResponse DS_HoaDon = new DanhSachHDDTResponse();
                if (submitBtn == "btn_TimDanhSach")
                {
                    if (MaKH == null)
                    {
                        TempData["thongbaoError"] = "Bạn phải nhập Mã KH !";
                        return View();
                    }
                    else
                    {
                        //format lại ngày 
                        DateTime dFrom = DateTime.ParseExact(from_date, "dd/MM/yyyy", provider, DateTimeStyles.None);
                        DateTime dTo = DateTime.ParseExact(to_date, "dd/MM/yyyy", provider, DateTimeStyles.None);
                        //Chuyển lại thành string để truyền vào
                        string dateFrom = dFrom.ToString("yyyy-MM-dd");
                        string dateTo = dTo.ToString("yyyy-MM-dd");
                        ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
                        ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
                        ViewBag.TicketNumber = TicketNumber;
                        ViewBag.RequestCode = RequestCode;
                        ViewBag.IkeySearch = IkeySearch;
                        @ViewBag.MaKH = MaKH;
                        DanhSachHDDTRequest request = new DanhSachHDDTRequest()
                        {

                            AgentId = MaKH,
                            FromDate = dateFrom,
                            ToDate = dateTo,
                            Status = Status,
                            TicketNumber = TicketNumber,
                            RequestCode = RequestCode,
                            IKey = IkeySearch

                        };

                        DS_HoaDon = await _invoiceService.DanhSachHDDT(request);
                        //Lấy ikey
                        string[] ikeys = new string[DS_HoaDon.Result.Count];
                        for (int i = 0; i < DS_HoaDon.Result.Count; i++)
                        {
                            string MaYeuCau = await _invoiceService.GetMaYeuCau(DS_HoaDon.Result[i].tongQuat.iKey.ToString());
                            ikeys[i] = DS_HoaDon.Result[i].tongQuat.iKey.ToString();
                            DS_HoaDon.Result[i].tongQuat.NGAYLAP = DS_HoaDon.Result[i].tongQuat.NGAYCT;
                            DS_HoaDon.Result[i].tongQuat.MaYeuCau = MaYeuCau;
                        }

                        byte[] bytes = convert.ObjectToByteArray(DS_HoaDon);
                        HttpContext.Session.Set("DSHD", bytes);

                        return View(DS_HoaDon);

                    }
                }
                if (submitBtn == "btn_InPDF")
                {

                    return await InHoaDon(SoHD, Pattern);

                }


                if (submitBtn == "btn_InXML")
                {

                    return await InHoaDonXML(SoHD, Pattern);

                }

                return View("~/Views/Ketoan/DanhSach.cshtml", DS_HoaDon);
            }
            catch (Exception ex)
            {
                uilti.LogErrorData(acc.MaNV, "POST", "DanhSachHoaDon", ex.Message);
                throw;
            }



        }
        [HttpPost]
        public async Task<IActionResult> InHoaDon(string ikey, string pattern)
        {
            //Lấy thông tin Account
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);

            try
            {
                InHDDTRequest requestIKey = new InHDDTRequest()
                {
                    UserName = acc.Email,
                    Ikey = ikey,
                    Pattern = pattern,
                    Type = 0


                };

                ReturnObject returnObject = new ReturnObject();
                returnObject = await _invoiceService.HDDTIn(requestIKey);

                byte[] bytes = Convert.FromBase64String(returnObject.Result.Replace("\"", ""));


                return File(
                   fileContents: bytes,
                   contentType: "application/pdf",
                   fileDownloadName: "HDDT" + ikey + "_" + pattern + ".pdf"
              );
            }
            catch (Exception ex)
            {
                uilti.LogErrorData(acc.MaNV, "POST", "InHoaDon", ex.Message);
                throw;
            }
        }
        public async Task<IActionResult> InHoaDonXML(string ikey, string pattern)
        {
            //Lấy thông tin Account
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);

            try
            {
                InHDDTRequest requestIKey = new InHDDTRequest()
                {
                    UserName = acc.Email,
                    Ikey = ikey,
                    Pattern = pattern,
                    Type = 1


                };

                ReturnObject returnObject = new ReturnObject();
                returnObject = await _invoiceService.HDDTIn(requestIKey);

                byte[] bytes = Convert.FromBase64String(returnObject.Result.Replace("\"", ""));


                return File(
                   fileContents: bytes,
                   contentType: "application/xml",
                   fileDownloadName: "HDDT" + ikey + "_" + pattern + ".xml"
              );
            }
            catch (Exception ex)
            {
                uilti.LogErrorData(acc.MaNV, "POST", "InHoaDonXML", ex.Message);
                throw;
            }




        }
        [HttpGet]
        public async Task<JsonResult> LayDanhSachSoVeHDDT(string ikey, string pattern, string serial, string maKH)
        {
            //Lấy thông tin Account
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            try
            {
                DanhSachHDDTResponse DS_HoaDon = new DanhSachHDDTResponse();

                DanhSachHDDTRequest request = new DanhSachHDDTRequest()
                {

                    AgentId = maKH,
                    FromDate = "1900-01-01",
                    ToDate = "1900-01-01",

                    IKey = ikey


                };

                DS_HoaDon = await _invoiceService.DanhSachHDDT(request);



                List<EInvoice_CT> ChiTietHDDT = new List<EInvoice_CT>();
                ChiTietHDDT = await _invoiceService.DanhSachVeHDDT(DS_HoaDon, ikey, pattern, serial);
                return Json(ChiTietHDDT);
            }
            catch (Exception ex)
            {
                uilti.LogErrorData(acc.MaNV, "POST", "LayDanhSachSoVeHDDT", ex.Message);
                throw;
            }

        }
        [HttpPost]
        public async Task<JsonResult> XemHoaDon(string ikey, string pattern)
        {
            //Lấy thông tin Account
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            try
            {
                InHDDTRequest requestIKey = new InHDDTRequest()
                {
                    UserName = acc.HoTen,
                    Ikey = ikey,
                    Pattern = pattern,
                    Type = 0


                };

                ReturnObject returnObject = new ReturnObject();
                returnObject = await _invoiceService.HDDTIn(requestIKey);

                byte[] bytes = Convert.FromBase64String(returnObject.Result.Replace("\"", ""));


                return Json(bytes);

            }
            catch (Exception ex)
            {
                uilti.LogErrorData(acc.MaNV, "POST", "XemHoaDon", ex.Message);
                throw;
            }



        }
        [HttpPost]
        public async Task<JsonResult> CancelHoaDon(string ikey, string serial, string pattern)
        {
            //Lấy thông tin Account
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            try
            {
                DeleteCancelHDDTRequest requestIKey = new DeleteCancelHDDTRequest()
                {
                    UserName = acc.HoTen,
                    Ikey = ikey,
                    Pattern = pattern,
                    Serial = serial


                };

                BaseResponse returnObject = new BaseResponse();
                returnObject = await _invoiceService.HDDTCancel(requestIKey);

                return Json(returnObject);
            }
            catch (Exception ex)
            {
                uilti.LogErrorData(acc.MaNV, "POST", "CancelHoaDon", ex.Message);
                throw;
            }

        }
        public IActionResult UpdateNgayChungTu()
        {

            //Lấy thông tin Account
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            provider = CultureInfo.InvariantCulture;
            return View();
        }

        [HttpPost]
        public IActionResult UpdateNgayChungTu(string from_date)
        {

            //Lấy thông tin Account
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Email == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Task<bool> result = _invoiceService.UpdateNgayChungTu(from_date);
            if (result.Result == true)
            {
                ViewBag.Message = "Update thành công";
            }
            else
            {
                ViewBag.Message = "Update thất bại";
            }
            return View();
        }

        [HttpPost]
        public JsonResult UpdateEFF(string MaCK)
        {
            //GuiMailDaiLyRepository rep_daily = _unitOfWork_Repository.GuiMail_DaiLy_Rep;
            BienDongSoDuRepository rep = _unitOfWork_Repository.BienDongSoDu_Rep;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (!rep.KiemTraCKDataEFF(MaCK))
            {
                string info = "TrungData";
                return Json(info);
            }

            string server_KT_NH = _configuration.GetConnectionString("SQL_KT_MAIN");
            string server = _configuration.GetConnectionString("SQL_EV_MAIN");

            Task<bool> result = rep.CapNhatDataEFF(MaCK, server, acc.HoTen, server_KT_NH);
            return Json(result.Result);
        }
        public IActionResult DanhSachVeSot()
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            GuiMailDaiLyRepository guimail_Rep = _unitOfWork_Repository.GuiMail_DaiLy_Rep;
            ViewBag.NOIDUNG = guimail_Rep.NoiDungLuuY();
            string dateFrom = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTo = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.DateTo = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.SoVe = "";
            TongQuatMail tongQuat = new TongQuatMail();
            //tongQuat.ListChiTietVe = guimail_Rep.ListChiTietVe(acc.MaNV, dateFrom, dateTo, "");
            return View(tongQuat);
        }
        [HttpPost]
        public IActionResult DanhSachVeSot(string MAKH_2, string cal_from, string cal_to, string SoVeSearch)
        {
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            string server_EV = _configuration.GetConnectionString("SQL_EV_MAIN");
            string server_EV_V2 = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            acc = AccountManager.GetAccountCurrent(HttpContext);
            GuiMailDaiLyRepository guimail_Rep = _unitOfWork_Repository.GuiMail_DaiLy_Rep;
            TongQuatMail result = new TongQuatMail();
            ViewBag.NOIDUNG = guimail_Rep.NoiDungLuuY();
            string dateFrom = "";
            string dateTo = "";
            //format lại ngày 
            if (cal_from != null)
            {
                DateTime dFrom = DateTime.ParseExact(cal_from, "dd/MM/yyyy", provider, DateTimeStyles.None);
                dateFrom = dFrom.ToString("yyyy-MM-dd");
                ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
            }
            if (cal_to != null)
            {
                DateTime dTo = DateTime.ParseExact(cal_to, "dd/MM/yyyy", provider, DateTimeStyles.None);
                dateTo = dTo.ToString("yyyy-MM-dd");
                ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
            }
            ViewBag.SoVe = SoVeSearch;
            result.ListChiTietVe = guimail_Rep.ListChiTietVe(acc.MaNV, dateFrom, dateTo, SoVeSearch, server_EV, server_KH_KT, "", server_EV_V2);
            return View(result);
        }
        [HttpPost]
        public IActionResult ChiTietVeSot(ChiTietVeSot DataDetail)
        {
            string Server = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            GuiMailDaiLyRepository rep = _unitOfWork_Repository.GuiMail_DaiLy_Rep;
            DataDetail.ListLoaiPhi = rep.ListPhiXuat(Server);
            for (int i = 0; i < DataDetail.ListLoaiPhi.Count; i++)
            {
                if (DataDetail.ListLoaiPhi[i].Name == DataDetail.LoaiPhi)
                {
                    DataDetail.ListLoaiPhi[i].Selected = "Selected";
                }
            }
            return PartialView("UpdateVeSot", DataDetail);
        }
        [HttpPost]
        public IActionResult ChiTietVeSotVoQuy(ChiTietVeSot DataDetail)
        {
            string Server = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            GuiMailDaiLyRepository rep = _unitOfWork_Repository.GuiMail_DaiLy_Rep;
            DataDetail.ListLoaiPhi = rep.ListPhiXuat(Server);
            for (int i = 0; i < DataDetail.ListLoaiPhi.Count; i++)
            {
                if (DataDetail.ListLoaiPhi[i].Name == DataDetail.LoaiPhi)
                {
                    DataDetail.ListLoaiPhi[i].Selected = "Selected";
                }
            }
            return PartialView("UpdateVeSotVoCongNo", DataDetail);
        }
        [HttpPost]
        public JsonResult KiemTraTinhTrangEFF(string Active, int RowID)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool result = _unitOfWork_Repository.VeSot_Rep.UpdateStatusEFF(Active, RowID);
            return Json(result);
        }
        [HttpPost]
        public JsonResult SaveBaoCaoVeSot(int ID, string PNR, string Hang, string SoVe, string MaKH, string GiaMua, string PhiMua, string PhiBan, string PhiHoan, string ChietKhau, string MaGioiThieu, string NguoiGioiThieu, string LoaiPhi, string PhiXuatVe)
        {
            string server_KT = _configuration.GetConnectionString("SQL_KT_MAIN");
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            int result = _unitOfWork_Repository.VeSot_Rep.UpdateBaoCaoVeSot(server_KT, server_KH_KT, PNR, Hang, SoVe, MaKH.ToUpper(), GiaMua, PhiMua, PhiBan, PhiHoan, ChietKhau, acc.MaNV, MaGioiThieu, NguoiGioiThieu, ID, LoaiPhi, PhiXuatVe);
            return Json(result);
        }
        [HttpPost]
        public JsonResult SaveBaoCaoVeSotWebVoQuy(int ID, string PNR, string Hang, string SoVe, string MaKH, string GiaMua, string PhiMua, string PhiBan, string PhiHoan, string ChietKhau, string MaGioiThieu, string NguoiGioiThieu, string LoaiPhi, string PhiXuatVe)
        {
            string server_EV = _configuration.GetConnectionString("SQL_EV_MAIN");
            string server_KT = _configuration.GetConnectionString("SQL_KT_MAIN");
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string result = _unitOfWork_Repository.VeSot_Rep.InsertBaoCaoVeSotWebVoQuy(server_EV, server_KT, server_KH_KT, PNR, Hang, SoVe, MaKH.ToUpper(), GiaMua, PhiMua, PhiBan, PhiHoan, ChietKhau, acc.MaNV, MaGioiThieu, NguoiGioiThieu, ID, LoaiPhi, PhiXuatVe);
            return Json(result);
        }
        public IActionResult CongNoQuaHan()
        {
            //CongNoQuaHanRepository CongNoQuaHan_Rep = new CongNoQuaHanRepository();

            List<CongNoQuaHanModel> ListCongNoQuaHan = _unitOfWork_Repository.CongNoQuaHan_Rep.ListCongNoQuaHan();
            return View(ListCongNoQuaHan);
        }
        public JsonResult GetCongNoNVQuaHan(string id)
        {
            //CongNoQuaHanRepository CongNoQuaHan_Rep = new CongNoQuaHanRepository();
            CongNoQuaHanModel CongNoQuaHan = _unitOfWork_Repository.CongNoQuaHan_Rep.GetCongNoNVQuaHan(id);
            return Json(CongNoQuaHan);
        }
        [HttpPost]
        public JsonResult SaveCongNoQuaHan(string data, string TieuDe, string Thang)
        {
            string server_EV_V2 = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            List<DSCongNoNVQuaHan> ListCongNoNVQuaHan = JsonConvert.DeserializeObject<List<DSCongNoNVQuaHan>>(data);
            //CongNoQuaHanRepository CongNoQuaHan_Rep = new CongNoQuaHanRepository();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string result = _unitOfWork_Repository.CongNoQuaHan_Rep.SaveCongNoQuaHan(acc.MaNV, ListCongNoNVQuaHan, TieuDe, Thang, server_EV_V2);
            return Json(result);
        }
        public IActionResult DS_CNNVQH()
        {
            //CongNoQuaHanRepository CongNoQuaHan_Rep = new CongNoQuaHanRepository();
            List<CongNoQuaHanModel> ListCongNoQuaHan = _unitOfWork_Repository.CongNoQuaHan_Rep.ListCongNoQuaHan();
            return View(ListCongNoQuaHan);
        }
        public JsonResult UpdateCongNoQuaHan(string ID, string TieuDe, string Thang)
        {
            //CongNoQuaHanRepository CongNoQuaHan_Rep = new CongNoQuaHanRepository();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string result = _unitOfWork_Repository.CongNoQuaHan_Rep.UpdateCongNoQuaHan(acc.MaNV, ID, TieuDe, Thang);
            return Json(result);
        }
        public IActionResult DS_CNNVQH_Detail(string id)
        {
            //CongNoQuaHanRepository CongNoQuaHan_Rep = new CongNoQuaHanRepository();
            List<CongNoQuaHanModel> ListCongNoNVQuaHan = _unitOfWork_Repository.CongNoQuaHan_Rep.ListCongNoQuaHanDetail(id);
            return View(ListCongNoNVQuaHan);
        }
        [HttpPost]
        public JsonResult SaveCongNoNVQuaHan(string data)
        {
            DSCongNoNVQuaHan CongNoNVQuaHan = JsonConvert.DeserializeObject<DSCongNoNVQuaHan>(data);
            //CongNoQuaHanRepository CongNoQuaHan_Rep = new CongNoQuaHanRepository();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            string result = _unitOfWork_Repository.CongNoQuaHan_Rep.SaveCongNoNVQuaHan(acc.MaNV, CongNoNVQuaHan);
            return Json(result);
        }
        public JsonResult DelCongNoNVQuaHan(string ID)
        {
            //CongNoQuaHanRepository CongNoQuaHan_Rep = new CongNoQuaHanRepository();
            string result = _unitOfWork_Repository.CongNoQuaHan_Rep.DelCongNoNVQuaHan(ID);
            return Json(result);
        }

        public IActionResult CauHinhPhiXuat()
        {
            //ConfigPhiXuatRepository rep = new ConfigPhiXuatRepository();
            List<ConfigPhiXuatModel> result = _unitOfWork_Repository.ConfigPhiXuat_Rep.ListPhiXuat();
            return View(result);
        }
        [HttpPost]
        public async Task<JsonResult> UpdatePhiXuat(int ID, string Price, string ExchangeRate, string Amount)
        {
            //ConfigPhiXuatRepository rep = new ConfigPhiXuatRepository();
            bool result = await _unitOfWork_Repository.ConfigPhiXuat_Rep.UpdatePhiXuat(ID, Price, ExchangeRate, Amount);
            return Json(result);
        }
    }
}
