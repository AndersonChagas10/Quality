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
    public class ParClusterGroupBusiness
    {
        public List<ParClusterGroup> GetListaParClusterGroup(int parCompany_Id)
        {
            var listaClusterGroup = new List<ParClusterGroup>();
            var listaCluster = new List<ParCluster>();
            var listaParVinculoPesoClusterIds = new List<ParVinculoPeso>();

            try
            {
                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParVinculoPesoClusterIds = GetListaParVinculoPesoClusterIds(parCompany_Id);

                    if (listaParVinculoPesoClusterIds.Count > 0)
                    {
                        listaCluster = GetListaParCluster(listaParVinculoPesoClusterIds);

                        if (listaCluster.Count > 0)
                        {
                            listaClusterGroup = GetListaParClusterGroup(listaCluster);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return listaClusterGroup;
        }

        private List<ParClusterGroup> GetListaParClusterGroup(List<ParCluster> listaCluster)
        {
            var listaClusterGroup = new List<ParClusterGroup>();

            var querylistaClustersGroup = $@"
                               select * from ParClusterGroup where Id in({ string.Join(",", listaCluster.Select(x => x.ParClusterGroup_Id).ToArray()) })
                            ";
            using (var factory = new Factory("DefaultConnection"))
            {
                using (SqlCommand cmd = new SqlCommand(querylistaClustersGroup, factory.connection))
                {
                    cmd.CommandType = CommandType.Text;

                    listaClusterGroup = factory.SearchQuery<ParClusterGroup>(cmd).ToList();
                }
            }
            return listaClusterGroup;
        }

        private List<ParCluster> GetListaParCluster(List<ParVinculoPeso> listaParVinculoPesoClusterIds)
        {
            var listaCluster = new List<ParCluster>();

            var querylistaClusters = $@"
                                select * from ParCluster where Id in ({ string.Join(",", listaParVinculoPesoClusterIds.Select(x => x.ParCluster_Id).ToArray()) })
                            ";
            using (var factory = new Factory("DefaultConnection"))
            {
                using (SqlCommand cmd = new SqlCommand(querylistaClusters, factory.connection))
                {
                    cmd.CommandType = CommandType.Text;

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
