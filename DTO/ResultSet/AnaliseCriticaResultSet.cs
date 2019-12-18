using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AnaliseCriticaResultSet
    {
        public int ParLevel1_Id { get; set; }
        public string ParLevel1_Name { get; set; }
        public List<TendenciaResultSet> ListaTendenciaResultSet { get; set; } //Aqui tem tabela de acoes
        //public List<GraficoNC> Monitoramentos { get; set; }
        //public List<GraficosNC> MonitoramentosDepartamentos { get; set; } //Aqui tem tabela
        //public List<GraficoNC> TarefasAcumuladas { get; set; }
        //public List<GraficosNC> Tarefas { get; set; } //Aqui tem tabela

    }

    public class HistoricoUnidades
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
        public decimal PorcentagemNc { get; set; }
        public decimal Av { get; set; }
        public decimal Av_Peso { get; set; }
        public decimal NC { get; set; }
        public decimal NC_Peso { get; set; }
        //public double Data { get { return _Data.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds; } }
        public string _Data { get; set; }

        //Aqui dentro colocar a lista de Monitoramentos de cada indicador
        public List<AcaoResultSet> Acoes { get; set; }
    }

    public class AcaoResultSet
    {

    }

    public class GraficosNC
    {
        public List<GraficoNC> GraficosDeNC { get; set; }
    }

    public class GraficoNC
    {
        public int AV { get; set; }
        public int NC { get; set; }
        public decimal NCPercent { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ChartName { get; set; }
        public List<AcaoResultSet> Acoes { get; set; }
    }

}
