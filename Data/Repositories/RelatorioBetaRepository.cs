using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;

namespace Data.Repositories
{
    public class RelatorioBetaRepository : RepositoryBase<ResultOld>, IRelatorioBetaRepository
    {
        public RelatorioBetaRepository(DbContextSgq Db) : base(Db)
        {
        }

        private DbSet<Operacao> _Operacao { get { return db.Set<Operacao>(); } }
        private DbSet<Monitoramento> _Monitoramento { get { return db.Set<Monitoramento>(); } }
        private DbSet<Tarefa> _Tarefa { get { return db.Set<Tarefa>(); } }

        public List<ResultOld> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd)
        {

            /*
             
            SELECT 
            id_operacao
            , SUM(Evaluate)"Evaluate"
            , SUM(NotConform) "NotConform"
            FROM(
            SELECT id_operacao
            , Case when sum(Evaluate) > 0 Then 1 ELSE 0 end "Evaluate" 
            , CASE WHEN SUM(NotConform) > 0 THEN 1 ELSE 0 END "NotConform" FROM ResultOld GROUP BY 
            numero1,
            Id_Monitoramento,
            numero2,
            Id_Operacao
            ) ind
            GROUP BY 
            Id_Operacao
            
            */

            //select CAST(getdate() as Date)

            var query = string.Format("SELECT" +
                " id_operacao, " +
                " CONVERT(DECIMAL(16,4),SUM(Evaluate)) 'Evaluate', " +
                " CONVERT(DECIMAL(16,4),SUM(NotConform)) 'NotConform' " +
                " FROM( select id_operacao," +
                " CASE WHEN SUM(Evaluate) > 0 THEN 1 ELSE 0 end 'Evaluate' , " +
                " CASE WHEN SUM(NotConform) > 0 THEN 1 ELSE 0 end 'NotConform'" +
                " FROM ResultOld " +
                " WHERE AddDate BETWEEN  '{0} 00:00:00' AND '{1} 23:59:59' " +
                " GROUP BY " +
                " numero1," +
                " numero2," +
                " Id_Monitoramento ," +
                " Id_Operacao) ind GROUP BY Id_Operacao", dateInit, dateEnd);

            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).OrderByDescending(r=>r.NotConform / r.Evaluate * 100).ToList();

            List<ResultOld> resultsList = RetornoQueryIndicadoresRelBateToResultOld(queryResult);

            return resultsList;
        }

        public List<ResultOld> GetNcPorMonitoramento(int indicadorId, string dateInit, string dateEnd)
        {

            /*
             
           
             
             */

            var query = string.Format("SELECT id_operacao," +
                " Id_Monitoramento," +
                " CONVERT(DECIMAL(16,4), sum(Evaluate)) 'Evaluate', " +
                " CONVERT(DECIMAL(16,4), sum(NotConform)) 'NotConform' " +
                " FROM(" +
                " SELECT id_operacao, " +
                " Id_Monitoramento, " +
                " CASE WHEN SUM(Evaluate) > 0 THEN 1 ELSE 0 END 'Evaluate'," +
                " CASE WHEN SUM(NotConform) > 0 THEN 1 ELSE 0 END 'NotConform' " +
                " FROM ResultOld " +
                " WHERE Id_Operacao = {0} " +
                " AND AddDate BETWEEN  '{1} 00:00:00' AND '{2} 23:59:59' " +
                " AND NotConform > 0" +
                " GROUP BY numero1, " +
                " numero2, " +
                " Id_Operacao, " +
                " Id_Monitoramento)" +
                " ind GROUP BY Id_Operacao," +
                " Id_Monitoramento" +
                " ORDER BY NotConform desc"
                , indicadorId, dateInit, dateEnd);


            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).ToList();

            List<ResultOld> resultsList = RetornoQueryIndicadoresRelBateToResultOld(queryResult);

            return resultsList;
        }

        public List<ResultOld> GetNcPorMonitoramentoJelsafa(int indicadorId, string dateInit, string dateEnd)
        {

            /*
             
           
             
             */

            var query = string.Format("SELECT id_operacao," +
                " Id_Monitoramento," +
                " CONVERT(DECIMAL(16,4), sum(Evaluate)) 'Evaluate', " +
                " CONVERT(DECIMAL(16,4), sum(NotConform)) 'NotConform' " +
                " FROM(" +
                " SELECT id_operacao, " +
                " Id_Monitoramento, " +
                " CASE WHEN SUM(Evaluate) > 0 THEN 1 ELSE 0 END 'Evaluate'," +
                " CASE WHEN SUM(NotConform) > 0 THEN 1 ELSE 0 END 'NotConform' " +
                " FROM ResultOld " +
                " WHERE Id_Operacao = {0} " +
                " AND AddDate BETWEEN  '{1} 00:00:00' AND '{2} 23:59:59' " +
                //" AND NotConform > 0" +
                " GROUP BY numero1, " +
                " numero2, " +
                " Id_Operacao, " +
                " Id_Monitoramento)" +
                " ind GROUP BY Id_Operacao," +
                " Id_Monitoramento" +
                " ORDER BY NotConform desc", indicadorId, dateInit, dateEnd);


            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).ToList();

            List<ResultOld> resultsList = RetornoQueryIndicadoresRelBateToResultOld(queryResult);

            return resultsList;
        }

        public List<ResultOld> GetNcPorTarefa(int indicadorId, int monitoramentoId, string dateInit, string dateEnd)
        {

            /*
          

            */
            //var query = string.Format("SELECT " +
            //                            "id_operacao" +
            //                            ", Id_Monitoramento " +
            //                            ", Id_Tarefa " +
            //                            ", CONVERT(DECIMAL(16,4), SUM(Evaluate)) AS 'Evaluate'" +
            //                            ", CONVERT(DECIMAL(16,4), SUM(NotConform)) AS 'NotConform'" +
            //                            " FROM ResultOld" +
            //                            " WHERE Id_Monitoramento = {0} AND Id_Operacao = {1}" +
            //                            " AND AddDate BETWEEN  '{2} 00:00:00' AND '{3} 23:59:59' " +
            //                            " AND NotConform > 0" +
            //                            " GROUP BY" +
            //                            " id_operacao" +
            //                            ", Id_Monitoramento" +
            //                            ", Id_Tarefa" +
            //                            " ORDER BY NotConform desc", monitoramentoId, indicadorId, dateInit, dateEnd);

            var query = string.Format("SELECT " +
                                        "id_operacao" +
                                        ", Id_Monitoramento " +
                                        ", Id_Tarefa " +
                                        ", CONVERT(DECIMAL(16,4), SUM(Evaluate)) AS 'Evaluate'" +
                                        ", CONVERT(DECIMAL(16,4), SUM(NotConform)) AS 'NotConform'" +
                                        " FROM ResultOld" +
                                        " WHERE Id_Monitoramento = {0} AND Id_Operacao = {1}" +
                                        " AND AddDate BETWEEN  '{2} 00:00:00' AND '{3} 23:59:59' " +
                                        " AND NotConform > 0" +
                                        " AND  numero1 = (select MAX(numero1) from ResultOld where Id_Operacao = {1} AND Id_Monitoramento = {0} AND AddDate BETWEEN  '{2} 00:00:00' AND '{3} 23:59:59')" +
                                        " AND numero2 = (select MAX(numero2) from ResultOld where Id_Operacao = {1} AND Id_Monitoramento = {0} AND AddDate BETWEEN  '{2} 00:00:00' AND '{3} 23:59:59')" +
                                        " GROUP BY" +
                                        " id_operacao" +
                                        ", Id_Monitoramento" +
                                        ", Id_Tarefa" +
                                        " ORDER BY NotConform desc", monitoramentoId, indicadorId, dateInit, dateEnd);

            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).ToList();

            List<ResultOld> resultsList = RetornoQueryIndicadoresRelBateToResultOld(queryResult);

            return resultsList;
        }

        #region Auxiliares

        private List<ResultOld> RetornoQueryIndicadoresRelBateToResultOld(List<RetornoQueryIndicadoresRelBate> queryResult)
        {
            var resultsList = new List<ResultOld>();

            foreach (var i in queryResult)
            {

                resultsList.Add(new ResultOld()
                {
                    Evaluate = i.Evaluate,
                    NotConform = i.NotConform,
                    Id_Operacao = i.Id_operacao,
                    Id_Monitoramento = i.Id_Monitoramento,
                    Id_Tarefa = i.Id_tarefa,
                    Operacao = db.indicadores.FirstOrDefault(r => r.Id == i.Id_operacao).Name,
                    Monitoramento = db.Monitoramentos.Where(r => r.Id == i.Id_Monitoramento).Select(r => r.Name ?? "").FirstOrDefault(),
                    Tarefa = db.Tarefas.Where(r => r.Id == i.Id_tarefa).Select(r => r.Name ?? "").FirstOrDefault()

                });
            }

            return resultsList;
        }

        #endregion

    }

    public class RetornoQueryIndicadoresRelBate
    {
        public int Id_operacao { get; set; }
        public int Id_Monitoramento { get; set; }
        public int Id_tarefa { get; set; }
        public decimal Evaluate { get; set; }
        public decimal NotConform { get; set; }
    }
}
