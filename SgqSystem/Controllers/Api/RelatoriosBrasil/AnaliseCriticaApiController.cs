using ADOFactory;
using DTO;
using SgqService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/AnaliseCritica")]
    public class AnaliseCriticaApiController : BaseApiController
    {

        [HttpPost]
        [Route("GetGraficoHistoricoUnidade")]
        public List<HistoricoUnidades> GetGraficoHistoricoUnidade([FromBody] DataCarrierFormularioNew form)
        {

            var retornoHistoricoUnidade = new List<HistoricoUnidades>();

            var titulo = "Histórico Consolidado";
            var SQLcentro = getQuery(form, 1);
            var script = SQLcentro;

            var tipoConsolidacao = form.DimensaoData;

            //fazer os ifs de tipo de consolidação - Dia - Semana - Mês
            if (tipoConsolidacao == 1)
            {
                script += $@"
                SELECT
                   '{ titulo }' AS ChartTitle
                   ,IIF(SUM(ISNULL (AVComPeso, 0)) = 0, 0, IIF(ISNULL (SUM(NULLIF(NCComPeso, 0)) / SUM(ISNULL (AVComPeso, 0)) * 100, 0) > 100, 100, ISNULL (SUM(NULLIF(NCComPeso, 0)) / SUM(ISNULL (AVComPeso, 0)) * 100, 0))) AS PorcentagemNc
                   ,ConsolidationDate AS [Date]
                   ,SUM(ISNULL (AVComPeso, 0)) AS AVComPeso
                   ,SUM(ISNULL (NCComPeso, 0)) AS NCComPeso
                   ,SUM(ISNULL (AV, 0)) AS AV
                   ,SUM(ISNULL (NC, 0)) AS NC
                   ,AVG(ISNULL (Meta, 0)) AS Meta
                FROM #CUBO Cubo WITH (NOLOCK)
                WHERE 1 = 1
                

                GROUP BY 
                		ConsolidationDate
                ORDER BY 3
                ";
            }
            else if (tipoConsolidacao == 2)
            {
                //fazer script de semana
                script += $@"
                    SELECT
                        'Histórico Consolidado' AS ChartTitle
                       ,IIF(SUM(ISNULL (AVComPeso, 0)) = 0, 0, IIF(ISNULL (SUM(NULLIF(NCComPeso, 0)) / SUM(ISNULL (AVComPeso, 0)) * 100, 0) > 100, 100, ISNULL (SUM(NULLIF(NCComPeso, 0)) / SUM(ISNULL (AVComPeso, 0)) * 100, 0))) AS PorcentagemNc
                       ,CONCAT(CONCAT(DATEPART(YEAR, ConsolidationDate), '-'), DATEPART(Week, ConsolidationDate)) AS [Semana]
                       ,SUM(ISNULL (AVComPeso, 0)) AS AVComPeso
                       ,SUM(ISNULL (NCComPeso, 0)) AS NCComPeso
                       ,SUM(ISNULL (AV, 0)) AS AV
                       ,SUM(ISNULL (NC, 0)) AS NC
                       ,AVG(ISNULL (Meta, 0)) AS Meta
                    FROM #CUBO Cubo WITH (NOLOCK)
                    WHERE 1 = 1

                    GROUP BY
                    CONCAT(CONCAT(DATEPART(YEAR, ConsolidationDate), '-'), DATEPART(Week, ConsolidationDate))
                    ORDER BY 3
                    ";
            }
            else
            {
                script += $@"
                SELECT
                    'Histórico Consolidado' AS ChartTitle
                   ,IIF(SUM(ISNULL (AVComPeso, 0)) = 0, 0, IIF(ISNULL (SUM(NULLIF(NCComPeso, 0)) / SUM(ISNULL (AVComPeso, 0)) * 100, 0) > 100, 100, ISNULL (SUM(NULLIF(NCComPeso, 0)) / SUM(ISNULL (AVComPeso, 0)) * 100, 0))) AS PorcentagemNc
                   ,CONCAT(CONCAT(DATEPART(YEAR, ConsolidationDate), '-'), DATEPART(MONTH, ConsolidationDate)) AS [Mes]
                   ,SUM(ISNULL (AVComPeso, 0)) AS AVComPeso
                   ,SUM(ISNULL (NCComPeso, 0)) AS NCComPeso
                   ,SUM(ISNULL (AV, 0)) AS AV
                   ,SUM(ISNULL (NC, 0)) AS NC
                   ,AVG(ISNULL (Meta, 0)) AS Meta
                FROM #CUBO Cubo WITH (NOLOCK)
                WHERE 1 = 1

                GROUP BY --Indicador
                CONCAT(CONCAT(DATEPART(YEAR, ConsolidationDate), '-'), DATEPART(MONTH, ConsolidationDate))
                ORDER BY CAST(CONCAT(CONCAT(DATEPART(YEAR, ConsolidationDate), '-'), DATEPART(MONTH, ConsolidationDate)) + '-01' AS DATE)
                ";
            }

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retornoHistoricoUnidade = factory.SearchQuery<HistoricoUnidades>(script).ToList();
            }

            return retornoHistoricoUnidade;
        }

        [HttpPost]
        [Route("GetAnaliseCritica")]
        public List<AnaliseCriticaResultSet> GetAnaliseCritica([FromBody] DataCarrierFormularioNew form)
        {
            var listParLevel1_Id = form.ParLevel1_Ids.ToList();
            var listTendenciaResultSet = new List<AnaliseCriticaResultSet>();

            foreach (var parLevel1_Id in listParLevel1_Id)
            {
                //TODO: Fazer a busca de todos os graficos de tendencia pelo Indicador
                var analiseCriticaResultSet = new AnaliseCriticaResultSet();
                var tendencia = GetGraficoTendenciaIndicador(form, parLevel1_Id);

                if (tendencia.Count > 0)
                {
                    //Indicador
                    analiseCriticaResultSet.ListaTendenciaResultSet = tendencia;
                    analiseCriticaResultSet.ParLevel1_Id = tendencia[0].level1_Id.GetValueOrDefault();
                    analiseCriticaResultSet.ParLevel1_Name = tendencia[0].Level1Name;
                    analiseCriticaResultSet.ListaAcaoIndicador = GetAcaoCorretivaIndicador(form, parLevel1_Id);

                    //Monitoramentos
                    analiseCriticaResultSet.ListaMonitoramento = GetMonitoramentos(form, parLevel1_Id);
                    analiseCriticaResultSet.ListaAcaoMonitoramento = GetAcaoCorretivaMonitoramento(form, parLevel1_Id);

                    //Monitorametnos por Departamento
                    //Trazer os Monitorametnos com Departamento
                    //Pegar todos os Departamentos
                    //Foreach no departamento e trazer por cada um e inserir na lista
                    var monitoramentosDepartamentos = GetMonitoramentosDepartamento(form, parLevel1_Id);

                    //analiseCriticaResultSet.MonitoramentosDepartamentos
                    var departamentos_Ids = monitoramentosDepartamentos.Select(x => x.ParDepartment_Id).Distinct().ToList();

                    analiseCriticaResultSet.MonitoramentosDepartamentos = new List<GraficoTabela>();

                    foreach (var departamento_Id in departamentos_Ids)
                    {
  
                        var grafico = new List<GraficoNC>();
                        var tabela = new List<AcaoResultSet>();

                        tabela = GetAcaoCorretivaMonitoramento(form, parLevel1_Id, departamento_Id);
                        grafico = monitoramentosDepartamentos.Where(x => x.ParDepartment_Id == departamento_Id).ToList();                

                        analiseCriticaResultSet.MonitoramentosDepartamentos.Add(new GraficoTabela { 
                            ListaGrafico = grafico, 
                            ListaTabelaAcaoCorretiva = tabela, 
                            ParDepartment = grafico[0].ParDepartment_Name, 
                            ParDepartment_Id = departamento_Id
                        });

                    }

                    //se for até monitoramento, não busca tarefas
                    if (form.Desdobramento[0] == 1)
                    {
                        listTendenciaResultSet.Add(analiseCriticaResultSet);
                        continue;
                    }

                    //Tarefa

                    //TarefasAc
                    analiseCriticaResultSet.ListaTarefasAcumuladas = GetTarefas(form, parLevel1_Id);

                    //Tarefas por monitoramento
                    analiseCriticaResultSet.TarefaMonitoramentos = new List<GraficoTabela>();

                    var monitoramentos_Ids = analiseCriticaResultSet.ListaMonitoramento.Select(x => x.Id).Distinct().ToList();

                    var tarefasMonitoramento = GetTarefasMonitramentos(form, parLevel1_Id);

                    var acoesPA = GetAcaoTarefaMonitoramento(form, parLevel1_Id);

                    foreach (var monitoramento_Id in monitoramentos_Ids)
                    {

                        var grafico = new List<GraficoNC>();
                        var tabela = new List<PAAcao>();

                        tabela = acoesPA.Where(x => x.ParLevel2_Id == monitoramento_Id).ToList();
                        grafico = tarefasMonitoramento.Where(x => x.ParLevel2_Id == monitoramento_Id).ToList();

                        if (grafico.Count == 0)
                            continue;

                        analiseCriticaResultSet.TarefaMonitoramentos.Add(new GraficoTabela
                        {
                            ListaGrafico = grafico,
                            ListaTabelaPlanoAcao = tabela,
                            ParLevel2_Name = grafico[0].ParLevel2_Name,
                            ParLevel2_Id = monitoramento_Id

                        });

                    }

                    listTendenciaResultSet.Add(analiseCriticaResultSet);
                }

                //TODO: Resto dos Indicadores

            }

            return listTendenciaResultSet;
        }

        private List<TendenciaResultSet> GetGraficoTendenciaIndicador(DataCarrierFormularioNew form, int ParLevel1_Id)
        {

            var retornoTendencia = new List<TendenciaResultSet>();
            var wModulo = "";
            var wParClusterGroup = "";
            var wParCluster = "";
            var wParStructure = "";
            var wParCompany = "";
            var wTurno = "";
            var wParCriticalLevel = "";
            var wParGroupParLevel1 = "";
            var wParDepartment = "";
            var wParLevel1 = "";
            var wParLevel2 = "";
            var wParLevel3 = "";
            var wAcaoStatus = "";
            var wPeriodo = "";
            var wNCComPeso = "";
            var wPeso = "";
            var wDesdobramento = "";
            var wSurpervisor = "";
            var orderBy = "S3.Data";
            var groupBy = "";
            var data = "";
            var data2 = "";

            //Módulo
            if (form.ParModule_Ids != null && form.ParModule_Ids.Length > 0)
                wModulo = $" AND L1XM.ParModule_Id IN ({form.ParModule_Ids[0]}) --Modulo";

            //Grupo de Processo
            if (form.ParClusterGroup_Ids != null && form.ParClusterGroup_Ids.Length > 0)
                wParClusterGroup = $" AND PCL.ParClusterGroup_Id IN ({string.Join(",", form.ParClusterGroup_Ids)}) --Grupo de Processo";

            //Processo
            if (form.ParCluster_Ids != null && form.ParCluster_Ids.Length > 0)
                wParCluster = $" AND C2XC.ParCluster_Id IN ({string.Join(",", form.ParCluster_Ids)}) --Processo";

            //Regional
            if (form.ParStructure_Ids != null && form.ParStructure_Ids.Length > 0)
                wParStructure = $" AND PCXS.ParStructure_Id IN ({ string.Join(",", form.ParStructure_Ids)}) --Regional)";

            //Unidade
            if (form.ParCompany_Ids != null && form.ParCompany_Ids.Length > 0)
                wParCompany = $" AND PC.Id in ({string.Join(",", form.ParCompany_Ids)}) --Unidade";

            //Turno
            if (form.Shift_Ids != null && form.Shift_Ids.Length > 0)
                wTurno = $" AND C2.Shift in ({string.Join(",", form.Shift_Ids)}) --Turno";

            //Nível Criticidade
            if (form.ParCriticalLevel_Ids != null && form.ParCriticalLevel_Ids.Length > 0)
                wParCriticalLevel = $" AND L1XC.ParCriticalLevel_Id IN ({string.Join(",", form.ParCriticalLevel_Ids)}) --Nível Criticidade";

            //Função
            if (form.ParGroupParLevel1_Ids != null && form.ParGroupParLevel1_Ids.Length > 0)
                wParGroupParLevel1 = $" AND PL1.ParGroupLevel1_Id IN ({string.Join(",", form.ParGroupParLevel1_Ids)}) --Função";

            //Departamento
            if (form.ParDepartment_Ids != null && form.ParDepartment_Ids.Length > 0)
                wParDepartment = $" AND PL2.ParDepartment_Id IN ({string.Join(",", form.ParDepartment_Ids)}) --Departamento";

            //Indicador
            //if (form.ParLevel1_Ids != null && form.ParLevel1_Ids.Length > 0)
            //    wParLevel1 = $" {string.Join(",", form.ParLevel1_Ids)})";

            //Monitoramento
            if (form.ParLevel2_Ids != null && form.ParLevel2_Ids.Length > 0)
                wParLevel2 = $" AND C2.ParLevel2_Id IN ({string.Join(",", form.ParLevel2_Ids)}) --Monitoramento";

            //Tarefa
            if (form.ParLevel3_Ids != null && form.ParLevel3_Ids.Length > 0)
                wParLevel3 = $" AND R3.ParLevel3_Id IN ({string.Join(",", form.ParLevel3_Ids)}) --Tarefa";



            //Plano de Ação Concluido
            if (form.AcaoStatus != null && form.AcaoStatus.Length > 0) { 
                if (form.AcaoStatus[0] == 1) //Concluido
                    wAcaoStatus = " AND (SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesConcluidas FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN (3, 4, 7, 8) AND Acao.Status NOT IN (1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";

                if (form.AcaoStatus[0] == 2)//Não Concluido
                    wAcaoStatus = " AND(SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesEmAndamento FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN(1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";
            }

            //Periodo
            if (form.Periodo != null && form.Periodo.Length > 0) {
                if (form.Periodo[0] == 1)//Diario
                {
                    wPeriodo = "convert(NVARCHAR, Data, 103)";
                    orderBy = "S3._Data";
                    groupBy = ",Data";
                    data = ",Data as _Data";
                }

                if (form.Periodo[0] == 2)//Semanal
                { 
                    wPeriodo = "CONCAT(CONCAT(DATEPART(YEAR, S1.Data), '-'), DatePart(WEEK, S1.Data))";
                    orderBy = "CONVERT(INT,replace(S3.Data, '-', ''))";
                }
                if (form.Periodo[0] == 3)//Mensal
                {
                    wPeriodo = "CONCAT(CONCAT(DATEPART(YEAR, S1.Data), '-'), DatePart(MONTH, S1.Data))";
                    orderBy = "CONVERT(INT,replace(S3.Data, '-', ''))";
                }
            }

            //Exibe NC Com peso
            if (form.NcComPeso != null && form.NcComPeso.Length > 0)
            {
                if(form.NcComPeso[0] == 1) //Com Peso
                    wNCComPeso = "'NcComPeso'";
                if (form.NcComPeso[0] == 2) //Sem Peso
                    wNCComPeso = "'NcSemPeso'";
            }

            //Peso
            if (form.Peso != null && form.Peso.Length > 0 && form.Peso[0] != null)
                wPeso = $" AND R3.Weight IN ({ string.Join(",", form.Peso) })--Peso";

            //Desdobramento
            //if (form.Desdobramento != null && form.Desdobramento.Length > 0)
            //    wDesdobramento = "";

            //Surpervisor
            if (form.UserSgqSurpervisor_Ids != null && form.UserSgqSurpervisor_Ids.Length > 0)
                wSurpervisor = $" AND C2.AuditorId IN ({string.Join(",", form.UserSgqSurpervisor_Ids)}) --Surpervisor";

            var query = $@" 

            DECLARE @INDICADOR INT = {ParLevel1_Id}
            
            DECLARE @dataInicio_ date = DATEADD(MONTH, 0, '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00')
            DECLARE @dataFim_ date = '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59'
            
            --SET @dataInicio_ = DATEFROMPARTS(YEAR(@dataInicio_), MONTH(@dataInicio_), 01)
            DECLARE @ListaDatas_ table(data_ date)
            WHILE @dataInicio_ <= @dataFim_ 
            BEGIN
            INSERT INTO @ListaDatas_
            	SELECT
            		@dataInicio_
            SET @dataInicio_ = DATEADD(DAY, 1, @dataInicio_)
            END
              
            DECLARE @DATAFINAL DATE = @dataFim_
            DECLARE @DATAINICIAL DATE = DATEADD(mm, DateDiff(mm, 0, @DATAFINAL) - 1, 0)
            
            DECLARE @RESS INT
            
            SELECT
            	@RESS = COUNT(1)
            FROM (SELECT
            		COUNT(1) AS NA
            	FROM CollectionLevel2 C2 WITH (NOLOCK)
            	LEFT JOIN Result_Level3 C3 WITH (NOLOCK)
            		ON C3.CollectionLevel2_Id = C2.Id
            	WHERE CONVERT(DATE, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL
            	AND C2.ParLevel1_Id = (SELECT TOP 1
            			Id
            		FROM ParLevel1 WITH (NOLOCK)
            		WHERE hashKey = 1)
            	AND C2.UnitId IN (1)
            	AND IsNotEvaluate = 1
            	GROUP BY C2.Id) NA
            WHERE NA = 2
            
            SELECT
            	level1_Id
               ,Level1Name
               ,PorcentagemNc
               ,(CASE
            		WHEN IsRuleConformity = 1 THEN (100 - Meta)
            		WHEN IsRuleConformity IS NULL THEN 0
            		ELSE Meta
            	END) AS Meta	
               ,IIF({wNCComPeso} = 'NcComPeso', NC , NcSemPeso) AS NC --Exibe NC Com peso 
               ,IIF({wNCComPeso} = 'NcComPeso', AV , AvSemPeso) as AV --Exibe Av Com peso 
               ,Data AS _Data
            FROM (SELECT
            		*
            	   ,CASE WHEN Av IS NULL OR Av = 0 THEN 0 ELSE NC / Av * 100 END AS PorcentagemNc
            	   ,CASE WHEN Av IS NULL OR Av = 0 THEN 0 ELSE NCSemPeso / Av * 100 END AS PorcentagemNcSemPeso
            	   ,CASE WHEN CASE WHEN Av IS NULL OR Av = 0 THEN 0 ELSE NC / Av * 100 END >= (CASE WHEN IsRuleConformity = 1 THEN (100 - Meta) ELSE Meta END) THEN 1 ELSE 0 END RELATORIO_DIARIO
            	FROM (SELECT
            			level1_Id
            		   ,IsRuleConformity
            		   ,Level1Name
            		   ,TituloGrafico
            		   ,Unidade_Id
            		   ,Unidade_Name
            		   ,{wPeriodo} as Data
            		   ,SUM(Av) AS Av
            		   ,SUM(AvSemPeso) AS AvSemPeso
            		   ,SUM(NC) AS NC
            		   ,SUM(NCSemPeso) AS NCSemPeso
            		   ,MAX(Meta) AS Meta
                       {data}
            		FROM (SELECT
            				PL1.Id AS level1_Id
            			   ,PL1.Name AS Level1Name
            			   ,'Tendência do Indicador ' + PL1.Name AS TituloGrafico
            			   ,PL1.IsRuleConformity
            			   ,PC.Id AS Unidade_Id
            			   ,PC.Name AS Unidade_Name
			               ,CASE
			            		WHEN PL1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) - @RESS FROM VolumePcc1b WITH (NOLOCK) WHERE ParCompany_Id = PC.Id AND Data = cast(C2.CollectionDate as date))
			            		WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.WeiEvaluation)
			            		WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiEvaluation)
			            		WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT ""KEY"")
			            		WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT ""KEY"")
			            		ELSE 0
			            	END AS Av
			               ,CASE
			            		WHEN PL1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) - @RESS FROM VolumePcc1b WITH (NOLOCK) WHERE ParCompany_Id = PC.Id AND Data = cast(C2.CollectionDate as date))
			            		WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.Evaluation)
			            		WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiEvaluation)
			            		WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT ""KEY"")
			            		WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT ""KEY"")
			            		ELSE 0
			            	END AS AvSemPeso
			               ,CASE
			            		WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.WeiDefects)
			            		WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiDefects)
			            		WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT IIF(R3.WeiDefects>0,""KEY"",NULL))
			            		WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT IIF(R3.WeiDefects>0,""KEY"",NULL))
			            		ELSE 0
			            	END AS NC
			               ,CASE
			            		WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.Defects)
			            		WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiDefects)
			            		WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT IIF(R3.WeiDefects>0,""KEY"",NULL))
			            		WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT IIF(R3.WeiDefects>0,""KEY"",NULL))
			            		ELSE 0
			            	END AS NCSemPeso
            			   ,CASE
            					WHEN (SELECT
            								COUNT(1)
            							FROM ParGoal G WITH (NOLOCK)
            							WHERE G.ParLevel1_Id = C2.ParLevel1_Id
            							AND (G.ParCompany_Id = C2.UnitId
            							OR G.ParCompany_Id IS NULL)
            							AND G.IsActive = 1
            							AND G.EffectiveDate <= @DATAFINAL)
            						> 0 THEN (SELECT TOP 1
            								ISNULL(G.PercentValue, 0)
            							FROM ParGoal G WITH (NOLOCK)
            							WHERE G.ParLevel1_Id = C2.ParLevel1_Id
            							AND (G.ParCompany_Id = C2.UnitId
            							OR G.ParCompany_Id IS NULL)
            							AND G.IsActive = 1
            							AND G.EffectiveDate <= @DATAFINAL
            							ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
            
            					ELSE (SELECT TOP 1
            								ISNULL(G.PercentValue, 0)
            							FROM ParGoal G WITH (NOLOCK)
            							WHERE G.ParLevel1_Id = C2.ParLevel1_Id
            							AND (G.ParCompany_Id = C2.UnitId
            							OR G.ParCompany_Id IS NULL)
            							ORDER BY G.ParCompany_Id DESC, EffectiveDate ASC)
            				END
            				AS Meta
            			   ,DD.data_ AS Data
            			FROM @ListaDatas_ DD
            			LEFT JOIN ConsolidationLevel1 CL1 ON DD.data_ = CAST(CL1.ConsolidationDate AS DATE)
            			INNER JOIN ConsolidationLevel2 CL2 WITH (NOLOCK) ON CL2.ConsolidationLevel1_Id = CL1.Id
            			INNER JOIN ParLevel1 PL1 WITH (NOLOCK) ON CL1.ParLevel1_Id = PL1.Id
            			INNER JOIN CollectionLevel2 C2 WITH (NOLOCK) ON C2.ConsolidationLevel2_Id = CL2.Id
            			INNER JOIN ParCompany PC WITH (NOLOCK) ON C2.UnitId = PC.Id
            			INNER JOIN Result_Level3 R3 WITH (NOLOCK) ON R3.CollectionLevel2_Id = C2.Id
            			LEFT JOIN CollectionLevel2XCluster C2XC WITH (NOLOCK) ON C2XC.CollectionLevel2_Id = C2.Id
			            --LEFT JOIN CollectionLevel2XParDepartment CL2XPD WITH (NOLOCK) ON CL2XPD.CollectionLevel2_Id = C2.Id
						INNER JOIN ParLevel2 Pl2 WITH (NOLOCK) ON CL2.ParLevel2_Id = PL2.Id
                        LEFT JOIN ParDepartment PD WITH (NOLOCK) on PD.Id = PL2.ParDepartment_Id
            			OUTER APPLY (SELECT TOP 1
            					*
            				FROM ParLevel1XModule L1XM WITH (NOLOCK)
            				WHERE L1XM.ParLevel1_Id = C2.ParLevel1_Id
            				AND L1XM.IsActive = 1
            				AND L1XM.EffectiveDateStart <= C2.CollectionDate
            				ORDER BY L1XM.EffectiveDateStart DESC) AS L1XM
            			OUTER APPLY (SELECT TOP 1
            					*
            				FROM ParLevel1XCluster L1XC WITH (NOLOCK)
            				WHERE L1XC.ParLevel1_Id = C2.ParLevel1_Id
            				AND L1XC.ParCluster_Id = C2XC.ParCluster_Id
            				AND L1XC.EffectiveDate <= C2.CollectionDate
            				AND L1XC.IsActive = 1
            				ORDER BY L1XC.EffectiveDate DESC) AS L1XC
            			INNER JOIN ParCluster PCL WITH (NOLOCK) ON PCL.Id = C2XC.ParCluster_Id AND PCL.IsActive = 1
            			LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PCXS.ParCompany_Id = C2.UnitId AND PCXS.Active = 1 --ParCriticalLevel
            			LEFT JOIN CorrectiveAction CA WITH (NOLOCK) ON CA.CollectionLevel02Id = C2.Id
            			WHERE 1 = 1
            			AND C2.ParLevel1_Id = @INDICADOR --Indicador
            			{wModulo}
            			{wParClusterGroup}
            			{wParCluster}	
            			{wParStructure}
            			{wParCompany}
            			{wTurno}
                        {wParCriticalLevel}
                        {wParGroupParLevel1}   
                        {wParDepartment}
            			{wParLevel2}
            			{wParLevel3}
                        {wAcaoStatus}
            			{wPeso}
                        {wSurpervisor}           			
            			GROUP BY PL1.Id
            					,PL1.IsRuleConformity
            					,PL1.Name
            					,PC.Id
            					,PC.Name
            					,DD.data_
            					,PL1.hashKey
            					,PL1.ParConsolidationType_Id
            					,C2.ParLevel1_Id
            					,C2.UnitId
            					,C2.CollectionDate) S1
            		GROUP BY S1.level1_Id
            				,S1.IsRuleConformity
            				,S1.Level1Name
            				,S1.TituloGrafico
            				,S1.Unidade_Id
            				,S1.Unidade_Name
            				,{wPeriodo}
                            {groupBy}) S2) S3
            	Where 1 = 1           	
            ORDER BY {orderBy}";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retornoTendencia = factory.SearchQuery<TendenciaResultSet>(query).ToList();
            }

            //Status do Indicador
            if (retornoTendencia.Count > 0 && form.ParLevel1Status_Ids != null && form.ParLevel1Status_Ids.Length > 0 && form.ParLevel1Status_Ids[0] != 0)
            {
                var somaPorcentagemNC = retornoTendencia.Select(x => x.PorcentagemNc).Aggregate((somaNC, NC) => somaNC + NC);
                var mediaPorcentagem = (somaPorcentagemNC / retornoTendencia.Count);
                var dentroMeta = mediaPorcentagem <= retornoTendencia[0].Meta;

                if (form.ParLevel1Status_Ids[0] == 1 && dentroMeta)//Dentro da meta
                    return retornoTendencia;

                if (form.ParLevel1Status_Ids[0] == 2 && !dentroMeta)//Fora da meta
                    return retornoTendencia;

                return new List<TendenciaResultSet>();
            }
            else
            {
                return retornoTendencia;
            }
        }

        private List<AcaoResultSet> GetAcaoCorretivaIndicador(DataCarrierFormularioNew form, int ParLevel1_Id)
        {
            var retornoAcaoCorretivaIndicador = new List<AcaoResultSet>();

            var wModulo = "";
            var wParClusterGroup = "";
            var wParCluster = "";
            var wParStructure = "";
            var wParCompany = "";
            var wTurno = "";
            var wParCriticalLevel = "";
            var wParGroupParLevel1 = "";
            var wParDepartment = "";
            var wParLevel2 = "";
            var wParLevel3 = "";
            var wAcaoStatus = "";
            var wPeso = "";
            var wSurpervisor = "";

            //Módulo
            if (form.ParModule_Ids != null && form.ParModule_Ids.Length > 0)
                wModulo = $" AND L1XM.ParModule_Id IN ({form.ParModule_Ids[0]}) --Modulo";

            //Grupo de Processo
            if (form.ParClusterGroup_Ids != null && form.ParClusterGroup_Ids.Length > 0)
                wParClusterGroup = $" AND PCL.ParClusterGroup_Id IN ({string.Join(",", form.ParClusterGroup_Ids)}) --Grupo de Processo";

            //Processo
            if (form.ParCluster_Ids != null && form.ParCluster_Ids.Length > 0)
                wParCluster = $" AND C2XC.ParCluster_Id IN ({string.Join(",", form.ParCluster_Ids)}) --Processo";

            //Regional
            if (form.ParStructure_Ids != null && form.ParStructure_Ids.Length > 0)
                wParStructure = $" AND PCXS.ParStructure_Id IN ({ string.Join(",", form.ParStructure_Ids)}) --Regional)";

            //Unidade
            if (form.ParCompany_Ids != null && form.ParCompany_Ids.Length > 0)
                wParCompany = $" AND PC.Id in ({string.Join(",", form.ParCompany_Ids)}) --Unidade";

            //Turno
            if (form.Shift_Ids != null && form.Shift_Ids.Length > 0)
                wTurno = $" AND C2.Shift in ({string.Join(",", form.Shift_Ids)}) --Turno";

            //Nível Criticidade
            if (form.ParCriticalLevel_Ids != null && form.ParCriticalLevel_Ids.Length > 0)
                wParCriticalLevel = $" AND L1XC.ParCriticalLevel_Id IN ({string.Join(",", form.ParCriticalLevel_Ids)}) --Nível Criticidade";

            //Função
            if (form.ParGroupParLevel1_Ids != null && form.ParGroupParLevel1_Ids.Length > 0)
                wParGroupParLevel1 = $" AND PL1.ParGroupLevel1_Id IN ({string.Join(",", form.ParGroupParLevel1_Ids)}) --Função";

            //Departamento
            if (form.ParDepartment_Ids != null && form.ParDepartment_Ids.Length > 0)
                wParDepartment = $" AND PL2.ParDepartment_Id IN ({string.Join(",", form.ParDepartment_Ids)}) --Departamento";

            //Indicador
            //if (form.ParLevel1_Ids != null && form.ParLevel1_Ids.Length > 0)
            //    wParLevel1 = $" {string.Join(",", form.ParLevel1_Ids)})";

            //Monitoramento
            if (form.ParLevel2_Ids != null && form.ParLevel2_Ids.Length > 0)
                wParLevel2 = $" AND C2.ParLevel2_Id IN ({string.Join(",", form.ParLevel2_Ids)}) --Monitoramento";

            //Tarefa
            if (form.ParLevel3_Ids != null && form.ParLevel3_Ids.Length > 0)
                wParLevel3 = $" AND R3.ParLevel3_Id IN ({string.Join(",", form.ParLevel3_Ids)}) --Tarefa";



            //Plano de Ação Concluido
            if (form.AcaoStatus != null && form.AcaoStatus.Length > 0)
            {
                if (form.AcaoStatus[0] == 1) //Concluido
                    wAcaoStatus = " AND (SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesConcluidas FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN (3, 4, 7, 8) AND Acao.Status NOT IN (1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";

                if (form.AcaoStatus[0] == 2)//Não Concluido
                    wAcaoStatus = " AND(SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesEmAndamento FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN(1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";
            }

            //Peso
            if (form.Peso != null && form.Peso.Length > 0 && form.Peso[0] != null)
                wPeso = $" AND R3.Weight IN ({ string.Join(",", form.Peso) })--Peso";

            //Desdobramento
            //if (form.Desdobramento != null && form.Desdobramento.Length > 0)
            //    wDesdobramento = "";

            //Surpervisor
            if (form.UserSgqSurpervisor_Ids != null && form.UserSgqSurpervisor_Ids.Length > 0)
                wSurpervisor = $" AND C2.AuditorId IN ({string.Join(",", form.UserSgqSurpervisor_Ids)}) --Surpervisor";

            var sql = $@"SELECT
                        	CA.Id
                           ,C2.ParLevel1_Id
                           ,PL1.Name AS ParLevel1_Name
                           ,ParLevel2_Id
                           ,PL2.Name AS ParLevel2_Name
                           ,AuditStartTime
                           ,CA.DescriptionFailure
                           ,CA.ImmediateCorrectiveAction
                           ,CA.PreventativeMeasure
                        FROM CorrectiveAction CA
                        INNER JOIN CollectionLevel2 C2 WITH (NOLOCK) ON C2.Id = CA.CollectionLevel02Id
                        INNER JOIN ParLevel1 PL1 WITH (NOLOCK) ON C2.ParLevel1_Id = PL1.Id
                        INNER JOIN ParLevel2 PL2 WITH (NOLOCK) ON C2.ParLevel2_Id = PL2.Id
                        INNER JOIN ParCompany PC WITH (NOLOCK) ON C2.UnitId = PC.Id
                        --INNER JOIN Result_Level3 R3 WITH (NOLOCK) ON R3.CollectionLevel2_Id = C2.Id
                        LEFT JOIN CollectionLevel2XCluster C2XC WITH (NOLOCK) ON C2XC.CollectionLevel2_Id = C2.Id
                        --LEFT JOIN CollectionLevel2XParDepartment CL2XPD WITH (NOLOCK) ON CL2XPD.CollectionLevel2_Id = C2.Id
                        LEFT JOIN ParDepartment PD WITH (NOLOCK) on PD.Id = PL2.ParDepartment_Id
                        OUTER APPLY (SELECT TOP 1
                        		*
                        	FROM ParLevel1XModule L1XM WITH (NOLOCK)
                        	WHERE L1XM.ParLevel1_Id = C2.ParLevel1_Id
                        	AND L1XM.IsActive = 1
                        	AND L1XM.EffectiveDateStart <= C2.CollectionDate
                        	ORDER BY L1XM.EffectiveDateStart DESC) AS L1XM
                        OUTER APPLY (SELECT TOP 1
                        		*
                        	FROM ParLevel1XCluster L1XC WITH (NOLOCK)
                        	WHERE L1XC.ParLevel1_Id = C2.ParLevel1_Id
                        	AND L1XC.ParCluster_Id = C2XC.ParCluster_Id
                        	AND L1XC.EffectiveDate <= C2.CollectionDate
                        	AND L1XC.IsActive = 1
                        	ORDER BY L1XC.EffectiveDate DESC) AS L1XC
                        INNER JOIN ParCluster PCL WITH (NOLOCK) ON PCL.Id = C2XC.ParCluster_Id AND PCL.IsActive = 1
                        LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PCXS.ParCompany_Id = C2.UnitId AND PCXS.Active = 1 --ParCriticalLevel
                        WHERE 1 = 1
                        AND C2.ParLevel1_Id = { ParLevel1_Id } --Indicador
                        AND CA.DateCorrectiveAction BETWEEN '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00' AND '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59'
                        {wModulo}
                        {wParClusterGroup}
                        {wParCluster}	
                        {wParStructure}
                        {wParCompany}
                        {wTurno}
                        {wParCriticalLevel}
                        {wParGroupParLevel1}   
                        {wParDepartment}
                        {wParLevel2}
                        --{wParLevel3}            			
                        --Periodo
                        {wAcaoStatus}
                        {wPeso}
                        {wSurpervisor}           			";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retornoAcaoCorretivaIndicador = factory.SearchQuery<AcaoResultSet>(sql).ToList();
            }

            return retornoAcaoCorretivaIndicador;
        }

        private List<GraficoNC> GetMonitoramentos(DataCarrierFormularioNew form, int ParLevel1_Id)
        {
            var retornoMonitoramentos = new List<GraficoNC>();

            var wModulo = "";
            var wParClusterGroup = "";
            var wParCluster = "";
            var wParStructure = "";
            var wParCompany = "";
            var wTurno = "";
            var wParCriticalLevel = "";
            var wParGroupParLevel1 = "";
            var wParDepartment = "";
            var wParLevel2 = "";
            var wParLevel3 = "";
            var wAcaoStatus = "";
            var wPeriodo = "";
            var wNCComPeso = "";
            var wPeso = "";
            var wSurpervisor = "";
            var orderBy = "S3.Data";

            //Módulo
            if (form.ParModule_Ids != null && form.ParModule_Ids.Length > 0)
                wModulo = $" AND L1XM.ParModule_Id IN ({form.ParModule_Ids[0]}) --Modulo";

            //Grupo de Processo
            if (form.ParClusterGroup_Ids != null && form.ParClusterGroup_Ids.Length > 0)
                wParClusterGroup = $" AND PCL.ParClusterGroup_Id IN ({string.Join(",", form.ParClusterGroup_Ids)}) --Grupo de Processo";

            //Processo
            if (form.ParCluster_Ids != null && form.ParCluster_Ids.Length > 0)
                wParCluster = $" AND C2XC.ParCluster_Id IN ({string.Join(",", form.ParCluster_Ids)}) --Processo";

            //Regional
            if (form.ParStructure_Ids != null && form.ParStructure_Ids.Length > 0)
                wParStructure = $" AND PCXS.ParStructure_Id IN ({ string.Join(",", form.ParStructure_Ids)}) --Regional)";

            //Unidade
            if (form.ParCompany_Ids != null && form.ParCompany_Ids.Length > 0)
                wParCompany = $" AND PC.Id in ({string.Join(",", form.ParCompany_Ids)}) --Unidade";

            //Turno
            if (form.Shift_Ids != null && form.Shift_Ids.Length > 0)
                wTurno = $" AND C2.Shift in ({string.Join(",", form.Shift_Ids)}) --Turno";

            //Nível Criticidade
            if (form.ParCriticalLevel_Ids != null && form.ParCriticalLevel_Ids.Length > 0)
                wParCriticalLevel = $" AND L1XC.ParCriticalLevel_Id IN ({string.Join(",", form.ParCriticalLevel_Ids)}) --Nível Criticidade";

            //Função
            if (form.ParGroupParLevel1_Ids != null && form.ParGroupParLevel1_Ids.Length > 0)
                wParGroupParLevel1 = $" AND PL1.ParGroupLevel1_Id IN ({string.Join(",", form.ParGroupParLevel1_Ids)}) --Função";

            //Departamento
            if (form.ParDepartment_Ids != null && form.ParDepartment_Ids.Length > 0)
                wParDepartment = $" AND PL2.ParDepartment_Id IN ({string.Join(",", form.ParDepartment_Ids)}) --Departamento";

            //Indicador
            //if (form.ParLevel1_Ids != null && form.ParLevel1_Ids.Length > 0)
            //    wParLevel1 = $" {string.Join(",", form.ParLevel1_Ids)})";

            //Monitoramento
            if (form.ParLevel2_Ids != null && form.ParLevel2_Ids.Length > 0)
                wParLevel2 = $" AND C2.ParLevel2_Id IN ({string.Join(",", form.ParLevel2_Ids)}) --Monitoramento";

            //Tarefa
            if (form.ParLevel3_Ids != null && form.ParLevel3_Ids.Length > 0)
                wParLevel3 = $" AND R3.ParLevel3_Id IN ({string.Join(",", form.ParLevel3_Ids)}) --Tarefa";



            //Plano de Ação Concluido
            if (form.AcaoStatus != null && form.AcaoStatus.Length > 0)
            {
                if (form.AcaoStatus[0] == 1) //Concluido
                    wAcaoStatus = " AND (SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesConcluidas FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN (3, 4, 7, 8) AND Acao.Status NOT IN (1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";

                if (form.AcaoStatus[0] == 2)//Não Concluido
                    wAcaoStatus = " AND(SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesEmAndamento FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN(1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";
            }

            //Periodo
            if (form.Periodo != null && form.Periodo.Length > 0)
            {
                if (form.Periodo[0] == 1)//Diario
                {
                    wPeriodo = "convert(NVARCHAR, Data, 111)";
                }

                if (form.Periodo[0] == 2)//Semanal
                {
                    wPeriodo = "CONCAT(CONCAT(DATEPART(YEAR, S1.Data), '-'), DatePart(WEEK, S1.Data))";
                    orderBy = "CONVERT(INT,replace(S3.Data, '-', ''))";
                }
                if (form.Periodo[0] == 3)//Mensal
                {
                    wPeriodo = "CONCAT(CONCAT(DATEPART(YEAR, S1.Data), '-'), DatePart(MONTH, S1.Data))";
                    orderBy = "CONVERT(INT,replace(S3.Data, '-', ''))";
                }
            }

            //Exibe NC Com peso
            if (form.NcComPeso != null && form.NcComPeso.Length > 0)
            {
                if (form.NcComPeso[0] == 1) //Com Peso
                    wNCComPeso = "'NcComPeso'";
                if (form.NcComPeso[0] == 2) //Sem Peso
                    wNCComPeso = "'NcSemPeso'";
            }

            //Peso
            if (form.Peso != null && form.Peso.Length > 0 && form.Peso[0] != null)
                wPeso = $" AND R3.Weight IN ({ string.Join(",", form.Peso) })--Peso";

            //Surpervisor
            if (form.UserSgqSurpervisor_Ids != null && form.UserSgqSurpervisor_Ids.Length > 0)
                wSurpervisor = $" AND C2.AuditorId IN ({string.Join(",", form.UserSgqSurpervisor_Ids)}) --Surpervisor";

            var query = $@" 

             
            DECLARE @INDICADOR INT = {ParLevel1_Id}
            
            DECLARE @DATAINICIAL datetime = '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00'
            DECLARE @DATAFINAL datetime = '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59'
             
            DECLARE @RESS INT
            
            SELECT
            	@RESS = COUNT(1)
            FROM (SELECT
            		COUNT(1) AS NA
            	FROM CollectionLevel2 C2 WITH (NOLOCK)
            	LEFT JOIN Result_Level3 C3 WITH (NOLOCK)
            		ON C3.CollectionLevel2_Id = C2.Id
            	WHERE CONVERT(DATE, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL
            	AND C2.ParLevel1_Id = (SELECT TOP 1 Id FROM ParLevel1 WITH (NOLOCK) WHERE hashKey = 1)
            	AND C2.UnitId IN (1)
            	AND IsNotEvaluate = 1
            	GROUP BY C2.Id) NA
            WHERE NA = 2
            
            SELECT
            	level2_Id as Id
               ,Level2Name as Name
               ,PorcentagemNc as NCPercent
               ,(CASE
            		WHEN IsRuleConformity = 1 THEN (100 - Meta)
            		WHEN IsRuleConformity IS NULL THEN 0
            		ELSE Meta
            	END) AS Meta	
               ,IIF({wNCComPeso} = 'NcComPeso', NC , NcSemPeso) AS NC --Exibe NC Com peso 
               ,IIF({wNCComPeso} = 'NcComPeso', AV , AvSemPeso) as AV --Exibe Av Com peso 
            FROM (SELECT
            		*
            	   ,CASE WHEN Av IS NULL OR Av = 0 THEN 0 ELSE NC / Av * 100 END AS PorcentagemNc
            	   ,CASE WHEN Av IS NULL OR Av = 0 THEN 0 ELSE NCSemPeso / Av * 100 END AS PorcentagemNcSemPeso
            	   ,CASE WHEN CASE WHEN Av IS NULL OR Av = 0 THEN 0 ELSE NC / Av * 100 END >= (CASE WHEN IsRuleConformity = 1 THEN (100 - Meta) ELSE Meta END) THEN 1 ELSE 0 END RELATORIO_DIARIO
            	FROM (SELECT
            			level2_Id
            		   ,IsRuleConformity
            		   ,Level2Name
            		   ,TituloGrafico
            		   ,Unidade_Id
            		   ,Unidade_Name
            		   --,convert(NVARCHAR, Data, 111) as Data
            		   ,SUM(Av) AS Av
            		   ,SUM(AvSemPeso) AS AvSemPeso
            		   ,SUM(NC) AS NC
            		   ,SUM(NCSemPeso) AS NCSemPeso
            		   ,MAX(Meta) AS Meta
            		FROM (SELECT
            				PL2.Id AS level2_Id
            			   ,PL2.Name AS Level2Name
            			   ,'Tendência do Indicador ' + PL2.Name AS TituloGrafico
            			   ,PL1.IsRuleConformity
            			   ,PC.Id AS Unidade_Id
            			   ,PC.Name AS Unidade_Name
			               ,CASE
			            		WHEN PL1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) - @RESS FROM VolumePcc1b WITH (NOLOCK) WHERE ParCompany_Id = PC.Id AND Data = cast(C2.CollectionDate as date))
			            		WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.WeiEvaluation)
			            		WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiEvaluation)
			            		WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT ""KEY"")
			            		WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT ""KEY"")
			            		ELSE 0
			            	END AS Av
			               ,CASE
			            		WHEN PL1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) - @RESS FROM VolumePcc1b WITH (NOLOCK) WHERE ParCompany_Id = PC.Id AND Data = cast(C2.CollectionDate as date))
			            		WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.Evaluation)
			            		WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiEvaluation)
			            		WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT ""KEY"")
			            		WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT ""KEY"")
			            		ELSE 0
			            	END AS AvSemPeso
			               ,CASE
			            		WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.WeiDefects)
			            		WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiDefects)
			            		WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT IIF(R3.WeiDefects>0,""KEY"",NULL))
			            		WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT IIF(R3.WeiDefects>0,""KEY"",NULL))
			            		ELSE 0
			            	END AS NC
			               ,CASE
			            		WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.Defects)
			            		WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiDefects)
			            		WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT IIF(R3.WeiDefects>0,""KEY"",NULL))
			            		WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT IIF(R3.WeiDefects>0,""KEY"",NULL))
			            		ELSE 0
			            	END AS NCSemPeso
            			   ,CASE
            					WHEN (SELECT
            								COUNT(1)
            							FROM ParGoal G WITH (NOLOCK)
            							WHERE G.ParLevel1_Id = C2.ParLevel1_Id
            							AND (G.ParCompany_Id = C2.UnitId
            							OR G.ParCompany_Id IS NULL)
            							AND G.IsActive = 1
            							AND G.EffectiveDate <= @DATAFINAL)
            						> 0 THEN (SELECT TOP 1
            								ISNULL(G.PercentValue, 0)
            							FROM ParGoal G WITH (NOLOCK)
            							WHERE G.ParLevel1_Id = C2.ParLevel1_Id
            							AND (G.ParCompany_Id = C2.UnitId
            							OR G.ParCompany_Id IS NULL)
            							AND G.IsActive = 1
            							AND G.EffectiveDate <= @DATAFINAL
            							ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
            
            					ELSE (SELECT TOP 1
            								ISNULL(G.PercentValue, 0)
            							FROM ParGoal G WITH (NOLOCK)
            							WHERE G.ParLevel1_Id = C2.ParLevel1_Id
            							AND (G.ParCompany_Id = C2.UnitId
            							OR G.ParCompany_Id IS NULL)
            							ORDER BY G.ParCompany_Id DESC, EffectiveDate ASC)
            				END
            				AS Meta
            			FROM ConsolidationLevel1 CL1 
            			INNER JOIN ConsolidationLevel2 CL2 WITH (NOLOCK) ON CL2.ConsolidationLevel1_Id = CL1.Id
            			INNER JOIN ParLevel1 PL1 WITH (NOLOCK) ON CL1.ParLevel1_Id = PL1.Id
            			INNER JOIN CollectionLevel2 C2 WITH (NOLOCK) ON C2.ConsolidationLevel2_Id = CL2.Id
            			INNER JOIN ParLevel2 PL2 WITH (NOLOCK) ON C2.ParLevel2_Id = PL2.Id
            			INNER JOIN ParCompany PC WITH (NOLOCK) ON C2.UnitId = PC.Id
            			INNER JOIN Result_Level3 R3 WITH (NOLOCK) ON R3.CollectionLevel2_Id = C2.Id
            			LEFT JOIN CollectionLevel2XCluster C2XC WITH (NOLOCK) ON C2XC.CollectionLevel2_Id = C2.Id
			            --LEFT JOIN CollectionLevel2XParDepartment CL2XPD WITH (NOLOCK) ON CL2XPD.CollectionLevel2_Id = C2.Id
                        LEFT JOIN ParDepartment PD WITH (NOLOCK) on PD.Id = PL2.ParDepartment_Id
            			OUTER APPLY (SELECT TOP 1
            					*
            				FROM ParLevel1XModule L1XM WITH (NOLOCK)
            				WHERE L1XM.ParLevel1_Id = C2.ParLevel1_Id
            				AND L1XM.IsActive = 1
            				AND L1XM.EffectiveDateStart <= C2.CollectionDate
            				ORDER BY L1XM.EffectiveDateStart DESC) AS L1XM
            			OUTER APPLY (SELECT TOP 1
            					*
            				FROM ParLevel1XCluster L1XC WITH (NOLOCK)
            				WHERE L1XC.ParLevel1_Id = C2.ParLevel1_Id
            				AND L1XC.ParCluster_Id = C2XC.ParCluster_Id
            				AND L1XC.EffectiveDate <= C2.CollectionDate
            				AND L1XC.IsActive = 1
            				ORDER BY L1XC.EffectiveDate DESC) AS L1XC
            			INNER JOIN ParCluster PCL WITH (NOLOCK) ON PCL.Id = C2XC.ParCluster_Id AND PCL.IsActive = 1
            			LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PCXS.ParCompany_Id = C2.UnitId AND PCXS.Active = 1 --ParCriticalLevel
            			LEFT JOIN CorrectiveAction CA WITH (NOLOCK) ON CA.CollectionLevel02Id = C2.Id
            			WHERE 1 = 1
						AND C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
            			AND C2.ParLevel1_Id = @INDICADOR --Indicador
            			{wModulo}
            			{wParClusterGroup}
            			{wParCluster}	
            			{wParStructure}
            			{wParCompany}
            			{wTurno}
                        {wParCriticalLevel}
                        {wParGroupParLevel1}   
                        {wParDepartment}
            			{wParLevel2}
            			{wParLevel3}
                        {wAcaoStatus}
            			{wPeso}
                        {wSurpervisor} 
            			GROUP BY PL2.Id
            					,PL1.IsRuleConformity
            					,PL2.Name
            					,PC.Id
            					,PC.Name
            					,C2.CollectionDate
            					,PL1.hashKey
            					,PL1.ParConsolidationType_Id
            					,C2.ParLevel1_Id
            					,C2.UnitId
            					) S1
            		GROUP BY S1.level2_Id
            				,S1.IsRuleConformity
            				,S1.Level2Name
            				,S1.TituloGrafico
            				,S1.Unidade_Id
            				,S1.Unidade_Name
							) S2) S3
            	Where 1 = 1     
                AND S3.NC > 0
            ORDER BY S3.PorcentagemNc DESC";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retornoMonitoramentos = factory.SearchQuery<GraficoNC>(query).ToList();
            }


            return retornoMonitoramentos;
        }

        private List<AcaoResultSet> GetAcaoCorretivaMonitoramento(DataCarrierFormularioNew form, int ParLevel1_Id, int? ParDepartment_Id = null)
        {
            var retornoAcaoCorretivaMonitoramento = new List<AcaoResultSet>();

            var wModulo = "";
            var wParClusterGroup = "";
            var wParCluster = "";
            var wParStructure = "";
            var wParCompany = "";
            var wTurno = "";
            var wParCriticalLevel = "";
            var wParGroupParLevel1 = "";
            var wParDepartment = "";
            var wParLevel2 = "";
            var wParLevel3 = "";
            var wAcaoStatus = "";
            var wPeso = "";
            var wSurpervisor = "";

            //Módulo
            if (form.ParModule_Ids != null && form.ParModule_Ids.Length > 0)
                wModulo = $" AND L1XM.ParModule_Id IN ({form.ParModule_Ids[0]}) --Modulo";

            //Grupo de Processo
            if (form.ParClusterGroup_Ids != null && form.ParClusterGroup_Ids.Length > 0)
                wParClusterGroup = $" AND PCL.ParClusterGroup_Id IN ({string.Join(",", form.ParClusterGroup_Ids)}) --Grupo de Processo";

            //Processo
            if (form.ParCluster_Ids != null && form.ParCluster_Ids.Length > 0)
                wParCluster = $" AND C2XC.ParCluster_Id IN ({string.Join(",", form.ParCluster_Ids)}) --Processo";

            //Regional
            if (form.ParStructure_Ids != null && form.ParStructure_Ids.Length > 0)
                wParStructure = $" AND PCXS.ParStructure_Id IN ({ string.Join(",", form.ParStructure_Ids)}) --Regional)";

            //Unidade
            if (form.ParCompany_Ids != null && form.ParCompany_Ids.Length > 0)
                wParCompany = $" AND PC.Id in ({string.Join(",", form.ParCompany_Ids)}) --Unidade";

            //Turno
            if (form.Shift_Ids != null && form.Shift_Ids.Length > 0)
                wTurno = $" AND C2.Shift in ({string.Join(",", form.Shift_Ids)}) --Turno";

            //Nível Criticidade
            if (form.ParCriticalLevel_Ids != null && form.ParCriticalLevel_Ids.Length > 0)
                wParCriticalLevel = $" AND L1XC.ParCriticalLevel_Id IN ({string.Join(",", form.ParCriticalLevel_Ids)}) --Nível Criticidade";

            //Função
            if (form.ParGroupParLevel1_Ids != null && form.ParGroupParLevel1_Ids.Length > 0)
                wParGroupParLevel1 = $" AND PL1.ParGroupLevel1_Id IN ({string.Join(",", form.ParGroupParLevel1_Ids)}) --Função";

            //Departamento
            if (form.ParDepartment_Ids != null && form.ParDepartment_Ids.Length > 0)
                wParDepartment = $" AND PL2.ParDepartment_Id IN ({string.Join(",", form.ParDepartment_Ids)}) --Departamento";

            if(ParDepartment_Id != null)
                wParDepartment += $" AND PL2.ParDepartment_Id IN ({ParDepartment_Id.ToString()}) --Departamento";

            //Indicador
            //if (form.ParLevel1_Ids != null && form.ParLevel1_Ids.Length > 0)
            //    wParLevel1 = $" {string.Join(",", form.ParLevel1_Ids)})";

            //Monitoramento
            if (form.ParLevel2_Ids != null && form.ParLevel2_Ids.Length > 0)
                wParLevel2 = $" AND C2.ParLevel2_Id IN ({string.Join(",", form.ParLevel2_Ids)}) --Monitoramento";

            //Tarefa
            if (form.ParLevel3_Ids != null && form.ParLevel3_Ids.Length > 0)
                wParLevel3 = $" AND R3.ParLevel3_Id IN ({string.Join(",", form.ParLevel3_Ids)}) --Tarefa";



            //Plano de Ação Concluido
            if (form.AcaoStatus != null && form.AcaoStatus.Length > 0)
            {
                if (form.AcaoStatus[0] == 1) //Concluido
                    wAcaoStatus = " AND (SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesConcluidas FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN (3, 4, 7, 8) AND Acao.Status NOT IN (1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";

                if (form.AcaoStatus[0] == 2)//Não Concluido
                    wAcaoStatus = " AND(SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesEmAndamento FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN(1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";
            }

            //Peso
            if (form.Peso != null && form.Peso.Length > 0 && form.Peso[0] != null)
                wPeso = $" AND R3.Weight IN ({ string.Join(",", form.Peso) })--Peso";

            //Desdobramento
            //if (form.Desdobramento != null && form.Desdobramento.Length > 0)
            //    wDesdobramento = "";

            //Surpervisor
            if (form.UserSgqSurpervisor_Ids != null && form.UserSgqSurpervisor_Ids.Length > 0)
                wSurpervisor = $" AND C2.AuditorId IN ({string.Join(",", form.UserSgqSurpervisor_Ids)}) --Surpervisor";

            var sql = $@"SELECT
                        	CA.Id
                           ,C2.ParLevel1_Id
                           ,PL1.Name AS ParLevel1_Name
                           ,ParLevel2_Id
                           ,PL2.Name AS ParLevel2_Name
                           ,AuditStartTime
                           ,CA.DescriptionFailure
                           ,CA.ImmediateCorrectiveAction
                           ,CA.PreventativeMeasure
                        FROM CorrectiveAction CA
                        INNER JOIN CollectionLevel2 C2 WITH (NOLOCK) ON C2.Id = CA.CollectionLevel02Id
                        INNER JOIN ParLevel1 PL1 WITH (NOLOCK) ON C2.ParLevel1_Id = PL1.Id
                        INNER JOIN ParLevel2 PL2 WITH (NOLOCK) ON C2.ParLevel2_Id = PL2.Id
                        INNER JOIN ParCompany PC WITH (NOLOCK) ON C2.UnitId = PC.Id
                        --INNER JOIN Result_Level3 R3 WITH (NOLOCK) ON R3.CollectionLevel2_Id = C2.Id
                        LEFT JOIN CollectionLevel2XCluster C2XC WITH (NOLOCK) ON C2XC.CollectionLevel2_Id = C2.Id
                        --LEFT JOIN CollectionLevel2XParDepartment CL2XPD WITH (NOLOCK) ON CL2XPD.CollectionLevel2_Id = C2.Id
                        LEFT JOIN ParDepartment PD WITH (NOLOCK) on PD.Id = PL2.ParDepartment_Id
                        OUTER APPLY (SELECT TOP 1
                        		*
                        	FROM ParLevel1XModule L1XM WITH (NOLOCK)
                        	WHERE L1XM.ParLevel1_Id = C2.ParLevel1_Id
                        	AND L1XM.IsActive = 1
                        	AND L1XM.EffectiveDateStart <= C2.CollectionDate
                        	ORDER BY L1XM.EffectiveDateStart DESC) AS L1XM
                        OUTER APPLY (SELECT TOP 1
                        		*
                        	FROM ParLevel1XCluster L1XC WITH (NOLOCK)
                        	WHERE L1XC.ParLevel1_Id = C2.ParLevel1_Id
                        	AND L1XC.ParCluster_Id = C2XC.ParCluster_Id
                        	AND L1XC.EffectiveDate <= C2.CollectionDate
                        	AND L1XC.IsActive = 1
                        	ORDER BY L1XC.EffectiveDate DESC) AS L1XC
                        INNER JOIN ParCluster PCL WITH (NOLOCK) ON PCL.Id = C2XC.ParCluster_Id AND PCL.IsActive = 1
                        LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PCXS.ParCompany_Id = C2.UnitId AND PCXS.Active = 1 --ParCriticalLevel
                        WHERE 1 = 1
                        AND C2.ParLevel1_Id = { ParLevel1_Id } --Indicador
                        AND CA.DateCorrectiveAction BETWEEN '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00' AND '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59'
                        {wModulo}
                        {wParClusterGroup}
                        {wParCluster}	
                        {wParStructure}
                        {wParCompany}
                        {wTurno}
                        {wParCriticalLevel}
                        {wParGroupParLevel1}   
                        {wParDepartment}
                        {wParLevel2}
                        --{wParLevel3}            			
                        --Periodo
                        {wAcaoStatus}
                        {wPeso}
                        {wSurpervisor}";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retornoAcaoCorretivaMonitoramento = factory.SearchQuery<AcaoResultSet>(sql).ToList();
            }

            return retornoAcaoCorretivaMonitoramento;
        }

        private List<GraficoNC> GetMonitoramentosDepartamento(DataCarrierFormularioNew form, int ParLevel1_Id)
        {
            var retornoMonitoramentosDepartamento = new List<GraficoNC>();

            var wModulo = "";
            var wParClusterGroup = "";
            var wParCluster = "";
            var wParStructure = "";
            var wParCompany = "";
            var wTurno = "";
            var wParCriticalLevel = "";
            var wParGroupParLevel1 = "";
            var wParDepartment = "";
            var wParLevel2 = "";
            var wParLevel3 = "";
            var wAcaoStatus = "";
            //var wPeriodo = "";
            var wNCComPeso = "";
            var wPeso = "";
            var wSurpervisor = "";
            //var orderBy = "S3.Data";

            //Módulo
            if (form.ParModule_Ids != null && form.ParModule_Ids.Length > 0)
                wModulo = $" AND L1XM.ParModule_Id IN ({form.ParModule_Ids[0]}) --Modulo";

            //Grupo de Processo
            if (form.ParClusterGroup_Ids != null && form.ParClusterGroup_Ids.Length > 0)
                wParClusterGroup = $" AND PCL.ParClusterGroup_Id IN ({string.Join(",", form.ParClusterGroup_Ids)}) --Grupo de Processo";

            //Processo
            if (form.ParCluster_Ids != null && form.ParCluster_Ids.Length > 0)
                wParCluster = $" AND C2XC.ParCluster_Id IN ({string.Join(",", form.ParCluster_Ids)}) --Processo";

            //Regional
            if (form.ParStructure_Ids != null && form.ParStructure_Ids.Length > 0)
                wParStructure = $" AND PCXS.ParStructure_Id IN ({ string.Join(",", form.ParStructure_Ids)}) --Regional)";

            //Unidade
            if (form.ParCompany_Ids != null && form.ParCompany_Ids.Length > 0)
                wParCompany = $" AND PC.Id in ({string.Join(",", form.ParCompany_Ids)}) --Unidade";

            //Turno
            if (form.Shift_Ids != null && form.Shift_Ids.Length > 0)
                wTurno = $" AND C2.Shift in ({string.Join(",", form.Shift_Ids)}) --Turno";

            //Nível Criticidade
            if (form.ParCriticalLevel_Ids != null && form.ParCriticalLevel_Ids.Length > 0)
                wParCriticalLevel = $" AND L1XC.ParCriticalLevel_Id IN ({string.Join(",", form.ParCriticalLevel_Ids)}) --Nível Criticidade";

            //Função
            if (form.ParGroupParLevel1_Ids != null && form.ParGroupParLevel1_Ids.Length > 0)
                wParGroupParLevel1 = $" AND PL1.ParGroupLevel1_Id IN ({string.Join(",", form.ParGroupParLevel1_Ids)}) --Função";

            //Departamento
            if (form.ParDepartment_Ids != null && form.ParDepartment_Ids.Length > 0)
                wParDepartment = $" AND PL2.ParDepartment_Id IN ({string.Join(",", form.ParDepartment_Ids)}) --Departamento";

            //Indicador
            //if (form.ParLevel1_Ids != null && form.ParLevel1_Ids.Length > 0)
            //    wParLevel1 = $" {string.Join(",", form.ParLevel1_Ids)})";

            //Monitoramento
            if (form.ParLevel2_Ids != null && form.ParLevel2_Ids.Length > 0)
                wParLevel2 = $" AND C2.ParLevel2_Id IN ({string.Join(",", form.ParLevel2_Ids)}) --Monitoramento";

            //Tarefa
            if (form.ParLevel3_Ids != null && form.ParLevel3_Ids.Length > 0)
                wParLevel3 = $" AND R3.ParLevel3_Id IN ({string.Join(",", form.ParLevel3_Ids)}) --Tarefa";



            //Plano de Ação Concluido
            if (form.AcaoStatus != null && form.AcaoStatus.Length > 0)
            {
                if (form.AcaoStatus[0] == 1) //Concluido
                    wAcaoStatus = " AND (SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesConcluidas FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN (3, 4, 7, 8) AND Acao.Status NOT IN (1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";

                if (form.AcaoStatus[0] == 2)//Não Concluido
                    wAcaoStatus = " AND(SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesEmAndamento FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN(1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";
            }

            //Periodo
            //if (form.Periodo != null && form.Periodo.Length > 0)
            //{
            //    if (form.Periodo[0] == 1)//Diario
            //    {
            //        wPeriodo = "convert(NVARCHAR, Data, 111)";
            //    }

            //    if (form.Periodo[0] == 2)//Semanal
            //    {
            //        wPeriodo = "CONCAT(CONCAT(DATEPART(YEAR, S1.Data), '-'), DatePart(WEEK, S1.Data))";
            //        orderBy = "CONVERT(INT,replace(S3.Data, '-', ''))";
            //    }
            //    if (form.Periodo[0] == 3)//Mensal
            //    {
            //        wPeriodo = "CONCAT(CONCAT(DATEPART(YEAR, S1.Data), '-'), DatePart(MONTH, S1.Data))";
            //        orderBy = "CONVERT(INT,replace(S3.Data, '-', ''))";
            //    }
            //}

            //Exibe NC Com peso
            if (form.NcComPeso != null && form.NcComPeso.Length > 0)
            {
                if (form.NcComPeso[0] == 1) //Com Peso
                    wNCComPeso = "'NcComPeso'";
                if (form.NcComPeso[0] == 2) //Sem Peso
                    wNCComPeso = "'NcSemPeso'";
            }

            //Peso
            if (form.Peso != null && form.Peso.Length > 0 && form.Peso[0] != null)
                wPeso = $" AND R3.Weight IN ({ string.Join(",", form.Peso) })--Peso";

            //Surpervisor
            if (form.UserSgqSurpervisor_Ids != null && form.UserSgqSurpervisor_Ids.Length > 0)
                wSurpervisor = $" AND C2.AuditorId IN ({string.Join(",", form.UserSgqSurpervisor_Ids)}) --Surpervisor";

            var query = $@" 

             
            DECLARE @INDICADOR INT = {ParLevel1_Id}
            
            DECLARE @DATAINICIAL datetime = '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00'
            DECLARE @DATAFINAL datetime = '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59'
             
            DECLARE @RESS INT
            
            SELECT
            	@RESS = COUNT(1)
            FROM (SELECT
            		COUNT(1) AS NA
            	FROM CollectionLevel2 C2 WITH (NOLOCK)
            	LEFT JOIN Result_Level3 C3 WITH (NOLOCK)
            		ON C3.CollectionLevel2_Id = C2.Id
            	WHERE CONVERT(DATE, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL
            	AND C2.ParLevel1_Id = (SELECT TOP 1 Id FROM ParLevel1 WITH (NOLOCK) WHERE hashKey = 1)
            	AND C2.UnitId IN (1)
            	AND IsNotEvaluate = 1
            	GROUP BY C2.Id) NA
            WHERE NA = 2
            
            SELECT
            	level2_Id as Id
               ,Level2Name as Name
               ,PorcentagemNc as NCPercent
			   ,ParDepartment_Id
			   ,ParDepartment_Name
               ,(CASE
            		WHEN IsRuleConformity = 1 THEN (100 - Meta)
            		WHEN IsRuleConformity IS NULL THEN 0
            		ELSE Meta
            	END) AS Meta	
               ,IIF({wNCComPeso} = 'NcComPeso', NC , NcSemPeso) AS NC --Exibe NC Com peso 
               ,IIF({wNCComPeso} = 'NcComPeso', AV , AvSemPeso) as AV --Exibe Av Com peso 
            FROM (SELECT
            		*
            	   ,CASE WHEN Av IS NULL OR Av = 0 THEN 0 ELSE NC / Av * 100 END AS PorcentagemNc
            	   ,CASE WHEN Av IS NULL OR Av = 0 THEN 0 ELSE NCSemPeso / Av * 100 END AS PorcentagemNcSemPeso
            	   ,CASE WHEN CASE WHEN Av IS NULL OR Av = 0 THEN 0 ELSE NC / Av * 100 END >= (CASE WHEN IsRuleConformity = 1 THEN (100 - Meta) ELSE Meta END) THEN 1 ELSE 0 END RELATORIO_DIARIO
            	FROM (SELECT
            			level2_Id
            		   ,IsRuleConformity
            		   ,Level2Name
            		   ,TituloGrafico
            		   ,Unidade_Id
            		   ,Unidade_Name
            		   ,IIF(ParDepartment_Id IS NULL, 0, ParDepartment_Id) as ParDepartment_Id
					   ,IIF(ParDepartment_Name IS NULL, 'Sem departamento', ParDepartment_Name) as ParDepartment_Name
            		   ,SUM(Av) AS Av
            		   ,SUM(AvSemPeso) AS AvSemPeso
            		   ,SUM(NC) AS NC
            		   ,SUM(NCSemPeso) AS NCSemPeso
            		   ,MAX(Meta) AS Meta
            		FROM (SELECT
            				PL2.Id AS level2_Id
            			   ,PL2.Name AS Level2Name
						   ,PD.Id as ParDepartment_Id
						   ,PD.Name as ParDepartment_Name
            			   ,'Tendência do Indicador ' + PL2.Name AS TituloGrafico
            			   ,PL1.IsRuleConformity
            			   ,PC.Id AS Unidade_Id
            			   ,PC.Name AS Unidade_Name
			               ,CASE
			            		WHEN PL1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) - @RESS FROM VolumePcc1b WITH (NOLOCK) WHERE ParCompany_Id = PC.Id AND Data = cast(C2.CollectionDate as date))
			            		WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.WeiEvaluation)
			            		WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiEvaluation)
			            		WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT ""KEY"")
			            		WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT ""KEY"")
			            		ELSE 0
			            	END AS Av
			               ,CASE
			            		WHEN PL1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) - @RESS FROM VolumePcc1b WITH (NOLOCK) WHERE ParCompany_Id = PC.Id AND Data = cast(C2.CollectionDate as date))
			            		WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.Evaluation)
			            		WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiEvaluation)
			            		WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT ""KEY"")
			            		WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT ""KEY"")
			            		ELSE 0
			            	END AS AvSemPeso
			               ,CASE
			            		WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.WeiDefects)
			            		WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiDefects)
			            		WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT IIF(R3.WeiDefects>0,""KEY"",NULL))
			            		WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT IIF(R3.WeiDefects>0,""KEY"",NULL))
			            		ELSE 0
			            	END AS NC
			               ,CASE
			            		WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.Defects)
			            		WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiDefects)
			            		WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT IIF(R3.WeiDefects>0,""KEY"",NULL))
			            		WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT IIF(R3.WeiDefects>0,""KEY"",NULL))
			            		ELSE 0
			            	END AS NCSemPeso
            			   ,CASE
            					WHEN (SELECT
            								COUNT(1)
            							FROM ParGoal G WITH (NOLOCK)
            							WHERE G.ParLevel1_Id = C2.ParLevel1_Id
            							AND (G.ParCompany_Id = C2.UnitId
            							OR G.ParCompany_Id IS NULL)
            							AND G.IsActive = 1
            							AND G.EffectiveDate <= @DATAFINAL)
            						> 0 THEN (SELECT TOP 1
            								ISNULL(G.PercentValue, 0)
            							FROM ParGoal G WITH (NOLOCK)
            							WHERE G.ParLevel1_Id = C2.ParLevel1_Id
            							AND (G.ParCompany_Id = C2.UnitId
            							OR G.ParCompany_Id IS NULL)
            							AND G.IsActive = 1
            							AND G.EffectiveDate <= @DATAFINAL
            							ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
            
            					ELSE (SELECT TOP 1
            								ISNULL(G.PercentValue, 0)
            							FROM ParGoal G WITH (NOLOCK)
            							WHERE G.ParLevel1_Id = C2.ParLevel1_Id
            							AND (G.ParCompany_Id = C2.UnitId
            							OR G.ParCompany_Id IS NULL)
            							ORDER BY G.ParCompany_Id DESC, EffectiveDate ASC)
            				END
            				AS Meta
            			FROM ConsolidationLevel1 CL1 
            			INNER JOIN ConsolidationLevel2 CL2 WITH (NOLOCK) ON CL2.ConsolidationLevel1_Id = CL1.Id
            			INNER JOIN ParLevel1 PL1 WITH (NOLOCK) ON CL1.ParLevel1_Id = PL1.Id
            			INNER JOIN CollectionLevel2 C2 WITH (NOLOCK) ON C2.ConsolidationLevel2_Id = CL2.Id
            			INNER JOIN ParLevel2 PL2 WITH (NOLOCK) ON C2.ParLevel2_Id = PL2.Id
            			INNER JOIN ParCompany PC WITH (NOLOCK) ON C2.UnitId = PC.Id
            			INNER JOIN Result_Level3 R3 WITH (NOLOCK) ON R3.CollectionLevel2_Id = C2.Id
            			LEFT JOIN CollectionLevel2XCluster C2XC WITH (NOLOCK) ON C2XC.CollectionLevel2_Id = C2.Id
			            --LEFT JOIN CollectionLevel2XParDepartment CL2XPD WITH (NOLOCK) ON CL2XPD.CollectionLevel2_Id = C2.Id
						--LEFT JOIN ParDepartment PD WITH (NOLOCK) on PD.Id = CL2XPD.ParDepartment_Id
                        LEFT JOIN ParDepartment PD WITH (NOLOCK) on PD.Id = PL2.ParDepartment_Id
            			OUTER APPLY (SELECT TOP 1
            					*
            				FROM ParLevel1XModule L1XM WITH (NOLOCK)
            				WHERE L1XM.ParLevel1_Id = C2.ParLevel1_Id
            				AND L1XM.IsActive = 1
            				AND L1XM.EffectiveDateStart <= C2.CollectionDate
            				ORDER BY L1XM.EffectiveDateStart DESC) AS L1XM
            			OUTER APPLY (SELECT TOP 1
            					*
            				FROM ParLevel1XCluster L1XC WITH (NOLOCK)
            				WHERE L1XC.ParLevel1_Id = C2.ParLevel1_Id
            				AND L1XC.ParCluster_Id = C2XC.ParCluster_Id
            				AND L1XC.EffectiveDate <= C2.CollectionDate
            				AND L1XC.IsActive = 1
            				ORDER BY L1XC.EffectiveDate DESC) AS L1XC
            			INNER JOIN ParCluster PCL WITH (NOLOCK) ON PCL.Id = C2XC.ParCluster_Id AND PCL.IsActive = 1
            			LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PCXS.ParCompany_Id = C2.UnitId AND PCXS.Active = 1 --ParCriticalLevel
            			LEFT JOIN CorrectiveAction CA WITH (NOLOCK) ON CA.CollectionLevel02Id = C2.Id
            			WHERE 1 = 1
						AND C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
            			AND C2.ParLevel1_Id = @INDICADOR --Indicador
            			{wModulo}
            			{wParClusterGroup}
            			{wParCluster}	
            			{wParStructure}
            			{wParCompany}
            			{wTurno}
                        {wParCriticalLevel}
                        {wParGroupParLevel1}   
                        {wParDepartment}
            			{wParLevel2}
            			{wParLevel3}
                        {wAcaoStatus}
            			{wPeso}
                        {wSurpervisor} 
            			GROUP BY PL2.Id
            					,PL1.IsRuleConformity
            					,PL2.Name
            					,PC.Id
            					,PC.Name
            					,C2.CollectionDate
            					,PL1.hashKey
            					,PL1.ParConsolidationType_Id
            					,C2.ParLevel1_Id
            					,C2.UnitId
								,PD.Id
								,PD.Name
            					) S1
            		GROUP BY S1.level2_Id
            				,S1.IsRuleConformity
            				,S1.Level2Name
            				,S1.TituloGrafico
            				,S1.Unidade_Id
            				,S1.Unidade_Name
							,S1.ParDepartment_Id
							,S1.ParDepartment_Name
							) S2) S3
            	Where 1 = 1
                AND S3.NC > 0
            ORDER BY S3.PorcentagemNc DESC";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retornoMonitoramentosDepartamento = factory.SearchQuery<GraficoNC>(query).ToList();
            }

            return retornoMonitoramentosDepartamento;
        }

        private List<GraficoNC> GetTarefas(DataCarrierFormularioNew form, int parLevel1_Id)
        {
            var retornoTarefas = new List<GraficoNC>();

            var wModulo = "";
            var wParClusterGroup = "";
            var wParCluster = "";
            var wParStructure = "";
            var wParCompany = "";
            var wTurno = "";
            var wParCriticalLevel = "";
            var wParGroupParLevel1 = "";
            var wParDepartment = "";
            var wParLevel2 = "";
            var wParLevel3 = "";
            var wAcaoStatus = "";
            var wNCComPeso = "";
            var wPeso = "";
            var wSurpervisor = "";

            //Módulo
            if (form.ParModule_Ids != null && form.ParModule_Ids.Length > 0)
                wModulo = $" AND L1XM.ParModule_Id IN ({form.ParModule_Ids[0]}) --Modulo";

            //Grupo de Processo
            if (form.ParClusterGroup_Ids != null && form.ParClusterGroup_Ids.Length > 0)
                wParClusterGroup = $" AND PCL.ParClusterGroup_Id IN ({string.Join(",", form.ParClusterGroup_Ids)}) --Grupo de Processo";

            //Processo
            if (form.ParCluster_Ids != null && form.ParCluster_Ids.Length > 0)
                wParCluster = $" AND C2XC.ParCluster_Id IN ({string.Join(",", form.ParCluster_Ids)}) --Processo";

            //Regional
            if (form.ParStructure_Ids != null && form.ParStructure_Ids.Length > 0)
                wParStructure = $" AND PCXS.ParStructure_Id IN ({ string.Join(",", form.ParStructure_Ids)}) --Regional)";

            //Unidade
            if (form.ParCompany_Ids != null && form.ParCompany_Ids.Length > 0)
                wParCompany = $" AND PC.Id in ({string.Join(",", form.ParCompany_Ids)}) --Unidade";

            //Turno
            if (form.Shift_Ids != null && form.Shift_Ids.Length > 0)
                wTurno = $" AND C2.Shift in ({string.Join(",", form.Shift_Ids)}) --Turno";

            //Nível Criticidade
            if (form.ParCriticalLevel_Ids != null && form.ParCriticalLevel_Ids.Length > 0)
                wParCriticalLevel = $" AND L1XC.ParCriticalLevel_Id IN ({string.Join(",", form.ParCriticalLevel_Ids)}) --Nível Criticidade";

            //Função
            if (form.ParGroupParLevel1_Ids != null && form.ParGroupParLevel1_Ids.Length > 0)
                wParGroupParLevel1 = $" AND PL1.ParGroupLevel1_Id IN ({string.Join(",", form.ParGroupParLevel1_Ids)}) --Função";

            //Departamento
            if (form.ParDepartment_Ids != null && form.ParDepartment_Ids.Length > 0)
                wParDepartment = $" AND PL2.ParDepartment_Id IN ({string.Join(",", form.ParDepartment_Ids)}) --Departamento";

            //Indicador
            //if (form.ParLevel1_Ids != null && form.ParLevel1_Ids.Length > 0)
            //    wParLevel1 = $" {string.Join(",", form.ParLevel1_Ids)})";

            //Monitoramento
            if (form.ParLevel2_Ids != null && form.ParLevel2_Ids.Length > 0)
                wParLevel2 = $" AND C2.ParLevel2_Id IN ({string.Join(",", form.ParLevel2_Ids)}) --Monitoramento";


            //Tarefa
            if (form.ParLevel3_Ids != null && form.ParLevel3_Ids.Length > 0)
                wParLevel3 = $" AND R3.ParLevel3_Id IN ({string.Join(",", form.ParLevel3_Ids)}) --Tarefa";


            //Plano de Ação Concluido
            if (form.AcaoStatus != null && form.AcaoStatus.Length > 0)
            {
                if (form.AcaoStatus[0] == 1) //Concluido
                    wAcaoStatus = " AND (SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesConcluidas FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN (3, 4, 7, 8) AND Acao.Status NOT IN (1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";

                if (form.AcaoStatus[0] == 2)//Não Concluido
                    wAcaoStatus = " AND(SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesEmAndamento FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN(1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";
            }

            //Periodo
            //if (form.Periodo != null && form.Periodo.Length > 0)
            //{
            //    if (form.Periodo[0] == 1)//Diario
            //    {
            //        wPeriodo = "convert(NVARCHAR, Data, 111)";
            //    }

            //    if (form.Periodo[0] == 2)//Semanal
            //    {
            //        wPeriodo = "CONCAT(CONCAT(DATEPART(YEAR, S1.Data), '-'), DatePart(WEEK, S1.Data))";
            //        orderBy = "CONVERT(INT,replace(S3.Data, '-', ''))";
            //    }
            //    if (form.Periodo[0] == 3)//Mensal
            //    {
            //        wPeriodo = "CONCAT(CONCAT(DATEPART(YEAR, S1.Data), '-'), DatePart(MONTH, S1.Data))";
            //        orderBy = "CONVERT(INT,replace(S3.Data, '-', ''))";
            //    }
            //}

            //Exibe NC Com peso
            if (form.NcComPeso != null && form.NcComPeso.Length > 0)
            {
                if (form.NcComPeso[0] == 1) //Com Peso
                    wNCComPeso = "'NcComPeso'";
                if (form.NcComPeso[0] == 2) //Sem Peso
                    wNCComPeso = "'NcSemPeso'";
            }

            //Peso
            if (form.Peso != null && form.Peso.Length > 0 && form.Peso[0] != null)
                wPeso = $" AND R3.Weight IN ({ string.Join(",", form.Peso) })--Peso";

            //Surpervisor
            if (form.UserSgqSurpervisor_Ids != null && form.UserSgqSurpervisor_Ids.Length > 0)
                wSurpervisor = $" AND C2.AuditorId IN ({string.Join(",", form.UserSgqSurpervisor_Ids)}) --Surpervisor";

            var query = $@"


DECLARE @INDICADOR INT = { parLevel1_Id }
DECLARE @DATAINICIAL datetime = '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00'
DECLARE @DATAFINAL datetime = '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59'
DECLARE @RESS INT

SELECT
	@RESS = COUNT(1)
FROM (SELECT
		COUNT(1) AS NA
	FROM CollectionLevel2 C2 WITH (NOLOCK)
	LEFT JOIN Result_Level3 C3 WITH (NOLOCK) ON C3.CollectionLevel2_Id = C2.Id
	WHERE CONVERT(DATE, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL
	AND C2.ParLevel1_Id = (SELECT TOP 1 Id FROM ParLevel1 WITH (NOLOCK) WHERE hashKey = 1)
	AND C2.UnitId IN (1)
	AND IsNotEvaluate = 1
	GROUP BY C2.Id) NA
WHERE NA = 2

SELECT
	*
   ,CASE WHEN AV IS NULL OR AV = 0 THEN 0 ELSE NC / AV * 100 END AS NCPercent
FROM (SELECT
		level3_Id AS Id
	   ,Level3Name AS Name
		--,PorcentagemNc as NCPercent
		--  ,(CASE
		--	WHEN IsRuleConformity = 1 THEN (100 - Meta)
		--	WHEN IsRuleConformity IS NULL THEN 0
		--	ELSE Meta
		--END) AS Meta				   
	   ,SUM(IIF({wNCComPeso} = 'NcComPeso', NC, NcSemPeso)) AS NC --Exibe NC Com peso 
	   ,SUM(IIF({wNCComPeso} = 'NcComPeso', AV, AvSemPeso)) AS AV --Exibe Av Com peso 
	FROM (SELECT
			*
			--,CASE WHEN Av IS NULL OR Av = 0 THEN 0 ELSE NC / Av * 100 END AS PorcentagemNc
			--,CASE WHEN Av IS NULL OR Av = 0 THEN 0 ELSE NCSemPeso / Av * 100 END AS PorcentagemNcSemPeso
		   ,CASE WHEN CASE WHEN AV IS NULL OR AV = 0 THEN 0 ELSE NC / AV * 100 END >= (CASE WHEN IsRuleConformity = 1 THEN (100 - Meta) ELSE Meta END) THEN 1 ELSE 0 END RELATORIO_DIARIO
		FROM (SELECT
				level3_Id
			   ,IsRuleConformity
			   ,Level3Name
			   ,TituloGrafico
			   ,Unidade_Id
			   ,Unidade_Name
			   ,SUM(Av) AS Av
			   ,SUM(AvSemPeso) AS AvSemPeso
			   ,SUM(NC) AS NC
			   ,SUM(NCSemPeso) AS NCSemPeso
			   ,MAX(Meta) AS Meta
			FROM (SELECT
					L3.Id AS level3_Id
				   ,L3.Name AS Level3Name
				   ,'Tendência do Indicador ' + L3.Name AS TituloGrafico
				   ,PL1.IsRuleConformity
				   ,PC.Id AS Unidade_Id
				   ,PC.Name AS Unidade_Name
			       ,CASE
			       	WHEN PL1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) - @RESS FROM VolumePcc1b WITH (NOLOCK) WHERE ParCompany_Id = PC.Id AND Data = cast(C2.CollectionDate as date))
			       	WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.WeiEvaluation)
			       	WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiEvaluation)
			       	WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT ""KEY"")
                        WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT ""KEY"")
			       	ELSE 0
                    END AS Av
			       ,CASE
                        WHEN PL1.hashKey = 1 THEN(SELECT TOP 1 SUM(Quartos) - @RESS FROM VolumePcc1b WITH(NOLOCK) WHERE ParCompany_Id = PC.Id AND Data = cast(C2.CollectionDate as date))
                        WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.Evaluation)
                        WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiEvaluation)
                        WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT ""KEY"")
			       	WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT ""KEY"")
			       	ELSE 0
                    END AS AvSemPeso
			       ,CASE
                        WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.WeiDefects)
                        WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiDefects)
                        WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT IIF(R3.WeiDefects> 0,""KEY"",NULL))
			       	WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT IIF(R3.WeiDefects> 0,""KEY"",NULL))
			       	ELSE 0
                    END AS NC
			       ,CASE
                        WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.Defects)
                        WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiDefects)
                        WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT IIF(R3.WeiDefects> 0,""KEY"",NULL))
			       	WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT IIF(R3.WeiDefects> 0,""KEY"",NULL))
			       	ELSE 0
                    END AS NCSemPeso
				   ,CASE
						WHEN (SELECT
									COUNT(1)
								FROM ParGoal G WITH (NOLOCK)
								WHERE G.ParLevel1_Id = C2.ParLevel1_Id
								AND (G.ParCompany_Id = C2.UnitId
								OR G.ParCompany_Id IS NULL)
								AND G.IsActive = 1
								AND G.EffectiveDate <= @DATAFINAL)
							> 0 THEN (SELECT TOP 1
									ISNULL(G.PercentValue, 0)
								FROM ParGoal G WITH (NOLOCK)
								WHERE G.ParLevel1_Id = C2.ParLevel1_Id
								AND (G.ParCompany_Id = C2.UnitId
								OR G.ParCompany_Id IS NULL)
								AND G.IsActive = 1
								AND G.EffectiveDate <= @DATAFINAL
								ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)

						ELSE (SELECT TOP 1
									ISNULL(G.PercentValue, 0)
								FROM ParGoal G WITH (NOLOCK)
								WHERE G.ParLevel1_Id = C2.ParLevel1_Id
								AND (G.ParCompany_Id = C2.UnitId
								OR G.ParCompany_Id IS NULL)
								ORDER BY G.ParCompany_Id DESC, EffectiveDate ASC)
					END
					AS Meta
				FROM ConsolidationLevel1 CL1
				INNER JOIN ConsolidationLevel2 CL2 WITH (NOLOCK) ON CL2.ConsolidationLevel1_Id = CL1.Id
				INNER JOIN ParLevel1 PL1 WITH (NOLOCK) ON CL1.ParLevel1_Id = PL1.Id
				INNER JOIN CollectionLevel2 C2 WITH (NOLOCK) ON C2.ConsolidationLevel2_Id = CL2.Id
				INNER JOIN ParLevel2 PL2 WITH (NOLOCK) ON C2.ParLevel2_Id = PL2.Id
				INNER JOIN ParCompany PC WITH (NOLOCK) ON C2.UnitId = PC.Id
				INNER JOIN Result_Level3 R3 WITH (NOLOCK) ON R3.CollectionLevel2_Id = C2.Id
				INNER JOIN ParLevel3 L3 WITH (NOLOCK) ON L3.Id = R3.ParLevel3_Id
				LEFT JOIN CollectionLevel2XCluster C2XC WITH (NOLOCK) ON C2XC.CollectionLevel2_Id = C2.Id
				--LEFT JOIN CollectionLevel2XParDepartment CL2XPD WITH (NOLOCK) ON CL2XPD.CollectionLevel2_Id = C2.Id
				--LEFT JOIN ParDepartment PD WITH (NOLOCK) ON PD.Id = CL2XPD.ParDepartment_Id
                LEFT JOIN ParDepartment PD WITH (NOLOCK) on PD.Id = PL2.ParDepartment_Id
				OUTER APPLY (SELECT TOP 1
						*
					FROM ParLevel1XModule L1XM WITH (NOLOCK)
					WHERE L1XM.ParLevel1_Id = C2.ParLevel1_Id
					AND L1XM.IsActive = 1
					AND L1XM.EffectiveDateStart <= C2.CollectionDate
					ORDER BY L1XM.EffectiveDateStart DESC) AS L1XM
				OUTER APPLY (SELECT TOP 1
						*
					FROM ParLevel1XCluster L1XC WITH (NOLOCK)
					WHERE L1XC.ParLevel1_Id = C2.ParLevel1_Id
					AND L1XC.ParCluster_Id = C2XC.ParCluster_Id
					AND L1XC.EffectiveDate <= C2.CollectionDate
					AND L1XC.IsActive = 1
					ORDER BY L1XC.EffectiveDate DESC) AS L1XC
				INNER JOIN ParCluster PCL WITH (NOLOCK) ON PCL.Id = C2XC.ParCluster_Id AND PCL.IsActive = 1
				LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PCXS.ParCompany_Id = C2.UnitId AND PCXS.Active = 1 --ParCriticalLevel
				LEFT JOIN CorrectiveAction CA WITH (NOLOCK) ON CA.CollectionLevel02Id = C2.Id
				WHERE 1 = 1
				AND C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
				AND C2.ParLevel1_Id = @INDICADOR --Indicador
                {wModulo}
                {wParClusterGroup}
                {wParCluster}	
                {wParStructure}
                {wParCompany}
                {wTurno}
                {wParCriticalLevel}
                {wParGroupParLevel1}   
                {wParDepartment}
                {wParLevel2}
                {wParLevel3}
                {wAcaoStatus}
                {wPeso}
                {wSurpervisor} 
				GROUP BY L3.Id
						,PL1.IsRuleConformity
						,L3.Name
						,PC.Id
						,PC.Name
						,C2.CollectionDate
						,PL1.hashKey
						,PL1.ParConsolidationType_Id
						,C2.ParLevel1_Id
						,C2.UnitId) S1
			GROUP BY S1.level3_Id
					,S1.IsRuleConformity
					,S1.Level3Name
					,S1.TituloGrafico
					,S1.Unidade_Id
					,S1.Unidade_Name) S2) S3
	GROUP BY S3.Level3Name
			,S3.level3_Id) S4
WHERE 1 = 1
AND S4.NC > 0
GROUP BY S4.Name
		,S4.Id
		,S4.NC
		,S4.AV
ORDER BY NCPercent DESC";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retornoTarefas = factory.SearchQuery<GraficoNC>(query).ToList();
            }

            return retornoTarefas;

        }

        private List<GraficoNC> GetTarefasMonitramentos(DataCarrierFormularioNew form, int parLevel1_Id)
        {
            var retornoTarefas = new List<GraficoNC>();

            var wModulo = "";
            var wParClusterGroup = "";
            var wParCluster = "";
            var wParStructure = "";
            var wParCompany = "";
            var wTurno = "";
            var wParCriticalLevel = "";
            var wParGroupParLevel1 = "";
            var wParDepartment = "";
            var wParLevel2 = "";
            var wParLevel3 = "";
            var wAcaoStatus = "";
            var wNCComPeso = "";
            var wPeso = "";
            var wSurpervisor = "";

            //Módulo
            if (form.ParModule_Ids != null && form.ParModule_Ids.Length > 0)
                wModulo = $" AND L1XM.ParModule_Id IN ({form.ParModule_Ids[0]}) --Modulo";

            //Grupo de Processo
            if (form.ParClusterGroup_Ids != null && form.ParClusterGroup_Ids.Length > 0)
                wParClusterGroup = $" AND PCL.ParClusterGroup_Id IN ({string.Join(",", form.ParClusterGroup_Ids)}) --Grupo de Processo";

            //Processo
            if (form.ParCluster_Ids != null && form.ParCluster_Ids.Length > 0)
                wParCluster = $" AND C2XC.ParCluster_Id IN ({string.Join(",", form.ParCluster_Ids)}) --Processo";

            //Regional
            if (form.ParStructure_Ids != null && form.ParStructure_Ids.Length > 0)
                wParStructure = $" AND PCXS.ParStructure_Id IN ({ string.Join(",", form.ParStructure_Ids)}) --Regional)";

            //Unidade
            if (form.ParCompany_Ids != null && form.ParCompany_Ids.Length > 0)
                wParCompany = $" AND PC.Id in ({string.Join(",", form.ParCompany_Ids)}) --Unidade";

            //Turno
            if (form.Shift_Ids != null && form.Shift_Ids.Length > 0)
                wTurno = $" AND C2.Shift in ({string.Join(",", form.Shift_Ids)}) --Turno";

            //Nível Criticidade
            if (form.ParCriticalLevel_Ids != null && form.ParCriticalLevel_Ids.Length > 0)
                wParCriticalLevel = $" AND L1XC.ParCriticalLevel_Id IN ({string.Join(",", form.ParCriticalLevel_Ids)}) --Nível Criticidade";

            //Função
            if (form.ParGroupParLevel1_Ids != null && form.ParGroupParLevel1_Ids.Length > 0)
                wParGroupParLevel1 = $" AND PL1.ParGroupLevel1_Id IN ({string.Join(",", form.ParGroupParLevel1_Ids)}) --Função";

            //Departamento
            if (form.ParDepartment_Ids != null && form.ParDepartment_Ids.Length > 0)
                wParDepartment = $" AND PL2.ParDepartment_Id IN ({string.Join(",", form.ParDepartment_Ids)}) --Departamento";

            //Indicador
            //if (form.ParLevel1_Ids != null && form.ParLevel1_Ids.Length > 0)
            //    wParLevel1 = $" {string.Join(",", form.ParLevel1_Ids)})";

            //Monitoramento
            if (form.ParLevel2_Ids != null && form.ParLevel2_Ids.Length > 0)
                wParLevel2 = $" AND C2.ParLevel2_Id IN ({string.Join(",", form.ParLevel2_Ids)}) --Monitoramento";


            //Tarefa
            if (form.ParLevel3_Ids != null && form.ParLevel3_Ids.Length > 0)
                wParLevel3 = $" AND R3.ParLevel3_Id IN ({string.Join(",", form.ParLevel3_Ids)}) --Tarefa";


            //Plano de Ação Concluido
            if (form.AcaoStatus != null && form.AcaoStatus.Length > 0)
            {
                if (form.AcaoStatus[0] == 1) //Concluido
                    wAcaoStatus = " AND (SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesConcluidas FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN (3, 4, 7, 8) AND Acao.Status NOT IN (1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";

                if (form.AcaoStatus[0] == 2)//Não Concluido
                    wAcaoStatus = " AND(SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesEmAndamento FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN(1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";
            }

            //Periodo
            //if (form.Periodo != null && form.Periodo.Length > 0)
            //{
            //    if (form.Periodo[0] == 1)//Diario
            //    {
            //        wPeriodo = "convert(NVARCHAR, Data, 111)";
            //    }

            //    if (form.Periodo[0] == 2)//Semanal
            //    {
            //        wPeriodo = "CONCAT(CONCAT(DATEPART(YEAR, S1.Data), '-'), DatePart(WEEK, S1.Data))";
            //        orderBy = "CONVERT(INT,replace(S3.Data, '-', ''))";
            //    }
            //    if (form.Periodo[0] == 3)//Mensal
            //    {
            //        wPeriodo = "CONCAT(CONCAT(DATEPART(YEAR, S1.Data), '-'), DatePart(MONTH, S1.Data))";
            //        orderBy = "CONVERT(INT,replace(S3.Data, '-', ''))";
            //    }
            //}

            //Exibe NC Com peso
            if (form.NcComPeso != null && form.NcComPeso.Length > 0)
            {
                if (form.NcComPeso[0] == 1) //Com Peso
                    wNCComPeso = "'NcComPeso'";
                if (form.NcComPeso[0] == 2) //Sem Peso
                    wNCComPeso = "'NcSemPeso'";
            }

            //Peso
            if (form.Peso != null && form.Peso.Length > 0 && form.Peso[0] != null)
                wPeso = $" AND R3.Weight IN ({ string.Join(",", form.Peso) })--Peso";

            //Surpervisor
            if (form.UserSgqSurpervisor_Ids != null && form.UserSgqSurpervisor_Ids.Length > 0)
                wSurpervisor = $" AND C2.AuditorId IN ({string.Join(",", form.UserSgqSurpervisor_Ids)}) --Surpervisor";

            var query = $@"
DECLARE @INDICADOR INT = {parLevel1_Id}
DECLARE @DATAINICIAL datetime = '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00'
DECLARE @DATAFINAL datetime = '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59'
DECLARE @RESS INT

SELECT
	@RESS = COUNT(1)
FROM (SELECT
		COUNT(1) AS NA
	FROM CollectionLevel2 C2 WITH (NOLOCK)
	LEFT JOIN Result_Level3 C3 WITH (NOLOCK) ON C3.CollectionLevel2_Id = C2.Id
	WHERE CONVERT(DATE, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL
	AND C2.ParLevel1_Id = (SELECT TOP 1 Id FROM ParLevel1 WITH (NOLOCK) WHERE hashKey = 1)
	AND C2.UnitId IN (1)
	AND IsNotEvaluate = 1
	GROUP BY C2.Id) NA
WHERE NA = 2

SELECT
	level3_Id AS Id
   ,Level3Name AS Name
   ,PorcentagemNc AS NCPercent
   ,level2_Id AS ParLevel2_Id
   ,Level2Name AS ParLevel2_Name
   ,(CASE
		WHEN IsRuleConformity = 1 THEN (100 - Meta)
		WHEN IsRuleConformity IS NULL THEN 0
		ELSE Meta
	END) AS Meta
   ,IIF({wNCComPeso} = 'NcComPeso', NC, NcSemPeso) AS NC --Exibe NC Com peso 
   ,IIF({wNCComPeso} = 'NcComPeso', AV, AvSemPeso) AS AV --Exibe Av Com peso 
FROM (SELECT
		*
	   ,CASE WHEN AV IS NULL OR AV = 0 THEN 0 ELSE NC / AV * 100 END AS PorcentagemNc
	   ,CASE WHEN AV IS NULL OR AV = 0 THEN 0 ELSE NCSemPeso / AV * 100 END AS PorcentagemNcSemPeso
	   ,CASE WHEN CASE WHEN AV IS NULL OR AV = 0 THEN 0 ELSE NC / AV * 100 END >= (CASE WHEN IsRuleConformity = 1 THEN (100 - Meta) ELSE Meta END) THEN 1 ELSE 0 END RELATORIO_DIARIO
	FROM (SELECT
			level3_Id
		   ,IsRuleConformity
		   ,Level3Name
		   ,level2_Id
		   ,Level2Name
		   ,TituloGrafico
		   ,Unidade_Id
		   ,Unidade_Name
		   ,SUM(Av) AS Av
		   ,SUM(AvSemPeso) AS AvSemPeso
		   ,SUM(NC) AS NC
		   ,SUM(NCSemPeso) AS NCSemPeso
		   ,MAX(Meta) AS Meta
		FROM (SELECT
				PL3.Id AS level3_Id
			   ,PL3.Name AS Level3Name
			   ,PL2.Id AS level2_Id
			   ,PL2.Name AS Level2Name
			   ,PD.Id AS ParDeparment_Id
			   ,PD.Name AS ParDeparment_Name
			   ,'Tendência do Indicador ' + PL3.Name AS TituloGrafico
			   ,PL1.IsRuleConformity
			   ,PC.Id AS Unidade_Id
			   ,PC.Name AS Unidade_Name
			   ,CASE
					WHEN PL1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) - @RESS FROM VolumePcc1b WITH (NOLOCK) WHERE ParCompany_Id = PC.Id AND Data = cast(C2.CollectionDate as date))
					WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.WeiEvaluation)
					WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiEvaluation)
					WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT ""KEY"")
                    WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT ""KEY"")
					ELSE 0
                END AS Av
			   ,CASE
                    WHEN PL1.hashKey = 1 THEN(SELECT TOP 1 SUM(Quartos) - @RESS FROM VolumePcc1b WITH(NOLOCK) WHERE ParCompany_Id = PC.Id AND Data = cast(C2.CollectionDate as date))
                    WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.Evaluation)
                    WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiEvaluation)
                    WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT ""KEY"")
					WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT ""KEY"")
					ELSE 0
                END AS AvSemPeso
			   ,CASE
                    WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.WeiDefects)
                    WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiDefects)
                    WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT IIF(R3.WeiDefects> 0,""KEY"",NULL))
					WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT IIF(R3.WeiDefects> 0,""KEY"",NULL))
					ELSE 0
                END AS NC
			   ,CASE
                    WHEN PL1.ParConsolidationType_Id = 1 THEN SUM(R3.Defects)
                    WHEN PL1.ParConsolidationType_Id = 2 THEN SUM(R3.WeiDefects)
                    WHEN PL1.ParConsolidationType_Id = 3 THEN COUNT(DISTINCT IIF(R3.WeiDefects> 0,""KEY"",NULL))
					WHEN PL1.ParConsolidationType_Id = 4 THEN COUNT(DISTINCT IIF(R3.WeiDefects> 0,""KEY"",NULL))
					ELSE 0
                END AS NCSemPeso
			   ,CASE
                    WHEN(SELECT
                                COUNT(1)

                            FROM ParGoal G WITH(NOLOCK)
                            WHERE G.ParLevel1_Id = C2.ParLevel1_Id
                            AND(G.ParCompany_Id = C2.UnitId
                            OR G.ParCompany_Id IS NULL)
                            AND G.IsActive = 1
                            AND G.EffectiveDate <= @DATAFINAL)
                        > 0 THEN(SELECT TOP 1
                                ISNULL(G.PercentValue, 0)
                            FROM ParGoal G WITH(NOLOCK)
                            WHERE G.ParLevel1_Id = C2.ParLevel1_Id
                            AND(G.ParCompany_Id = C2.UnitId
                            OR G.ParCompany_Id IS NULL)
                            AND G.IsActive = 1
                            AND G.EffectiveDate <= @DATAFINAL
                            ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
                    ELSE(SELECT TOP 1
                                ISNULL(G.PercentValue, 0)
                            FROM ParGoal G WITH(NOLOCK)
                            WHERE G.ParLevel1_Id = C2.ParLevel1_Id
                            AND(G.ParCompany_Id = C2.UnitId
                            OR G.ParCompany_Id IS NULL)
                            ORDER BY G.ParCompany_Id DESC, EffectiveDate ASC)
                END
                AS Meta
            FROM ConsolidationLevel1 CL1
            INNER JOIN ConsolidationLevel2 CL2 WITH(NOLOCK) ON CL2.ConsolidationLevel1_Id = CL1.Id
            INNER JOIN ParLevel1 PL1 WITH(NOLOCK) ON CL1.ParLevel1_Id = PL1.Id
            INNER JOIN CollectionLevel2 C2 WITH(NOLOCK) ON C2.ConsolidationLevel2_Id = CL2.Id
            INNER JOIN ParLevel2 PL2 WITH(NOLOCK) ON C2.ParLevel2_Id = PL2.Id
            INNER JOIN ParCompany PC WITH(NOLOCK) ON C2.UnitId = PC.Id
            INNER JOIN Result_Level3 R3 WITH(NOLOCK) ON R3.CollectionLevel2_Id = C2.Id
            INNER JOIN ParLevel3 PL3 WITH(NOLOCK) ON R3.ParLevel3_Id = PL3.Id
            LEFT JOIN CollectionLevel2XCluster C2XC WITH(NOLOCK) ON C2XC.CollectionLevel2_Id = C2.Id
            --LEFT JOIN CollectionLevel2XParDepartment CL2XPD WITH(NOLOCK) ON CL2XPD.CollectionLevel2_Id = C2.Id
            --LEFT JOIN ParDepartment PD WITH(NOLOCK) ON PD.Id = CL2XPD.ParDepartment_Id
            LEFT JOIN ParDepartment PD WITH (NOLOCK) on PD.Id = PL2.ParDepartment_Id
            OUTER APPLY(SELECT TOP 1
                   *
               FROM ParLevel1XModule L1XM WITH(NOLOCK)
                WHERE L1XM.ParLevel1_Id = C2.ParLevel1_Id
                AND L1XM.IsActive = 1
                AND L1XM.EffectiveDateStart <= C2.CollectionDate
                ORDER BY L1XM.EffectiveDateStart DESC) AS L1XM
            OUTER APPLY(SELECT TOP 1
                   *
               FROM ParLevel1XCluster L1XC WITH(NOLOCK)
                WHERE L1XC.ParLevel1_Id = C2.ParLevel1_Id
                AND L1XC.ParCluster_Id = C2XC.ParCluster_Id
                AND L1XC.EffectiveDate <= C2.CollectionDate
                AND L1XC.IsActive = 1
                ORDER BY L1XC.EffectiveDate DESC) AS L1XC
            INNER JOIN ParCluster PCL WITH(NOLOCK) ON PCL.Id = C2XC.ParCluster_Id AND PCL.IsActive = 1
            LEFT JOIN ParCompanyXStructure PCXS WITH(NOLOCK) ON PCXS.ParCompany_Id = C2.UnitId AND PCXS.Active = 1--ParCriticalLevel
          LEFT JOIN CorrectiveAction CA WITH(NOLOCK) ON CA.CollectionLevel02Id = C2.Id
            WHERE 1 = 1
            AND C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL
            AND C2.ParLevel1_Id = @INDICADOR --Indicador
            {wModulo}
            {wParClusterGroup}
            {wParCluster}	
            {wParStructure}
            {wParCompany}
            {wTurno}
            {wParCriticalLevel}
            {wParGroupParLevel1}   
            {wParDepartment}
            {wParLevel2}
            {wParLevel3}
            {wAcaoStatus}
            {wPeso}
            {wSurpervisor} 
            GROUP BY PL3.Id
					,PL1.IsRuleConformity
					,PL3.Name
					,PC.Id
					,PC.Name
					,PL2.Id
					,PL2.Name
					,C2.CollectionDate
					,PL1.hashKey
					,PL1.ParConsolidationType_Id
					,C2.ParLevel1_Id
					,C2.UnitId
					,PD.Id
					,PD.Name) S1
        GROUP BY S1.level3_Id
				,S1.IsRuleConformity
				,S1.Level3Name
				,S1.TituloGrafico
				,S1.Unidade_Id
				,S1.Unidade_Name
				,S1.level2_Id
				,S1.Level2Name) S2) S3
WHERE 1 = 1
AND S3.NC > 0
ORDER BY S3.PorcentagemNc DESC";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retornoTarefas = factory.SearchQuery<GraficoNC>(query).ToList();
            }

            return retornoTarefas;

        }

        private List<PAAcao> GetAcaoTarefaMonitoramento(DataCarrierFormularioNew form, int ParLevel1_Id)
        {
            var acoes = new List<PAAcao>();

            var wModulo = "";
            var wParClusterGroup = "";
            var wParCluster = "";
            var wParStructure = "";
            var wParCompany = "";
            var wTurno = "";
            var wParCriticalLevel = "";
            var wParGroupParLevel1 = "";
            var wParDepartment = "";
            var wParLevel2 = "";
            var wParLevel3 = "";
            var wAcaoStatus = "";
            var wPeso = "";
            var wSurpervisor = "";

            //Módulo
            if (form.ParModule_Ids != null && form.ParModule_Ids.Length > 0)
                wModulo = $"";

            //Grupo de Processo
            if (form.ParClusterGroup_Ids != null && form.ParClusterGroup_Ids.Length > 0)
                wParClusterGroup = $"";

            //Processo
            if (form.ParCluster_Ids != null && form.ParCluster_Ids.Length > 0)
                wParCluster = $"";

            //Regional
            if (form.ParStructure_Ids != null && form.ParStructure_Ids.Length > 0)
                wParStructure = $" AND PCXS.ParStructure_Id IN ({ string.Join(",", form.ParStructure_Ids)}) --Regional)";

            //Unidade
            if (form.ParCompany_Ids != null && form.ParCompany_Ids.Length > 0)
                wParCompany = $" AND PC.Id in ({string.Join(",", form.ParCompany_Ids)}) --Unidade";

            //Turno
            if (form.Shift_Ids != null && form.Shift_Ids.Length > 0)
                wTurno = $"";

            //Nível Criticidade
            if (form.ParCriticalLevel_Ids != null && form.ParCriticalLevel_Ids.Length > 0)
                wParCriticalLevel = $"";

            //Função
            if (form.ParGroupParLevel1_Ids != null && form.ParGroupParLevel1_Ids.Length > 0)
                wParGroupParLevel1 = $" AND PL1.ParGroupLevel1_Id IN ({string.Join(",", form.ParGroupParLevel1_Ids)}) --Função";

            //Departamento
            if (form.ParDepartment_Ids != null && form.ParDepartment_Ids.Length > 0)
                wParDepartment = $" AND Acao.Departamento_Id IN ({string.Join(",", form.ParDepartment_Ids)}) --Departamento";

            //Indicador
            //if (form.ParLevel1_Ids != null && form.ParLevel1_Ids.Length > 0)
            //    wParLevel1 = $" {string.Join(",", form.ParLevel1_Ids)})";

            //Monitoramento
            if (form.ParLevel2_Ids != null && form.ParLevel2_Ids.Length > 0)
                wParLevel2 = $" AND PL2.Id IN ({string.Join(",", form.ParLevel2_Ids)}) --Monitoramento";

            //Tarefa
            if (form.ParLevel3_Ids != null && form.ParLevel3_Ids.Length > 0)
                wParLevel3 = $" AND PL3.Id IN ({string.Join(",", form.ParLevel3_Ids)}) --Tarefa";

            ////Plano de Ação Concluido
            //if (form.AcaoStatus != null && form.AcaoStatus.Length > 0)
            //{
            //    if (form.AcaoStatus[0] == 1) //Concluido
            //        wAcaoStatus = " AND (SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesConcluidas FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN (3, 4, 7, 8) AND Acao.Status NOT IN (1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";

            //    if (form.AcaoStatus[0] == 2)//Não Concluido
            //        wAcaoStatus = " AND(SELECT IIF(COUNT(*) > 0, 1, 0) as HaveAcoesEmAndamento FROM Pa_Acao Acao WHERE 1 = 1 AND Acao.Level1Id = 1 AND Acao.Status IN(1, 5, 6, 9, 10)) = 1 --Plano de Ação Concluido";
            //}

            //Peso
            //if (form.Peso != null && form.Peso.Length > 0 && form.Peso[0] != null)
            //    wPeso = $" AND R3.Weight IN ({ string.Join(",", form.Peso) })--Peso";

            //Desdobramento
            //if (form.Desdobramento != null && form.Desdobramento.Length > 0)
            //    wDesdobramento = "";

            //Surpervisor
            //if (form.UserSgqSurpervisor_Ids != null && form.UserSgqSurpervisor_Ids.Length > 0)
            //    wSurpervisor = $" AND C2.AuditorId IN ({string.Join(",", form.UserSgqSurpervisor_Ids)}) --Surpervisor";

            var sql = $@"

SELECT
   PL1.Id AS ParLevel1_Id
   ,PL1.Name AS ParLevel1
   ,PL2.Id as ParLevel2_Id
   ,PL2.Name AS ParLevel2
   ,PL3.Id AS ParLevel3
   ,PL3.Name AS ParLevel3
   ,CG.Id as CausaGenerica_Id
   ,CG.CausaGenerica
   ,CMG.Id as AcaoGenerica_Id
   ,CMG.ContramedidaGenerica as AcaoGenerica
   ,[Status].Id as Status_Id
   ,[Status].Name as Status
FROM Pa_Acao Acao
INNER JOIN Pa_CausaGenerica CG ON Acao.CausaGenerica_Id = CG.Id
INNER JOIN Pa_ContramedidaGenerica CMG ON Acao.ContramedidaGenerica_Id = CMG.Id
INNER JOIN Pa_Status [Status] WITH (NOLOCK) ON [Status].Id = Acao.Status 
INNER JOIN ParLevel1 PL1 WITH (NOLOCK) ON Acao.Level1Id = PL1.Id
INNER JOIN ParLevel2 PL2 WITH (NOLOCK) ON Acao.Level2Id = PL2.Id
INNER JOIN ParLevel3 PL3 WITH (NOLOCK) ON Acao.Level3Id = PL3.Id
INNER JOIN ParCompany PC WITH (NOLOCK) ON Acao.Unidade_Id = PC.Id
LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK) ON PCXS.ParCompany_Id = Acao.Unidade_Id AND PCXS.Active = 1 
WHERE 1 = 1
AND Acao.QuandoInicio BETWEEN '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00' AND '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59' --Data
AND PL1.Id = 2 --Indicador
{wModulo}
{wParClusterGroup}
{wParCluster}	
{wParStructure}
{wParCompany}
{wTurno}
{wParCriticalLevel}
{wParGroupParLevel1}   
{wParDepartment}
{wParLevel2}
{wParLevel3}            			
{wAcaoStatus}
{wPeso}
{wSurpervisor}  

            ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                acoes = factory.SearchQuery<PAAcao>(sql).ToList();
            }

            return acoes;
        }

        private static string getQuery(DataCarrierFormularioNew form, int? nivel)
        {

            var Wunidade = "";
            var query = "";

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids[0] != 0)
            {
                Wunidade = " AND C1.UnitId IN (" + string.Join(",", form.ParCompany_Ids) + ")";
            }

            query = $@"  

                DECLARE @DATEINI DATETIME = '{form.startDate.ToString("yyyy-MM-dd")} 00:00:00'
                DECLARE @DATEFIM DATETIME = '{form.endDate.ToString("yyyy-MM-dd")} 23:59:59'
                DECLARE @dataFim_ date = @DATEFIM
                DECLARE @dataInicio_ date = @DATEINI
                
                SET @dataInicio_ = @DATEINI
                
                CREATE TABLE #DATA (data date)
                
                WHILE @dataInicio_ <= @dataFim_  
                BEGIN
                INSERT INTO #DATA
                	SELECT
                		@dataInicio_
                SET @dataInicio_ = DATEADD(DAY, 1, @dataInicio_)
                              
                             END
                             DECLARE @DATAFINAL DATE = @dataFim_
                             DECLARE @DATAINICIAL DATE = DateAdd(mm, DateDiff(mm, 0, @DATAFINAL) - 1, 0)
                SET @DATAINICIAL = @DATEINI
                            
                CREATE INDEX IDX_Data ON #Data (Data);
                
                SELECT
                	V.ParCompany_Id
                   ,V.data
                   ,SUM(V.Quartos) AS VOLUMEPCC INTO #VOLUMES
                FROM VolumePcc1b V WITH (NOLOCK)
                WHERE 1 = 1
                GROUP BY V.ParCompany_Id
                		,V.data
                
                SELECT
                	Unidade
                   ,Indicador
                   ,[Shift]
                   ,[Period]
                   ,data
                   ,COUNT(1) AM
                   ,SUM(DEF_AM) DEF_AM INTO #AMOSTRA4
                FROM (SELECT
                		CAST(C2.CollectionDate AS DATE) AS DATA
                	   ,C.Id AS UNIDADE
                	   ,C2.ParLevel1_Id AS INDICADOR
                	   ,C2.[Shift]
                	   ,C2.[Period]
                	   ,C2.EvaluationNumber AS AV
                	   ,C2.Sample AS AM
                	   ,CASE
                			WHEN SUM(C2.WeiDefects) = 0 THEN 0
                			ELSE 1
                		END DEF_AM
                	FROM CollectionLevel2 C2 (NOLOCK)
                	INNER JOIN ParLevel1 L1 (NOLOCK)
                		ON L1.Id = C2.ParLevel1_Id
                	INNER JOIN ParCompany C (NOLOCK)
                		ON C.Id = C2.UnitId
                	WHERE CAST(C2.CollectionDate AS DATE) BETWEEN @DATAINICIAL AND @DATAFINAL
                	AND C2.NotEvaluatedIs = 0
                	AND C2.Duplicated = 0
                	AND L1.ParConsolidationType_Id = 4
                	GROUP BY C.Id
                			,ParLevel1_Id
                			,C2.[Shift]
                			,C2.[Period]
                			,EvaluationNumber
                			,Sample
                			,CAST(CollectionDate AS DATE)) TAB
                GROUP BY Unidade
                		,Indicador
                		,data
                		,[Shift]
                		,[Period]
                
                SELECT
                	CL2.CollectionDate
                   ,CL2.UnitId
                   ,COUNT(DISTINCT CL2.Id) AS NA INTO #NA
                FROM CollectionLevel2 CL2 WITH (NOLOCK)
                LEFT JOIN Result_Level3 CL3 WITH (NOLOCK)
                	ON CL3.CollectionLevel2_Id = CL2.Id
                WHERE CONVERT(DATE, CL2.CollectionDate) BETWEEN CONVERT(DATE, @DATEINI) AND CONVERT(DATE, @DATEFIM)
                AND CL2.ParLevel1_Id IN (SELECT
                		Id
                	FROM ParLevel1 WITH (NOLOCK)
                	WHERE hashKey = 1)
                AND CL3.IsNotEvaluate = 1
                GROUP BY CL2.CollectionDate
                		,CL2.UnitId
                HAVING COUNT(DISTINCT CL2.Id) > 1
                
                SELECT
                	CL1.Id
                   ,CL1.ConsolidationDate
                   ,CL1.UnitId
                   ,CL1.ParLevel1_Id
                   ,CL1.[Shift]
                   ,CL1.[Period]
                   ,CL1.DefectsResult
                   ,CL1.WeiDefects
                   ,CL1.EvaluatedResult
                   ,CL1.WeiEvaluation
                   ,CL1.EvaluateTotal
                   ,CL1.TotalLevel3WithDefects
                   ,CL1.DefectsTotal INTO #ConsolidationLevel
                FROM ConsolidationLevel1 CL1 WITH (NOLOCK)
                WHERE 1 = 1
                AND CL1.ConsolidationDate BETWEEN @DATEINI AND @DATEFIM
                
                AND CL1.ParLevel1_Id != 43
                AND CL1.ParLevel1_Id != 42
                            
                CREATE INDEX IDX_HashConsolidationLevel ON #ConsolidationLevel (ConsolidationDate,UnitId,ParLevel1_Id);
                CREATE INDEX IDX_HashConsolidationLevel_level1 ON #ConsolidationLevel (ConsolidationDate,ParLevel1_Id);
                CREATE INDEX IDX_HashConsolidationLevel_Unitid ON #ConsolidationLevel (ConsolidationDate,UnitId);
                CREATE INDEX IDX_HashConsolidationLevel_id ON #ConsolidationLevel (id);
                
                
                -- CUBO
                SELECT
                	CL.Id AS ParCluster_ID
                   ,CL.Name AS ParCluster_Name
                   ,CS.ParStructure_id AS ParStructure_id
                   ,S.Name AS ParStructure_Name
                   ,C1.UnitId AS Unidade
                   ,PC.Name AS UnidadeName
                   ,C1.ConsolidationDate AS ConsolidationDate
                   ,L1.ParConsolidationType_Id AS ParConsolidationType_Id
                   ,C1.ParLevel1_Id AS Indicador
                   ,L1.Name AS IndicadorName
                   ,L1C.ParCriticalLevel_Id AS ParCriticalLevel_Id
                   ,CRL.Name AS ParCriticalLevel_Name
                   ,L1.IsRuleConformity
                   ,CASE
                		WHEN L1.hashKey = 1 THEN ISNULL ((SELECT TOP 1
                					SUM(VOLUMEPCC)
                				FROM #VOLUMES V WITH (NOLOCK)
                				WHERE 1 = 1
                				AND V.data = C1.ConsolidationDate
                				AND V.ParCompany_Id = C1.UnitId)
                			, 0)
                			-
                			ISNULL ((SELECT
                					SUM(NA) AS NA
                				FROM #NA NA
                				WHERE NA.UnitId = C1.UnitId
                				AND NA.CollectionDate = C1.ConsolidationDate)
                			, 0)
                		WHEN L1.ParConsolidationType_Id = 1 THEN SUM(C1.WeiEvaluation)
                		WHEN L1.ParConsolidationType_Id = 2 THEN SUM(C1.WeiEvaluation)
                		WHEN L1.ParConsolidationType_Id = 3 THEN SUM(C1.EvaluatedResult)
                		WHEN L1.ParConsolidationType_Id = 4 THEN ISNULL ((SELECT
                					SUM(AM) AM
                				FROM #AMOSTRA4 A4
                				WHERE 1 = 1
                				AND C1.UnitId = A4.Unidade
                				AND C1.ParLevel1_Id = A4.Indicador
                				AND C1.[Shift] = A4.[Shift]
                				AND C1.[Period] = A4.[Period]
                				AND C1.ConsolidationDate = A4.data)
                			, 0)
                		WHEN L1.ParConsolidationType_Id = 5 THEN SUM(C1.WeiEvaluation)
                		WHEN L1.ParConsolidationType_Id = 6 THEN SUM(C1.WeiEvaluation)
                		ELSE SUM(0)
                	END AS [AVComPeso]
                   ,CASE
                		WHEN L1.ParConsolidationType_Id = 1 THEN SUM(C1.WeiDefects)
                		WHEN L1.ParConsolidationType_Id = 2 THEN SUM(C1.WeiDefects)
                		WHEN L1.ParConsolidationType_Id = 3 THEN SUM(C1.DefectsResult)
                		WHEN L1.ParConsolidationType_Id = 4 THEN ISNULL ((SELECT
                					SUM(DEF_AM) DEF_AM
                				FROM #AMOSTRA4 A4
                				WHERE 1 = 1
                				AND C1.UnitId = A4.Unidade
                				AND C1.ParLevel1_Id = A4.Indicador
                				AND C1.[Shift] = A4.[Shift]
                				AND C1.[Period] = A4.[Period]
                				AND C1.ConsolidationDate = A4.data)
                			, 0)
                		WHEN L1.ParConsolidationType_Id = 5 THEN SUM(C1.WeiDefects)
                		WHEN L1.ParConsolidationType_Id = 6 THEN SUM(C1.TotalLevel3WithDefects)
                		ELSE SUM(0)
                	END AS [NCComPeso]
                   ,CASE
                		WHEN L1.hashKey = 1 THEN ISNULL ((SELECT TOP 1
                					SUM(VOLUMEPCC)
                				FROM #VOLUMES V WITH (NOLOCK)
                				WHERE 1 = 1
                				AND V.data = C1.ConsolidationDate
                				AND V.ParCompany_Id = C1.UnitId)
                			, 0)
                			-
                			ISNULL ((SELECT
                					SUM(NA) AS NA
                				FROM #NA NA
                				WHERE NA.UnitId = C1.UnitId
                				AND NA.CollectionDate = C1.ConsolidationDate)
                			, 0)
                		WHEN L1.ParConsolidationType_Id = 1 THEN SUM(C1.EvaluateTotal)
                		WHEN L1.ParConsolidationType_Id = 2 THEN SUM(C1.WeiEvaluation)
                		WHEN L1.ParConsolidationType_Id = 3 THEN SUM(C1.EvaluatedResult)
                		WHEN L1.ParConsolidationType_Id = 4 THEN ISNULL ((SELECT
                					SUM(AM) AM
                				FROM #AMOSTRA4 A4
                				WHERE 1 = 1
                				AND C1.UnitId = A4.Unidade
                				AND C1.ParLevel1_Id = A4.Indicador
                				AND C1.[Shift] = A4.[Shift]
                				AND C1.[Period] = A4.[Period]
                				AND C1.ConsolidationDate = A4.data)
                			, 0)
                		WHEN L1.ParConsolidationType_Id = 5 THEN SUM(C1.EvaluateTotal)
                		WHEN L1.ParConsolidationType_Id = 6 THEN SUM(C1.EvaluateTotal)
                		ELSE SUM(0)
                	END AS [AV]
                   ,CASE
                		WHEN L1.ParConsolidationType_Id = 1 THEN SUM(C1.DefectsTotal)
                		WHEN L1.ParConsolidationType_Id = 2 THEN SUM(C1.DefectsTotal)
                		WHEN L1.ParConsolidationType_Id = 3 THEN SUM(C1.DefectsResult)
                		WHEN L1.ParConsolidationType_Id = 4 THEN ISNULL ((SELECT
                					SUM(DEF_AM) DEF_AM
                				FROM #AMOSTRA4 A4
                				WHERE 1 = 1
                				AND C1.UnitId = A4.Unidade
                				AND C1.ParLevel1_Id = A4.Indicador
                				AND C1.[Shift] = A4.[Shift]
                				AND C1.[Period] = A4.[Period]
                				AND C1.ConsolidationDate = A4.data)
                			, 0)
                		WHEN L1.ParConsolidationType_Id = 5 THEN SUM(C1.DefectsTotal)
                		WHEN L1.ParConsolidationType_Id = 6 THEN SUM(C1.TotalLevel3WithDefects)
                		ELSE SUM(0)
                	END AS [NC]
                   ,ISNULL ((SELECT TOP 1
                			PercentValue
                		FROM ParGoal pg
                		WHERE 1 = 1
                		AND pg.IsActive = 1
                		AND pg.ParLevel1_Id = C1.ParLevel1_Id
                		AND (ISNULL (pg.EffectiveDate, pg.EffectiveDate) <= C1.ConsolidationDate)
                		AND (pg.ParCompany_Id = C1.UnitId
                		OR pg.ParCompany_Id IS NULL)
                		ORDER BY EffectiveDate DESC, ParCompany_Id DESC)
                	, (SELECT TOP 1
                			PercentValue
                		FROM ParGoal pg
                		WHERE 1 = 1
                		AND pg.IsActive = 1
                		AND pg.ParLevel1_Id = C1.ParLevel1_Id
                		AND (ISNULL (pg.EffectiveDate, pg.EffectiveDate) <= C1.ConsolidationDate)
                		AND (pg.ParCompany_Id = C1.UnitId
                		OR pg.ParCompany_Id IS NULL)
                		ORDER BY EffectiveDate DESC, ParCompany_Id DESC)
                	) AS Meta INTO #CUBO
                FROM #ConsolidationLevel C1
                INNER JOIN ParLevel1 L1 WITH (NOLOCK)
                	ON C1.ParLevel1_Id = L1.Id
                		AND ISNULL (L1.ShowScorecard, 1) = 1
                		AND L1.IsActive = 1
                
                LEFT JOIN ParCompany PC WITH (NOLOCK)
                	ON PC.Id = C1.UnitId
                
                INNER JOIN ParCompanyCluster CCL WITH (NOLOCK)
                	ON CCL.ParCompany_Id = PC.Id
                		AND CCL.Active = 1
                
                INNER JOIN ParLevel1XCluster L1C WITH (NOLOCK)
                	ON CCL.ParCluster_ID = L1C.ParCluster_ID
                		AND C1.ParLevel1_Id = L1C.ParLevel1_Id
                		AND L1C.Id = (SELECT TOP 1
                				aaa.Id
                			FROM ParLevel1XCluster aaa (NOLOCK)
                			WHERE aaa.ParLevel1_Id = L1.Id
                			AND aaa.ParCluster_ID = CCL.ParCluster_ID
                			AND aaa.EffectiveDate < @DATAFINAL
                			AND IsActive = 1
                			ORDER BY ParLevel1_Id, ParCluster_ID, EffectiveDate, AddDate, AlterDate)
                		AND L1C.IsActive = 1
                
                INNER JOIN ParCompanyXStructure CS WITH (NOLOCK)
                	ON PC.Id = CS.ParCompany_Id
                		AND CS.Active = 1
                
                INNER JOIN ParStructure S WITH (NOLOCK)
                	ON CS.ParStructure_id = S.Id
                		AND S.Active = 1
                
                INNER JOIN ParCluster CL WITH (NOLOCK)
                	ON L1C.ParCluster_ID = CL.Id
                		AND CL.IsActive = 1
                
                INNER JOIN ParStructureGroup SG WITH (NOLOCK)
                	ON S.ParStructureGroup_Id = SG.Id
                		AND SG.Id = 3
                
                LEFT JOIN ParScoreType ST WITH (NOLOCK)
                	ON L1.ParConsolidationType_Id = ST.Id
                		AND ST.IsActive = 1
                
                LEFT JOIN ParCriticalLevel CRL WITH (NOLOCK)
                	ON L1C.ParCriticalLevel_Id = CRL.Id
                		AND CRL.IsActive = 1
                
                LEFT JOIN ParLevel1XModule P1M WITH (NOLOCK)
                	ON P1M.ParLevel1_Id = L1C.ParLevel1_Id
                		AND P1M.IsActive = 1
                		AND P1M.EffectiveDateStart <= @DATAINICIAL
                		AND (P1M.ParCluster_ID IS NULL
                			OR P1M.ParCluster_ID IN (SELECT
                					ParCluster_ID
                				FROM ParCompanyCluster
                				WHERE Active = 1)
                		)
                
                WHERE 1 = 1
                { Wunidade }
                GROUP BY CL.Id
                		,CL.Name
                		,CS.ParStructure_id
                		,S.Name
                		,C1.UnitId
                		,PC.Name
                		,C1.ConsolidationDate
                		,L1.ParConsolidationType_Id
                		,L1.hashKey
                		,C1.ParLevel1_Id
                		,C1.[Shift]
                		,C1.[Period]
                		,L1.Name
                		,L1C.ParCriticalLevel_Id
                		,CRL.Name
                		,L1.IsRuleConformity
                
                UPDATE #CUBO
                SET Meta = IIF(IsRuleConformity = 0, Meta, (100 - Meta))";


            return query;
        }

    }
}