using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParFieldTypeDTO : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}