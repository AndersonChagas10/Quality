using Dominio;
using DTO.Helpers;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace SgqSystem.Controllers.Api.Manutencao
{

    [RoutePrefix("api/Manutencao")]
    public class ManDataCollectITsController : ApiController
    {
        //[HttpPost]
        //[HandleApi()]
        //[Route("SaveCreate")]
        //public int SaveCreate(Obj obj)
        //{
        //    string sql = "";

        //    //string sql = "INSERT INTO [dbo].[ManDataCollectIT]" +
        //    //  "([AddDate]" +
        //    //  ",[ReferenceDatetime]" +
        //    //  ",[UserSGQ_Id]" +
        //    //  ",[ParCompany_Id]" +
        //    //  ",[DimManutencaoColetaITs_id]" +
        //    //  ",[AmountData]" +
        //    //  ",[Comments]" +
        //    //  ",[IsActive])" +
        //    //  "VALUES" +
        //    //  "(" + DateTime.Now + "," +
        //      //"," + manDataCollectITs.ReferenceDatetime +
        //      ////"," + Guard.GetUsuarioLogado_Id(System.Web.HttpContext) +
        //      //"," + manDataCollectITs.ParCompany_Id +
        //      //"," + manDataCollectITs.DimManutencaoColetaITs_id +
        //      //"," + manDataCollectITs.AmountData +
        //      //"," + manDataCollectITs.Comments +
        //      //"," + true + ")";

        //    using (var db = new SgqDbDevEntities())
        //    {
        //        var d = db.Database.ExecuteSqlCommand(sql);
        //        return d;
        //    }

        //}

        [HttpPost]
        [Route("SaveCreate")]
        public int SaveCreate(Obj obj)
        {
            string sql = "";

            sql = "select Name as indicadorNome from DimManColetaDados where DimRealTarget = 'Real' and DimName is not null and DimName = '" + obj.descricao + "'";

            var db1 = new SgqDbDevEntities();
            
            List<Obj2> list = db1.Database.SqlQuery<Obj2>(sql).ToList();

            obj.indicadorNome = list[0].indicadorNome;

            sql = "";

            sql = "INSERT INTO dbo.ManColetaDados " +
            "(" +
            "Base_parCompany_id " +
            ",Base_dateAdd " +
            ",Base_dateRef " +
            ",Comentarios " +
            "," + obj.indicadorNome +
            ") " +
            "VALUES " +
            "(" +
            "" + obj.parCompany + "," +
            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
            "'" + obj.data.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
            "'" + obj.comentarios + "'," +
            "'" + obj.quantidade + "'" +
            ")";

            using (var db = new SgqDbDevEntities())
            {
                var d = db.Database.ExecuteSqlCommand(sql);
                return d;
            }

        }

        [HttpPost]
        [Route("SaveCreateAll")]
        public int SaveCreateAll(Obj obj)
        {
            string sql = "";

            using (var db = new SgqDbDevEntities())
            {
                var d = db.Database.ExecuteSqlCommand(sql);
                return d;
            }

        }

    }

    public class Obj
    {
        public string indicadorNome { get; set; }
        public string descricao { get; set; }
        public decimal quantidade { get; set; }
        public string comentarios { get; set; }
        public DateTime data { get; set; }
        public int parCompany { get; set; }
    }

    public class Obj2
    {
        public string indicadorNome { get; set; }
    }

}
