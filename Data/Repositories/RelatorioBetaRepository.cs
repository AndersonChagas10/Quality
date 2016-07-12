using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using System;

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

        public List<ResultOld> GetNcPorIndicador(int indicadorId)
        {

            /*
             
            
            
             */

            var query = "SELECT" +
                " id_operacao, " +
                " CONVERT(DECIMAL(16,4),SUM(Evaluate)) 'Evaluate', " +
                " CONVERT(DECIMAL(16,4),SUM(NotConform)) 'NotConform' " +
                " FROM( select id_operacao," +
                " CASE WHEN SUM(Evaluate) > 0 THEN 1 ELSE 0 end 'Evaluate' , " +
                " CASE WHEN SUM(NotConform) > 0 THEN 1 ELSE 0 end 'NotConform'" +
                " FROM ResultOld " +
                " WHERE NotConform > 0" +
                " GROUP BY numero1, numero2, Id_Operacao) ind GROUP BY Id_Operacao";

            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).ToList();

            List<ResultOld> resultsList = RetornoQueryIndicadoresRelBateToResultOld(queryResult);

            return resultsList;
        }

        public List<ResultOld> GetNcPorMonitoramento(int indicadorId)
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
                " AND NotConform > 0" +
                " GROUP BY numero1, " +
                " numero2, " +
                " Id_Operacao, " +
                " Id_Monitoramento)" +
                " ind GROUP BY Id_Operacao," +
                " Id_Monitoramento" +
                " ORDER BY NotConform desc", indicadorId);


            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).ToList();

            List<ResultOld> resultsList = RetornoQueryIndicadoresRelBateToResultOld(queryResult);

            return resultsList;
        }

        public List<ResultOld> GetNcPorTarefa(int indicadorId, int monitoramentoId)
        {

            /*
          

            */

            var query = string.Format("SELECT " +
                                        "id_operacao" +
                                        ", Id_Monitoramento " +
                                        ", Id_Tarefa " +
                                        ", CONVERT(DECIMAL(16,4), SUM(Evaluate)) AS 'Evaluate'" +
                                        ", CONVERT(DECIMAL(16,4), SUM(NotConform)) AS 'NotConform'" +
                                        " FROM ResultOld" +
                                        " WHERE Id_Monitoramento = {0} AND Id_Operacao = {1}" +
                                        " AND NotConform > 0" +
                                        " GROUP BY" +
                                        " id_operacao" +
                                        ", Id_Monitoramento" +
                                        ", Id_Tarefa" +
                                        " ORDER BY NotConform desc", monitoramentoId, indicadorId);

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
                    Tarefa = db.indicadores.Where(r => r.Id == i.Id_tarefa).Select(r => r.Name ?? "").FirstOrDefault()

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
