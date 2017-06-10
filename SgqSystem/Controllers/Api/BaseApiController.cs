using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net.Http;
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
    }

}