using DTO.BaseEntity;

namespace DTO.DTO
{
    public class ParFieldTypeDTO : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}