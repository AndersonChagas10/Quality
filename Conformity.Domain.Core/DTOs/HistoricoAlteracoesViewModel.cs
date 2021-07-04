using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conformity.Domain.Core.DTOs
{
    public class HistoricoAlteracoesViewModel
    {
        public string Username { get; set; }
        public DateTime DataModificacao { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorAlterado { get; set; }
        public string Propriedade { get; set; }
    }
}
