using AutoMapper;
using Dominio.Helpers;
using System;
using System.Collections.Generic;

namespace SgqSystem.ViewModels
{
    public class GenericReturnViewModel<T>
    {

        public string MensagemErro { get; set; }
        public string MensagemSucesso { get; set; }
        public string MensagemAlerta { get; set; }
        public string MensagemExcecao { get; private set; }
        public T Retorno { get; private set; }
        public List<T> ListRetorno { get; private set; }
        public bool ReturnisBool { get; set; }
        public string Inner { get; private set; }

        /// <summary>
        /// Constructor for Entity Framework.        
        /// </summary>
        public GenericReturnViewModel()
        {

        }

        public GenericReturnViewModel(T obj)
        {
            SetRetorno(obj);
        }

        public GenericReturnViewModel(List<T> listObj)
        {
            SetListRetorno(listObj);
        }

        public GenericReturnViewModel(Exception _ex, string mensagemErro = "", string mensagemAlerta = "")
        {
            SetMensagemExcecao(_ex, MensagemErro, MensagemAlerta);
        }

        public void SetMensagemExcecao(Exception _ex, string mensagemErro = "", string mensagemAlerta = "")
        {

            var innerMessage = "";
            Exception ex;
            if (_ex.GetType() == typeof(AutoMapperMappingException))
                ex = _ex.InnerException;
            else
                ex = _ex;

            while (ex.InnerException != null)
                innerMessage += " Inner: " + ex.InnerException.Message;

            MensagemErro = mensagemErro ?? ex.Message;
            MensagemExcecao = ex.Message;
            MensagemAlerta = mensagemAlerta;
            Inner = innerMessage;
        }

        public void SetRetorno(T obj)
        {
            if (obj.IsNull())
                throw new Exception("Objeto não encontrado.");

        }

        public void SetListRetorno(List<T> obj)
        {
            if (obj.IsNull())
                throw new Exception("Objetos não encontrados.");

        }

    }
}
