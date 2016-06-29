using System;
using System.Collections.Generic;

namespace SgqSystem.ViewModels
{
    public class GenericReturnViewModel<T>
    {
        public string MensagemErro { get; set; }
        public string MensagemAlerta { get; set; }
        public string MensagemExcecao { get; set; }
        public T Retorno { get; set; }
        public List<T> listRetorno { get; set; }
    }
}
