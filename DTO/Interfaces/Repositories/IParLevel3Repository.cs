using Dominio;
using System.Collections.Generic;

namespace DTO.Interfaces.Repositories
{
    public interface IParLevel3Repository
    {
        List<ParLevel3Level2> GetLevel3VinculadoLevel2(int idLevel1);
    }
}
