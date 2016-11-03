using DTO;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace SgqSystem.Handlres
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HandleApi : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {

            LogException(context.Exception);
            context.Response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, context.Exception.Message.ToString());
            base.OnException(context);
        }

        /// <summary>
        /// Cria Log.
        /// </summary>
        /// <param name="ex"></param>
        private void LogException(Exception ex)
        {
            new CreateLog(ex);
        }

    }
}