using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParCounterLocalDTO : EntityBase
    {
        public int ParLocal_Id { get; set; }
        public int ParCounter_Id { get; set; }
        public int? ParLevel1_Id { get; set; }
        public int? ParLevel2_Id { get; set; }
        public int? ParLevel3_Id { get; set; }
        public bool IsActive { get; set; } = true;

    }
}