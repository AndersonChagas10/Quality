using ADOFactory;
using DTO.Helpers;
using Newtonsoft.Json.Linq;
using PlanoDeAcaoMVC.SgqIntegracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Sgq")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApiSgqController : ApiController
    {

        private PlanoAcaoEF.PlanoDeAcaoEntities db;

        public ApiSgqController()
        {
            db = new PlanoAcaoEF.PlanoDeAcaoEntities();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.AutoDetectChangesEnabled = false;
        }

        [HttpPost]
        [Route("GetAcoes")]
        public Dictionary<string, int> GetAcoes(JObject filtro)
        {
            dynamic filtroDyn = filtro;
            //var dataInicio = DateTime.Now.AddDays(-3);
            //var dataFim = DateTime.Now.AddDays(3);
            var retorno = new Dictionary<string, int>();
            string inicio = filtroDyn.startDate;
            string fim = filtroDyn.endDate;
            int level = filtroDyn.isLevel;
            var dataInicio = Guard.ParseDateToSqlV2(inicio, Guard.CultureCurrent.BR);
            var dataFim = Guard.ParseDateToSqlV2(fim, Guard.CultureCurrent.BR).AddHours(23).AddMinutes(59).AddSeconds(59);
            
            using (var dbSgq = new ConexaoSgq().db)
            {
                string unidade = filtroDyn.unitName;
                dynamic unidadeDyn = dbSgq.QueryNinjaADO("SELECT Id FROM PARCOMPANY WHERE Initials = '" + unidade + "' OR Name = '" + unidade + "'").FirstOrDefault();
                int unidadeId = unidadeDyn.Id;

                if (level == 3)
                {
                    string level1 = filtroDyn.level1Name;
                    string level2 = filtroDyn.level2Name;
                    List<string> level3Name = filtroDyn.level3names.ToObject<List<string>>();
                    retorno = FiltraPorLevel3(filtroDyn, dataInicio, dataFim, dbSgq, unidade, level1, unidadeId, level2, level3Name);
                } else if (level == 2)
                {
                    string level1 = filtroDyn.level1Name;
                    List<string> level2Name = filtroDyn.level2names.ToObject<List<string>>();
                    retorno = FiltraPorLevel2(dataInicio, dataFim, dbSgq, level1, unidadeId , level2Name);
                }
                else if (level == 1)
                {
                    List<string> level1Name = filtroDyn.level1names.ToObject<List<string>>();
                    retorno = FiltraPorLevel1(dataInicio, dataFim, dbSgq, level1Name, unidadeId);
                }

            }

            return retorno;
        }

        private Dictionary<string, int> FiltraPorLevel2(DateTime dataInicio, DateTime dataFim, Factory dbSgq, string level1, int unidadeId, List<string> level2Name)
        {
            var registros = 0;
            var retorno = new Dictionary<string, int>();
            foreach (var l2Name in level2Name)
            {
                registros = db.Pa_Acao.Count(r => r.Unidade_Id == unidadeId && r.Level1Name == level1 && r.Level2Name == l2Name && r.AddDate<= dataFim && r.AddDate >= dataInicio);
                retorno.Add(l2Name, registros);
            }
            return retorno;
        }

        private Dictionary<string, int> FiltraPorLevel1(DateTime dataInicio, DateTime dataFim, Factory dbSgq, List<string> level1Name, int unidadeId)
        {
            var registros = 0;
            var retorno = new Dictionary<string, int>();
            foreach (var l1Name in level1Name)
            {
                registros = db.Pa_Acao.Count(r => r.Unidade_Id == unidadeId && r.Level1Name == l1Name && r.AddDate <= dataFim && r.AddDate >= dataInicio);
                retorno.Add(l1Name, registros);
            }
            return retorno;
        }

        private new Dictionary<string, int> FiltraPorLevel3(dynamic filtroDyn, DateTime dataInicio, DateTime dataFim, Factory dbSgq, string unidade, string level1, int unidadeId, string level2, List<string> level3Name)
        {
            var registros = 0;
            var retorno = new Dictionary<string, int>();
            foreach (var l3Name in level3Name)
            {
                registros = db.Pa_Acao.Count(r => r.Unidade_Id == unidadeId && r.Level1Name == level1 && r.Level2Name == level2 && r.Level3Name == l3Name && r.AddDate <= dataFim && r.AddDate >= dataInicio);
                retorno.Add(l3Name, registros);
            }
            return retorno;
        }


    }
}
