
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
    public class LoginRepository : WebClient
    {
        static DBase db = new DBase();
        private string SQL_EV_MAIN; /*"Data Source=27.71.232.40,1453;Initial Catalog=Manager;User ID=sa;Password=EnViet@123;";*/

        public LoginRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN = configuration.GetConnectionString("SQL_EV_MAIN");
        }
        //Kiểm tra đăng nhập
        public static AccountModel Login(string UserName, string Password)
        {
            AccountModel result = new AccountModel();
            //Mã hóa password
            string StrPassword = db.Encrypt(Password, "tranquocquan", true);
            string s = db.Encrypt("Manager.airline24h.com", "tranquocquan", true);
            string sql = "select * from DM_NV where TENDANGNHAP='" + UserName + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb.Rows.Count > 0)
            {
                string strMatKhau = tb.Rows[0]["MATKHAU"].ToString();
                if (StrPassword == strMatKhau)
                {
                    if (bool.Parse(tb.Rows[0]["TINHTRANG"].ToString()) == true)
                    {
                        result.RowID = Convert.ToInt32(tb.Rows[0]["RowID"].ToString());
                        result.MaNV = tb.Rows[0]["Yahoo"].ToString();
                        result.NgaySinh = DateTime.Parse(tb.Rows[0]["SinhNhat"].ToString());
                        result.HoTen = tb.Rows[0]["Ten"].ToString();
                        string[] Ten = tb.Rows[0]["Ten"].ToString().Split(' ');
                        if (tb.Rows[0]["GioiTinh"].ToString() == "Nam")
                        {
                            result.Ten = "Mr." + Ten[Ten.Length - 1];
                        }
                        else
                        {
                            result.Ten = "Ms." + Ten[Ten.Length - 1];
                        }

                        result.ChiNhanh = tb.Rows[0]["ChiNhanh"].ToString();
                        result.DiaChiThuongTru = tb.Rows[0]["DiaChiThuongTru"].ToString();
                        result.DienThoai = tb.Rows[0]["DienThoai"].ToString();
                        result.Email = tb.Rows[0]["Email"].ToString();
                        result.PhongBan = tb.Rows[0]["PhongBan"].ToString();
                        result.MaPhongBan = tb.Rows[0]["MaPhongBan"].ToString();
                        result.TenHinh = tb.Rows[0]["TenHinh"].ToString();
                        result.Per_Group = tb.Rows[0]["IDGroupPermison"].ToString();
                        result.TenDangNhap = tb.Rows[0]["TenDangNhap"].ToString();
                        if (tb.Rows[0]["Yahoo_Status"].ToString() == "3")
                        {
                            result.Active = "1";
                        }
                        else
                        {
                            result.Active = "0";
                        }
                        string TNMoi = "";
                        string TBao = "";
                        string BCVe = "";
                        string NBo = "";
                        string DLi = "";
                        string KToan = "";
                        string KDoanh = "";
                        string PVe = "";
                        string BPDoan = "";
                        string HDon = "";
                        string CA = "";
                        string YSao = "";
                        string CS = "";
                        string DTa = "";
                        string STing = "";
                        string KThuat = "";
                        string Dulich = "";


                        string TNMoiTV = "";
                        string TBaoTV = "";
                        string BCVeTV = "";
                        string NBoTV = "";
                        string DLiTV = "";
                        string KToanTV = "";
                        string KDoanhTV = "";
                        string PVeTV = "";
                        string BPDoanTV = "";
                        string HDonTV = "";
                        string CATV = "";
                        string YSaoTV = "";
                        string CSTV = "";
                        string DTaTV = "";
                        string STingTV = "";
                        string KThuatTV = "";
                        string DulichTV = "";

                        string sql_KTTV = "select * from PERMISION_SYS_NV where MANV = '" + tb.Rows[0]["Yahoo"].ToString() + "'";
                        DataTable dtktTV = db.ExecuteDataSet(sql_KTTV, CommandType.Text, "server37", null).Tables[0];
                        if (dtktTV.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {
                                if ("1" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    TNMoiTV = "true";
                                    break;
                                }
                                else
                                {
                                    TNMoiTV = "false";
                                }

                            }
                            result.TNMoiTV = TNMoiTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {
                                if ("2" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    TBaoTV = "true";
                                    break;
                                }
                                else
                                {
                                    TBaoTV = "false";
                                }
                            }
                            result.TBaoTV = TBaoTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {
                                if ("3" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    BCVeTV = "true";
                                    break;
                                }
                                else
                                {
                                    BCVeTV = "false";
                                }
                            }
                            result.BCVeTV = BCVeTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {
                                if ("4" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    NBoTV = "true";
                                    break;
                                }
                                else
                                {
                                    NBoTV = "false";
                                }
                            }
                            result.NBoTV = NBoTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {
                                if ("5" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    DLiTV = "true";
                                    break;
                                }
                                else
                                {
                                    DLiTV = "false";
                                }
                            }
                            result.DLiTV = DLiTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {
                                if ("6" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    KToanTV = "true";
                                    break;
                                }
                                else
                                {
                                    KToanTV = "false";
                                }
                            }
                            result.KToanTV = KToanTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {

                                if ("7" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    KDoanhTV = "true";
                                    break;
                                }
                                else
                                {
                                    KDoanhTV = "false";
                                }
                            }
                            result.KDoanhTV = KDoanhTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {

                                if ("8" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    PVeTV = "true";
                                    break;
                                }
                                else
                                {
                                    PVeTV = "false";
                                }
                            }
                            result.PVeTV = PVeTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {

                                if ("9" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    BPDoanTV = "true";
                                    break;
                                }
                                else
                                {
                                    BPDoanTV = "false";
                                }
                            }
                            result.BPDoanTV = BPDoanTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {
                                if ("10" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    HDonTV = "true";
                                    break;
                                }
                                else
                                {
                                    HDonTV = "false";
                                }
                            }
                            result.HDonTV = HDonTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {

                                if ("11" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    CATV = "true";
                                    break;
                                }
                                else
                                {
                                    CATV = "false";
                                }
                            }
                            result.CATV = CATV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {

                                if ("12" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    YSaoTV = "true";
                                    break;
                                }
                                else
                                {
                                    YSaoTV = "false";
                                }
                            }
                            result.YSaoTV = YSaoTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {

                                if ("13" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    CSTV = "true";
                                    break;
                                }
                                else
                                {
                                    CSTV = "false";
                                }

                            }
                            result.CSTV = CSTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {

                                if ("14" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    DTaTV = "true";
                                    break;
                                }
                                else
                                {
                                    DTaTV = "false";
                                }

                            }
                            result.DTaTV = DTaTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {

                                if ("15" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    STingTV = "true";
                                    break;
                                }
                                else
                                {
                                    STingTV = "false";
                                }
                            }
                            result.STingTV = STingTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {
                                if ("16" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    KThuatTV = "true";
                                    break;
                                }
                                else
                                {
                                    KThuatTV = "false";
                                }
                            }
                            result.KThuatTV = KThuatTV;
                            for (int i = 0; i < dtktTV.Rows.Count; i++)
                            {
                                if ("98" == dtktTV.Rows[i]["FeatureID"].ToString())
                                {
                                    DulichTV = "true";
                                    break;
                                }
                                else
                                {
                                    DulichTV = "false";
                                }
                            }
                            result.DulichTV = DulichTV;

                            result.TNMoi = "false";
                            result.TBao = "false";
                            result.BCVe = "false";
                            result.NBo = "false";
                            result.DLi = "false";
                            result.KToan = "false";
                            result.KDoanh = "false";
                            result.PVe = "false";
                            result.BPDoan = "false";
                            result.HDon = "false";
                            result.CA = "false";
                            result.YSao = "false";
                            result.CS = "false";
                            result.DTa = "false";
                            result.STing = "false";
                            result.KThuat = "false";
                            result.Dulich = "false";

                        }
                        else
                        {
                            string sql_KT = "select * from PERMISSION_SYS where PositionID = '" + tb.Rows[0]["IDGroupPermison"].ToString() + "'";
                            DataTable dtkt = db.ExecuteDataSet(sql_KT, CommandType.Text, "server37", null).Tables[0];
                            if (dtkt != null)
                            {
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {
                                    if ("1" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        TNMoi = "true";
                                        break;
                                    }
                                    else
                                    {
                                        TNMoi = "false";
                                    }
                                }
                                result.TNMoi = TNMoi;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {
                                    if ("2" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        TBao = "true";
                                        break;
                                    }
                                    else
                                    {
                                        TBao = "false";
                                    }
                                }
                                result.TBao = TBao;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {
                                    if ("3" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        BCVe = "true";
                                        break;
                                    }
                                    else
                                    {
                                        BCVe = "false";
                                    }
                                }
                                result.BCVe = BCVe;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {

                                    if ("4" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        NBo = "true";
                                        break;
                                    }
                                    else
                                    {
                                        NBo = "false";
                                    }
                                }
                                result.NBo = NBo;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {
                                    if ("5" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        DLi = "true";
                                        break;
                                    }
                                    else
                                    {
                                        DLi = "false";
                                    }
                                }
                                result.DLi = DLi;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {
                                    if ("6" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        KToan = "true";
                                        break;
                                    }
                                    else
                                    {
                                        KToan = "false";
                                    }
                                }
                                result.KToan = KToan;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {

                                    if ("7" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        KDoanh = "true";
                                        break;
                                    }
                                    else
                                    {
                                        KDoanh = "false";
                                    }
                                }
                                result.KDoanh = KDoanh;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {

                                    if ("8" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        PVe = "true";
                                        break;
                                    }
                                    else
                                    {
                                        PVe = "false";
                                    }
                                }
                                result.PVe = PVe;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {

                                    if ("9" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        BPDoan = "true";
                                        break;
                                    }
                                    else
                                    {
                                        BPDoan = "false";
                                    }
                                }
                                result.BPDoan = BPDoan;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {
                                    if ("10" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        HDon = "true";
                                        break;
                                    }
                                    else
                                    {
                                        HDon = "false";
                                    }
                                }
                                result.HDon = HDon;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {
                                    if ("11" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        CA = "true";
                                        break;
                                    }
                                    else
                                    {
                                        CA = "false";
                                    }
                                }
                                result.CA = CA;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {

                                    if ("12" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        YSao = "true";
                                        break;
                                    }
                                    else
                                    {
                                        YSao = "false";
                                    }
                                }
                                result.YSao = YSao;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {

                                    if ("13" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        CS = "true";
                                        break;
                                    }
                                    else
                                    {
                                        CS = "false";
                                    }

                                }
                                result.CS = CS;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {
                                    if ("14" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        DTa = "true";
                                        break;
                                    }
                                    else
                                    {
                                        DTa = "false";
                                    }

                                }
                                result.DTa = DTa;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {
                                    if ("15" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        STing = "true";
                                        break;
                                    }
                                    else
                                    {
                                        STing = "false";
                                    }
                                }
                                result.STing = STing;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {
                                    if ("16" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        KThuat = "true";
                                        break;
                                    }
                                    else
                                    {
                                        KThuat = "false";
                                    }
                                }
                                result.KThuat = KThuat;
                                for (int i = 0; i < dtkt.Rows.Count; i++)
                                {
                                    if ("98" == dtkt.Rows[i]["FeatureID"].ToString())
                                    {
                                        Dulich = "true";
                                        break;
                                    }
                                    else
                                    {
                                        Dulich = "false";
                                    }
                                }
                                result.Dulich = Dulich;

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
                    }

                }


                else
                {
                    result.ThongBao = "Mật khẩu không đúng!";

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







