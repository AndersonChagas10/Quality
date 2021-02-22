using Dominio;
using Dominio.AppViewModel;
using SgqSystem.Controllers.Api;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.V2.Api
{
    [RoutePrefix("api")]
    public class ParFrequencyXParLevel1APIController : BaseApiController
    {

        [HttpPost]
        [Route("GetParFrequencyXParLevel1")]
        public IHttpActionResult GetParFrequencyXParLevel1(PlanejamentoColetaViewModel appParametrization)
        {

            List<ParFrequencyAppViewModel> frequencies = new List<ParFrequencyAppViewModel>();

            List<ParLevel1AppViewModel> levels1 = new List<ParLevel1AppViewModel>();

            List<ParVinculoPesoAppViewModel> vinculosPeso = new List<ParVinculoPesoAppViewModel>();

            using (Dominio.SgqDbDevEntities db = new Dominio.SgqDbDevEntities())
            {
                vinculosPeso = db.ParVinculoPeso
                        .Where(x => x.ParCluster_Id == appParametrization.ParCluster_Id 
                        && (x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                        && x.IsActive)
                        .Select(x => new ParVinculoPesoAppViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ParCompany_Id = x.ParCompany_Id,
                            ParCluster_Id = x.ParCluster_Id,
                            ParFrequency_Id = x.ParFrequencyId,
                            ParLevel1_Id = x.ParLevel1_Id
                        })
                        .ToList();

                levels1 = db.ParLevel1
                    .ToList()          
                    .Where(x => vinculosPeso.Any(y => y.ParLevel1_Id == x.Id))
                    .Select(x => new ParLevel1AppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList();

                frequencies = db.ParFrequency
                    .ToList()
                    .Where(x => vinculosPeso.Any(y => y.ParFrequency_Id == x.Id))
                    .Select(x => new ParFrequencyAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                    })
                    .ToList();
            }

            return Ok(new
            {
                frequencies,
                levels1,
                vinculosPeso
            });
        }
    }
}
