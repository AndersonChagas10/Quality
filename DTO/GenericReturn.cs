using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DTO.Helpers
{
    public class GenericReturn<T>
    {
        public string Mensagem { get; set; }
        public string MensagemExcecao { get; set; }
        public T Retorno { get; private set; }
        public string Inner { get; private set; }
        public string StackTrace { get; private set; }
        public int IdSaved { get; set; }
        public string InnerStackTrace { get; private set; }
        public string InnerStackTrace2 { get; private set; }

        #region Contrutores

        public GenericReturn()
        {
        }

        public GenericReturn(T obj)
        {
            SetRetorno(obj);
        }

        public GenericReturn(string _mensagem)
        {
            SetMessage(_mensagem);
        }

        public GenericReturn(string _mensagem, T obj)
        {
            Guard.ForNullOrEmpty(_mensagem, "A mensagem de retorno esta vazia.(GEnericReturn)");
            Mensagem = _mensagem;

            SetRetorno(obj);
        }

        public GenericReturn(Exception _ex, string mensagemPadrao = "")
        {
            SetMensagemExcecao(_ex, mensagemPadrao);
        }

        public GenericReturn(Exception _ex, string mensagemPadrao, T obj)
        {
            SetRetorno(obj);
            SetMensagemExcecao(_ex, mensagemPadrao);
        }

        #endregion

        #region Auxiliares

        public void SetRetorno(T obj)
        {
            if (obj.IsNull())
                throw new ExceptionHelper("Objeto não encontrado.");

            Retorno = obj;
        }

        private void SetMessage(string _mensagem)
        {
            Guard.ForNullOrEmpty(_mensagem, "A mensagem de retorno esta vazia.(GEnericReturn)");
            Mensagem = _mensagem;
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
                //if (_ex.InnerException.StackTrace != null)
                //    innerStackTrace = _ex.InnerException.StackTrace;
                //if (_ex.InnerException.InnerException != null)
                //{
                //    innerMessage2 += " Inner: " + _ex.InnerException.InnerException.Message;
                //    if (_ex.InnerException.InnerException.StackTrace != null)
                //        innerStackTrace2 = _ex.InnerException.StackTrace;
                //}
            }

            CreateStackTrace(_ex);
            
            if (_ex.InnerException.IsNotNull())
                CreateStackTrace(_ex.InnerException);

            // Get the line number from the stack frame
            Mensagem = isExceptionHelper ? _ex.Message : mensagemPadrao;
            MensagemExcecao = _ex.Message;
            //StackTrace += "\n ////////// Original Stack trace : " + _ex.StackTrace;
            //StackTrace += "\n \\\\\\\\\\";
            Inner = innerMessage;

            if (StackTrace.IndexOf("Line") > 0)
            {

            }

            if (!isExceptionHelper)
            {
                var logger = LogManager.GetLogger("dataBaseLogger");
                GlobalDiagnosticsContext.Set("StackTrace", StackTrace);
                logger.Error(_ex, Mensagem, this);
            }

        }

        private void CreateStackTrace(Exception _ex)
        {
            var st = new StackTrace(_ex, true);
            var counter = 0;
            while (st.GetFrame(counter) != null)
            {
                var frame = st.GetFrame(counter);
                StackTrace += " Method: " + frame.GetMethod().DeclaringType.Name;
                StackTrace += " Line:" + frame.GetFileLineNumber();
                StackTrace += "\n";
                counter++;
            }
        }

        #endregion

    }
}
