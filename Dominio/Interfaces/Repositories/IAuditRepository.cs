using Dominio.Entities;

namespace Dominio.Interfaces.Repositories
{
    public interface IAuditRepository
    {

        void Salvar(Audit user);

    }
}
