using Newtonsoft.Json.Linq;
using PlanoAcaoCore;
using PlanoAcaoEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/ApiGerenciar")]
    public class ApiGerenciarController : BaseApiController
    {
        string dtInit;
        string dtFim;
           
        private PlanoDeAcaoEntities db;
        public ApiGerenciarController()
        {
            db = new PlanoDeAcaoEntities();
        }
        [HttpGet]
        [Route("List")]
        public List<JObject> List(JObject filtro)
        {
            //GetParamsPeloFiltro(filtro, out dtInit, out dtFim);
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
                "\n  ISNULL(Coord.Name, 'Não possui Coordenação') AS 'Coordenação'";
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
            "\n LEFT JOIN Pa_Coordenacao Coord ON Coord.Id = PlanTatico.Coordenacao_Id";
            //"\n WHERE Acao.AddDate BETWEEN ('" + dtInit + "') AND('" + dtFim + "')";


            return QueryNinja(db, query);
        }
    }
}
