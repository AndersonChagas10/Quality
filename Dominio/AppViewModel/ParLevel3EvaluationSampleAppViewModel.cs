using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.AppViewModel
{
    public class ParLevel3EvaluationSampleAppViewModel
    {
        public int Id { get; set; }

        public int? ParCompany_Id { get; set; }

        public int? ParLevel1_Id { get; set; }

        public int? ParLevel2_Id { get; set; }

        public int ParLevel3_Id { get; set; }

        public decimal? SampleNumber { get; set; }

        public decimal? EvaluationNumber { get; set; }
    }
}
