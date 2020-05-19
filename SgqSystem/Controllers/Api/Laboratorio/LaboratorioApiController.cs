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
        public List<RelatorioLaboratorioReultSet> Get([FromBody] DataCarrierFormularioLaboratorio form)
        {

            string whereDate = $"AND cast(dColetaAmostra as date) BETWEEN '{ form.startDate }' AND '{ form.endDate }' ";

            string sqlQuery = $@"

SET LANGUAGE 'Portuguese';

SELECT --TOP 100
	UPPER(S3.Name) AS cNmHolding,
	UPPER(S2.Name) AS cNmRegional,
	UPPER(S1.Name) AS cNmSubRegional,
	E.nCdEmpresa,
	E.cNmEmpresa,
	E.cSgEmpresa,
	CASE
		WHEN cNmTpColeta = 'Água' 
			THEN 'ÁGUA'
		WHEN cNmTpColeta IN ('Swab Pré-operacional','Swab Operacional','Exposição de placa'/*ambiente*/) 
			THEN 'AMBIENTE'
		WHEN cNmTpColeta IN ('In natura','Subprodutos','Alimentos Preparados','Não Destrutivo'/*cc*/,'Destrutiva'/*cc*/) --cc,Cortes, MP,charque
			THEN 'PRODUTO'
			--Carcaca
			----dColetaAmostra vs dMovimento para Fria ou Quente em CC

	END AS cNmVeiculos,
	CUBO.dColetaAmostra,
	YEAR(dColetaAmostra) AS 'dColetaAmostra_ANO',
	MONTH(dColetaAmostra) AS 'dColetaAmostra_MES',
	CONCAT(RIGHT(CONCAT('0', MONTH(dColetaAmostra)), 2), '-', DATENAME(MONTH, dColetaAmostra)) AS 'dColetaAmostra_MES_NOME',
	CONCAT(RIGHT(CONCAT('0', MONTH(dColetaAmostra)), 2), '-', LEFT(DATENAME(MONTH, dColetaAmostra), 3)) AS 'dColetaAmostra_MES_SIGLA',
	DAY(dColetaAmostra) AS 'dColetaAmostra_DIA',
	DATEPART(WEEKDAY, dColetaAmostra) AS 'dColetaAmostra_DIA_SEMANA_NUM',
	CONCAT(RIGHT(CONCAT('0', DATEPART(WEEKDAY, dColetaAmostra)), 2), '-', DATENAME(WEEKDAY, dColetaAmostra)) AS 'dColetaAmostra_DIA_SEMANA',
	CUBO.dProducao,
	CUBO.nCdProduto,
	CASE
		WHEN CUBO.cNmTpColeta IN('Não Destrutivo'/*cc*/,'Destrutiva'/*cc*/)
		THEN 'CARCARÇA'
	ELSE P.cNmProduto
	END cNmProduto,
	CASE
		WHEN CUBO.cNmTpColeta IN('Não Destrutivo'/*cc*/,'Destrutiva'/*cc*/)
		THEN 'CARCARÇA'
	ELSE P.cNmTipoProdutoAL
	END cNmTipoProdutoAL,
	CASE
		WHEN CUBO.cNmTpColeta IN('Não Destrutivo'/*cc*/,'Destrutiva'/*cc*/)
		THEN CASE
				WHEN CAST(dColetaAmostra as date) = CAST(dMovimento AS DATE)
				THEN 'FRIA'
			 ELSE 'QUENTE'
		END
	ELSE P.cNmFamiliaSubGrupo
	END cNmFamiliaSubGrupo,
	CUBO.nCdAnalise,
	CUBO.cNmAnalise,
	CUBO.cGrupoColeta,
	CASE
		WHEN cGrupoColeta = 2 THEN 'Requisição de carcaça'
		WHEN cGrupoColeta = 3 THEN 'Requisição de produto'
		WHEN cGrupoColeta = 4 THEN 'Requisição ambiente'
	END AS cNmGrupoColeta,
	CUBO.nCdSetor,
	CUBO.cNmSetor,
	CUBO.nCdTpColeta,
	CASE
		WHEN CUBO.cNmTpColeta IN('Não Destrutivo'/*cc*/,'Destrutiva'/*cc*/)
		THEN 'In natura'
	ELSE CUBO.cNmTpColeta
	END cNmTpColeta,
	CUBO.nResultadoAnalise,
	CUBO.cSgUnidadeMedidaLaboratorio,
	CUBO.dMovimento,
	CUBO.iLote,
	CUBO.nCdPontoColeta,
	CUBO.cNmPontoColeta,
	CUBO.cNmDetalhePontoColeta,
	CUBO.nCdRequisicaoAnalise,
	CUBO.nCdRequisicaoFilial,
	CUBO.nCdLaboratorio,
	CUBO.cNmLaboratorio,
	CUBO.iAmostra,
	CUBO.cCdBarraAmostra,
	CUBO.nCdMatriz,
	CUBO.cNmMatriz,
	CUBO.nCdGrupoAnalise,
	CUBO.cNmGrupoAnalise,
	CUBO.nCdMetodologia,
	CUBO.cCdMetodologia,
	CUBO.cNmMetodologia,
	CUBO.dAnaliseCritica,
	CUBO.dAnaliseInicial,
	CUBO.dAnaliseFinal,
	CUBO.dPrevisaoFinalAnalise,
	CUBO.nCdValorPadrao,
	CUBO.cSinal1,
	CUBO.nValor1,
	CUBO.cOperadorPadrao,
	CUBO.cSinal2,
	CUBO.nValor2,
	CUBO.cRastreabilidadeLaudo,
	CUBO.Key_Integ,
	CUBO.cResultadoAnalise,
	CASE
		WHEN cResultadoAnalise = '3' THEN 'NC'
		ELSE 'C'
	END Conformidade,
	1 AS AV,
	CASE
		WHEN cResultadoAnalise = '3' THEN 1
		ELSE 0
	END NC
	FROM INTEG.CollectionAnaliseLaboratorial CUBO WITH(NOLOCK)
	LEFT JOIN Empresa E WITH(NOLOCK) ON CUBO.nCdEmpresa = E.nCdEmpresa
	LEFT JOIN ParCompany C WITH(NOLOCK) ON E.nCdEmpresa = C.CompanyNumber
	LEFT JOIN ParCompanyXStructure CS WITH(NOLOCK) ON C.ID = CS.ParCompany_Id
	LEFT JOIN ParStructure S1 WITH(NOLOCK) ON CS.ParStructure_Id = S1.Id
	LEFT JOIN ParStructure S2 WITH(NOLOCK) ON S1.ParStructureParent_Id = S2.Id
	LEFT JOIN ParStructure S3 WITH(NOLOCK) ON S2.ParStructureParent_Id = S3.Id
	LEFT JOIN dbPlanejamentoGestao.dbo.DMProduto P WITH(NOLOCK) ON CUBO.nCdProduto = P.nCdProduto

	WHERE 1 = 1
	--AND cNmTpColeta NOT IN('Não Destrutivo'/*cc*/,'Destrutiva'/*cc*/,'Exposição de placa'/*ambiente*/)
	--AND cNmTpColeta = 'Exposição de placa'
	AND C.IsActive = 1
	AND CS.Active = 1
	AND S1.Active = 1
	AND S2.Active = 1
	AND S3.Active = 1
	{ whereDate }";

            var retorno = new List<RelatorioLaboratorioReultSet>();

            using (var db = new Factory("DefaultConnection"))
            {
                retorno = db.SearchQuery<RelatorioLaboratorioReultSet>(sqlQuery).ToList();
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
