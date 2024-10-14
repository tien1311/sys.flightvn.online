using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Web;

using System.Net;
using EasyInvoice.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Dapper;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Drawing.Printing;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;
using Manager.Model.Models.VeDoan;

namespace Manager.DataAccess.Repository
{
    public class FlightRepository
    {
        DBase db = new DBase();
        private string SQL_EV_MAIN;
        private string SQL_EV_SERVICE;
        private string SQL_EV_GROUPBOOKING;
        public FlightRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN = configuration.GetConnectionString("SQL_EV_MAIN");
            SQL_EV_SERVICE = configuration.GetConnectionString("SQL_EV_SERVICE");
            SQL_EV_GROUPBOOKING = configuration.GetConnectionString("SQL_GROUP_BOOKING");
        }
        //public List<FlightModel> ListFlight()
        //{
        //    UpdateFlightEndDate();
        //    List<FlightModel> ListFlight = new List<FlightModel>();

        //    string sql = "select * from Flight where Status = 1 order by ID Desc";
        //    DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
        //    if (tb != null)
        //    {
        //        if (tb.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < tb.Rows.Count; i++)
        //            {
        //                FlightModel flight = new FlightModel();
        //                flight.ID = int.Parse(tb.Rows[i]["ID"].ToString());
        //                flight.Airline = tb.Rows[i]["Airline"].ToString();
        //                flight.Itinerary = tb.Rows[i]["Itinerary"].ToString();
        //                flight.NumberOfGuests = int.Parse(tb.Rows[i]["NumberOfGuests"].ToString());
        //                flight.Price = decimal.Parse(tb.Rows[i]["Price"].ToString());
        //                flight.PriceAgent = decimal.Parse(tb.Rows[i]["PriceAgent"].ToString());
        //                flight.KindTrip = tb.Rows[i]["KindTrip"].ToString();
        //                flight.Specification = tb.Rows[i]["Specification"].ToString();


        //                string sql1 = "select * from FlightDetail where FlightID=" + int.Parse(tb.Rows[i]["ID"].ToString());
        //                DataTable tb1 = db.ExecuteDataSet(sql1, CommandType.Text, "server37", null).Tables[0];
        //                if (tb1 != null)
        //                {
        //                    if (tb1.Rows.Count > 0)
        //                    {
        //                        List<FlightDetailModel> listDetail = new List<FlightDetailModel>();
        //                        for (int a = 0; a < tb1.Rows.Count; a++)
        //                        {
        //                            FlightDetailModel flightDetail = new FlightDetailModel();

        //                            flightDetail.FlightNumber = tb1.Rows[a]["FlightNumber"].ToString();
        //                            flightDetail.FlightHour = tb1.Rows[a]["FlightHour"].ToString();
        //                            flightDetail.FlightDate = DateTime.Parse(tb1.Rows[a]["FlightDate"].ToString());
        //                            listDetail.Add(flightDetail);
        //                        }
        //                        flight.ListFlightDetail = listDetail;
        //                    }
        //                }
        //                ListFlight.Add(flight);
        //            }
        //        }
        //    }
        //    return ListFlight;
        //}
        public List<FlightModel> ListFlight()
        {
            UpdateFlightEndDate();
            //string Server = "Data Source=.;Initial Catalog=Manager;User ID=sa;Password=EnViet@123;";
            List<FlightModel> result = new List<FlightModel>();
            string sql = @"select F.*  from Flight F left join FlightDetail FD on F.ID = FD.FlightID where F.status = 1 and F.Active =1 and FD.KindFlight = 1 order by FD.ID asc";
            using (var conn = new SqlConnection(SQL_EV_MAIN))
            {
                result = (List<FlightModel>)conn.Query<FlightModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            if (result.Count > 0)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    List<FlightDetailModel> result_Detail = new List<FlightDetailModel>();
                    string sqlFlightDetail = @"select FlightHour, RTRIM(FlightNumber) as FlightNumber, FlightDate  ,DATEPART(WEEKDAY, FlightDate) as WeekDay from FlightDetail where  FlightID = '" + result[i].ID + "'";
                    using (var conn = new SqlConnection(SQL_EV_MAIN))
                    {
                        result_Detail = (List<FlightDetailModel>)conn.Query<FlightDetailModel>(sqlFlightDetail, null, commandType: CommandType.Text, commandTimeout: 30);
                        conn.Close();
                    }
                    result[i].ListFlightDetail = result_Detail;
                }
            }
            return result;
        }
        public bool UpdateFlightEndDate()
        {
            string update = @"update Flight
                            set Status = 0
                            where ID  in (
                                           select A.ID from Flight A
                                           left join FlightDetail B on A.ID = B.FlightID
                                           where B.KindFlight = 1 and A.Status = 1 and B.FlightDate < GETDATE()
                                         )";
            int result = db.ExecuteNoneQuery(update, CommandType.Text, "server37", null);
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public bool DeleteFlight(int idkhoachinh)
        {
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@ID", idkhoachinh));
            int result = db.ExecuteNoneQuery("SP_DELETE_FLIGHT", CommandType.StoredProcedure, "server37", Param);
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public FlightModel SetDateFlight(int idkhoachinh)
        {
            FlightModel result = new FlightModel();
            List<FlightDetailModel> listDetail = new List<FlightDetailModel>();
            string sql = "select * from Flight where  ID = " + idkhoachinh;
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    result.ID = idkhoachinh;
                    result.Airline = tb.Rows[0]["Airline"].ToString();
                    result.Itinerary = tb.Rows[0]["Itinerary"].ToString();
                    result.NumberOfGuests = int.Parse(tb.Rows[0]["NumberOfGuests"].ToString());
                    result.Price = decimal.Parse(tb.Rows[0]["Price"].ToString());
                    result.PriceAgent = decimal.Parse(tb.Rows[0]["PriceAgent"].ToString());
                    result.KindTrip = tb.Rows[0]["KindTrip"].ToString();
                    result.Condition = tb.Rows[0]["Condition"].ToString();
                    result.Specification = tb.Rows[0]["Specification"].ToString();
                    result.Donvi = tb.Rows[0]["Unit"].ToString();

                    string sql1 = "select * from FlightDetail where FlightID=" + idkhoachinh;
                    DataTable tb1 = db.ExecuteDataSet(sql1, CommandType.Text, "server37", null).Tables[0];
                    if (tb1 != null)
                    {
                        if (tb1.Rows.Count > 0)
                        {
                            for (int i = 0; i < tb1.Rows.Count; i++)
                            {
                                FlightDetailModel flightDetail = new FlightDetailModel();
                                flightDetail.FlightID = int.Parse(tb1.Rows[i]["ID"].ToString());
                                flightDetail.FlightNumber = tb1.Rows[i]["FlightNumber"].ToString();
                                flightDetail.FlightHour = tb1.Rows[i]["FlightHour"].ToString();
                                flightDetail.FlightDate = DateTime.Parse(tb1.Rows[i]["FlightDate"].ToString());
                                listDetail.Add(flightDetail);
                            }
                            result.ListFlightDetail = listDetail;
                        }
                    }
                }

            }
            return result;
        }

        public bool UpdateFlight(FlightModel flight, string TenNhanVien)
        {
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@Airline", flight.Airline));
            Param.Add(new DBase.AddParameters("@Itinerary", flight.Itinerary));
            Param.Add(new DBase.AddParameters("@NumberOfGuests", flight.NumberOfGuests));
            Param.Add(new DBase.AddParameters("@Price", flight.Price));
            Param.Add(new DBase.AddParameters("@PriceAgent", flight.PriceAgent));
            Param.Add(new DBase.AddParameters("@KindTrip", flight.KindTrip));
            Param.Add(new DBase.AddParameters("@UpdateName", TenNhanVien));
            Param.Add(new DBase.AddParameters("@ID", flight.ID));
            Param.Add(new DBase.AddParameters("@Active", flight.active));
            Param.Add(new DBase.AddParameters("@Condition", flight.Condition));
            Param.Add(new DBase.AddParameters("@Specification", flight.Specification));
            Param.Add(new DBase.AddParameters("@Unit", flight.Donvi));

            int result = db.ExecuteNoneQuery("SP_UPDATE_FLIGHT", CommandType.StoredProcedure, "server37", Param);
            if (result > 0)
            {
                string delete = "Delete FlightDetail where FlightID = " + flight.ID;
                db.ExecuteNoneQuery(delete, CommandType.Text, "server37", null);
                List<DBase.AddParameters> Param_ChieuDi = new List<DBase.AddParameters>();
                Param_ChieuDi.Add(new DBase.AddParameters("@FlightNumber", flight.ListFlightDetail[0].FlightNumber));
                Param_ChieuDi.Add(new DBase.AddParameters("@FlightDate", flight.ListFlightDetail[0].FlightDate));
                Param_ChieuDi.Add(new DBase.AddParameters("@FlightHour", flight.ListFlightDetail[0].FlightHour));
                Param_ChieuDi.Add(new DBase.AddParameters("@FlightID", flight.ID));
                Param_ChieuDi.Add(new DBase.AddParameters("@KindFlight", 1));
                result = db.ExecuteNoneQuery("SP_INSERT_FLIGHTDETAIL", CommandType.StoredProcedure, "server37", Param_ChieuDi);

                if (result > 0)
                {
                    {

                        if (flight.KindTrip == "KH")
                        {
                            List<DBase.AddParameters> Param_ChieuVe = new List<DBase.AddParameters>();
                            Param_ChieuVe.Add(new DBase.AddParameters("@FlightNumber", flight.ListFlightDetail[1].FlightNumber));
                            Param_ChieuVe.Add(new DBase.AddParameters("@FlightDate", flight.ListFlightDetail[1].FlightDate));
                            Param_ChieuVe.Add(new DBase.AddParameters("@FlightHour", flight.ListFlightDetail[1].FlightHour));
                            Param_ChieuVe.Add(new DBase.AddParameters("@FlightID", flight.ID));
                            Param_ChieuVe.Add(new DBase.AddParameters("@KindFlight", 2));
                            result = db.ExecuteNoneQuery("SP_INSERT_FLIGHTDETAIL", CommandType.StoredProcedure, "server37", Param_ChieuVe);
                        }

                    }

                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public bool SaveFlight(FlightModel flight, string TenNhanVien)
        {
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@Airline", flight.Airline));
            Param.Add(new DBase.AddParameters("@Itinerary", flight.Itinerary));
            Param.Add(new DBase.AddParameters("@NumberOfGuests", flight.NumberOfGuests));
            Param.Add(new DBase.AddParameters("@Price", flight.Price));
            Param.Add(new DBase.AddParameters("@PriceAgent", flight.PriceAgent));
            Param.Add(new DBase.AddParameters("@KindTrip", flight.KindTrip));
            Param.Add(new DBase.AddParameters("@CreateName", TenNhanVien));
            Param.Add(new DBase.AddParameters("@Active", flight.active));
            Param.Add(new DBase.AddParameters("@Condition", flight.Condition));
            Param.Add(new DBase.AddParameters("@Specification", flight.Specification));
            Param.Add(new DBase.AddParameters("@Unit", flight.Donvi));


            string ID = db.ExecuteDataSet("SP_INSERT_FLIGHT", CommandType.StoredProcedure, "server37", Param).Tables[0].Rows[0][0].ToString();
            if (ID != "" && ID != null)
            {
                List<DBase.AddParameters> Param_ChieuDi = new List<DBase.AddParameters>();
                Param_ChieuDi.Add(new DBase.AddParameters("@FlightNumber", flight.ListFlightDetail[0].FlightNumber));
                Param_ChieuDi.Add(new DBase.AddParameters("@FlightDate", flight.ListFlightDetail[0].FlightDate));
                Param_ChieuDi.Add(new DBase.AddParameters("@FlightHour", flight.ListFlightDetail[0].FlightHour));
                Param_ChieuDi.Add(new DBase.AddParameters("@FlightID", int.Parse(ID)));
                Param_ChieuDi.Add(new DBase.AddParameters("@KindFlight", 1));
                int result = db.ExecuteNoneQuery("SP_INSERT_FLIGHTDETAIL", CommandType.StoredProcedure, "server37", Param_ChieuDi);
                if (result > 0)
                {
                    if (flight.KindTrip == "KH")
                    {
                        List<DBase.AddParameters> Param_ChieuVe = new List<DBase.AddParameters>();
                        Param_ChieuVe.Add(new DBase.AddParameters("@FlightNumber", flight.ListFlightDetail[1].FlightNumber));
                        Param_ChieuVe.Add(new DBase.AddParameters("@FlightDate", flight.ListFlightDetail[1].FlightDate));
                        Param_ChieuVe.Add(new DBase.AddParameters("@FlightHour", flight.ListFlightDetail[1].FlightHour));
                        Param_ChieuVe.Add(new DBase.AddParameters("@FlightID", int.Parse(ID)));
                        Param_ChieuVe.Add(new DBase.AddParameters("@KindFlight", 2));
                        result = db.ExecuteNoneQuery("SP_INSERT_FLIGHTDETAIL", CommandType.StoredProcedure, "server37", Param_ChieuVe);
                    }
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
        public VeDoanAll DanhSachVedoan(string dafr, string dato)
        {
            try
            {

                VeDoanAll result = new VeDoanAll();
                List<VeDoanModel> CV = new List<VeDoanModel>();
                VeDoanModel guimail = new VeDoanModel();
                string sql = @"select B.*,F.Airline,F.Itinerary from BOOKING B left join FLIGHT F on F.ID = B.FlightID where B.CreateDate > '" + dafr + "' and B.CreateDate <='" + dato + " 23:59:59' ORDER BY B.CreateDate DESC ";
                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                if (tb != null && tb.Rows.Count > 0)
                {
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        guimail = new VeDoanModel();
                        guimail.ID = tb.Rows[i]["ID"].ToString();
                        guimail.Name = tb.Rows[i]["Name"].ToString();
                        guimail.Email = tb.Rows[i]["Email"].ToString();
                        guimail.Sdt = tb.Rows[i]["Tel"].ToString();
                        guimail.Code = tb.Rows[i]["Code"].ToString();
                        guimail.Hang = tb.Rows[i]["Airline"].ToString();
                        guimail.HanhTrinh = tb.Rows[i]["Itinerary"].ToString();
                        guimail.FindID = tb.Rows[i]["FlightID"].ToString();
                        guimail.Gia = double.Parse(tb.Rows[i]["Price"].ToString());
                        guimail.SoLuongKhach = tb.Rows[i]["Quantity"].ToString();
                        guimail.Tongsotien = double.Parse(tb.Rows[i]["Total"].ToString());
                        guimail.Ghichu = tb.Rows[i]["Note"].ToString();
                        CV.Add(guimail);
                    }
                    result.vedoan = CV;
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public YeuCauVeDoan ChiTietYeuCauDoan(string code)
        {
            //string connectionString = "Data Source=.;Initial Catalog=GROUP_BOOKING;User ID=sa;Password=EnViet@123;";
            YeuCauVeDoan chiTietYeuCauDoan = new YeuCauVeDoan();
            string sql = @"
                            SELECT 
		                            r.ID,
                                    r.Code,
                                    r.AgentCode,
                                    r.GroupName,
                                    r.GroupSize,
                                    r.BriefDescription,
                                    r.TripType,
                                    r.ContactFullName,
                                    r.ContactTitle,
                                    r.ContactEmail,
                                    r.ContactPhone,
                                    r.ContactOtherPhone,
                                    r.ContactCompanyName,
                                    r.Remark,
                                    r.StreetLine1,
                                    r.StreetLine2,
                                    r.PostalCode,
                                    r.CityName,
                                    r.CountryCode,
                                    r.CreatedDate,
                                    r.GroupType,
                                    rd1.DepartureCode AS DepartureCode1,
                                    rd1.ArrivalCode AS ArrivalCode1,
                                    rd1.DepartureDateTime AS DepartureDateTime1,
                                    rd2.DepartureCode AS DepartureCode2,
                                    rd2.ArrivalCode AS ArrivalCode2,
                                    rd2.DepartureDateTime AS DepartureDateTime2,
                                    rd1.PreferredAirlineCode AS PreferredAirlineCode,
                                    rd1.PreferredFlightNumber AS PreferredFlightNumber,
                                    rd1.TravelClass AS TravelClass
                                FROM 
                                    Request_Flight r
                                JOIN 
                                    Request_Flight_Detail rd1 
                                    ON r.ID = rd1.RequestID AND rd1.RouteNo = 1
                                LEFT JOIN 
                                    Request_Flight_Detail rd2 
                                    ON r.ID = rd2.RequestID AND rd2.RouteNo = 2
                                WHERE
                                   r.Code = @code";
            using (var conn = new SqlConnection(SQL_EV_GROUPBOOKING))
            {
                conn.Open();
                chiTietYeuCauDoan = conn.Query<YeuCauVeDoan>(sql, new { code = code }, commandType: System.Data.CommandType.Text, commandTimeout: 90).FirstOrDefault();
                conn.Close();
            }
            return chiTietYeuCauDoan;
        }

        public string GetAirportName(string code)
        {
            string connectionString = "Data Source=.;Initial Catalog=Manager_V2;User ID=sa;Password=EnViet@123;";

            string AirportName = string.Empty;
            string sql = @"
                            select ap.AirportName
                            from AIRPORT a
                            JOIN AIRPORT_PROFILE ap ON a.ID = ap.AirportID
                            WHERE a.AirportCode = @code AND ap.LocaleId = 'VN'
                        ";
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                AirportName = conn.Query<string>(sql, new { code = code }, commandType: System.Data.CommandType.Text, commandTimeout: 90).FirstOrDefault();
                conn.Close();
            }

            return AirportName;
        }

        public (List<YeuCauVeDoan> , int) DanhSachYeuCau(int page, int pageSize, DateTime? dateFrom, DateTime? dateTo)
        {
            if(dateFrom == null)
            {
                dateFrom = new DateTime(1990, 1, 1);

            }
            if (dateTo == null)
            {
                dateTo = DateTime.Now;
            }
            List<YeuCauVeDoan> listYeuCauVeDoan = null;
            //string connectionString = "Data Source=.;Initial Catalog=GROUP_BOOKING;User ID=sa;Password=EnViet@123;";
            string sql = @"
                         WITH RequestCTE AS (
                            SELECT 
                                ROW_NUMBER() OVER (ORDER BY r.CreatedDate DESC) AS RowNum,
                                r.ID,
                                r.Code,
                                r.AgentCode,
                                r.GroupName,
                                r.GroupSize,
                                r.BriefDescription,
                                r.TripType,
                                r.ContactFullName,
                                r.ContactTitle,
                                r.ContactEmail,
                                r.ContactPhone,
                                r.ContactOtherPhone,
                                r.ContactCompanyName,
                                r.Remark,
                                r.StreetLine1,
                                r.StreetLine2,
                                r.PostalCode,
                                r.CityName,
                                r.CountryCode,
                                r.CreatedDate,
                                r.GroupType,
                                rd1.DepartureCode AS DepartureCode1,
                                rd1.ArrivalCode AS ArrivalCode1,
                                rd1.DepartureDateTime AS DepartureDateTime1,
                                rd2.DepartureCode AS DepartureCode2,
                                rd2.ArrivalCode AS ArrivalCode2,
                                rd2.DepartureDateTime AS DepartureDateTime2,
                                rd1.PreferredAirlineCode AS PreferredAirlineCode,
                                rd1.PreferredFlightNumber AS PreferredFlightNumber,
                                rd1.TravelClass AS TravelClass
                            FROM 
                                Request_Flight r
                            JOIN 
                                Request_Flight_Detail rd1 
                                ON r.ID = rd1.RequestID AND rd1.RouteNo = 1
                            LEFT JOIN 
                                Request_Flight_Detail rd2 
                                ON r.ID = rd2.RequestID AND rd2.RouteNo = 2
                            WHERE
                                r.CreatedDate BETWEEN @dateFrom AND @dateTo
                        )
                        ";

            string querySQL = sql + @" SELECT *
                                    FROM RequestCTE
                                    WHERE RowNum BETWEEN (@Page - 1) * @PageSize + 1 AND @Page * @PageSize;";
            string countTotalRecordSQL = sql + @" SELECT COUNT(*)
                                                FROM RequestCTE";

            int totalRecord = 0;

            using (var conn = new SqlConnection(SQL_EV_GROUPBOOKING))
            {
                conn.Open();
                listYeuCauVeDoan = conn.Query<YeuCauVeDoan>(querySQL, new { Page = page, PageSize = pageSize, dateFrom = dateFrom, dateTo = dateTo}, commandType: System.Data.CommandType.Text, commandTimeout: 90).ToList();
                totalRecord = conn.ExecuteScalar<int>(countTotalRecordSQL, new { dateFrom = dateFrom, dateTo = dateTo }, commandTimeout: 30);
                conn.Close();
            }
            return (listYeuCauVeDoan, totalRecord);

        }
        public List<ChitietBooking> ChiTietBooking(int khoachinh)
        {
            try
            {

                List<ChitietBooking> thongtin = new List<ChitietBooking>();
                string sql = @"select B.*,F.Airline,F.Itinerary from BOOKING B left join FLIGHT F on F.ID = B.FlightID where B.ID='" + khoachinh + "'";
                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                if (tb != null && tb.Rows.Count > 0)
                {
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        ChitietBooking result = new ChitietBooking();
                        result.ID = tb.Rows[i]["ID"].ToString();
                        result.Name = tb.Rows[i]["Name"].ToString();
                        result.Email = tb.Rows[i]["Email"].ToString();
                        result.Sdt = tb.Rows[i]["Tel"].ToString();
                        result.Code = tb.Rows[i]["Code"].ToString();
                        result.Makh = tb.Rows[i]["Mst"].ToString();
                        result.Ghichu = tb.Rows[i]["Note"].ToString();
                        result.Ngaygui = DateTime.Parse(tb.Rows[i]["CreateDate"].ToString()).ToString("dd/MM/yyyy");
                        result.Soluong = tb.Rows[i]["Quantity"].ToString();
                        result.Tongtien = double.Parse(tb.Rows[i]["Total"].ToString());
                        result.Hang = tb.Rows[i]["Airline"].ToString();
                        result.Hanhtrinh = tb.Rows[i]["Itinerary"].ToString();
                        result.FindID = tb.Rows[i]["FlightID"].ToString();


                        List<BookingDetail> ListVeDoanDetail = new List<BookingDetail>();
                        string sqlFlightDetail = @"select *, (convert(varchar, getdate(), 106)) as ngaybay from FlightDetail where  FlightID = '" + tb.Rows[i]["FlightID"].ToString() + "'";
                        DataTable tbFlightDetail = db.ExecuteDataSet(sqlFlightDetail, CommandType.Text, "server37", null).Tables[0];
                        if (tbFlightDetail != null && tbFlightDetail.Rows.Count > 0)
                        {
                            for (int y = 0; y < tbFlightDetail.Rows.Count; y++)
                            {
                                BookingDetail resultDetail = new BookingDetail();
                                resultDetail.Mabay = tbFlightDetail.Rows[y]["FlightNumber"].ToString().Trim();
                                resultDetail.Ngaybay = DateTime.Parse(tbFlightDetail.Rows[y]["FlightDate"].ToString()).ToString("dd/MM/yyyy");
                                resultDetail.giobay = tbFlightDetail.Rows[y]["FlightHour"].ToString().Trim();
                                ListVeDoanDetail.Add(resultDetail);
                            }
                            result.bookingdetail = ListVeDoanDetail;
                        }
                        thongtin.Add(result);
                    }

                }
                return thongtin;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public List<Luuthongtin> ChiTietYeuCauBooking(int khoachinh)
        {
            try
            {
                List<Luuthongtin> CV = new List<Luuthongtin>();

                string sql = @"select * from REQUEST_FLIGHT  where ID='" + khoachinh + "'";
                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                if (tb != null && tb.Rows.Count > 0)
                {
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        Luuthongtin guimail = new Luuthongtin();
                        if (tb.Rows[i]["ReturnDate"].ToString() == "")
                        {
                            guimail.ID = tb.Rows[i]["ID"].ToString();
                            guimail.Name = tb.Rows[i]["Name"].ToString();
                            guimail.Email = tb.Rows[i]["Email"].ToString();
                            guimail.Sdt = tb.Rows[i]["Tel"].ToString();
                            guimail.Hang = tb.Rows[i]["Airline"].ToString();
                            guimail.Note = tb.Rows[i]["Note"].ToString();
                            guimail.SoLuongKhach = tb.Rows[i]["Quantity"].ToString();
                            guimail.Code = tb.Rows[i]["Code"].ToString();
                            guimail.Ngaydi = DateTime.Parse(tb.Rows[i]["DepartureDate"].ToString()).ToString("dd/MM/yyyy");

                            guimail.Noidi = tb.Rows[i]["PointDeparture"].ToString();
                            guimail.Noiden = tb.Rows[i]["PointReturn"].ToString();
                            guimail.CreateDate = DateTime.Parse(tb.Rows[i]["CreateDate"].ToString()).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            guimail.ID = tb.Rows[i]["ID"].ToString();
                            guimail.Name = tb.Rows[i]["Name"].ToString();
                            guimail.Email = tb.Rows[i]["Email"].ToString();
                            guimail.Sdt = tb.Rows[i]["Tel"].ToString();
                            guimail.Hang = tb.Rows[i]["Airline"].ToString();
                            guimail.Note = tb.Rows[i]["Note"].ToString();
                            guimail.SoLuongKhach = tb.Rows[i]["Quantity"].ToString();
                            guimail.Code = tb.Rows[i]["Code"].ToString();
                            guimail.Ngaydi = DateTime.Parse(tb.Rows[i]["DepartureDate"].ToString()).ToString("dd/MM/yyyy");
                            guimail.Ngayve = DateTime.Parse(tb.Rows[i]["ReturnDate"].ToString()).ToString("dd/MM/yyyy");
                            guimail.Noidi = tb.Rows[i]["PointDeparture"].ToString();
                            guimail.Noiden = tb.Rows[i]["PointReturn"].ToString();
                            guimail.CreateDate = DateTime.Parse(tb.Rows[i]["CreateDate"].ToString()).ToString("dd/MM/yyyy");
                        }

                        CV.Add(guimail);
                    }

                }
                return CV;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public List<Airline> ListAirline()
        {

            List<Airline> ListAirline = new List<Airline>();
            string sql = "select * from AIRLINE ";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37_V2", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        Airline airline = new Airline();
                        airline.Name = tb.Rows[i]["Name"].ToString();
                        airline.IMG = tb.Rows[i]["ImageUrl"].ToString();

                        ListAirline.Add(airline);
                    }
                }
            }
            return ListAirline;
        }
        public List<PaymentAppota> ListPayment()
        {
            List<PaymentAppota> ListPayment = new List<PaymentAppota>();


            //string connectionString = "Data Source=.;Initial Catalog=EV_SERVICES;User ID=sa;Password=EnViet@123;";

            using (IDbConnection dbConnection = new SqlConnection(SQL_EV_SERVICE))
            {
                dbConnection.Open();
                ListPayment = dbConnection.Query<PaymentAppota>(
                    "SELECT * FROM Payment_Notification ORDER BY CreatedDate DESC"
                ).ToList();
            }

            return ListPayment;
        }
        public List<PaymentAppota> SearchPayment(string cal_from, string cal_to, string TransactionPerson, string PaymentChannel, string PaymentPage, string Product)
        {
            List<PaymentAppota> ListPayment = new List<PaymentAppota>();

            //string connectionString = "Data Source=.;Initial Catalog=EV_SERVICES;User ID=sa;Password=EnViet@123;";

            using (IDbConnection dbConnection = new SqlConnection(SQL_EV_SERVICE))
            {
                dbConnection.Open();

                // Tạo một câu truy vấn SQL với các điều kiện tìm kiếm
                string query = "SELECT * FROM Payment_Notification WHERE 1=1";

                if (!string.IsNullOrEmpty(cal_from) && !string.IsNullOrEmpty(cal_to))
                {
                    query += " AND CreatedDate >= '" + cal_from + "' AND CreatedDate <= '" + cal_to + " 23:59:59'";
                }

                if (!string.IsNullOrEmpty(TransactionPerson))
                {
                    query += " AND TransactionPerson = @TransactionPerson";
                }

                if (!string.IsNullOrEmpty(PaymentChannel))
                {
                    query += " AND PaymentChannel = @PaymentChannel";
                }

                if (!string.IsNullOrEmpty(PaymentPage))
                {
                    query += " AND PaymentPage = @PaymentPage";
                }

                if (!string.IsNullOrEmpty(Product))
                {
                    query += " AND Product = @Product";
                }
                query += " ORDER BY CreatedDate DESC";


                ListPayment = dbConnection.Query<PaymentAppota>(
                    query,
                    new
                    {
                        TransactionPerson,
                        PaymentChannel,
                        PaymentPage,
                        Product
                    }
                ).ToList();
            }

            return ListPayment;
        }


    }
}
