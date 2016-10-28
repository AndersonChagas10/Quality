using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParLevel3BoolFalseDTO : EntityBase
    {
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}