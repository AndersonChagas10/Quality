using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conformity.Infra.CrossCutting
{
    public static class ExceptionHelper
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

            if (exception.GetType() == typeof(DbEntityValidationException))
            {
                stackTrace = "";
                foreach (var eve in (exception as DbEntityValidationException).EntityValidationErrors)
                {
                    stackTrace += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        stackTrace += string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }

            stackTrace = $"Ocorreu um erro inesperado: {message} -> {stackTrace}";
            return stackTrace;
        }

    }
}
