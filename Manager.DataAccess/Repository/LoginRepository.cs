
using Dapper;
using EasyInvoice.Client;
//using Manager.Common.Models;
using Manager.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using TangDuLieu;

namespace Manager.DataAccess.Repository
{
    public class LoginRepository
    {
        static DBase db = new DBase();
        private string SQL_EV_MAIN; /*"Data Source=27.71.232.40,1453;Initial Catalog=Manager;User ID=sa;Password=EnViet@123;";*/

        public LoginRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN = configuration.GetConnectionString("SQL_EV_MAIN");
        }
        //Kiểm tra đăng nhập
        public static AccountModel Login(string userName, string password, string SQL_EV_MAIN)
        {

            AccountModel result = new AccountModel();

            // Encrypt password
            string encryptedPassword = db.Encrypt(password, "tranquocquan", true);

            // Define the connection string and SQL query
            using (IDbConnection dbConnection = new SqlConnection(SQL_EV_MAIN))
            {
                string sql = "SELECT * FROM DM_NV WHERE TENDANGNHAP = @UserName";

                // Execute the query using Dapper
                var account = dbConnection.QueryFirstOrDefault(sql, new { UserName = userName });

                if (account != null)
                {
                    // Check if password matches
                    if (account.MatKhau == encryptedPassword)
                    {
                        if (bool.Parse(account.TinhTrang.ToString()))
                        {
                            // Populate result object with account data
                            result.RowID = Convert.ToInt32(account.RowID);
                            result.MaNV = account.Yahoo;
                            result.NgaySinh = DateTime.Parse(account.SinhNhat.ToString());
                            result.HoTen = account.Ten;
                            string[] nameParts = account.Ten.ToString().Split(' ');

                            result.Ten = (account.GioiTinh.ToString() == "Nam" ? "Mr. " : "Ms. ") + nameParts[nameParts.Length - 1];
                            result.ChiNhanh = account.ChiNhanh;
                            result.DiaChiThuongTru = account.DiaChiThuongTru;
                            result.DienThoai = account.DienThoai;
                            result.Email = account.Email;
                            result.PhongBan = account.PhongBan;
                            result.MaPhongBan = account.MaPhongBan;
                            result.TenHinh = account.TenHinh;
                            result.Per_Group = account.IDGroupPermison.ToString();
                            result.TenDangNhap = account.TenDangNhap;

                            result.Active = account.Yahoo_Status.ToString() == "3" ? "1" : "0";

                            // Fetch permissions
                            string permissionSql = "SELECT * FROM PERMISION_SYS_NV WHERE MANV = @MaNV";
                            var permissions = dbConnection.Query(permissionSql, new { MaNV = account.Yahoo });

                            foreach (var permission in permissions)
                            {
                                // Check and set each permission in the result object
                                switch (permission.FeatureID.ToString())
                                {
                                    case "1": result.TNMoi = "true"; break;
                                    case "2": result.TBao = "true"; break;
                                    case "3": result.BCVe = "true"; break;
                                    case "4": result.NBo = "true"; break;
                                    case "5": result.DLi = "true"; break;
                                    case "6": result.KToan = "true"; break;
                                    case "7": result.KDoanh = "true"; break;
                                    case "8": result.PVe = "true"; break;
                                    case "9": result.BPDoan = "true"; break;
                                    case "10": result.HDon = "true"; break;
                                    case "11": result.CA = "true"; break;
                                    case "12": result.YSao = "true"; break;
                                    case "13": result.CS = "true"; break;
                                    case "14": result.DTa = "true"; break;
                                    case "15": result.STing = "true"; break;
                                    case "16": result.KThuat = "true"; break;
                                    case "98": result.Dulich = "true"; break;
                                    default: break;
                                }
                            }
                            result.TNMoiTV = "false";
                            result.TBaoTV = "false";
                            result.BCVeTV = "false";
                            result.NBoTV = "false";
                            result.DLiTV = "false";
                            result.KToanTV = "false";
                            result.KDoanhTV = "false";
                            result.PVeTV = "false";
                            result.BPDoanTV = "false";
                            result.HDonTV = "false";
                            result.CATV = "false";
                            result.YSaoTV = "false";
                            result.CSTV = "false";
                            result.DTaTV = "false";
                            result.STingTV = "false";
                            result.KThuatTV = "false";
                            result.DulichTV = "false";
                        }
                    }
                    else
                    {
                        result.ThongBao = "Mật khẩu không đúng!";
                    }
                }
            }

            return result;
        }

        //public string LoginABC()
        //{
        //    string baseUrl = "https://www.yourwebsite.com";
        //    string loginUrl = "/Account/LogOn";
        //    string sessionUrl = "/Data";

        //    var uri = new Uri(baseUrl);

        //    CookieContainer cookies = new CookieContainer();
        //    System.Net.Http.HttpClientHandler handler = new System.Net.Http.HttpClientHandler();
        //    handler.CookieContainer = cookies;

        //    using (var client = new HttpClient(handler))
        //    {
        //        client.BaseAddress = uri;

        //        var request = new { userName = "you", password = "pwd" };
        //        var resLogin = client.PostAsJsonAsync(loginUrl, request).Result;
        //        if (resLogin.StatusCode != HttpStatusCode.OK)
        //            return "abc";

        //        // see what cookies are returned   
        //        IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
        //        foreach (Cookie cookie in responseCookies)
        //            Console.WriteLine(cookie.Name + ": " + cookie.Value);

        //        var resData = client.GetAsync(dataUrl).Result;
        //        if (resSession.StatusCode != HttpStatusCode.OK)
        //            Console.WriteLine("Could not get data html -> StatusCode = " + resSession.StatusCode);

        //        var html = resSession.Content.ReadAsStringAsync().Result;

        //        var doc = new HtmlDocument();
        //        doc.LoadHtml(html);
        //    }
        //}

        //public void Login123(string loginPageAddress, NameValueCollection loginData)
        //{
        //    var driver = new HtmlUnitDriver(true);
        //    driver.Url = @"http://www.facebook.com/login.php";

        //    var email = driver.FindElement(By.Name("email"));
        //    email.SendKeys("some@email.com");

        //    var pass = driver.FindElement(By.Name("pass"));
        //    pass.SendKeys("xxxxxxxx");

        //    var inputs = driver.FindElements(By.TagName("input"));
        //    var loginButton = (from input in inputs
        //                       where input.GetAttribute("value").ToLower() == "login"
        //                       && input.GetAttribute("type").ToLower() == "submit"
        //                       select input).First();
        //    loginButton.Click();

        //    driver.Url = @"https://m.facebook.com/profile.php?id=1111111111";
        //    Assert.That(driver.Title, Is.StringContaining("Title of page goes here"));

        //}






        public static string Templmail(string StrUserName, string StrNewPass, string StrDateTime)
        {
            string StrTempMail = @"
            <!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
            <html xmlns='http://www.w3.org/1999/xhtml'>
                        <head>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                        <title>Đại lý vé máy bay Flight VN</title>
                        <style>
            body {
	            font-family: Arial;
	            font-size: 13px;
	            margin: 0;
            }
            #content {
	            width: 90%;
	            margin: auto;
            }
            .BDwrapper {
	            width: 100%;
	            height: 80px;
	            background-image: url(http://Manager.airline24h.com/images/bg-top.png);
            }
            #wrap {
	            width: 100%;
	            height: 71px;
	            margin: auto;
	            background-image: url(http://Manager.airline24h.com/images/group-logo.png);
	            background-repeat: no-repeat;
	            background-position: left top;
            }
            .wrapper1 {
	            width: 100%;
	            background-repeat: no-repeat;
	            background-position: center;
            }
            .main {
	            padding: 20px;
	            min-height: 500px;
	            margin: auto;
	            margin: 20px 0 20px 0;
            }
            .foot {
	            background-color: #909090;
	            width: 100%;
	            height: 256px;
	            position: static;
	            bottom: 0;
	            margin: auto;
            }
            .roundedTop-corners {
	            border-top-left-radius: 10px 5px;
	            border-top-right-radius: 10px 5px;
            }
            .textfoot {
	            float: right;
	            bottom: 0px;
	            font-size: 10px;
	            color: #FFF;
	            margin: 20px 20px 0 20px;
            }
            .rounded-corners {
	            border-radius: 10px;
            }
            .foot table tr th {
	            color: #fff;
	            font-size: 13px;
	            text-transform: uppercase;
	            text-align: left
            }
            .foot table tr th span {
	            font-size: 17px;
	            float: right;
	            color: #000;
            }
            .foot table tr td {
	            color: #fff;
	            vertical-align: top;
            }
            .foot table tr td img {
	            float: left;
	            margin: 0 10px 10px 0;
            }
            .foot table tr td span {
	            margin-bottom: 3px;
            }
            .foot table tr td strong {
	            font-size: 11px;
	            font-weight: normal;
            }
            .call {
	            margin: 10px;
	            font-size: 16px;
            }
            .call p {
	            height: 20px;font-size: 12px;margin: 0 0 2px 0;padding: 0;
            }
            .header {j
                        background-color:#ff6a00;
                        color:#fff;
                        font-weight:bold;
                    }
                    .label {
                        color:#000;background-color:#ededed;
                    }
            </style>
                        </head>

                        <body>
                        <div style='width:90%;margin:auto;'>
                          <div style='width:100%;height:80px;background-image:url(http://Manager.airline24h.com/images/bg-top.png);'>
                            <div style='width:100%;height:71px;margin:auto;background-image:url(http://Manager.airline24h.com/images/group-logo.png);background-repeat:no-repeat;background-position:left top;'></div>
                          </div>
                          <div style='width:100%;background-repeat: no-repeat;background-position:center;'>
                            <div style='padding:20px;min-height:100px;margin:auto;margin:20px 0 20px 0;'> 
                              <!------ Body ---->
                              <table cellpadding='5' cellspacing='0' border='0' width='100%'>
                                <!--tr>
                                  <td colspan='4' style='font-size:11px;text-align:right;'><label style='float:left;font-size:12px;font-weight:bold;'>Công Ty cổ phần Flight VN </label>
                                   </td>
                                </tr-->
                                <tr>
                                  <td colspan='4'> Flight VN xin thông báo, chúng tôi nhận được yêu cầu khôi phục mật khẩu của bạn</td>
                                </tr>
                                <tr>
                                  <td colspan='4' align='center' valign='top'>
                                  <!--- Thong Tin Ve Hoan --->
                                    <table cellspacing='2' cellpadding='5' border='0' align='center' width='80%'>
                                      <tr>
                                        <td class='header' style='background-color:#ff6a00;color:#fff;font-weight:bold;' colspan='2'> Thông Tin Mật Khẩu Mới</td>
                                      </tr>
                                      <tr>
                                        <td class='label' style='color:#000;background-color:#ededed;width:200px;'>Tên đăng nhập</td>
                                        <td>" + StrUserName + @"</td>
                                      </tr>
                                      <tr>
                                        <td class='label' style='color:#000;background-color:#ededed;'>Mật khẩu:</td>
                                        <td>" + StrNewPass.Trim() + @"</td>
                                      </tr>
                                      <tr>
                                        <td colspan='2'> Thông tin trên được gửi vào lúc :
                                          " + StrDateTime + @"</td>
                                      </tr>
                                    </table>
                                    <!--- Thong Tin Ve Hoan --->
                                    </td>
                                </tr>
                              </table>
                              <!------ Body ----> 
                            </div>
                          </div>
                        </div>
            </body>
            </html>
            ";
            return StrTempMail;
        }


        //Khởi tạo password mới
        public static string GeneratePassword()
        {
            string strPwdchar = "abcdefghijklmnopqrstuvwxyz0123456789#+@&$ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string strPwd = "";
            Random rnd = new Random();
            for (int i = 0; i <= 7; i++)
            {
                int iRandom = rnd.Next(0, strPwdchar.Length - 1);
                strPwd += strPwdchar.Substring(iRandom, 1);
            }
            return strPwd;
        }
        //Gửi mail
        public static string SendMail_recov(string StrMailTo, string from, string subject, string body)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress(from, "no-reply<reset@enviet-group.com>");
                message.From = fromAddress;
                message.To.Add(StrMailTo);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = "<html><body>" + body + "</body></html>";
                smtpClient.Host = "mail.enviet-group.com";   // We use gmail as our smtp client
                smtpClient.Port = 25;
                smtpClient.EnableSsl = false;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new NetworkCredential("reset", "Evevgroup@458903@");
                smtpClient.Send(message);
                msg = "Successful";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
        //Reset password và gửi mail
        public static string ForgetPassword(string Email)
        {
            string message = "";
            string Strtempl = "";
            string NewPass = GeneratePassword();
            string StrDateNow = DateTime.Now.ToString("dd/MM/yyyy H:m");
            string EnsryptPass = db.MD5Encrypt(NewPass);
            string sql = " SELECT member_id FROM member WHERE member_email='" + Email + "'";
            DataTable dt = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];

            if (dt.Rows.Count > 0 && dt.Rows.Count < 2)
            {
                string SqlUp = "UPDATE member SET member_password='" + EnsryptPass + "' WHERE member_email='" + Email + "'";
                int reck = db.ExecuteNoneQuery(SqlUp, CommandType.Text, "server18", null);
                if (reck > 0)
                {
                    Strtempl = Templmail(Email, NewPass.Trim(), StrDateNow);
                    string StrAler = SendMail_recov(Email, "reset@enviet-group.com", " Thông Tin Khôi Phục Mật Khẩu ", Strtempl);//CustomerMail
                    if (StrAler == "Successful")
                    {
                        message = "Khôi phục mật khẩu thành công !";

                    }
                    else
                    {
                        message = StrAler;

                    }
                }
            }
            else
            {
                message = "Tên email không đúng";

            }
            return message;
        }
        public bool ActiveUser(string Active, string RowID)
        {
            string SqlUpdate = "";
            if (Active == "1")
            {
                SqlUpdate = @"UPDATE DM_NV SET Yahoo_Status = 3 WHERE RowID = '" + RowID + "' ";
            }
            else
            {
                SqlUpdate = @"UPDATE DM_NV SET Yahoo_Status = 1 WHERE RowID = '" + RowID + "' ";
            }
            int result = db.ExecuteNoneQuery(SqlUpdate, CommandType.Text, "server37", null);

            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public Task<string> GetActiveUserAsync(string maNV)
        {
            string Active = "", Yahoo_Status = "";
            string Sql = @"select Yahoo_Status from DM_NV WHERE Yahoo = '" + maNV + "' ";
            using (var conn = new SqlConnection(SQL_EV_MAIN))
            {
                Yahoo_Status = conn.QueryFirst<string>(Sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (Yahoo_Status == "3")
            {
                Active = "1";
            }
            else
            {
                Active = "0";
            }
            return Task.FromResult(Active);
        }

    }
}







