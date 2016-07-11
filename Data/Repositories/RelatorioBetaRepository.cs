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

        public List<ResultOld> GetNcPorIndicador(int indicadorId)
        {

            var resultsList = db.Results
                .Where(r => r.Evaluate > 0 && r.NotConform > 0
                    //&& _Operacao.Any(z => z.Id == r.Id_Operacao)
                    //&& _Monitoramento.Any(z => z.Id == r.Id_Monitoramento)
                    //&& _Tarefa.Any(z => z.Id == r.Id_Tarefa)
                    )
                .GroupBy(x => x.Id_Operacao)
                .ToList()
                .Select(r => new ResultOld(
                      id: r.Key,
                      id_Tarefa: r.Select(y => y.Id_Tarefa).FirstOrDefault(),
                      id_Monitoramento: r.Select(y => y.Id_Monitoramento).FirstOrDefault(),
                      id_Operacao: r.Select(y => y.Id_Operacao).FirstOrDefault(),
                      evaluate: r.Sum(y => y.Evaluate),
                      notConform: r.Sum(y => y.NotConform))
                {
                    AddDate = r.Select(y => y.AddDate).FirstOrDefault(),
                })
            .OrderByDescending(r => (r.NotConform / r.Evaluate) * 100)
                .ToList();

            AdicionaNomeAoResultadoDaQuery(resultsList);

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
            .OrderByDescending(r => (r.NotConform / r.Evaluate) * 100)
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
            .OrderByDescending(r => (r.NotConform / r.Evaluate) * 100)
            .ToList();

            AdicionaNomeAoResultadoDaQuery(resultsList);

            return resultsList;
        }

        #region Auxiliares

        private void AdicionaNomeAoResultadoDaQuery(List<ResultOld> resultsList)
        {
            foreach (var i in resultsList)
            {
                i.Operacao = _Operacao.Find(i.Id_Operacao).Name;
                i.Monitoramento = _Monitoramento.Find(i.Id_Monitoramento).Name;
                i.Tarefa = _Tarefa.Find(i.Id_Tarefa).Name;
            }
        }

        #endregion
    }
}
