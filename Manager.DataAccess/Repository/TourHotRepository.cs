using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Dapper;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using EasyInvoice.Json.Linq;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.IO;
using Manager.Model.Models;

namespace Manager.DataAccess.Repository
{
    public class TourHotRepository
    {


        private readonly string _connectionString;
        private readonly string _connectionStringTour;
        private readonly string _connectionStringTourhot;
        DBase db = new DBase();


        public TourHotRepository(IConfiguration configuration)
        {

            _connectionString = configuration.GetConnectionString("SQL_EV_TOUR");
            _connectionStringTour = configuration.GetConnectionString("SQL_POST");
            _connectionStringTourhot = configuration.GetConnectionString("SQL_EV_TOURHOT");
        }
        public List<Destination> ListDestination()
        {
            List<Destination> ListDestination = new List<Destination>();
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                ListDestination = dbConnection.Query<Destination>(
                    "SELECT * FROM Destination ORDER BY IDTinh"
                ).ToList();
            }

            return ListDestination;
        }
        public List<Destination> ListDestinationQT()
        {
            List<Destination> ListDestination = new List<Destination>();
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                ListDestination = dbConnection.Query<Destination>(
                    "SELECT * FROM DestinationQT ORDER BY IDTinh"
                ).ToList();
            }

            return ListDestination;
        }
        public List<FlagModel> ListFlag()
        {
            List<FlagModel> ListDestination = new List<FlagModel>();
            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                dbConnection.Open();
                ListDestination = dbConnection.Query<FlagModel>(
                    "SELECT * FROM Flag"
                ).ToList();
            }

            return ListDestination;
        }
        public List<TourEV> GetAllListTourHot()
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                dbConnection.Open();

                var sql = @"
        SELECT * FROM tours;         
        SELECT * FROM sale;
        SELECT * FROM img;
        SELECT * FROM gia;
        SELECT * FROM [file];";

                using (var multi = dbConnection.QueryMultiple(sql))
                {
                    var tours = multi.Read<TourEV>().ToList();
                    var sales = multi.Read<SaleEV>().ToList();
                    var imgs = multi.Read<ImgEV>().ToList();
                    var gias = multi.Read<GiaEV>().ToList();
                    var files = multi.Read<FileEV>().ToList();

                    foreach (var tour in tours)
                    {
                        tour.Sale = sales.FirstOrDefault(s => s.Tour_Id == tour.Tour_Id);
                        tour.Imgs = imgs.Where(i => i.Tour_Id == tour.Tour_Id).ToList();
                        tour.Gias = gias.Where(g => g.Tour_Id == tour.Tour_Id).ToList();
                        tour.Files = files.Where(f => f.Tour_Id == tour.Tour_Id).ToList();
                    }

                    return tours;
                }
            }
        }

        public List<TourEV> SearchIDTourHot(string IDCity)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                dbConnection.Open();

                var sql = @"
WITH CityIDs AS (
    SELECT Value AS IDCity
    FROM dbo.SplitStringToTable(@IDCity)
)
SELECT DISTINCT tours.* 
FROM tours
CROSS APPLY dbo.SplitStringToTable(tours.diem_den) AS TourDiemDen
WHERE EXISTS (
    SELECT 1 
    FROM CityIDs 
    WHERE TourDiemDen.Value = CityIDs.IDCity
);     
SELECT * FROM sale;
SELECT * FROM img;
SELECT * FROM gia;
SELECT * FROM [file];";

                using (var multi = dbConnection.QueryMultiple(sql, new { IDCity }))
                {
                    var tours = multi.Read<TourEV>().ToList();
                    var sales = multi.Read<SaleEV>().ToList();
                    var imgs = multi.Read<ImgEV>().ToList();
                    var gias = multi.Read<GiaEV>().ToList();
                    var files = multi.Read<FileEV>().ToList();

                    foreach (var tour in tours)
                    {
                        tour.Sale = sales.FirstOrDefault(s => s.Tour_Id == tour.Tour_Id);
                        tour.Imgs = imgs.Where(i => i.Tour_Id == tour.Tour_Id).ToList();
                        tour.Gias = gias.Where(g => g.Tour_Id == tour.Tour_Id).ToList();
                        tour.Files = files.Where(f => f.Tour_Id == tour.Tour_Id).ToList();
                    }

                    return tours;
                }
            }
        }


        public List<TourEV> SearchDayAndIDTourHot(string IDCity, string selectedDate)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                dbConnection.Open();

                var sql = @"
WITH CityIDs AS (
    SELECT Value AS IDCity
    FROM dbo.SplitStringToTable(@IDCity)
)
SELECT DISTINCT tours.* 
FROM tours
CROSS APPLY dbo.SplitStringToTable(tours.diem_den) AS TourDiemDen
WHERE EXISTS (
    SELECT 1 
    FROM CityIDs 
    WHERE TourDiemDen.Value = CityIDs.IDCity
)
AND MONTH(tours.ngay_dong_tour) = @Month
AND YEAR(tours.ngay_dong_tour) = @Year;     
SELECT * FROM sale;
SELECT * FROM img;
SELECT * FROM gia;
SELECT * FROM [file];";

                // Parse the selectedDate into month and year
                var dateParts = selectedDate.Split('/');
                var month = int.Parse(dateParts[0]);
                var year = int.Parse(dateParts[1]);

                using (var multi = dbConnection.QueryMultiple(sql, new { IDCity, Month = month, Year = year }))
                {
                    var tours = multi.Read<TourEV>().ToList();
                    var sales = multi.Read<SaleEV>().ToList();
                    var imgs = multi.Read<ImgEV>().ToList();
                    var gias = multi.Read<GiaEV>().ToList();
                    var files = multi.Read<FileEV>().ToList();

                    foreach (var tour in tours)
                    {
                        tour.Sale = sales.FirstOrDefault(s => s.Tour_Id == tour.Tour_Id);
                        tour.Imgs = imgs.Where(i => i.Tour_Id == tour.Tour_Id).ToList();
                        tour.Gias = gias.Where(g => g.Tour_Id == tour.Tour_Id).ToList();
                        tour.Files = files.Where(f => f.Tour_Id == tour.Tour_Id).ToList();
                    }

                    return tours;
                }
            }
        }


        public PostsAdsModel DieuKienNoiDia()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionStringTour))
                {
                    string query = @"SELECT top 1 * FROM Posts WHERE CategoryID = 8";
                    PostsAdsModel postsAdsModel = db.QueryFirstOrDefault<PostsAdsModel>(query);
                    return postsAdsModel;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Lỗi trong quá trình truy vấn dữ liệu", ex);
            }
        }

        //public bool SendMailBookingTourHot(string trienkhai_id, string tour_id, string name, string phone, string email, string note, string tieuchuan, string sl_lon, string sl_tre_em, string sl_em_be, string tourname, string ngaydi, string ngayve, string tourcode, decimal hoahong, decimal tongtien, string ghichu, string LoaiTour, decimal price, decimal vat)
        //{

        //    MailMessage mail = new MailMessage(baoNS_BookingTourStatus.username, email);
        //    mail.From = new MailAddress(baoNS_BookingTourStatus.username, "ENVIET GROUP");
        //    SmtpClient client = new SmtpClient();
        //    client.EnableSsl = true;
        //    client.Port = baoNS_BookingTourStatus.port;
        //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    client.UseDefaultCredentials = false;
        //    client.Credentials = new NetworkCredential(baoNS_BookingTourStatus.username, new DBase().Decrypt(baoNS_BookingTourStatus.password, "vodacthe", true));
        //    client.Host = baoNS_BookingTourStatus.host;
        //    string Dieukien = "";
        //    PostsAdsModel postsAdsModel = DieuKienNoiDia();
        //    Dieukien = postsAdsModel.Description;
        //    LoaiTour = "[TOUR Flight VN]";

        //    string subject = LoaiTour + " XÁC NHẬN GIỮ CHỖ ";

        //    try
        //    {
        //        string mailCC = baoNS_BookingTourStatus.CC;

        //        if (note == "ENVIETTESTING")
        //        {
        //            mail.CC.Add("it05@enviet-group.com");
        //        }
        //        else
        //        {
        //            mail.CC.Add(mailCC);
        //        }
        //    }
        //    catch { }




        //    mail.Subject = subject;

        //    string mailBody;

        //    var webRequest = WebRequest.Create(baoNS_BookingTourStatus.templateUrl);
        //    using (var response = webRequest.GetResponse())
        //    using (var content = response.GetResponseStream())
        //    using (var reader = new StreamReader(content))
        //    { mailBody = reader.ReadToEnd(); }
        //    mailBody = mailBody.Replace("$_TenTour", tourname);
        //    mailBody = mailBody.Replace("$_MaTour", trienkhai_id);
        //    mailBody = mailBody.Replace("$_Ngaygui", DateTime.Now.ToString("dd/MM/yyyy"));
        //    mailBody = mailBody.Replace("$_Fullname", name);
        //    mailBody = mailBody.Replace("$_ProductName", tourname);
        //    mailBody = mailBody.Replace("$_Code", tourcode);
        //    mailBody = mailBody.Replace("$_TinhTrang", "đã đặt");
        //    mailBody = mailBody.Replace("$_NgayDi", ngaydi);
        //    mailBody = mailBody.Replace("$_NgayVe", ngayve);
        //    mailBody = mailBody.Replace("$_hotelTour", tieuchuan);
        //    mailBody = mailBody.Replace("$_adult", sl_lon);
        //    mailBody = mailBody.Replace("$_child", sl_tre_em);
        //    mailBody = mailBody.Replace("$_kid", sl_em_be);
        //    mailBody = mailBody.Replace("$_GhiChu", note);
        //    mailBody = mailBody.Replace("$_price", string.Format("{0:0,0}", price).Replace(".", ","));
        //    mailBody = mailBody.Replace("$_commission", string.Format("{0:0,0}", hoahong).Replace(".", ","));
        //    mailBody = mailBody.Replace("$_totalprice", string.Format("{0:0,0}", tongtien).Replace(".", ","));
        //    mailBody = mailBody.Replace("$_Dieukien", Dieukien);
        //    if (vat == 0)
        //    {
        //        mailBody = mailBody.Replace("$_vat", "0");
        //    }
        //    else
        //    {
        //        mailBody = mailBody.Replace("$_vat", string.Format("{0:0,0}", vat).Replace(".", ","));
        //    }
        //    mail.Body = mailBody;
        //    mail.IsBodyHtml = true;
        //    client.Send(mail);
        //    return true;
        //}


       
        public bool SaveLationReasonTourHot(string tourID, string lydohuy)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                try
                {
                    dbConnection.Open();

                    int rowsAffected = dbConnection.Execute(
                        "UPDATE TourHotBooking SET lydohuy = @lydohuy WHERE tourID = @tourID",
                        new { lydohuy, tourID }
                    );
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }
        }
        public List<ListNhanVienDuLich> DSDuLich()
        {
            List<ListNhanVienDuLich> result = new List<ListNhanVienDuLich>();
            string sql_NoiDung = " SELECT TENDANGNHAP,Ten as TENNV, MaNV FROM DM_NV WHERE MaPhongBan='DL' AND TINHTRANG=1 ";
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListNhanVienDuLich ten = new ListNhanVienDuLich();
                        ten.Ten = dt.Rows[i]["TenNV"].ToString();
                        ten.MaNV = dt.Rows[i]["MaNV"].ToString();
                        result.Add(ten);
                    }
                }
            }
            return result;
        }


        public bool AddTourHot(
    string tour_name, string[] diem_den, string chuyen_bay, string loai_xe, string so_ngay, string so_dem, string loai_tour,
    IFormFile[] files, string ngay_di, string ngay_ve, string ngay_dong_tour, string hdv, string sales_name, string sales_email, string sales_phoneNumber, string[] gia_loai,
    string[] gia_nguoi_lon, string[] gia_tre_em, string[] gia_em_be, string[] phu_thu_don, string[] phu_thu_quoctich, string[] hh_gia_nguoi_lon, string[] hh_gia_tre_em,
    string[] hh_gia_em_be, string[] km_gia_nguoi_lon, string[] km_gia_tre_em, string[] km_gia_em_be, string note, string short_notes, string long_notes,
    List<bool> mainImages, List<IFormFile> imageFiles, string diem_di, string tong, string da_dat, string giu_cho, string diem_don, string gan_co)
        {
            string tour_id = IDTourhot();
            string CreateDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");


            string RemoveCommas(string input) => input.Replace(",", "");
            decimal HandleNullOrEmpty(string input)
            {
                if (string.IsNullOrEmpty(input))
                    return 0;
                return decimal.Parse(RemoveCommas(input));
            }

            using (IDbConnection db = new SqlConnection(_connectionStringTourhot))
            {
                string queryTours = @"INSERT INTO tours (tour_id, ghi_chu, loai_xe, hdv, ngay_di, ngay_ve, createdate, short_notes, long_notes, active, tong, da_dat, giu_cho, ngay_dong_tour,name_tour,diem_don,diem_di,diem_den,so_ngay,so_dem,FlagID) 
                              VALUES (@tour_id, @ghi_chu, @loai_xe, @hdv, @ngay_di, @ngay_ve, @CreateDate, @short_notes, @long_notes, 1, @tong, @da_dat, @giu_cho, @ngay_dong_tour,@name_tour,@diem_don,@diem_di,@diem_den,@so_ngay,@so_dem,@FlagID)";

                try
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        int rowsAffectedTours = db.Execute(queryTours, new
                        {
                            tour_id,
                            ghi_chu = note,
                            loai_xe,
                            hdv,
                            ngay_di,
                            ngay_ve,
                            CreateDate,
                            short_notes,
                            long_notes,
                            tong,
                            da_dat,
                            giu_cho,
                            ngay_dong_tour,
                            name_tour = tour_name,
                            diem_don,
                            diem_di,
                            diem_den = string.Join(",", diem_den),
                            so_ngay = int.Parse(so_ngay),
                            so_dem = int.Parse(so_dem),
                            FlagID = gan_co,
                        }, transaction);



                        string querySale = @"INSERT INTO sale (tour_id, name, email, phone) 
                                     VALUES (@tour_id, @name, @email, @phone)";
                        int rowsAffectedSale = db.Execute(querySale, new
                        {
                            tour_id,
                            name = sales_name,
                            email = sales_email,
                            phone = sales_phoneNumber
                        }, transaction);

                        bool allGiaInserted = true;
                        for (int i = 0; i < gia_loai.Length; i++)
                        {
                            string queryGia = @"INSERT INTO gia (tour_id, gia_nguoi_lon, gia_tre_em, gia_em_be, phu_thu_don, phu_thu_quoctich, hh_gia_nguoi_lon, hh_gia_tre_em, hh_gia_em_be, km_gia_nguoi_lon, km_gia_tre_em, km_gia_em_be, loai_gia) 
                                        VALUES (@tour_id, @gia_nguoi_lon, @gia_tre_em, @gia_em_be, @phu_thu_don, @phu_thu_quoctich, @hh_gia_nguoi_lon, @hh_gia_tre_em, @hh_gia_em_be, @km_gia_nguoi_lon, @km_gia_tre_em, @km_gia_em_be, @loai_gia)";
                            int rowsAffectedGia = db.Execute(queryGia, new
                            {
                                tour_id,
                                gia_nguoi_lon = HandleNullOrEmpty(gia_nguoi_lon[i]),
                                gia_tre_em = HandleNullOrEmpty(gia_tre_em[i]),
                                gia_em_be = HandleNullOrEmpty(gia_em_be[i]),
                                phu_thu_don = HandleNullOrEmpty(phu_thu_don[i]),
                                phu_thu_quoctich = HandleNullOrEmpty(phu_thu_quoctich[i]),
                                hh_gia_nguoi_lon = HandleNullOrEmpty(hh_gia_nguoi_lon[i]),
                                hh_gia_tre_em = HandleNullOrEmpty(hh_gia_tre_em[i]),
                                hh_gia_em_be = HandleNullOrEmpty(hh_gia_em_be[i]),
                                km_gia_nguoi_lon = HandleNullOrEmpty(km_gia_nguoi_lon[i]),
                                km_gia_tre_em = HandleNullOrEmpty(km_gia_tre_em[i]),
                                km_gia_em_be = HandleNullOrEmpty(km_gia_em_be[i]),
                                loai_gia = gia_loai[i]
                            }, transaction);

                            if (rowsAffectedGia <= 0)
                            {
                                allGiaInserted = false;
                                break;
                            }
                        }

                        bool allImagesInserted = true;
                        for (int i = 0; i < mainImages.Count; i++)
                        {
                            string imgUrl = UploadImg(imageFiles[i]);
                            bool mainImg = mainImages[i];

                            string queryImg = @"INSERT INTO img (tour_id, name, url, mainimg) 
                                        VALUES (@tour_id, @name, @url, @mainimg)";
                            int rowsAffectedImg = db.Execute(queryImg, new
                            {
                                tour_id,
                                name = imageFiles[i].FileName,
                                url = imgUrl,
                                mainimg = mainImg
                            }, transaction);

                            if (rowsAffectedImg <= 0)
                            {
                                allImagesInserted = false;
                                break;
                            }
                        }

                        bool filesUploaded = true;
                        foreach (var file in files)
                        {
                            string fileUrl = UploadImg(file);
                            string queryFile = @"INSERT INTO [file] (tour_id, name, url) VALUES (@tour_id, @name, @url)";
                            int rowsAffectedFile = db.Execute(queryFile, new
                            {
                                tour_id,
                                name = file.FileName,
                                url = fileUrl
                            }, transaction);

                            if (rowsAffectedFile <= 0)
                            {
                                filesUploaded = false;
                                break;
                            }
                        }

                        if (rowsAffectedTours > 0 && rowsAffectedSale > 0 && allGiaInserted && filesUploaded)
                        {
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }
        public bool EditTourHot(
    string tour_name, string[] diem_den, string chuyen_bay, string loai_xe, string so_ngay, string so_dem, string loai_tour,
    IFormFile[] files, string ngay_di, string ngay_ve, string ngay_dong_tour, string hdv, string sales_name, string sales_email, string sales_phoneNumber, string[] gia_loai,
    string[] gia_nguoi_lon, string[] gia_tre_em, string[] gia_em_be, string[] phu_thu_don, string[] phu_thu_quoctich, string[] hh_gia_nguoi_lon, string[] hh_gia_tre_em,
    string[] hh_gia_em_be, string[] km_gia_nguoi_lon, string[] km_gia_tre_em, string[] km_gia_em_be, string note, string short_notes, string long_notes,
    List<bool> mainImages, List<IFormFile> imageFiles, List<bool> mainImgs, List<string> imagesURL, string diem_di, string tong, string da_dat, string giu_cho, string diem_don, string tour_id, string gan_co)
        {
            string RemoveCommas(string input) => input.Replace(",", "");
            decimal HandleNullOrEmpty(string input)
            {
                if (string.IsNullOrEmpty(input))
                    return 0;
                return decimal.Parse(RemoveCommas(input));
            }

            using (IDbConnection db = new SqlConnection(_connectionStringTourhot))
            {
                string queryDeleteRelatedTables = @"DELETE FROM sale WHERE tour_id = @tour_id;
                                            DELETE FROM gia WHERE tour_id = @tour_id;
                                            DELETE FROM img WHERE tour_id = @tour_id;";
                string queryTours = @"UPDATE tours SET 
                          ghi_chu = @ghi_chu, loai_xe = @loai_xe, hdv = @hdv, ngay_di = @ngay_di, ngay_ve = @ngay_ve, 
                          short_notes = @short_notes, long_notes = @long_notes, 
                          tong = @tong, da_dat = @da_dat, giu_cho = @giu_cho, ngay_dong_tour = @ngay_dong_tour, 
                          name_tour = @name_tour, diem_don = @diem_don, diem_di = @diem_di, diem_den = @diem_den, 
                          so_ngay = @so_ngay, so_dem = @so_dem, FlagID= @FlagID
                          WHERE tour_id = @tour_id";

                string querySale = @"INSERT INTO sale (tour_id, name, email, phone) 
                         VALUES (@tour_id, @name, @email, @phone)";

                string queryGia = @"INSERT INTO gia (tour_id, gia_nguoi_lon, gia_tre_em, gia_em_be, phu_thu_don, phu_thu_quoctich, hh_gia_nguoi_lon, hh_gia_tre_em, hh_gia_em_be, km_gia_nguoi_lon, km_gia_tre_em, km_gia_em_be, loai_gia) 
                        VALUES (@tour_id, @gia_nguoi_lon, @gia_tre_em, @gia_em_be, @phu_thu_don, @phu_thu_quoctich, @hh_gia_nguoi_lon, @hh_gia_tre_em, @hh_gia_em_be, @km_gia_nguoi_lon, @km_gia_tre_em, @km_gia_em_be, @loai_gia)";

                string queryImg = @"INSERT INTO img (tour_id, name, url, mainimg) 
                        VALUES (@tour_id, @name, @url, @mainimg)";

                // Cập nhật thông tin file
                string queryFileUpdate = @"UPDATE [file] SET url = @url, name = @name 
                         WHERE tour_id = @tour_id";

                try
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        // Xóa các bản ghi liên quan
                        db.Execute(queryDeleteRelatedTables, new { tour_id }, transaction);

                        // Cập nhật bảng tours
                        int rowsAffectedTours = db.Execute(queryTours, new
                        {
                            tour_id,
                            ghi_chu = note,
                            loai_xe,
                            hdv,
                            ngay_di,
                            ngay_ve,
                            short_notes,
                            long_notes,
                            tong,
                            da_dat,
                            giu_cho,
                            ngay_dong_tour,
                            name_tour = tour_name,
                            diem_don,
                            diem_di,
                            diem_den = string.Join(",", diem_den),
                            so_ngay = int.Parse(so_ngay),
                            so_dem = int.Parse(so_dem),
                            FlagID = gan_co,
                        }, transaction);

                        // Chèn vào bảng sale
                        int rowsAffectedSale = db.Execute(querySale, new
                        {
                            tour_id,
                            name = sales_name,
                            email = sales_email,
                            phone = sales_phoneNumber
                        }, transaction);

                        // Chèn vào bảng gia
                        bool allGiaInserted = true;
                        for (int i = 0; i < gia_loai.Length; i++)
                        {
                            int rowsAffectedGia = db.Execute(queryGia, new
                            {
                                tour_id,
                                gia_nguoi_lon = HandleNullOrEmpty(gia_nguoi_lon[i]),
                                gia_tre_em = HandleNullOrEmpty(gia_tre_em[i]),
                                gia_em_be = HandleNullOrEmpty(gia_em_be[i]),
                                phu_thu_don = HandleNullOrEmpty(phu_thu_don[i]),
                                phu_thu_quoctich = HandleNullOrEmpty(phu_thu_quoctich[i]),
                                hh_gia_nguoi_lon = HandleNullOrEmpty(hh_gia_nguoi_lon[i]),
                                hh_gia_tre_em = HandleNullOrEmpty(hh_gia_tre_em[i]),
                                hh_gia_em_be = HandleNullOrEmpty(hh_gia_em_be[i]),
                                km_gia_nguoi_lon = HandleNullOrEmpty(km_gia_nguoi_lon[i]),
                                km_gia_tre_em = HandleNullOrEmpty(km_gia_tre_em[i]),
                                km_gia_em_be = HandleNullOrEmpty(km_gia_em_be[i]),
                                loai_gia = gia_loai[i]
                            }, transaction);

                            if (rowsAffectedGia <= 0)
                            {
                                allGiaInserted = false;
                                break;
                            }
                        }

                        // Tạo danh sách hình ảnh bao gồm cả mới và cũ
                        List<Image> ListImg = new List<Image>();

                        // Thêm hình ảnh mới
                        for (int i = 0; i < imageFiles.Count; i++)
                        {
                            Image img = new Image();
                            img.MainImage = mainImages[i];
                            img.ImageURL = UploadImg(imageFiles[i]);
                            ListImg.Add(img);
                        }

                        // Thêm hình ảnh cũ
                        for (int i = 0; i < imagesURL.Count; i++)
                        {
                            Image img = new Image();
                            img.MainImage = mainImgs[i];
                            img.ImageURL = imagesURL[i];
                            ListImg.Add(img);
                        }

                        // Thêm vào bảng img
                        bool allImagesInserted = true;
                        foreach (var img in ListImg)
                        {
                            int rowsAffectedImg = db.Execute(queryImg, new
                            {
                                tour_id,
                                name = Path.GetFileName(img.ImageURL),
                                url = img.ImageURL,
                                mainimg = img.MainImage
                            }, transaction);

                            if (rowsAffectedImg <= 0)
                            {
                                allImagesInserted = false;
                                break;
                            }
                        }

                        // Cập nhật thông tin file
                        if (files.Length > 0)
                        {
                            var file = files[0]; // Chỉ lấy file đầu tiên
                            string fileUrl = UploadImg(file);

                            int rowsAffectedFileUpdate = db.Execute(queryFileUpdate, new
                            {
                                tour_id,
                                name = file.FileName,
                                url = fileUrl
                            }, transaction);

                            if (rowsAffectedFileUpdate <= 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }

                        if (rowsAffectedTours > 0 && rowsAffectedSale > 0 && allGiaInserted && allImagesInserted)
                        {
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
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




        public string IDTourhot()
        {
            try
            {
                string ID = "";
                string searchPattern = DateTime.Now.ToString("ddMMyy");
                string sql = @"SELECT TOP 1 tour_id  
                  FROM tours  
                  WHERE tour_id LIKE @searchPattern 
                  ORDER BY createdate DESC";
                //string server_EV_SERVICES = "Data Source=27.71.232.40,1453;Initial Catalog=TOURHOT;User ID=sa;Password=EnViet@123;";
                using (var connection = new SqlConnection(_connectionStringTourhot))
                {
                    connection.Open();
                    var result = connection.QueryFirstOrDefault<string>(sql, new { searchPattern = '%' + searchPattern + '%' });
                    if (!string.IsNullOrEmpty(result))
                    {
                        int STT = 0;
                        if (int.TryParse(result.Substring(10, 3), out STT))
                        {
                            STT += 1;
                            ID = $"SPTH{DateTime.Now.ToString("ddMMyy")}{STT:D3}";
                        }
                    }


                    if (string.IsNullOrEmpty(ID) || ID.Substring(4, 4) != DateTime.Now.ToString("ddMM"))
                    {
                        ID = $"SPTH{DateTime.Now.ToString("ddMMyy")}001";
                    }
                }

                return ID;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<TourEV> ListTourEV()
        {
            List<TourEV> listtour = new List<TourEV>();
            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                dbConnection.Open();
                listtour = dbConnection.Query<TourEV>(
                    "SELECT * FROM tours",
                    new { }
                ).ToList();
            }

            return listtour;
        }

        public bool DeleteImg(int img_id)
        {

            try
            {
                using (var conn = new SqlConnection(_connectionStringTourhot))
                {

                    conn.Open();
                    string sqlType = "DELETE FROM img WHERE img_id = @img_id";


                    var rowsAffected = conn.Execute(sqlType, new { img_id }, commandTimeout: 30, commandType: CommandType.Text);
                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {

                        return false;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Xử lý lỗi SQL

                return false;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chung

                return false;
            }
        }

        public List<BookingInfoModel> SearchBookingTourHot(string fromDate, string toDate, int page, int pageSize)
        {
            List<BookingInfoModel> listBooking = new List<BookingInfoModel>();
            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                dbConnection.Open();
                int offset = (page - 1) * pageSize;
                listBooking = dbConnection.Query<BookingInfoModel>(
                    "SELECT * FROM TourHotBooking WHERE CreateDate BETWEEN @FromDate AND @ToDate ORDER BY CreateDate DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY",
                    new { FromDate = fromDate, ToDate = toDate, Offset = offset, PageSize = pageSize }
                ).ToList();
            }

            return listBooking;
        }
        public int GetTotalSearchBookingsCountTourHot(string fromDate, string toDate)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                dbConnection.Open();
                int count = dbConnection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM TourHotBooking WHERE createdate BETWEEN @FromDate AND @ToDate",
                    new { FromDate = fromDate, ToDate = toDate }
                );
                return count;
            }
        }
        public List<BookingInfoModel> ListBookingTourHot(int page, int pageSize)
        {
            List<BookingInfoModel> listBooking = new List<BookingInfoModel>();
            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                dbConnection.Open();
                int offset = (page - 1) * pageSize;
                listBooking = dbConnection.Query<BookingInfoModel>(
                    "SELECT * FROM TourHotBooking ORDER BY CreateDate DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY",
                    new { Offset = offset, PageSize = pageSize }
                ).ToList();
            }

            return listBooking;
        }
        public int GetTotalBookingsCountTourHot()
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                dbConnection.Open();
                int count = dbConnection.ExecuteScalar<int>("SELECT COUNT(*) FROM TourHotBooking");
                return count;
            }
        }
        public DetailTourModel NewDetailBookingTourHot(string khoachinh)
        {
            DetailTourModel Booking = new DetailTourModel();
            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                dbConnection.Open();
                Booking = dbConnection.QuerySingle<DetailTourModel>(
                    "SELECT * FROM TourHotBooking where tourID='" + khoachinh + "'"
                );
            }
            return Booking;
        }
        public bool ChangeBookingStatusTourHot(string IDStatus, string tourID)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                try
                {
                    dbConnection.Open();

                    int rowsAffected = dbConnection.Execute(
                        "UPDATE TourHotBooking SET IDStatus = @IDStatus WHERE tourID = @tourID",
                        new { IDStatus, tourID }
                    );
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }
        }
        public bool AddNguoiNhanTourHot(string tourID, string NguoiNhan, string NguoiChuyen)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                try
                {
                    dbConnection.Open();

                    int rowsAffected = dbConnection.Execute(
                        "UPDATE TourHotBooking SET NguoiNhan = @NguoiNhan, NguoiChuyen = @NguoiChuyen WHERE tourID = @tourID",
                        new { NguoiNhan, NguoiChuyen, tourID }
                    );
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }
        }

        public TourEV DetailTourEV(string id)
        {

            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                dbConnection.Open();

                var sql = @"
                SELECT * FROM tours WHERE Tour_Id = @TourId;              
                SELECT * FROM sale WHERE Tour_Id = @TourId;
                SELECT * FROM img WHERE Tour_Id = @TourId;
                SELECT * FROM gia WHERE Tour_Id = @TourId;
                SELECT * FROM [file] WHERE Tour_Id = @TourId;";

                using (var multi = dbConnection.QueryMultiple(sql, new { TourId = id }))
                {
                    var tour = multi.Read<TourEV>().FirstOrDefault();
                    if (tour != null)
                    {

                        tour.Sale = multi.Read<SaleEV>().FirstOrDefault();
                        tour.Imgs = multi.Read<ImgEV>().ToList();
                        tour.Gias = multi.Read<GiaEV>().ToList();
                        tour.Files = multi.Read<FileEV>().ToList();
                    }

                    return tour;
                }
            }
        }
        public DetailTourHotBooking GetBookingDetail(string tourID)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionStringTourhot))
            {
                dbConnection.Open();

                var sql = "SELECT * FROM TourHotBooking WHERE tourID = @tourID";

                var booking = dbConnection.QueryFirstOrDefault<DetailTourHotBooking>(sql, new { tourID });

                return booking;
            }
        }
        //public bool SendMailSuccessBookingTourHot(string email, string TourCode, string LoaiTour, string note)
        //{

        //    MailMessage mail = new MailMessage(baoNS_SuccessBooking.username, email);
        //    mail.From = new MailAddress(baoNS_SuccessBooking.username, "ENVIET GROUP");
        //    SmtpClient client = new SmtpClient();
        //    client.EnableSsl = true;
        //    client.Port = baoNS_SuccessBooking.port;
        //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    client.UseDefaultCredentials = false;
        //    client.Credentials = new NetworkCredential(baoNS_SuccessBooking.username, new DBase().Decrypt(baoNS_SuccessBooking.password, "vodacthe", true));
        //    client.Host = baoNS_SuccessBooking.host;

        //    LoaiTour = "[TOUR Flight VN]";

        //    string subject = LoaiTour + " CẢM ƠN QUÝ KHÁCH HÀNG";

        //    try
        //    {
        //        string mailCC = baoNS_BookingTourStatus.CC;

        //        if (note == "ENVIETTESTING")
        //        {
        //            mail.CC.Add("it05@enviet-group.com");
        //        }
        //        else
        //        {
        //            mail.CC.Add(mailCC);
        //        }
        //    }
        //    catch { }




        //    mail.Subject = subject;

        //    string mailBody;

        //    var webRequest = WebRequest.Create(baoNS_SuccessBooking.templateUrl);
        //    using (var response = webRequest.GetResponse())
        //    using (var content = response.GetResponseStream())
        //    using (var reader = new StreamReader(content))
        //    { mailBody = reader.ReadToEnd(); }
        //    mailBody = mailBody.Replace("$_Ngaygui", DateTime.Now.ToString("dd/MM/yyyy"));
        //    mailBody = mailBody.Replace("$_Code", TourCode);
        //    mail.Body = mailBody;
        //    mail.IsBodyHtml = true;
        //    client.Send(mail);
        //    return true;
        //}
        public bool ChangeActiveTourHot(int ID, int Active)
        {
            int x = 0;
            string sql = @"UPDATE [tours] SET [active] = '" + Active + "' where id = '" + ID + "'";
            using (var conn = new SqlConnection(_connectionStringTourhot))
            {
                x = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (x > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
