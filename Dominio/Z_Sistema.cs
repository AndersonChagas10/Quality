namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Z_Sistema
    {
        public int Id { get; set; }

        public int IntervaloAtualizacao { get; set; }

        public int UsuarioInsercao { get; set; }

        public DateTime DataInsercao { get; set; }

        public int? UsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        [Required]
        [StringLength(20)]
        public string VersaoDB { get; set; }

        [StringLength(100)]
        public string ArquivoAtualizacao { get; set; }

        [StringLength(100)]
        public string VersaoAPP { get; set; }

        public DateTime? Atualizacao { get; set; }
    }
}
