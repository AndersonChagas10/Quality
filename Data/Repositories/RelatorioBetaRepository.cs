using System;
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

        public List<ResultOld> GetNcPorIndicador(int indicadorId)
        {
            //indicadorId = 1;
            var resultsList = db.Results
                //.Where(r => r.Id_Operacao == indicadorId)
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
                    AddDate = r.Select(y=>y.AddDate).FirstOrDefault()
                })
                .ToList();

            return resultsList;
        }

        public List<ResultOld> GetNcPorMonitoramento(int indicadorId)
        {
            var resultsList = db.Results.Where(r => r.Id_Operacao == indicadorId)
             .GroupBy(x => x.Id_Operacao)
             .ToList()
             .Select(r => new ResultOld(id: r.Key,
                 id_Tarefa: r.Select(y => y.Id_Tarefa).FirstOrDefault(),
                 id_Monitoramento: r.Select(y => y.Id_Monitoramento).FirstOrDefault(),
                 id_Operacao: r.Select(y => y.Id_Operacao).FirstOrDefault(),
                 evaluate: r.Sum(y => y.Evaluate),
                 notConform: r.Sum(y => y.NotConform))).ToList();

            return resultsList;
        }

        public List<ResultOld> GetNcPorTarefa(int indicadorId, int monitoramentoId)
        {
            var resultsList = db.Results.Where(r => r.Id_Operacao == indicadorId)
             .GroupBy(x => x.Id_Operacao)
             .ToList()
             .Select(r => new ResultOld(id: r.Key,
                 id_Tarefa: r.Select(y => y.Id_Tarefa).FirstOrDefault(),
                 id_Monitoramento: r.Select(y => y.Id_Monitoramento).FirstOrDefault(),
                 id_Operacao: r.Select(y => y.Id_Operacao).FirstOrDefault(),
                 evaluate: r.Sum(y => y.Evaluate),
                 notConform: r.Sum(y => y.NotConform))).ToList();

            return resultsList;
        }
    }
}
