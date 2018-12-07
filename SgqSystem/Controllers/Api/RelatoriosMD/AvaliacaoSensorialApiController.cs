using Dominio;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosMD
{
    [RoutePrefix("api/AvaliacaoSensorial")]
    public class AvaliacaoSensorialApiController : ApiController
    {

        [HttpPost]
        [Route("Get")]
        public Retorno Get([FromBody] FormularioParaRelatorioViewModel form)
        {
            var retorno = new Retorno();

            GetCabecalhoValues(form, ref retorno);

            GetColetaValues(form, ref retorno);

            retorno.Shift = form.shift;

            return retorno;
        }

        public Retorno GetCabecalhoValues(FormularioParaRelatorioViewModel form, ref Retorno retorno)
        {
            var sql = $@"SELECT DISTINCT
                        	phf.Name
                           ,CASE
                        		WHEN C2XHF.ParFieldType_Id = 1 THEN pmv.Name
                                WHEN C2XHF.ParFieldType_Id = 2 THEN P.cNmProduto
                        		ELSE C2XHF.value
                        	END AS 'Value'
                           ,C2XHF.ParHeaderField_Id AS Id
                        FROM CollectionLevel2XParHeaderField C2XHF
                        INNER JOIN CollectionLevel2 c2
                        	ON c2.id = C2XHF.CollectionLevel2_Id
                        INNER JOIN ParHeaderField phf
                        	ON phf.Id = C2XHF.ParHeaderField_Id
                        LEFT JOIN ParMultipleValues pmv
                        	ON C2XHF.value = CAST(PMV.Id AS VARCHAR(500))
                        LEFT JOIN Produto P
                        	ON CAST(P.nCdProduto AS VARCHAR(500)) = CAST(C2XHF.value AS VARCHAR(500))
                        WHERE c2.parlevel1_id = { form.level1Id }
                        and c2.ParLevel2_Id = { form.level2Id }
                        AND c2.UnitId = { form.unitId }
                        AND C2.Shift = { form.shift }
                        AND c2.EvaluationNumber = 1
                        AND CAST(c2.CollectionDate AS DATE) = '{ form._dataInicioSQL }'
                        --AND C2XHF.ParFieldType_Id <> 2";

            using (var db = new SgqDbDevEntities())
            {
                retorno.Cabecalhos = db.Database.SqlQuery<Cabecalho>(sql).ToList();
            }

            return retorno;
        }

        public Retorno GetColetaValues(FormularioParaRelatorioViewModel form, ref Retorno retorno)
        {
            var sql = $@"SELECT
                    	pd.nCdProduto AS Produto_Id
                       ,pd.cNmProduto AS Produto
                       ,c2.CollectionDate
                       ,p2.Id AS ParLevel2_Id
                       ,p2.Name AS ParLevel2
                       ,c2.EvaluationNumber AS Avaliacao
                       ,c2.Sample AS Amostra
                       ,r3.ParLevel3_Id
                       ,r3.ParLevel3_Name AS ParLevel3
                       ,L3G.Name AS Level3Group
                       ,r3.Id
                       ,r3.IsConform
                       ,r3.IsNotEvaluate
                       ,r3.value
                       ,L3V.ParLevel3InputType_Id
                       ,L3V.Id as ParLevel3Value_Id
                       ,IIF(L3V.IntervalMin is null OR L3V.IntervalMax is null, '', Concat(concat(Cast(L3V.IntervalMin as INT), ', '),Cast(L3V.IntervalMax as INT))) as Intervalo
                       ,REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(L3V.DynamicValue, 'calcularSensorial(', ''), ')', ''), '[{{', ''), '}}]', ''), '}},{{', ',') AS DynamicValue
                    FROM CollectionLevel2 C2
                    INNER JOIN CollectionLevel2XParHeaderField C2XHF
                    	ON c2.Id = C2XHF.CollectionLevel2_Id
                    INNER JOIN Produto pd
                    	ON C2XHF.value = CAST(pd.nCdProduto AS VARCHAR(500))
                    INNER JOIN Result_Level3 R3
                    	ON R3.CollectionLevel2_Id = c2.Id
                    INNER JOIN parlevel2 P2
                    	ON c2.ParLevel2_Id = p2.Id
                    INNER JOIN ParLevel3Level2 L3L2
                    	ON r3.ParLevel3_Id = L3L2.ParLevel3_Id
                    		AND c2.ParLevel2_Id = L3L2.ParLevel2_Id
                    		AND (c2.UnitId = L3L2.ParCompany_Id
                    			OR L3L2.ParCompany_Id IS NULL)
                    LEFT JOIN ParLevel3Group L3G
                    	ON L3G.Id = L3L2.ParLevel3Group_Id
                    LEFT JOIN ParLevel3Value L3V
                    	ON L3V.ParLevel3_Id = r3.ParLevel3_Id
                    		AND L3V.IsActive = 1
                    WHERE 1 = 1
                    AND CAST(c2.CollectionDate AS DATE) = '{form._dataInicioSQL}'
                    AND c2.parlevel1_id = { form.level1Id }
                    AND c2.ParLevel2_Id = { form.level2Id }
                    AND c2.UnitId = { form.unitId }
                    AND C2.Shift = { form.shift }
                    AND c2.EvaluationNumber = 1
                    --AND C2XHF.ParFieldType_Id = 2
                    ORDER BY L3G.Name DESC, c2.EvaluationNumber, c2.Sample, C2.CollectionDate DESC, DynamicValue";


            using (var db = new SgqDbDevEntities())
            {
                retorno.Coletas = db.Database.SqlQuery<Coleta>(sql).ToList();
            }

            return retorno;

        }

        public class Retorno
        {
            public List<Cabecalho> Cabecalhos { get; set; }
            public List<Coleta> Coletas { get; set; }
            public int Shift { get; set; }
        }

        public class Coleta
        {
            public DateTime CollectionDate { get; set; }
            public int ParLevel2_Id { get; set; }
            public string ParLevel2 { get; set; }
            public int Avaliacao { get; set; }
            public int Amostra { get; set; }
            public int ParLevel3_Id { get; set; }
            public string ParLevel3 { get; set; }
            public string Level3Group { get; set; }
            public int Id { get; set; }
            public bool IsConform { get; set; }
            public bool IsNotEvaluate { get; set; }
            public string Value { get; set; }
            public int ParLevel3InputType_Id { get; set; }
            public string DynamicValue { get; set; }
            public int ParLevel3Value_Id { get; set; }
            public string Intervalo { get; set; }
            public string Interval
            {
                get
                {
                    if (string.IsNullOrEmpty(Intervalo))
                    {
                        return Intervalo;
                    }
                    else
                    {

                        List<int> intervalo = new List<int>();
                        var inicio = int.Parse(Intervalo.Split(',')[0]);
                        var fim = int.Parse(Intervalo.Split(',')[1]);

                        for (int i = inicio; i <= fim; i++)
                        {
                            intervalo.Add(i);
                        }

                        return string.Join(",", intervalo);

                    }
                }
            }
        }

        public class Cabecalho
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}
