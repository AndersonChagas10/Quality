using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IParamsRepository
    {
        void SaveParLevel1(ParLevel1 paramLevel1, List<ParHeaderField> listaParHeadField, List<ParLevel1XCluster> listaParLevel1XCluster);
    }
}
