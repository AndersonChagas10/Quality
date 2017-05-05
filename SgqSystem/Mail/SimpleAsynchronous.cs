using System;
using System.Net.Mail;
using System.ComponentModel;
using System.Net;
using Dominio;
using System.Linq;
using System.Data;
using DTO;
using System.Collections.Generic;
using SgqSystem.Services;

namespace SgqSystem.Mail
{
    public class SimpleAsynchronous
    {

        public static List<EmailContent> ListaDeMail;
        private static int tamanhoDoPool = 4;
        private static bool running { get; set; }

        public static List<EmailContent> CreateMailSgqAppDeviation()
        {

            using (var db = new SgqDbDevEntities())
            {
                //db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                /*Cria Novos Emails de acordo com a quantidade do pool na emailContent*/
                var Mails = db.Deviation.Where(r => r.AlertNumber > 0 && (r.sendMail == null || r.sendMail == false) && r.DeviationMessage != null).Take(tamanhoDoPool).ToList();

                if (Mails != null && Mails.Count() > 0)
                {
                    foreach (var m in Mails)
                    {

                        var newMail = new EmailContent()
                        {
                            AddDate = DateTime.Now,
                            Body = RemoveEspacos(m.DeviationMessage),
                            IsBodyHtml = true,
                            Subject = "teste v2",
                            To = "celso.bernar@grtsolucoes.com.br",
                            Project = "SGQApp"
                        };
                        //ListaDeMail.Add(newMail);

                        db.EmailContent.Add(newMail);

                        db.Deviation.Attach(m);
                        var entry = db.Entry(m);
                        m.sendMail = true;
                        entry.State = System.Data.Entity.EntityState.Modified;
                        entry.Property(r => r.sendMail).IsModified = true;

                    }

                    db.SaveChanges();

                }
                
                return db.EmailContent.Where(r => r.SendStatus == null && r.Project == "SGQApp").ToList();
            }

        }

        private static string RemoveEspacos(string deviationMessage)
        {
            try
            {

                var result = string.Empty;
                foreach (var i in deviationMessage.Split('>'))
                    result += i.TrimStart().TrimEnd() + "> ";

                return result.Substring(0, result.IndexOf("<button"));
            }
            catch (Exception e)
            {
                new CreateLog(e);
                return deviationMessage;
            }
        }

        public static void SendMailFromDeviationSgqApp()
        {
            try
            {
                ListaDeMail = CreateMailSgqAppDeviation();
                if (ListaDeMail != null && ListaDeMail.Count() > 0)
                    foreach (var i in ListaDeMail.ToList())
                        SendMail(i);
            }
            catch (Exception ex)
            {
                new CreateLog(new Exception("Erro ao enviar email", ex));
                throw ex;
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

        public static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                using (var db = new SgqDbDevEntities())
                {


                    // Get the unique identifier for this asynchronous operation.
                    String token = (string)e.UserState;
                    var id = int.Parse(token);

                    var emailContent = db.EmailContent.FirstOrDefault(r => r.Id == id);

                    if (e.Cancelled)
                    {
                        //Console.WriteLine("[{0}] Send canceled.", token);
                        emailContent.SendStatus = string.Format("[{0}] Send canceled.", token);
                    }
                    if (e.Error != null)
                    {
                        //Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
                        emailContent.SendStatus = string.Format("[{0}] {1}", token, e.Error.ToString());
                    }
                    else
                    {
                        //Console.WriteLine("Message sent.");
                        emailContent.SendStatus = string.Format("Message sent.");
                    }

                    emailContent.AlterDate = DateTime.Now;
                    emailContent.SendDate = DateTime.Now;

                    /*Atualiza email enviados no emailContent DB*/
                    db.EmailContent.Attach(emailContent);
                    //emailContent.Body = "";
                    var entry = db.Entry(emailContent);
                    entry.State = System.Data.Entity.EntityState.Modified;
                    entry.Property(r => r.SendStatus).IsModified = true;
                    entry.Property(r => r.AlterDate).IsModified = true;
                    entry.Property(r => r.SendDate).IsModified = true;
                    //entry.Property(r => r.Body).IsModified = true;
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                new CreateLog(new Exception("Erro ao enviar e mail", ex));
                //throw ex;
            }
        }


        #region SEI LA =)

        public static void ResendProcessJson()
        {

            using (var db = new SgqDbDevEntities())
            {
                var ids = db.CollectionJson.Where(r => r.IsProcessed && r.TTP == null).Select(r => r.Id);
                foreach (var i in ids)
                {
                    using (var service = new SyncServices())
                    {
                        service.ProcessJson("", i);
                    }
                }
            }

        }

        #endregion

    }
}

//public class FimDoEnvio
//{
//    private SgqDbDevEntities db = new SgqDbDevEntities();

//    public void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
//    {
//        try
//        {
//            db.Configuration.AutoDetectChangesEnabled = false;
//            db.Configuration.ValidateOnSaveEnabled = false;

//            // Get the unique identifier for this asynchronous operation.
//            String token = (string)e.UserState;
//            var id = int.Parse(token);

//            var emailContent = db.EmailContent.FirstOrDefault(r => r.Id == id);
//            //var emailContent = ListaDeMail.FirstOrDefault(r => r.Id == id);
//            if (e.Cancelled)
//            {
//                //Console.WriteLine("[{0}] Send canceled.", token);
//                emailContent.SendStatus = string.Format("[{0}] Send canceled.", token);
//            }
//            if (e.Error != null)
//            {
//                //Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
//                emailContent.SendStatus = string.Format("[{0}] {1}", token, e.Error.ToString());
//            }
//            else
//            {
//                //Console.WriteLine("Message sent.");
//                emailContent.SendStatus = string.Format("Message sent.");
//            }

//            //
//            //await db.SaveChangesAsync();

//            emailContent.AlterDate = DateTime.Now;
//            emailContent.SendDate = DateTime.Now;

//            db.EmailContent.Attach(emailContent);
//            var entry = db.Entry(emailContent);
//            entry.State = System.Data.Entity.EntityState.Modified;
//            entry.Property(r => r.SendStatus).IsModified = true;
//            entry.Property(r => r.AlterDate).IsModified = true;
//            entry.Property(r => r.SendDate).IsModified = true;

//            db.SaveChanges();
//            //mailSent = true;
//            db.Dispose();
//        }
//        catch (Exception ex)
//        {
//            new CreateLog(new Exception("Erro ao enviar e mail", ex));
//            //throw ex;
//        }
//    }
//}
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
//}

