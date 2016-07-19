using Dominio.Entities;

namespace Dominio.Interfaces.Repositories
{
    public interface IUserRepository 
    {
        UserSgq AuthenticationLogin(UserSgq user);

        void Salvar(UserSgq user);

        UserSgq Get(string Name);

        bool UserNameIsCadastrado(string Name, int id);
    }
} 
