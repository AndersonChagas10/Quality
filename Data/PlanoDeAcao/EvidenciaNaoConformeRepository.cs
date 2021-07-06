using ADOFactory;
using DTO.PlanoDeAcao;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Data.PlanoDeAcao.Repositorio
{
    public class EvidenciaNaoConformeRepository : IEvidenciaNaoConformeRepository
    {
        public List<EvidenciaViewModel> BuscarListaEvidencias(int acao_Id)
        {
            var lista = new List<EvidenciaViewModel>();

            var query = $@"select * from Pa.EvidenciaNaoConformidade
                            where Acao_id = {acao_Id}
                            and IsActive = 1";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                lista = factory.SearchQuery<EvidenciaViewModel>(query).ToList();
            }

            return lista;
        }

        public List<EvidenciaViewModel> RetornarEvidenciasDaAcao(int acao_Id)
        {
            var query = $@"
                select * from Pa.EvidenciaNaoConformidade 
                where Acao_Id = {acao_Id}
                and IsActive = 1
            ";

            using (ADOFactory.Factory factory = new ADOFactory.Factory("DefaultConnection"))
            {
                var lista = factory.SearchQuery<EvidenciaViewModel>(query);
                return lista;
            }
        }

        public void InativarEvidencias(List<EvidenciaViewModel> listaInativar)
        {
            foreach (var item in listaInativar)
            {
                try
                {
                    string sql = $@" UPDATE Pa.EvidenciaNaoConformidade 
                                        set IsActive = 0 
                                    where Id = @Id";

                    using (Factory factory = new Factory("DefaultConnection"))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                        {
                            cmd.CommandType = CommandType.Text;
                            UtilSqlCommand.AddParameterNullable(cmd, "@Id", item.Id);

                            var id = Convert.ToInt32(cmd.ExecuteScalar());

                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
