using Dominio;
using DTO.Helpers;
using SgqSystem.Handlres;
using SgqSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.App
{
    [RoutePrefix("api/AppParams")]
    public class AppParamsApiController : ApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        public AppParamsApiController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        //$.post('http://192.168.25.200/SgqBr/api/AppParams/GetContadoresX',{ }, function(r)
        //{
        //    console.log(r);
        //});
        [HttpPost]
        [HandleApi()]
        [Route("GetContadoresX")]
        public List<RetornoLevel1> GetContadoresX()
        {
          

            /*Busca Items parametrizados a serem calculados*/
            var listLevel1Retorno = new List<RetornoLevel1>();
            var level2 = db.ParCounterXLocal.Where(r => r.ParLevel2_Id != null && r.ParCounter_Id == 1).Select(r => r.ParLevel2).Include("ParFrequency").Distinct();
            var collectionLevel2PorFrequenciaLevel1 = GetCollectionLevel2PelaFrquencia(level2);
            var todosLevel1 = collectionLevel2PorFrequenciaLevel1.Select(r => r.ParLevel1_Id).Distinct();

            /*Para cada level 1 calcula av / am independente dos Parlevel2 deste*/
            foreach (var l1 in todosLevel1)
            {
                var retornoLevel1 = new RetornoLevel1() { idLevel1 = l1 };
                retornoLevel1.avAmPorLevel1 = new List<AvAmPorLevel1>();
                var level1DaCollectionLevel2 = collectionLevel2PorFrequenciaLevel1.Where(r => r.ParLevel1_Id == l1).ToList();
                var todosAv = level1DaCollectionLevel2.Select(r => r.EvaluationNumber).Distinct();
                var todosAm = level1DaCollectionLevel2.Select(r => r.Sample).Distinct();

                foreach (var av in todosAv)
                    foreach (var am in todosAm)
                    {
                        var AvAmPorLevel1Tmp = new AvAmPorLevel1();
                        /*Soma Av e Am Iguais de um determinado level1*/
                        AvAmPorLevel1Tmp.SomaWeiDefect = level1DaCollectionLevel2.Where(r => r.EvaluationNumber == av && r.Sample == am)?.Sum(r => r.WeiDefects);
                        AvAmPorLevel1Tmp.AvAm = av + "/" + am;
                        if (AvAmPorLevel1Tmp.SomaWeiDefect > 0)
                        {
                            retornoLevel1.avAmPorLevel1.Add(AvAmPorLevel1Tmp);

#if DEBUG
                            Debug.Write("\n AV:" + av + " AM:" + am + " SomaWeiDefect" + AvAmPorLevel1Tmp.SomaWeiDefect);
#endif

                        }

                    }

#if DEBUG
                Debug.Write("\n\n");
#endif

                listLevel1Retorno.Add(retornoLevel1);
            }

            DebugDoResult(listLevel1Retorno);

            return listLevel1Retorno;

        }

        [Conditional("DEBUG")]
        private static void DebugDoResult(List<RetornoLevel1> listLevel1Retorno)
        {
            foreach (var debug in listLevel1Retorno)
            {
                Debug.Write("\n" + debug.idLevel1 + ": ");
                foreach (var debugLevel2 in debug.avAmPorLevel1)
                    Debug.Write("\nAvAm:" + debugLevel2.AvAm + " SomaWeiDefect: " + debugLevel2.SomaWeiDefect);
                Debug.Write("\n ----------------------- \n");
            }
        }

        private List<CollectionLevel2> GetCollectionLevel2PelaFrquencia(IQueryable<ParLevel2> level2)
        {
            var retorno = new List<CollectionLevel2>();
            foreach (var level2item in level2)
            {
                string dataInicio = string.Empty;
                string dataFim = string.Empty;

                SyncServices.getFrequencyDate(level2item.ParFrequency_Id, DateTime.Now, ref dataInicio, ref dataFim);
                var rangePelaFrquenciaInicio = Guard.ParseDateToSqlV2(dataInicio, "yyyyMMdd");
                var rangePelaFrquenciaFim = Guard.ParseDateToSqlV2(dataFim, "yyyyMMdd");

                /*MOCK para teste range de 15 dias*/
                //var rangePelaFrquenciaInicio = DateTime.Now.AddDays(-15);
                //var rangePelaFrquenciaFim = DateTime.Now;

                var level2PelaFrquencia = db.CollectionLevel2
                                        .Where(r =>
                                        /*Frequencia vai AQUI*/
                                        DbFunctions.TruncateTime(r.CollectionDate) >= DbFunctions.TruncateTime(rangePelaFrquenciaInicio)
                                        && DbFunctions.TruncateTime(r.CollectionDate) <= DbFunctions.TruncateTime(rangePelaFrquenciaFim)
                                        && r.WeiDefects > 0
                                        && r.ParLevel2_Id == level2item.Id
                                        );

                if (level2PelaFrquencia.IsNotNull())
                    retorno.AddRange(level2PelaFrquencia);

            }

            retorno = retorno.OrderBy(r => r.ParLevel1_Id).ThenBy(r => r.EvaluationNumber).ThenBy(r => r.Sample).ToList();
            return retorno;
        }

    }

    public class RetornoLevel1
    {
        public int idLevel1 { get; set; }
        public List<AvAmPorLevel1> avAmPorLevel1 { get; set; }
    }

    public class AvAmPorLevel1
    {
        public string AvAm { get; set; }
        public decimal? SomaWeiDefect { get; set; }
        public decimal SomaNotIsConform { get; set; }
    }

}