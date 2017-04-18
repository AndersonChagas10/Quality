using System.Collections.Generic;
using DTO.DTO;

namespace Dominio.Interfaces.Services
{
    /// <summary>
    /// Passa base repo dinamica.
    /// </summary>
    /// <typeparam name="T">Domain</typeparam>
    /// <typeparam name="Y">DTO</typeparam>
    public interface IBaseDomain<T, Y> where T : class where Y : class
    {

        Y GetById(int id);

        Y GetByIdNoLazyLoad(int id);

        IEnumerable<Y> GetAll();

        IEnumerable<Y> GetAllNoLazyLoad();

        Y First();

        Y FirstNoLazyLoad();

        Y AddOrUpdate(Y obj);

        int ExecuteSql(string v);

        Y AddOrUpdate(Y userSgqDto, bool v);
    }
}
