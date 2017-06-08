using Dominio;
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

            CartasCepResultSet _mockcartasCep = new CartasCepResultSet();

            List<Cep> _ceps = new List<Cep>();

            bool x = false;
            var query1 = "";

            if (x)
            {
                    //Carta X
                    query1 = "DECLARE @N INT = 1 --CASO X, ENTÃO 1  " +
                    "\n DECLARE @LSE DECIMAL(30, 5) = 0.8--Limite Superior de Especificação " +
                    "\n DECLARE @LIE DECIMAL(30, 5) = 0.2--Limite Inferior de Especificação " +
                    "\n DECLARE @MEDIA DECIMAL(30, 5)--Media " +
                    "\n DECLARE @MEDIAAM DECIMAL(30, 5) --Media Amostral " +
                    "\n DECLARE @UCL DECIMAL(30, 5) --Limite Superior de Controle " +
                    "\n DECLARE @LCL DECIMAL(30, 5) --Limite Inferior de Controle " +
                    "\n DECLARE @LSC DECIMAL(30, 5) " +
                    "\n DECLARE @LIC DECIMAL(30, 5) " +
                    
                    /* vetor CEP */
                    "\n --SELECT TB1.defeitos,  " +
                    "\n --CASE " +
                    "\n --         WHEN TB2.defeitos IS NULL THEN 0 " +
                    "\n --         WHEN TB1.defeitos IS NULL THEN TB2.defeitos " +
                    "\n --         ELSE Abs(TB2.defeitos - TB1.defeitos) " +
                    "\n --       END AM " +
                    "\n --FROM(SELECT Row_number() " +
                    "\n --                 OVER( " +
                    "\n --ORDER BY CONVERT(DATE, adddate) ASC) AS Row#,  " +
                    "\n --               CONVERT(DATE, adddate)                   DATA, " +
                    "\n --Isnull(Sum(defects), 0)                  DEFEITOS, " +
                    "\n --Isnull(Sum(defects) / @N, 0)             AS VALOR " +
                    "\n --        FROM   collectionlevel2 " +
                    "\n --        GROUP  BY CONVERT(DATE, adddate)) TB1 " +
                    "\n --       FULL JOIN(SELECT Row_number() " +
                    "\n --                           OVER( " +
                    "\n --ORDER BY CONVERT(DATE, adddate) ASC) AS Row#,  " +
                    "\n --                         CONVERT(DATE, adddate)                   DATA, " +
                    "\n --Isnull(Sum(defects), 0)                  DEFEITOS, " +
                    "\n --Isnull(Sum(defects) / @N, 0)             AS VALOR " +
                    "\n --                  FROM   collectionlevel2 " +
                    "\n --                  GROUP  BY CONVERT(DATE, adddate)) TB2 " +
                    "\n --              ON TB1.row# = ( TB2.row# + 1 )  " +
                    
                    "\n --/* Média */ " +
                    "\n --select @MEDIA = AVG(media.VALOR)  from " +
                    "\n SELECT @MEDIA = Avg(media.defeitos), " +
                    "\n        @MEDIAAM = Avg(media.am) " +
                    "\n FROM( " +
                    "\n /* vetor CEP */ " +
                    "\n        SELECT TB1.*, " +
                    "\n               CASE " +
                    "\n                 WHEN TB2.defeitos IS NULL THEN NULL " +
                    "\n                 WHEN TB1.defeitos IS NULL THEN TB2.defeitos " +
                    "\n                 ELSE Abs(TB2.defeitos - TB1.defeitos) " +
                    "\n               END AM " +
                    "\n         FROM(SELECT Row_number() " +
                    "\n                          OVER( " +
                    "\n                            ORDER BY CONVERT(DATE, adddate) ASC) AS Row#,  " +
                    "\n                        CONVERT(DATE, adddate)                   DATA, " +
                    "\n                        Isnull(Sum(defects), 0)                  DEFEITOS " +
                    "\n                 FROM   collectionlevel2 " +
                    "\n                 GROUP  BY CONVERT(DATE, adddate)) TB1 " +
                    "\n                FULL JOIN(SELECT Row_number() " +
                    "\n                                    OVER( " +
                    "\n                                      ORDER BY CONVERT(DATE, adddate) ASC) AS " +
                    "\n                                  Row#,  " +
                    "\n                                  CONVERT(DATE, adddate)                   DATA, " +
                    "\n                                  Isnull(Sum(defects), 0) " +
                    "\n                                  DEFEITOS " +
                    "\n                           FROM   collectionlevel2 " +
                    "\n                           GROUP  BY CONVERT(DATE, adddate)) TB2 " +
                    "\n                       ON TB1.row# = ( TB2.row# + 1 )) media  " +
                    
                    "\n SET @UCL = @MEDIA + (3 * (@MEDIAAM / 1.128)) " +
                    "\n SET @LCL = @MEDIA - (3 * (@MEDIAAM / 1.128)) " +
                    "\n SET @LSC = 3.267 * @MEDIAAM " +
                    "\n SET @LIC = 0 " +
                    
                    "\n --SELECT @MEDIA   AS 'X LM', " +
                    "\n --@MEDIAAM AS 'AM LM', " +
                    "\n --@UCL     AS UCL, " +
                    "\n --@LCL     AS LCL, " +
                    "\n --@LSE     AS LSE, " +
                    "\n --@LIE     AS LIE, " +
                    "\n --@LSC     AS LSC, " +
                    "\n --@LIC     AS LIC " +
                    
                    "\n --/* vetor CEP */  " +
                    "\n SELECT ISNULL(TB1.defeitos, 0 ) as VALOR, " +
                    "\n        --CASE " +
                    "\n        --  WHEN TB2.defeitos IS NULL THEN 0 " +
                    "\n        --  WHEN TB1.defeitos IS NULL THEN TB2.defeitos " +
                    "\n        --  ELSE Abs(TB2.defeitos - TB1.defeitos) " +
                    "\n        --END AM, " +
                    "\n        @MEDIA  AS pbar, --AS 'X LM', " +
                    "\n        --@MEDIAAM AS 'AM LM', " +
                    "\n        @UCL     AS UCL, " +
                    "\n        @LCL     AS LCL, " +
                    "\n        @LSE     AS LSE, " +
                    "\n        @LIE     AS LIE  " +
                    "\n        --@LSC     AS LSC, " +
                    "\n        --@LIC     AS LIC " +
                    "\n FROM(SELECT Row_number() " +
                    "\n                  OVER( " +
                    "\n                    ORDER BY CONVERT(DATE, adddate) ASC) AS Row#,  " +
                    "\n                CONVERT(DATE, adddate)                   DATA, " +
                    "\n                Isnull(Sum(defects), 0)                  DEFEITOS, " +
                    "\n                Isnull(Sum(defects) / @N, 0)             AS VALOR " +
                    "\n         FROM   collectionlevel2 " +
                    "\n         GROUP  BY CONVERT(DATE, adddate)) TB1 " +
                    "\n        FULL JOIN(SELECT Row_number() " +
                    "\n                            OVER( " +
                    "\n                              ORDER BY CONVERT(DATE, adddate) ASC) AS Row#,  " +
                    "\n                          CONVERT(DATE, adddate)                   DATA, " +
                    "\n                          Isnull(Sum(defects), 0)                  DEFEITOS, " +
                    "\n                          Isnull(Sum(defects) / @N, 0)             AS VALOR " +
                    "\n                   FROM   collectionlevel2 " +
                    "\n                   GROUP  BY CONVERT(DATE, adddate)) TB2 " +
                    "\n               ON TB1.row# = ( TB2.row# + 1 ) ";

            }
            else
            {

                //Carta P
                var FiltroUnidade = "";
                var FiltroLevel1 = "";
                var FiltroLevel2 = "";
                var FiltroLevel3 = "";

                if (form.unitId > 0)
                    FiltroUnidade = "\n and UnitId = " + form.unitId + "";


                if (form.level1Id > 0)
                    FiltroLevel1 = "\n and ParLevel1_Id = " + form.level1Id + "";

                if (form.level2Id > 0)
                    FiltroLevel2 = "\n and ParLevel2_Id = " + form.level2Id + "";

                if (form.level3Id > 0)
                    FiltroLevel3 = "\n and Result_Level3.CollectionLevel2_Id = CollectionLevel2.Id and Result_Level3.ParLevel3_Id = " + form.level3Id + "";


                query1 = "\n DECLARE @N INT = 120 --CASO X, ENTÃO 1 " +
                "\n DECLARE @LimiteSuperiorEspecificacao DECIMAL(30, 5) = 0.8 " +
                "\n DECLARE @LimiteInferiorEspecificacao DECIMAL(30, 5) = 0.2 " +
                "\n DECLARE @MEDIA DECIMAL(30, 5) " +
                "\n DECLARE @LimiteSuperiorControle DECIMAL(30, 5) " +
                "\n DECLARE @LimiteInferiorControle DECIMAL(30, 5) " +
                "\n DECLARE @ParCompany VARCHAR(153) " +
                "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + "'                                                                                                                                                                                                                    " +
                "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + "' " +
                "\n DECLARE @INDICADOR   VARCHAR(153) = " + form.level1Id +
                "\n DECLARE @MONITORAMENTO VARCHAR(153) = " + form.level2Id +
                "\n DECLARE @TAREFA VARCHAR(153) = " + form.level3Id +

                "\n /* Média */ " +
                "\n select @MEDIA = AVG(media.VALOR)  from " +
                "\n --select @MEDIA = AVG(media.DEFEITOS)  from " +
                "\n ( " +
                "\n select cast(AddDate as date) as Data, sum(Defects) DEFEITOS, SUM(DEFECTS) / @N AS VALOR from CollectionLevel2 where Cast(CollectionDate as Date) between @DATAINICIAL and @DATAFINAL group by cast(AddDate as date) " +
                "\n ) media " +

                "\n --SELECT @MEDIA " +

                "\n SET @LimiteSuperiorControle = @MEDIA + 3 * SQRT(abs(@MEDIA * (1 - @MEDIA) / @N)) " +

                "\n SET @LimiteInferiorControle = @MEDIA - 3 * SQRT(abs(@MEDIA * (1 - @MEDIA) / @N)) " +

                "\n --SELECT @MEDIA AS[p - bar], @UCL as UCL, @LCL as LCL, @LSE as LSE, @LIE as LIE " +

                "\n /* vetor CEP */ " +
                "\n select @MEDIA AS[pbar], @LimiteSuperiorControle as UCL, @LimiteInferiorControle as LCL, @LimiteSuperiorEspecificacao as LSE, @LimiteInferiorEspecificacao as LIE, ISNULL(SUM(CollectionLevel2.DEFECTS) / @N, 0) * 100 AS VALOR, CONVERT(DATE, AddDate) as Data from CollectionLevel2 " +

                "\n where Cast(CollectionDate as Date) between @DATAINICIAL and @DATAFINAL " +

                FiltroUnidade +
                FiltroLevel1 +
                FiltroLevel2 +
                FiltroLevel3 +

                "\n group by CONVERT(DATE, AddDate) " +
                "\n order by CONVERT(DATE, AddDate) ";

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
