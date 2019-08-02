﻿using ADOFactory;
using Dominio;
using DTO.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Relatorios.RH
{
    [RoutePrefix("api/RelatorioDeResultadosRH")]
    public class RelatorioDeResultadosRHApiController : BaseApiController
    {
        List<RelatorioResultadosPeriodo> retorno;
        List<RetornoGenerico> retorno2;
        List<RetornoGenerico> retorno3;
        List<RetornoGenerico> retorno4;
        //List<RetornoLogJson> logJson;


        public RelatorioDeResultadosRHApiController()
        {
            retorno = new List<RelatorioResultadosPeriodo>();
            retorno2 = new List<RetornoGenerico>();
            retorno3 = new List<RetornoGenerico>();
            retorno4 = new List<RetornoGenerico>();
            //logJson = new List<RetornoLogJson>();
        }


        [HttpPost]
        [Route("listaResultadosPeriodoTabela")]
        public List<RelatorioResultadosPeriodo> listaResultadosPeriodoTabela([FromBody] DTO.DataCarrierFormularioNew form)
        {

            //Nenhum Indicador Sem Unidade
            //Nenhum Indicador Com Unidade
            //Nenhum Monitoramento Sem Unidade
            //Nenhum Monitoramento Com Unidade
            //Nenhuma Tarefa Sem Unidade
            //Nenhuma Tarefa Com Unidade
            //Indicador Monitoramento Tarefa Com Unidade
            //Indicador Monitoramento tarefa Sem Unidade

            if (/*form.level1Id == 0 && */form.ParLevel1_Ids.Length != 1) //Nenhum Indicador Sem Unidade
            {
                GetResultadosIndicador(form);
            }
            else if (/*form.level2Id == 0 && */form.ParLevel2_Ids.Length != 1) //Nenhum Monitoramento Sem Unidade
            {
                GetResultadosMonitoramento(form);
            }
            else
            {
                GetResultadosTarefa(form);
            }

            return retorno;
        }

        [HttpPost]
        [Route("listaResultadosPeriodoTabela2")]
        public List<RelatorioResultadosPeriodo> listaResultadosPeriodoTabela2([FromBody] DTO.DataCarrierFormularioNew form)
        {
            GetResultadosTarefa(form);

            return retorno;
        }

        private void GetResultadosIndicador(DTO.DataCarrierFormularioNew form)
        {
            var nivel = 1;
            var tipoVisao = false;


            #region Filtros

            var titulo = "Histórico do Indicador";

            var Wmodulo = "";
            var Wprocesso = "";
            var Wregional = "";
            var Wnivelcritico = "";
            var Wfuncao = "";

            // Módulo

            if (form.ParClusterGroup_Ids.Length > 0)
            {
                Wmodulo += " AND ParCluster_ID IN (SELECT ID FROM ParCluster WITH (NOLOCK) WHERE ParClusterGroup_Id IN (" + string.Join(",", form.ParCluster_Ids) + ") ";
            }

            // Processo

            if (form.ParCluster_Ids.Length > 0)
            {
                Wprocesso += " AND ParCluster_ID IN (" + string.Join(",", form.ParCluster_Ids) + ") ";
            }

            // Regional

            if (form.ParStructure_Ids.Length > 0)
            {
                Wregional += " AND ParStructure_id  IN (" + string.Join(",", form.ParStructure_Ids) + ") ";
            }
            else if (form.Param["structureId"] != null && form.Param["structureId"].ToString() != "")
            {
                Wregional += " AND ParStructure_id  IN (" + form.Param["structureId"] + ") ";
            }

            // Nivel Crítico

            if (form.ParCriticalLevel_Ids.Length > 0)
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.ParCriticalLevel_Ids) + ") ";
            }
            else if (form.Param["criticalLevelId"] != null && form.Param["criticalLevelId"].ToString() != "")
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + form.Param["criticalLevelId"] + ") ";
            }

            // Função

            if (form.ParLevel1Group_Ids.Length > 0)
            {
                Wfuncao += " AND Indicador IN (SELECT distinct ParLevel1_id  FROM ParGroupParLevel1XParLevel1 WHERE IsActive = 1 AND ParGroupParLevel1_Id IN (" + string.Join(",", form.ParLevel1Group_Ids) + "))"; // " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.criticalLevelIdArr) + ") ";
            }

            #endregion


            var script = "";
            var SQLcentro = "";

            SQLcentro = getQuery(form, nivel);

            #region Status do Indicador: Fora ou Dentro da Meta

            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 1) // Indicadores Fora Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 2) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }
            #endregion

            #region Indicadores com Acao Concluída: Sim ou Não

            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 1)
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 2) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }
            #endregion

            if (tipoVisao == false) // 0: Listagem / 1: Evolutivo 
            { // Considero Dimensões
                #region ScriptLista
                script += @"

            " + SQLcentro + @"

            SELECT 
                ParStructure_Name as Regional
               ,Indicador 
               ,IndicadorName 
               ,Unidade
               ,UnidadeName 
               ,concat(IndicadorName, ' - ', UnidadeName) AS IndicadorUnidade
               --,'" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS PC
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,max(ISNULL(Meta,0)) AS Meta
               
                /*,CASE
                    WHEN IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) > max(ISNULL(Meta,0)) THEN 0
                    ELSE 1
                END AS Status
                */

               ,cast(1 as bit) IsIndicador
               ,IIF(IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)))>max(ISNULL(Meta,0)),0,1) AS Status
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                Indicador 
               ,IndicadorName
               ,Unidade
               ,UnidadeName 
               ,ParStructure_Name
            ORDER BY 7 DESC

            drop table #NA
			drop table #AMOSTRA4
			drop table #DATA
			drop table #VOLUMES
			drop table #CUBO
			drop table #ConsolidationLevel
    
            ";
                #endregion
            }
            else if (tipoVisao == true)
            { // Desconsidero Dimensões
                #region ScriptGrafico
                script += @"

            " + SQLcentro + @"

            SELECT 
               '" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS pc
               ,ConsolidationDate as [date]
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,sum(ISNULL(Meta,0)) AS Meta
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                ConsolidationDate
            ORDER BY 3 
            ";
                #endregion
            }

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno = factory.SearchQuery<RelatorioResultadosPeriodo>(script).ToList();
            }
        }

        private void GetResultadosMonitoramento(DTO.DataCarrierFormularioNew form)
        {

            var nivel = 2;
            var tipoVisao = false;


            #region Filtros

            var titulo = "Histórico do Monitoramento";

            var Wmodulo = "";
            var Wprocesso = "";
            var Wregional = "";
            var Wnivelcritico = "";
            var Wfuncao = "";


            // Módulo

            if (form.Param["clusterGroupId"] != null && form.Param["clusterGroupId"].ToString() != "")
            {
                Wmodulo += " AND ParCluster_ID IN (SELECT ID FROM ParCluster WITH (NOLOCK) WHERE ParClusterGroup_Id = " + form.Param["clusterGroupId"] + ") ";
            }

            // Processo

            if (form.ParCluster_Ids.Length > 0)
            {
                Wprocesso += " AND ParCluster_ID IN (" + string.Join(",", form.ParCluster_Ids) + ") ";
            }

            // Regional

            if (form.ParStructure_Ids.Length > 0)
            {
                Wregional += " AND ParStructure_id  IN (" + string.Join(",", form.ParStructure_Ids) + ") ";
            }
            else if (form.Param["structureId"] != null && form.Param["structureId"].ToString() != "")
            {
                Wregional += " AND ParStructure_id  IN (" + form.Param["structureId"] + ") ";
            }

            // Nivel Crítico

            if (form.ParCriticalLevel_Ids.Length > 0)
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.ParCriticalLevel_Ids) + ") ";
            }
            else if (form.Param["criticalLevelId"] != null && form.Param["criticalLevelId"].ToString() != "")
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + form.Param["criticalLevelId"] + ") ";
            }

            // Função

            if (form.ParLevel1Group_Ids.Length > 0)
            {
                Wfuncao += " AND Indicador IN (SELECT distinct ParLevel1_id  FROM ParGroupParLevel1XParLevel1 WHERE IsActive = 1 AND ParGroupParLevel1_Id IN (" + string.Join(",", form.ParLevel1Group_Ids) + "))"; // " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.criticalLevelIdArr) + ") ";
            }

            #endregion


            var script = "";
            var SQLcentro = "";

            SQLcentro = getQuery(form, nivel);

            #region Status do Indicador: Fora ou Dentro da Meta
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 1) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 2) // Indicadores Fora Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }

            #endregion


            #region Indicadores com Acao Concluída: Sim ou Não

            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 1)
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 2) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }
            #endregion


            if (tipoVisao == false) // 0: Listagem / 1: Evolutivo 
            { // Considero Dimensões
                #region ScriptLista
                script += @"

            " + SQLcentro + @"

            SELECT 
                ParStructure_Name as Regional
               ,Unidade
               ,UnidadeName 
               ,Indicador 
               ,IndicadorName 
               ,Monitoramento 
               ,MonitoramentoName 
               ,concat(MonitoramentoName, ' - ', UnidadeName) AS MonitoramentoUnidade
               --,'" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS PC
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,max(ISNULL(Meta,0)) AS Meta
               ,cast(1 as bit) IsMonitoramento
               ,IIF(IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)))>max(ISNULL(Meta,0)),0,1) AS Status
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                Indicador 
               ,IndicadorName
               ,Monitoramento 
               ,MonitoramentoName 
               ,Unidade
               ,UnidadeName
               ,ParStructure_Name
            ORDER BY 9 DESC

            drop table #NA
			drop table #AMOSTRA4
			drop table #DATA
			drop table #VOLUMES
			drop table #CUBO
			drop table #ConsolidationLevel
            ";
                #endregion
            }
            else if (tipoVisao == true)
            { // Desconsidero Dimensões
                #region ScriptGrafico
                script += @"

            " + SQLcentro + @"

            SELECT 
               '" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS pc
               ,ConsolidationDate as [date]
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,sum(ISNULL(Meta,0)) AS Meta
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                ConsolidationDate
            ORDER BY 3 
            ";
                #endregion
            }


            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno = factory.SearchQuery<RelatorioResultadosPeriodo>(script).ToList();
            }

        }

        private void GetResultadosTarefa(DTO.DataCarrierFormularioNew form)
        {
            var nivel = 3;
            var tipoVisao = false;


            #region Filtros

            var titulo = "Histórico do Monitoramento";

            var Wmodulo = "";
            var Wprocesso = "";
            var Wregional = "";
            var Wnivelcritico = "";
            var Wfuncao = "";


            // Módulo

            if (form.Param["clusterGroupId"] != null && form.Param["clusterGroupId"].ToString() != "")
            {
                Wmodulo += " AND ParCluster_ID IN (SELECT ID FROM ParCluster WITH (NOLOCK) WHERE ParClusterGroup_Id = " + form.Param["clusterGroupId"].ToString() + ") ";
            }

            // Processo

            if (form.ParCluster_Ids.Length > 0)
            {
                //Lindo
                Wprocesso += " AND ParCluster_ID IN (" + string.Join(",", form.ParCluster_Ids) + ") ";
            }

            // Regional

            if (form.ParStructure_Ids.Length > 0)
            {
                Wregional += " AND ParStructure_id  IN (" + string.Join(",", form.ParStructure_Ids) + ") ";
            }
            else if (form.Param["structureId"] != null && form.Param["structureId"].ToString() != "")
            {
                Wregional += " AND ParStructure_id  IN (" + form.Param["structureId"].ToString() + ") ";
            }

            // Nivel Crítico

            if (form.ParCriticalLevel_Ids.Length > 0)
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.ParCriticalLevel_Ids) + ") ";
            }
            else if (form.Param["criticalLevelId"] != null && form.Param["criticalLevelId"].ToString() != "")
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + form.Param["criticalLevelId"].ToString() + ") ";
            }

            // Função

            if (form.ParLevel1Group_Ids.Length > 0)
            {
                Wfuncao += " AND Indicador IN (SELECT distinct ParLevel1_id  FROM ParGroupParLevel1XParLevel1 WHERE IsActive = 1 AND ParGroupParLevel1_Id IN (" + string.Join(",", form.ParLevel1Group_Ids) + "))"; // " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.criticalLevelIdArr) + ") ";
            }

            #endregion


            var script = "";
            var SQLcentro = "";

            SQLcentro = getQuery(form, nivel);

            #region Status do Indicador: Fora ou Dentro da Meta
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 1) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 2) // Indicadores Fora Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }

            #endregion


            #region Indicadores com Acao Concluída: Sim ou Não

            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 1)
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 2) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }
            #endregion


            if (tipoVisao == false) // 0: Listagem / 1: Evolutivo 
            { // Considero Dimensões
                #region ScriptLista
                script += @"

            " + SQLcentro + @"

            SELECT 
                ParStructure_Name as Regional
               ,Unidade
               ,UnidadeName 
               ,Indicador 
               ,IndicadorName 
               ,Monitoramento 
               ,MonitoramentoName 
               ,Tarefa
               ,TarefaName 
               ,concat(TarefaName, ' - ', UnidadeName) AS TarefaUnidade
               --,'" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS PC
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,max(ISNULL(Meta,0)) AS Meta
               ,cast(1 as bit) IsTarefa
               ,IIF(IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)))>max(ISNULL(Meta,0)),0,1) AS Status
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                Indicador 
               ,IndicadorName
               ,Monitoramento 
               ,MonitoramentoName 
               ,Tarefa
               ,TarefaName 
               ,Unidade
               ,UnidadeName 
               ,ParStructure_Name
            ORDER BY 11 DESC
            ";
                #endregion
            }
            else if (tipoVisao == true)
            { // Desconsidero Dimensões
                #region ScriptGrafico
                script += @"

            " + SQLcentro + @"

            SELECT 
               '" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS pc
               ,ConsolidationDate as [date]
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,sum(ISNULL(Meta,0)) AS Meta
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                ConsolidationDate
            ORDER BY 3 
            ";
                #endregion
            }


            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno = factory.SearchQuery<RelatorioResultadosPeriodo>(script).ToList();
            }

        }

        [HttpPost]
        [Route("listaResultadosPeriodoSemUnidadeTabela")]
        public List<RelatorioResultadosPeriodo> listaResultadosPeriodoSemUnidadeTabela([FromBody] DTO.DataCarrierFormularioNew form)
        {

            if (form.ParLevel1_Ids.Length != 1) //Nenhum Indicador Sem Unidade
            {
                GetResultadosIndicadorSemUnidade(form);
            }
            else if (form.ParLevel2_Ids.Length != 1) //Nenhum Monitoramento Sem Unidade
            {
                GetResultadosMonitoramentoSemUnidade(form);
            }
            else
            {
                GetResultadosTarefaSemUnidade(form);
            }

            return retorno;
        }

        private void GetResultadosIndicadorSemUnidade(DTO.DataCarrierFormularioNew form)
        {
            var nivel = 1;
            var tipoVisao = false;


            #region Filtros

            var titulo = "Histórico do Indicador";

            var Wmodulo = "";
            var Wprocesso = "";
            var Wregional = "";
            var Wnivelcritico = "";
            var Wfuncao = "";


            // Módulo

            if (form.Param["clusterGroupId"] != null && form.Param["clusterGroupId"].ToString() != "")
            {
                Wmodulo += " AND ParCluster_ID IN (SELECT ID FROM ParCluster WITH (NOLOCK) WHERE ParClusterGroup_Id = " + form.Param["clusterGroupId"].ToString() + ") ";
            }

            // Processo

            if (form.ParCluster_Ids.Length > 0)
            {
                Wprocesso += " AND ParCluster_ID IN (" + string.Join(",", form.ParCluster_Ids) + ") ";
            }

            // Regional

            if (form.ParCluster_Ids.Length > 0)
            {
                Wregional += " AND ParStructure_id  IN (" + string.Join(",", form.ParCluster_Ids) + ") ";
            }
            else if (form.Param["structureId"] != null && form.Param["structureId"].ToString() != "")
            {
                Wregional += " AND ParStructure_id  IN (" + form.Param["structureId"].ToString() + ") ";
            }

            // Nivel Crítico

            if (form.ParCriticalLevel_Ids.Length > 0)
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.ParCriticalLevel_Ids) + ") ";
            }
            else if (form.Param["criticalLevelId"] != null && form.Param["criticalLevelId"].ToString() != "")
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + form.Param["criticalLevelId"].ToString() + ") ";
            }

            // Função

            if (form.ParLevel1Group_Ids.Length > 0)
            {
                Wfuncao += " AND Indicador IN (SELECT distinct ParLevel1_id  FROM ParGroupParLevel1XParLevel1 WHERE IsActive = 1 AND ParGroupParLevel1_Id IN (" + string.Join(",", form.ParLevel1Group_Ids) + "))"; // " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.criticalLevelIdArr) + ") ";
            }

            #endregion


            var script = "";
            var SQLcentro = "";

            SQLcentro = getQuery(form, nivel);

            #region Status do Indicador: Fora ou Dentro da Meta
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 1) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 2) // Indicadores Fora Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }

            #endregion


            #region Indicadores com Acao Concluída: Sim ou Não

            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 1)
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 2) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }
            #endregion


            if (tipoVisao == false) // 0: Listagem / 1: Evolutivo 
            { // Considero Dimensões
                #region ScriptLista
                script += @"

            " + SQLcentro + @"

            SELECT 
            	Indicador 
               ,IndicadorName 
               --,'" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS PC
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,max(ISNULL(Meta,0)) AS Meta
               ,cast(1 as bit) IsIndicador
               ,IIF(IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)))>max(ISNULL(Meta,0)),0,1) AS Status
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                Indicador 
               ,IndicadorName 
            ORDER BY 3 DESC
            ";
                #endregion
            }
            else if (tipoVisao == true)
            { // Desconsidero Dimensões
                #region ScriptGrafico
                script += @"

            " + SQLcentro + @"

            SELECT 
               '" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS pc
               ,ConsolidationDate as [date]
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,sum(ISNULL(Meta,0)) AS Meta
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                ConsolidationDate
            ORDER BY 3 
            ";
                #endregion
            }


            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno = factory.SearchQuery<RelatorioResultadosPeriodo>(script).ToList();
            }
        }

        private void GetResultadosMonitoramentoSemUnidade(DTO.DataCarrierFormularioNew form)
        {
            var nivel = 2;
            var tipoVisao = false;

            #region Filtros

            var titulo = "Histórico do Indicador";

            var Wmodulo = "";
            var Wprocesso = "";
            var Wregional = "";
            var Wnivelcritico = "";
            var Wfuncao = "";


            // Módulo

            if (form.Param["clusterGroupId"] != null && form.Param["clusterGroupId"].ToString() != "")
            {
                Wmodulo += " AND ParCluster_ID IN (SELECT ID FROM ParCluster WITH (NOLOCK) WHERE ParClusterGroup_Id = " + form.Param["clusterGroupId"].ToString() + ") ";
            }

            // Processo

            if (form.ParCluster_Ids.Length > 0)
            {
                Wprocesso += " AND ParCluster_ID IN (" + string.Join(",", form.ParCluster_Ids) + ") ";
            }

            // Regional

            if (form.ParStructure_Ids.Length > 0)
            {
                Wregional += " AND ParStructure_id  IN (" + string.Join(",", form.ParStructure_Ids) + ") ";
            }
            else if (form.Param["structureId"] != null && form.Param["structureId"].ToString() != "")
            {
                Wregional += " AND ParStructure_id  IN (" + form.Param["structureId"].ToString() + ") ";
            }

            // Nivel Crítico

            if (form.ParCriticalLevel_Ids.Length > 0)
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.ParCriticalLevel_Ids) + ") ";
            }
            else if (form.Param["criticalLevelId"] != null && form.Param["criticalLevelId"].ToString() != "")
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + form.Param["criticalLevelId"].ToString() + ") ";
            }

            // Função

            if (form.ParLevel1Group_Ids.Length > 0)
            {
                Wfuncao += " AND Indicador IN (SELECT distinct ParLevel1_id  FROM ParGroupParLevel1XParLevel1 WHERE IsActive = 1 AND ParGroupParLevel1_Id IN (" + string.Join(",", form.ParLevel1Group_Ids) + "))"; // " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.criticalLevelIdArr) + ") ";
            }

            #endregion


            var script = "";
            var SQLcentro = "";

            SQLcentro = getQuery(form, nivel);

            #region Status do Indicador: Fora ou Dentro da Meta
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 1) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 2) // Indicadores Fora Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }

            #endregion


            #region Indicadores com Acao Concluída: Sim ou Não

            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 1)
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 2) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }
            #endregion


            if (tipoVisao == false) // 0: Listagem / 1: Evolutivo 
            { // Considero Dimensões
                #region ScriptLista
                script += @"

            " + SQLcentro + @"

            SELECT 
            	Indicador 
               ,IndicadorName 
               ,Monitoramento
               ,MonitoramentoName
               --,'" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS PC
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,max(ISNULL(Meta,0)) AS Meta
               ,cast(1 as bit) IsMonitoramento
               ,IIF(IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)))>max(ISNULL(Meta,0)),0,1) AS Status
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                Indicador 
               ,IndicadorName 
               ,Monitoramento
               ,MonitoramentoName
            ORDER BY 5 DESC
            ";
                #endregion
            }
            else if (tipoVisao == true)
            { // Desconsidero Dimensões
                #region ScriptGrafico
                script += @"

            " + SQLcentro + @"

            SELECT 
               '" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS pc
               ,ConsolidationDate as [date]
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,sum(ISNULL(Meta,0)) AS Meta
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                ConsolidationDate
            ORDER BY 3 
            ";
                #endregion
            }


            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno = factory.SearchQuery<RelatorioResultadosPeriodo>(script).ToList();
            }

        }

        private void GetResultadosTarefaSemUnidade(DTO.DataCarrierFormularioNew form)
        {
            var nivel = 3;
            var tipoVisao = false;

            #region Filtros

            var titulo = "Histórico do Indicador";

            var Wmodulo = "";
            var Wprocesso = "";
            var Wregional = "";
            var Wnivelcritico = "";
            var Wfuncao = "";


            // Módulo

            if (form.Param["clusterGroupId"] != null && form.Param["clusterGroupId"].ToString() != "")
            {
                Wmodulo += " AND ParCluster_ID IN (SELECT ID FROM ParCluster WITH (NOLOCK) WHERE ParClusterGroup_Id = " + form.Param["clusterGroupId"].ToString() + ") ";
            }

            // Processo

            if (form.ParCluster_Ids.Length > 0)
            {
                Wprocesso += " AND ParCluster_ID IN (" + string.Join(",", form.ParCluster_Ids) + ") ";
            }

            // Regional

            if (form.ParStructure_Ids.Length > 0)
            {
                Wregional += " AND ParStructure_id  IN (" + string.Join(",", form.ParStructure_Ids) + ") ";
            }
            else if (form.Param["structureId"] != null && form.Param["structureId"].ToString() != "")
            {
                Wregional += " AND ParStructure_id  IN (" + form.Param["structureId"].ToString() + ") ";
            }

            // Nivel Crítico

            if (form.ParCriticalLevel_Ids.Length > 0)
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.ParCriticalLevel_Ids) + ") ";
            }
            else if (form.Param["criticalLevelId"] != null && form.Param["criticalLevelId"].ToString() != "")
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + form.Param["criticalLevelId"].ToString() + ") ";
            }

            // Função

            if (form.ParLevel1Group_Ids.Length > 0)
            {
                Wfuncao += " AND Indicador IN (SELECT distinct ParLevel1_id  FROM ParGroupParLevel1XParLevel1 WHERE IsActive = 1 AND ParGroupParLevel1_Id IN (" + string.Join(",", form.ParLevel1Group_Ids) + "))"; // " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.criticalLevelIdArr) + ") ";
            }

            #endregion


            var script = "";
            var SQLcentro = "";

            SQLcentro = getQuery(form, nivel);

            #region Status do Indicador: Fora ou Dentro da Meta
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 1) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 2) // Indicadores Fora Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }

            #endregion

            #region Indicadores com Acao Concluída: Sim ou Não

            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 1)
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 2) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }
            #endregion


            if (tipoVisao == false) // 0: Listagem / 1: Evolutivo 
            { // Considero Dimensões
                #region ScriptLista
                script += @"

            " + SQLcentro + @"

            SELECT 
            	Indicador 
               ,IndicadorName 
               ,Monitoramento
               ,MonitoramentoName
               ,Tarefa
               ,TarefaName
               --,'" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS PC
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,max(ISNULL(Meta,0)) AS Meta
               ,cast(1 as bit) IsTarefa
               ,IIF(IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)))>max(ISNULL(Meta,0)),0,1) AS Status
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                Indicador 
               ,IndicadorName 
               ,Monitoramento
               ,MonitoramentoName
               ,Tarefa
               ,TarefaName
            ORDER BY 7 DESC
            ";
                #endregion
            }
            else if (tipoVisao == true)
            { // Desconsidero Dimensões
                #region ScriptGrafico
                script += @"

            " + SQLcentro + @"

            SELECT 
               '" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS pc
               ,ConsolidationDate as [date]
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,sum(ISNULL(Meta,0)) AS Meta
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                ConsolidationDate
            ORDER BY 3 
            ";
                #endregion
            }


            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno = factory.SearchQuery<RelatorioResultadosPeriodo>(script).ToList();
            }


        }

        [HttpPost]
        [Route("GetGraficoHistoricoModal")]
        public List<RetornoGenerico> GetGraficoHistoricoModal([FromBody] DTO.DataCarrierFormularioNew form)
        {

            string query = "";

            if (form.Param["level3Id"] != null && form.Param["level3Id"].ToString() != "")
            {
                query = getQueryHistoricoTarefa(form, true, 3); // 0: Listagem / 1: Evolutivo 

            }
            else if (form.Param["level2Id"] != null && form.Param["level2Id"].ToString() != "")
            {
                query = getQueryHistoricoMonitoramento(form, true, 2); // 0: Listagem / 1: Evolutivo 
            }
            else
            {
                query = getQueryHistorioIndicador(form, true, 1); // 0: Listagem / 1: Evolutivo 
            }

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno2 = factory.SearchQuery<RetornoGenerico>(query).ToList();
            }

            //GetMockHistoricoModal();
            return retorno2;
        }

        [HttpPost]
        [Route("GetHistoricoScore")]
        public List<RetornoGenerico> GetHistoricoScore([FromBody] DTO.DataCarrierFormularioNew form)
        {

            #region consultaPrincipal

            /*
            * neste score NAO devo considerar a regra dos 70 %
            * 
            */

            var query = "";//VisaoGeralDaAreaApiController.sqlBase(form);"
            #endregion

            #region Queryes Trs Meio

            var whereClusterGroup = "";
            var whereCluster = "";
            var whereStructure = "";
            var whereCriticalLevel = "";
            var whereParCompany = "";
            var whereUnit = "";

            if (form.Param["clusterGroupId"] != null && form.Param["clusterGroupId"].ToString() != "")
            {
                whereClusterGroup = $@" AND C.id IN (SELECT DISTINCT c.Id FROM Parcompany c LEFT JOIN ParCompanyCluster PCC WITH (NOLOCK) ON C.Id = PCC.ParCompany_Id LEFT JOIN ParCluster PC WITH (NOLOCK) ON PC.Id = PCC.ParCluster_Id LEFT JOIN ParClusterGroup PCG WITH (NOLOCK) ON PC.ParClusterGroup_Id = PCG.Id WHERE PCG.id = { form.Param["clusterGroupId"].ToString() } AND PCC.Active = 1)";
            }

            if (form.Param["clusterSelected_Id"] != null && form.Param["clusterSelected_Id"].ToString() != "")
            {
                whereCluster = $@" AND C.ID IN (SELECT DISTINCT c.id FROM Parcompany c Left Join ParCompanyCluster PCC with (nolock) on c.id= pcc.ParCompany_Id WHERE PCC.ParCluster_Id = { form.Param["clusterSelected_Id"].ToString() } and PCC.Active = 1)";
            }

            if (form.Param["structureId"] != null && form.Param["structureId"].ToString() != "")
            {
                whereStructure = $@" AND reg.id = { form.Param["structureId"].ToString() }";
            }

            if (form.Param["unitId"] != null && form.Param["unitId"].ToString() != "")
            {
                whereUnit = $@"AND C.Id = { form.Param["unitId"].ToString() }";
            }

            if (form.Param["criticalLevelId"] != null && form.Param["criticalLevelId"].ToString() != "")
            {
                whereCriticalLevel = $@"  AND S.Level1Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.Param["criticalLevelId"].ToString() })";
            }

            if (form.ParCompany_Ids.Count() > 0 && form.ParCompany_Ids != null)
            {
                whereParCompany = $@"AND S.ParCompany_Id IN (" + string.Join(",", form.ParCompany_Ids) + ") ";
            }


            var where = string.Empty;
            where += "";


            var queryTotal =

                $@" SELECT
                   mesData [date]
                   ,  0 as level1Id                
                   ,'a1' as Level1Name             
                   ,'a2' as ChartTitle             
                   ,0 as UnidadeId               
                   ,'a4' as UnidadeName            
                   ,20.0 AS procentagemNc          
                   ,10.0 AS Meta           
                   ,50.0 as av                     
                  ,case when sum(av) is null or sum(av) = 0 then 0 else cast(round(cast(case when isnull(avg(PontosIndicador), 100) = 0 or isnull(avg([PONTOS ATINGIDOS OK]), 100) = 0 then 0 else (ISNULL(avg([PONTOS ATINGIDOS OK]), 100) / isnull(avg(PontosIndicador), 100)) * 100  end as decimal (10, 1)), 2) as decimal(10,1)) end nc
                  FROM " + "VisaoGeralDaAreaApiController.sqlBaseGraficosScorecardDiario()" +
                  @"
                  where 1=1 AND (pC.IsActive = 1 OR PC.ISACTIVE IS NULL) 
                  " + whereClusterGroup +
                  whereCluster +
                  whereStructure +
                  whereCriticalLevel +
                  whereParCompany +
                  whereUnit +
                @"
                  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 3        
                  AND C.IsActive = 1
                GROUP BY C.Initials, C.Name, S.LEVEL1ID, s.LEVEL1NAME, S.TIPOINDICADOR, Reg.Id, Reg.Name, S.mesData
                
                  ) AAA 
                  GROUP BY companySigla, companyTitle, LEVEL1ID, LEVEL1NAME, TIPOINDICADOR, RegId, RegName, mesData
                      ) A
              group by mesData";

            #endregion


            string grandeQuery = query + " " + queryTotal;

            var result = new List<RetornoGenerico>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                result = factory.SearchQuery<RetornoGenerico>(grandeQuery).ToList();
            }

            return result;
        }

        [HttpPost]
        [Route("GetHistoricoScoreNumero")]
        public List<RetornoGenerico> GetHistoricoScoreNumero([FromBody] DTO.DataCarrierFormularioNew form)
        {
            #region consultaPrincipal

            /*
            * neste score NAO devo considerar a regra dos 70 %
            * 
            */

            var query = ""; //VisaoGeralDaAreaApiController.sqlBase(form);

            #endregion

            #region Queryes Trs Meio

            var whereClusterGroup = "";
            var whereCluster = "";
            var whereStructure = "";
            var whereCriticalLevel = "";
            var whereParCompany = "";
            var whereUnit = "";

            if (form.Param["clusterGroupId"] != null && form.Param["clusterGroupId"].ToString() != "")
            {
                whereClusterGroup = $@" AND C.id IN (SELECT DISTINCT c.Id FROM Parcompany c LEFT JOIN ParCompanyCluster PCC WITH (NOLOCK) ON C.Id = PCC.ParCompany_Id LEFT JOIN ParCluster PC WITH (NOLOCK) ON PC.Id = PCC.ParCluster_Id LEFT JOIN ParClusterGroup PCG WITH (NOLOCK) ON PC.ParClusterGroup_Id = PCG.Id WHERE PCG.id = { form.Param["clusterGroupId"].ToString() } AND PCC.Active = 1)";
            }

            if (form.Param["clusterSelected_Id"] != null && form.Param["clusterSelected_Id"].ToString() != "")
            {
                whereCluster = $@" AND C.ID IN (SELECT DISTINCT c.id FROM Parcompany c Left Join ParCompanyCluster PCC with (nolock) on c.id= pcc.ParCompany_Id WHERE PCC.ParCluster_Id = { form.Param["clusterSelected_Id"].ToString() } and PCC.Active = 1)";
            }

            if (form.Param["structureId"] != null && form.Param["structureId"].ToString() != "")
            {
                whereStructure = $@" AND reg.id = { form.Param["structureId"].ToString() }";
            }

            if (form.Param["unitId"] != null && form.Param["unitId"].ToString() != "")
            {
                whereUnit = $@" AND C.Id = { form.Param["unitId"].ToString() }";
            }

            if (form.Param["criticalLevelId"] != null && form.Param["criticalLevelId"].ToString() != "")
            {
                whereCriticalLevel = $@"  AND S.Level1Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.Param["criticalLevelId"].ToString() })";
            }

            if (form.ParCompany_Ids.Count() > 0 && form.ParCompany_Ids != null)
            {
                whereParCompany = $@"AND S.ParCompany_Id IN (" + string.Join(",", form.ParCompany_Ids) + ") ";
            }

            var where = string.Empty;
            where += "";

            string queryLocal =

             $@" SELECT
              '0001-01-01' [date]
              ,  0 as level1Id                
              ,'a1' as Level1Name             
              ,'a2' as ChartTitle             
              ,0 as UnidadeId               
              ,'a4' as UnidadeName            
              ,20.0 AS procentagemNc          
              ,10.0 AS Meta           
              ,50.0 as av                     
             ,case when sum(av) is null or sum(av) = 0 then 0 else cast(round(cast(case when isnull(avg(PontosIndicador), 100) = 0 or isnull(avg([PONTOS ATINGIDOS OK]), 100) = 0 then 0 else (ISNULL(avg([PONTOS ATINGIDOS OK]), 100) / isnull(avg(PontosIndicador), 100)) * 100  end as decimal (10, 1)), 2) as decimal(10,1)) end nc
             FROM " + "VisaoGeralDaAreaApiController.sqlBaseGraficosVGA()" +
               @"
                 where 1=1 AND (pC.IsActive = 1 OR PC.ISACTIVE IS NULL)
                 " + whereClusterGroup +
               whereCluster +
               whereStructure +
               whereCriticalLevel +
               whereParCompany +
               whereUnit +
               @"
                AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 3        
                AND C.IsActive = 1
               GROUP BY C.Initials, C.Name, S.LEVEL1ID, s.LEVEL1NAME, S.TIPOINDICADOR, Reg.Id, Reg.Name

             ) AAA 
             GROUP BY companySigla, companyTitle, LEVEL1ID, LEVEL1NAME, TIPOINDICADOR, RegId, RegName
                      ) A
                     ";



            #endregion



            string grandeQuery = query + " " + queryLocal;

            var result = new List<RetornoGenerico>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                result = factory.SearchQuery<RetornoGenerico>(grandeQuery).ToList();
            }

            return result;
        }

        [HttpPost]
        [Route("GetHistoricoScoreMensal")]
        public List<RetornoGenerico> GetHistoricoScoreMensal([FromBody] DTO.DataCarrierFormularioNew form)

        {
            #region consultaPrincipal

            /*
            * neste score NAO devo considerar a regra dos 70 %
            * 
            */

            var query = "" +

// "\n DECLARE @DATAINICIAL DATETIME = '" + form._dataInicioSQL + "'    " +
// "\n DECLARE @DATAFINAL   DATETIME = '" + form._dataFimSQL + "'       " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n  IF OBJECT_ID('tempdb.dbo.#SCORE', 'U') IS NOT NULL																																																										                                               " +
"\n    DROP TABLE #SCORE; 																																																																	                                               " +
"\n  																																																																						                                               " +
"\n  CREATE TABLE #SCORE (																																																																	                                               " +
"\n  	Cluster int null,																																																																	                                               " +
"\n  	ClusterName Varchar(153) null,																																																														                                               " +
"\n  	Regional int null,																																																																	                                               " +
"\n  	RegionalName Varchar(153) null, 																																																													                                               " +
"\n  	ParCompany_Id int null,																																																																                                               " +
"\n  	ParCompanyName Varchar(153) null, 																																																													                                               " +
"\n  	TipoIndicador int null,																																																																                                               " +
"\n  	TipoIndicadorName Varchar(153) null, 																																																												                                               " +
"\n  	Level1Id int null,																																																																	                                               " +
"\n  	Level1Name Varchar(153) null, 																																																														                                               " +
"\n  	Criterio int null,																																																																	                                               " +
"\n  	CriterioName Varchar(153) null, 																																																													                                               " +
"\n  	Av decimal(30,5) null,																																																																                                               " +
"\n  	Nc decimal(30,5) null,																																																																                                               " +
"\n  	Pontos decimal(30,5) null,																																																															                                               " +
"\n  	PontosIndicador decimal(30,5) null,																																																													                                               " +
"\n  	Meta decimal(30,5) null,																																																															                                               " +
"\n  	Real decimal(30,5) null,																																																															                                               " +
"\n  	PontosAtingidos decimal(30,5) null,																																																													                                               " +
"\n  	Scorecard decimal(30,5) null,																																																														                                               " +
"\n  	TipoScore Varchar(153) null," +
"\n mesData datetime2(7) null  " +
"\n  	)																																																																					                                               " +
"\n  																																																																						                                               " +
"\n  																																																																						                                               " +
"\n  																																																																						                                               " +
"\n  DECLARE @I INT = 0																																																																		                                               " +
"\n  																																																																						                                               " +
"\n  WHILE (SELECT @I) < 1																																																																	                                               " +
"\n  BEGIN  																																																																					                                           " +
"\n     																																																																						                                           " +
"\n      																																																																					                                               " +
"\n   DECLARE @DATAINICIAL DATETIME = '" + form.Param["_dataInicioSQL"].ToString() + " 00:00'                                                                                                                                                                                                                    					                                               " +
"\n   DECLARE @DATAFINAL   DATETIME = '" + form.Param["_dataFimSQL"].ToString() + "  23:59:59'                                                                                                                                                                                                                    					                                               " +


@"            SELECT 

                CL1.id,
            	CL1.ConsolidationDate,
            	CL1.UnitId,
            	CL1.ParLevel1_Id,
            	CL1.DefectsResult,
            	CL1.WeiDefects,
            	CL1.EvaluatedResult,
            	CL1.WeiEvaluation,
            	CL1.EvaluateTotal,
            	CL1.TotalLevel3WithDefects,
            	CL1.DefectsTotal
            INTO #ConsolidationLevel
            FROM ConsolidationLevel1 CL1 WITH(NOLOCK)
            WHERE 1 = 1
            AND CL1.ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL

            CREATE INDEX IDX_HashConsolidationLevel ON #ConsolidationLevel (ConsolidationDate,UnitId,ParLevel1_Id); 
            CREATE INDEX IDX_HashConsolidationLevel_level1 ON #ConsolidationLevel (ConsolidationDate,ParLevel1_Id); 
            CREATE INDEX IDX_HashConsolidationLevel_Unitid ON #ConsolidationLevel (ConsolidationDate,UnitId); 
            CREATE INDEX IDX_HashConsolidationLevel_id ON #ConsolidationLevel (id); " +


               // Alteração
               "\n CREATE TABLE #AMOSTRATIPO4 ( " +

                "\n UNIDADE INT NULL, " +
                "\n INDICADOR INT NULL, " +
                "\n AM INT NULL, " +
                "\n DEF_AM INT NULL " +
                "\n ) " +


                "\n INSERT INTO #AMOSTRATIPO4 " +
                /*
                "\n SELECT " +
                "\n  UNIDADE, INDICADOR, " +
                "\n FROM " +
                "\n ( " +
                */
                "\n     SELECT " +
                //"\n     cast(C2.CollectionDate as DATE) AS DATA " +
                "\n     C.Id AS UNIDADE " +
                "\n     , C2.ParLevel1_Id AS INDICADOR " +
                "\n , COUNT(DISTINCT CONCAT(c2.Period, '-', c2.shift, '-', C2.EvaluationNumber, '-', C2.Sample, '-', cast(cast(C2.CollectionDate as date) as varchar))) AM " +
                "\n , SUM(IIF(C2.WeiDefects = 0, 0, 1)) DEF_AM " +
                //"\n     , C2.EvaluationNumber AS AV " +
                // "\n     , C2.Sample AS AM " +
                //"\n     , case when SUM(C2.WeiDefects) = 0 then 0 else 1 end DEF_AM " +
                "\n     FROM CollectionLevel2 C2 (nolock) " +
                "\n     INNER JOIN ParLevel1 L1 (nolock)  " +
                "\n     ON L1.Id = C2.ParLevel1_Id AND ISNULL(L1.ShowScorecard, 1) = 1" +

                "\n     INNER JOIN ParCompany C (nolock)  " +
                "\n     ON C.Id = C2.UnitId " +
                "\n     where C2.CollectionDate BETWEEN @DATAINICIAL AND @DATAFINAL " +
                "\n     and C2.NotEvaluatedIs = 0 " +
                "\n     and C2.Duplicated = 0 " +
                "\n     and L1.ParConsolidationType_Id = 4 " +
                "\n     group by C.Id, ParLevel1_Id" +
           /*
           "\n ) TAB " +
           "\n GROUP BY UNIDADE, INDICADOR " +
           */



           "\n                                                                                                                                                                                                                                                                     " +
           "\n DECLARE @VOLUMEPCC INT                                                                                                                                                                                                                                              " +
           "\n DECLARE @DIASABATE INT                                                                                                                                                                                                                                              " +
           "\n DECLARE @DIASDEVERIFICACAO INT                                                                                                                                                                                                                                      " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n DECLARE @AVFREQUENCIAVERIFICACAO INT                                                                                                                                                                                                                                " +
           "\n DECLARE @NCFREQUENCIAVERIFICACAO INT                                                                                                                                                                                                                                " +
           "\n                                                                                                                                                                                                                                                                     " +

"\n   /* INICIO DADOS DA FREQUENCIA ------------------------------------------------------*/                                                                                                                                                                              					                                               " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n CREATE TABLE #FREQ (                                                                                                                                                                                                                                                                                                                   " +
"\n    clusterId int null                                                                                                                                                                                                                                                                                                                  " +
"\n    ,     cluster varchar(255) null                                                                                                                                                                                                                                                                                                     " +
"\n    , regionalId int null                                                                                                                                                                                                                                                                                                               " +
"\n    , regional varchar(255) null                                                                                                                                                                                                                                                                                                        " +
"\n    , unitId int null                                                                                                                                                                                                                                                                                                                   " +
"\n    , unidade  varchar(255) null                                                                                                                                                                                                                                                                                                        " +
"\n    , criticalLevelId  int null                                                                                                                                                                                                                                                                                                         " +
"\n    , criticalLevel varchar(255) null                                                                                                                                                                                                                                                                                                   " +
"\n    , pontos decimal(35, 10) null                                                                                                                                                                                                                                                                                                       " +
"\n    )                                                                                                                                                                                                                                                                                                                                   " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                 					                                                                                   " +
"\n                                                                                                                                                                                                                                                                        					                                               " +
"\n    insert into #freq                                                                                                                                                                                                                                                                                                                   " +
"\n    SELECT                                                                                                                                                                                                                                                                                                                              " +
"\n    CL.Id                                                                                                                                                                                                                                                                                                                               " +
"\n    , CL.Name                                                                                                                                                                                                                                                                                                                           " +
"\n    , S.Id                                                                                                                                                                                                                                                                                                                              " +
"\n    , S.Name                                                                                                                                                                                                                                                                                                                            " +
"\n    , C.Id                                                                                                                                                                                                                                                                                                                              " +
"\n    , C.Name                                                                                                                                                                                                                                                                                                                            " +
"\n    , L1C.ParCriticalLevel_Id                                                                                                                                                                                                                                                                                                           " +
"\n    , CRL.Name                                                                                                                                                                                                                                                                                                                          " +
"\n    , L1C.Points                                                                                                                                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n    FROM ParCompany C                                                                                                                                                                                                                                                                                                                   " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n    LEFT JOIN ParCompanyXStructure CS                                                                                                                                                                                                                                                                                                   " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n           ON CS.ParCompany_Id = C.Id                                                                                                                                                                                                                                                                                                   " +
"\n    LEFT JOIN ParStructure S                                                                                                                                                                                                                                                                                                            " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n           ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                                                                                 " +
"\n    LEFT JOIN ParStructureGroup SG                                                                                                                                                                                                                                                                                                      " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n           ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                                                                                            " +
"\n    LEFT JOIN ParCompanyCluster CCL                                                                                                                                                                                                                                                                                                     " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n           ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1                                                                                                                                                                                                                                                                              " +
"\n    LEFT JOIN ParCluster CL                                                                                                                                                                                                                                                                                                             " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n           ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                                                                                 " +
"\n    LEFT JOIN ParLevel1XCluster L1C                                                                                                                                                                                                                                                                                                     " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n           ON L1C.ParLevel1_Id = 25 AND L1C.ParCluster_Id = Cl.Id   AND L1C.IsActive = 1   AND CCL.ParCluster_Id = L1C.ParCluster_Id                                                                                                                                                                                                                                               " +
"\n    LEFT JOIN ParCriticalLevel CRL                                                                                                                                                                                                                                                                                                      " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n           ON L1C.ParCriticalLevel_Id = CRL.Id                                                                                                                                                                                                                                                                                          " +
"\n    WHERE C.Id >= 1                                                                                                                                                                                                                                                                                                                     " +
"\n    AND L1C.ParLevel1_Id = 25                                                                                                                                                                                                                                                                                                           " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                       					                                               " +
"\n   /* FIM DOS DADOS DA FREQUENCIA -----------------------------------------------------*/                                                                                                                                                                              					                                               " +
"\n   CREATE TABLE #VOLUMES (                                                                                                                                                                                                                                                                                                              " +
"\n 	DIASABATE INT NULL,                                                                                                                                                                                                                                                                                                                " +
"\n 	VOLUMEPCC INT NULL, unitid int null , data date null                                                                                                                                                                                                                                                                                                                " +
"\n   )                                                                                                                                                                                                                                                                                                                                    " +
"\n   INSERT INTO #VOLUMES                                                                                                                                                                                                                                                                    					                           " +
"\n   SELECT COUNT(1) AS DIASABATE, SUM(Quartos) AS VOLUMEPCC, ParCompany_id as UnitId, cast(data as date) data FROM VolumePcc1b WHERE Data BETWEEN @DATAINICIAL AND @DATAFINAL GROUP BY ParCompany_id, cast(data as date)                                                                                                  					                                                                   " +
"\n                                                                                                                                                                                                                                                                                                                                        " +
"\n   CREATE TABLE #DIASVERIFICACAO (                                                                                                                                                                                                                                                                                                      " +
"\n 	DIASVERIFICACAO INT NULL,                                                                                                                                                                                                                                                                                                          " +
"\n 	UnitId INT NULL                                                                                                                                                                                                                                                                                                                    " +
"\n   )                                                                                                                                                                                                                                                                                                                                    " +
"\n   INSERT INTO #DIASVERIFICACAO                                                                                                                                                                                                                                                                    					                   " +
"\n   SELECT COUNT(1) as DIASDEVERIFICACAO, UnitId FROM                                                                                                                                                                                                                                                                                    " +
"\n   (                                                                                                                                                                                                                                                                                                                                    " +
"\n 	SELECT CL1.UnitId, CONVERT(DATE, ConsolidationDate) DATA                                                                                                                                                                                                                                                                           " +
"\n 	FROM ConsolidationLevel1 CL1                                                                                                                                                                                                                                                                                                       " +
"\n 	WHERE ParLevel1_Id = 24 AND CONVERT(DATE, ConsolidationDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                                                                                                                                                   " +
"\n 	GROUP BY CONVERT(DATE, ConsolidationDate), CL1.UnitId                                                                                                                                                                                                                                                                              " +
"\n   ) VT                                                                                                                                                                                                                                                                                                                                 " +
"\n   GROUP BY UnitId    " +
           "\n                                                                                                                                                                                                                                                                                                                                        " +
"\n   CREATE TABLE #NAPCC (                                                                                                                                                                                                                                                                                                                " +
"\n 	NAPCC INT NULL,                                                                                                                                                                                                                                                                                                                    " +
"\n 	UnitId INT NULL, data date null                                                                                                                                                                                                                                                                                                                    " +
"\n   )                                                                                                                                                                                                                                                                                                                                    " +
"\n   INSERT INTO #NAPCC  					                                                                                                                                                                                                                                               					                               " +
"\n   SELECT                                                                                                                                                                                                                                                              					                                               " +
"\n            COUNT(1) as NAPCC, unitId, data                                                                                                                                                                                                                                                                                                          " +
"\n 		                                                                                                                                                                                                                                                   						                                               " +
"\n            FROM                                                                                                                                                                                                                                                     						                                           " +
"\n       (                                                                                                                                                                                                                                                             						                                           " +
"\n                SELECT                                                                                                                                                                                                                                               						                                           " +
"\n                COUNT(1) AS NA, cast(c2.collectiondate as date) data,                                                                                                                                                                                                                                                                                                         " +
"\n 			   C2.UnitId                                                                                                                                                                                                                                      						                                                   " +
"\n                FROM CollectionLevel2 C2                                                                                                                                                                                                                             						                                           " +
"\n                LEFT JOIN Result_Level3 C3                                                                                                                                                                                                                           						                                           " +
"\n                ON C3.CollectionLevel2_Id = C2.Id                                                                                                                                                                                                                    						                                           " +
"\n                WHERE convert(date, C2.CollectionDate) BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                                                                                           						                                           " +
"\n                AND C2.ParLevel1_Id = (SELECT top 1 id FROM Parlevel1 where Hashkey = 1 AND ISNULL(ShowScorecard, 1) = 1)                                                                                                                                                                             						                                           " +
"\n                --AND C2.UnitId = @ParCompany_Id                                                                                                                                                                                                                       						                                           " +
"\n                AND IsNotEvaluate = 1                                                                                                                                                                                                                                						                                           " +
"\n                GROUP BY C2.ID, C2.UnitId, cast(c2.collectiondate as date)                                                                                                                                                                                                                                       						                                   " +
"\n            ) NA		                                                                                                                                                                                                                                                        						                                   " +
"\n            WHERE NA = 2                                                                                                                                                                                                                                             						                                           " +
"\n 		   GROUP BY UnitId, data                                                                                                                                                                                                                                                                                                                                                                                                                                " +
"\n   																																																																						                                               " +
"\n   INSERT INTO #SCORE																																						" +
"\n SELECT                                                                                                                                                                                                                                                                                                                                                        " +
"\n   Cluster                                                                                                                                                                                                                                                                                                                                                     " +
"\n  , ClusterName                                                                                                                                                                                                                                                                                                                                                " +
"\n  , Regional                                                                                                                                                                                                                                                                                                                                                   " +
"\n  , RegionalName                                                                                                                                                                                                                                                                                                                                               " +
"\n  , ParCompanyId                                                                                                                                                                                                                                                                                                                                              " +
"\n  , ParCompanyName                                                                                                                                                                                                                                                                                                                                             " +
"\n  , TipoIndicador                                                                                                                                                                                                                                                                                                                                              " +
"\n  , TipoIndicadorName                                                                                                                                                                                                                                                                                                                                          " +
"\n  , Level1Id                                                                                                                                                                                                                                                                                                                                                   " +
"\n  , Level1Name                                                                                                                                                                                                                                                                                                                                                 " +
"\n  , Criterio                                                                                                                                                                                                                                                                                                                                                   " +
"\n  , CriterioName                                                                                                                                                                                                                                                                                                                                               " +
"\n  , case when Level1Id = 3 then avg(av) else sum(av) end av                                                                                                                                                                                                                                                                                                                                                 " +
"\n  , sum(nc) nc                                                                                                                                                                                                                                                                                                                                                 " +
"\n  , (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) pontos                                                                                                                                                                                                                                                                                                                                         " +
"\n  , (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) pontosIndicador                                                                                                                                                                                                                                                                                                                                " +
"\n  , max(meta) meta                                                                                                                                                                                                                                                                                                                                             " +

"\n  , round(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) * 100) end end, 2) real " +
"\n  , case when(isnull(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else case when tipoIndicador = 1 then(max(meta) / (sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end))) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) / max(meta) * 100 * 100) end end * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) / nullif((CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END), 0) end, 0)) > 100 then 100 else (isnull(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else case when tipoIndicador = 1 then(max(meta) / (sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end))) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) / max(meta) * 100 * 100) end end * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) / nullif((CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END), 0) end, 0)) end / 100 * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) as pontosAtingidos " +
"\n  , case when(isnull(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else case when tipoIndicador = 1 then(max(meta) / (sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end))) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) / max(meta) * 100 * 100) end end * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) / nullif((CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END), 0) end, 0)) > 100 then 100 else (isnull(case when (case when Level1Id = 3 then avg(av) else sum(av) end) = 0 then 0 else case when sum(nc) = 0 then (case when tipoIndicador = 1 then 100 else 0 end) else case when tipoIndicador = 1 then(max(meta) / (sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end))) else ((sum(nc) / (case when Level1Id = 3 then avg(av) else sum(av) end)) / max(meta) * 100 * 100) end end * (CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END) / nullif((CASE WHEN SUM(AV) = 0 THEN 0 ELSE max(pontos) END), 0) end, 0)) end as Scorecard " +

"\n  , TipoScore " +
"\n  , max(month(mesData) + '-' + month(mesData) ) mesData FROM                                                                                                                                                                                                                                           " +
           "\n (                                                                                                                                                                                                                                                                   " +
           "\n SELECT                                                                                                                                                                                                                                                              " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n   Cluster                                                                                                                                                                                                                                                           " +
           "\n  , ClusterName                                                                                                                                                                                                                                                      " +
           "\n  , Regional                                                                                                                                                                                                                                                         " +
           "\n  , RegionalName                                                                                                                                                                                                                                                     " +
           "\n  , ParCompanyId                                                                                                                                                                                                                                                     " +
           "\n  , ParCompanyName                                                                                                                                                                                                                                                   " +
           "\n  , CASE WHEN TipoIndicador = 0 THEN 1 ELSE 2 END TipoIndicador                                                                                                                                                                                                      " +
           "\n  , CASE WHEN TipoIndicador = 0 THEN 'Menor' ELSE 'Maior' END TipoIndicadorName                                                                                                                                                                                      " +
           "\n  , Level1Id                                                                                                                                                                                                                                                         " +
           "\n  , Level1Name                                                                                                                                                                                                                                                       " +
           "\n  , Criterio                                                                                                                                                                                                                                                         " +
           "\n  , CriterioName                                                                                                                                                                                                                                                     " +
           "\n  , AV AV                                                                                                                                                                                                                                                               " +
           "\n  , CASE WHEN Level1Id = 25 THEN AV - NC ELSE NC END NC /* VERIFICAÇÃO DA TIPIFICAÇÃO */                                                                                                                                                                             " +
           "\n  , Pontos Pontos                                                                                                                                                                                                                                                           " +
           "\n  , CASE WHEN AV = 0 THEN 0 ELSE Pontos END AS PontosIndicador                                                                                                                                                                                                                                        " +
           "\n  , Meta AS Meta                                                                                                                                                                                                                                                             " +
           "\n  , CASE WHEN Level1Id = 25 THEN CASE WHEN AV = 0 THEN 0 ELSE (AV - NC) / AV * 100 END WHEN Level1Id = 43 THEN case when NC = 0 then 0 when (Meta / NC) > 1 then 1 else Meta / NC end * 100 ELSE Real END Real /* VERIFICAÇÃO DA TIPIFICAÇÃO */                                                                                                                            " +
           "\n  , CASE WHEN Level1Id = 43 AND AV > 0 AND  NC = 0 THEN Pontos ELSE PontosAtingidos END  PontosAtingidos                                                                                                                                                                                                                                               " +
           "\n  , CASE WHEN Level1Id = 43 AND AV > 0 AND NC = 0 THEN 100 ELSE Scorecard END  Scorecard                                                                                                                                                                                                                                                        " +
           "\n  , TipoScore ,mesData                                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n FROM                                                                                                                                                                                                                                                                " +
           "\n (                                                                                                                                                                                                                                                                   " +
           "\n SELECT                                                                                                                                                                                                                                                              " +
           "\n *,                                                                                                                                                                                                                                                                  " +
           "\n /* INICIO SCORECARD COMPLETO-------------------------------------*/                                                                                                                                                                                                 " +
           "\n CASE                                                                                                                                                                                                                                                                " +
           "\n     WHEN Level1Id = 43 THEN case when NC = 0 then 0 when (Meta / NC) > 1 then 1 else Meta / NC end                                                                                                                                                                                                                                                                " +
           "\n     WHEN                                                                                                                                                                                                                                                            " +
           "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     CASE                                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         ELSE                                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         END                                                                                                                                                                                                                                                         " +
           "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     > 1                                                                                                                                                                                                                                                             " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     THEN 1                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     ELSE                                                                                                                                                                                                                                                            " +
           "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     CASE                                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         ELSE                                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         END                                                                                                                                                                                                                                                         " +
           "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
           "\n END * 100                                                                                                                                                                                                                                                           " +
           "\n /* FIM SCORECARD COMPLETO----------------------------------------*/                                                                                                                                                                                                 " +
           "\n AS SCORECARD                                                                                                                                                                                                                                                        " +
           "\n ,                                                                                                                                                                                                                                                                   " +
           "\n /* INICIO SCORECARD COMPLETO-------------------------------------*/                                                                                                                                                                                                 " +
           "\n CASE                                                                                                                                                                                                                                                                " +
           "\n     WHEN Level1Id = 43 THEN case when NC = 0 then 0 when (Meta / NC) > 1 then 1 when (Meta / NC) < 0.00 then 0 else Meta / NC end                                                                                                                                                                                                                                                                 " +
           "\n     WHEN                                                                                                                                                                                                                                                            " +
           "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     CASE                                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         ELSE                                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         END                                                                                                                                                                                                                                                         " +
           "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     > 1                                                                                                                                                                                                                                                             " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     THEN 1                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     WHEN                                                                                                                                                                                                                                                            " +
           "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     CASE                                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         ELSE                                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         END                                                                                                                                                                                                                                                         " +
           "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     < 0.00                                                                                                                                                                                                                                                           " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     THEN 0                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     ELSE                                                                                                                                                                                                                                                            " +
           "\n     /*INICIO SCORECARD------------------------------------------------*/                                                                                                                                                                                            " +
           "\n     CASE                                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN TipoIndicador = 1 THEN                                                                                                                                                                                                                                 " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN (CASE WHEN Level1Id = 25 then 100 else 0 end) ELSE (CASE WHEN Level1Id = 25 then (100 - real) else real end) / meta END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         ELSE                                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             CASE WHEN NC = 0 AND AV > 0 THEN 100 ELSE META / NULLIF(REAL,0) END                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         END                                                                                                                                                                                                                                                         " +
           "\n     /*FIM SCORECARD---------------------------------------------------*/                                                                                                                                                                                            " +
           "\n END                                                                                                                                                                                                                                                                 " +
           "\n /* FIM SCORECARD COMPLETO----------------------------------------*/                                                                                                                                                                                                 " +
           "\n * /* MULTIPLICAÇÃO */                                                                                                                                                                                                                                               " +
           "\n PONTOS                                                                                                                                                                                                                                                              " +
           "\n AS PONTOSATINGIDOS                                                                                                                                                                                                                                                  " +
           "\n FROM                                                                                                                                                                                                                                                                " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n (                                                                                                                                                                                                                                                                   " +
           "\n SELECT                                                                                                                                                                                                                                                              " +
           "\n                                                                                                                                                                                                                                                                     " +
//"\n           ISNULL(CL.Id, (SELECT top 1 clusterId FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS Cluster                                                                                                                                                                      " +
//"\n , ISNULL(CL.Name, (SELECT top 1 cluster FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS ClusterName                                                                                                                                                                          " +
//"\n , ISNULL(S.Id, (SELECT top 1 regionalId FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS Regional                                                                                                                                                                             " +
//"\n , ISNULL(S.Name, (SELECT top 1 regional FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS RegionalName                                                                                                                                                                         " +
//"\n , ISNULL(CL1.UnitId, ft.ParCompany_id) AS ParCompanyId                                                                                                                                                                                                                       " +
//"\n , ISNULL(C.Name, (SELECT top 1 unidade FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS ParCompanyName                                                                                                                                                                        " +
//"\n , L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                                       " +
//"\n , L1.Id AS Level1Id                                                                                                                                                                                                                                                          " +
//"\n , L1.Name AS Level1Name                                                                                                                                                                                                                                                      " +
//"\n , ISNULL(CRL.Id, (SELECT top 1 criticalLevelId FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS Criterio                                                                                                                                                                      " +
//"\n , ISNULL(CRL.Name, (SELECT top 1 criticalLevel FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS CriterioName                                                                                                                                                                  " +
//"\n , ISNULL((select top 1 Points from ParLevel1XCluster aaa (nolock) where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate < @DATAFINAL), (SELECT top 1 pontos FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS Pontos                                    " +
//"\n   , ISNULL(CL1.ConsolidationDate, FT.Data) as mesData                                                                                                                                                                                                                       " +

"\n           ISNULL(CL.Id, (SELECT top 1 clusterId FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS Cluster                                                                                                                                                                      " +
  "\n , ISNULL(CL.Name, (SELECT top 1 cluster FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS ClusterName                                                                                                                                                                          " +
  "\n , ISNULL(S.Id, (SELECT top 1 regionalId FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS Regional                                                                                                                                                                             " +
  "\n , ISNULL(S.Name, (SELECT top 1 regional FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS RegionalName                                                                                                                                                                         " +
  "\n , ISNULL(CL1.UnitId, 0) AS ParCompanyId                                                                                                                                                                                                                       " +
  "\n , ISNULL(C.Name, (SELECT top 1 unidade FROM #FREQ WHERE unitId = FT.PARCOMPANY_ID)) AS ParCompanyName                                                                                                                                                                        " +
  "\n , L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                                       " +
  "\n , L1.Id AS Level1Id                                                                                                                                                                                                                                                          " +
  "\n , L1.Name AS Level1Name                                                                                                                                                                                                                                                      " +
  "\n , ISNULL(CRL.Id, (SELECT top 1 criticalLevelId FROM #FREQ WHERE unitId = 0)) AS Criterio                                                                                                                                                                      " +
  "\n , ISNULL(CRL.Name, (SELECT top 1 criticalLevel FROM #FREQ WHERE unitId = 0)) AS CriterioName                                                                                                                                                                  " +
  "\n , ISNULL((select top 1 Points from ParLevel1XCluster aaa (nolock) where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate < @DATAFINAL), (SELECT top 1 pontos FROM #FREQ WHERE unitId = 0)) AS Pontos                                    " +
  "\n   , ISNULL(CL1.ConsolidationDate, FT.DATA) as mesData                                                                                                                                                                                                                       " +


"\n                                                                                                                                                                                                                                                                     " +
           "\n  --ISNULL(CL.Id, @CLUSTER) AS Cluster                                                                                                                                                                                                                               " +
           "\n  --, (CL.Name)AS ClusterName                                                                                                                                                                                                                                        " +
           "\n  --, (S.Id)AS Regional                                                                                                                                                                                                                                              " +
           "\n  --, (S.Name)AS RegionalName                                                                                                                                                                                                                                        " +
           "\n  --, (CL1.UnitId)AS ParCompanyId                                                                                                                                                                                                                                    " +
           "\n  --, (C.Name)AS ParCompanyName                                                                                                                                                                                                                                      " +
           "\n  --, L1.IsRuleConformity AS TipoIndicador                                                                                                                                                                                                                           " +
           "\n  --, L1.Id AS Level1Id                                                                                                                                                                                                                                              " +
           "\n  --, L1.Name AS Level1Name                                                                                                                                                                                                                                          " +
           "\n  --, (CRL.Id)AS Criterio                                                                                                                                                                                                                                            " +
           "\n  --, (CRL.Name)AS CriterioName                                                                                                                                                                                                                                      " +
           "\n  --, (L1C.Points)AS Pontos                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n  , ST.Name AS TipoScore                                                                                                                                                                                                                                             " +
           "\n  ,                                                                                                                                                                                                                                                                  " +
           "\n   /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                              " +


           "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            //    "\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC)        FROM #VOLUMES WHERE UnitId = C.Id) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id)                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date ) ) " +

            "\n                                                                                                                                                                                                                                                                       					                                               " +
            "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
            "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
            "\n     END                                                                                                                                                                                                                                                               					                                               " +



           "\n   /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                              " +
           "\n   AS AV                                                                                                                                                                                                                                                             " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n  ,                                                                                                                                                                                                                                                                  " +
           "\n   /*INICIO NC COMPLETO----------------------------------------------*/                                                                                                                                                                                              " +
           "\n   CASE WHEN L1.IsRuleConformity = 1 THEN                                                                                                                                                                                                                            " +
           "\n       /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          " +
          "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            //"\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id and data = cast(cl1.ConsolidationDate as date)) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id  and data = cast(cl1.ConsolidationDate as date))                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date )) " +

            "\n                                                                                                                                                                                                                                                                       					                                               " +
            "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
            "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
            "\n     END                                                                                                                                                                                                                                                               					                                               " +


           "\n         /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                        " +
           "\n         -                                                                                                                                                                                                                                                           " +
           "\n       /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n       CASE                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           //"\n         WHEN L1.Id = 25 THEN SUM(FT.FREQ)       " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                                " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                              " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                        " +

           "\n       END                                                                                                                                                                                                                                                           " +
           "\n       /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n    ELSE                                                                                                                                                                                                                                                             " +
           "\n       /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n       CASE                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           //"\n         WHEN L1.Id = 25 THEN SUM(FT.FREQ)       " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                                " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                              " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n         WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                        " +

           "\n       END                                                                                                                                                                                                                                                           " +
           "\n       /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n      end                                                                                                                                                                                                                                                           " +
           "\n    /*FIM NC COMPLETO-------------------------------------------------*/                                                                                                                                                                                             " +
           "\n    AS NC                                                                                                                                                                                                                                                            " +
           "\n  ,                                                                                                                                                                                                                                                                  " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n   CASE                                                                                                                                                                                                                                                              " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     WHEN                                                                                                                                                                                                                                                            " +
           "\n       /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            //"\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id and data = cast(cl1.ConsolidationDate as date)) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id  and data = cast(cl1.ConsolidationDate as date))                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date )) " +

            "\n                                                                                                                                                                                                                                                                       					                                               " +
            "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
            "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
            "\n     END                                                                                                                                                                                                                                                               					                                               " +

           "\n                                                                                                                                                                                                                                                                 " +
           "\n       /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n       = 0                                                                                                                                                                                                                                                           " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     THEN 0                                                                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n      ELSE                                                                                                                                                                                                                                                           " +
           "\n       /*INICIO NC COMPLETO----------------------------------------------*/                                                                                                                                                                                          " +
           "\n       CASE WHEN L1.IsRuleConformity = 1 THEN                                                                                                                                                                                                                        " +
           "\n           /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                      " +
           "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            //"\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id and data = cast(cl1.ConsolidationDate as date)) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id  and data = cast(cl1.ConsolidationDate as date))                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date )) " +

            "\n                                                                                                                                                                                                                                                                       					                                               " +
            "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
            "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
            "\n     END                                                                                                                                                                                                                                                               					                                               " +
           "\n             /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                    " +
           "\n             -  /* SUBTRAÇÃO */                                                                                                                                                                                                                                      " +
           "\n                /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                 " +
           "\n           CASE                                                                                                                                                                                                                                                      " +
           "\n                                                                                                                                                                                                                                                                     " +
           //"\n             WHEN L1.Id = 25 THEN SUM(FT.FREQ)       " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                            " +
           "\n           END                                                                                                                                                                                                                                                       " +
           "\n           /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                      " +
           "\n        ELSE                                                                                                                                                                                                                                                         " +
           "\n           /*INICIO NC-------------------------------------------------------*/                                                                                                                                                                                      " +
           "\n           CASE                                                                                                                                                                                                                                                      " +
           "\n                                                                                                                                                                                                                                                                     " +
           //"\n             WHEN L1.Id = 25 THEN SUM(FT.FREQ)       " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiDefects)                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN CT.Id IN(3)   THEN SUM(CL1.DefectsResult)                                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n             WHEN CT.Id IN(4) THEN AVG(A4.DEF_AM)                                                                                                                                                                                                                            " +

           "\n           END                                                                                                                                                                                                                                                       " +
           "\n           /*FIM NC----------------------------------------------------------*/                                                                                                                                                                                      " +
           "\n        END                                                                                                                                                                                                                                                         " +
           "\n        /*FIM NC COMPLETO-------------------------------------------------*/                                                                                                                                                                                         " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        / /*DIVISÃO*/                                                                                                                                                                                                                                                " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n       /*INICIO AV-------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n     CASE                                                                                                                                                                                                                                                              					                                               " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            //"\n       WHEN L1.Id = 25 THEN SUM(FT.DIASABATE)       " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            //"\n       WHEN L1.hashKey = 1 THEN (SELECT sum(VOLUMEPCC) FROM #VOLUMES WHERE UnitId = C.Id and data = cast(cl1.ConsolidationDate as date)) - (SELECT isnull(sum(NAPCC),0) FROM #NAPCC WHERE UnitId = C.Id  and data = cast(cl1.ConsolidationDate as date))                                                                                                                                                                                         " +

            "\n       WHEN L1.hashKey = 1 THEN (SELECT TOP 1 SUM(Quartos) FROM VolumePcc1b WHERE ParCompany_id = C.Id AND Data = cast(CL1.ConsolidationDate as date )) " +

            "\n                                                                                                                                                                                                                                                                       					                                               " +
            "\n       WHEN CT.Id IN(1, 2) THEN SUM(CL1.WeiEvaluation)                                                                                                                                                                                                                 					                                               " +
            "\n                                                                                                                                                                                                                                                                       					                                               " +
            "\n       WHEN CT.Id IN(3)   THEN SUM(CL1.EvaluatedResult)                                                                                                                                                                                                                					                                               " +
            "\n       WHEN CT.Id IN(4) THEN SUM(A4.AM)																																																													                                               " +
            "\n     END                                                                                                                                                                                                                                                               					                                               " +
           "\n       /*FIM AV----------------------------------------------------------*/                                                                                                                                                                                          " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n      END * 100                                                                                                                                                                                                                                                      " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n      AS REAL                                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n  ,                                                                                                                                                                                                                                                                  " +
           "\n  CASE                                                                                                                                                                                                                                                               " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     WHEN(SELECT COUNT(1) FROM ParGoal G WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL) > 0 THEN                                                                                                   " +
           "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)                                         " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n     ELSE                                                                                                                                                                                                                                                            " +
           "\n         (SELECT TOP 1 ISNULL(G.PercentValue, 0) FROM ParGoal G (nolock)  WHERE G.ParLevel1_id = L1.id AND(G.ParCompany_id = C.id OR G.ParCompany_id IS NULL) AND G.EffectiveDate <= @DATAFINAL ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)                                                                      " +
           "\n  END                                                                                                                                                                                                                                                                " +
           "\n  AS META                                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n FROM ParLevel1(nolock) L1    -- (SELECT* FROM ParLevel1(nolock) WHERE ISNULL(ShowScorecard, 1) = 1) L1                                                                                                                                                                                                                                           " +
           "\n LEFT JOIN #ConsolidationLevel CL1   (nolock)                                                                                                                                                                                                                                  " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON L1.Id = CL1.ParLevel1_Id AND ISNULL(ShowScorecard, 1) = 1                                                                                                                                                                                                                                " +
           "\n LEFT JOIN ParScoreType ST  (nolock)                                                                                                                                                                                                                                           " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON ST.Id = L1.ParScoreType_Id                                                                                                                                                                                                                                " +
           "\n LEFT JOIN ParCompany C    (nolock)                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON C.Id = CL1.UnitId                                                                                                                                                                                                                                         " +
           "\n LEFT JOIN #AMOSTRATIPO4 A4    (nolock)                                                                                                                                                                                                                                        " +
           "\n         ON A4.UNIDADE = C.Id                                                                                                                                                                                                                                      " +
           "\n         AND A4.INDICADOR = L1.ID                                                                                                                                 " +
           "\n LEFT JOIN ParCompanyXStructure CS   (nolock)                                                                                                                                                                                                                                  " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CS.ParCompany_Id = C.Id                                                                                                                                                                                                                                   " +
           "\n LEFT JOIN ParStructure S     (nolock)                                                                                                                                                                                                                                         " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON S.Id = CS.ParStructure_Id                                                                                                                                                                                                                                 " +
           "\n LEFT JOIN ParStructureGroup SG     (nolock)                                                                                                                                                                                                                                   " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON SG.Id = S.ParStructureGroup_Id                                                                                                                                                                                                                            " +
           "\n LEFT JOIN ParCompanyCluster CCL   (nolock)                                                                                                                                                                                                                                    " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CCL.ParCompany_Id = C.Id  AND CCL.Active = 1                                                                                                                                                                                                                                 " +
           "\n LEFT JOIN ParCluster CL       (nolock)                                                                                                                                                                                                                                        " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CL.Id = CCL.ParCluster_Id                                                                                                                                                                                                                                 " +
           "\n LEFT JOIN ParConsolidationType CT  (nolock)                                                                                                                                                                                                                                   " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CT.Id = L1.ParConsolidationType_Id                                                                                                                                                                                                                        " +
           "\n LEFT JOIN ParLevel1XCluster L1C  (nolock)                                                                                                                                                                                                                                     " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON L1C.ParLevel1_Id = L1.Id AND L1C.ParCluster_Id = CL.Id  AND L1C.IsActive = 1   AND CCL.ParCluster_Id = L1C.ParCluster_Id                                                                                                                                                                                                 " +
           "\n LEFT JOIN ParCriticalLevel CRL   (nolock)                                                                                                                                                                                                                                     " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n        ON CRL.Id  = (select top 1 ParCriticalLevel_Id from ParLevel1XCluster aaa (nolock)  where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CL.Id AND aaa.AddDate <  @DATAFINAL)                                                                              " +
"\n  --------------------------                                                                                                                                                                                                                                                    " +
"\n  --------------------------                                                                                                                                                                                                                                                    " +
"\n                                                                                                                                                                                                                                                                                " +
//"\n  LEFT JOIN                                                                                                                                                                                                                                                                     " +
//"\n (                                                                                                                                                                                                                                                                              " +
//"\n SELECT 25 AS INDICADOR, CASE WHEN DATAP IS NULL THEN DATAV ELSE DATAP END AS DATA, *,                                                                                                                                                                                                                                                     " +
//"\n CASE WHEN ISNULL(V.DIASDEVERIFICACAO, 0) > ISNULL(P.DIASABATE, 0) THEN ISNULL(P.DIASABATE, 0) ELSE ISNULL(V.DIASDEVERIFICACAO, 0) END AS FREQ                                                                                                                                  " +
//"\n FROM                                                                                                                                                                                                                                                                           " +
//"\n (                                                                                                                                                                                                                                                                              " +
//"\n SELECT Data AS DATAP, COUNT(1) DIASABATE, SUM(Quartos) VOLUMEPCC, ParCompany_id                                                                                                                                                                                                " +
//"\n FROM VolumePcc1b(nolock)                                                                                                                                                                                                                                                       " +
//"\n WHERE Data BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                                                                                                                                                 " +
//"\n GROUP BY ParCompany_id, Data                                                                                                                                                                                                                                                   " +
//"\n ) P                                                                                                                                                                                                                                                                            " +
//"\n FULL JOIN                                                                                                                                                                                                                                                                      " +
//"\n (                                                                                                                                                                                                                                                                              " +
//"\n SELECT COUNT(1) AS DIASDEVERIFICACAO, UNITID, DATA AS DATAV                                                                                                                                                                                                                    " +
//"\n FROM(SELECT CONVERT(DATE, ConsolidationDate) DATA, cl1.UNITID FROM ConsolidationLevel1 CL1(nolock)                                                                                                                                                                             " +
//"\n WHERE ParLevel1_Id = 24                                                                                                                                                                                                                                                        " +
//"\n AND ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL                                                                                                                                                                                                                      " +
//"\n GROUP BY CONVERT(DATE, ConsolidationDate), UNITID) VT                                                                                                                                                                                                                          " +
//"\n GROUP BY DATA, UNITID                                                                                                                                                                                                                                                          " +
//"\n ) V                                                                                                                                                                                                                                                                            " +
//"\n ON V.DATAV = P.DataP                                                                                                                                                                                                                                                           " +
//"\n AND V.UnitId = P.ParCompany_id                                                                                                                                                                                                                                                 " +
//"\n                                                                                                                                                                                                                                                                                " +
//"\n ) FT                                                                                                                                                                                                                                                                           " +
//"\n ON L1.Id = FT.INDICADOR                                                                                                                                                                                                                                                        " +
"\n                                                                                                                                                                                                                                                                                " +
"\n  --------------------------                                                                                                                                                                                                                                                    " +
"\n  --------------------------                                                                                                                                                                                                                                                    " +
           "\n WHERE(ConsolidationDate BETWEEN @DATAINICIAL AND @DATAFINAL OR L1.Id = 25)                                                                                                                                                                                          " +
           "\n   AND(C.Id >= 1  OR(C.Id IS NULL AND L1.Id = 25))                                                                                                                                                                                                       " +
           "\n GROUP BY                                                                                                                                                                                                                                                            " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n      CL.Id                                                                                                                                                                                                                                                          " +
           "\n     , CL.Name                                                                                                                                                                                                                                                       " +
           "\n     , S.Id                                                                                                                                                                                                                                                          " +
           "\n     , S.Name                                                                                                                                                                                                                                                        " +
           "\n     , CL1.UnitId                                                                                                                                                                                                                                                    " +
           "\n     , C.Name                                                                                                                                                                                                                                                        " +
           "\n     , L1.IsRuleConformity                                                                                                                                                                                                                                           " +
           "\n     , L1.Id                                                                                                                                                                                                                                                         " +
           "\n     , L1.Name                                                                                                                                                                                                                                                       " +
           "\n     , CRL.Id                                                                                                                                                                                                                                                        " +
           "\n     , CRL.Name                                                                                                                                                                                                                                                      " +
           //"\n     , L1C.Points                                                                                                                                                                                                                                                    " +
           "\n     , ST.Name                                                                                                                                                                                                                                                       " +
           "\n     , CT.Id                                                                                                                                                                                                                                                         " +
           "\n     , L1.HashKey                                                                                                                                                                                                                                                    " +
           //"\n     , C.Id   , CL1.ConsolidationDate,FT.DATA, FT.PARCOMPANY_ID                                                                                                                                                                                                                                                        " +
           "\n     , C.Id   , CL1.ConsolidationDate                                                                                                                                                                                                                                                       " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n ) SCORECARD                                                                                                                                                                                                                                                         " +
           "\n                                                                                                                                                                                                                                                                     " +
           "\n ) FIM                                                                                                                                                                                                                                                               " +
           "\n                                                                                                                                                                                                                                                                     " +
"\n    ) SC                                                                                                                                                                                                                                                               					                                               " +
"\n WHERE ParCompanyId <> 2 " +
"\n GROUP BY                 " +
"\n   Cluster                " +
"\n   , ClusterName          " +
"\n   , Regional             " +
"\n   , RegionalName         " +
"\n   , ParCompanyId         " +
"\n   , ParCompanyName       " +
"\n   , TipoIndicador        " +
"\n   , TipoIndicadorName    " +
"\n   , Level1Id             " +
"\n   , Level1Name           " +
"\n   , Criterio             " +
"\n   , CriterioName         " +
"\n   , TipoScore            " +
"\n   , month(mesData) + '-' + month(mesData)              " +

"\n    ,CAST(mesData AS DATE)   ORDER BY 11, 10                                                                                                                                                                                                                                                    					                                               " +
"\n    DROP TABLE #AMOSTRATIPO4                                                                                                                                                                                                                                                                                                            " +
"\n    DROP TABLE #VOLUMES	                                                                                                                                                                                                                                                                                                               " +
"\n    DROP TABLE #DIASVERIFICACAO                                                                                                                                                                                                                                                                                                         " +
"\n    DROP TABLE #NAPCC	DROP TABLE #FREQ																																																														                                                       " +
"\n  																																																																						                                               " +
"\n    SET @I = @I + 1																																																																		                                               " +
"\n  																																																																						                                               " +
"\n  END                                                                                                                                                                                                                                                                                                                                   " +
"\n  																																																																						                                               " +
"\n     ";

            #endregion

            #region Queryes Trs Meio

            var where = string.Empty;
            where += "";


            var query4 =

                 "SELECT                     " +

                "\n    0 as level1Id                " +
                "\n   ,'a1' as Level1Name             " +
                "\n   ,'a2' as ChartTitle             " +
                "\n   ,0 as UnidadeId               " +
                "\n   ,'a4' as UnidadeName            " +
                "\n   ,20.0 AS procentagemNc          " +
                "\n   ,10.0 AS Meta                   " +
                "\n   ,CAST(ISNULL(case when sum(av) is null or sum(av) = 0 then '0'else cast(round(cast(case when isnull(sum(Pontos),100) = 0 or isnull(sum(PontosAtingidos),100) = 0 then 0 else (ISNULL(sum(PontosAtingidos),100) / isnull(sum(Pontos),100))*100  end  as decimal (10,1)),2) as varchar) end, 0) AS DECIMAL(10,2)) AS nc        " +
                "\n   ,50.0 as av                     " +
                "\n   ,mesData as [date]                 " +


                  "\n FROM ParStructure Reg " +
                    "\n  LEFT JOIN ParCompanyXStructure CS " +
                    "\n  ON CS.ParStructure_Id = Reg.Id " +
                    "\n  left join ParCompany C " +
                    "\n  on C.Id = CS.ParCompany_Id " +
                    "\n  left join ParLevel1 P1 " +
                    "\n  on 1=1 AND ISNULL(P1.ShowScorecard, 1) = 1" +

                    "\n  LEFT JOIN ParGroupParLevel1XParLevel1 PP " +
                    "\n  ON PP.ParLevel1_Id = P1.Id " +
                    "\n  LEFT JOIN ParGroupParLevel1 PP1 " +
                    "\n  ON PP.ParGroupParLevel1_Id = PP1.Id " +

                    "\n INNER JOIN #SCORE S " +
                    "\n  on C.Id = S.ParCompany_Id  and S.Level1Id = P1.Id " +

                    "\n  WHERE 1 = 1 " +
                    "\n  AND Reg.Active = 1 and Reg.ParStructureGroup_Id = 3  and PP1.Name is not null" +
                    "\n group by mesData ORDER BY 10";

            #endregion

            //db.Database.ExecuteSqlCommand(query);

            string grandeQuery = query + " " + query4;

            var result = new List<RetornoGenerico>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                result = factory.SearchQuery<RetornoGenerico>(grandeQuery).ToList();
            }

            //var result1 = result.Where(r => r.QUERY == 1).ToList();
            //var result2 = result.Where(r => r.QUERY == 2).ToList();
            //var result3 = result.Where(r => r.QUERY == 3).ToList();
            // var result4 = result.Where(r => r.QUERY == 4).ToList();
            //var queryRowsBody = result.Where(r => r.QUERY == 6).ToList();

            var retorno = result;



            return retorno;
        }

        [HttpPost]
        [Route("GetGraficoHistoricoModalGeral")]
        public List<RetornoGenerico> GetGraficoHistoricoModalGeral([FromBody] DTO.DataCarrierFormularioNew form)
        {

            string query = "";

            query = getQueryHistorioGeral(form, true, 5);

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno2 = factory.SearchQuery<RetornoGenerico>(query).ToList();
            }

            //GetMockHistoricoModal();
            return retorno2;
        }

        [HttpPost]
        [Route("GetGraficoHistoricoModalUnidade")]
        public List<RetornoGenerico> GetGraficoHistoricoModalUnidade([FromBody] DTO.DataCarrierFormularioNew form)
        {

            string query = "";

            query = getQueryHistorioGeral(form, true, 4);

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno2 = factory.SearchQuery<RetornoGenerico>(query).ToList();
            }

            //GetMockHistoricoModal();
            return retorno2;
        }

        private static string getQueryHistoricoTarefa(DTO.DataCarrierFormularioNew form, bool? tipoVisao = false, int? nivel = 1) // || tipoVisao => 0: Listagem / 1: Evolutivo  || nivel =>  1: indicador / 2: Monitoramento / 3: Tarefa / 4: Unidade / 5: JBS 
        {

            #region Filtros

            var titulo = "Histórico da Tarefa";

            var Wmodulo = "";
            var Wprocesso = "";
            var Wregional = "";
            var Wnivelcritico = "";
            var Wfuncao = "";


            // Módulo

            if (form.Param["clusterGroupId"] != null && form.Param["clusterGroupId"].ToString() != "")
            {
                Wmodulo += " AND ParCluster_ID IN (SELECT ID FROM ParCluster WITH (NOLOCK) WHERE ParClusterGroup_Id = " + form.Param["clusterGroupId"].ToString() + ") ";
            }

            // Processo

            if (form.ParCluster_Ids.Length > 0)
            {
                Wprocesso += " AND ParCluster_ID IN (" + string.Join(",", form.ParCluster_Ids) + ") ";
            }

            // Regional

            if (form.ParStructure_Ids.Length > 0)
            {
                Wregional += " AND ParStructure_id  IN (" + string.Join(",", form.ParStructure_Ids) + ") ";
            }
            else if (form.Param["structureId"] != null && form.Param["structureId"].ToString() != "")
            {
                Wregional += " AND ParStructure_id  IN (" + form.Param["structureId"].ToString() + ") ";
            }

            // Nivel Crítico

            if (form.ParCriticalLevel_Ids.Length > 0)
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.ParCriticalLevel_Ids) + ") ";
            }
            else if (form.Param["criticalLevelId"] != null && form.Param["criticalLevelId"].ToString() != "")
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + form.Param["criticalLevelId"].ToString() + ") ";
            }

            // Função

            if (form.ParLevel1Group_Ids.Length > 0)
            {
                Wfuncao += " AND Indicador IN (SELECT distinct ParLevel1_id  FROM ParGroupParLevel1XParLevel1 WHERE IsActive = 1 AND ParGroupParLevel1_Id IN (" + string.Join(",", form.ParLevel1Group_Ids) + "))"; // " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.criticalLevelIdArr) + ") ";
            }


            #endregion

            var script = "";
            var SQLcentro = "";

            SQLcentro = getQuery(form, nivel);

            #region Status do Indicador: Fora ou Dentro da Meta

            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 1) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 2) // Indicadores Fora Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }

            #endregion

            #region Indicadores com Acao Concluída: Sim ou Não

            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 1)
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 2) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }
            #endregion


            if (tipoVisao == false) // 0: Listagem / 1: Evolutivo 
            { // Considero Dimensões
                #region ScriptLista
                script += @"

            " + SQLcentro + @"

            SELECT 
            	Indicador AS level1Id
               ,IndicadorName AS Level1Name
               ,Monitoramento AS level2Id
               ,MonitoramentoName AS Level2Name
               ,Tarefa AS level3Id
               ,TarefaName AS level3Name
               ,'" + titulo + $@"' AS ChartTitle
		       ,Unidade	AS UnidadeId			
		       ,UnidadeName AS UnidadeName
               ,1 IsTarefa
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS procentagemNc
               ,ConsolidationDate as [date]
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,sum(ISNULL(Meta,0)) AS Meta
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                Indicador 
               ,IndicadorName 
               ,Monitoramento
               ,MonitoramentoName 
               ,Tarefa 
               ,TarefaName 
		       ,Unidade	
		       ,UnidadeName 
               ,ConsolidationDate
            ORDER BY 7 
            ";
                #endregion
            }
            else if (tipoVisao == true)
            { // Desconsidero Dimensões
                #region ScriptGrafico
                script += @"

            " + SQLcentro + @"

            SELECT 
               '" + titulo + $@"' AS ChartTitle
               ,1 IsTarefa
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS procentagemNc
               ,ConsolidationDate as [date]
			   ,max(UnidadeName)UnidadeName
			   ,max(IndicadorName) level1Name
			   ,max(MonitoramentoName) level2Name
			   ,max(TarefaName) level3Name
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,sum(ISNULL(Meta,0)) AS Meta
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
                GROUP BY 
                    ConsolidationDate
            ORDER BY 4 
            ";
                #endregion
            }



            return script;

        }

        private static string getQueryHistoricoMonitoramento(DTO.DataCarrierFormularioNew form, bool? tipoVisao = false, int? nivel = 1) // || tipoVisao => 0: Listagem / 1: Evolutivo  || nivel =>  1: indicador / 2: Monitoramento / 3: Tarefa / 4: Unidade / 5: JBS 
        {


            #region Filtros

            var titulo = "Histórico do Monitoramento";

            var Wmodulo = "";
            var Wprocesso = "";
            var Wregional = "";
            var Wnivelcritico = "";
            var Wfuncao = "";


            // Módulo

            if (form.Param["clusterGroupId"] != null && form.Param["clusterGroupId"].ToString() != "")
            {
                Wmodulo += " AND ParCluster_ID IN (SELECT ID FROM ParCluster WITH (NOLOCK) WHERE ParClusterGroup_Id = " + form.Param["clusterGroupId"].ToString() + ") ";
            }

            // Processo

            if (form.ParCluster_Ids.Length > 0)
            {
                Wprocesso += " AND ParCluster_ID IN (" + string.Join(",", form.ParCluster_Ids) + ") ";
            }

            // Regional

            if (form.ParStructure_Ids.Length > 0)
            {
                Wregional += " AND ParStructure_id  IN (" + string.Join(",", form.ParStructure_Ids) + ") ";
            }
            else if (form.Param["structureId"] != null && form.Param["structureId"].ToString() != "")
            {
                Wregional += " AND ParStructure_id  IN (" + form.Param["structureId"].ToString() + ") ";
            }

            // Nivel Crítico

            if (form.ParCriticalLevel_Ids.Length > 0)
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.ParCriticalLevel_Ids) + ") ";
            }
            else if (form.Param["criticalLevelId"] != null && form.Param["criticalLevelId"].ToString() != "")
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + form.Param["criticalLevelId"].ToString() + ") ";
            }

            // Função

            if (form.ParLevel1Group_Ids.Length > 0)
            {
                Wfuncao += " AND Indicador IN (SELECT distinct ParLevel1_id  FROM ParGroupParLevel1XParLevel1 WHERE IsActive = 1 AND ParGroupParLevel1_Id IN (" + string.Join(",", form.ParLevel1Group_Ids) + "))"; // " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.criticalLevelIdArr) + ") ";
            }

            #endregion

            var script = "";
            var SQLcentro = "";

            SQLcentro = getQuery(form, nivel);

            #region Status do Indicador: Fora ou Dentro da Meta

            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 1) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 2) // Indicadores Fora Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }

            #endregion

            #region Indicadores com Acao Concluída: Sim ou Não

            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 1)
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 2) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }
            #endregion

            var D = getDimensaoData(form, "ConsolidationDate");

            if (tipoVisao == false) // 0: Listagem / 1: Evolutivo 
            { // Considero Dimensões
                #region ScriptLista
                script += @"

            " + SQLcentro + @"

            SELECT 
            	Indicador AS level1Id
               ,IndicadorName AS Level1Name
               ,Monitoramento AS level2Id
               ,MonitoramentoName AS Level2Name
               ,'" + titulo + $@"' AS ChartTitle
		       ,Unidade	AS UnidadeId			
		       ,UnidadeName AS UnidadeName
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS procentagemNc
               ,ConsolidationDate as [date]
               ,1 IsMonitoramento
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,sum(ISNULL(Meta,0)) AS Meta
	FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                Indicador 
               ,IndicadorName 
               ,Monitoramento
               ,MonitoramentoName 
		       ,Unidade	
		       ,UnidadeName 
               ,ConsolidationDate
ORDER BY 7 
            ";
                #endregion
            }
            else if (tipoVisao == true)
            { // Desconsidero Dimensões
                #region ScriptGrafico
                script += @"

            " + SQLcentro + @"

            SELECT 
               '" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS procentagemNc
               ,{D.queryDimensao}{D.nomeAlias}
               ,1 IsMonitoramento
			   ,max(UnidadeName)UnidadeName
			   ,max(IndicadorName) level1Name
			   ,max(MonitoramentoName) level2Name
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,sum(ISNULL(Meta,0)) AS Meta
	FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
               {D.queryDimensao}
ORDER BY 3 
            ";
                #endregion
            }



            return script;

        }

        private static string getQueryHistorioIndicador(DTO.DataCarrierFormularioNew form, bool? tipoVisao = false, int? nivel = 1) // || tipoVisao => 0: Listagem / 1: Evolutivo  || nivel =>  1: indicador / 2: Monitoramento / 3: Tarefa / 4: Unidade / 5: JBS
        {

            #region Filtros

            var titulo = "Histórico do Indicador";

            var Wmodulo = "";
            var Wprocesso = "";
            var Wregional = "";
            var Wnivelcritico = "";
            var Wfuncao = "";


            // Módulo

            if (form.Param["clusterGroupId"] != null && form.Param["clusterGroupId"].ToString() != "")
            {
                Wmodulo += " AND ParCluster_ID IN (SELECT ID FROM ParCluster WITH (NOLOCK) WHERE ParClusterGroup_Id = " + form.Param["clusterGroupId"].ToString() + ") ";
            }

            // Processo

            if (form.ParCluster_Ids.Length > 0)
            {
                Wprocesso += " AND ParCluster_ID IN (" + string.Join(",", form.ParCluster_Ids) + ") ";
            }

            // Regional

            if (form.ParStructure_Ids.Length > 0)
            {
                Wregional += " AND ParStructure_id  IN (" + string.Join(",", form.ParStructure_Ids) + ") ";
            }
            else if (form.Param["structureId"] != null && form.Param["structureId"].ToString() != "")
            {
                Wregional += " AND ParStructure_id  IN (" + form.Param["structureId"].ToString() + ") ";
            }

            // Nivel Crítico

            if (form.ParCriticalLevel_Ids.Length > 0)
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.ParCriticalLevel_Ids) + ") ";
            }
            else if (form.Param["criticalLevelId"] != null && form.Param["criticalLevelId"].ToString() != "")
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + form.Param["criticalLevelId"].ToString() + ") ";
            }

            // Função

            if (form.ParLevel1Group_Ids.Length > 0)
            {
                Wfuncao += " AND Indicador IN (SELECT distinct ParLevel1_id  FROM ParGroupParLevel1XParLevel1 WHERE IsActive = 1 AND ParGroupParLevel1_Id IN (" + string.Join(",", form.ParLevel1Group_Ids) + "))"; // " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.criticalLevelIdArr) + ") ";
            }

            #endregion


            var script = "";
            var SQLcentro = "";

            SQLcentro = getQuery(form, nivel);

            #region Status do Indicador: Fora ou Dentro da Meta
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 1) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 2) // Indicadores Fora Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }

            #endregion

            #region Indicadores com Acao Concluída: Sim ou Não

            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 1)
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 2) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }
            #endregion

            var D = getDimensaoData(form, "ConsolidationDate");

            if (tipoVisao == false) // 0: Listagem / 1: Evolutivo 
            { // Considero Dimensões
                #region ScriptLista
                script += @"

            " + SQLcentro + @"

            SELECT 
            	Indicador AS level1Id
               ,IndicadorName AS Level1Name
               ,'" + titulo + $@"' AS ChartTitle
		       ,Unidade	AS UnidadeId			
		       ,UnidadeName AS UnidadeName
               ,1 IsIndicador
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS procentagemNc
               ,ConsolidationDate as [date]
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,sum(ISNULL(Meta,0)) AS Meta
	FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                Indicador 
               ,IndicadorName 
		       ,Unidade	
		       ,UnidadeName 
               ,ConsolidationDate
    ORDER BY 7 
            ";
                #endregion
            }
            else if (tipoVisao == true)
            { // Desconsidero Dimensões
                #region ScriptGrafico
                script += @"

            " + SQLcentro + @"

            SELECT 
               '" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS procentagemNc
               ,{D.queryDimensao}{D.nomeAlias}
               ,1 IsIndicador
			   ,max(UnidadeName)UnidadeName
			   ,max(IndicadorName) level1Name
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,sum(ISNULL(Meta,0)) AS Meta
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
               {D.queryDimensao}
ORDER BY 3 
            ";
                #endregion
            }



            return script;
        }

        private static string getQueryHistorioGeral(DTO.DataCarrierFormularioNew form, bool? tipoVisao = false, int? nivel = 1) // || tipoVisao => 0: Listagem / 1: Evolutivo  || nivel =>  1: indicador / 2: Monitoramento / 3: Tarefa / 4: Unidade / 5: JBS
        {

            #region Filtros

            var titulo = "Histórico Consolidado";

            var Wmodulo = "";
            var Wprocesso = "";
            var Wregional = "";
            var Wnivelcritico = "";
            var Wfuncao = "";


            // Módulo

            if (form.Param["clusterGroupId"] != null && form.Param["clusterGroupId"].ToString() != "")
            {
                Wmodulo += " AND ParCluster_ID IN (SELECT ID FROM ParCluster WITH (NOLOCK) WHERE ParClusterGroup_Id = " + form.Param["clusterGroupId"].ToString() + ") ";
            }

            // Processo

            if (form.ParCluster_Ids.Length > 0)
            {
                Wprocesso += " AND ParCluster_ID IN (" + string.Join(",", form.ParCluster_Ids) + ") ";
            }

            // Regional

            if (form.ParStructure_Ids.Length > 0)
            {
                Wregional += " AND ParStructure_id  IN (" + string.Join(",", form.ParStructure_Ids) + ") ";
            }
            else if (form.Param["structureId"] != null && form.Param["structureId"].ToString() != "")
            {
                Wregional += " AND ParStructure_id  IN (" + form.Param["structureId"].ToString() + ") ";
            }

            // Nivel Crítico

            if (form.ParCriticalLevel_Ids.Length > 0)
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.ParCriticalLevel_Ids) + ") ";
            }
            else if (form.Param["criticalLevelId"] != null && form.Param["criticalLevelId"].ToString() != "")
            {
                Wnivelcritico += " AND ParCriticalLevel_Id  IN (" + form.Param["criticalLevelId"].ToString() + ") ";
            }

            // Função

            //if (form.groupParLevel1IdArr.Length > 0)
            //{
            //    Wfuncao += " AND Indicador IN (SELECT id FROM ParGroupParLevel1XParLevel1 WHERE IsActive = 1 AND ParGroupParLevel1_Id IN (" + string.Join(",", form.groupParLevel1IdArr) + "))"; // " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.criticalLevelIdArr) + ") ";
            //}

            #endregion

            var script = "";
            var SQLcentro = "";

            SQLcentro = getQuery(form, nivel);

            #region Status do Indicador: Fora ou Dentro da Meta
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 1) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            if (form.Param["statusIndicador"] != null && int.Parse(form.Param["statusIndicador"]?.ToString()) == 2) // Indicadores Fora Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryStatusIndicador(form, int.Parse(form.Param["statusIndicador"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }

            #endregion

            #region Indicadores com Acao Concluída: Sim ou Não

            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 1)
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            if (form.Param["createActionPlane"] != null && int.Parse(form.Param["createActionPlane"]?.ToString()) == 2) // Indicadores Dentro Da Meta
            {
                SQLcentro += @"";
                SQLcentro += @"";
                SQLcentro += getQueryPAConcluidos(form, int.Parse(form.Param["createActionPlane"]?.ToString()));
            }
            else
            {
                SQLcentro += @"";
            }
            #endregion

            var D = getDimensaoData(form, "ConsolidationDate");

            if (tipoVisao == false) // 0: Listagem / 1: Evolutivo 
            { // Considero Dimensões
                #region ScriptLista
                script += @"

            " + SQLcentro + @"

            SELECT 
            	Indicador AS level1Id
               ,IndicadorName AS Level1Name
               ,'" + titulo + $@"' AS ChartTitle
		       ,Unidade	AS UnidadeId			
		       ,UnidadeName AS UnidadeName
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS procentagemNc
               ,ConsolidationDate as [date]
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,AVG(ISNULL(Meta,0)) AS Meta
	        FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
                Indicador 
               ,IndicadorName 
		       ,Unidade	
		       ,UnidadeName 
               ,ConsolidationDate
            ORDER BY 7 
            ";
                #endregion
            }
            else if (tipoVisao == true)
            { // Desconsidero Dimensões
                #region ScriptGrafico
                script += @"

            " + SQLcentro + @"

            SELECT 
               '" + titulo + $@"' AS ChartTitle
               ,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS procentagemNc
               ,{D.queryDimensao}{D.nomeAlias}
			   ,max(UnidadeName) UnidadeName
		       ,sum(ISNULL(AVComPeso,0)) AS AVComPeso
		       ,sum(ISNULL(NCComPeso,0)) AS NCComPeso
		       ,sum(ISNULL(AV,0)) AS AV
		       ,sum(ISNULL(NC,0)) AS NC
		       ,AVG(ISNULL(Meta,0)) AS Meta
	FROM #CUBO Cubo WITH (NOLOCK)
            WHERE 1=1 
                { Wmodulo }
                { Wprocesso }
                { Wregional }
                { Wnivelcritico }
            GROUP BY 
               {D.queryDimensao}
ORDER BY 3 
            ";
                #endregion
            }



            return script;
        }

        [HttpPost]
        [Route("listaResultados")]
        public List<RetornoGenerico> listaResultados([FromBody] DTO.DataCarrierFormularioNew form)
        {



            string query = "";

            if (form.Param["level3Id"] != null && form.Param["level3Id"].ToString() != "")
            {
                query = getQueryHistoricoTarefa(form, false, 3);
            }
            else if (form.Param["level2Id"] != null && form.Param["level2Id"].ToString() != "")
            {
                query = getQueryHistoricoMonitoramento(form, false, 2);
            }
            else
            {
                query = getQueryHistorioIndicador(form, false, 1);
            }

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno3 = factory.SearchQuery<RetornoGenerico>(query).ToList();
            }

            //if(retornaSomenteAv == true)
            //{
            //retorno3 = retorno3.Where(r => r.av > 0).ToList();
            //}

            //GetMockListaResultados();
            return retorno3;
        }

        [HttpPost]
        [Route("listaResultadosAcoesConcluidas")]
        public List<RetornoGenerico> listaResultadosAcoesConcluidas([FromBody] DTO.DataCarrierFormularioNew form)
        {

            string query = "";

            if (form.Param["level3Id"] != null && form.Param["level3Id"].ToString() != "")
            {
                query = getQueryHistoricoTarefa(form, false, 3);
            }
            else if (form.Param["level2Id"] != null && form.Param["level2Id"].ToString() != "")
            {
                query = getQueryHistoricoMonitoramento(form, false, 2);
            }
            else
            {
                query = getQueryHistorioIndicador(form, false, 1);
            }

            using (Factory factory = new Factory("DefaultConnection"))
            {
                retorno4 = factory.SearchQuery<RetornoGenerico>(query).ToList();
            }

            return retorno4;
        }

        [HttpPost]
        [Route("listaAcoesIndicador")]
        public List<JObject> listaAcoesIndicador([FromBody] DTO.DataCarrierFormularioNew form)
        {
            //string sql = "select sei la o que";

            //var result = QueryNinja(conexao, sql);

            var result5 = new List<JObject>();
            result5.Add(new JObject { });

            return result5;
        }

        #region Mocks

        private void GetMockResultadosPeriodo()
        {

            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC nas Operações de Esfola - LIN", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC nas Operações de Esfola - CGR", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC nas Operações de Esfola - MTZ", UnidadeName = "Matriz" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC CEP Desossa - LIN", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC CEP Desossa - CGR", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC CEP Desossa - MTZ", UnidadeName = "Matriz" });

        }

        private void GetMockResultadosPeriodo2()
        {

            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Sangria", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Sangria - LIN", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Sangria", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Sangria - CGR", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Sangria", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Sangria - MTZ", UnidadeName = "Matriz" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Esfola da Pata TRS Esquerda", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Esfola da Pata TRS Esquerda - LIN", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Esfola da Pata TRS Esquerda", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Esfola da Pata TRS Esquerda - CGR", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Esfola da Pata TRS Esquerda", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Esfola da Pata TRS Esquerda - MTZ", UnidadeName = "Matriz" });

        }

        private void GetMockResultadosPeriodo3()
        {

            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de contato - quarto esfolado / não esfolado", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Ausência de contato - quarto esfolado / não esfolado - LIN", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de contato - quarto esfolado / não esfolado", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Ausência de contato - quarto esfolado / não esfolado - CGR", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de contato - quarto esfolado / não esfolado", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Ausência de contato - quarto esfolado / não esfolado - MTZ", UnidadeName = "Matriz" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de perfurações / contaminação", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Ausência de perfurações / contaminação - LIN", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de perfurações / contaminação", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Ausência de perfurações / contaminação - CGR", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de perfurações / contaminação", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "Ausência de perfurações / contaminação - MTZ", UnidadeName = "Matriz" });

        }

        private void GetMockResultadosPeriodo4()
        {

            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC nas Operações de Esfola", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC nas Operações de Esfola", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC nas Operações de Esfola", UnidadeName = "Matriz" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC CEP Desossa", UnidadeName = "Lins" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC CEP Desossa", UnidadeName = "Campo Grande II" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, IndicadorUnidade = "(%) NC CEP Desossa", UnidadeName = "Matriz" });

        }

        private void GetMockHistoricoModal()
        {
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 100, limiteSuperior = 200, nc = 250, date = DateTime.Now });
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 100, limiteSuperior = 200, nc = 200, date = DateTime.Now });
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 100, limiteSuperior = 200, nc = 150, date = DateTime.Now });
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 100, limiteSuperior = 200, nc = 50, date = DateTime.Now });
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 100, limiteSuperior = 200, nc = 350, date = DateTime.Now });
        }

        private void GetMockListaResultados()
        {
            //retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 120, Real = 10 });
            //retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 150, Real = 10 });
            //retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 200, Real = 10 });
            //retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 300, Real = 10 });
            //retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 400, Real = 10 });
        }

        #endregion

        private static string GetUserUnits(int UserId)
        {
            using (var db = new SgqDbDevEntities())
            {

                if (db.UserSgq.Find(UserId).ShowAllUnits == true)
                {
                    return string.Join(",", db.ParCompany.Where(r => r.IsActive).Select(r => r.Id).ToList());
                }
                else
                {
                    return string.Join(",", db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == UserId).Select(r => r.ParCompany_Id).ToList());
                }
            }
        }

        [HttpPost]
        [Route("GetAcoesByDate")]
        public List<AcoesConcluidas> GetAcoesByDate(JObject filtro)
        {
            try
            {
                var BancoPA = "PlanoDeAcao";
                //const BancoPA = "PlanoDeAcaoUSA2";  

                dynamic filtroDyn = filtro;
                var retorno = new List<AcoesConcluidas>();
                string inicio = filtroDyn.startDate;
                string fim = filtroDyn.endDate;
                int level = filtroDyn.isLevel;
                var dataInicio = Guard.ParseDateToSqlV2(inicio, Guard.CultureCurrent.BR).ToString("yyyyMMdd");
                var dataFim = Guard.ParseDateToSqlV2(fim, Guard.CultureCurrent.BR).ToString("yyyyMMdd");
                //var dataInicio = Guard.ParseDateToSqlV2(inicio, Guard.CultureCurrent.EUA).ToString("yyyyMMdd");
                //var dataFim = Guard.ParseDateToSqlV2(fim, Guard.CultureCurrent.EUA).ToString("yyyyMMdd");

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

                var query = @" 
                Use " + BancoPA + @"
                DECLARE @dataFim_ date = '" + dataFim + @"'
  
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


                using (Factory factory = new Factory("DefaultConnection"))
                {
                    retorno = factory.SearchQuery<AcoesConcluidas>(query).ToList();
                }

                return retorno;
            }
            catch (Exception)
            {
                return new List<AcoesConcluidas>();
            }

        }

        [HttpPost]
        [Route("GetAcoesIndicador")]
        public List<Dominio.Pa_Acao> GetAcoesIndicador(JObject filtro)
        {
            try
            {

                var BancoPA = "PlanoDeAcao";
                //const BancoPA = "PlanoDeAcaoUSA2"; 

                dynamic filtroDyn = filtro;
                var retorno = new List<Dominio.Pa_Acao>();
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

                var query = @"
                                Use " + BancoPA + @"
                                SELECT
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

                using (Factory factory = new Factory("DefaultConnection"))
                {
                    retorno = factory.SearchQuery<Dominio.Pa_Acao>(query).ToList();
                }

                return retorno;
            }
            catch (Exception)
            {
                return new List<Dominio.Pa_Acao>();
            }

        }

        public class AcoesConcluidas
        {
            public DateTime Data { get; set; }
            public int QteConcluidas { get; set; }
            public string _data { get { return Data.ToString("dd/MM/yyyy"); } }
        }

        private static string getQuery(DTO.DataCarrierFormularioNew form, int? nivel)
        {

            var Wshift = "";
            var Wunidade = "";
            var Windicador = "";
            var Wmonitoramento = "";
            var Wtarefa = "";
            var Wfuncao = "";

            var WunidadeAcesso = GetUserUnits(int.Parse(form.Param["auditorId"]?.ToString()));

            if (nivel == 1 || nivel == 2 || nivel == 3)
            {
                // Indicador
                if (form.ParLevel1_Ids.Length > 0 && form.ParLevel1_Ids != null)
                {
                    Windicador = " AND CL1.ParLevel1_id IN (" + string.Join(",", form.ParLevel1_Ids) + ")";
                }
                else if (form.Param["level1Id"] != null && form.Param["level1Id"].ToString() != "")
                {
                    Windicador = " AND CL1.ParLevel1_id IN (" + form.Param["level1Id"].ToString() + ")";
                }
            }
            // Monitoramento
            if (nivel == 2 || nivel == 3)
            {
                if (form.ParLevel2_Ids.Length > 0 && form.ParLevel2_Ids != null)
                {
                    Wmonitoramento = " AND CL2.ParLevel2_id IN (" + string.Join(",", form.ParLevel2_Ids) + ")";
                }
                else if (form.Param["level2Id"] != null && form.Param["level2Id"].ToString() != "")
                {
                    Wmonitoramento = " AND CL2.ParLevel2_id IN (" + form.Param["level2Id"].ToString() + ")";
                }
            }
            // Tarefa
            if (nivel == 3)
            {
                if (form.ParLevel3_Ids.Length > 0 && form.ParLevel3_Ids != null)
                {
                    Wtarefa = " AND R3.ParLevel3_Id IN (" + string.Join(",", form.ParLevel3_Ids) + ")";
                }
                else if (form.Param["level3Id"] != null && form.Param["level3Id"].ToString() != "")
                {
                    Wtarefa = " AND R3.ParLevel3_Id IN (" + form.Param["level3Id"].ToString() + ")";
                }
            }

            if (nivel == 1 || nivel == 2 || nivel == 3 || nivel == 4)
            {
                // Turno

                if (form.Param["shift"] != null && form.Param["shift"].ToString() != "")
                {
                    Wshift = $@" AND CL1.shift = " + form.Param["shift"].ToString();
                }

                // Função

                if (form.ParLevel1Group_Ids.Length > 0)
                {
                    Wfuncao += " AND CL1.ParLevel1_id IN (SELECT distinct ParLevel1_id FROM ParGroupParLevel1XParLevel1 WHERE IsActive = 1 AND ParGroupParLevel1_Id IN (" + string.Join(",", form.ParLevel1Group_Ids) + "))"; // " AND ParCriticalLevel_Id  IN (" + string.Join(",", form.criticalLevelIdArr) + ") ";
                }

                // Unidade

                if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids != null)
                {
                    Wunidade = " AND CL1.UnitId IN (" + string.Join(",", form.ParCompany_Ids) + ")";
                }
                else if (form.Param["unitId"] != null && form.Param["unitId"].ToString() != "")
                {
                    Wunidade = " AND CL1.UnitId IN (" + form.Param["unitId"].ToString() + ")";
                }
            }


            var Query = "";

            if (nivel == 1 || nivel == 4 || nivel == 5)
            {

                #region Consolidação Por JBS, Unidade e Indicador

                Query = $@"
            
            DECLARE @DATEINI DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
            DECLARE @DATEFIM DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'
            
             DECLARE @dataFim_ date = @DATEFIM
              
             DECLARE @dataInicio_ date = @DATEINI
            SET @dataInicio_ = @DATEINI
             
             -- DROP TABLE #DATA
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
            
            
            -- DROP TABLE #VOLUMES
            
            SELECT V.ParCompany_id,V.Data
            	, SUM(V.Quartos) AS VOLUMEPCC
            INTO #VOLUMES
            FROM VolumePcc1b V WITH (NOLOCK)
            WHERE 1=1 
            GROUP BY V.ParCompany_id,V.Data
            
            
            	-- DROP TABLE #AMOSTRA4
            
	SELECT
		UNIDADE
	   ,INDICADOR
       ,[SHIFT]
       ,[PERIOD]
       ,DATA
	   ,COUNT(1) AM
	   ,SUM(DEF_AM) DEF_AM
INTO #AMOSTRA4
FROM (SELECT
			CAST(C2.CollectionDate AS DATE) AS DATA
		   ,C.Id AS UNIDADE
		   ,C2.ParLevel1_Id AS INDICADOR
           ,C2.[SHIFT]
           ,C2.[PERIOD]
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
                ,C2.[SHIFT]
                ,C2.[PERIOD]
				,EvaluationNumber
				,Sample
				,CAST(CollectionDate AS DATE)) TAB
	GROUP BY UNIDADE
			,INDICADOR
            ,DATA
            ,[SHIFT]
            ,[PERIOD]
            
            -- NA
            -- DROP TABLE #NA
            
            SELECT CL2.CollectionDate,CL2.UnitId,COUNT(distinct CL2.id) AS NA
            	INTO #NA
            	FROM CollectionLevel2 CL2 WITH (NOLOCK)
            	LEFT JOIN Result_Level3 CL3 WITH (NOLOCK)
            		ON CL3.CollectionLevel2_Id = CL2.Id
            	WHERE CONVERT(DATE, CL2.CollectionDate) between CONVERT(DATE,@DATEINI) and CONVERT(DATE,@DATEFIM)
            	AND CL2.ParLevel1_Id IN (SELECT 
            			id
            		FROM Parlevel1 WITH (NOLOCK)
            		WHERE Hashkey = 1 )
            	AND CL3.IsNotEvaluate = 1
            	GROUP BY CL2.CollectionDate,CL2.UnitId
            HAVING COUNT(DISTINCT CL2.id) > 1
            
            -- C1
            -- DROP TABLE #ConsolidationLevel
            
            SELECT 
            	CL1.id,
            	CL1.ConsolidationDate,
            	CL1.UnitId,
            	CL1.ParLevel1_Id,
            	CL1.[Shift],
            	CL1.[Period],
            	CL1.DefectsResult,
            	CL1.WeiDefects,
            	CL1.EvaluatedResult,
            	CL1.WeiEvaluation,
            	CL1.EvaluateTotal,
            	CL1.TotalLevel3WithDefects,
            	CL1.DefectsTotal
            INTO #ConsolidationLevel
            FROM ConsolidationLevel1 CL1 WITH (NOLOCK) 
            WHERE 1=1 
            AND CL1.ConsolidationDate BETWEEN @DATEINI AND @DATEFIM
            AND CL1.UnitId IN ({ WunidadeAcesso }) 
            " + Wshift + @"
            " + Wunidade + @"
            " + Wfuncao + @"
            " + Windicador + @"
            AND CL1.ParLevel1_Id != 43
            AND CL1.ParLevel1_Id != 42
            
            CREATE INDEX IDX_HashConsolidationLevel ON #ConsolidationLevel (ConsolidationDate,UnitId,ParLevel1_Id); 
            CREATE INDEX IDX_HashConsolidationLevel_level1 ON #ConsolidationLevel (ConsolidationDate,ParLevel1_Id); 
            CREATE INDEX IDX_HashConsolidationLevel_Unitid ON #ConsolidationLevel (ConsolidationDate,UnitId); 
            CREATE INDEX IDX_HashConsolidationLevel_id ON #ConsolidationLevel (id); 
            
            
            -- CUBO
            -- DROP TABLE #CUBO
            SELECT 
            	 CL.id						AS ParCluster_ID
            	,CL.Name					AS ParCluster_Name
            	,CS.ParStructure_id			AS ParStructure_id
            	,S.Name						AS ParStructure_Name
            	,C1.UnitId					AS Unidade
            	,PC.Name					AS UnidadeName
            	,C1.ConsolidationDate		AS ConsolidationDate
            	,L1.ParConsolidationType_Id AS ParConsolidationType_Id
            	,C1.ParLevel1_Id			AS Indicador
            	,L1.Name					AS IndicadorName
            	,L1C.ParCriticalLevel_Id	AS ParCriticalLevel_Id
            	,CRL.Name					AS ParCriticalLevel_Name
            	,L1.IsRuleConformity
            	,CASE 
            				WHEN L1.hashKey = 1 THEN ISNULL((SELECT top 1 SUM(VOLUMEPCC) From #VOLUMES V WITH (NOLOCK)
            											WHERE 1=1 
            											AND V.Data = c1.ConsolidationDate
            											AND V.ParCompany_id = c1.UnitId
            											) ,0)
            											-
            										ISNULL((SELECT SUM(NA) AS NA FROM #NA NA WHERE NA.UnitId = C1.UnitId AND NA.CollectionDate = C1.ConsolidationDate),0)
            				WHEN L1.ParConsolidationType_Id = 1 THEN SUM(C1.WeiEvaluation)
            				WHEN L1.ParConsolidationType_Id = 2 THEN SUM(C1.WeiEvaluation)
            				WHEN L1.ParConsolidationType_Id = 3 THEN SUM(C1.EvaluatedResult)
            				WHEN L1.ParConsolidationType_Id = 4 THEN ISNULL((SELECT SUM(AM) AM FROM #AMOSTRA4 A4 
            																WHERE 1=1 
            																  AND C1.Unitid = A4.UNIDADE 
            																  AND C1.ParLevel1_id = A4.INDICADOR 
            																  AND C1.[SHIFT] = A4.[SHIFT]
            																  AND C1.[PERIOD] = A4.[PERIOD]
            																  AND C1.ConsolidationDate = A4.DATA
                                                                             )
            															,0)
            				WHEN L1.ParConsolidationType_Id = 5 THEN SUM(C1.WeiEvaluation)
            				WHEN L1.ParConsolidationType_Id = 6 THEN SUM(C1.WeiEvaluation)
            				ELSE SUM(0)
            	 END AS [AVComPeso]
            	,CASE 
            				WHEN L1.ParConsolidationType_Id = 1 THEN SUM(c1.WeiDefects)
            				WHEN L1.ParConsolidationType_Id = 2 THEN SUM(c1.WeiDefects)
            				WHEN L1.ParConsolidationType_Id = 3 THEN SUM(c1.DefectsResult)
            				WHEN L1.ParConsolidationType_Id = 4 THEN  ISNULL((SELECT SUM(DEF_AM) DEF_AM FROM #AMOSTRA4 A4 
            																WHERE 1=1 
            																  AND C1.Unitid = A4.UNIDADE 
            																  AND C1.ParLevel1_id = A4.INDICADOR 
            																  AND C1.[SHIFT] = A4.[SHIFT]
            																  AND C1.[PERIOD] = A4.[PERIOD]
            																  AND C1.ConsolidationDate = A4.DATA
                                                                             )
            															,0)
            				WHEN L1.ParConsolidationType_Id = 5 THEN SUM(c1.WeiDefects)
            				WHEN L1.ParConsolidationType_Id = 6 THEN SUM(c1.TotalLevel3WithDefects)
            				ELSE SUM(0)
            	 END AS [NCComPeso]
            	,CASE 
            				WHEN L1.hashKey = 1 THEN ISNULL((SELECT top 1 SUM(VOLUMEPCC) From #VOLUMES V WITH (NOLOCK)
            											WHERE 1=1 
            											AND V.Data = c1.ConsolidationDate
            											AND V.ParCompany_id = c1.UnitId
            											),0)
            											-
            										ISNULL((SELECT SUM(NA) AS NA FROM #NA NA WHERE NA.UnitId = C1.UnitId AND NA.CollectionDate = C1.ConsolidationDate),0)
            				WHEN L1.ParConsolidationType_Id = 1 THEN SUM(C1.EvaluateTotal)
            				WHEN L1.ParConsolidationType_Id = 2 THEN SUM(C1.WeiEvaluation)
            				WHEN L1.ParConsolidationType_Id = 3 THEN SUM(C1.EvaluatedResult)
            				WHEN L1.ParConsolidationType_Id = 4 THEN  ISNULL((SELECT SUM(AM) AM FROM #AMOSTRA4 A4 
            																WHERE 1=1 
            																  AND C1.Unitid = A4.UNIDADE 
            																  AND C1.ParLevel1_id = A4.INDICADOR 
            																  AND C1.[SHIFT] = A4.[SHIFT]
            																  AND C1.[PERIOD] = A4.[PERIOD]
            																  AND C1.ConsolidationDate = A4.DATA
                                                                             )
            															,0)
            				WHEN L1.ParConsolidationType_Id = 5 THEN SUM(C1.EvaluateTotal)
            				WHEN L1.ParConsolidationType_Id = 6 THEN SUM(C1.EvaluateTotal)
            				ELSE SUM(0)
            	 END AS [AV]
            	,CASE 
            				WHEN L1.ParConsolidationType_Id = 1 THEN SUM(C1.DefectsTotal)
            				WHEN L1.ParConsolidationType_Id = 2 THEN SUM(C1.DefectsTotal)
            				WHEN L1.ParConsolidationType_Id = 3 THEN SUM(C1.DefectsResult)
            				WHEN L1.ParConsolidationType_Id = 4 THEN  ISNULL((SELECT SUM(DEF_AM) DEF_AM FROM #AMOSTRA4 A4 
            																WHERE 1=1 
            																  AND C1.Unitid = A4.UNIDADE 
            																  AND C1.ParLevel1_id = A4.INDICADOR 
            																  AND C1.[SHIFT] = A4.[SHIFT]
            																  AND C1.[PERIOD] = A4.[PERIOD]
            																  AND C1.ConsolidationDate = A4.DATA
                                                                             )
            															,0)
            				WHEN L1.ParConsolidationType_Id = 5 THEN SUM(C1.DefectsTotal)
            				WHEN L1.ParConsolidationType_Id = 6 THEN SUM(C1.TotalLevel3WithDefects)
            				ELSE SUM(0)
            	 END AS [NC]
            	--,CASE
            	--	WHEN (SELECT
            	--				COUNT(1)
            	--			FROM ParGoal G WITH (NOLOCK)
            	--			WHERE G.ParLevel1_Id = C1.ParLevel1_Id
            	--			AND (G.ParCompany_Id = C1.UnitId
            	--			OR G.ParCompany_id IS NULL)
            	--			AND G.EffectiveDate <= C1.ConsolidationDate)
            	--		> 0 THEN (SELECT TOP 1
            	--				ISNULL(G.PercentValue, 0)
            	--			FROM ParGoal G WITH (NOLOCK)
            	--			WHERE G.ParLevel1_id = C1.ParLevel1_Id
            	--			AND (G.ParCompany_id = C1.UnitId
            	--			OR G.ParCompany_id IS NULL)
            	--			AND G.EffectiveDate <= C1.ConsolidationDate
            	--			ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
                --
            	--	ELSE (SELECT TOP 1
            	--				ISNULL(G.PercentValue, 0)
            	--			FROM ParGoal G WITH (NOLOCK)
            	--			WHERE G.ParLevel1_id = C1.ParLevel1_Id
            	--			AND (G.ParCompany_id = C1.UnitId
            	--			OR G.ParCompany_id IS NULL)
                --          AND G.EffectiveDate <= C1.ConsolidationDate
                --			ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
            	--		END
            	--		AS Meta
                ,ISNULL((  SELECT TOP 1
					                PercentValue
					            FROM ParGoal pg
					            WHERE 1=1
					            AND pg.IsActive = 1
					            AND pg.ParLevel1_Id = C1.ParLevel1_Id
					            AND (isnull(pg.EffectiveDate,pg.EffectiveDate) <= C1.ConsolidationDate)
					            -- AND (isnull(pg.EffectiveDate,'1900-01-01') <= C1.ConsolidationDate)
					            AND (pg.ParCompany_Id =  C1.UnitId or pg.ParCompany_Id is null)
					            Order By /*EffectiveDate DESC,*/ EffectiveDate DESC, ParCompany_Id DESC),
					            (  SELECT TOP 1
					                PercentValue
					            FROM ParGoal pg
					            WHERE 1=1
					            AND pg.IsActive = 1
					            AND pg.ParLevel1_Id = C1.ParLevel1_Id
					            AND (isnull(pg.EffectiveDate,pg.EffectiveDate) <= C1.ConsolidationDate)
					            -- AND (isnull(pg.EffectiveDate,'1900-01-01') <= C1.ConsolidationDate)
					            AND (pg.ParCompany_Id =  C1.UnitId or pg.ParCompany_Id is null)
					            Order By /*EffectiveDate DESC,*/ EffectiveDate DESC, ParCompany_Id DESC))	AS Meta
            	INTO #CUBO
            	FROM #ConsolidationLevel C1
            	INNER JOIN ParLevel1 L1 WITH (NOLOCK)
             		ON C1.ParLevel1_Id = L1.ID
             		AND ISNULL(L1.ShowScorecard,1) = 1
             		AND L1.IsActive = 1
            
            	LEFT JOIN ParCompany PC WITH (NOLOCK)
             		ON PC.Id = C1.Unitid
            
            	INNER JOIN ParCompanyCluster CCL WITH (NOLOCK)
            		ON CCL.ParCompany_id = PC.id 
            		AND CCL.Active = 1
            
            	INNER JOIN ParLevel1XCluster L1C WITH (NOLOCK)
            		ON CCL.ParCluster_ID = L1C.ParCluster_ID 
             		AND C1.ParLevel1_Id = L1C.ParLevel1_Id 
             		AND L1C.Id = (select top 1 aaa.ID from ParLevel1XCluster aaa (nolock)  where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CCL.ParCluster_Id AND aaa.EffectiveDate <  @DATAFINAL AND Isactive = 1 ORDER BY ParLevel1_Id,ParCluster_Id,EffectiveDate,AddDate,AlterDate)
             		AND L1C.IsActive = 1
            
            	INNER JOIN ParCompanyXStructure CS WITH (NOLOCK)
            		ON PC.Id = CS.ParCompany_Id 
            		AND CS.Active = 1
            
            	INNER JOIN ParStructure S WITH (NOLOCK)
            		ON CS.ParStructure_id = S.id 
            		AND S.Active = 1
            
            	INNER JOIN ParCluster CL WITH (NOLOCK)
            		ON L1C.ParCluster_ID = CL.ID 
            		AND CL.IsActive = 1
            
            	INNER JOIN ParStructureGroup SG WITH (NOLOCK)
            		ON S.ParStructureGroup_Id = SG.ID 
            		AND SG.ID = 3 
            	
            	LEFT JOIN ParScoreType ST WITH (NOLOCK)
            		ON L1.ParConsolidationType_Id = ST.Id 
            		AND ST.IsActive = 1
            
            	LEFT JOIN ParCriticalLevel CRL WITH (NOLOCK)
            		ON L1C.ParCriticalLevel_id = CRL.id 
            		AND CRL.IsActive = 1
            
            GROUP BY
            	 CL.id						
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
            
                UPDATE #CUBO set Meta = iif(IsRuleConformity = 0,Meta, (100 - Meta))

            	-- DROP TABLE #DIM
            
            	--  select DISTINCT 
            	--  	ParCluster_ID
            	--  	,ParCluster_Name
            	--  	,ParStructure_id
            	--  	,ParStructure_Name
            	--  	,Unidade
            	--  	,UnidadeName
            	--  	,ParConsolidationType_Id
            	--  	,Indicador
            	--  	,IndicadorName
            	--  	,ParCriticalLevel_Id
            	--  	,ParCriticalLevel_Name
            	--  	,IsRuleConformity 
            	--  INTO #DIM
            	--  from #CUBO
            
            
            	--  DELETE DATA 
            	--  	FROM #DATA DATA
            	--  LEFT JOIN #CUBO CUBO
            	--  	ON DATA.DATA = CUBO.CONSOLIDATIONDATE
            	--  WHERE CUBO.CONSOLIDATIONDATE IS NOT NULL
            
            
            	--  INSERT INTO #CUBO (ConsolidationDate,ParCluster_ID,ParCluster_Name,ParStructure_id,ParStructure_Name,Unidade,UnidadeName,ParConsolidationType_Id,Indicador,IndicadorName,ParCriticalLevel_Id,ParCriticalLevel_Name,IsRuleConformity,AVComPeso,NCComPeso,AV,NC,Meta)
            	--  SELECT DATA.DATA, CUBO.*,0,0,0,0,0 
            	--  FROM #DATA DATA
            	--  Cross Join #DIM CUBO

                ";

                #endregion

            }

            else if (nivel == 2)
            {

                #region Consolidação Por Indicador e Monitoramento

                Query = $@"
            
            DECLARE @DATEINI DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
            DECLARE @DATEFIM DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'
            
             DECLARE @dataFim_ date = @DATEFIM
              
             DECLARE @dataInicio_ date = @DATEINI
            SET @dataInicio_ = @DATEINI
             
             -- DROP TABLE #DATA
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
            
            
            -- DROP TABLE #VOLUMES
            
            SELECT V.ParCompany_id,V.Data
            	, SUM(V.Quartos) AS VOLUMEPCC
            INTO #VOLUMES
            FROM VolumePcc1b V WITH (NOLOCK)
            WHERE 1=1 
            GROUP BY V.ParCompany_id,V.Data
            
            
            	-- DROP TABLE #AMOSTRA4
            
            	SELECT
            		UNIDADE
            	   ,INDICADOR
            	   ,DATA
            	   ,COUNT(1) AM
            	   ,SUM(DEF_AM) DEF_AM
            	INTO #AMOSTRA4
            	FROM (SELECT
            			CAST(C2.CollectionDate AS DATE) AS DATA
            		   ,C.Id AS UNIDADE
            		   ,C2.ParLevel1_Id AS INDICADOR
            		   ,C2.EvaluationNumber AS AV
            		   ,C2.Sample AS AM
            		   ,CASE
            				WHEN SUM(C2.WeiDefects) = 0 THEN 0
            				ELSE 1
            			END DEF_AM
            		FROM CollectionLevel2 C2 (NOLOCK)
            		INNER JOIN ParLevel1 L1 (NOLOCK)
            			ON L1.Id = C2.ParLevel1_Id AND ISNULL(L1.ShowScorecard, 1) = 1
                        AND L1.Id <> 43
            		INNER JOIN ParCompany C (NOLOCK)
            			ON C.Id = C2.UnitId
            		WHERE CAST(C2.CollectionDate AS DATE) BETWEEN @DATEINI AND @DATEFIM
            		AND C2.NotEvaluatedIs = 0
            		AND C2.Duplicated = 0
            		AND L1.ParConsolidationType_Id = 4
            		GROUP BY C.Id
            				,ParLevel1_Id
            				,EvaluationNumber
            				,Sample
            				,CAST(CollectionDate AS DATE)) TAB
            	GROUP BY UNIDADE
            			,INDICADOR
            			,DATA
            
            -- NA
            -- DROP TABLE #NA
            
            SELECT CL2.CollectionDate,CL2.UnitId,COUNT(distinct CL2.id) AS NA
            	INTO #NA
            	FROM CollectionLevel2 CL2 WITH (NOLOCK)
            	LEFT JOIN Result_Level3 CL3 WITH (NOLOCK)
            		ON CL3.CollectionLevel2_Id = CL2.Id
            	WHERE CONVERT(DATE, CL2.CollectionDate) between CONVERT(DATE,@DATEINI) and CONVERT(DATE,@DATEFIM)
            	AND CL2.ParLevel1_Id IN (SELECT 
            			id
            		FROM Parlevel1 WITH (NOLOCK)
            		WHERE Hashkey = 1 )
            	AND CL3.IsNotEvaluate = 1
            	GROUP BY CL2.CollectionDate,CL2.UnitId
            HAVING COUNT(DISTINCT CL2.id) > 1
            
            -- C1
            -- DROP TABLE #ConsolidationLevel
            
            SELECT 
            	CL2.id,
            	CL2.ConsolidationDate,
            	CL2.UnitId,
            	CL1.ParLevel1_Id,
            	CL2.ParLevel2_Id,
            	CL2.DefectsResult,
            	CL2.WeiDefects,
            	CL2.EvaluatedResult,
            	CL2.WeiEvaluation,
            	CL2.EvaluateTotal,
            	CL2.TotalLevel3WithDefects,
            	CL2.DefectsTotal
            INTO #ConsolidationLevel
            FROM ConsolidationLevel1 CL1 WITH (NOLOCK) 
            LEFT JOIN ConsolidationLevel2 CL2 WITH (NOLOCK) 
            	ON CL1.ID = CL2.ConsolidationLevel1_Id
            WHERE 1=1 
            AND CL1.ConsolidationDate BETWEEN @DATEINI AND @DATEFIM
            AND CL1.UnitId IN ({ WunidadeAcesso }) 
            " + Wshift + @"
            " + Wunidade + @"
            " + Wfuncao + @"
            " + Windicador + @"
            " + Wmonitoramento + @"
            AND CL1.ParLevel1_Id != 43
            AND CL1.ParLevel1_Id != 42
            
            CREATE INDEX IDX_HashConsolidationLevel ON #ConsolidationLevel (ConsolidationDate,UnitId,ParLevel1_Id,ParLevel2_Id); 
            CREATE INDEX IDX_HashConsolidationLevel_level2 ON #ConsolidationLevel (ConsolidationDate,ParLevel1_Id,ParLevel2_Id); 
            CREATE INDEX IDX_HashConsolidationLevel_Unitid ON #ConsolidationLevel (ConsolidationDate,UnitId); 
            CREATE INDEX IDX_HashConsolidationLevel_id ON #ConsolidationLevel (id); 

            
            -- CUBO
            -- DROP TABLE #CUBO
            SELECT 
            	 CL.id						AS ParCluster_ID
            	,CL.Name					AS ParCluster_Name
            	,CS.ParStructure_id			AS ParStructure_id
            	,S.Name						AS ParStructure_Name
            	,C1.UnitId					AS Unidade
            	,PC.Name					AS UnidadeName
            	,C1.ConsolidationDate		AS ConsolidationDate
            	,L1.ParConsolidationType_Id AS ParConsolidationType_Id
            	,C1.ParLevel1_Id			AS Indicador
            	,L1.Name					AS IndicadorName
            	,C1.ParLevel2_Id			AS Monitoramento
            	,L2.Name					AS MonitoramentoName
            	,concat(L2.Name, ' - ', PC.Name) as MonitoramentoUnidade
            	,L1C.ParCriticalLevel_Id	AS ParCriticalLevel_Id
            	,CRL.Name					AS ParCriticalLevel_Name
            	,L1.IsRuleConformity
            	,CASE 
            				WHEN L1.hashKey = 1 THEN ISNULL((SELECT top 1 SUM(VOLUMEPCC) From #VOLUMES V WITH (NOLOCK)
            											WHERE 1=1 
            											AND V.Data = c1.ConsolidationDate
            											AND V.ParCompany_id = c1.UnitId
            											) ,0)
            											-
            										ISNULL((SELECT SUM(NA) AS NA FROM #NA NA WHERE NA.UnitId = C1.UnitId AND NA.CollectionDate = C1.ConsolidationDate),0)
            				WHEN C1.ParLevel1_Id = 36 THEN SUM(C1.WeiEvaluation)
            				WHEN L1.ParConsolidationType_Id = 1 THEN SUM(C1.WeiEvaluation)
            				WHEN L1.ParConsolidationType_Id = 2 THEN SUM(C1.WeiEvaluation)
            				WHEN L1.ParConsolidationType_Id = 3 THEN SUM(C1.EvaluatedResult)
            				-- WHEN L1.ParConsolidationType_Id = 4 THEN ISNULL((SELECT SUM(AM) AM FROM #AMOSTRA4 A4 
            				-- 												WHERE 1=1 
            				-- 												  AND C1.Unitid = A4.UNIDADE 
            				-- 												  AND C1.ParLevel1_id = A4.INDICADOR 
            				-- 												  AND C1.ConsolidationDate = A4.DATA)
            				-- 											,0)
            				WHEN L1.ParConsolidationType_Id = 5 THEN SUM(C1.EvaluateTotal)
            				WHEN L1.ParConsolidationType_Id = 6 THEN SUM(C1.EvaluateTotal)
            				ELSE SUM(0)
            	 END AS [AVComPeso]
            	,CASE 
            				WHEN C1.ParLevel1_Id = 36 THEN SUM(C1.WeiDefects)
            				WHEN L1.ParConsolidationType_Id = 1 THEN SUM(c1.WeiDefects)
            				WHEN L1.ParConsolidationType_Id = 2 THEN SUM(c1.WeiDefects)
            				WHEN L1.ParConsolidationType_Id = 3 THEN SUM(c1.DefectsResult)
            				-- WHEN L1.ParConsolidationType_Id = 4 THEN  ISNULL((SELECT SUM(DEF_AM) DEF_AM FROM #AMOSTRA4 A4 
            				-- 												WHERE 1=1 
            				-- 												  AND C1.Unitid = A4.UNIDADE 
            				-- 												  AND C1.ParLevel1_id = A4.INDICADOR 
            				-- 												  AND C1.ConsolidationDate = A4.DATA)
            				-- 											,0)
            				WHEN L1.ParConsolidationType_Id = 5 THEN SUM(c1.WeiDefects)
            				WHEN L1.ParConsolidationType_Id = 6 THEN SUM(c1.TotalLevel3WithDefects)
            				ELSE SUM(0)
            	 END AS [NCComPeso]
            	,CASE 
            				WHEN L1.hashKey = 1 THEN ISNULL((SELECT top 1 SUM(VOLUMEPCC) From #VOLUMES V WITH (NOLOCK)
            											WHERE 1=1 
            											AND V.Data = c1.ConsolidationDate
            											AND V.ParCompany_id = c1.UnitId
            											),0)
            											-
            										ISNULL((SELECT SUM(NA) AS NA FROM #NA NA WHERE NA.UnitId = C1.UnitId AND NA.CollectionDate = C1.ConsolidationDate),0)
            				WHEN C1.ParLevel1_Id = 36 THEN SUM(C1.EvaluateTotal)
            				WHEN L1.ParConsolidationType_Id = 1 THEN SUM(C1.EvaluateTotal)
            				WHEN L1.ParConsolidationType_Id = 2 THEN SUM(C1.WeiEvaluation)
            				WHEN L1.ParConsolidationType_Id = 3 THEN SUM(C1.EvaluatedResult)
            				-- WHEN L1.ParConsolidationType_Id = 4 THEN  ISNULL((SELECT SUM(AM) AM FROM #AMOSTRA4 A4 
            				-- 												WHERE 1=1 
            				-- 												  AND C1.Unitid = A4.UNIDADE 
            				-- 												  AND C1.ParLevel1_id = A4.INDICADOR 
            				-- 												  AND C1.ConsolidationDate = A4.DATA)
            				-- 											,0)
            				WHEN L1.ParConsolidationType_Id = 5 THEN SUM(C1.EvaluateTotal)
            				WHEN L1.ParConsolidationType_Id = 6 THEN SUM(C1.EvaluateTotal)
            				ELSE SUM(0)
            	 END AS [AV]
            	,CASE 
            				WHEN C1.ParLevel1_Id = 36 THEN SUM(C1.DefectsTotal)
            				WHEN L1.ParConsolidationType_Id = 1 THEN SUM(C1.DefectsTotal)
            				WHEN L1.ParConsolidationType_Id = 2 THEN SUM(C1.WeiDefects)
            				WHEN L1.ParConsolidationType_Id = 3 THEN SUM(C1.DefectsResult)
            				-- WHEN L1.ParConsolidationType_Id = 4 THEN  ISNULL((SELECT SUM(DEF_AM) DEF_AM FROM #AMOSTRA4 A4 
            				-- 												WHERE 1=1 
            				-- 												  AND C1.Unitid = A4.UNIDADE 
            				-- 												  AND C1.ParLevel1_id = A4.INDICADOR 
            				-- 												  AND C1.ConsolidationDate = A4.DATA)
            				-- 											,0)
            				WHEN L1.ParConsolidationType_Id = 5 THEN SUM(C1.DefectsTotal)
            				WHEN L1.ParConsolidationType_Id = 6 THEN SUM(C1.TotalLevel3WithDefects)
            				ELSE SUM(0)
            	 END AS [NC]
            	--,CASE
            	--	WHEN (SELECT
            	--				COUNT(1)
            	--			FROM ParGoal G WITH (NOLOCK)
            	--			WHERE G.ParLevel1_Id = C1.ParLevel1_Id
            	--			AND (G.ParCompany_Id = C1.UnitId
            	--			OR G.ParCompany_id IS NULL)
            	--			AND G.EffectiveDate <= C1.ConsolidationDate)
            	--		> 0 THEN (SELECT TOP 1
            	--				ISNULL(G.PercentValue, 0)
            	--			FROM ParGoal G WITH (NOLOCK)
            	--			WHERE G.ParLevel1_id = C1.ParLevel1_Id
            	--			AND (G.ParCompany_id = C1.UnitId
            	--			OR G.ParCompany_id IS NULL)
            	--			AND G.EffectiveDate <= C1.ConsolidationDate
            	--			ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
                --
            	--	ELSE (SELECT TOP 1
            	--				ISNULL(G.PercentValue, 0)
            	--			FROM ParGoal G WITH (NOLOCK)
            	--			WHERE G.ParLevel1_id = C1.ParLevel1_Id
            	--			AND (G.ParCompany_id = C1.UnitId
            	--			OR G.ParCompany_id IS NULL)
            	--			AND G.EffectiveDate <= C1.ConsolidationDate
            	--			ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
                --		END
            	--		AS Meta
                ,ISNULL((  SELECT TOP 1
					                PercentValue
					            FROM ParGoal pg
					            WHERE 1=1
					            AND pg.IsActive = 1
					            AND pg.ParLevel1_Id = C1.ParLevel1_Id
					            AND (isnull(pg.EffectiveDate,pg.EffectiveDate) <= C1.ConsolidationDate)
					            -- AND (isnull(pg.EffectiveDate,'1900-01-01') <= C1.ConsolidationDate)
					            AND (pg.ParCompany_Id =  C1.UnitId or pg.ParCompany_Id is null)
					            Order By /*EffectiveDate DESC,*/ EffectiveDate DESC, ParCompany_Id DESC),
					            (  SELECT TOP 1
					                PercentValue
					            FROM ParGoal pg
					            WHERE 1=1
					            AND pg.IsActive = 1
					            AND pg.ParLevel1_Id = C1.ParLevel1_Id
					            AND (isnull(pg.EffectiveDate,pg.EffectiveDate) <= C1.ConsolidationDate)
					            -- AND (isnull(pg.EffectiveDate,'1900-01-01') <= C1.ConsolidationDate)
					            AND (pg.ParCompany_Id =  C1.UnitId or pg.ParCompany_Id is null)
					            Order By /*EffectiveDate DESC,*/ EffectiveDate DESC, ParCompany_Id DESC))	AS Meta
            	INTO #CUBO
            	FROM #ConsolidationLevel C1
            	INNER JOIN ParLevel1 L1 WITH (NOLOCK)
             		ON C1.ParLevel1_Id = L1.ID
             		AND ISNULL(L1.ShowScorecard,1) = 1
             		AND L1.IsActive = 1
            
            	INNER JOIN ParLevel2 L2 WITH (NOLOCK)
             		ON C1.ParLevel2_Id = L2.ID
             		AND L2.IsActive = 1
            
            	LEFT JOIN ParCompany PC WITH (NOLOCK)
             		ON PC.Id = C1.Unitid
            
            	INNER JOIN ParCompanyCluster CCL WITH (NOLOCK)
            		ON CCL.ParCompany_id = PC.id 
            		AND CCL.Active = 1
            
            	INNER JOIN ParLevel1XCluster L1C WITH (NOLOCK)
            		ON CCL.ParCluster_ID = L1C.ParCluster_ID 
             		AND C1.ParLevel1_Id = L1C.ParLevel1_Id 
             		AND L1C.Id = (select top 1 aaa.ID from ParLevel1XCluster aaa (nolock)  where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CCL.ParCluster_Id AND aaa.EffectiveDate <  @DATAFINAL AND Isactive = 1 ORDER BY ParLevel1_Id,ParCluster_Id,EffectiveDate,AddDate,AlterDate)
             		AND L1C.IsActive = 1
            
            	INNER JOIN ParCompanyXStructure CS WITH (NOLOCK)
            		ON PC.Id = CS.ParCompany_Id 
            		AND CS.Active = 1
            
            	INNER JOIN ParStructure S WITH (NOLOCK)
            		ON CS.ParStructure_id = S.id 
            		AND S.Active = 1
            
            	INNER JOIN ParCluster CL WITH (NOLOCK)
            		ON L1C.ParCluster_ID = CL.ID 
            		AND CL.IsActive = 1
            
            	INNER JOIN ParStructureGroup SG WITH (NOLOCK)
            		ON S.ParStructureGroup_Id = SG.ID 
            		AND SG.ID = 3 
            	
            	LEFT JOIN ParScoreType ST WITH (NOLOCK)
            		ON L1.ParConsolidationType_Id = ST.Id 
            		AND ST.IsActive = 1
            
            	LEFT JOIN ParCriticalLevel CRL WITH (NOLOCK)
            		ON L1C.ParCriticalLevel_id = CRL.id 
            		AND CRL.IsActive = 1
            
            GROUP BY
            	 CL.id						
            	,CL.Name					
            	,CS.ParStructure_id			
            	,S.Name						
            	,C1.UnitId 	
            	,PC.Name 	
            	,C1.ConsolidationDate 
            	,L1.ParConsolidationType_Id	
            	,L1.hashKey
            	,C1.ParLevel1_Id 	
            	,L1.Name 	
            	,C1.ParLevel2_Id 	
            	,L2.Name 	
            	,L1C.ParCriticalLevel_Id	
            	,CRL.Name
            	,L1.IsRuleConformity
            
                update #CUBO set Meta = iif(IsRuleConformity = 0,Meta, (100 - Meta))

            	-- DROP TABLE #DIM
            
            	--  select DISTINCT 
            	--  	ParCluster_ID
            	--  	,ParCluster_Name
            	--  	,ParStructure_id
            	--  	,ParStructure_Name
            	--  	,Unidade
            	--  	,UnidadeName
            	--  	,ParConsolidationType_Id
            	--  	,Indicador
            	--  	,IndicadorName
            	--  	,Monitoramento
            	--  	,MonitoramentoName
            	--  	,MonitoramentoUnidade
            	--  	,ParCriticalLevel_Id
            	--  	,ParCriticalLevel_Name
            	--  	,IsRuleConformity 
            	--  INTO #DIM
            	--  from #CUBO
            
            
            	--  DELETE DATA 
            	--  	FROM #DATA DATA
            	--  LEFT JOIN #CUBO CUBO
            	--  	ON DATA.DATA = CUBO.CONSOLIDATIONDATE
            	--  WHERE CUBO.CONSOLIDATIONDATE IS NOT NULL
            
            
            	--  INSERT INTO #CUBO (ConsolidationDate,ParCluster_ID,ParCluster_Name,ParStructure_id,ParStructure_Name,Unidade,UnidadeName,ParConsolidationType_Id,Indicador,IndicadorName,Monitoramento,MonitoramentoName,MonitoramentoUnidade,ParCriticalLevel_Id,ParCriticalLevel_Name,IsRuleConformity,AVComPeso,NCComPeso,AV,NC,Meta)
            	--  SELECT DATA.DATA, CUBO.*,0,0,0,0,0 
            	--  FROM #DATA DATA
            	--  Cross Join #DIM CUBO

                ";

                #endregion

            }

            else if (nivel == 3)
            {

                #region Consolidação Por Indicador, Monitoramento e Tarefa

                Query = $@"
            
        DECLARE @DATEINI DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
        DECLARE @DATEFIM DATETIME = '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'
        
          DECLARE @dataFim_ date = @DATEFIM
          
        DECLARE @dataInicio_ date = @DATEINI
        SET @dataInicio_ = @DATEINI
         
        -- DROP TABLE  #DATA
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
        
        
        -- DROP TABLE  #VOLUMES
        
        SELECT V.ParCompany_id,V.Data
        	, SUM(V.Quartos) AS VOLUMEPCC
        INTO #VOLUMES
        FROM VolumePcc1b V WITH (NOLOCK)
        WHERE 1=1 
        GROUP BY V.ParCompany_id,V.Data
        
        
        	-- DROP TABLE  #AMOSTRA4
        
        	SELECT
        		UNIDADE
        	   ,INDICADOR
        	   ,DATA
        	   ,COUNT(1) AM
        	   ,SUM(DEF_AM) DEF_AM
        	INTO #AMOSTRA4
        	FROM (SELECT
        			CAST(C2.CollectionDate AS DATE) AS DATA
        		   ,C.Id AS UNIDADE
        		   ,C2.ParLevel1_Id AS INDICADOR
        		   ,C2.EvaluationNumber AS AV
        		   ,C2.Sample AS AM
        		   ,CASE
        				WHEN SUM(C2.WeiDefects) = 0 THEN 0
        				ELSE 1
        			END DEF_AM
        		FROM CollectionLevel2 C2 (NOLOCK)
        		INNER JOIN ParLevel1 L1 (NOLOCK)
        			ON L1.Id = C2.ParLevel1_Id AND ISNULL(L1.ShowScorecard, 1) = 1
                    AND L1.Id <> 43
        		INNER JOIN ParCompany C (NOLOCK)
        			ON C.Id = C2.UnitId
        		WHERE CAST(C2.CollectionDate AS DATE) BETWEEN @DATEINI AND @DATEFIM
        		AND C2.NotEvaluatedIs = 0
        		AND C2.Duplicated = 0
        		AND L1.ParConsolidationType_Id = 4
        		GROUP BY C.Id
        				,ParLevel1_Id
        				,EvaluationNumber
        				,Sample
        				,CAST(CollectionDate AS DATE)) TAB
        	GROUP BY UNIDADE
        			,INDICADOR
        			,DATA
        
        -- NA
        -- DROP TABLE  #NA
        
        SELECT CL2.CollectionDate,CL2.UnitId,COUNT(distinct CL2.id) AS NA
        	INTO #NA
        	FROM CollectionLevel2 CL2 WITH (NOLOCK)
        	LEFT JOIN Result_Level3 CL3 WITH (NOLOCK)
        		ON CL3.CollectionLevel2_Id = CL2.Id
        	WHERE CONVERT(DATE, CL2.CollectionDate) between CONVERT(DATE,@DATEINI) and CONVERT(DATE,@DATEFIM)
        	AND CL2.ParLevel1_Id IN (SELECT 
        			id
        		FROM Parlevel1 WITH (NOLOCK)
        		WHERE Hashkey = 1 )
        	AND CL3.IsNotEvaluate = 1
        	GROUP BY CL2.CollectionDate,CL2.UnitId
        HAVING COUNT(DISTINCT CL2.id) > 1
        
        -- C1
        -- DROP TABLE  #ConsolidationLevel
        
        SELECT 
        	CL2.id,
        	CL2.ConsolidationDate,
        	CL2.UnitId,
        	CL1.ParLevel1_Id,
        	CL2.ParLevel2_Id,
        	R3.ParLevel3_Id,
        	R3.WeiDefects,
        	R3.Defects,
        	R3.WeiEvaluation,
        	R3.Evaluation
        INTO #ConsolidationLevel
        FROM ConsolidationLevel1 CL1 WITH (NOLOCK) 
        LEFT JOIN ConsolidationLevel2 CL2 WITH (NOLOCK) 
        	ON CL1.ID = CL2.ConsolidationLevel1_Id
        LEFT JOIN CollectionLevel2 C2 WITH (NOLOCK) 
        	ON CL2.ID = C2.ConsolidationLevel2_Id
        LEFT JOIN Result_Level3 R3 WITH (NOLOCK) 
        	ON C2.ID = R3.CollectionLevel2_Id
        WHERE 1=1 
        AND CL1.ConsolidationDate BETWEEN @DATEINI AND @DATEFIM
        AND CL1.UnitId IN ({ WunidadeAcesso })
        " + Wshift + @"
        " + Wunidade + @"
        " + Wfuncao + @"
        " + Windicador + @"
        " + Wmonitoramento + @"
        " + Wtarefa + @"
        AND CL1.ParLevel1_Id != 43
        AND CL1.ParLevel1_Id != 42
        AND R3.IsNotEvaluate = 0      

        CREATE INDEX IDX_HashConsolidationLevel ON #ConsolidationLevel (ConsolidationDate,UnitId,ParLevel1_Id,ParLevel2_Id,ParLevel3_Id); 
        CREATE INDEX IDX_HashConsolidationLevel_level3 ON #ConsolidationLevel (ConsolidationDate,ParLevel1_Id,ParLevel2_Id,ParLevel3_Id); 
        CREATE INDEX IDX_HashConsolidationLevel_Unitid ON #ConsolidationLevel (ConsolidationDate,UnitId); 
        CREATE INDEX IDX_HashConsolidationLevel_id ON #ConsolidationLevel (id); 

        
        -- CUBO
        -- DROP TABLE  #CUBO
        SELECT 
        	 CL.id						AS ParCluster_ID
        	,CL.Name					AS ParCluster_Name
        	,CS.ParStructure_id			AS ParStructure_id
        	,S.Name						AS ParStructure_Name
        	,C1.UnitId					AS Unidade
        	,PC.Name					AS UnidadeName
        	,C1.ConsolidationDate		AS ConsolidationDate
        	,L1.ParConsolidationType_Id AS ParConsolidationType_Id
        	,C1.ParLevel1_Id			AS Indicador
        	,L1.Name					AS IndicadorName
        	,C1.ParLevel2_Id			AS Monitoramento
        	,L2.Name					AS MonitoramentoName
        	,concat(L2.Name, ' - ', PC.Name) as MonitoramentoUnidade
        	,C1.ParLevel3_Id			AS Tarefa
        	,L3.Name					AS TarefaName
        	,L1C.ParCriticalLevel_Id	AS ParCriticalLevel_Id
        	,CRL.Name					AS ParCriticalLevel_Name
        	,L1.IsRuleConformity
        	,CASE 
        		WHEN L1.hashKey = 1 THEN ISNULL((SELECT top 1 SUM(VOLUMEPCC) From #VOLUMES V WITH (NOLOCK)
        											WHERE 1=1 
        											AND V.Data = c1.ConsolidationDate
        											AND V.ParCompany_id = c1.UnitId
        										) ,0)/2
        											-
        										ISNULL((SELECT SUM(NA) AS NA FROM #NA NA WHERE NA.UnitId = C1.UnitId AND NA.CollectionDate = C1.ConsolidationDate),0)
        		ELSE SUM(WeiEvaluation)
        	END AS [AVComPeso]
        	,SUM(WeiDefects) AS [nCComPeso]
        	,CASE 
        		WHEN L1.hashKey = 1 THEN ISNULL((SELECT top 1 SUM(VOLUMEPCC) From #VOLUMES V WITH (NOLOCK)
        										WHERE 1=1 
        										AND V.Data = c1.ConsolidationDate
        										AND V.ParCompany_id = c1.UnitId
        									),0)
        									-
        									ISNULL((SELECT SUM(NA) AS NA FROM #NA NA WHERE NA.UnitId = C1.UnitId AND NA.CollectionDate = C1.ConsolidationDate),0)
        		WHEN L1.ParConsolidationType_Id = 2 THEN SUM(WeiEvaluation)
        		ELSE SUM(Evaluation)
        	 END AS [AV]
        	,CASE
        		WHEN L1.ParConsolidationType_Id = 2 THEN SUM(WeiDefects)
        		ELSE SUM(Defects)
        	END AS [NC]
            	--,CASE
            	--	WHEN (SELECT
            	--				COUNT(1)
            	--			FROM ParGoal G WITH (NOLOCK)
            	--			WHERE G.ParLevel1_Id = C1.ParLevel1_Id
            	--			AND (G.ParCompany_Id = C1.UnitId
            	--			OR G.ParCompany_id IS NULL)
            	--			AND G.EffectiveDate <= C1.ConsolidationDate)
            	--		> 0 THEN (SELECT TOP 1
            	--				ISNULL(G.PercentValue, 0)
            	--			FROM ParGoal G WITH (NOLOCK)
            	--			WHERE G.ParLevel1_id = C1.ParLevel1_Id
            	--			AND (G.ParCompany_id = C1.UnitId
            	--			OR G.ParCompany_id IS NULL)
            	--			AND G.EffectiveDate <= C1.ConsolidationDate
            	--			ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
                --
            	--	ELSE (SELECT TOP 1
            	--				ISNULL(G.PercentValue, 0)
            	--			FROM ParGoal G WITH (NOLOCK)
            	--			WHERE G.ParLevel1_id = C1.ParLevel1_Id
            	--			AND (G.ParCompany_id = C1.UnitId
            	--			OR G.ParCompany_id IS NULL)
            	--			AND G.EffectiveDate <= C1.ConsolidationDate
            	--			ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
            	--		END
            	--		AS Meta
                ,ISNULL((  SELECT TOP 1
					                PercentValue
					            FROM ParGoal pg
					            WHERE 1=1
					            AND pg.IsActive = 1
					            AND pg.ParLevel1_Id = C1.ParLevel1_Id
					            AND (isnull(pg.EffectiveDate,pg.EffectiveDate) <= C1.ConsolidationDate)
					            -- AND (isnull(pg.EffectiveDate,'1900-01-01') <= C1.ConsolidationDate)
					            AND (pg.ParCompany_Id =  C1.UnitId or pg.ParCompany_Id is null)
					            Order By /*EffectiveDate DESC,*/ EffectiveDate DESC, ParCompany_Id DESC),
					            (  SELECT TOP 1
					                PercentValue
					            FROM ParGoal pg
					            WHERE 1=1
					            AND pg.IsActive = 1
					            AND pg.ParLevel1_Id = C1.ParLevel1_Id
					            AND (isnull(pg.EffectiveDate,pg.EffectiveDate) <= C1.ConsolidationDate)
					            -- AND (isnull(pg.EffectiveDate,'1900-01-01') <= C1.ConsolidationDate)
					            AND (pg.ParCompany_Id =  C1.UnitId or pg.ParCompany_Id is null)
					            Order By /*EffectiveDate DESC,*/ EffectiveDate DESC, ParCompany_Id DESC))	AS Meta
        	INTO #CUBO
        	FROM #ConsolidationLevel C1
        	INNER JOIN ParLevel1 L1 WITH (NOLOCK)
         		ON C1.ParLevel1_Id = L1.ID
         		AND ISNULL(L1.ShowScorecard,1) = 1
         		AND L1.IsActive = 1
        
        	INNER JOIN ParLevel2 L2 WITH (NOLOCK)
         		ON C1.ParLevel2_Id = L2.ID
         		AND L2.IsActive = 1
        
        	INNER JOIN ParLevel3 L3 WITH (NOLOCK)
         		ON C1.ParLevel3_Id = L3.ID
         		AND L3.IsActive = 1
        
        	LEFT JOIN ParCompany PC WITH (NOLOCK)
         		ON PC.Id = C1.Unitid
        
        	INNER JOIN ParCompanyCluster CCL WITH (NOLOCK)
        		ON CCL.ParCompany_id = PC.id 
        		AND CCL.Active = 1
        
        	INNER JOIN ParLevel1XCluster L1C WITH (NOLOCK)
        		ON CCL.ParCluster_ID = L1C.ParCluster_ID 
         		AND C1.ParLevel1_Id = L1C.ParLevel1_Id 
             	AND L1C.Id = (select top 1 aaa.ID from ParLevel1XCluster aaa (nolock)  where aaa.ParLevel1_Id = L1.Id AND aaa.ParCluster_Id = CCL.ParCluster_Id AND aaa.EffectiveDate <  @DATAFINAL AND Isactive = 1 ORDER BY ParLevel1_Id,ParCluster_Id,EffectiveDate,AddDate,AlterDate)
         		AND L1C.IsActive = 1
        
        	INNER JOIN ParCompanyXStructure CS WITH (NOLOCK)
        		ON PC.Id = CS.ParCompany_Id 
        		AND CS.Active = 1
        
        	INNER JOIN ParStructure S WITH (NOLOCK)
        		ON CS.ParStructure_id = S.id 
        		AND S.Active = 1
        
        	INNER JOIN ParCluster CL WITH (NOLOCK)
        		ON L1C.ParCluster_ID = CL.ID 
        		AND CL.IsActive = 1
        
        	INNER JOIN ParStructureGroup SG WITH (NOLOCK)
        		ON S.ParStructureGroup_Id = SG.ID 
        		AND SG.ID = 3 
        	
        	LEFT JOIN ParScoreType ST WITH (NOLOCK)
        		ON L1.ParConsolidationType_Id = ST.Id 
        		AND ST.IsActive = 1
        
        	LEFT JOIN ParCriticalLevel CRL WITH (NOLOCK)
        		ON L1C.ParCriticalLevel_id = CRL.id 
        		AND CRL.IsActive = 1
        
        GROUP BY
        	 CL.id						
        	,CL.Name					
        	,CS.ParStructure_id			
        	,S.Name						
        	,C1.UnitId 	
        	,PC.Name 	
        	,C1.ConsolidationDate 
        	,L1.ParConsolidationType_Id	
        	,L1.hashKey
        	,C1.ParLevel1_Id 	
        	,L1.Name 	
        	,C1.ParLevel2_Id
        	,L2.Name 	
        	,C1.ParLevel3_id
        	,L3.Name 	
        	,L1C.ParCriticalLevel_Id	
        	,CRL.Name
        	,L1.IsRuleConformity
        
        
            update #CUBO set Meta = iif(IsRuleConformity = 0,Meta, (100 - Meta))

        	-- DROP TABLE  #DIM
        
        	--  select DISTINCT 
        	--  	ParCluster_ID
        	--  	,ParCluster_Name
        	--  	,ParStructure_id
        	--  	,ParStructure_Name
        	--  	,Unidade
        	--  	,UnidadeName
        	--  	,ParConsolidationType_Id
        	--  	,Indicador
        	--  	,IndicadorName
        	--  	,Monitoramento
        	--  	,MonitoramentoName
        	--  	,MonitoramentoUnidade
        	--  	,Tarefa
        	--  	,TarefaName
        	--  	,ParCriticalLevel_Id
        	--  	,ParCriticalLevel_Name
        	--  	,IsRuleConformity 
        	--  INTO #DIM
        	--  from #CUBO
        
        	
        
        	--  DELETE DATA 
        	--  	FROM #DATA DATA
        	--  LEFT JOIN #CUBO CUBO
        	--  	ON DATA.DATA = CUBO.CONSOLIDATIONDATE
        	--  WHERE CUBO.CONSOLIDATIONDATE IS NOT NULL
        
        
        	--  INSERT INTO #CUBO (ConsolidationDate,ParCluster_ID,ParCluster_Name,ParStructure_id,ParStructure_Name,Unidade,UnidadeName,ParConsolidationType_Id,Indicador,IndicadorName,Monitoramento,MonitoramentoName,MonitoramentoUnidade,Tarefa,TarefaName,ParCriticalLevel_Id,ParCriticalLevel_Name,IsRuleConformity,AVComPeso,NCComPeso,AV,NC,Meta)
        	--  SELECT DATA.DATA, CUBO.*,0,0,0,0,0 
        	--  FROM #DATA DATA
        	--  Cross Join #DIM CUBO
        
            ";

                #endregion

            }


            return Query;
        }

        private static string getQueryStatusIndicador(DTO.DataCarrierFormularioNew form, int? whereStatus)
        {
            var whereStatusQuery = "";

            if (whereStatus == 1)
            {
                whereStatusQuery = " AND WHERESTATUS.PC < WHERESTATUS.Meta ";
            }
            if (whereStatus == 2)
            {
                whereStatusQuery = " AND WHERESTATUS.PC > WHERESTATUS.Meta ";
            }

            var Query = "";

            Query = @"
            
            
            SELECT 
            	 ParCluster_ID
            	,ParCluster_Name
            	,ParStructure_id
            	,ParStructure_Name
            	,Unidade					
            	,UnidadeName				
            	,ParConsolidationType_Id	
            	,Indicador					
            	,IndicadorName				
            	,IIF(sum(isnull(AVComPeso,0))=0,0,IIF(isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0)>100,100,isnull(sum(NULLIF(NCComPeso,0))/sum(isnull(AVComPeso,0))*100,0))) AS [PC]
            INTO #STATUS
            FROM #CUBO Cubo WITH (NOLOCK)
            	WHERE 1=1
            group by 
            	ParCluster_ID
            	,ParCluster_Name
            	,ParStructure_id
            	,ParStructure_Name
            	,Unidade					
            	,UnidadeName				
            	,ParConsolidationType_Id	
            	,Indicador					
            	,IndicadorName	
            

            -- Calcula Indicadores fora ou dentro da meta
            
            SELECT 
            	 ParCluster_ID
            	,ParCluster_Name
            	,ParStructure_id
            	,ParStructure_Name
            	,Unidade
            	,UnidadeName
            	,ParConsolidationType_Id
            	,Indicador
            	,IndicadorName
            	,PC
            	,CASE
            		WHEN (SELECT
            					COUNT(1)
            				FROM ParGoal G WITH (NOLOCK)
            				WHERE G.ParLevel1_Id = Indicador
            				AND (G.ParCompany_Id = Unidade
            				OR G.ParCompany_id IS NULL)
            				AND G.EffectiveDate <= @DATEFIM)
            			> 0 THEN (SELECT TOP 1
            					ISNULL(G.PercentValue, 0)
            				FROM ParGoal G WITH (NOLOCK)
            				WHERE G.ParLevel1_id = Indicador
            				AND (G.ParCompany_id = Unidade
            				OR G.ParCompany_id IS NULL)
            				AND G.EffectiveDate <= @DATEFIM
            				ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
            
            		ELSE (SELECT TOP 1
            					ISNULL(G.PercentValue, 0)
            				FROM ParGoal G WITH (NOLOCK)
            				WHERE G.ParLevel1_id = Indicador
            				AND (G.ParCompany_id = Unidade
            				OR G.ParCompany_id IS NULL)
            				AND G.EffectiveDate <= @DATEFIM
            				ORDER BY G.ParCompany_Id DESC, EffectiveDate DESC)
            			END
            			AS Meta
             INTO #WHERESTATUS
             FROM #STATUS


            DELETE CUBO 
	        FROM #CUBO CUBO
	        INNER JOIN #WHERESTATUS WHERESTATUS
		        ON CUBO.ParCluster_ID = WHERESTATUS.ParCluster_ID
		        AND CUBO.ParStructure_id = WHERESTATUS.ParStructure_id
		        AND CUBO.Unidade = WHERESTATUS.Unidade
		        AND CUBO.Indicador = WHERESTATUS.Indicador
	        WHERE 1=1
	    	" + whereStatusQuery + @"
           
            ";

            return Query;
        }

        private static string getQueryPAConcluidos(DTO.DataCarrierFormularioNew form, int? wherePA)
        {
            var wherePAConcQuery = "";

            if (wherePA == 1)
            {
                wherePAConcQuery = " AND Pa_Acao_View.Data IS NULL ";
            }
            if (wherePA == 2)
            {
                wherePAConcQuery = " AND Pa_Acao_View.Data IS NOT NULL ";
            }

            var Query = "";

            Query = @"


            DELETE CUBO 
            FROM #CUBO CUBO
	        LEFT JOIN (SELECT * FROM Pa_Acao_View WHERE DATA BETWEEN @DATEINI AND @DATEFIM  )Pa_Acao_View
		        ON CUBO.Unidade = Pa_Acao_View.Unidade_Id
		        AND CUBO.Indicador = Pa_Acao_View.Level1Id
	        WHERE 1=1
            
	    	" + wherePAConcQuery + @"
           
            ";

            return Query;
        }

        [HttpPost]
        [Route("GraficoTarefasAcumuladas")]
        public List<NaoConformidadeResultsSet> GraficoTarefasAcumuladas([FromBody] DTO.DataCarrierFormularioNew form)
        {
            var _list = new List<NaoConformidadeResultsSet>();

            var whereDepartment = "";
            var whereShift = "";
            var whereCriticalLevel = "";
            var whereGroupLevel1 = "";
            var whereLevel1 = "";
            var whereUnit = "";
            var WunidadeAcesso = GetUserUnits(int.Parse(form.Param["auditorId"]?.ToString()));

            if (form.Param["departmentId"] != null && form.Param["departmentId"].ToString() != "")
            {
                whereDepartment = "\n AND MON.ParDepartment_Id = " + form.Param["departmentId"].ToString() + " ";
            }

            if (form.Param["departmentId"] != null && form.Param["departmentId"].ToString() != "")
            {
                whereDepartment = "\n AND D.Name = '" + form.Param["departmentId"].ToString() + "'";
            }

            if (form.Param["shift"] != null && form.Param["shift"].ToString() != "")
            {
                whereShift = "\n AND CL1.Shift = " + form.Param["shift"].ToString() + " ";
            }

            if (form.Param["criticalLevelId"] != null && form.Param["criticalLevelId"].ToString() != "")
            {
                whereCriticalLevel = $@"AND IND.Id IN (SELECT P1XC.ParLevel1_Id FROM ParLevel1XCluster P1XC WHERE P1XC.ParCriticalLevel_Id = { form.Param["criticalLevelId"].ToString() })";
            }

            if (form.ParLevel1Group_Ids.Length > 0)
            {
                whereGroupLevel1 = $@" AND IND.Id IN(SELECT Distinct ParLevel1_Id FROM ParGroupParLevel1XParLevel1 WHERE 1=1 AND ParGroupParLevel1_Id in ({ string.Join(",", form.ParLevel1Group_Ids) }))";
            }

            if (form.ParLevel1_Ids.Length > 0)
            {
                whereLevel1 = $@"AND IND.Id IN({ string.Join(",", form.ParLevel1_Ids) })";
            }

            if (form.ParCompany_Ids.Length > 0 && form.ParCompany_Ids != null)
            {
                whereUnit = $@"AND CL2.UNITID IN({ string.Join(",", form.ParCompany_Ids) })";
            }

            var queryGraficoTarefasAcumuladas = $@"
            SELECT
            
            	--IND.Id AS Indicador_id
               --,IND.Name AS IndicadorName
               --,MON.Id AS Monitoramento_Id
               --,MON.Name AS MonitoramentoName
                TAR.ID AS Tarefa_Id
               ,TAR.NAME AS TarefaName
			   ,IIF(count(distinct C2.UnitId) = 1,MAX(C2.UnitId),0) Unidade_Id
			   ,(SELECT TOP 1 Name FROM ParLevel1 WHERE ID = IIF(count(distinct C2.UnitId) = 1,MAX(C2.UnitId),0)) UnidadeName
			   ,IIF(count(distinct C2.ParLevel1_Id) = 1 AND count(distinct C2.ParLevel2_Id) = 1,MAX(C2.ParLevel1_Id),0) Indicador_id
			   ,(SELECT TOP 1 Name FROM ParLevel1 WHERE id = IIF(count(distinct C2.ParLevel1_Id) = 1 AND count(distinct C2.ParLevel2_Id) = 1,MAX(C2.ParLevel1_Id),0)) IndicadorName
			   ,IIF(count(distinct C2.ParLevel1_Id) = 1 AND count(distinct C2.ParLevel2_Id) = 1,MAX(C2.ParLevel2_Id),0) Monitoramento_Id
			   ,(SELECT TOP 1 Name FROM ParLevel1 WHERE id = IIF(count(distinct C2.ParLevel1_Id) = 1 AND count(distinct C2.ParLevel2_Id) = 1,MAX(C2.ParLevel2_Id),0)) MonitoramentoName
               ,CASE 
						WHEN CAST(SUM(R3.WeiDefects) AS INT) = 0 OR CAST(SUM(R3.WeiEvaluation) AS INT) = 0 
						THEN 0 
						WHEN CAST(SUM(R3.WeiDefects) AS INT)>CAST(SUM(R3.WeiEvaluation) AS INT)
						THEN 100
						ELSE ISNULL(NULLIF(SUM(R3.WeiDefects),0) / NULLIF(SUM(R3.WeiEvaluation),0),0) * 100 
				END  AS [PROC]
        	,SUM(R3.WeiEvaluation)
        	 AS [AV]
        	, SUM(R3.WeiDefects)
        	 AS [NC]

            FROM Result_Level3 R3 (NOLOCK)
            INNER JOIN CollectionLevel2 C2 (NOLOCK)
            	ON C2.Id = R3.CollectionLevel2_Id
            INNER JOIN ConsolidationLevel2 CL2 (NOLOCK)
            	ON CL2.Id = C2.ConsolidationLevel2_Id
            INNER JOIN ConsolidationLevel1 CL1 (NOLOCK)
            	ON CL1.Id = CL2.ConsolidationLevel1_Id
            INNER JOIN ParCompany UNI (NOLOCK)
            	ON UNI.Id = CL1.UnitId
            INNER JOIN ParLevel1 IND (NOLOCK)
            	ON IND.Id = CL1.ParLevel1_Id
            INNER JOIN ParLevel2 MON (NOLOCK)
            	ON MON.Id = CL2.ParLevel2_Id
            INNER JOIN ParLevel3 TAR (NOLOCK)
            	ON TAR.Id = R3.ParLevel3_Id
            WHERE 1 = 1 
            { whereLevel1 }
            { whereUnit }
            AND CL2.UnitId in ({WunidadeAcesso}) 
            AND C2.ParLevel1_Id != 43
            AND C2.ParLevel1_Id != 42
			AND IND.IsActive = 1
			AND MON.IsActive = 1
			AND TAR.IsActive = 1
            AND CL2.ConsolidationDate BETWEEN '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}' AND '{ form.endDate.ToString("yyyy-MM-dd") } {" 23:59:59"}'
            { whereGroupLevel1 }
            { whereDepartment }
            { whereShift }            
            { whereCriticalLevel }

            GROUP BY --IND.Id
            		--,IND.Name
            		--,MON.Id
            		--,MON.Name
            		 TAR.ID
            		,TAR.NAME
            		--,UNI.Name
            		--,UNI.Id
            HAVING SUM(R3.WeiDefects) > 0
            AND SUM(R3.Defects) > 0
            ORDER BY 9 DESC
            ";


            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<NaoConformidadeResultsSet>(queryGraficoTarefasAcumuladas).ToList();
            }

            return _list;
        }

        private static DimensaoData getDimensaoData(DTO.DataCarrierFormularioNew form, string nomeColuna)
        {

            DimensaoData D = new DimensaoData();

            D.queryDimensao = $@"";

            int dimensaoData = 0;
            int.TryParse(form.Param["dimensaoData"].ToString(), out dimensaoData);

            if (dimensaoData == 2)
            {
                D.queryDimensao = $@" CONCAT(DATEPART(YEAR,{nomeColuna}),'/',RIGHT(CONCAT(0,DATEPART(WEEK,{nomeColuna})),2))  ";
                D.nomeAlias = $@" AS [SEMANA] ";
            }           
            else if (dimensaoData == 4)
            {
                D.queryDimensao = $@" CONVERT(VARCHAR(7),{nomeColuna},120) ";
                D.nomeAlias = $@" AS [MES] ";
            }
            else
            {
                D.queryDimensao = $@"{ nomeColuna } ";
                D.nomeAlias = $@" AS [DATE] ";
            }


            return D;
        }
    }

    public class RelatorioResultadosPeriodo
    {
        public DateTime Data { get; set; }
        public string Regional { get; set; }
        public int Unidade { get; set; }
        public int Indicador { get; set; }
        public int Monitoramento { get; set; }
        public string MonitoramentoName { get; set; }
        public int Tarefa { get; set; }
        public string TarefaName { get; set; }
        public string UnidadeName { get; set; }
        public string IndicadorName { get; set; }
        public decimal? Av { get; set; }
        public decimal? Nc { get; set; }
        public decimal? avComPeso { get; set; }
        public decimal? ncComPeso { get; set; }
        public decimal? Pc { get; set; }
        public string Historico_Id { get; set; }
        public decimal? Meta { get; set; }
        public int Status { get; set; }
        public string IndicadorUnidade { get; set; }
        public string MonitoramentoUnidade { get; set; }
        public string TarefaUnidade { get; set; }
        public int NumeroAcoesConcluidas { get; set; }
        public string Name { get; set; }
        public string _Data
        {
            get
            {
                return Data.ToString("dd/MM/yyyy");
            }
        }
        public bool IsIndicador { get; set; }
        public bool IsMonitoramento { get; set; }
        public bool IsTarefa { get; set; }
    }

    public class RetornoGenerico
    {

        public decimal? avComPeso { get; set; }
        public decimal? ncComPeso { get; set; }

        public decimal? av { get; set; }
        public string ChartTitle { get; set; }
        public decimal? companyScorecard { get; set; }
        public string companySigla { get; set; }
        public DateTime? date { get; set; }
        public string semana { get; set; }
        public string quinzena { get; set; }
        public string mes { get; set; }
        public int level1Id { get; set; }
        public string level1Name { get; set; }
        public int level2Id { get; set; }
        public string level2Name { get; set; }
        public int level3Id { get; set; }
        public string level3Name { get; set; }
        public decimal? nc { get; set; }
        public decimal? procentagemNc { get; set; }
        public int regId { get; set; }
        public string regName { get; set; }
        public decimal? scorecard { get; set; }
        public decimal? scorecardJbs { get; set; }
        public decimal? scorecardJbsReg { get; set; }
        public string _date
        {
            get
            {
                if (date.HasValue)
                {
                    return date.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public bool haveHistorico { get; set; }
        public decimal? limiteInferior { get; set; }
        public decimal? limiteSuperior { get; set; }
        public string levelName { get; set; }
        public int? UnidadeId { get; set; }
        public string UnidadeName { get; set; }
        public string HISTORICO_ID { get; set; }
        public int? IsPaAcao { get; set; }
        public decimal? Meta { get; set; }
        public string _dateEUA
        {
            get
            {

                if (date.HasValue)
                {
                    return date.Value.ToString("MM-dd-yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public bool IsIndicador { get; set; }
        public bool IsMonitoramento { get; set; }
        public bool IsTarefa { get; set; }
    }

    public class DimensaoData
    {
        public string queryDimensao { get; set; }
        public string nomeAlias { get; set; }
    }
}
