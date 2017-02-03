using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    class RoleTypeDTO : EntityBase
    {
        public int Id { get; set; }
        public string Type { get; set; }

    }
}
