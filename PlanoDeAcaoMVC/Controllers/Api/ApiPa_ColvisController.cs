using Newtonsoft.Json.Linq;
using PlanoAcaoEF;
using System.Collections.Generic;
using System.Web.Http;
using PlanoAcaoCore;
using System.Data.SqlClient;
using System;
using System.Net.Http;
using System.Net;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/ApiColvis")]
    public class ApiPa_ColvisController : BaseApiController
    {
        private PlanoDeAcaoEntities db;

        public ApiPa_ColvisController()
        {
            db = new PlanoDeAcaoEntities();
        }


        [HttpPost]
        [Route("Get")]
        public List<JObject> Get(Pa_Colvis colvis)
        {
            var query = "Select * from Pa_Colvis where Pa_Quem_Id = " + colvis.Pa_Quem_Id;

            //var lista = Pa_Colvis.ListarGenerico<Pa_Colvis>(query);

            return QueryNinja(db, query);
        }

        [HttpPost]
        [Route("Save")]
        public HttpResponseMessage Save(Pa_Colvis colvis)
        {
            var UserColvis = Get(colvis);

            var query = "";          

            if (UserColvis.Count > 0)
            {
                dynamic colvisObj = UserColvis;
                int Id = colvisObj[0].Id;

                //Update   
                query = $@"UPDATE [dbo].[Pa_Colvis]
                    SET [ColVisShow] = '{ colvis.ColVisShow }', [ColVisHide] = '{ colvis.ColVisHide }', [AlterDate] = '{ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") }'
                    WHERE Id = { Id }";
            }
            else
            {
                //Insert
                query = $@"INSERT INTO [dbo].[Pa_Colvis]
                    ([ColVisShow], [ColVisHide], [Pa_Quem_Id])
                    VALUES('{ colvis.ColVisShow }','{ colvis.ColVisHide }',{ colvis.Pa_Quem_Id })";
            }


            try
            {
                db.Database.ExecuteSqlCommand(query);

                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                var message = ex.InnerException.ToString();
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
                throw;
            }

            

        }

    }
}
