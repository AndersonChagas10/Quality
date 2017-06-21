using System;
using System.ComponentModel;
using Dominio;
using System.Linq;
using System.Data;
using DTO;
using System.Collections.Generic;
using SgqSystem.Services;
using System.Data.Entity.Validation;
using SgqSystem.Controllers.Api;
using Helper;
using AutoMapper;
using DTO.DTO;
using System.Threading.Tasks;

namespace SgqSystem.Mail
{
    public class SimpleAsynchronous
    {

        public static List<EmailContent> ListaDeMail;
        private static int tamanhoDoPool = 1000;
        private static bool running { get; set; }

        #region SGQ Email

        public static void CreateMailSgqAppDeviation()
        {

            using (var db = new SgqDbDevEntities())
            {
                try
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

                            var body = Uri.UnescapeDataString(m.DeviationMessage).ToString().Replace("O Supervisor da área será notificado Ok ", "");
                            var parLevel1 = db.ParLevel1.FirstOrDefault(r => r.Id == m.ParLevel1_Id).Name;
                            var parLevel2 = db.ParLevel2.FirstOrDefault(r => r.Id == m.ParLevel2_Id).Name;
                            string company = string.Empty;

                            if (m.ParCompany_Id > 0)
                                company = db.ParCompany.FirstOrDefault(r => r.Id == m.ParCompany_Id).Name;
                            else
                                company = "Corporativo";

                            var subject = "Alerta emitido para o Indicador: " + parLevel1 + ", Monitoramento: " + parLevel2 + " da Unidade: " + company;

                            var newMail = new EmailContent()
                            {
                                AddDate = DateTime.Now,
                                Body = subject + "<br><br>" + RemoveEspacos(body),
                                IsBodyHtml = true,
                                Subject = subject,
                                To = "cgnunes7@gmail.com",
                                Project = "SGQApp"
                            };

                            db.EmailContent.Add(newMail);
                            db.Database.ExecuteSqlCommand("UPDATE Deviation SET sendMail = 1 WHERE ID = " + m.Id);

                        }

                        db.SaveChanges();

                    }
                }
                catch (Exception e)
                {
                    new CreateLog(new Exception("Ocorreu um erro em: [CreateMailSgqAppDeviation]", e));
                }

                //return db.EmailContent.Where(r => r.SendStatus == null && r.Project == "SGQApp").ToList();
            }

        }

        public static void CreateMailSgqAppCorrectiveAction()
        {
            try
            {
                using (var db = new SgqDbDevEntities())
                {
                    using (var controller = new CorrectActApiController())
                    {
                        var listaCorrectiveActionDb = db.Database.SqlQuery<CorrectiveAction>("SELECT * FROM CorrectiveAction WHERE MailProcessed = 0");
                        foreach (var ca in listaCorrectiveActionDb)
                        {
                            var colectionLevel2 = db.CollectionLevel2.FirstOrDefault(r => r.Id == ca.CollectionLevel02Id);
                            var parLevel1 = db.ParLevel1.FirstOrDefault(r => r.Id == colectionLevel2.ParLevel1_Id).Name;
                            var parLevel2 = db.ParLevel2.FirstOrDefault(r => r.Id == colectionLevel2.ParLevel2_Id).Name;
                            string company = string.Empty;

                            if (colectionLevel2.UnitId > 0)
                                company = db.ParCompany.FirstOrDefault(r => r.Id == colectionLevel2.UnitId).Name;
                            else
                                company = "Corporativo";

                            var subject = "Ação coretiva emitida para o Indicador: " + parLevel1 + ", Monitoramento: " + parLevel2 + " da Unidade: " + company;

                            var newMail = new EmailContent()
                            {
                                AddDate = DateTime.Now,
                                IsBodyHtml = true,
                                Subject = subject,
                                To = "celso.bernar@grtsolucoes.com.br",
                                Project = "SGQApp"
                            };
                            var model = controller.GetCorrectiveActionById(ca.Id);
                            newMail.Body = subject + "<br><br>" + model.SendMeByMail;
                            db.EmailContent.Add(newMail);
                            db.SaveChanges();

                            db.Database.ExecuteSqlCommand("UPDATE CorrectiveAction SET MailProcessed = 1 WHERE Id = " + ca.Id);
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                new CreateLog(new Exception("Ocorreu um erro em: [CreateMailSgqAppDeviation]", e));
            }

        }

        public static void SendMailFromDeviationSgqApp()
        {
            try
            {
                CreateMailSgqAppDeviation();
                CreateMailSgqAppCorrectiveAction();
                using (var db = new SgqDbDevEntities())
                    ListaDeMail = db.EmailContent.Where(r => r.SendStatus == null && r.Project == "SGQApp").ToList();

                if (ListaDeMail != null && ListaDeMail.Count() > 0)
                    foreach (var i in ListaDeMail.ToList())
                        Task.Run(() => MailSender.SendMail(Mapper.Map<EmailContentDTO>(i), GlobalConfig.emailFrom, GlobalConfig.emailPass, GlobalConfig.emailSmtp, GlobalConfig.emailPort, GlobalConfig.emailSSL, SendCompletedCallbackSgq, true));
            }
            catch (Exception ex)
            {
                new CreateLog(new Exception("Erro no metodo [SendMailFromDeviationSgqApp]", ex));
                throw ex;
            }

        }

        public static void SendMailFromDeviationSgqAppTesteBR(string mailTo, bool deviation)
        {
            var emailFrom = "celsogea@hotmail.com";
            var emailPass = "Thebost1";
            var emailSmtp = "smtp.live.com";
            var emailPort = 587;
            var emailSSL = true;

            CreateMailSgqAppDeviation();
            CreateMailSgqAppCorrectiveAction();

            using (var db = new SgqDbDevEntities())
                ListaDeMail = db.EmailContent.Where(r => r.SendStatus == null && r.Project == "SGQApp").ToList();

            if (ListaDeMail != null && ListaDeMail.Count() > 0)
                foreach (var i in ListaDeMail.Take(3).ToList())
                {
                    i.To = mailTo;
                    Task.Run(() => MailSender.SendMail(Mapper.Map<EmailContentDTO>(i), emailFrom, emailPass, emailSmtp, emailPort, emailSSL, SendCompletedCallbackSgq, true));
                }

        }

        public static void SendMailFromDeviationSgqAppTesteUSA(string mailTo)
        {

            var emailFrom = "celsogea@hotmail.com";
            var emailPass = "Thebost1";
            var emailSmtp = "smtp.live.com";
            var emailPort = 587;
            var emailSSL = true;

            //CreateMailSgqAppDeviation();
            CreateMailSgqAppCorrectiveAction();

            using (var db = new SgqDbDevEntities())
                ListaDeMail = db.EmailContent.Where(r => r.SendStatus == null && r.Project == "SGQApp").ToList();

            if (ListaDeMail != null && ListaDeMail.Count() > 0)
                foreach (var i in ListaDeMail.Take(3).ToList())
                {
                    i.To = mailTo;
                    Task.Run(() => MailSender.SendMail(Mapper.Map<EmailContentDTO>(i), emailFrom, emailPass, emailSmtp, emailPort, emailSSL, SendCompletedCallbackSgq, true));
                }
        }

        //private static EmailContent CreateMailUSAFromCOrrectiveAction(string mailTo)
        //{
        //    var testeMail = new EmailContent()
        //    {
        //        AddDate = DateTime.Now,
        //        IsBodyHtml = true,
        //        Subject = "teste v2",
        //        To = mailTo,
        //        Project = "SGQApp"
        //    };
        //    try
        //    {
        //        using (var db = new SgqDbDevEntities())
        //        {
        //            using (var controller = new CorrectActApiController())
        //            {
        //                var id = db.CorrectiveAction.OrderByDescending(r => r.Id).FirstOrDefault().Id;
        //                var model = controller.GetCorrectiveActionById(id);
        //                testeMail.Body = model.SendMeByMail;
        //            }
        //            db.EmailContent.Add(testeMail);
        //            db.SaveChanges();
        //        }
        //        return testeMail;
        //    }
        //    catch (Exception ex)
        //    {
        //        new CreateLog(new Exception("Erro no metodo [CreateMailUSAFromCOrrectiveAction]", ex), testeMail);
        //        throw ex;
        //    }
        //}

        public static void SendCompletedCallbackSgq(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                using (var db = new SgqDbDevEntities())
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
                new CreateLog(new Exception(aMerdaQueDeu, ex1));
                throw;
            }
            catch (Exception ex)
            {
                new CreateLog(new Exception("Erro ao enviar e mail", ex));
            }
        }

        #endregion

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

        #region ResendProcessJson

        public static void ResendProcessJson()
        {
            using (var db = new SgqDbDevEntities())
            {
                var ids = db.CollectionJson.Where(r => !r.IsProcessed /*&& r.TTP == null*/).Select(r => r.Id).ToList();
                foreach (var i in ids)
                {
                    using (var service = new SyncServices())
                    {
                        try
                        {
                            service.ProcessJson("", i, false);
                        }
                        catch (Exception e)
                        {
                            //new CreateLog(e);
                        }
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

