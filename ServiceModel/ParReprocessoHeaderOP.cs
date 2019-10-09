using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel
{
    public class ParReprocessoHeaderOP
    {
        public decimal nCdOrdemProducao { get; set; }
        public decimal nCdEmpresa { get; set; }
        public DateTime dLancamento { get; set; }
        public decimal nCdUsuario { get; set; }
        public String cCdRastreabilidade { get; set; }
        public String cValidaHabilitacaoEntrada { get; set; }
        public decimal nCdHabilitacao { get; set; }
        public String cNmHabilitacao { get; set; }
        public String cSgHabilitacao { get; set; }
    }
}
