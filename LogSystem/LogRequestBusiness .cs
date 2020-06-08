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
    public class LogRequestBusiness : LogSystem
    {
        private Dominio.LogRequest _LogRequest;

        public LogRequestBusiness()
        {
        }

        public void TryRegister(Dominio.LogRequest logRequest)
        {
            try
            {
                Register(logRequest);
            }
            catch (Exception ex)
            {
            }
        }

        public void RegistrarInicioRequisicao(string request, string path, object _params, object userId)
        {
            _LogRequest = new Dominio.LogRequest();
            _LogRequest.DateStart = DateTime.Now;
            _LogRequest.Request = request;
            _LogRequest.Path = path;

            //TODO: melhorar esse if (criado para não tentar salvar o arquivo excel de importação e quebrar o mesmo)
            if (path.Contains("/ImportacaoExcel/Importar"))
                _params = "";

            _LogRequest.Params = JsonConvert.SerializeObject(_params, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            });

            int _userId;
            if (userId != null && int.TryParse(userId.ToString(), out _userId))
            {
                _LogRequest.UserId = _userId;
            }
        }
        public void RegistrarFimRequisicao()
        {
            if (_LogRequest != null)
            {
                _LogRequest.DateEnd = DateTime.Now;
                TryRegister(_LogRequest);
            }
        }

        public void Register(Dominio.LogRequest logRequest)
        {
            string sql = $@"
INSERT INTO [dbo].[LogRequest]
           ([datestart]
           ,[dateend]
           ,[params]
           ,[path]
           ,[request]
           ,[UserId])
     VALUES
           (@DateStart
           ,@DateEnd
           ,@Params
           ,@Path
           ,@Request
           ,@UserId);
            SELECT @@IDENTITY AS 'Identity';";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                {
                    cmd.CommandType = CommandType.Text;
                    UtilSqlCommand.AddParameterNullable(cmd, "@DateStart", logRequest.DateStart);
                    UtilSqlCommand.AddParameterNullable(cmd, "@DateEnd", logRequest.DateEnd);
                    UtilSqlCommand.AddParameterNullable(cmd, "@Params", logRequest.Params);
                    UtilSqlCommand.AddParameterNullable(cmd, "@Path", logRequest.Path);
                    UtilSqlCommand.AddParameterNullable(cmd, "@Request", logRequest.Request);
                    UtilSqlCommand.AddParameterNullable(cmd, "@UserId", logRequest.UserId);
                    var id = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

    }
}
