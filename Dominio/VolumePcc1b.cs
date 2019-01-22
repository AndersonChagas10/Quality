namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VolumePcc1b : BaseModel
    {
        public int Id { get; set; }

        public int? Indicador { get; set; }

        public int? Unidade { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Data { get; set; }

        [StringLength(255)]
        public string Departamento { get; set; }

        public int? VolumeAnimais { get; set; }

        public int? Quartos { get; set; }

        public decimal? Meta { get; set; }

        public float? ToleranciaDia { get; set; }

        public float? Nivel11 { get; set; }

        public float? Nivel12 { get; set; }

        public float? Nivel13 { get; set; }

        public int? Avaliacoes { get; set; }

        public int? Amostras { get; set; }

        public int? ParCompany_id { get; set; }

        public int? ParLevel1_id { get; set; }

        public int? Shift_Id { get; set; }

        [NotMapped]
        public bool IsRetroativo { get; set; }

        [ForeignKey("Shift_Id")]
        public virtual Shift Shift { get; set; }

        [ForeignKey("ParCompany_id")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("ParLevel1_id")]
        public virtual ParLevel1 ParLevel1 { get; set; }
    }
}
