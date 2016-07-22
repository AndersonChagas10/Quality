using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ComplexType
{
    public class RetornoQueryIndicadoresRelBate
    {
        public int Id_Level1 { get; set; }
        public int Id_Level2 { get; set; }
        public int Id_Level3 { get; set; }
        public decimal Evaluate { get; set; }
        public decimal NotConform { get; set; }
    }
}
