using Dominio.Entities;

namespace Dominio.Interfaces.Repositories
{
    public interface IUserRepository 
    {
        UserSgq AuthenticationLogin(UserSgq user);

        void Salvar(UserSgq user);

        UserSgq GetByName(string username);

        bool UserNameIsCadastrado(string Name, int id);
    }
} 
