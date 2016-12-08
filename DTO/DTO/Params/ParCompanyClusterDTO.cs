using DTO.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO.Params
{
    public class ParCompanyClusterDTO : EntityBase
    {
        public int ParCompany_Id { get; set; }
        public int ParCluster_Id { get; set; }
        public bool Active { get; set; }
    }
}
