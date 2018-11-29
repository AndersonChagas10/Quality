using Dominio;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{

    [HandleApi()]
    [RoutePrefix("api/EscalaLikertAPI")]
    public class ParInputTypeValuesApiController : ApiController
    {
        [HttpGet]
        [Route("Get/{id}")]
        public List<ParInputTypeValues> Get(int id)
        {
            var db = new SgqDbDevEntities();
            db.Configuration.LazyLoadingEnabled = false;

            var escalasLikert = db.ParInputTypeValues.Where(x => x.ParLevel3Value_Id == id).ToList();

            return escalasLikert;
        }

        [HttpPost]
        [Route("Post")]
        public List<ParInputTypeValues> Post([FromBody] List<ParInputTypeValues> listaEscalaLikert)
        {
            foreach (var item in listaEscalaLikert)
            {

                item.IsActive = true;
                using (var db = new SgqDbDevEntities())
                {
                    if (item.Id > 0)
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }
                    else
                    {
                        db.ParInputTypeValues.Add(item);
                    }
                    db.SaveChanges();
                }

            }
            return listaEscalaLikert;
        }
    }
}
