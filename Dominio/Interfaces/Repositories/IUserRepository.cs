using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IUserRepository
    {
        UserSgq AuthenticationLogin(UserSgq user);

        void Salvar(UserSgq user);

        UserSgq GetByName(string username);
        
        List<UserSgq> GetAllUser();

        bool UserNameIsCadastrado(string Name, int id);
    }
}
