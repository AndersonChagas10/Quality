namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Horarios
    {
        [Key]
        public int HorarioId { get; set; }

        public int? UnidadeId { get; set; }

        public int OperacaoId { get; set; }

        public TimeSpan Hora { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public virtual Operacoes Operacoes { get; set; }

        public virtual Unidades Unidades { get; set; }
    }
}
