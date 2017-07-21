using DTO.DTO;
using System;
using System.Net;
using System.Net.Mail;

namespace Helper
{
    public static class MailSender
    {

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
            MailAddress to = new MailAddress(mailEntry.To);

            //MailMessage - Subject + Body
            // Specify the message content.
            MailMessage message = new MailMessage(from, to);
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
