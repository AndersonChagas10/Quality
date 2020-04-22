using ADOFactory;
using DTO.Formulario;
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
        public List<Object> Get([FromBody] DataCarrierFormularioLaboratorio form)
        {
            string sqlQuery = $@"

SELECT DISTINCT top 10
C.nCdEmpresa
,C.cNmEmpresa
,C.cSgEmpresa
,dColetaAmostra
--,YEAR(dColetaAmostra) ""dColetaAmostra - ANO""
--,MONTH(dColetaAmostra) ""dColetaAmostra - MES""
--,DAY(dColetaAmostra) ""dColetaAmostra - DIA""
,dProducao
,nCdProduto
,nCdAnalise
,cNmAnalise
,cGrupoColeta
,nCdSetor
,cNmSetor
,nCdTpColeta
,cNmTpColeta
,cSgUnidadeMedidaLaboratorio
,dMovimento
,iLote
,nCdPontoColeta
,cNmPontoColeta
,cNmDetalhePontoColeta
,nCdRequisicaoAnalise
,nCdRequisicaoFilial
,nCdLaboratorio
,cNmLaboratorio
,iAmostra
,cCdBarraAmostra
,nCdMatriz
,cNmMatriz
,nCdGrupoAnalise
,cNmGrupoAnalise
,nCdMetodologia
,cCdMetodologia
,cNmMetodologia
,dAnaliseCritica
,dAnaliseInicial
,dAnaliseFinal
,dPrevisaoFinalAnalise
,nCdValorPadrao
,nResultadoAnalise
,cSinal1
,nValor1
,cOperadorPadrao
,cSinal2
,nValor2
,cRastreabilidadeLaudo
,Key_Integ
,CASE
    WHEN cResultadoAnalise = '2' THEN 'C'

    WHEN cResultadoAnalise = '3' THEN 'NC'
END Conformidade
,1 AS AV
, CASE

    WHEN cResultadoAnalise = '2' THEN 0

    WHEN cResultadoAnalise = '3' THEN 1
END NC

        FROM INTEG.CollectionAnaliseLaboratorial INTEG

        LEFT JOIN Empresa C

            ON INTEG.nCdEmpresa = C.nCdEmpresa


            ";

            var retorno = new List<Object>();

            using (var db = new Factory("DefaultConnection"))
            {
                retorno = db.SearchQuery<Object>(sqlQuery).ToList();
            }

            return retorno;

        }
    }

    public class RelatorioLaboratorioReultSet
    {
        public decimal nCdEmpresa { get; set; }
        public DateTime dColetaAmostra { get; set; }
        public DateTime dProducao { get; set; }
        public decimal nCdProduto { get; set; }
        public decimal nCdAnalise { get; set; }
        public string cNmAnalise { get; set; }
        public string cGrupoColeta { get; set; }
        public decimal nCdSetor { get; set; }
        public string cNmSetor { get; set; }
        public decimal nCdTpColeta { get; set; }
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
        public string Key_Integ { get; set; }
        public DateTime AddDate { get; set; }
        public decimal cResultadoAnalise { get; set; }
    }
}
