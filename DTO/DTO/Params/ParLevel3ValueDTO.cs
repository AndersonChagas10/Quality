using DTO.BaseEntity;
using DTO.Helpers;

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

        public decimal? IntervalMin { get; set; } = 0;
        public decimal? IntervalMax { get; set; } = 0;

        public string IntervalMinCalculado { get; set; }
        public string IntervalMaxCalculado { get; set; }


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
            if (ParLevel3InputType_Id == 3 || ParLevel3InputType_Id == 4)
            {
                ParLevel3BoolFalse_Id = null;
                ParLevel3BoolTrue_Id = null;
            }

            if (ParLevel3InputType_Id == 1)
            {
                IntervalMax = 0;
                IntervalMin = 0;
                AcceptableValueBetween = null;
                ParMeasurementUnit_Id = null;
            }

            if (!string.IsNullOrEmpty(IntervalMaxCalculado) && IntervalMax == 0)
                IntervalMax = Guard.ConverteValorCalculado(IntervalMaxCalculado);

            if(!string.IsNullOrEmpty(IntervalMinCalculado) && IntervalMin == 0)
                IntervalMin = Guard.ConverteValorCalculado(IntervalMinCalculado);

            if (ParCompany_Id <= 0)
                ParCompany_Id = null;

        }

        public void PreparaGet()
        {
            if (ParLevel3InputType_Id == 4)
            {
                if (IntervalMax.HasValue && IntervalMin.HasValue)
                {
                    IntervalMinCalculado = Guard.ConverteValorCalculado(IntervalMin.Value);
                    IntervalMaxCalculado = Guard.ConverteValorCalculado(IntervalMax.Value);
                }
            }
        }
    }
}