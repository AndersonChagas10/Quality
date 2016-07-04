using Dominio.Entities;
using System;

namespace Dominio.Helpers
{
    public static class ExceptionHelper<T> where T : class
    {

        public static GenericReturn<T> RetornaExcecaoBase(Exception ex, string mensagemErro = "", string mensagemAlerta = "", T obj = null)
        {


            var inner = "Não consta.";

            if(ex.InnerException.IsNotNull())
            {
                inner = ex.InnerException.Message;
                if(ex.InnerException.InnerException.IsNotNull())
                    inner += ex.InnerException.InnerException.Message;
            }

            return new GenericReturn<T>()
            {
                MensagemErro = mensagemErro,
                MensagemExcecao = ex.Message + inner,
                Retorno = obj,
                MensagemAlerta = mensagemAlerta
            };

        }

    }
}
