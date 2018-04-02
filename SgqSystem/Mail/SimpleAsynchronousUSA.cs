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

        /// <summary>
        /// Realiza Chamada para API de outro SERVIDOR
        /// </summary>
        public static void Mail()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var url = string.Empty;
                    if (GlobalConfig.Ambient.Equals(GlobalConfig.Ambiets.Desenvolvimento.ToString()))
                    {
                        //GlobalConfig.emailFrom = "grtservicos@hotmail.com";
                        //GlobalConfig.emailPass = Guard.EncryptStringAES("1qazmko0");
                        //GlobalConfig.emailSmtp = "smtp-mail.outlook.com";//"smtp.live.com";
                        //GlobalConfig.emailPort = 587;
                        //GlobalConfig.emailSSL = true;

                        GlobalConfig.emailFrom = "celsogea@hotmail.com";
                        GlobalConfig.emailPass = "tR48MJsfaz1Rf+dT+Ag8dQ==";
                        GlobalConfig.emailSmtp = "smtp.live.com";
                        GlobalConfig.emailPort = 587;
                        GlobalConfig.emailSSL = true;

                        url = "http://localhost/SgqSystem" + "/api/hf/SendMail";
                    }
                    else if (GlobalConfig.Ambient.Equals(GlobalConfig.Ambiets.DesenvolvimentoDeployServidorGrtParaTeste.ToString()))
                    {
                        //GlobalConfig.emailFrom = "grtservicos@hotmail.com";
                        //GlobalConfig.emailPass = Guard.EncryptStringAES("1qazmko0");
                        //GlobalConfig.emailSmtp = "smtp-mail.outlook.com";//"smtp.live.com";
                        //GlobalConfig.emailPort = 587;
                        //GlobalConfig.emailSSL = true;

                        GlobalConfig.emailFrom = "celsogea@hotmail.com";
                        GlobalConfig.emailPass = "tR48MJsfaz1Rf+dT+Ag8dQ==";
                        GlobalConfig.emailSmtp = "smtp.live.com";
                        GlobalConfig.emailPort = 587;
                        GlobalConfig.emailSSL = true;

                        url = "http://192.168.25.200/sgqusa" + "/api/hf/SendMail";
                    }
                    else
                    {
                        url = GlobalConfig.urlPreffixAppColleta + "/api/hf/SendMail";
                    }
                  
                    client.Timeout = TimeSpan.FromMinutes(2);
                    client.GetAsync(url).Result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// 1 - CRIA LISTA DE EMAIL NA EMAIL CONTENT COM TODOS OS CAMPOS, INCLUSIVE DESTINATÁRIOS, DEVIATION e CORRECTIVE ACTION
        /// 2 - ENVIA EMAILS NA EMAILCONTENT
        /// 3 - CALLBACK PREENCHE EMAIL CONTENT COM RESULTADO DO ENVIO
        /// </summary>
        public static void SendMailUSA()
        {
            //CRIA LISTA DE EMAIL NA EMAIL CONTENT COM TODOS OS CAMPOS
            //CreateMailSgqAppDeviationUSA(); //DEVIATION 
            CreateMailFromCorrectiveActionUSA(); //CORRECTIVE ACTION
            using (var db = new SgqDbDevEntities())//RECUPERA EMAILS NA EMAILCONTENT
                ListaDeMail = db.EmailContent.Where(r => r.SendStatus == null && r.Project == "SGQApp").ToList();

            if (ListaDeMail != null && ListaDeMail.Count() > 0)
                foreach (var i in ListaDeMail.Take(tamanhoDoPool).Distinct().ToList())
                {
                    Task.Run(() => MailSender.SendMail(Mapper.Map<EmailContentDTO>(i), GlobalConfig.emailFrom, Guard.DecryptStringAES(GlobalConfig.emailPass), GlobalConfig.emailSmtp, GlobalConfig.emailPort, GlobalConfig.emailSSL, SendCompletedCallbackSgq, true));//ENVIA EMAILS NA EMAILCONTENT
                }

        }

        /// <summary>
        /// 1 - CRIA LISTA DE EMAIL NA EMAIL CONTENT COM TODOS OS CAMPOS, INCLUSIVE DESTINATÁRIOS, DEVIATION e CORRECTIVE ACTION
        /// 2 - ENVIA EMAILS NA EMAILCONTENT
        /// 3 - CALLBACK PREENCHE EMAIL CONTENT COM RESULTADO DO ENVIO
        /// </summary>
        public static void SendMailUSA(string destinatario)
        {
            //CRIA LISTA DE EMAIL NA EMAIL CONTENT COM TODOS OS CAMPOS
            //CreateMailSgqAppDeviationUSA(); //DEVIATION 
            CreateMailFromCorrectiveActionUSA(); //CORRECTIVE ACTION
            using (var db = new SgqDbDevEntities())//RECUPERA EMAILS NA EMAILCONTENT
                ListaDeMail = db.EmailContent.Where(r => r.SendStatus == null && r.Project == "SGQApp").ToList();

            if (ListaDeMail != null && ListaDeMail.Count() > 0)
                foreach (var i in ListaDeMail.Take(tamanhoDoPool).ToList())
                {
                    //i.To = destinatario;
                    Task.Run(() => MailSender.SendMail(Mapper.Map<EmailContentDTO>(i), GlobalConfig.emailFrom, Guard.DecryptStringAES(GlobalConfig.emailPass), GlobalConfig.emailSmtp, GlobalConfig.emailPort, GlobalConfig.emailSSL, SendCompletedCallbackSgq, true));//ENVIA EMAILS NA EMAILCONTENT
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

                            var ultimoBodyEmailContent = "<div style='color:red'>"+db.Database.SqlQuery<string>(sqlSelecionaUltimaCorrectiveActionReferenteAEsta).FirstOrDefault()+"</div>";
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
                            newMail.To = DestinatariosSGQJBSUSAPorAlertaEAssinatura(newMail, ca, model);
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

        /// <summary>
        /// Envia para destinatários de acordo com documento na pasta desta classe
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="caEmail"></param>
        /// <returns></returns>
        private static string DestinatariosSGQJBSUSAPorAlertaEAssinatura(EmailContent mail, CorrectiveActionEmail caEmail, CorrectiveActionDTO modelCa)
        {

            using (var db = new SgqDbDevEntities())
            {
                var Roles = new List<string>();
                var listaEmails = new List<string>();
                var emailAssinatura = string.Empty;
                var listaUserComEmailNaoNulo = db.UserSgq.Where(r => r.Email != null && r.Email.Length > 0);
                var usersQueRecebemALertaNivelUm = listaUserComEmailNaoNulo.Where(r => r.Role.Contains("Alert1") && r.ParCompany_Id == caEmail.ParCompany_Id).ToList();
                var usersQueRecebemALertaNivelDois = listaUserComEmailNaoNulo.Where(r => r.Role.Contains("Alert2")).ToList();
                var usuarioQueAssinouSlaugther = listaUserComEmailNaoNulo.FirstOrDefault(r => r.Id == modelCa.SlaughterId);
                var usuarioQueAssinouTechinical = listaUserComEmailNaoNulo.FirstOrDefault(r => r.Id == modelCa.TechinicalId);

                if (caEmail.AlertNumber == 1)
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

                if(usuarioQueAssinouSlaugther != null)
                    listaEmails.Add(usuarioQueAssinouSlaugther.Email);
                if (usuarioQueAssinouTechinical != null)
                    listaEmails.Add(usuarioQueAssinouTechinical.Email);

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

                return result.Substring(0, result.IndexOf("<button"));
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
