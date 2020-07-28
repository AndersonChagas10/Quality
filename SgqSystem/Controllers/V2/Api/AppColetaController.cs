using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Dominio;
using SgqSystem.Controllers.Api;
using Dominio.AppViewModel;
using ADOFactory;
using System.Web.Http.Cors;
using SgqSystem.Helpers;
using SgqSystem.ViewModels;
using SgqSystem.Jobs;
using System.Data.SqlClient;
using SgqServiceBusiness.Controllers.RH;
using ServiceModel;

namespace SgqSystem.Controllers.V2.Api
{
    [RoutePrefix("api/AppColeta")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AppColetaController : BaseApiController
    {
        public class SimpleCollect
        {
            public string Evaluation { get; set; }
            public string Sample { get; set; }
            public string ParDepartment_Id { get; set; }
            public string ParCargo_Id { get; set; }
            public string ParCompany_Id { get; set; }
            public string ParLevel1_Id { get; set; }
            public string ParLevel2_Id { get; set; }
            public string ParLevel3_Id { get; set; }
            public string IntervalMin { get; set; }
            public string IntervalMax { get; set; }
            public string IsConform { get; set; }
            public string Value { get; set; }
            public string ValueText { get; set; }
            public string NotEvaluated { get; set; }
            public DateTime CollectionDate { get; set; }
            public bool IsCollected { get; set; }
            public string HasError { get; set; }
        }

        #region Coleta Padrão RH
        [Route("SetCollect")]
        public IHttpActionResult SetCollect(List<Collection> listSimpleCollect)
        {
            #region Gambi Log de Coletas
            var guiid = Guid.NewGuid();
            LogSystem.LogErrorBusiness.TryRegister(new Exception("Iniciado o registro das coletas (" + listSimpleCollect.Count + ")")
                , new { GUIID = guiid.ToString() });
            #endregion

            //var parLevel3List = db.ParLevel3.ToList();
            AppColetaBusiness appColetaBusiness = new AppColetaBusiness();

            #region ColetaParcial
            var listaParcial = appColetaBusiness.SaveCollectionPartial(listSimpleCollect, guiid);
            #endregion

            listSimpleCollect = listSimpleCollect.Where(x => !x.IsPartialSave).ToList();

            DateTime dataSetCollect = DateTime.Now;
            List<Collection> listaSimpleCollectDuplicadas = new List<Collection>();

            //Adiciona os arquivos na Collection
            foreach (var item in listSimpleCollect)
            {
                try
                {
                    item.AddDate = dataSetCollect;
                    item.Shift_Id = 1;
                    item.Period_Id = 1;
                    item.IsProcessed = false;

                    List<Collection> collectionDuplicada = new List<Collection>();

                    collectionDuplicada = appColetaBusiness.GetCollectionDuplicada(item);

                    if (collectionDuplicada.Count == 0)
                    {
                        appColetaBusiness.SaveCollection(item);
                    }
                    else
                    {
                        listaSimpleCollectDuplicadas.Add(item);
                        item.Id = collectionDuplicada[0].Id;
                    }
                }
                catch (Exception ex)
                {
                    item.HasError = true;
                    item.GUIID = guiid.ToString();

                    LogSystem.LogErrorBusiness.TryRegister(ex, new { GUIID = guiid.ToString() });

                }
            }

            var listaDeColetasComErro = listSimpleCollect.Where(x => x.HasError == true).ToList();
            var listaDeColetasSemErro = listSimpleCollect
                .Where(x => x.HasError != true)
                .ToList();
            var listaDeColetasDuplicadas = listSimpleCollect
                .Where(x => x.HasError != true && listaSimpleCollectDuplicadas.Any(y => y.Id == x.Id))
                .ToList();

            #region Gambi Log de Coletas
            LogSystem.LogErrorBusiness.TryRegister(new Exception("Finalizou a inserção das coletas (" + listaDeColetasDuplicadas.Count + "/" + listaDeColetasSemErro.Count + "/" + listSimpleCollect.Count + ")"),
                new { GUIID = guiid.ToString(), ListaCollection = string.Join(",", listaDeColetasSemErro.Select(x => x.Id)) });
            #endregion

            if (listSimpleCollect.Count > 0 && listaDeColetasComErro.Count == listSimpleCollect.Count)
                return BadRequest("Ocorreu erro em todas as tentativas de registrar as coletas.");

            #region Consolidação Sincrona
            int intervalTimeCollectionJob = 0;
            try
            {
                Int32.TryParse(DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.CollectionJobTime0IsDisabled, out intervalTimeCollectionJob);
            }
            catch (Exception)
            {
            }

            if (intervalTimeCollectionJob <= 0)
            {
                var coletasRegistradasPorCollectionLevel2 = listaDeColetasSemErro
                .Where(x => x.ParHeaderField_Id == null
                && x.ParHeaderField_Value == null
                && x.Evaluation != null
                && x.Sample != null
                && !listaSimpleCollectDuplicadas.Any(y => y.Id == x.Id))
                .Select(x => new CollectionLevel2()
                {
                    EvaluationNumber = (int)x.Evaluation,
                    Sample = x.Sample.Value,
                    ParLevel1_Id = x.ParLevel1_Id.Value,
                    ParLevel2_Id = x.ParLevel2_Id.Value,
                    Shift = x.Shift_Id.Value,
                    Period = x.Period_Id.Value,
                    UnitId = x.ParCompany_Id.Value,
                    ParCargo_Id = x.ParCargo_Id,
                    ParCluster_Id = x.ParCluster_Id,
                    ParDepartment_Id = x.ParDepartment_Id,
                    ParFrequency_Id = x.Parfrequency_Id,
                    AuditorId = x.UserSgq_Id ?? 0,
                    CollectionDate = x.CollectionDate.Value,
                    Outros = x.Outros
                })
                .Distinct()
                .ToList();

                CollectionJob.ConsolidarCollectionLevel2(coletasRegistradasPorCollectionLevel2);
            }
            #endregion

            #region Gambi Log de Coletas
            try
            {
                LogSystem.LogErrorBusiness.Register(new Exception("Finalizado sem nenhum problema"), new { GUIID = guiid.ToString() });
            }
            catch { }
            #endregion

            appColetaBusiness.DeleteCollectionsPartialDuplicadas(listaDeColetasSemErro, guiid);

            listaDeColetasSemErro.AddRange(listaParcial);

            return Ok(listaDeColetasSemErro);

        }

        [HttpPost]
        [Route("GetAppParametrization")]
        public IHttpActionResult GetAppParametrization(PlanejamentoColeta appParametrization)
        {
            InicioRequisicao();
            List<ParVinculoPesoAppViewModel> listaParVinculoPeso;
            List<ParLevel1AppViewModel> listaParLevel1;
            List<ParLevel2AppViewModel> listaParLevel2;
            List<ParLevel3AppViewModel> listaParLevel3;
            List<ParDepartmentAppViewModel> listaParDepartment;
            List<ParCargoAppViewModel> listaParCargo;
            List<ParCargoXDepartmentAppViewModel> listaParCargoXDepartment;
            List<ParEvaluationXDepartmentXCargoAppViewModel> listaParEvaluationXDepartmentXCargoAppViewModel;
            List<ParLevel3ValueAppViewModel> listaParLevel3Value;
            List<ParLevel3InputTypeAppViewModel> listaParLevel3InputType;
            List<ParMeasurementUnitAppViewModel> listaParMeasurementUnit;
            List<ParLevel3BoolTrueAppViewModel> listaParLevel3BoolTrue;
            List<ParLevel3BoolFalseAppViewModel> listaParLevel3BoolFalse;
            List<ParLevel3XHelp> listaParLevel3XHelp;
            List<ParAlert> listaParAlert;
            //List<ParHeaderField> listaParHeaderField;
            //List<ParDepartmentXHeaderField> listaParDepartmentXHeaderField;
            List<ParHeaderFieldGeral> listaParHeaderFieldGeral;
            //List<ParMultipleValues> listaParMultipleValues;
            List<ParMultipleValuesGeral> listaParMultipleValuesGeral;
            List<ParDepartmentXRotinaIntegracao> listaParDepartmentXRotinaIntegracao;
            List<RotinaIntegracao> listaRotinaIntegracao;
            List<RotinaIntegracaoViewModel> listaRotinaIntegracaoOffline;
            List<ParEvaluation> listaParEvaluation;
            List<ParCluster> listaParCluster;
            List<ParClusterGroup> listaParClusterGroup;
            List<ParQualificationViewModel> listaParQualification;
            List<PargroupQualificationXParQualificationVewModel> listaPargroupQualificationXParQualification;
            List<PargroupQualificationXParLevel3ValueViewModel> listaPargroupQualificationXParLevel3Value;
            List<PargroupQualificationViewModel> listaPargroupQualification;

            GetAppParametrizationBusiness business = new GetAppParametrizationBusiness(appParametrization);

            using (Dominio.SgqDbDevEntities db = new Dominio.SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                db.Database.CommandTimeout = 180;

                listaParVinculoPeso = business.GetListaParVinculoPeso();

                listaParLevel1 = business.GetListaParLevel1(listaParVinculoPeso);

                //listaParLevel1 = db.ParLevel1
                //   .AsNoTracking()
                //   .Where(x => x.IsActive)
                //   .Select(x => new ParLevel1AppViewModel()
                //   {
                //       Id = x.Id,
                //       HasTakePhoto = x.HasTakePhoto,
                //       Name = x.Name
                //   })
                //   .ToList()
                //   .Where(x => listaParVinculoPeso.Any(y => y.ParLevel1_Id == x.Id))
                //   .ToList();

                listaParLevel2 = db.ParLevel2
                    .AsNoTracking()
                    //.Where(x => x.ParFrequency_Id == appParametrization.ParFrequency_Id)
                    .Where(x => x.IsActive)
                    .Select(x => new ParLevel2AppViewModel()
                    {
                        Id = x.Id,
                        HasTakePhoto = x.HasTakePhoto,
                        Name = x.Name
                    })
                    .ToList()
                    .Where(x => listaParVinculoPeso.Any(y => y.ParLevel2_Id == x.Id))
                    .ToList();

                listaParLevel3 = db.ParLevel3
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Select(x => new ParLevel3AppViewModel()
                    {
                        Id = x.Id,
                        HasTakePhoto = x.HasTakePhoto,
                        Name = x.Name
                    })
                    .ToList()
                    .Where(x => listaParVinculoPeso.Any(y => y.ParLevel3_Id == x.Id))
                    .ToList();

                listaParEvaluation = db.ParEvaluation
                    .AsNoTracking()
                    .Where(x => x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                    .Where(x => x.ParFrequency_Id == appParametrization.ParFrequency_Id || x.ParFrequency_Id == null)
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.ParCompany_Id)
                    .ToList();

                listaParEvaluationXDepartmentXCargoAppViewModel = db.ParEvaluationXDepartmentXCargo
                .AsNoTracking()
                .Where(x => x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                .Where(x => x.ParFrequencyId == appParametrization.ParFrequency_Id)
                .Where(x => x.ParCluster_Id == appParametrization.ParCluster_Id)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.ParCompany_Id)
                .Select(x => new ParEvaluationXDepartmentXCargoAppViewModel()
                {
                    Id = x.Id,
                    ParCompany_Id = x.ParCompany_Id,
                    ParDepartment_Id = x.ParDepartment_Id,
                    ParCargo_Id = x.ParCargo_Id,
                    Sample = x.Sample,
                    Evaluation = x.Evaluation,
                    ParCluster_Id = x.ParCluster_Id,
                    RedistributeWeight = x.RedistributeWeight,
                    IsPartialCollection = x.IsPartialCollection
                })
                .ToList();

                var listaEvaluations = db.ParEvaluationSchedule
                        .Where(y => y.IsActive);

                foreach (var item in listaParEvaluationXDepartmentXCargoAppViewModel)
                {
                    item.ParEvaluationScheduleAppViewModel = listaEvaluations
                        .Where(y => y.ParEvaluationXDepartmentXCargo_Id == item.Id)
                        .Select(y => new ParEvaluationScheduleAppViewModel()
                        {
                            Inicio = y.Inicio,
                            Fim = y.Fim,
                            Av = y.Av,
                            Intervalo = y.Intervalo
                        }).ToList();
                }

                listaParLevel3Value = db.ParLevel3Value
                    .AsNoTracking()
                    .Where(x => x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                    .Where(x => x.IsActive == true)
                    .OrderByDescending(x => x.ParCompany_Id)
                    .Select(x => new ParLevel3ValueAppViewModel()
                    {
                        Id = x.Id,
                        DynamicValue = x.DynamicValue,
                        IntervalMax = x.IntervalMax,
                        IntervalMin = x.IntervalMin,
                        AcceptableValueBetween = x.AcceptableValueBetween,
                        ParLevel3BoolFalse_Id = x.ParLevel3BoolFalse_Id,
                        ParLevel3BoolTrue_Id = x.ParLevel3BoolTrue_Id,
                        ParLevel3InputType_Id = x.ParLevel3InputType_Id,
                        ParMeasurementUnit_Id = x.ParMeasurementUnit_Id,
                        ParCompany_Id = x.ParCompany_Id,
                        ParLevel1_Id = x.ParLevel1_Id,
                        ParLevel2_Id = x.ParLevel2_Id,
                        ParLevel3_Id = x.ParLevel3_Id,
                        ShowLevel3Limits = x.ShowLevel3Limits,
                        IsRequired = x.IsRequired,
                        IsNCTextRequired = x.IsNCTextRequired,
                        IsDefaultAnswer = x.IsDefaultAnswer,
                        IsAtiveNA = x.IsAtiveNA,
                        DefaultMessageText = x.DefaultMessageText,
                        StringSizeAllowed = x.StringSizeAllowed
                    })
                    .ToList()
                    .Where(x => listaParLevel1.Any(y => y.Id == x.ParLevel1_Id))
                    .Where(x => listaParLevel2.Any(y => y.Id == x.ParLevel2_Id))
                    .Where(x => listaParLevel3.Any(y => y.Id == x.ParLevel3_Id))
                    .ToList();

                listaParLevel3InputType = db.ParLevel3InputType
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Select(x => new ParLevel3InputTypeAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    })
                    .ToList();

                listaParMeasurementUnit = db.ParMeasurementUnit
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Select(x => new ParMeasurementUnitAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    })
                    .ToList();

                listaParLevel3BoolTrue = db.ParLevel3BoolTrue
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Select(x => new ParLevel3BoolTrueAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList();

                listaParLevel3BoolFalse = db.ParLevel3BoolFalse
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Select(x => new ParLevel3BoolFalseAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList();

                //if (departamentosFiltrados.Count > 0)
                //{
                //    var idsDosDepartamentos = departamentosFiltrados.Select(x => x.Id).ToList();

                //    listaParDepartment = db.ParDepartment
                //        .AsNoTracking()
                //        .Where(x => x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                //        .Where(x => idsDosDepartamentos.Contains(x.Id))
                //        .Where(x => x.Active)
                //        .Select(x => new ParDepartmentAppViewModel()
                //        {
                //            Id = x.Id,
                //            Name = x.Name,
                //            Description = x.Description,
                //            Parent_Id = x.Parent_Id,
                //            Hash = x.Hash
                //        })
                //        .ToList();
                //}
                //else
                //{

                var listaDepartamentoFiltradoComCargo_Id = listaParEvaluationXDepartmentXCargoAppViewModel
                    .Where(x => x.ParCargo_Id != null)
                    .Select(x => x.ParDepartment_Id)
                    .ToList();
                var listaDepartamentoFiltradoSemCargo_Id = listaParEvaluationXDepartmentXCargoAppViewModel
                    .Where(x => x.ParCargo_Id == null)
                    .Select(x => x.ParDepartment_Id)
                    .ToList();

                listaParDepartment = db.ParDepartment
                    .AsNoTracking()
                    .Where(x => x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                    .Where(x => x.Active)
                    .Where(x => listaDepartamentoFiltradoComCargo_Id.Any(y => y == x.Id)
                    || listaDepartamentoFiltradoSemCargo_Id.Any(y => y == x.Id))
                    .Select(x => new ParDepartmentAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Parent_Id = x.Parent_Id,
                        Hash = x.Hash
                    })
                    .ToList();

                var listaDepartamentoPaiFiltrado_Id = listaParDepartment.Select(x => x.Parent_Id).Distinct().ToList();
                var listaDepartamentoPaiFiltrado = db.ParDepartment.Where(x => listaDepartamentoPaiFiltrado_Id.Any(y => y == x.Id))
                            .Select(x => new ParDepartmentAppViewModel()
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Description = x.Description,
                                Parent_Id = x.Parent_Id,
                                Hash = x.Hash
                            }).ToList();
                listaParDepartment.AddRange(listaDepartamentoPaiFiltrado);

                var listaCargoFiltrado_Id = listaParEvaluationXDepartmentXCargoAppViewModel.Select(x => x.ParCargo_Id).ToList();

                var listaCargoFiltradoPorDepartamento_Id = db.ParCargoXDepartment
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Where(x => listaDepartamentoFiltradoSemCargo_Id.Any(y => y == x.ParDepartment_Id))
                    .Select(x => x.ParCargo_Id)
                    .ToList();

                listaParCargo = db.ParCargo
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Where(x => listaCargoFiltrado_Id.Any(y => y == x.Id) || listaCargoFiltradoPorDepartamento_Id.Any(y => y == x.Id))
                    .Select(x => new ParCargoAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                    })
                    .ToList();

                listaParCargoXDepartment = db.ParCargoXDepartment
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Where(x =>
                        (listaCargoFiltrado_Id.Any(y => y == x.ParCargo_Id)
                        && listaDepartamentoFiltradoComCargo_Id.Any(y => y == x.ParDepartment_Id))
                        || listaDepartamentoFiltradoSemCargo_Id.Any(y => y == x.ParDepartment_Id))
                    .Select(x => new ParCargoXDepartmentAppViewModel()
                    {
                        Id = x.Id,
                        ParCargo_Id = x.ParCargo_Id,
                        ParDepartment_Id = x.ParDepartment_Id
                    })
                    .ToList();

                listaParLevel3XHelp = db.ParLevel3XHelp
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                listaParAlert = db.ParAlert
                    .AsNoTracking()
                    .Where(x => x.IsActive && x.IsCollectAlert)
                    .ToList();

                //listaParDepartmentXHeaderField = db.ParDepartmentXHeaderField
                //    .AsNoTracking()
                //    .Where(x => x.IsActive)
                //    .ToList();

                //listaParHeaderField = db.ParHeaderField
                //    .AsNoTracking()
                //    .Where(x => x.IsActive)
                //    .ToList();

                //listaParMultipleValues = db.ParMultipleValues
                //    .AsNoTracking()
                //    .Where(x => x.IsActive)
                //    .ToList();


                listaParHeaderFieldGeral = db.ParHeaderFieldGeral
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                listaParMultipleValuesGeral = db.ParMultipleValuesGeral
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                listaParDepartmentXRotinaIntegracao = db.ParDepartmentXRotinaIntegracao
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                //Aqui precisa tirar a query para não ir pro APP
                listaRotinaIntegracao = db.RotinaIntegracao
                    .AsNoTracking()
                    .Where(x => x.IsActive && x.IsOffline == false)
                    .ToList();

                //Rotina Integração Offline
                listaRotinaIntegracaoOffline = GetRotinaIntegracaoComResultados();

                //lista de Cluster
                listaParCluster = db.ParCluster
                    .Where(x => x.IsActive)
                    .ToList()
                     .Where(x => listaParEvaluationXDepartmentXCargoAppViewModel.Any(y => y.ParCluster_Id == x.Id)).ToList();

                listaParClusterGroup = db.ParClusterGroup
                    .Where(x => x.IsActive && x.Id == appParametrization.ParClusterGroup_Id).ToList();

                var listaParLevel3_Ids = new List<int>();
                foreach (var item in listaParLevel3Value)
                {
                    listaParLevel3_Ids.Add(item.Id);
                }

                listaPargroupQualificationXParLevel3Value = db.PargroupQualificationXParLevel3Value
                     .Where(x => listaParLevel3_Ids.Any(y => y == x.ParLevel3Value_Id))
                     .Select(x => new PargroupQualificationXParLevel3ValueViewModel()
                     {
                         Id = x.Id,
                         PargroupQualification_Id = x.PargroupQualification_Id,
                         ParLevel3Value_Id = x.ParLevel3Value_Id,
                         Value = x.Value,
                         IsActive = x.IsActive,
                         IsRequired = x.IsRequired
                     })
                    .Where(x => x.IsActive)
                    .ToList();

                var listaPargroupQualificationXParLevel3Value_Ids = new List<int>();
                foreach (var item in listaPargroupQualificationXParLevel3Value)
                {
                    listaPargroupQualificationXParLevel3Value_Ids.Add(item.PargroupQualification_Id);
                }

                listaPargroupQualificationXParQualification = db.PargroupQualificationXParQualification
                    .Select(x => new PargroupQualificationXParQualificationVewModel()
                    {
                        Id = x.Id,
                        PargroupQualification_Id = x.PargroupQualification_Id,
                        ParQualification_Id = x.ParQualification_Id,
                        IsActive = x.IsActive
                    })
                    .Where(x => listaPargroupQualificationXParLevel3Value_Ids.Any(y => y == x.PargroupQualification_Id))
                    .Where(x => x.IsActive)
                    .ToList();

                var listaPargroupQualificationXParQualification_Ids = new List<int?>();
                foreach (var item in listaPargroupQualificationXParQualification)
                {
                    listaPargroupQualificationXParQualification_Ids.Add(item.ParQualification_Id);
                }

                listaParQualification = db.ParQualification
                    .Select(x => new ParQualificationViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsActive = x.IsActive
                    })
                    .Where(x => listaPargroupQualificationXParQualification_Ids.Any(y => y == x.Id))
                    .Where(x => x.IsActive)
                    .ToList();

                var listaPargroupQualification_Ids = new List<int?>();
                foreach (var item in listaPargroupQualificationXParQualification)
                {
                    listaPargroupQualification_Ids.Add(item.PargroupQualification_Id);
                }

                listaPargroupQualification = db.PargroupQualification
                   .Select(x => new PargroupQualificationViewModel()
                   {
                       Id = x.Id,
                       Name = x.Name,
                       IsActive = x.IsActive
                   })
                   .Where(x => listaPargroupQualification_Ids.Any(y => y == x.Id))
                   .Where(x => x.IsActive)
                   .ToList();
            }

            return Ok(new
            {
                listaParVinculoPeso,
                listaParLevel1,
                listaParLevel2,
                listaParLevel3,
                listaParEvaluationXDepartmentXCargoAppViewModel,
                listaParLevel3Value,
                listaParLevel3InputType,
                listaParMeasurementUnit,
                listaParLevel3BoolTrue,
                listaParLevel3BoolFalse,
                listaParDepartment,
                listaParCargo,
                listaParCargoXDepartment,
                listaParLevel3XHelp,
                listaParAlert,
                //listaParDepartmentXHeaderField,
                //listaParHeaderField,
                //listaParMultipleValues,
                listaParHeaderFieldGeral,
                listaParMultipleValuesGeral,
                listaParDepartmentXRotinaIntegracao,
                listaRotinaIntegracao,
                listaRotinaIntegracaoOffline,
                listaParCluster,
                listaParClusterGroup,
                listaParEvaluation,
                listaParQualification,
                listaPargroupQualification,
                listaPargroupQualificationXParQualification,
                listaPargroupQualificationXParLevel3Value
            });
        }

        [HttpPost]
        [Route("GetResults")]
        public IHttpActionResult GetResults(GetResultsData data)
        {
            InicioRequisicao();
            var coletaAgrupada = new List<ColetaAgrupadaViewModel>();

            var sql = $@"
-- INPUTS --
DECLARE @ParFrequency_Id INT = { data.ParFrequency_Id };
DECLARE @ParCompany_Id INT = {data.ParCompany_Id};
DECLARE @DataColeta DATETIME = '{data.CollectionDate.ToString("yyyy-MM-dd HH:mm:ss")}';
DECLARE @DateTimeInicio DATETIME;
DECLARE @DateTimeFinal DATETIME;

SET @DateTimeInicio = 
CASE @ParFrequency_Id
	WHEN 1 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 00:00:00') AS DATETIME)  -- Período
	WHEN 2 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 00:00:00') AS DATETIME)  -- Turno
	WHEN 3 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 00:00:00') AS DATETIME)  -- Diario
	WHEN 4 THEN CAST(DATEADD(DAY,-DATEPART(WEEKDAY,@DataColeta)+1,@DataColeta) AS DATE)  -- Semanal
	WHEN 5 THEN IIF(DATEPART(DAY,@DataColeta)<=15,CONCAT(CONVERT(VARCHAR(7),@DataColeta,120),'-01'), CONCAT(CONVERT(VARCHAR(7),@DataColeta,120),'-16'))  -- Quinzenal
	WHEN 6 THEN CONCAT(CONVERT(VARCHAR(7),@DataColeta,120),'-01')  -- Mensal
	WHEN 10 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 00:00:00') AS DATETIME) -- Diario com Intervalo 
END

SET @DateTimeFinal = 
CASE @ParFrequency_Id
	WHEN 1 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 23:59:59') AS DATETIME) -- Período
	WHEN 2 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 23:59:59') AS DATETIME) -- Turno
	WHEN 3 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 23:59:59') AS DATETIME) -- Diario
	WHEN 4 THEN CAST(CONCAT(CAST(DATEADD(DAY,7-DATEPART(WEEKDAY,@DataColeta),@DataColeta) AS DATE),' 23:59:59') AS DATETIME) -- Semanal
	WHEN 5 THEN IIF(DATEPART(DAY,@DataColeta)<=15,CONCAT(CONVERT(VARCHAR(7),@DataColeta,120),'-15 23:59:59'), CONCAT(EOMONTH(@DataColeta),' 23:59:59'))  -- Quinzenal
	WHEN 6 THEN CAST(CONCAT(CONVERT(VARCHAR(10), EOMONTH(@DataColeta), 120), ' 23:59:59') AS DATETIME)  -- Mensal
	WHEN 10 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 23:59:59') AS DATETIME) -- Diario com Intervalo 
END

--Id	Name
--1		Período
--2		Turno
--3		Diario
--4		Semanal
--5		Quinzenal
--6		Mensal
--10	Diario com Intervalo

SELECT 
	Evaluation,
	CAST(SUBSTRING(Sample,CHARINDEX('-',Sample)+1,LEN(Sample)+1-CHARINDEX('-',Sample))AS INT)+1 AS Sample,
	ParLevel1_Id,
	ParLevel2_Id,
	UnitId,
	Shift,
	ParDepartment_Id,
	ParCargo_Id,
	ParCluster_Id
FROM (

SELECT
        MAX(C2.EvaluationNumber) Evaluation
        ,MAX(CONCAT(RIGHT('0'+CAST(C2.EvaluationNumber AS VARCHAR),2),'-',RIGHT('0'+CAST(C2.Sample AS VARCHAR),2)))  Sample
        ,C2.ParLevel1_Id
        ,C2.ParLevel2_Id
        ,C2.UnitId
        ,C2.Shift
        ,C2XPD.ParDepartment_Id AS ParDepartment_Id
        ,C2XPC.ParCargo_Id AS ParCargo_Id
        ,C2XC.ParCluster_Id AS ParCluster_Id
    FROM CollectionLevel2 C2 WITH (NOLOCK)
    LEFT JOIN CollectionLevel2XCluster C2XC WITH (NOLOCK) ON C2XC.CollectionLevel2_Id = C2.Id
    INNER JOIN CollectionLevel2XParCargo C2XPC WITH (NOLOCK) ON C2XPC.CollectionLevel2_Id = C2.Id
    INNER JOIN CollectionLevel2XParDepartment C2XPD WITH (NOLOCK) ON C2XPD.CollectionLevel2_Id = C2.Id
	INNER JOIN ParEvaluationXDepartmentXCargo PEDC WITH(NOLOCK) ON C2.UnitId = PEDC.ParCompany_Id 
																AND C2XPC.ParCargo_Id      = ISNULL(PEDC.ParCargo_Id,C2XPC.ParCargo_Id)
																AND C2XPD.ParDepartment_Id = ISNULL(PEDC.ParDepartment_Id,C2XPD.ParDepartment_Id)
																AND C2.ParFrequency_Id     = PEDC.ParFrequencyId
WHERE 1 = 1
		AND C2.CollectionDate BETWEEN @DateTimeInicio AND @DateTimeFinal
		AND C2.UnitId = @ParCompany_Id
		AND PEDC.ParFrequencyId = @ParFrequency_Id
    GROUP BY C2.ParLevel1_Id
            ,C2.ParLevel2_Id
            ,C2.UnitId
            ,C2.Shift
		    ,C2XPD.ParDepartment_Id
		    ,C2XPC.ParCargo_Id
		    ,C2XC.ParCluster_Id
		) A";

            using (var factory = new Factory("DefaultConnection"))
            {
                coletaAgrupada = factory.SearchQuery<ColetaAgrupadaViewModel>(sql).ToList();
            }

            return Ok(coletaAgrupada.ToList());
        }
        #endregion

        #region Padrão GQ
        [Route("SetCollect123")]
        public IHttpActionResult SetCollect123(List<Collection> listSimpleCollect)
        {
            #region Gambi Log de Coletas
            var guiid = Guid.NewGuid();
            LogSystem.LogErrorBusiness.TryRegister(new Exception("Iniciado o registro das coletas (" + listSimpleCollect.Count + ")")
                , new { GUIID = guiid.ToString() });
            #endregion
            foreach (var item in listSimpleCollect)
            {

                try
                {
                    item.AddDate = DateTime.Now;
                    item.Shift_Id = 1;
                    item.Period_Id = 1;
                    item.IsProcessed = false;

                    List<Collection> collectionDuplicada = new List<Collection>();

                    #region inserir collection
                    string sql = $@"
INSERT INTO [dbo].[Collection]
           ([CollectionDate]
           ,[AddDate]
           ,[UserSgq_Id]
           ,[Shift_Id]
           ,[Period_Id]
           ,[ParCargo_Id]
           ,[ParCompany_Id]
           ,[ParDepartment_Id]
           ,[ParCluster_Id]
           ,[ParLevel1_Id]
           ,[ParLevel2_Id]
           ,[ParLevel3_Id]
           ,[CollectionType]
           ,[Weigth]
           ,[IntervalMin]
           ,[IntervalMax]
           ,[Value]
           ,[ValueText]
           ,[IsNotEvaluate]
           ,[IsConform]
           ,[Defects]
           ,[PunishimentValue]
           ,[WeiEvaluation]
           ,[Evaluation]
           ,[WeiDefects]
           ,[HasPhoto]
           ,[Sample]
           ,[HaveCorrectiveAction]
           ,[Parfrequency_Id]
           ,[AlertLevel]
           ,[ParHeaderField_Id]
           ,[ParHeaderField_Value]
           ,[Outros]
           ,[IsProcessed])
     VALUES
           (@CollectionDate
           ,@AddDate
           ,@UserSgq_Id
           ,@Shift_Id
           ,@Period_Id
           ,@ParCargo_Id
           ,@ParCompany_Id
           ,@ParDepartment_Id
           ,@ParCluster_Id
           ,@ParLevel1_Id
           ,@ParLevel2_Id
           ,@ParLevel3_Id
           ,@CollectionType
           ,@Weigth
           ,@IntervalMin
           ,@IntervalMax
           ,@Value
           ,@ValueText
           ,@IsNotEvaluate
           ,@IsConform
           ,@Defects
           ,@PunishimentValue
           ,@WeiEvaluation
           ,@Evaluation
           ,@WeiDefects
           ,@HasPhoto
           ,@Sample
           ,@HaveCorrectiveAction
           ,@Parfrequency_Id
           ,@AlertLevel
           ,@ParHeaderField_Id
           ,@ParHeaderField_Value
           ,@Outros
           ,@IsProcessed);
            SELECT @@IDENTITY AS 'Identity';";

                    using (Factory factory = new Factory("DefaultConnection"))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                        {
                            cmd.CommandType = CommandType.Text;
                            UtilSqlCommand.AddParameterNullable(cmd, "@CollectionDate", item.CollectionDate);
                            UtilSqlCommand.AddParameterNullable(cmd, "@AddDate", item.AddDate);
                            UtilSqlCommand.AddParameterNullable(cmd, "@UserSgq_Id", item.UserSgq_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@Shift_Id", item.Shift_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@Period_Id", item.Period_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@ParCargo_Id", item.ParCargo_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@ParCompany_Id", item.ParCompany_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@ParDepartment_Id", item.ParDepartment_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@ParCluster_Id", item.ParCluster_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@ParLevel1_Id", item.ParLevel1_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@ParLevel2_Id", item.ParLevel2_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@ParLevel3_Id", item.ParLevel3_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@CollectionType", item.CollectionType);
                            UtilSqlCommand.AddParameterNullable(cmd, "@Weigth", item.Weigth);
                            UtilSqlCommand.AddParameterNullable(cmd, "@IntervalMin", item.IntervalMin);
                            UtilSqlCommand.AddParameterNullable(cmd, "@IntervalMax", item.IntervalMax);
                            UtilSqlCommand.AddParameterNullable(cmd, "@Value", item.Value);
                            UtilSqlCommand.AddParameterNullable(cmd, "@ValueText", item.ValueText);
                            UtilSqlCommand.AddParameterNullable(cmd, "@IsNotEvaluate", item.IsNotEvaluate);
                            UtilSqlCommand.AddParameterNullable(cmd, "@IsConform", item.IsConform);
                            UtilSqlCommand.AddParameterNullable(cmd, "@Defects", item.Defects);
                            UtilSqlCommand.AddParameterNullable(cmd, "@PunishimentValue", item.PunishimentValue);
                            UtilSqlCommand.AddParameterNullable(cmd, "@WeiEvaluation", item.WeiEvaluation);
                            UtilSqlCommand.AddParameterNullable(cmd, "@Evaluation", item.Evaluation);
                            UtilSqlCommand.AddParameterNullable(cmd, "@WeiDefects", item.WeiDefects);
                            UtilSqlCommand.AddParameterNullable(cmd, "@HasPhoto", item.HasPhoto);
                            UtilSqlCommand.AddParameterNullable(cmd, "@Sample", item.Sample);
                            UtilSqlCommand.AddParameterNullable(cmd, "@HaveCorrectiveAction", item.HaveCorrectiveAction);
                            UtilSqlCommand.AddParameterNullable(cmd, "@Parfrequency_Id", item.Parfrequency_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@AlertLevel", item.AlertLevel);
                            UtilSqlCommand.AddParameterNullable(cmd, "@ParHeaderField_Id", item.ParHeaderField_Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@ParHeaderField_Value", item.ParHeaderField_Value);
                            UtilSqlCommand.AddParameterNullable(cmd, "@IsProcessed", item.IsProcessed);
                            UtilSqlCommand.AddParameterNullable(cmd, "@Outros", item.Outros);
                            var id = Convert.ToInt32(cmd.ExecuteScalar());

                            item.Id = id;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    item.HasError = true;
                    item.GUIID = guiid.ToString();

                    LogSystem.LogErrorBusiness.TryRegister(ex, new { GUIID = guiid.ToString() });
                }
            }


            var listaDeColetasComErro = listSimpleCollect.Where(x => x.HasError == true).ToList();
            var listaDeColetasSemErro = listSimpleCollect
                .Where(x => x.HasError != true)
                .ToList();

            #region Gambi Log de Coletas
            LogSystem.LogErrorBusiness.TryRegister(new Exception("Finalizou a inserção das coletas (" + listaDeColetasSemErro.Count + "/" + listSimpleCollect.Count + ")"),
                new { GUIID = guiid.ToString(), ListaCollection = string.Join(",", listaDeColetasSemErro.Select(x => x.Id)) });
            #endregion

            if (listaDeColetasComErro.Count == listSimpleCollect.Count)
                return BadRequest("Ocorreu erro em todas as tentativas de registrar as coletas.");

            var coletasRegistradas = listSimpleCollect.Where(x => x.HasError != true).ToList();

            int intervalTimeCollectionJob = 0;
            try
            {
                Int32.TryParse(DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.CollectionJobTime0IsDisabled, out intervalTimeCollectionJob);
            }
            catch (Exception)
            {
            }

            if (intervalTimeCollectionJob == 0)
            {
                var coletasRegistradasPorCollectionLevel2 = coletasRegistradas
                    .Where(x => x.ParHeaderField_Id == null
                    && x.ParHeaderField_Value == null
                    && x.Evaluation != null
                    && x.Sample != null)
                    .Select(x => new CollectionLevel2()
                    {
                        EvaluationNumber = (int)x.Evaluation,
                        Sample = x.Sample.Value,
                        ParLevel1_Id = x.ParLevel1_Id.Value,
                        ParLevel2_Id = x.ParLevel2_Id.Value,
                        Shift = x.Shift_Id.Value,
                        Period = x.Period_Id.Value,
                        UnitId = x.ParCompany_Id.Value,
                        ParCargo_Id = x.ParCargo_Id,
                        ParCluster_Id = x.ParCluster_Id,
                        ParDepartment_Id = x.ParDepartment_Id,
                        ParFrequency_Id = x.Parfrequency_Id,
                        AuditorId = x.UserSgq_Id ?? 0,
                        CollectionDate = x.CollectionDate.Value,
                        Outros = x.Outros
                    })
                    .Distinct()
                    .ToList();

                CollectionJob.ConsolidarCollectionLevel2(coletasRegistradasPorCollectionLevel2);
            }

            #region Gambi Log de Coletas
            try
            {
                LogSystem.LogErrorBusiness.Register(new Exception("Finalizado sem nenhum problema"), new { GUIID = guiid.ToString() });
            }
            catch { }
            #endregion

            return Ok(listaDeColetasSemErro);
        }

        [HttpPost]
        [Route("GetAppParametrization123")]
        public IHttpActionResult GetAppParametrization123(PlanejamentoColetaViewModel appParametrization)
        {
            InicioRequisicao();
            List<ParVinculoPesoAppViewModel> listaParVinculoPeso;
            List<ParLevel1AppViewModel> listaParLevel1;
            List<ParLevel2AppViewModel> listaParLevel2;
            List<ParLevel3AppViewModel> listaParLevel3;
            List<ParDepartmentAppViewModel> listaParDepartment;
            List<ParCargoAppViewModel> listaParCargo;
            List<ParCargoXDepartmentAppViewModel> listaParCargoXDepartment;
            List<ParEvaluationXDepartmentXCargoAppViewModel> listaParEvaluationXDepartmentXCargoAppViewModel;
            List<ParLevel3ValueAppViewModel> listaParLevel3Value;
            List<ParLevel3InputTypeAppViewModel> listaParLevel3InputType;
            List<ParMeasurementUnitAppViewModel> listaParMeasurementUnit;
            List<ParLevel3BoolTrueAppViewModel> listaParLevel3BoolTrue;
            List<ParLevel3BoolFalseAppViewModel> listaParLevel3BoolFalse;
            List<ParLevel3XHelp> listaParLevel3XHelp;
            List<ParAlert> listaParAlert;
            List<ParHeaderFieldGeral> listaParHeaderFieldGeral;
            List<ParMultipleValuesGeral> listaParMultipleValuesGeral;
            List<ParDepartmentXRotinaIntegracao> listaParDepartmentXRotinaIntegracao;
            List<RotinaIntegracao> listaRotinaIntegracao;
            List<RotinaIntegracaoViewModel> listaRotinaIntegracaoOffline;
            List<ParEvaluation> listaParEvaluation;
            List<ParCluster> listaParCluster;
            List<ParCompanyCluster> listaParCompanyXCluster;
            List<ParLevel1XCluster> listaParLevel1XCluster;
            List<Dominio.Seara.ParVinculoPesoParLevel2> listaParVinculoPesoParLevel2;
            List<Dominio.Seara.ParLevel1XParFamiliaProduto> listaParLevel1XParFamiliaProduto;

            List<Dominio.Seara.ParFamiliaProduto> listaParFamiliaProduto;
            List<Dominio.Seara.ParFamiliaProdutoXParProduto> listaParFamiliaProdutoXParProduto;
            List<Dominio.Seara.ParProduto> listaParProduto;
            List<ParQualificationViewModel> listaParQualification;
            List<PargroupQualificationXParQualificationVewModel> listaPargroupQualificationXParQualification;
            List<PargroupQualificationXParLevel3ValueViewModel> listaPargroupQualificationXParLevel3Value;
            List<PargroupQualificationViewModel> listaPargroupQualification;

            using (Dominio.SgqDbDevEntities db = new Dominio.SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                listaParVinculoPeso = db.ParVinculoPeso
                    .AsNoTracking()
                    .Where(x => x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.ParCompany_Id)
                    .Select(x => new ParVinculoPesoAppViewModel()
                    {
                        Id = x.Id,
                        ParLevel1_Id = x.ParLevel1_Id,
                        ParLevel2_Id = x.ParLevel2_Id,
                        ParLevel3_Id = x.ParLevel3_Id,
                        ParCompany_Id = x.ParCompany_Id,
                        ParDepartment_Id = x.ParDepartment_Id,
                        ParGroupParLevel1_Id = x.ParGroupParLevel1_Id,
                        ParCluster_Id = x.ParCluster_Id,
                        Peso = x.Peso,
                        ParCargo_Id = x.ParCargo_Id,
                        ParFrequency_Id = x.ParFrequencyId,
                        Evaluation = x.Evaluation,
                        Sample = x.Sample

                    }).ToList();

                listaParLevel1 = db.ParLevel1
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Select(x => new ParLevel1AppViewModel()
                    {
                        Id = x.Id,
                        HasTakePhoto = x.HasTakePhoto,
                        Name = x.Name
                    })
                    .ToList()
                    .Where(x => listaParVinculoPeso.Any(y => y.ParLevel1_Id == x.Id))
                    .ToList();

                listaParLevel1XParFamiliaProduto = db.ParLevel1XParFamiliaProduto
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                listaParLevel1XCluster = db.ParLevel1XCluster
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                listaParCompanyXCluster = db.ParCompanyCluster.AsNoTracking()
                    .Where(x => x.Active)
                    .Where(x => x.ParCompany_Id == appParametrization.ParCompany_Id)
                    .ToList();

                listaParCluster = db.ParCluster
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList()
                    .Where(x => listaParCompanyXCluster.Any(y => y.ParCluster_Id == x.Id))
                    .ToList();

                listaParLevel2 = db.ParLevel2
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Select(x => new ParLevel2AppViewModel()
                    {
                        Id = x.Id,
                        HasTakePhoto = x.HasTakePhoto,
                        Name = x.Name
                    })
                    .ToList()
                    .Where(x => listaParVinculoPeso.Any(y => y.ParLevel2_Id == x.Id))
                    .ToList();

                listaParVinculoPesoParLevel2 = db.ParVinculoPesoParLevel2
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList()
                    .Where(x =>
                        listaParLevel2.Any(y => y.Id == x.ParLevel2_Id)
                        && listaParLevel1.Any(y => y.Id == x.ParLevel1_Id))
                    .ToList();

                listaParLevel3 = db.ParLevel3
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Select(x => new ParLevel3AppViewModel()
                    {
                        Id = x.Id,
                        HasTakePhoto = x.HasTakePhoto,
                        Name = x.Name
                    })
                    .ToList()
                    .Where(x => listaParVinculoPeso.Any(y => y.ParLevel3_Id == x.Id))
                    .ToList();

                listaParEvaluation = db.ParEvaluation
                    .AsNoTracking()
                    .Where(x => x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.ParCompany_Id)
                    .ToList();

                listaParEvaluationXDepartmentXCargoAppViewModel = db.ParEvaluationXDepartmentXCargo
                    .AsNoTracking()
                    .Where(x => x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.ParCompany_Id)
                    .Select(x => new ParEvaluationXDepartmentXCargoAppViewModel()
                    {
                        Id = x.Id,
                        ParCompany_Id = x.ParCompany_Id,
                        ParDepartment_Id = x.ParDepartment_Id,
                        ParCargo_Id = x.ParCargo_Id,
                        Sample = x.Sample,
                        Evaluation = x.Evaluation
                    })
                    .ToList();

                var listaEvaluations = db.ParEvaluationSchedule
                        .Where(y => y.IsActive);

                foreach (var item in listaParEvaluationXDepartmentXCargoAppViewModel)
                {
                    item.ParEvaluationScheduleAppViewModel = listaEvaluations
                        .Where(y => y.ParEvaluationXDepartmentXCargo_Id == item.Id)
                        .Select(y => new ParEvaluationScheduleAppViewModel()
                        {
                            Inicio = y.Inicio,
                            Fim = y.Fim,
                            Av = y.Av,
                            Intervalo = y.Intervalo
                        }).ToList();
                }

                listaParLevel3Value = db.ParLevel3Value
                    .AsNoTracking()
                    .Where(x => x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                    .Where(x => x.IsActive == true)
                    .OrderByDescending(x => x.ParCompany_Id)
                    .Select(x => new ParLevel3ValueAppViewModel()
                    {
                        Id = x.Id,
                        DynamicValue = x.DynamicValue,
                        IntervalMax = x.IntervalMax,
                        IntervalMin = x.IntervalMin,
                        AcceptableValueBetween = x.AcceptableValueBetween,
                        ParLevel3BoolFalse_Id = x.ParLevel3BoolFalse_Id,
                        ParLevel3BoolTrue_Id = x.ParLevel3BoolTrue_Id,
                        ParLevel3InputType_Id = x.ParLevel3InputType_Id,
                        ParMeasurementUnit_Id = x.ParMeasurementUnit_Id,
                        ParCompany_Id = x.ParCompany_Id,
                        ParLevel1_Id = x.ParLevel1_Id,
                        ParLevel2_Id = x.ParLevel2_Id,
                        ParLevel3_Id = x.ParLevel3_Id,
                        ShowLevel3Limits = x.ShowLevel3Limits,
                        IsRequired = x.IsRequired,
                        IsNCTextRequired = x.IsNCTextRequired,
                        IsDefaultAnswer = x.IsDefaultAnswer,
                        LimiteNC = x.LimiteNC
                    })
                    .ToList()
                    .Where(x => listaParLevel1.Any(y => y.Id == x.ParLevel1_Id))
                    .Where(x => listaParLevel2.Any(y => y.Id == x.ParLevel2_Id))
                    .Where(x => listaParLevel3.Any(y => y.Id == x.ParLevel3_Id))
                    .ToList();

                listaParLevel3InputType = db.ParLevel3InputType
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Select(x => new ParLevel3InputTypeAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    })
                    .ToList();

                listaParMeasurementUnit = db.ParMeasurementUnit
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Select(x => new ParMeasurementUnitAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    })
                    .ToList();

                listaParLevel3BoolTrue = db.ParLevel3BoolTrue
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Select(x => new ParLevel3BoolTrueAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList();

                listaParLevel3BoolFalse = db.ParLevel3BoolFalse
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Select(x => new ParLevel3BoolFalseAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList();

                listaParDepartment = db.ParDepartment
                    .AsNoTracking()
                    .Where(x => x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                    .Where(x => x.Active)
                    .Select(x => new ParDepartmentAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Parent_Id = x.Parent_Id,
                        Hash = x.Hash
                    })
                    .ToList();

                listaParCargo = db.ParCargo
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Select(x => new ParCargoAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                    })
                    .ToList();

                listaParCargoXDepartment = db.ParCargoXDepartment
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .Select(x => new ParCargoXDepartmentAppViewModel()
                    {
                        Id = x.Id,
                        ParCargo_Id = x.ParCargo_Id,
                        ParDepartment_Id = x.ParDepartment_Id
                    })
                    .ToList();

                listaParLevel3XHelp = db.ParLevel3XHelp
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                listaParAlert = db.ParAlert
                    .AsNoTracking()
                    .Where(x => x.IsActive && x.IsCollectAlert)
                    .ToList();

                listaParHeaderFieldGeral = db.ParHeaderFieldGeral
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                listaParMultipleValuesGeral = db.ParMultipleValuesGeral
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                listaParDepartmentXRotinaIntegracao = db.ParDepartmentXRotinaIntegracao
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                //Aqui precisa tirar a query para não ir pro APP
                listaRotinaIntegracao = db.RotinaIntegracao
                    .AsNoTracking()
                    .Where(x => x.IsActive && x.IsOffline == false)
                    .ToList();

                //Rotina Integração Offline
                listaRotinaIntegracaoOffline = GetRotinaIntegracaoComResultados();

                listaParFamiliaProduto = db.ParFamiliaProduto
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                listaParFamiliaProdutoXParProduto = db.ParFamiliaProdutoXParProduto
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                listaParProduto = db.ParProduto
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                var listaParLevel3_Ids = new List<int>();
                foreach (var item in listaParLevel3Value)
                {
                    listaParLevel3_Ids.Add(item.Id);
                }

                listaPargroupQualificationXParLevel3Value = db.PargroupQualificationXParLevel3Value
                     .ToList()
                     .Where(x => listaParLevel3_Ids.Any(y => y == x.ParLevel3Value_Id))
                     .Select(x => new PargroupQualificationXParLevel3ValueViewModel()
                     {
                         Id = x.Id,
                         PargroupQualification_Id = x.PargroupQualification_Id,
                         ParLevel3Value_Id = x.ParLevel3Value_Id,
                         Value = x.Value,
                         IsActive = x.IsActive,
                         IsRequired = x.IsRequired
                     })
                    .Where(x => x.IsActive)
                    .ToList();

                var listaPargroupQualificationXParLevel3Value_Ids = new List<int>();
                foreach (var item in listaPargroupQualificationXParLevel3Value)
                {
                    listaPargroupQualificationXParLevel3Value_Ids.Add(item.PargroupQualification_Id);
                }

                listaPargroupQualificationXParQualification = db.PargroupQualificationXParQualification
                    .Select(x => new PargroupQualificationXParQualificationVewModel()
                    {
                        Id = x.Id,
                        PargroupQualification_Id = x.PargroupQualification_Id,
                        ParQualification_Id = x.ParQualification_Id,
                        IsActive = x.IsActive
                    })
                    .Where(x => listaPargroupQualificationXParLevel3Value_Ids.Any(y => y == x.PargroupQualification_Id))
                    .Where(x => x.IsActive)
                    .ToList();

                var listaPargroupQualificationXParQualification_Ids = new List<int?>();
                foreach (var item in listaPargroupQualificationXParQualification)
                {
                    listaPargroupQualificationXParQualification_Ids.Add(item.ParQualification_Id);
                }

                listaParQualification = db.ParQualification
                    .Select(x => new ParQualificationViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsActive = x.IsActive
                    })
                    .Where(x => listaPargroupQualificationXParQualification_Ids.Any(y => y == x.Id))
                    .Where(x => x.IsActive)
                    .ToList();

                var listaPargroupQualification_Ids = new List<int?>();
                foreach (var item in listaPargroupQualificationXParQualification)
                {
                    listaPargroupQualification_Ids.Add(item.PargroupQualification_Id);
                }

                listaPargroupQualification = db.PargroupQualification
                   .Select(x => new PargroupQualificationViewModel()
                   {
                       Id = x.Id,
                       Name = x.Name,
                       IsActive = x.IsActive
                   })
                   .Where(x => listaPargroupQualification_Ids.Any(y => y == x.Id))
                   .Where(x => x.IsActive)
                   .ToList();
            }

            return Ok(new
            {
                listaParVinculoPeso,
                listaParLevel1,
                listaParLevel2,
                listaParLevel3,
                listaParEvaluationXDepartmentXCargoAppViewModel,
                listaParLevel3Value,
                listaParLevel3InputType,
                listaParMeasurementUnit,
                listaParLevel3BoolTrue,
                listaParLevel3BoolFalse,
                listaParDepartment,
                listaParCargo,
                listaParCargoXDepartment,
                listaParLevel3XHelp,
                listaParAlert,
                listaParHeaderFieldGeral,
                listaParMultipleValuesGeral,
                listaParDepartmentXRotinaIntegracao,
                listaRotinaIntegracao,
                listaRotinaIntegracaoOffline,
                listaParEvaluation,
                listaParFamiliaProduto,
                listaParFamiliaProdutoXParProduto,
                listaParProduto,
                listaParCluster,
                listaParLevel1XCluster,
                listaParVinculoPesoParLevel2,
                listaParLevel1XParFamiliaProduto,
                listaParQualification,
                listaPargroupQualification,
                listaPargroupQualificationXParQualification,
                listaPargroupQualificationXParLevel3Value
            });
        }

        [HttpPost]
        [Route("GetResults123")]
        public IHttpActionResult GetResults123(GetResultsData data)
        {
            InicioRequisicao();
            var coletaAgrupada = new List<ColetaAgrupadaViewModel>();
            data.ParFrequency_Id = 3;//mock diario

            var sql = $@"
-- INPUTS --
DECLARE @ParFrequency_Id INT = { data.ParFrequency_Id };
DECLARE @ParCompany_Id INT = {data.ParCompany_Id};
DECLARE @DataColeta DATETIME = '{data.CollectionDate.ToString("yyyy-MM-dd HH:mm:ss")}';
DECLARE @DateTimeInicio DATETIME;
DECLARE @DateTimeFinal DATETIME;

SET @DateTimeInicio = 
CASE @ParFrequency_Id
	WHEN 1 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 00:00:00') AS DATETIME)  -- Período
	WHEN 2 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 00:00:00') AS DATETIME)  -- Turno
	WHEN 3 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 00:00:00') AS DATETIME)  -- Diario
	WHEN 4 THEN CAST(DATEADD(DAY,-DATEPART(WEEKDAY,@DataColeta)+1,@DataColeta) AS DATE)  -- Semanal
	WHEN 5 THEN IIF(DATEPART(DAY,@DataColeta)<=15,CONCAT(CONVERT(VARCHAR(7),@DataColeta,120),'-01'), CONCAT(CONVERT(VARCHAR(7),@DataColeta,120),'-16'))  -- Quinzenal
	WHEN 6 THEN CONCAT(CONVERT(VARCHAR(7),@DataColeta,120),'-01')  -- Mensal
	WHEN 10 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 00:00:00') AS DATETIME) -- Diario com Intervalo 
END

SET @DateTimeFinal = 
CASE @ParFrequency_Id
	WHEN 1 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 23:59:59') AS DATETIME) -- Período
	WHEN 2 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 23:59:59') AS DATETIME) -- Turno
	WHEN 3 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 23:59:59') AS DATETIME) -- Diario
	WHEN 4 THEN CAST(CONCAT(CAST(DATEADD(DAY,7-DATEPART(WEEKDAY,@DataColeta),@DataColeta) AS DATE),' 23:59:59') AS DATETIME) -- Semanal
	WHEN 5 THEN IIF(DATEPART(DAY,@DataColeta)<=15,CONCAT(CONVERT(VARCHAR(7),@DataColeta,120),'-15 23:59:59'), CONCAT(EOMONTH(@DataColeta),' 23:59:59'))  -- Quinzenal
	WHEN 6 THEN CAST(CONCAT(CONVERT(VARCHAR(10), EOMONTH(@DataColeta), 120), ' 23:59:59') AS DATETIME)  -- Mensal
	WHEN 10 THEN CAST(CONCAT(CONVERT(VARCHAR(10),@DataColeta,120),' 23:59:59') AS DATETIME) -- Diario com Intervalo 
END

--Id	Name
--1		Período
--2		Turno
--3		Diario
--4		Semanal
--5		Quinzenal
--6		Mensal
--10	Diario com Intervalo

SELECT 
	Evaluation,
	CAST(SUBSTRING(Sample,CHARINDEX('-',Sample)+1,LEN(Sample)+1-CHARINDEX('-',Sample))AS INT)+1 AS Sample,
	ParLevel1_Id,
	ParLevel2_Id,
	UnitId,
	Shift,
	ParFamiliaProduto_Id
FROM (

SELECT
        MAX(C2.EvaluationNumber) Evaluation
        ,MAX(CONCAT(RIGHT('0'+CAST(C2.EvaluationNumber AS VARCHAR),2),'-',RIGHT('0'+CAST(C2.Sample AS VARCHAR),2)))  Sample
        ,C2.ParLevel1_Id
        ,C2.ParLevel2_Id
        ,C2.UnitId
        ,C2.Shift
		,CL2XSFP.ParFamiliaProduto_Id
    FROM CollectionLevel2 C2 WITH (NOLOCK)
    LEFT JOIN CollectionLevel2XCluster C2XC WITH (NOLOCK) ON C2XC.CollectionLevel2_Id = C2.Id
	LEFT JOIN CollectionLevel2XParFamiliaProdutoXParProduto CL2XSFP ON CL2XSFP.CollectionLevel2_Id = c2.Id
WHERE 1 = 1
		AND C2.CollectionDate BETWEEN @DateTimeInicio AND @DateTimeFinal
		AND C2.UnitId = @ParCompany_Id
    GROUP BY C2.ParLevel1_Id
            ,C2.ParLevel2_Id
            ,C2.UnitId
            ,C2.Shift
            ,CL2XSFP.ParFamiliaProduto_Id
		) A";

            using (var factory = new Factory("DefaultConnection"))
            {
                coletaAgrupada = factory.SearchQuery<ColetaAgrupadaViewModel>(sql).ToList();
            }

            return Ok(coletaAgrupada.ToList());
        }

        [HttpPost]
        [Route("GetColetaParcial")]
        public IHttpActionResult GetColetaParcial(GetResultsData data)
        {
            //Enviar parametros para não buscar todas as coletas
            List<CollectionPartial> coletasParciais = new List<CollectionPartial>();
            InicioRequisicao();

            var sql = $@"
DECLARE @ParFrequency_Id INT = {data.ParFrequency_Id};
DECLARE @ParCompany_Id INT = {data.ParCompany_Id};
DECLARE @DataColeta DATETIME = '{data.CollectionDate:yyyy-MM-dd HH:mm:ss}';
DECLARE @DateTimeInicio DATETIME;
DECLARE @DateTimeFinal DATETIME;

SET @DateTimeInicio =
CASE @ParFrequency_Id
	WHEN 1 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 00:00:00') AS DATETIME)  -- Período
	WHEN 2 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 00:00:00') AS DATETIME)  -- Turno
	WHEN 3 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 00:00:00') AS DATETIME)  -- Diario
	WHEN 4 THEN CAST(DATEADD(DAY, -DATEPART(WEEKDAY, @DataColeta) + 1, @DataColeta) AS DATE)  -- Semanal
	WHEN 5 THEN IIF(DATEPART(DAY, @DataColeta) <= 15, CONCAT(CONVERT(VARCHAR(7), @DataColeta, 120), '-01'), CONCAT(CONVERT(VARCHAR(7), @DataColeta, 120), '-16'))  -- Quinzenal
	WHEN 6 THEN CONCAT(CONVERT(VARCHAR(7), @DataColeta, 120), '-01')  -- Mensal
	WHEN 10 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 00:00:00') AS DATETIME) -- Diario com Intervalo 
END

SET @DateTimeFinal =
CASE @ParFrequency_Id
	WHEN 1 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 23:59:59') AS DATETIME) -- Período
	WHEN 2 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 23:59:59') AS DATETIME) -- Turno
	WHEN 3 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 23:59:59') AS DATETIME) -- Diario
	WHEN 4 THEN CAST(CONCAT(CAST(DATEADD(DAY, 7 - DATEPART(WEEKDAY, @DataColeta), @DataColeta) AS DATE), ' 23:59:59') AS DATETIME) -- Semanal
	WHEN 5 THEN IIF(DATEPART(DAY, @DataColeta) <= 15, CONCAT(CONVERT(VARCHAR(7), @DataColeta, 120), '-15 23:59:59'), CONCAT(EOMONTH(@DataColeta), ' 23:59:59'))  -- Quinzenal
	WHEN 6 THEN CAST(CONCAT(CONVERT(VARCHAR(10), EOMONTH(@DataColeta), 120), ' 23:59:59') AS DATETIME)  -- Mensal
	WHEN 10 THEN CAST(CONCAT(CONVERT(VARCHAR(10), @DataColeta, 120), ' 23:59:59') AS DATETIME) -- Diario com Intervalo 
END

SELECT
	*
FROM CollectionPartial CP
WHERE 1 = 1
AND CP.CollectionDate BETWEEN @DateTimeInicio AND @DateTimeFinal
AND CP.ParCompany_Id = @ParCompany_Id
AND cp.Parfrequency_Id = @ParFrequency_Id";

            using (var factory = new Factory("DefaultConnection"))
            {
                coletasParciais = factory.SearchQuery<CollectionPartial>(sql).ToList();
            }

            return Ok(coletasParciais);
        }

        #endregion

        public class GetResultsData
        {
            public int ParCompany_Id { get; set; }
            public DateTime CollectionDate { get; set; }
            public int ParFrequency_Id { get; set; }
        }

        public class ColetaAgrupadaViewModel
        {
            public int Evaluation { get; set; }
            public int Sample { get; set; }
            public int ParLevel1_Id { get; set; }
            public int ParLevel2_Id { get; set; }
            public int UnitId { get; set; }
            public int AuditorId { get; set; }
            public int Shift { get; set; }
            public DateTime CollectionDate { get; set; }
            public int ParDepartment_Id { get; set; }
            public int ParCargo_Id { get; set; }
            public int ParCluster_Id { get; set; }
            public int? ParFamiliaProduto_Id { get; set; }
        }

        public class AppParametrization
        {
            public int ParCompany_Id { get; set; }
            public int ParFrequency_Id { get; set; }
            public DateTime AppDate { get; set; }
        }

        #region RotinaIntegracaoOffline

        public List<RotinaIntegracaoViewModel> GetRotinaIntegracaoComResultados()
        {

            var rotinasIntegracaoComResutados = new List<RotinaIntegracaoViewModel>();
            var rotinasIntegracao = GetRotinaIntegracao();

            foreach (var rotinaIntegracao in rotinasIntegracao)
            {

                var rotinaIntegracaoComResutados = GetResultado(rotinaIntegracao);

                if (rotinaIntegracaoComResutados != null)
                    rotinasIntegracaoComResutados.Add(rotinaIntegracaoComResutados);

            }

            return rotinasIntegracaoComResutados;
        }

        private List<RotinaIntegracao> GetRotinaIntegracao()
        {
            var rotinas = new List<RotinaIntegracao>();

            try
            {
                using (var db = new SgqDbDevEntities())
                {

                    rotinas = db.RotinaIntegracao
                        .AsNoTracking()
                        .Where(x => x.IsActive && x.IsOffline)
                        .ToList();
                }

            }
            catch (Exception ex)
            {

            }

            return rotinas;
        }

        private RotinaIntegracaoViewModel GetResultado(RotinaIntegracao rotinaIntegracao)
        {
            using (var db = new SgqDbDevEntities())
            {
                try
                {
                    var resultados = QueryNinja(db, rotinaIntegracao.Query);

                    if (resultados.Count > 0)
                    {
                        return new RotinaIntegracaoViewModel()
                        {
                            Id = rotinaIntegracao.Id,
                            IsOffline = rotinaIntegracao.IsOffline,
                            Name = rotinaIntegracao.Name,
                            Parametro = rotinaIntegracao.Parametro,
                            Retornos = rotinaIntegracao.Retornos,
                            Resultado = resultados
                        };
                    }
                }
                catch (Exception ex)
                {

                }

                return null;
            }
        }

        #endregion
    }
}