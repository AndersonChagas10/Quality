using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParCounterXLocalDTO : EntityBase
    {
        public int ParLocal_Id { get; set; }
        public int ParCounter_Id { get; set; }
        public int? ParLevel1_Id { get; set; }
        public int? ParLevel2_Id { get; set; }
        public int? ParLevel3_Id { get; set; }
        public bool IsActive { get; set; } = true;

        public ParCounterDTO ParCounter { get; set; }
        //public ParLevel1DTO ParLevel1 { get; set; }
        //public ParLevel2DTO ParLevel2 { get; set; }
        //public ParLevel3DTO ParLevel3 { get; set; }
        public ParLocalDTO ParLocal { get; set; }
    }
}