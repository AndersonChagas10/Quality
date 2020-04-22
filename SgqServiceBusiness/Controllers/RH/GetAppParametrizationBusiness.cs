using ADOFactory;
using Dominio.AppViewModel;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SgqSystem;
using ServiceModel;

namespace SgqServiceBusiness.Controllers.RH
{
    public class GetAppParametrizationBusiness
    {
        public PlanejamentoColeta appParametrization { get; set; }

        public GetAppParametrizationBusiness(PlanejamentoColeta appParametrization)
        {
            this.appParametrization = appParametrization;
        }

        public List<ParVinculoPesoAppViewModel> GetListaParVinculoPeso()
        {

            List<ParVinculoPesoAppViewModel> listaParVinculoPeso = new List<ParVinculoPesoAppViewModel>();

            var query = $@"
                        DECLARE @ParCompany_Id int = @ParamParCompany_Id;
                        DECLARE @ParFrequencyId int = @ParamParFrequencyId;
                        DECLARE @ParCluster_Id int = @ParamParCluster_Id;

                        select Id,
                        ParLevel1_Id,
                        ParLevel2_Id,
                        ParLevel3_Id,
                        ParCompany_Id,
                        ParDepartment_Id,
                        ParGroupParLevel1_Id,
                        Peso,
                        ParCargo_Id,
                        ParFrequencyId,
                        Evaluation,
                        Sample
                        ParCluster_Id
                        From ParVinculoPeso
                        where ParFrequencyId = @ParFrequencyId
                        and ParCluster_Id = @ParCluster_Id or ParCluster_Id is null
                        and (ParCompany_Id = @ParCompany_Id or ParCompany_Id is null)
                        and IsActive = 1
                        order by ParCompany_Id desc";

            using (var factory = new Factory("DefaultConnection"))
            {
                using (SqlCommand cmd = new SqlCommand(query, factory.connection))
                {
                    cmd.CommandType = CommandType.Text;
                    UtilSqlCommand.AddParameterNullable(cmd, "@ParamParCompany_Id", appParametrization.ParCompany_Id);
                    UtilSqlCommand.AddParameterNullable(cmd, "@ParamParFrequencyId", appParametrization.ParFrequency_Id);
                    UtilSqlCommand.AddParameterNullable(cmd, "@ParamParCluster_Id", appParametrization.ParCluster_Id);

                    listaParVinculoPeso = factory.SearchQuery<ParVinculoPesoAppViewModel>(cmd).ToList();
                }
            }

            return listaParVinculoPeso;
        }

        public List<ParLevel1AppViewModel> GetListaParLevel1(List<ParVinculoPesoAppViewModel> listaParVinculoPeso)
        {
            List<ParLevel1AppViewModel> listaParLevel1 = new List<ParLevel1AppViewModel>();
            try
            {
                var query = $@" 
                            select
                            Id,
                            HasTakePhoto,
                            Name
                            from ParLevel1
                            where
                            IsActive = 1
                            and Id in ({ string.Join(",", listaParVinculoPeso.Select(x => x.ParLevel1_Id).ToArray()) }) ";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParLevel1 = factory.SearchQuery<ParLevel1AppViewModel>(query).ToList();
                }

                return listaParLevel1;
            }
            catch (Exception e)
            {

                throw e;
            }
    
        }
    }
}
