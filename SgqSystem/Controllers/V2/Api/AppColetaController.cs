﻿using System;
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

                foreach (var item in listSimpleCollect)
                {

                    try
                    {
                        //Validar para inserir?
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

                //Fazer uma "Task" para essas funções abaixo?

                //TODO: Fazer uma função que leia do banco o Collection que não foram processados, e separe os CollectionLevel2 e Result_Level3 da tabela salva
                var collectionsLevel2 = GetCollectionsLevel2NotProcess();

                try
                {
                    foreach (var collectionLevel2 in collectionsLevel2)
                    {
                        var collectionsProcess_Id = new List<int>();
                        var resultsLevel3 = GetResultLevel3NotProcess(collectionLevel2);

                        //TODO: chamar a função de SetConsolidation() para inserir os dados consolidados na CollectionLevel2
                        var collectionLevel2Consolidada = SetConsolidation(collectionLevel2, resultsLevel3);

                        //TODO: salvar a collectionLevel2 e apos a Result_Level3 com o respectivo CollectionLevel2_Id


                        var collection = db.CollectionLevel2.Where(x => x.Key == collectionLevel2Consolidada.Key).FirstOrDefault();
                        if (collection == null)
                        {
                            db.CollectionLevel2.Add(collectionLevel2Consolidada);
                            db.SaveChanges();
                        }
                        else
                        {
                            collectionLevel2Consolidada = collection;
                        }

                        foreach (var resultLevel3 in resultsLevel3)
                        {
                            try
                            {
                                resultLevel3.CollectionLevel2_Id = collectionLevel2Consolidada.Id;
                                resultLevel3.HasPhoto = resultLevel3.HasPhoto == null ? false : resultLevel3.HasPhoto;
                                resultLevel3.ParLevel3_Name = parLevel3List.Where(x => x.Id == resultLevel3.ParLevel3_Id).Select(x => x.Name).FirstOrDefault();
                                collectionsProcess_Id.Add(resultLevel3.Id);
                                db.Result_Level3.Add(resultLevel3);

                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                collectionsProcess_Id.RemoveAt(collectionsProcess_Id.Count - 1);
                            }
                        }

                        try
                        {
                            if (collectionsProcess_Id.Count > 0)
                            {
                                db.Database.ExecuteSqlCommand("UPDATE Collection set IsProcessed = 1 where Id in (" + string.Join(",", collectionsProcess_Id) + ")");
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToClient());
                }
            }


            var lista = listSimpleCollect.Where(x => x.HasError == true).ToList();
            if (lista.Count == listSimpleCollect.Count)
                return BadRequest("Ocorreu erro em todas as tentativas de registrar as coletas.");

            return Ok(listSimpleCollect.Where(x => x.HasError != true).ToList());
        }

        [HttpPost]
        [Route("GetAppParametrization")]
        public IHttpActionResult GetAppParametrization(AppParametrization appParametrization)
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
                    })
                    .ToList();

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
                listaParMultipleValues
            });
        }

        private CollectionLevel2 SetConsolidation(CollectionLevel2 collectionLevel2, List<Result_Level3> results_Level3)
        {

            decimal defects = 0;
            decimal weiDefects = 0;
            int evaluation = 0;
            decimal weiEavaluation = 0;

            foreach (var result_Level3 in results_Level3)
            {
                defects += result_Level3.Defects == null ? 0 : result_Level3.Defects.Value;
                weiDefects += result_Level3.WeiDefects == null ? 0 : result_Level3.WeiDefects.Value;
                evaluation++;
                weiEavaluation += result_Level3.Weight == null ? 0 : result_Level3.Weight.Value;
            }

            collectionLevel2.Defects = defects;
            collectionLevel2.WeiDefects = weiDefects;
            collectionLevel2.TotalLevel3Evaluation = evaluation;
            collectionLevel2.WeiEvaluation = weiEavaluation;
            collectionLevel2.AuditorId = collectionLevel2.AuditorId == 0 ? 1 : collectionLevel2.AuditorId;
            collectionLevel2.AlterDate = null;
            collectionLevel2.AddDate = DateTime.Now;
            collectionLevel2.Key = collectionLevel2.CollectionDate.ToString("yyyy-MM-dd") + "-" + collectionLevel2.UnitId + "-" +
                collectionLevel2.ParLevel1_Id + "-" + collectionLevel2.ParLevel2_Id + "-" + collectionLevel2.Shift + "-" +
                collectionLevel2.ParCluster_Id + "-" + collectionLevel2.ParCargo_Id + "-" + collectionLevel2.ParDepartment_Id + "-" +
                collectionLevel2.EvaluationNumber + "-" + collectionLevel2.Sample;

            return collectionLevel2;
        }

        private List<CollectionLevel2> GetCollectionsLevel2NotProcess()
        {
            var sql = $@"
                        SELECT DISTINCT top 100 
                    	Evaluation as EvaluationNumber
                       ,Sample
                       ,ParLevel1_Id
                       ,ParLevel2_Id
                       ,Shift_Id as Shift
                       ,Period_Id as Period
                       ,ParCompany_Id as UnitId
                       ,ParCargo_Id
                       ,ParCluster_Id
                       ,ParDepartment_Id
                       ,IIF(UserSgq_Id is null, 0,UserSgq_Id) as AuditorId
                       ,CONVERT(VARCHAR(19),IIF(DATEPART(MILLISECOND,CollectionDate)>500,DATEADD(SECOND,1,CollectionDate),CollectionDate),120) AS CollectionDate
                       ,GETDATE() as StartPhaseDate
                       ,UserSgq_Id as AuditorId
                        FROM Collection
                        WHERE IsProcessed = 0
                        AND ParHeaderField_Id IS NULL";

            var collectionLevel2 = new List<CollectionLevel2>();

            using (Factory factory = new Factory("DefaultConnection"))
            {

                try
                {
                    collectionLevel2 = factory.SearchQuery<CollectionLevel2>(sql).ToList();
                }
                catch (Exception ex)
                {
                }

                return collectionLevel2;
            }
        }

        private List<Result_Level3> GetResultLevel3NotProcess(CollectionLevel2 collectionLevel2)
        {
            var resultsLevel3 = new List<Result_Level3>();

            var sql = $@"
                    SELECT ParLevel3_Id
                        ,Id
                        ,Weigth
                        ,IntervalMin
                        ,IntervalMax
                        ,Value
                        ,ValueText
                        ,IsConform
                        ,IsNotEvaluate
                        ,Defects
                        ,PunishimentValue
                        ,WeiEvaluation
                        ,Evaluation
                        ,WeiDefects
                        ,HasPhoto  FROM Collection 
                    WHERE Evaluation = {collectionLevel2.EvaluationNumber} AND IsProcessed = 0 AND
                        Sample = {collectionLevel2.Sample} AND ParLevel1_Id = {collectionLevel2.ParLevel1_Id} AND
                        ParLevel2_Id = {collectionLevel2.ParLevel2_Id} AND Shift_Id = {collectionLevel2.Shift} AND
                        Period_Id = {collectionLevel2.Period} AND ParCompany_Id = {collectionLevel2.UnitId} AND
                        CAST(CONVERT(VARCHAR(19), IIF(DATEPART(MILLISECOND, CollectionDate) > 500, DATEADD(SECOND, 1, CollectionDate), CollectionDate), 120) AS DATE) = '{collectionLevel2.CollectionDate.ToString("yyyy-MM-dd")}'";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                try
                {
                    resultsLevel3 = factory.SearchQuery<Result_Level3>(sql).ToList();
                }
                catch (Exception ex)
                {
                }

                return resultsLevel3;
            }
        }

        [HttpPost]
        [Route("GetResults")]
        public IHttpActionResult GetResults(GetResultsData data)
        {
            var coletaAgrupada = new List<ColetaAgrupadaViewModel>();

            var sql = $@"
                    SELECT
                    	MAX(EvaluationNumber) Evaluation
                       ,(MAX(Sample)+1) Sample
                       ,ParLevel1_Id
                       ,ParLevel2_Id
                       ,UnitId
                       ,Shift
                       --,CollectionDate
                       ,ParDepartment_Id
                       ,ParCargo_Id
                       ,ParCluster_Id
                    FROM CollectionLevel2
                    WHERE 1 = 1
                    AND CAST(CollectionDate AS DATE) = CAST('{data.CollectionDate.ToString("yyyy-MM-dd")}' AS DATE)
                    AND UnitId = {data.ParCompany_Id}
                    GROUP BY ParLevel1_Id
                    		,ParLevel2_Id
                    		,UnitId
                    		,Shift
                    		--,CollectionDate
                    		,ParDepartment_Id
                    		,ParCargo_Id
                    		,ParCluster_Id
                    ";

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
    }
}