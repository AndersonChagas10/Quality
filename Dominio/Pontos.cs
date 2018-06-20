namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pontos
    {
        public int Id { get; set; }

        public int Cluster { get; set; }

        public int Operacao { get; set; }

        [Column("Pontos")]
        public decimal Pontos1 { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        [StringLength(20)]
        public string Nivel { get; set; }

        public virtual Clusters Clusters { get; set; }

        public virtual Operacoes Operacoes { get; set; }
    }
}
