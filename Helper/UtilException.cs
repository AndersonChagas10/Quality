using System;
using System.Collections.Generic;
using System.Text;

namespace SgqSystem.Helpers
{
    public static class UtilException
    {

        public static string ToClient(this Exception exception)
        {
            string stackTrace = "";
            string message = "";

            if (exception.InnerException != null)
            {
                if (exception.InnerException.InnerException != null)
                {
                    stackTrace = exception.InnerException.InnerException.StackTrace;
                    message = exception.InnerException.InnerException.Message;
                }
                else
                {
                    stackTrace = exception.InnerException.StackTrace;
                    message = exception.InnerException.Message;
                }
            }
            else
            {
                stackTrace = exception.StackTrace;
                message = exception.Message;
            }

            stackTrace = $"Ocorreu um erro inesperado: {message} -> {stackTrace}";
            return stackTrace;
        }
    }
}