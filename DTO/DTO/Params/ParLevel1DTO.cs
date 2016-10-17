using DTO.BaseEntity;
using DTO.Helpers;

namespace DTO.DTO.Params
{
    public class ParLevel1DTO : EntityBase
    {
        public string _ParConsolidationType_Id { get; set; }  //ok
        public int ParConsolidationType_Id
        {
            get
            {
                return Guard.ConverteValor<int>(_ParConsolidationType_Id);
            }
            set
            {
                ParConsolidationType_Id = value;
            }
        }

        public string _ParFrequency_Id { get; set; } //ok
        public int ParFrequency_Id
        {
            get
            {
                return Guard.ConverteValor<int>(_ParFrequency_Id);
            }
            set
            {
                ParConsolidationType_Id = value;
            }
        }

        public string Name { get; set; } //ok
        public string Description { get; set; } //ok
        public bool SaveLevel2 { get; set; } //ok
        public bool NoApplicableLevel2 { get; set; } //ok
        public bool GroupLevel2 { get; set; } //ok
        public bool Alert { get; set; }
        public bool Specific { get; set; }
        public bool SpecificHeaderField { get; set; }
        public bool SpecificNumberEvaluetion { get; set; }
        public bool SpecificNumberSample { get; set; }
        public bool SpecificLevel3 { get; set; }
        public bool SpecificGoal { get; set; }
        public bool Active { get; set; } = true;

        public ParConsolidationTypeDTO parConsolidationTypeDto { get; set; }
        public ParFrequencyDTO parFrequencyDto { get; set; }

    }
}