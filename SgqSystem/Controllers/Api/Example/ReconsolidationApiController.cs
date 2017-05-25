using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using SgqSystem.Handlres;
using SgqSystem.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Example
{
    [HandleApi()]
    [RoutePrefix("api/Example")]
    public class ReconsolidationApiController : ApiController
    {
        
        [HttpPost]
        [HandleApi()]
        [Route("Reconsolidation/{dataHora}")]
        public string Reconsolidation(DateTime dataHora)
        {
            var SgqSystem = new SgqSystem.Services.SyncServices();

            var db = new SgqDbDevEntities();
            using (db)
            {
                var listP = (from p in db.ParLevel1
                                              where p.IsActive == true
                                              select p);

                var listC = (from p in db.ParCompany
                            where p.IsActive == true
                            select p);

                foreach(ParCompany c in listC)
                {
                    foreach (ParLevel1 p in listP)
                    {
                        SgqSystem._ReConsolidationByLevel1(c.Id, p.Id, dataHora);
                    }
                }

                

            }

                
            return "ok";
        }
        

    }
}
