using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DataCarrierFormularioNew
    {
        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }

        public int[] ParStructure_Ids { get; set; } = new int[] { };

        public int[] ParStructureGroup_Ids { get; set; } = new int[] { };

        public int[] ParCompany_Ids { get; set; } = new int[] { };

        public int[] Shift_Ids { get; set; } = new int[] { };

        public int[] ParDepartment_Ids { get; set; } = new int[] { };

        public int[] ParSecao_Ids { get; set; } = new int[] { };

        public int[] ParCargo_Ids { get; set; } = new int[] { };

        public int[] ParLevel1_Ids { get; set; } = new int[] { };

        public int[] ParLevel2_Ids { get; set; } = new int[] { };

        public int[] ParLevel3_Ids { get; set; } = new int[] { };

        public int[] ParCriticalLevel_Ids { get; set; } = new int[] { };

        public int[] ParCluster_Ids { get; set; } = new int[] { };

        public int[] ParClusterGroup_Ids { get; set; } = new int[] { };

        public JObject Param { get; set; }

    }
}
