using System;
using Dominio.Entities;
using Dominio.Interfaces.Repositories;

namespace Data.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly IRepositoryBase<Audit> _repoBase;

        public AuditRepository(IRepositoryBase<Audit> repoBase)
        {
            _repoBase = repoBase;
        }

        public void Salvar(Audit audit)
        {
            _repoBase.AddOrUpdate(audit);
            _repoBase.Commit();
        }
    }
}
