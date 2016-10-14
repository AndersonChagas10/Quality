using NLog;
using System;
using System.Diagnostics;

namespace DTO
{
    public class CreateLog
    {

        public CreateLog(Exception e)
        {
            LogException(e);
        }

        /// <summary>
        /// Faz log do Nlog, salva em DB e em arquivo.
        /// </summary>
        /// <param name="_ex"></param>
        private string innerMessage = "";
        private string stackTrace = "";
        private string mensagem = "";
        private string mensagemExcecao = "";
        private string inner = "";
        private void LogException(Exception _ex)
        {
            mensagem = _ex.Message;
            mensagemExcecao = mensagem + " " + _ex.Message;
            inner = innerMessage;

            CreateStackTrace(_ex);
            CreateInnerStacktrace(_ex);

            var logger = LogManager.GetLogger("dataBaseLogger");
            GlobalDiagnosticsContext.Set("StackTrace", stackTrace);
            //GlobalDiagnosticsContext.Set("MensagemExcecao", MensagemExcecao);
            logger.Error(_ex, mensagemExcecao, this);

        }

        /// <summary>
        /// Cria Inner StackTrace personalizada com quebra de linha e numero da linha (caso objeto não esteja em dll.
        /// </summary>
        /// <param name="_ex"></param>
        private void CreateInnerStacktrace(Exception _ex)
        {
            var hasInnnerException = _ex.InnerException;
            var counter = 0;
            while (hasInnnerException != null)
            {
                mensagemExcecao += "\nInnerMesage(" + counter + "):\n" + hasInnnerException.Message;
                inner += " InnerException(" + counter + "): " + hasInnnerException.InnerException;
                stackTrace += "\nInnerStackTrace(" + counter + "):\n";
                CreateStackTrace(hasInnnerException);
                hasInnnerException = hasInnnerException.InnerException;
                counter++;
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
                if (frame.GetMethod().DeclaringType != null)
                {
                    stackTrace += " Method: " + frame.GetMethod().DeclaringType.Name;
                    stackTrace += " Line:" + frame.GetFileLineNumber();
                    stackTrace += "\n";
                }
                counter++;
            }
        }

    }
}
