using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Globalization;
using System.Data.SqlClient;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class FlightGroupRepository
    {
        private string SQL_EV_MAIN_V2;/* = "Data Source=.;Initial Catalog=Manager_V2;User ID=sa;Password=EnViet@123;";*/
        private string SQL_GROUP_BOOKING; /*= "Data Source=.;Initial Catalog=GROUP_BOOKING;User ID=sa;Password=EnViet@123;";*/
        public FlightGroupRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN_V2 = configuration.GetConnectionString("SQL_EV_MAIN_V2");
            SQL_GROUP_BOOKING = configuration.GetConnectionString("SQL_GROUP_BOOKING");
        }
        public List<FlightGroupModel> ListFlightGroup()
        {
            UpdateFlightEndDate();

            List<FlightGroupModel> result = new List<FlightGroupModel>();
            string sql = @"select * from Flight where status = 1 and Active = 1 order by ID desc";
            using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
            {
                result = (List<FlightGroupModel>)conn.Query<FlightGroupModel>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            if (result.Count > 0)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    List<FlightGroupDetailModel> result_Detail = new List<FlightGroupDetailModel>();
                    string sqlFlightDetail = @"select * from Flight_Detail where  FlightID = '" + result[i].ID + "' order by KindFlight";
                    using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
                    {
                        result_Detail = (List<FlightGroupDetailModel>)conn.Query<FlightGroupDetailModel>(sqlFlightDetail, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                        conn.Close();
                    }
                    result[i].ListFlightDetail = result_Detail;
                }
            }
            return result;
        }
        public List<FlightGroupModel> SeachFlightGroup(int TinhTrang, string MaChuyenBay, string NoiDi, string NoiDen, string cal_from)
        {
            string strWhere = WhereFlightGroup(TinhTrang, MaChuyenBay, NoiDi, NoiDen, cal_from);
            List<FlightGroupModel> result = new List<FlightGroupModel>();
            string sql = @"select F.ID,F.Airline,F.NumberOfGuests,F.TotalNumberOfGuests,F.Price,F.PriceAgent,F.Status,F.TypeOfTrip,F.active,F.Condition,F.CreatedDate,F.CreatedBy,F.Specification,F.Unit,F.Fare,F.Charge,F.Discount from FLIGHT F
                            left join FLIGHT_DETAIL FD on F.ID = FD.FlightID
                            where " + strWhere + " group by F.ID,F.Airline,F.NumberOfGuests,F.TotalNumberOfGuests,F.Price,F.PriceAgent,F.Status,F.TypeOfTrip,F.active,F.Condition,F.CreatedDate,F.CreatedBy,F.Specification,F.Unit,F.Fare,F.Charge,F.Discount order by ID desc";
            using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
            {
                result = (List<FlightGroupModel>)conn.Query<FlightGroupModel>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            if (result.Count > 0)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    List<FlightGroupDetailModel> result_Detail = new List<FlightGroupDetailModel>();
                    string sqlFlightDetail = @"select * from Flight_Detail where  FlightID = '" + result[i].ID + "' order by KindFlight";
                    using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
                    {
                        result_Detail = (List<FlightGroupDetailModel>)conn.Query<FlightGroupDetailModel>(sqlFlightDetail, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                        conn.Close();
                    }
                    result[i].ListFlightDetail = result_Detail;
                }
            }
            return result;
        }
        public string WhereFlightGroup(int TinhTrang, string MaChuyenBay, string NoiDi, string NoiDen, string cal_from)
        {
            string result = "";
            if (TinhTrang < 0 && MaChuyenBay == null && NoiDi == null && NoiDen == null && cal_from == null)
            {
                result = "1 = 1";
            }
            else
            {
                result += "F.Status = 1";
            }
            if (TinhTrang >= 0)
            {
                result += "and F.Active = '" + TinhTrang + "'";
            }
            if (MaChuyenBay != null)
            {
                result += "and FD.FlightCode = '" + MaChuyenBay + "'";
            }
            if (NoiDi != null)
            {
                result += "and FD.DepartureCode = '" + NoiDi + "'";
            }
            if (NoiDen != null)
            {
                result += "and FD.ArrivalCode = '" + NoiDen + "'";
            }
            if (cal_from != null)
            {
                result += "and FD.DepartureDate >= '" + cal_from + "'";
            }

            return result;
        }
        public List<ListAirline> ListAirline()
        {
            List<ListAirline> ListAirline = new List<ListAirline>();
            string sql = "select * from AIRLINE ";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                ListAirline = (List<ListAirline>)conn.Query<ListAirline>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            return ListAirline;
        }
        public bool SaveCreateFlightGroup(FlightGroupModel data, string TenNhanVien)
        {
            int i = 0, x = 0, ID = 0, y = 1;
            string sqlFD = "";
            string sql = @"INSERT INTO [Flight] ([Airline],[TotalNumberOfGuests],[NumberOfGuests],[Status],[CreatedDate],[CreatedBy],[Condition],[Active],[Price],[PriceAgent],[Specification],[Unit],[Fare],[Charge],[Discount],[TypeOfTrip],[QuickSearch])
                                        VALUES ('" + data.Airline + "','" + data.TotalNumberOfGuests + "','" + data.NumberOfGuests + "','1',GETDATE(),N'" + TenNhanVien + "',N'" + data.Condition + "','1','" + data.Price + "','" + data.PriceAgent + "','" + data.Specification + "','" + data.Unit + "','" + data.Fare + "','" + data.Charge + "','" + data.Discount + "','" + data.TypeOfTrip + "','" + data.QuickSearch + "') SELECT SCOPE_IDENTITY() AS ID";
            using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
            {
                ID = conn.QueryFirst<int>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (ID > 0)
            {
                for (int z = 0; z < data.ListFlightDetail.Count; z++)
                {
                    sqlFD += @"INSERT INTO [Flight_Detail] ([FlightID],[FlightCode],[DepartureDate],[DepartureHour],[Status],[KindFlight],[DepartureCode],[ArrivalCode])
                                                    VALUES ('" + ID + "','" + data.ListFlightDetail[z].FlightCode + "','" + data.ListFlightDetail[z].DepartureDate + "','" + data.ListFlightDetail[z].DepartureHour + "','1','" + y + "','" + data.ListFlightDetail[z].DepartureCode + "','" + data.ListFlightDetail[z].ArrivalCode + "')";
                    y++;
                }
                using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
                {
                    x = conn.Execute(sqlFD, null, null, commandTimeout: 30, commandType: System.Data.CommandType.Text);
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
            else
                return false;
        }
        public bool SavePlusFlightGroup(FlightGroupModel data, string TenNhanVien)
        {
            int i = 0, x = 0, ID = 0, y = 1;
            string sqlFD = "";
            string sql = @"INSERT INTO [Flight] ([Airline],[TotalNumberOfGuests],[NumberOfGuests],[Status],[CreatedDate],[CreatedBy],[Condition],[Active],[Price],[PriceAgent],[Specification],[Unit],[Fare],[Charge],[Discount],[TypeOfTrip],[QuickSearch])
                                        VALUES ('" + data.Airline + "','" + data.TotalNumberOfGuests + "','" + data.NumberOfGuests + "','1',GETDATE(),N'" + TenNhanVien + "',N'" + data.Condition + "','1','" + data.Price + "','" + data.PriceAgent + "','" + data.Specification + "','" + data.Unit + "','" + data.Fare + "','" + data.Charge + "','" + data.Discount + "','" + data.TypeOfTrip + "','" + data.QuickSearch + "') SELECT SCOPE_IDENTITY() AS ID";
            using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
            {
                ID = conn.QueryFirst<int>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (ID > 0)
            {
                for (int z = 0; z < data.ListFlightDetail.Count; z++)
                {
                    sqlFD += @"INSERT INTO [Flight_Detail] ([FlightID],[FlightCode],[DepartureDate],[DepartureHour],[Status],[KindFlight],[DepartureCode],[ArrivalCode])
                                                    VALUES ('" + ID + "','" + data.ListFlightDetail[z].FlightCode + "','" + data.ListFlightDetail[z].DepartureDate + "','" + data.ListFlightDetail[z].DepartureHour + "','1','" + y + "','" + data.ListFlightDetail[z].DepartureCode + "','" + data.ListFlightDetail[z].ArrivalCode + "')";
                    y++;
                }
                using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
                {
                    x = conn.Execute(sqlFD, null, null, commandTimeout: 30, commandType: System.Data.CommandType.Text);
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
            else
                return false;
        }
        public FlightGroupModel EditFlightGroup(int ID)
        {
            FlightGroupModel result = new FlightGroupModel();
            string sql = @"select F.*  from Flight F where F.status = 1 and ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
            {
                result = conn.QueryFirst<FlightGroupModel>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            if (result != null)
            {
                List<FlightGroupDetailModel> result_Detail = new List<FlightGroupDetailModel>();
                string sqlFlightDetail = @"select * from Flight_Detail where  FlightID = '" + result.ID + "' order by KindFlight";
                using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
                {
                    result_Detail = (List<FlightGroupDetailModel>)conn.Query<FlightGroupDetailModel>(sqlFlightDetail, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
                result.ListFlightDetail = result_Detail;
                result.ListAirline = ListAirline();
            }
            return result;
        }
        public bool SaveEditFlightGroup(FlightGroupModel data, string TenNhanVien)
        {
            int i = 0, x = 0;
            string sqlFD = "";
            string sql = @"UPDATE [Flight] SET [Airline] = '" + data.Airline + "',[TotalNumberOfGuests] = '" + data.TotalNumberOfGuests + "',[NumberOfGuests] = '" + data.NumberOfGuests + "',[UpdatedBy] = N'" + TenNhanVien + "',[UpdatedDate] = GETDATE(),[Condition] = N'" + data.Condition + "',[Price] = '" + data.Price + "',[PriceAgent] = '" + data.PriceAgent + "',[Specification] = '" + data.Specification + "',[Unit] = '" + data.Unit + "',[Fare] = '" + data.Fare + "',[Charge] = '" + data.Charge + "',[Discount] = '" + data.Discount + "',[TypeOfTrip] = '" + data.TypeOfTrip + "', [QuickSearch] = '" + data.QuickSearch + "' where ID = '" + data.ID + "'";
            using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: System.Data.CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                for (int z = 0; z < data.ListFlightDetail.Count; z++)
                {
                    if (data.ListFlightDetail[z].ID > 0)
                    {
                        sqlFD += @"UPDATE [Flight_Detail] SET [FlightCode] = '" + data.ListFlightDetail[z].FlightCode + "',[DepartureDate] = '" + data.ListFlightDetail[z].DepartureDate + "',[DepartureHour] = '" + data.ListFlightDetail[z].DepartureHour + "',[DepartureCode] = '" + data.ListFlightDetail[z].DepartureCode + "',[ArrivalCode] = '" + data.ListFlightDetail[z].ArrivalCode + "' where ID = '" + data.ListFlightDetail[z].ID + "'";
                    }
                    else
                    {
                        sqlFD += @"INSERT INTO [Flight_Detail] ([FlightID],[FlightCode],[DepartureDate],[DepartureHour],[Status],[KindFlight],[DepartureCode],[ArrivalCode])
                                                    VALUES ('" + data.ID + "','" + data.ListFlightDetail[z].FlightCode + "','" + data.ListFlightDetail[z].DepartureDate + "','" + data.ListFlightDetail[z].DepartureHour + "','1','2','" + data.ListFlightDetail[z].DepartureCode + "','" + data.ListFlightDetail[z].ArrivalCode + "')";
                    }
                }
                using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
                {
                    x = conn.Execute(sqlFD, null, null, commandTimeout: 30, commandType: System.Data.CommandType.Text);
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
            else
                return false;
        }
        public bool DeleteFlightGroup(int ID)
        {
            int i = 0;
            string sql = @"UPDATE [Flight] SET [Active] = '0' where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: System.Data.CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool ActiveFlightGroup(int ID)
        {
            int i = 0;
            string sql = @"UPDATE [Flight] SET [Active] = '1' where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: System.Data.CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool UpdateFlightEndDate()
        {
            int i = 0;
            string sql = @"update Flight
                            set Status = 0
                            where ID  in (select A.ID from Flight A
                                           left join Flight_Detail B on A.ID = B.FlightID
                                           where B.KindFlight = 1 and A.Status = 1 and B.DepartureDate < GETDATE())";
            using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: System.Data.CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public List<BookingFlightGroup> DanhSachVedoan(string dateFrom, string dateTo)
        {
            List<BookingFlightGroup> result = new List<BookingFlightGroup>();
            string sql = "select * from BOOKING where CreatedDate >='" + dateFrom + "' and CreatedDate <='" + dateTo + "' order by ID desc";
            using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
            {
                result = (List<BookingFlightGroup>)conn.Query<BookingFlightGroup>(sql, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            return result;
        }
        public DetailBookingFlightGroup DetailBooking(int ID_Booking)
        {
            DetailBookingFlightGroup result = new DetailBookingFlightGroup();
            List<BookingFlight> ListFlights = new List<BookingFlight>();
            List<BookingPassenger> ListPassenger = new List<BookingPassenger>();

            string sql1 = "select * from BOOKING_FLIGHT where ID_Booking = '" + ID_Booking + "' order by RouteNo ";
            using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
            {
                ListFlights = (List<BookingFlight>)conn.Query<BookingFlight>(sql1, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            string sql2 = "select * from BOOKING_PASSENGER where ID_Booking = '" + ID_Booking + "' order by ID desc";
            using (var conn = new SqlConnection(SQL_GROUP_BOOKING))
            {
                ListPassenger = (List<BookingPassenger>)conn.Query<BookingPassenger>(sql2, null, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Close();
            }

            result.ListFlights = ListFlights;
            result.ListPassenger = ListPassenger;
            return result;
        }
    }
}
