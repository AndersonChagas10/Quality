namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class fa_GrupoCausa
    {
        public int Id { get; set; }

        public int IdFormularioTratamentoAnomalia { get; set; }

        public int IdGrupoCausa { get; set; }

        [Required]
        [MaxLength(8)]
        public byte[] Versao { get; set; }

        public virtual GrupoCausa GrupoCausa { get; set; }
    }
}
