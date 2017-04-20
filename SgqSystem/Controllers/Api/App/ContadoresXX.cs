using Dominio;
using DTO;
using DTO.Helpers;
using SgqSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace SgqSystem.Controllers.Api.App
{
    public class ContadoresXX
    {

        public List<RetornoLucas> GetContadoresXX(SgqDbDevEntities db, int level1Id, int period = 0)
        {

            /*Busca Items parametrizados a serem calculados*/
            var retorno = new List<RetornoLucas>();
            var level2 = db.ParLevel3Level2Level1.Where(r => r.ParLevel1_Id == level1Id && r.Active && r.ParLevel3Level2.ParLevel2.IsActive).Select(r => r.ParLevel3Level2.ParLevel2).Include("ParFrequency").Distinct();
            var collectionLevel2PorFrequenciaLevel1 = GetCollectionLevel2PelaFrquencia(level2, db).OrderBy(r=>r.EvaluationNumber).ThenBy(r=>r.Sample);
            var todosLevel1 = collectionLevel2PorFrequenciaLevel1.Select(r => r.ParLevel1_Id).Distinct();
            var SidesWithDefectsTotal = 0;

            foreach (var evaluation in collectionLevel2PorFrequenciaLevel1.Select(r=>r.EvaluationNumber).Distinct())
            {
                foreach (var sample in collectionLevel2PorFrequenciaLevel1.Select(r => r.Sample).Distinct())
                {
                    var elementoDalista = new RetornoLucas();
                    elementoDalista.Defects = collectionLevel2PorFrequenciaLevel1.Where(r => r.EvaluationNumber == evaluation && r.Sample == sample).Sum(r => r.WeiDefects).GetValueOrDefault();
                    elementoDalista.evaluation = evaluation;
                    elementoDalista.sample = sample;
                    elementoDalista.IdLevel1 = level1Id;
                    SidesWithDefectsTotal += elementoDalista.Defects > 0 ? 1 : 0;
                    elementoDalista.SidesWithDefects = SidesWithDefectsTotal;

                    if (elementoDalista.Defects > 0)
                        retorno.Add(elementoDalista);
                }
            }

            foreach (var i in retorno)
            {
                Debug.Write("\n IdLevel1: " + i.IdLevel1); 
                Debug.Write("\n evaluation: " + i.evaluation); 
                Debug.Write("\n sample: " + i.sample); 
                Debug.Write("\n Defects: " + i.Defects); 
                Debug.Write("\n SidesWithDefects: " + i.SidesWithDefects);
                Debug.Write("\n\n -------------- \n\n");
            }

            return retorno;
        }

        private List<CollectionLevel2> GetCollectionLevel2PelaFrquencia(IQueryable<ParLevel2> level2, SgqDbDevEntities db)
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

    }

    public class RetornoLucas
    {
        internal int sample;
        internal int? evaluation;
        public int IdLevel1 { get; set; }
        public decimal SidesWithDefects { get; set; }
        public decimal Defects { get; set; }
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
        public decimal SomaWeiDefectsPorAv { get; set; }
        public decimal SomaWeiDefectsPorAvAcumulada { get; set; }
        public int Am { get; internal set; }
        public int Av { get; internal set; }
    }
}