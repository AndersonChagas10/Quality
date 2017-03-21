namespace PlanoAcaoEF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pa_CausaGenerica
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string CausaGenerica { get; set; }

        public int? GrupoCausa { get; set; }
    }
}
