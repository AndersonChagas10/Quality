using Dominio;
using SgqSystem.Helpers;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/CartasCep")]
    public class CartasCepApiController : ApiController
    {

        [HttpPost]
        [Route("Get")]
        public CartasCepResultSet GetApontamentosDiarios([FromBody] FormularioParaRelatorioViewModel form)
        {

            CommonLog.SaveReport(form, "Report_Cartas_Cep");

            CartasCepResultSet _mockcartasCep = new CartasCepResultSet();

            List<Cep> _ceps = new List<Cep>();

            bool x = false;
            var query1 = "";

            //decimal[] dados1 = { 4.11M, 4.2M, 3.93M, 3.24M, 3.5M };
            //string[] dataAv1 = { "10/05/2015 18:33:56", "10/05/2015 18:34:30", "10/05/2015 18:35:04", "10/05/2015 18:35:36", "10/05/2015 18:36:21" };
            //decimal[] lciData1 = { 3.05M, 3.05M, 3.05M, 3.05M, 3.05M };
            ////decimal Ics1;
            //decimal[] lcsData1 = { 4.25M, 4.25M, 4.25M, 4.25M, 4.25M };
            ////decimal media1;
            //int[] nivel1Max1 = { 1, 1, 1, 1, 1 };
            //int[] nivel1Min1 = { 0, 0, 0, 0, 0 }; //LIE Limite Inferior
            //int[] nivel2Max1 = { 2, 2, 2, 2, 2 };
            //int[] nivel2Min1 = { 1, 1, 1, 1, 1 };
            //int[] nivel3Max1 = { 3, 3, 3, 3, 3 }; //LSE Limite Superior
            //int[] nivel3Min1 = { 2, 2, 2, 2, 2 };

            if (x)
            {
                    //Carta X
                    query1 = " " +
                    "\n DECLARE @N INT = 1--CASO X, ENTÃO 1                                                                                          " +
                    "\n DECLARE @MEDIA DECIMAL(30, 5)                                                                                                " +
                    "\n DECLARE @MEDIAAM DECIMAL(30, 5) = 1                                                                                             " +
                    "\n DECLARE @UCL DECIMAL(30, 5)                                                                                                  " +
                    "\n DECLARE @LCL DECIMAL(30, 5)                                                                                                  " +
                    "\n DECLARE @LSC DECIMAL(30, 5)                                                                                                  " +
                    "\n DECLARE @LIC DECIMAL(30, 5)                                                                                                  " +
                    "\n                                                                                                                              " +
                    "\n                                                                                                                              " +
                    "\n DECLARE @DATA_INI DATE = '" + form._dataInicioSQL + "'                                                                       " +
                    "\n DECLARE @DATA_FIM DATE = '" + form._dataFimSQL + "'                                                                          " +
                    "\n DECLARE @UNIDADE INT = " + form.unitId + "                                                                                   " +
                    "\n DECLARE @INDICADOR INT = " + form.level1Id + "                                                                               " +
                    "\n DECLARE @MONITORAMENTO INT = " + form.level2Id + "                                                                           " +
                    "\n DECLARE @TAREFA INT = " + form.level3Id + "                                                                                  " +
                    "\n                                                                                                                              " +
                    "\n                                                                                                                              " +
                    "\n /* Média */                                                                                                                  " +
                    "\n --select @MEDIA = AVG(media.VALOR)  from                                                                                     " +
                    "\n select @MEDIA = AVG(media.DEFEITOS)  from                                                          " +
                    "\n (                                                                                                                            " +
                    "\n /* vetor CEP */                                                                                                              " +
                    "\n SELECT                                                                                                                       " +
                    "\n TB1.*,                                                                                                                       " +
                    "\n CASE WHEN TB2.defeitos IS NULL THEN NULL                                                                                     " +
                    "\n       WHEN TB1.defeitos IS NULL THEN TB2.defeitos                                                                            " +
                    "\n                                                                                                                              " +
                    "\n       ELSE abs(TB2.defeitos - TB1.defeitos) end AM                                                                           " +
                    "\n FROM                                                                                                                         " +
                    "\n (                                                                                                                            " +
                    "\n select                                                                                                                       " +
                    "\n  ROW_NUMBER() OVER(ORDER BY CONVERT(DATE, CL.CollectionDate) ASC) AS Row#                                                    " +
                    "\n , CL.CollectionDate DATA                                                                                                     " +
                    "\n                                                                                                                              " +
                    "\n , ISNULL((cast(R3.IntervalMin as decimal(30, 10))), 0) IntervalMin                                                        " +
                    "\n , ISNULL((cast(R3.IntervalMax as decimal(30, 10))), 0) IntervalMax                                                        " +
                    "\n                                                                                                                              " +
                    "\n ,ISNULL((cast(R3.Value as decimal(30, 10))),0) DEFEITOS                                                                   " +
                    "\n ,ISNULL((cast(R3.Value as decimal(30, 10))) / @N,0) AS VALOR                                                              " +
                    "\n from CollectionLevel2 CL                                                                                                     " +
                    "\n inner                                                                                                                        " +
                    "\n join Result_Level3 R3                                                                                                        " +
                    "\n on R3.CollectionLevel2_Id = CL.id                                                                                            " +
                    "\n where CAST(CL.CollectionDate as date) BETWEEN @DATA_INI and @DATA_FIM                                                             " +
                    "\n and CL.UnitId = @UNIDADE                                                                                                     " +
                    "\n and CL.ParLevel1_Id = @INDICADOR                                                                                             " +
                    "\n and CL.ParLevel2_Id = @MONITORAMENTO                                                                                         " +
                    "\n and R3.ParLevel3_Id = @TAREFA                                                                                                " +
                    "\n -- group by CL.CollectionDate                                                                                                   " +
                    "\n ) TB1                                                                                                                        " +
                    "\n FULL JOIN                                                                                                                    " +
                    "\n (                                                                                                                            " +
                    "\n select                                                                                                                       " +
                    "\n  ROW_NUMBER() OVER(ORDER BY CONVERT(DATE, CL.CollectionDate) ASC) AS Row#                                                    " +
                    "\n , CL.CollectionDate DATA                                                                                                     " +
                    "\n                                                                                                                              " +
                    "\n                                                                                                                              " +
                    "\n , ISNULL((cast(R3.IntervalMin as decimal(30, 10))), 0) IntervalMin                                                        " +
                    "\n ,ISNULL((cast(R3.IntervalMax as decimal(30, 10))),0) IntervalMax                                                          " +
                    "\n                                                                                                                              " +
                    "\n ,ISNULL((cast(R3.Value as decimal(30, 10))),0) DEFEITOS                                                                   " +
                    "\n ,ISNULL((cast(R3.Value as decimal(30, 10))) / @N,0) AS VALOR                                                              " +
                    "\n from CollectionLevel2 CL                                                                                                     " +
                    "\n inner                                                                                                                        " +
                    "\n join Result_Level3 R3                                                                                                        " +
                    "\n on R3.CollectionLevel2_Id = CL.id                                                                                            " +
                    "\n where CAST(CL.CollectionDate as date) BETWEEN @DATA_INI and @DATA_FIM                                                        " +
                    "\n and CL.UnitId = @UNIDADE                                                                                                     " +
                    "\n and CL.ParLevel1_Id = @INDICADOR                                                                                             " +
                    "\n and CL.ParLevel2_Id = @MONITORAMENTO                                                                                         " +
                    "\n and R3.ParLevel3_Id = @TAREFA                                                                                                " +
                    "\n -- group by CL.CollectionDate                                                                                                   " +
                    "\n ) TB2                                                                                                                        " +
                    "\n ON TB1.Row# = (TB2.Row#)                                                                                                     " +
                    "\n ) media                                                                                                                      " +
                    "\n                                                                                                                              " +
                    "\n                                                                                                                              " +
                    "\n SET @UCL = @MEDIA + (3 * (@MEDIAAM / 1.128))                                                                                 " +
                    "\n                                                                                                                              " +
                    "\n SET @LCL = @MEDIA - (3 * (@MEDIAAM / 1.128))                                                                                 " +
                    "\n                                                                                                                              " +
                    "\n SET @LSC = 3.267 * @MEDIAAM                                                                                                  " +
                    "\n                                                                                                                              " +
                    "\n SET @LIC = 0                                                                                                                 " +
                    "\n                                                                                                                              " +
                    "\n --SELECT @MEDIA AS \"X LM\", @MEDIAAM as \"AM LM\", @UCL as UCL, @LCL as LCL, @LSE as LSE, @LIE as LIE, @LSC as LSC              " +
                    "\n                                                                                                                              " +
                    "\n                                                                                                                              " +
                    "\n /* vetor CEP */                                                                                                              " +
                    "\n SELECT                                                                                                                       " +
                    "\n (TB1.DATA) as Data,                                                                                                                    " +
                    "\n TB1.Defeitos AS VALOR,                                                                                                                " +
                    "\n CASE WHEN TB2.defeitos IS NULL THEN 0                                                                                        " +
                    "\n       WHEN TB1.defeitos IS NULL THEN TB2.defeitos                                                                            " +
                    "\n       ELSE abs(TB2.defeitos - TB1.defeitos) end AM,                                                                          " +
                    "\n                                                                                                                              " +
                    "\n @MEDIA AS pbar, @MEDIAAM as \"AM LM\", @UCL as UCL, @LCL as LCL, TB1.IntervalMax as LSE, TB1.IntervalMin as LIE, @LSC as LSC " +
                    "\n                                                                                                                              " +
                    "\n FROM                                                                                                                         " +
                    "\n (                                                                                                                            " +
                    "\n select                                                                                                                       " +
                    "\n  ROW_NUMBER() OVER(ORDER BY CONVERT(DATE, CL.CollectionDate) ASC) AS Row#                                                    " +
                    "\n , CL.CollectionDate DATA                                                                                                     " +
                    "\n                                                                                                                              " +
                    "\n , ISNULL((cast(REPLACE(R3.IntervalMin,',','.')  as decimal(30, 10))), 0) IntervalMin                                                        " +
                    "\n ,ISNULL((cast(REPLACE(R3.IntervalMax,',','.')  as decimal(30, 10))),0) IntervalMax                                                          " +
                    "\n                                                                                                                              " +
                    "\n ,ISNULL((cast(R3.Value as decimal(30, 10))),0) DEFEITOS                                                                   " +
                    "\n ,ISNULL((cast(R3.Value as decimal(30, 10))) / @N,0) AS VALOR                                                              " +
                    "\n from CollectionLevel2 CL                                                                                                     " +
                    "\n inner                                                                                                                        " +
                    "\n join Result_Level3 R3                                                                                                        " +
                    "\n on R3.CollectionLevel2_Id = CL.id                                                                                            " +
                    "\n where CAST(CL.CollectionDate as date) BETWEEN @DATA_INI and @DATA_FIM                                                             " +
                    "\n and CL.UnitId = @UNIDADE                                                                                                     " +
                    "\n and CL.ParLevel1_Id = @INDICADOR                                                                                             " +
                    "\n and CL.ParLevel2_Id = @MONITORAMENTO                                                                                         " +
                    "\n and R3.ParLevel3_Id = @TAREFA                                                                                                " +
                    "\n -- group by CL.CollectionDate                                                                                                   " +
                    "\n ) TB1                                                                                                                        " +
                    "\n FULL JOIN                                                                                                                    " +
                    "\n (                                                                                                                            " +
                    "\n select                                                                                                                       " +
                    "\n  ROW_NUMBER() OVER(ORDER BY CONVERT(DATE, CL.CollectionDate) ASC) AS Row#                                                    " +
                    "\n , CL.CollectionDate DATA                                                                                                     " +
                    "\n                                                                                                                              " +
                    "\n , ISNULL((cast(REPLACE(R3.IntervalMin,',','.')  as decimal(30, 10))), 0) IntervalMin                                                        " +
                    "\n ,ISNULL((cast(REPLACE(R3.IntervalMax,',','.')  as decimal(30, 10))),0) IntervalMax                                                          " +
                    "\n                                                                                                                              " +
                    "\n ,ISNULL((cast(R3.Value as decimal(30, 10))),0) DEFEITOS                                                                   " +
                    "\n ,ISNULL((cast(R3.Value as decimal(30, 10))) / @N,0) AS VALOR                                                              " +
                    "\n from CollectionLevel2 CL                                                                                                     " +
                    "\n inner                                                                                                                        " +
                    "\n join Result_Level3 R3                                                                                                        " +
                    "\n on R3.CollectionLevel2_Id = CL.id                                                                                            " +
                    "\n where CAST(CL.CollectionDate as date) BETWEEN @DATA_INI and @DATA_FIM                                                             " +
                    "\n and CL.UnitId = @UNIDADE                                                                                                     " +
                    "\n and CL.ParLevel1_Id = @INDICADOR                                                                                             " +
                    "\n and CL.ParLevel2_Id = @MONITORAMENTO                                                                                         " +
                    "\n and R3.ParLevel3_Id = @TAREFA                                                                                                " +
                    "\n -- group by CL.CollectionDate                                                                                                   " +
                    "\n ) TB2                                                                                                                        " +
                    "\n ON TB1.Row# = (TB2.Row#)                                                                                                     ";
                                                                                                                                                     


            }
            else
            {

                //Carta P

                query1 = "" +
                "\n DECLARE @N INT = 120 --CASO X, ENTÃO 1                                                                                                   " +
                "\n DECLARE @LimiteSuperiorEspecificacao DECIMAL(30, 5) = 2.5                                                                               " +
                "\n DECLARE @LimiteInferiorEspecificacao DECIMAL(30, 5) = 1                                                                               " +
                "\n DECLARE @MEDIA DECIMAL(30, 5)                                                                                                           " +
                "\n DECLARE @LimiteSuperiorControle DECIMAL(30, 5)                                                                                          " +
                "\n DECLARE @LimiteInferiorControle DECIMAL(30, 5)                                                                                          " +
                "\n                                                                                                                                         " +

                "\n DECLARE @DATA_INI DATE = '" + form._dataInicioSQL + "'                                                                       " +
                "\n DECLARE @DATA_FIM DATE = '" + form._dataFimSQL + "'                                                                          " +
                "\n DECLARE @UNIDADE INT = " + form.unitId + "                                                                                   " +
                "\n DECLARE @INDICADOR INT = " + form.level1Id + "                                                                               " +
                "\n DECLARE @MONITORAMENTO INT = " + form.level2Id + "                                                                           " +

                "\n select top 1                                                                                                                 " +
                "\n                                                                                                                              " +
                "\n  @LimiteSuperiorEspecificacao = CASE WHEN L1.IsRuleConformity = 0 THEN PercentValue ELSE 0 END,                              " +
                "\n  @LimiteInferiorEspecificacao = CASE WHEN L1.IsRuleConformity = 0 THEN 0 ELSE PercentValue END                               " +
                "\n                                                                                                                              " +
                "\n from parGoal G                                                                                                               " +
                "\n  INNER JOIN ParLevel1 L1                                                                                                     " +
                "\n  ON L1.Id = G.ParLevel1_Id                                                                                                   " +
                "\n  where CAST(G.AddDate AS DATE) <= @DATA_FIM                                                                                  " +
                "\n  AND(G.ParCompany_id = @UNIDADE OR G.ParCompany_Id is null)                                                                  " +
                "\n  AND G.ParLevel1_Id = @INDICADOR                                                                                             " +
                "\n  ORDER BY G.Adddate desc, G.IsActive desc, G.ParCompany_Id desc                                                              " +


                "\n                                                                                                                                         " +
                "\n /* Média */                                                                                                                             " +
                "\n select @MEDIA = AVG(media.VALOR) * 100 from                                                                                                  " +
                "\n --select @MEDIA = AVG(media.DEFEITOS)  from                                                                                             " +
                "\n (                                                                                                                                       " +
                "\n select sum(WeiDefects) DEFEITOS, SUM(WeiDefects) / @N AS VALOR from CollectionLevel2                                                          " +

                "\n where CAST(CollectionDate as date) BETWEEN @DATA_INI AND @DATA_FIM                                                                      " +
                "\n AND UnitId = @UNIDADE                                                                                                                   " +
                "\n AND ParLevel1_Id = @INDICADOR                                                                                                           ";


                if (form.level2Id != 0)
                {
                    query1 += "\n AND ParLevel2_Id = @MONITORAMENTO                                                                                         ";
                }

                query1 += "\n group by (CAST(CollectionDate AS DATE))                                                                                                 " +

                "\n ) media                                                                                                                                 " +
                "\n                                                                                                                                         " +
                "\n --SELECT @MEDIA                                                                                                                         " +
                "\n                                                                                                                                         " +
                "\n SET @LimiteSuperiorControle = @MEDIA + 3 * SQRT(@MEDIA * (100 - @MEDIA) / @N)                                                             " +
                "\n                                                                                                                                         " +
                "\n SET @LimiteInferiorControle = CASE WHEN @MEDIA - 3 * SQRT(@MEDIA * (100 - @MEDIA) / @N) < 0 THEN 0 ELSE @MEDIA - 3 * SQRT(@MEDIA * (100 - @MEDIA) / @N) END                                                              " +
                "\n                                                                                                                                         " +
                "\n --SELECT @MEDIA AS[p - bar], @UCL as UCL, @LCL as LCL, @LSE as LSE, @LIE as LIE                                                         " +
                "\n                                                                                                                                         " +
                "\n /* vetor CEP */                                                                                                                         " +
                "\n select                                                                                                                                  " +
                "\n CAST(CollectionDate AS DATE) as Data,                                                                                                   " +
                "\n @MEDIA AS[pbar],                                                                                                                        " +
                "\n @LimiteSuperiorControle as UCL,                                                                                                         " +
                "\n @LimiteInferiorControle as LCL,                                                                                                         " +
                "\n @LimiteSuperiorEspecificacao as LSE,                                                                                                    " +
                "\n @LimiteInferiorEspecificacao as LIE,                                                                                                    " +
                "\n ISNULL(SUM(WeiDefects) / @N * 100, 0) AS VALOR                                                                                                   " +
                "\n from CollectionLevel2                                                                                                                   " +
                "\n where CAST(CollectionDate as date) BETWEEN @DATA_INI AND @DATA_FIM                                                                      " +
                "\n AND UnitId = @UNIDADE                                                                                                                   " +
                "\n AND ParLevel1_Id = @INDICADOR                                                                                                           ";

                if (form.level2Id != 0) {
                    query1 += "\n AND ParLevel2_Id = @MONITORAMENTO                                                                                         ";
                }

                query1 += "\n group by (CAST(CollectionDate AS DATE))                                                                                       " +
                "\n ORDER BY 1 ";



            }
            using (var db = new SgqDbDevEntities())
            {
                _ceps = db.Database.SqlQuery<Cep>(query1).ToList();
            }

            var dados1 = new List<decimal>();
            var dataAv1 = new List<DateTime>();
            var lciData1 = new List<decimal>();
            //decimal Ics1;
            var lcsData1 = new List<decimal>();
            //decimal media1;                  
            var nivel1Max1 = new List<decimal>();
            var nivel1Min1 = new List<decimal>();
            var nivel2Max1 = new List<decimal>();
            var nivel2Min1 = new List<decimal>();
            var nivel3Max1 = new List<decimal>();
            var nivel3Min1 = new List<decimal>();
            var media = new List<decimal>();
            var lci = new List<decimal>();
            var lcs = new List<decimal>();

            foreach (var cep in _ceps)
            {
                dados1.Add(cep.VALOR);
                dataAv1.Add(cep.Data);
                lciData1.Add(cep.LCL);
                lcsData1.Add(cep.UCL);
                lci.Add(cep.LIE);
                lcs.Add(cep.LSE);
                //nivel1Max1.Add(cep);
                //nivel1Min1.Add(cep);
                //nivel2Max1.Add(cep);
                //nivel2Min1.Add(cep);
                //nivel3Max1.Add(cep);
                //nivel3Min1.Add(cep);
                media.Add(cep.pbar);

            }

            //decimal[] dados1 = { 4.11M, 4.2M, 3.93M, 3.24M, 3.5M };
            //string[] dataAv1 = { "10/05/2015 18:33:56", "10/05/2015 18:34:30", "10/05/2015 18:35:04", "10/05/2015 18:35:36", "10/05/2015 18:36:21" };
            //decimal[] lciData1 = { 3.05M, 3.05M, 3.05M, 3.05M, 3.05M };
            ////decimal Ics1;
            //decimal[] lcsData1 = { 4.25M, 4.25M, 4.25M, 4.25M, 4.25M };
            ////decimal media1;
            //int[] nivel1Max1 = { 1, 1, 1, 1, 1 };
            //int[] nivel1Min1 = { 0, 0, 0, 0, 0 }; //LIE Limite Inferior
            //int[] nivel2Max1 = { 2, 2, 2, 2, 2 };
            //int[] nivel2Min1 = { 1, 1, 1, 1, 1 };
            //int[] nivel3Max1 = { 3, 3, 3, 3, 3 }; //LSE Limite Superior
            //int[] nivel3Min1 = { 2, 2, 2, 2, 2 };

            _mockcartasCep.dados = dados1; //Valor
            _mockcartasCep.dataAv = dataAv1;  //Data
            _mockcartasCep.lci = lci;//lci.Average(item => item);// 3.05M; //LIE 
            _mockcartasCep.lciData = lciData1; //LCL
            _mockcartasCep.lcs = lcs;//lcs.Average(item => item); //4.25M; //LSE 
            _mockcartasCep.lcsData = lcsData1; //UCL
            if (media.Count > 0) //Para não dar estouro na hora de pegar a media
                _mockcartasCep.media = media[0];//media.Average(item => item); //3.648275862068966M;  //Pbar
            _mockcartasCep.nivel1Max = nivel1Max1;
            _mockcartasCep.nivel1Min = nivel1Min1;
            _mockcartasCep.nivel2Max = nivel2Max1;
            _mockcartasCep.nivel2Min = nivel2Min1;
            _mockcartasCep.nivel3Max = nivel3Max1;
            _mockcartasCep.nivel3Min = nivel3Min1;
            _mockcartasCep.amostras = null;

            //var query = "Select";

            //var query = new ApontamentosDiariosResultSet().Select(form);
            //_list = db.Database.SqlQuery<ApontamentosDiariosResultSet>(query).ToList();
            //return _list;

            return _mockcartasCep;
        }

    }

    public class CartasCepResultSet
    {
        //public List<Chartdata> chartData;
        public List<decimal> amostras;
        public List<decimal> dados;
        public List<DateTime> dataAv;
        public List<decimal> lci;
        public List<decimal> lciData;
        public List<decimal> lcs;
        public List<decimal> lcsData;
        public decimal media;
        public List<decimal> nivel1Max;
        public List<decimal> nivel1Min;
        public List<decimal> nivel2Max;
        public List<decimal> nivel2Min;
        public List<decimal> nivel3Max;
        public List<decimal> nivel3Min;
    }

    public class Cep
    {
        public decimal pbar { get; set; }
        public decimal UCL { get; set; }
        public decimal LCL { get; set; }
        public decimal LSE { get; set; }
        public decimal LIE { get; set; }
        public decimal VALOR { get; set; }
        public DateTime Data { get; set; }
    }


    //public class Chartdata
    //{
    //    public List<string> value;
    //    public decimal height;
    //    public string nivel1Maximo;
    //    public string nivel1Minimo;
    //    public string nivel2Maximo;
    //    public string nivel2Minimo;
    //    public string nivel3Maximo;
    //    public string nivel3Minimo;
    //}
}
