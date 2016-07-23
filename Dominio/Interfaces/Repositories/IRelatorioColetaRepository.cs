using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IRelatorioColetaRepository
    {
        List<Coleta> GetColetas();
    }
}
