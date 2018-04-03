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
    /// <summary>
    /// Classe de serviços asyncronos, utilizada principalmnente pela instancia do HANGFIRE do SGQ
    /// </summary>
    public class SimpleAsynchronous
    {

        public static List<EmailContent> ListaDeMail;
        private static int tamanhoDoPool = 1000;
        private static bool running { get; set; }

        #region SGQ Email

        /// <summary>
        /// Realiza Chamada para API de outro SERVIDOR
        /// </summary>
        public static void Mail()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var url = System.Configuration.ConfigurationManager.AppSettings["EnderecoEmailAlertaBR"];
                    
                    client.Timeout = TimeSpan.FromMinutes(10);
                    client.GetAsync(url).Result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                
            }
        }

        /// <summary>
        /// Realiza Chamada para API de outro SERVIDOR
        /// </summary>
        public static void Reconsolidacao()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var url = GlobalConfig.urlPreffixAppColleta + "/api/hf/SendMail";
                    client.Timeout = TimeSpan.FromMinutes(10);
                    client.GetAsync(url).Result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Controle de chamadas para envio de email SGQ utilizando os email que estão na tabela EmailContent: (r => r.SendStatus == null && r.Project == "SGQApp"),
        /// utiliza callback e configs da tabela SgqConfig
        /// </summary>
        public static void SendMailFromDeviationSgqApp()
        {
            try
            {
                CreateMailSgqAppDeviation();
                //CreateMailSgqAppCorrectiveAction();
                using (var db = new SgqDbDevEntities())
                    ListaDeMail = db.EmailContent.Where(r => r.SendStatus == null && r.Project == "SGQApp").ToList();

                if (ListaDeMail != null && ListaDeMail.Count() > 0)                   
                    foreach (var i in ListaDeMail.Distinct().ToList())
                        Task.Run(() => MailSender.SendMail(Mapper.Map<EmailContentDTO>(i), GlobalConfig.emailFrom, GlobalConfig.emailPass, GlobalConfig.emailSmtp, GlobalConfig.emailPort, GlobalConfig.emailSSL, SendCompletedCallbackSgq, true));
            }
            catch (Exception ex)
            {
                new CreateLog(new Exception("Erro no metodo [SendMailFromDeviationSgqApp]", ex));
                throw ex;
            }
        }

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
        /// Cria lista de emails na tabela EmailContent, a partir da tabela !!!DEVIATION!!!, TUDO que esta na EmailContent é enviado atravez do SGQ pelo send mail.
        /// Os destinatários devem ser preenchidos neste método, e isneridos na tabela EmailContent corretamente. Devem ser separados por VIRGULA caso exista mais de um,
        /// EX com 2 destinatários: EmailContent.To = "email1@teste.com, email2@teste.com"
        /// EX com 1 destinatário: EmailContent.To = "email1@teste.com"
        /// </summary>
        public static void CreateMailSgqAppDeviation()
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
                            EmailContent newMail = GetMailByDeviation(db, m, m.AlertNumber);
                            newMail.To = DestinatariosSGQJBSBR(newMail, m.AlertNumber, m.ParCompany_Id);
                            db.EmailContent.Add(newMail);
                            db.SaveChanges();

                            db.Database.ExecuteSqlCommand($"UPDATE Deviation SET sendMail = 1, EmailContent_Id = { newMail.Id } WHERE ID = { m.Id }");
                            db.SaveChanges();
                        }

                    }
                }
                catch (Exception e)
                {
                    new CreateLog(new Exception("Ocorreu um erro em: [CreateMailSgqAppDeviation]", e));
                }

                //return db.EmailContent.Where(r => r.SendStatus == null && r.Project == "SGQApp").ToList();
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
        private static EmailContent GetMailByDeviation(SgqDbDevEntities db, Deviation m, int alertNumber)
        {
            var body = Uri.UnescapeDataString(m.DeviationMessage).ToString().Replace("O Supervisor da área será notificado Ok ", "").Replace("O Supervisor, o Gerente e o Diretor da área serão notificados Ok ", "").Replace("O Supervisor e o Gerente da área serão notificados. Ok", "").Replace(" Ok", "");
            var parLevel1 = db.ParLevel1.FirstOrDefault(r => r.Id == m.ParLevel1_Id).Name;
            var parLevel2 = db.ParLevel2.FirstOrDefault(r => r.Id == m.ParLevel2_Id).Name;
            string company = string.Empty;

            if (m.ParCompany_Id > 0)
                company = db.ParCompany.FirstOrDefault(r => r.Id == m.ParCompany_Id).Name;
            else
                company = "Corporativo";

            var subject = "Alerta emitido para o Indicador: " + parLevel1 + ", Monitoramento: " + parLevel2 + " da Unidade: " + company;

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
                var model = new CorrectActApiController().GetCorrectiveActionById(valor2);
                ultimoBodyEmailContent = "<br><br><div style='color:#333'>" + model.EmailBodyCorrectiveAction + "</div><br><br>";
            }
            ultimoBodyEmailContent += "<div style='color:red'>" + valor1 + "</div>";
            #endregion

            var newMail = new EmailContent()
            {
                AddDate = DateTime.Now,
                Body = m.DeviationDate.ToShortDateString() + " " + m.DeviationDate.ToShortTimeString() + ": " + subject + "<br><br>" + RemoveEspacos(body) + "<br/><br/>" + ultimoBodyEmailContent,
                IsBodyHtml = true,
                Subject = subject,
                Project = "SGQApp"
            };

            //if (alertNumber > 1)
            //{
            //    var alerta = m.AlertNumber - 1;
            //    var deviationAnterior = db.Deviation.Where(r => r.AlertNumber == alerta && r.ParCompany_Id == m.ParCompany_Id && r.ParLevel1_Id == m.ParLevel1_Id && r.DeviationMessage != null).OrderByDescending(r => r.DeviationDate).FirstOrDefault();
            //    if (deviationAnterior != null)
            //    {
            //        newMail.Body += "<hr><br> Alerta Anterior: <br><br>";
            //        newMail.Body += GetMailByDeviation(db, deviationAnterior, alerta).Body;
            //    }
            //}

            return newMail;
        }

        /// <summary>
        /// 1.1 - Os destinatários: Pela tabela DesvioNiveis, é possível encontrar as ROLES referentes aos níveis de desvio da JBS.
        /// 1.1.1 - Alerta de Nível 1: Supervisores da Unidade
        /// 1.1.2 - Alerta de Nível 2: Supervisores e Gerentes da Unidade
        /// 1.1.3 - Alerta de Nível 3: Supervisores e Gerentes da Unidade e Diretor
        /// 1.1.4 - Alertas de nivel 4 acima enviam para Nivel 2 da tabela DesvioNiveis
        /// </summary>
        /// <param name="m"></param>
        /// <param name="nivel"></param>
        /// <param name="companyId"></param>
        private static string DestinatariosSGQJBSBR(EmailContent m, int nivel, int companyId)
        {
            using (var dbLegado = new SgqDbDevEntities())
            {
                //Alertas de nivel 4 acima enviam para Nivel 2 da tabela DesvioNiveis
                if (nivel > 3)
                    nivel = 2;

                //var query = "\n SELECT Email FROM UserSgq" +
                //            "\n WHERE Id in (SELECT UserSgq_Id FROM ParCompanyXUserSgq WHERE ParCompany_Id = " + companyId + " AND [Role] IN (SELECT Nivel FROM desvioNiveis WHERE Desvio = " + nivel + "))" +
                //            "\n  AND Email IS NOT NULL" +
                //            "\n  AND Email <> ''";

                var query = @"SELECT U.Email FROM UserSgq U
                            INNER JOIN ParCompanyXUserSgq UU
                            ON UU.UserSgq_Id = U.id
                            INNER JOIN ParCompanyXStructure UniReg
                            ON UU.ParCompany_Id = UniReg.ParCompany_Id
                            INNER JOIN ParStructure Reg
                            ON UniReg.ParStructure_Id = Reg.Id
                            WHERE
                            (UU.ParCompany_Id = " + companyId + @" AND UU.Role in (SELECT Nivel FROM desvioNiveis WHERE Desvio = " + nivel + @"))
                            AND U.Email IS NOT NULL
                            AND U.Email <> ''
                            AND Reg.Id =
                            CASE --forçar regional para diretores

                                WHEN U.ID = 1002 THEN 5

                                WHEN U.ID = 1003 THEN 3

                                WHEN U.ID = 1004 THEN 4

                                WHEN U.ID = 1872 THEN 2

                            ELSE(SELECT ParStructure_Id FROM ParCompanyXStructure where ParCompany_Id = " + companyId + @") END
                            AND U.Id NOT IN (543,546,511)"; //Tirar Célia e Mariana da JBS

                var listaEmails = dbLegado.Database.SqlQuery<string>(query);
                if (listaEmails != null && listaEmails.Count() > 0)
                {
                    return string.Join(",", listaEmails.ToArray());
                }
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
        /// Cria lista de emails na tabela EmailContent, a partir da tabela !!!CorrectiveAction!!!, TUDO que esta na EmailContent é enviado atravez do SGQ pelo send mail.
        /// Os destinatários devem ser preenchidos neste método, e isneridos na tabela EmailContent corretamente. Devem ser separados por VIRGULA caso exista mais de um,
        /// EX com 2 destinatários: EmailContent.To = "email1@teste.com, email2@teste.com"
        /// EX com 1 destinatário: EmailContent.To = "email1@teste.com"
        /// </summary>
        public static void CreateMailSgqAppCorrectiveAction()
        {
            try
            {
                using (var db = new SgqDbDevEntities())
                {
                    using (var controller = new CorrectActApiController())
                    {
                        var listaCorrectiveActionDb = new List<CorrectiveAction>();
                        using (Factory factory = new Factory("DefaultConnection"))
                        {
                            listaCorrectiveActionDb = factory.SearchQuery<CorrectiveAction>("SELECT * FROM CorrectiveAction WHERE MailProcessed = 0");
                        }

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
                            newMail.Body = subject + "<br><br>" + model.EmailBodyCorrectiveAction;
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
        /// Metodo para testes de rotinas de email, pode ser chamado do GlobalConfig/Config
        /// </summary>
        /// <param name="mailTo"></param>
        /// <param name="deviation"></param>
        public static void SendMailFromDeviationSgqAppTesteBR(string mailTo, bool deviation)
        {
            var emailFrom = "celsogea@hotmail.com";
            var emailPass = "tR48MJsfaz1Rf+dT+Ag8dQ==";
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
                    Task.Run(() => MailSender.SendMail(Mapper.Map<EmailContentDTO>(i), GlobalConfig.emailFrom, GlobalConfig.emailPass, GlobalConfig.emailSmtp, GlobalConfig.emailPort, GlobalConfig.emailSSL, SendCompletedCallbackSgq, true));
                    //Task.Run(() => MailSender.SendMail(Mapper.Map<EmailContentDTO>(i), emailFrom, Guard.DecryptStringAES(emailPass), emailSmtp, emailPort, emailSSL, SendCompletedCallbackSgq, true));
                }
        }

      

        #region ResendProcessJson

        /// <summary>
        /// Reenvia a solicitação para reprocesamento do Json salvo pela coleta de TODOS os registros aonde CollectionJson.Where(r => !r.IsProcessed)
        /// </summary>
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

        #region Auxiliares

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