using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/RelatorioGenerico")]
    public class RelatorioGenericoApiController : ApiController
    {
        //Tabela Angular
        [HttpPost]
        [Route("getTabela")]
        public dynamic getTabela(FormularioParaRelatorioViewModel model)
        {

            //var data = "And Data Between" + model._dataInicioSQL + " AND " + model._dataFimSQL;

            var db = new SgqDbDevEntities();
            dynamic retorno = new ExpandoObject();
            //var queryHeader = "select Column_name from Information_schema.columns where Table_name like 'UserSgq'";
            var querybody = "SELECT TOP 10 STR(Id) as Col1, Name as Col2, [Password] as Col3 from UserSgq";
            //var querybody = "select STR(*) from UserSgq";

            //retorno.header = db.Database.SqlQuery<string>(queryHeader).ToList();
            retorno.header = new List<string> { "Id", "Nome", "Senha" };
            retorno.body = db.Database.SqlQuery<PropriedadesGenericas>(querybody).ToList();

            return retorno;
        }

        //Tabela Javascript
        [HttpPost]
        [Route("getTabela2")]
        public dynamic getTabela2(FormularioParaRelatorioViewModel model)
        {

            //var data = "And Data Between" + model._dataInicioSQL + " AND " + model._dataFimSQL;

            var db = new SgqDbDevEntities();
            dynamic retorno = new ExpandoObject();

            //var queryHeader = "select Column_name from Information_schema.columns where Table_name like 'UserSgq'";
            var querybody = "SELECT TOP 10 STR(Id) as Col1, Name as Col2, [Password] as Col3 from UserSgq";
            //var querybody = "select STR(*) from UserSgq";

            //retorno.header = db.Database.SqlQuery<string>(queryHeader).ToList();
            //retorno.header = new List<string> { "Id", "Nome", "Senha" };

            var header = new List<string> { "Id", "Nome", "Senha" };
            var header2 = new List<Header>();


            foreach (var item in header)
            {
                header2.Add(new Header() { title = item });
            }

            retorno.header = header2;
            retorno.body = db.Database.SqlQuery<PropriedadesGenericas>(querybody).ToList();

            return retorno;
        }


        [HttpPost]
        [Route("getTabela3")]
        public dynamic getTabela3(JObject model)
        {
            var db = new SgqDbDevEntities();
            var query = "SELECT TOP 10 STR(Id) as Id, Name as Nome, [Password] as Pass from UserSgq";
            var retorno = QueryNinja(db, query);

            return retorno;
        }
        

        //QueryGenerica para implementar
        protected dynamic QueryNinja(DbContext db, string query)
        {

            db.Database.Connection.Open();
            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = query;
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

        //ApiRelatoriosController
    }

    public class PropriedadesGenericas
    {
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
        public string Col5 { get; set; }
        public string Col6 { get; set; }
        public string Col7 { get; set; }
        public string Col8 { get; set; }
        public string Col9 { get; set; }
        public string Col10 { get; set; }
        public string Col11 { get; set; }
        public string Col12 { get; set; }
        public string Col13 { get; set; }
        public string Col14 { get; set; }
        public string Col15 { get; set; }
        public string Col16 { get; set; }
        public string Col17 { get; set; }
        public string Col18 { get; set; }
        public string Col19 { get; set; }
        public string Col20 { get; set; }
        public string Col21 { get; set; }
        public string Col22 { get; set; }
        public string Col23 { get; set; }
        public string Col24 { get; set; }
        public string Col25 { get; set; }
        public string Col26 { get; set; }
        public string Col27 { get; set; }
        public string Col28 { get; set; }
        public string Col29 { get; set; }
        public string Col30 { get; set; }
        public string Col31 { get; set; }
        public string Col32 { get; set; }
        public string Col33 { get; set; }
        public string Col34 { get; set; }
        public string Col35 { get; set; }
        public string Col36 { get; set; }
        public string Col37 { get; set; }
        public string Col38 { get; set; }
        public string Col39 { get; set; }
        public string Col40 { get; set; }
        public string Col41 { get; set; }
        public string Col42 { get; set; }
        public string Col43 { get; set; }
        public string Col44 { get; set; }
        public string Col45 { get; set; }
        public string Col46 { get; set; }
        public string Col47 { get; set; }
        public string Col48 { get; set; }
        public string Col49 { get; set; }
        public string Col50 { get; set; }
    }

    public class Header
    {
        public string title { get; set; }
    }

}
