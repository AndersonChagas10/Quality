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
            var listaCluster = new List<ParCluster>();

            try
            {
                using (var factory = new Factory("DefaultConnection"))
                {
                    listaCluster = GetParClusterList(parClusterGroupId, parCompany_Id);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return listaCluster;
        }

        private List<ParCluster> GetParClusterList(int parClusterGroupId, int parCompany_Id)
        {
            var listaCluster = new List<ParCluster>();

            var query = $@"
                            DECLARE @ParClusterGroup_Id int = @ParamParClusterGroup_Id;
                            DECLARE @ParCompany_Id int = @ParamParCompany_Id;

                            select * from ParCluster PC where Id in (select Distinct(PVP.ParCluster_Id) from ParVinculoPeso PVP
	                                INNER join ParCompanyCluster PCxC on PVP.ParCluster_Id = PCxC.ParCluster_Id and PCxC.Active = 1
                                    where ((PVP.ParCompany_Id = @ParCompany_Id OR PVP.ParCompany_Id is null) and PCxC.ParCompany_Id = @ParCompany_Id) and PVP.IsActive = 1 )
                            and PC.ParClusterGroup_Id = @ParClusterGroup_Id
                            ";

            using (var factory = new Factory("DefaultConnection"))
            {
                using (SqlCommand cmd = new SqlCommand(query, factory.connection))
                {
                    cmd.CommandType = CommandType.Text;
                    UtilSqlCommand.AddParameterNullable(cmd, "@ParamParClusterGroup_Id", parClusterGroupId);
                    UtilSqlCommand.AddParameterNullable(cmd, "@ParamParCompany_Id", parCompany_Id);

                    listaCluster = factory.SearchQuery<ParCluster>(cmd).ToList();
                }
            }
            return listaCluster;
        }
    }
}
