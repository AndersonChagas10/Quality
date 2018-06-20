namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParGroupParLevel1
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ParGroupParLevel1Type_Id { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime AddDate { get; set; }

        public DateTime? AlterDate { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool IsActive { get; set; }
    }
}
