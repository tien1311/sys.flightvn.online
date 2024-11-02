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
using Manager.Model.Models.HCNS;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System.Data.Common;
using System.Drawing.Printing;
using System.Net.WebSockets;
using EasyInvoice.Client;

namespace Manager.DataAccess.Repository
{

    public class EmployeeRepository
    {
        private IConfiguration _configuration;
        private string _connectionString;
        private readonly string _uploadsFolder;
        private DBase db = new DBase();
        public EmployeeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("SQL_EV_MAIN");
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }
        public bool EditEmployee(EmployeeModel employee)
        {
            int result_insert = 0;

            string store = "SP_UPDATE_DM_NV";
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        if(employee.Avatar != null)
                        {
                            var fileName = UploadFileAsync(employee.Avatar, employee.EmployeeCode).Result;
                        }    
                        var param = new
                        {
                            IDGroupPermission = 1,
                            MaNV = employee.EmployeeCode,
                            IDTen = "",
                            TEN = employee.LastName + " " + employee.FirstName,
                            HOLOT = employee.FirstName,
                            TENNV = employee.LastName,
                            GioiTinh = employee.Gender,
                            ChiNhanh = "MIỀN NAM",
                            SinhNhat = employee.BirthDate,
                            DiaChiThuongTru = employee.PermanentAddress,
                            DiaChiTamTru = employee.TemporaryAddress,
                            DienThoai = employee.PersonalPhone,
                            DienThoaiCN = employee.CompanyPhone,
                            MaChucVu = employee.Position,
                            NgayCap = employee.IssueDate,
                            NoiCap = employee.IssuedBy,
                            CCCD = employee.CCCD,
                            NgayLamViec = employee.JoiningDate,
                            NgayTinhPhep = employee.VacationDate,
                            NgayNghiViec = employee.LeavingDate,
                            Email = employee.Email,
                            MaPhongBan = employee.Department,
                            MaBoPhan = employee.Division,
                            TenDangNhap = employee.Username,
                            MatKhau = db.Encrypt(employee.Password, "tranquocquan", true),
                            TinhTrang = true,
                            CheDoLamViec = employee.WorkRegime,
                            TrangThaiLamViec = employee.WorkStatus,
                            QuyenHanCongViec = employee.JobPermissions,
                            MaSoThue = employee.PersonalTaxCode,
                            MaKeToan = employee.AccountantCode,
                            NgayCapMST = employee.TaxIssueDate,
                            EXT = employee.Extension
                        };
                        result_insert = con.Execute(store, param, transaction, commandType: CommandType.StoredProcedure, commandTimeout: 30);
                        transaction.Commit();
                        if (result_insert > 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    return false;
                }
            }

        }
        public bool CreateEmployee(EmployeeModel employee)
        {
            int result_insert = 0;

            string store = "SP_INSERT_DM_NV";
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        var fileName = UploadFileAsync(employee.Avatar, employee.EmployeeCode).Result;
                        var param = new
                        {
                            IDGroupPermission = 1,
                            MaNV = employee.EmployeeCode,
                            IDTen = "",
                            TEN = employee.LastName + " " + employee.FirstName,
                            HOLOT = employee.FirstName,
                            TENNV = employee.LastName,
                            GioiTinh = employee.Gender,
                            ChiNhanh = "MIỀN NAM",
                            SinhNhat = employee.BirthDate,
                            DiaChiThuongTru = employee.PermanentAddress,
                            DiaChiTamTru = employee.TemporaryAddress,
                            DienThoai = employee.PersonalPhone,
                            DienThoaiCN = employee.CompanyPhone,
                            MaChucVu = employee.Position,
                            NgayCap = employee.IssueDate,
                            NoiCap = employee.IssuedBy,
                            CCCD = employee.CCCD,
                            NgayLamViec = employee.JoiningDate,
                            NgayTinhPhep = employee.VacationDate,
                            NgayNghiViec = employee.LeavingDate,
                            Email = employee.Email,
                            MaPhongBan = employee.Department,
                            MaBoPhan = employee.Division,
                            TenDangNhap = employee.Username,
                            MatKhau = db.Encrypt(employee.Password, "tranquocquan", true),
                            TinhTrang = true,
                            CheDoLamViec = employee.WorkRegime,
                            TrangThaiLamViec = employee.WorkStatus,
                            QuyenHanCongViec = employee.JobPermissions,
                            MaSoThue = employee.PersonalTaxCode,
                            MaKeToan = employee.AccountantCode,
                            EXT = employee.Extension,
                            NgayCapMST = employee.TaxIssueDate,
                            TenHinh = fileName
                        };
                        result_insert = con.Execute(store, param, transaction, commandType: CommandType.StoredProcedure, commandTimeout: 30);
                        transaction.Commit();
                        if (result_insert > 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    return false;
                }
            }

        }
        public bool DeleteEmployeeID(int EmployeeID)
        {
            int result_delete = 0;
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        string sql_delete = "Update DM_NV set TinhTrang = 0, TinhTrangLamViec = 3 where RowID = @EmployeeID";
                        var param = new
                        {
                            EmployeeID = EmployeeID
                        };
                        result_delete = con.Execute(sql_delete, param, transaction, commandType: CommandType.Text, commandTimeout: 30);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            return true;

        }

        public async Task<EmployeeModel> GetEmployeeID(int ID)
        {
            try
            {
                EmployeeModel result = new EmployeeModel();
                string sql = @"select 
                        EmployeeID = RowID,
                        EmployeeCode = MANV,
                        FirstName = TenNV,
                        Gender = GioiTinh,
                        LastName = HOLOT,
                        BirthDate = SinhNhat,
                        PersonalPhone = DienThoaiCN,
                        PermanentAddress = DiachiThuongTru,
                        TemporaryAddress = DiachiTamTru,
                        CCCD = CCCD,
                        CMND = CMND,
                        IssuedBy = NoiCap,
                        IssueDate = NgayCap,
                        Department = MaPhongBan,
                        Division = MABOPHAN,
                        Position = MaChucVu,
                        Username = TenDangNhap,
                        Password = MatKhau,
                        Email = email,
                        CompanyPhone = DienThoai,
                        AccountantCode = Yahoo,
                        Extension = Extension,
                        PersonalTaxCode = MaSoThue,
                        TaxIssueDate = NgayCapMST,
                        JoiningDate = NgayLamViec,
                        VacationDate = NgayTinhPhep,
                        LeavingDate = NgayNghiViec,
                        WorkRegime = CheDoLamViec,
                        WorkStatus = TinhTrang,
                        JobPermissions = QuyenHanCongViec,
                        CompanyPhone = DienThoaiCN,
                        AvatarPreview = TenHinh
                        from DM_NV
                        where RowID = @ID
                        ";
                using (var con = new SqlConnection(_connectionString))
                {
                    var param = new
                    {
                        ID = ID
                    };
                    result = await con.QueryFirstAsync<EmployeeModel>(sql, param, commandType: CommandType.Text, commandTimeout: 30);

                    result.Password = db.Decrypt(result.Password, "tranquocquan", true);
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<IEnumerable<EmployeeModel>> GetEmployees()
        {
            try
            {
                IEnumerable<EmployeeModel> result;
                string sql = @"select 
                        EmployeeID = RowID,
                        EmployeeCode = MANV,
                        FirstName = TenNV,
                        Gender = GioiTinh,
                        LastName = HOLOT,
                        BirthDate = SinhNhat,
                        PersonalPhone = DienThoaiCN,
                        PermanentAddress = DiachiThuongTru,
                        TemporaryAddress = DiachiTamTru,
                        CCCD = CCCD,
                        IssuedBy = NoiCap,
                        IssueDate = NgayCap,
                        Department = MaPhongBan,
                        Division = MABOPHAN,
                        Position = MaChucVu,
                        Username = TenDangNhap,
                        Passowrd = MatKhau,
                        Email = email,
                        CompanyPhone = DienThoai,
                        AccountantCode = Yahoo,
                        Extension = '',
                        PersonalTaxCode = MaSoThue,
                        TaxIssueDate = NgayCapMST,
                        JoiningDate = NgayLamViec,
                        VacationDate = NgayTinhPhep,
                        LeavingDate = NgayNghiViec,
                        WorkRegime = CheDoLamViec,
                        WorkStatus = TinhTrang,
                        JobPermissions = '',
                        CompanyPhone = DienThoai,
                        AvatarPreview = TenHinh
                        from DM_NV
                        order by TinhTrang desc
                        ";
                using (var con = new SqlConnection(_connectionString))
                {
                    result = await con.QueryAsync<EmployeeModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Upload File

        public async Task<string> UploadFileAsync(IFormFile file, string EmployeeCode)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }
            // Get file extension
            var fileExtension = Path.GetExtension(file.FileName);

            // Generate a unique file name using a GUID
            var newFileName = $"{EmployeeCode}{fileExtension}";
            // Lưu file vào server
            var filePath = Path.Combine(_uploadsFolder, newFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return newFileName;
        }

        #endregion

        #region Department, Division, Position
        public async Task<IEnumerable<SelectOption>> GetDivision(string Key)
        {
            IEnumerable<SelectOption> result;
            string sql = "select *, Selected = (case when Code = @Key then 'Selected' else '' end ) from DEPARTMENT where STATUS=1 order by NAME";
            using (var con = new SqlConnection(_connectionString))
            {
                var param = new
                {
                    Key = Key
                };
                result = await con.QueryAsync<SelectOption>(sql, param, commandType: CommandType.Text, commandTimeout: 30);
            }

            return result;
        }
        public async Task<IEnumerable<SelectOption>> GetDepartment(string Key)
        {
            IEnumerable<SelectOption> result;
            string sql = "select Code = ID, Name = Ten, Selected = (case when ID = @Key then 'Selected' else '' end ) from PHONGBAN where TINHTRANG=1 order by Ten";
            using (var con = new SqlConnection(_connectionString))
            {
                var param = new
                {
                    Key = Key
                };
                result = await con.QueryAsync<SelectOption>(sql, param, commandType: CommandType.Text, commandTimeout: 30);
            }

            return result;
        }
        public async Task<IEnumerable<SelectOption>> GetPosition(string Key)
        {
            IEnumerable<SelectOption> result;
            string sql = "select Code = ID, Name = Ten, Selected = (case when ID = @Key then 'Selected' else '' end ) from CHUCVU where TINHTRANG=1 order by Ten";
            using (var con = new SqlConnection(_connectionString))
            {
                var param = new
                {
                    Key = Key
                };
                result = await con.QueryAsync<SelectOption>(sql, param, commandType: CommandType.Text, commandTimeout: 30);
            }

            return result;
        }

        #endregion

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