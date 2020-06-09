using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.AppViewModel
{
    public class ParEvaluationXDepartmentXCargoAppViewModel
    {
        public int Id { get; set; }
        public int? ParCargo_Id { get; set; }
        public int ParDepartment_Id { get; set; }
        public int? ParCompany_Id { get; set; }
        public int? ParCluster_Id { get; set; }
        public int Evaluation { get; set; }
        public int Sample { get; set; }
        public bool RedistributeWeight { get; set; }
        public bool IsPartialCollection { get; set; }

        public virtual List<ParEvaluationScheduleAppViewModel> ParEvaluationScheduleAppViewModel { get; set; }
    }
}
