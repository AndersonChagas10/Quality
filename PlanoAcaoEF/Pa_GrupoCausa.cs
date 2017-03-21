namespace PlanoAcaoEF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pa_GrupoCausa
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string GrupoCausa { get; set; }
    }
}
