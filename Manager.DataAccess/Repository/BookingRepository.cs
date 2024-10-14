using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using Manager.Model.Models.ViewModel;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class BookingRepository
    {
        public static int SMS_ = 0;
        Mail baoBK = new Mail("EVM_BOOKINGXUATVE");
        DBase db = new DBase();
        private string server_AIRLINE_BOOKING;
        public BookingRepository(IConfiguration configuration)
        {
            server_AIRLINE_BOOKING = configuration.GetConnectionString("SQL_AIRLINE_BOOKING");
        }
        public BookingViewModel Booking(string server_AIRLINE_BOOKING, string server_EV_MAIN)
        {
            BookingViewModel result = new BookingViewModel();
            List<Booking> ListBooking = new List<Booking>();
            List<SelectOptionValue> ListTinhTrang = new List<SelectOptionValue>();
            List<SelectOptionValue> ListHTThanhToan = new List<SelectOptionValue>();
            List<SelectOptionValue> ListNguoiThayDoi = new List<SelectOptionValue>();
            string abt = DateTime.Now.ToString("dd/MM/yyy");
            string sql = " SELECT *,0 as STT,(select top 1 [NAME] from Booking_Status where [ID]=Booking.Booking_Status) as BkStatus" +
                              ",(SELECT CASE WHEN Code_OneWay='Waiting' OR Code_RoundTrip='Waiting' THEN '1' ELSE '0' end )AS codeWait,(SELECT case when sum(BagGage_OneWay)>1 then '1' else '0' end as N01 FROM DETAIL_BOOKING where Order_ID=BOOKING.Order_ID) as N01,(SELECT case when sum(BagGage_RoundTrip)>1 then '1' else '0' end as N02 FROM DETAIL_BOOKING where Order_ID=BOOKING.Order_ID) as N02 FROM BOOKING WHERE convert(nvarchar(10),DateBook,103) = '" + abt.Replace('-', '/') + "' AND Booking_Status!='5'  ORDER BY DateBook DESC ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                ListBooking = (List<Booking>)conn.Query<Booking>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            string SqlTinhTrang = " SELECT ID,NAME FROM BOOKING_STATUS ORDER BY ID ASC ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                ListTinhTrang = (List<SelectOptionValue>)conn.Query<SelectOptionValue>(SqlTinhTrang, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            string SqlHTThanhToan = " SELECT ID,NAME FROM PAYMENT_METHOD ORDER BY ID ASC ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                ListHTThanhToan = (List<SelectOptionValue>)conn.Query<SelectOptionValue>(SqlHTThanhToan, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            string SqlNguoiThayDoi = "SELECT Ten as Name,TenDangNhap as ID FROM DM_NV WHERE MABOPHAN = 'CC' AND TinhTrang='1' ";
            using (var conn = new SqlConnection(server_EV_MAIN))
            {
                ListNguoiThayDoi = (List<SelectOptionValue>)conn.Query<SelectOptionValue>(SqlNguoiThayDoi, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }

            result.ListBooking = ListBooking;
            result.ListTinhTrang = ListTinhTrang;
            result.ListHTThanhToan = ListHTThanhToan;
            result.ListNguoiThayDoi = ListNguoiThayDoi;
            return result;
        }
        public BookingViewModel SearchBooking(string server_AIRLINE_BOOKING, string server_EV_MAIN, BookingSearch bookingSearch)
        {
            string StrWhere = Param(bookingSearch);
            BookingViewModel result = new BookingViewModel();
            List<Booking> ListBooking = new List<Booking>();
            List<SelectOptionValue> ListTinhTrang = new List<SelectOptionValue>();
            List<SelectOptionValue> ListHTThanhToan = new List<SelectOptionValue>();
            List<SelectOptionValue> ListNguoiThayDoi = new List<SelectOptionValue>();

            string sql = " SELECT TOP 500 *,0 as STT,(SELECT top 1 [NAME] FROM Booking_Status WHERE [ID]=Booking.Booking_Status) as BkStatus,(SELECT CASE WHEN Code_OneWay='Waiting' OR Code_RoundTrip='Waiting' THEN '1' ELSE '0' end )AS codeWait,(SELECT case when sum(BagGage_OneWay)>1 then '1' else '0' end as N01   FROM DETAIL_BOOKING where Order_ID=BOOKING.Order_ID) as N01,(SELECT case when sum(BagGage_RoundTrip)>1 then '1' else '0' end as N02 FROM DETAIL_BOOKING where Order_ID=BOOKING.Order_ID) as N02 FROM BOOKING WHERE " + StrWhere + " ORDER BY RowID DESC ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                ListBooking = (List<Booking>)conn.Query<Booking>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            string SqlTinhTrang = " SELECT ID,NAME FROM BOOKING_STATUS ORDER BY ID ASC ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                ListTinhTrang = (List<SelectOptionValue>)conn.Query<SelectOptionValue>(SqlTinhTrang, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            string SqlHTThanhToan = " SELECT ID,NAME FROM PAYMENT_METHOD ORDER BY ID ASC ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                ListHTThanhToan = (List<SelectOptionValue>)conn.Query<SelectOptionValue>(SqlHTThanhToan, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            string SqlNguoiThayDoi = "SELECT Ten as Name,TenDangNhap as ID FROM DM_NV WHERE MABOPHAN = 'CC' AND TinhTrang='1' ";
            using (var conn = new SqlConnection(server_EV_MAIN))
            {
                ListNguoiThayDoi = (List<SelectOptionValue>)conn.Query<SelectOptionValue>(SqlNguoiThayDoi, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }

            result.ListBooking = ListBooking;
            result.ListTinhTrang = ListTinhTrang;
            result.ListHTThanhToan = ListHTThanhToan;
            result.ListNguoiThayDoi = ListNguoiThayDoi;
            return result;
        }
        public string Param(BookingSearch bookingSearch)
        {
            string Param = " 1=1 ";
            if (bookingSearch.Booking != null)
            {
                Param = "  Order_ID='" + bookingSearch.Booking.Trim() + "'";
            }
            if (bookingSearch.NguoiLH != null)
            {
                Param = " Contact_Name like N'%" + bookingSearch.NguoiLH.Trim() + "%'";
            }
            if (bookingSearch.Phone != null)
            {
                Param = " Contact_Phone='" + bookingSearch.Phone.Trim() + "'";
            }
            if (bookingSearch.Email != null)
            {
                Param = " Contact_Email='" + bookingSearch.Email.Trim() + "'";
            }
            if (bookingSearch.cal_from != null && bookingSearch.cal_to != null)
            {
                DateTime t = DateTime.ParseExact(bookingSearch.cal_from, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime t2 = DateTime.ParseExact(bookingSearch.cal_to, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Param += " AND CONVERT(datetime,CONVERT(nvarchar(10),DateBook,101)) >=  CONVERT(datetime,'" + t.ToString("yyyy-MM-dd") + "') AND CONVERT(datetime,CONVERT(nvarchar(10),DateBook,101)) <= CONVERT(datetime,'" + t2.ToString("yyyy-MM-dd") + "')";
            }
            if (bookingSearch.cal_from != null && bookingSearch.cal_to == null)
            {
                DateTime t = DateTime.ParseExact(bookingSearch.cal_from, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Param += " AND CONVERT(datetime,CONVERT(nvarchar(10),DateBook,101)) = CONVERT(datetime,'" + t.ToString("yyyy-MM-dd") + "')";
            }
            if (bookingSearch.Tinhtrang != "0")
            {
                Param += " AND Booking_Status = '" + bookingSearch.Tinhtrang + "' ";
            }
            if (bookingSearch.Thanhtoan != "0")
            {
                Param += " AND Payment_Method = '" + bookingSearch.Thanhtoan + "'";
            }
            if (bookingSearch.NguoiThayDoi != "0")
            {
                Param += " AND UserUpdate = '" + bookingSearch.NguoiThayDoi + "'";
            }
            if (bookingSearch.Xacnhan != "0")
            {
                Param += " AND Transaction_Status = '" + bookingSearch.Xacnhan + "'";
            }
            if (bookingSearch.Thietbi != "0")
            {
                if (bookingSearch.Thietbi == "APP_NEW")
                {
                    Param += " AND (TypeApp = 'APP_NEW' OR TypeApp = 'APP_IOS' OR TypeApp = 'APP_ANDROID')";
                }
                else
                {
                    Param += " AND TypeApp = '" + bookingSearch.Thietbi + "'";
                }
            }

            return Param;
        }
        public DetailBooking DetailBooking(string server_AIRLINE_BOOKING, string server_EV_MAIN, string ID, string TenDangNhap)
        {
            DetailBooking result = new DetailBooking();
            List<SelectOptionValue> ListTinhTrang = new List<SelectOptionValue>();
            List<SelectOptionValue> ListHTThanhToan = new List<SelectOptionValue>();
            List<SelectOptionValue> ListNguoiThayDoi = new List<SelectOptionValue>();
            List<ThongTinKhach> ListThongTinKhach = new List<ThongTinKhach>();
            List<DaXuatVe> ListDaXuatVe = new List<DaXuatVe>();
            List<Log> ListLogTemp = new List<Log>();
            List<Log> ListLog = new List<Log>();
            int result_NguoiThayDoi = 0;
            string SqlNguoiThayDoi = "SELECT Ten as Name,TenDangNhap as ID FROM DM_NV WHERE MaPhongBan = 'PV' and TinhTrang='1' ";
            using (var conn = new SqlConnection(server_EV_MAIN))
            {
                ListNguoiThayDoi = (List<SelectOptionValue>)conn.Query<SelectOptionValue>(SqlNguoiThayDoi, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            string sql = @" SELECT 
                                 OrderID_SMS=Order_ID,
                                 HanhTrinh_SMS = (case when TypeFlight = 'RoundTrip' then Departure + '-' + Arrival else  Departure + '-' + Arrival + '-' + Departure end),
                                 NguoiLienHe_SMS = Contact_Name,
                                 DienThoai_SMS = Contact_Phone,
                                 Gio_SMS = '',
                                 NgayThanhToan_SMS = '',
                                 GiaVeMoi_SMS = '',

                                 LoaiVe = N'Quốc Nội',
                                 HinhThucThanhToan = (select top 1 [NAME] from PAYMENT_METHOD where [ID]=Booking.PAYMENT_METHOD),
                                 NganHangChuyenKhoan = Bank_Transaction,
                                 HanhTrinh = TypeFlight,
                                 DiemDi = Departure,
                                 DiemDen = Arrival,
                                 LuotDi = NoFlight_OneWay,
                                 GhiChu = Booking_Request,
                                 ThietBi = TypeApp,
                                 NguoiThayDoi = UserUpdate,
                                 DateLimit = DateSMS,
                                 TimeLimit = HourSMS,
                                 MaGiaoDich = Code_Transaction,
                                 NganHangTTTT = Bank_Name,
                                 RemarkKeToan = Transaction_Note,
                                 TinhTrang = (select top 1 [NAME] from Booking_Status where [ID]=Booking.Booking_Status),
                                 CheckTinhTrangTT = Payment_Status,
                                 SoTienChuyenKhoan = Price_Transaction,
                                 MaDatCho_LD = Code_OneWay,
                                 MaDatCho_LV = Code_RoundTrip,
                                 LuotVe = NoFlight_RoundTrip,
                                 Remark = Remark,
                                 NgonNgu = (CASE WHEN Language = 'VI' THEN N'Tiếng Việt' ELSE N'Tiếng Anh' END),
                                 NgayBook = DateBook,
                                 NgayThayDoi = DateUpdate,
                                 MaThamChieu = Code_Reference,
                                 SoTienGDTrucTuyen = Bank_Price,
                                 TongTienMoi = NewTotal,

                                 MaVoucher = Voucher,
                                 MaKeToan = '',
                                 NguoiLienHe = Contact_Name,
                                 Email = Contact_Email,
                                 ThanhPho = Contact_City,
                                 DonVi = '',
                                 GiamGia = '',
                                 DienThoai = Contact_Phone,
                                 DiaChi = Contact_Address,
                                 QuocGia = Contact_Country,

                                 Airline_LD = AirLines_OneWay,
                                 SoHieu_LD = NoFlight_OneWay,
                                 Hang_LD = CategoriesTicket_OneWay,
                                 Code_LD = Code_OneWay,
                                 DiemDi_LD = Departure,
                                 DiemDen_LD = Arrival,
                                 NgayDi_LD = DateDeparture,
                                 GioDi_LD = HourFrom_OneWay,
                                 GioDen_LD = HourTo_OneWay,
                                 GiaNet_LD = Price_OneWay,
                                 Thue_LD = TaxFee_OneWay,
                                 PhiDV_LD = ServiceFee_OneWay,
                                 Giam_LD = Discounts_OneWay,
                                 TongTien_LD = Total_OneWay,

                                 Airline_LV = AirLines_RoundTrip,
                                 SoHieu_LV = NoFlight_RoundTrip,
                                 Hang_LV = CategoriesTicket_RoundTrip,
                                 Code_LV = Code_RoundTrip,
                                 DiemDi_LV = (CASE WHEN TypeFlight = 'RoundTrip' THEN Arrival ELSE N'' END),
                                 DiemDen_LV = (CASE WHEN TypeFlight = 'RoundTrip' THEN Departure ELSE N'' END),
                                 NgayDi_LV = DateArrival,
                                 GioDi_LV = HourFrom_RoundTrip,
                                 GioDen_LV = HourTo_RoundTrip,
                                 GiaNet_LV = Price_RoundTrip,
                                 Thue_LV = TaxFee_RoundTrip,
                                 PhiDV_LV = ServiceFee_RoundTrip,
                                 Giam_LV = Discounts_RoundTrip,
                                 TongTien_LV = Total_RoundTrip,

                                 CongTy_HD = CompanyName_Bill,
                                 MST_HD = TaxCode_Bill,
                                 DiaChiCT_HD = AddressCompany_Bill,
                                 DiaChiHD_HD = Address_Bill,

                                 NguoiNhan_GV = UserPay,
                                 DiaChiGiaoVe_GV = AddressPay,
                                 DienThoai_GV = PhonePay,
                                 ThanhPho_GV = CityPay, 
                                 GiaoCho = Assigned
                                 FROM Booking Where Order_ID='" + ID + "' ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                result = conn.QueryFirst<DetailBooking>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                if (result.NguoiThayDoi == "" || result.NguoiThayDoi == null)
                {
                    for (int i = 0; i < ListNguoiThayDoi.Count; i++)
                    {
                        if (ListNguoiThayDoi[i].ID == TenDangNhap)
                        {
                            string updateNguoiThayDoi = "update Booking set UserUpdate = N'" + TenDangNhap + "'  where Order_ID= '" + ID + "'";
                            using (var conn1 = new SqlConnection(server_AIRLINE_BOOKING))
                            {
                                result_NguoiThayDoi = conn1.Execute(updateNguoiThayDoi, null, null, commandTimeout: 30, commandType: CommandType.Text);
                                conn.Dispose();
                            }
                            result.NguoiThayDoi = TenDangNhap;
                            break;
                        }
                    }
                }
                if (result.CheckTinhTrangTT == "False")
                {
                    result.TinhTrangThanhToan = "Chưa Thanh Toán";
                }
                else
                {
                    result.TinhTrangThanhToan = "Đã Thanh Toán";
                }
                if (result.HanhTrinh.Trim() == "RoundTrip")
                {
                    result.HanhTrinh = "Khứ hồi";
                }
                else
                {
                    result.HanhTrinh = "Một chiều";
                }
                conn.Dispose();
            }
            string SqlTinhTrang = " SELECT ID,NAME FROM BOOKING_STATUS ORDER BY ID ASC ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                ListTinhTrang = (List<SelectOptionValue>)conn.Query<SelectOptionValue>(SqlTinhTrang, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
                for (int i = 0; i < ListTinhTrang.Count; i++)
                {
                    if (ListTinhTrang[i].Name == result.TinhTrang)
                    {
                        ListTinhTrang[i].Select = "selected";
                    }
                }
            }
            string SqlHTThanhToan = " SELECT ID,NAME FROM PAYMENT_METHOD ORDER BY ID ASC ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                ListHTThanhToan = (List<SelectOptionValue>)conn.Query<SelectOptionValue>(SqlHTThanhToan, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
                for (int i = 0; i < ListHTThanhToan.Count; i++)
                {
                    if (ListHTThanhToan[i].Name == result.HinhThucThanhToan)
                    {
                        ListHTThanhToan[i].Select = "selected";
                    }
                }
            }

            string SqlQeu = " SELECT RowID, DanhXung = Gender,Ho = FirstName,Ten = LastName,NgaySinh = DateOfBirth,Code_LD = Code_OneWay,Code_LV = Code_RoundTrip,HanhLy_LD = BagGage_OneWay,HanhLy_LV = BagGage_RoundTrip  FROM DETAIL_BOOKING Where Order_ID='" + ID + "' ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                ListThongTinKhach = (List<ThongTinKhach>)conn.Query<ThongTinKhach>(SqlQeu, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            string SqlLog = "SELECT * FROM Event_Log WHERE Order_ID='" + ID + "' ORDER BY Row_ID DESC";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                ListLogTemp = (List<Log>)conn.Query<Log>(SqlLog, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            for (int i = 0; i < ListLogTemp.Count; i++)
            {
                string[] DtProti = ListLogTemp[i].Properties.ToString().Split('|');
                string[] DtOvalues = ListLogTemp[i].OldValues.ToString().Split('|');
                string[] DtNValues = ListLogTemp[i].NewValues.ToString().Split('|');
                string StrUserUp = ListLogTemp[i].UserUpdate.ToString();
                string StrDateTime = ListLogTemp[i].DateUpdate.ToString();
                for (int j = 0; j < DtProti.Count(); j++)
                {
                    Log item = new Log();
                    if (DtProti[j] != "")
                    {
                        item.Properties = DtProti[j].ToString();
                        item.OldValues = DtOvalues[j].ToString();
                        item.NewValues = DtNValues[j].ToString();
                        item.UserUpdate = StrUserUp.ToString();
                        item.DateUpdate = StrDateTime.ToString();
                        ListLog.Add(item);
                    }
                }
            }

            ListDaXuatVe = VeDaXuat(result.DienThoai.Trim(), server_AIRLINE_BOOKING);
            result.ListDaXuatVe = ListDaXuatVe;
            result.ListTinhTrang = ListTinhTrang;
            result.ListHTThanhToan = ListHTThanhToan;
            result.ListNguoiThayDoi = ListNguoiThayDoi;
            result.ListThongTinKhach = ListThongTinKhach;
            result.ListLog = ListLog;
            return result;
        }

        public List<DaXuatVe> VeDaXuat(string Phone, string server_AIRLINE_BOOKING)
        {
            List<DaXuatVe> ListDaXuatVe = new List<DaXuatVe>();
            List<DaXuatVe> ListSoLuong = new List<DaXuatVe>();
            List<DaXuatVe> ListVeXuat = new List<DaXuatVe>();
            int soluong = 0;
            string sqlCountVe = @"SELECT COUNT(RowID) from Booking where Booking_Status='6' and YEAR(DateBook) >= 2019 and Contact_Phone=N'" + Phone + "'";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                soluong = conn.QueryFirst<int>(sqlCountVe, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            DaXuatVe item = new DaXuatVe();
            item.Order_ID = "Đã có " + soluong + " booking được xuất vé";
            ListSoLuong.Add(item);

            string sqlShowVe = @"SELECT TOP 10 Order_ID from Booking where Booking_Status='6' and YEAR(DateBook) >= 2019 and Contact_Phone=N'" + Phone + "'";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                ListVeXuat = (List<DaXuatVe>)conn.Query<DaXuatVe>(sqlShowVe, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            ListDaXuatVe.AddRange(ListSoLuong);
            ListDaXuatVe.AddRange(ListVeXuat);
            return ListDaXuatVe;
        }
        public bool UpdateThongTinKhach(string ID, string OrderID, string CodeDi, string CodeVe, string server_AIRLINE_BOOKING, string LgsProtiValues, string LgsOldValues, string LgsNewValues, string TenDangNhap)
        {
            bool eventlog = SaveEventLog(OrderID, LgsProtiValues, LgsOldValues, LgsNewValues, TenDangNhap, server_AIRLINE_BOOKING);
            int i = 0;
            string sql = "UPDATE DETAIL_BOOKING SET Code_OneWay = '" + CodeDi + "', Code_RoundTrip = '" + CodeVe + "' WHERE RowID = '" + ID + "' ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                i = conn.Execute(sql, null, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateHoaDon(string ID, string CongTy, string DiaChiCTY, string MST, string DiaChiHD, string server_AIRLINE_BOOKING, string LgsProtiValues, string LgsOldValues, string LgsNewValues, string TenDangNhap)
        {
            bool eventlog = SaveEventLog(ID, LgsProtiValues, LgsOldValues, LgsNewValues, TenDangNhap, server_AIRLINE_BOOKING);
            int i = 0;
            string sql = "UPDATE Booking SET CompanyName_Bill = N'" + CongTy + "',AddressCompany_Bill = N'" + DiaChiCTY + "',TaxCode_Bill = N'" + MST + "', Address_Bill = N'" + DiaChiHD + "' WHERE Order_ID = '" + ID + "' ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                i = conn.Execute(sql, null, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateLienHe(string ID, string NguoiLienHe, string Email, string ThanhPho, string DienThoai, string DiaChi, string QuocGia, string server_AIRLINE_BOOKING, string LgsProtiValues, string LgsOldValues, string LgsNewValues, string TenDangNhap)
        {
            bool eventlog = SaveEventLog(ID, LgsProtiValues, LgsOldValues, LgsNewValues, TenDangNhap, server_AIRLINE_BOOKING);
            int i = 0;
            string sql = "UPDATE Booking SET Contact_Name = N'" + NguoiLienHe + "',Contact_Email = N'" + Email + "',Contact_City = N'" + ThanhPho + "',Contact_Phone = N'" + DienThoai + "',Contact_Address = N'" + DiaChi + "', Contact_Country = N'" + QuocGia + "' WHERE Order_ID = '" + ID + "' ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                i = conn.Execute(sql, null, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateTinhTrang(string ID, string HTThanhToan, string NganHangCK, string LuotDi, string TimeLimit, string DateLimit, string MaGiaoDich, string NganHangTTTT, string TinhTrang, string TinhTrangTT, string SoTienCK, string MaDatChoLD, string MaDatChoLV, string LuotVe, string Remark, string MaThamChieu, string TienTrucTuyen, string server_AIRLINE_BOOKING, string LgsProtiValues, string LgsOldValues, string LgsNewValues, string TenDangNhap)
        {
            bool eventlog = SaveEventLog(ID, LgsProtiValues, LgsOldValues, LgsNewValues, TenDangNhap, server_AIRLINE_BOOKING);
            int i = 0;
            string sql = @"UPDATE Booking SET PAYMENT_METHOD = N'" + HTThanhToan + "', Bank_Transaction = N'" + NganHangCK + "', NoFlight_OneWay = N'" + LuotDi + "', HourSMS = N'" + TimeLimit + "', DateSMS = N'" + DateLimit + "', Code_Transaction = N'" + MaGiaoDich + "', Bank_Name = N'" + NganHangTTTT + "', Booking_Status = N'" + TinhTrang + "', Payment_Status = N'" + TinhTrangTT + "', Price_Transaction = N'" + SoTienCK + "', Code_OneWay = N'" + MaDatChoLD + "', Code_RoundTrip = N'" + MaDatChoLV + "', NoFlight_RoundTrip = N'" + LuotVe + "', Remark = N'" + Remark + "', Code_Reference = N'" + MaThamChieu + "', Bank_Price = N'" + TienTrucTuyen + "' WHERE Order_ID = '" + ID + "' ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                i = conn.Execute(sql, null, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateThongTinCBLD(string ID, string SoHieu_LD, string Hang_LD, string Code_LD, string DiemDi_LD, string DiemDen_LD, string NgayDi_LD, string GioDi_LD, string GioDen_LD, string GiaNet_LD, string ThuePhi_LD, string PhiDV_LD, string Giam_LD, string TongTien_LD, string server_AIRLINE_BOOKING, string LgsProtiValues, string LgsOldValues, string LgsNewValues, string TenDangNhap)
        {
            bool eventlog = SaveEventLog(ID, LgsProtiValues, LgsOldValues, LgsNewValues, TenDangNhap, server_AIRLINE_BOOKING);
            int i = 0;
            string sql = @"UPDATE Booking SET NoFlight_OneWay = N'" + SoHieu_LD + "', CategoriesTicket_OneWay = N'" + Hang_LD + "', Code_OneWay = N'" + Code_LD + "', Departure = N'" + DiemDi_LD + "', Arrival = N'" + DiemDen_LD + "', DateDeparture = N'" + NgayDi_LD + "', HourFrom_OneWay = N'" + GioDi_LD + "', HourTo_OneWay = N'" + GioDen_LD + "', Price_OneWay = N'" + GiaNet_LD + "', TaxFee_OneWay = N'" + ThuePhi_LD + "', ServiceFee_OneWay = N'" + PhiDV_LD + "', Discounts_OneWay = N'" + Giam_LD + "', Total_OneWay = N'" + TongTien_LD + "' WHERE Order_ID = '" + ID + "' ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                i = conn.Execute(sql, null, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateThongTinCBLV(string ID, string SoHieu_LV, string Hang_LV, string Code_LV, string DiemDi_LV, string DiemDen_LV, string NgayDi_LV, string GioDi_LV, string GioDen_LV, string GiaNet_LV, string ThuePhi_LV, string PhiDV_LV, string Giam_LV, string TongTien_LV, string server_AIRLINE_BOOKING, string LgsProtiValues, string LgsOldValues, string LgsNewValues, string TenDangNhap)
        {
            int i = 0;
            bool eventlog = SaveEventLog(ID, LgsProtiValues, LgsOldValues, LgsNewValues, TenDangNhap, server_AIRLINE_BOOKING);
            string sql = @"UPDATE Booking SET NoFlight_RoundTrip = N'" + SoHieu_LV + "', CategoriesTicket_RoundTrip = N'" + Hang_LV + "', Code_RoundTrip = N'" + Code_LV + "', Arrival = N'" + DiemDi_LV + "', Departure = N'" + DiemDen_LV + "', DateArrival = N'" + NgayDi_LV + "', HourFrom_RoundTrip = N'" + GioDi_LV + "', HourTo_RoundTrip = N'" + GioDen_LV + "', Price_RoundTrip = N'" + GiaNet_LV + "', TaxFee_RoundTrip = N'" + ThuePhi_LV + "', ServiceFee_RoundTrip = N'" + PhiDV_LV + "', Discounts_RoundTrip = N'" + Giam_LV + "', Total_RoundTrip = N'" + TongTien_LV + "' WHERE Order_ID = '" + ID + "' ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                i = conn.Execute(sql, null, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateTinhTrangBooking(string ID, string TinhTrang, string server_AIRLINE_BOOKING, string LgsProtiValues, string LgsOldValues, string LgsNewValues, string TenDangNhap)
        {
            bool eventlog = SaveEventLog(ID, LgsProtiValues, LgsOldValues, LgsNewValues, TenDangNhap, server_AIRLINE_BOOKING);
            int i = 0;
            string sql = @"UPDATE Booking SET  Booking_Status = N'" + TinhTrang + "' WHERE Order_ID = '" + ID + "' ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                i = conn.Execute(sql, null, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateGiaoCho(string ID, string per, string server_AIRLINE_BOOKING, string LgsProtiValues, string LgsOldValues, string LgsNewValues, string TenDangNhap)
        {
            int i = 0;
            bool eventlog = SaveEventLog(ID, LgsProtiValues, LgsOldValues, LgsNewValues, TenDangNhap, server_AIRLINE_BOOKING);
            string sql = @"UPDATE Booking SET  Assigned = N'" + per + "' WHERE Order_ID = '" + ID + "' ";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                i = conn.Execute(sql, null, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SendSMSBooking(string ID, string hanhTrinh_SMS, string nguoiLienHe_SMS, string dienThoai_SMS, string gio_SMS, string ngayThanhToan_SMS, string giaVeMoi_SMS, string trangThai_SMS, string server_AIRLINE_BOOKING, string UserName)
        {
            SMSService.InformationSMS_Book smsinfor = new SMSService.InformationSMS_Book();
            string dsstatus = trangThai_SMS;
            smsinfor.Order_ID = ID;
            smsinfor.Phone_Receive = "84" + dienThoai_SMS.Substring(1, dienThoai_SMS.Length - 1);
            smsinfor.FullName_Receive = nguoiLienHe_SMS;
            smsinfor.Hour_TimeOut = gio_SMS;
            smsinfor.Date_TimeOut = ngayThanhToan_SMS;
            smsinfor.PriceTicket = giaVeMoi_SMS;
            smsinfor.RouteTrip = hanhTrinh_SMS;

            SMSService.ServiceSoapClient sms = new SMSService.ServiceSoapClient(SMSService.ServiceSoapClient.EndpointConfiguration.ServiceSoap);
            string error = sms.SendSMS_Book("a1di6Ex+ASHUSbw7Od2bkA==", smsinfor);
            if (error != "")
            {
                return false;
            }
            else
            {
                //-------- logs-----
                string Lgsms = " gửi sms ";
                string LgsOldValues = "";
                string LgsNewValues = " mã đặt chỗ " + ID + " cho  khách " + nguoiLienHe_SMS + " số đt " + dienThoai_SMS + " time limit  " + gio_SMS + " - " + ngayThanhToan_SMS + " giá tiền " + giaVeMoi_SMS;
                EventLogs(Lgsms, LgsOldValues, LgsNewValues, ID, UserName, server_AIRLINE_BOOKING);
                //-----------------logs đổi giá tiền ------------------------
                int result = 0;
                string SqlQuery = " UPDATE Booking Set NewTotal= '" + giaVeMoi_SMS.Replace(",", "").Replace(".", "") + "' WHERE Order_ID='" + ID + "' ";
                using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
                {
                    result = conn.Execute(SqlQuery, null, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                if (result > 0)
                {
                    string LgesTIEN = " thêm tổng tiền mới";
                    LgsOldValues = "";
                    LgsNewValues = giaVeMoi_SMS;
                    EventLogs(LgesTIEN, LgsOldValues, LgsNewValues, ID, UserName, server_AIRLINE_BOOKING);
                }
                return true;
            }

        }
        public bool EventLogs(string StrProperti, string StrOldvalues, string StrNewvalues, string StrOrID, string UserUpdate, string server_AIRLINE_BOOKING)
        {
            string[] StrSplProti = StrProperti.Split('ϟ');
            string[] StrSplOvalues = StrOldvalues.Split('ϟ');
            string[] StrSplNValues = StrNewvalues.Split('ϟ');

            string Strpasce = "";
            string StrRelProti = "";
            string StrRelOvalue = "";
            string StrRelnvalues = "";
            if (StrSplOvalues.Count() == StrSplProti.Count() && StrSplNValues.Count() == StrSplProti.Count())
            {
                for (int i = 0; i < StrSplProti.Count(); i++)
                {
                    if (i != StrSplProti.Count())
                    {
                        Strpasce = "|";
                    }
                    if (StrSplNValues[i].Trim() != StrSplOvalues[i].Trim())
                    {
                        StrRelProti += StrSplProti[i].Trim() + Strpasce;
                        StrRelOvalue += StrSplOvalues[i].Trim() + Strpasce;
                        StrRelnvalues += StrSplNValues[i].Trim() + Strpasce;
                    }
                }
                int result = 0;
                string SqlLogs = " INSERT INTO Event_Log VALUES ('" + StrOrID + "',N'" + StrRelProti + "',N'" + StrRelOvalue + "', N'" + StrRelnvalues + "', '" + UserUpdate + "', GETDATE()) ";
                using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
                {
                    result = conn.Execute(SqlLogs, null, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public bool SendSMSBookingXuatVe(string ID)
        {
            SMSService.ServiceSoapClient sms = new SMSService.ServiceSoapClient(SMSService.ServiceSoapClient.EndpointConfiguration.ServiceSoap);
            string error = sms.SendSMS_Xuat("SMS", ID);
            if (error != "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool SaveEventLog(string OrderID, string LgsProtiValues, string LgsOldValues, string LgsNewValues, string TenDangNhap, string server_AIRLINE_BOOKING)
        {
            int c = 0;
            if (LgsOldValues == null)
            {
                LgsOldValues = "";
            }
            if (LgsNewValues == null)
            {
                LgsNewValues = "";
            }
            if (LgsProtiValues == null)
            {
                LgsProtiValues = "";
            }
            string[] StrSplProti = LgsProtiValues.Split('|');
            string[] StrSplOvalues = LgsOldValues.Split('|');
            string[] StrSplNValues = LgsNewValues.Split('|');
            for (int i = 0; i < StrSplProti.Count(); i++)
            {
                if (StrSplNValues[i].Trim() != StrSplOvalues[i].Trim())
                {
                    string SqlLogs = " INSERT INTO Event_Log VALUES ('" + OrderID + "',N'" + StrSplProti[i].Trim() + "',N'" + StrSplOvalues[i].Trim() + "', N'" + StrSplNValues[i].Trim() + "', '" + TenDangNhap + "', GETDATE()) ";
                    using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
                    {
                        c = conn.Execute(SqlLogs, null, null, commandType: CommandType.Text, commandTimeout: 30);
                        conn.Dispose();
                    }
                }
            }

            if (c > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SendMailBooking(DetailBooking DetailBooking)
        {
            bool resut_sendmail = false;
            DetailBooking.DiemDi = ReplaceAfter(DetailBooking.DiemDi);
            DetailBooking.DiemDen = ReplaceAfter(DetailBooking.DiemDen);
            if (DetailBooking.TinhTrangStatus == "6")
            {
                resut_sendmail = SendMails_DaXuatVe(DetailBooking);
            }
            else
            {
                resut_sendmail = SendMails_DatCho(DetailBooking);
            }
            return resut_sendmail;
        }
        public bool SendMails_DaXuatVe(DetailBooking DetailBooking)
        {
            DetailBooking result = new DetailBooking();
            //string server_AIRLINE_BOOKING = "Data Source=146.196.65.74;Initial Catalog=AIRLINE_BOOKING;User ID=envietsystemagent;Password=SQl_EvGroUp_d4T4_c0nNectIon;";
            string slkhach = "";
            string sql = @"select * from BOOKING where Order_ID = '" + DetailBooking.OrderID_SMS + "'";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                result = conn.QueryFirst<DetailBooking>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
                if (result.Adt != "0") { slkhach += result.Adt + " người lớn, "; }
                if (result.Chd != "0") { slkhach += result.Chd + " trẻ em, "; }
                if (result.Inf != "0") { slkhach += result.Inf + " em bé "; }
            }
            string ThongTinKhach = "";
            for (int i = 0; i < DetailBooking.ListThongTinKhach.Count; i++)
            {
                ThongTinKhach += "<tr><td>" + DetailBooking.ListThongTinKhach[i].DanhXung + "</td><td>" + DetailBooking.ListThongTinKhach[i].FullName + "</td></tr>";
            }

            MailMessage mail = new MailMessage(baoBK.username, DetailBooking.Email);
            mail.From = new MailAddress(baoBK.username, "ENVIET.AIR");
            SmtpClient client = new SmtpClient();
            client.EnableSsl = false;
            client.Port = baoBK.port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(baoBK.username, new DBase().Decrypt(baoBK.password, "vodacthe", true));
            client.Host = baoBK.host;

            string subject = "XAC NHAN DA THANH TOAN DAT CHO";
            try
            {
                mail.Bcc.Add(baoBK.BCC);
            }
            catch { }


            mail.Subject = subject;
            ///-------- Start of mail body ------------
            string mailBody;
            var webRequest = System.Net.WebRequest.Create(baoBK.templateUrl);
            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new System.IO.StreamReader(content))
            { mailBody = reader.ReadToEnd(); }
            if (DetailBooking.HanhTrinh == "Một chiều")
            {
                mailBody = mailBody.Replace("$_ThuocTinh", "none");
            }

            mailBody = mailBody.Replace("$_HTThanhToan", DetailBooking.HinhThucThanhToan);
            mailBody = mailBody.Replace("$_TongTien", string.Format("{0:#,0}", double.Parse(DetailBooking.TongTienMoi)));
            mailBody = mailBody.Replace("$_Order_ID", DetailBooking.OrderID_SMS);
            mailBody = mailBody.Replace("$_HanhTrinh", DetailBooking.DiemDi + " - " + DetailBooking.DiemDen);
            mailBody = mailBody.Replace("$_SLHanhKhach", slkhach);
            mailBody = mailBody.Replace("$_NgayDi", DetailBooking.NgayDi_LD);
            mailBody = mailBody.Replace("$_NgayVe", DetailBooking.NgayDi_LV);
            mailBody = mailBody.Replace("$_HanhTrinh_LD", DetailBooking.DiemDi + " - " + DetailBooking.DiemDen);
            mailBody = mailBody.Replace("$_HanhTrinh_LV", DetailBooking.DiemDen + " - " + DetailBooking.DiemDi);
            mailBody = mailBody.Replace("$_Hang_LD", DetailBooking.Airline_LD);
            mailBody = mailBody.Replace("$_Hang_LV", DetailBooking.Airline_LV);
            mailBody = mailBody.Replace("$_MaDatCho_LD", DetailBooking.Code_LD);
            mailBody = mailBody.Replace("$_MaDatCho_LV", DetailBooking.Code_LV);
            mailBody = mailBody.Replace("$_DiemDi_LD", DetailBooking.DiemDi);
            mailBody = mailBody.Replace("$_DiemDen_LD", DetailBooking.DiemDen);
            mailBody = mailBody.Replace("$_MaChuyenBay_LD", DetailBooking.SoHieu_LD);
            mailBody = mailBody.Replace("$_DiemDi_LV", DetailBooking.DiemDen);
            mailBody = mailBody.Replace("$_DiemDen_LV", DetailBooking.DiemDi);
            mailBody = mailBody.Replace("$_MaChuyenBay_LV", DetailBooking.SoHieu_LV);
            mailBody = mailBody.Replace("$_GioDi_LD", DetailBooking.GioDi_LD);
            mailBody = mailBody.Replace("$_GioDen_LD", DetailBooking.GioDen_LD);
            mailBody = mailBody.Replace("$_HangVe_LD", DetailBooking.Hang_LD);
            mailBody = mailBody.Replace("$_GioDi_LV", DetailBooking.GioDi_LV);
            mailBody = mailBody.Replace("$_GioDen_LV", DetailBooking.GioDen_LV);
            mailBody = mailBody.Replace("$_HangVe_LV", DetailBooking.Hang_LV);
            mailBody = mailBody.Replace("$_ThongTinKhach", ThongTinKhach);
            mail.Body = mailBody;
            mail.IsBodyHtml = true; // Format mail dạng HTML
            ///-------- End of mail body --------------
            client.Send(mail);
            return true;
        }
        public bool SendMails_DatCho(DetailBooking DetailBooking)
        {
            DetailBooking result = new DetailBooking();
            //string server_AIRLINE_BOOKING = "Data Source=146.196.65.74;Initial Catalog=AIRLINE_BOOKING;User ID=envietsystemagent;Password=SQl_EvGroUp_d4T4_c0nNectIon;";
            string slkhach = "";
            string sql = @"select * from BOOKING where Order_ID = '" + DetailBooking.OrderID_SMS + "'";
            using (var conn = new SqlConnection(server_AIRLINE_BOOKING))
            {
                result = conn.QueryFirst<DetailBooking>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
                if (result.Adt != "0") { slkhach += result.Adt + " người lớn, "; }
                if (result.Chd != "0") { slkhach += result.Chd + " trẻ em, "; }
                if (result.Inf != "0") { slkhach += result.Inf + " em bé "; }
            }
            string ThongTinKhach = "";
            for (int i = 0; i < DetailBooking.ListThongTinKhach.Count; i++)
            {
                ThongTinKhach += "<tr><td>" + DetailBooking.ListThongTinKhach[i].DanhXung + "</td><td>" + DetailBooking.ListThongTinKhach[i].FullName + "</td></tr>";
            }

            MailMessage mail = new MailMessage(baoBK.username, DetailBooking.Email);
            mail.From = new MailAddress(baoBK.username, "ENVIET.AIR");
            SmtpClient client = new SmtpClient();
            client.EnableSsl = false;
            client.Port = baoBK.port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(baoBK.username, new DBase().Decrypt(baoBK.password, "vodacthe", true));
            client.Host = baoBK.host;

            string subject = "XAC NHAN DAT CHO";
            try
            {
                mail.Bcc.Add(baoBK.BCC);
            }
            catch { }


            mail.Subject = subject;
            ///-------- Start of mail body ------------
            string mailBody;
            var webRequest = System.Net.WebRequest.Create(baoBK.templateUrl);
            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new System.IO.StreamReader(content))
            { mailBody = reader.ReadToEnd(); }
            if (DetailBooking.HanhTrinh == "Một chiều")
            {
                mailBody = mailBody.Replace("$_ThuocTinh", "none");
            }
            mailBody = mailBody.Replace("$_HTThanhToan", DetailBooking.HinhThucThanhToan);
            mailBody = mailBody.Replace("$_Timelimit", DetailBooking.TimeLimit);
            mailBody = mailBody.Replace("$_Datelimit", DetailBooking.DateLimit);
            mailBody = mailBody.Replace("$_TongTien", string.Format("{0:#,0}", double.Parse(DetailBooking.TongTienMoi)));
            mailBody = mailBody.Replace("$_Order_ID", DetailBooking.OrderID_SMS);
            mailBody = mailBody.Replace("$_HanhTrinh", DetailBooking.DiemDi + " - " + DetailBooking.DiemDen);
            mailBody = mailBody.Replace("$_SLHanhKhach", slkhach);
            mailBody = mailBody.Replace("$_NgayDi", DetailBooking.NgayDi_LD);
            mailBody = mailBody.Replace("$_NgayVe", DetailBooking.NgayDi_LV);
            mailBody = mailBody.Replace("$_HanhTrinh_LD", DetailBooking.DiemDi + " - " + DetailBooking.DiemDen);
            mailBody = mailBody.Replace("$_HanhTrinh_LV", DetailBooking.DiemDen + " - " + DetailBooking.DiemDi);
            mailBody = mailBody.Replace("$_Hang_LD", DetailBooking.Airline_LD);
            mailBody = mailBody.Replace("$_Hang_LV", DetailBooking.Airline_LV);
            mailBody = mailBody.Replace("$_MaDatCho_LD", DetailBooking.Code_LD);
            mailBody = mailBody.Replace("$_MaDatCho_LV", DetailBooking.Code_LV);
            mailBody = mailBody.Replace("$_DiemDi_LD", DetailBooking.DiemDi);
            mailBody = mailBody.Replace("$_DiemDen_LD", DetailBooking.DiemDen);
            mailBody = mailBody.Replace("$_MaChuyenBay_LD", DetailBooking.SoHieu_LD);
            mailBody = mailBody.Replace("$_DiemDi_LV", DetailBooking.DiemDen);
            mailBody = mailBody.Replace("$_DiemDen_LV", DetailBooking.DiemDi);
            mailBody = mailBody.Replace("$_MaChuyenBay_LV", DetailBooking.SoHieu_LV);
            mailBody = mailBody.Replace("$_GioDi_LD", DetailBooking.GioDi_LD);
            mailBody = mailBody.Replace("$_GioDen_LD", DetailBooking.GioDen_LD);
            mailBody = mailBody.Replace("$_HangVe_LD", DetailBooking.Hang_LD);
            mailBody = mailBody.Replace("$_GioDi_LV", DetailBooking.GioDi_LV);
            mailBody = mailBody.Replace("$_GioDen_LV", DetailBooking.GioDen_LV);
            mailBody = mailBody.Replace("$_HangVe_LV", DetailBooking.Hang_LV);
            mailBody = mailBody.Replace("$_ThongTinKhach", ThongTinKhach);
            mail.Body = mailBody;
            mail.IsBodyHtml = true; // Format mail dạng HTML
            ///-------- End of mail body --------------
            client.Send(mail);
            return true;
        }
        public string ReplaceAfter(string StrSrc)
        {
            string StrAfter = "";
            switch (StrSrc)
            {
                case "BMV":
                    StrAfter = "Ban Mê Thuột";
                    break;
                case "CAH":
                    StrAfter = "Cà Mau";
                    break;
                case "DAD":
                    StrAfter = "Đà Nẵng";
                    break;
                case "DIN":
                    StrAfter = "Điện Biên";
                    break;
                case "DLI":
                    StrAfter = "Đà Lạt";
                    break;
                case "HAN":
                    StrAfter = "Hà Nội";
                    break;
                case "HPH":
                    StrAfter = "Hải Phòng";
                    break;
                case "HUI":
                    StrAfter = "Huế";
                    break;
                case "CXR":
                    StrAfter = "Nha Trang";
                    break;
                case "PQC":
                    StrAfter = "Phú Quốc";
                    break;
                case "PXU":
                    StrAfter = "Pleiku";
                    break;
                case "SGN":
                    StrAfter = "TP.Hồ Chí Minh";
                    break;
                case "THD":
                    StrAfter = "Thanh Hóa";
                    break;
                case "UIH":
                    StrAfter = "Quy Nhơn ";
                    break;
                case "VCA":
                    StrAfter = "Cần Thơ";
                    break;
                case "VCL":
                    StrAfter = "Chu Lai";
                    break;
                case "VCS":
                    StrAfter = "Côn Đảo";
                    break;
                case "VDH":
                    StrAfter = "Đồng Hới";
                    break;
                case "VII":
                    StrAfter = "Nghệ An";
                    break;
                case "VKG":
                    StrAfter = "Rạch Giá";
                    break;
            }
            return StrAfter;
        }
    }
}
