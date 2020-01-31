using DTO.BaseEntity;
using System.Collections.Generic;

namespace DTO.DTO
{
    public class RoleUserSgqDTO : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool FazColeta { get; set; }

        public bool? IsCorporativo { get; set; }

        public bool? IsNegocio { get; set; }

        public bool? IsRegional { get; set; }

        public List<RoleUserSgqXItemMenuDTO> RoleUserSgqXItemMenuDTO { get;set; }

        public IEnumerable<int> ItemMenuIDs { get; set; }
    }
}
