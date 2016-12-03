using DTO.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace DTO.DTO.Params
{
    public class ParLevel3ValueDTO : EntityBase
    {
        public int ParLevel3_Id { get; set; }

        //[Range(0, 9999999999, ErrorMessage = "Tipo de Dados de Entrada deve ser selecionado.")]
        public int ParLevel3InputType_Id { get; set; }

        public int? ParLevel3BoolFalse_Id { get; set; }
        public int? ParLevel3BoolTrue_Id { get; set; }
        public int? ParCompany_Id { get; set; }
        public int? ParMeasurementUnit_Id { get; set; }
        public bool? AcceptableValueBetween { get; set; }

        [Range(-999999999.99, 999999999.99)]
        public decimal? IntervalMin { get; set; } = 0;
        [Range(-999999999.99, 999999999.99)]
        public decimal? IntervalMax { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        //public ParCompanyDTO parCompany { get; set; }
        //public ParLevel3DTO parLevel3 { get; set; }
        public ParCompanyDTO ParCompany { get; set; }
        public ParLevel3BoolFalseDTO ParLevel3BoolFalse { get; set; }
        public ParLevel3BoolTrueDTO ParLevel3BoolTrue { get; set; }
        public ParLevel3InputTypeDTO ParLevel3InputType { get; set; }
        public ParMeasurementUnitDTO ParMeasurementUnit { get; set; }

        public void preparaParaInsertEmBanco()
        {
            if (this.ParLevel3InputType_Id == 3)
            {
                this.ParLevel3BoolFalse_Id = null;
                this.ParLevel3BoolTrue_Id = null;
            }

            if (this.ParLevel3InputType_Id == 1)
            {
                this.IntervalMax = 0;
                this.IntervalMin = 0;
                this.AcceptableValueBetween = null;
                this.ParMeasurementUnit_Id = null;
            }

            if (this.ParCompany_Id <= 0)
                this.ParCompany_Id = null;

        }
    }
}