using Dominio.Entities;

namespace Dominio.Interfaces.Repositories
{
    public interface IUserRepository 
    {
        User AuthenticationLogin(string name, string password);

        void Salvar(User user);

        User Get(string Name);

        bool UserNameIsCadastrado(string Name, int id);
    }
} 
