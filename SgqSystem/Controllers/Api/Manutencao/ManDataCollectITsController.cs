using Dominio;
using DTO.Helpers;
using SgqSystem.Handlres;
using System;
using System.Web.Http;


namespace SgqSystem.Controllers.Api.Manutencao
{
    [HandleApi()]
    [RoutePrefix("api/Manutencao")]
    public class ManDataCollectITsController : ApiController
    {
        [HttpPost]
        [HandleApi()]
        [Route("SaveCreate")]
        public int SaveCreate(ManDataCollectIT manDataCollectITs)
        {
            string sql = "INSERT INTO [dbo].[ManDataCollectIT]" +
              "([AddDate]" +
              ",[ReferenceDatetime]" +
              ",[UserSGQ_Id]" +
              ",[ParCompany_Id]" +
              ",[DimManutencaoColetaITs_id]" +
              ",[AmountData]" +
              ",[Comments]" +
              ",[IsActive])" +
              "VALUES" +
              "(" + DateTime.Now + "," +
              "," + manDataCollectITs.ReferenceDatetime +
              //"," + Guard.GetUsuarioLogado_Id(System.Web.HttpContext) +
              "," + manDataCollectITs.ParCompany_Id +
              "," + manDataCollectITs.DimManutencaoColetaITs_id +
              "," + manDataCollectITs.AmountData +
              "," + manDataCollectITs.Comments +
              "," + true + ")";

            using (var db = new SgqDbDevEntities())
            {
                var d = db.Database.ExecuteSqlCommand(sql);
                return d;
            }

        }

    }

    public class Obj
    { 
        private string descricao { get; set; }
        private decimal quantidade { get; set; }
        private string comentarios { get; set; }
        private DateTime data { get; set; }
        private int parCompany { get; set; }
    }

}
