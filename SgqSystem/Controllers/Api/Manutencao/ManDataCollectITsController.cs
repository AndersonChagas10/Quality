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
            bool mockRetorno = true;

            try
            {
                string sql = "";

                sql = "select Name as indicadorNome from DimManColetaDados where DimRealTarget = 'Real' and DimName is not null and DimName = '" + obj.descricao + "'";

                var db1 = new SgqDbDevEntities();

                List<Obj2> list = db1.Database.SqlQuery<Obj2>(sql).ToList();

                obj.indicadorNome = list[0].indicadorNome;

                string verColeta = "";
                int validaColeta = 1;


                using (var dbVerColeta = new SgqDbDevEntities())
                {
                    verColeta = " \n SELECT  ";
                    verColeta += "\n TOP 1 Retorno.Retorno ";
                    verColeta += "\n FROM ( ";
                    verColeta += "\n	SELECT 1 Retorno ";
                    verColeta += "\n	FROM ManColetaDados  ";
                    verColeta += "\n		WHERE Base_parCompany_id = " + obj.parCompany + "  ";
                    verColeta += "\n		  AND Base_dateRef = \'" + obj.data.ToString("yyyy - MM - dd HH: mm: ss") + "\'  ";
                    verColeta += "\n		  AND " + obj.indicadorNome + " IS NOT NULL ";
                    verColeta += "\n		  UNION ALL  ";
                    verColeta += "\n	SELECT 0 ";
                    verColeta += "\n		  )Retorno ";
                    validaColeta = dbVerColeta.Database.SqlQuery<int>(verColeta).FirstOrDefault();
                }

                if (validaColeta == 0)
                {
                    sql = "";

                    sql = "INSERT INTO dbo.ManColetaDados " +
                    "(" +
                    "Base_parCompany_id " +
                    ",Base_dateAdd " +
                    ",Base_dateRef " +
                    ",Comentarios " +
                    "," + obj.indicadorNome +
                    ", userAdd" +
                    ") " +
                    "VALUES " +
                    "(" +
                    "" + obj.parCompany + "," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'" + obj.data.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'" + obj.comentarios + "'," +
                    " replace('" + obj.quantidade + "',',','.')," + //BS: Alteração feita=> Troca de Virgula (,) por Ponto (.): Replace()
                    "'" + obj.user + "'" +
                    ")";

                    using (var db = new SgqDbDevEntities())
                    {
                        var d = db.Database.ExecuteSqlCommand(sql);
                        //return d;
                    }

                }
                else
                {
                    throw new Exception();
                }



                //string sqlTarget = "select * into #ManColetaDadosRealConsolidado from ManColetaDadosRealConsolidado " +

                //                    "\n UPDATE Coleta " +
                //                    "\n SET Coleta.HeadCount_Meta = SgqDbDev.dbo.fn_ManColetaDadosMetas(Man.Base_parCompany_id, Man.Base_dateRef, Man.IndicadorReal) " +
                //                    "\n FROM ManColetaDados Coleta " +
                //                    "\n INNER JOIN #ManColetaDadosRealConsolidado Man " +
                //                     "\n ON Coleta.Base_parCompany_id = Man.Base_parCompany_id " +
                //                     "\n AND Coleta.Base_dateRef = Man.Base_dateRef " +
                //                    "\n WHERE " +
                //                     "\n Man.IndicadorMeta = " + obj.indicadorNome + "" +
                //                    "\n AND " +
                //                     "\n Coleta.Base_DateRef = '" + obj.data.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                //                    "\n AND " +
                //                     "\n Coleta.Base_parCompany_id = " + obj.parCompany + "";


                //using (var db = new SgqDbDevEntities())
                //{
                //    var rs = db.Database.ExecuteSqlCommand(sql);
                //}

                if (mockRetorno)
                {
                    return "Salvo com Sucesso!";
                }
                else
                    return "Sua meta para essa coleta é de 19% , com a informação de 20% ela foi reajustada para 18%";

            }
            catch (Exception e)
            {
                return "     Coleta não realizada! \n Provavelmente você já fez essa coleta hoje!" /*+ e*/;
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
        public string SaveEdit(Obj obj)
        {

            bool mockRetorno = true;
            try
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
                        "," + obj.indicadorNome + " = replace('" + obj.quantidade + "',',','.') " +
                        ",Comentarios = '" + obj.comentarios + "' " +
                        ",userAlter = '" + obj.user + "'" +
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
                    //return d;
                }

                if (mockRetorno)
                {
                    return "Salvo com Sucesso!";
                }
                else
                    return "Target Diário Atual: 19% <br/> Coleta de Hoje: 20% <br/> Novo Target Diário: 21%";

            }
            catch (Exception e)
            {
                return "Erro ao Salvar no Banco: " + e;
                throw;
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
        public string user { get; set; }
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

