using ADOFactory;
using Dominio;
using DTO;
using DTO.ResultSet;
using SgqService.ViewModels;
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
        [Route("Get")]
        public cabecalhoSIF Get([FromBody] DataCarrierFormularioNew form, int itenMenu)
        {
            var retorno = new cabecalhoSIF();

            using (var db = new SgqDbDevEntities())
            {
                retorno.Aprovador = getAprovadorName(form, itenMenu, db);

                retorno.Elaborador = getElaboradorName(form, itenMenu, db);

                retorno.NomeRelatorio = getNomeRelatorio(form, itenMenu, db);

                retorno.ReferenciaDocumento = getReferenciaDocumento(form, itenMenu, db);
            }
            
            return retorno;
        }

        private string getAprovadorName(DataCarrierFormularioNew form, int itenMenu, SgqDbDevEntities dbSgq)
        {
            var SQL = $@"SELECT top 1
                         Aprovador
                         FROM ReportXUserSgq RXU      
                         WHERE (RXU.Parcompany_Id in ({string.Join(",", form.ParCompany_Ids)}) OR RXU.Parcompany_Id IS NULL)
                         AND RXU.ParLevel1_Id in ({string.Join(",", form.ParLevel1_Ids)})
                         AND RXU.ItemMenu_Id = {itenMenu}
                         Order by RXU.Parcompany_Id desc";

            return dbSgq.Database.SqlQuery<string>(SQL).FirstOrDefault();
        }

        private string getElaboradorName(DataCarrierFormularioNew form, int itenMenu, SgqDbDevEntities dbSgq)
        {
            var SQL = $@"SELECT top 1
    	                 Elaborador
                         FROM ReportXUserSgq RXU
                         WHERE (RXU.Parcompany_Id in ({string.Join(",", form.ParCompany_Ids)}) OR RXU.Parcompany_Id IS NULL)
                         AND RXU.ParLevel1_Id in ({string.Join(",", form.ParLevel1_Ids)})
                         AND RXU.ItemMenu_Id = {itenMenu}
                         Order by RXU.Parcompany_Id desc";

            return dbSgq.Database.SqlQuery<string>(SQL).FirstOrDefault();
        }

        private string getNomeRelatorio(DataCarrierFormularioNew form, int itenMenu, SgqDbDevEntities dbSgq)
        {
            var SQL = $@"SELECT top 1
    	                 NomeRelatorio
                         FROM ReportXUserSgq RXU
                         WHERE (RXU.Parcompany_Id in ({string.Join(",", form.ParCompany_Ids)}) OR RXU.Parcompany_Id IS NULL)
                         AND RXU.ParLevel1_Id in ({string.Join(",", form.ParLevel1_Ids)})
                         AND RXU.ItemMenu_Id = {itenMenu}
                         Order by RXU.Parcompany_Id desc";

            return dbSgq.Database.SqlQuery<string>(SQL).FirstOrDefault();
        }

        private string getReferenciaDocumento(DataCarrierFormularioNew form, int itenMenu, SgqDbDevEntities dbSgq)
        {
            var SQL = $@"SELECT top 1
    	                 ParLevel1_Id as ReferenciaDocumento
                         FROM ReportXUserSgq RXU
                         inner join ParLevel1 p1 on RXU.ParLevel1_Id = p1.Id
                         WHERE (RXU.Parcompany_Id in ({string.Join(",", form.ParCompany_Ids)}) OR RXU.Parcompany_Id IS NULL)
                         AND RXU.ParLevel1_Id in ({string.Join(",", form.ParLevel1_Ids)})
                         AND RXU.ItemMenu_Id = {itenMenu}
                         Order by RXU.Parcompany_Id desc";

            return dbSgq.Database.SqlQuery<string>(SQL).FirstOrDefault();
        }

        public class cabecalhoSIF
        {
            public string Aprovador { get; set; }
            public string Elaborador { get; set; }
            public string NomeRelatorio { get; set; }
            public string ReferenciaDocumento { get; set; }
        }

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
