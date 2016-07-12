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

            /*select 
            id_operacao
            , sum(Evaluate)"Evaluate"
            , sum(NotConform) "NotConform"
            from(
            select id_operacao
            , Case when sum(Evaluate) > 0 Then 1 ELSE 0 end "Evaluate"
            , Case when sum(NotConform) > 0 Then 1 ELSE 0 end "NotConform" from ResultOld group by numero1
            , numero2
            , Id_Operacao
            ) ind
            group by 
            Id_Operacao*/


            var query = "select  id_operacao, sum(Evaluate)'Evaluate', sum(NotConform) 'NotConform' from( select id_operacao, Case when sum(Evaluate) > 0 Then 1 ELSE 0 end 'Evaluate' , Case when sum(NotConform) > 0 Then 1 ELSE 0 end 'NotConform' from ResultOld group by numero1, numero2, Id_Operacao) ind group by Id_Operacao";

            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).ToList();

            List<ResultOld> resultsList = RetornoQueryIndicadoresRelBateToResultOld(queryResult);

            return resultsList;
        }

       

        public List<ResultOld> GetNcPorMonitoramento(int indicadorId)
        {
            var resultsList = db.Results.Where(r => r.Id_Operacao == indicadorId && r.Evaluate > 0 && r.NotConform > 0
                    && _Operacao.Any(z => z.Id == r.Id_Operacao)
                    && _Monitoramento.Any(z => z.Id == r.Id_Monitoramento)
                    && _Tarefa.Any(z => z.Id == r.Id_Tarefa)
                    )
             .GroupBy(x => x.Id_Monitoramento)
             .ToList()
             .Select(r => new ResultOld(id: r.Key,
                 id_Tarefa: r.Select(y => y.Id_Tarefa).FirstOrDefault(),
                 id_Monitoramento: r.Select(y => y.Id_Monitoramento).FirstOrDefault(),
                 id_Operacao: r.Select(y => y.Id_Operacao).FirstOrDefault(),
                 evaluate: r.Sum(y => y.Evaluate),
                 notConform: r.Sum(y => y.NotConform)))
            .OrderByDescending(r => r.NotConform)
            .ToList();

            AdicionaNomeAoResultadoDaQuery(resultsList);

            return resultsList;
        }

        public List<ResultOld> GetNcPorTarefa(int indicadorId, int monitoramentoId)
        {
            var resultsList = db.Results.Where(r => r.Id_Operacao == indicadorId && r.Id_Monitoramento == monitoramentoId && r.Evaluate > 0 && r.NotConform > 0
                    && _Operacao.Any(z => z.Id == r.Id_Operacao)
                    && _Monitoramento.Any(z => z.Id == r.Id_Monitoramento)
                    && _Tarefa.Any(z => z.Id == r.Id_Tarefa)
                    )
             .GroupBy(x => x.Id_Tarefa)
             .ToList()
             .Select(r => new ResultOld(id: r.Key,
                 id_Tarefa: r.Select(y => y.Id_Tarefa).FirstOrDefault(),
                 id_Monitoramento: r.Select(y => y.Id_Monitoramento).FirstOrDefault(),
                 id_Operacao: r.Select(y => y.Id_Operacao).FirstOrDefault(),
                 evaluate: r.Sum(y => y.Evaluate),
                 notConform: r.Sum(y => y.NotConform)))
            .OrderByDescending(r => r.NotConform)
            .ToList();

            AdicionaNomeAoResultadoDaQuery(resultsList);

            return resultsList;
        }

        #region Auxiliares

        private void AdicionaNomeAoResultadoDaQuery<T>(T resultsList)
        {


        }

        #endregion

        private List<ResultOld> RetornoQueryIndicadoresRelBateToResultOld(List<RetornoQueryIndicadoresRelBate> queryResult)
        {
            var resultsList = new List<ResultOld>();

            foreach (var i in queryResult)
            {
                resultsList.Add(new ResultOld() { Evaluate = i.Evaluate, NotConform = i.NotConform, Id_Operacao = i.Id_operacao, Operacao = db.indicadores.Find(i.Id_operacao).Name });
            }

            return resultsList;
        }
    }

    public class RetornoQueryIndicadoresRelBate
    {
        public int Id_operacao { get; set; }
        public int Evaluate { get; set; }
        public int NotConform { get; set; }
    }
}
