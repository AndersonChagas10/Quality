using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IUserAppService : IAppServiceBase<User>
    {

        bool Autorizado(string name, string password);

    }
}
