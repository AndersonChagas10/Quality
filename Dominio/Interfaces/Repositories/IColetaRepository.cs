using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IColetaRepository
    {
        void Salvar(Coleta r);
        void SalvarLista(List<Coleta> list);
        void ValidaFkResultado(Coleta r);
    }
}
