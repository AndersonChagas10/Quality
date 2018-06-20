namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pa_ObjetivoGeral
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? Order { get; set; }

        public int? Pa_IndicadoresDeProjeto_Id { get; set; }

        public bool IsActive { get; set; }
    }
}
