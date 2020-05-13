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

        public int[] IsCascadeLevel2Level3 { get; set; } = new int[] { };

        public int[] ParStructure_Ids { get; set; } = new int[] { };

        public int[] ParStructure1_Ids { get; set; } = new int[] { };

        public int[] ParStructure2_Ids { get; set; } = new int[] { };

        public int[] ParStructure3_Ids { get; set; } = new int[] { };

        public int[] ParStructureGroup_Ids { get; set; } = new int[] { };

        public bool ShowUserCompanies { get; set; } = false;

        public int[] ShowIndicador_Id { get; set; } = new int[] { };

        public int[] ParCompany_Ids { get; set; } = new int[] { };

        public int[] Shift_Ids { get; set; } = new int[] { };

        public int[] ParDepartment_Ids { get; set; } = new int[] { };

        public int[] ShowModeloGrafico_Id { get; set; } = new int[] { };

        public int[] ShowDimensaoGrafico_Id { get; set; } = new int[] { };
      
        public int[] ParSecao_Ids { get; set; } = new int[] { };

        public int[] ParCargo_Ids { get; set; } = new int[] { };

        public int[] ParGroupParLevel1_Ids { get; set; } = new int[] { };

        public int[] ParLevel1_Ids { get; set; } = new int[] { };

        public int[] ParLevel2_Ids { get; set; } = new int[] { };

        public int[] ParLevel3_Ids { get; set; } = new int[] { };

        public int[] ParLevel1Status_Ids { get; set; } = new int[] { };

        public int[] ParCriticalLevel_Ids { get; set; } = new int[] { };

        public int[] ParCluster_Ids { get; set; } = new int[] { };

        public int[] ParClusterGroup_Ids { get; set; } = new int[] { };

        public int[] ParLevel1Group_Ids { get; set; } = new int[] { };

        //public int[] ParLevel1Status { get; set; } = new int[] { };

        public int[] AcaoStatus { get; set; } = new int[] { };

        public int[] Periodo { get; set; } = new int[] { };

        public int[] ParModule_Ids { get; set; } = new int[] { };

        public int?[] Peso { get; set; } = new int?[] { };

        public int[] Desdobramento { get; set; } = new int[] { };

        public int[] UserSgqSurpervisor_Ids { get; set; } = new int[] { };

        public int[] userSgqMonitor_Ids { get; set; } = new int[] { };

        public int[] userSgqAuditor_Ids { get; set; } = new int[] { };

        public int[] tipoEdicao { get; set; } = new int[] { };

        public int[] ParReason_Ids { get; set; } = new int[] { };

        public int[] NcComPeso { get; set; } = new int[] { };

        public int[] DesdobramentoPorDepartamento { get; set; } = new int[] { };

        public int DimensaoData { get; set; }

        public bool ShowLinkedFilters { get; set; } = true;

        public JObject Param { get; set; }
    }
}
