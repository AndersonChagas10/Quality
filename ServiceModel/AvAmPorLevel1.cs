using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel
{
    public class AvAmPorLevel1
    {
        public string AvAm { get; set; }
        public decimal? SomaWeiDefect { get; set; }
        public decimal SomaNotIsConform { get; set; }
        public decimal SomaWeiDefectsPorAv { get; set; }
        public decimal SomaWeiDefectsPorAvAcumulada { get; set; }
        public int Am { get; internal set; }
        public int Av { get; internal set; }
    }
}
