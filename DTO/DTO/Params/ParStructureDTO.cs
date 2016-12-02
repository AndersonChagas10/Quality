using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO.Params
{
    public class ParStructureDTO : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParStructureParent_Id { get; set; }
        public int ParStructureGroup_Id { get; set; } 
        public bool IsActive { get; set; } = true;

        public ParStructureGroupDTO ParStructureGroup { get; set; }
    }
}
