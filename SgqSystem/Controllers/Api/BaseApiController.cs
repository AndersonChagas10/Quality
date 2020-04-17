using ADOFactory;
using Dominio;
using DTO.DTO;
using Newtonsoft.Json.Linq;
using SGQDBContext;
using SgqService.ViewModels;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SgqSystem.Controllers.Api
{
    public class CredenciaisSgq
    {
        public string Username { get; set; }
        public string Senha { get; set; }
    }

    public class BaseApiController : ApiController
    {
        protected string token;
        protected string tokenFiltros;
        // GET: BaseAPI
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            // Do some stuff
            base.Initialize(controllerContext);

            try
            {
                token = Request.Headers.GetValues("token").FirstOrDefault().ToString();

            }
            catch
            {
            }

            try
            {
                tokenFiltros = Request.Headers.GetValues("Cookie").FirstOrDefault().ToString().Split('&').ElementAt(1).Split('=')[1];
                tokenFiltros += "|00000";
            }
            catch
            {
            }

            string language = "";
            try
            {
                language = Request.Headers.GetValues("lang").FirstOrDefault();
            }
            catch
            {

            }

            if (string.IsNullOrEmpty(language))
            {
                language = "pt-BR";
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
        }

        protected void VerifyIfIsAuthorized()
        {
            try
            {
                using (SgqDbDevEntities db = new SgqDbDevEntities())
                {
                    var user = new CredenciaisSgq()
                    {
                        Username = token.Split('|')[0],
                        Senha = token.Split('|')[1]
                    };
                    if (!db.UserSgq.Any(x => x.Name == user.Username && x.Password == user.Senha && x.IsActive == true))
                    {
                        throw new UnauthorizedAccessException("Acesso negado!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Acesso negado!");
            }
        }

        /// <summary>
        /// Retorna Objeto Dinamico com dados da query no formato da Datatable.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected dynamic QueryNinjaDataTable(DbContext db, string query)
        {
            db.Database.Connection.Open();
            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = query;
            cmd.CommandTimeout = 9600;
            var reader = cmd.ExecuteReader();
            List<JObject> datas = new List<JObject>();
            List<JObject> columns = new List<JObject>();
            dynamic retorno = new ExpandoObject();

            while (reader.Read())
            {
                var row = new JObject();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader[i].ToString();

                datas.Add(row);
            }


            for (int i = 0; i < reader.FieldCount; i++)
            {
                var col = new JObject();
                col["title"] = col["data"] = reader.GetName(i);
                columns.Add(col);
            }

            retorno.datas = datas;
            retorno.columns = columns;

            return retorno;
        }


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
            cmd.CommandTimeout = 300;
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
            cookie.MaxAge = TimeSpan.FromHours(48);
            cookie.Expires = DateTime.Now.AddHours(48);
            cookie.Path = "/";

            return cookie;
        }

        protected object ToDynamic(string value)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };

            return Newtonsoft.Json.JsonConvert.DeserializeObject(value, settings);
        }

        protected string ToJson(object value)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented, settings);
        }
        public static string GetWebConfigSettings(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        protected string getAprovadorName(FormularioParaRelatorioViewModel form, SgqDbDevEntities dbSgq)
        {
            var SQL = $@"SELECT top 1
        Aprovador
        FROM ReportXUserSgq RXU      
        WHERE (RXU.Parcompany_Id = {form.unitId} OR RXU.Parcompany_Id IS NULL)
        AND RXU.ParLevel1_Id = {form.level1Id}
        AND RXU.ItemMenu_Id = {form.ItemMenu_Id}
        Order by RXU.Parcompany_Id desc";

            return dbSgq.Database.SqlQuery<string>(SQL).FirstOrDefault();
        }

        protected string getElaboradorName(FormularioParaRelatorioViewModel form, SgqDbDevEntities dbSgq)
        {
            var SQL = $@"SELECT top 1
    	Elaborador
        FROM ReportXUserSgq RXU
        WHERE (RXU.Parcompany_Id = {form.unitId} OR RXU.Parcompany_Id IS NULL)
        AND RXU.ParLevel1_Id = {form.level1Id}
        AND RXU.ItemMenu_Id = {form.ItemMenu_Id}
        Order by RXU.Parcompany_Id desc";

            return dbSgq.Database.SqlQuery<string>(SQL).FirstOrDefault();
        }

        protected string getNomeRelatorio(FormularioParaRelatorioViewModel form, SgqDbDevEntities dbSgq)
        {
            var SQL = $@"SELECT top 1
    	NomeRelatorio
        FROM ReportXUserSgq RXU
        WHERE (RXU.Parcompany_Id = {form.unitId} OR RXU.Parcompany_Id IS NULL)
        AND RXU.ParLevel1_Id = {form.level1Id}
        AND RXU.ItemMenu_Id = {form.ItemMenu_Id}
        Order by RXU.Parcompany_Id desc";

            return dbSgq.Database.SqlQuery<string>(SQL).FirstOrDefault();
        }

        protected string getSiglaUnidade(FormularioParaRelatorioViewModel form, SgqDbDevEntities dbSgq)
        {
            return dbSgq.ParCompany.Where(r => r.Id == form.unitId && r.IsActive).Select(r => r.Initials).FirstOrDefault();
        }

        protected string GetDicionarioEstatico(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            try
            {
                using (var db = new SgqDbDevEntities())
                {
                    return db.DicionarioEstatico.Where(r => r.Key == key).FirstOrDefault().Value;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected string GetUserUnitsIds(bool showUserCompanies = false)
        {

            var unidadesUsuario = new List<string>();

            try
            {
                using (SgqDbDevEntities db = new SgqDbDevEntities())
                {
                    var user = new CredenciaisSgq()
                    {
                        Username = tokenFiltros.Split('|')[0],
                        Senha = tokenFiltros.Split('|')[1]
                    };

                    var usuarioLogado = db.UserSgq.Where(x => x.Name == user.Username).FirstOrDefault();


                    if (usuarioLogado == null)
                        throw new UnauthorizedAccessException("Acesso negado!");

                    if (usuarioLogado.ShowAllUnits.Value || !showUserCompanies)
                    {
                        unidadesUsuario = db.ParCompany.Where(x => x.IsActive).Select(x => x.Id.ToString()).ToList();
                    }
                    else
                    {
                        unidadesUsuario = db.ParCompanyXUserSgq.Where(x => x.UserSgq_Id == usuarioLogado.Id).Select(x => x.ParCompany_Id.ToString()).ToList();
                        unidadesUsuario.Add(usuarioLogado.ParCompany_Id.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
                throw new UnauthorizedAccessException("Acesso negado!");
            }

            var retorno = string.Join(",", unidadesUsuario);

            return retorno;

        }

    }
}