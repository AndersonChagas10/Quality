using System;
using System.Net.Mail;
using System.Net;
using System.Runtime.Remoting.Messaging;

namespace EmailComponent
{
    public class Email :  ConfigEmail
    {
        public bool ssl { get; set; }
        public string subject { get; set; }
        public string toName { get; set; }
        public string toEmail { get; set; }
        public string messageBody { get; set; }
        public string smtpServer { get; set; }
        public NetworkCredential smtpCredentials { get; set; }

        public bool SendEmail(string toEmail, string toName)
        {
            try
            {
                MailMessage Message = new MailMessage();
                Message.IsBodyHtml = true;
                Message.To.Add(new MailAddress(toEmail, toName));
                Message.From = (new MailAddress(this.emailLogin, this.emailLoginName));
                Message.Subject = this.subject;
                Message.Body = this.messageBody;

                SmtpClient sc = new SmtpClient();
                sc.Port = 587;
                sc.Host = "smtp.live.com";
                sc.EnableSsl = true; // <-- ATENÇÃO.
                sc.UseDefaultCredentials = false;
                sc.Credentials = new NetworkCredential(this.emailLogin, this.pass);


                sc.Send(Message);
            }
            #pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
            #pragma warning restore CS0168 // Variable is declared but never used
            {
                return false;
            }
            return true;
        }

        public delegate bool SendEmailDelegate(string toEmail, string toName);

        public void GetResultsOnCallback(IAsyncResult ar)
        {
            SendEmailDelegate del = (SendEmailDelegate)((AsyncResult)ar).AsyncDelegate;
            try
            {
                bool result = del.EndInvoke(ar);
            }
            #pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
            #pragma warning restore CS0168 // Variable is declared but never used
            {
                #pragma warning disable CS0219 // Variable is assigned but its value is never used
                bool result = false;
                #pragma warning restore CS0219 // Variable is assigned but its value is never used
            }
        }

        public bool SendEmailAsync(string toEmail, string toName)
        {
            try
            {
                SendEmailDelegate dc = new SendEmailDelegate(this.SendEmail);
                AsyncCallback cb = new AsyncCallback(this.GetResultsOnCallback);
                IAsyncResult ar = dc.BeginInvoke(toEmail, toName, cb, null);
            }
            #pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
            #pragma warning restore CS0168 // Variable is declared but never used
            {
                return false;
            }
            return true;
        }
    }

}