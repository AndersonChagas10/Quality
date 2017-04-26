using System;
using System.Net.Mail;
using System.ComponentModel;
using System.Net;
using Dominio;
using System.Linq;
using System.Data;
using DTO;
using System.Collections.Generic;

namespace SgqSystem.Mail
{
    public class SimpleAsynchronous
    {

        public static List<EmailContent> ListaDeMail;
        private static int antiSpanMock = 0;

        public static List<EmailContent> CreateMailSgqAppDeviation()
        {

            using (var db = new SgqDbDevEntities())
            {
                //db.Configuration.ValidateOnSaveEnabled = false;
                //db.Configuration.AutoDetectChangesEnabled = false;
                //ListaDeMail = new List<EmailContent>();
                db.Configuration.LazyLoadingEnabled = false;
                var Mails = db.Deviation.Where(r => r.AlertNumber > 0 && !r.sendMail.GetValueOrDefault() && r.DeviationMessage != null).ToList();


                if (Mails != null && Mails.Count() > 0)
                {
                    foreach (var m in Mails)
                    {
                        var newMail = new EmailContent()
                        {
                            AddDate = DateTime.Now,
                            Body = m.DeviationMessage,
                            IsBodyHtml = true,
                            Subject = "teste v2",
                            To = "celso.bernar@grtsolucoes.com.br",
                            Project = "SGQApp"
                        };
                        //ListaDeMail.Add(newMail);

                        /*Adiciona 5 email caso seja para testes.*/
                        if (GlobalConfig.mockEmail)
                        {
                            if (antiSpanMock > 5)
                                continue;

                            antiSpanMock++;
                        }

                        db.EmailContent.Add(newMail);

                        m.sendMail = true;
                        db.Deviation.Attach(m);
                        var entry = db.Entry(m);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        entry.Property(r => r.sendMail).IsModified = true;

                    }

                    db.SaveChanges();

                }

                return db.EmailContent.Where(r => r.SendStatus == null && r.Project == "SGQApp").ToList();
            }

        }

        public static void SendMailFromDeviationSgqApp()
        {
            var emails = CreateMailSgqAppDeviation();
            if (emails != null && emails.Count() > 0)
                foreach (var i in emails)
                    SendMail(i);
        }

        //static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                using (var db = new SgqDbDevEntities())
                {
                    //db.Configuration.AutoDetectChangesEnabled = false;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;

                    // Get the unique identifier for this asynchronous operation.
                    String token = (string)e.UserState;
                    var id = int.Parse(token);
                    var entryEmailContent =  db.EmailContent.FirstOrDefault(r => r.Id == id);
                    db.EmailContent.Attach(entryEmailContent);
                    var entry = db.Entry(entryEmailContent);
                    entry.State = System.Data.Entity.EntityState.Modified;
                    entry.Property(r => r.AddDate).IsModified = false;
                    entryEmailContent.AlterDate = DateTime.Now;
                    entryEmailContent.SendDate = DateTime.Now;

                    if (e.Cancelled)
                    {
                        //Console.WriteLine("[{0}] Send canceled.", token);
                        entryEmailContent.SendStatus = string.Format("[{0}] Send canceled.", token);
                    }
                    if (e.Error != null)
                    {
                        //Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
                        entryEmailContent.SendStatus = string.Format("[{0}] {1}", token, e.Error.ToString());
                    }
                    else
                    {
                        //Console.WriteLine("Message sent.");
                        entryEmailContent.SendStatus = string.Format("Message sent.");
                    }

                    db.SaveChanges();
                    //mailSent = true;
                }
            }
            catch (Exception ex)
            {
                new CreateLog(new Exception("Erro ao enviar e mail", ex));
            }
        }

        public static void SendMail(EmailContent mailEntry)
        {
            // Command line argument must the the SMTP host.
            if (GlobalConfig.mockEmail)
            {
                GlobalConfig.emailFrom = "celsogea@hotmail.com";
                GlobalConfig.emailPass = "Thebost1";
                GlobalConfig.emailSmtp = "smtp.live.com";
                GlobalConfig.emailPort = 587;
                GlobalConfig.emailSSL = true;
            }

            SmtpClient client = new SmtpClient(GlobalConfig.emailSmtp, GlobalConfig.emailPort);
            client.EnableSsl = GlobalConfig.emailSSL; //true Hotmail
            client.Credentials = new NetworkCredential(GlobalConfig.emailFrom, GlobalConfig.emailPass);

            //Address
            // Specify the e-mail sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            MailAddress from = new MailAddress(GlobalConfig.emailFrom, "SGQ", System.Text.Encoding.UTF8);
            // Set destinations for the e-mail message.
            MailAddress to = new MailAddress(mailEntry.To);

            //MailMessage - Subject + Body
            // Specify the message content.
            MailMessage message = new MailMessage(from, to);
            message.Subject = mailEntry.Subject;
            message.Body = mailEntry.Body;
            message.Body += Environment.NewLine;
            message.IsBodyHtml = mailEntry.IsBodyHtml.GetValueOrDefault();
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.BodyEncoding = System.Text.Encoding.UTF8;

            //Callback
            // Set the method that is called back when the send operation ends.
            client.SendCompleted += new
            SendCompletedEventHandler(SendCompletedCallback);

            // The userState can be any object that allows your callback 
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            mailEntry.SendStatus = "Sending";
            string userState = mailEntry.Id.ToString();
            client.SendAsync(message, userState);

        }
    }


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
}

