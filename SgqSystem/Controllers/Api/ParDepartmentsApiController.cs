using Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/ParDepartments")]
    public class ParDepartmentsApiController : BaseApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GetParDepartmentParents")]
        public List<KeyValuePair<int, string>> GetParDepartmentParents(string search)
        {
            List<KeyValuePair<int, string>> allsearch = new List<KeyValuePair<int, string>>();

            //if (profileID == 1)
            //{
            //    allsearch = CBO.Retrieve()
            //        .Where(x => x.name.Contains(search)).ToList()
            //        .Select(x => new KeyValuePair<int, string>(x.ID, x.name + '(' + x.ID + ')'))
            //        .ToList();
            //    return allsearch;
            //}
            //if (profileID == 2)
            //{
            //    CollaboratorServiceProviderBO CBOSP = new CollaboratorServiceProviderBO();

            //    string token = Request.Headers.GetValues("token").FirstOrDefault().ToString();
            //    string userId = new ApplicationUserBO().Retrieve().Where(x => x.token == token).Select(x => x.Id).FirstOrDefault();
            //    int idServiceProvider = new ServiceProviderBO().Retrieve().Where(x => x.ApplicationUserID == userId).Select(x => x.ID).FirstOrDefault();

            //    //Busca todos Ids e nomer=s dos colaboradores vinculados a transportadora logada
            //    allsearch = CBOSP.Retrieve().Where(p => p.ServiceProviderID == idServiceProvider && p.Collaborator.name.Contains(search)).ToList()
            //        .Select(x => new KeyValuePair<int, string>(x.Collaborator.ID, x.Collaborator.name + '(' + x.Collaborator.ID + ')'))
            //        .ToList();
            //}
            allsearch = db.ParDepartment.Where(x => x.Name.Contains(search)).ToList()
                .Select(x => new KeyValuePair<int, string>(x.Id, x.Id + "- " + x.Name))
                .ToList();

            return allsearch;
        }

    }
}
