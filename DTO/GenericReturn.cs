using NLog;
using System;

namespace DTO.Helpers
{
    public class GenericReturn<T> 
    {
        public string Mensagem { get; set; }
        public string MensagemExcecao { get; set; }
        public T Retorno { get; private set; }
        public string Inner { get; private set; }

        public GenericReturn()
        {
        }

        public GenericReturn(T obj)
        {
             SetRetorno(obj);
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

        public GenericReturn(Exception _ex, string mensagemPadrao = "")
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

            if (!isExceptionHelper)
            {
                Logger logger = LogManager.GetLogger("dataBaseLogger");
                logger.Error(_ex, Mensagem, null);
            }

        }

    }
}
