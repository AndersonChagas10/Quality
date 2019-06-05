using ADOFactory;
using Dominio;
using Newtonsoft.Json.Linq;
using ServiceModel;
using SgqService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace SgqService.Controllers.Api
{
    [RoutePrefix("api/RelatorioGenerico")]
    public class RelatorioGenericoApiController : BaseApiController
    {

        

        //Tabela Angular
        [HttpPost]
        [Route("getTabela")]
        public dynamic getTabela(FormularioParaRelatorioViewModel model)
        {

            using (Factory factory = new Factory("DefaultConnection"))
            {
                //var data = "And Data Between" + model._dataInicioSQL + " AND " + model._dataFimSQL;

                var db = new SgqDbDevEntities();
                dynamic retorno = new ExpandoObject();
                //var queryHeader = "select Column_name from Information_schema.columns where Table_name like 'UserSgq'";
                var querybody = "SELECT TOP 10 STR(Id) as Col1, Name as Col2, [Password] as Col3 from UserSgq";
                //var querybody = "select STR(*) from UserSgq";

                //retorno.header = db.Database.SqlQuery<string>(queryHeader).ToList();
                retorno.header = new List<string> { "Id", "Nome", "Senha" };
                retorno.body = factory.SearchQuery<PropriedadesGenericas>(querybody).ToList();

                return retorno;
            }
        }

        //Tabela Javascript
        [HttpPost]
        [Route("getTabela2")]
        public dynamic getTabela2(FormularioParaRelatorioViewModel model)
        {
            using (Factory factory = new Factory("DefaultConnection"))
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
                retorno.body = factory.SearchQuery<PropriedadesGenericas>(querybody).ToList();

                return retorno;
            }
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

        [HttpGet]
        [Route("reciveDataPCC1b2/{unidadeId}/{data}")]
        public dynamic reciveDataPCC1b2(string unidadeId, string data)
        {
            VerifyIfIsAuthorized();
            //DateTime dataConsolidation = DateCollectConvert(data);
            //string consolidation = getConsolidation(unidadeId, dataConsolidation, 0);

            string sql = @"
                            SELECT 
                            sequential, side, cast(
							
							case 
								when (select avg( cast(IsNotEvaluate as int)) from result_level3 where CollectionLevel2_Id = c2.id) = 1 
								then 2 
								when ReauditIs = 1 
										then case 
												when Defects = 0 
												then 0 
												else 1 
											 end 
								else 0 end 
							
							as varchar) resultado, P2.NAME AS monitoramento
                            -- '{""sequencial"":""' + cast(Sequential as varchar) + '"",""banda"":""' + cast(Side as varchar) + '"",""resultado"":""' + cast(case when Defects = 0 then 0 else 1 end as varchar) + '""}' as retorno

                            FROM COLLECTIONLEVEL2 C2
                            INNER JOIN PARLEVEL2 P2
							ON P2.ID = C2.PARLEVEL2_ID
                            WHERE PARLEVEL1_ID = 3
                            AND UnitId = " + unidadeId + @"
                            AND CAST(CollectionDate AS DATE) = '" + data + @"'
                           ORDER BY 4,1,2";

            var resultadoPCC1b = new List<JObject>();

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(conexao))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader r = command.ExecuteReader())
                        {

                            while (r.Read())
                            {
                                JObject row = new JObject();
                                for (int i = 0; i < r.FieldCount; i++)
                                    row[r.GetName(i)] = r[i].ToString();

                                resultadoPCC1b.Add(row);

                            }
                        }
                    }
                    if (connection.State == System.Data.ConnectionState.Open) connection.Close();
                }
            }

            catch (Exception ex)
            {
                //int insertLog = insertLogJson(sql, ex.Message, "N/A", "N/A", "resultadoPCC1b");
            }

            /* métado JQuery
             * 
            $.get("http://localhost:57506/api/RelatorioGenerico/reciveDataPCC1b2/10/20181206", function(data) {
                console.log(data);
            });

            */


            return resultadoPCC1b;
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

        [HttpPost]
        [Route("GetParametrizacaoGeral")]
        public dynamic GetParametrizacaoGeral([FromBody] FormularioParaRelatorioViewModel form)
        {

            using (var db = new SgqDbDevEntities())
            {

                dynamic a = new ExpandoObject();
                /*Estas 2 primeiras queryes são independentes*/

                var level1 = "";
                var level2 = "";
                var level3 = "";

                if (form.level1Id > 0)
                    level1 = " AND P1.ID = " + form.level1Id;

                if (form.level2Id > 0)
                    level2 = " AND P2.ID = " + form.level2Id;

                if (form.level3Id > 0)
                    level3 = " AND P3.ID = " + form.level3Id;

                var query = @"  SELECT
                                P1.NAME AS Col1,
                                P2.NAME AS Col2,
                                P3.NAME AS Col3
                                FROM PARLEVEL3LEVEL2 P32 with (nolock)
                                INNER JOIN ParLevel3Level2Level1 P321 with (nolock)
                                ON P321.PARLEVEL3LEVEL2_ID = P32.Id
                                INNER JOIN ParLevel1 P1 with (nolock)
                                ON P321.ParLevel1_Id = P1.ID
                                INNER JOIN PARLEVEL2 P2 with (nolock)
                                ON P32.ParLevel2_Id = P2.ID
                                INNER JOIN PARLEVEL3 P3 with (nolock)
                                ON P32.ParLevel3_Id = P3.Id
                                WHERE 1=1"
                                + level1 + level2 + level3 +
                              @"  AND P32.IsActive = 1
                                  AND P321.Active = 1
                                  AND P1.IsActive = 1
                                  AND P2.IsActive = 1
                                  AND P3.IsActive = 1
                                GROUP BY P1.NAME, P2.NAME, P3.NAME
                                order by 1,2,3
";


                using (Factory factory = new Factory("DefaultConnection"))
                {
                    a = factory.SearchQuery<PropriedadesGenericas>(query).ToList();

                }
                //if (a.Count() == 0)
                    return a;

            }

            return null;

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

}
