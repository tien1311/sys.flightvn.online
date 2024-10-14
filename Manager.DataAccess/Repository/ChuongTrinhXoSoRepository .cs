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
    public class ChuongTrinhXoSoRepository
    {
        private  string SQL_Agent_MAIN;
        DBase db = new DBase();
        public ChuongTrinhXoSoRepository(IConfiguration configuration)
        {
            SQL_Agent_MAIN = configuration.GetConnectionString("SQL_Agent_MAIN");
        }
        public ThongTinXoSoModel ChuongTrinhXoSo()
        {
            ThongTinXoSoModel result = new ThongTinXoSoModel();
            List<XoSoDetail> listXoSoDetail = new List<XoSoDetail>();
            List<XoSo> listXoSo = new List<XoSo>();

            string sql = @"select ID,Title, Status, DateFrom, DateTo, SoLuong from ChuongTrinhXoSo";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                listXoSo = (List<XoSo>)conn.Query<XoSo>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            result.ListXoSo = listXoSo;
            result.ListXoSoDetail = listXoSoDetail;
            return result;
        }
        public bool EditStatusCTXS(string title, string Status)
        {
            int i = 0;
            string sql = @"update ChuongTrinhXoSo set Status = '" + Status + "' where Title = N'" + title + "'";
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

        public List<XoSoDetail> ReadExcelFileXoSo(IFormFile file)
        {
            List<XoSoDetail> ListXoSoDetail = new List<XoSoDetail>();
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
                            XoSoDetail DLKM = new XoSoDetail();
                            DLKM.MaKH = reader.GetValue(0).ToString();
                            DLKM.SoLuong = int.Parse(reader.GetValue(1).ToString());
                            ListXoSoDetail.Add(DLKM);
                        }
                        i++;
                    }
                }
            }
            return ListXoSoDetail;
        }
        public bool InsertListXoSoDetail(List<XoSoDetail> xoSoDetails, string MaNV)
        {
            int result = 0, dem = 0;
            string SqlInsert = "";
            for (int i = 0; i < xoSoDetails.Count; i++)
            {
                SqlInsert += @"INSERT INTO ChuongTrinhXoSoDetail (IDXoSo,MaKH, SoLuong, Status, CreateDate, CreateEmployee) VALUES (" + xoSoDetails[i].IDXoSo + ",'" + xoSoDetails[i].MaKH + "', '" + xoSoDetails[i].SoLuong + "',  1, '" + DateTime.Now + "',N'" + MaNV + "')";
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
        //Kiểm tra mã KH đã có chưa 
        public bool CheckMaKHXoSo(string MaKH, int IDXoSo)
        {
            bool result = true;
            try
            {
                int result_sql = 0;
                string sql = "select * from ChuongTrinhXoSoDetail where MAKH = '" + MaKH + "' and  IDXoSo = " + IDXoSo;
                using (var conn = new SqlConnection(SQL_Agent_MAIN))
                {
                    result_sql = conn.QueryFirst<int>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                if (result_sql > 0)
                {
                    result = false;
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        public bool InsertXoSo(string title, DateTime dateform, DateTime dateto, string description, int soluong, string MaNV)
        {
            //DateTime dFrom = DateTime.ParseExact(dateform, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            //DateTime dTo = DateTime.ParseExact(dateto, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            int result = 0;
            int result_number = 0;
            int dem = 0;
            string SqlInsert = @"INSERT INTO ChuongTrinhXoSo (SoLuong, Title, Description, DateFrom, DateTo, Status, CreateDate, CreateEmployee) VALUES (" + soluong + ", N'" + title.Trim() + "', N'" + description + "', '" + dateform + "', '" + dateto + "', 1, '" + DateTime.Now + "',N'" + MaNV + "') SELECT SCOPE_IDENTITY() ";
            result = int.Parse(db.ExecuteDataSet(SqlInsert, CommandType.Text, "server18", null).Tables[0].Rows[0][0].ToString());
            if (result > 0)
            {
                string sqlInsertNumber = "";
                int length_soluong = soluong.ToString().Length;
                for (int i = 1; i <= soluong; i++)
                {
                    string D = "D" + length_soluong;
                    sqlInsertNumber += @"INSERT INTO Number_ChuongTrinhXoSo (Number, IDXoSo) VALUES ('" + i.ToString(D) + "',N'" + result + "')";
                    dem++;
                    if (dem == 80)
                    {
                        result_number = db.ExecuteNoneQuery(sqlInsertNumber, CommandType.Text, "server18", null);
                        sqlInsertNumber = "";
                        dem = 0;
                    }
                }
                if (dem > 0)
                {
                    result_number = db.ExecuteNoneQuery(sqlInsertNumber, CommandType.Text, "server18", null);
                }

                return true;
            }
            return false;
        }
        //public bool InsertNumberXoSo(List<string> Number, int XoSo)
        //{
        //    for (int i = 0; i < length; i++)
        //    {

        //    }
        //    return true;
        //}
        public bool UpdateXoSo(string title, DateTime dateform, DateTime dateto, string description, int soluong, string MaNV, string ID)
        {
            //DateTime dFrom = DateTime.ParseExact(dateform, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            //DateTime dTo = DateTime.ParseExact(dateto, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            int result = 0;
            string SqlInsert = @"Update ChuongTrinhXoSo  set title = N'" + title.Trim() + "', Description = N'" + description + "', DateFrom = '" + dateform + "', DateTo = '" + dateto + "', EditDate = GetDate(), EditEmployee='" + MaNV + "' where ID =  " + ID;
            result = db.ExecuteNoneQuery(SqlInsert, CommandType.Text, "server18", null);
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public List<XoSoDetail> ListXoSo(string title)
        {
            List<XoSoDetail> listDaiLy = new List<XoSoDetail>();
            string sqlDaiLy = @"select ID, MaKH, SoLuong, Title, DateFrom, DateTo from ChuongTrinhXoSo where Title = N'" + title + "'";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                listDaiLy = (List<XoSoDetail>)conn.Query<XoSoDetail>(sqlDaiLy, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return listDaiLy;
        }
        public string DescriptionXoSo(string khoachinh)
        {
            string result = "";
            string sqlDaiLy = @"select Description from ChuongTrinhXoSo where ID = " + khoachinh;
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                result = conn.QueryFirst<string>(sqlDaiLy, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool UpdateStatusCTXS(int Status, int ID)
        {
            int i = 0;
            string sql = @"update ChuongTrinhXoSo set Status = " + Status + " where ID = " + ID;
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
        public XoSo GetChuongTrinhXoSo(int ID)
        {
            XoSo result = new XoSo();
            string sql = @"select * from ChuongTrinhXoSo where ID = " + ID;
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                result = conn.QueryFirst<XoSo>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public List<XoSoDetail> GetChiTietChuongTrinhXoSo(int ID)
        {
            List<XoSoDetail> result = new List<XoSoDetail>();
            string sql = @"select *,(select Count(N.MaKH) from Number_ChuongTrinhXoSo N where N.IDXoSo = CT.IDXoSo  and N.MaKH = CT.MaKH) as SoLuongDaChon  from ChuongTrinhXoSoDetail CT where CT.IDXoSo = " + ID;
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                result = (List<XoSoDetail>)conn.Query<XoSoDetail>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public bool DeleteDetailXoSo(int ID)
        {
            int i = 0;
            string sql = @"delete ChuongTrinhXoSoDetail  where ID = " + ID;
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
        public List<XoSoDetail> SearchMaKHDetailXoSo(string MAKH, int ID)
        {
            List<XoSoDetail> result = new List<XoSoDetail>();
            string sql = @"select CT.*,(select Count(N.MaKH) from Number_ChuongTrinhXoSo N where N.IDXoSo = CT.IDXoSo  and N.MaKH = CT.MaKH) as SoLuongDaChon from ChuongTrinhXoSoDetail CT  where CT.IDXoSo = " + ID + " and MaKH = '" + MAKH + "'";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                result = (List<XoSoDetail>)conn.Query<XoSoDetail>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public List<Number_XoSoDetail> NumberKHXoSo(int ID)
        {
            List<Number_XoSoDetail> result = new List<Number_XoSoDetail>();
            string sql = @"select * from Number_ChuongTrinhXoSo where IDXoSo = " + ID + " and MaKH is not null";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                result = (List<Number_XoSoDetail>)conn.Query<Number_XoSoDetail>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
    }
}
