using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParLevel1DTO : EntityBase
    {
        public int ParConsolidationType_Id { get; set; }
        public int ParFrequency_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool SaveLevel2 { get; set; }
        public bool NoApplicableLevel2 { get; set; }
        public bool GroupLevel2 { get; set; }
        public bool Alert { get; set; }
        public bool Specific { get; set; }
        public bool SpecificHeaderField { get; set; }
        public bool SpecificNumberEvaluetion { get; set; }
        public bool SpecificNumberSample { get; set; }
        public bool SpecificLevel3 { get; set; }
        public bool SpecificGoal { get; set; }
        public bool Active { get; set; }
    }
}