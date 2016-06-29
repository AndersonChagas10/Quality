using Dominio.Entities;
using Dominio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces.Services
{
    public interface IUserService 
    {
        GenericReturn<User> AuthenticationLogin(string name, string password);
    }
}
