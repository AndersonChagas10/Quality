using ADOFactory;
using DTO.DTO;
using DTO.DTO.Params;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC
{
    public class IntegraFTA : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            string catalog = "SgqDbDev";
            string dataSource = @"SERVERGRT\MSSQLSERVER2014";
            string user = "sa";
            string pass = "1qazmko0";
            var parcompany = new List<ParCompanyDTO>();
            var usersgq = new List<UserDTO>();
            
            using (var db = new Factory(dataSource, catalog, pass, user))
            {

                var level1 = db.SearchQuery<ParLevel1DTO>("Select * from parlevel1").Where(r=>r.IsActive).ToList();
                var level2 = db.SearchQuery<ParLevel1DTO>("Select * from parlevel2").Where(r=>r.IsActive).ToList();
                var level3 = db.SearchQuery<ParLevel1DTO>("Select * from parlevel3").Where(r => r.IsActive).ToList();
                usersgq = db.SearchQuery<UserDTO>("Select * from usersgq");
                parcompany = db.SearchQuery<ParCompanyDTO>("Select * from parcompany").Where(r => r.IsActive).ToList();

             

            }

        }
    }
}
