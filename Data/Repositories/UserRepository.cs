using Dominio;
using Dominio.Interfaces.Repositories;
using System.Linq;

namespace Data.Repositories
{
    public class UserRepository :  RepositoryBase<UserSgq>, IUserRepository
    {
        
        public UserRepository(SgqDbDevEntities _db)
            :base (_db)
        {
        }

        public UserSgq Get(string Name)
        {
            return GetAll().FirstOrDefault(r => r.Name == Name);
        }

        public bool UserNameIsCadastrado(string Name, int id)
        {
            return GetAll().Any(x => x.Id != id && x.Name == Name);
        }

        public void Salvar(UserSgq user)
        {
            AddOrUpdate(user);
            Commit();
        }

        public UserSgq AuthenticationLogin(UserSgq user)
        {
            var result = db.Set<UserSgq>().FirstOrDefault(r => r.Name.Equals(user.Name) && r.Password.Equals(user.Password));
            return result;
        }
    }
}
