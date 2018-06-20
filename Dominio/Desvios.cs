namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Desvios
    {
        public int Id { get; set; }

        public DateTime DataHora { get; set; }

        public int UnidadeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Unidade { get; set; }

        public int DepartamentoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Departamento { get; set; }

        public int OperacaoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Operacao { get; set; }

        public int NumeroAvaliacao { get; set; }

        public int Desvio { get; set; }

        public int? MailItemId { get; set; }

        public decimal? Meta { get; set; }

        public decimal? Real { get; set; }

        public int? TarefaId { get; set; }

        public int? NumeroAmostra { get; set; }

        public int? AlertaEmitido { get; set; }
    }
}
