using ExcelDataReader;
using Microsoft.AspNetCore.Http;
//using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;
using Dapper;
using System.Data.SqlClient;
using System.Globalization;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class BookerClubRepository
    {
        private  string SQL_EV_MAIN_V2;
        private  string SQL_Agent_MAIN;
        DBase db = new DBase();
        public BookerClubRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN_V2 = configuration.GetConnectionString("SQL_EV_MAIN_V2");
            SQL_Agent_MAIN = configuration.GetConnectionString("SQL_Agent_MAIN");
        }
        public ThongTinBookerClubModel ChuongTrinhBookerClub()
        {
            ThongTinBookerClubModel result = new ThongTinBookerClubModel();
            List<BookerClubDetail> listBookerClubDetail = new List<BookerClubDetail>();
            List<BookerClub> listBookerClub = new List<BookerClub>();

            string sql = @"select ID,Title, Status, DateFrom, DateTo from BookerClub";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                listBookerClub = (List<BookerClub>)conn.Query<BookerClub>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            result.ListBookerClub = listBookerClub;
            result.ListBookerClubDetail = listBookerClubDetail;
            return result;
        }
        public bool EditStatusBC(string title, string Status)
        {
            int i = 0;
            string sql = @"update BookerClub set Status = '" + Status + "' where Title = N'" + title + "'";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }

        public List<BookerClubDetail> ReadExcelFileXoSo(IFormFile file)
        {
            List<BookerClubDetail> ListBookerClubDetail = new List<BookerClubDetail>();
            using (var stream = new MemoryStream())
            {
                file.CopyToAsync(stream);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int i = 0;
                    while (reader.Read()) //Each row of the file
                    {
                        if (i != 0)
                        {
                            BookerClubDetail BLDetail = new BookerClubDetail();
                            BLDetail.MaKH = reader.GetValue(0).ToString();
                            BLDetail.ID_Booker = reader.GetValue(1).ToString();
                            ListBookerClubDetail.Add(BLDetail);
                        }
                        i++;
                    }
                }
            }
            return ListBookerClubDetail;
        }
        public bool InsertListBookerClubDetail(List<BookerClubDetail> bookClubDetails, string MaNV)
        {
            int result = 0, dem = 0;
            string SqlInsert = "";
            for (int i = 0; i < bookClubDetails.Count; i++)
            {
                SqlInsert += @"INSERT INTO BookerClubDetail (ID_BookerClub,MaKH, TicketNumber, ID_Booker, Status, CreateDate, CreateEmployee) VALUES (N'" + bookClubDetails[i].ID_BookerClub + "',N'" + bookClubDetails[i].MaKH + "',N'" + bookClubDetails[i].TicketNumber + "', N'" + bookClubDetails[i].ID_Booker + "',  1, '" + DateTime.Now + "',N'" + MaNV + "')";
                dem++;
                if (dem == 80)
                {
                    result = db.ExecuteNoneQuery(SqlInsert, CommandType.Text, "server18", null);
                    SqlInsert = "";
                    dem = 0;
                }
            }
            if (dem > 0)
            {
                result = db.ExecuteNoneQuery(SqlInsert, CommandType.Text, "server18", null);
            }

            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public bool InsertBookerClub(string title, DateTime dateform, DateTime dateto, string description, int soluong, string MaNV)
        {
            int result = 0;
            string SqlInsert = @"INSERT INTO BookerClub (Title, Description, DateFrom, DateTo, Status, CreateDate, CreateEmployee) VALUES (N'" + title.Trim() + "', N'" + description + "', '" + dateform + "', '" + dateto + "', 1, '" + DateTime.Now + "',N'" + MaNV + "') SELECT SCOPE_IDENTITY() ";
            result = int.Parse(db.ExecuteDataSet(SqlInsert, CommandType.Text, "server18", null).Tables[0].Rows[0][0].ToString());
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public bool UpdateBookerClub(string title, DateTime dateform, DateTime dateto, string description, int soluong, string MaNV, string ID)
        {
            int result = 0;
            string SqlInsert = @"Update BookerClub  set title = N'" + title.Trim() + "', Description = N'" + description + "', DateFrom = '" + dateform + "', DateTo = '" + dateto + "', EditDate = GetDate(), EditEmployee='" + MaNV + "' where ID =  " + ID;
            result = db.ExecuteNoneQuery(SqlInsert, CommandType.Text, "server18", null);
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public List<BookerClubDetail> ListBookerClub(string title)
        {
            List<BookerClubDetail> listBookerClubDetail = new List<BookerClubDetail>();
            string sqlDaiLy = @"select ID, MaKH, SoLuong, Title, DateFrom, DateTo from BookerClub where Title = N'" + title + "'";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                listBookerClubDetail = (List<BookerClubDetail>)conn.Query<BookerClubDetail>(sqlDaiLy, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return listBookerClubDetail;
        }
        public string DescriptionBookerClub(string khoachinh)
        {
            string result = "";
            string sqlDaiLy = @"select Description from BookerClub where ID = " + khoachinh;
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                result = conn.QueryFirst<string>(sqlDaiLy, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool UpdateStatusDetail(int Status, int ID)
        {
            int i = 0;
            string sql = @"update BookerClub set Status = " + Status + " where ID = " + ID;
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public BookerClub GetBookerClub(int ID)
        {
            BookerClub result = new BookerClub();
            string sql = @"select * from BookerClub where ID = " + ID;
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                result = conn.QueryFirst<BookerClub>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public List<BookerClubDetail> GetBookerClubDetail(int ID)
        {
            List<BookerClubDetail> result = new List<BookerClubDetail>();
            string sql = @"select * from BookerClubDetail where ID_BookerClub = " + ID;
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                result = (List<BookerClubDetail>)conn.Query<BookerClubDetail>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool DeleteDetail(int ID)
        {
            int i = 0;
            string sql = @"delete BookerClubDetail  where ID = " + ID;
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public List<BookerClubDetail> SearchMaKHDetail(string MAKH, int ID)
        {
            List<BookerClubDetail> result = new List<BookerClubDetail>();
            string sql = @"select * from BookerClubDetail  where ID_BookerClub = " + ID + " and MaKH = '" + MAKH + "'";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                result = (List<BookerClubDetail>)conn.Query<BookerClubDetail>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public List<INFO_IDBookerModel> ListIDBooker()
        {
            List<INFO_IDBookerModel> result = new List<INFO_IDBookerModel>();
            string sql = "select * from ID_BOOKER_INFO order by UpdateDate desc";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<INFO_IDBookerModel>)conn.Query<INFO_IDBookerModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public List<INFO_IDBookerModel> SearchIDBooker(string select, string value)
        {
            string sql = "";
            List<INFO_IDBookerModel> result = new List<INFO_IDBookerModel>();
            if (value == "" || value == null)
            {
                sql = "select * from ID_BOOKER_INFO  order by UpdateDate desc";
            }
            else
            {
                if (select == "MaKH")
                {
                    sql = "select * from ID_BOOKER_INFO where CreateUp = '" + value.Trim() + "' order by UpdateDate desc";
                }
                else
                {
                    sql = "select * from ID_BOOKER_INFO where ID_BOOKER = '" + value.Trim() + "' order by UpdateDate desc";
                }
            }
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                result = (List<INFO_IDBookerModel>)conn.Query<INFO_IDBookerModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool DelIDBooker(string ID)
        {
            int i = 0;
            string sql = @"delete ID_BOOKER_INFO where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_EV_MAIN_V2))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
