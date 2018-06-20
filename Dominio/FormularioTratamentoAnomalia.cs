namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FormularioTratamentoAnomalia")]
    public partial class FormularioTratamentoAnomalia
    {
        public int Id { get; set; }

        public int Unidade { get; set; }

        public int Operacao { get; set; }

        public int Monitoramento { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataInsercao { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataInicio { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataFim { get; set; }

        public int Tarefa { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Versao { get; set; }

        public int Supervisor { get; set; }

        public int Gerente { get; set; }
    }
}
