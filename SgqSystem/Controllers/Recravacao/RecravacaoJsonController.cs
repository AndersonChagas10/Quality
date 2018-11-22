using Dominio;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Recravacao
{
    public class RecravacaoJsonController : BaseController
    {
        // GET: RecravacaoJson
        public ActionResult Index(string date = null)
        {
            if (string.IsNullOrEmpty(date) || date.Length != 10)
                date = DateTime.Now.ToString("yyyy-MM-dd");
            List<RecravacaoJson> listaRecravacao = new List<RecravacaoJson>();
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {

                var dataFiltroRecravacao = DateTime.ParseExact(date, "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);
                var dataFiltroRecravacaoAmanha = DateTime.ParseExact(date, "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture).AddDays(1);
                int companyId = getUserUnitId();

                var query = string.Format(@"
SELECT RJ.AlterDate, RJ.UserValidated_Id, RJ.Id, RJ.UserFinished_Id, PRL.Name FROM RecravacaoJson RJ 
LEFT JOIN ParRecravacao_Linhas PRL ON PRL.Id = RJ.Linha_Id
WHERE RJ.AlterDate >= '{0}' AND RJ.AlterDate < '{1}' AND RJ.ParCompany_Id = {2}"
                    , dataFiltroRecravacao.ToString("yyyy-MM-dd")
                    , dataFiltroRecravacaoAmanha.ToString("yyyy-MM-dd")
                    , companyId);

                var results = QueryNinja(db, query);

                //listaRecravacao = db.RecravacaoJson
                //    .Where(x => x.AlterDate >= dataFiltroRecravacao
                //&& x.AlterDate < dataFiltroRecravacaoAmanha).ToList();
                ViewBag.dataFiltroRecravacao = dataFiltroRecravacao;

                return View(results);
            }
        }

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
    }
}