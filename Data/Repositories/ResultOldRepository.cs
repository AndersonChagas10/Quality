using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data.Repositories
{
    public class ResultOldRepository : RepositoryBase<ResultOld>, IResultOldRepository
    {


        //private readonly IRepositoryBase<ResultOld> _repoBase;
        //public ResultOldRepository(IRepositoryBase<ResultOld> repoBase)
        //{
        //    _repoBase = repoBase;
        //}

        //private DbSet<Operacao> _Operacao { get { return db.Set<Operacao>(); } }
        //private DbSet<Monitoramento> _Monitoramento { get { return db.Set<Monitoramento>(); } }
        //private DbSet<Tarefa> _Tarefa { get { return db.Set<Tarefa>(); } }

        public ResultOldRepository(DbContextSgq _db)
            : base(_db)
        {
        }

        public void Salvar(ResultOld r)
        {
            var op = db.Set<Operacao>().ToList();
            var mon = db.Set<Monitoramento>().ToList();
            var tar = db.Set<Tarefa>().ToList();

            if (op.FirstOrDefault(z => z.Id == r.Id_Operacao) == null)
                throw new ExceptionHelper("Id Invalido para Operação");

            if (mon.FirstOrDefault(z => z.Id == r.Id_Monitoramento) == null)
                throw new ExceptionHelper("Id Invalido para Monitoramento");

            if (tar.FirstOrDefault(z => z.Id == r.Id_Tarefa) == null)
                throw new ExceptionHelper("Id Invalido para Tarefa");

            Add(r);
        }

        public void SalvarLista(List<ResultOld> list)
        {
            var op = db.Set<Operacao>().ToList();
            var mon = db.Set<Monitoramento>().ToList();
            var tar = db.Set<Tarefa>().ToList();
            foreach (var i in list)
            {
                if (op.FirstOrDefault(r => r.Id == i.Id_Operacao) == null)
                    throw new ExceptionHelper("Id Invalido para Operação");

                if (mon.FirstOrDefault(r => r.Id == i.Id_Monitoramento) == null)
                    throw new ExceptionHelper("Id Invalido para Monitoramento");

                if (tar.FirstOrDefault(r => r.Id == i.Id_Tarefa) == null)
                    throw new ExceptionHelper("Id Invalido para Tarefa");

            }

            AddAll(list);
        }


    }
}
