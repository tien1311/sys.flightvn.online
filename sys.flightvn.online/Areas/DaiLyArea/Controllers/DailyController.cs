using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Manager.Common.Helpers.AreaHelpers;
using Manager.DataAccess;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Dapper;
using Manager.Common.Helpers;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Manager.DataAccess.Repository;
using Manager.Model.Models;
using Manager.Model.Models.Other;
using Manager.Model.Models.PaginationBase;
using Manager.Model.Models.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using ReflectionIT.Mvc.Paging;
using RtfPipe.Tokens;
using Manager.Model.Models.FTP;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;

namespace Manager_EV.Areas.DaiLyArea.Controllers
{
    [Area(AreaNameConst.AREA_DaiLy)]
    public class DailyController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork_Repository _unitOfWork_Repository;
        private readonly FtpSettings _ftpSettings;


        public DailyController(IConfiguration configuration, IUnitOfWork_Repository unitOfWork_Repository, IOptions<FtpSettings> ftpSettings)
        {
            _configuration = configuration;
            _ftpSettings = ftpSettings.Value;
            _unitOfWork_Repository = unitOfWork_Repository;
        }


        public async Task<IActionResult> GetImagesFromFTP(int page = 1, int pageSize = 50)
        {
            string ftpDirectory = $"{_ftpSettings.Host}/subject";
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpDirectory);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(_ftpSettings.Username, _ftpSettings.Password);

            var imageFiles = new List<string>();

            using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string line = await reader.ReadLineAsync();
                while (!string.IsNullOrEmpty(line))
                {
                    if (line.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        line.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                        line.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                        line.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                        line.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                    {
                        imageFiles.Add(line);
                    }
                    line = reader.ReadLine();
                }
            }

            var paginatedList = imageFiles.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)imageFiles.Count / pageSize);
            ViewBag.CurrentPage = page;

            return PartialView("_ImageList", paginatedList);
        }


        public async Task<IActionResult> UploadFileToFTP(IFormFile ImageInputValueFTP)
        {
            string imageUrl = Common.UploadImg(ImageInputValueFTP,"/subject");
           
            return Json(new { imageUrl = imageUrl });
        }


        public IActionResult BaiViet(int page = 1, int pageSize = 50)
        {
            var paginationVM = GetListPostPagination(0, page, pageSize);
            ViewData["listCategory"] = _unitOfWork_Repository.Post_Rep.GetAllCategory();
            return View(paginationVM);
        }

        private PaginationBase<PostModel> GetListPostPagination(int categoryId, int page, int pageSize)
        {
            var (listOrders, totalRecord) = _unitOfWork_Repository.Post_Rep.GetAllPost(categoryId, page, pageSize);

            PaginationBase<PostModel> paginationVM = new PaginationBase<PostModel>()
            {
                ListProduct = listOrders,
                TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize),
                CurrentPage = page,
                PageSize = pageSize,
                TotalQuantityOfProduct = totalRecord
            };
            return paginationVM;
        }

        public IActionResult GetAllOrderByFilter(int categoryId, int page = 1, int pageSize = 50)
        {
            var orderHeaderPagination = GetListPostPagination(categoryId, page, pageSize);
            return PartialView("_TablePaginationPatial", orderHeaderPagination);
        }


        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveCreateOrUpdatePost(PostModel request, string ImageInputValue)
        {
            DateTime now = DateTime.Now;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            request.subject_author = acc.TenDangNhap;
            request.subject_picture = ImageInputValue.Substring(ImageInputValue.LastIndexOf("/")+1);
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("SQL_Agent_MAIN")))
                {
                    connection.Open();
                    var parameters = new DynamicParameters();
                    // Nếu là chèn mới (INSERT), truyền subject_id = 0
                    if (request.subject_id == 0)
                    {
                        parameters.Add("@subject_id", null);
                    }
                    else
                    {
                        parameters.Add("@subject_id", request.subject_id);
                    }
                    parameters.Add("@subject_code", "");
                    parameters.Add("@subject_name", request.subject_name);
                    parameters.Add("@subject_name_en", "");
                    parameters.Add("@subject_content", request.subject_content);
                    parameters.Add("@subject_content_en", "");
                    parameters.Add("@subject_author", request.subject_author);
                    parameters.Add("@subject_date", now);
                    parameters.Add("@subject_isshow", request.subject_isshow);
                    parameters.Add("@subject_isnew", request.subject_isnew);
                    parameters.Add("@subject_ishot", request.subject_ishot);
                    parameters.Add("@subject_com", request.subject_com);
                    parameters.Add("@subject_seq", request.subject_seq);
                    parameters.Add("@section_id", request.section_id);
                    parameters.Add("@subject_picture", request.subject_picture);
                    parameters.Add("@subject_picnote", request.subject_picnote);
                    parameters.Add("@subject_header", request.subject_header);
                    int result = connection.Execute("Sp_InsertUpdateSubject", parameters, commandType: CommandType.StoredProcedure);
                    return Json(new { success = true, message = "Lưu thành công!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lưu thất bại! Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeletePost(int subjectId)
        {
            bool isSuccess = _unitOfWork_Repository.Post_Rep.DeletePost(subjectId);

            if (isSuccess)
            {
                return Json(new { success = true, message = "Xoá thành công" });
            }

            return Json(new { success = false, message = "Xoá thất bại, có lỗi xảy ra" });
        }

        [HttpPost]
        public IActionResult SendMail(int subjectId)
        {
            bool isSuccess = false;
            var item = _unitOfWork_Repository.Post_Rep.GetPost(subjectId);
            if (item != null)
            {
                isSuccess = _unitOfWork_Repository.Post_Rep.SendMail(item);
                if (isSuccess)
                {
                    isSuccess = _unitOfWork_Repository.Post_Rep.UpdateSendMail(subjectId);
                    if (isSuccess)
                    {
                        return Json(new { success = true, message = "Gửi mail thành công" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Cập nhật dữ liệu thất bại" });
                    }
                }
            }
            return Json(new { success = false, message = "Gửi mail thất bại, có lỗi xảy ra" });
        }

        public IActionResult DetailPost(int subjectId)
        {
            PostModel subject = _unitOfWork_Repository.Post_Rep.GetPost(subjectId);
            return View(subject);
        }


        [HttpPost]
        public IActionResult IsActive(int subjectId)
        {
            var item = _unitOfWork_Repository.Post_Rep.GetPost(subjectId);
            if (item != null)
            {
                if (item.subject_isshow == 1)
                {
                    item.subject_isshow = 0;
                }
                else
                {
                    item.subject_isshow = 1;
                }
                bool isSuccess =  _unitOfWork_Repository.Post_Rep.ToggleShowItem(subjectId, item.subject_isshow);
                if (isSuccess)
                {
                    return Json(new { success = true, message = "Thay đổi trạng thái thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra" });
                }
            }
            return Json(new { success = false, message = "Có lỗi xảy ra" });
        }

        [HttpPost]
        public IActionResult Search(string DieuKien, string GiaTri)
        {
            try
            {
                if(String.IsNullOrEmpty(GiaTri))
                {
                    TempData["thongbaoError"] = "Vui lòng không được bỏ trống";
                    return View("ThongTinDaiLy");
                }
                DaiLyModel daiLy = _unitOfWork_Repository.DaiLy_Rep.Search(DieuKien, GiaTri);
                if (daiLy.DSDaiLy.Count == 0)
                {
                     TempData["thongbaoError"] = "Giá trị " + GiaTri + " không tìm thấy tài khoản đại lý, báo ngay Mr Thiết Trưởng P.Kinh doanh (ĐT : 0969270270 hoặc Chat) để xử lý kịp thời.";
                }
                return View("ThongTinDaiLy", daiLy);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult ChiTietDaiLy(string id, string checkMaKH)
        {
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            DaiLyModel dailyModel = new DaiLyModel();
            //result.ThongTinVe = HDVatVNARepo.LayThongTin(soVe);
            DaiLyModel daiLy = _unitOfWork_Repository.DaiLy_Rep.ChiTietDaiLy(id, checkMaKH, server_KH_KT);
            return Json(daiLy);
        }
        public IActionResult ThongTinDaiLy()
        {
            return View();
        }
        public IActionResult TraCuuSoVe()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SearchSoVe(string sove)
        {
            if (sove != null && sove != "")
            {
                List<SoVeModel> SoVe =  _unitOfWork_Repository.DaiLy_Rep.SearchSoVe(sove);
                return View("TraCuuSoVe", SoVe);
            }
            else return View("TraCuuSoVe");
        }
        public IActionResult TraCuuVeHoan()
        {
            List<VeHoanModel> VeHoan = new List<VeHoanModel>();
            return View("TraCuuVeHoan", VeHoan);
        }
        [HttpPost]
        public IActionResult SearchVeHoan(string sove)
        {
            if (sove != null && sove != "")
            {
                List<VeHoanModel> VeHoan =  _unitOfWork_Repository.DaiLy_Rep.SearchVeHoan(sove);
                return View("TraCuuVeHoan", VeHoan);
            }
            else return View("TraCuuVeHoan");
        }
        [HttpPost]
        public IActionResult ThongTinVe(int khoachinh)
        {
            SubjectModel result =  _unitOfWork_Repository.DaiLy_Rep.ThongTinVe(khoachinh);
            return PartialView("ThongTinVe", result);
        }
        public IActionResult QuyDinhHang(int subject_id)
        {
            BangTinRepository bangtin_Repository = _unitOfWork_Repository.BangTin_Rep;
            SubjectModel content = bangtin_Repository.QuyDinhHang(subject_id);

            return View("DetailQuyDinhHang", content);
        }
        public IActionResult SearchAllCongVan(string tieude, int? page = 1, int pageSize = 7)
        {
            try
            {
                string Title = "";
                if (tieude != null)
                {
                    Title = tieude;
                    HttpContext.Session.SetString("tieude", tieude);
                }
                else
                {
                    Title = HttpContext.Session.GetString("tieude");
                }
                BangTinRepository bangtin_Repository = _unitOfWork_Repository.BangTin_Rep;
                List<SubjectModel> list = bangtin_Repository.SearchAllCongVan(Title);
                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "SearchAllCongVan";
                model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };

                return View("AllCongVan", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult SearchNews(int section_id, string tieude, int? page = 1, int pageSize = 7)
        {
            try
            {
                string Title = "";
                int? ID = 0;
                if (tieude != null && section_id != 0)
                {
                    Title = tieude;
                    ID = section_id;
                    HttpContext.Session.SetString("tieude", tieude);
                    HttpContext.Session.SetInt32("ID", section_id);
                }
                else
                {
                    Title = HttpContext.Session.GetString("tieude");
                    ID = HttpContext.Session.GetInt32("ID");
                }
            BangTinRepository bangtin_Repository = _unitOfWork_Repository.BangTin_Rep;
                List<SubjectModel> list = bangtin_Repository.SearchListAll(ID, Title);
                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);

                if (ID == 22)
                {
                    model.Action = "SearchNews";
                    model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                    return View("CongVan", model);
                }
                if (ID == 124)
                {
                    model.Action = "SearchNews";
                    model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                    return View("VietnamAirlines", model);
                }
                if (ID == 126)
                {
                    model.Action = "SearchNews";
                    model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                    return View("BambooAirway", model);
                }
                if (ID == 125)
                {
                    model.Action = "SearchNews";
                    model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                    return View("Vietjet", model);
                }
                if (ID == 132)
                {
                    model.Action = "SearchNews";
                    model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                    return View("VietravelAirlines", model);
                }
                if (ID == 127)
                {
                    model.Action = "SearchNews";
                    model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                    return View("OtherAir", model);
                }
                if (ID == 141)
                {
                    model.Action = "SearchNews";
                    model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                    return View("CongVanManager", model);
                }
                else return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //all công văn ev và các hãng
        public IActionResult AllCongVan(int? page = 1, int pageSize = 7)
        {
            try
            {
            BangTinRepository bangtin_Repository = _unitOfWork_Repository.BangTin_Rep;
                List<SubjectModel> list = bangtin_Repository.AllCongVan();
                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "AllCongVan";
                model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Công văn EV
        public IActionResult CongVan(int section_id = 22, int? page = 1, int pageSize = 7)
        {

            try
            {


            BangTinRepository bangtin_Repository = _unitOfWork_Repository.BangTin_Rep;
                List<SubjectModel> list = bangtin_Repository.ListAll(section_id);
                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "CongVan";
                model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };

                return View(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        //Công văn riêng Manager
        public IActionResult CongVanManager(int section_id = 141, int? page = 1, int pageSize = 7)
        {

            try
            {


                BangTinRepository QuyDinhNghiepVu_Repository = _unitOfWork_Repository.BangTin_Rep;

                string listdanhsach = "";
                List<SubjectModel> list = QuyDinhNghiepVu_Repository.ListAll(section_id);

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);

                model.Action = "CongVanManager";
                model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                return View(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        //Quy định VietnamAirlines
        public IActionResult VietnamAirlines(int section_id = 124, int? page = 1, int pageSize = 7)
        {

            try
            {


                BangTinRepository QuyDinhNghiepVu_Repository = _unitOfWork_Repository.BangTin_Rep;

                string listdanhsach = "";
                List<SubjectModel> list = QuyDinhNghiepVu_Repository.ListAll(section_id);

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);

                model.Action = "VietnamAirlines";
                model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                return View(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }



        //Quy định BambooAirway
        public IActionResult BambooAirway(int section_id = 126, int? page = 1, int pageSize = 7)
        {

            try
            {


                BangTinRepository QuyDinhNghiepVu_Repository = _unitOfWork_Repository.BangTin_Rep;

                string listdanhsach = "";
                List<SubjectModel> list = QuyDinhNghiepVu_Repository.ListAll(section_id);

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "BambooAirway";
                model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                return View(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        //Quy định vietject
        public IActionResult Vietjet(int section_id = 125, int? page = 1, int pageSize = 7)
        {

            try
            {


                BangTinRepository QuyDinhNghiepVu_Repository = _unitOfWork_Repository.BangTin_Rep;

                string listdanhsach = "";
                List<SubjectModel> list = QuyDinhNghiepVu_Repository.ListAll(section_id);

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "Vietjet";
                model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                return View(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        //Quy định các hãng hàng không khác
        public IActionResult OtherAir(int section_id = 127, int? page = 1, int pageSize = 7)
        {

            try
            {


                BangTinRepository QuyDinhNghiepVu_Repository = _unitOfWork_Repository.BangTin_Rep;

                string listdanhsach = "";
                List<SubjectModel> list = QuyDinhNghiepVu_Repository.ListAll(section_id);

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "OtherAir";
                model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                return View(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }



        //Vietravel Airlines
        public IActionResult VietravelAirlines(int section_id = 132, int? page = 1, int pageSize = 7)
        {

            try
            {


                BangTinRepository QuyDinhNghiepVu_Repository = _unitOfWork_Repository.BangTin_Rep;

                string listdanhsach = "";
                List<SubjectModel> list = QuyDinhNghiepVu_Repository.ListAll(section_id);

                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "OtherAir";
                model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                return View(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public IActionResult KhuyenMai(int? page = 1, int pageSize = 9)
        {
            try
            {
            BangTinRepository bangtin_Repository = _unitOfWork_Repository.BangTin_Rep;
                List<SubjectModel> list = bangtin_Repository.KhuyenMai();
                int pageNumber = page ?? 1;
                //Phân trang 
                var model = PagingList.Create(list, pageSize, pageNumber);
                model.Action = "KhuyenMai";
                model.RouteValue = new RouteValueDictionary {
                        { "i", 8}
                    };
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult TraCuuSignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TraCuuSignIn(string GiaTri, string DieuKien)
        {
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            string server_EV_MAIN = _configuration.GetConnectionString("SQL_EV_MAIN");
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            //Hết token
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            DaiLyRepository DaiLy_Rep = _unitOfWork_Repository.DaiLy_Rep;
            List<DaiLyEV> content = new List<DaiLyEV>();
            content = DaiLy_Rep.TraCuuSignIn(GiaTri, DieuKien);
            if (content.Count == 0)
            {
                TempData["thongbaoError"] = "Không tìm thấy code hoặc sign in";
            }

            return View("TraCuuSignIn", content);
        }

        public IActionResult TraCuuChuyenKhoan(string MAKH = "", string SOTIEN = "", string NGANHANG = "", int? page = 1, int pageSize = 50, string search = "", string SelectOptions = "", string i = "")
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.Ten == null)
            {
                return RedirectToAction("Index", "Login");
            }
            List<BienDongSoDuModel> List = new List<BienDongSoDuModel>();
            int pageNumber = page ?? 1;
            BienDongSoDuRepository rep = _unitOfWork_Repository.BienDongSoDu_Rep;

            if (search == "")
            {
                return View();
            }
            else
            {
                if (search == "Search")
                {
                    if (MAKH == null || MAKH == "")
                    {
                        List = rep.TraCuuChuyenKhoanDaiLy(MAKH, "", "", search);
                    }
                    else
                    {
                        if (MAKH.Length == 7)
                        {
                            List = rep.TraCuuChuyenKhoanDaiLy(MAKH, "", "", search);
                        }
                        else
                        {
                            TempData["thongbaoError"] = "Mã KH không đúng định dạng";       
                            ViewBag.MaKH = MAKH;
                            return View();
                        }

                    }


                }
                else
                {
                    List = rep.TraCuuChuyenKhoanDaiLy(MAKH, NGANHANG, SOTIEN, search);
                }
            }
            ViewBag.NganHang = NGANHANG;
            ViewBag.MaKH = MAKH;
            ViewBag.SoTien = SOTIEN;
            ViewBag.i = "8";
            ViewBag.SelectOptions = SelectOptions;
            var model = PagingList.Create(List, pageSize, pageNumber);
            model.Action = "TraCuuChuyenKhoan";
            model.RouteValue = new RouteValueDictionary {
                        {
                         "i", 8
                          },
                          {
                              "MAKH",MAKH
                          },

                          {
                              "SOTIEN",SOTIEN
                          },
                        {
                                "NGANHANG",NGANHANG
                        },
                        {
                             "search",search
                        }
                        ,
                        {
                             "SelectOptions",SelectOptions
                        }
             };
            return View(model);
        }



        [HttpPost]
        public IActionResult EditMaKH(string MaCK)
        {
            return PartialView("CapNhatMaKH", MaCK);
        }
        [HttpPost]
        public JsonResult UpdateMaKH(string MaCK, string MaKH)
        {
            //Hết token
            string server_KH_KT_NH = _configuration.GetConnectionString("SQL_KT_MAIN");
            string server_KH_KT = _configuration.GetConnectionString("SQL_KH_KT");
            GuiMailDaiLyRepository rep_daily = _unitOfWork_Repository.GuiMail_DaiLy_Rep;
            BienDongSoDuRepository rep = _unitOfWork_Repository.BienDongSoDu_Rep;
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (!rep.KiemTraMaKH(MaCK, MaKH))
            {
                string info = "TrungKH";
                return Json(info);
            }
            if (rep_daily.ExistsMaKH(MaKH, server_KH_KT) == "")
            {
                string info = "KhongTonTai";
                return Json(info);
            }
            string server = _configuration.GetConnectionString("SQL_EV_MAIN");
            Task<bool> result = rep.CapNhatMaKH(MaCK, MaKH, server, acc.HoTen, server_KH_KT_NH, acc.MaNV);
            return Json(result.Result);
        }
        public IActionResult DanhSachThanhVien(string resetBtn, string searchBtn, string TenDL, string NguoiLH, string MaKH, string Email, string Phone)
        {
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            if (acc.TenDangNhap == null)
            {
                return RedirectToAction("Index", "Login");
            }

            MemberRepository member_rep = new MemberRepository();
            Danhsachmodel result = new Danhsachmodel();
            if (resetBtn != null)
            {
                result = member_rep.DanhSachMember();
                return View("DanhSachThanhVien", result);
            }
            if (searchBtn != null)
            {
                ViewBag.TenDL = TenDL;
                ViewBag.NguoiLH = NguoiLH;
                ViewBag.MaKH = MaKH;
                ViewBag.Email = Email;
                ViewBag.Phone = Phone;
                result = member_rep.SearchMember(TenDL, NguoiLH, MaKH, Email, Phone);
                return View("DanhSachThanhVien", result);
            }

            result = member_rep.DanhSachMember();
            return View("DanhSachThanhVien", result);
        }

        public IActionResult ChiTietMember(string khoachinh)
        {
            MemberRepository member_rep = new MemberRepository();
            List<Member> result = member_rep.Chitietmember(khoachinh);
            return View(result);
        }
        [HttpPost]
        public JsonResult UpdatePassword(string password, string memberid)
        {
            MemberRepository member_rep = new MemberRepository();
            bool result = member_rep.UpdatePassword(password, memberid);
            return Json(result);
        }
        [HttpPost]
        public JsonResult UpdateMember(string memberid, string khuvuc, string company, string name, string makh, string code, string email, string address, string phone, string fax, int isactive, bool isshow, string ketoan, string kinhdoanh, string dulich, string[] member_childs)
        {

            MemberRepository member_rep = new MemberRepository();
            AccountModel acc = AccountManager.GetAccountCurrent(HttpContext);
            bool result = member_rep.UpdateMember(memberid, khuvuc, company, name, makh, code, email, address, phone, fax, isactive, isshow, kinhdoanh, ketoan, dulich, member_childs, acc.TenDangNhap);
            return Json(result);
        }
        [HttpPost]
        public JsonResult ActiveMember(string Active, int RowID)
        {
            MemberRepository member_rep = new MemberRepository();
            bool result = member_rep.ActiveMember(Active, RowID);
            return Json(result);

        }
        [HttpPost]
        public JsonResult ResetPass(string RowID)
        {
            string pass = "enviet";
            MemberRepository member_rep = new MemberRepository();
            bool result = member_rep.UpdatePassword(pass, RowID);
            return Json(result);

        }
        public IActionResult HuongDan()
        {
            return View();
        }
    }

}

