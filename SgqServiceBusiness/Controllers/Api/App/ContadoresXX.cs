using ADOFactory;
using Dominio;
using DTO;
using DTO.Helpers;
using ServiceModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace SgqServiceBusiness.Api.App
{
    public class ContadoresXX
    {
        public List<DefeitosPorAmostra> GetContadoresXX(SgqDbDevEntities db, int level1Id, int ParCompany_Id)
        {

            /*Busca Items parametrizados a serem calculados*/
            //var retorno = new List<RetornoLucas>();
            var level2 = db.ParLevel3Level2Level1.Where(r => r.ParLevel1_Id == level1Id && r.Active && r.ParLevel3Level2.ParLevel2.IsActive).Select(r => r.ParLevel3Level2.ParLevel2).Include("ParFrequency").Distinct();
            //var collectionLevel2PorFrequenciaLevel1 = GetCollectionLevel2PelaFrquencia(level2, db).OrderBy(r=>r.EvaluationNumber).ThenBy(r=>r.Sample);

            var level1 = db.ParLevel1.FirstOrDefault(r => r.Id == level1Id);
            //var todosLevel1 = collectionLevel2PorFrequenciaLevel1.Select(r => r.ParLevel1_Id).Distinct();
            //var SidesWithDefectsTotal = 0;


            var newReturn = new List<DefeitosPorAmostra>();

            string dataInicio = string.Empty;
            string dataFim = string.Empty;

            SyncServiceApiController.getFrequencyDate(level1.ParFrequency_Id??0, DateTime.Now /*MOCK DEV> OBS:PRECISA PASSaR COMO REFERENCIA PARA RETROATIVO*/, ref dataInicio, ref dataFim);


            var sql = "select" +
                        "\n EvaluationNumber, [Sample], " +
                        //" SUM(WeiDefects) as WeiDefects,"+
                        "\n CASE WHEN SUM(WeiDefects) > 0 THEN 1 ELSE 0 END as WeiDefects," +
                        "\n [Period], [Shift]" +
                        "\n from CollectionLevel2" +
                        "\n where UnitId = " + ParCompany_Id + " and ParLevel1_Id = "+ level1Id + " and CollectionDate <= '"+dataInicio+ " 23:59:59' and CollectionDate >= '" + dataFim + " 00:00:00' and WeiDefects > 0" +
                        "\n group by EvaluationNumber, [Sample], [Period], [Shift]";

            List<DefeitosPorAmostra> results = new List<DefeitosPorAmostra>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                results = factory.SearchQuery<DefeitosPorAmostra>(sql).ToList();
            }

            newReturn.AddRange(results);
            

            return newReturn;
            //foreach (var evaluation in collectionLevel2PorFrequenciaLevel1.Select(r=>r.EvaluationNumber).Distinct())
            //{
            //    foreach (var sample in collectionLevel2PorFrequenciaLevel1.Select(r => r.Sample).Distinct())
            //    {
            //        var elementoDalista = new RetornoLucas();
            //        elementoDalista.Defects = collectionLevel2PorFrequenciaLevel1.Where(r => r.EvaluationNumber == evaluation && r.Sample == sample).Sum(r => r.WeiDefects).GetValueOrDefault();
            //        elementoDalista.evaluation = evaluation;
            //        elementoDalista.sample = sample;
            //        elementoDalista.IdLevel1 = level1Id;
            //        SidesWithDefectsTotal += elementoDalista.Defects > 0 ? 1 : 0;
            //        elementoDalista.SidesWithDefects = SidesWithDefectsTotal;

            //        if (elementoDalista.Defects > 0)
            //            retorno.Add(elementoDalista);
            //    }
            //}

            //foreach (var i in retorno)
            //{
            //    Debug.Write("\n IdLevel1: " + i.IdLevel1); 
            //    Debug.Write("\n evaluation: " + i.evaluation); 
            //    Debug.Write("\n sample: " + i.sample); 
            //    Debug.Write("\n Defects: " + i.Defects); 
            //    Debug.Write("\n SidesWithDefects: " + i.SidesWithDefects);
            //    Debug.Write("\n\n -------------- \n\n");
            //}

            //return retorno;
        }
    }
}