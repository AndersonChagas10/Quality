using ADOFactory;
using DTO.DTO;
using DTO.DTO.Params;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC
{
    public class IntegraSgq : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            string catalog = "SgqDbDev";
            string dataSource = @"SERVERGRT\MSSQLSERVER2014";
            string user = "sa";
            string pass = "1qazmko0";


            using (var db = new Factory(dataSource, catalog, pass, user))
            {
                var level1 = db.SearchQuery<ParLevel1DTO>("Select * from parlevel1").Where(r=>r.IsActive).ToList();
                var level2 = db.SearchQuery<ParLevel1DTO>("Select * from parlevel2").Where(r=>r.IsActive).ToList();
                var level3 = db.SearchQuery<ParLevel1DTO>("Select * from parlevel3").Where(r => r.IsActive).ToList();
                var usersgq = db.SearchQuery<UserDTO>("Select * from usersgq");
                var parcompany = db.SearchQuery<ParCompanyDTO>("Select * from parcompany").Where(r => r.IsActive).ToList();

             

                filterContext.Controller.ViewBag.Level1 = level1;
                filterContext.Controller.ViewBag.Level2 = level2;
                filterContext.Controller.ViewBag.Level3 = level3;
                filterContext.Controller.ViewBag.UserSgq = usersgq;
                filterContext.Controller.ViewBag.ParCompany = parcompany;
            }
        }
    }
}
