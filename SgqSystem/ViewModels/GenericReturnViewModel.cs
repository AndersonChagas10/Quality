using AutoMapper;
using System;
using System.Collections.Generic;

namespace SgqSystem.ViewModels
{
    public class GenericReturnViewModel<T>
    {

        public string Mensagem { get; set; }
        public string MensagemExcecao { get; set; }
        public T Retorno { get; private set; }
        public List<T> ListRetorno { get; set; }
        public string Inner { get; private set; }
        
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
            //Exception ex;

            //if (isExceptionHelper)
            //    ex = _ex;
            //else
            //    ex = _ex.InnerException ?? _ex;

            while (_ex.InnerException != null)
                innerMessage += " Inner: " + _ex.InnerException.Message;

            Mensagem = isExceptionHelper ? _ex.Message : mensagemPadrao;
            MensagemExcecao = _ex.Message;
            Inner = innerMessage;
        }

    }
}
