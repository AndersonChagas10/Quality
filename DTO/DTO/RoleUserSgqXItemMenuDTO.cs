using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class RoleUserSgqXItemMenuDTO : EntityBase
    {
        public int RoleUserSgq_Id { get; set; }
        public int SgqItemMenu_Id { get; set; }
        public bool IsActive { get; set; }
    }
}
