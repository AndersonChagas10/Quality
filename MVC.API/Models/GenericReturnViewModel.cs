using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.API.Models
{
    public class GenericReturnViewModel<T>
    {
        public String MensagemErro { get; set; }
        public String MensagemAlerta { get; set; }
        public String MensagemExcecao { get; set; }
        public T Retorno { get; set; }
        public List<T> listRetorno { get; set; }
    }
}
