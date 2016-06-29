using System;
using System.Collections.Generic;

namespace Dominio.Entities
{
    public class GenericReturn<T>
    {
        public string MensagemErro { get; set; }
        public string MensagemAlerta { get; set; }
        public string MensagemExcecao { get; set; }
        public T Retorno { get; set; }
        public List<T> listRetorno { get; set; }
    }
}
