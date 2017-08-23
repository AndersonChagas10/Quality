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
    public class ApiSgqController : BaseApiController
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
            try
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
                    dynamic unidadeDyn = dbSgq.QueryNinjaADO("SELECT Name FROM PARCOMPANY WHERE Initials = '" + unidade + "' OR Name = '" + unidade + "'").FirstOrDefault();
                    //int unidadeId = unidadeDyn.Id;
                    string unidadeNameSGQ = unidadeDyn.Name;

                    if (level == 3)
                    {
                        string level1 = filtroDyn.level1Name;
                        string level2 = filtroDyn.level2Name;
                        List<string> level3Name = filtroDyn.level3names.ToObject<List<string>>();
                        retorno = FiltraPorLevel3(filtroDyn, dataInicio, dataFim, dbSgq, unidade, level1, unidadeNameSGQ, level2, level3Name);
                    }
                    else if (level == 2)
                    {
                        string level1 = filtroDyn.level1Name;
                        List<string> level2Name = filtroDyn.level2names.ToObject<List<string>>();
                        retorno = FiltraPorLevel2(dataInicio, dataFim, dbSgq, level1, unidadeNameSGQ, level2Name);
                    }
                    else if (level == 1)
                    {
                        List<string> level1Name = filtroDyn.level1names.ToObject<List<string>>();
                        retorno = FiltraPorLevel1(dataInicio, dataFim, dbSgq, level1Name, unidadeNameSGQ);
                    }

                }

                return retorno;
            }
            catch (Exception e)
            {
                return new Dictionary<string, int>();
            }

        }


//1	2017-03-08 20:12:27.8500000	NULL Atrasado
//2	2017-03-08 20:12:27.8500000	NULL Cancelado
//3	2017-03-08 20:12:27.8500000	NULL Concluído
//4	2017-03-08 20:12:27.8500000	NULL Concluído com atraso
//5	2017-03-08 20:12:27.8500000	NULL Em Andamento

        private Dictionary<string, int> FiltraPorLevel2(DateTime dataInicio, DateTime dataFim, Factory dbSgq, string level1, string unidadeNameSGQ, List<string> level2Name)
        {
            var registros = 0;
            var retorno = new Dictionary<string, int>();
            foreach (var l2Name in level2Name)
            {
                //registros = db.Pa_Acao.Count(r => r.UnidadeName == unidadeNameSGQ && r.Level1Name == level1 && r.Level2Name == l2Name && r.AddDate<= dataFim && r.AddDate >= dataInicio && (r.Status == 5 || r.Status == 1));7
                registros = db.Pa_Acao.Count(r => r.UnidadeName == unidadeNameSGQ && r.Level1Name == level1 && r.Level2Name == l2Name &&  (r.Status == 5 || r.Status == 1));
                retorno.Add(l2Name, registros);
            }
            return retorno;
        }

        private Dictionary<string, int> FiltraPorLevel1(DateTime dataInicio, DateTime dataFim, Factory dbSgq, List<string> level1Name, string unidadeNameSGQ)
        {
            var registros = 0;
            var retorno = new Dictionary<string, int>();
            foreach (var l1Name in level1Name)
            {
                //registros = db.Pa_Acao.Count(r => r.UnidadeName == unidadeNameSGQ && r.Level1Name == l1Name && r.AddDate <= dataFim && r.AddDate >= dataInicio && (r.Status == 5  || r.Status == 1));
                registros = db.Pa_Acao.Count(r => r.UnidadeName == unidadeNameSGQ && r.Level1Name == l1Name && (r.Status == 5 || r.Status == 1));
                retorno.Add(l1Name, registros);
            }
            return retorno;
        }

        private Dictionary<string, int> FiltraPorLevel3(dynamic filtroDyn, DateTime dataInicio, DateTime dataFim, Factory dbSgq, string unidade, string level1, string unidadeNameSGQ, string level2, List<string> level3Name)
        {
            var registros = 0;
            var retorno = new Dictionary<string, int>();
            foreach (var l3Name in level3Name)
            {
                //registros = db.Pa_Acao.Count(r => r.UnidadeName == unidadeNameSGQ && r.Level1Name == level1 && r.Level2Name == level2 && r.Level3Name == l3Name && r.AddDate <= dataFim && r.AddDate >= dataInicio && (r.Status == 5 || r.Status == 1));
                registros = db.Pa_Acao.Count(r => r.UnidadeName == unidadeNameSGQ && r.Level1Name == level1 && r.Level2Name == level2 && r.Level3Name == l3Name &&  (r.Status == 5 || r.Status == 1));
                retorno.Add(l3Name, registros);
            }
            return retorno;
        }


    }
}
