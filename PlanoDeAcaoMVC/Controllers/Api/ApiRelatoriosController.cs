using DTO.Helpers;
using Newtonsoft.Json.Linq;
using PlanoAcaoCore;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Relatorios")]
    public class ApiRelatoriosController : BaseApiController
    {
        PlanoAcaoEF.PlanoDeAcaoEntities db;
        public ApiRelatoriosController()
        {
            db = new PlanoAcaoEF.PlanoDeAcaoEntities();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.AutoDetectChangesEnabled = false;
        }

        //Retorna Categorias Series e Dados
        [HttpGet]
        [Route("GetGrafico")]
        public List<JObject> GetGrafico([FromBody] filtros filtro)
        {
            //var query = "SELECT ACAO.* ,                                                                   " +
            //            "\n  STA.*,                                                                        " +
            //            "\n  UN.*,                                                                         " +
            //            "\n  DPT.*,                                                                        " +
            //            "\n  Q.*,                                                                          " +
            //            "\n  CG.*,                                                                         " +
            //            "\n  CMG.*,                                                                        " +
            //            "\n  GC.*,                                                                          " +
            //            "\n  PLANE.*                                                                          " +
            //            "\n  FROM pa_acao ACAO                                                             " +
            //            "\n  LEFT JOIN Pa_Unidade UN ON UN.Id = ACAO.Unidade_Id                            " +
            //            "\n LEFT JOIN Pa_Departamento DPT ON DPT.Id = ACAO.Departamento_Id                 " +
            //            "\n  LEFT JOIN Pa_Quem Q ON Q.Id = ACAO.Quem_Id                                    " +
            //            "\n  LEFT JOIN Pa_CausaGenerica CG ON CG.Id = ACAO.CausaGenerica_Id                " +
            //            "\n  LEFT JOIN Pa_ContramedidaGenerica CMG ON CMG.Id = ACAO.ContramedidaGenerica_Id" +
            //            "\n  LEFT JOIN Pa_GrupoCausa GC ON GC.Id = ACAO.GrupoCausa_Id                      " +
            //            "\n  LEFT JOIN Pa_Planejamento PLANE ON PLANE.Id = ACAO.Panejamento_Id "+ 
            //            "\n LEFT JOIN Pa_Status STA ON STA.Id = ACAO.[Status]; ";

            var query = "SELECT Acao.Id, Sta.Name AS [Status], ISNULL(Quem.Name, 'Não Atribuido') AS Responsavel" +
                        "--, cast(Acao.QuandoInicio As Date) AS QuandoInicio" +
                        "--, cast(Acao.QuandoFim As Date) AS QuandoFim" +
                        "\n FROM Pa_Acao AS Acao                            " +
                        "\n LEFT JOIN Pa_Status Sta ON Sta.Id = Acao.Status " +
                        "\n LEFT JOIN Pa_Quem Quem ON Quem.Id = Acao.Quem_Id";

            dynamic results = QueryNinja(db, query);

            return results;
            //return retorno;
        }

        //Retorna apenas Series e Dados
        [HttpPost]
        [Route("GraficoPie")]
        public List<JObject> GraficoPie([FromBody] filtros filtro)
        {
            var dataInicio = Guard.ParseDateToSqlV2(filtro.dataInicio, Guard.CultureCurrent.BR).ToString("yyyyMMdd");
            var dataFim = Guard.ParseDateToSqlV2(filtro.dataFim, Guard.CultureCurrent.BR).ToString("yyyyMMdd");
            var query = "SELECT COUNT(s.id) AS y, s.Name AS name" +
                        "\n FROM pa_acao acao INNER JOIN pa_status s ON acao.Status = s.Id" +
                        "\n WHERE acao.AddDate BETWEEN ('" + dataInicio + " 00:00:00') AND('" + dataFim + "  23:59:59')" +
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
