using ADOFactory;
using DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Relatorios.SGCOVID
{
    [RoutePrefix("api/RelatorioResultadoCovid")]
    public class RelatorioResultadoCovidApiController : BaseApiController
    {
        [HttpPost]
        [Route("Get")]
        public List<JObject> GetRelatorio([FromBody] DataCarrierFormularioNew form)
        {
            string whereDate = $"AND cast(C2.CollectionDate as date) BETWEEN cast('{form.startDate.ToString("yyyy-MM-dd")}' as date) AND cast('{ form.endDate.ToString("yyyy-MM-dd") }' as date) ";

            string whereCluster = string.Empty;
            if (form.ParCluster_Ids.Length > 0)
                whereCluster = $"AND C2xC.ParCluster_Id in ({string.Join(",", form.ParCluster_Ids)})";

            string whereStructure2 = string.Empty;
            if (form.ParStructure2_Ids.Length > 0)
                whereStructure2 = $"AND S2.ID in ({string.Join(",", form.ParStructure2_Ids)})";

            string whereStructure3 = string.Empty;
            if (form.ParStructure3_Ids.Length > 0)
                whereStructure3 = $"AND S1.ID in ({string.Join(",", form.ParStructure3_Ids)})";

            string whereUnidade = string.Empty;
            if (form.ParCompany_Ids.Length > 0)
                whereUnidade = $"AND C2.UnitId in ({string.Join(",", form.ParCompany_Ids)})";

            string parFrequency = "0";
            if (form.ParFrequency_Ids.Count() > 0)
                parFrequency = form.ParFrequency_Ids[0].ToString();

            string sqlQuery = GetDicionarioEstatico("queryRelatorioResultadoCovid");

            string idsClusterRelatorioResultadoCovid = GetDicionarioEstatico("idsClusterRelatorioResultadoCovid");

            if (!string.IsNullOrEmpty(idsClusterRelatorioResultadoCovid))
                sqlQuery = sqlQuery.Replace("{idsClusterRelatorioResultadoCovid}", $"AND C2xC.ParCluster_Id IN({ idsClusterRelatorioResultadoCovid })");
            else
                sqlQuery = sqlQuery.Replace("{idsClusterRelatorioResultadoCovid}", string.Empty);

            var userSgq = GetUsuarioLogado();

            if (form.ShowUserCompanies && 
                userSgq != null && 
                userSgq.ShowAllUnits != null && !userSgq.ShowAllUnits.Value && 
                !userSgq.Role.Contains("Admin"))
                whereDate += $@" AND C.Id IN(SELECT ParCompany_Id FROM ParCompanyXUserSgq WHERE UserSgq_Id = {userSgq.Id})";

            sqlQuery = sqlQuery.Replace("{parFrequency}", parFrequency);
            sqlQuery = sqlQuery.Replace("{whereUnidade}", whereUnidade);
            sqlQuery = sqlQuery.Replace("{whereDate}", whereDate);
            sqlQuery = sqlQuery.Replace("{whereCluster}", whereCluster);
            sqlQuery = sqlQuery.Replace("{whereStructure2}", whereStructure2);
            sqlQuery = sqlQuery.Replace("{whereStructure3}", whereStructure3);

            var retorno = new List<JObject>();

            using (var db = new Factory("DefaultConnection"))
            {

                retorno = db.QueryNinjaADO(sqlQuery);
            }

            return retorno;
        }
    }
}
