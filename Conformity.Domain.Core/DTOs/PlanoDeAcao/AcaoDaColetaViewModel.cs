using System;
using static Conformity.Domain.Core.Enums.PlanoDeAcao.Enums;

namespace Conformity.Domain.Core.DTOs.PlanoDeAcao
{
    public class AcaoDaColetaViewModel
    {
        public int Id { get; set; }
        public int ParLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int ParLevel3_Id { get; set; }
        public string ParLevel3_Name { get; set; }
        public int ParCompany_Id { get; set; }
        public int ParDepartment_Id { get; set; }
        public int ParDepartmentParent_Id { get; set; }
        public int ParCargo_Id { get; set; }
        public int Status { get; set; }
        public string StatusName { get { return Enum.GetName(typeof(EAcaoStatus), Status); } }
        public int ParCluster_Id { get; set; }
        public string CodigoDaAcao { get; set; }
    }
}
