using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces.Services
{
    public interface IUserService : IServiceBase<User>
    {
        bool Autorizado(User u);
    }
}
