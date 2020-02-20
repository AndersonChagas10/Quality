using ADOFactory;
using Dominio;
using DTO;
using DTO.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api.Relatorios
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/RelatorioFormularioSIF")]

    public class RelatorioFormularioSIFApiController : BaseApiController
    {
        private string conexao;

        public RelatorioFormularioSIFApiController()
        {
            conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            db.Configuration.LazyLoadingEnabled = false;
        }

        private List<RelatorioFormularioSIFResultSet> _list { get; set; }
        private SgqDbDevEntities db = new SgqDbDevEntities();

        [HttpPost]
        [Route("GetRelatorioFormularioSIF")]
        public List<RelatorioFormularioSIFResultSet> GetRelatorioFormularioSIF([FromBody] DataCarrierFormularioNew form)
        {
            var query = new RelatorioFormularioSIFResultSet().Select(form, GetUserUnitsIds(form.ShowUserCompanies));

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<RelatorioFormularioSIFResultSet>(query).ToList();

                return _list;
            }
        }
    }
}
