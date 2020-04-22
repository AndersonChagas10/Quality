using ADOFactory;
using Dominio;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SgqServiceBusiness.Controllers.RH
{
    public class ParClusterBusiness
    {
        public List<ParCluster> GetListaParCluster(int parCompany_Id, int parClusterGroupId)
        {
            var listaParVinculoPesoClusterIds = new List<ParVinculoPeso>();
            var listaCluster = new List<ParCluster>();

            try
            {
                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParVinculoPesoClusterIds = GetListaParVinculoPesoClusterIds(parCompany_Id);

                    if(listaParVinculoPesoClusterIds.Count > 0)
                    {
                        listaCluster = GetListaParCluster(listaParVinculoPesoClusterIds, parClusterGroupId);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return listaCluster;
        }

        private List<ParCluster> GetListaParCluster(List<ParVinculoPeso> listaParVinculoPesoClusterIds, int parClusterGroupId)
        {
            var listaCluster = new List<ParCluster>();
            var query = $@"
                            DECLARE @ParClusterGroup_Id int = @ParamParClusterGroup_Id;
                            select * from ParCluster PC where Id in ({ string.Join(",", listaParVinculoPesoClusterIds.Select(x => x.ParCluster_Id).ToArray()) }) 
                            and PC.ParClusterGroup_Id = @ParClusterGroup_Id
                            ";
            using (var factory = new Factory("DefaultConnection"))
            {
                using (SqlCommand cmd = new SqlCommand(query, factory.connection))
                {
                    cmd.CommandType = CommandType.Text;
                    UtilSqlCommand.AddParameterNullable(cmd, "@ParamParClusterGroup_Id", parClusterGroupId);

                    listaCluster = factory.SearchQuery<ParCluster>(cmd).ToList();
                }
            }
            return listaCluster;
        }

        private List<ParVinculoPeso> GetListaParVinculoPesoClusterIds(int parCompany_Id)
        {
            var listaParVinculoPesoClusterIds = new List<ParVinculoPeso>();
            var querylistaParVinculoPeso = $@"
                            DECLARE @ParCompany_Id int = @ParamParCompany_Id;
                            select Distinct(PVP.ParCluster_Id) from ParVinculoPeso PVP
	                        INNER join ParCompanyCluster PCxC on PVP.ParCluster_Id = PCxC.ParCluster_Id
	                        where (PVP.ParCompany_Id = @ParCompany_Id or PVP.ParCompany_Id is Null) and PVP.IsActive = 1 and PCxC.Active = 1
                            ";
            using (var factory = new Factory("DefaultConnection"))
            {
                using (SqlCommand cmd = new SqlCommand(querylistaParVinculoPeso, factory.connection))
                {
                    cmd.CommandType = CommandType.Text;
                    UtilSqlCommand.AddParameterNullable(cmd, "@ParamParCompany_Id", parCompany_Id);

                    listaParVinculoPesoClusterIds = factory.SearchQuery<ParVinculoPeso>(cmd).ToList();
                }
            }
            return listaParVinculoPesoClusterIds;
        }
    }
}
