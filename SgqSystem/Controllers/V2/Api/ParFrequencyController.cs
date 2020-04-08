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
using SgqSystem.ViewModels;

namespace SgqSystem.Controllers.V2.Api
{
    public class ParFrequencyController : BaseApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: api/ParFrequency
        [HttpPost]
        public IHttpActionResult GetParFrequency(PlanejamentoColetaViewModel appParametrization)
        {
            InicioRequisicao();
            db.Configuration.LazyLoadingEnabled = false;

            var listaParFrequencyVinculada_Id = db.ParEvaluationXDepartmentXCargo
                   .AsNoTracking()
                   .Where(x => x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                   .Where(x => x.ParCluster_Id == appParametrization.ParCluster_Id || x.ParCluster_Id == null)
                   .Where(x => x.IsActive)
                   .Select(x => x.ParFrequencyId)
                   .Distinct()
                   .ToList();

            var listaParFrequency = db.ParFrequency
                .Where(x => x.IsActive && listaParFrequencyVinculada_Id.Any(y=>y == x.Id))
                .ToList();

            return Ok(listaParFrequency);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}