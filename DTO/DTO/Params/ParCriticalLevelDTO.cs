using DTO.BaseEntity;
using System;

namespace DTO.DTO.Params
{
    public class ParCriticalLevelDTO : EntityBase
    {
        public string Name { get; set; }
        public Nullable<bool> IsActive { get; set; } = true;

    }
}
