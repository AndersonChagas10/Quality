using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.AppViewModel
{
    public class ParEvaluationAppViewModel
    {
        public int Id { get; set; }

        public int? ParCompany_Id { get; set; }

        public int ParLevel2_Id { get; set; }

        public int Number { get; set; }

        public int? Sample { get; set; }

        public bool IsActive { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParCluster_Id { get; set; }
    }
}
