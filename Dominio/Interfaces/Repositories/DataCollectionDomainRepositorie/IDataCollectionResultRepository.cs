using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories.DataCollectionDomainRepositorie
{
    public interface IDataCollectionResultRepository
    {
        void SaveDataCollectionResult(DataCollection obj);
        void SaveDataCollectionResult(List<DataCollection> ListObj);
        void SaveOrUpdateDataCollectionResult(DataCollection obj);
        void SaveOrUpdateDataCollectionResult(List<DataCollection> ListObj);
        void UpdateDataCollectionResult(List<DataCollection> ListObj);
        void UpdateDataCollectionResult(DataCollection obj);
    }
}
