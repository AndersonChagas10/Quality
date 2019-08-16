using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Pa_Planejamento
    {
        public int Id { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? AlterDate { get; set; }
        public int? Diretoria_Id { get; set; }
        public int? Gerencia_Id { get; set; }
        public int? Coordenacao_Id { get; set; }
        public int? Missao_Id { get; set; }
        public int? Visao_Id { get; set; }
        public int? TemaAssunto_Id { get; set; }
        public int? Indicadores_Id { get; set; }
        public int? Iniciativa_Id { get; set; }
        public int? ObjetivoGerencial_Id { get; set; }
        public string Dimensao { get; set; }
        public string Objetivo { get; set; }
        public decimal? ValorDe { get; set; }
        public decimal? ValorPara { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int? Order { get; set; }
        public int? Dimensao_Id { get; set; }
        public int? Objetivo_Id { get; set; }
        public int? IndicadoresDiretriz_Id { get; set; }
        public int? IndicadoresDeProjeto_Id { get; set; }
        public int? Estrategico_Id { get; set; }
        public int? Responsavel_Diretriz { get; set; }
        public int? Responsavel_Projeto { get; set; }
        public int? UnidadeDeMedida_Id { get; set; }
        public bool IsTatico { get; set; }
        public int? Tatico_Id { get; set; }
        public bool? IsFta { get; set; }
        public int? TemaProjeto_Id { get; set; }
        public int? TipoProjeto_Id { get; set; }
    }
}
