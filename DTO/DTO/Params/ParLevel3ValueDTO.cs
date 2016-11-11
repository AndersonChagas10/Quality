using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParLevel3ValueDTO : EntityBase
    {
        public int ParLevel3_Id { get; set; }
        public int ParLevel3InputType_Id { get; set; }
        public int? ParLevel3BoolFalse_Id { get; set; }
        public int? ParLevel3BoolTrue_Id { get; set; }
        public int ParCompany_Id { get; set; }
        public int ParMeasurementUnit_Id { get; set; }
        public bool? AcceptableValueBetween { get; set; }
        public decimal? IntervalMin { get; set; }
        public decimal? IntervalMax { get; set; }
        public bool IsActive { get; set; } = true;

        //public ParCompanyDTO parCompany { get; set; }
        //public ParLevel3DTO parLevel3 { get; set; }
        public ParLevel3BoolFalseDTO ParLevel3BoolFalse { get; set; }
        public ParLevel3BoolTrueDTO ParLevel3BoolTrue { get; set; }
        public ParLevel3InputTypeDTO ParLevel3InputType { get; set; }
        public ParMeasurementUnitDTO ParMeasurementUnit { get; set; }

    }
}