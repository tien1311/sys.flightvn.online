﻿using System;
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
using Manager.Model.Models.HCNS;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System.Data.Common;
using System.Drawing.Printing;

namespace Manager.DataAccess.Repository
{
   
    public class EmployeeRepository
    {
        private IConfiguration _configuration;
        private string _connectionString;
        public EmployeeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("SQL_EV_MAIN");
        }
        public async Task<bool> CreateEmployee(EmployeeModel employee)
        {
            try
            {
                int result_insert = 0;

                //string store = "SP_INSERT_DM_NV";
                //using (var con = new SqlConnection(_connectionString))
                //{
                //    var param = new
                //    {
                //        IDGroupPermission = employee.Position,
                //        MaNV = employee.EmployeeCode,
                //        IDTen = "",
                //        TEN = employee.FirstName + " " + employee.LastName,
                //        HOLOT = employee.FirstName,
                //        TENNV = employee.LastName,
                //        TRUONGBOPHAN = false,
                //        GioiTinh = employee.Gender,
                //        ChiNhanh = "MIỀN NAM",
                //        SinhNhat = employee.DateOfBirth,
                //        DiaChiThuongTru = employee.PermanentAddress,
                //        DiaChiTamTru = employee.TemporaryAddress,
                //        DienThoai = employee.PersonalPhone
                //    };

                //    result_insert = await con.QueryAsync<SelectOption>(store, null, commandType: CommandType.Text, commandTimeout: 30);
                //}

                //string sql = "INSERT INTO [PHIEUBAOLANH] ([ID_KhachHang] ,[BaoLanh],[GhiChu] ,[NgayLap] ,[NhanVienLap] ,[NgaySua] ,[NhanVienSua] ,[NgayXoa],[NhanVienXoa],[TinhTrang],[TenDaiLy],[MaPhieu],[SoPhut]) VALUES ( @MaKH,@baolanh,@ghichu,GETDATE(),@tenNV,null,null,null,null,@tinhtrang,@tenDL,@MaPBL,@thoigian)";
                //List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                //Param.Add(new DBase.AddParameters("@MaKH", MaKH));
                //Param.Add(new DBase.AddParameters("@tenDL", tenDL));
                //Param.Add(new DBase.AddParameters("@ghichu", ghichu));
                //Param.Add(new DBase.AddParameters("@baolanh", baolanh));
                //Param.Add(new DBase.AddParameters("@tenNV", tenNV));
                //Param.Add(new DBase.AddParameters("@MaPBL", MaPBL));
                //Param.Add(new DBase.AddParameters("@tinhtrang", "1"));
                //Param.Add(new DBase.AddParameters("@ngaysua", ""));
                //Param.Add(new DBase.AddParameters("@nhanviensua", ""));
                //Param.Add(new DBase.AddParameters("@ngayxoa", ""));
                //Param.Add(new DBase.AddParameters("@nhanvienxoa", ""));
                //Param.Add(new DBase.AddParameters("@thoigian", thoigian));

                //int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);

                if (result_insert > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<EmployeeModel>> GetEmployees()
        {
            IEnumerable<EmployeeModel> result;
            string sql = @"select 
	                       EmployeeID = RowID,
	                       EmployeeCode = MANV,
	                       FirstName = TenNV,
	                       LastName = HOLOT,
	                       Gender = GioiTinh,
	                       BirthDate = SinhNhat,
	                       PermanentAddress = DiachiThuongTru,
	                       CCCD = CMND,
	                       PersonalPhone = DienThoai,
	                       CompanyPhone = DienThoaiCN
                    from DM_NV
                    where TinhTrang = 1";
            using (var con = new SqlConnection(_connectionString))
            {
                result = await con.QueryAsync<EmployeeModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
            }

            return result;
        }
        public async Task<IEnumerable<SelectOption>> GetDivision()
        {
            IEnumerable<SelectOption> result;
            string sql  = "select * from DEPARTMENT where STATUS=1 order by NAME";
            using (var con = new SqlConnection(_connectionString))
            {
                result = await con.QueryAsync<SelectOption>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
            }

            return result;
        }
        public async Task<IEnumerable<SelectOption>> GetDepartment()
        {
            IEnumerable<SelectOption> result;
            string sql = "select Code = ID, Name = Ten from PHONGBAN where TINHTRANG=1 order by Ten";
            using (var con = new SqlConnection(_connectionString))
            {
                result = await con.QueryAsync<SelectOption>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
            }

            return result;
        }


        public async Task<IEnumerable<SelectOption>> GetPosition()
        {
            IEnumerable<SelectOption> result;
            string sql = "select Code = ID, Name = Ten from CHUCVU where TINHTRANG=1 order by Ten";
            using (var con = new SqlConnection(_connectionString))
            {
                result = await con.QueryAsync<SelectOption>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
            }

            return result;
        }

        public async Task<string> GetEmployeeCode()
        {
            string result = "";
            string sql = @"SELECT top 1 convert(int,right(MaNV,len(MaNV)-2)) as ma
                        FROM [DM_NV] order by convert(int,right(MaNV,len(MaNV)-2)) desc";

            using (var con = new SqlConnection(_connectionString))
            {
                result = await con.QueryFirstAsync<string>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
            }
            if (result != "")
            {
                int ma = int.Parse(result);
                ma++;
                if (ma.ToString().Length == 1)
                    result = "NV00" + ma.ToString();
                if (ma.ToString().Length == 2)
                    result = "NV0" + ma.ToString();
                if (ma.ToString().Length == 3)
                    result = "NV" + ma.ToString();
            }
            else
                result = "NV001";

            return result;
        }
       
    }
}