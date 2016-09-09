using System.Collections.Generic;
using DTO;

namespace Dominio.Interfaces.Repositories
{
    public interface IRelatorioColetaRepository<T> where T : class
    {
        IEnumerable<T> GetByDate(DataCarrierFormulario form);

        IEnumerable<CollectionLevel02> GetLastEntryCollectionLevel02(IEnumerable<ConsolidationLevel02> cl2);
        IEnumerable<CollectionLevel03> GetLastEntryCollectionLevel03(IEnumerable<CollectionLevel02> cll2);
        IEnumerable<ConsolidationLevel02> GetLastEntryConsildatedLevel02(IEnumerable<ConsolidationLevel01> cl1);

        IEnumerable<ConsolidationLevel02> GetEntryConsildatedLevel02ByDate(DataCarrierFormulario form);
        IEnumerable<ConsolidationLevel01> GetEntryConsildatedLevel01ByDateAndUnit(DataCarrierFormulario form);

    }
}
