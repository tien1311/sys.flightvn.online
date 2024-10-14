using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using TangDuLieu;

namespace Manager.DataAccess.Repository
{
    public class SupportRequestRepository
    {
        DBase db = new DBase();
        Mail mailDb = new Mail("EVM_HOTRO");





        public bool SendMail(string supportCode, string receiver, string daiLy, string boPhan, string nguoiGui, string noiDung, string mail_daily, IFormFile[] files, string ccReceiver)
        {
            MailMessage mail = new MailMessage("feedback@enviet-group.com", receiver);
            mail.From = new MailAddress("feedback@enviet-group.com", "");
            SmtpClient client = new SmtpClient();
            client.EnableSsl = Convert.ToBoolean(mailDb.useSSL);
            client.Port = mailDb.port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("feedback@enviet-group.com", new DBase().Decrypt(mailDb.password, "vodacthe", true));
            client.Host = mailDb.host;
            string subject = $"[HỖ TRỢ] {supportCode}";
            try
            {
                string mailCC = "";
                if (receiver != "vicedirector@enviet-group.com")
                {
                    mailCC = mailDb.CC;

                    if (ccReceiver != "")
                    {
                        mailCC += "," + ccReceiver;
                    }
                    mail.CC.Add(mailCC);
                }
                else
                {
                    mailCC = ccReceiver;
                    mail.CC.Add(mailCC);
                }

            }
            catch { }
            try { mail.Bcc.Add(mailDb.BCC); }
            catch { }

            if (files.Length > 0)
            {
                foreach (IFormFile file in files)
                {

                    string FileName = Path.GetFullPath(file.FileName);

                    mail.Attachments.Add(new Attachment(file.OpenReadStream(), FileName));
                }

            }




            mail.CC.Add(mail_daily);

            mail.Subject = subject;
            ///-------- Start of mail body ------------
            string mailBody;
            var webRequest = System.Net.WebRequest.Create(mailDb.templateUrl);
            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            { mailBody = reader.ReadToEnd(); }
            mailBody = mailBody.Replace("$_DaiLy", daiLy);
            mailBody = mailBody.Replace("$_NguoiGui", nguoiGui);
            mailBody = mailBody.Replace("$_Email", mail_daily);
            mailBody = mailBody.Replace("$_BoPhan", boPhan);
            mailBody = mailBody.Replace("$_NoiDung", noiDung);

            mail.Body = mailBody;
            mail.IsBodyHtml = true; // Format mail dạng HTML
            ///-------- End of mail body --------------
            client.Send(mail);
            return true;
        }


    }
}
