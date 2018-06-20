namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class fa_ContramedidaGenerica
    {
        public int Id { get; set; }

        public int IdFormularioTratamentoAnomalia { get; set; }

        public int IdContramedidaGenerica { get; set; }

        public int Prioridade { get; set; }

        [Required]
        [MaxLength(8)]
        public byte[] Versao { get; set; }

        public virtual ContramedidaGenerica ContramedidaGenerica { get; set; }
    }
}
