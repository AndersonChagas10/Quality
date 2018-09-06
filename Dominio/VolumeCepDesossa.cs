namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VolumeCepDesossa")]
    public partial class VolumeCepDesossa
    {
        public int Id { get; set; }

        public int? Indicador { get; set; }

        public int? Unidade { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Data { get; set; }

        [StringLength(255)]
        public string Departamento { get; set; }

        public int? HorasTrabalhadasPorDia { get; set; }

        public int? AmostraPorDia { get; set; }

        public int? QtdadeFamiliaProduto { get; set; }

        public int? Avaliacoes { get; set; }

        public int? Amostras { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public int? ParCompany_id { get; set; }

        public int? ParLevel1_id { get; set; }

        public int? Shift_Id { get; set; }

        public virtual Shift Shift { get; set; }

        public virtual ParCompany ParCompany { get; set; }

        public virtual ParLevel1 ParLevel1 { get; set; }
    }
}
