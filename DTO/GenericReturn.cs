using NLog;
using System;
using System.Diagnostics;

namespace DTO.Helpers
{
    /// <summary>
    /// Objeto Utilizado para retorno nas API's, 
    /// *** Atenção, a utilização deste tipo de retorno em try/catch Na camada "Domain" inibe o status de "Erro faltal" no servidor, porem deve ser tratado no retorno de sua chamada WEB ***.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericReturn<T>
    {
        public string Mensagem { get; set; }
        public string MensagemExcecao { get; set; }
        public T Retorno { get; private set; }
        public string Inner { get; private set; }
        public string StackTrace { get; private set; }
        public int IdSaved { get; set; }

        #region Contrutores

        /// <summary>
        /// Construtor default;
        /// </summary>
        public GenericReturn()
        {
        }

        /// <summary>
        /// Contrutor que recebe um objeto, geralmente utilizado para retornos que não requerem parametros adicionais, somente o objeto.
        /// </summary>
        /// <param name="obj"></param>
        public GenericReturn(T obj)
        {
            SetRetorno(obj);
        }

        /// <summary>
        /// Contrutor que recebe uma string, geralmente utilizado para retornos que não requerem objetos "Acesso negado." por exemplo.
        /// </summary>
        /// <param name="_mensagem"></param>
        public GenericReturn(string _mensagem)
        {
            SetMessage(_mensagem);
        }

        /// <summary>
        /// Cosntrutor com mensagem e objeto, geralmente utilizado para retornos que não requerem objetos '"Acesso negado para o user" + r.Obj' por exemplo.
        /// </summary>
        /// <param name="_mensagem"></param>
        /// <param name="obj"></param>
        public GenericReturn(string _mensagem, T obj)
        {
            Guard.ForNullOrEmpty(_mensagem, "A mensagem de retorno esta vazia.(GEnericReturn)");
            Mensagem = _mensagem;

            SetRetorno(obj);
        }

        /// <summary>
        /// Cosntrutor com uma Exception, mensagem, geralmente utilizado para retornos que não requerem objetos "Acesso negado." por exemplo.
        /// </summary>
        /// <param name="_ex"></param>
        /// <param name="mensagemPadrao"></param>
        public GenericReturn(Exception _ex, string mensagemPadrao = "")
        {
            SetMensagemExcecao(_ex, mensagemPadrao);
        }

        /// <summary>
        /// Cosntrutor com uma Exception, mensagem e objeto, utilizado para retorno de objetos e debugs JavaScript quando ocorrem exceptions.
        /// </summary>
        /// <param name="_ex"></param>
        /// <param name="mensagemPadrao"></param>
        /// <param name="obj"></param>
        public GenericReturn(Exception _ex, string mensagemPadrao, T obj)
        {
            SetRetorno(obj);
            SetMensagemExcecao(_ex, mensagemPadrao);
        }

        #endregion

        #region Auxiliares
        
        /// <summary>
        /// Setter Object T Retorno
        /// </summary>
        /// <param name="obj"></param>
        public void SetRetorno(T obj)
        {
            if (obj.IsNull())
                throw new ExceptionHelper("Objeto não encontrado.");

            Retorno = obj;
        }

        /// <summary>
        /// Setter String mensagem
        /// </summary>
        /// <param name="_mensagem"></param>
        private void SetMessage(string _mensagem)
        {
            Guard.ForNullOrEmpty(_mensagem, "A mensagem de retorno esta vazia.(GEnericReturn)");
            Mensagem = _mensagem;
        }
        
        /// <summary>
        /// Para cada exceção e innerException este método elabora um Log De Operação.
        /// </summary>
        /// <param name="_ex"></param>
        /// <param name="mensagemPadrao"></param>
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

            Mensagem = isExceptionHelper ? _ex.Message : mensagemPadrao;
            MensagemExcecao = Mensagem + " " + _ex.Message;
            Inner = innerMessage;
            CreateStackTrace(_ex);
            var hasInnnerException = _ex.InnerException;
            var counter = 0;
            while (hasInnnerException != null)
            {
                MensagemExcecao += "\nInnerMesage(" + counter + "):\n" + hasInnnerException.Message;
                Inner += " InnerException(" + counter + "): " + hasInnnerException.InnerException;
                this.StackTrace += "\nInnerStackTrace(" + counter + "):\n";
                CreateStackTrace(hasInnnerException);
                hasInnnerException = hasInnnerException.InnerException;
                counter++;
            }

            if (!isExceptionHelper)
            {
                var logger = LogManager.GetLogger("dataBaseLogger");
                GlobalDiagnosticsContext.Set("StackTrace", StackTrace);
                //GlobalDiagnosticsContext.Set("MensagemExcecao", MensagemExcecao);
                logger.Error(_ex, MensagemExcecao, this);
            }

        }

        /// <summary>
        /// Cria StackTrace personalizada com quebra de linha e numero da linha (caso objeto não esteja em dll.
        /// </summary>
        /// <param name="_ex"></param>
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
