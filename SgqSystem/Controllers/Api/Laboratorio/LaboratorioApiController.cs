using ADOFactory;
using DTO.Formulario;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/Laboratorio")]
    public class LaboratorioApiController : BaseApiController
    {

        [HttpPost]
        [Route("Get")]
        public List<JObject> Get([FromBody] DataCarrierFormularioLaboratorio form)
        {

            string whereDate = $"AND cast(dColetaAmostra as date) BETWEEN '{ form.startDate }' AND '{ form.endDate }' ";

            string sqlQuery = GetDicionarioEstatico("queryRelatorioLaboratorio");

            var retorno = new List<JObject>();

            using (var db = new Factory("DefaultConnection"))
            {

                retorno = db.QueryNinjaADO(sqlQuery + " " + whereDate);
            }

            return retorno;

        }
    }

    public class RelatorioLaboratorioReultSet
    {
        public string cNmHolding { get; set; }
        public string cNmRegional { get; set; }
        public string cNmSubRegional { get; set; }
        public int nCdEmpresa { get; set; }
        public string cNmEmpresa { get; set; }
        public string cSgEmpresa { get; set; }
        public string cNmVeiculos { get; set; }
        public DateTime dColetaAmostra { get; set; }
        public string dColetaAmostra_ANO { get; set; }
        public string dColetaAmostra_MES { get; set; }
        public string dColetaAmostra_MES_NOME { get; set; }
        public string dColetaAmostra_MES_SIGLA { get; set; }
        public string dColetaAmostra_DIA { get; set; }
        public string dColetaAmostra_DIA_SEMANA { get; set; }
        public DateTime dProducao { get; set; }
        public int nCdProduto { get; set; }
        public string cNmProduto { get; set; }
        public int nCdAnalise { get; set; }
        public string cNmAnalise { get; set; }
        public int cGrupoColeta { get; set; }
		public string cNmGrupoColeta { get; set; }
		public int nCdSetor { get; set; }
        public string cNmSetor { get; set; }
        public int nCdTpColeta { get; set; }
        public string cNmTpColeta { get; set; }
        public decimal nResultadoAnalise { get; set; }
        public string cSgUnidadeMedidaLaboratorio { get; set; }
        public DateTime dMovimento { get; set; }
        public int iLote { get; set; }
        public decimal nCdPontoColeta { get; set; }
        public string cNmPontoColeta { get; set; }
        public string cNmDetalhePontoColeta { get; set; }
        public decimal nCdRequisicaoAnalise { get; set; }
        public decimal nCdRequisicaoFilial { get; set; }
        public decimal nCdLaboratorio { get; set; }
        public string cNmLaboratorio { get; set; }
        public int iAmostra { get; set; }
        public string cCdBarraAmostra { get; set; }
        public decimal nCdMatriz { get; set; }
        public string cNmMatriz { get; set; }
        public decimal nCdGrupoAnalise { get; set; }
        public string cNmGrupoAnalise { get; set; }
        public decimal nCdMetodologia { get; set; }
        public string cCdMetodologia { get; set; }
        public string cNmMetodologia { get; set; }
        public DateTime dAnaliseCritica { get; set; }
        public DateTime dAnaliseInicial { get; set; }
        public DateTime dAnaliseFinal { get; set; }
        public DateTime dPrevisaoFinalAnalise { get; set; }
        public decimal nCdValorPadrao { get; set; }
        public string cSinal1 { get; set; }
        public decimal nValor1 { get; set; }
        public string cOperadorPadrao { get; set; }
        public string cSinal2 { get; set; }
        public decimal nValor2 { get; set; }
        public string cRastreabilidadeLaudo { get; set; }
		public string cNmTipoProdutoAL { get; set; }
		public string Key_Integ { get; set; }
        public decimal cResultadoAnalise { get; set; }
		public string cNmFamiliaSubGrupo { get; set; }
		public string Conformidade { get; set; }
        public int AV { get; set; }
        public int NC { get; set; }
    }
}
