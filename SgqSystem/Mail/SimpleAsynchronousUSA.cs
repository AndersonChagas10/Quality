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
using ADOFactory;
using SgqSystem.Helpers;

namespace SgqSystem.Mail
{
    public static class IntegerExtensions
    {
        public static string DisplayWithSuffix(this int num)
        {
            if (num.ToString().EndsWith("11")) return num.ToString() + "th";
            if (num.ToString().EndsWith("12")) return num.ToString() + "th";
            if (num.ToString().EndsWith("13")) return num.ToString() + "th";
            if (num.ToString().EndsWith("1")) return num.ToString() + "st";
            if (num.ToString().EndsWith("2")) return num.ToString() + "nd";
            if (num.ToString().EndsWith("3")) return num.ToString() + "rd";
            return num.ToString() + "th";
        }
    }

    /// <summary>
    /// Classe de serviços asyncronos, utilizada principalmnente pela instancia do HANGFIRE do SGQ
    /// </summary>
    public class SimpleAsynchronousUSA
    {

        public static List<EmailContent> ListaDeMail;
        private static int tamanhoDoPool = 1000;
        private static bool running { get; set; }

        #region SGQ USA Email

        #region Regras e funções para enviar email

        public static void HandleErrorMethod(string error, int emailId)
        {
            using (var db = new SgqDbDevEntities())
            {
                try
                {
                    var emailContent = db.EmailContent.Find(emailId);
                    if (emailContent != null)
                    {

                        if (string.IsNullOrEmpty(emailContent.To))
                        {
                            emailContent.To = "-";
                        }

                        emailContent.SendStatus = "Erro: " + error.Substring(0, error.Length > 500 ? 500 : error.Length);
                        emailContent.AlterDate = DateTime.Now;
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        /// <summary>
        /// 1 - CRIA LISTA DE EMAIL NA EMAIL CONTENT COM TODOS OS CAMPOS, INCLUSIVE DESTINATÁRIOS, DEVIATION e CORRECTIVE ACTION
        /// 2 - ENVIA EMAILS NA EMAILCONTENT
        /// 3 - CALLBACK PREENCHE EMAIL CONTENT COM RESULTADO DO ENVIO
        /// </summary>
        public static void SendMailUSA()
        {
            try
            {
                using (var db = new SgqDbDevEntities())
                {
                    var listDeviation = db.Deviation.AsNoTracking().Where(r => r.EmailContent_Id != null).OrderByDescending(r => r.AddDate).Take(200).Select(r => r.EmailContent_Id).ToList();
                    var listCorrectiveAction = db.CorrectiveAction.AsNoTracking().Where(r => r.EmailContent_Id != null).OrderByDescending(r => r.AddDate).Take(200).Select(r => r.EmailContent_Id).ToList();
                    ListaDeMail = db.EmailContent.AsNoTracking().Where(r => r.SendDate == null && r.Project == "SGQApp" && (listDeviation.Contains(r.Id) || listCorrectiveAction.Contains(r.Id))).Take(tamanhoDoPool).ToList();

                    MailSender.HandleError HandleErrorDelegate = HandleErrorMethod;
                    if (ListaDeMail != null && ListaDeMail.Count() > 0)
                    {
                        foreach (var i in ListaDeMail.Distinct().ToList())
                        {
                            var email = db.EmailContent.FirstOrDefault(p => p.Id == i.Id && (p.SendStatus != "Tentando Enviar" && p.SendDate == null));
                            if (email != null)
                            {
                                Task.Run(() => MailSender.SendMail(Mapper.Map<EmailContentDTO>(email), GlobalConfig.emailFrom, GlobalConfig.emailPass, GlobalConfig.emailSmtp, GlobalConfig.emailPort, GlobalConfig.emailSSL, SendCompletedCallbackSgq, true, HandleErrorDelegate));
                                //var emailContent = db.EmailContent.Find(i.Id);
                                email.SendStatus = "Sending...";
                                email.AlterDate = DateTime.Now;
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new CreateLog(new Exception("Erro no metodo [SendEmailUSA]", ex));
                //throw ex;
            }

        }

        /// <summary>
        /// Cria lista de emails na tabela EmailContent, a partir da tabela !!!CorrectiveAction!!!, TUDO que esta na EmailContent é enviado atravez do SGQ pelo send mail.
        /// Os destinatários devem ser preenchidos neste método, e isneridos na tabela EmailContent corretamente. Devem ser separados por VIRGULA caso exista mais de um,
        /// EX com 2 destinatários: EmailContent.To = "email1@teste.com, email2@teste.com"
        /// EX com 1 destinatário: EmailContent.To = "email1@teste.com"
        /// </summary>
        public static void CreateMailFromCorrectiveActionUSA()
        {
            try
            {
                using (var db = new SgqDbDevEntities())
                {
                    using (var controller = new CorrectActApiController())
                    {
                        var sql = "SELECT TOP 1000 CA.*,                                                                                                                        "
+ " \n ParLevel1_Id,                                                                                                                                                            "
//+ " \n Period,                                                                                                                                                                  "
+ " \n ParCompany_Id                                                                                                                                                            "
+ " \n FROM                                                                                                                                                                     "
+ " \n     (                                                                                                                                                                    "
+ " \n                                                                                                                                                                          "
+ " \n         SELECT                                                                                                                                                           "
+ " \n                                                                                                                                                                          "
+ " \n         ca.id,                                                                                                                                                           "
+ " \n         CAST(cl2.CollectionDate AS Date) CollectionDate,                                                                                                                 "
+ " \n         cl2.UnitId as ParCompany_Id,                                                                                                                                     "
+ " \n         cl2.ParLevel1_Id as ParLevel1_Id,                                                                                                                                "
+ " \n         cl2.[shift] as Shift,                                                                                                                                            "
+ " \n         cl2.period as Period,                                                                                                                                            "
+ " \n         cl2.ReauditNumber                                                                                                                                                "
+ " \n         --COUNT(1) AS AlertLevel                                                                                                                                         "
+ " \n                                                                                                                                                                          "
+ " \n         FROM                                                                                                                                                             "
+ " \n                                                                                                                                                                          "
+ " \n         CollectionLevel2 cl2                                                                                                                                             "
+ " \n                                                                                                                                                                          "
+ " \n         INNER JOIN correctiveaction ca ON cl2.Id = ca.CollectionLevel02Id                                                                                                "
+ " \n         --INNER JOIN deviation dev ON dev.Parlevel1_Id = cl2.Parlevel1_Id and dev.alertnumber > 0 AND CAST(dev.DeviationDate AS Date) = CAST(cl2.CollectionDate AS Date) "
+ " \n                                                                                                                                                                          "
+ " \n         WHERE ca.MailProcessed = 0                                                                                                                                       "
+ " \n                                                                                                                                                                          "
+ " \n         AND CAST(GETDATE() AS Date) = CAST(cl2.CollectionDate AS Date)                                                                                                   "
+ " \n                                                                                                                                                                          "
+ " \n         GROUP BY                                                                                                                                                         "
+ " \n                                                                                                                                                                          "
+ " \n         ca.id,                                                                                                                                                           "
+ " \n         cl2.CollectionDate,                                                                                                                                              "
+ " \n         cl2.UnitId,                                                                                                                                                      "
+ " \n         cl2.ParLevel1_Id,                                                                                                                                                "
+ " \n         cl2.[shift],                                                                                                                                                     "
+ " \n         cl2.period,                                                                                                                                                      "
+ " \n         cl2.ReauditNumber                                                                                                                                                "
+ " \n                                                                                                                                                                          "
+ " \n     ) vaiLaJesus                                                                                                                                                         "
+ " \n INNER JOIN correctiveaction CA ON CA.Id = vaiLaJesus.Id                                                                                                                  "
+ " \n order by 1 ASC";

                        var listaCorrectiveActionDb = new List<CorrectiveActionEmail>();
                        using (Factory factory = new Factory("DefaultConnection"))
                        {
                            listaCorrectiveActionDb = factory.SearchQuery<CorrectiveActionEmail>(sql).ToList();
                        }

                        foreach (var ca in listaCorrectiveActionDb)
                        {

                            var alertNivelQuery = string.Format("\n SELECT              "
                               + "\n COUNT(*) AS AlertLevel                                                "
                               + "\n FROM                                                                  "
                               + "\n CollectionLevel2 cl2                                                  "
                               + "\n INNER JOIN correctiveaction ca ON cl2.Id = ca.CollectionLevel02Id     "
                               + "\n WHERE--ca.MailProcessed = 0                                           "
                               + "\n CAST(GETDATE() AS Date) = CAST(cl2.CollectionDate AS Date)            "
                               + "\n AND cl2.ParLevel1_Id = {0}                                             "
                               + "\n AND cl2.UnitId = {1}                                                   "
                               + "\n AND ca.Id <= {2}", ca.ParLevel1_Id, ca.ParCompany_Id, ca.Id);

                            ca.AlertNumber = db.Database.SqlQuery<int>(alertNivelQuery).FirstOrDefault();

                            #region Captura ultimo body do email content enviado
                            var sqlSelecionaUltimaCorrectiveActionReferenteAEsta =
                                $@"SELECT  top 1 ec.Body   FROM  CollectionLevel2 cl2                                                 
                                    INNER JOIN correctiveaction ca ON cl2.Id = ca.CollectionLevel02Id    
                                    INNER JOIN deviation d ON d.ParLevel1_Id = cl2.ParLevel1_Id AND   d.ParCompany_Id = cl2.UnitId
                                    INNER JOIN EmailContent ec ON ec.Id = ca.EmailContent_Id   
                                    WHERE                                         
                                    CAST(GETDATE() AS Date) = CAST(cl2.CollectionDate AS Date)        
                                    AND cl2.ParLevel1_Id = { ca.ParLevel1_Id }                                           
                                    AND cl2.UnitId = { ca.ParCompany_Id }
                                    AND ca.Id <= { ca.Id }
                                    order by ec.id desc";

                            var ultimoBodyEmailContent = "<div style='color:red'>" + db.Database.SqlQuery<string>(sqlSelecionaUltimaCorrectiveActionReferenteAEsta).FirstOrDefault() + "</div>";
                            #endregion

                            var colectionLevel2 = db.CollectionLevel2.FirstOrDefault(r => r.Id == ca.CollectionLevel02Id);
                            var parLevel1 = db.ParLevel1.FirstOrDefault(r => r.Id == colectionLevel2.ParLevel1_Id).Name;
                            var parLevel2 = db.ParLevel2.FirstOrDefault(r => r.Id == colectionLevel2.ParLevel2_Id).Name;
                            string company = string.Empty;

                            if (colectionLevel2.UnitId > 0)
                                company = db.ParCompany.FirstOrDefault(r => r.Id == colectionLevel2.UnitId).Name;
                            else
                                company = "All";

                            //"unidade" - "nome do indicador": 1st failure of the day
                            var subject = string.Format("SGQ Corretive Action: Plant - {0}: {1}: {2} failure of the day.", company, parLevel1, ca.AlertNumber.DisplayWithSuffix());

                            var newMail = new EmailContent()
                            {
                                AddDate = DateTime.Now,
                                IsBodyHtml = true,
                                Subject = subject,
                                Project = "SGQApp"
                            };

                            var model = controller.GetCorrectiveActionById(ca.Id);
                            newMail.Body = subject + "<br><br>" + model.EmailBodyCorrectiveAction + "<br><br>" + ultimoBodyEmailContent;
                            newMail.To = DestinatariosSGQJBSUSAPorAlertaEAssinatura(newMail, ca.ParCompany_Id, ca.AlertNumber, model);
                            db.EmailContent.Add(newMail);
                            db.SaveChanges();

                            db.Database.ExecuteSqlCommand($"UPDATE CorrectiveAction SET MailProcessed = 1, EmailContent_Id = { newMail.Id } WHERE Id = { ca.Id }");
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

        public static void CreateMailSgqAppDeviationUSA()
        {
            using (var db = new SgqDbDevEntities())
            {
                try
                {
                    //db.Configuration.ValidateOnSaveEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;

                    /*Cria Novos Emails de acordo com a quantidade do pool na emailContent*/
                    DateTime dateLimit = DateTime.Now.AddHours(-24);
                    DateTime dateLimitDeviation = DateTime.Now.AddHours(-72);
                    var listDeviations = db.Deviation.Where(r => r.AlertNumber > 0 && (r.sendMail == null || r.sendMail == false) && r.DeviationMessage != null && r.DeviationDate > dateLimitDeviation && r.AddDate > dateLimit).OrderBy(r => r.AddDate).Take(tamanhoDoPool).ToList();

                    if (listDeviations != null && listDeviations.Count() > 0)
                    {
                        foreach (var deviation in listDeviations)
                        {
                            if (!(db.Deviation.FirstOrDefault(d => d.Id == deviation.Id)?.EmailContent_Id > 0))
                            {
                                CorrectiveActionDTO caDTO;
                                EmailContent newMail = GetMailByDeviationUSA(db, deviation, deviation.AlertNumber, out caDTO);
                                newMail.To = DestinatariosSGQJBSUSAPorRegraDeUsuarioPorDepartamentoDoLevel2(newMail, deviation, caDTO);
                                newMail.To = newMail.To.Substring(0, newMail.To.Length > 500 ? 500 : newMail.To.Length);
                                db.EmailContent.Add(newMail);
                                db.SaveChanges();

                                db.Database.ExecuteSqlCommand($"UPDATE Deviation SET sendMail = 1, EmailContent_Id = { newMail.Id } WHERE ID = { deviation.Id }");
                                db.SaveChanges();
                            }
                        }

                    }
                }
                catch (DbEntityValidationException e)
                {
                    var erro = "";
                    foreach (var error in e.EntityValidationErrors.FirstOrDefault().ValidationErrors)
                    {
                        erro += error.PropertyName + ": " + error.ErrorMessage + " ";
                    }

                    new CreateLog(new Exception($"Ocorreu um erro em: [CreateMailSgqAppDeviationUSA] --- {erro} ---", e));
                }
                catch (Exception e)
                {
                    new CreateLog(new Exception("Ocorreu um erro em: [CreateMailSgqAppDeviationUSA] - " + e.ToClient(), e));
                }
            }

        }

        private static EmailContent GetMailByDeviationUSA(SgqDbDevEntities db, Deviation m, int alertNumber, out CorrectiveActionDTO caDTO)
        {
            caDTO = null;

            var body = Uri.UnescapeDataString(m.DeviationMessage)?.ToString()?.Replace("O Supervisor da área será notificado Ok ", "").Replace("O Supervisor, o Gerente e o Diretor da área serão notificados Ok ", "").Replace("O Supervisor e o Gerente da área serão notificados. Ok", "").Replace(" Ok", "");

            var quebraProcesso = "98789";
            m.ParLevel1_Id = m.ParLevel1_Id.ToString().Contains(quebraProcesso) ? Convert.ToInt32(m.ParLevel1_Id.ToString().Replace(quebraProcesso, "|").Split('|')[1]) : m.ParLevel1_Id;
            var parLevel1 = db.ParLevel1.FirstOrDefault(r => r.Id == m.ParLevel1_Id).Name;

            m.ParLevel2_Id = m.ParLevel2_Id.ToString().Contains(quebraProcesso) ? Convert.ToInt32(m.ParLevel2_Id.ToString().Replace(quebraProcesso, "|").Split('|')[1]) : m.ParLevel2_Id;
            var parLevel2 = db.ParLevel2.FirstOrDefault(r => r.Id == m.ParLevel2_Id).Name;
            string company = string.Empty;

            if (m.ParCompany_Id > 0)
                company = db.ParCompany.FirstOrDefault(r => r.Id == m.ParCompany_Id).Name;
            else
                company = "All";

            var subject = "Alert to Indicator: " + parLevel1 + ", Monitoring: " + parLevel2 + " on Company: " + company;

            #region Captura ultimo body do email content enviado
            var sqlSelecionaUltimaCorrectiveActionReferenteAEsta1 =
                $@"select top 1 ec.Body from deviation d
                                INNER JOIN EmailContent ec ON ec.Id = d.EmailContent_Id   
                                WHERE
                                CAST(GETDATE() AS Date) = CAST(d.DeviationDate AS Date)    
                                and ParLevel1_Id = { m.ParLevel1_Id }                                           
                                AND d.ParCompany_Id = { m.ParCompany_Id }  
                                and d.alertnumber > 0
                                order by d.alertnumber desc";


            var sqlSelecionaUltimaCorrectiveActionReferenteAEsta2 =
                $@"
                SELECT  top 1 ca.id   FROM  CollectionLevel2 cl2                                                 
                INNER JOIN correctiveaction ca ON cl2.Id = ca.CollectionLevel02Id    
                INNER JOIN deviation d ON d.ParLevel1_Id = cl2.ParLevel1_Id AND d.ParCompany_Id = cl2.UnitId AND d.ParLevel2_Id = cl2.ParLevel2_Id AND d.Evaluation = cl2.EvaluationNumber AND d.Sample = cl2.Sample AND d.DeviationDate = cl2.CollectionDate
                INNER JOIN EmailContent ec ON ec.Id = d.EmailContent_Id   
                WHERE                                         
                CAST(GETDATE() AS Date) = CAST(cl2.CollectionDate AS Date)          
                AND cl2.ParLevel1_Id = { m.ParLevel1_Id }                                           
                AND cl2.UnitId = { m.ParCompany_Id }
                AND ca.Id <= { m.Id }
                order by ec.id desc";

            var valor1 = db.Database.SqlQuery<string>(sqlSelecionaUltimaCorrectiveActionReferenteAEsta1).FirstOrDefault();
            var valor2 = db.Database.SqlQuery<int>(sqlSelecionaUltimaCorrectiveActionReferenteAEsta2).FirstOrDefault();

            //var ultimoBodyEmailContent = "<div style='color:red'>" + db.Database.SqlQuery<string>(sqlSelecionaUltimaCorrectiveActionReferenteAEsta).FirstOrDefault() + "</div>";
            #endregion

            var ultimoBodyEmailContent = "";
            if (valor2 > 0)
            {
                caDTO = new CorrectActApiController().GetCorrectiveActionById(valor2);
                ultimoBodyEmailContent = "<br><br><div style='color:#222'>" + caDTO.EmailBodyCorrectiveAction + "</div><br><br>";
            }
            ultimoBodyEmailContent += "<div style='color:#666'>" + valor1 + "</div>";
            #endregion

            var newMail = new EmailContent()
            {
                AddDate = DateTime.Now,
                Body = $"<span style='color:#000'>{ m.DeviationDate.ToShortDateString() } {m.DeviationDate.ToShortTimeString()}: { subject }<br><br>{ RemoveEspacos(body) }<span><br/><br/>{ ultimoBodyEmailContent}",
                IsBodyHtml = true,
                Subject = subject,
                Project = "SGQApp"
            };

            return newMail;
        }

        /// <summary>
        /// Envia para destinatários de acordo com documento na pasta desta classe
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="caEmail"></param>
        /// <returns></returns>
        private static string DestinatariosSGQJBSUSAPorAlertaEAssinatura(EmailContent mail, int parCompany_Id, int alertNumber, CorrectiveActionDTO modelCa)
        {

            using (var db = new SgqDbDevEntities())
            {
                var Roles = new List<string>();
                var listaEmails = new List<string>();
                var emailAssinatura = string.Empty;
                var listaUserComEmailNaoNulo = db.UserSgq.Where(r => r.Email != null && r.Email.Length > 0);
                var usersQueRecebemALertaNivelUm = listaUserComEmailNaoNulo.Where(r => r.Role.Contains("Alert1") && r.ParCompany_Id == parCompany_Id).ToList();
                var usersQueRecebemALertaNivelDois = listaUserComEmailNaoNulo.Where(r => r.Role.Contains("Alert2")).ToList();

                if (modelCa != null)
                {
                    var usuarioQueAssinouSlaugther = listaUserComEmailNaoNulo.FirstOrDefault(r => r.Id == modelCa.SlaughterId);
                    var usuarioQueAssinouTechinical = listaUserComEmailNaoNulo.FirstOrDefault(r => r.Id == modelCa.TechinicalId);

                    if (usuarioQueAssinouSlaugther != null)
                        listaEmails.Add(usuarioQueAssinouSlaugther.Email);
                    if (usuarioQueAssinouTechinical != null)
                        listaEmails.Add(usuarioQueAssinouTechinical.Email);
                }

                if (alertNumber == 1)
                {
                    foreach (var usuario in usersQueRecebemALertaNivelUm)
                        listaEmails.Add(usuario.Email);
                }
                else
                {
                    foreach (var usuario in usersQueRecebemALertaNivelUm)
                        listaEmails.Add(usuario.Email);
                    foreach (var usuario in usersQueRecebemALertaNivelDois)
                        listaEmails.Add(usuario.Email);
                }

                if (listaEmails != null && listaEmails.Count() > 0)
                    return string.Join(",", listaEmails.Distinct().ToArray());
                else
                {
                    mail.SendStatus = "Não existem destinatários para este email.";
                    return string.Empty;
                }
            }
        }

        private static string DestinatariosSGQJBSUSAPorRegraDeUsuarioPorDepartamentoDoLevel2(EmailContent mail, Deviation deviation, CorrectiveActionDTO modelCa)
        {

            using (var db = new SgqDbDevEntities())
            {
                var Roles = new List<string>();
                var listaEmails = new List<string>();
                var emailAssinatura = string.Empty;
                var listaUserComEmailNaoNulo = db.UserSgq.Where(r => r.Email != null && r.Email.Length > 0);

                var departmentLevel02 = db.ParLevel2.Where(x => x.Id == deviation.ParLevel2_Id).Select(x => x.ParDepartment.Name).FirstOrDefault();

                if (!string.IsNullOrEmpty(departmentLevel02))
                {
                    var usuariosComRegraIgualAoDepartamento = listaUserComEmailNaoNulo.Where(r => r.Role.Contains(departmentLevel02) && r.ParCompany_Id == deviation.ParCompany_Id).ToList();

                    foreach (var usuario in usuariosComRegraIgualAoDepartamento)
                        listaEmails.Add(usuario.Email);
                }

                if (modelCa != null)
                {
                    var usuarioQueAssinouSlaugther = listaUserComEmailNaoNulo.FirstOrDefault(r => r.Id == modelCa.SlaughterId);
                    var usuarioQueAssinouTechinical = listaUserComEmailNaoNulo.FirstOrDefault(r => r.Id == modelCa.TechinicalId);

                    if (usuarioQueAssinouSlaugther != null)
                        listaEmails.Add(usuarioQueAssinouSlaugther.Email);
                    if (usuarioQueAssinouTechinical != null)
                        listaEmails.Add(usuarioQueAssinouTechinical.Email);
                }

                if (listaEmails != null && listaEmails.Count() > 0)
                    return string.Join(",", listaEmails.Distinct().ToArray());
                else
                {
                    mail.SendStatus = "Não existem destinatários para este email.";
                    return string.Empty;
                }
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

                var indexOfButton = result.IndexOf("<button");
                if (indexOfButton >= 0) {
                    return result.Substring(0, indexOfButton);
                }
                else
                {
                    return result;
                }
            }
            catch (Exception e)
            {
                new CreateLog(e);
                return deviationMessage;
            }
        }

        #endregion

        public class CorrectiveActionEmail : CorrectiveAction
        {
            public int AlertNumber { get; set; }
            public int ParLevel1_Id { get; set; }
            public int ParCompany_Id { get; set; }
            //public int Period { get; set; }
            //public int Shift { get; set; }
            //public int ReauditNumber { get; set; }
            public DateTime CollectionDate { get; set; }
        }
    }

}
