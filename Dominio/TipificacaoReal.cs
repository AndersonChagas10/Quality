namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipificacaoReal")]
    public partial class TipificacaoReal
    {
        public int Id { get; set; }

        public int Operacao { get; set; }

        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        public int Unidade { get; set; }

        public decimal PercReal { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public virtual Operacoes Operacoes { get; set; }

        public virtual Unidades Unidades { get; set; }
    }
}
