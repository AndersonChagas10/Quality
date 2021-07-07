using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Conformity.Infra.CrossCutting
{
    public static class MailHelper
    {
        public static void SendMail(string emailFrom, string emailPass, string emailSmtp,
            int emailPort, bool emailSSL, string emailList, string subject, string body, string systemName)
        {

            try
            {
                SmtpClient client = new SmtpClient(emailSmtp, emailPort);
                client.EnableSsl = emailSSL; //true Hotmail
                client.Credentials = new NetworkCredential(emailFrom, emailPass);

                MailAddress from = new MailAddress(emailFrom, systemName, System.Text.Encoding.UTF8);

                var mails = emailList.Split(',');

                mails = mails.Where(v => v.Contains("@") && v.Length > 5).Select(v => v.Replace(" ", "").Trim()).Distinct().ToArray();

                MailMessage message = new MailMessage();
                message.From = from;

                foreach (var i in mails)
                    message.To.Add(i);

                message.Subject = subject;
                message.Body = body;
                message.Body += Environment.NewLine;
                message.IsBodyHtml = true;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.BodyEncoding = System.Text.Encoding.UTF8;

                client.SendAsync(message, DateTime.Now.ToString("yyyyMMhhss"));
            }
            catch (Exception ex)
            {

            }

        }
    }
}
