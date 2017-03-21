namespace PlanoAcaoEF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pa_Planejamento
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddDate { get; set; }

        [Column(TypeName = "datetime2")]
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

        [Column(TypeName = "datetime2")]
        public DateTime? DataInicio { get; set; }

        [Column(TypeName = "datetime2")]
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
    }
}
