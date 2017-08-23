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
        private bool saveLog;

        public HandleApi()
        {
            saveLog = true;
        }

        public HandleApi(bool saveLog)
        {
            this.saveLog = saveLog;
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            if(saveLog)
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