using DTO.BaseEntity;
using System.Collections.Generic;

namespace DTO.DTO.Params
{
    public class ParLevel1DTO : EntityBase
    {
        public int ParConsolidationType_Id { get; set; }
        public int ParFrequency_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasSaveLevel2 { get; set; }
        public bool HasNoApplicableLevel2 { get; set; }
        public bool HasGroupLevel2 { get; set; }
        public bool HasAlert { get; set; }
        public bool IsSpecific { get; set; }
        public bool IsSpecificHeaderField { get; set; }
        public bool IsSpecificNumberEvaluetion { get; set; }
        public bool IsSpecificNumberSample { get; set; }
        public bool IsSpecificLevel3 { get; set; }
        public bool IsSpecificGoal { get; set; }
        public bool IsRuleConformity { get; set; }
        public bool IsFixedEvaluetionNumber { get; set; }
        public bool IsLimitedEvaluetionNumber { get; set; }
        public bool IsActive { get; set; } = true;

        public ParConsolidationTypeDTO parConsolidationTypeDto { get; set; }
        public ParFrequencyDTO parFrequencyDto { get; set; }
        public List<ParLevel1HeaderFieldDTO> parLevel1HeaderFieldDto { get; set; }

    }
}