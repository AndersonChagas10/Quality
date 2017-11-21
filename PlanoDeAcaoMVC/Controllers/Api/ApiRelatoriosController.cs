using ADOFactory;
using Dominio;
using DTO.Helpers;
using Newtonsoft.Json.Linq;
using PlanoAcaoCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Relatorios")]
    public class ApiRelatoriosController : BaseApiController
    {
        private PlanoAcaoEF.PlanoDeAcaoEntities db;

        public ApiRelatoriosController()
        {
            db = new PlanoAcaoEF.PlanoDeAcaoEntities();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.AutoDetectChangesEnabled = false;
          
            UpdateStatus();

        }

        [HttpPost]
        [Route("GetGrafico")]
        public List<JObject> GetGrafico(JObject filtro)
        {
            string dtInit;
            string dtFim;
            GetParamsPeloFiltro(filtro, out dtInit, out dtFim);

            var query = "SELECT Acao.Id," +
           "\n CAST(Acao.QuandoInicio As Date) as Início," +
           "\n CAST(Acao.QuandoFim As Date) as Fim," +
           "\nSta.Name AS[Status]," +
           "\n  ISNULL(Quem.Name, 'Não possui Responsável') AS Responsavel";

            if (!Conn.visaoOperacional)
            {
                query += "\n,  ISNULL(ContraMedGen.ContramedidaGenerica, 'Não possui Ação Genérica') AS 'Ação Genérica'," +
                "\n  ISNULL(CausaGen.CausaGenerica, 'Não possui Causa Genérica') AS 'Causa Genérica'," +
                "\n  ISNULL(GrpCausa.GrupoCausa, 'Não possui Grupo Causa') AS 'Grupo Causa'," +
                "\n  ISNULL(Un.Description, 'Corporativo') AS 'Unidade'," +
                "\n  ISNULL(IndicDir.Name, 'Não possui Indicadores Diretriz') AS 'Indicadores Diretriz'," +
                "\n  ISNULL(DiretObj.Name, 'Não possui Diretriz / Objetivo') AS 'Diretriz / Objetivo'," +
                "\n  ISNULL(Diretor.Name, 'Não possui Diretoria') AS 'Diretoria'," +
                "\n  ISNULL(Dimens.Name, 'Não possui Dimensão') AS 'Dimensão'," +
                "\n  ISNULL(Gere.Name, 'Não possui Gerência') AS 'Gerência'," +
                "\n  ISNULL(Coord.Name, 'Não possui Coordenação') AS 'Coordenação', " +
                "\n  ISNULL(Acao.Regional, 'Não possui Regional') as 'Regional', "+
                "\n  ISNULL(Acao.UnidadeName, 'Não possui Unidade') as 'Unidade', " +
                "\n  ISNULL(Acao.Level1Name, 'Não possui Indicador') as 'Indicador', " +
                "\n  ISNULL(Acao.Level2Name, 'Não possui Monitoramento') as 'Monitoramento', " +
                "\n  ISNULL(Acao.Level3Name, 'Não possui Tarefa') as 'Tarefa',"+
                "\n  ISNULL(Inici.Name, 'Não possui Projeto / Iniciativa') AS 'Projeto Iniciativa'";
            }

            query += "\n FROM Pa_Acao AS Acao" +
            "\n LEFT JOIN Pa_Status Sta ON Sta.Id = Acao.Status" +
            "\n LEFT JOIN Pa_Quem Quem ON Quem.Id = Acao.Quem_Id" +
            "\n LEFT JOIN Pa_ContramedidaGenerica ContraMedGen ON ContraMedGen.Id = Acao.ContramedidaGenerica_Id" +
            "\n LEFT JOIN Pa_CausaGenerica CausaGen ON CausaGen.Id = Acao.CausaGenerica_Id" +
            "\n LEFT JOIN Pa_GrupoCausa GrpCausa ON GrpCausa.Id = Acao.GrupoCausa_Id" +
            "\n LEFT JOIN Pa_Unidade Un ON Un.Id = Acao.Unidade_Id" +
            "\n LEFT JOIN Pa_Planejamento PlanTatico ON PlanTatico.Id = Acao.Panejamento_Id" +
            "\n LEFT JOIN Pa_Planejamento PlanEstrategy ON PlanEstrategy.Id = PlanTatico.Estrategico_Id" +
            "\n LEFT JOIN Pa_IndicadoresDiretriz IndicDir ON IndicDir.Id = PlanEstrategy.IndicadoresDiretriz_Id" +
            "\n LEFT JOIN Pa_Objetivo DiretObj ON DiretObj.Id = PlanEstrategy.Objetivo_Id" +
            "\n LEFT JOIN Pa_Diretoria Diretor ON Diretor.Id = PlanEstrategy.Diretoria_Id" +
            "\n LEFT JOIN Pa_Dimensao Dimens ON Dimens.Id = PlanEstrategy.Dimensao_Id" +
            "\n LEFT JOIN Pa_Gerencia Gere ON Gere.Id = PlanTatico.Gerencia_Id" +
            "\n LEFT JOIN Pa_Coordenacao Coord ON Coord.Id = PlanTatico.Coordenacao_Id" +
            "\n LEFT JOIN Pa_Iniciativa Inici ON Inici.Id = PlanTatico.Iniciativa_Id" +
            "\n WHERE Acao.AddDate BETWEEN ('" + dtInit + "') AND('" + dtFim + "')";

            var results = QueryNinja(db, query);

            return results;
        }

        [HttpPost]
        [Route("GraficoPie")]
        public List<JObject> GraficoPie(JObject filtro)
        {
            string dtInit;
            string dtFim;
            GetParamsPeloFiltro(filtro, out dtInit, out dtFim);

            var query = "SELECT COUNT(s.id) AS y, s.Name AS name" +
                        "\n FROM pa_acao acao INNER JOIN pa_status s ON acao.Status = s.Id" +
                        "\n WHERE acao.AddDate BETWEEN ('" + dtInit + "') AND('" + dtFim + "')" +
                        "\n GROUP BY s.Id, s.Name";
            dynamic resultPie = QueryNinja(db, query);

            return resultPie;
        }

        [HttpPost]
        [Route("NumeroDeAcoesEstabelecidas")]
        public List<JObject> NumeroDeAcoesEstabelecidas(JObject form)
        {
            dynamic teste = form;

            var query = "SELECT DATEPART(mm,QuandoInicio) as Mes, Count(id) as Quantidade FROM [Pa_Acao] " +
                 //"\n where [Status] not in (4,3) "+
                 "\n WHERE QuandoInicio > '" + DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd 00:00:00") + "'" +
                "\n group by  DATEPART(mm,QuandoInicio)";

            var items = QueryNinja(db, query);

            return items;
        }

        [HttpPost]
        [Route("NumeroDeAcoesConcluidas")]
        public List<JObject> NumeroDeAcoesConcluidas(JObject form)
        {
            dynamic teste = form;

            //var query = "SELECT DATEPART(mm,QuandoInicio) as Mes," +
            //    "\n Count(id) as Quantidade " +
            //    "\n FROM  [Pa_Acao] " +
            //    "\n where [Status] in (4,3) " +
            //     "\n AND QuandoInicio > '" + DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd 00:00:00") + "'" +
            //    "\n group by  DATEPART(mm,QuandoInicio)";

            var query = @"SELECT
                    	DATEPART(mm, Acompanhamento.Max_Date) AS Mes
                       ,COUNT(id) AS Quantidade
                    FROM Pa_Acao PA
                    INNER JOIN (SELECT
                    		Acao_id
                    	   ,MAX(AddDate) Max_Date
                    	FROM Pa_Acompanhamento
                    	WHERE Status_Id IN (3, 4)
                    	GROUP BY Acao_id) Acompanhamento
                    	ON Acompanhamento.Acao_Id = PA.Id
                    		AND Acompanhamento.Max_Date > '" + DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd 00:00:00") + @"' 
                    GROUP BY DATEPART(mm, Acompanhamento.Max_Date)";

            //var query = @"create table #seismeses (
            //            mes int null
            //            )

            //            insert #seismeses select month(dateadd(month, -6, getdate()))
            //            insert #seismeses select month(dateadd(month, -5, getdate()))
            //            insert #seismeses select month(dateadd(month, -4, getdate()))
            //            insert #seismeses select month(dateadd(month, -3, getdate()))
            //            insert #seismeses select month(dateadd(month, -2, getdate()))
            //            insert #seismeses select month(dateadd(month, -1, getdate()))
            //            insert #seismeses select month(getdate())

            //            select SS.mes as Mes, isnull(Quantidade,0) as Quantidade from #seismeses SS
            //            left join (
            //            SELECT DATEPART(mm,QuandoInicio) as Mes,
            //            Count(id) as Quantidade 
            //            FROM  [Pa_Acao] 
            //            where [Status] in (4,3)
            //            AND QuandoInicio > dateadd(month, -6, getdate())
            //            group by  DATEPART(mm,QuandoInicio)
            //            ) CS
            //            ON SS.mes = CS.Mes

            //            drop table #seismeses";

            var items = QueryNinja(db, query);

            return items;
        }

        [HttpPost]
        [Route("NumeroDeAcoes")]
        public List<JObject> NumeroDeAcoes(JObject form)
        {
            dynamic teste = form;

            //var query = "SELECT A.*, B.MesConcluidas, IsNull(B.QuantidadeConcluidas, 0) as QuantidadeConcluidas, (A.QuantidadeIniciadas - IsNull(B.QuantidadeConcluidas, 0)) as Acc" +
            //            "\n FROM" +
            //            "\n (SELECT DATEPART(mm, QuandoInicio) as MesIniciadas, Count(id) as QuantidadeIniciadas FROM [Pa_Acao]  group by  DATEPART(mm, QuandoInicio)) A" +
            //            "\n LEFT JOIN(SELECT DATEPART(mm, QuandoInicio) as MesConcluidas, Count(id) as QuantidadeConcluidas FROM [Pa_Acao] where[Status] in (4, 3) " +
            //             "\n AND QuandoInicio > '" + DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd 00:00:00") + "'" +
            //            "group by  DATEPART(mm, QuandoInicio)) B on A.MesIniciadas = B.MesConcluidas";

            //var query = @"create table #seismeses (
            //                mes int null
            //                )

            //                insert #seismeses select month(dateadd(month, -6, getdate()))
            //                insert #seismeses select month(dateadd(month, -5, getdate()))
            //                insert #seismeses select month(dateadd(month, -4, getdate()))
            //                insert #seismeses select month(dateadd(month, -3, getdate()))
            //                insert #seismeses select month(dateadd(month, -2, getdate()))
            //                insert #seismeses select month(dateadd(month, -1, getdate()))
            //                insert #seismeses select month(getdate())

            //                select * from #seismeses SS
            //                left join (

            //                SELECT A.*, isnull(B.MesConcluidas,A.MesIniciadas) as MesConcluidas, IsNull(B.QuantidadeConcluidas, 0) as QuantidadeConcluidas, (A.QuantidadeIniciadas - IsNull(B.QuantidadeConcluidas, 0)) as Acc
            //                FROM
            //                (SELECT DATEPART(mm, QuandoInicio) as MesIniciadas, Count(id) as QuantidadeIniciadas FROM [Pa_Acao]  group by  DATEPART(mm, QuandoInicio)) A
            //                LEFT JOIN(SELECT DATEPART(mm, QuandoInicio) as MesConcluidas, Count(id) as QuantidadeConcluidas FROM [Pa_Acao] where[Status] in (4, 3) 
            //                AND QuandoInicio > dateadd(month, -6, getdate()) 
            //                group by  DATEPART(mm, QuandoInicio)) B on A.MesIniciadas = B.MesConcluidas
            //                ) CS
            //                ON SS.mes = CS.MesConcluidas

            //                drop table #seismeses

            //                ";

            var query = @"SELECT
                    	A.*
                       ,B.MesConcluidas
                       ,ISNULL(B.QuantidadeConcluidas, 0) AS QuantidadeConcluidas
                       ,(A.QuantidadeIniciadas - ISNULL(B.QuantidadeConcluidas, 0)) AS Acc
                    FROM (SELECT
                    		DATEPART(mm, QuandoInicio) AS MesIniciadas
                    	   ,COUNT(id) AS QuantidadeIniciadas
                    	FROM [Pa_Acao]
                    	GROUP BY DATEPART(mm, QuandoInicio)) A
                    LEFT JOIN (SELECT
                    		DATEPART(mm, Acompanhamento.Max_Date) AS MesConcluidas
                    	   ,COUNT(id) AS QuantidadeConcluidas
                    	FROM Pa_Acao PA
                    	INNER JOIN (SELECT
                    			Acao_id
                    		   ,MAX(AddDate) Max_Date
                    		FROM Pa_Acompanhamento
                    		WHERE Status_Id IN (3, 4)
                    		GROUP BY Acao_id) Acompanhamento
                    		ON Acompanhamento.Acao_Id = PA.Id
                    		AND Acompanhamento.Max_Date > '" + DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd 00:00:00") + @"'
                        GROUP BY DATEPART(mm, Acompanhamento.Max_Date)) B
                    	ON A.MesIniciadas = B.MesConcluidas";

            var items = QueryNinja(db, query);

            return items;
        }

        [HttpPost]
        [Route("EvolucaoDoResultado")]
        public List<JObject> EvolucaoDoResultado(JObject form)
        {
            dynamic teste = form;

            //var query = "SELECT A.*, B.MesConcluidas, IsNull(B.QuantidadeConcluidas, 0) as QuantidadeConcluidas, (A.QuantidadeIniciadas - IsNull(B.QuantidadeConcluidas, 0)) as Acc, 20 as Meta" +
            //            "\n FROM" +
            //            "\n (SELECT DATEPART(mm, QuandoInicio) as MesIniciadas, Count(id) as QuantidadeIniciadas FROM [Pa_Acao]  group by  DATEPART(mm, QuandoInicio)) A" +
            //            "\n LEFT JOIN(SELECT DATEPART(mm, QuandoInicio) as MesConcluidas, Count(id) as QuantidadeConcluidas FROM [Pa_Acao] where[Status] in (4, 3) " +
            //              "\n AND QuandoInicio > '" + DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd 00:00:00") + "'" +
            //             "\n group by  DATEPART(mm, QuandoInicio)) B on A.MesIniciadas = B.MesConcluidas";

            //var query = @"create table #seismeses (
            //            mes int null
            //            )

            //            insert #seismeses select month(dateadd(month, -6, getdate()))
            //            insert #seismeses select month(dateadd(month, -5, getdate()))
            //            insert #seismeses select month(dateadd(month, -4, getdate()))
            //            insert #seismeses select month(dateadd(month, -3, getdate()))
            //            insert #seismeses select month(dateadd(month, -2, getdate()))
            //            insert #seismeses select month(dateadd(month, -1, getdate()))
            //            insert #seismeses select month(getdate())

            //            select CS.* from #seismeses SS
            //            left join (
            //            SELECT A.*, isnull(B.MesConcluidas,A.MesIniciadas) as MesConcluidas, IsNull(B.QuantidadeConcluidas, 0) as QuantidadeConcluidas, (A.QuantidadeIniciadas - IsNull(B.QuantidadeConcluidas, 0)) as Acc, 20 as Meta
            //                                    FROM
            //                                    (SELECT DATEPART(mm, QuandoInicio) as MesIniciadas, Count(id) as QuantidadeIniciadas FROM [Pa_Acao]  group by  DATEPART(mm, QuandoInicio)) A
            //                                     LEFT JOIN(SELECT DATEPART(mm, QuandoInicio) as MesConcluidas, Count(id) as QuantidadeConcluidas FROM [Pa_Acao] where[Status] in (4, 3) 
            //                                      AND QuandoInicio > dateadd(month, -6, getdate())
            //                                      group by  DATEPART(mm, QuandoInicio)) B on A.MesIniciadas = B.MesConcluidas

            //            ) CS
            //            ON SS.mes = CS.MesConcluidas

            //            drop table #seismeses";

            var query = @"create table #seismeses (
                        mes int null
                        )
                        INSERT #seismeses
                        	SELECT
                        		MONTH(DATEADD(MONTH, -6, GETDATE()))
                        INSERT #seismeses
                        	SELECT
                        		MONTH(DATEADD(MONTH, -5, GETDATE()))
                        INSERT #seismeses
                        	SELECT
                        		MONTH(DATEADD(MONTH, -4, GETDATE()))
                        INSERT #seismeses
                        	SELECT
                        		MONTH(DATEADD(MONTH, -3, GETDATE()))
                        INSERT #seismeses
                        	SELECT
                        		MONTH(DATEADD(MONTH, -2, GETDATE()))
                        INSERT #seismeses
                        	SELECT
                        		MONTH(DATEADD(MONTH, -1, GETDATE()))
                        INSERT #seismeses
                        	SELECT
                        		MONTH(GETDATE())
                        
                        SELECT
                        	CS.*
                        FROM #seismeses SS
                        LEFT JOIN (SELECT
                        		A.*
                        	   ,ISNULL(B.MesConcluidas, A.MesIniciadas) AS MesConcluidas
                        	   ,ISNULL(B.QuantidadeConcluidas, 0) AS QuantidadeConcluidas
                        	   ,(A.QuantidadeIniciadas - ISNULL(B.QuantidadeConcluidas, 0)) AS Acc
                        	   ,20 AS Meta
                        	FROM (SELECT
                        			DATEPART(mm, QuandoInicio) AS MesIniciadas
                        		   ,COUNT(id) AS QuantidadeIniciadas
                        		FROM [Pa_Acao]
                        		GROUP BY DATEPART(mm, QuandoInicio)) A
                        	LEFT JOIN (SELECT
                        			DATEPART(mm, Acompanhamento.Max_Date) AS MesConcluidas
                        		   ,COUNT(id) AS QuantidadeConcluidas
                        		FROM [Pa_Acao] PA
                        		INNER JOIN (SELECT
                        				Acao_id
                        			   ,MAX(AddDate) Max_Date
                        			FROM Pa_Acompanhamento
                        			WHERE Status_Id IN (3, 4)
                        			GROUP BY Acao_id) Acompanhamento
                        			ON Acompanhamento.Acao_Id = PA.Id
                        			AND PA.QuandoInicio > DATEADD(MONTH, -6, GETDATE())
                        		GROUP BY DATEPART(mm, Acompanhamento.Max_Date)) B
                        		ON A.MesIniciadas = B.MesConcluidas) CS
                        	ON SS.mes = CS.MesConcluidas
                        
                        DROP TABLE #seismeses";

            var items = QueryNinja(db, query);

            return items;
        }

        [HttpPost]
        [Route("GetGraficoSgq")]
        public List<JObject> GetGraficoSgq(JObject filtro)
        {
            string dtInit;
            string dtFim;
            GetParamsPeloFiltro(filtro, out dtInit, out dtFim);

            var query = "SELECT Acao.Id," +
            "\n Acao.Level1Id, " +
            "\n Acao.Level2Id, " +
            "\n Acao.Level3Id, " +
            "\n CAST(Acao.QuandoInicio As Date) as Início," +
            "\n CAST(Acao.QuandoFim As Date) as Fim," +
            "\nSta.Name AS[Status]," +
            "\n  ISNULL(Quem.Name, 'Não possui Responsável') AS Responsavel";

            if (!Conn.visaoOperacional)
            {
                query += "\n,  ISNULL(ContraMedGen.ContramedidaGenerica, 'Não possui Ação Genérica') AS 'Ação Genérica'," +
                "\n  ISNULL(CausaGen.CausaGenerica, 'Não possui Causa Genérica') AS 'Causa Genérica'," +
                "\n  ISNULL(GrpCausa.GrupoCausa, 'Não possui Grupo Causa') AS 'Grupo Causa'," +
                "\n  ISNULL(Un.Description, 'Corporativo') AS 'Unidade'," +
                "\n  ISNULL(IndicDir.Name, 'Não possui Indicadores Diretriz') AS 'Indicadores Diretriz'," +
                "\n  ISNULL(DiretObj.Name, 'Não possui Diretriz / Objetivo') AS 'Diretriz / Objetivo'," +
                "\n  ISNULL(Diretor.Name, 'Não possui Diretoria') AS 'Diretoria'," +
                "\n  ISNULL(Dimens.Name, 'Não possui Dimensão') AS 'Dimensão'," +
                "\n  ISNULL(Gere.Name, 'Não possui Gerência') AS 'Gerência'," +
                "\n  ISNULL(Coord.Name, 'Não possui Coordenação') AS 'Coordenação'," +
                "\n  ISNULL(Inici.Name, 'Não possui Projeto / Iniciativa') AS 'Projeto Iniciativa'";
            }

            query += "\n FROM Pa_Acao AS Acao" +
            "\n LEFT JOIN Pa_Status Sta ON Sta.Id = Acao.Status" +
            "\n LEFT JOIN Pa_Quem Quem ON Quem.Id = Acao.Quem_Id" +
            "\n LEFT JOIN Pa_ContramedidaGenerica ContraMedGen ON ContraMedGen.Id = Acao.ContramedidaGenerica_Id" +
            "\n LEFT JOIN Pa_CausaGenerica CausaGen ON CausaGen.Id = Acao.CausaGenerica_Id" +
            "\n LEFT JOIN Pa_GrupoCausa GrpCausa ON GrpCausa.Id = Acao.GrupoCausa_Id" +
            "\n LEFT JOIN Pa_Unidade Un ON Un.Id = Acao.Unidade_Id" +
            "\n LEFT JOIN Pa_Planejamento PlanTatico ON PlanTatico.Id = Acao.Panejamento_Id" +
            "\n LEFT JOIN Pa_Planejamento PlanEstrategy ON PlanEstrategy.Id = PlanTatico.Estrategico_Id" +
            "\n LEFT JOIN Pa_IndicadoresDiretriz IndicDir ON IndicDir.Id = PlanEstrategy.IndicadoresDiretriz_Id" +
            "\n LEFT JOIN Pa_Objetivo DiretObj ON DiretObj.Id = PlanEstrategy.Objetivo_Id" +
            "\n LEFT JOIN Pa_Diretoria Diretor ON Diretor.Id = PlanEstrategy.Diretoria_Id" +
            "\n LEFT JOIN Pa_Dimensao Dimens ON Dimens.Id = PlanEstrategy.Dimensao_Id" +
            "\n LEFT JOIN Pa_Gerencia Gere ON Gere.Id = PlanTatico.Gerencia_Id" +
            "\n LEFT JOIN Pa_Coordenacao Coord ON Coord.Id = PlanTatico.Coordenacao_Id" +
            "\n LEFT JOIN Pa_Iniciativa Inici ON Inici.Id = PlanTatico.Iniciativa_Id" +
            "\n WHERE Acao.AddDate BETWEEN ('" + dtInit + "') AND('" + dtFim + "') AND Acao.TipoIndicador = 2";


            var results = QueryNinja(db, query);



            if (results.Count > 0)
            {
                var parLevel1Ids = results.Select(r => r.GetValue("Level1Id").ToString()).Distinct().ToList();
                var parLevel2Ids = results.Select(r => r.GetValue("Level2Id").ToString()).Distinct().ToList();
                var parLevel3Ids = results.Select(r => r.GetValue("Level3Id").ToString()).Distinct().ToList();
                var level1ids = String.Join(", ", parLevel1Ids.ToArray());
                var level2ids = String.Join(", ", parLevel2Ids.ToArray());
                var level3ids = String.Join(", ", parLevel3Ids.ToArray());

                //foreach (var i in )
                //{

                //    parLevel1Ids += i.GetValue("Level1Id") + ",";
                //   // parLevel2Ids = i.Level2Id + ",";
                //    //parLevel3Ids = i.Level3Id + ",";
                //}
                //var result = String.Join(", ", names.ToArray());

                var level1 = QueryNinjaPeloDbSgq("SELECT Id, Name FROM ParLevel1 WHERE Id in(" + level1ids + ")");
                var level2 = QueryNinjaPeloDbSgq("SELECT Id, Name FROM ParLevel2 WHERE Id in(" + level2ids + ")");
                var level3 = QueryNinjaPeloDbSgq("SELECT Id, Name FROM ParLevel3 WHERE Id in(" + level3ids + ")");

                foreach (var i in results)
                {
                    i["Indicador"] = level1.FirstOrDefault(r => i.GetValue("Level1Id").Equals(r.GetValue("Id"))).GetValue("Name");
                    i["Monitoramento"] = level2.FirstOrDefault(r => i.GetValue("Level2Id").Equals(r.GetValue("Id"))).GetValue("Name");
                    i["Tarefa"] = level3.FirstOrDefault(r => i.GetValue("Level3Id").Equals(r.GetValue("Id"))).GetValue("Name");
                }
            }

            return results;
        }

        [HttpPost]
        [Route("GetPessoasEnvolvidas")]
        public List<JObject> GetPessoasEnvolvidas(JObject filtro)
        {
            string dtInit;
            string dtFim;
            GetParamsPeloFiltro(filtro, out dtInit, out dtFim);

            var query = "SELECT"+
             "  ISNULL(Quem.Name, 'Não possui Responsável') AS Responsavel,"+
             "  Sta.Name AS[Status],"+
             "  COUNT(1) as Quantidade"+
             " FROM Pa_Acao AS Acao"+
             " LEFT JOIN Pa_Status Sta ON Sta.Id = Acao.Status"+
             " LEFT JOIN Pa_Quem Quem ON Quem.Id = Acao.Quem_Id"+
             " LEFT JOIN Pa_ContramedidaGenerica ContraMedGen ON ContraMedGen.Id = Acao.ContramedidaGenerica_Id"+
             " LEFT JOIN Pa_CausaGenerica CausaGen ON CausaGen.Id = Acao.CausaGenerica_Id"+
             " LEFT JOIN Pa_GrupoCausa GrpCausa ON GrpCausa.Id = Acao.GrupoCausa_Id"+
             " LEFT JOIN Pa_Unidade Un ON Un.Id = Acao.Unidade_Id"+
             " LEFT JOIN Pa_Planejamento PlanTatico ON PlanTatico.Id = Acao.Panejamento_Id"+
             " LEFT JOIN Pa_Planejamento PlanEstrategy ON PlanEstrategy.Id = PlanTatico.Estrategico_Id"+
             " LEFT JOIN Pa_IndicadoresDiretriz IndicDir ON IndicDir.Id = PlanEstrategy.IndicadoresDiretriz_Id"+
             " LEFT JOIN Pa_Objetivo DiretObj ON DiretObj.Id = PlanEstrategy.Objetivo_Id"+
             " LEFT JOIN Pa_Diretoria Diretor ON Diretor.Id = PlanEstrategy.Diretoria_Id"+
             " LEFT JOIN Pa_Dimensao Dimens ON Dimens.Id = PlanEstrategy.Dimensao_Id"+
             " LEFT JOIN Pa_Gerencia Gere ON Gere.Id = PlanTatico.Gerencia_Id"+
             " LEFT JOIN Pa_Coordenacao Coord ON Coord.Id = PlanTatico.Coordenacao_Id"+
             "\n WHERE Acao.AddDate BETWEEN ('" + dtInit + "') AND('" + dtFim + "')"+//; AND Sta.Id not in (2, 3, 4)"+
             " GROUP BY ISNULL(Quem.Name, 'Não possui Responsável') , Sta.Name";

            var results = QueryNinja(db, query);

            return results;
        }

        [HttpGet]
        [Route("getUnits")]
        public List<JObject> getUnits()
        {
            var query = "SELECT * from Pa_Unidade";
            var items = QueryNinja(db, query);

            return items;
        }

        [HttpGet]
        [Route("getIndicadores")]
        public List<ParLevel1> getIndicadores()
        {
            var items = new List<ParLevel1>();
            using (var db = new SgqDbDevEntities())
            {
                items = db.ParLevel1.ToList();
            }
            //var items = QueryNinja(db, query);
            
            return items;
        }
    }
}