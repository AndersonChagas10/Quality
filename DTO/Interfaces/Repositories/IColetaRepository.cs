using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IColetaRepository
    {
        void SalvarColeta(Coleta r);
        void SalvarListaColeta(List<Coleta> list);
        void ValidaFkColeta(Coleta r);
    }
}
