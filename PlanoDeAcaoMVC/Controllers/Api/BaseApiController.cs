using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using RazorEngine;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    public class BaseApiController : ApiController
    {
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

      
    }
}
