using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace SgqSystem.Controllers.Api.Manutencao
{

    [RoutePrefix("api/Manutencao")]
    public class ManDataCollectITsController : ApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        [HttpPost]
        [Route("SaveCreate")]
        public string SaveCreate(Obj obj)
        {
            //Fazer a inserção do Orçado
            try
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
                " replace('" + obj.quantidade + "',',','.')" + //BS: Alteração feita=> Troca de Virgula (,) por Ponto (.): Replace()
                ")";

                using (var db = new SgqDbDevEntities())
                {
                    var d = db.Database.ExecuteSqlCommand(sql);
                    //return d;
                }


                if (true)
                {
                    return "Salvo com sucesso";
                }
                    //else
                    //return "Sua meta para essa coleta é de "++" , com a informação de "++" ela foi reajustada para"++;

            }
            catch (Exception e)
            {
                return "Erro ao Salvar no Banco: " + e;
                throw;
            }

        }

        [HttpPost]
        [Route("GetIndicadores")]
        public List<Indicador> GetIdicadores(Obj3 obj3)
        {
            List<Indicador> Indicadores;
            List<Indicador> Indicadores2 = new List<Indicador>();

            string query = "SELECT DISTINCT DimName as Nome, Name as NomeReal FROM DimManColetaDados WHERE DimRealTarget = 'Real' and DimName is not null";

            Indicadores = db.Database.SqlQuery<Indicador>(query).ToList();

            //Pergunta se existe o Indicador na data

            int nIndicadores = Indicadores.Count;

            for (int i = 0; i < nIndicadores; i++)
            {
                query = "SELECT top 1 " + Indicadores[i].NomeReal + " as PerguntaIndicador FROM ManColetaDados WHERE Base_dateRef = '" + obj3.Date.ToString("yyyy-MM-dd") + "' AND " + Indicadores[i].NomeReal + " IS NOT NULL AND Base_parCompany_id = " + obj3.Unit;

                Nullable<decimal> result = db.Database.SqlQuery<Nullable<decimal>>(query).FirstOrDefault();

                if (result == null)
                {
                    Indicadores2.Add(Indicadores[i]);
                }

            }

            return Indicadores2;
        }

        [HttpPost]
        [Route("GetIndicadoresEdit")]
        public List<Indicador> GetIndicadoresEdit(Obj3 obj3)
        {
            List<Indicador> Indicadores;
            List<Indicador> Indicadores2 = new List<Indicador>();

            string query = "SELECT DISTINCT DimName as Nome, Name as NomeReal FROM DimManColetaDados WHERE DimRealTarget = 'Real' and DimName is not null";

            Indicadores = db.Database.SqlQuery<Indicador>(query).ToList();

            //Busca valores do real e meta dos indicadores

            //Pergunta se existe o Indicador na data

            int nIndicadores = Indicadores.Count;

            for (int i = 0; i < nIndicadores; i++)
            {
                query = "SELECT top 1 " + Indicadores[i].NomeReal + " as PerguntaIndicador FROM ManColetaDados WHERE Base_dateRef = '" + obj3.Date.ToString("yyyy-MM-dd") + "' AND " + Indicadores[i].NomeReal + " IS NOT NULL AND Base_parCompany_id = " + obj3.Unit;

                Nullable<decimal> result = db.Database.SqlQuery<Nullable<decimal>>(query).FirstOrDefault();

                if (result != null)
                {
                    Indicadores2.Add(Indicadores[i]);
                }

            }

            nIndicadores = Indicadores2.Count;

            for (int i = 0; i < nIndicadores; i++)
            {
                query = "SELECT top 1 " + Indicadores2[i].NomeReal + " as ValorReal FROM ManColetaDados WHERE Base_dateRef = '" + obj3.Date.ToString("yyyy-MM-dd") + "' AND " + Indicadores2[i].NomeReal + " IS NOT NULL AND Base_parCompany_id = " + obj3.Unit;
                decimal result = db.Database.SqlQuery<decimal>(query).FirstOrDefault();
                Indicadores2[i].ValorReal = result;
            }

            return Indicadores2;
        }

        [HttpPost]
        [Route("SaveEdit")]
        public int SaveEdit(Obj obj)
        {
            string sql = "";

            sql = "select Name as indicadorNome from DimManColetaDados where DimRealTarget = 'Real' and DimName is not null and DimName = '" + obj.descricao + "'";

            var db1 = new SgqDbDevEntities();

            List<Obj2> list = db1.Database.SqlQuery<Obj2>(sql).ToList();

            obj.indicadorNome = list[0].indicadorNome;

            sql = "";

            sql = "UPDATE ManColetaDados " +
                   "SET " +
                    "Base_dateAlter = GETDATE() " +
                    "," + obj.indicadorNome + " = '" + obj.quantidade + "' " +
                    ",Comentarios = '" + obj.comentarios + "' " +
                   "from ManColetaDados " +
                   "WHERE " +
                    "Base_parCompany_id = '" + obj.parCompany + "' " +
                   "AND " +
                   "Base_dateRef = '" + obj.data.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                   " AND " +
                    obj.indicadorNome + " IS NOT NULL";

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

    public class Obj3
    {
        public DateTime Date { get; set; }
        public string Unit { get; set; }
    }

    public class Indicador
    {
        public string Nome { get; set; }
        public string NomeReal { get; set; }
        public decimal? ValorReal { get; set; }
    }

    public class Pergunta
    {
        public string PerguntaIndicador { get; set; }
    }
}

