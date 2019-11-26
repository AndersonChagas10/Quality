﻿using DTO;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Helper
{
    public class HandleController : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// Executa ao receber exceção no controller
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;
            filterContext.ExceptionHandled = false;
            var controllerAction = String.Join(",", filterContext.Controller.ControllerContext.RouteData.Values.ToList());
            LogException(ex, controllerAction);
            CreateExceptionContextResult(filterContext);
            throw ex;
        }

        /// <summary>
        /// Cria Log.
        /// </summary>
        /// <param name="ex"></param>
        private void LogException(Exception ex)
        {
            LogSystem.LogErrorBusiness.Register(ex);
        }

        /// <summary>
        /// Cria Log.
        /// </summary>
        /// <param name="ex"></param>
        private void LogException(Exception ex, string controllerAction)
        {
            //new CreateLog(ex, controllerAction);
            LogSystem.LogErrorBusiness.Register(ex);
        }

        /// <summary>
        /// Redireciona para uma tela "amigavel" ao receber uma exception.
        /// </summary>
        private const string XMLHttpRequest = "XMLHttpRequest";
        private const string XRequestedWithHeadername = "X-Requested-With";
        private const string JSONErrorMessage = "Sorry, an error occurred while processing your request.";
        private void CreateExceptionContextResult(ExceptionContext exceptionContext)
        {
            //Se for ajax.
            if (exceptionContext.HttpContext.Request.Headers[XRequestedWithHeadername] == XMLHttpRequest)
            {
                //Return JSON
                exceptionContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { error = true, message = JSONErrorMessage }
                };
            }
            else
            {
                ////Redirect user to error page
                //exceptionContext.ExceptionHandled = true;
                //exceptionContext.Result = new RedirectToRouteResult(
                //                   new RouteValueDictionary
                //                   {
                //                       { "action", "Index" },
                //                       { "controller", "Error" }
                //                   });
            }
        }

    }
}
