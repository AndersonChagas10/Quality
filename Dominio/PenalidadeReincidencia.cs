namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PenalidadeReincidencia")]
    public partial class PenalidadeReincidencia
    {
        public int PenalidadeReincidenciaId { get; set; }

        public int UnidadeId { get; set; }

        public int DepartamentoId { get; set; }

        public int OperacaoId { get; set; }

        public int TarefaId { get; set; }

        public int MonitoramentoId { get; set; }

        public DateTime Data { get; set; }

        public int Indice { get; set; }
    }
}
