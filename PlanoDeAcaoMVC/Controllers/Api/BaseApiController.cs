using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using DTO.DTO;
using PlanoAcaoCore;
using ADOFactory;
using DTO.Helpers;
using PlanoDeAcaoMVC.PaMail;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    public class BaseApiController : ApiController
    {

        /// <summary>
        /// Retorna Objeto Dinamico com dados da query.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected List<JObject> QueryNinja(DbContext db, string query)
        {
            db.Database.Connection.Open();
            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = query;
            var reader = cmd.ExecuteReader();
            List<JObject> items = new List<JObject>();
            while (reader.Read())
            {
                JObject row = new JObject();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader[i].ToString();

                items.Add(row);
            }
            db.Database.Connection.Close();
            return items;
        }

        /// <summary>
        /// Realiza Chamada para API de outro SERVIDOR
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        protected async Task<string> GetExternalResponse(string url)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(2);
                var response = await client.GetAsync(url).Result.Content.ReadAsStringAsync();
                return response;
            }
        }


        /// <summary>
        /// Executa 1 batch pelo caminho indicado, o arquivo deve ter permissão para o usuario do IIS
        /// </summary>
        /// <param name="path"></param>
        protected void ExecuteBatch(string path)
        {
            var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + path);
            var proc = new System.Diagnostics.Process();

            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = false;
            proc.StartInfo.FileName = mappedPath;
            //proc.StartInfo.RedirectStandardError = true;
            //proc.StartInfo.RedirectStandardOutput = true;
            //proc.StartInfo.WorkingDirectory = "C:\\Watcher";
            proc.Start();
            proc.WaitForExit();
        }

        /// <summary>
        /// Cria Cookie para UserSgq
        /// </summary>
        /// <param name="userDto">UserDTO</param>
        /// <returns></returns>
        protected CookieHeaderValue CreateCookieFromUserDTO(UserDTO userDto)
        {
            var values = new NameValueCollection();
            values.Add("userId", userDto.Id.ToString());
            values.Add("userName", userDto.Name);
            values.Add("CompanyId", userDto.ParCompany_Id.GetValueOrDefault().ToString());
            if (userDto.AlterDate != null)
                values.Add("alterDate", userDto.AlterDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            else
                values.Add("alterDate", "");

            values.Add("addDate", userDto.AddDate.ToString("dd/MM/yyyy"));

            if (userDto.Role != null)
                values.Add("roles", userDto.Role.Replace(';', ',').ToString());//"admin, teste, operacional, 3666,344, 43434,...."
            else
                values.Add("roles", "");

            if (userDto.ParCompanyXUserSgq != null)
                if (userDto.ParCompanyXUserSgq.Any(r => r.Role != null))
                    values.Add("rolesCompany", string.Join(",", userDto.ParCompanyXUserSgq.Select(n => n.Role).Distinct().ToArray()));
                else
                    values.Add("rolesCompany", string.Join(",", userDto.ParCompanyXUserSgq.Select(n => n.ParCompany_Id).Distinct().ToArray()));


            var cookie = new CookieHeaderValue("webControlCookie", values);
            cookie.MaxAge = TimeSpan.FromMinutes(Conn.sessionTimer);
            cookie.Path = "/";

            return cookie;
        }

        protected List<JObject> QueryNinjaPeloDbSgq(string query)
        {
            using (var dbSgq = new Factory(Conn.dataSource2, Conn.catalog2, Conn.pass2, Conn.user2))
            {
                return dbSgq.QueryNinjaADO(query);
            }
        }

        protected Factory ConexaoSgq()
        {
            return new Factory(Conn.dataSource2, Conn.catalog2, Conn.pass2, Conn.user2);
        }

        protected void UpdateStatus()
        {
            using (var dbPa = new PlanoAcaoEF.PlanoDeAcaoEntities())
            {
                dbPa.Database.ExecuteSqlCommand("UPDATE Pa_acao SET [STATUS] = 1 WHERE Id IN (SELECT Id FROM Pa_acao WHERE [Status] = (5) AND  CONVERT (date ,QuandoFim) < CONVERT (date ,GETDATE()))");
            }
        }

        protected void CreateMail(int idPlanejamento, int idAcao, int? idQuem, string title, bool? isAcompanhamento = false)
        {
            if (idQuem.GetValueOrDefault() > 0)
                using (var dbSgq = ConexaoSgq())
                {
                    var paUser = Pa_Quem.Get(idQuem.GetValueOrDefault());
                    dynamic enviarPara = dbSgq.QueryNinjaADO("SELECT * FROM UserSgq WHERE Name  = '" + paUser.Name + "'").FirstOrDefault();
                    string emailTo = "";
                    if (enviarPara != null)
                    {
                        emailTo = enviarPara.Email;
                    }

                    var todoConteudo = string.Empty;

                    if (!isAcompanhamento.GetValueOrDefault())
                    {
                        var conteudoPlanejamento = GetExternalResponse(Conn.selfRoot + "/Pa_Planejamento/Details?id=" + idPlanejamento);
                        var conteudoAcao = GetExternalResponse(Conn.selfRoot + "/Pa_Acao/Details?id=" + idAcao);
                        if (Conn.visaoOperacional)
                            todoConteudo = conteudoAcao.Result;
                        else
                            todoConteudo = conteudoPlanejamento.Result + conteudoAcao.Result;
                    }
                    else
                    {
                        var conteudoAcompanhamento = GetExternalResponse(Conn.selfRoot + "/Pa_Acao/Acompanhamento?id=" + idAcao);
                        todoConteudo = conteudoAcompanhamento.Result;
                    }
                    if (string.IsNullOrEmpty(emailTo))
                    {
                        emailTo = "gabriel@grtsolucoes.com.br";
                        title += " (Destinatário sem Email)" + paUser.Name;
                    }
                    //emailTo = "celso.bernar@grtsolucoes.com.br";
                    CreateMail(idPlanejamento, idAcao, emailTo, title, todoConteudo);
                }
        }

        protected void CreateMail(int idPlanejamento, int idAcao, string emailTo, string title, string body)
        {
            if (string.IsNullOrEmpty(emailTo))
            {
                emailTo = "gabriel@grtsolucoes.com.br";
                title += " (Destinatário sem Email)";
            }
            var email = new PlanoAcaoEF.EmailContent()
            {
                IsBodyHtml = true,
                AddDate = DateTime.Now,
                Subject = title,
                Project = "Plano de Ação",
                Body = body,
                To = emailTo,
            };
            PaAsyncServices.SendMailPATeste(email);
        }

        /// <summary>
        /// Filtro deve ser da var enviar JS pelo calendario
        /// </summary>
        /// <param name="filtro"></param>
        /// <param name="dtInit"></param>
        /// <param name="dtFim"></param>
        protected void GetParamsPeloFiltro(JObject filtro, out string dtInit, out string dtFim)
        {
            dynamic filtroObj = filtro;
            string startDate = filtroObj.startDate;
            string endDate = filtroObj.endDate;
            dtInit = Guard.ParseDateToSqlV2(startDate, Guard.CultureCurrent.BR).ToString("yyyy-MM-dd 00:00:00");
            dtFim = Guard.ParseDateToSqlV2(endDate, Guard.CultureCurrent.BR).ToString("yyyy-MM-dd 23:59:59");
        }

    }
}
