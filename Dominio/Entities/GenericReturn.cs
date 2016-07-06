using Dominio.Helpers;
using System;
using System.Collections.Generic;

namespace Dominio.Entities
{
    public class GenericReturn<T> : ExceptionHelper
    {

        public string Mensagem { get; set; }
        public string MensagemExcecao { get; set; }
        public T Retorno { get; private set; }
        public List<T> ListRetorno { get; set; }
        public string Inner { get; private set; }

        public GenericReturn(T obj)
        {
            SetRetorno(obj);
        }

        public GenericReturn(List<T> listObj)
        {
            SetListRetorno(listObj);
        }

        public GenericReturn(string _mensagem)
        {
            Guard.ForNullOrEmpty(_mensagem,"A mensagem de retorno esta vazia.(GEnericReturn)");
            Mensagem = _mensagem;
        }

        public void SetRetorno(T obj)
        {
            if (obj.IsNull())
                throw new ExceptionHelper("Objeto não encontrado.");

            Retorno = obj;
        }

        public void SetListRetorno(List<T> obj)
        {
            if (obj.IsNull())
                throw new ExceptionHelper("Objetos não encontrados.");

        }

    }
}
