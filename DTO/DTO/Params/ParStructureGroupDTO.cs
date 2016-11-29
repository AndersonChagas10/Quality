using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO.Params
{
    public class ParStructureGroupDTO : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParStructureGroupParent_Id { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
