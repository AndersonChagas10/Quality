using Conformity.Domain.Core.Entities.Log;
using Conformity.Infra.CrossCutting;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Conformity.Infra.Data.Core.Repository.Log
{
    public class LogErrorRepository
    {
        private readonly ADOContext _aDOContext;

        public LogErrorRepository(ADOContext aDOContext)
        {
            _aDOContext = aDOContext;
        }

        public void Add(LogError error)
        {
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

            using (SqlCommand cmd = new SqlCommand(sql, _aDOContext.connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.AddParameterNullable("@ErrorMessage", error.ErrorMessage);
                cmd.AddParameterNullable("@Line", error.Line);
                cmd.AddParameterNullable("@Method", error.Method);
                cmd.AddParameterNullable("@Controller", error.Controller);
                cmd.AddParameterNullable("@Object", error.Object);
                cmd.AddParameterNullable("@StackTrace", error.StackTrace);
                cmd.AddParameterNullable("@AddDate", error.AddDate);
                int id = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
