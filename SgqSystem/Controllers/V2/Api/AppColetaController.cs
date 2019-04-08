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

namespace SgqSystem.Controllers.V2.Api
{
    [RoutePrefix("api/AppColeta")]
    public class AppColetaController : BaseApiController
    {
        [Route("GetAppParametrization")]
        public IHttpActionResult GetAppParametrization()
        {

            List<ParVinculoPesoAppViewModel> listaParVinculoPeso;
            List<ParLevel1AppViewModel> listaParLevel1;
            List<ParLevel2AppViewModel> listaParLevel2;
            List<ParLevel3AppViewModel> listaParLevel3;
            List<ParEvaluationAppViewModel> listaParEvaluation;
            List<ParLevel3EvaluationSampleAppViewModel> listaParLevel3EvaluationSample;
            var companyId = 50;

            using (Dominio.SgqDbDevEntities db = new Dominio.SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                listaParVinculoPeso = db.ParVinculoPeso.AsNoTracking()
                    .Where(x => x.ParCompany_Id == companyId || x.ParCompany_Id == null)
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
                        Peso = x.Peso
                    })
                    .ToList();

                listaParLevel1 = db.ParLevel1
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
                    .Where(x => x.ParCompany_Id == companyId || x.ParCompany_Id == null)
                    .Where(x => x.IsActive)
                    .Select(x => new ParEvaluationAppViewModel()
                    {
                        Id = x.Id,
                        Number = x.Number,
                        ParCluster_Id = x.ParCluster_Id,
                        ParCompany_Id = x.ParCompany_Id,
                        ParLevel1_Id = x.ParLevel1_Id,
                        ParLevel2_Id = x.ParLevel2_Id,
                        Sample = x.Sample
                    })
                    .ToList()
                    .Where(x => listaParVinculoPeso.Any(y => y.ParLevel1_Id == x.ParLevel1_Id))
                    .Where(x => listaParVinculoPeso.Any(y => y.ParLevel2_Id == x.ParLevel2_Id))
                    .ToList();

                listaParLevel3EvaluationSample = db.ParLevel3EvaluationSample
                    .Where(x => x.ParCompany_Id == companyId || x.ParCompany_Id == null)
                    .Where(x => x.IsActive == true)
                    .Select(x => new ParLevel3EvaluationSampleAppViewModel()
                    {
                        Id = x.Id,
                        EvaluationNumber = x.EvaluationNumber,
                        SampleNumber = x.SampleNumber,
                        ParCompany_Id = x.ParCompany_Id,
                        ParLevel1_Id = x.ParLevel1_Id,
                        ParLevel2_Id = x.ParLevel2_Id,
                        ParLevel3_Id = x.ParLevel3_Id
                    })
                    .ToList()
                    .Where(x => listaParVinculoPeso.Any(y => y.ParLevel1_Id == x.ParLevel1_Id))
                    .Where(x => listaParVinculoPeso.Any(y => y.ParLevel2_Id == x.ParLevel2_Id))
                    .Where(x => listaParVinculoPeso.Any(y => y.ParLevel3_Id == x.ParLevel3_Id))
                    .ToList();
            }

            return Ok(new { listaParVinculoPeso,
                listaParLevel1,
                listaParLevel2,
                listaParLevel3,
                listaParEvaluation,
                listaParLevel3EvaluationSample });
        }
    }
}