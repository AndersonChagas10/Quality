namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel3Value_Outer
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AlterDate { get; set; }

        public bool IsActive { get; set; }

        public int ParLevel3_Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(200)]
        public string ParLevel3_Name { get; set; }

        public int ParLevel3InputType_Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(200)]
        public string ParLevel3InputType_Name { get; set; }

        public int? ParCompany_Id { get; set; }

        [StringLength(200)]
        public string ParCompany_Name { get; set; }

        public int? ParMeasurementUnit_Id { get; set; }

        [StringLength(200)]
        public string ParMeasurementUnit_Name { get; set; }

        public int? OuterEmpresa_Id { get; set; }

        [StringLength(200)]
        public string OuterEmpresa_Text { get; set; }

        public int? OuterLevel3_Id { get; set; }

        [StringLength(200)]
        public string OuterLevel3_Text { get; set; }

        public int? OuterLevel3Value_Id { get; set; }

        [StringLength(200)]
        public string OuterLevel3Value_Text { get; set; }

        public int? OuterLevel3ValueIntervalMaxValue { get; set; }

        public int? OuterLevel3ValueIntervalMinValue { get; set; }

        [StringLength(200)]
        public string Operator { get; set; }

        public int? Order { get; set; }

        public int? UnidadeMedida_Id { get; set; }

        [StringLength(200)]
        public string UnidadeMedidaText { get; set; }

        [StringLength(200)]
        public string AceitavelEntreText { get; set; }

        public int? AceitavelEntre_Id { get; set; }

        public decimal? LimInferior { get; set; }

        public decimal? LimSuperior { get; set; }
    }
}
