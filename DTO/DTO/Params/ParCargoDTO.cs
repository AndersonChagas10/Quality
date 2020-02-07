using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO.Params
{
    public class ParCargoDTO : EntityBase
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
