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
    public class ParCompanyBusiness
    {
        public List<ParCompany> GetListaParCompany(int userSgq_Id)
        {
            var listaCompany = new List<ParCompany>();

            try
            {
                using (var factory = new Factory("DefaultConnection"))
                {
                    listaCompany = GetParCompanyList(userSgq_Id);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return listaCompany;
        }

        private List<ParCompany> GetParCompanyList(int userSgq_Id)
        {
            var listaParCompany = new List<ParCompany>();

            var query = $@"
                    DECLARE @UserSgq_Id int = @ParamUserSgq_Id;

		            select DISTINCT ParCompany.* from ParCompanyXUserSgq PCXU 
			            inner join ParCompany on PCXU.ParCompany_Id = ParCompany.Id
			            where PCXU.UserSgq_Id = @UserSgq_Id and Exists (
                        select * from ParClusterGroup where Id in(
	                        select PC.ParClusterGroup_Id from ParCluster PC where Id in(
		                        select Distinct(PVP.ParCluster_Id) from ParVinculoPeso PVP
		                        INNER join ParCompanyCluster PCxC on PVP.ParCluster_Id = PCxC.ParCluster_Id and PCxC.Active = 1
		                        where ((PVP.ParCompany_Id = ParCompany.Id OR PVP.ParCompany_Id is null) and PCxC.ParCompany_Id = ParCompany.Id) and PVP.IsActive = 1 )))
                            ";

            using (var factory = new Factory("DefaultConnection"))
            {
                using (SqlCommand cmd = new SqlCommand(query, factory.connection))
                {
                    cmd.CommandType = CommandType.Text;
                    UtilSqlCommand.AddParameterNullable(cmd, "@ParamUserSgq_Id", userSgq_Id);

                    listaParCompany = factory.SearchQuery<ParCompany>(cmd).ToList();
                }
            }
            return listaParCompany;
        }

        public ParCompany GetBy(int id)
        {
            ParCompany ParCluster;

            using (var factory = new Factory("DefaultConnection"))
            {

                ParCluster = factory.SearchQuery<ParCompany>($"SELECT * FROM ParCompany WHERE ID = {id}").SingleOrDefault();

            }

            return ParCluster;
        }
    }
}
