using AutoMapper;
using DTO;
using DTO.DTO;
using Helper;
using PlanoAcaoEF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanoDeAcaoMVC.PaMail
{
    public static class PaAsyncServices
    {

        #region PA Email

        public static void SendMailPATeste(EmailContent email, string mailTo)
        {

            try
            {
                var emailFrom = "celsogea@hotmail.com";
                var emailPass = "Thebost1";
                var emailSmtp = "smtp.live.com";
                var emailPort = 587;
                var emailSSL = true;

                //var emailFrom = "celsogea@hotmail.com";
                //var emailPass = "Thebost1";
                //var emailSmtp = "smtp.live.com";
                //var emailPort = 587;
                //var emailSSL = true;

                using (var db = new PlanoDeAcaoEntities())
                {
                    SaveEmailContenteEF(email, db);
                    MailSender.SendMail(Mapper.Map<EmailContentDTO>(email), emailFrom, emailPass, emailSmtp, emailPort, emailSSL, SendCompletedCallbackPA, true);
                }
            }
            catch (Exception ex)
            {
                //new CreateLog(ex, teste);
                //throw ex;
            }

        }

        private static void SaveEmailContenteEF(EmailContent email, PlanoDeAcaoEntities dbPa)
        {
            if (email.Id > 0)
            {
                email.AlterDate = DateTime.Now;
                dbPa.EmailContent.Attach(email);
                var entry = dbPa.Entry(email);
                entry.State = System.Data.Entity.EntityState.Modified;
                entry.Property(e => e.AddDate).IsModified = false;
                dbPa.SaveChanges();
            }
            else
            {
                email.AddDate = DateTime.Now;
                dbPa.EmailContent.Add(email);
                dbPa.SaveChanges();
            }
        }

        public  static void SendCompletedCallbackPA(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                using (var db = new PlanoDeAcaoEntities())
                {

                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;

                    // Get the unique identifier for this asynchronous operation.
                    String token = (string)e.UserState;
                    var id = int.Parse(token);

                    var emailContent = db.EmailContent.FirstOrDefault(r => r.Id == id);

                    if (e.Cancelled)
                    {
                        //Console.WriteLine("[{0}] Send canceled.", token);
                        emailContent.SendStatus = string.Format("Send canceled: [{0}].", token);
                    }
                    if (e.Error != null)
                    {
                        //Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
                        emailContent.SendStatus = string.Format("Error sending: [{0}] {1}", token, e.Error.ToString());
                    }
                    else
                    {
                        //Console.WriteLine("Message sent.");
                        emailContent.SendStatus = string.Format("Message sent.");
                    }

                    emailContent.AlterDate = DateTime.Now;
                    emailContent.SendDate = DateTime.Now;

                    if (emailContent.SendStatus.Length > 889)
                        emailContent.SendStatus = emailContent.SendStatus.Substring(0, 889);

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
            catch (DbEntityValidationException ex1)
            {
                var aMerdaQueDeu = string.Empty;
                //transaction.Rollback();
                foreach (var i in ex1.EntityValidationErrors)
                {
                    aMerdaQueDeu += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", i.Entry.Entity.GetType().Name, i.Entry.State);
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", i.Entry.Entity.GetType().Name, i.Entry.State);
                    foreach (var ve in i.ValidationErrors)
                    {
                        aMerdaQueDeu += string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //new CreateLog(new Exception(aMerdaQueDeu, ex1));
                //throw ex1;
            }
            catch (Exception ex)
            {
                //throw ex;
                //new CreateLog(new Exception("Erro ao enviar e mail", ex));
            }
        }

        #endregion

    }
}
