namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ResultadosPCC")]
    public partial class ResultadosPCC
    {
        public int Id { get; set; }

        public string Chave { get; set; }

        public int EmpresaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Empresa { get; set; }

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

        public int TarefaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Tarefa { get; set; }

        public int NumeroAvaliacao { get; set; }

        public int NumeroAmostra { get; set; }

        public int MonitoramentoId { get; set; }

        [StringLength(300)]
        public string Monitoramento { get; set; }

        public int ProdutoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Produto { get; set; }

        public DateTime DataHora { get; set; }

        public int Monitor { get; set; }

        public int? Peso { get; set; }

        [StringLength(50)]
        public string Lote { get; set; }

        public int? PecasAvaliadas { get; set; }

        [Required]
        [StringLength(20)]
        public string Minimo { get; set; }

        [Required]
        [StringLength(20)]
        public string Maximo { get; set; }

        [Required]
        [StringLength(20)]
        public string Acesso { get; set; }

        [Required]
        [StringLength(20)]
        public string Avaliacao_1 { get; set; }

        public int Avaliacao_2 { get; set; }

        public decimal? Meta { get; set; }

        public int? Sequencial { get; set; }

        public int? Banda { get; set; }

        public DateTime? DataHoraMonitor { get; set; }

        public int? ToleranciaDia { get; set; }

        public int? Nivel1 { get; set; }

        public int? Nivel2 { get; set; }

        public int? Nivel3 { get; set; }

        [StringLength(100)]
        public string Status { get; set; }

        public bool? AvaliacaoAvulsa { get; set; }

        [StringLength(20)]
        public string Amostragem { get; set; }

        [StringLength(100)]
        public string FormaAmostragem { get; set; }

        [StringLength(50)]
        public string Sexo { get; set; }

        public int? Idade { get; set; }

        [StringLength(5)]
        public string SiglaContusao { get; set; }

        [StringLength(5)]
        public string SiglaFalhaOperacional { get; set; }

        public DateTime? DataTipificacao { get; set; }

        public bool? Mobile { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Data { get; set; }

        [Required]
        [StringLength(450)]
        public string Identificador { get; set; }

        [StringLength(100)]
        public string VersaoAPP { get; set; }
    }
}
