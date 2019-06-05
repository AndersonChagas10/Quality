using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel
{
    public class ParReprocessoCertificadosSaidaOP
    {
        public decimal nCdOrdemProducao { get; set; }
        public decimal nCdCertificacao { get; set; }
        public String cNmCertificacao { get; set; }
        public String cSgCertificacao { get; set; }
        public decimal nCdEmpresa { get; set; }
    }
}
