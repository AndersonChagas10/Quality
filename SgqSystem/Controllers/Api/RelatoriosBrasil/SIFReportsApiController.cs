using ADOFactory;
using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/SIFReports")]
    public class SIFReportsApiController : BaseApiController
    {

        [HttpPost]
        [Route("Get")]
        public Retorno Get([FromBody] FormularioParaRelatorioViewModel form)
        {

            var company = new ParCompany();
            var retorno = new Retorno();

            var BoisProcessados = new List<Dado>();
            var ListaEscalaAbate = new List<EscalaAbate>();

            var userName = "UserGQualidade";
            var pass = "grJsoluco3s";

            var query = "";

            try
            {
                using (SgqDbDevEntities dbSgq = new SgqDbDevEntities())
                {
                    company = dbSgq.ParCompany.FirstOrDefault(r => r.Id == form.unitId);

                    try
                    {
                        using (var dbUnidade = new Factory(company.IPServer, company.DBServer, pass, userName))
                        {
                            query = $@"EXECUTE FBED_GRTEscalaAbate '{ form._dataInicioSQL }' , { company.CompanyNumber }";

                            ListaEscalaAbate = dbUnidade.SearchQuery<EscalaAbate>(query).ToList();
                        }
                    }
                    catch (Exception) // Apagar ou não esse catch
                    {
                        query = $@"SELECT * FROM ESCALAABATE";

                        using (Factory db = new Factory("DefaultConnection"))
                        {
                            ListaEscalaAbate = db.SearchQuery<EscalaAbate>(query).ToList();
                        }

                    }

                    query = getQuery1(form);

                    BoisProcessados = dbSgq.Database.SqlQuery<Dado>(query).ToList();

                    retorno.Dados = new List<Dado>();

                    if (true)
                    {

                        for (int i = 0; i < ListaEscalaAbate.Count; i++)
                        {

                            var dado = new Dado();

                            var boiProcessado = BoisProcessados.Where(r => r.Numero == ListaEscalaAbate[i].iSequencial).FirstOrDefault();

                            if (boiProcessado != null)
                            {
                                dado.Esquerdo = boiProcessado.Esquerdo;
                                dado.Direito = boiProcessado.Direito;
                            }

                            dado.Initials = company.Initials;
                            dado.Numero = ListaEscalaAbate[i].iSequencial;
                            dado.Sequestro = ListaEscalaAbate[i].iSequencialTipificacao == null ? 1 : 0;

                            retorno.Dados.Add(dado);

                        }
                    }
                    else
                    {
                        retorno.Dados = BoisProcessados;
                    }

                    if (retorno.Dados.Count > 0)
                    {
                        retorno.InitialTime = getInitialTime(form, dbSgq);

                        retorno.FinalTime = getFinalTime(form, dbSgq);
                    }
                }

            }
            catch (Exception)
            {


            }

            return retorno;
        }

        private DateTime getInitialTime(FormularioParaRelatorioViewModel form, SgqDbDevEntities dbSgq)
        {

            var query = $@"SELECT
	MIN(CollectionDate)
FROM CollectionLevel2
WHERE 1 = 1
AND UnitId = { form.unitId }
AND ParLevel1_Id = { form.level1Id }
AND ParLevel2_Id = { form.level2Id }
AND CAST(CollectionDate AS DATE) = CAST('{form._dataInicioSQL}' AS DATE)";


            return dbSgq.Database.SqlQuery<DateTime>(query).FirstOrDefault();

        }

        private DateTime getFinalTime(FormularioParaRelatorioViewModel form, SgqDbDevEntities dbSgq)
        {
            var query = $@"SELECT
	MAX(CollectionDate)
FROM CollectionLevel2
WHERE 1 = 1
AND UnitId = { form.unitId }
AND ParLevel1_Id = { form.level1Id }
AND ParLevel2_Id = { form.level2Id }
AND CAST(CollectionDate AS DATE) = CAST('{form._dataInicioSQL}' AS DATE)";


            return dbSgq.Database.SqlQuery<DateTime>(query).FirstOrDefault();
        }

        private string getQuery1(FormularioParaRelatorioViewModel form)
        {
            var query = $@"SELECT
	em_coluna.Sequential AS Numero
   ,CASE
		WHEN [1] > 0 THEN 1
		ELSE 0
	END AS 'Esquerdo'
   ,CASE
		WHEN [2] > 0 THEN 1
		ELSE 0
	END AS 'Direito'
   ,em_coluna.Initials
FROM (SELECT
		C2.Sequential
	   ,C2.Side
	   ,C2.Defects
	   ,PC.Initials
	FROM CollectionLevel2 C2
	LEFT JOIN ParCompany pc
		ON pc.Id = C2.UnitId
	WHERE 1 = 1
	AND C2.ParLevel1_Id = {form.level1Id}
	AND C2.ParLevel2_Id = {form.level2Id}
	AND C2.UnitId = {form.unitId} 
--and Shift = 1
AND CAST(CollectionDate AS DATE) = CAST('{form._dataInicioSQL}' AS DATE)
) em_linha
PIVOT (SUM(Defects) FOR Side IN ([1], [2])) em_coluna
ORDER BY em_coluna.Sequential";

            return query;
        }

    }

    public class Retorno
    {
        public List<Dado> Dados { get; set; }
        public DateTime InitialTime { get; set; }
        public DateTime FinalTime { get; set; }
    }

    public class EscalaAbate
    {
        public DateTime dMovimento { get; set; }
        public int nCdEmpresa { get; set; }
        public int iLote { get; set; }
        public int iQtdeLote { get; set; }
        public int nCdPedidoComercial { get; set; }
        public int iCarga { get; set; }
        public int nCdTerceiro { get; set; }
        public string cNmTerceiro { get; set; }
        public int nCdEntrega { get; set; }
        public string cNmEntrega { get; set; }
        public string cCidade { get; set; }
        public int nCdProdutoCarregar { get; set; }
        public string cNmProduto { get; set; }
        public int nCdHabilitacao { get; set; }
        public string cSgHabilitacao { get; set; }
        public string cNmHabilitacao { get; set; }
        public int iSequencial { get; set; }
        public int? iSequencialTipificacao { get; set; }
    }

    public class Dado
    {
        public int Numero { get; set; }
        public int? Esquerdo { get; set; }
        public int? Direito { get; set; }
        public string Initials { get; set; }
        public int? Sequestro { get; set; }
    }
}
