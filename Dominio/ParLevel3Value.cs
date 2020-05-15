namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParLevel3Value : BaseModel
    {
        public int Id { get; set; }

        public int? LimiteNC { get; set; }
        
        public int ParLevel3_Id { get; set; }

        public int ParLevel3InputType_Id { get; set; }

        public int? ParLevel3BoolFalse_Id { get; set; }

        public int? ParLevel3BoolTrue_Id { get; set; }

        public int? ParCompany_Id { get; set; }

        public int? ParMeasurementUnit_Id { get; set; }

        public bool? AcceptableValueBetween { get; set; }

        public decimal? IntervalMin { get; set; }

        public decimal? IntervalMax { get; set; }

        public bool IsActive { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParDepartment_Id { get; set; }

        public int? ParCargo_Id { get; set; }

        public int? ParLevel2_Id { get; set; }

        public string DynamicValue { get; set; }

        public bool ShowLevel3Limits { get; set; }

        public bool? IsRequired { get; set; }

        public bool? IsNCTextRequired { get; set; }

        public bool? IsDefaultAnswer { get; set; }

        public int? ParCluster_Id { get; set; }

        public bool IsAtiveNA { get; set; }

        public string DefaultMessageText { get; set; }

        public int? StringSizeAllowed { get; set; }

        [ForeignKey("ParCluster_Id")]
        public virtual ParCluster ParCluster { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParLevel2_Id")]
        public virtual ParLevel2 ParLevel2 { get; set; }

        [ForeignKey("ParLevel3_Id")]
        public virtual ParLevel3 ParLevel3 { get; set; }

        [ForeignKey("ParLevel3BoolFalse_Id")]
        public virtual ParLevel3BoolFalse ParLevel3BoolFalse { get; set; }

        [ForeignKey("ParLevel3BoolTrue_Id")]
        public virtual ParLevel3BoolTrue ParLevel3BoolTrue { get; set; }

        [ForeignKey("ParLevel3InputType_Id")]
        public virtual ParLevel3InputType ParLevel3InputType { get; set; }

        [ForeignKey("ParMeasurementUnit_Id")]
        public virtual ParMeasurementUnit ParMeasurementUnit { get; set; }

        [NotMapped]
        public List<ParInputTypeValues> ParInputTypeValues { get; set; }
    }
}
