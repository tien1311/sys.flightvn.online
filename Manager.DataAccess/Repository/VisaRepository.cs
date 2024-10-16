using Dapper;
using Manager.Model.Models;
//using FluentFTP;
//using Manager_EV.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
//using RtfPipe.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using TangDuLieu;


namespace Manager.DataAccess.Repository
{
    public class VisaRepository
    {
        string SQL_VISA; /*= "Data Source=27.71.232.40,1453;Initial Catalog=VISA;User ID=sa;Password=EnViet@123;";*/
        string SQL_EV_MAIN; /* = "Data Source=27.71.232.40,1453;Initial Catalog=Manager;User ID=sa;Password=EnViet@123;";*/
        Mail mailDb = new Mail("EVM_CHANGESTATUSVISA");
        Mail mailDbSuccess = new Mail("EVM_SUCCESSBOOKING");
        Mail mailDbXacNhanDonHang = new Mail("EVM_XACNHANDONHANG_VISA");
        public VisaRepository(IConfiguration configuration)
        {
            SQL_VISA = configuration.GetConnectionString("SQL_EV_VISA");
            SQL_EV_MAIN = configuration.GetConnectionString("SQL_EV_MAIN");
        }
        public List<VisaModel> Visa()
        {
            List<VisaModel> result = new List<VisaModel>();
            string sql = @"select ID,Code,Name,IsActive from Product order by ID desc";
            using (var conn = new SqlConnection(SQL_VISA))
            {
                result = (List<VisaModel>)conn.Query<VisaModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            return result;
        }
        public VisaModel EditVisa(int ID)
        {
            VisaModel result = new VisaModel();
            List<Manager.Model.Models.Image> ListImg = new List<Manager.Model.Models.Image>();
            string sql = @"select * from Product where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_VISA))
            {
                result = conn.QueryFirst<VisaModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            if (result != null)
            {
                string sqlImg = @"select * from Image where ProductID = '" + result.ID + "'";
                using (var conn = new SqlConnection(SQL_VISA))
                {
                    ListImg = (List<Manager.Model.Models.Image>)conn.Query<Manager.Model.Models.Image>(sqlImg, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
                result.ListImages = ListImg;
            }
            return result;
        }
        public Task<bool> SaveCreateVisa(VisaModel data, string MaNV)
        {
            int x = 0, ID = 0;
            string sqlImg = "";
            string Code = CodeProduct();
            string sql = @"INSERT INTO [Product] ([Code],[Name],[VisaType],[ShortDescription],[CreatedBy],[CreatedDate])
                                        VALUES ('" + Code + "',N'" + data.Name + "',N'" + data.VisaType + "',N'" + data.ShortDescription.Trim() + "','" + MaNV + "',GETDATE()) SELECT SCOPE_IDENTITY() AS ID";
            using (var conn = new SqlConnection(SQL_VISA))
            {
                ID = conn.QueryFirst<int>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (ID > 0)
            {
                for (int i = 0; i < data.ListImages.Count; i++)
                {
                    sqlImg += @"INSERT INTO [Image] ([ProductID],[ImageURL],[MainImage]) VALUES ('" + ID + "','" + data.ListImages[i].ImageURL + "','" + data.ListImages[i].MainImage + "')";
                }
                using (var conn = new SqlConnection(SQL_VISA))
                {
                    x = conn.Execute(sqlImg, null, null, commandTimeout: 30, commandType: CommandType.Text);
                    conn.Dispose();
                }
                if (x > 0)
                {
                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromResult(false);
                }
            }
            else
                return Task.FromResult(false);
        }
        public Task<bool> SaveEditVisa(VisaModel data)
        {
            int x = 0, z = 0, y = 0;
            string sqlDelImg = @"DELETE FROM Image WHERE ProductID = '" + data.ID + "';";
            using (var conn = new SqlConnection(SQL_VISA))
            {
                y = conn.Execute(sqlDelImg, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }

            string sqlImg = "";
            string sql = @"UPDATE [Product] SET [Name] = N'" + data.Name + "', [VisaType] = N'" + data.VisaType + "',[ShortDescription] = N'" + data.ShortDescription.Trim() + "' where ID = '" + data.ID + "'";
            using (var conn = new SqlConnection(SQL_VISA))
            {
                z = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (z > 0)
            {
                for (int i = 0; i < data.ListImages.Count; i++)
                {
                    sqlImg += @"INSERT INTO [Image] ([ProductID],[ImageURL],[MainImage]) VALUES ('" + data.ID + "','" + data.ListImages[i].ImageURL + "','" + data.ListImages[i].MainImage + "')";
                }
                using (var conn = new SqlConnection(SQL_VISA))
                {
                    x = conn.Execute(sqlImg, null, null, commandTimeout: 30, commandType: CommandType.Text);
                    conn.Dispose();
                }
                if (x > 0)
                {
                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromResult(false);
                }
            }
            else
                return Task.FromResult(false);
        }
        public bool ChangeActiveVisa(int ID, int Active)
        {
            int x = 0;
            string sql = @"UPDATE [Product] SET [IsActive] = '" + Active + "' where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_VISA))
            {
                x = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
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
        public List<TypeVisa> TypeVisa()
        {
            List<TypeVisa> result = new List<TypeVisa>();
            string ProductName = "";
            string sql = @"select ID,Name,Price,DiscountPrice,ProductID from Type order by ID desc";
            using (var conn = new SqlConnection(SQL_VISA))
            {
                result = (List<TypeVisa>)conn.Query<TypeVisa>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            if (result.Count > 0)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    string sqlProduct = @"select Name from Product where ID = '" + result[i].ProductID + "'";
                    using (var conn = new SqlConnection(SQL_VISA))
                    {
                        ProductName = conn.QueryFirst<string>(sqlProduct, null, commandType: CommandType.Text, commandTimeout: 30);
                        conn.Close();
                    }
                    result[i].ProductName = ProductName;
                }
            }
            return result;
        }
        public bool SaveCreateType(TypeVisa data)
        {
            int x = 0;
            string sqlType = @"INSERT INTO [Type] ([ProductID],[Name],[Price],[DiscountPrice],[Description],[Documents])
                                                VALUES ('" + data.ProductID + "',N'" + data.Name + "','" + data.Price + "','" + data.DiscountPrice + "',N'" + data.Description + "',N'" + data.Documents + "')";

            using (var conn = new SqlConnection(SQL_VISA))
            {
                x = conn.Execute(sqlType, null, null, commandTimeout: 30, commandType: CommandType.Text);
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
        public TypeVisa EditTypeVisa(int ID)
        {
            TypeVisa result = new TypeVisa();
            string sql = @"select * from Type where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_VISA))
            {
                result = conn.QueryFirst<TypeVisa>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            return result;
        }
        public bool SaveEditType(TypeVisa data)
        {
            int x = 0;
            string sqlType = @"UPDATE [Type] SET [ProductID] = '" + data.ProductID + "',[Name] = N'" + data.Name + "',[Price] = '" + data.Price + "',[DiscountPrice] = '" + data.DiscountPrice + "',[Description] = N'" + data.Description + "', [Documents] = N'" + data.Documents + "' where ID = '" + data.ID + "'";
            using (var conn = new SqlConnection(SQL_VISA))
            {
                x = conn.Execute(sqlType, null, null, commandTimeout: 30, commandType: CommandType.Text);
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
        public bool DeleteImg(int ID)
        {
            string imageUrl = null;
            using (var conn = new SqlConnection(SQL_VISA))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Lấy URL của hình ảnh từ cơ sở dữ liệu
                        string selectImage = "SELECT ImageURL FROM Image WHERE ID = @ID";
                        imageUrl = conn.QuerySingleOrDefault<string>(selectImage, new { ID }, transaction);

                        // Xóa bản ghi hình ảnh khỏi cơ sở dữ liệu
                        string deleteImageQuery = "DELETE FROM Image WHERE ID = @ID";
                        int result = conn.Execute(deleteImageQuery, new { ID }, transaction);

                        // Xóa hình ảnh khỏi FTP nếu có URL
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            bool imageDeleted = Manager.Common.Helpers.Common.DeleteImg(imageUrl);

                            if (!imageDeleted)
                            {
                                // Nếu xóa hình ảnh không thành công, rollback giao dịch
                                transaction.Rollback();
                                return false;
                            }
                        }

                        if (result > 0)
                        {
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            // Nếu không xóa được bản ghi, rollback giao dịch
                            transaction.Rollback();
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction if an error occurs
                        transaction.Rollback();
                        // Log the exception (not implemented here)
                        return false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
        public string CodeProduct()
        {
            string date = DateTime.Now.ToString("ddMMyy");
            string code = "SPVS" + DateTime.Now.ToString("ddMMyy");
            string MSP = "";
            try
            {
                string sql = @"select top 1 code from Product where code like '%" + date + "%' order by ID desc";
                using (var conn = new SqlConnection(SQL_VISA))
                {
                    MSP = conn.QueryFirst<string>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
                if (MSP != null)
                {
                    int stt = int.Parse(MSP.Substring(10, 3));
                    if (stt < 9)
                    {
                        stt++;
                        code += "00" + stt;
                    }
                    else
                    {
                        if (stt < 99)
                        {
                            stt++;
                            code += "0" + stt;
                        }
                        else
                        {
                            stt++;
                            code += stt;
                        }
                    }
                }
                else
                {
                    code += "001";
                }
            }
            catch (Exception)
            {
                code += "001";
            }

            return code;
        }
        public List<VisaBookingModel> VisaBooking()
        {
            List<VisaBookingModel> list = new List<VisaBookingModel>();
            try
            {
                string sql = @"select BK.*, Type.Name as VisaType,Product.Name as VisaName, BK.BookingCode as Code,Status.name as Status from Booking BK
                            left join Status on Status.ID = BK.StatusID
	                        left join Type on Type.ID = BK.TypeID
	                        left join Product on Product.ID = Type.ProductID order by ID desc";
                using (var conn = new SqlConnection(SQL_VISA))
                {
                    list = (List<VisaBookingModel>)conn.Query<VisaBookingModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
            }
            catch (Exception)
            {
                return list;
            }
            return list;
        }
        public VisaBookingModel DetailVisaBooking(int ID, string MaPB, string TenNV)
        {
            VisaBookingModel result = new VisaBookingModel();
            List<VisaStatus> ListVisaStatus = new List<VisaStatus>();
            int x = 0;
            try
            {
                string sql = @"select BK.*, Type.Name as VisaType,Product.Name as VisaName, BK.BookingCode as Code,Status.Name as Status  from Booking BK
                            left join Status on Status.ID = BK.StatusID
	                        left join Type on Type.ID = BK.TypeID
	                        left join Product on Product.ID = Type.ProductID where BK.ID = '" + ID + "'";
                using (var conn = new SqlConnection(SQL_VISA))
                {
                    result = conn.QueryFirst<VisaBookingModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
                if (result != null)
                {
                    if (result.Reciever == null)
                    {
                        if (MaPB.Trim() == "DL")
                        {
                            string sqlupdate = @"update Booking set Reciever = N'" + TenNV + "', StatusID = '2' where ID = '" + ID + "'";
                            using (var conn = new SqlConnection(SQL_VISA))
                            {
                                x = conn.Execute(sqlupdate, null, null, commandTimeout: 30, commandType: CommandType.Text);
                                conn.Dispose();
                            }
                            if (x > 0)
                            {
                                result.Reciever = TenNV;
                            }
                        }
                    }
                    string sqlStatus = @"select ID,Name from Status";
                    using (var conn = new SqlConnection(SQL_VISA))
                    {
                        ListVisaStatus = (List<VisaStatus>)conn.Query<VisaStatus>(sqlStatus, null, commandType: CommandType.Text, commandTimeout: 30);
                        conn.Close();
                    }
                    result.ListVisaStatus = ListVisaStatus;
                }

            }
            catch (Exception)
            {
                return result;
            }
            return result;
        }
        public async Task<bool> ChangeStatus(int ID, int StatusID, string Note)
        {
            int x = 0;
            string status = "", sendmail = "";
            VisaBookingModel result = new VisaBookingModel();
            string sql = @"select BK.*, Product.Code as VisaCode, Type.Name as VisaType,Type.Documents as DocumentsType,Product.Name as VisaName,  BK.BookingCode as Code from Booking BK
	                        left join Type on Type.ID = BK.TypeID
	                        left join Product on Product.ID = Type.ProductID where BK.ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_VISA))
            {
                result = conn.QueryFirst<VisaBookingModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }
            if (result != null)
            {
                string sqlStatus = @"select UPPER(Name) from Status where ID = '" + StatusID + "'";
                using (var conn = new SqlConnection(SQL_VISA))
                {
                    status = conn.QueryFirst<string>(sqlStatus, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
                result.Status = status;
                result.Note = Note;
            }
            result.StatusID = StatusID;
            if (StatusID == 8)
            {
                sendmail = SendMailSuccessVisa(result);
            }
            else if (StatusID == 6)
            {

                sendmail = await SendMailXacNhanThanhToan_Visa(result);
            }
            else
            {
                sendmail = await SendMailVisa(result);
            }
            if (sendmail == "Successful")
            {
                string sqlType = @"Update Booking set StatusID = '" + StatusID + "', Note = N'" + Note + "' WHERE ID = '" + ID + "';";
                using (var conn = new SqlConnection(SQL_VISA))
                {
                    x = conn.Execute(sqlType, null, null, commandTimeout: 30, commandType: CommandType.Text);
                    conn.Dispose();
                }
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

        public async Task<string> SendMailXacNhanThanhToan_Visa(VisaBookingModel result)
        {
            bool isSuccess = false;
            string program = "EVM_XACNHANDONHANG_VISA";
            string resultSendMail = "";
            string linkGateway = string.Empty;
            EVEmail ev_Email = new EVEmail();
            EVMailRepository evMail_Rep = new EVMailRepository();
            ev_Email = evMail_Rep.GetEVEMailContentByProgram(program);

            if (ev_Email != null)
            {
                string mailBody = "";

                GatewayRepository gateway_Rep = new GatewayRepository();

                var productInfo = new[]
                {
                        new { ProductName = result.VisaName, Quantity = 1, UnitPrice = result.Price }
                    };

                linkGateway = await gateway_Rep.GetLinkToGateway_V2(result.Code, "", result.FullName, result.Phone, result.Email, result.Address, productInfo, result.Price, 0, 0, 0, result.Price, "", "https://gateway.envietgroup.com/Home/ChiTietDonHang?orderId=" + result.Code);


                var webRequest = WebRequest.Create(ev_Email.templateUrl);
                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                { mailBody = reader.ReadToEnd(); }
                // Mail xác nhạn
                mailBody = mailBody.Replace("$_evcode", result.Code);
                mailBody = mailBody.Replace("$_Fullname", result.FullName);
                mailBody = mailBody.Replace("$_Phone", result.Phone);
                mailBody = mailBody.Replace("$_Email", result.Email);
                mailBody = mailBody.Replace("$_Price", Manager.Common.Helpers.Common.FormatNumber(result.Price, 0) + " VND");
                mailBody = mailBody.Replace("$_Total", Manager.Common.Helpers.Common.FormatNumber(result.Price, 0) + " VND");
                mailBody = mailBody.Replace("$_VISA_Code_$", result.VisaCode);
                mailBody = mailBody.Replace("$_VISA_Name_$", result.VisaName);
                mailBody = mailBody.Replace("$_LinkGateway", linkGateway);
                bool isCC = true;
                bool isBCC = true;
                if (result.Address.Trim() == "ENVIETTESTING")
                {
                    isCC = false;
                    isBCC = false;
                }
                string subject = "[VISA] XÁC NHẬN " + result.Status;
                isSuccess = Manager.Common.Helpers.Common.SendMail("ENVIET GROUP", subject, mailBody, result.Email, ev_Email.username, ev_Email.password, ev_Email.hostName, ev_Email.port, ev_Email.useSSL, ev_Email.CC, ev_Email.BCC, isCC, isBCC);

                if (isSuccess)
                {
                    resultSendMail = "Successful";
                }
                else
                {
                    resultSendMail = "Failed";
                }

            }
            return resultSendMail;
        }

        public async Task<string> SendMailVisa(VisaBookingModel result)
        {
            MailMessage message = new MailMessage(mailDb.username, result.Email);
            SmtpClient smtpClient = new SmtpClient();
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress(mailDb.username, "ENVIET GROUP");
                message.From = fromAddress;
                message.Subject = "[VISA] XÁC NHẬN " + result.Status;

                if (result.Address != "ENVIETTESTING")
                {
                    message.CC.Add(mailDb.CC);
                }
                ///-------- Start of mail body ------------
                string mailBody;
                string linkGateway = string.Empty;
                string templateUrl = "";

                if (result.StatusID == 6)
                {
                    templateUrl = mailDbXacNhanDonHang.templateUrl;
                    GatewayRepository gateway_Rep = new GatewayRepository();

                    var productInfo = new[]
                    {
                        new { ProductName = result.VisaName, Quantity = 1, UnitPrice = result.Price }
                    };

                    linkGateway = await gateway_Rep.GetLinkToGateway_V2(result.Code, "", result.FullName, result.Phone, result.Email, result.Address, productInfo, result.Price, 0, 0, 0, result.Price, "", "https://gateway.envietgroup.com/Home/ChiTietDonHang?orderId=" + result.Code);
                }
                else
                {
                    templateUrl = mailDb.templateUrl;
                }
                var webRequest = WebRequest.Create(templateUrl);
                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                { mailBody = reader.ReadToEnd(); }
                mailBody = mailBody.Replace("$_Code", result.Code);
                mailBody = mailBody.Replace("$_ProductName", result.VisaName);
                mailBody = mailBody.Replace("$_ProductType", result.VisaType);
                mailBody = mailBody.Replace("$_ProductPrice", string.Format("{0:#,0}", result.Price));
                mailBody = mailBody.Replace("$_ProductStatus", result.Status);
                if (result.Status.Trim() == "ĐÃ TIẾP NHẬN THÔNG TIN")
                {
                    mailBody = mailBody.Replace("$_Note", result.DocumentsType + result.Note);
                }
                else
                {
                    mailBody = mailBody.Replace("$_Note", result.Note);
                }
                mailBody = mailBody.Replace("$_Fullname", result.FullName);
                mailBody = mailBody.Replace("$_Phone", result.Phone);
                mailBody = mailBody.Replace("$_Email", result.Email);
                mailBody = mailBody.Replace("$_Address", result.Address);
                mailBody = mailBody.Replace("$_Ngaygui", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));

                // Mail xác nhạn
                mailBody = mailBody.Replace("$_evcode", result.Code);
                mailBody = mailBody.Replace("$_Price", Manager.Common.Helpers.Common.FormatNumber(result.Price, 0) + " VND");
                mailBody = mailBody.Replace("$_Total", Manager.Common.Helpers.Common.FormatNumber(result.Price, 0) + " VND");
                mailBody = mailBody.Replace("$_VISA_Code_$", result.VisaCode);
                mailBody = mailBody.Replace("$_VISA_Name_$", result.VisaName);
                mailBody = mailBody.Replace("$_LinkGateway", linkGateway);


                message.Body = mailBody;
                message.IsBodyHtml = true; // Format mail dạng HTML
                ///-------- End of mail body --------------
                smtpClient.Host = mailDb.host;   // We use gmail as our smtp client
                smtpClient.Port = mailDb.port;
                smtpClient.EnableSsl = Convert.ToBoolean(mailDb.useSSL);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(mailDb.username, new DBase().Decrypt(mailDb.password, "vodacthe", true));
                smtpClient.Send(message);
                msg = "Successful";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
        public string SendMailSuccessVisa(VisaBookingModel result)
        {
            MailMessage message = new MailMessage(mailDbSuccess.username, result.Email);
            SmtpClient smtpClient = new SmtpClient();
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress(mailDbSuccess.username, "ENVIET GROUP");
                message.From = fromAddress;
                message.Subject = "[VISA] CẢM ƠN QUÝ KHÁCH HÀNG";

                if (result.Address != "ENVIETTESTING")
                {
                    message.CC.Add(mailDbSuccess.CC);
                }
                ///-------- Start of mail body ------------
                string mailBody;
                var webRequest = WebRequest.Create(mailDbSuccess.templateUrl);
                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                { mailBody = reader.ReadToEnd(); }
                mailBody = mailBody.Replace("$_Code", result.Code);
                mailBody = mailBody.Replace("$_Ngaygui", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                message.Body = mailBody;
                message.IsBodyHtml = true; // Format mail dạng HTML
                ///-------- End of mail body --------------
                smtpClient.Host = mailDbSuccess.host;   // We use gmail as our smtp client
                smtpClient.Port = mailDbSuccess.port;
                smtpClient.EnableSsl = Convert.ToBoolean(mailDbSuccess.useSSL);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(mailDbSuccess.username, new DBase().Decrypt(mailDbSuccess.password, "vodacthe", true));
                smtpClient.Send(message);
                msg = "Successful";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
    }
}
