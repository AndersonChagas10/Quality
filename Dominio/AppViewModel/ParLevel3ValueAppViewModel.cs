namespace Dominio.AppViewModel
{
    public class ParLevel3ValueAppViewModel
    {
        public int Id { get; set; }

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

        public bool IsAtiveNA { get; set; }

        public bool? IsRequired { get; set; }

        public int IsRequiredInt
        {
            get
            {
                return IsRequired == true ? 1 : 0;
            }
        }

        public bool? IsDefaultAnswer { get; set; }

        public int IsDefaultAnswerInt
        {
            get
            {
                return IsDefaultAnswer == false ? 0 : 1;
            }
        }

        public int? ParLevel1_Id { get; set; }

        public int? ParLevel2_Id { get; set; }

        public string DynamicValue { get; set; }

        public bool ShowLevel3Limits { get; set; }
    }
}
