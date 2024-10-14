using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections;
using System.Data;

using System.Net.Mail;
//using Telerik.Web.UI;


using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.IO;

/// <summary>
/// Summary description for Uilti
/// </summary>
public class Uilti
{
  
  
    DataTable dt = new DataTable();
    Hashtable HasInfor = new Hashtable();
    //Proty Proti = new Proty();


    /// <summary>
    /// kiem tra khoa thang
    /// </summary>
    /// <param name="ThangVe">thang xuat ve</param>
    /// <returns>true(duoc di tiep), false (ko dc phep)</returns>

   

    public string ChuDauTienVietHoa(string ChuoiHT)
    {
        string KyTuTim = "";
        try
        {

            ChuoiHT = ChuoiHT.Trim().ToLower();
            while (ChuoiHT.IndexOf("  ") != -1)
            {
                ChuoiHT = ChuoiHT.Replace("  ", " ");
            }
            KyTuTim = ChuoiHT.Substring(0, 1).ToUpper();
            ChuoiHT = ChuoiHT.Remove(0, 1);
            ChuoiHT = ChuoiHT.Insert(0, KyTuTim);
            for (int i = 1; i < ChuoiHT.Length; i++)
            {
                if (ChuoiHT[i - 1].ToString() == " ")
                {
                    KyTuTim = ChuoiHT.Substring(i, 1).ToUpper();
                    ChuoiHT = ChuoiHT.Remove(i, 1);
                    ChuoiHT = ChuoiHT.Insert(i, KyTuTim);
                }
            }
        }
        catch { }
        return ChuoiHT;
    }
    public void LogErrorData(string MaKH, string methold, string page, string error)
    {
        string logPath = AppDomain.CurrentDomain.BaseDirectory + "HDDT_Error.log";

        using (StreamWriter upError = new StreamWriter(logPath, true))
        {
            upError.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + MaKH + " - " + methold + " - " + page + " - " + error + Environment.NewLine + "--------------------------" + Environment.NewLine);
            upError.Close();
        }


    }




    //public string ReplaceAfter(string StrSrc)
    //{
    //    string StrAfter = "";
    //    switch (StrSrc)
    //    {
    //        case "Ban Mê Thuột(BMV)":
    //            StrAfter += "Ban Mê Thuột";
    //            break;
    //        case "Cà Mau (CAH)":
    //            StrAfter += "Cà Mau";
    //            break;
    //        case "Đà Nẵng (DAD)":
    //            StrAfter += "Đà Nẵng";
    //            break;
    //        case "Điện Biên (DIN)":
    //            StrAfter += "Điện Biên";
    //            break;
    //        case "Đà Lạt (DLI)":
    //            StrAfter += "Đà Lạt";
    //            break;
    //        case "Hà Nội (HAN)":
    //            StrAfter += "Hà Nội";
    //            break;
    //        case "Hải Phòng (HPH)":
    //            StrAfter += "Hải Phòng";
    //            break;
    //        case "Huế (HUI)":
    //            StrAfter += "Huế";
    //            break;
    //        case "Nha Trang (NHA)":
    //            StrAfter += "Nha Trang";
    //            break;
    //        case "Phú Quốc (PQC)":
    //            StrAfter += "Phú Quốc";
    //            break;
    //        case "Pleiku(PXU)":
    //            StrAfter += "Pleiku";
    //            break;
    //        case "Hồ Chí Minh (SGN)":
    //            StrAfter += "TP.Hồ Chí Minh";
    //            break;
    //        case "hcm(SGN)":
    //            StrAfter += "TP.Hồ Chí Minh";
    //            break;
    //        case "Thanh Hóa (TBD)":
    //            StrAfter += "Thanh Hóa";
    //            break;
    //        case "Quy Nhơn (UIH)":
    //            StrAfter += "Quy Nhơn ";
    //            break;
    //        case "Cần Thơ (VCA)":
    //            StrAfter += "Cần Thơ";
    //            break;
    //        case "Chu Lai (VCL)":
    //            StrAfter += "Chu Lai";
    //            break;
    //        case "Côn Đảo (VCS)":
    //            StrAfter += "Côn Đảo";
    //            break;
    //        case "Đồng Hới (VDH)":
    //            StrAfter += "Đồng Hới";
    //            break;
    //        case "nghệ an (VII)":
    //            StrAfter += "Nghệ An";
    //            break;
    //        case "Rạch Giá(VKG)":
    //            StrAfter += "Rạch Giá";
    //            break;
    //        //}
    //    }
    //    return StrAfter;
    //}

    //public string AfterReplace(string StrSrc)
    //{
    //    string StrAfter = "";
    //    switch (StrSrc)
    //    {
    //        case "Ban Mê Thuột":
    //            StrAfter += "Ban Mê Thuột (BMV)";
    //            break;
    //        case "Cà Mau":
    //            StrAfter += "Cà Mau (CAH)";
    //            break;
    //        case "Đà Nẵng":
    //            StrAfter += "Đà Nẵng (DAD)";
    //            break;
    //        case "Điện Biên":
    //            StrAfter += "Điện Biên (DIN)";
    //            break;
    //        case "Đà Lạt":
    //            StrAfter += "Đà Lạt (DLI)";
    //            break;
    //        case "Hà Nội":
    //            StrAfter += "Hà Nội (HAN)";
    //            break;
    //        case "Hải Phòng":
    //            StrAfter += "Hải Phòng (HPH)";
    //            break;
    //        case "Huế":
    //            StrAfter += "Huế (HUI)";
    //            break;
    //        case "Nha Trang":
    //            StrAfter += "Nha Trang (NHA)";
    //            break;
    //        case "Phú Quốc":
    //            StrAfter += "Phú Quốc (PQC)";
    //            break;
    //        case "Pleiku":
    //            StrAfter += "Pleiku(PXU)";
    //            break;
    //        case "TP.Hồ Chí Minh"://""
    //            StrAfter += "Hồ Chí Minh (SGN)";
    //            break;
    //        case "hcm(SGN)":
    //            StrAfter += "TP.Hồ Chí Minh";
    //            break;
    //        case "Thanh Hóa":
    //            StrAfter += "Thanh Hóa (TBD)";
    //            break;
    //        case "Quy Nhơn":
    //            StrAfter += "Quy Nhơn (UIH)";
    //            break;
    //        case "Cần Thơ":
    //            StrAfter += "Cần Thơ (VCA)";
    //            break;
    //        case "Chu Lai":
    //            StrAfter += "Chu Lai (VCL)";
    //            break;
    //        case "Côn Đảo":
    //            StrAfter += "Côn Đảo (VCS)";
    //            break;
    //        case "Đồng Hới":
    //            StrAfter += "Đồng Hới (VDH)";
    //            break;
    //        case "nghệ An":
    //            StrAfter += "Nghệ an (VII)";
    //            break;
    //        case "Rạch Giá":
    //            StrAfter += "Rạch Giá(VKG)";
    //            break;
    //        //}
    //    }
    //    return StrAfter;
    //}






















    /// <summary>
    /// ham kiem tra quyen truy cap danh muc ve hoan 
    /// </summary>
    /// <returns> sign in can kiem tra </returns>
    //public string StrRefurnd(string StrUserID)
    //{
    //    string SRefurnd = "";
    //    //string StrUserName = Session["UserName"].ToString();
    //    string sql = "SELECT * FROM RefundGroup WHERE UserID='" + StrUserID + "'";
    //    DataTable dt2 = new DataTable();
    //    dt2 = Cls2.FillTBl(sql, "RefundGroup");
    //    if (dt2.Rows.Count > 0)
    //    {
    //        if (dt2.Rows[0]["GrFull"].ToString() == "1")
    //        {
    //            SRefurnd = "1";
    //        }
    //        else
    //        {
    //            SRefurnd = dt2.Rows[0]["RefundID"].ToString();
    //        }
    //    }
    //    return SRefurnd;
    //}

    //public void GetPremission(string f)
    //{
    //    DataTable dt = new DataTable();
    //    string SQlF = " SELECT * FROM [Premission] WHERE PremiID='" + f + "' ";
    //    dt = Cls.FillTBl(SQlF, "Premission");
    //    if (dt.Rows.Count > 0)
    //    {
    //        Proti.PreRead = int.Parse(dt.Rows[0]["PreRead"].ToString());
    //        Proti.PreWrite = int.Parse(dt.Rows[0]["PreWrite"].ToString());
    //        Proti.PreDelete = int.Parse(dt.Rows[0]["PreDelete"].ToString());
    //        Proti.PreView = int.Parse(dt.Rows[0]["PreView"].ToString());
    //        Proti.PreExcel = int.Parse(dt.Rows[0]["PreExcel"].ToString());
    //    }
    //}
    // status : 1 = read , 2 = write, 3 = delete, 4 = view, 5 = excel 
    //public void GetAccess(string kt, string StrUsername, WebControl StrControl, string sts)
    //{
    //    string SqlCon = " SELECT * FROM [Premission] WHERE UserID='" + StrUsername + "' AND CategoryID='" + kt + "' ";
    //    dt = Cls2.FillTBl(SqlCon, "Premission");
    //    if (dt.Rows.Count > 0)
    //    {
    //        string StrRead = dt.Rows[0]["PreRead"].ToString();
    //        string StrWrite = dt.Rows[0]["PreWrite"].ToString();
    //        string StrWDelete = dt.Rows[0]["PreDelete"].ToString();
    //        string StrView = dt.Rows[0]["PreView"].ToString();
    //        string StrExcel = dt.Rows[0]["PreExcel"].ToString();
    //        if (StrRead == "0" && sts == "1")
    //        {
    //            StrControl.Enabled = false;
    //        }
    //        if (StrWrite == "0" && sts == "2")
    //        {
    //            StrControl.Enabled = false;
    //        }
    //        if (StrWDelete == "0" && sts == "3")
    //        {
    //            StrControl.Enabled = false;
    //        }
    //        if (StrView == "0" && sts == "4")
    //        {
    //            StrControl.Enabled = false;
    //        }
    //        if (StrExcel == "0" && sts == "5")
    //        {
    //            StrControl.Enabled = false;
    //        }
    //    }
    //}

    //public string SubDicript(string Source,int leth)
    //{
    //    string ReString = "";
    //    string[] Sc= Source.Split(' ');
    //    //int Sc = Source.IndexOf(" ");
    //    //ReString = Source.Substring(0, Sc);
    //    for (int i = 0; i < Source.Length; i++)
    //    {
    //        if (i < leth)
    //        {
    //            ReString += Sc[i].ToString()+" ";
    //        }
    //    }
    //    ReString = ReString + "...";
    //        return ReString;
    //}

    public string FormatContentNews(string value, int count)
    {
        string _value = value;
        if (_value.Length >= count)
        {
            string ValueCut = _value.Substring(0, count - 3);
            string[] valuearray = ValueCut.Split(' ');
            string valuereturn = "";
            for (int i = 0; i < valuearray.Length - 1; i++)
            {
                valuereturn = valuereturn + " " + valuearray[i];
            }
            return valuereturn + "...";
        }
        else
        {
            return _value;
        }
    }

    /// <summary>
    /// function send mail
    /// </summary>
    /// <param name="MailTo">mail address to</param>
    /// <param name="from">mail address from</param>
    /// <param name="subject">mail title</param>
    /// <param name="body">body</param>
    /// <returns>Successful</returns>
    public string SendMail_CT(string MailTo, string from, string subject, string body)
    {
        MailMessage message = new MailMessage();
        SmtpClient smtpClient = new SmtpClient();
        string msg = string.Empty;
        try
        {
            MailAddress fromAddress = new MailAddress(from, "hoanveev@enviet-group.com", System.Text.Encoding.UTF8);
            message.From = fromAddress;
            message.To.Add(new MailAddress(MailTo));
            //message.CC.Add("");
            //message.Bcc.Add(new MailAddress("hoanve@enviet-group.com"));

            message.CC.Add(new MailAddress("hoanveev@enviet-group.com"));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            smtpClient.Host = "mail.enviet-group.com";   // We use gmail as our smtp client
            smtpClient.Port = 25;
            smtpClient.EnableSsl = false;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new System.Net.NetworkCredential("hoanveev@enviet-group.com", "Evevgroup@458903@");
            smtpClient.Send(message);
            msg = "Successful";

        }
        catch (Exception ex)
        {
            msg = ex.Message;
        }
        return msg;
    }

    public string SendMail_Reset(string MailTo, string from, string subject, string body)
    {
        MailMessage message = new MailMessage();
        SmtpClient smtpClient = new SmtpClient();
        string msg = string.Empty;
        try
        {
            MailAddress fromAddress = new MailAddress(from, "No-Reply<reset@enviet-group.com>", System.Text.Encoding.UTF8);
            message.From = fromAddress;
            message.To.Add(new MailAddress(MailTo));
            //message.CC.Add("");
            //message.Bcc.Add(new MailAddress("hoanve@enviet-group.com"));

            message.CC.Add(new MailAddress("reset@enviet-group.com"));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            smtpClient.Host = "mail.enviet-group.com";   // We use gmail as our smtp client
            smtpClient.Port = 25;
            smtpClient.EnableSsl = false;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new System.Net.NetworkCredential("reset@enviet-group.com", "Evevgroup@458903@");
            smtpClient.Send(message);
            msg = "Successful";

        }
        catch (Exception ex)
        {
            msg = ex.Message;
        }
        return msg;
    }

    public string SendMail_Alert(string MailTo, string Mailcc, string MailBc, string from, string subject, string body)
    {
        MailMessage message = new MailMessage();
        SmtpClient smtpClient = new SmtpClient();
        string msg = string.Empty;
        try
        {
            MailAddress fromAddress = new MailAddress(from, "it04@enviet-group.com", System.Text.Encoding.UTF8);
            message.From = fromAddress;
            message.To.Add(new MailAddress(MailTo));
            if (Mailcc!="")
            {
                message.CC.Add(Mailcc);
            }
            message.Bcc.Add(new MailAddress(MailBc));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            smtpClient.Host = "mail.enviet-group.com";   // We use gmail as our smtp client
            smtpClient.Port = 25;
            smtpClient.EnableSsl = false;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new System.Net.NetworkCredential("it04@enviet-group.com", "enviet@123");

            smtpClient.Send(message);
            msg = "Successful";

        }
        catch (Exception ex)
        {
            msg = ex.Message;
        }
        return msg;
    }

    //public string CheckAdmin(string StrUser)
    //{
    //    string StrPre = "0";
    //    if (StrUser != "")
    //    {
    //        //string RefundID = Request.QueryString["id"].ToString();
    //        //string StrUser = Session["UserName"].ToString();
    //        string SqlCheck = " SELECT GrFull FROM RefundGroup WHERE UserID ='" + StrUser + "' ";
    //        DataTable dt2 = new DataTable();
    //        dt2 = Cls2.FillTBl(SqlCheck, "RefundGroup");
    //        if (dt2.Rows[0]["GrFull"].ToString() != "0")
    //        {
    //            StrPre = "1";
    //        }
    //    }
    //    return StrPre;
    //}

    /// <summary>
    /// MD5 Encrypt
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
  

    public string FillUnicode(string text)
    {
        string[] pattern = new string[8];
        pattern[0] = "a|(á|ả|à|ạ|ã|ă|ắ|ẳ|ằ|ặ|ẵ|â|ấ|ẩ|ầ|ậ|ẫ)";
        pattern[1] = "o|(ó|ỏ|ò|ọ|õ|ô|ố|ổ|ồ|ộ|ỗ|ơ|ớ|ở|ờ|ợ|ỡ)";
        pattern[2] = "e|(é|è|ẻ|ẹ|ẽ|ê|ế|ề|ể|ệ|ễ|ẹ)";
        pattern[3] = "u|(ú|ù|ủ|ụ|ũ|ư|ứ|ừ|ử|ự|ữ)";
        pattern[4] = "i|(í|ì|ỉ|ị|ĩ)";
        pattern[5] = "y|(ý|ỳ|ỷ|ỵ|ỹ)";
        pattern[6] = "d|đ";
        pattern[7] = "-| ";

        for (int i = 0; i < pattern.Length; i++)
        {
            char replaceChar = pattern[i][0];
            MatchCollection matchs = Regex.Matches(text, pattern[i]);
            foreach (Match m in matchs)
            {
                text = text.Replace(m.Value[0], replaceChar);
            }
        }
        return text;
    }

    private readonly string[] VietnameseSigns = new string[]
    {
        "aAeEoOuUiIdDyY",
        "áàạảãâấầậẩẫăắằặẳẵ",
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
        "éèẹẻẽêếềệểễ",
        "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ",
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
        "úùụủũưứừựửữ",
        "ÚÙỤỦŨƯỨỪỰỬỮ",
        "íìịỉĩ",
        "ÍÌỊỈĨ",
        "đ",
        "Đ",
        "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
    };

    public string RemoveSign4VietnameseString(string str)
    {
        for (int i = 1; i < VietnameseSigns.Length; i++)
        {
            for (int j = 0; j < VietnameseSigns[i].Length; j++)
            {
                str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]).Replace(" ", "-");
            }
        }
        return str;

    }
    /// <summary>
    /// check ky cua so ve 
    /// </summary>
    /// <param name="StrNgayve">MM/dd/yyyy</param>
    /// <returns>dd/MM/yyyy</returns>
   

    /// <summary>
    /// check ky cua so ve 
    /// </summary>
    /// <param name="StrNgayve">MM/dd/yyyy</param>
    /// <returns>dd/MM/yyyy</returns>
  

    /// <summary>
    /// check ky cua so ve 
    /// </summary>
    /// <param name="StrNgayve">MM/dd/yyyy</param>
    /// <returns>dd/MM/yyyy</returns>
    

    /// <summary>
    /// tao ngay lap HD 
    /// </summary>
    /// <param name="Strky">dd/MM/yyyy</param>
    /// <returns>MM/dd/yyyy</returns>
    public string BuilNgayLapHD(string Strky)
    {
        string Strreturn = "";
        string NewDay = "";
        int NoDay = DateTime.DaysInMonth(Convert.ToInt32(Strky.Substring(6, 4).ToString()), Convert.ToInt32(Strky.Substring(3, 2).ToString()));
        string asdas=Strky.Substring(0,2).ToString();
        switch (asdas)
        { 
            case"01":
                NewDay = "07";
                break;
            case "02":
                NewDay = "15";
                break;
            case "03":
                NewDay = "23";
                break;
            case "04":
                NewDay = NoDay.ToString();
                break;
        }
        Strreturn = Strky.Substring(3, 2).ToString() + "/" + NewDay + "/" + Strky.Substring(6, 4).ToString();
        return Strreturn;
    }

    /// <summary>
    /// ham xu ly ngay lap hoa don theo chuan 1/5
    /// 1. ve xuat truoc ngay 10 thang truoc, ngay lap se dua vao cuoi thang xuat ve.
    /// 2. ve trong thang co ngay lap la ngay gui 
    /// </summary>
    /// <param name="Ngayxuat">ngay xuat ve (dd/MM/yyyy) </param>
    /// <returns>MM/dd/yyyy</returns>
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="NgayXuat">ngay xuat ve MM/dd/yyyy</param>
    /// <returns>0=>false,1=true</returns>
    
    /// <summary>
    /// ham dinh dang tien te
    /// </summary>
    /// <param name="giatri">kieu string truyen vao</param>
    /// <returns>kieu string</returns>
    public string Mk_MoneyFormat(string giatri)
    {
        if (giatri == "" || giatri==null)
        {
            giatri = "0";
        }
        string Result = "0";
        if (giatri != "0")
        {
            Result = Convert.ToDouble(giatri).ToString("#,###");
        }
        else {
            Result = "0";
        }
        return Result;
    }

    public string GeneratePassword()
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
    ///mail template
    ///strbody noi dung chinh 
    public string Templmail(string StrBody)
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
	            height: 20px;
	            font-size: 12px;
	            margin: 0 0 2px 0;
	            padding: 0;
            }
            .header {
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
                  " + StrBody + @"
                  <!------ Body ----> 
                </div>
              </div>
            </div>
</body>
</html>
            ";

        return StrTempMail;
    }

  

    

}