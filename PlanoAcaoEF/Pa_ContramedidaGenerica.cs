namespace PlanoAcaoEF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pa_ContramedidaGenerica
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string ContramedidaGenerica { get; set; }

        public int? CausaGenerica { get; set; }
    }
}
