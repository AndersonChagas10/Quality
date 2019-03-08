using ADOFactory;
using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.Handlres;
using SgqSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{

    [HandleApi()]
    [RoutePrefix("api/RetornaQueryRotinaApi")]
    public class RetornaQueryRotinaApiController : BaseApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        [HttpPost]
        [Route("RetornaQueryRotina")]
        public Object RetornaQueryRotina(JToken body)
        {
            var service = new SyncServices();
            var empresaPadrao = new ParCompany();
            var retornoRotinaNinja = new Object();
            var idUsuario = 0;

            Rotina myDeserializedObjList = (Rotina)Newtonsoft.Json.JsonConvert.DeserializeObject(body.ToString(), typeof(Rotina));
            RotinaIntegracao rotinaSelecionada;

            if (!string.IsNullOrEmpty(myDeserializedObjList.IdUsuario))
            {
                idUsuario = Convert.ToInt32(myDeserializedObjList.IdUsuario);
                var usuarioLogado = db.UserSgq.Where(x => x.Id == idUsuario).FirstOrDefault();
                empresaPadrao = db.ParCompany.Where(x => x.Id == usuarioLogado.ParCompany_Id).FirstOrDefault();
            }

            var queryFormatada = RetornaQueryRotina(myDeserializedObjList.IdRotina, myDeserializedObjList.Params, out rotinaSelecionada);

            PropriedadesConexaoDB propriedadesConexao = new PropriedadesConexaoDB
            {
                IP = empresaPadrao.IPServer,
                DBSERVER = empresaPadrao.DBServer
            };

            rotinaSelecionada.DataSource = MapearValoresDinamicos(rotinaSelecionada.DataSource, propriedadesConexao);

            rotinaSelecionada.InitialCatalog = MapearValoresDinamicos(rotinaSelecionada.InitialCatalog, propriedadesConexao);

            if (!string.IsNullOrEmpty(queryFormatada))
            {

                using (Factory factory = new Factory(rotinaSelecionada.DataSource, rotinaSelecionada.InitialCatalog, rotinaSelecionada.Password, rotinaSelecionada.User))
                {
                    retornoRotinaNinja = QueryNinja(db, queryFormatada).FirstOrDefault();
                }
            }

            return retornoRotinaNinja;
        }

        private string MapearValoresDinamicos(string dataSource, Object propriedadesConexao)
        {
            if (dataSource.Contains('{') && dataSource.Contains('}'))
            {
                var propriedadeNomeComparar = dataSource.Replace("{", "").Replace("}", "").ToUpperInvariant();

                var retornoValorDinamico = propriedadesConexao.GetType().GetProperty(propriedadeNomeComparar).GetValue(propriedadesConexao);

                return retornoValorDinamico.ToString();
            }
            else
                return dataSource;

        }

        public string RetornaQueryRotina(string rotina_Id, List<Dictionary<string, string>> parametro, out RotinaIntegracao rotinaSelecionada)
        {
            var query = "";
            rotinaSelecionada = null;

            if (parametro.Count > 0 && parametro[0].Count > 0)
            {
                var idRotina = Convert.ToInt32(rotina_Id);

                rotinaSelecionada = db.RotinaIntegracao.Where(x => x.Id == idRotina).FirstOrDefault();

                for (int i = 0; i < parametro[0].Count; i++)
                {
                    query = rotinaSelecionada.query.Replace('{' + parametro[0].ElementAt(i).Key + '}', parametro[0].ElementAt(i).Value);
                    rotinaSelecionada.query = query;
                }
            }
            return query;
        }
    }

    public class Rotina
    {
        public string IdRotina { get; set; }
        public string IdUsuario { get; set; }
        public List<Dictionary<string, string>> Params { get; set; }
    }

    public class PropriedadesConexaoDB
    {
        //Ao inserir novas propriedades colocar em letras Maiusculas. É feita uma comparação.
        public string IP { get; set; }
        public string DBSERVER { get; set; }
    }
}
