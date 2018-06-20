namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pa_Acompanhamento
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int Acao_Id { get; set; }

        public int? Order { get; set; }

        public int Status_Id { get; set; }

        public int Author_Id { get; set; }
    }
}
