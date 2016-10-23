using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParLevel2DTO : EntityBase
    {
        public int ParFrequency_Id { get; set; }
        public int ParDepartment_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool EmptyLevel3 { get; set; }
        public bool ShowLevel03 { get; set; }
        public bool GroupLevel3 { get; set; }
        public bool Active { get; set; } = true;

        public ParConsolidationTypeDTO parConsolidationTypeDto { get; set; }
        public ParFrequencyDTO parFrequencyDto { get; set; }

    }
}