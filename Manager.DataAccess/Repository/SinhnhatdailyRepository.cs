using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using TangDuLieu;
using Manager.Model.Models;
using Manager.DataAccess.Services.Sinhnhatkhachvip;
using System.Net;

namespace Manager.DataAccess.Repository
{
    public class SinhnhatdailyRepository
    {
        DBase db = new DBase();
        private readonly SinhnhatDbContext _context;
        Mail baoNS_KHV = new Mail("EVM_THONGBAOSINHNHATKHACHVIP");
        private readonly string connectionString;
        public SinhnhatdailyRepository(SinhnhatDbContext context, IConfiguration configuration)
        {
            _context = context;
            connectionString = configuration.GetConnectionString("SQL_EV_MAIN");
        }
        

        public async Task<List<KHACHHANGVIP>> GetCustomersWithBirthdayTodayAsync()
        {
            var tomorrow = DateTime.Today.AddDays(1);
            var month = tomorrow.Month;
            var day = tomorrow.Day;

            
            var commandText = @"
        SELECT * 
        FROM [dbo].[KHACHHANGVIP] 
        WHERE MONTH(NgaySinh) = @Month 
        AND DAY(NgaySinh) = @Day
        AND IsHoTro = 1"; 

            

            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                // Execute the query with Dapper
                var customers = (await connection.QueryAsync<KHACHHANGVIP>(commandText, new { Month = month, Day = day })).ToList();

                return customers;
            }
        }








        public async Task SendBirthdayEmailsAsync()
        {
            // Lấy danh sách nhân viên PR từ DSNhanVienPR()
            var nhanVienList = DSNhanVienPR();

            var birthdayCustomers = await GetCustomersWithBirthdayTodayAsync();

            if (!birthdayCustomers.Any())
            {
                Console.WriteLine("Không có khách hàng nào có sinh nhật hôm nay.");
                return;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                // Duyệt qua từng khách hàng có sinh nhật hôm nay
                foreach (var customer in birthdayCustomers)
                {
                    bool isMailSent = await SendMailSinhNhatKhachHangAsync(
                        FULLNAME: customer.HOTEN,
                        DONVI: customer.DONVI,
                        NGAYSINH: customer.NgaySinh,
                        HANG: customer.Hang,
                        MIEN: customer.Mien,
                        SODIENTHOAI: customer.SODIENTHOAI,
                        DIACHI: customer.DIACHI,
                        NGUOIGIOITHIEU: customer.NguoiGioiThieu,
                        NHOM: customer.NHOM,
                        NHANVIENKINHDOANH: customer.NHANVIENKINHDOANH
                    );

                    if (isMailSent)
                    {
                        string ngaysinh = customer.NgaySinh.HasValue ? customer.NgaySinh.Value.ToString("dd/MM/yyyy") : "";

                        // Nếu khách hàng có NHANVIENKINHDOANH, lưu thông tin cho người đó
                        if (customer.NHOM == "Đại Lý")
                        {
                            var insertCommandText = @"
                        INSERT INTO [dbo].[ThongBaoSinhNhatDaiLi] 
                            ([NguoiNhan], [NgaySinh], [NguoiTiepQuan], [NgayTao],[ChucVu],[Hang],[Mien],[DaXem]) 
                        VALUES 
                            (@NguoiNhan, @NgaySinh, @NguoiTiepQuan, @NgayTao,@ChucVu,@Hang,@Mien,@DaXem)";

                            var parameters = new
                            {
                                NguoiNhan = customer.HOTEN,
                                NgaySinh = customer.NgaySinh,
                                NguoiTiepQuan = customer.NHANVIENKINHDOANH,  
                                NgayTao = DateTime.Now,
                                ChucVu = customer.DONVI,
                                Hang = customer.Hang,
                                Mien = customer.Mien,
                                DaXem = 0
                            };

                            await connection.ExecuteAsync(insertCommandText, parameters);
                            Console.WriteLine($"Lưu thông tin sinh nhật cho {customer.HOTEN} với người tiếp quản {customer.NHANVIENKINHDOANH}.");
                        }

                        // Luôn lưu thông tin cho toàn bộ nhân viên PR
                        foreach (var nhanVien in nhanVienList)
                        {
                            var insertCommandTextForPR = @"
                        INSERT INTO [dbo].[ThongBaoSinhNhatDaiLy] 
                            ([NguoiNhan], [NgaySinh], [NguoiTiepQuan], [NgayTao],[ChucVu],[Hang],[Mien],[DaXem]) 
                        VALUES 
                            (@NguoiNhan, @NgaySinh, @NguoiTiepQuan, @NgayTao,@ChucVu,@Hang,@Mien,@DaXem)";

                            var parametersForPR = new
                            {
                                NguoiNhan = customer.HOTEN,
                                NgaySinh = customer.NgaySinh,
                                NguoiTiepQuan = nhanVien.Yahoo,  
                                NgayTao = DateTime.Now,
                                ChucVu = customer.DONVI,
                                Hang = customer.Hang,
                                Mien = customer.Mien,
                                DaXem = 0
                            };

                            await connection.ExecuteAsync(insertCommandTextForPR, parametersForPR);
                            Console.WriteLine($"Lưu thông tin sinh nhật cho {customer.HOTEN} với người tiếp quản {nhanVien.Yahoo}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Không thể gửi email cho {customer.HOTEN}, bỏ qua việc lưu vào cơ sở dữ liệu.");
                    }
                }
            }
        }




        public async Task<string> GetEmailKinhDoanh(string tenNhanVien)
        {
            string email = null;
            string sql = "SELECT TOP 1 Email FROM DM_NV WHERE TINHTRANG = 1 AND Yahoo = @TenNhanVien";

            
            using (var connection = new SqlConnection(connectionString))
            {
                
                email = await connection.QueryFirstOrDefaultAsync<string>(sql, new { TenNhanVien = tenNhanVien });
            }

            return email;
        }
        public async Task<string> GetYahooKinhDoanh(string tenNhanVien)
        {
            string Yahoo = null;
            string sql = "SELECT TOP 1 Yahoo FROM DM_NV WHERE TINHTRANG = 1 AND Yahoo = @TenNhanVien";


            using (var connection = new SqlConnection(connectionString))
            {

                Yahoo = await connection.QueryFirstOrDefaultAsync<string>(sql, new { TenNhanVien = tenNhanVien });
            }

            return Yahoo;
        }

        public async Task<bool> SendMailSinhNhatKhachHangAsync(string FULLNAME, string DONVI, DateTime? NGAYSINH, string HANG, string MIEN, string SODIENTHOAI, string DIACHI, string NGUOIGIOITHIEU, string NHOM, string NHANVIENKINHDOANH)
        {
                       
            using (var mail = new MailMessage(baoNS_KHV.username, baoNS_KHV.CC))
            {
                mail.From = new MailAddress(baoNS_KHV.username, "ENVIET PR");

                using (var client = new SmtpClient())
                {
                    client.EnableSsl = true;
                    client.Port = baoNS_KHV.port;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(baoNS_KHV.username, new DBase().Decrypt(baoNS_KHV.password, "vodacthe", true));
                    client.Host = baoNS_KHV.host;

                    string subject = "THÔNG BÁO SINH NHẬT KHÁCH VIP";


                    if (NHOM == "Đại lý")
                    {
                        string emailkdEmployees = await GetEmailKinhDoanh(NHANVIENKINHDOANH);
                        try
                        {

                            mail.CC.Add(emailkdEmployees);
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {

                        }
                        catch { }
                    }

                    mail.Subject = subject;

                    // Đọc nội dung email từ template
                    string mailBody;
                    using (var httpClient = new HttpClient())
                    {
                        mailBody = await httpClient.GetStringAsync(baoNS_KHV.templateUrl);
                    }

                    string formattedNgaysinh = NGAYSINH.HasValue ? NGAYSINH.Value.ToString("MM/dd/yyyy") : string.Empty;


                    mailBody = mailBody.Replace("$_Fullname", FULLNAME);
                    mailBody = mailBody.Replace("$_Donvi", DONVI);
                    mailBody = mailBody.Replace("$_Ngaysinh", formattedNgaysinh);
                    mailBody = mailBody.Replace("$_Hang", "VN");
                    mailBody = mailBody.Replace("$_Mien", "Miền Nam");
                    mailBody = mailBody.Replace("$_Sodienthoai", SODIENTHOAI);
                    mailBody = mailBody.Replace("$_Email", "");
                    mailBody = mailBody.Replace("$_Diachi", DIACHI);
                    mailBody = mailBody.Replace("$_Nguoigioithieu", NGUOIGIOITHIEU);

                    mail.Body = mailBody;
                    mail.IsBodyHtml = true;


                    try
                    {
                        await client.SendMailAsync(mail);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
                        return false;
                    }
                }
            }
            return true;
        }





        public List<ListNhanVienMK> DSNhanVienPR()
        {
            List<ListNhanVienMK> result = new List<ListNhanVienMK>();
            string sql_NoiDung = " SELECT Email,Yahoo FROM DM_NV WHERE MABOPHAN='MK' AND TINHTRANG=1 ";
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListNhanVienMK ten = new ListNhanVienMK();
                        
                        ten.Email = dt.Rows[i]["Email"].ToString();
                        ten.Yahoo = dt.Rows[i]["Yahoo"].ToString();
                        result.Add(ten);
                    }
                }
            }
            return result;
        }
    }
}
