using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface IUserService 
    {
        GenericReturn<User> AuthenticationLogin(string name, string password);
    }
}
