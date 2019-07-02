using ADOFactory;
using Dominio;
using DTO;
using DTO.Formulario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Formulario
{
    [RoutePrefix("api/Formulario")]
    public class FormularioApiController : BaseApiController
    {

        public FormularioApiController()
        {

        }

        [HttpPost]
        [Route("GetForm")]
        public FormularioViewModel GetForm([FromBody] DataCarrierFormularioNew form)
        {
            var retornoFormulario = new FormularioViewModel();

            using (var factory = new Factory("DefaultConnection"))
            {
                retornoFormulario.ParStructures = GetParStructure(form, factory);
                retornoFormulario.ParCompanies = GetParCompanies(form, factory);
                retornoFormulario.ParLevel1s = GetParLevel1s(form, factory);
                retornoFormulario.ParLevel2s = GetParLevel2s(form, factory);
                retornoFormulario.ParLevel3s = GetParLevel3s(form, factory);
            }

            return retornoFormulario;

        }

        private List<ParStructure> GetParStructure(DataCarrierFormularioNew form, Factory factory)
        {
            var sql = "Select Id, Name from ParStructure where Active = 1";

            var retorno = factory.SearchQuery<ParStructure>(sql).ToList();

            return retorno;
        }

        private List<ParCompany> GetParCompanies(DataCarrierFormularioNew form, Factory factory)
        {

            var filtroStructure = form.ParStructure_Ids.Length > 0 ? $@"AND PCXS.Id IN ({ string.Join(",", form.ParStructure_Ids) })" : "";

            var query = $@"SELECT
                        	PC.Id, PC.Name
                        FROM ParCompany PC
                        LEFT JOIN ParCompanyXStructure PCXS
                        	ON PC.Id = PCXS.ParCompany_Id
                        		AND PCXS.Active = 1
                        WHERE 1 = 1
                        AND PC.IsActive = 1
                        --Filtros
                        {filtroStructure}";

            var retorno = factory.SearchQuery<ParCompany>(query).ToList();

            return retorno;
        }


        private List<ParLevel1> GetParLevel1s(DataCarrierFormularioNew form, Factory factory)
        {

            var query = "SELECT * FROM parLevel1";

            var retorno = factory.SearchQuery<ParLevel1>(query).ToList();

            return retorno;
        }

        private List<ParLevel2> GetParLevel2s(DataCarrierFormularioNew form, Factory factory)
        {

            var query = "SELECT * FROM parLevel2";

            var retorno = factory.SearchQuery<ParLevel2>(query).ToList();

            return retorno;
        }

        private List<ParLevel3> GetParLevel3s(DataCarrierFormularioNew form, Factory factory)
        {

            var query = "SELECT * FROM parLevel3";

            var retorno = factory.SearchQuery<ParLevel3>(query).ToList();

            return retorno;
        }
    }
}
