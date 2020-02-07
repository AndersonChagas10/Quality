using ADOFactory;
using DTO;
using DTO.ResultSet;
using SgqService.ViewModels;
using SgqServiceBusiness.Helpers;
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
    [RoutePrefix("api/RelatorioEdicao")]
    public class RelatorioEdicaoApiController : ApiController
    {
        private List<RelatorioEdicaoResultSet> _list { get; set; }

        [HttpPost]
        [Route("GetRelatorioEdicao")]
        public List<RelatorioEdicaoResultSet> GetRelatorioEdicao([FromBody] DataCarrierFormularioNew form)
        {
            var query = "";

            if (form.tipoEdicao[0] == 1)
            {
                query = new RelatorioEdicaoResultSet().SelectEdicaoResultado(form);
            }
            else if (form.tipoEdicao[0] == 2)
            {
                 query = new RelatorioEdicaoResultSet().SelectEdicaoCabecalho(form);
            }

            using (Factory factory = new Factory("DefaultConnection"))
            {
                _list = factory.SearchQuery<RelatorioEdicaoResultSet>(query).ToList();

                return _list;
            }
        }
    }
}
