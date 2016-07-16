using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ComplexType
{
    public class RetornoQueryIndicadoresRelBate
    {
        public int Id_operacao { get; set; }
        public int Id_Monitoramento { get; set; }
        public int Id_tarefa { get; set; }
        public decimal Evaluate { get; set; }
        public decimal NotConform { get; set; }
    }
}
