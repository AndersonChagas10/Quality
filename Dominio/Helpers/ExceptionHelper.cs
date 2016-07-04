using Dominio.Entities;
using System;

namespace Dominio.Helpers
{
    public static class ExceptionHelper<T> where T : class
    {

        public static GenericReturn<T> RetornaExcecaoBase(Exception ex, string mensagemErro = "", string mensagemAlerta = "", T obj = null)
        {
            return new GenericReturn<T>(ex, mensagemErro, mensagemAlerta);
        }

    }
}
