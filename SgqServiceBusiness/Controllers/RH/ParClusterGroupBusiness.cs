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

            try
            {
                listaClusterGroup = GetListaParVinculoPesoClusterIds(parCompany_Id);
            }
            catch (Exception e)
            {
                throw e;
            }
            return listaClusterGroup;
        }

        private List<ParClusterGroup> GetListaParVinculoPesoClusterIds(int parCompany_Id)
        {

            var listaClusterGroup = new List<ParClusterGroup>();

            var query = $@"	
                            DECLARE @ParCompany_Id int = @ParamParCompany_Id;
	                        select * from ParClusterGroup where Id in(
		                        select PC.ParClusterGroup_Id from ParCluster PC where Id in(
		                         select Distinct(PVP.ParCluster_Id) from ParVinculoPeso PVP
			                        INNER join ParCompanyCluster PCxC on PVP.ParCluster_Id = PCxC.ParCluster_Id and PCxC.Active = 1
			                        where ((PVP.ParCompany_Id = @ParCompany_Id OR PVP.ParCompany_Id is null) and PCxC.ParCompany_Id = @ParCompany_Id) and PVP.IsActive = 1 ))";

            using (var factory = new Factory("DefaultConnection"))
            {
                using (SqlCommand cmd = new SqlCommand(query, factory.connection))
                {
                    cmd.CommandType = CommandType.Text;
                    UtilSqlCommand.AddParameterNullable(cmd, "@ParamParCompany_Id", parCompany_Id);

                    listaClusterGroup = factory.SearchQuery<ParClusterGroup>(cmd).ToList();
                }
            }
            return listaClusterGroup;
        }
    }
}
