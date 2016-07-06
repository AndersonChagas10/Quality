using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository :  RepositoryBase<User>, IUserRepository
    {
        //private readonly IRepositoryBase<User> _repoBase;
        //private readonly DbContextSgq _db;

        public UserRepository(DbContextSgq _db)
            :base (_db)
        {
          //  _repoBase = repoBase;
        }

        public User Get(string Name)
        {
            return GetAll().FirstOrDefault(r => r.Name == Name);
        }

        public bool UserNameIsCadastrado(string Name, int id)
        {
            return GetAll().Any(x => x.Id != id && x.Name == Name);
        }

        public void Salvar(User user)
        {
            AddOrUpdate(user);
            Commit();
        }

        public User AuthenticationLogin(User user)
        {
            var result = db.Set<User>().FirstOrDefault(r => r.Name.Equals(user.Name) && r.Password.Equals(user.Password));
            return result;
        }
    }
}
