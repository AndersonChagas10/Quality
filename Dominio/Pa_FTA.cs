using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Pa_FTA
    {
        public int Id { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? AlterDate { get; set; }
        public string MetaFTA { get; set; }
        public string PercentualNCFTA { get; set; }
        public string ReincidenciaDesvioFTA { get; set; }
        public int? Supervisor_Id { get; set; }
        public int? Order { get; set; }
    }
}
