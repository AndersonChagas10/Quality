using DTO.DTO;
using System;
using System.Net;
using System.Net.Mail;

namespace Helper
{
    public static class MailSender
    {
        /// <summary>
        /// Enviar email Asyncrono, os destinatários devem ser separados por VIRGULA, adivindos de mailEntry.To: Type string. ex: "email1@teste.com, email2@teste.com"
        /// </summary>
        /// <param name="mailEntry"></param>
        /// <param name="emailFrom"></param>
        /// <param name="emailPass"></param>
        /// <param name="emailSmtp"></param>
        /// <param name="emailPort"></param>
        /// <param name="emailSSL"></param>
        /// <param name="callback"></param>
        /// <param name="runCallBack"></param>
        public static void SendMail(EmailContentDTO mailEntry, string emailFrom, string emailPass, string emailSmtp, int emailPort, bool emailSSL,  SendCompletedEventHandler callback, bool runCallBack)
        {

            //return;

            SmtpClient client = new SmtpClient(emailSmtp, emailPort);
            client.EnableSsl = emailSSL; //true Hotmail
            client.Credentials = new NetworkCredential(emailFrom, emailPass);

            //Address
            // Specify the e-mail sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            MailAddress from = new MailAddress(emailFrom, "SGQ", System.Text.Encoding.UTF8);
            // Set destinations for the e-mail message.
            //MailAddress to = new MailAddress();
            var mails = mailEntry.To.Split(',');


            //MailMessage - Subject + Body
            // Specify the message content.
            MailMessage message = new MailMessage();
            message.From = from;
            foreach (var i in mails)
                message.To.Add(i);
            message.Subject = mailEntry.Subject;
            message.Body = mailEntry.Body;
            message.Body += Environment.NewLine;
            message.IsBodyHtml = mailEntry.IsBodyHtml;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.BodyEncoding = System.Text.Encoding.UTF8;

            if (runCallBack)
            {
                //Callback
                // Set the method that is called back when the send operation ends.
                client.SendCompleted += new
                SendCompletedEventHandler(callback);
            }

            // The userState can be any object that allows your callback 
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            mailEntry.SendStatus = "Sending";
            string userState = mailEntry.Id.ToString();
            client.SendAsync(message, userState);

        }

    }
}
