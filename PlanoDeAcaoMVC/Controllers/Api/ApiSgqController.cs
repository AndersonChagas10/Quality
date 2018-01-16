using ADOFactory;
using DTO.Helpers;
using Newtonsoft.Json.Linq;
using PlanoAcaoCore;
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
                registros = db.Pa_Acao.Count(r => r.UnidadeName == unidadeNameSGQ && r.Level1Name == level1 && r.Level2Name == l2Name && (r.Status == 5 || r.Status == 1));
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
                registros = db.Pa_Acao.Count(r => r.UnidadeName == unidadeNameSGQ && r.Level1Name == level1 && r.Level2Name == level2 && r.Level3Name == l3Name && (r.Status == 5 || r.Status == 1));
                retorno.Add(l3Name, registros);
            }
            return retorno;
        }


        [HttpPost]
        [Route("GetAcoesByDate")]
        public List<AcoesConcluidas> GetAcoesByDate(JObject filtro)
        {
            try
            {
                dynamic filtroDyn = filtro;
                var retorno = new List<AcoesConcluidas>();
                string inicio = filtroDyn.startDate;
                string fim = filtroDyn.endDate;
                int level = filtroDyn.isLevel;
                var dataInicio = Guard.ParseDateToSqlV2(inicio, Guard.CultureCurrent.BR).ToString("yyyyMMdd");
                var dataFim = Guard.ParseDateToSqlV2(fim, Guard.CultureCurrent.BR).ToString("yyyyMMdd");

                string unidade = filtroDyn.unitId;
                string level1Id = filtroDyn.level1Id;
                string level2Id = filtroDyn.level2Id;
                string level3Id = filtroDyn.level3Id;

                var whereLevel = "";

                if (level == 3)
                {
                    whereLevel = "AND pa.Level1Id = " + level1Id;
                    whereLevel += "AND pa.Level2Id = " + level2Id;
                    whereLevel += "AND pa.Level3Id = " + level3Id;
                }
                else if (level == 2)
                {
                    whereLevel = "AND pa.Level1Id = " + level1Id;
                    whereLevel += "AND pa.Level2Id = " + level2Id;
                }
                else if (level == 1)
                {
                    whereLevel = "AND pa.Level1Id = " + level1Id;
                }

                var query = @" DECLARE @dataFim_ date = '" + dataFim + @"'
  
                 DECLARE @dataInicio_ date = '" + dataInicio + @"'
                SET @dataInicio_ = '" + dataInicio + @"'
                  
                 CREATE TABLE #ListaDatas_ (data_ date)
                  
                 WHILE @dataInicio_ <= @dataFim_  
                 BEGIN
                INSERT INTO #ListaDatas_
                
                	SELECT
                		@dataInicio_
                SET @dataInicio_ = DATEADD(DAY, 1, @dataInicio_)
                  
                 END
                
                SELECT
                	LD.data_ as Data
                   ,COUNT(DISTINCT PA.Id) QteConcluidas
                FROM #ListaDatas_ LD
                
                LEFT JOIN (SELECT
                		Acao_id
                	   ,MAX(AddDate) Max_Date
                	   ,COUNT(DISTINCT Acao_id) QteAcao
                	FROM Pa_Acompanhamento
                	WHERE Status_Id IN (3, 4)
                	GROUP BY Acao_id) PC
                	ON LD.data_ = CAST(PC.Max_Date AS DATE)
                LEFT JOIN (SELECT
                		*
                	FROM Pa_Acao PA
                	WHERE PA.Status IN (3, 4)
                	" + whereLevel + @"
                	AND PA.Unidade_Id = " + unidade + @") PA
                	ON PC.Acao_Id = PA.Id
                GROUP BY LD.data_
                order by ld.data_

                DROP TABLE #ListaDatas_";

                retorno = db.Database.SqlQuery<AcoesConcluidas>(query).ToList();

                return retorno;
            }
            catch (Exception e)
            {
                return new List<AcoesConcluidas>();
            }

        }

        [HttpPost]
        [Route("GetAcoesIndicador")]
        public List<Pa_Acao> GetAcoesIndicador(JObject filtro)
        {
            try
            {
                dynamic filtroDyn = filtro;
                var retorno = new List<Pa_Acao>();
                DateTime dataConclusao = filtroDyn.Data;
                int level = filtroDyn.isLevel;
                //var dataConclusao = Guard.ParseDateToSqlV2(data, Guard.CultureCurrent.EUA).ToString("yyyyMMdd");


                string unidade = filtroDyn.unitId;
                string level1Id = filtroDyn.level1Id;
                string level2Id = filtroDyn.level2Id;
                string level3Id = filtroDyn.level3Id;

                var whereLevel = "";

                if (level == 3)
                {
                    whereLevel = "AND pa.Level1Id = " + level1Id;
                    whereLevel += "AND pa.Level2Id = " + level2Id;
                    whereLevel += "AND pa.Level3Id = " + level3Id;
                }
                else if (level == 2)
                {
                    whereLevel = "AND pa.Level1Id = " + level1Id;
                    whereLevel += "AND pa.Level2Id = " + level2Id;
                }
                else if (level == 1)
                {
                    whereLevel = "AND pa.Level1Id = " + level1Id;
                }

                var query = @"SELECT
                                	ISNULL(PA.UnidadeName,'') as 'UnidadeName'
                               ,ISNULL(PA.Level1Name,'') as 'Level1Name'
                               ,ISNULL(PA.Level2Name,'') as 'Level2Name'
                               ,ISNULL(PA.Level3Name,'') as 'Level3Name'
                               ,ISNULL(PC.Max_Date, '') as 'Max_Date'
                               ,ISNULL(PA.QuandoInicio,'') as 'QuandoInicio'
                               ,ISNULL(PA.QuandoFim,'') as 'QuandoFim'
                            	--,'Como' as 'Como'
                               ,PA.QuantoCusta
                               ,ISNULL(S.Name,'') AS '_StatusName'
                               ,ISNULL(U.Name,'') AS '_Quem'
                               ,ISNULL(CG.CausaGenerica,'') AS '_CausaGenerica'
                               ,ISNULL(GC.GrupoCausa,'') AS '_GrupoCausa'
                               ,ISNULL(CMG.ContramedidaGenerica,'') AS '_ContramedidaGenerica'
                            --,'Assunto' as 'Assunto' 
                            --,'O que' as 'O que'
                            --,'Observacao' as 'Observacao'
                            FROM (SELECT
                            		*
                            	FROM Pa_Acao PA
                            	WHERE PA.Status IN (3, 4)
                            	" + whereLevel + @"
                            	AND PA.Unidade_Id = " + unidade + @") PA
                            INNER JOIN (SELECT
                            		Acao_id
                            	   ,MAX(AddDate) Max_Date
                            	   ,COUNT(DISTINCT Acao_id) QteAcao
                            	FROM Pa_Acompanhamento
                            	WHERE Status_Id IN (3, 4)
                            	AND CAST(AddDate AS DATE) = '" + dataConclusao.ToString("yyyyMMdd") + @"'
                            	GROUP BY Acao_id) PC
                            	ON PC.Acao_Id = PA.Id
                            LEFT JOIN Pa_Quem U
                            	ON PA.Quem_Id = U.Id
                            LEFT JOIN Pa_CausaGenerica CG
                            	ON PA.CausaGenerica_Id = CG.id
                            LEFT JOIN Pa_GrupoCausa GC
                            	ON GC.Id = PA.GrupoCausa_Id
                            LEFT JOIN Pa_ContramedidaGenerica CMG
                            	ON CMG.Id = PA.ContramedidaGenerica_Id
                            LEFT JOIN Pa_Status S
                            	ON S.Id = PA.Status";

                retorno = db.Database.SqlQuery<Pa_Acao>(query).ToList();

                return retorno;
            }
            catch (Exception e)
            {
                return new List<Pa_Acao>();
            }

        }

        public class AcoesConcluidas
        {
            public DateTime Data { get; set; }
            public int QteConcluidas { get; set; }
            public string _data { get { return Data.ToString("dd/MM/yyyy"); } }
        }

    }
}
