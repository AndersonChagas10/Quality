using System;
using System.Net.Mail;
using System.ComponentModel;
using System.Net;
using Dominio;
using System.Linq;
using DTO.Helpers;
using System.Data;

namespace SgqSystem.Mail
{
    public class SimpleAsynchronous
    {
        private static bool executing { get; set; }
        public static void UpdatePassAES()
        {
            if (!executing)
            {
                executing = true;
                using (var db = new SgqDbDevEntities())
                {
                    using (var ts = db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
                    {

                        db.Configuration.AutoDetectChangesEnabled = false;
                        db.Configuration.LazyLoadingEnabled = false;

                        var users = db.UserSgq.ToList();

                        foreach (var i in users)
                        {

                            var old = i.Password;
                            var nova = Guard.Descriptografar3DES(old);
                            var salvar = Guard.EncryptStringAES(nova);
                            i.Password = salvar;

                            db.UserSgq.Attach(i);
                            var entry = db.Entry(i);
                            entry.State = System.Data.Entity.EntityState.Modified;
                            entry.Property(e => e.Password).IsModified = true;
                        }

                        db.SaveChanges();
                        ts.Commit();

                    }
                }
                //executing = false;
            }

        }

        static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }

        public static void SendMail()
        {


            //MailMessage Message = new MailMessage();
            //Message.IsBodyHtml = true;
            //Message.To.Add(new MailAddress(toEmail, toName));
            //Message.From = (new MailAddress(this.emailLogin, this.emailLoginName));
            //Message.Subject = this.subject;
            //Message.Body = this.messageBody;

            //SmtpClient sc = new SmtpClient();
            //sc.Port = 587;
            //sc.Host = "smtp.live.com";
            //sc.EnableSsl = true; // <-- ATENÇÃO.
            //sc.UseDefaultCredentials = false;
            //sc.Credentials = new NetworkCredential(this.emailLogin, this.pass);


            //sc.Send(Message);

            // Command line argument must the the SMTP host.
            SmtpClient client = new SmtpClient("smtp.live.com", 587);
            client.EnableSsl = true; //true Hotmail
            client.Credentials = new NetworkCredential("celsogea@hotmail.com", "Thebost1");

            #region Address

            // Specify the e-mail sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            MailAddress from = new MailAddress("celsogea@hotmail.com", "SGQ", System.Text.Encoding.UTF8);
            // Set destinations for the e-mail message.
            MailAddress to = new MailAddress("celsogea@hotmail.com");

            #endregion

            #region MailMessage - Subject + Body

            // Specify the message content.
            MailMessage message = new MailMessage(from, to);
            message.Subject = "test message 1";
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.Body = "This is a test e-mail message sent by an application. ";
            message.Body += Environment.NewLine;
            message.BodyEncoding = System.Text.Encoding.UTF8;

            #endregion

            #region Callback

            // Set the method that is called back when the send operation ends.
            client.SendCompleted += new
            SendCompletedEventHandler(SendCompletedCallback);

            #endregion

            // The userState can be any object that allows your callback 
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            string userState = "test message1";
            client.SendAsync(message, userState);


        }
    }
}

