using Newtonsoft.Json;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSystem
{
    public class LogErrorBusiness : LogSystem
    {
        public static void Register(Exception ex, object obj = null)
        {
            Dominio.LogError error = new Dominio.LogError();

            // Get stack trace for the exception with source file information
            var st = new StackTrace(ex, true);
            // Get the top stack frame
            var frame = st.GetFrame(0);
            // Get the line number from the stack frame
            var line = frame?.GetFileLineNumber();

            //monta o objeto com as informações do log
            error.AddDate = DateTime.Now;
            error.Line = line ?? 0;
            error.Method = frame?.GetMethod().Name;
            error.Controller = frame?.GetMethod().DeclaringType.Name;
            error.Object = obj?.GetType() != typeof(string) ? LogErrorBusiness.ToJson(obj).ToString() : "";
            error.StackTrace = ex.ToClient();
            error.StackTrace = error.StackTrace.Substring(0, error.StackTrace.Length > 900 ? 900 : error.StackTrace.Length);

            using (var db = new Dominio.SgqDbDevEntities())
            {
                db.LogError.Add(error);
                db.SaveChanges();
            }
        }

    }
}
