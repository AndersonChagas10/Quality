using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO.Manutencao
{
    public class PainelIndicadoresUniManutencaoDTO
    {
        public string dado { get; set; }
        public decimal orcado { get; set; }
        public decimal realizado { get; set; }
        public decimal desvio { get; set; }
        public decimal porcDesvio { get; set; }
    }
}
