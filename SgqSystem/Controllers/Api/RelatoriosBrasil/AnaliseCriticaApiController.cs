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
        public List<HistoricoUnidade> GetGraficoHistoricoUnidade([FromBody] FormularioParaRelatorioViewModel form)
        {

            var retornoHistoricoUnidade = new List<HistoricoUnidade>();

            retornoHistoricoUnidade.Add(new HistoricoUnidade() { Av = 1, AvComPeso = 1, Date = DateTime.Now, Mes = "2019-06", Nc = 1, NcComPeso = 1, PorcentagemNc = 100, Semana = "2019/1" });
            retornoHistoricoUnidade.Add(new HistoricoUnidade() { Av = 2, AvComPeso = 1, Date = DateTime.Now, Mes = "2019-06", Nc = 2, NcComPeso = 1, PorcentagemNc = 100, Semana = "2019/2" });
            retornoHistoricoUnidade.Add(new HistoricoUnidade() { Av = 3, AvComPeso = 1, Date = DateTime.Now, Mes = "2019-07", Nc = 3, NcComPeso = 1, PorcentagemNc = 100, Semana = "2019/4" });
            retornoHistoricoUnidade.Add(new HistoricoUnidade() { Av = 4, AvComPeso = 1, Date = DateTime.Now, Mes = "2019-07", Nc = 4, NcComPeso = 1, PorcentagemNc = 100, Semana = "2019/4" });
            retornoHistoricoUnidade.Add(new HistoricoUnidade() { Av = 5, AvComPeso = 1, Date = DateTime.Now, Mes = "2019-07", Nc = 5, NcComPeso = 1, PorcentagemNc = 100, Semana = "2019/4" });

            return retornoHistoricoUnidade;
        }

        [HttpPost]
        [Route("GetGraficoTendenciaIndicador")]
        public List<TendenciaResultSet> GetGraficoTendenciaIndicador([FromBody] FormularioParaRelatorioViewModel form)
        {

            var retornoHistoricoUnidade = new List<TendenciaResultSet>();

            retornoHistoricoUnidade.Add(new TendenciaResultSet() { Av = 1, Av_Peso = 1, Level1Name = "Indicador 1", level1_Id = 1, NC = 1, NC_Peso = 1,  });


            return retornoHistoricoUnidade;
        }
    }

    public class HistoricoUnidade
    {
        public decimal? Nc { get; set; }
        public decimal? PorcentagemNc { get; set; }
        public decimal? Av { get; set; }
        public decimal? AvComPeso { get; set; }
        public decimal? NcComPeso { get; set; }
        public string Semana { get; set; }
        public string Mes { get; set; }
        public DateTime? Date { get; set; }
        public string _Date
        {
            get
            {
                if (Date.HasValue)
                {
                    return Date.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string _DateEUA
        {
            get
            {

                if (Date.HasValue)
                {
                    return Date.Value.ToString("MM-dd-yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }

    public class TendenciaResultSet
    {
        public int? level1_Id { get; set; }
        public string Level1Name { get; set; }
        public int? level2_Id { get; set; }
        public string Level2Name { get; set; }
        public int? level3_Id { get; set; }
        public string Level3Name { get; set; }
        public int? Unidade_Id { get; set; }
        public string Unidade { get; set; }
        public decimal Meta { get; set; }
        public decimal ProcentagemNc { get; set; }
        public decimal Av { get; set; }
        public decimal Av_Peso { get; set; }
        public decimal NC { get; set; }
        public decimal NC_Peso { get; set; }
        public double Data { get { return _Data.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds; } }
        public DateTime _Data { get; set; }
    }

}