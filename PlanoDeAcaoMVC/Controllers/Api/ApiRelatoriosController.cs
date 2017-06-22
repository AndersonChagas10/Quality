using DTO.Helpers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
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
        }
        

        //Retorna Categorias Series e Dados
        [HttpPost]
        [Route("GetGrafico")]
        public List<JObject> GetGrafico(JObject filtro)
        {
            string dtInit;
            string dtFim;
            GetParamsPeloFiltro(filtro, out dtInit, out dtFim);

            var query = "SELECT Acao.Id, Sta.Name AS[Status]," +
            "\n  ISNULL(Quem.Name, 'Não possui Responsável') AS Responsavel," +
            "\n  ISNULL(ContraMedGen.ContramedidaGenerica, 'Não possui Contramedida Genérica') AS 'Contramedida Genérica'," +
            "\n  ISNULL(CausaGen.CausaGenerica, 'Não possui Causa Genérica') AS 'Causa Genérica'," +
            "\n  ISNULL(GrpCausa.GrupoCausa, 'Não possui Grupo Causa') AS 'Grupo Causa'," +
            "\n  ISNULL(Un.Description, 'Corporativo') AS 'Unidade'," +
            "\n  ISNULL(IndicDir.Name, 'Não possui Indicadores Diretriz') AS 'Indicadores Diretriz'," +
            "\n  ISNULL(DiretObj.Name, 'Não possui Diretriz / Objetivo') AS 'Diretriz / Objetivo'," +
            "\n  ISNULL(Diretor.Name, 'Não possui Diretoria') AS 'Diretoria'," +
            "\n  ISNULL(Dimens.Name, 'Não possui Dimensão') AS 'Dimensão'," +
            "\n  ISNULL(Gere.Name, 'Não possui Gerência') AS 'Gerência'," +
            "\n  ISNULL(Coord.Name, 'Não possui Coordenação') AS 'Coordenação'" +
            "\n FROM Pa_Acao AS Acao" +
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
            "\n WHERE Acao.AddDate BETWEEN ('" + dtInit + "') AND('" + dtFim + "')";

            dynamic results = QueryNinja(db, query);

            return results;
        }

        private void GetParamsPeloFiltro(JObject filtro, out string dtInit, out string dtFim)
        {
            dynamic filtroObj = filtro;
            string startDate = filtroObj.startDate;
            string endDate = filtroObj.endDate;
            dtInit = Guard.ParseDateToSqlV2(startDate, Guard.CultureCurrent.BR).ToString("yyyy-MM-dd 00:00:00");
            dtFim = Guard.ParseDateToSqlV2(endDate, Guard.CultureCurrent.BR).ToString("yyyy-MM-dd 23:59:59");
        }

        //Retorna apenas Series e Dados
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
                "\n group by  DATEPART(mm,QuandoInicio)";

            var items = QueryNinja(db, query);

            return items;
        }

        [HttpPost]
        [Route("NumeroDeAcoesConcluidas")]
        public List<JObject> NumeroDeAcoesConcluidas(JObject form)
        {
            dynamic teste = form;

            var query = "SELECT DATEPART(mm,QuandoInicio) as Mes," +
                "\n Count(id) as Quantidade " +
                "\n FROM  [Pa_Acao] " +
                "\n where [Status] in (4,3) " +
                "\n group by  DATEPART(mm,QuandoInicio)";

            var items = QueryNinja(db, query);

            return items;
        }

        [HttpPost]
        [Route("NumeroDeAcoes")]
        public List<JObject> NumeroDeAcoes(JObject form)
        {
            dynamic teste = form;

            var query = "SELECT A.*, B.MesConcluidas, IsNull(B.QuantidadeConcluidas, 0) as QuantidadeConcluidas, (A.QuantidadeIniciadas - IsNull(B.QuantidadeConcluidas, 0)) as Acc" +
                        "\n FROM" +
                        "\n (SELECT DATEPART(mm, QuandoInicio) as MesIniciadas, Count(id) as QuantidadeIniciadas FROM [Pa_Acao]  group by  DATEPART(mm, QuandoInicio)) A" +
                        "\n LEFT JOIN(SELECT DATEPART(mm, QuandoInicio) as MesConcluidas, Count(id) as QuantidadeConcluidas FROM [Pa_Acao] where[Status] in (4, 3)  group by  DATEPART(mm, QuandoInicio)) B on A.MesIniciadas = B.MesConcluidas";

            var items = QueryNinja(db, query);

            return items;
        }

        [HttpPost]
        [Route("EvolucaoDoResultado")]
        public List<JObject> EvolucaoDoResultado(JObject form)
        {
            dynamic teste = form;

            var query = "SELECT A.*, B.MesConcluidas, IsNull(B.QuantidadeConcluidas, 0) as QuantidadeConcluidas, (A.QuantidadeIniciadas - IsNull(B.QuantidadeConcluidas, 0)) as Acc, 20 as Meta" +
                        "\n FROM" +
                        "\n (SELECT DATEPART(mm, QuandoInicio) as MesIniciadas, Count(id) as QuantidadeIniciadas FROM [Pa_Acao]  group by  DATEPART(mm, QuandoInicio)) A" +
                        "\n LEFT JOIN(SELECT DATEPART(mm, QuandoInicio) as MesConcluidas, Count(id) as QuantidadeConcluidas FROM [Pa_Acao] where[Status] in (4, 3)  group by  DATEPART(mm, QuandoInicio)) B on A.MesIniciadas = B.MesConcluidas";

            var items = QueryNinja(db, query);

            return items;
        }

    }

    public class GraficoPieSet
    {
        public string name { get; set; }
        public int y { get; set; }
    }

    public class RetornoInt
    {
        public int valor { get; set; }
    }

    public class RetornoGrafico1
    {
        public string Indicador { get; set; }
        public int Indicador_Id { get; set; }
        public List<Status> Status { get; set; }
    }

    public class Status
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<int> QuantidadeAcoes { get; set; }
    }

    /// <summary>
    /// Json : 
    /// { 
    ///     diretoria: 1,
    ///     gerencia: 2,
    ///     dataInicio: '20/01/2017'
    ///     dataFim: '20/01/2017'
    /// }
    /// </summary>
    public class filtros
    {
        public int diretoria { get; set; }
        public int gerencia { get; set; }
        public string dataInicio { get; set; }
        public string dataFim { get; set; }
        public string categoria { get; set; }
        public string serie { get; set; }
        public string[] filtroCategoria { get; set; }
        public int relatorio { get; set; }
    }

}
