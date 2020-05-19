namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IntegracaoSistemica")]
    public partial class IntegracaoSistemica : BaseModel
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Configuration { get; set; }

        public string Script { get; set; }

        [StringLength(50)]
        public string TableName { get; set; }

        public int Intervalo { get; set; }

        [NotMapped]
        public DateTime? LastExecution { get; set; }

        [NotMapped]
        public string ExecutionTime { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
