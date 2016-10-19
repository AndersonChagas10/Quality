using DTO.DTO;
using Newtonsoft.Json;
using NLog;
using System;
using System.Diagnostics;

namespace DTO
{
    public class CreateLog
    {
        /// <summary>
        /// cria Log de uma exception
        /// </summary>
        /// <param name="e"></param>
        public CreateLog(Exception e)
        {
            GlobalDiagnosticsContext.Clear();
            LogException(e);
        }

        //public CreateLog(Exception e, object obj, string prefix, string mensagemExtra = "")
        //{
        //    GlobalDiagnosticsContext.Clear();
        //    LogException(e, obj, prefix, mensagemExtra);
        //}

        public CreateLog(Exception exception, object obj)
        {
            LogException(exception, obj);
        }

        private Logger _logger;
        public void GetLog(string name)
        {
            _logger = LogManager.GetLogger(name);
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
            logger.Warn(_ex, mensagemExcecao, this);

        }

        private void LogException(Exception _ex, object obj)
        {

            mensagem = _ex.Message;
            mensagemExcecao = mensagem + " " + _ex.Message;
            inner = innerMessage;

            CreateStackTrace(_ex);
            CreateInnerStacktrace(_ex);

            var logger = LogManager.GetLogger("dataBaseLogger");

            GlobalDiagnosticsContext.Set("StackTrace", stackTrace);
            GlobalDiagnosticsContext.Set("Objects", ToJson(obj).ToString());
            logger.Warn(_ex, mensagemExcecao, obj);
            
        }

        public string ToJson(object value)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return JsonConvert.SerializeObject(value, Formatting.Indented, settings);
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
