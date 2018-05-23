using DTO.BaseEntity;
using System.Collections.Generic;

namespace DTO.DTO
{
    public class RoleUserSgqDTO : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public List<RoleUserSgqXItemMenuDTO> RoleUserSgqXItemMenuDTO { get;set; }
    }
}
