using ADOFactory;
using DTO.DTO;
using Newtonsoft.Json.Linq;
using SGQDBContext;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
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
            cookie.MaxAge = TimeSpan.FromMinutes(60);
            cookie.Path = "/";

            return cookie;
        }

        

    }
}