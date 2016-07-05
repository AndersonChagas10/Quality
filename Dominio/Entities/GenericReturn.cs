using Dominio.Helpers;
using System;
using System.Collections.Generic;

namespace Dominio.Entities
{
    public class GenericReturn<T> : Exception
    {

        public string MensagemErro { get; set; }
        public string MensagemSucesso { get; set; }
        public string MensagemAlerta { get; set; }
        public string MensagemExcecao { get; private set; }
        public T Retorno { get; private set; }
        public List<T> ListRetorno { get; private set; }
        public bool ReturnisBool { get; set; }
        public string Inner { get; private set; }

        //Constructor for EF        
        public GenericReturn()
        {

        }

        public GenericReturn(T obj)
        {
            SetRetorno(obj);
        }

        public GenericReturn(List<T> listObj)
        {
            SetListRetorno(listObj);
        }

        /// <summary>
        /// Cria retorno Genérico com mensagem de exception + inner exception, com opcional.
        /// </summary>
        /// <param name="_ex"></param>
        /// <param name="mensagemErro"></param>
        /// <param name="mensagemAlerta"></param>
        public GenericReturn(Exception _ex, string mensagemErro = "", string mensagemAlerta = "")
        {
            SetMensagemExcecao(_ex, MensagemErro, MensagemAlerta);
        }


        /// <summary>
        /// Verifica os dados da Classe Exception 
        /// </summary>
        /// <param name="_ex"></param>
        /// <param name="mensagemErro"></param>
        /// <param name="mensagemAlerta"></param>
        public void SetMensagemExcecao(Exception _ex, string mensagemErro = "", string mensagemAlerta = "")
        {
            
            var inner = "Não consta.";

            if (_ex.InnerException.IsNotNull())
            {
                inner = _ex.InnerException.Message;
                if (_ex.InnerException.InnerException.IsNotNull())
                    inner += _ex.InnerException.InnerException.Message;
            }
            MensagemErro = mensagemErro;
            MensagemExcecao = _ex.Message;
            Inner = inner;
            MensagemAlerta = mensagemAlerta;
        }

        public void SetRetorno(T obj)
        {
            if(obj.IsNull())
                throw new Exception("Objeto não encontrado.");

            Retorno = obj;
        }

        public void SetListRetorno(List<T> obj)
        {
            if (obj.IsNull())
                throw new Exception("Objetos não encontrados.");

        }
    }
}
