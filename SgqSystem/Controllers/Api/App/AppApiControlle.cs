using AutoMapper;
using Dominio;
using DTO.DTO.Params;
using DTO.Helpers;
using SgqSystem.Handlres;
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
        public List<RetornoLevel1> GetContadoresX(int unidadeId)
        {
            db.Configuration.LazyLoadingEnabled = false;
            
            #region Variáveis

            var listLevel1Retorno = new List<RetornoLevel1>();

            #endregion

            #region Busca Items parametrizados a serem calculados

            var level1Id = 1;
            /*Seleciono apenas os ParLevel2 COm o TIPO DE CONTADOR parametrizado para este caso*/
            var level2 = db.ParLevel2.Include("ParFrequency").Where(r => r.IsActive && r.ParCounterXLocal.Any(x => x.ParCounter_Id == 18));

            #endregion

            /*Seleciono apenas o Level1 com as parametrizaçoes X (desconsiderar mock Frquenci = 1)*/
            foreach (var level2Corrente in level2.Where(r => r.ParFrequency_Id == 3))
            {

                #region Frequencia

                /*Encontro a frequencia*/
                var dataPelaFrequency = DateTime.Now;
                switch (/*item.ParFrequency_Id*/3)
                {
                    case 3:
                        dataPelaFrequency = DateTime.Now.AddDays(-15);
                        break;
                    default:
                        break;
                }

                #endregion

                #region Select do itens no DB pela frequecia e level 1 Id

                /*Seleciono a collection level 2 de acordo com a frequencia*/
                var collectionLevel2NaFrequenciaUnidadeLevel1 = db.CollectionLevel2
                        .Where(r => DbFunctions.TruncateTime(r.CollectionDate) >= DbFunctions.TruncateTime(dataPelaFrequency)
                        && DbFunctions.TruncateTime(r.CollectionDate) <= DbFunctions.TruncateTime(DateTime.Now)
                        && r.ParLevel1_Id == level1Id && r.UnitId == unidadeId).ToList();

                #endregion

                #region Todas Av e Am Possíveis a se calcular

                var todasAvDoLevel2 = collectionLevel2NaFrequenciaUnidadeLevel1.Select(r => r.EvaluationNumber).Distinct();
                var todasAmDoLevel2 = collectionLevel2NaFrequenciaUnidadeLevel1.Select(r => r.Sample).Distinct();

                #endregion

                #region RN e Calculo

                var listaAvAmResult = new List<AvAmPorLevel1>();
                foreach (var av in todasAvDoLevel2)
                {
                    foreach (var am in todasAmDoLevel2)
                    {
                        var AvAmResult = new AvAmPorLevel1();
                        var collectionLevel2NaFrequenciaUnidadeLevel1PorAmostra = collectionLevel2NaFrequenciaUnidadeLevel1.Where(r => r.EvaluationNumber == av && r.Sample == am);

                        AvAmResult.AvAm = av.ToString() + "/" + am.ToString();
                        AvAmResult.SomaWeiDefect = 0;

                        foreach (var avalicoes in collectionLevel2NaFrequenciaUnidadeLevel1PorAmostra)
                        {
                            AvAmResult.SomaWeiDefect += avalicoes.WeiDefects;
                            Debug.Write("\n Av/Am: " + AvAmResult.AvAm + " -- Av: " + avalicoes.EvaluationNumber + " - AM: " + avalicoes.Sample + " Level1id: " + avalicoes.ParLevel1_Id + " Level2Id: " + avalicoes.ParLevel2_Id + " WeiDefect: " + avalicoes.WeiDefects + " SomaWeiDefect: " + AvAmResult.SomaWeiDefect);
                        }

                        listaAvAmResult.Add(AvAmResult);
                    }
                } 

                #endregion

                listLevel1Retorno.Add(new RetornoLevel1() { idLevel1 = level1Id, avAmPorLevel1 = listaAvAmResult });

                //foreach (var debug in listaAvAmResult)
                //    Debug.Write("\n" + debug.AvAm + " - " + debug.SomaWeiDefect);

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