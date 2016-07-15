using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface IUserService 
    {
        GenericReturn<UserSgq> AuthenticationLogin(UserSgq user);
    }
}
