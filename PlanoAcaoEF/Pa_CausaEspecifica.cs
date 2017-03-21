namespace PlanoAcaoEF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pa_CausaEspecifica
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Text { get; set; }
    }
}
