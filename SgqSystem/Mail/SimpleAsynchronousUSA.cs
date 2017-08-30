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
using DTO.Helpers;
using Newtonsoft.Json.Linq;
using System.Data.Entity;
using System.Net.Http;

namespace SgqSystem.Mail
{
    /// <summary>
    /// Classe de serviços asyncronos, utilizada principalmnente pela instancia do HANGFIRE do SGQ
    /// </summary>
    public class SimpleAsynchronousUSA
    {

        public static List<EmailContent> ListaDeMail;
        private static int tamanhoDoPool = 1000;
        private static bool running { get; set; }

        #region SGQ USA Email

        /// <summary>
        /// 1 - CRIA LISTA DE EMAIL NA EMAIL CONTENT COM TODOS OS CAMPOS, INCLUSIVE DESTINATÁRIOS, DEVIATION e CORRECTIVE ACTION
        /// 2 - ENVIA EMAILS NA EMAILCONTENT
        /// 3 - CALLBACK PREENCHE EMAIL CONTENT COM RESULTADO DO ENVIO
        /// </summary>
        public static void SendMailUSA()
        {
            //CRIA LISTA DE EMAIL NA EMAIL CONTENT COM TODOS OS CAMPOS
            CreateMailSgqAppDeviationUSA(); //DEVIATION 
            CreateMailSgqAppCorrectiveActionUSA(); //CORRECTIVE ACTION
            using (var db = new SgqDbDevEntities())//RECUPERA EMAILS NA EMAILCONTENT
                ListaDeMail = db.EmailContent.Where(r => r.SendStatus == null && r.Project == "SGQApp").ToList();

            if (ListaDeMail != null && ListaDeMail.Count() > 0)
                foreach (var i in ListaDeMail.Take(3).ToList())
                {
                    Task.Run(() => MailSender.SendMail(Mapper.Map<EmailContentDTO>(i), GlobalConfig.emailFrom, Guard.DecryptStringAES(GlobalConfig.emailPass), GlobalConfig.emailSmtp, GlobalConfig.emailPort, GlobalConfig.emailSSL, SendCompletedCallbackSgq, true));//ENVIA EMAILS NA EMAILCONTENT
                }

        }

        /// <summary>
        /// CRIA LISTA DE EMAIL NA EMAIL CONTENT COM TODOS OS CAMPOS, INCLUSIVE DESTINATÁRIOS
        /// </summary>
        private static void CreateMailSgqAppDeviationUSA()
        {
            using (var db = new SgqDbDevEntities())
            {
                try
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    /*Cria Novos Emails de acordo com a quantidade do pool na emailContent*/
                    var Mails = db.Deviation.Where(r => r.AlertNumber > 0 && (r.sendMail == null || r.sendMail == false) && r.DeviationMessage != null).Take(tamanhoDoPool).ToList();
                    if (Mails != null && Mails.Count() > 0)
                    {
                        foreach (var m in Mails)
                        {
                            EmailContent newMail = CreateNewEmailFromDeviationUSA(db, m, m.AlertNumber);
                            newMail.To = DestinatariosSGQJBSUSA(newMail, m);
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
            }

        }

        /// <summary>
        /// Cria lista de emails na tabela EmailContent, a partir da tabela !!!CorrectiveAction!!!, TUDO que esta na EmailContent é enviado atravez do SGQ pelo send mail.
        /// Os destinatários devem ser preenchidos neste método, e isneridos na tabela EmailContent corretamente. Devem ser separados por VIRGULA caso exista mais de um,
        /// EX com 2 destinatários: EmailContent.To = "email1@teste.com, email2@teste.com"
        /// EX com 1 destinatário: EmailContent.To = "email1@teste.com"
        /// </summary>
        public static void CreateMailSgqAppCorrectiveActionUSA()
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
                                To = "gcnunes7@gmail.com",
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

        /// <summary>
        /// 1.2 - O Conteúdo do Email:
        /// 1.2.1 - Se for alerta 1 enviar somente o alerta
        /// 1.2.2 - Se for alerta > 1 alerta e seu historico
        /// 1.2.3 - Colocar a data do alerta(depende do tablet tbm)
        /// 1.2.4 - Remover Possivel OK das mensagens
        /// </summary>
        /// <param name="db"></param>
        /// <param name="m"></param>
        /// <param name="alertNumber"></param>
        /// <returns></returns>
        private static EmailContent CreateNewEmailFromDeviationUSA(SgqDbDevEntities db, Deviation m, int alertNumber)
        {
            var body = Uri.UnescapeDataString(m.DeviationMessage).ToString();//.Replace(Resources.Resource., "").Replace("O Supervisor, o Gerente e o Diretor da área serão notificados Ok ", "").Replace("O Supervisor e o Gerente da área serão notificados. Ok", "").Replace(" Ok", "");
            var parLevel1 = db.ParLevel1.FirstOrDefault(r => r.Id == m.ParLevel1_Id).Name;
            var parLevel2 = db.ParLevel2.FirstOrDefault(r => r.Id == m.ParLevel2_Id).Name;
            string company = string.Empty;

            if (m.ParCompany_Id > 0)
                company = db.ParCompany.FirstOrDefault(r => r.Id == m.ParCompany_Id).Name;
            else
                company = "Corporativo";

            var subject = "Alert trigger for: " + parLevel1 + " -> " + parLevel2 + " Company: " + company;


            var newMail = new EmailContent()
            {
                AddDate = DateTime.Now,
                Body = m.DeviationDate.ToShortDateString() + " " + m.DeviationDate.ToShortTimeString() + ": " + subject + "<br><br>" + RemoveEspacos(body),
                IsBodyHtml = true,
                Subject = subject,
                Project = "SGQApp"
            };

            return newMail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private static string DestinatariosSGQJBSUSA(EmailContent m, Deviation d)
        {
            using (var dbLegado = new SgqDbDevEntities())
            {
                const int CCA = 52;
                const int CFF = 55;
                const int HTP = 56;
                var Roles = new List<string>();
                var query = "SELECT * FROM UserSgq WHERE  1=1 AND [role] like ";
                switch (d.ParLevel1_Id)
                {
                    case CCA:
                        if (d.AlertNumber == 1)
                        {
                            query += "'%CCALVL1%' \n AND ";
                            //Roles.Add("CCALVL1");
                        }
                        else if (d.AlertNumber > 1)
                        {
                            query += "'%CCALVL1%' \n AND ";
                            query += "'%CCALVL2%' \n AND ";
                            //Roles.Add("CCALVL1");
                            //Roles.Add("CCALVL2");
                        }
                        break;
                    case CFF:
                        if (d.AlertNumber == 1)
                        {
                            query += "'%CCALVL1%' \n AND ";
                            //Roles.Add("CFFLVL1");
                        }
                        else if (d.AlertNumber > 1)
                        {
                            query += "'%CFFLVL1%' \n AND ";
                            query += "'%CFFLVL2%' \n AND ";
                            //Roles.Add("CFFLVL1");
                            //Roles.Add("CFFLVL2");
                        }
                        break;
                    case HTP:
                        if (d.AlertNumber == 1)
                        {
                            query += "'%HTPLVL1%' \n AND ";
                            //Roles.Add("HTPLVL1");
                        }
                        else if (d.AlertNumber > 1)
                        {
                            query += "'%HTPLVL1%' \n AND ";
                            query += "'%HTPLVL2%' \n AND ";
                            //Roles.Add("HTPLVL1");
                            //Roles.Add("HTPLVL2");
                        }
                        break;

                    default:
                        break;
                }

                query += "\n AND Email IS NOT NULL";
                query += "\n AND Email <> ''";
                query += "\n 1=1"; // ;)

                var listaEmails = new List<string>();
                var listaUsuario = dbLegado.Database.SqlQuery<UserSgq>(query);

                //var query = "\n SELECT Email FROM UserSgq" +
                //            "\n WHERE Id in (SELECT UserSgq_Id FROM ParCompanyXUserSgq WHERE ParCompany_Id = " + companyId + " AND [Role] IN (SELECT Nivel FROM desvioNiveis WHERE Desvio = " + nivel + "))" +
                //            "\n  AND Email IS NOT NULL" +
                //            "\n  AND Email <> ''";

                foreach (var usuario in listaUsuario)
                {
                    var rolesUser = usuario.Role.Split(',');
                    foreach (var role in Roles)
                        if (rolesUser.Contains(role))
                            listaEmails.Add(usuario.Email);
                }

                if (listaEmails != null && listaEmails.Count() > 0)
                    return string.Join(",", listaEmails.ToArray());
                else
                {
                    //Caso não existam emails cadastrados
                    //Preenche send status e não vai para a lista de envio.
                    m.SendStatus = "Não existem destinatários para este email.";
                    return string.Empty;
                }
            }
        }


        /// <summary>
        /// Metodo para testes de rotinas de email, pode ser chamado do GlobalConfig/Config
        /// </summary>
        /// <param name="mailTo"></param>
        /// <param name="deviation"></param>
        public static void SendMailFromDeviationSgqAppTesteUSA(string mailTo)
        {

            var emailFrom = "celsogea@hotmail.com";
            var emailPass = "tR48MJsfaz1Rf+dT+Ag8dQ==";
            var emailSmtp = "smtp.live.com";
            var emailPort = 587;
            var emailSSL = true;

            //CreateMailSgqAppDeviation();
            //CreateMailSgqAppCorrectiveAction();

            using (var db = new SgqDbDevEntities())
                ListaDeMail = db.EmailContent.Where(r => r.SendStatus == null && r.Project == "SGQApp").ToList();

            if (ListaDeMail != null && ListaDeMail.Count() > 0)
                foreach (var i in ListaDeMail.Take(3).ToList())
                {
                    i.To = mailTo;
                    Task.Run(() => MailSender.SendMail(Mapper.Map<EmailContentDTO>(i), GlobalConfig.emailFrom, Guard.DecryptStringAES(GlobalConfig.emailPass), GlobalConfig.emailSmtp, GlobalConfig.emailPort, GlobalConfig.emailSSL, SendCompletedCallbackSgq, true));
                    //Task.Run(() => MailSender.SendMail(Mapper.Map<EmailContentDTO>(i), emailFrom, emailPass, emailSmtp, emailPort, emailSSL, SendCompletedCallbackSgq, true));
                }
        }

        #endregion

        #region Auxiliares

        /// <summary>
        /// Este callback preenche os campos AlterDate, SendDate, e SendStatus da tabela EmailContent, dos emails enviados, falhos, etc.... 
        /// TODO email que executar este callback em seua chamada exibirá algum resultado refletido nestes campos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Remove espaços desnecessários em partes do HTML salvos nas tabelas deviation ou corrective action.
        /// </summary>
        /// <param name="deviationMessage"></param>
        /// <returns></returns>
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

        #endregion

    }
}
