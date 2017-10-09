using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/RelatorioDeResultados")]
    public class RelatorioDeResultadosApiController : ApiController
    {
        List<RelatorioResultadosPeriodo> retorno;
        List<RetornoGenerico> retorno2;
        List<RelatorioResultados> retorno3;
        List<RelatorioResultadosPeriodo> retorno4;

        public RelatorioDeResultadosApiController()
        {
            retorno = new List<RelatorioResultadosPeriodo>();
            retorno2 = new List<RetornoGenerico>();
            retorno3 = new List<RelatorioResultados>();
            retorno4 = new List<RelatorioResultadosPeriodo>();
        }

        [HttpPost]
        [Route("listaResultadosPeriodoTabela")]
        public List<RelatorioResultadosPeriodo> listaResultadosPeriodoTabela([FromBody] FormularioParaRelatorioViewModel form)
        {            

            //Nenhum Indicador Sem Unidade
            //Nenhum Indicador Com Unidade
            //Nenhum Monitoramento Sem Unidade
            //Nenhum Monitoramento Com Unidade
            //Nenhuma Tarefa Sem Unidade
            //Nenhuma Tarefa Com Unidade
            //Indicador Monitoramento Tarefa Com Unidade
            //Indicador Monitoramento tarefa Sem Unidade
            if (form.level1Id == 0 && form.unitId == 0) //Nenhum Indicador Sem Unidade
            {
                GetMockResultadosPeriodo();
            }
            else if (form.level2Id == 0 && form.unitId == 0) //Nenhum Monitoramento Sem Unidade
            {
                GetMockResultadosPeriodo2();
            }
            else if (form.level3Id == 0 && form.unitId == 0) //Nenhuma Tarefa Com Unidade
            {
                GetMockResultadosPeriodo3();
            }
            else if (form.unitId != 0) //Indicador Monitoramento tarefa Sem Unidade
            {
                GetMockResultadosPeriodo4(); 
            }

            return retorno;
        }

        [HttpPost]
        [Route("GetGraficoHistoricoModal")]
        public List<RetornoGenerico> GetGraficoHistoricoModal([FromBody] FormularioParaRelatorioViewModel form)
        {
            GetMockHistoricoModal();
            return retorno2;
        }

        [HttpPost]
        [Route("listaResultados")]
        public List<RelatorioResultados> listaResultados([FromBody] FormularioParaRelatorioViewModel form)
        {
            GetMockListaResultados();
            return retorno3;
        }

        [HttpPost]
        [Route("listaResultadosAcoesConcluidas")]
        public List<RelatorioResultadosPeriodo> listaResultadosAcoesConcluidas([FromBody] FormularioParaRelatorioViewModel form)
        {
            
            retorno4.Add(new RelatorioResultadosPeriodo { Av = 1, Data = DateTime.Now, Indicador = 1, IndicadorName = "Nome Indicador", Nc = 10, Pc = 10, Meta = 80, Status = 1, NumeroAcoesConcluidas = 10 });
            retorno4.Add(new RelatorioResultadosPeriodo { Av = 2, Data = DateTime.Now, Indicador = 2, IndicadorName = "Nome Indicador", Nc = 10, Pc = 10, Meta = 80, Status = 1, NumeroAcoesConcluidas = 10 });
            retorno4.Add(new RelatorioResultadosPeriodo { Av = 3, Data = DateTime.Now, Indicador = 3, IndicadorName = "Nome Indicador", Nc = 10, Pc = 10, Meta = 80, Status = 1, NumeroAcoesConcluidas = 10 });
            retorno4.Add(new RelatorioResultadosPeriodo { Av = 4, Data = DateTime.Now, Indicador = 4, IndicadorName = "Nome Indicador", Nc = 10, Pc = 10, Meta = 80, Status = 1, NumeroAcoesConcluidas = 10 });
            retorno4.Add(new RelatorioResultadosPeriodo { Av = 5, Data = DateTime.Now, Indicador = 5, IndicadorName = "Nome Indicador", Nc = 10, Pc = 10, Meta = 80, Status = 1, NumeroAcoesConcluidas = 10 });
            return retorno4;
        }

        #region Mocks

        private void GetMockResultadosPeriodo()
        {

            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "(%) NC nas Operações de Esfola - LIN" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "(%) NC nas Operações de Esfola - CGR" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "(%) NC nas Operações de Esfola - MTZ" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "(%) NC CEP Desossa - LIN" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "(%) NC CEP Desossa - CGR" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "(%) NC CEP Desossa - MTZ" });

        }

        private void GetMockResultadosPeriodo2()
        {

            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Sangria", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "Sangria - LIN" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Sangria", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "Sangria - CGR" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Sangria", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "Sangria - MTZ" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Esfola da Pata TRS Esquerda", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "Esfola da Pata TRS Esquerda - LIN" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Esfola da Pata TRS Esquerda", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "Esfola da Pata TRS Esquerda - CGR" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Esfola da Pata TRS Esquerda", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "Esfola da Pata TRS Esquerda - MTZ" });

        }

        private void GetMockResultadosPeriodo3()
        {

            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de contato - quarto esfolado / não esfolado", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "Ausência de contato - quarto esfolado / não esfolado - LIN" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de contato - quarto esfolado / não esfolado", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "Ausência de contato - quarto esfolado / não esfolado - CGR" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de contato - quarto esfolado / não esfolado", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "Ausência de contato - quarto esfolado / não esfolado - MTZ" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de perfurações / contaminação", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "Ausência de perfurações / contaminação - LIN" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de perfurações / contaminação", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "Ausência de perfurações / contaminação - CGR" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "Ausência de perfurações / contaminação", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "Ausência de perfurações / contaminação - MTZ" });

        }

        private void GetMockResultadosPeriodo4()
        {

            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "(%) NC nas Operações de Esfola" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "(%) NC nas Operações de Esfola" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC nas Operações de Esfola", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "(%) NC nas Operações de Esfola" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 1, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "(%) NC CEP Desossa" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 2, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "(%) NC CEP Desossa" });
            retorno.Add(new RelatorioResultadosPeriodo { IndicadorName = "(%) NC CEP Desossa", Indicador = 3, Av = 30, Meta = 80, Nc = 10, Pc = 90, Name = "(%) NC CEP Desossa" });

        }

        private void GetMockHistoricoModal()
        {
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 1, limiteSuperior = 200, nc = 250, date = DateTime.Now });
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 2, limiteSuperior = 200, nc = 200, date = DateTime.Now });
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 3, limiteSuperior = 200, nc = 150, date = DateTime.Now });
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 4, limiteSuperior = 200, nc = 50, date = DateTime.Now });
            retorno2.Add(new RetornoGenerico { ChartTitle = "Histórico: Mock de Grafico Historico", HISTORICO_ID = "1", UnidadeName = "Unidade Mock", levelName = "Level Name Mock", av = 5, limiteSuperior = 200, nc = 350, date = DateTime.Now });
        }

        private void GetMockListaResultados()
        {
            retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 100, Real = 10 });
            retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 100, Real = 10 });
            retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 100, Real = 10 });
            retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 100, Real = 10 });
            retorno3.Add(new RelatorioResultados { Data = DateTime.Now, Unidade = "Unidade Mock", Indicador = "Mock Indicador", LimiteSuperior = 100, LimiteInferior = 0, Sentido = "Maior", Nc = 100, Real = 10 });
        }

        #endregion

    }


    public class RelatorioResultadosPeriodo
    {
        public DateTime Data { get; set; }
        public int Unidade { get; set; }
        public int Indicador { get; set; }
        public string UnidadeName { get; set; }
        public string IndicadorName { get; set; }
        public decimal Av { get; set; }
        public decimal Nc { get; set; }
        public decimal? Pc { get; set; }
        public string Historico_Id { get; set; }
        public decimal Meta { get; set; }
        public int Status { get; set; }
        public string IndicadorUnidade { get; set; }
        public int NumeroAcoesConcluidas { get; set; }
        public string Name { get; set; }
        public string _Data
        {
            get
            {
                return Data.ToString("dd/MM/yyyy");
            }
        }
    }

    public class RetornoGenerico
    {

        public decimal av { get; set; }
        public string ChartTitle { get; set; }
        public decimal companyScorecard { get; set; }
        public string companySigla { get; set; }
        public DateTime date { get; set; }
        public int level1Id { get; set; }
        public string level1Name { get; set; }
        public int level2Id { get; set; }
        public string level2Name { get; set; }
        public int level3Id { get; set; }
        public string level3Name { get; set; }
        public decimal nc { get; set; }
        public decimal procentagemNc { get; set; }
        public int regId { get; set; }
        public string regName { get; set; }
        public decimal scorecard { get; set; }
        public decimal scorecardJbs { get; set; }
        public decimal scorecardJbsReg { get; set; }
        public string _date { get; }
        public bool haveHistorico { get; set; }
        public decimal? limiteInferior { get; set; }
        public decimal? limiteSuperior { get; set; }
        public string levelName { get; set; }
        public string UnidadeName { get; set; }
        public string HISTORICO_ID { get; set; }
        public int? IsPaAcao { get; set; }

    }

    public class RelatorioResultados
    {
        public DateTime Data { get; set; }
        public string Unidade { get; set; }
        public string Indicador { get; set; }
        public decimal? LimiteInferior { get; set; }
        public decimal? LimiteSuperior { get; set; }
        public string Sentido { get; set; }
        public decimal? Nc { get; set; }
        public decimal? Real { get; set; }
        public string Historico_Id { get; set; }
        public int NumeroAcoesConcluidas { get; set; }
        public string _Data
        {
            get
            {
                return Data.ToString("yyyy-MM-dd");
            }
        }
    }
}
