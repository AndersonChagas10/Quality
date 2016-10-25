using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParLevel3GroupDTO : EntityBase
    {
        public int ParLevel2_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public ParLevel2DTO parLevel2Dto { get; set; }

    }
}