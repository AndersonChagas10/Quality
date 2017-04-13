using AutoMapper;
using Dominio;
using DTO.DTO.Params;
using DTO.Helpers;
using SgqSystem.Handlres;
using SgqSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
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

        [HttpPost]
        [HandleApi()]
        [Route("GetContadoresX")]
        public List<RetornoLevel1> GetContadoresX()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var listLevel1Retorno = new List<RetornoLevel1>();
            var collectionLevel2PorFrequenciaLevel1 = new List<CollectionLevel2>();
            /*Busca Items parametrizados a serem calculados*/
            var level2 = db.ParCounterXLocal.Where(r => r.ParLevel2_Id != null).Select(r => r.ParLevel2).Include("ParFrequency").Distinct();

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

                if(level2PelaFrquencia.IsNotNull())
                    collectionLevel2PorFrequenciaLevel1.AddRange(level2PelaFrquencia);

            }
            collectionLevel2PorFrequenciaLevel1 = collectionLevel2PorFrequenciaLevel1.OrderBy(r => r.ParLevel1_Id).ThenBy(r => r.EvaluationNumber).ThenBy(r => r.Sample).ToList();
            var todosLevel1 = collectionLevel2PorFrequenciaLevel1.Select(r => r.ParLevel1_Id).Distinct();
            /*Seleciono apenas o Level1 com as parametrizaçoes X (desconsiderar mock Frquenci = 1)*/

            foreach (var l1 in todosLevel1)
            {
                var retornoLevel1 = new RetornoLevel1() { idLevel1 = l1 };
                listLevel1Retorno.Add(retornoLevel1);
                retornoLevel1.avAmPorLevel1 = new List<AvAmPorLevel1>();

                var level1DaCollectionLevel2 = collectionLevel2PorFrequenciaLevel1.Where(r => r.ParLevel1_Id == l1).ToList();

                var todosAv = level1DaCollectionLevel2.Select(r => r.EvaluationNumber).Distinct();
                var todosAm = level1DaCollectionLevel2.Select(r => r.Sample).Distinct();

                foreach (var av in todosAv)
                {
                    foreach (var am in todosAm)
                    {
                        var AvAmPorLevel1Tmp = new AvAmPorLevel1();
                        AvAmPorLevel1Tmp.SomaWeiDefect = level1DaCollectionLevel2.Where(r => r.EvaluationNumber == av && r.Sample == am)?.Sum(r => r.WeiDefects);
                        if (AvAmPorLevel1Tmp.SomaWeiDefect > 0)
                        {
                            Debug.Write("\n AV:" + av + " AM:" + am + " SomaWeiDefect" + AvAmPorLevel1Tmp.SomaWeiDefect); 
                            retornoLevel1.avAmPorLevel1.Add(AvAmPorLevel1Tmp);
                        }
                        
                    }

                }
                //foreach (var level2Corrente in todosLevel2id)
                //{

                //    /*Todas Av e Am Possíveis a se calcular*/

                //    var todasAvDoLevel2 = listaLevel2PeloLevel1.Max(r => r.EvaluationNumber);
                //    var todasAmDoLevel2 = listaLevel2PeloLevel1.Max(r => r.Sample);

                //    ///* RN e Calculo*/
                //    //var AvAmResult = new AvAmPorLevel1();
                //    //foreach (var av in todasAvDoLevel2)
                //    //{

                //    //    foreach (var am in todasAmDoLevel2)
                //    //    {
                //    //        var level2NaAvAmDoloop = listaLevel2PeloLevel1.Where(r => r.EvaluationNumber == av && r.Sample == am);
                //    //        AvAmResult.AvAm = av.ToString() + "/" + am.ToString();
                //    //        AvAmResult.SomaWeiDefect = 0;

                //    //        foreach (var avalicoes in level2NaAvAmDoloop)
                //    //        {
                //    //            AvAmResult.SomaWeiDefect += avalicoes.WeiDefects;

                //    //            Debug.Write("\n Av/Am: " + AvAmResult.AvAm +
                //    //                " -- Av: " + avalicoes.EvaluationNumber +
                //    //                " - AM: " + avalicoes.Sample +
                //    //                " Level1id: " + avalicoes.ParLevel1_Id +
                //    //                " Level2Id: " + avalicoes.ParLevel2_Id +
                //    //                " WeiDefect: " + avalicoes.WeiDefects +
                //    //                " SomaWeiDefect: " + AvAmResult.SomaWeiDefect);

                //    //        }
                //    //    }

                //    //}

                //    //if (AvAmResult.SomaWeiDefect > 0)
                //    //    retornoLevel1.avAmPorLevel1.Add(AvAmResult);


                //}

              
                Debug.Write("\n\n");

            }


            foreach (var debug in listLevel1Retorno)
            {
                Debug.Write("\n" + debug.idLevel1 + ": ");
                foreach (var debugLevel2 in debug.avAmPorLevel1)
                    Debug.Write("\nAvAm:" + debugLevel2.AvAm + " SomaWeiDefect: " + debugLevel2.SomaWeiDefect);

                Debug.Write("\n ----------------------- \n");
            }
            return listLevel1Retorno;

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