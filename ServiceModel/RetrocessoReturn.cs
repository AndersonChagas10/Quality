using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel
{
    public class RetrocessoReturn
    {
        public List<ParReprocessoHeaderOP> parReprocessoHeaderOPs { get; set; }
        public List<ParReprocessoCertificadosSaidaOP> parReprocessoCertificadosSaidaOP { get; set; }
        public List<ParReprocessoSaidaOP> parReprocessoSaidaOPs { get; set; }
        public List<ParReprocessoEntradaOP> parReprocessoEntradaOPs { get; set; }
        public List<Header> headerFieldsEntrada { get; set; }
        public List<Header> headerFieldsSaida { get; set; }
    }
}
