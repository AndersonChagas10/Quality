using ADOFactory;
using DTO.DTO;
using DTO.DTO.Params;
using PlanoAcaoCore;
using PlanoDeAcaoMVC.SgqIntegracao;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC
{
    /// <summary>
    /// Disponibiliza as DDL's
    /// Level1, Level2, Level3, Undiade e Quem do SGQ.
    /// </summary>
    public class IntegraSgq : AuthorizeAttribute
    {
        /// <summary>
        /// Disponibiliza as DDL's
        /// Level1, Level2, Level3, Undiade e Quem do SGQ.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            var parcompany = new List<ParCompanyDTO>();
            var usersgq = new List<UserDTO>();

            using (var db = new ConexaoSgq().db)
            {
                var level1 = db.SearchQuery<ParLevel1DTO>("Select * from parlevel1 WHERE IsActive = 1").ToList();
                var level2 = db.SearchQuery<ParLevel1DTO>("Select * from parlevel2 WHERE IsActive = 1").ToList();
                var level3 = db.SearchQuery<ParLevel1DTO>("Select * from parlevel3 WHERE IsActive = 1").ToList();
                usersgq = db.SearchQuery<UserDTO>("Select * from usersgq");
                parcompany = db.SearchQuery<ParCompanyDTO>("Select * from parcompany WHERE IsActive = 1").ToList();

                filterContext.Controller.ViewBag.Level1 = level1;
                filterContext.Controller.ViewBag.Level2 = level2;
                filterContext.Controller.ViewBag.Level3 = level3;
            }

            var pa_unidades = PlanoAcaoCore.Pa_Unidade.Listar();
            if (parcompany.Count() > 0)
            {
                var iterator = parcompany.Where(r => !pa_unidades.Any(c => c.Name.Equals(r.Initials)) && r.Initials != null).ToList();
                foreach (var i in iterator)
                {
                    var unidadeInsert = new PlanoAcaoCore.Pa_Unidade() { Name = i.Initials, Description = i.Name, Sgq_Id = i.Id };
                    PlanoAcaoCore.Pa_BaseObject.SalvarGenerico(unidadeInsert);
                }
            }

            var pa_quem = PlanoAcaoCore.Pa_Quem.Listar();
            if (usersgq.Count() > 0)
            {
                var iterator = usersgq.Where(r => !pa_quem.Any(c => c.Name.Equals(r.Name) && c.Name != null)).ToList();
                foreach (var i in iterator)
                {
                    var userInsert = new PlanoAcaoCore.Pa_Quem() { Name = i.Name };
                    PlanoAcaoCore.Pa_BaseObject.SalvarGenerico(userInsert);
                }
            }

            filterContext.Controller.ViewBag.Unidade = PlanoAcaoCore.Pa_Unidade.Listar();
            filterContext.Controller.ViewBag.Quem = PlanoAcaoCore.Pa_Quem.Listar();

        }
    }
}
