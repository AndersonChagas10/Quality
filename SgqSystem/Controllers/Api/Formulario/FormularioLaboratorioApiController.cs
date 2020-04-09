using ADOFactory;
using DTO.Formulario;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Formulario
{
    [RoutePrefix("api/FormularioLaboratorio")]
    public class FormularioLaboratorioApiController : BaseApiController
    {
        public class ViewModelLaboratorio
        {
            public string Name { get; set; }

            public string Id { get; set; }
        }

        [HttpPost]
        [Route("GetFilteredcNmSetor")]
        public List<ViewModelLaboratorio> GetFilteredcNmSetor(string search, [FromBody] DataCarrierFormularioLaboratorio form)
        {

            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"
                                select DISTINCT top 500 n.cNmSetor as Name, n.nCdSetor as Id  from  integ.collectionAnaliseLaboratorial n
                                where cNmSetor is not null
                                AND n.cNmSetor like '%{search}%'
                                order by 1";

                var retorno = factory.SearchQuery<ViewModelLaboratorio>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredcNmAnalise")]
        public List<ViewModelLaboratorio> GetFilteredcNmAnalise(string search, [FromBody] DataCarrierFormularioLaboratorio form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"
                                select DISTINCT top 500 intLabo.cNmAnalise as Name, intLabo.nCdAnalise as Id  from  integ.collectionAnaliseLaboratorial intLabo
                                where intLabo.cNmAnalise is not null
                                AND intLabo.cNmAnalise like '%{search}%'
                                order by 1";

                var retorno = factory.SearchQuery<ViewModelLaboratorio>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredcNmTpColeta")]
        public List<ViewModelLaboratorio> GetFilteredcNmTpColeta(string search, [FromBody] DataCarrierFormularioLaboratorio form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"
                                select DISTINCT top 500 intLabo.cNmTpColeta as Name, intLabo.nCDTpColeta as Id  from  integ.collectionAnaliseLaboratorial intLabo
                                where intLabo.cNmTpColeta is not null
                                AND intLabo.cNmTpColeta like '%{search}%'
                                order by 1";

                var retorno = factory.SearchQuery<ViewModelLaboratorio>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredcSgUnidadeMedidaLaboratorio")]
        public List<ViewModelLaboratorio> GetFilteredcSgUnidadeMedidaLaboratorio(string search, [FromBody] DataCarrierFormularioLaboratorio form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"
                                select DISTINCT top 500 intLabo.cSgUnidadeMedidaLaboratorio as Name, intLabo.cSgUnidadeMedidaLaboratorio as Id  from  integ.collectionAnaliseLaboratorial intLabo
                                where intLabo.cSgUnidadeMedidaLaboratorio is not null
                                AND intLabo.cSgUnidadeMedidaLaboratorio like '%{search}%'
                                order by 1";

                var retorno = factory.SearchQuery<ViewModelLaboratorio>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredcNmPontoColeta")]
        public List<ViewModelLaboratorio> GetFilteredcNmPontoColeta(string search, [FromBody] DataCarrierFormularioLaboratorio form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"
                                select DISTINCT top 500 intLabo.cNmPontoColeta as Name, intLabo.nCdPontoColeta as Id  from  integ.collectionAnaliseLaboratorial intLabo
                                where intLabo.cNmPontoColeta is not null
                                AND intLabo.cNmPontoColeta like '%{search}%'
                                order by 1";

                var retorno = factory.SearchQuery<ViewModelLaboratorio>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredcNmDetalhePontoColeta")]
        public List<ViewModelLaboratorio> GetFilteredcNmDetalhePontoColeta(string search, [FromBody] DataCarrierFormularioLaboratorio form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"
                                select DISTINCT top 500 intLabo.cNmDetalhePontoColeta as Name, intLabo.cNmDetalhePontoColeta as Id  from  integ.collectionAnaliseLaboratorial intLabo
                                where intLabo.cNmDetalhePontoColeta is not null
                                AND intLabo.cNmDetalhePontoColeta like '%{search}%'
                                order by 1";

                var retorno = factory.SearchQuery<ViewModelLaboratorio>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredcNmLaboratorio")]
        public List<ViewModelLaboratorio> GetFilteredcNmLaboratorio(string search, [FromBody] DataCarrierFormularioLaboratorio form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"
                                select DISTINCT top 500 intLabo.cNmLaboratorio as Name, intLabo.nCdLaboratorio as Id  from  integ.collectionAnaliseLaboratorial intLabo
                                where intLabo.cNmLaboratorio is not null
                                AND intLabo.cNmLaboratorio like '%{search}%'
                                order by 1";

                var retorno = factory.SearchQuery<ViewModelLaboratorio>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredcCdBarraAmostra")]
        public List<ViewModelLaboratorio> GetFilteredcCdBarraAmostra(string search, [FromBody] DataCarrierFormularioLaboratorio form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"
                                select DISTINCT top 500 intLabo.cCdBarraAmostra as Name, intLabo.cCdBarraAmostra as Id  from  integ.collectionAnaliseLaboratorial intLabo
                                where intLabo.cCdBarraAmostra is not null
                                AND intLabo.cCdBarraAmostra like '%{search}%'
                                order by 1";

                var retorno = factory.SearchQuery<ViewModelLaboratorio>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredcNmMatriz")]
        public List<ViewModelLaboratorio> GetFilteredcNmMatriz(string search, [FromBody] DataCarrierFormularioLaboratorio form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"
                                select DISTINCT top 500 intLabo.cNmMatriz as Name, intLabo.nCdMatriz as Id  from  integ.collectionAnaliseLaboratorial intLabo
                                where intLabo.cNmMatriz is not null
                                AND intLabo.cNmMatriz like '%{search}%'
                                order by 1";

                var retorno = factory.SearchQuery<ViewModelLaboratorio>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredcNmGrupoAnalise")]
        public List<ViewModelLaboratorio> GetFilteredcNmGrupoAnalise(string search, [FromBody] DataCarrierFormularioLaboratorio form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"
                                select DISTINCT top 500 intLabo.cNmGrupoAnalise as Name, intLabo.nCdGrupoAnalise as Id  from  integ.collectionAnaliseLaboratorial intLabo
                                where intLabo.cNmGrupoAnalise is not null
                                AND intLabo.cNmGrupoAnalise like '%{search}%'
                                order by 1";

                var retorno = factory.SearchQuery<ViewModelLaboratorio>(query).ToList();

                return retorno;
            }
        }

        [HttpPost]
        [Route("GetFilteredcNmMetodologia")]
        public List<ViewModelLaboratorio> GetFilteredcNmMetodologia(string search, [FromBody] DataCarrierFormularioLaboratorio form)
        {
            using (var factory = new Factory("DefaultConnection"))
            {
                var query = $@"
                                select DISTINCT top 500 intLabo.cNmMetodologia as Name, intLabo.nCdMetodologia as Id  from  integ.collectionAnaliseLaboratorial intLabo
                                where intLabo.cNmMetodologia is not null
                                AND intLabo.cNmMetodologia like '%{search}%'
                                order by 1";

                var retorno = factory.SearchQuery<ViewModelLaboratorio>(query).ToList();

                return retorno;
            }
        }
    }
}
