using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.DTO
{
    public class ParClusterXModulesDTO : EntityBase
    {

        public int ParCluster_Id { get; set; }
        public int ParModule_Id { get; set; }
        public decimal? Points { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }

    }
}
