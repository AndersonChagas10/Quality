using System;
using System.Collections.Generic;

namespace SgqSystem.ViewModels
{
    public class GenericReturnViewModel<T>
    {

        public string Mensagem { get; set; }
        public string MensagemExcecao { get; set; }
        public T Retorno { get; set; }
        //public List<T> ListRetorno { get; set; }
        public string Inner { get; set; }

        /// <summary>
        /// Construtor para o AutoMapper
        /// </summary>
        public GenericReturnViewModel()
        {

        }

        public GenericReturnViewModel(Exception _ex, string mensagemPadrao = "")
        {
            SetMensagemExcecao(_ex, mensagemPadrao);
        }

        public void SetMensagemExcecao(Exception _ex, string mensagemPadrao)
        {
            var innerMessage = "";
            var isExceptionHelper = _ex.GetType() == typeof(ExceptionHelper);

            if (!isExceptionHelper && _ex.InnerException != null) // Se a Exception lancada não for Exception Helper & Se a InnerException for diferente de null:
                if (_ex.InnerException.GetType() == typeof(ExceptionHelper)) //Se for exceção de validação Guard pelo contrutor, acionado pelo auto mapper, a inner exception é a execeção lançada pelo guard. Caso alguma outra excessão caia neste contexto, adaptar o mesmo para tratamento.
                {
                    _ex = _ex.InnerException;
                    isExceptionHelper = _ex.GetType() == typeof(ExceptionHelper);
                }

            if (_ex.InnerException != null)
            {
                innerMessage += " Inner: " + _ex.InnerException.Message;
                if (_ex.InnerException.InnerException != null)
                    innerMessage += " Inner: " + _ex.InnerException.InnerException.Message;
            }

            Mensagem = isExceptionHelper ? _ex.Message : mensagemPadrao;
            MensagemExcecao = _ex.Message;
            Inner = innerMessage;
        }

    }
}
