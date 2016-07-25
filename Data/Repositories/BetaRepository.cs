using Data.ComplexType;
using Dominio;
using Dominio.Interfaces.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data.Repositories
{

    /// <summary>
    /// BetaRepository é uma Classe criada para Sprint-1 do SgqGlobal, esta classe serve como exemplo para construção das demais sprints.
    /// </summary>
    public class BetaRepository : RepositoryBase<Coleta>, IBetaRepository
    {

        #region Construtor e atributos privados

        public BetaRepository(SgqDbDevEntities _db)
            : base(_db)
        {
        }

        private DbSet<Level1> _Level1 { get { return db.Set<Level1>(); } }
        private DbSet<Level2> _Level2 { get { return db.Set<Level2>(); } }
        private DbSet<Level3> _Level3 { get { return db.Set<Level3>(); } }

        #endregion

    

        #region Busca de Dados

        /// <summary>
        /// Busca quantidade de Não conformidades para Nivel1 agrupada por Avaliação e Amostra. No caso deste metodo, na Sprint-1 não foi contemplado Parametrização para estes dados além da descrita anteriormente.
        /// </summary>
        /// <param name="indicadorId"></param>
        /// <param name="dateInit"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<Coleta> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd)
        {

            /*
             
                SELECT 
                id_Level1
                , SUM(Evaluate)"Evaluate"
                , SUM(NotConform) "NotConform"
                FROM(
                SELECT id_Level1
                , Case when sum(Evaluate) > 0 Then 1 ELSE 0 end "Evaluate" 
                , CASE WHEN SUM(NotConform) > 0 THEN 1 ELSE 0 END "NotConform" FROM Coleta GROUP BY 
                numero1,
                Id_Level2,
                numero2,
                Id_Level1
                ) ind
                GROUP BY 
                Id_Level1
            
            */

            //select CAST(getdate() as Date) >>> VERIFICAR PARA TRUNCAR DATA.

            var query = string.Format("SELECT" +
                " id_Level1, " +
                " CONVERT(DECIMAL(16,4),SUM(Evaluate)) 'Evaluate', " +
                " CONVERT(DECIMAL(16,4),SUM(NotConform)) 'NotConform' " +
                " FROM( select id_Level1," +
                " CASE WHEN SUM(Evaluate) > 0 THEN 1 ELSE 0 end 'Evaluate' , " +
                " CASE WHEN SUM(NotConform) > 0 THEN 1 ELSE 0 end 'NotConform'" +
                " FROM Coleta " +
                " WHERE AddDate BETWEEN  '{0} 00:00:00' AND '{1} 23:59:59' " +
                " GROUP BY " +
                " numero1," +
                " numero2," +
                " Id_Level2 ," +
                " Id_Level1) ind GROUP BY Id_Level1", dateInit, dateEnd);

            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).OrderByDescending(r => r.NotConform / r.Evaluate * 100).ToList();

            List<Coleta> resultsList = RetornoQueryIndicadoresRelBateToColeta(queryResult);

            return resultsList;
        }

        /// <summary>
        /// Busca quantidade de Não conformidades para Nivel2 agrupada por Avaliação e Amostra. No caso deste metodo, na Sprint-1 não foi contemplado Parametrização para estes dados além da descrita anteriormente.
        /// </summary>
        /// <param name="indicadorId"></param>
        /// <param name="dateInit"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<Coleta> GetNcPorLevel2(int indicadorId, string dateInit, string dateEnd)
        {

            /*
             */

            var query = string.Format("SELECT id_Level1," +
                " Id_Level2," +
                " CONVERT(DECIMAL(16,4), sum(Evaluate)) 'Evaluate', " +
                " CONVERT(DECIMAL(16,4), sum(NotConform)) 'NotConform' " +
                " FROM(" +
                " SELECT id_Level1, " +
                " Id_Level2, " +
                " CASE WHEN SUM(Evaluate) > 0 THEN 1 ELSE 0 END 'Evaluate'," +
                " CASE WHEN SUM(NotConform) > 0 THEN 1 ELSE 0 END 'NotConform' " +
                " FROM Coleta " +
                " WHERE Id_Level1 = {0} " +
                " AND AddDate BETWEEN  '{1} 00:00:00' AND '{2} 23:59:59' " +
                " AND NotConform > 0" +
                " GROUP BY numero1, " +
                " numero2, " +
                " Id_Level1, " +
                " Id_Level2)" +
                " ind GROUP BY Id_Level1," +
                " Id_Level2" +
                " ORDER BY NotConform desc"
                , indicadorId, dateInit, dateEnd);


            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).ToList();

            List<Coleta> resultsList = RetornoQueryIndicadoresRelBateToColeta(queryResult);

            return resultsList;
        }

        /// <summary>
        /// Busca quantidade de Não conformidades para Nivel3. No caso deste metodo, na Sprint-1 não foi contemplado Parametrização.
        /// </summary>
        /// <param name="indicadorId"></param>
        /// <param name="Level2Id"></param>
        /// <param name="dateInit"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<Coleta> GetNcPorLevel3(int indicadorId, int Level2Id, string dateInit, string dateEnd)
        {

            /*
             */

            var query = string.Format("SELECT " +
                                        "id_Level1" +
                                        ", Id_Level2 " +
                                        ", Id_Level3 " +
                                        ", CONVERT(DECIMAL(16,4), SUM(Evaluate)) AS 'Evaluate'" +
                                        ", CONVERT(DECIMAL(16,4), SUM(NotConform)) AS 'NotConform'" +
                                        " FROM Coleta" +
                                        " WHERE Id_Level2 = {0} AND Id_Level1 = {1}" +
                                        " AND AddDate BETWEEN  '{2} 00:00:00' AND '{3} 23:59:59' " +
                                        " AND NotConform > 0" +
                                        //" AND  numero1 = (select MAX(numero1) from Coleta where Id_Level1 = {1} AND Id_Level2 = {0} AND AddDate BETWEEN  '{2} 00:00:00' AND '{3} 23:59:59')" +
                                        //" AND numero2 = (select MAX(numero2) from Coleta where Id_Level1 = {1} AND Id_Level2 = {0} AND AddDate BETWEEN  '{2} 00:00:00' AND '{3} 23:59:59')" +
                                        " GROUP BY" +
                                        " id_Level1" +
                                        ", Id_Level2" +
                                        ", Id_Level3" +
                                        " ORDER BY NotConform desc", Level2Id, indicadorId, dateInit, dateEnd);

            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).ToList();

            List<Coleta> resultsList = RetornoQueryIndicadoresRelBateToColeta(queryResult);

            return resultsList;
        }

        /// <summary>
        /// Busca quantidade de Não conformidades para Nivel3 somente da ultima avaliação realizada de um indicador. No caso deste metodo, na Sprint-1 não foi contemplado Parametrização. Este método foi construido e depois modificado para a funcionalidade da tela de Coleta de dados > Level2s: coluna Defects.
        /// </summary>
        /// <param name="indicadorId"></param>
        /// <param name="dateInit"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<Coleta> GetNcPorLevel2Jelsafa(int indicadorId, string dateInit, string dateEnd)
        {

            /*
             */

            var query = string.Format("SELECT id_Level1," +
                " Id_Level2," +
                " CONVERT(DECIMAL(16,4), sum(Evaluate)) 'Evaluate', " +
                " CONVERT(DECIMAL(16,4), sum(NotConform)) 'NotConform' " +
                " FROM(" +
                " SELECT id_Level1, " +
                " Id_Level2, " +
                " CASE WHEN SUM(Evaluate) > 0 THEN 1 ELSE 0 END 'Evaluate'," +
                " CASE WHEN SUM(NotConform) > 0 THEN 1 ELSE 0 END 'NotConform' " +
                " FROM Coleta " +
                " WHERE Id_Level1 = {0} " +
                " AND AddDate BETWEEN  '{1} 00:00:00' AND '{2} 23:59:59' " +
                //" AND NotConform > 0" +
                " GROUP BY numero1, " +
                " numero2, " +
                " Id_Level1, " +
                " Id_Level2)" +
                " ind GROUP BY Id_Level1," +
                " Id_Level2" +
                " ORDER BY NotConform desc", indicadorId, dateInit, dateEnd);


            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).ToList();

            List<Coleta> resultsList = RetornoQueryIndicadoresRelBateToColeta(queryResult);

            return resultsList;
        }

        #endregion

        #region Auxiliares

        /// <summary>
        /// Metodo que faz a conversão de uma complexType para Model, devido utilização de uma query distinta de Linq e Lambda, pela complexibilidade de query, foi necessário realizar o procedimento para reaproveitar a Classe ja definida.
        /// </summary>
        /// <param name="queryResult">Resultado de uma query com headers compatíveis com: Coleta</param>
        /// <returns></returns>
        private List<Coleta> RetornoQueryIndicadoresRelBateToColeta(List<RetornoQueryIndicadoresRelBate> queryResult)
        {
            var resultsList = new List<Coleta>();

            foreach (var i in queryResult)
            {

                resultsList.Add(new Coleta()
                {
                    Evaluate = i.Evaluate,
                    NotConform = i.NotConform,
                    Id_Level1 = i.Id_Level1,
                    Id_Level2 = i.Id_Level2,
                    Id_Level3 = i.Id_Level3,
                    //Level1 = db.indicadores.FirstOrDefault(r => r.Id == i.Id_Level1).Name,
                    //Level2 = db.Level2s.Where(r => r.Id == i.Id_Level2).Select(r => r.Name ?? "").FirstOrDefault(),
                    //Level3 = db.Level3s.Where(r => r.Id == i.Id_Level3).Select(r => r.Name ?? "").FirstOrDefault()
                });
            }

            return resultsList;
        }

        #endregion

    }

  
}
