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

        /// <summary>
        /// cria Log de uma exception
        /// </summary>
        /// <param name="e"></param>
        public CreateLog(Exception e, string ControllerAction)
        {
            GlobalDiagnosticsContext.Clear();
            LogException(e, ControllerAction);
        }


        #region Colleta

        public CreateLog(Exception exception, int user_Id, int period, int shift, string html, object json)
        {
            LogExceptionColleta(exception, user_Id, period, shift, html, json);
        }

        private void LogExceptionColleta(Exception _ex, int user_Id, int period, int shift, string html, object json)
        {

            mensagem = _ex.Message;
            mensagemExcecao = mensagem + " " + _ex.Message;
            inner = innerMessage;

            CreateStackTrace(_ex);
            CreateInnerStacktrace(_ex);

            var logger = LogManager.GetLogger("BkpCollectionLogger");

            /*
              < parameter name = "@User_Id" layout = " ${gdc:User_Id}" />
              < parameter name = "@Period" layout = " ${gdc:Period}" />
              < parameter name = "@Shift" layout = " ${gdc:Shift}" />
              < parameter name = "@Html" layout = " ${gdc:Html}" />
              < parameter name = "@Json" layout = " ${gdc:Json}" />
              < parameter name = "@Stack_Trace" layout = " ${gdc:StackTrace}" />
            */

            GlobalDiagnosticsContext.Set("User_Id", user_Id);
            GlobalDiagnosticsContext.Set("Period", period);
            GlobalDiagnosticsContext.Set("Shift", shift);
            GlobalDiagnosticsContext.Set("Html", html);
            GlobalDiagnosticsContext.Set("Json", ToJson(json).ToString());
            GlobalDiagnosticsContext.Set("StackTrace", stackTrace);

            logger.Warn(_ex, mensagemExcecao, json);

        } 

        #endregion

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
        private void LogException(Exception _ex, string ControllerAction = null)
        {

            mensagem = _ex.Message;
            if (!string.IsNullOrEmpty(ControllerAction))
                mensagem += " " + ControllerAction;

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
