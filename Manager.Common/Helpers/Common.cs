using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net.Http;
using Renci.SshNet;
using System.Threading.Tasks;

namespace Manager.Common.Helpers
{
    public class Common
    {
        private static Random random = new Random();

        /// <summary>
        /// Trước khi SendMail thì nhớ sử dụng hàm GetEVEMailContentByProgram để lấy các nội dung như: usernameEmail, password, host, isSSL, CC, BCC <br> </br>
        /// displayName: Tên hiển thị trên Email. Ví dụ: Flight VN Group <br> </br>
        /// subject: Tiêu đề. Ví dụ: Thông báo ABC việc XYZ..... <br> </br>
        /// content: Các nội dung trong email <br> </br>
        /// toMail: Email nhận <br> </br>
        /// usernameEmail: Email gửi. Ví dụ: service@enviet-group.com <br> </br>
        /// Với isCC và isBCC. true thì đồng ý, false thì không
        /// </summary>
        public static bool SendMail(string displayName, string subject, string content,
    string toMail, string usernameEmail, string password, string host, int port, bool isSSL, string CC, string BCC, bool isCC, bool isBCC)
        {
            bool rs = false;
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new SmtpClient();
                {
                    smtp.Host = host; //host name
                    smtp.Port = port; //port number
                    smtp.EnableSsl = isSSL; //whether your smtp server requires SSL
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential()
                    {
                        UserName = usernameEmail,
                        Password = DecryptMD5(password, "vodacthe", true)
                    };
                }
                MailAddress fromAddress = new MailAddress(usernameEmail, displayName);
                message.From = fromAddress;
                message.To.Add(toMail);
                if (!string.IsNullOrEmpty(CC) && isCC)
                {
                    message.CC.Add(CC);
                }
                if (!string.IsNullOrEmpty(BCC) && isBCC)
                {
                    message.Bcc.Add(BCC);
                }
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = content;
                smtp.Send(message);
                rs = true;
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine("SMTP Exception: " + smtpEx.Message);
                Console.WriteLine("Status Code: " + smtpEx.StatusCode);
                rs = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                rs = false;
            }
            return rs;
        }

        /// <summary>
        /// Decryp MD5
        /// </summary>
        public static string DecryptMD5(string toDecrypt, string key, bool useHashing)
        {
            try
            {
                byte[] array = Convert.FromBase64String(toDecrypt);
                byte[] key2;
                if (useHashing)
                {
                    MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                    key2 = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(key));
                }
                else
                {
                    key2 = Encoding.UTF8.GetBytes(key);
                }

                TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
                tripleDESCryptoServiceProvider.Key = key2;
                tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
                tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
                ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor();
                byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        /// Định dạng số thành 1,000,000
        /// </summary>
        public static string FormatNumber(object value, int SoSauDauPhay = 2)
        {
            bool isNumber = IsNumeric(value);
            decimal GT = 0;
            if (isNumber)
            {
                GT = Convert.ToDecimal(value);
            }
            string str = "";
            string thapPhan = "";
            for (int i = 0; i < SoSauDauPhay; i++)
            {
                thapPhan += "#";
            }
            if (thapPhan.Length > 0) thapPhan = "." + thapPhan;
            string snumformat = string.Format("0:#,##0{0}", thapPhan);
            str = string.Format("{" + snumformat + "}", GT);

            return str;
        }

        private static bool IsNumeric(object value)
        {
            return value is sbyte
                       || value is byte
                       || value is short
                       || value is ushort
                       || value is int
                       || value is uint
                       || value is long
                       || value is ulong
                       || value is float
                       || value is double
                       || value is decimal;
        }

        /// <summary>
        /// Random chuỗi
        /// </summary>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        /// <summary>
        /// Generate JWT Token (HMACSHA256)
        /// </summary>
        public static string GenerateJwtToken(string partnerCode, string ApiKey, string SecretKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));
            var payload = new JwtPayload
            {
                {"typ", "JWT"},
                {"alg","HS256" },
                {"iss", partnerCode },
                {"jti",ApiKey + "-" },
                {"api_key", ApiKey }
            };

            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        /// <summary>
        /// Generate Chữ ký HMACSHA256
        /// </summary>
        public static string GenerateSignature(string data, string SecretKey)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(SecretKey);

            using (HMACSHA256 hmacSHA256 = new HMACSHA256(secretKeyBytes))
            {
                byte[] hashBytes = hmacSHA256.ComputeHash(dataBytes);

                StringBuilder stringBuilder = new StringBuilder();

                foreach (byte b in hashBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        public static bool IsStringDiacritic(string input)
        {
            Regex regex = new Regex(@"[\u00C0-\u017F]");
            return regex.IsMatch(input);
        }

        public static List<string> ParseStringList(string input)
        {
            return string.IsNullOrEmpty(input)
                ? new List<string>()
                : input.Split(',')
                       .Select(item => item.Trim())
                       .Where(item => !string.IsNullOrEmpty(item))
                       .ToList();
        }
        // Upload Image Du lịch
        public static string UploadImg(IFormFile imageFiles)
        {
            string ftpServerUrl = "ftp://Manager.airline24h.com";
            string username = "envietDuLich";
            string password = "enviet@123";
            // Create FtpWebRequest object
            var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + imageFiles.FileName;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServerUrl + "/" + filename);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);
            // Upload the file to the FTP server
            using (Stream ftpStream = request.GetRequestStream())
            {
                imageFiles.CopyTo(ftpStream);
            }
            string http = "https://Manager.airline24h.com/upload/dulich/" + filename;
            return http;
        }

        // Upload Image có postfix (thư mục)
        public static string UploadImg(IFormFile imageFiles, string postfix)
        {
            string ftpServerUrl = "ftp://Manager.airline24h.com";
            string username = "envietManager";
            string password = "EnViet@456";
            // Create FtpWebRequest object
            var filename =  imageFiles.FileName;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServerUrl + postfix + "/" + filename);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);
            // Upload the file to the FTP server
            using (Stream ftpStream = request.GetRequestStream())
            {
                imageFiles.CopyTo(ftpStream);
            }
            string http = $"https://Manager.airline24h.com/upload{postfix}/" + filename;
            return http;
        }

        public static string UploadFileToSFTP(IFormFile file)
        {
            string sftpServerUrl = "125.212.248.147"; 
            string username = "EnVietIT"; 
            string password = "EnViet@456";
            string remoteDirectory = "/uploads/";
            string newFileName = string.Empty;

            // Kết nối tới SFTP server
            using (var sftp = new SftpClient(sftpServerUrl, username, password))
            {
                sftp.Connect(); // Kết nối

                // Đọc dữ liệu từ IFormFile
                // Đọc dữ liệu từ IFormFile
                using (var stream = file.OpenReadStream())
                {
                    // Tạo chuỗi thời gian theo định dạng ddMMyyyyHHmmss
                    string timestamp = DateTime.Now.ToString("ddMMyyyyHHmmss");

                    // Tạo tên file mới với timestamp
                    newFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}{timestamp}{Path.GetExtension(file.FileName)}";

                    // Tạo đường dẫn đầy đủ trên SFTP server
                    string remoteFileName = remoteDirectory + newFileName;

                    // Upload file lên SFTP server
                    sftp.UploadFile(stream, remoteFileName);
                }

                sftp.Disconnect(); // Ngắt kết nối
            }

            // Trả về đường dẫn đến file trên SFTP server (giả sử có thể truy cập qua URL)
            string fileUrl = $"https://assets.envietgroup.com{remoteDirectory}{newFileName}";
            return fileUrl;
        }

        public static async Task DeleteFileFromSFTP(string fileUrl)
        {
            string sftpServerUrl = "125.212.248.147";
            string username = "EnVietIT";
            string password = "EnViet@456";
            string remoteDirectory = "/uploads/";

            // Phân tích đường dẫn để lấy tên file
            var uri = new Uri(fileUrl);
            string fileName = Path.GetFileName(uri.LocalPath); // Lấy tên file từ URL

            // Tạo đường dẫn đầy đủ trên SFTP server
            string remoteFileName = remoteDirectory + fileName;

            // Kết nối tới SFTP server
            using (var sftp = new SftpClient(sftpServerUrl, username, password))
            {
                sftp.Connect(); // Kết nối

                // Kiểm tra xem file có tồn tại trên SFTP server không
                if (sftp.Exists(remoteFileName))
                {
                   sftp.DeleteFile(remoteFileName); // Xóa file
                }
                else
                {
                    throw new FileNotFoundException("File not found on SFTP server.", remoteFileName);
                }

                sftp.Disconnect(); // Ngắt kết nối
            }
        }

        public static bool DeleteImg(string imageUrl)
        {
            try
            {
                // Cấu hình các thông tin FTP
                string ftpServerUrl = "ftp://Manager.airline24h.com";
                string username = "envietDuLich";
                string password = "enviet@123";

                // Trích xuất tên tệp từ URL
                Uri uri = new Uri(imageUrl);
                string filename = Path.GetFileName(uri.LocalPath);

                // Tạo đối tượng FtpWebRequest để yêu cầu xóa tệp
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServerUrl + "/" + filename);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(username, password);

                // Gửi yêu cầu và nhận phản hồi
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    // Xử lý phản hồi nếu cần thiết
                    Console.WriteLine($"Delete status: {response.StatusDescription}");
                }

                return true; // Xóa thành công
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine($"Error deleting file: {ex.Message}");
                return false; // Xóa thất bại
            }
        }
    }
}
