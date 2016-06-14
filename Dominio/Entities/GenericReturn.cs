using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class GenericReturn<T>
    {
        public String MensagemErro { get; set; }
        public String MensagemAlerta { get; set; }
        public String MensagemExcecao { get; set; }
        public T Retorno { get; set; }
        public T ViewModel { get; set; }
        public List<T> listRetorno { get; set; }
    }
}
