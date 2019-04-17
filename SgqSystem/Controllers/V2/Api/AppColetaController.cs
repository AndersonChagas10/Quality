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

namespace SgqSystem.Controllers.V2.Api
{
    [RoutePrefix("api/AppColeta")]
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
        public IHttpActionResult SetCollect(List<SimpleCollect> listSimpleCollect)
        {
            foreach (var amostra in listSimpleCollect)
            {

                var coleta = new Coleta()
                {
                    ParLevel1_Id = amostra.ParLevel1_Id?.ToString(),
                    ParLevel2_Id = amostra.ParLevel2_Id?.ToString(),
                    ParCluster_Id = "3",
                    UnidadeId = amostra.ParCompany_Id?.ToString(),
                    ColetaTarefa = new List<ColetaTarefa>()
                {
                    new ColetaTarefa()
                    {
                        Level03Id = amostra.ParLevel3_Id?.ToString(),
                        ValueConform = string.IsNullOrEmpty(amostra.Value) ? "0" : Convert.ToInt32(amostra.Value).ToString(),
                        ValueText = amostra.ValueText?.ToString(),
                        IntervalMin = string.IsNullOrEmpty(amostra.IntervalMin) ? "0" : Convert.ToInt32(amostra.IntervalMin).ToString(),
                        IntervalMax = string.IsNullOrEmpty(amostra.IntervalMax) ? "0" : Convert.ToInt32(amostra.IntervalMax).ToString(),
                        Conform = amostra.IsConform?.ToString(),
                        IsnotEvaluate = amostra.NotEvaluated?.ToString(),
                        HasPhoto = "0",
                        CollectionDate = amostra.CollectionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        Defects = "0",
                        WeiDefects = "0",
                        WeiEvaluation = "0",
                        Weight = "1",
                    }
                },
                    Level01DataCollect = amostra.CollectionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Level02DataCollect = amostra.CollectionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Evaluate = Convert.ToInt32(amostra.Evaluation).ToString(),
                    Sample = Convert.ToInt32(amostra.Sample).ToString(),
                    Weidefects = "0",
                    Weievaluation = "0",
                    Defects = "0",
                    Defectsresult = "0",
                    Cluster = "3",
                    Shift = "1",
                    VersaoApp = "AppColeta2",
                    HashKey = "",
                };

                try
                {
                    var x = new SgqSystem.Services.SyncServices().InsertJson(coleta.ToString(), "", "", false);
                    if (x == null)
                    {
                        //Passa isso pra coleta
                        amostra.IsCollected = true;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return Ok(listSimpleCollect);
        }

        [Route("GetAppParametrization/{parCompany_Id}/{parFrequency_Id}")]
        public IHttpActionResult GetAppParametrization(int parCompany_Id, int parFrequency_Id)
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


            using (Dominio.SgqDbDevEntities db = new Dominio.SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                listaParVinculoPeso = db.ParVinculoPeso
                    .AsNoTracking()
                    .Where(x => x.ParCompany_Id == parCompany_Id || x.ParCompany_Id == null)
                    .Where(x => x.IsActive)
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
                        ParCargo_Id = x.ParCargo_Id
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
                    .Where(x => x.ParFrequency_Id == parFrequency_Id)
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
                    .Where(x => x.ParCompany_Id == parCompany_Id || x.ParCompany_Id == null)
                    .Where(x => x.IsActive)
                    .Select(x => new ParEvaluationXDepartmentXCargoAppViewModel()
                    {
                        Id = x.Id,
                        ParCompany_Id = x.ParCompany_Id,
                        ParDepartment_Id = x.ParDepartment_Id,
                        ParCargo_Id = x.ParCargo_Id,
                        Sample = x.Sample
                    })
                    .ToList();

                listaParLevel3Value = db.ParLevel3Value
                    .AsNoTracking()
                    .Where(x => x.ParCompany_Id == parCompany_Id || x.ParCompany_Id == null)
                    .Where(x => x.IsActive == true)
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
                    .Select(x => new ParLevel3InputTypeAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    })
                    .ToList();

                listaParMeasurementUnit = db.ParMeasurementUnit
                    .AsNoTracking()
                    .Select(x => new ParMeasurementUnitAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    })
                    .ToList();

                listaParLevel3BoolTrue = db.ParLevel3BoolTrue
                    .AsNoTracking()
                    .Select(x => new ParLevel3BoolTrueAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList();

                listaParLevel3BoolFalse = db.ParLevel3BoolFalse
                    .AsNoTracking()
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
            });
        }
    }
}