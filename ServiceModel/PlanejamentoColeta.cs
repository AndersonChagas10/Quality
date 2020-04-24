using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel
{
    public class PlanejamentoColeta
    {
        public int ParCompany_Id { get; set; }
        public int ParFrequency_Id { get; set; }
        public int ParCluster_Id { get; set; }
        public int ParClusterGroup_Id { get; set; }
        public DateTime AppDate { get; set; }
    }
}
