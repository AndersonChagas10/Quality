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
using Dominio;

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
                        ParFrequencyId as ParFrequency_Id,
                        Evaluation,
                        Sample,
                        ParCluster_Id
                        From ParVinculoPeso
                        where 
                        (ParCompany_Id = @ParCompany_Id or ParCompany_Id is Null)
                        and ParFrequencyId = @ParFrequencyId
                        and (ParCluster_Id = @ParCluster_Id or ParCluster_Id is null)
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

            if (listaParVinculoPeso.Count == 0)
                return listaParLevel1;

            try
            {
                var query = $@" 
                            select
                            Id,
                            HasTakePhoto,
                            Name,
                            GenerateActionOnNotConformity,
                            OpenPhotoGallery
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

        public List<ParLevel2AppViewModel> GetListaParLevel2(List<ParVinculoPesoAppViewModel> listaParVinculoPeso)
        {
            List<ParLevel2AppViewModel> listaParLevel2 = new List<ParLevel2AppViewModel>();
            try
            {
                var query = $@" 
                            select 
                            Id,
                            HasTakePhoto,
                            Name
                            from parlevel2
                            Where 
                            IsActive = 1
                            and Id in ({ string.Join(",", listaParVinculoPeso.Select(x => x.ParLevel2_Id).ToArray()) }) ";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParLevel2 = factory.SearchQuery<ParLevel2AppViewModel>(query).ToList();
                }

                return listaParLevel2;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<ParLevel3AppViewModel> GetListaParLevel3(List<ParVinculoPesoAppViewModel> listaParVinculoPeso)
        {
            List<ParLevel3AppViewModel> listaParLevel3 = new List<ParLevel3AppViewModel>();
            try
            {
                var query = $@" 
                            select 
                            Id,
                            HasTakePhoto,
                            Name
                            from parlevel3
                            Where 
                            IsActive = 1
                            and Id in ({ string.Join(",", listaParVinculoPeso.Select(x => x.ParLevel3_Id).ToArray()) }) ";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParLevel3 = factory.SearchQuery<ParLevel3AppViewModel>(query).ToList();
                }

                return listaParLevel3;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<ParEvaluation> GetListaParEvaluation()
        {
            List<ParEvaluation> listaParEvaluation = new List<ParEvaluation>();
            try
            {
                var query = $@" 
                            DECLARE @ParCompany_Id int = @ParamParCompany_Id;
                            DECLARE @ParFrequencyId int = @ParamParFrequencyId;

                            select
                            *
                            from ParEvaluation
                            where 
                            (ParCompany_Id = @ParamParCompany_Id or ParCompany_Id is null)
                            and (ParFrequency_Id = @ParamParFrequencyId or ParFrequency_Id is null)
                            and IsActive = 1
                            Order By ParCompany_Id Desc";

                using (var factory = new Factory("DefaultConnection"))
                {
                    using (SqlCommand cmd = new SqlCommand(query, factory.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParamParCompany_Id", appParametrization.ParCompany_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParamParFrequencyId", appParametrization.ParFrequency_Id);

                        listaParEvaluation = factory.SearchQuery<ParEvaluation>(cmd).ToList();
                    }
                }

                return listaParEvaluation;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<ParEvaluationXDepartmentXCargoAppViewModel> GetListaParEvaluationXDepartmentXCargoAppViewModel()
        {
            List<ParEvaluationXDepartmentXCargoAppViewModel> listaParEvaluationXDepartmentXCargoAppViewModel = new List<ParEvaluationXDepartmentXCargoAppViewModel>();
            try
            {
                var query = $@"
                            DECLARE @ParCompany_Id int = @ParamParCompany_Id;
                            DECLARE @ParFrequencyId int = @ParamParFrequencyId;
                            DECLARE @ParCluster_Id int = @ParamParCluster_Id;

                            select
                            Id ,
                            ParCompany_Id ,
                            ParDepartment_Id ,
                            ParCargo_Id ,
                            Sample ,
                            Evaluation ,
                            ParCluster_Id,
                            RedistributeWeight
                            from ParEvaluationXDepartmentXCargo
                            Where
                            (ParCompany_Id = @ParamParCompany_Id or ParCompany_Id is null)
                            and ParFrequencyId = @ParamParFrequencyId
                            and ParCluster_Id = @ParamParCluster_Id
                            and IsActive = 1
                            Order By ParCompany_Id Desc";

                using (var factory = new Factory("DefaultConnection"))
                {
                    using (SqlCommand cmd = new SqlCommand(query, factory.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParamParCompany_Id", appParametrization.ParCompany_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParamParFrequencyId", appParametrization.ParFrequency_Id);
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParamParCluster_Id", appParametrization.ParCluster_Id);

                        listaParEvaluationXDepartmentXCargoAppViewModel = factory.SearchQuery<ParEvaluationXDepartmentXCargoAppViewModel>(cmd).ToList();
                    }
                }

                return listaParEvaluationXDepartmentXCargoAppViewModel;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<ParEvaluationSchedule> GetListaParEvaluationSchedule()
        {
            List<ParEvaluationSchedule> listaParEvaluationSchedule = new List<ParEvaluationSchedule>();
            try
            {
                var query = $@" 
                            select
                            Inicio ,
                            Fim ,
                            Av ,
                            Intervalo
                            from ParEvaluationSchedule
                            where 
                            IsActive = 1";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParEvaluationSchedule = factory.SearchQuery<ParEvaluationSchedule>(query).ToList();
                }

                return listaParEvaluationSchedule;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<ParLevel3ValueAppViewModel> GetListaParLevel3Value(List<ParLevel1AppViewModel> listaParLevel1, List<ParLevel2AppViewModel> listaParLevel2, List<ParLevel3AppViewModel> listaParLevel3)
        {
            List<ParLevel3ValueAppViewModel> listaParLevel3Value = new List<ParLevel3ValueAppViewModel>();
            try
            {
                var query = $@" 
                            DECLARE @ParCompany_Id int = @ParamParCompany_Id;

                            select 
                            Id ,
                            DynamicValue ,
                            IntervalMax ,
                            IntervalMin ,
                            AcceptableValueBetween ,
                            ParLevel3BoolFalse_Id ,
                            ParLevel3BoolTrue_Id,
                            ParLevel3InputType_Id ,
                            ParMeasurementUnit_Id ,
                            ParCompany_Id ,
                            ParLevel1_Id ,
                            ParLevel2_Id ,
                            ParLevel3_Id ,
                            ShowLevel3Limits ,
                            IsRequired ,
                            IsNCTextRequired ,
                            IsDefaultAnswer ,
                            IsAtiveNA ,
                            DefaultMessageText ,
                            StringSizeAllowed 
                            from ParLevel3Value
                            Where(ParCompany_Id = @ParamParCompany_Id or ParCompany_Id is null)
                            and IsActive = 1
                            and ParLevel1_Id IN ({ string.Join(",", listaParLevel1.Select(x => x.Id).ToArray()) })
                            and ParLevel2_Id IN ({ string.Join(",", listaParLevel2.Select(x => x.Id).ToArray()) })
                            and ParLevel3_Id IN ({ string.Join(",", listaParLevel3.Select(x => x.Id).ToArray()) })
                            Order By ParCompany_Id Desc";

                using (var factory = new Factory("DefaultConnection"))
                {
                    using (SqlCommand cmd = new SqlCommand(query, factory.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        UtilSqlCommand.AddParameterNullable(cmd, "@ParamParCompany_Id", appParametrization.ParCompany_Id);

                        listaParLevel3Value = factory.SearchQuery<ParLevel3ValueAppViewModel>(cmd).ToList();
                    }
                }

                return listaParLevel3Value;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<ParLevel3InputTypeAppViewModel> GetListaParLevel3InputType()
        {
            List<ParLevel3InputTypeAppViewModel> listaParLevel3InputType = new List<ParLevel3InputTypeAppViewModel>();
            try
            {
                var query = $@" 
                            select 
                            Id ,
                            Name ,
                            Description 
                            from ParLevel3InputType
                            Where IsActive = 1";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParLevel3InputType = factory.SearchQuery<ParLevel3InputTypeAppViewModel>(query).ToList();
                }

                return listaParLevel3InputType;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<ParMeasurementUnitAppViewModel> GetListaParMeasurementUnit()
        {
            List<ParMeasurementUnitAppViewModel> listaParMeasurementUnit = new List<ParMeasurementUnitAppViewModel>();
            try
            {
                var query = $@" 
                            select
                            Id ,
                            Name ,
                            Description 
                            from ParMeasurementUnit
                            Where IsActive = 1";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParMeasurementUnit = factory.SearchQuery<ParMeasurementUnitAppViewModel>(query).ToList();
                }

                return listaParMeasurementUnit;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<ParLevel3BoolTrueAppViewModel> GetListaParLevel3BoolTrue()
        {
            List<ParLevel3BoolTrueAppViewModel> listaParLevel3BoolTrue = new List<ParLevel3BoolTrueAppViewModel>();
            try
            {
                var query = $@" 
                            select 
                            Id ,
                            Name
                            from ParLevel3BoolTrue
                            Where IsActive = 1";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParLevel3BoolTrue = factory.SearchQuery<ParLevel3BoolTrueAppViewModel>(query).ToList();
                }

                return listaParLevel3BoolTrue;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<ParLevel3BoolFalseAppViewModel> GetListaParLevel3BoolFalse()
        {
            List<ParLevel3BoolFalseAppViewModel> listaParLevel3BoolFalse = new List<ParLevel3BoolFalseAppViewModel>();
            try
            {
                var query = $@" 
                            select 
                            Id ,
                            Name
                            from ParLevel3BoolFalse
                            Where IsActive = 1";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParLevel3BoolFalse = factory.SearchQuery<ParLevel3BoolFalseAppViewModel>(query).ToList();
                }

                return listaParLevel3BoolFalse;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<ParLevel3XHelp> GetListaParLevel3XHelp()
        {
            List<ParLevel3XHelp> listaParLevel3XHelp = new List<ParLevel3XHelp>();
            try
            {
                var query = $@" 
                            select
                            *
                            from ParLevel3XHelp
                            where IsActive = 1";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParLevel3XHelp = factory.SearchQuery<ParLevel3XHelp>(query).ToList();
                }

                return listaParLevel3XHelp;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
        public List<ParAlert> GetListaParAlert()
        {
            List<ParAlert> listaParLevel3XHelp = new List<ParAlert>();
            try
            {
                var query = $@" 
                            select
                            *
                            from ParAlert
                            where IsActive = 1
                            and IsCollectAlert = 1";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParLevel3XHelp = factory.SearchQuery<ParAlert>(query).ToList();
                }

                return listaParLevel3XHelp;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<ParHeaderFieldGeral> GetListaParHeaderFieldGeral()
        {
            List<ParHeaderFieldGeral> listaParHeaderFieldGeral = new List<ParHeaderFieldGeral>();
            try
            {
                var query = $@" 
                            select
                            *
                            from ParHeaderFieldGeral
                            where IsActive = 1";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParHeaderFieldGeral = factory.SearchQuery<ParHeaderFieldGeral>(query).ToList();
                }

                return listaParHeaderFieldGeral;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<ParMultipleValuesGeral> GetListaParMultipleValuesGeral()
        {
            List<ParMultipleValuesGeral> listaParMultipleValuesGeral = new List<ParMultipleValuesGeral>();
            try
            {
                var query = $@" 
                            select
                            *
                            from ParMultipleValuesGeral
                            where IsActive = 1";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParMultipleValuesGeral = factory.SearchQuery<ParMultipleValuesGeral>(query).ToList();
                }

                return listaParMultipleValuesGeral;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<ParDepartmentXRotinaIntegracao> GetListaParDepartmentXRotinaIntegracao()
        {
            List<ParDepartmentXRotinaIntegracao> listaParDepartmentXRotinaIntegracao = new List<ParDepartmentXRotinaIntegracao>();
            try
            {
                var query = $@" 
                            select
                            *
                            from ParDepartmentXRotinaIntegracao
                            where IsActive = 1";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParDepartmentXRotinaIntegracao = factory.SearchQuery<ParDepartmentXRotinaIntegracao>(query).ToList();
                }

                return listaParDepartmentXRotinaIntegracao;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<RotinaIntegracao> GetListaRotinaIntegracao()
        {
            List<RotinaIntegracao> listaRotinaIntegracao = new List<RotinaIntegracao>();
            try
            {
                var query = $@" 
                            select
                            *
                            from RotinaIntegracao
                            where IsActive = 1
                            and IsOffline = 0";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaRotinaIntegracao = factory.SearchQuery<RotinaIntegracao>(query).ToList();
                }

                return listaRotinaIntegracao;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<PargroupQualificationXParLevel3Value> GetListaPargroupQualificationXParLevel3Value(List<int> listaParLevel3Value_Ids)
        {
            List<PargroupQualificationXParLevel3Value> listaPargroupQualificationXParLevel3Value = new List<PargroupQualificationXParLevel3Value>();
            try
            {
                var query = $@" 
                            select
                            Id,
                            PargroupQualification_Id ,
                            ParLevel3Value_Id ,
                            Value ,
                            IsActive ,
                            IsRequired 
                            from PargroupQualificationXParLevel3Value
                            where IsActive = 1
                            and ParLevel3Value_Id in ({ string.Join(",", listaParLevel3Value_Ids) })";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaPargroupQualificationXParLevel3Value = factory.SearchQuery<PargroupQualificationXParLevel3Value>(query).ToList();
                }

                return listaPargroupQualificationXParLevel3Value;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<PargroupQualificationXParQualification> GetListaPargroupQualificationXParQualification(List<int> listaPargroupQualificationXParLevel3Value_Ids)
        {
            List<PargroupQualificationXParQualification> listaPargroupQualificationXParQualification = new List<PargroupQualificationXParQualification>();
            try
            {
                if (listaPargroupQualificationXParLevel3Value_Ids.Count == 0)
                    return listaPargroupQualificationXParQualification;

                var query = $@" 
                            select
                            Id ,
                            PargroupQualification_Id ,
                            ParQualification_Id ,
                            IsActive 
                            from PargroupQualificationXParQualification
                            where IsActive = 1
                            and PargroupQualification_Id in ({ string.Join(",", listaPargroupQualificationXParLevel3Value_Ids) })";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaPargroupQualificationXParQualification = factory.SearchQuery<PargroupQualificationXParQualification>(query).ToList();
                }

                return listaPargroupQualificationXParQualification;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<ParQualification> GetListaParQualification(List<int?> listaPargroupQualificationXParQualification_Ids)
        {
            List<ParQualification> listaParQualification = new List<ParQualification>();
            if (listaPargroupQualificationXParQualification_Ids.Count() == 0)
                return listaParQualification;
            try
            {
                var query = $@" 
                            select 
                            Id ,
                            Name ,
                            IsActive 
                            from ParQualification
                            where IsActive = 1
                            and Id in ({ string.Join(",", listaPargroupQualificationXParQualification_Ids) })";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaParQualification = factory.SearchQuery<ParQualification>(query).ToList();
                }

                return listaParQualification;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<PargroupQualification> GetListaPargroupQualification(List<int?> listaPargroupQualification_Ids)
        {
            List<PargroupQualification> listaPargroupQualification = new List<PargroupQualification>();
            if (listaPargroupQualification_Ids.Count() == 0)
                return listaPargroupQualification;
            try
            {
                var query = $@" 
                            select
                            Id ,
                            Name ,
                            IsActive 
                            from PargroupQualification
                            where IsActive = 1
                            and Id in ({ string.Join(",", listaPargroupQualification_Ids) })";

                using (var factory = new Factory("DefaultConnection"))
                {
                    listaPargroupQualification = factory.SearchQuery<PargroupQualification>(query).ToList();
                }

                return listaPargroupQualification;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
    }
}
