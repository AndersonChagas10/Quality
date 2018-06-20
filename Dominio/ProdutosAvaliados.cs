namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProdutosAvaliados
    {
        public int Id { get; set; }

        public string Chave { get; set; }

        [Required]
        [StringLength(100)]
        public string Produto { get; set; }

        public TimeSpan? HorarioPesagem { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataPesagem { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }
    }
}
