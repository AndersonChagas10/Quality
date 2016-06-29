using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces.Repositories
{
    public interface IUserRepository 
    {
        User AuthenticationLogin(string name, string password);
    }
} 
