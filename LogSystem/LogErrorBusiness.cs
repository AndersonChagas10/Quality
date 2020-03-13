using ADOFactory;
using Newtonsoft.Json;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSystem
{
    public class LogErrorBusiness : LogSystem
    {
        public static void TryRegister(Exception ex, object obj = null)
        {
            try
            {
                Register(ex, obj);
            }
            catch { }
        }

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
            error.Controller = frame?.GetMethod().DeclaringType?.Name;
            error.Object = obj?.GetType() != typeof(string) ? LogErrorBusiness.ToJson(obj).ToString() : "";
            error.StackTrace = ex.ToClient();
            error.StackTrace = error.StackTrace.Substring(0, error.StackTrace.Length > 900 ? 900 : error.StackTrace.Length);

            string sql = $@"
INSERT INTO [dbo].[LogError]
           ([ErrorMessage]
           ,[Line]
           ,[Method]
           ,[Controller]
           ,[Object]
           ,[StackTrace]
           ,[AddDate])
     VALUES
           (@ErrorMessage
           ,@Line
           ,@Method
           ,@Controller
           ,@Object
           ,@StackTrace
           ,@AddDate);
            SELECT @@IDENTITY AS 'Identity';";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                {
                    cmd.CommandType = CommandType.Text;
                    UtilSqlCommand.AddParameterNullable(cmd, "@ErrorMessage", error.ErrorMessage);
                    UtilSqlCommand.AddParameterNullable(cmd, "@Line", error.Line);
                    UtilSqlCommand.AddParameterNullable(cmd, "@Method", error.Method);
                    UtilSqlCommand.AddParameterNullable(cmd, "@Controller", error.Controller);
                    UtilSqlCommand.AddParameterNullable(cmd, "@Object", error.Object);
                    UtilSqlCommand.AddParameterNullable(cmd, "@StackTrace", error.StackTrace);
                    UtilSqlCommand.AddParameterNullable(cmd, "@AddDate", error.AddDate);
                    var id = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

    }
}
