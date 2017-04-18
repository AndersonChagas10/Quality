using DTO.BaseEntity;

namespace DTO.DTO
{
    public class RoleUserSgqDTO : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
