namespace PlanoAcaoEF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pa_Query
    {
        public int Id { get; set; }

        [Required]
        public string Query { get; set; }

        [Required]
        public string Desc { get; set; }

        [Required]
        public string FastKey { get; set; }

        public bool IsActive { get; set; }
    }
}
