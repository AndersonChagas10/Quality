using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Verifica se o Usuario existe no DB
        /// Returns : UserSgq, null.
        /// </summary>
        /// <param name="user">UserSgq</param>
        /// <returns></returns>s
        UserSgq AuthenticationLogin(UserSgq user);

        void Salvar(UserSgq user);

        UserSgq GetByName(string username);
        
        List<UserSgq> GetAllUser();

        bool UserNameIsCadastrado(string Name, int id);

        List<UserSgq> GetAllUserByUnit(int unidadeId);
    }
}
