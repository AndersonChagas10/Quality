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

        [Route("SetCollect")]
        public IHttpActionResult SetCollect(List<Collection> listSimpleCollect)
        {

            using (var db = new SgqDbDevEntities())
            {

                var parLevel3List = db.ParLevel3.ToList();

                //Adiciona os arquivos na Collection
                foreach (var item in listSimpleCollect)
                {

                    try
                    {
                        item.AddDate = DateTime.Now;
                        item.Shift_Id = 1;
                        item.Period_Id = 1;
                        item.IsProcessed = false;
                        db.Collection.Add(item);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        item.HasError = true;
                        //Registrar LOG
                    }
                }
            }


            var lista = listSimpleCollect.Where(x => x.HasError == true).ToList();
            if (lista.Count == listSimpleCollect.Count)
                return BadRequest("Ocorreu erro em todas as tentativas de registrar as coletas.");

            return Ok(listSimpleCollect.Where(x => x.HasError != true).ToList());
        }

        [HttpPost]
        [Route("GetAppParametrization")]
        public IHttpActionResult GetAppParametrization(PlanejamentoColetaViewModel appParametrization)
        {
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
            List<ParHeaderField> listaParHeaderField;
            List<ParDepartmentXHeaderField> listaParDepartmentXHeaderField;
            List<ParMultipleValues> listaParMultipleValues;
            List<ParDepartmentXRotinaIntegracao> listaParDepartmentXRotinaIntegracao;
            List<RotinaIntegracao> listaRotinaIntegracao;
            List<RotinaIntegracaoViewModel> listaRotinaIntegracaoOffline;

            using (Dominio.SgqDbDevEntities db = new Dominio.SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                listaParVinculoPeso = db.ParVinculoPeso
                    .AsNoTracking()
                    .Where(x => x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                    .Where(x => x.ParFrequencyId == appParametrization.ParFrequency_Id)
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

                listaParEvaluationXDepartmentXCargoAppViewModel = db.ParEvaluationXDepartmentXCargo
                    .AsNoTracking()
                    .Where(x => x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                    .Where(x => x.ParFrequencyId == appParametrization.ParFrequency_Id || x.ParFrequencyId == null)
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
                        ParLevel3_Id = x.ParLevel3_Id
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

                listaParDepartmentXHeaderField = db.ParDepartmentXHeaderField
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                listaParHeaderField = db.ParHeaderField
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .ToList();

                listaParMultipleValues = db.ParMultipleValues
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
                listaParDepartmentXHeaderField,
                listaParHeaderField,
                listaParMultipleValues,
                listaParDepartmentXRotinaIntegracao,
                listaRotinaIntegracao,
                listaRotinaIntegracaoOffline
            });
        }

        [HttpPost]
        [Route("GetResults")]
        public IHttpActionResult GetResults(GetResultsData data)
        {
            var coletaAgrupada = new List<ColetaAgrupadaViewModel>();

            var sql = $@"
                    SELECT
                    	MAX(C2.EvaluationNumber) Evaluation
                       ,(MAX(C2.Sample) + 1) Sample
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
                    WHERE 1 = 1
                    AND CAST(CollectionDate AS DATE) = CAST('{data.CollectionDate.ToString("yyyy-MM-dd")}' AS DATE)
                    AND UnitId = {data.ParCompany_Id}
                    GROUP BY C2.ParLevel1_Id
                    		,C2.ParLevel2_Id
                    		,C2.UnitId
                    		,C2.Shift
		                    ,C2XPD.ParDepartment_Id
		                    ,C2XPC.ParCargo_Id
		                    ,C2XC.ParCluster_Id";

            using (var factory = new Factory("DefaultConnection"))
            {
                coletaAgrupada = factory.SearchQuery<ColetaAgrupadaViewModel>(sql).ToList();
            }

            return Ok(coletaAgrupada.ToList());
        }
        
        public class GetResultsData
        {
            public int ParCompany_Id { get; set; }
            public DateTime CollectionDate { get; set; }
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