using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.Entities.PlanoDeAcao;
using Conformity.Infra.CrossCutting;
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

                    using (SqlCommand cmd = new SqlCommand(sql, _aDOContext.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.AddParameterNullable("@Id", item.Id);

                        var id = Convert.ToInt32(cmd.ExecuteScalar());

                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        public void SalvarEvidenciaAcaoConcluida(EvidenciaConcluida evidenciaAcaoConcluida)
        {
            try
            {
                string sql = $@"INSERT INTO Pa.EvidenciaAcaoConcluida(
                                    Acao_Id				
                                    ,Path
                                    ,AddDate)
                                    VALUES(
                                          @Acao_Id			
                                         ,@Path	
                                         ,@AddDate			
                                        )";

                using (SqlCommand cmd = new SqlCommand(sql, _aDOContext.connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParameterNullable("@Acao_Id", evidenciaAcaoConcluida.Acao_Id);
                    cmd.AddParameterNullable("@Path", evidenciaAcaoConcluida.Path);
                    cmd.AddParameterNullable("@AddDate", DateTime.Now);

                    var id = Convert.ToInt32(cmd.ExecuteScalar());

                    evidenciaAcaoConcluida.Id = id;
                }
            }
            catch (Exception e)
            {

            }
        }

    }
}
