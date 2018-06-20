namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tipificacao")]
    public partial class Tipificacao
    {
        [Key]
        [Column(Order = 0)]
        public DateTime Data { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string Unidade { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Sequencial { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Banda { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(100)]
        public string Sexo { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idade { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(100)]
        public string Tarefa { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Padrao { get; set; }
    }
}
