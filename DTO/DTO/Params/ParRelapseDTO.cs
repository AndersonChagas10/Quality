using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParRelapseDTO : EntityBase
    { 
        public int? ParLevel1_Id { get; set; }
        public int? ParLevel2_Id { get; set; }
        public int? ParLevel3_Id { get; set; }
        public int ParFrequency_Id { get; set; }
        public int NcNumber { get; set; }
        public int EffectiveLength { get; set; }
        public bool IsActive { get; set; } = true;

        public ParFrequencyDTO parFrequency { get; set; }
        public ParLevel1DTO parLevel1 { get; set; }
        public ParLevel2DTO parLevel2 { get; set; }
        public ParLevel3DTO parLevel3 { get; set; }

    }
}