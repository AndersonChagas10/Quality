using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {

        public bool Autorizado(string name, string password) //Daqui iria pro B.D.
        {
            return name.Equals("admin") &&  password.Equals("123");
        }
    }
}
