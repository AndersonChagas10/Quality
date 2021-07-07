using Conformity.Domain.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Conformity.Infra.Data.Core.Repository.PlanoDeAcao
{
    public class EvidenciaConcluidaRepository
    {
        private readonly ADOContext _aDOContext;

        public EvidenciaConcluidaRepository(ADOContext aDOContext)
        {
            _aDOContext = aDOContext;
        }

        public List<EvidenciaViewModel> BuscarListaEvidenciasConcluidas(int acao_Id)
        {
            var lista = new List<EvidenciaViewModel>();

            var query = $@"select * from Pa.EvidenciaAcaoConcluida
                            where Acao_id = {acao_Id}
                            and IsActive = 1";

            
             lista = _aDOContext.SearchQuery<EvidenciaViewModel>(query).ToList();

            return lista;
        }

        public void InativarEvidenciasDaAcaoConcluida(List<EvidenciaViewModel> listaInativar)
        {
            foreach (var item in listaInativar)
            {
                try
                {
                    string sql = $@" UPDATE Pa.EvidenciaAcaoConcluida 
                                        set IsActive = 0 
                                    where Id = @Id";

                    _aDOContext.ExecuteStoredProcedure<SqlCommand>(sql);

                    //using (Factory factory = new Factory("DefaultConnection"))
                    //{
                    //    using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                    //    {
                    //        cmd.CommandType = CommandType.Text;
                    //        UtilSqlCommand.AddParameterNullable(cmd, "@Id", item.Id);

                    //        var id = Convert.ToInt32(cmd.ExecuteScalar());

                    //    }
                    //}
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
