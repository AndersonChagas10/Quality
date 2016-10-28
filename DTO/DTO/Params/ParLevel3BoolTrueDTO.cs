using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParLevel3BoolTrueDTO : EntityBase
    {
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}