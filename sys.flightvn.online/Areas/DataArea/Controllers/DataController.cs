using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.Serialization.Formatters.Binary;


using EasyInvoice.Json;
using System.Drawing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System.Net;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Manager.DataAccess.Repository;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;

namespace Manager_EV.Areas.DataArea.Controllers
{
    [Area(AreaNameConst.AREA_Data)]
    public class DataController : Controller
    {
        //PhanViecRepository _unitOfWork_Rep.PhanViec_Rep = new PhanViecRepository();
        //KhachHangRepository _unitOfWork_Rep.KhachHang_Rep = new KhachHangRepository();
        //SanPhamDichVuRepository _unitOfWork_Rep.SPDV_Rep = new SanPhamDichVuRepository();
        //ThongBaoRepository _unitOfWork_Rep.ThongBao_Rep = new ThongBaoRepository();
        //ChuongTrinhKhuyenMaiRepository _unitOfWork_Rep.CTKM_Rep = new ChuongTrinhKhuyenMaiRepository();
        //ChuongTrinhXoSoRepository _unitOfWork_Rep.CTXS_Rep = new ChuongTrinhXoSoRepository();
        //BookerClubRepository _unitOfWork_Rep.BookerClub_Rep = new BookerClubRepository();
        private readonly IHostingEnvironment _hostingEnvironment;   
        private readonly IUnitOfWork_Repository _unitOfWork_Rep;
        private readonly IConfiguration _configuration;
        CultureInfo provider;

        public DataController(IHostingEnvironment hostingEnvironment, IConfiguration configuration, IUnitOfWork_Repository unitOfWork_Repository)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _unitOfWork_Rep = unitOfWork_Repository;        
            
        }

        public IActionResult TraCuuCongVan()
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public IActionResult TraCuuDaiLy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult TraCuuDaiLy(string DieuKien, string GiaTri, string tungay, string denngay)
        {
            List<DaiLyEV> result = new List<DaiLyEV>();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (GiaTri != null)
            {
                string dateFrom = "", dateTo = "";
                provider = CultureInfo.InvariantCulture;
                if (tungay != null && tungay != "")
                {
                    //format lại ngày 
                    DateTime dFrom = DateTime.ParseExact(tungay, "dd/MM/yyyy", provider, DateTimeStyles.None);
                    //Chuyển lại thành string để truyền vào
                    dateFrom = dFrom.ToString("yyyy-MM-dd");
                }
                if (denngay != null && denngay != "")
                {
                    //format lại ngày 
                    DateTime dTo = DateTime.ParseExact(denngay, "dd/MM/yyyy", provider, DateTimeStyles.None);
                    //Chuyển lại thành string để truyền vào
                    dateTo = dTo.ToString("yyyy-MM-dd");
                }
                result = _unitOfWork_Rep.DaiLy_Rep.SearchThongTinHD(DieuKien, GiaTri, dateFrom, dateTo);
                return View("TraCuuDaiLy", result);
            }
            else
            {
                TempData["thongbaoError"] = "Bạn phải nhập mã KH hoặc Mã NV !";
                return View("TraCuuDaiLy");
            }
        }

        public IActionResult DSCongViec()
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            CongViecModel result = new CongViecModel();
            result = _unitOfWork_Rep.PhanViec_Rep.ListNhanVien();
            return View(result);
        }

        [HttpPost]
        public IActionResult PhanViec(string khoachinh, string congviec)
        {
            NhanVienModel result = new NhanVienModel();
            result = _unitOfWork_Rep.PhanViec_Rep.PhanViec(khoachinh, congviec);
            return PartialView("CongViec", result);
        }

        [HttpPost]
        public IActionResult QuyDinh(string khoachinh, string congviec)
        {
            NhanVienModel result = new NhanVienModel();
            result = _unitOfWork_Rep.PhanViec_Rep.QuyDinh(khoachinh, congviec);
            return PartialView("CongViec", result);
        }
        [HttpPost]
        public IActionResult PhanViecChung(string khoachinh, string congviec)
        {
            NhanVienModel result = new NhanVienModel();
            result = _unitOfWork_Rep.PhanViec_Rep.PhanViecChung(khoachinh, congviec);
            return PartialView("CongViec", result);
        }

        [HttpPost]
        public IActionResult QuyDinhChung(string khoachinh, string congviec)
        {
            NhanVienModel result = new NhanVienModel();
            result = _unitOfWork_Rep.PhanViec_Rep.QuyDinhChung(khoachinh, congviec);
            return PartialView("CongViec", result);
        }

        [HttpPost]
        public IActionResult UploadCKEditor(IFormFile upload)
        {
            string folderName = "UploadImg";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);

            var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
            string fullPath = Path.Combine(newPath, filename);
            var stream = new FileStream(fullPath, FileMode.Create);
            upload.CopyToAsync(stream);
            return new JsonResult(new
            {
                uploaded = 1,
                fileName = upload.FileName,
                url = "/UploadImg/" + filename
            });
        }
        [HttpPost]
        public IActionResult UploadCKEditorDuLich(IFormFile upload)
        {
            string ftpServerUrl = "ftp://Manager.airline24h.com";
            string username = "envietDuLich";
            string password = "enviet@123";
            // Create FtpWebRequest object
            var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServerUrl + "/" + filename);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);
            // Upload the file to the FTP server
            using (Stream ftpStream = request.GetRequestStream())
            {
                upload.CopyTo(ftpStream);
            }
            string http = "https://Manager.airline24h.com/upload/dulich/" + filename;
            return new JsonResult(new
            {
                uploaded = 1,
                fileName = filename,
                url = http
            });
        }
        [HttpPost]
        public IActionResult LuuCongViec(string MaNV, string CongViec, string ThaoTac, string NoiDungChiTiet)
        {
            bool ret = _unitOfWork_Rep.PhanViec_Rep.LuuCongViec(MaNV, CongViec, ThaoTac, NoiDungChiTiet);
            CongViecModel result = new CongViecModel();
            result = _unitOfWork_Rep.PhanViec_Rep.ListNhanVien();
            return View("DSCongViec", result);
        }

        [HttpGet]
        public IEnumerable<AccountModel> ListNhanVien(string id)
        {
            IEnumerable<AccountModel> result = _unitOfWork_Rep.PhanViec_Rep.GetListNV(id);
            return result;
        }
        [HttpPost]
        public IActionResult SaveCongViecPB(string MaPB, string CongViec, string NoiDung)
        {
            bool ret = _unitOfWork_Rep.PhanViec_Rep.SaveCongViecPB(MaPB, CongViec, NoiDung);
            CongViecModel result = new CongViecModel();
            result = _unitOfWork_Rep.PhanViec_Rep.ListNhanVien();
            return View("DSCongViec", result);
        }

        public IActionResult ContentNews()
        {
            List<ArticleModel> result = _unitOfWork_Rep.Article_Rep.Article();
            return View(result);
        }
        public IActionResult CreateContentNews()
        {
            List<SideMenu_ChildModel> result = _unitOfWork_Rep.Article_Rep.SideMenuChild();
            return PartialView("CreateContentNews", result);
        }
        public IActionResult EditContentNews(int ID)
        {
            ArticleModel result = _unitOfWork_Rep.Article_Rep.EditArticle(ID);
            return PartialView("EditContentNews", result);
        }
        public IActionResult SaveCreateContentNews(string Danhmuc, string Title, string CreateContent)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Rep.Article_Rep.SaveCreate(Danhmuc, Title, CreateContent, acc.MaNV);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu bài viết thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu bài viết không thành công";

            List<ArticleModel> result = _unitOfWork_Rep.Article_Rep.Article();
            return View("ContentNews", result);
        }
        public IActionResult SaveEditContentNews(string ID, string Danhmuc, string Title, string CreateContent)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Rep.Article_Rep.SaveEdit(ID, Danhmuc, Title, CreateContent, acc.MaNV);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã chỉnh  sửa bài viết thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã chỉnh sửa bài viết không thành công";

            List<ArticleModel> result = _unitOfWork_Rep.Article_Rep.Article();
            return View("ContentNews", result);
        }
        public IActionResult CreateMenuParent()
        {
            return PartialView("CreateMenuParent");
        }
        public IActionResult CreateMenuChild()
        {
            List<SideMenu_ParentModel> result = _unitOfWork_Rep.Article_Rep.SideMenuParent();
            return PartialView("CreateMenuChild", result);
        }
        public IActionResult CreateAirport()
        {
            return PartialView("CreateAirport");
        }
        public IActionResult EditMenuParent(int ID)
        {
            SideMenu_ParentModel result = _unitOfWork_Rep.Article_Rep.EditMenuParent(ID);
            return PartialView("EditMenuParent", result);
        }
        public IActionResult EditMenuChild(int ID)
        {
            SideMenu_ChildModel result = _unitOfWork_Rep.Article_Rep.EditMenuChild(ID);
            return PartialView("EditMenuChild", result);
        }
        public IActionResult EditAirport(int ID)
        {
            Airport result = _unitOfWork_Rep.Article_Rep.EditAirport(ID);
            return PartialView("EditAirport", result);
        }
        public JsonResult StatusMenuParent(int ID, string status)
        {
            bool ret = _unitOfWork_Rep.Article_Rep.StatusMenuParent(ID, status);
            return Json(ret);
        }
        public JsonResult StatusMenuChild(int ID, string status)
        {
            bool ret = _unitOfWork_Rep.Article_Rep.StatusMenuChild(ID, status);
            return Json(ret);
        }
        public JsonResult StatusAirport(int ID, string status)
        {
            bool ret = _unitOfWork_Rep.Article_Rep.StatusAirport(ID, status);
            return Json(ret);
        }
        public IActionResult MenuNews()
        {
            SideMenuModel result = _unitOfWork_Rep.Article_Rep.SideMenu();
            return View(result);
        }
        public IActionResult SaveCreateMenuParent(string Name)
        {
            bool ret = _unitOfWork_Rep.Article_Rep.SaveCreateMenuParent(Name);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu danh mục thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu danh mục không thành công";

            SideMenuModel result = _unitOfWork_Rep.Article_Rep.SideMenu();
            return View("MenuNews", result);
        }
        public IActionResult SaveCreateMenuChild(string DanhMuc, string Name)
        {
            bool ret = _unitOfWork_Rep.Article_Rep.SaveCreateMenuChild(DanhMuc, Name);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu danh mục thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu danh mục không thành công";

            SideMenuModel result = _unitOfWork_Rep.Article_Rep.SideMenu();
            return View("MenuNews", result);
        }
        public IActionResult SaveCreateAirport(string Name)
        {
            bool ret = _unitOfWork_Rep.Article_Rep.SaveCreateAirport(Name);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu danh mục thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu danh mục không thành công";

            SideMenuModel result = _unitOfWork_Rep.Article_Rep.SideMenu();
            return View("MenuNews", result);
        }
        public IActionResult SaveEditMenuParent(int ID, string Name)
        {
            bool ret = _unitOfWork_Rep.Article_Rep.SaveEditMenuParent(ID, Name);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu danh mục thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu danh mục không thành công";

            SideMenuModel result = _unitOfWork_Rep.Article_Rep.SideMenu();
            return View("MenuNews", result);
        }
        public IActionResult SaveEditMenuChild(int ID, string Name, string Danhmuc)
        {
            bool ret = _unitOfWork_Rep.Article_Rep.SaveEditMenuChild(ID, Name, Danhmuc);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu danh mục thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu danh mục không thành công";

            SideMenuModel result = _unitOfWork_Rep.Article_Rep.SideMenu();
            return View("MenuNews", result);
        }
        public IActionResult SaveEditAirport(int ID, string Name)
        {
            bool ret = _unitOfWork_Rep.Article_Rep.SaveEditAirport(ID, Name);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu danh mục thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu danh mục không thành công";

            SideMenuModel result = _unitOfWork_Rep.Article_Rep.SideMenu();
            return View("MenuNews", result);
        }
        public IActionResult NhomKH()
        {
            List<NhomDL> resutl = _unitOfWork_Rep.KhachHang_Rep.ListNhomDL();
            return View(resutl);
        }
        public IActionResult CreateNhomDL()
        {
            return View();
        }
        public IActionResult EditNhomDL(int ID)
        {
            NhomDL resutl = _unitOfWork_Rep.KhachHang_Rep.EditNhomDL(ID);
            return View(resutl);
        }
        public IActionResult SaveCreateNhomDL(string Title, string CreateContent)
        {
            bool ret = _unitOfWork_Rep.KhachHang_Rep.SaveCreateNhomDL(Title, CreateContent);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu bài viết thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu bài viết không thành công";

            List<NhomDL> resutl = _unitOfWork_Rep.KhachHang_Rep.ListNhomDL();
            return View("NhomKH", resutl);
        }
        public IActionResult SaveEditNhomDL(int ID, string Title, string CreateContent)
        {
            bool ret = _unitOfWork_Rep.KhachHang_Rep.SaveEditNhomDL(ID, Title, CreateContent);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu bài viết thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu bài viết không thành công";

            List<NhomDL> resutl = _unitOfWork_Rep.KhachHang_Rep.ListNhomDL();
            return View("NhomKH", resutl);
        }
        public IActionResult SanPham()
        {
            List<SanPhamDichVuModel> result = _unitOfWork_Rep.SPDV_Rep.SanPham();
            return View(result);
        }
        public IActionResult CreateSanPham()
        {
            return View();
        }
        public IActionResult EditSanPham(int ID)
        {
            SanPhamDichVuModel result = _unitOfWork_Rep.SPDV_Rep.EditSanPham(ID);
            return View(result);
        }
        [HttpPost]
        public IActionResult SaveCreateSanPham(string Name, string GiaShow, string GiaDN, string MoTa, string CreateContent, IFormFile files)
        {
            string url = UploadImg(files);
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Rep.SPDV_Rep.SaveCreateSanPham(Name, GiaShow, GiaDN, MoTa, CreateContent, acc.MaNV, url);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu sản phẩm thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu sản phẩm không thành công";

            List<SanPhamDichVuModel> result = _unitOfWork_Rep.SPDV_Rep.SanPham();
            return View("SanPham", result);
        }
        public IActionResult SaveEditSanPham(int ID, string Name, string GiaShow, string GiaDN, string MoTa, string CreateContent, IFormFile files, string MainImg)
        {
            string url = "";
            if (files == null)
            {
                url = MainImg;
            }
            else
            {
                url = UploadImg(files);
            }
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Rep.SPDV_Rep.SaveEditSanPham(ID, Name, GiaShow, GiaDN, MoTa, CreateContent, acc.MaNV, url);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu sản phẩm thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu sản phẩm không thành công";

            List<SanPhamDichVuModel> result = _unitOfWork_Rep.SPDV_Rep.SanPham();
            return View("SanPham", result);
        }

        public IActionResult DichVu()
        {
            List<SanPhamDichVuModel> result = _unitOfWork_Rep.SPDV_Rep.DichVu();
            return View(result);
        }
        public IActionResult CreateDichVu()
        {
            return View();
        }
        public IActionResult EditDichVu(int ID)
        {
            SanPhamDichVuModel result = _unitOfWork_Rep.SPDV_Rep.EditDichVu(ID);
            return View(result);
        }
        public IActionResult SaveCreateDichVu(string Name, string GiaShow, string GiaDN, string MoTa, string CreateContent, IFormFile files)
        {
            string url = UploadImg(files);
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Rep.SPDV_Rep.SaveCreateDichVu(Name, GiaShow, GiaDN, MoTa, CreateContent, acc.MaNV, url);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu dịch vụ thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu dịch vụ không thành công";

            List<SanPhamDichVuModel> result = _unitOfWork_Rep.SPDV_Rep.DichVu();
            return View("DichVu", result);
        }
        public IActionResult SaveEditDichVu(int ID, string Name, string GiaShow, string GiaDN, string MoTa, string CreateContent, IFormFile files, string MainImg)
        {
            string url = "";
            if (files == null)
            {
                url = MainImg;
            }
            else
            {
                url = UploadImg(files);
            }
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Rep.SPDV_Rep.SaveEditDichVu(ID, Name, GiaShow, GiaDN, MoTa, CreateContent, acc.MaNV, url);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu dịch vụ thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu dịch vụ không thành công";

            List<SanPhamDichVuModel> result = _unitOfWork_Rep.SPDV_Rep.DichVu();
            return View("DichVu", result);
        }
        public IActionResult SanPhamChild()
        {
            List<SanPhamChild> result = _unitOfWork_Rep.SPDV_Rep.SanPhamChild();
            return View(result);
        }
        public IActionResult CreateSanPhamChild()
        {
            return View();
        }
        public IActionResult EditSanPhamChild(int ID)
        {
            SanPhamChild result = _unitOfWork_Rep.SPDV_Rep.EditSanPhamChild(ID);
            return View(result);
        }
        public IActionResult SaveCreateSanPhamChild(string Name, string GiaShow, string GiaDN, IFormFile files, string ID_Parent)
        {
            string url = UploadImg(files);
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Rep.SPDV_Rep.SaveCreateSanPhamChild(Name, GiaShow, GiaDN, acc.MaNV, url, ID_Parent);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu sản phẩm thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu sản phẩm không thành công";

            List<SanPhamChild> result = _unitOfWork_Rep.SPDV_Rep.SanPhamChild();
            return View("SanPhamChild", result);
        }
        public IActionResult SaveEditSanPhamChild(int ID, string Name, string GiaShow, string GiaDN, IFormFile files, string ID_Parent, string ChildImg)
        {
            string url = "";
            if (files == null)
            {
                url = ChildImg;
            }
            else
            {
                url = UploadImg(files);
            }
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Rep.SPDV_Rep.SaveEditSanPhamChild(ID, Name, GiaShow, GiaDN, acc.MaNV, url, ID_Parent);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu sản phẩm thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu sản phẩm không thành công";

            List<SanPhamChild> result = _unitOfWork_Rep.SPDV_Rep.SanPhamChild();
            return View("SanPhamChild", result);
        }
        public IActionResult DichVuChild()
        {
            List<SanPhamChild> result = _unitOfWork_Rep.SPDV_Rep.DichVuChild();
            return View(result);
        }
        public IActionResult CreateDichVuChild()
        {
            return View();
        }
        public IActionResult EditDichVuChild(int ID)
        {
            SanPhamChild result = _unitOfWork_Rep.SPDV_Rep.EditDichVuChild(ID);
            return View(result);
        }
        public IActionResult SaveCreateDichVuChild(string Name, string GiaShow, string GiaDN, IFormFile files, string ID_Parent)
        {
            string url = UploadImg(files);
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Rep.SPDV_Rep.SaveCreateDichVuChild(Name, GiaShow, GiaDN, acc.MaNV, url, ID_Parent);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu dịch vụ thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu dịch vụ không thành công";

            List<SanPhamChild> result = _unitOfWork_Rep.SPDV_Rep.DichVuChild();
            return View("DichVuChild", result);
        }
        public IActionResult SaveEditDichVuChild(int ID, string Name, string GiaShow, string GiaDN, IFormFile files, string ID_Parent, string ChildImg)
        {
            string url = "";
            if (files == null)
            {
                url = ChildImg;
            }
            else
            {
                url = UploadImg(files);
            }
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Rep.SPDV_Rep.SaveEditDichVuChild(ID, Name, GiaShow, GiaDN, acc.MaNV, url, ID_Parent);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu dịch vụ thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu dịch vụ không thành công";

            List<SanPhamChild> result = _unitOfWork_Rep.SPDV_Rep.DichVuChild();
            return View("DichVuChild", result);
        }
        public string UploadImg(IFormFile upload)
        {
            string url = "";
            string folderName = "UploadImg";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);

            var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
            string fullPath = Path.Combine(newPath, filename);
            var stream = new FileStream(fullPath, FileMode.Create);
            upload.CopyToAsync(stream);
            url = "/UploadImg/" + filename;
            return url;
        }
        public IActionResult ThongBao()
        {
            List<ThongBao_ALL> result = _unitOfWork_Rep.ThongBao_Rep.ThongBaoALL();
            return View(result);
        }
        public IActionResult CreateThongBao()
        {
            return PartialView("CreateThongBao");
        }
        public IActionResult SaveCreateThongBao(string Danhmuc, string Title, string CreateContent)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Rep.ThongBao_Rep.SaveCreateThongBao(Danhmuc, Title, CreateContent, acc.Ten);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu bài viết thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu bài viết không thành công";

            List<ThongBao_ALL> result = _unitOfWork_Rep.ThongBao_Rep.ThongBaoALL();
            return View("ThongBao", result);
        }
        public IActionResult EditThongBao(int ID)
        {
            ChiTietTB result = _unitOfWork_Rep.ThongBao_Rep.EditThongBao(ID);
            return PartialView("EditThongBao", result);
        }
        public IActionResult SaveEditThongBao(string Danhmuc, string Title, string CreateContent, string ID)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Rep.ThongBao_Rep.SaveEditThongBao(Danhmuc, Title, CreateContent, acc.Ten, ID);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu bài viết thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu bài viết không thành công";

            List<ThongBao_ALL> result = _unitOfWork_Rep.ThongBao_Rep.ThongBaoALL();
            return View("ThongBao", result);
        }
        public IActionResult Map()
        {
            MapModel result = new MapModel();
            result = _unitOfWork_Rep.Article_Rep.Map();
            return View(result);
        }
        public IActionResult CreateMap()
        {
            List<Airport> result = new List<Airport>();
            result = _unitOfWork_Rep.Article_Rep.ListAirport();
            return View(result);
        }
        public IActionResult SaveCreateMap(string Danhmuc, string Title, string CreateContent, string Loai)
        {
            bool ret = _unitOfWork_Rep.Article_Rep.SaveCreateMap(Danhmuc, Title, CreateContent, Loai);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu bài viết thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu bài viết không thành công";

            MapModel result = _unitOfWork_Rep.Article_Rep.Map();
            return View("Map", result);
        }
        public IActionResult EditMap(int ID)
        {
            Map_QN result = _unitOfWork_Rep.Article_Rep.EditMap(ID);
            return PartialView("EditMap", result);
        }
        public IActionResult SaveEditMap(string ID, string Danhmuc, string Title, string CreateContent, string Loai)
        {

            bool ret = _unitOfWork_Rep.Article_Rep.SaveEditMap(ID, Danhmuc, Title, CreateContent, Loai);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã chỉnh sửa bài viết thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã chỉnh sửa bài viết không thành công";

            MapModel result = new MapModel();
            result = _unitOfWork_Rep.Article_Rep.Map();
            return View("Map", result);
        }
        public IActionResult Bus()
        {
            List<BusModel> result = new List<BusModel>();
            result = _unitOfWork_Rep.Article_Rep.Bus();
            return View(result);
        }
        public IActionResult CreateBus()
        {
            List<Airport> result = new List<Airport>();
            result = _unitOfWork_Rep.Article_Rep.ListAirport();
            return View(result);
        }
        public IActionResult SaveCreateBus(string Danhmuc, string Title, string CreateContent)
        {
            bool ret = _unitOfWork_Rep.Article_Rep.SaveCreateBus(Danhmuc, Title, CreateContent);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu bài viết thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu bài viết không thành công";

            List<BusModel> result = _unitOfWork_Rep.Article_Rep.Bus();
            return View("Bus", result);
        }
        public IActionResult EditBus(int ID)
        {
            BusModel result = _unitOfWork_Rep.Article_Rep.EditBus(ID);
            return PartialView("EditBus", result);
        }
        public IActionResult SaveEditBus(string ID, string Danhmuc, string Title, string CreateContent)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool ret = _unitOfWork_Rep.Article_Rep.SaveEditBus(ID, Danhmuc, Title, CreateContent);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã chỉnh  sửa bài viết thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã chỉnh sửa bài viết không thành công";

            List<BusModel> result = _unitOfWork_Rep.Article_Rep.Bus();
            return View("Bus", result);
        }
        public IActionResult ImportChuongTrinhKhuyenMai()
        {
            KhuyenMaiDaiLyModel result = new KhuyenMaiDaiLyModel();
            result = _unitOfWork_Rep.CTKM_Rep.ChuongTrinhKhuyenMai();
            return View(result);
        }
        public IActionResult SaveDataKhuyenMai(string data)
        {
            string result = "";
            List<DaiLyKhuyenMai> ListDaiLyKhuyenMai = JsonConvert.DeserializeObject<List<DaiLyKhuyenMai>>(data);
            if (ListDaiLyKhuyenMai != null)
            {
                if (ListDaiLyKhuyenMai.Count > 0)
                {
                    bool ret = _unitOfWork_Rep.CTKM_Rep.InsertListDaiLy(ListDaiLyKhuyenMai);
                    if (ret == true)
                    {
                        result = "Lưu File thành công !";
                    }
                    else
                    {
                        result = "Lưu File không thành công !";
                    }
                }
            }
            return Json(result);
        }
        private byte[] ObjectToByteArray(List<DaiLyKhuyenMai> listDaiLyKhuyenMai)
        {
            byte[] bytes = null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, listDaiLyKhuyenMai);
                bytes = ms.ToArray();
            }
            return bytes;
        }
        private object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = binForm.Deserialize(memStream);

            return obj;
        }
        public IActionResult ListDaiLy(int IDChuongTrinh)
        {
            List<DaiLyKhuyenMai> ret = _unitOfWork_Rep.CTKM_Rep.ListDaiLy(IDChuongTrinh);
            return View(ret);
        }
        public IActionResult EditStatusCTKM(int IDChuongTrinh, string Status)
        {
            bool ret = _unitOfWork_Rep.CTKM_Rep.EditStatusCTKM(IDChuongTrinh, Status);
            return Json(ret);
        }
        public IActionResult SaveDaiLy(string ID, string MaKH, string SoLuong)
        {
            bool ret = _unitOfWork_Rep.CTKM_Rep.SaveDaiLy(ID, MaKH, SoLuong);
            return Json(ret);
        }
        public IActionResult CreateChuongTrinhXoSo()
        {
            ThongTinXoSoModel result = new ThongTinXoSoModel();
            result = _unitOfWork_Rep.CTXS_Rep.ChuongTrinhXoSo();
            return View(result);
        }
        [HttpPost]
        public IActionResult CreateChuongTrinhXoSo(string title, DateTime datefrom, DateTime dateto, string description, int soluong, string buttonName, string ID)
        {
            try
            {
                ThongTinXoSoModel result = new ThongTinXoSoModel();
                List<XoSoDetail> ListXoSoDetail = new List<XoSoDetail>();
                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                if (acc.Ten == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                if (buttonName == "Save")
                {
                    bool ret = _unitOfWork_Rep.CTXS_Rep.InsertXoSo(title, datefrom, dateto, description, soluong, acc.MaNV);
                    if (ret == true)
                    {
                        ViewBag.ThongBaoPop = "Thêm chương trình thành công";
                    }
                }
                else
                {
                    if (buttonName == "Update")
                    {
                        bool ret = _unitOfWork_Rep.CTXS_Rep.UpdateXoSo(title, datefrom, dateto, description, soluong, acc.MaNV, ID);
                        if (ret == true)
                        {
                            ViewBag.ThongBaoPop = "Update thành công";
                        }
                    }
                    else
                    {
                        if (buttonName == "Export")
                        {
                            return PrintExcel(int.Parse(ID));
                        }
                    }
                }
                result = _unitOfWork_Rep.CTXS_Rep.ChuongTrinhXoSo();
                return View(result);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public IActionResult EditStatusCTXS(string title, string Status)
        {
            bool ret = _unitOfWork_Rep.CTXS_Rep.EditStatusCTXS(title, Status);
            return Json(ret);
        }
        public IActionResult ListXoSo(string title)
        {
            List<XoSoDetail> ret = _unitOfWork_Rep.CTXS_Rep.ListXoSo(title);
            return PartialView("ListXoSo", ret);
        }
        [HttpPost]
        public IActionResult DescriptionXoSo(string khoachinh)
        {
            string result = _unitOfWork_Rep.CTXS_Rep.DescriptionXoSo(khoachinh);
            return PartialView("DescriptionXoSo", result);
        }
        [HttpPost]
        public IActionResult DetailXoSo(string khoachinh)
        {
            ThongTinXoSoModel thongTin = new ThongTinXoSoModel();
            //Info
            List<XoSo> result = new List<XoSo>();
            XoSo xoso = new XoSo();
            xoso = _unitOfWork_Rep.CTXS_Rep.GetChuongTrinhXoSo(int.Parse(khoachinh));
            result.Add(xoso);
            //Chi Tiết
            List<XoSoDetail> result_Detail = new List<XoSoDetail>();
            result_Detail = _unitOfWork_Rep.CTXS_Rep.GetChiTietChuongTrinhXoSo(int.Parse(khoachinh));

            thongTin.ListXoSo = result;
            thongTin.ListXoSoDetail = result_Detail;

            //string result = _unitOfWork_Rep.CTXS_Rep.DescriptionXoSo(khoachinh);
            return PartialView("DetailXoSo", thongTin);
        }
        [HttpPost]
        public IActionResult ImportDataXoSo(string khoachinh)
        {
            //string result = _unitOfWork_Rep.CTXS_Rep.DescriptionXoSo(khoachinh);
            return PartialView("ImportDataXoSo", khoachinh);
        }

        [HttpPost]
        public JsonResult SaveDataXoSo(string data)
        {
            string result = "";
            List<XoSoDetail> xoSoDetails = JsonConvert.DeserializeObject<List<XoSoDetail>>(data);
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (xoSoDetails != null)
            {
                if (xoSoDetails.Count > 0)
                {
                    bool ret = _unitOfWork_Rep.CTXS_Rep.InsertListXoSoDetail(xoSoDetails, acc.MaNV);
                    if (ret == true)
                    {
                        result = "Lưu dữ liệu thành công !";
                    }
                    else
                    {
                        result = "Lưu dữ liệu không thành công !";
                    }
                }
            }
            return Json(result);
        }
        [HttpPost]
        public IActionResult UpdateStatusCTXS(int Status, int ID)
        {
            bool ret = _unitOfWork_Rep.CTXS_Rep.UpdateStatusCTXS(Status, ID);
            return Json(ret);
        }
        public JsonResult GetChuongTrinhXoSo(int ID)
        {
            XoSo result = new XoSo();
            result = _unitOfWork_Rep.CTXS_Rep.GetChuongTrinhXoSo(ID);
            return Json(result);
        }
        [HttpPost]
        public JsonResult DeleteDetailXoSo(int ID)
        {
            bool ret = _unitOfWork_Rep.CTXS_Rep.DeleteDetailXoSo(ID);
            return Json(ret);
        }
        [HttpPost]
        public JsonResult SearchMaKHDetailXoSo(string MAKH, int ID)
        {
            List<XoSoDetail> result = new List<XoSoDetail>();
            result = _unitOfWork_Rep.CTXS_Rep.SearchMaKHDetailXoSo(MAKH, ID);
            return Json(result);
        }
        [HttpPost]
        public JsonResult InsertDetailXoSo(string MAKH, int ID, int SL)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool result = false;
            XoSoDetail xoSoDetail = new XoSoDetail();
            xoSoDetail.IDXoSo = ID;
            xoSoDetail.MaKH = MAKH;
            xoSoDetail.SoLuong = SL;
            List<XoSoDetail> list = new List<XoSoDetail>();
            list.Add(xoSoDetail);
            if (_unitOfWork_Rep.CTXS_Rep.CheckMaKHXoSo(MAKH, ID) == true)
            {
                result = _unitOfWork_Rep.CTXS_Rep.InsertListXoSoDetail(list, acc.MaNV);
            }
            return Json(result);
        }
        //ABC
        public IActionResult CreateBookerClub()
        {
            ThongTinBookerClubModel result = new ThongTinBookerClubModel();
            result = _unitOfWork_Rep.BookerClub_Rep.ChuongTrinhBookerClub();
            return View(result);
        }
        [HttpPost]
        public IActionResult CreateBookerClub(string title, DateTime datefrom, DateTime dateto, string description, int soluong, string buttonName, string ID)
        {
            try
            {
                ThongTinBookerClubModel result = new ThongTinBookerClubModel();
                List<BookerClubDetail> ListXoSoDetail = new List<BookerClubDetail>();
                AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
                if (acc.Ten == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                if (buttonName == "Save")
                {
                    bool ret = _unitOfWork_Rep.BookerClub_Rep.InsertBookerClub(title, datefrom, dateto, description, soluong, acc.MaNV);
                    if (ret == true)
                    {
                        ViewBag.ThongBaoPop = "Thêm chương trình thành công";
                    }
                }
                else
                {
                    if (buttonName == "Update")
                    {
                        bool ret = _unitOfWork_Rep.BookerClub_Rep.UpdateBookerClub(title, datefrom, dateto, description, soluong, acc.MaNV, ID);
                        if (ret == true)
                        {
                            ViewBag.ThongBaoPop = "Update thành công";
                        }
                    }
                }
                result = _unitOfWork_Rep.BookerClub_Rep.ChuongTrinhBookerClub();
                return View(result);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public IActionResult EditStatusBookerClub(string title, string Status)
        {
            bool ret = _unitOfWork_Rep.BookerClub_Rep.EditStatusBC(title, Status);
            return Json(ret);
        }
        public IActionResult ListBookerClub(string title)
        {
            List<BookerClubDetail> ret = _unitOfWork_Rep.BookerClub_Rep.ListBookerClub(title);
            return PartialView("LisBookerClub", ret);
        }
        [HttpPost]
        public IActionResult DescriptionBookerClub(string khoachinh)
        {
            string result = _unitOfWork_Rep.BookerClub_Rep.DescriptionBookerClub(khoachinh);
            return PartialView("DescriptionBookerClub", result);
        }
        [HttpPost]
        public IActionResult DetailBookerClub(string khoachinh)
        {
            ThongTinBookerClubModel thongTin = new ThongTinBookerClubModel();
            //Info
            List<BookerClub> result = new List<BookerClub>();
            BookerClub bookClub = new BookerClub();
            bookClub = _unitOfWork_Rep.BookerClub_Rep.GetBookerClub(int.Parse(khoachinh));
            result.Add(bookClub);
            //Chi Tiết
            List<BookerClubDetail> result_Detail = new List<BookerClubDetail>();
            result_Detail = _unitOfWork_Rep.BookerClub_Rep.GetBookerClubDetail(int.Parse(khoachinh));

            thongTin.ListBookerClub = result;
            thongTin.ListBookerClubDetail = result_Detail;

            //string result = _unitOfWork_Rep.CTXS_Rep.DescriptionXoSo(khoachinh);
            return PartialView("DetailBookerClub", thongTin);
        }
        [HttpPost]
        public IActionResult ImportDataBookerClub(string khoachinh)
        {
            //string result = _unitOfWork_Rep.CTXS_Rep.DescriptionXoSo(khoachinh);
            return PartialView("ImportDataBookerClub", khoachinh);
        }

        [HttpPost]
        public JsonResult SaveDataBookerClubDetail(string data)
        {
            string result = "";
            List<BookerClubDetail> bookerClubDetails = JsonConvert.DeserializeObject<List<BookerClubDetail>>(data);
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (bookerClubDetails != null)
            {
                if (bookerClubDetails.Count > 0)
                {
                    bool ret = _unitOfWork_Rep.BookerClub_Rep.InsertListBookerClubDetail(bookerClubDetails, acc.MaNV);
                    if (ret == true)
                    {
                        result = "Lưu dữ liệu thành công !";
                    }
                    else
                    {
                        result = "Lưu dữ liệu không thành công !";
                    }
                }
            }
            return Json(result);
        }
        [HttpPost]
        public IActionResult UpdateStatusBookerClub(int Status, int ID)
        {
            bool ret = _unitOfWork_Rep.BookerClub_Rep.UpdateStatusDetail(Status, ID);
            return Json(ret);
        }
        public JsonResult GetBookerClub(int ID)
        {
            BookerClub result = new BookerClub();
            result = _unitOfWork_Rep.BookerClub_Rep.GetBookerClub(ID);
            return Json(result);
        }
        [HttpPost]
        public JsonResult DeleteBookerClubDetail(int ID)
        {
            bool ret = _unitOfWork_Rep.BookerClub_Rep.DeleteDetail(ID);
            return Json(ret);
        }
        [HttpPost]
        public JsonResult SearchMaKHBookerClubDetail(string MAKH, int ID)
        {
            List<BookerClubDetail> result = new List<BookerClubDetail>();
            result = _unitOfWork_Rep.BookerClub_Rep.SearchMaKHDetail(MAKH, ID);
            return Json(result);
        }
        [HttpPost]
        public JsonResult InsertBookerClubDetail(string MAKH, int ID, string ID_Booker, string TicketNumber)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool result = true;
            BookerClubDetail bookerClubDetail = new BookerClubDetail();
            bookerClubDetail.ID_BookerClub = ID;
            bookerClubDetail.MaKH = MAKH;
            bookerClubDetail.ID_Booker = ID_Booker;
            bookerClubDetail.TicketNumber = TicketNumber;
            List<BookerClubDetail> list = new List<BookerClubDetail>();
            list.Add(bookerClubDetail);
            result = _unitOfWork_Rep.BookerClub_Rep.InsertListBookerClubDetail(list, acc.MaNV);
            return Json(result);
        }
        public IActionResult ChuongTrinhKhuyenMai()
        {
            List<ChuongTrinhKhuyenMai> result = _unitOfWork_Rep.CTKM_Rep.DSChuongTrinhKhuyenMai();
            return View(result);
        }
        public IActionResult CreateCTKM()
        {
            return PartialView("CreateCTKM");
        }
        public IActionResult SaveCreateCTKM(string Title, string Images, string datefrom, string dateto, string CreateContent)
        {
            bool ret = _unitOfWork_Rep.CTKM_Rep.SaveCreateCTKM(Title, Images, datefrom, dateto, CreateContent);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu bài viết thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu bài viết không thành công";
            List<ChuongTrinhKhuyenMai> result = _unitOfWork_Rep.CTKM_Rep.DSChuongTrinhKhuyenMai();
            return View("ChuongTrinhKhuyenMai", result);
        }
        public IActionResult EditCTKM(int ID)
        {
            ChuongTrinhKhuyenMai result = _unitOfWork_Rep.CTKM_Rep.EditCTKM(ID);

            string formattedDateFrom = DateTime.Parse(result.DateFrom).ToString("yyyy-MM-ddTHH:mm");
            result.DateFrom = formattedDateFrom;

            string formattedDateTo = DateTime.Parse(result.DateTo).ToString("yyyy-MM-ddTHH:mm");
            result.DateTo = formattedDateTo;
            return PartialView("EditCTKM", result);
        }
        public IActionResult SaveEditCTKM(int ID, string Title, string Images, string datefrom, string dateto, string CreateContent)
        {
            bool ret = _unitOfWork_Rep.CTKM_Rep.SaveEditCTKM(ID, Title, Images, datefrom, dateto, CreateContent);
            if (ret == true)
            {
                TempData["thongbaoSuccess"] = "Bạn đã lưu bài viết thành công";
            }
            else TempData["thongbaoError"] = "Bạn đã lưu bài viết không thành công";
            List<ChuongTrinhKhuyenMai> result = _unitOfWork_Rep.CTKM_Rep.DSChuongTrinhKhuyenMai();
            return View("ChuongTrinhKhuyenMai", result);
        }
        public IActionResult StatusCTKM(int ID, string Status)
        {
            bool ret = _unitOfWork_Rep.CTKM_Rep.StatusCTKM(ID, Status);
            return Json(ret);
        }
        #region Private Function
        public IActionResult PrintExcel(int ID)
        {
            List<Number_XoSoDetail> result = new List<Number_XoSoDetail>();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            result = _unitOfWork_Rep.CTXS_Rep.NumberKHXoSo(ID);
            XoSo xoSo = new XoSo();
            xoSo = _unitOfWork_Rep.CTXS_Rep.GetChuongTrinhXoSo(ID);

            byte[] fileContents;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ThongKe");

            ws.Cells.Style.Font.Name = "Times New Roman";
            ws.Cells.Style.Font.Size = 12;


            int rowHeader = 1;

            ws.Cells["A" + rowHeader].Value = "SoVe";
            ws.Cells["B" + rowHeader].Value = "MaKH";
            ws.Cells["C" + rowHeader].Value = "GhiChu";
            ws.Cells["A" + rowHeader + ":C" + rowHeader].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells["A" + rowHeader + ":C" + rowHeader].Style.Fill.BackgroundColor.SetColor(Color.Green);
            ws.Cells["A" + rowHeader + ":C" + rowHeader].Style.Font.Bold = true;
            ws.Cells["A" + rowHeader + ":C" + rowHeader].Style.Font.Color.SetColor(Color.White);
            ws.Column(1).Width = 20;
            ws.Column(2).Width = 20;
            ws.Column(3).Width = 40;


            int rowStart = rowHeader + 1;
            int stt = 1;
            foreach (var item in result)
            {
                ws.Cells["A" + rowStart].Value = item.Number;
                ws.Cells["A" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["B" + rowStart].Value = item.MaKH;
                ws.Cells["B" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["C" + rowStart].Value = "";
                ws.Cells["C" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                rowStart++;
                stt++;
            }

            fileContents = pck.GetAsByteArray();
            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "ThongKe_" + xoSo.Title + ".xlsx"
            );

        }
        #endregion
        public IActionResult DanhSachIDBooker()
        {
            List<INFO_IDBookerModel> list = _unitOfWork_Rep.BookerClub_Rep.ListIDBooker();
            ViewBag.select = "MaKH";
            ViewBag.value = "";
            return View(list);
        }
        public IActionResult SearchIDBooker(string select, string value, string Search)
        {
            if (Search == "Search")
            {
                List<INFO_IDBookerModel> list = _unitOfWork_Rep.BookerClub_Rep.SearchIDBooker(select, value);
                ViewBag.select = select;
                ViewBag.value = value;
                return View("DanhSachIDBooker", list);
            }
            else
            {
                return ExportExcelIDBooker(select, value);
            }

        }
        [HttpPost]
        public IActionResult ExportExcelIDBooker(string select, string value)
        {
            List<INFO_IDBookerModel> result = new List<INFO_IDBookerModel>();
            if (value != null)
            {
                result = _unitOfWork_Rep.BookerClub_Rep.SearchIDBooker(select, value);
            }
            else
            {
                result = _unitOfWork_Rep.BookerClub_Rep.ListIDBooker();
            }

            byte[] fileContents;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("IDBooker");

            ws.Cells.Style.Font.Name = "Times New Roman";
            ws.Cells.Style.Font.Size = 12;
            int rowHeader = 1;
            ws.Cells["A" + rowHeader].Value = "STT";
            ws.Cells["B" + rowHeader].Value = "Ngay up";
            ws.Cells["C" + rowHeader].Value = "MaKH";
            ws.Cells["D" + rowHeader].Value = "Ten dai ly";
            ws.Cells["E" + rowHeader].Value = "NVKD";
            ws.Cells["F" + rowHeader].Value = "ID Booker";
            ws.Cells["G" + rowHeader].Value = "Ho tên";
            ws.Cells["H" + rowHeader].Value = "SDT";
            ws.Cells["I" + rowHeader].Value = "STK";
            ws.Cells["J" + rowHeader].Value = "Ngan hang";
            ws.Cells["K" + rowHeader].Value = "Chu tai khoan";
            ws.Cells["A" + rowHeader + ":K" + rowHeader].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Cells["A" + rowHeader + ":K" + rowHeader].Style.Fill.BackgroundColor.SetColor(Color.Green);
            ws.Cells["A" + rowHeader + ":K" + rowHeader].Style.Font.Bold = true;
            ws.Cells["A" + rowHeader + ":K" + rowHeader].Style.Font.Color.SetColor(Color.White);
            ws.Column(1).Width = 10;
            ws.Column(2).Width = 20;
            ws.Column(3).Width = 10;
            ws.Column(4).Width = 40;
            ws.Column(5).Width = 20;
            ws.Column(6).Width = 20;
            ws.Column(7).Width = 30;
            ws.Column(8).Width = 20;
            ws.Column(9).Width = 20;
            ws.Column(10).Width = 20;
            ws.Column(11).Width = 40;
            int rowStart = rowHeader + 1;
            int STT = 1;
            foreach (var item in result)
            {
                ws.Cells["A" + rowStart].Value = STT;
                ws.Cells["A" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["B" + rowStart].Value = item.UpdateDate;
                ws.Cells["B" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["C" + rowStart].Value = item.CreateUp;
                ws.Cells["C" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["D" + rowStart].Value = item.CompanyName;
                ws.Cells["D" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["E" + rowStart].Value = item.Sales;
                ws.Cells["E" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["F" + rowStart].Value = item.ID_Booker;
                ws.Cells["F" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["G" + rowStart].Value = item.Name;
                ws.Cells["G" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["H" + rowStart].Value = item.Tel;
                ws.Cells["H" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["I" + rowStart].Value = item.BankNumber;
                ws.Cells["I" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["J" + rowStart].Value = item.BankName;
                ws.Cells["J" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                ws.Cells["K" + rowStart].Value = item.BankAccount;
                ws.Cells["K" + rowStart].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                STT++;
                rowStart++;
            }

            fileContents = pck.GetAsByteArray();
            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "DanhSachIDBooker.xlsx"
            );

        }
        [HttpPost]
        public IActionResult DelIDBooker(string ID)
        {
            bool ret = _unitOfWork_Rep.BookerClub_Rep.DelIDBooker(ID);
            return Json(ret);
        }
        public IActionResult DSVeSotKhac()
        {
            string timeOut = _configuration.GetSection("DataService").GetSection("TimeoutEditVeSot").Value;
            string dateFrom = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTo = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy");
            ViewBag.DateTo = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.SoVe = "";
            ViewBag.TimeOut = timeOut;
            return View();
        }
        [HttpPost]
        public IActionResult DSVeSotKhac(string cal_from, string cal_to, string SoVeSearch, string Status)
        {
            string timeOut = _configuration.GetSection("DataService").GetSection("TimeoutEditVeSot").Value;
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            string server_EV = _configuration.GetConnectionString("SQL_EV_MAIN");
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            acc = AccountManager.GetAccountCurrent(HttpContext);
            GuiMailDaiLyRepository guimail_Rep = new GuiMailDaiLyRepository(_configuration);
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
            ViewBag.TimeOut = timeOut;
            ViewBag.SoVe = SoVeSearch;
            string TienMat = "TM";
            result.TimeOutEdit = int.Parse(timeOut);
            result.ListChiTietVe = guimail_Rep.ListChiTietVeOther(acc.MaNV, dateFrom, dateTo, SoVeSearch, server_EV, server_KH_KT, Status);
            return View(result);
        }

        public async Task<IActionResult> ReportPaymentChannel()
        {
            string Server = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            List<string> channels = new List<string>();
            ReportPaymentChannelRepository repository = new ReportPaymentChannelRepository();
            channels = await repository.Channels(Server);

            string dateFrom = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTo = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.DateTo = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.Channels = channels;

            return View();

        }
        [HttpPost]
        public async Task<IActionResult> ReportPaymentChannel(string FromDate, string ToDate, string Channel)
        {
            string Server = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
            List<string> channels = new List<string>();
            ReportPaymentChannelRepository repository = new ReportPaymentChannelRepository();
            channels = await repository.Channels(Server);
            DateTime dFrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider, DateTimeStyles.None);
            string dateFrom = dFrom.ToString("yyyy-MM-dd");
            DateTime dTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider, DateTimeStyles.None);
            string dateTo = dTo.ToString("yyyy-MM-dd");
            ViewBag.DateFrom = dFrom.ToString("dd/MM/yyyy");
            ViewBag.DateTo = dTo.ToString("dd/MM/yyyy");
            ViewBag.Channels = channels;

            //Search
            List<ReportPaymentChannelModel> result = new List<ReportPaymentChannelModel>();
            result = await repository.ReportPaymentChannel(dateFrom, dateTo, Channel, Server);
            return View(result);

        }

    }
}
